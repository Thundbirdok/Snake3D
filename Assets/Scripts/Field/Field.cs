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
        private GameObject applePrefab;

        private bool _isAppleSet;
        
        private void Start()
        {
            InstantiateApple();
            SetAppleNewPosition();
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

        public void SetAppleNewPosition()
        {
            var x = Random.Range(0, Size.x);
            var y = Random.Range(0, Size.y);
            var z = Random.Range(0, Size.z);
            
            Apple.position = new Vector3(x, y, z);
        }
    }
}
