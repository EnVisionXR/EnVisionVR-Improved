using UnityEngine;

public class CameraController : MonoBehaviour
{
    bool canMoveCamera = true;
    public float movementSpeed = 5f;
    public float rotationSpeed = 100f;

    private void Update()
    {
        if (canMoveCamera)
        {
            // Camera movement
            float horizontalMovement = Input.GetAxis("Horizontal");
            float verticalMovement = Input.GetAxis("Vertical");

            Vector3 movement = new Vector3(horizontalMovement, 0f, verticalMovement) * movementSpeed * Time.deltaTime;
            transform.Translate(movement);

            // Camera rotation
            float mouseX = Input.GetAxis("Mouse X");

            Vector3 rotation = new Vector3(0f, mouseX, 0f) * rotationSpeed * Time.deltaTime;
            transform.Rotate(rotation);
        }
    }
    
    public void DisableCameraMovement()
    {
        canMoveCamera = false;
    }

    public void EnableCameraMovement()
    {
        canMoveCamera = true;
    }
}
