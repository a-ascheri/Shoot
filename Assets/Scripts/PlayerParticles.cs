using UnityEngine;

public class PlayerParticles : MonoBehaviour
{
    public GameObject shootParticlesPrefab;
    public GameObject impactParticlesPrefab;

    public void PlayShootParticles(Vector3 position)
    {
        if (shootParticlesPrefab != null)
        {
            Instantiate(shootParticlesPrefab, position, Quaternion.identity);
        }
    }

    public void PlayImpactParticles(Vector3 position, Vector2 velocity, Vector2 normal)
    {
        if (impactParticlesPrefab != null)
        {
            float impactSpeed = velocity.magnitude;
            float angle = Mathf.Atan2(normal.y, normal.x) * Mathf.Rad2Deg;
            Quaternion rot = Quaternion.AngleAxis(angle - 90f, Vector3.forward);
            GameObject impactObj = Instantiate(impactParticlesPrefab, position, rot);
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