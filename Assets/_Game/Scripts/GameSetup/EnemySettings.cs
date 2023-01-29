using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(menuName = "Magero/Enemy Settings")]
public class EnemySettings : ScriptableObject
{
    public GameObject Prefab;

    public float Speed;
    public int Health;
    public float Radius;
    public float RepulsionRadius;
}