using UnityEngine;

public class PlayerPowerBar : MonoBehaviour
{
    public LineRenderer line;
    public float maxForce = 100f;

    void Awake()
    {
        if (line == null) line = GetComponent<LineRenderer>();
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
        else if (force < 20f)
            endColor = Color.yellow;
        else
            endColor = Color.red;

        Gradient gradient = new Gradient();
        gradient.SetKeys(
            new GradientColorKey[] {
                new GradientColorKey(Color.green, 0f),
                new GradientColorKey(endColor, forceRatio),
                new GradientColorKey(endColor, 0.8f),
                new GradientColorKey(Color.clear, 0f)
            },
            new GradientAlphaKey[] {
                new GradientAlphaKey(1f, 0f),
                new GradientAlphaKey(1f, 0f),
                new GradientAlphaKey(1f, 0f),
                new GradientAlphaKey(0f, 1f)
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