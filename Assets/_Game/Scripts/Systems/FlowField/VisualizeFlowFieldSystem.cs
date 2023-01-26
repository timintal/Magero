using System.Collections.Generic;
using Entitas;
using UnityEngine;
using UnityEngine.Rendering;

public class VisualizeFlowFieldSystem : IExecuteSystem
{
    private static readonly int BaseColor = Shader.PropertyToID("_Colors");
    
    private readonly Contexts _contexts;

    private IGroup<GameEntity> _flowFieldGroup;

    public VisualizeFlowFieldSystem(Contexts contexts)
    {
        _contexts = contexts;
        _flowFieldGroup = contexts.game.GetGroup(GameMatcher.FlowField);
    }


    List<Matrix4x4> matrices = new List<Matrix4x4>();
    List<Vector4> colors = new List<Vector4>();

    public void Execute()
    {
        DebugSettings debugSettings = _contexts.game.gameSetup.value.DebugSettings;
        if (!debugSettings.VisualizeFlowField || _flowFieldGroup.count == 0)
            return;
        
        var e = _flowFieldGroup.GetSingleEntity();
        var lvlField = e.flowField.CurrentField;
        var initialPoint = e.flowField.InitialPoint;

        var cellsCount = e.flowField.Width * e.flowField.Height;

        EnsureArrayLength(cellsCount);

        var quaternion = Quaternion.AngleAxis(90, new Vector3(1,0,0));

        for (int i = 0; i < e.flowField.Width; i++)
        {
            for (int j = 0; j < e.flowField.Height; j++)
            {
                Color c = Color.black;

                if (lvlField[i][j] < debugSettings.MaxValueColor)
                {
                    c = debugSettings.mapGradient.Evaluate(lvlField[i][j] / (float) debugSettings.MaxValueColor);
                }
                else
                {
                    continue;
                }
                
                var currMatrix = matrices[i * e.flowField.Height+ j];
                currMatrix.SetTRS(
                    initialPoint + new Vector3((i + 0.5f) * e.flowField.CellSize,1, (j + 0.5f) * e.flowField.CellSize),
                    quaternion, 
                    Vector3.one * e.flowField.CellSize);

                matrices[i * e.flowField.Height + j] = currMatrix;
                
                colors[i * e.flowField.Height + j] = c;
            }
        }

        List<Matrix4x4> tempMatrices = new List<Matrix4x4>(1024);
        List<Vector4> tempColors = new List<Vector4>(1024);
        int drawnCount = 0;
        while (drawnCount < matrices.Count)
        {
            tempMatrices.Clear();
            tempColors.Clear();
            
            int count = Mathf.Min(256, matrices.Count - drawnCount);
            for (int i = 0; i < count; i++)
            {
                tempMatrices.Add(matrices[drawnCount + i]);
                tempColors.Add(colors[drawnCount + i]);
            }
            
            MaterialPropertyBlock block = new MaterialPropertyBlock();
            
            block.SetVectorArray(BaseColor, tempColors);
            Graphics.DrawMeshInstanced(debugSettings.CellMesh, 0, debugSettings.FlowFieldMaterial, tempMatrices, block);

            drawnCount += count;
        }
        
    }

    private void EnsureArrayLength(int cellsCount)
    {
        while (matrices.Count < cellsCount)
        {
            matrices.Add(new Matrix4x4());
        }

        while (colors.Count < cellsCount)
        {
            colors.Add(Color.black);
        }

        if (matrices.Count > cellsCount)
            matrices.RemoveRange(cellsCount - 1, matrices.Count - cellsCount);
        if (colors.Count > cellsCount)
            colors.RemoveRange(cellsCount - 1, colors.Count - cellsCount);
    }
}