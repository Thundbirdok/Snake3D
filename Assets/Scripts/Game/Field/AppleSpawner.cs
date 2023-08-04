using System.Linq;

namespace Game.Field
{
    using System;
    using Effects;
    using UnityEngine;

    [Serializable]
    public class AppleSpawner
    {
        public Transform Apple { get; private set; }
        
        [SerializeField]
        private GameObject applePrefab;

        [SerializeField]
        private ParticleSystemSpawner particleSystemSpawner;

        [SerializeField] 
        private Transform container;

        private Field _field;

        private bool _isInitialized;

        public void Initialize(Field field)
        {
            if (_isInitialized)
            {
                return;
            }

            _isInitialized = true;
            
            _field = field;

            InstantiateApple();

            SetAppleNewPosition();
        }

        public void SetToStart() => SetAppleNewPosition();

        public void EatApple()
        {
            particleSystemSpawner.PlaySystem(Apple.position);

            SetAppleNewPosition();
        }

        private void SetAppleNewPosition()
        {
            while (true)
            {
                var x = UnityEngine.Random.Range(1, _field.Size.x - 1) + 0.5f;
                var y = UnityEngine.Random.Range(1, _field.Size.y - 1) + 0.5f;
                var z = UnityEngine.Random.Range(1, _field.Size.z - 1) + 0.5f;

                var appleLocalPosition = new Vector3(x, y, z);

                if (IsInsideSnake(appleLocalPosition))
                {
                    continue;
                }

                Apple.localPosition = appleLocalPosition;

                break;
            }
        }

        private void InstantiateApple()
        {
            Apple = UnityEngine.Object.Instantiate
                (
                    applePrefab,
                    container
                )
                .transform;
        }

        private bool IsInsideSnake(Vector3 appleLocalPosition)
        {
            return _field.Snake.PartsTarget?.Any(part => part.Position == appleLocalPosition) ?? false;
        }
    }
}
