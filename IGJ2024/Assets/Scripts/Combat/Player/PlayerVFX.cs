using Cinemachine;
using System.Collections;
using UnityEngine;

public class PlayerVFX : MonoBehaviour
{
    [SerializeField] private float maxZoomMultiplier;
    [SerializeField] private float zoomInDuration;
    [SerializeField] private float zoomOutDuration;

    public CinemachineVirtualCamera VirtualCamera { get; set; }

    private ParticleSystem _tentacleHitImpactVFX;
    private ParticleSystem _landingImpactVFX;
    private float _defaultZoom;
    private float _zoomedZoom;
    private Coroutine _currentZoomCoroutine;

    private void Awake()
    {
        _tentacleHitImpactVFX = Resources.Load<ParticleSystem>("Prefabs/HitImpact");
        _landingImpactVFX = Resources.Load<ParticleSystem>("Prefabs/LandingImpact");
    }

    private void Start()
    {
        _defaultZoom = Camera.main.orthographicSize;
        _zoomedZoom = _defaultZoom / maxZoomMultiplier;
    }

    public void SpawnLandingVFX(Vector2 where) => StartCoroutine(SpawnAndDestroyParticles(_landingImpactVFX, where));
    public void SpawnHitVFX(Vector2 where) => StartCoroutine(SpawnAndDestroyParticles(_tentacleHitImpactVFX, where));

    private IEnumerator SpawnAndDestroyParticles(ParticleSystem system, Vector2 where)
    {
        var instance = Instantiate(system, where, Quaternion.identity);
        yield return new WaitForSeconds(1f);
        Destroy(instance.gameObject);
    }

    public void AnimateCameraZoom(bool shouldZoomIn)
    {
        if (_currentZoomCoroutine != null)
            StopCoroutine(_currentZoomCoroutine);

        _currentZoomCoroutine = StartCoroutine(PerformZoomCoroutine());


        IEnumerator PerformZoomCoroutine()
        {
            var duration = zoomInDuration;
            var startValue = _defaultZoom;
            var endValue = _zoomedZoom;

            if (!shouldZoomIn)
            {
                startValue = _zoomedZoom;
                endValue = _defaultZoom;
                duration = zoomOutDuration;
            }

            var elapsedTime = 0f;
            while (elapsedTime < duration)
            {
                VirtualCamera.m_Lens.OrthographicSize = Mathf.Lerp(startValue, endValue, elapsedTime / zoomInDuration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
        }
    }
}
