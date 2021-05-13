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

// The following code represents the concept of an entrance portal in a 2D platformer game and contains functions to simulate the act of "teleporting" a player upon contact.

public class PortalEntranceScript : MonoBehaviour
{
    public Vector2 portalLocation;
    public PlayerController player;
    public PortalExitScript PartnerPortal;
    public AudioClip portalEntry;
    public Animator animator;

    private void Start()
    {
        animator.SetBool("isGameRunning", true);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            AudioSource.PlayClipAtPoint(portalEntry, this.transform.position);
            AudioSource.PlayClipAtPoint(portalEntry, PartnerPortal.ExitLocation);
            player.warpTo(PartnerPortal.ExitLocation);
        }
    }
}
