namespace Game.States
{
    using UnityEngine;

    public abstract class State : MonoBehaviour
    {
        private void Awake() => Disable();

        public virtual void Enable()
        {
            gameObject.SetActive(true);
        }

        public virtual void Disable()
        {
            gameObject.SetActive(false);
        }
    }
}
