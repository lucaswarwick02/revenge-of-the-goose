using System.Linq;
using UnityEngine;

[RequireComponent(typeof(MouseTracker))]
public class PlayerCombat : MonoBehaviour
{
    public SpriteRenderer muzzleFlash;

    public float shootDelay = 0.25f;
    float shootTimer = 0f;

    public float flashDelay = 0.15f;
    float flashTimer = 0f;

    [Header("Damage")]
    [SerializeField] private float baseDamage;
    [SerializeField] private AnimationCurve damageMultiplierByNumberOfCollisionsBefore;
    [SerializeField] private AnimationCurve damageMultiplierByDistance;

    private MouseTracker mouseTracker;

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

        if (flashTimer <= 0f && muzzleFlash.enabled) {
            muzzleFlash.enabled = false;
        }
    }

    private void Shoot()
    {
        Vector3 origin = new Vector3(transform.position.x, mouseTracker.MousePlaneHeight, transform.position.z);

        if (mouseTracker.MouseWorldPosition.HasValue)
        {
            RaycastHit[] hits = Physics.RaycastAll(origin, mouseTracker.MouseWorldPosition.Value - origin, 100).OrderBy(h => h.distance).ToArray();

            for (int i = 0; i < hits.Length; i++)
            {
                RaycastHit hit = hits[i];

                Destructible destructibleObj = hit.collider.gameObject.GetComponentInParent<Destructible>();
                Killable killableObj = hit.collider.gameObject.GetComponentInParent<Killable>();

                if (destructibleObj is not null)
                {
                    float damage = baseDamage * damageMultiplierByNumberOfCollisionsBefore.Evaluate(i) * damageMultiplierByDistance.Evaluate((hit.point - origin).magnitude);
                    destructibleObj.InflictDamage(damage);
                }
                else if (killableObj is not null)
                {
                    float damage = baseDamage * damageMultiplierByNumberOfCollisionsBefore.Evaluate(i) * damageMultiplierByDistance.Evaluate((hit.point - origin).magnitude);
                    killableObj.InflictDamage(damage);
                }
                else
                {
                    break;
                }
            }
        }
    }
}
