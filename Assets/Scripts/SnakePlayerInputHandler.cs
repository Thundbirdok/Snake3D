using Lean.Touch;
using UnityEngine;

public class SnakePlayerInputHandler : SnakeInputHandler
{
    [SerializeField]
    private LeanFingerSwipe up;

    [SerializeField]
    private LeanFingerSwipe down;

    [SerializeField]
    private LeanFingerSwipe left;

    [SerializeField]
    private LeanFingerSwipe right;

    private void OnEnable()
    {
        up.onFinger.AddListener(OnUpSwipe);
        down.onFinger.AddListener(OnDownSwipe);
        left.onFinger.AddListener(OnLeftSwipe);
        right.onFinger.AddListener(OnRightSwipe);
    }
    
    private void OnDisable()
    {
        up.onFinger.RemoveListener(OnUpSwipe);
        down.onFinger.RemoveListener(OnDownSwipe);
        left.onFinger.RemoveListener(OnLeftSwipe);
        right.onFinger.RemoveListener(OnRightSwipe);
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            InvokeOnUp();
        }
        
        if (Input.GetKeyDown(KeyCode.S))
        {
            InvokeOnDown();
        }
        
        if (Input.GetKeyDown(KeyCode.A))
        {
            InvokeOnLeft();
        }
         
        if (Input.GetKeyDown(KeyCode.D))
        {
            InvokeOnRight();
        }
    }

    private void OnUpSwipe(LeanFinger arg0) => InvokeOnUp();
    private void OnDownSwipe(LeanFinger arg0) => InvokeOnDown();
    private void OnLeftSwipe(LeanFinger arg0) => InvokeOnLeft();
    private void OnRightSwipe(LeanFinger arg0) => InvokeOnRight();
}
