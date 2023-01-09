using UnityEngine;
using Game.Utility;

public class CameraFollow : MonoBehaviour
{
    private const float FOLLOW_LERP_SPEED = 5;
    private const float FOV_LERP_SPEED = 2;

    private const float PLAYER_OFFSET = -4.5f;
    private const float PLAYER_FOV = 40;

    [SerializeField] private Transform player;
    [SerializeField] private GameObject gooseBoundaryPrefab;
    [SerializeField] private float cameraTargetZOffset;

    private enum Mode
    {
        Player,
        Target,
    }

    private Mode currentMode;
    private CameraFocusComponent focusTarget;

    private Vector3 targetPos;
    private float targetFOV;

    private void Awake()
    {
        SetPlayerMode();

        GameHandler.OnGameOver += OnGameOver;
        Converser.OnStartConversation += OnStartConversation;
        Converser.OnEndConversation += SetPlayerMode;

        if (player)
        {
            UpdateTargetPosition(player.position, true);

            float vp_y = Camera.main.WorldToViewportPoint(player.position).y;
            Ray ray = Camera.main.ViewportPointToRay(new Vector3(1, vp_y, 1));
            VectorMaths.LinePlaneIntersection(player.position, player.transform.forward, ray.origin, ray.direction, out Vector3 intersect);

            Instantiate(gooseBoundaryPrefab, intersect, Quaternion.identity, transform);
            Instantiate(gooseBoundaryPrefab, -intersect, Quaternion.identity, transform);
        }
        else
        {
            Debug.LogError("Could not find player!");
            enabled = false;
        }
    }

    private void OnDisable()
    {
        GameHandler.OnGameOver -= OnGameOver;
        Converser.OnStartConversation -= OnStartConversation;
        Converser.OnEndConversation -= SetPlayerMode;
    }

    void Update()
    {
        if (currentMode == Mode.Player)
        {
            UpdateTargetPosition(player.position + Vector3.forward * PLAYER_OFFSET);
        }
        else if (focusTarget != null)
        {
            UpdateTargetPosition(focusTarget.FocusPosition + Vector3.forward * cameraTargetZOffset, freeX: true);
        }

        transform.position = Vector3.Lerp(transform.position, targetPos, FOLLOW_LERP_SPEED * Time.unscaledDeltaTime);
        Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, targetFOV, FOV_LERP_SPEED * Time.unscaledDeltaTime);
    }

    public void SetPlayerMode()
    {
        currentMode = Mode.Player;
        targetFOV = PLAYER_FOV;
    }

    public void SetTargetMode(CameraFocusComponent focusTarget)
    {
        currentMode = Mode.Target;
        this.focusTarget = focusTarget;
        targetFOV = focusTarget.FocusFOV;
    }

    private void UpdateTargetPosition(Vector3 targetPosition, bool doInstantly = false, bool freeX = false)
    {
        targetPos = new Vector3(freeX ? targetPosition.x : 0, transform.position.y, targetPosition.z);

        if (doInstantly)
        {
            transform.position = targetPos;
        }
    }

    private void OnStartConversation(Transform converser)
    {
        if (converser.TryGetComponent(out CameraFocusComponent focusTarget))
        {
            SetTargetMode(focusTarget);
        }
        else
        {
            Debug.LogError("No focus target could be found!");
        }
    }

    private void OnGameOver(Transform killer)
    {
        if (killer.TryGetComponent(out CameraFocusComponent focusTarget))
        {
            SetTargetMode(focusTarget);
        }
        else
        {
            Debug.LogError("No focus target could be found!");
        }
    }
}
