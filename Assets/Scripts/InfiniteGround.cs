
using System.Collections.Generic;
using UnityEngine;

public class InfiniteGround : MonoBehaviour
{
    public GameObject groundSegmentPrefab; // Prefab del segmento de suelo
    public Transform player; // Referencia al player
    public int initialSegments = 3; // Cuántos segmentos instanciar al inicio
    public float segmentWidth = 250f; // Ancho de cada segmento
    public int segmentsAhead = 2; // Cuántos segmentos mantener adelante y atrás

    private LinkedList<GameObject> segments = new LinkedList<GameObject>();
    private int currentCenterIndex = 0;


    private int leftIndex;
    private int rightIndex;

    void Start()
    {
        // Instanciar segmentos iniciales centrados en X=0
        int half = initialSegments / 2;
        for (int i = -half; i <= half; i++)
        {
            float x = Mathf.Round(i * segmentWidth * 100f) / 100f;
            Vector3 pos = new Vector3(x, transform.position.y, 0);
            GameObject seg = Instantiate(groundSegmentPrefab, pos, Quaternion.identity);
            segments.AddLast(seg);
        }
        leftIndex = -half;
        rightIndex = half;
    }

    void Update()
    {
        if (player == null) return;
        float playerX = player.position.x;
        float margin = segmentWidth * 0.5f; // margen para evitar superposiciones

        // Instanciar segmento a la derecha si el player se acerca al borde derecho
        float rightEdge = Mathf.Round(rightIndex * segmentWidth * 100f) / 100f;
        if (playerX > rightEdge - margin)
        {
            rightIndex++;
            float x = Mathf.Round(rightIndex * segmentWidth * 100f) / 100f;
            Vector3 newPos = new Vector3(x, transform.position.y, 0);
            GameObject seg = Instantiate(groundSegmentPrefab, newPos, Quaternion.identity);
            segments.AddLast(seg);
            if (segments.Count > segmentsAhead * 2 + 1)
            {
                Destroy(segments.First.Value);
                segments.RemoveFirst();
                leftIndex++;
            }
        }

        // Instanciar segmento a la izquierda si el player se acerca al borde izquierdo
        float leftEdge = Mathf.Round(leftIndex * segmentWidth * 100f) / 100f;
        if (playerX < leftEdge + margin)
        {
            leftIndex--;
            float x = Mathf.Round(leftIndex * segmentWidth * 100f) / 100f;
            Vector3 newPos = new Vector3(x, transform.position.y, 0);
            GameObject seg = Instantiate(groundSegmentPrefab, newPos, Quaternion.identity);
            segments.AddFirst(seg);
            if (segments.Count > segmentsAhead * 2 + 1)
            {
                Destroy(segments.Last.Value);
                segments.RemoveLast();
                rightIndex--;
            }
        }
    }
}
