using System;
using System.Runtime.CompilerServices;
using Entitas;
using Entitas.CodeGeneration.Attributes;
using UnityEngine;

public enum DirectionFetchResult
{
    Found,
    FoundForced,
    NotFound
}

[Game]
public class FlowFieldComponent : IComponent
{
    public Vector3 InitialPoint;
    public float CellSize;
    
    public int[][] LevelField;
    public int[][] CurrentField;
    public int[][] BackField;

    public int Width => LevelField.Length;
    public int Height => LevelField[0].Length;
    
    public void ResetField()
    {
        CopyField(LevelField, BackField);
    }

    public void CopyField(int[][] source, int[][] destination)
    {
        int maxX = Width;
        int maxY = Height;
        for (int i = 0; i < maxX; i++)
        {
            for (int j = 0; j < maxY; j++)
            {
                destination[i][j] = source[i][j];
            }
        }
    }
    

    public void SwapFields()
    {
        (CurrentField, BackField) = (BackField, CurrentField);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public (int x, int y) GetIndex(Vector3 pos)
    {
        return (Mathf.RoundToInt((pos.x - InitialPoint.x) / CellSize - 0.5f), Mathf.RoundToInt((pos.z - InitialPoint.z) / CellSize - 0.5f));
    }

    public Vector3 GetPosition(int x, int y)
    {
        return new Vector3(InitialPoint.x + (x + 0.5f) * CellSize, 0, InitialPoint.z + (y + 0.5f) * CellSize);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool IsIndexValid(int x, int y)
    {
        return x > 0 && x < LevelField.Length && y > 0 && y < LevelField[0].Length;
    }

    public bool IsPassablePosition(Vector3 position, float maxDistance)
    {
        var (x, y) = GetIndex(position);

        if (!IsIndexValid(x, y)) return false;
        
        return IsPassablePosition(maxDistance, x, y);
    }

    private bool IsPassablePosition(float maxDistance, int x, int y)
    {
        return CurrentField[x][y] <= maxDistance;
    }


    public (int, int) GetMinNeighbour(Vector3 pos, int depth)
    {
        int x = -1, y = -1;
        
        var (initX, initY) = GetIndex(pos);
        
        int currValue = Int32.MaxValue;

        for (int i = -depth; i <= depth; i++)
        {
            for (int j = -depth; j <= depth; j++)
            {
                if (i == 0 && j == 0) continue;

                if (IsIndexValid(initX + i, initY + j) &&
                    CurrentField[initX + i][initY + j] < currValue)
                {
                    x = initX + i;
                    y = initY + j;
                    currValue = CurrentField[x][y];
                }
            }
        }

        return (x, y);
    }
    
    public Vector3 GetDirection(Vector3 pos, int maxValue, out DirectionFetchResult result, out int xTarget, out int yTarget)
    {
        var direction = Vector3.zero;
        result = DirectionFetchResult.NotFound;
        xTarget = 0;
        yTarget = 0;
        
        var (x, y) = GetIndex(pos);
        
        if (IsIndexValid(x, y) && IsPassablePosition(maxValue, x, y))
        {
            // var (newX, newY) = GetMinNeighbour(pos, 1);
            //
            // if (newX >= 0 && newY >= 0)
            // {
            //     direction = GetPosition(newX, newY) - pos;
            //     direction.Normalize();
            //     result = DirectionFetchResult.Found;
            //     xTarget = newX;
            //     yTarget = newY;
            //     return direction;
            // }

            int currValue = CurrentField[x][y];
            int currentWeight = currValue;
            
            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    if (i == 0 && j == 0) continue;
                    
                    int currX = x + i;
                    int currY = y + j;
            
                    if (IsIndexValid(currX, currY) &&
                        CurrentField[currX][currY] < maxValue)
                    {
                        if (CurrentField[currX][currY] < currentWeight)
                        {
                            currentWeight = CurrentField[currX][currY];
                            direction = GetPosition(currX, currY) - pos;
                            result = DirectionFetchResult.Found;
                            xTarget = currX;
                            yTarget = currY;
                        }
                        else if (CurrentField[currX][currY] == currentWeight)
                        {
                            var possibleDirection = GetPosition(currX, currY) - pos;
                            if (possibleDirection.ManhattanDistance() < direction.ManhattanDistance())
                            {
                                direction = possibleDirection;
                                result = DirectionFetchResult.Found;
                                xTarget = currX;
                                yTarget = currY;
                            }
                        }
                    }
                }
            }
        }
        else
        {
            for (int offset = 1; offset < 100 && result == DirectionFetchResult.NotFound; offset++)
            {
                for (int i = -offset; i <= offset && result == DirectionFetchResult.NotFound; i++)
                {
                    for (int j = -offset; j <= offset && result == DirectionFetchResult.NotFound; j++)
                    {
                        if (i != offset && j != offset && i != -offset && j != -offset) continue;
                        
                        int currX = x + i;
                        int currY = y + j;

                        if (IsIndexValid(currX, currY) && IsPassablePosition(maxValue, currX, currY))
                        {
                            direction = GetPosition(currX, currY) - pos;
                            result = DirectionFetchResult.FoundForced;
                            xTarget = currX;
                            yTarget = currY;
                        }
                    }
                }
            }
        }
        
        return direction.normalized;
    }
}