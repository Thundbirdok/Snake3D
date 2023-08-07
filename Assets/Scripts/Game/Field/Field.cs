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
        public WallsHandler WallsHandler { get; private set; }

        [field: SerializeField]
        public AppleSpawner AppleSpawner { get; private set; }

        private bool _isInitialized;

        public void Setup()
        {
            if (_isInitialized == false)
            {
                WallsHandler.Initialize(this);
                AppleSpawner.Initialize(this);
                
                _isInitialized = true;
            }
            
            AppleSpawner.SetToStart();
        }

        private void Update() => WallsHandler.UpdateWalls();
    }
}
