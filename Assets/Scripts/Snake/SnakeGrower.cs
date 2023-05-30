namespace Snake
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.Pool;
    using Object = UnityEngine.Object;

    [Serializable]
    public class SnakeGrower
    {
        private List<Transform> _parts;
        public IReadOnlyList<Transform> Parts => _parts;

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

            var partPosition = snake.transform.localPosition;
            
            for (var i = 0; i < startPartsNumber; i++)
            {
                Grow(partPosition);
                --partPosition.z;
            }
        }

        public void Grow() => Grow(_snake.TailPreviousTargetPosition);

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

        private void ActionOnDestroy(GameObject part)
        {
            Object.Destroy(part);
        }

        private void Grow(Vector3 localPosition)
        {
            var part = _pool.Get();
            
            part.transform.localPosition = localPosition;
        }
    }
}
