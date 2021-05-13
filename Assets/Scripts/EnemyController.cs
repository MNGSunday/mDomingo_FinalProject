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


// The following class represents the concept of an enemy in a 2D Platformer game and contains functionality to simulate enemy behavior in response to seeing the player.
public class EnemyController : MonoBehaviour
{
    public List<EnemyWaypoint> Waypoints = new List<EnemyWaypoint>();
    public float speed = 1.0f;
    public int DestinationWaypoint = 1;

    public Transform raycastOrigin;
    public float raycastDistance;

    public AudioClip alertSound;
    public AudioClip shotSound;

    public Rigidbody2D enemyRb;

    public bool hasBeenAlerted = false;
    private bool isShooting = false;
    private float timerBullet;
    private float maxTimerBullet;
    public GameObject bullet;
    public EnemyBulletScript bulletScript;

    public float timerMin = 0.25f;
    public float timerMax = 0.5f;

    private Vector3 Destination;
    // "Forward" for enemies is going to the left
    public bool Forwards = true;
    // public bool messages;
    private float TimePassed = 0f;

    public bool hasSeenPlayer;
    private const float TIME_UNTIL_SHOOTING_STOPS = 2f;
    private float playerIsGoneTime;
    public bool playerHasLeft;

    // Start is called before the first frame update
    void Start()
    {
        this.Destination = this.Waypoints[DestinationWaypoint].transform.position;

        timerBullet = 0;
        // Determine a random time between timerMin and timerMax (adjusted by player) to determine how long it takes for the enemy to fire a bullet
        maxTimerBullet = Random.Range(timerMin, timerMax);
        // Change the enemy's sprite to white as it has not been alerted of the player's presence.
        this.GetComponent<SpriteRenderer>().color = Color.white;
    }

    // Update is called once per frame
    void Update()
    {
        StopAllCoroutines();
        StartCoroutine(MoveTo());
        StartCoroutine(CheckForPlayer(raycastDistance));
    }

    // Creates a raycast that constantly checks for the player. If the raycast spots the player, the enemy can shoot as long as the player remains in the raycast's range.
    IEnumerator CheckForPlayer(float distance)
    {
        if (Forwards == true)
        {
            distance = -distance;
        }

        Vector2 raycastEnd = raycastOrigin.position + Vector3.right * distance;

        RaycastHit2D hit = Physics2D.Linecast(raycastOrigin.position , raycastEnd, 1 << LayerMask.NameToLayer("Action"));

        Debug.DrawLine(raycastOrigin.position, raycastEnd, Color.green);

        if (hit.collider != null)
        {
            // Debug.Log("Raycast hit!");
            // If the raycast finds the player, set the enemy to an "alerted state." The enemy can shoot.
            if (hit.collider.CompareTag("Player"))
            {
                if (!hasBeenAlerted)
                {
                    AudioSource.PlayClipAtPoint(alertSound, transform.position);
                    hasBeenAlerted = true;
                }
                isShooting = true;
                hasSeenPlayer = true;
                playerHasLeft = false;
                this.GetComponent<SpriteRenderer>().color = Color.red;
            }
        }

        // If the player is not in the enemy's sights, do not shoot. If the enemy had been previously shooting, stop the enemy from shooting.
        else if (hit.collider == null /* && messages == true */)
        {
            // Debug.Log("Raycast did not hit.");
            if (isShooting)
            {
                playerHasLeft = true;
                isShooting = false;
                yield return new WaitForSeconds(TIME_UNTIL_SHOOTING_STOPS);
            }
        }

        yield return null;
    }

    // If the enemy spots the player, stop moving and shoot the player. Otherwise, travel to each of the enemy's waypoints, stop if the waypoint is a sentry point, and if the waypoint is an endpoint, turn around.
    IEnumerator MoveTo()
    {
        if (hasSeenPlayer == true)
        {
            StopAllCoroutines();
            isShooting = true;
            StartCoroutine(FireBullet());
        }

        else
        {
            while ((transform.position - this.Destination).sqrMagnitude > 0.01f)
            {
                transform.position = Vector2.MoveTowards(transform.position,
                    this.Destination, this.speed * Time.deltaTime);
                yield return null;
            }

            if ((transform.position - this.Destination).sqrMagnitude < 0.01f)
            {
                if (this.Waypoints[DestinationWaypoint].IsSentry)
                {
                    while (this.TimePassed < this.Waypoints[DestinationWaypoint].PauseTime)
                    {
                        this.TimePassed += Time.deltaTime;
                        yield return null;
                    }

                    this.TimePassed = 0f;
                }
            }
            GetNextWaypoint();
        }
    }

    // Determines the next waypoint the enemy travels to, and if the waypoint is an endpoint, turns the enemy around.
    void GetNextWaypoint()
    {
        if (this.Waypoints[DestinationWaypoint].IsEndpoint)
        {
            if (this.Forwards)
            {
                this.Forwards = false;
                this.transform.localScale = new Vector2(-1, 1);
            }


            else
            {
                this.Forwards = true;
                this.transform.localScale = new Vector2(1, 1);
            }
        }

        if (this.Forwards)
            ++DestinationWaypoint;
        else
            --DestinationWaypoint;

        if (DestinationWaypoint >= this.Waypoints.Count)
            DestinationWaypoint = 0;

        this.Destination = this.Waypoints[DestinationWaypoint].transform.position;
    }

    // Simulates where the enemy shoots a bullet.
    void SpawnBullet()
    {
        if (this.Forwards)
        {
            Vector3 spawnPoint = transform.position;
            spawnPoint.x -= (bullet.GetComponent<Renderer>().bounds.size.x / 2) + (GetComponent<Renderer>().bounds.size.x / 2);
            bulletScript.forwardBullet = true;
            GameObject.Instantiate(bullet, spawnPoint, transform.rotation);
        }

        else
        {
            Vector3 spawnPoint = transform.position;
            spawnPoint.x += (bullet.GetComponent<Renderer>().bounds.size.x / 2) + (GetComponent<Renderer>().bounds.size.x / 2);
            bulletScript.forwardBullet = false;
            GameObject.Instantiate(bullet, spawnPoint, transform.rotation);

        }
    }

    // Simulates an enemy firing a bullet.
    IEnumerator FireBullet()
    {
        while (hasSeenPlayer == true)
        {
            if (timerBullet >= maxTimerBullet && !playerHasLeft)
            {
                AudioSource.PlayClipAtPoint(shotSound, transform.position);
                SpawnBullet();
                timerBullet = 0f;
                maxTimerBullet = Random.Range(timerMin, timerMax);
            }

            // If the player left the enemy raycast's sight, wait 2 seconds before allowing the enemy to move again.
            if (playerHasLeft == true)
            {
                while (timerBullet <= TIME_UNTIL_SHOOTING_STOPS)
                {
                    timerBullet += Time.deltaTime;
                    yield return null;
                }
                hasSeenPlayer = false;
                yield break;
            }
            timerBullet += Time.deltaTime;
            yield return null;
        }
    }

    // Resets the enemy's status, position, and destination.
    public void resetEnemy()
    {
        this.transform.position = Waypoints[0].waypointPosition;
        DestinationWaypoint = 1;
        this.Forwards = true;
        this.Destination = this.Waypoints[DestinationWaypoint].transform.position;
        this.GetComponent<SpriteRenderer>().color = Color.white;

        timerBullet = 0;
        maxTimerBullet = Random.Range(timerMin, timerMax);
        hasBeenAlerted = false;
        hasSeenPlayer = false;

    }
}
