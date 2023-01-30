using UnityEngine;
using UnityEngine.Pool;

public class GameObjectPool
{
    private GameObject _template;

    private ObjectPool<GameObject> _pool;

    public GameObjectPool(GameObject template)
    {
        _template = template;
        _pool = new ObjectPool<GameObject>(CreatePooledItem, OnTakeFromPool, OnReturnToPool, OnDestroyPooledObject);
    }

    private GameObject CreatePooledItem()
    {
        var obj = Object.Instantiate(_template);
        var poolableMonoBehaviour = obj.AddComponent<PoolableMonoBehaviour>();
        poolableMonoBehaviour.Init(_pool);
        return obj;
    }

    private void OnTakeFromPool(GameObject obj)
    {
        obj.SetActive(true);
    }

    private void OnReturnToPool(GameObject obj)
    {
        obj.SetActive(false);
    }

    private void OnDestroyPooledObject(GameObject obj)
    {
        Object.Destroy(obj);
    }
}