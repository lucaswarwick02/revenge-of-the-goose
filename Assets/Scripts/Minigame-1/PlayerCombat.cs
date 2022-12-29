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

    private Vector3 initialGunUpDir;
    private Vector3 raycastBarrelPos;
    private Vector3 raycastDir;
    private Vector3 raycastPivotPos;

    private void Awake()
    {
        mouseTracker = GetComponent<MouseTracker>();
        initialGunUpDir = arms.transform.up;
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

        if (!MouseTracker.MousePlaneIntersection(armsPivot, camToPivotDir, out raycastPivotPos))
        {
            Debug.LogError("Could not calculate shooting pivot as no intersection was made with the mouse-plane!");
            return;
        }


        // Calculate gun image rotation
        Vector3 camPos = Camera.main.transform.position;
        Vector3 camToMouseDir = (mouseTracker.MouseWorldPosition.Value - camPos).normalized;

        if (!VectorMaths.LinePlaneIntersection(armsPivot, arms.transform.forward, camPos, camToMouseDir, out Vector3 imagePlaneMouseIntersect))
        {
            Debug.LogError("Could not calculate gun-rotation-plane intersect!");
            return;
        }

        Vector3 desiredGunDir = imagePlaneMouseIntersect - armsPivot;
        float clampedGunAngle = Mathf.Clamp(Vector3.Angle(initialGunUpDir, desiredGunDir), MIN_SHOOT_ANGLE, MAX_SHOOT_ANGLE);
        Vector3 clampedGunDir = Vector3.RotateTowards(initialGunUpDir, desiredGunDir, Mathf.Deg2Rad * clampedGunAngle, 0);
        Vector3 imageBarrelPos = armsPivot + clampedGunDir * barrelLength;

        arms.transform.LookAt(armsPivot + arms.transform.forward, clampedGunDir);


        // Calculate shooting direction from pivot

        if (!MouseTracker.MousePlaneIntersection(camPos, imageBarrelPos - camPos, out raycastBarrelPos))
        {
            Debug.LogError("Could not calculate gun-rotation-plane intersect!");
            return;
        }

        raycastDir = (raycastBarrelPos - raycastPivotPos).normalized;
    }

    private void Shoot()
    {
        if (mouseTracker.MouseWorldPosition.HasValue)
        {
            RaycastHit[] hits = Physics.RaycastAll(raycastBarrelPos, raycastDir, 100, ~(1 << LayerMask.NameToLayer("NotPlayerShootable"))).OrderBy(h => h.distance).ToArray();

            for (int i = 0; i < hits.Length; i++)
            {
                RaycastHit hit = hits[i];

                Destructible destructibleObj = hit.collider.gameObject.GetComponentInParent<Destructible>();

                if (destructibleObj is not null)
                {
                    float damage = baseDamage * damageMultiplierByNumberOfCollisionsBefore.Evaluate(i) * damageMultiplierByDistance.Evaluate((hit.point - raycastBarrelPos).magnitude);
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
        Gizmos.color = Color.red;
        Gizmos.DrawLine(raycastPivotPos, raycastBarrelPos);
        Gizmos.DrawSphere(raycastBarrelPos, 0.025f);
    }
#endif
}
