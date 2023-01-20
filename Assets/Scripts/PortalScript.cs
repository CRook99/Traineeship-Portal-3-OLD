using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PortalScript : MonoBehaviour
{
    [SerializeField] string nextScene;
    [SerializeField] float spinSpeed;
    public bool active;
    public Collider2D collider;

    public SpriteRenderer sr;
    private Vector3 nullScale = new Vector3(0f, 0f, 0f);
    private Vector3 worldScale = new Vector3(2f, 2f, 2f);
    private Vector3 transitionScale = new Vector3(100f, 100f, 100f);
    [SerializeField] Vector3 exitPosition;
    
    void Start()
    {
        spinSpeed = 1f;
        active = false;
        sr.enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0f, 0f, spinSpeed * -1);

        if (Input.GetKeyDown("space"))
        {
            Appear();
        }

        if (Input.GetKeyDown("return"))
        {
            StartCoroutine(EnterPortal());
        }

        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            StartCoroutine(ExitPortal());
        }
    }

    void Appear()
    {
        active = true;
        sr.enabled = true;
        transform.localScale = new Vector3(0f, 0f, 0f);
        StartCoroutine(ScaleOverTime(1f, worldScale));
    }

    IEnumerator EnterPortal()
    {
        if (active)
        {
            StartCoroutine(ScaleOverTime(2f, transitionScale));
            yield return new WaitForSeconds(2f);
            SceneManager.LoadScene(nextScene);
        }
    }

    IEnumerator ExitPortal()
    {
        active = false;
        collider.enabled = false;
        transform.localScale = transitionScale;
        StartCoroutine(ScaleOverTime(2f, nullScale));
        yield return new WaitForSeconds(2f);
        sr.enabled = false;
        transform.position = exitPosition;
        collider.enabled = true;
    }

    IEnumerator ScaleOverTime(float duration, Vector3 targetScale)
    {
        Vector3 original = transform.localScale;
        Vector3 target = targetScale;

        float time = 0.0f;

        while (time <= duration)
        {
            transform.localScale = Vector3.Lerp(original, target, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
    }
}
