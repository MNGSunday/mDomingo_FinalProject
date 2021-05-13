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

// The following class represents the concept of an exit portal in a 2D platformer game and simulates the "exit point" for a player that enters an entrance portal.
public class PortalExitScript : MonoBehaviour
{
    public Animator animator; 

    public Vector2 ExitLocation;
    void Start()
    {
        ExitLocation = this.transform.position;
        animator.SetBool("isGameRunning", true);
    }
}
