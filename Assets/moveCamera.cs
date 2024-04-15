using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target; // The target object to rotate around and look at
    public float rotationSpeed = 70.0f; // The speed of rotation
    public float zoomSpeed = 10.0f; // The speed of zooming

    void Update()
    {
        // Rotate the camera around the target object when the left mouse button is held down
        if (Input.GetMouseButton(0)) // Left mouse button
        {
            float horizontalInput = Input.GetAxis("Mouse X") * rotationSpeed * Time.deltaTime;
            transform.RotateAround(target.position, Vector3.up, horizontalInput);
        }

        // Zoom in and out with the mouse wheel
        float zoomInput = Input.GetAxis("Mouse ScrollWheel") * zoomSpeed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, target.position, zoomInput);

        // Make the camera look at the target object
        transform.LookAt(target);
    }
}