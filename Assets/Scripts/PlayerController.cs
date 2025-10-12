using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float launchPower = 10f;
    private Rigidbody2D rb;

    private Vector2 startPoint;
    private Vector2 endPoint;
    private bool isDragging = false;

    private LineRenderer line; // para mostrar dirección

    [Header("Efectos de Partículas")]
    public GameObject shootParticlesPrefab;
    public GameObject impactParticlesPrefab;

    private bool hasLaunched = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        line = GetComponent<LineRenderer>();
        line.positionCount = 0; // arranca sin nada
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
            DrawLine(startPoint, currentPoint);
        }

        // soltar
        if (Input.GetMouseButtonUp(0) && isDragging)
        {
            endPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Launch();
            ClearLine();
            isDragging = false;
        }
    }

    void Launch()
    {
        Vector2 direction = startPoint - endPoint; 
        rb.velocity = direction * launchPower;
        hasLaunched = true;
        // Instanciar partículas de disparo
        if (shootParticlesPrefab != null)
        {
            Instantiate(shootParticlesPrefab, transform.position, Quaternion.identity);
        }
    }

    void DrawLine(Vector2 start, Vector2 end)
    {
        line.positionCount = 2;
        line.SetPosition(0, start);
        line.SetPosition(1, end);
    }

    void ClearLine()
    {
        line.positionCount = 0;
    }

    void OnCollisionEnter2D(Collision2D collision)
{
    if (impactParticlesPrefab != null)
    {
        float impactSpeed = rb.velocity.magnitude;
        Vector2 normal = collision.contacts.Length > 0 ? collision.contacts[0].normal : Vector2.up;
        float angle = Mathf.Atan2(normal.y, normal.x) * Mathf.Rad2Deg;
        Quaternion rot = Quaternion.AngleAxis(angle - 90f, Vector3.forward);
        GameObject impactObj = Instantiate(impactParticlesPrefab, transform.position, rot);
        ParticleSystem ps = impactObj.GetComponent<ParticleSystem>();
        if (ps != null)
        {
            int burstCount = Mathf.Clamp(Mathf.RoundToInt(impactSpeed * 10), 20, 50);
            var main = ps.main;
            main.startSize = Mathf.Clamp(impactSpeed * 0.05f, 0.1f, 0.5f);
            main.startSpeed = Mathf.Clamp(impactSpeed * 1.5f, 2f, 10f);
            ps.Emit(burstCount);
        }
    }
}
}
