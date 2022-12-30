using Game.Utility;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    private const float MIN_SHOOT_ANGLE = -45;
    private const float MAX_SHOOT_ANGLE = 45;
    private const float MOUSE_PLANE_HEIGHT = 0.25f;

    [Header("General")]
    [SerializeField] private SpriteRenderer arms;
    [SerializeField] private float barrelLength;

    [Header("Damage")]
    [SerializeField] private float baseDamage;
    [SerializeField] private AnimationCurve damageMultiplierByNumberOfCollisionsBefore;
    [SerializeField] private AnimationCurve damageMultiplierByDistance;

    private Animator animator;
    private MouseCursor mouseCursor;

    private Vector3 initialGunUpDir;
    private Vector3 raycastBarrelPos;
    private Vector3 raycastDir;
    private Vector3 raycastPivotPos;
    private Vector3 clampedMouseTarget;

    public bool CanShoot { get; private set; } = true;

    private static bool RaycastPlaneIntersection(Vector3 linePoint, Vector3 lineDirection, out Vector3 intersectPoint)
        => VectorMaths.LinePlaneIntersection(new Vector3(0, MOUSE_PLANE_HEIGHT, 0), Vector3.up, linePoint, lineDirection, out intersectPoint);

    private void Awake()
    {
        animator = GetComponent<Animator>();
        mouseCursor = FindObjectOfType<MouseCursor>();
        initialGunUpDir = arms.transform.up;
    }

    private void Update()
    {

        if (CanShoot && Input.GetMouseButtonDown(0))
        {
            animator.SetTrigger("Shoot");
            Shoot();
        }

        CalculateShootingInfoAndRotateArms();
    }

    private void FixedUpdate()
    {
        mouseCursor.MoveCursorOverWorldPosition("crosshair", clampedMouseTarget);
    }

    public void DisableShooting()
    {
        CanShoot = false;
    }

    public void EnableShooting()
    {
        CanShoot = true;
    }

    /// <summary>
    /// Due to the 2.5D view, a lot of calculations are required to negotiate between the horizontal raycasting plane, the 30deg sprite plane, and the perspective camera.
    /// This method essentially works to rotate the gun/arm sprite to point the barrel at where the mouse intersects with the 30deg plane, and then projects (from the camera perspective)
    /// the pivot and barrel positions of the image onto the horizontal raycasting plane so that the horizontal shooting and the 30deg image match up exactly.
    /// </summary>
    private void CalculateShootingInfoAndRotateArms()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (!RaycastPlaneIntersection(ray.origin, ray.direction, out Vector3 mouseRaycastPlanePos))
        {
            Debug.LogError("Could not get mouse position on the raycast plane!");
            return;
        }

        // Calculate gun image rotation
        Vector3 armsPivot = arms.transform.position;
        Vector3 camPos = Camera.main.transform.position;
        Vector3 camToMouseDir = (mouseRaycastPlanePos - camPos).normalized;

        if (!VectorMaths.LinePlaneIntersection(armsPivot, arms.transform.forward, camPos, camToMouseDir, out Vector3 imagePlaneMouseIntersect)) // where the mouse intersects with the 30deg plane
        {
            Debug.LogError("Could not calculate gun-rotation-plane intersect!");
            return;
        }

        Vector3 desiredGunDir = imagePlaneMouseIntersect - armsPivot;
        float clampedGunAngle = Mathf.Clamp(Vector3.Angle(initialGunUpDir, desiredGunDir), MIN_SHOOT_ANGLE, MAX_SHOOT_ANGLE);
        Vector3 clampedGunDir = Vector3.RotateTowards(initialGunUpDir, desiredGunDir, Mathf.Deg2Rad * clampedGunAngle, 0);
        Vector3 imageBarrelPos = armsPivot + clampedGunDir * barrelLength;

        arms.transform.LookAt(armsPivot + arms.transform.forward, clampedGunDir);
        clampedMouseTarget = armsPivot + clampedGunDir * desiredGunDir.magnitude * Vector3.Dot(desiredGunDir.normalized, clampedGunDir.normalized);


        // Calculate shooting direction from pivot
        Vector3 camToPivotDir = (armsPivot - Camera.main.transform.position).normalized;
        Vector3 camToBarrelDir = (imageBarrelPos - Camera.main.transform.position).normalized;

        if (!RaycastPlaneIntersection(armsPivot, camToPivotDir, out raycastPivotPos)) // the arm/gun pivot projected onto the horizontal raycasting plane
        {
            Debug.LogError("Could not calculate shooting pivot as no intersection was made with the mouse-plane!");
            return;
        }

        if (!RaycastPlaneIntersection(camPos, camToBarrelDir, out raycastBarrelPos)) // the barrel position projected onto the horizontal raycasting plane
        {
            Debug.LogError("Could not calculate gun-rotation-plane intersect!");
            return;
        }

        raycastDir = (raycastBarrelPos - raycastPivotPos).normalized;
    }

    private void Shoot()
    {
        // Get all colliders hit (in order of distance from player), excluding those on the "NotPlayerShootable" layer
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

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(raycastPivotPos, raycastBarrelPos);
        Gizmos.DrawSphere(raycastBarrelPos, 0.025f);
    }
#endif
}
