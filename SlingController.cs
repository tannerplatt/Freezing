using System.Collections;
using UnityEngine;

public class SlingController : MonoBehaviour
{
    public float power;
    public Vector2 minPower;
    public Vector2 maxPower;
    public Camera cam;
    public PlayerController playerController;

    private Vector2 force;
    //private Vector3 slingPoint;
    private Vector3 endPoint;
    private Vector3 slingPoint;
    private Vector3 storedVelocity;
    private Rigidbody2D rigidbody;
    private bool canSling = false;

    private TrajectoryLine trajectoryLine;

    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        trajectoryLine = GetComponent<TrajectoryLine>();
    }

    void Update()
    {
        if (canSling)
        {
            if (Input.GetMouseButtonDown(0))
            {
               // slingPoint = cam.ScreenToWorldPoint(Input.mousePosition);
                //slingPoint.z = 15f;
            }

            if (Input.GetMouseButton(0))
            {
                Vector3 currentPoint = cam.ScreenToWorldPoint(Input.mousePosition);
                currentPoint.z = 15f;
                trajectoryLine.RenderLine(slingPoint, currentPoint);
            }

            if (Input.GetMouseButtonUp(0))
            {
                endPoint = cam.ScreenToWorldPoint(Input.mousePosition);
                endPoint.z = 15f;

                force = new Vector2(Mathf.Clamp(slingPoint.x - endPoint.x, minPower.x, maxPower.x), Mathf.Clamp(slingPoint.y - endPoint.y, minPower.y, maxPower.y));

                rigidbody.AddForce(force * power, ForceMode2D.Impulse);
                trajectoryLine.EndLine();
                OnLaunch();
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Sling"))
        {
            playerController.MovementOff();
            slingPoint = other.transform.position;
            SlingOn();
        }
    }

    public void SlingOff()
    {
        canSling = false;
    }

    public void SlingOn()
    {
        canSling = true;
    }

    public void OnLaunch()
    {
        SlingOff();
        rigidbody.gravityScale = playerController.GetStoredGravity();
        //StartCoroutine(DelayMovement());
    }
    
    IEnumerator DelayMovement()
    {
        storedVelocity = rigidbody.velocity;
        yield return new WaitForSeconds(2f);
        playerController.MovementOn();
    }
}
