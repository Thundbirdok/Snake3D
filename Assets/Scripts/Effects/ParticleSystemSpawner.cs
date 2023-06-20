using UnityEngine;

namespace Effects
{
    using System.Collections.Generic;
    using UnityEngine.Pool;

    public class ParticleSystemSpawner : MonoBehaviour
    {
        [SerializeField]
        private ParticleSystemWrapper particleSystemPrefab;

        [SerializeField]
        private Transform container;
        
        private readonly List<ParticleSystemWrapper> _particleSystems = new List<ParticleSystemWrapper>();

        private ObjectPool<ParticleSystemWrapper> _pool;

        private void Awake()
        {
            _pool = new ObjectPool<ParticleSystemWrapper>
            (
                CreateFunc,
                ActionOnGet,
                ActionOnRelease,
                ActionOnDestroy,
                false,
                1,
                10
            );
        }

        private void OnDestroy()
        {
            if (_particleSystems == null || _pool == null)
            {
                return;
            }

            for (var i = 0; i < _particleSystems.Count; )
            {
                _pool.Release(_particleSystems[i]);
            }
        }

        public void PlaySystem(Vector3 position)
        {
            var system = _pool.Get();
            
            system.transform.position = position;
            system.Play();
        }
        
        private ParticleSystemWrapper CreateFunc() => Instantiate(particleSystemPrefab, container);

        private void ActionOnGet(ParticleSystemWrapper system)
        {
            system.gameObject.SetActive(true);
            system.Clear();
            
            _particleSystems.Add(system);
            
            system.OnStopped += Release;
        }

        private void ActionOnRelease(ParticleSystemWrapper system)
        {
            system.gameObject.SetActive(false);
            _particleSystems.Remove(system);
            
            system.OnStopped -= Release;
        }

        private static void ActionOnDestroy(ParticleSystemWrapper system) => Destroy(system.gameObject);

        private void Release(ParticleSystemWrapper system) => _pool.Release(system);
    }
}
