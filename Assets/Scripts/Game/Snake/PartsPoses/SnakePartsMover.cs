namespace Game.Snake.PartsPoses
{
    using System;
    using Game.Snake.PartsTargetPoses;
    using Unity.Burst;
    using Unity.Jobs;
    using Unity.Jobs.LowLevel.Unsafe;
    using UnityEngine;
    using Utility;

    [Serializable, BurstCompile]
    public class SnakePartsMover
    {
        public float Delay => timer.Duration;

        [SerializeField]
        private Timer timer;
        
        [SerializeField]
        private float moveTime = 0.25f;

        private SnakePartsPosesHandler _partsPosesHandler;
        private SnakePartsTargetPosesHandler _partsTargetPosesHandler;
        private SnakeDirectionController _directionController;
        
        private bool _isPartsMoved;

        private JobHandle _positionJob;
        private JobHandle _rotationJob;
        
        public void Construct
        (
            SnakePartsPosesHandler partsPosesHandler,
            SnakePartsTargetPosesHandler partsTargetPosesHandler,
            SnakeDirectionController directionController
        )
        {
            _partsPosesHandler = partsPosesHandler;
            _partsTargetPosesHandler = partsTargetPosesHandler;
            _directionController = directionController;
            
            _partsPosesHandler.OnPartAdded += AddTargetForLastPart;
        }

        public void Dispose()
        {
            if (_partsPosesHandler != null)
            {
                _partsPosesHandler.OnPartAdded -= AddTargetForLastPart;
            }
        }

        public void Setup()
        {
            timer.SetTimeToMax();
            _isPartsMoved = true;
        }

        public bool IsTimeToSetNewTargetPositions(float time)
        {
            var isTime = timer.AddTime(time);

            var isForceTurnCauseInput = _isPartsMoved && _directionController.IsUpdated;

            return isTime || isForceTurnCauseInput;
        } 
        
        [BurstCompile]
        public void ScheduleNewPartsPoses()
        {
            if (_isPartsMoved)
            {
                return;
            }
            
            var delta = Time.fixedDeltaTime / moveTime;

            _isPartsMoved = true;
            
            _positionJob = new MovePositionToTargetJob()
            {
                PartsTargetPositions = _partsTargetPosesHandler.Positions,
                PartsPositions = _partsPosesHandler.PartsPositions,
                Delta = delta
            }.Schedule(_partsTargetPosesHandler.Positions.Length, JobsUtility.JobWorkerMaximumCount / 2);
            
            _rotationJob = new MoveRotationToTargetJob()
            {
                PartsTargetRotations = _partsTargetPosesHandler.Rotations,
                PartsRotations = _partsPosesHandler.PartsRotations,
                Delta = delta
            }.Schedule(_partsTargetPosesHandler.Rotations.Length, JobsUtility.JobWorkerMaximumCount / 2);
        }

        [BurstCompile]
        public void GetNewPartsPoses()
        {
            _positionJob.Complete();
            _rotationJob.Complete();

            _isPartsMoved = true;
            
            for (var i = 0; i < _partsTargetPosesHandler.Positions.Length; i++)
            {
                var position = _partsPosesHandler.PartsPositions[i];
                var rotation = _partsPosesHandler.PartsRotations[i];

                var targetPosition = _partsTargetPosesHandler.Positions[i];
                var targetRotation = _partsTargetPosesHandler.Rotations[i];

                if (position.Equals(targetPosition) == false)
                {
                    _isPartsMoved = false;
                }
                else if (rotation.Equals(targetRotation) == false)
                {
                    _isPartsMoved = false;
                }
            }
        }
        
        public void SetTargetPositions()
        {
            if (_isPartsMoved == false)
            {
                return;
            }
        
            _isPartsMoved = false;
            timer.ResetTime();
            
            _partsTargetPosesHandler.SchedulePartsTargetPoses();
        }

        private void AddTargetForLastPart()
        {
            _partsTargetPosesHandler.AddTargetForLastPart();
        }
    }
}
