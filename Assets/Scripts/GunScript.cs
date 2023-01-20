using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunScript : MonoBehaviour
{
    [SerializeField] GameObject crosshair;
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] Transform camTarget;
    [SerializeField] Transform bulletOrigin;

    [SerializeField] float recoilTime;
    [SerializeField] bool canFire = true;
    private Vector2 direction;
    private float aimAngle;
    private int numDirection;
    private Vector3[] origins = new Vector3[] { new Vector3(0.18f, -0.035f, 0f), new Vector3(0.075f, 0.05f, 0f), new Vector3(-0.18f, -0.035f, 0f), new Vector3(-0.075f, -0.1f, 0f) }; // RIGHT - UP - LEFT - DOWN

    private void Start()
    {
        recoilTime = 0.3f;
    }

    // Update is called once per frame
    void Update()
    {
        direction = new Vector2(Input.GetAxis("AimHorizontal"), Input.GetAxis("AimVertical"));
        direction.Normalize();
        aimAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        MoveCrosshair(direction);
        UpdateNumDirection(aimAngle);

        if (Input.GetButtonDown("Fire") && crosshair.activeSelf == true && canFire)
        {
            Fire(direction, aimAngle);
        }
    }

    void Fire(Vector2 direction, float aimAngle)
    {
        GameObject bullet = Instantiate(bulletPrefab, bulletOrigin.position, Quaternion.identity);

        bullet.GetComponent<Rigidbody2D>().velocity = direction * 8;
        bullet.transform.Rotate(0.0f, 0.0f, aimAngle);
        StartCoroutine(Recoil());
        Destroy(bullet, 2.0f);
    }

    IEnumerator Recoil()
    {
        canFire = false;
        yield return new WaitForSeconds(recoilTime);
        canFire = true;
    }

    void MoveCrosshair(Vector2 direction)
    {
        Vector3 aim = direction;

        if (aim.magnitude > 0.1f)
        {
            aim.Normalize();
            crosshair.transform.localPosition = aim * 0.7f;
            crosshair.SetActive(true);

            camTarget.localPosition = aim * 1f;
        }
        else
        {
            crosshair.SetActive(false);
            camTarget.localPosition = new Vector3(0f, 0f, 0f);
        }
    }

    void UpdateNumDirection(float angle)
    {
        // RIGHT = 0 ; UP = 1 ; LEFT = 2 ; DOWN = 3

        if (angle < 0)
            angle += 360f;

        angle += 45f;

        numDirection = Mathf.CeilToInt(angle / 90) - 1;
        if (numDirection == 4)
            numDirection = 0;

        bulletOrigin.localPosition = origins[numDirection];
    }
}
