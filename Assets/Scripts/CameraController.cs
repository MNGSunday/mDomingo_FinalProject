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

// The following code represents the concept of a camera within a 2D Platformer game, and contains functions to simulate a camera following the player.

public class CameraController : MonoBehaviour
{
    public Transform followTransform;
    private Vector3 smoothPos;
    private float smoothSpeed = 0.5f;

    public GameObject cameraLeftBorder;
    public GameObject cameraRightBorder;
    public GameObject cameraTopBorder;
    public GameObject cameraBottomBorder;

    private float cameraHalfWidth;
    // Start is called before the first frame update
    // Find half of the camera's width based on the game's aspect ratio.
    void Start()
    {
        cameraHalfWidth = Camera.main.orthographicSize * Camera.main.aspect;
    }

    // Update is called once per frame
    // Ensure that the camera follows the player while remaining within the boundaries of the game.
    void FixedUpdate()
    {
        float borderLeft = cameraLeftBorder.transform.position.x + cameraHalfWidth;
        float borderRight = cameraRightBorder.transform.position.x - cameraHalfWidth;
        float borderTop = cameraTopBorder.transform.position.y;
        float borderBottom = cameraBottomBorder.transform.position.y + (cameraHalfWidth / 2);

        smoothPos = Vector3.Lerp(this.transform.position,
            new Vector3(Mathf.Clamp(followTransform.position.x, borderLeft, borderRight),
            Mathf.Clamp(followTransform.position.y, borderBottom, borderTop) ,
            this.transform.position.z), smoothSpeed);

        this.transform.position = smoothPos;
    }
}
