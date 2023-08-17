using UnityEngine;

public class CameraScreenshot : MonoBehaviour
{
    public int width = 1920;  // Screenshot width
    public int height = 1080; // Screenshot height
    public string screenshotPath = "Screenshots/"; // Path to save the screenshots

    private Camera mainCamera;
    private int screenshotCount = 0;

    private void Start()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        // Check for screenshot trigger (e.g., a keyboard input)
        if (Input.GetKeyDown(KeyCode.S))
        {
            // Set camera position and orientation
            mainCamera.transform.position = new Vector3(10f, 0.9f, -0.73f); // Example position
            mainCamera.transform.rotation = Quaternion.Euler(0f, 0f, 0f); // Example rotation
            CaptureScreenshot();

            mainCamera.transform.rotation = Quaternion.Euler(0f, 45f, 0f); // Example rotation
            CaptureScreenshot();

            mainCamera.transform.rotation = Quaternion.Euler(0f, 90f, 0f); // Example rotation
            CaptureScreenshot();

            mainCamera.transform.rotation = Quaternion.Euler(0f, 135f, 0f); // Example rotation
            CaptureScreenshot();

            mainCamera.transform.rotation = Quaternion.Euler(0f, 180f, 0f); // Example rotation
            CaptureScreenshot();

            mainCamera.transform.rotation = Quaternion.Euler(0f, 225f, 0f); // Example rotation
            CaptureScreenshot();

            mainCamera.transform.rotation = Quaternion.Euler(0f, 270f, 0f); // Example rotation
            CaptureScreenshot();

            mainCamera.transform.rotation = Quaternion.Euler(0f, 315f, 0f); // Example rotation
            CaptureScreenshot();

        }
    }

    private void CaptureScreenshot()
    {
        // Ensure the screenshot path exists
        System.IO.Directory.CreateDirectory(screenshotPath);

        // Capture the screenshot
        RenderTexture renderTexture = new RenderTexture(width, height, 24);
        mainCamera.targetTexture = renderTexture;
        Texture2D screenshotTexture = new Texture2D(width, height, TextureFormat.RGB24, false);
        mainCamera.Render();
        RenderTexture.active = renderTexture;
        screenshotTexture.ReadPixels(new Rect(0, 0, width, height), 0, 0);
        mainCamera.targetTexture = null;
        RenderTexture.active = null;
        Destroy(renderTexture);

        // Convert the screenshot texture to bytes
        byte[] bytes = screenshotTexture.EncodeToPNG();
        Destroy(screenshotTexture);

        // Save the screenshot as a PNG file
        string filename = screenshotPath + "screenshot" + screenshotCount.ToString() + ".png";
        System.IO.File.WriteAllBytes(filename, bytes);
        screenshotCount++;

        Debug.Log("Screenshot captured and saved as " + filename);
    }
}
