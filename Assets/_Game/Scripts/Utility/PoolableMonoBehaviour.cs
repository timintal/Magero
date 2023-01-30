using UnityEngine;
using UnityEngine.Pool;

public class PoolableMonoBehaviour : MonoBehaviour, IPoolableObject<GameObject>
{
    private IObjectPool<GameObject> _parentPool;
    public IObjectPool<GameObject> ParentPool => _parentPool;
    public void ReleaseObject()
    {
        _parentPool.Release(gameObject);
    }
    public void Init(IObjectPool<GameObject> parentPool)
    {
        _parentPool = parentPool;
    }
    
    
}
