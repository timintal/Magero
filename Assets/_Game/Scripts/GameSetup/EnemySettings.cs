using UnityEngine;

[CreateAssetMenu(menuName = "Magero/Enemy Settings")]
public class EnemySettings : ScriptableObject
{
    public GameObject Prefab;

    public float Speed;
    public int Health;
}