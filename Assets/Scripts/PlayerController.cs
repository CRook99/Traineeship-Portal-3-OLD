using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public Animator animator;
    public Rigidbody2D rb;
    public SpriteRenderer sr;
    public ParticleSystem ps;
    public HealthBar hb;

    public Vector2 aimDirection;

    public int maxHealth = 100;
    public int health;
    private Color normal;
    private Color damage;
    [SerializeField] bool dead;

    private void OnEnable()
    {
        health = PlayerPrefs.GetInt("Health");
    }
    private void OnDisable()
    {
        PlayerPrefs.SetInt("Health", health);
    }
    void Start()
    {
        if (SceneManager.GetActiveScene().name == "Level1")
            health = maxHealth;

        hb.SendMessage("SetHealth", health);
        normal = new Color(1, 1, 1, 1);
        damage = new Color(1, 0.5f, 0.5f, 1);
        dead = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!dead)
            MovePlayer();

        if (health <= 0)
            StartCoroutine(Die());
    }

    void MovePlayer()
    {
        Vector3 movement = new Vector3(Input.GetAxis("JoyHorizontal"), Input.GetAxis("JoyVertical"), 0.0f);

        aimDirection = new Vector2(Input.GetAxis("AimHorizontal"), Input.GetAxis("AimVertical"));

        rb.velocity = new Vector2(movement.x, movement.y);

        animator.SetFloat("MoveMagnitude", movement.magnitude);
        if (movement.magnitude > 0.01f)
        {
            animator.SetFloat("MoveHor", movement.x);
            animator.SetFloat("MoveVer", movement.y);
        }

        animator.SetFloat("AimHor", aimDirection.x);
        animator.SetFloat("AimVer", aimDirection.y);
        animator.SetFloat("AimMagnitude", aimDirection.magnitude);
    }

    IEnumerator TakeDamage(int damage)
    {
        health -= damage;
        hb.SendMessage("SetHealth", health);

        sr.color = this.damage;
        yield return new WaitForSeconds(0.1f);
        sr.color = normal;
    }

    IEnumerator Die()
    {
        rb.velocity = new Vector2(0f, 0f);
        dead = true;
        ps.Play();
        sr.enabled = false;
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene("MainMenu");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 13)
        {
            Heal(50);
            collision.gameObject.SendMessage("EnterPortal");
        }
    }

    void Heal(int healAmount)
    {
        health += healAmount;
    }
}
