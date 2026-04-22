using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 7f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    // Update is called once per frame
    private void Update()
    {
        Vector3 inputVector = new Vector3(0, 0, 0);
        if (Input.GetKey(KeyCode.W))
        {
            inputVector.z = +1;
        }
        if (Input.GetKey(KeyCode.S))
        {
            inputVector.z = -1;
        }
        if (Input.GetKey(KeyCode.A))
        {
            inputVector.x = -1;
        }
        if (Input.GetKey(KeyCode.D))
        {
            inputVector.x = +1;
        }
        inputVector = inputVector.normalized;
        transform.position += inputVector * moveSpeed * Time.deltaTime;
    }
}
