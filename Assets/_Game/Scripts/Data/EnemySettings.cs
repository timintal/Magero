using _Game.Data;
using UnityEngine;

[CreateAssetMenu(menuName = "Magero/Enemy Settings")]
public class EnemySettings : ScriptableObject
{
    public UnitType Type;
    
    public GameObject Prefab;

    public float Speed;

    public float Radius;
    public bool IsFlying;

    public int Exp;
    
}