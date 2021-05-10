using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject player;
    public CharacterController2D controller;
    public Animator animator;

    public AudioClip jumpClip;
    public AudioClip hurtClip;
    public AudioClip escapeClip;
    public float runSpeed = 30f;
    public Sprite[] playerSprite = new Sprite[2];

    float horizontalMove = 0f;
    bool jumpFlag = false;
    bool jump = false;

    public int numberOfDeaths;
    public bool isDead = false;
    public List<PlayerCheckpoint> Checkpoint = new List<PlayerCheckpoint>();
    // public int respawnAtCheckpoint = 0;
    private int highestActiveCheckpoint = 0;
    public bool hasEscaped = false;

    // Update is called once per frame
    void Update()
    {
        horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;

        animator.SetFloat("Speed", Mathf.Abs(horizontalMove));

        if (jumpFlag)
        {
            jumpFlag = false;
            animator.SetBool("isJumping", jump);
        }

        if (Input.GetButtonDown("Jump"))
        {
            if (animator.GetBool("isJumping") == false)
            {
                AudioSource.PlayClipAtPoint(jumpClip, transform.position);
                jump = true;
                animator.SetBool("isJumping", true);
            }
        }

        foreach (PlayerCheckpoint potentialSpawnpoint in Checkpoint)
        {
            if (potentialSpawnpoint.isActive)
            {
                if (potentialSpawnpoint.checkpointNumber > highestActiveCheckpoint)
                {
                    highestActiveCheckpoint = potentialSpawnpoint.checkpointNumber;
                    // respawnAtCheckpoint = highestActiveCheckpoint;
                    continue;
                }
            }
        }
    }

    public void OnLanding()
    {
        animator.SetBool("isJumping", false);
        jump = false;
    }

    void FixedUpdate()
    {
        controller.Move(horizontalMove * Time.fixedDeltaTime, false, jump);

        if (jump)
        {
            jumpFlag = true;
        }
    }

    public void warpTo(Vector2 position)
    {
        this.transform.position = position;
    }

    public void respawnAtCheckpoint()
    {
        this.transform.position = Checkpoint[highestActiveCheckpoint].checkpointLocation;
    }

    private IEnumerator SwitchBackToPlayerSprite()
    {
        yield return new WaitForSeconds(0.75f);
        animator.SetBool("isDead", false);
        this.GetComponent<SpriteRenderer>().sprite = playerSprite[0];
    }

    public void resetPlayer()
    {
        this.transform.position = Checkpoint[0].checkpointLocation;
        Checkpoint[0].isActive = true;
        Checkpoint[1].isActive = false;
        Checkpoint[2].isActive = false;
        Checkpoint[3].isActive = false;
        highestActiveCheckpoint = 0;
        this.hasEscaped = false;
        this.numberOfDeaths = 0;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemies" || collision.gameObject.tag == "EnemyBullets")
        {
            AudioSource.PlayClipAtPoint(hurtClip, transform.position);
            animator.SetBool("isDead", true);
            numberOfDeaths += 1;
            this.GetComponent<SpriteRenderer>().sprite = playerSprite[1];
            StartCoroutine(SwitchBackToPlayerSprite());
            respawnAtCheckpoint();
        }

        if (collision.gameObject.tag == "LevelExit")
        {
            AudioSource.PlayClipAtPoint(escapeClip, transform.position);
            hasEscaped = true;
            player.SetActive(false);
        }
    }
}
