using Game.Utility;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(MouseTracker))]
public class PlayerCombat : MonoBehaviour
{
    private const float MIN_SHOOT_ANGLE = -45;
    private const float MAX_SHOOT_ANGLE = 45;

    [Header("General")]
    [SerializeField] private SpriteRenderer arms;
    [SerializeField] private SpriteRenderer muzzleFlash;
    [SerializeField] private float shootDelay = 0.25f;
    [SerializeField] private float flashDelay = 0.15f;

    [Header("Damage")]
    [SerializeField] private float baseDamage;
    [SerializeField] private AnimationCurve damageMultiplierByNumberOfCollisionsBefore;
    [SerializeField] private AnimationCurve damageMultiplierByDistance;

    [Header("Shooting Parameters")]
    [SerializeField] private float barrelLength;

    private MouseTracker mouseTracker;
    float shootTimer = 0f;
    float flashTimer = 0f;

    private Vector3 barrelPos;
    private Vector3 shootDir;
    private Vector3 armPivotOnMousePlane;

    private void Awake()
    {
        mouseTracker = GetComponent<MouseTracker>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && shootTimer <= 0f)
        {
            // Show the muzzle flash
            muzzleFlash.enabled = true;

            // Reset the shoot delay timer
            shootTimer = shootDelay;
            flashTimer = flashDelay;

            Shoot();
        }

        shootTimer = Mathf.Clamp(shootTimer -= Time.deltaTime, 0f, shootDelay);
        flashTimer = Mathf.Clamp(flashTimer - Time.deltaTime, 0f, flashDelay);

        if (flashTimer <= 0f && muzzleFlash.enabled)
        {
            muzzleFlash.enabled = false;
        }

        CalculateShootingInfoAndRotateArms();
    }

    private void CalculateShootingInfoAndRotateArms()
    {
        // Calculate arm-pivot as projected on the mouse-plane
        Vector3 armsPivot = arms.transform.position;
        Vector3 camToPivotDir = (armsPivot - Camera.main.transform.position).normalized;

        if (!MouseTracker.MousePlaneIntersection(armsPivot, camToPivotDir, out armPivotOnMousePlane))
        {
            Debug.LogError("Could not calculate shooting pivot as no intersection was made with the mouse-plane!");
            return;
        }

        // Calculate shooting direction from pivot
        Vector3 desiredRaycastDir = (mouseTracker.MouseWorldPosition.Value - armPivotOnMousePlane).normalized;
        float actualRaycastAngle = Mathf.Clamp(Vector3.Angle(Vector3.forward, desiredRaycastDir) * (desiredRaycastDir.x < 0 ? -1 : 1), MIN_SHOOT_ANGLE, MAX_SHOOT_ANGLE);
        shootDir = Quaternion.Euler(0, actualRaycastAngle, 0) * Vector3.forward;
        barrelPos = armPivotOnMousePlane + shootDir * barrelLength;

        // Calculate gun image rotation
        Vector3 camPos = Camera.main.transform.position;
        Vector3 camToBarrelDir = (barrelPos - camPos).normalized;

        if (!VectorMaths.LinePlaneIntersection(armsPivot, arms.transform.forward, camPos, camToBarrelDir, out Vector3 gunRotationPlaneIntersect))
        {
            Debug.LogError("Could not calculate gun-rotation-plane intersect!");
            return;
        }

        Vector3 desiredGunDir = gunRotationPlaneIntersect - armsPivot;
        arms.transform.LookAt(armsPivot + camToPivotDir, desiredGunDir);
    }

    private void Shoot()
    {
        if (mouseTracker.MouseWorldPosition.HasValue)
        {
            RaycastHit[] hits = Physics.RaycastAll(barrelPos, shootDir, 100, ~(1 << LayerMask.NameToLayer("NotPlayerShootable"))).OrderBy(h => h.distance).ToArray();

            for (int i = 0; i < hits.Length; i++)
            {
                RaycastHit hit = hits[i];

                Destructible destructibleObj = hit.collider.gameObject.GetComponentInParent<Destructible>();

                if (destructibleObj is not null)
                {
                    float damage = baseDamage * damageMultiplierByNumberOfCollisionsBefore.Evaluate(i) * damageMultiplierByDistance.Evaluate((hit.point - barrelPos).magnitude);
                    destructibleObj.InflictDamage(damage, hit);
                }
                else
                {
                    break;
                }
            }
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(armPivotOnMousePlane, barrelPos);
    }
#endif
}
