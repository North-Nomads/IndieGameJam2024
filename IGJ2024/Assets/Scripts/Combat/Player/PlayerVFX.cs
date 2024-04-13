using System.Collections;
using UnityEngine;

public class PlayerVFX : MonoBehaviour
{
    private ParticleSystem _tentacleHitImpactVFX;
    private ParticleSystem _landingImpactVFX;

    private void Awake()
    {
        _tentacleHitImpactVFX = Resources.Load<ParticleSystem>("Prefabs/HitImpact");
        _landingImpactVFX = Resources.Load<ParticleSystem>("Prefabs/LandingImpact");
        print($"{_landingImpactVFX}, {_tentacleHitImpactVFX}");
    }

    public void SpawnLandingVFX(Vector2 where) => StartCoroutine(SpawnAndDestroyParticles(_landingImpactVFX, where));
    public void SpawnHitVFX(Vector2 where) => StartCoroutine(SpawnAndDestroyParticles(_tentacleHitImpactVFX, where));

    private IEnumerator SpawnAndDestroyParticles(ParticleSystem system, Vector2 where)
    {
        var instance = Instantiate(system, where, Quaternion.identity);
        yield return new WaitForSeconds(1f);
        Destroy(instance.gameObject);
    }
}
