namespace Game.Field.Wall
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    [Serializable]
    public class WallSpawner
    {
        [SerializeField]
        private Wall wallPrefab;

        [SerializeField] 
        private Transform container;
        
        private Field _field;

        private List<Wall> _walls = new List<Wall>();

        public void Initialize(Field field)
        {
            _field = field;
            
            InstantiateWalls();
        }

        public void Setup()
        {
            foreach (var wall in _walls)
            {
                wall.Setup(_field.Snake.Head);
            }
        }
        
        private void InstantiateWalls()
        {
            var size = _field.Size;
            
            InstantiateWall
            (
                new Vector3(0, (float)size.y / 2, (float)size.z / 2),
                Quaternion.Euler(0, 0, -90),
                (float)size.z / 10
            );
            
            InstantiateWall
            (
                new Vector3(size.x, (float)size.y / 2, (float)size.z / 2),
                Quaternion.Euler(0, 0, 90),
                (float)size.z / 10
            );

            InstantiateWall
            (
                new Vector3((float)size.x / 2, (float)size.y / 2, 0),
                Quaternion.Euler(0, 90, 90),
                (float)size.z / 10
            );
            
            InstantiateWall
            (
                new Vector3((float)size.x / 2, (float)size.y / 2, size.z),
                Quaternion.Euler(0, 90, -90),
                (float)size.z / 10
            );

            InstantiateWall
            (
                new Vector3((float)size.x / 2, size.y, (float)size.z / 2),
                Quaternion.Euler(0, 0, 180),
                (float)size.z / 10
            );
        }

        private void InstantiateWall(Vector3 position, Quaternion rotation, float scale)
        {
            var wall = UnityEngine.Object.Instantiate(wallPrefab, container);

            var wallTransform = wall.transform;
            
            wallTransform.localPosition = position;
            wallTransform.localRotation = rotation;
            wallTransform.localScale = scale * Vector3.one;

            _walls.Add(wall);
            
            wall.Initialize();
        }
    }
}
