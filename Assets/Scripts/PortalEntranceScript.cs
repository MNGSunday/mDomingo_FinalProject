using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalEntranceScript : MonoBehaviour
{
    public Vector2 portalLocation;
    public PlayerController player;
    public PortalExitScript PartnerPortal;
    public AudioClip portalEntry;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            AudioSource.PlayClipAtPoint(portalEntry, PartnerPortal.ExitLocation);
            player.warpTo(PartnerPortal.ExitLocation);
        }
    }
}
