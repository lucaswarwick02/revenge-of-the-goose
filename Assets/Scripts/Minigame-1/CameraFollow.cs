using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;
    public float zFollowOffset = -5.25f;

    void Update()
    {
        transform.position = new Vector3(0, transform.position.y, player.position.z + zFollowOffset);
    }
}
