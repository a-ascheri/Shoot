using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public PlayerParticles particles;
    public PlayerPowerBar powerBar;
    public float launchPower = 10f;
    
    private Rigidbody2D rb;
    private Vector2 startPoint;
    private Vector2 endPoint;
    private bool isDragging = false;
    private bool hasLaunched = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // inicio drag
        if (Input.GetMouseButtonDown(0))
        {
            startPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            isDragging = true;
        }

        // mientras arrastrás
        if (Input.GetMouseButton(0) && isDragging)
        {
            Vector2 currentPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            float force = (startPoint - currentPoint).magnitude;
            powerBar.DrawBar(startPoint, currentPoint, force);
        }

        // soltar
        if (Input.GetMouseButtonUp(0) && isDragging)
        {
            endPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Launch();
            powerBar.ClearBar();
            isDragging = false;
        }
    }

    void Launch()
    {
        Vector2 direction = startPoint - endPoint; 
        rb.velocity = direction * launchPower;
        hasLaunched = true;
        // Instanciar partículas de disparo
        if (particles != null)
            particles.PlayShootParticles(transform.position);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (particles != null)
        {
            Vector2 normal = collision.contacts.Length > 0 ? collision.contacts[0].normal : Vector2.up;
            particles.PlayImpactParticles(transform.position, rb.velocity, normal);
        }
    }
}
