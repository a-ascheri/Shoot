using UnityEngine;

[ExecuteAlways]
public class PlayerPowerBar : MonoBehaviour
{
    public LineRenderer line;
    public float maxForce = 100f;

void OnEnable()
{
    if (line == null) line = GetComponent<LineRenderer>();
    line.positionCount = 0;
}

    public void DrawBar(Vector2 start, Vector2 end, float force)
    {
        float forceRatio = Mathf.Clamp01(force / maxForce);
        Vector2 actualEnd = Vector2.Lerp(start, end, forceRatio);

        line.positionCount = 2;
        line.SetPosition(0, start);
        line.SetPosition(1, actualEnd);

        // Color final depende de la fuerza
        Color endColor;
        if (force < 10f)
            endColor = Color.green;
        else if (force < 95f)
            endColor = Color.yellow;
        else
            endColor = Color.red;

        Gradient gradient = new Gradient();
        gradient.SetKeys(
            new GradientColorKey[] {
                new GradientColorKey(Color.green, 0f),                                // Verde al inicio de la barra (0%)
                new GradientColorKey(Color.green, Mathf.Clamp01(forceRatio * 0.3f)),  // Amarillo en el punto medio según la fuerza
                new GradientColorKey(Color.yellow, Mathf.Clamp01(forceRatio * 0.8f)),
                new GradientColorKey(Color.red, Mathf.Clamp01(forceRatio * 0.99f)),
                new GradientColorKey(Color.red, forceRatio),                          // Rojo en el punto de fuerza actual
                new GradientColorKey(Color.clear, 1f)                                 // Transparente al final (100%)
            },
            new GradientAlphaKey[] {
                new GradientAlphaKey(1f, 0f),       // 1) En el inicio de la barra (0%), la barra es opaca (alpha = 1)
                new GradientAlphaKey(0.75f, 0.5f),  // 2) En el mismo inicio (0%), sigue siendo opaca (repetido, no aporta diferencia)
                new GradientAlphaKey(0.5f, 0.25f),  // 3) En la mitad de la barra (50%), sigue siendo opaca (alpha = 1)
                new GradientAlphaKey(0f, 1f)        // 4) En el extremo final (100%), la barra es completamente transparente (alpha = 0)
        }
        );
        line.colorGradient = gradient;

        float minWidth = 0.05f;
        float maxWidth = Mathf.Clamp(force * 0.15f, 0.1f, 0.5f);
        line.startWidth = maxWidth;
        line.endWidth = minWidth;
    }

    public void ClearBar()
    {
        line.positionCount = 0;
    }
}