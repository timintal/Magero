using Entitas;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(menuName = "MageTower/EntityTemplate")]
public class EntityTemplate : SerializedScriptableObject
{
    public IComponent[] Components;
}
