using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class TankMovement : MonoBehaviour
{
    public KeyCode KeyUp;
    public KeyCode KeyDown;
    public KeyCode KeyLeft;
    public KeyCode KeyRight;
    public KeyCode KeyFire;

    public GameObject bulletPrefab;

    public float movementSpeed = 0;
    public float reverseMovementSpeed = 0;
    public float rotationSpeed = 0;
    public float bulletForce = 0;
    public int timerFire = 0;
    public bool isDead = false;
    public bool enabledMovement = true;

    private int direction_mov = 0, direction_rot = 0;
    private bool canFire = true;
    private int _timerFire = 0;

    private int pickup = 0;

    Rigidbody2D rb2d;
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (enabledMovement)
        {
            if (!canFire && _timerFire++ >= timerFire) canFire = true;
            GetPlayerInput();
            RotatePlayer();
            MovePlayer();
        }
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.tag == "Pickup")
        {
            pickup = 1;
            Destroy(coll.gameObject);
        }
    }

    private void GetPlayerInput()
    {
        if(Input.GetKey(KeyFire) && canFire)
        {
            canFire = false;
            _timerFire = 0;

            if (pickup == 0)
            {
                GameObject bullet = Instantiate(bulletPrefab, transform.position + transform.forward + transform.up * 0.6f, transform.rotation);
                bullet.GetComponent<Rigidbody2D>().AddForce(transform.up * bulletForce * Time.deltaTime);
            }
            else if(pickup == 1)
            {
                GameObject bullet = Instantiate(bulletPrefab, transform.position + transform.forward + transform.up * -0.6f, transform.rotation);
            }
            pickup = 0;
        }

        if (Input.GetKey(KeyUp)) direction_mov = 1;
        else if (Input.GetKey(KeyDown)) direction_mov = -1;
        else direction_mov = 0;

        if (Input.GetKey(KeyLeft)) direction_rot = 1;
        else if (Input.GetKey(KeyRight)) direction_rot = -1;
        else direction_rot = 0;

    }

    private void RotatePlayer()
    {
        transform.Rotate(direction_rot * Vector3.forward * rotationSpeed * Time.deltaTime);
    }

    private void MovePlayer()
    {
        rb2d.velocity = transform.up * (direction_mov == -1 ? reverseMovementSpeed : movementSpeed) * direction_mov * Time.deltaTime;
    }
}
