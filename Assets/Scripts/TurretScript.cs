using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretScript : MonoBehaviour
{
    [SerializeField] Transform castPoint;
    [SerializeField] SpriteRenderer sr;
    [SerializeField] GameObject player;
    [SerializeField] GameObject laserPrefab;
    [SerializeField] ParticleSystem ps;
    [SerializeField] StateManager sm;

    [SerializeField] int health;
    [SerializeField] float fireRate;
    [SerializeField] float viewDistance;
    [SerializeField] bool canFire;
    [SerializeField] float laserSpeed;
    [SerializeField] bool dead;

    [SerializeField] Sprite HealthyIdle;
    [SerializeField] Sprite HealthyFire;
    [SerializeField] Sprite HealthyHurt;
    [SerializeField] Sprite DamagedIdle;
    [SerializeField] Sprite DamagedFire;
    [SerializeField] Sprite DamagedHurt;

    void Start()
    {
        sr.sprite = HealthyIdle;
        health = 100;
        viewDistance = 5f;
        canFire = true;
        laserSpeed = 5f;
        dead = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerVisible(viewDistance) && canFire && !dead)
        {
            Shoot(laserPrefab);
        }

        if (health <= 0)
            StartCoroutine(Die());
    }

    bool PlayerVisible(float distance)
    {
        bool visible = false;

        RaycastHit2D hit = Physics2D.Linecast(castPoint.position, player.transform.position);

        if (hit.transform.gameObject.layer == 8 && hit.distance < distance) 
        {
            visible = true;
        }

        Debug.DrawLine(castPoint.position, player.transform.position, Color.blue);

        return visible;
    }

    void Shoot(GameObject laserPrefab)
    {
        GameObject laser = Instantiate(laserPrefab, castPoint.position, Quaternion.identity);
        Vector2 direction = (player.transform.position - castPoint.position); // Vector subtraction creates Vector2 between points
        direction.Normalize();
        float aimAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        StartCoroutine(FireSprite());

        laser.GetComponent<Rigidbody2D>().velocity = direction * laserSpeed;
        laser.transform.Rotate(0f, 0f, aimAngle);
        Destroy(laser, 2.0f);

        StartCoroutine(Recharge());
    }

    IEnumerator Recharge()
    {
        canFire = false;
        yield return new WaitForSeconds(1.2f);
        canFire = true;
    }

    IEnumerator FireSprite()
    {
        if (health >= 40)
        {
            sr.sprite = HealthyFire;
            yield return new WaitForSeconds(0.2f);
            sr.sprite = HealthyIdle;
        }
        else
        {
            sr.sprite = DamagedFire;
            yield return new WaitForSeconds(0.2f);
            sr.sprite = DamagedIdle;
        }
    }

    IEnumerator TakeDamage(int damage)
    {
        health -= damage;

        if (health > 0)
        {
            if (health >= 40)
                sr.sprite = HealthyHurt;
            else
                sr.sprite = DamagedHurt;

            Vector2 originalPosition = transform.position;
            for (int i = 0; i < 2; i++)
            {
                transform.position = new Vector2(originalPosition.x + 0.02f, originalPosition.y);
                yield return new WaitForSeconds(0.1f);
                transform.position = new Vector2(originalPosition.x - 0.02f, originalPosition.y);
                yield return new WaitForSeconds(0.1f);
            }
            transform.position = originalPosition;

            if (health >= 40)
                sr.sprite = HealthyIdle;
            else
                sr.sprite = DamagedIdle;
        }
    }

    IEnumerator Die()
    {
        dead = true;
        ps.Play();
        sr.enabled = false;
        yield return new WaitForSeconds(1);
        sm.SendMessage("EnemyDie");
        Destroy(gameObject);
    }
}
