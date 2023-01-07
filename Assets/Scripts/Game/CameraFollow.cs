using UnityEngine;
using Game.Utility;

public class CameraFollow : MonoBehaviour
{
    private const float FOLLOW_LERP_SPEED = 5;
    private const float FOV_LERP_SPEED = 2;

    private const float PLAYER_OFFSET = -4.5f;
    private const float PLAYER_FOV = 40;

    private const float CONVERSATION_OFFSET = -7.5f;
    private const float CONVERSATION_FOV = 30;

    private const float GAME_OVER_OFFSET = -7f;
    private const float GAME_OVER_FOV = 20;

    [SerializeField] private Transform player;
    [SerializeField] private GameObject gooseBoundaryPrefab;

    private enum Mode
    {
        Player,
        Target,
    }

    private Mode currentMode;
    private Transform target;
    private Vector3 targetPos;
    private float targetOffset;
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
            UpdateTargetPosition(player.position);
        }
        else if (target != null)
        {
            UpdateTargetPosition(target.position);
        }

        transform.position = Vector3.Lerp(transform.position, targetPos, FOLLOW_LERP_SPEED * Time.unscaledDeltaTime);
        Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, targetFOV, FOV_LERP_SPEED * Time.unscaledDeltaTime);
    }

    public void SetPlayerMode()
    {
        currentMode = Mode.Player;
        targetFOV = PLAYER_FOV;
        targetOffset = PLAYER_OFFSET;
    }

    public void SetTargetMode(Transform target, float offset, float FOV)
    {
        currentMode = Mode.Target;
        this.target = target;
        targetOffset = offset;
        targetFOV = FOV;
    }

    public void SetTargetMode(Vector3 position, float offset, float FOV)
    {
        currentMode = Mode.Target;
        target = null;
        targetOffset = offset;
        targetFOV = FOV;
        UpdateTargetPosition(position);
    }

    private void UpdateTargetPosition(Vector3 targetPosition, bool doInstantly = false)
    {
        targetPos = new Vector3(0, transform.position.y, targetPosition.z + targetOffset);

        if (doInstantly)
        {
            transform.position = targetPos;
        }
    }

    private void OnStartConversation(Vector3 converserPos)
    {
        SetTargetMode(converserPos, CONVERSATION_OFFSET, CONVERSATION_FOV);
    }

    private void OnGameOver(Vector3 killerPos)
    {
        SetTargetMode(killerPos, GAME_OVER_OFFSET, GAME_OVER_FOV);
    }
}
