using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBulletScript : MonoBehaviour
{
    public float speed;
    public EnemyController enemy;
    private Rigidbody2D rb;
    public bool forwardBullet;

    private float bulletFlyTime;
    private const float BULLET_AIR_TIME = 6f;
    // Start is called before the first frame update
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
    void Update()
    {
        StopAllCoroutines();
        StartCoroutine(destroyLater());

        if (bulletFlyTime >= BULLET_AIR_TIME)
        {
            Destroy(this.gameObject);
        }
    }

    IEnumerator destroyLater()
    {
        while (bulletFlyTime <= BULLET_AIR_TIME)
        {
            bulletFlyTime += Time.deltaTime;
            yield return null;
        }
    }

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
