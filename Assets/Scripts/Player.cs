using System;
using UnityEngine;

public class Player : MonoBehaviour
{

    public static Player Instance { get; private set; }

    public event EventHandler<onSelectedCounterChangedEventArgs> OnSelectedCounterChanged;
    public class onSelectedCounterChangedEventArgs : EventArgs
    {
        public ClearCounter selectedCounter;
    }

    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private float rotateSpeed = 10f;
    [SerializeField] private GameInput gameInput;
    [SerializeField] private LayerMask countersLayerMask;

    private bool isWalking;
    private Vector3 lastInteractDir;
    public ClearCounter selectedCounter;

    private void Start()
    {
        // Subscribe to the OnInteractAction event from the GameInput component
        gameInput.OnInteractAction += GameInput_OnInteractAction;
    }

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("Error: More than One Player Instances.");
            throw new Exception("Error: More than One Player Instances.");
        }
        Instance = this;
    }

    private void GameInput_OnInteractAction(object sender, EventArgs e)
    {
        if (selectedCounter != null)
        {
            selectedCounter.Interact();
        }
    }

    private void Update()
    {
        HandleMovement();
        HandleInteractions();
    }

    private void HandleMovement()
    {
        // Get input and calculate move direction
        Vector2 inputVector = gameInput.GetNormalizedMovementVector();
        Vector3 moveDir = new Vector3(inputVector.x, 0, inputVector.y);
        float playerRadius = .7f;
        float playerHeight = 2f;
        float moveDistance = moveSpeed * Time.deltaTime;

        // Check for collisions in the move direction using a capsule cast
        bool canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDir, moveDistance);

        if (!canMove)
        {
            // attempt to move only in the x direction
            Vector3 moveDirX = new Vector3(moveDir.x, 0, 0).normalized;
            canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirX, moveDistance);
            if (canMove)
            {
                moveDir = moveDirX;
            }
            else
            {
                // attempt to move only in the z direction
                Vector3 moveDirZ = new Vector3(0, 0, moveDir.z).normalized;
                canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirZ, moveDistance);
                if (canMove)
                {
                    moveDir = moveDirZ;
                }

                // if both fail, canMove will be false and the player won't move
            }
        }

        if (canMove)
        {
            transform.position += moveDir * moveDistance;
        }

        isWalking = inputVector != Vector2.zero;
        if (inputVector != Vector2.zero)
        {
            transform.forward = Vector3.Slerp(transform.forward, moveDir, rotateSpeed * Time.deltaTime);
        }
    }

    private void SetSelectedCounter(ClearCounter clearCounter)
    {
        this.selectedCounter = clearCounter;
        // Invoke the OnSelectedCounterChanged event to notify subscribers of the change
        OnSelectedCounterChanged?.Invoke(this, new onSelectedCounterChangedEventArgs { selectedCounter = selectedCounter });
    }

    private void HandleInteractions()
    {
        // Handle interactions with objects in the game world
        Vector2 inputVector = gameInput.GetNormalizedMovementVector();
        Vector3 moveDir = new Vector3(inputVector.x, 0, inputVector.y);

        if(moveDir != Vector3.zero)
        {
            lastInteractDir = moveDir;
        }

        float interactionDistance = 2f;
        // Raycasthit to check for interactable objects in the move direction
        bool canInteract = Physics.Raycast(transform.position, lastInteractDir, out RaycastHit raycastHit, interactionDistance, countersLayerMask);
        if (canInteract)
        {
            // Check if the object hit has a ClearCounter component and interact with it
            ClearCounter clearCounter = raycastHit.transform.GetComponent<ClearCounter>();
            if (clearCounter != null)
            {
                // Only update the selected counter if it's different from the current one to avoid unnecessary updates
                if (selectedCounter != clearCounter)
                {
                    SetSelectedCounter(clearCounter);
                }
            } else
            {
                // If the object hit doesn't have a ClearCounter component, deselect the current counter
                SetSelectedCounter(null);
            }
        } else
        {
            // If the raycast doesn't hit anything, deselect the current counter
            SetSelectedCounter(null);
        }
    }

    public bool IsWalking()
    {
        return isWalking;
    }
}