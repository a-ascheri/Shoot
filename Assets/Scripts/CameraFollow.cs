using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Seguimiento")]
    public Transform player;          // objeto a seguir
    public Vector3 offset;            // separación fija de la cámara
    public float smoothSpeed = 0.125f; // suavizado del movimiento

    [Header("Zoom")]
    public float minSize = 5f;        // zoom más cercano
    public float maxSize = 15f;       // zoom más lejano
    public float zoomSpeed = 0.05f;   // suavizado del zoom
    public float distanceFactor = 0.1f; // cuánto afecta la distancia al zoom
    public float speedFactor = 0.5f;    // cuánto afecta la velocidad al zoom

    private Rigidbody2D playerRb;

    void Start()
    {
        if (player != null)
            playerRb = player.GetComponent<Rigidbody2D>();
    }

    void LateUpdate()
    {
        if (player == null) return;

        // -----------------------
        // Movimiento suavizado
        // -----------------------
        Vector3 desiredPosition = player.position + offset;
        Vector3 smoothed = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = new Vector3(smoothed.x, smoothed.y, transform.position.z);

        // -----------------------
        // Zoom dinámico
        // -----------------------
        float distance = Vector2.Distance(player.position, Vector2.zero); // referencia 0,0
        float speed = playerRb != null ? playerRb.velocity.magnitude : 0f;

    float targetSize = minSize + distance * distanceFactor + speed * speedFactor;
    // Limitar el zoom máximo para que no se aleje demasiado
    float zoomLimit = 25f; // Puedes ajustar este valor según tu preferencia
    targetSize = Mathf.Clamp(targetSize, minSize, Mathf.Min(maxSize, zoomLimit));

    Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize, targetSize, zoomSpeed);
    }
}
