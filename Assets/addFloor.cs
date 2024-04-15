using UnityEngine;

public class AddBlackFloor : MonoBehaviour
{
    void Start()
    {
        // Create a new plane at position (0, 0, 0) and rotation (0, 0, 0)
        GameObject plane = GameObject.CreatePrimitive(PrimitiveType.Plane);
        plane.transform.position = new Vector3(0, 0, 0);
        plane.transform.rotation = Quaternion.Euler(0, 0, 0);

        // Increase the size of the plane
        plane.transform.localScale = new Vector3(100, 1, 100); // Change the scale to fit your needs

        // Create a new material with black color
        Material blackMaterial = new Material(Shader.Find("Standard"));
        blackMaterial.color = Color.black;

        // Apply the black material to the plane
        plane.GetComponent<Renderer>().material = blackMaterial;
    }
}