using System.Collections.Generic;
using Entitas;
using UnityEngine;

public static class HelperFunctions
{
    public static int PackedIndex(int x, int y)
    {
        int plainIndex = x;
        plainIndex <<= 16;
        plainIndex += y;
        return plainIndex;
    }

    public static void UpdateFlowFieldForTarget(Vector3 targetPosition, FlowFieldComponent flowField,
        FlowFieldSettings fieldSettings, float targetSize, List<int> cellsBuffer, float maxValue = int.MaxValue)
    {
        cellsBuffer.Clear();

        var (initX, initY) = flowField.GetIndex(targetPosition);

        int size = Mathf.RoundToInt(targetSize / fieldSettings.CellSize);
        for (int i = -size; i <= size; i++)
        {
            for (int j = -size; j <= size; j++)
            {
                if (flowField.IsIndexValid(initX + i, initY + j))
                {
                    cellsBuffer.Add(HelperFunctions.PackedIndex(initX + i, initY + j));
                    flowField.BackField[initX + i][initY + j] = 0;
                }
            }
        }

        while (cellsBuffer.Count > 0)
        {
            int currPlainIndex = cellsBuffer[0];
            cellsBuffer.RemoveAt(0);

            int currX = currPlainIndex >> 16;
            int currY = currPlainIndex - (currX << 16);

            int currValue = flowField.BackField[currX][currY];

            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    if (i == 0 && j == 0) continue;

                    int addValue = i != 0 && j != 0 ? fieldSettings.StepDiagonalWeight : fieldSettings.StepWeight;

                    if (addValue > maxValue) continue;
                    
                    if (flowField.IsIndexValid(currX + i, currY + j))
                    {
                        var val = flowField.BackField[currX + i][currY + j];
                        if (val > currValue + addValue && val != int.MaxValue)
                        {
                            flowField.BackField[currX + i][currY + j] = currValue + addValue;
                            cellsBuffer.Add(HelperFunctions.PackedIndex(currX + i, currY + j));
                        }
                    }
                }
            }
        }
    }
}