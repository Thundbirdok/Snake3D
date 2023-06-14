namespace Game.Snake
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Game.Field;
    using UnityEngine;
    using UnityEngine.Pool;
    using Object = UnityEngine.Object;

    [Serializable]
    public class SnakeGrower
    {
        private List<Transform> _parts;
        public IReadOnlyList<Transform> Parts => _parts;

        [SerializeField]
        private Field field;
        
        [SerializeField]
        private Transform container;

        [SerializeField]
        private GameObject partPrefab;

        [SerializeField]
        private int startPartsNumber;

        private ObjectPool<GameObject> _pool;

        private Snake _snake;

        public void Construct(Snake snake)
        {
            _snake = snake;
            
            _pool = new ObjectPool<GameObject>
            (
                ActionOnCreate,
                ActionOnGet,
                ActionOnRelease,
                ActionOnDestroy, 
                false, 
                10, 
                100
            );
            
            _parts = new List<Transform>();
        }

        public void Dispose()
        {
            ClearParts();
            _pool?.Dispose();
        }
        
        public void Setup()
        {
            ClearParts();
            SetupParts();
        }

        public void Grow()
        {
            var tailPreviousPosition = _snake.TailPreviousTargetPosition;

            var tailPosition = _snake.PartsTargetPosition.Last();

            var rotation = Quaternion.LookRotation(tailPosition - tailPreviousPosition);
            
            Grow(tailPreviousPosition, rotation);
        }

        private GameObject ActionOnCreate()
        {
            var part = Object.Instantiate(partPrefab, container);
            part.name = $"Part {_parts.Count}";

            return part;
        }

        private void ActionOnGet(GameObject part)
        {
            _parts.Add(part.transform);
            part.SetActive(true);
        }

        private void ActionOnRelease(GameObject part)
        {
            _parts.Remove(part.transform);
            part.SetActive(false);
        }

        private void ActionOnDestroy(GameObject part) => Object.Destroy(part);

        private void SetupParts()
        {
            var xPartPosition = (float)field.Size.x / 2 - (field.Size.x % 2 == 0 ? 0.5f : 0);
            var yPartPosition = (float)field.Size.y / 2 - (field.Size.y % 2 == 0 ? 0.5f : 0);
            var zPartPosition = (float)field.Size.z / 2 - (field.Size.z % 2 == 0 ? 0.5f : 0);

            var partPosition = new Vector3(xPartPosition, yPartPosition, zPartPosition);

            for (var i = 0; i < startPartsNumber; i++)
            {
                Grow(partPosition, Quaternion.identity);
                --partPosition.z;
            }
        }

        private void ClearParts()
        {
            if (_parts == null)
            {
                return;
            }
            
            for (var i = 0; i < _parts.Count;)
            {
                if (_parts[i] == null)
                {
                    ++i;
                    
                    continue;
                }

                _pool.Release(_parts[i].gameObject);
            }
            
            _parts.Clear();
        }

        private void Grow(Vector3 localPosition, Quaternion rotation)
        {
            if (_pool == null)
            {
                return;
            }

            var part = _pool.Get();
            
            part.transform.localPosition = localPosition;
            part.transform.localRotation = rotation;
        }
    }
}
