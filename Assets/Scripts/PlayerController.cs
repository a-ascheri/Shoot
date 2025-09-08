using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float launchPower = 10f;
    private Rigidbody2D rb;

    private Vector2 startPoint;
    private Vector2 endPoint;
    private bool isDragging = false;

    private LineRenderer line; // para mostrar dirección

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
}
