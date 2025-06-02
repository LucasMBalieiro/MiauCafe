using UnityEngine;

public class BackgroundScale : MonoBehaviour
{
    private void Start()
    {
        ScaleBackground();
    }

    private void ScaleBackground()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();

        Camera cam = Camera.main;

        if (cam == null || spriteRenderer == null)
        {
            Debug.LogError("Camera ou sprite errados");
        }
        
        float camHeight = cam.orthographicSize * 2f; 
        float camWidth = camHeight * cam.aspect;


        float spriteHeight = spriteRenderer.sprite.bounds.size.y;
        float spriteWidth = spriteRenderer.sprite.bounds.size.x;

        // Calculate the scale factors
        float scaleX = camWidth / spriteWidth;
        float scaleY = camHeight / spriteHeight;


        float uniformScale = Mathf.Max(scaleX, scaleY); // For covering the entire screen (may crop)
        // float uniformScale = Mathf.Min(scaleX, scaleY); // For fitting entirely on screen (may leave borders)

        transform.localScale = new Vector3(uniformScale, uniformScale, 1f);

        // Optional: If you want to perfectly center the background
        transform.position = new Vector3(cam.transform.position.x, cam.transform.position.y, transform.position.z);
    }
}