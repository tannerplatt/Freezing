using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlideController : MonoBehaviour
{
    public PlayerController playerController;
    public float glideSpeed;
    public float fallSpeed;

    private Rigidbody2D rigidbody;
    private bool canGlide = false;

    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (canGlide)
        {
            Glide();
        }
    }

    void Glide()
    {
        // Horizontal control
        float hInput = Input.GetAxis("Horizontal");
        Vector3 horizontalMovement = new Vector3(hInput * glideSpeed, 0, 0);
        rigidbody.velocity = new Vector3(horizontalMovement.x, fallSpeed, 0);

        // Reduced gravity
        rigidbody.gravityScale = 1f;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Ground")
        {
            GlideOff();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Glide"))
        {
            playerController.MovementOff();
            GlideOn();
        }
        if (other.CompareTag("Sling"))
        {
            GlideOff();
        }
    }

    public void GlideOff()
    {
        canGlide = false;
    }

    public void GlideOn()
    {
        canGlide = true;
    }
}
