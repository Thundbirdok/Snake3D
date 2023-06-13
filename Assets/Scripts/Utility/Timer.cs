using UnityEngine;

namespace Utility
{
    using System;

    [Serializable]
    public class Timer
    {
        [field: SerializeField]
        public float Duration { get; private set; }
        
        private float _time;

        public Timer(float duration)
        {
            Duration = duration;
        }

        public void ResetTime()
        {
            _time = 0f;
        }

        public bool AddTime(float time)
        {
            _time += time;

            if (_time < Duration)
            {
                return false;
            }

            _time = 0f;
            
            return true;
        }
    }
}
