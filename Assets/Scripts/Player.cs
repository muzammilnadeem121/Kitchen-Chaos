using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private float rotateSpeed = 10f;
    [SerializeField] private GameInput gameInput;
    private bool isWalking;
    
    private void Update()
    {
        Vector2 inputVector = gameInput.GetNormalizedMovementVector();
        Vector3 moveDir = new Vector3(inputVector.x, 0, inputVector.y);
        transform.position += moveDir * moveSpeed * Time.deltaTime;
        isWalking = inputVector != Vector2.zero;
        if (inputVector != Vector2.zero)
        {
            transform.forward = Vector3.Slerp(transform.forward, moveDir, rotateSpeed * Time.deltaTime);
        }
    }

    public bool IsWalking()
    {
        return isWalking;
    }
}
