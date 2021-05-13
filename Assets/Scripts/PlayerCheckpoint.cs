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

// The following class simulates the functionality of a Checkpoint in a 2D platformer game and contains functions to simulate the activation and positioning of a checkpoint.
public class PlayerCheckpoint : MonoBehaviour
{
    public Vector2 checkpointLocation;
    public bool isActive;
    public int checkpointNumber;

    public AudioClip checkpointPass;
    private bool hasPlayedClip;

    public GameObject Player;
    // Upon start, the checkpoint should not be active.
    void Start()
    {
        checkpointLocation = this.transform.position;
        isActive = false;
        this.GetComponent<SpriteRenderer>().color = Color.red;
    }

    // Update is called once per frame
    // Changes the status and color of the checkpoint if the player passes the checkpoint.
    void Update()
    {
        if (isActive == true)
        {
            this.GetComponent<SpriteRenderer>().color = Color.green;
        }

        else
        {
            this.GetComponent<SpriteRenderer>().color = Color.red;
            hasPlayedClip = false;
        }

        if ((transform.position - Player.transform.position).sqrMagnitude < 1f)
        {
            if (hasPlayedClip == false)
            {
                AudioSource.PlayClipAtPoint(checkpointPass, checkpointLocation);
                hasPlayedClip = true;
            }
            isActive = true;
        }
    }
}
