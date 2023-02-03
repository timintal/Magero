using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Game.Common
{
    public class AutoInjectFactory 
    {
        private readonly IObjectResolver _container;

        public AutoInjectFactory(IObjectResolver container)
        {
            _container = container;
        }

        public T Spawn<T>(T prefab, Vector3 pos, Quaternion rot, Transform parent) where T : UnityEngine.Object
        {
            var newObject = _container.Instantiate(prefab, pos, rot, parent);
            return newObject;
        }
        
        public T Spawn<T>(T prefab, Transform parent = null) where T : UnityEngine.Object
        {
            var newObject = _container.Instantiate(prefab, parent);
            return newObject;
        }

    }
}
