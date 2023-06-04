using UnityEngine;

namespace Field
{
    using Random = UnityEngine.Random;

    public class Field : MonoBehaviour
    {
        public Transform Apple { get; private set; }
        
        [field: SerializeField]
        public Vector3Int Size { get; private set; }

        [SerializeField]
        private GameObject wallPrefab;
        
        [SerializeField]
        private GameObject applePrefab;

        private bool _isInitalized;

        public void Setup()
        {
            if (_isInitalized == false)
            {
                InstantiateWalls();
                InstantiateApple();
                
                _isInitalized = true;
            }
            
            SetAppleNewPosition();
        }

        public void SetAppleNewPosition()
        {
            var x = Random.Range(1, Size.x - 1) + 0.5f;
            var y = Random.Range(1, Size.y - 1) + 0.5f;
            var z = Random.Range(1, Size.z - 1) + 0.5f;
            
            Apple.localPosition = new Vector3(x, y, z);
        }

        private void InstantiateApple()
        {
            Apple = Instantiate
            (
                applePrefab,
                transform
            )
            .transform;
        }

        private void InstantiateWalls()
        {
            InstantiateWall
            (
                new Vector3(0, (float)Size.y / 2, (float)Size.z / 2),
                Quaternion.Euler(0, 0, -90),
                (float)Size.z / 10
            );
            
            InstantiateWall
            (
                new Vector3(Size.x, (float)Size.y / 2, (float)Size.z / 2),
                Quaternion.Euler(0, 0, 90),
                (float)Size.z / 10
            );

            InstantiateWall
            (
                new Vector3((float)Size.x / 2, (float)Size.y / 2, 0),
                Quaternion.Euler(0, 90, 90),
                (float)Size.z / 10
            );
            
            InstantiateWall
            (
                new Vector3((float)Size.x / 2, (float)Size.y / 2, Size.z),
                Quaternion.Euler(0, 90, -90),
                (float)Size.z / 10
            );

            InstantiateWall
            (
                new Vector3((float)Size.x / 2, Size.y, (float)Size.z / 2),
                Quaternion.Euler(0, 0, 180),
                (float)Size.z / 10
            );
        }

        private void InstantiateWall(Vector3 position, Quaternion rotation, float scale)
        {
            var wall = Instantiate(wallPrefab, transform);
            
            wall.transform.localPosition = position;
            wall.transform.localRotation = rotation;
            wall.transform.localScale = scale * Vector3.one;
        }
    }
}
