using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public float timerMin = 15f;
    public float timerMax = 20f;

    private Vector3 Destination;
    // "Forward" for enemies is going to the left
    public bool Forwards = true;
    // public bool messages;
    private float TimePassed = 0f;

    public bool hasSeenPlayer;
    private const float TIME_UNTIL_SHOOTING_STOPS = 1.5f;
    private float playerIsGoneTime;
    public bool playerHasLeft;

    // Start is called before the first frame update
    void Start()
    {
        this.Destination = this.Waypoints[DestinationWaypoint].transform.position;

        timerBullet = 0;
        maxTimerBullet = Random.Range(timerMin, timerMax);
    }

    // Update is called once per frame
    void Update()
    {
        StopAllCoroutines();
        StartCoroutine(MoveTo());
        StartCoroutine(CheckForPlayer(raycastDistance));
    }

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
            }
        }

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

    IEnumerator StopShooting()
    {
        yield return new WaitForSeconds(TIME_UNTIL_SHOOTING_STOPS);
        playerHasLeft = true;
        StopCoroutine(FireBullet());
        Debug.Log("Player has left has been set to true.");
    }

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

    public void changePosition(Vector2 position)
    {
        this.transform.position = position;
    }

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

    IEnumerator FireBullet()
    {
        while (hasSeenPlayer == true)
        {
            if (timerBullet >= maxTimerBullet)
            {
                AudioSource.PlayClipAtPoint(shotSound, transform.position);
                SpawnBullet();
                timerBullet = 0f;
                maxTimerBullet = Random.Range(timerMin, timerMax);
            }

            if (playerHasLeft == true)
            {
                hasSeenPlayer = false;
                yield return new WaitForSeconds(2.0f);
            }
            timerBullet += Time.deltaTime;
            yield return null;
        }
    }

    public void resetEnemy()
    {
        this.transform.position = Waypoints[0].waypointPosition;
        DestinationWaypoint = 1;
        this.Forwards = true;
        this.Destination = this.Waypoints[DestinationWaypoint].transform.position;

        timerBullet = 0;
        maxTimerBullet = Random.Range(timerMin, timerMax);
        hasBeenAlerted = false;
        hasSeenPlayer = false;

    }
}
