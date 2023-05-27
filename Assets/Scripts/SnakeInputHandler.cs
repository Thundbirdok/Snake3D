using System;
using UnityEngine;

public abstract class SnakeInputHandler : MonoBehaviour
{
    public event Action OnUp;
    public event Action OnDown;
    public event Action OnLeft;
    public event Action OnRight;

    protected void InvokeOnUp() => OnUp?.Invoke();
    protected void InvokeOnDown() => OnDown?.Invoke();
    protected void InvokeOnLeft() => OnLeft?.Invoke();
    protected void InvokeOnRight() => OnRight?.Invoke();
}
