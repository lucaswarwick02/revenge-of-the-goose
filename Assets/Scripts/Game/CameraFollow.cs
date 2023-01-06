using UnityEngine;
using Game.Utility;
using System.Collections;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private float zFollowOffset = -5.25f;
    [SerializeField] private float zGameOverOffset = -6f;
    [SerializeField] private float cameraLerpSpeed;
    [SerializeField] private GameObject gooseBoundaryPrefab;
    [SerializeField] private float cameraGameOverFOV;

    private Vector3 targetCamPos;

    private void Awake()
    {
        GameHandler.OnGameOver += OnGameOver;

        if (player)
        {
            UpdateTargetPosition(player.position, zFollowOffset, true);

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
    }

    void Update()
    {
        if (!GameHandler.IsGameOver)
        {
            UpdateTargetPosition(player.position, zFollowOffset);
        }

        transform.position = Vector3.Lerp(transform.position, targetCamPos, cameraLerpSpeed * Time.unscaledDeltaTime);
    }

    private void UpdateTargetPosition(Vector3 targetPosition, float zOffset, bool doInstantly = false)
    {
        targetCamPos = new Vector3(0, transform.position.y, targetPosition.z + zOffset);

        if (doInstantly)
        {
            transform.position = targetCamPos;
        }
    }

    private void OnGameOver(Vector3 deathCausePosition)
    {
        UpdateTargetPosition(deathCausePosition, zGameOverOffset);
        StartCoroutine(LerpToCameraFOV(cameraGameOverFOV));
    }

    private IEnumerator LerpToCameraFOV(float newFOV)
    {
        while (Mathf.Abs(Camera.main.fieldOfView - newFOV) > 0.001f)
        {
            Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, newFOV, cameraLerpSpeed * Time.unscaledDeltaTime);
            yield return null;
        }

        Camera.main.fieldOfView = newFOV;
    }
}
