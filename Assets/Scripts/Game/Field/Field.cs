namespace Game.Field
{
    using Game.Field.Wall;
    using Game.Snake;
    using UnityEngine;

    public class Field : MonoBehaviour
    {
        [field: SerializeField]
        public Vector3Int Size { get; private set; }

        [field: SerializeField]
        public Snake Snake { get; private set; }
        
        [field: SerializeField]
        public WallSpawner WallSpawner { get; private set; }

        [field: SerializeField]
        public AppleSpawner AppleSpawner { get; private set; }

        private bool _isInitialized;

        public void Setup()
        {
            if (_isInitialized == false)
            {
                WallSpawner.Initialize(this);
                AppleSpawner.Initialize(this);
                
                _isInitialized = true;
            }

            WallSpawner.Setup();
            AppleSpawner.SetToStart();
        }
    }
}
