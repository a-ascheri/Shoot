using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerBoundsReset : MonoBehaviour
{
    public float minX = -500f;
    public float maxX = 500f;
    public float minY = -100f;
    public float maxY = 100f;

    void Update()
    {
        Vector2 pos = transform.position;
        if (pos.x < minX || pos.x > maxX || pos.y < minY || pos.y > maxY)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
