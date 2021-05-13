/*
Name: Marc Domingo
Student ID: 2346778
Chapman Email: mdomingo@chapman.edu
Course Number and Section: 236-03
Assignment: Final Project
This is my own work, and I did not cheat on this assignment.
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// The following class represents the concept of an Enemy Waypoint in a 2D Platformer and contains functionality to simulate a patrolling enemy's sentry points.
public class EnemyWaypoint : MonoBehaviour
{
    public bool IsEndpoint;
    public bool IsSentry = false;
    public float PauseTime = 1.0f;
    public Vector2 waypointPosition;

    // At the start of build, record this location.
    void Start()
    {
        waypointPosition = transform.position;
    }
}
