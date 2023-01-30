using UnityEngine;
using UnityEngine.Pool;

public interface IPoolableObject<T> where T : class
{
    public IObjectPool<T> ParentPool { get; }
    public void ReleaseObject();
}

public class PoolService : MonoBehaviour
{
    
}