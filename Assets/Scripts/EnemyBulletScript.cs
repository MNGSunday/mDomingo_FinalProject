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

// The following class represents the concept of a bullet fired by an enemy in a 2D platformer game and contains functions to simulate a bullet hitting an object.
public class EnemyBulletScript : MonoBehaviour
{
    public float speed;
    public EnemyController enemy;
    private Rigidbody2D rb;
    public bool forwardBullet;

    private float bulletFlyTime;
    private const float BULLET_AIR_TIME = 6f;
    // Start is called before the first frame update
    // Ensures that when the bullet is spawned, it is facing and travels in the correct direction.
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (forwardBullet)
        {
            rb.velocity = new Vector2(-speed, 0);
        }

        else
        {
            rb.velocity = new Vector2(speed, 0);
            this.transform.localScale = new Vector2(-1, 1);
        }
        
        bulletFlyTime = 0f;
    }

    // Update is called once per frame
    // Ensures that a bullet is destroyed if it remains in the air for 6 seconds without hitting a player or the environment.
    void Update()
    {
        StopAllCoroutines();
        StartCoroutine(destroyLater());

        if (bulletFlyTime >= BULLET_AIR_TIME)
        {
            Destroy(this.gameObject);
        }
    }

    // A timer used to determine when a bullet should be destroyed.
    IEnumerator destroyLater()
    {
        while (bulletFlyTime <= BULLET_AIR_TIME)
        {
            bulletFlyTime += Time.deltaTime;
            yield return null;
        }
    }

    // If the bullet collides with something, destroy it.
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 12)
        {
            Destroy(this.gameObject);
        }

        if (collision.gameObject.tag == "Player")
        {
            Destroy(this.gameObject);
        }
    }
}
