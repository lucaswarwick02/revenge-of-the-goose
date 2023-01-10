using System.Collections;
using UnityEngine;

public class AnimalEscapeArea : MonoBehaviour
{
    [SerializeField] private float escapeAreaWidth;

    public Vector3 LineStart => transform.position - Vector3.right * escapeAreaWidth / 2;

    public Vector3 LineEnd => transform.position + Vector3.right * escapeAreaWidth / 2;

    public void TriggerAnimalEscape()
    {
        GetComponentInChildren<SpriteRenderer>().color = Color.clear;
        foreach (AnimalEscapee animal in FindObjectsOfType<AnimalEscapee>())
        {
            animal.TriggerEscape(this);
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(LineStart, LineEnd);
    }
#endif
}
