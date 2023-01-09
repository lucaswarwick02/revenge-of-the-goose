using UnityEngine;

public class CameraFocusComponent : MonoBehaviour
{
    [SerializeField] private Vector3 focusOffset;
    [SerializeField] private float focusFOV;

    public Vector3 FocusPosition => transform.position + focusOffset;

    public float FocusFOV => focusFOV;

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(FocusPosition, 0.1f);
    }
#endif
}
