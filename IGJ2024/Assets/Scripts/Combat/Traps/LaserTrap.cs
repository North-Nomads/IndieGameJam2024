using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(CompositeCollider2D), typeof(LineRenderer))]
public class LaserTrap : MonoBehaviour
{
    private const float OffsetThreshold = 0.01f;
    [SerializeField] private float collisionWidth;
    [SerializeField] private Transform[] lasers;
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private ParticleSystem particlesPrefab;

    private Vector2 GetTwoVectorsCenter(Vector3 first, Vector3 second) => (second + first) / 2;

    /// <summary>
    /// Validates two Transforms that are not null and are not the same object
    /// </summary>
    private bool AreLasersValid(Transform transform1, Transform transform2) 
        => transform1 != null && transform2 != null && !ReferenceEquals(transform1,transform2);

    private void OnValidate()
    {
        CleanLaserSetup();
        if (lasers is not null && lasers.Length > 1)
            AddLaserCollidersToCompositeCollider();

        // Crutch to avoid unity OnValidate() restrictions
        IEnumerator RemoveInEditMode<T>(T go) where T : UnityEngine.Object
        {
            yield return new WaitForSeconds(0);
            DestroyImmediate(go);
        }

        void SetUpBoxColliderBetweenLasers(Transform first, Transform second)
        {
            try
            {
                var startLaser = first.position;
                var endLaser = second.position;

                var collider = gameObject.AddComponent<BoxCollider2D>();
                collider.offset = GetTwoVectorsCenter(startLaser, endLaser);
                collider.size = GetTwoVectorsBoxSize(startLaser, endLaser);
                collider.usedByComposite = true;
            }
            catch (UnassignedReferenceException)
            {
                print("Don't forget to assign all missing items to lasers list in the Inspector");
            }
        }

        void CleanLaserSetup()
        {
            foreach (BoxCollider2D collider2D in GetComponents<BoxCollider2D>())
                StartCoroutine(RemoveInEditMode(collider2D));

            Transform parent = transform.Find("ParticlesParent");
            for (int i = 0; i < parent.childCount; i++)
                StartCoroutine(RemoveInEditMode(parent.GetChild(i).gameObject));
        }

        void AddLaserCollidersToCompositeCollider()
        {
            var points = new Vector3[lasers.Length];
            points[0] = lasers[0].position;
            for (int i = 1; i < lasers.Length; i++)
            {
                if (!AreLasersValid(lasers[i - 1], lasers[i]))
                    continue;
                
                AlignNextColliderAlongAxis(lasers[i - 1], lasers[i]);
                SetUpBoxColliderBetweenLasers(lasers[i - 1], lasers[i]);
                points[i] = lasers[i].position;
                SpawnVFXPrefabBetween(lasers[i - 1], lasers[i]);
            }

            lineRenderer.positionCount = points.Length;
            lineRenderer.SetPositions(points);
        }
    
        void AlignNextColliderAlongAxis(Transform collider, Transform nextCollider)
        {
            var deltaX = Mathf.Abs(collider.position.x - nextCollider.position.x);
            var deltaY = Mathf.Abs(collider.position.y - nextCollider.position.y);

            // Align second collider along nearest axis
            if (deltaX > deltaY)
                nextCollider.position = new Vector3(nextCollider.position.x, collider.position.y);
            else
                nextCollider.position = new Vector3(collider.position.x, nextCollider.position.y);
        }
    
        Vector2 GetTwoVectorsBoxSize(Vector3 startLaser, Vector3 endLaser)
        {
            var deltaX = Mathf.Abs(startLaser.x - endLaser.x);
            var deltaY = Mathf.Abs(startLaser.y - endLaser.y);

            if (deltaX < OffsetThreshold)
                return new Vector2(collisionWidth, deltaY);
            else
                return new Vector2(deltaX, collisionWidth);
        }
        
        void SpawnVFXPrefabBetween(Transform transform1, Transform transform2)
        {
            Transform parent = transform.Find("ParticlesParent");

            try
            {
                var prefab = Instantiate(particlesPrefab, parent);

                var prefabShape = prefab.shape;
                prefab.transform.position = GetTwoVectorsCenter(transform1.position, transform2.position);
                prefabShape.scale = GetTwoVectorsBoxSize(transform1.position, transform2.position);
            }
            catch (UnassignedReferenceException)
            {
                print("Assign particles prefab in the Inspector");
            }
        }
    }

}
