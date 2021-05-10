using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWaypoint : MonoBehaviour
{
    public bool IsEndpoint;
    public bool IsSentry = false;
    public float PauseTime = 1.0f;
    public Vector2 waypointPosition;

    void Start()
    {
        waypointPosition = transform.position;
    }
}
