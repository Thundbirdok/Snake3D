namespace Game.Snake
{
    using System;
    using Game.Snake.PartsPoses;
    using Game.Snake.PartsTargetPoses;
    using Unity.Collections;
    using Unity.Mathematics;
    using UnityEngine;

    public class Snake : MonoBehaviour
    {
        public event Action OnNewPositionSet;

        public bool IsActive { get; set; }
        
        public NativeArray<float3> PartsTargetPositions => _partsTargetPosesHandler.Positions;
        public float3 HeadTargetPosition => _partsTargetPosesHandler.HeadTargetPosition;
        public float3 HeadPosition => partsPosesHandler.HeadPosition;
        
        [SerializeField]
        private SnakePartsMover snakePartsMover;
        
        [SerializeField]
        private CameraMover cameraMover;

        [SerializeField]
        private SnakePartsPosesHandler partsPosesHandler;
        
        [SerializeField]
        private SnakeDrawer drawer;

        [SerializeField]
        private SnakeDirectionController directionController;

        private bool _isInitialized;

        private bool _isNeedGetTargetPositions;
        
        private SnakePartsTargetPosesHandler _partsTargetPosesHandler;

        private void OnDestroy()
        {
            directionController.Dispose();
            snakePartsMover.Dispose();
            partsPosesHandler.Dispose();
        }

        private void Update()
        {
            if (IsActive == false)
            {
                drawer.Draw();
                
                return;
            }
            
            var fixedDeltaTime = Time.fixedDeltaTime;

            if (snakePartsMover.IsTimeToSetNewTargetPositions(fixedDeltaTime))
            {
                directionController.UpdateDirection();
                directionController.TakeDirection();
                snakePartsMover.SetTargetPositions();

                _isNeedGetTargetPositions = true;
            }
            else
            {
                snakePartsMover.ScheduleNewPartsPoses();
            }
            
            drawer.Draw();
        }

        public void LateUpdate()
        {
            if (IsActive == false)
            {
                return;
            }

            if (_isNeedGetTargetPositions)
            {
                _partsTargetPosesHandler.GetPartsTargetPoses();

                _isNeedGetTargetPositions = false;
                
                OnNewPositionSet?.Invoke();
            }
            else
            {
                snakePartsMover.GetNewPartsPoses();
            }
            
            cameraMover.Move();
        }
        
        public void Setup()
        {
            if (_isInitialized == false)
            {
                Initialize();
            }
            
            partsPosesHandler.Setup();
            _partsTargetPosesHandler.SetPartsToTargets();
            directionController.Setup();
            snakePartsMover.Setup();
            cameraMover.Setup();
        }

        public void Grow() => partsPosesHandler.Grow();

        private void Initialize()
        {
            IsActive = false;
            
            _partsTargetPosesHandler = new SnakePartsTargetPosesHandler(partsPosesHandler, directionController);
            
            partsPosesHandler.Construct(_partsTargetPosesHandler);
            directionController.Construct();
            snakePartsMover.Construct(partsPosesHandler, _partsTargetPosesHandler, directionController);
            
            drawer.Construct(partsPosesHandler);
            cameraMover.Construct(partsPosesHandler, directionController, snakePartsMover);

            _isInitialized = true;
        }
    }
}
