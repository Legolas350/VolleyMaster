using UnityEngine;

public class SetResolution : MonoBehaviour
{
    public int targetWidth = 1920;
    public int targetHeight = 1080;
    public bool fullscreen = true;

    void Start()
    {
        Screen.SetResolution(targetWidth, targetHeight, fullscreen);
    }
}
