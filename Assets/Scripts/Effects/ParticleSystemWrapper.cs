using UnityEngine;

namespace Effects
{
    using System;

    public class ParticleSystemWrapper : MonoBehaviour
    {
        public event Action<ParticleSystemWrapper> OnStopped;

        [SerializeField]
        private ParticleSystem system;

        public void OnParticleSystemStopped() => OnStopped?.Invoke(this);

        public void Play() => system.Play();

        public void Stop() => system.Stop();

        public void Pause() => system.Pause();

        public void Clear() => system.Clear();
    }
}
