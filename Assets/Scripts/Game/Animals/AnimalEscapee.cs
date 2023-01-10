using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class AnimalEscapee : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private Transform image;

    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (rb.velocity.x > 0.01f)
        {
            image.localScale = new Vector3(-1, 1, 1);
        }
        else if (rb.velocity.x < -0.01f)
        {
            image.localScale = new Vector3(1, 1, 1);
        }
    }

    public void TriggerEscape(AnimalEscapeArea areaToEscapeThrough)
    {
        StartCoroutine(EscapeRoutine(areaToEscapeThrough));
    }

    private IEnumerator EscapeRoutine(AnimalEscapeArea areaToEscapeThrough)
    {
        Vector3 targetPos = FindNearestPointOnLine(areaToEscapeThrough.LineStart, areaToEscapeThrough.LineEnd, transform.position);
        Vector3 dir = (targetPos - transform.localPosition).normalized;
        float duration = Vector3.Distance(transform.position, targetPos) / speed;
        float timePassed = 0;

        while (timePassed <= duration)
        {
            rb.velocity = dir * speed;
            timePassed += Time.unscaledDeltaTime;
            yield return null;
        }

        while (enabled)
        {
            rb.velocity = Vector3.back * speed;
            yield return null;
        }
    }

    public static Vector3 FindNearestPointOnLine(Vector3 origin, Vector3 end, Vector3 point)
    {
        Vector3 lineVec = (end - origin);
        Vector3 lineDir = lineVec.normalized;

        Vector3 lhs = point - origin;
        float dotP = Vector3.Dot(lhs, lineDir);
        dotP = Mathf.Clamp(dotP, 0f, lineVec.magnitude);
        return origin + lineDir * dotP;
    }
}
