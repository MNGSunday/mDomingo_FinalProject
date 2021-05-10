using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCheckpoint : MonoBehaviour
{
    public Vector2 checkpointLocation;
    public bool isActive;
    public int checkpointNumber;

    public AudioClip checkpointPass;
    private bool hasPlayedClip;

    public GameObject Player;
    void Start()
    {
        checkpointLocation = this.transform.position;
        isActive = false;
        this.GetComponent<SpriteRenderer>().color = Color.red;
    }

    // Update is called once per frame
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
