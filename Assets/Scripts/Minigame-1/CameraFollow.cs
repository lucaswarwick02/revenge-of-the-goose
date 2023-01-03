using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Utility;

public class CameraFollow : MonoBehaviour
{
    public Transform player;
    public float zFollowOffset = -5.25f;

    [SerializeField] private GameObject gooseBoundaryPrefab;

    private void Awake()
    {
        UpdatePosition();

        if (player)
        {
            float vp_y = Camera.main.WorldToViewportPoint(player.position).y;
            Ray ray = Camera.main.ViewportPointToRay(new Vector3(1, vp_y, 1));
            VectorMaths.LinePlaneIntersection(player.position, player.transform.forward, ray.origin, ray.direction, out Vector3 intersect);

            Instantiate(gooseBoundaryPrefab, intersect, Quaternion.identity, transform);
            Instantiate(gooseBoundaryPrefab, -intersect, Quaternion.identity, transform);
        }
    }

    void Update()
    {
        UpdatePosition();
    }

    private void UpdatePosition()
    {
        transform.position = new Vector3(0, transform.position.y, player.position.z + zFollowOffset);
    }
}
