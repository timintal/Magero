using System;
using System.Runtime.CompilerServices;
using Entitas;
using UnityEngine;



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
        for (int i = 0; i < Width; i++)
        {
            for (int j = 0; j < Height; j++)
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
        return (Mathf.RoundToInt((pos.x - InitialPoint.x) / CellSize), Mathf.RoundToInt((pos.z - InitialPoint.z) / CellSize));
    }

    public Vector3 GetPosition(int x, int y)
    {
        return new Vector3(InitialPoint.x + (x + 0.5f) * CellSize, 0, InitialPoint.y + (y + 0.5f) * CellSize);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool IsIndexValid(int x, int y)
    {
        return x > 0 && x < LevelField.Length && y > 0 && y < LevelField[0].Length;
    }

    public Vector3 GetDirection(Vector3 pos, int maxValue, out bool directionFound)
    {
        var direction = Vector3.zero;
        directionFound = false;
        
        var (x, y) = GetIndex(pos);
        
        if (IsIndexValid(x, y))
        {
            int currValue = CurrentField[x][y];
            int currentWeight = currValue;
            
            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    int currX = x + i;
                    int currY = y + j;

                    if (IsIndexValid(currX, currY) &&
                        CurrentField[currX][currY] < maxValue)
                    {
                        if (CurrentField[currX][currY] < currentWeight)
                        {
                            currentWeight = CurrentField[currX][currY];
                            direction = GetPosition(currX, currY) - pos;
                            directionFound = true;
                        }
                        else if (CurrentField[currX][currY] == currentWeight)
                        {
                            var possibleDirection = GetPosition(currX, currY) - pos;
                            if (possibleDirection.ManhattanDistance() < direction.ManhattanDistance())
                            {
                                direction = possibleDirection;
                                directionFound = true;
                            }
                        }
                    }
                }
            }
        }

        return direction.normalized;
    }
}