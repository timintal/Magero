using UnityEngine;
using UnityEngine.Pool;

public class GameObjectPool
{
    private GameObject _template;

    private ObjectPool<PoolableMonoBehaviour> _pool;

    public ObjectPool<PoolableMonoBehaviour> Pool => _pool;

    public GameObjectPool(GameObject template)
    {
        _template = template;
        _pool = new ObjectPool<PoolableMonoBehaviour>(CreatePooledItem, OnTakeFromPool, OnReturnToPool, OnDestroyPooledObject);
    }

    private PoolableMonoBehaviour CreatePooledItem()
    {
        var obj = Object.Instantiate(_template);
        var poolableMonoBehaviour = obj.AddComponent<PoolableMonoBehaviour>();
        poolableMonoBehaviour.Init(_pool);
        return poolableMonoBehaviour;
    }

    private void OnTakeFromPool(PoolableMonoBehaviour obj)
    {
        obj.gameObject.SetActive(true);
        obj.ResetAll();
    }

    private void OnReturnToPool(PoolableMonoBehaviour obj)
    {
        obj.gameObject.SetActive(false);
    }

    private void OnDestroyPooledObject(PoolableMonoBehaviour obj)
    {
        Object.Destroy(obj);
    }
}