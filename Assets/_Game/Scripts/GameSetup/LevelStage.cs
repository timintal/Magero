using System.Linq;
using Cinemachine;
using Sirenix.OdinInspector;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

public class LevelStage : MonoBehaviour
{
    public Rect bounds;
    
    [ReadOnly] public int width, height;
    [ReadOnly] public float cellSize;

    public Obstacle[] obstacles;

    public Transform[] CrowdTargets;
    public MonoEntityLink[] EnemySpawners;

    public CinemachineVirtualCamera StageCamera;

    public Vector3 FlowFieldInitialPos => transform.position + new Vector3(bounds.x, 0, bounds.y);
    
#if UNITY_EDITOR
    public bool drawBounds;
    public bool drawObstacles;
    public bool drawSpawners;

    [Button]
    void UpdateSettings()
    {
        var gameSetupGUID = AssetDatabase.FindAssets("t:GameSetup").First();
        var gameSetup = AssetDatabase.LoadAssetAtPath<GameSetup>(AssetDatabase.GUIDToAssetPath(gameSetupGUID));

        cellSize = gameSetup.FlowFieldSettings.CellSize;
        width = Mathf.RoundToInt(bounds.size.x / cellSize);
        height = Mathf.RoundToInt(bounds.size.y / cellSize);
        
        obstacles = GetComponentsInChildren<Obstacle>();
        foreach (var obstacle in obstacles)
        {
            var obstacleTransform = obstacle.transform;
            RoundPosition(obstacleTransform);
            RoundScale(obstacleTransform);
            
            var obstacleScale = obstacleTransform.localScale;
            
            obstacle.width = Mathf.RoundToInt(obstacleScale.x / cellSize);
            obstacle.height = Mathf.RoundToInt(obstacleScale.z / cellSize);
            obstacle.indexX = Mathf.RoundToInt((obstacleTransform.position.x - FlowFieldInitialPos.x) / cellSize);
            obstacle.indexY = Mathf.RoundToInt((obstacleTransform.position.z - FlowFieldInitialPos.z) / cellSize);
        }

        foreach (var spawner in EnemySpawners)
        {
            for (int i = 0; i < spawner.Overrides.Count; i++)
            {
                if (spawner.Overrides[i] is PositionComponent)
                {
                    ((PositionComponent) spawner.Overrides[i]).Value = spawner.transform.position;
                }
            }
        }
    }
    
    private void RoundPosition(Transform tr)
    {
        Vector3 pos = tr.position;
        pos.x = Mathf.RoundToInt(pos.x / cellSize) * cellSize;
        pos.z = Mathf.RoundToInt(pos.z / cellSize) * cellSize;
        tr.position = pos;
    }

    private void RoundScale(Transform tr)
    {
        Vector3 scale = tr.localScale;
        scale.x = Mathf.RoundToInt(scale.x / cellSize) * cellSize;
        scale.z = Mathf.RoundToInt(scale.z / cellSize) * cellSize;
        tr.localScale = scale;
    }

    private void OnDrawGizmos()
    {
        Color col = Gizmos.color;
        
        if (drawBounds)
        {
            Gizmos.color = new Color(0, 1f, 1f, 0.5f);

            // var center = new Vector3(width * cellSize * 0.5f, 1, height * cellSize * 0.5f) + transform.position;

            // Gizmos.DrawCube(center, new Vector3(width * cellSize, 1, height * cellSize));
            
            Gizmos.DrawCube(new Vector3(bounds.center.x, 1, bounds.center.y) + transform.position, new Vector3(bounds.size.x, 1, bounds.size.y));
        }

        if (drawObstacles)
        {
            Gizmos.color = new Color(0.1f, 0.1f, 0.1f, 0.8f);

            foreach (var obstacle in obstacles)
            {
                var startPos = obstacle.transform.position;
                
                var center = new Vector3(obstacle.width * cellSize * 0.5f,1, obstacle.height * cellSize * 0.5f) + startPos;
                Gizmos.DrawCube(center, new Vector3(obstacle.width * cellSize,1,  obstacle.height * cellSize));
                
            }
        }

        if (drawSpawners)
        {
            Gizmos.color = Color.green;
            foreach (var spawner in EnemySpawners)
            {
                if (spawner == null) continue;
                
                foreach (var spawnerOverride in spawner.Overrides)
                {
                    if (spawnerOverride is EnemySpawnerComponent spawnerComponent)
                    {
                        Gizmos.DrawCube(spawner.transform.position, new Vector3(spawnerComponent.SpawnArea.x, 1, spawnerComponent.SpawnArea.y));
                    }
                }
            }
        }

        Gizmos.color = col;
    }
#endif
}