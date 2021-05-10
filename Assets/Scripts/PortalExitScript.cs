using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalExitScript : MonoBehaviour
{
    public Vector2 ExitLocation;
    void Start()
    {
        ExitLocation = this.transform.position;
    }
}
