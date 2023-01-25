using Sirenix.OdinInspector;
using UnityEngine;

public class Level : MonoBehaviour
{
    public int width, height;
    public float cellSize;

    public Obstacle[] obstacles;

    public Transform[] CrowdTargets;

#if UNITY_EDITOR
    public bool drawBounds;
    public bool drawObstacles;
    [Button]
    void UpdateObstacles()
    {
        obstacles = GetComponentsInChildren<Obstacle>();
        foreach (var obstacle in obstacles)
        {
            var obstacleTransform = obstacle.transform;
            RoundPosition(obstacleTransform);
            RoundScale(obstacleTransform);
            
            var obstacleScale = obstacleTransform.localScale;
            
            obstacle.width = Mathf.RoundToInt(obstacleScale.x / cellSize);
            obstacle.height = Mathf.RoundToInt(obstacleScale.z / cellSize);
            obstacle.indexX = Mathf.RoundToInt((obstacleTransform.position.x - transform.position.x) / cellSize);
            obstacle.indexY = Mathf.RoundToInt((obstacleTransform.position.z - transform.position.z) / cellSize);
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

            var center = new Vector3(width * cellSize * 0.5f, 1, height * cellSize * 0.5f) + transform.position;

            Gizmos.DrawCube(center, new Vector3(width * cellSize, 1, height * cellSize));
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

        Gizmos.color = col;
    }
#endif
}