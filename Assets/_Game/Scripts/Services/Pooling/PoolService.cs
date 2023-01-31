using System.Collections.Generic;
using UnityEngine;

public class PoolService 
{
    private Dictionary<ParticleSystem, AutoDestroyParticleFxPool> _fxPools = new();
    private Dictionary<GameObject, GameObjectPool> _objectPools = new();

    public ParticleSystem GetParticleFx(ParticleSystem template)
    {
        if (!_fxPools.ContainsKey(template))
        {
            _fxPools.Add(template, new AutoDestroyParticleFxPool(template));
        }

        return _fxPools[template].Pool.Get();
    }

    public PoolableMonoBehaviour GetGameObject(GameObject template)
    {
        if (!_objectPools.ContainsKey(template))
        {
            _objectPools.Add(template, new GameObjectPool(template));
        }

        var gameObject = _objectPools[template].Pool.Get();
        return gameObject.GetComponent<PoolableMonoBehaviour>();
    }
}