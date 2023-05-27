using System;
using System.Collections.Generic;
using UnityEngine;

public class Snake : MonoBehaviour
{
    public event Action OnAddPart;
    
    [field: SerializeField]
    public Transform Direction { get; set; }

    [field: SerializeField]
    public Transform CameraTarget { get; private set; }
    
    [field: SerializeField]
    public List<GameObject> Parts { get; private set; }

    [SerializeField]
    private SnakeMover snakeMover;

    [SerializeField]
    private SnakeDirectionController directionController;
    
    private void Awake()
    {
        snakeMover.Construct(this);
        directionController.Construct(this);
    }

    private void OnDestroy()
    {
        directionController.Dispose();
        //snakeMover.Dispose();
    }

    private void FixedUpdate()
    {
        directionController.UpdateDirection();
        snakeMover.Move();
    }

    public void AddPart()
    {
        OnAddPart?.Invoke();
    }
}
