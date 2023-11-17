using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapplingHook : MonoBehaviour
{
    public static GrapplingHook instance;
    [Header("Rope Mechanic")]
    public GameObject linePosition;
    GameObject target;

    public float grapplingSpeed = 10f;
    public float maxDistance = 20f;

    public LayerMask grappleableMask;
    public LayerMask enemyMask;

    private SpringJoint2D sj;
    private Rigidbody2D rb;
    public LineRenderer lineRenderer;
    public LineRenderer lr;
    //private DistanceJoint2D Dj;

    private Vector2 grapplingHookPos;

    public bool isGrappling = false;
    public bool enemyisGrappled = false;

    void Start()
    {
        instance = this;
        rb = GetComponent<Rigidbody2D>();
        //Dj = GetComponent<DistanceJoint2D>();
        //Dj.enabled = false;
        sj = GetComponent<SpringJoint2D>();
        sj.enabled = false;
        lineRenderer.enabled = false;
        lr.enabled = false;

        lr.SetPosition(1, lr.transform.position);
    }
    void Update()
    {
        Graplling();
    }
    private void Graplling()
    {
        if (Input.GetMouseButtonDown(2))
        {
            // Get the position of the mouse click
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            // Cast a ray from the mouse position to check for grappleable objects
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero, Mathf.Infinity, grappleableMask);
            RaycastHit2D pull = Physics2D.Raycast(mousePos, Vector2.zero, Mathf.Infinity, enemyMask);
            linePosition.transform.position = mousePos;

            //StartCoroutine(AnimationRope(target.position));

            if (hit.collider != null)
            {
                // Grapple to the object
                grapplingHookPos = hit.point;
                sj.connectedAnchor = mousePos;
                sj.enabled = true;
                Debug.Log("Grapple!");
                //detect.SetActive(true);
                //Dj.connectedAnchor = mousePos;
                //Dj.enabled = true;
                isGrappling = true;
            }
            if (pull.collider != null)
            {
                // Grapple to the object
                //detect.SetActive(true);
                //Dj.connectedAnchor = mousePos;
                //Dj.enabled = true;
                enemyisGrappled = true;
                Debug.Log("Enemy is grappled");
            }
        }
        GrapplePullPlayer();
        GrapplePullEnemy();
        
    }

    public void GrapplePullEnemy()
    {
        if (enemyisGrappled == true)
        {
            if(target != null)
            {
                lr.SetPosition(0, lr.transform.position);
                lr.SetPosition(1, target.transform.position);
            }
            // Update the line renderer to show the grapple line

            // Move the enemy towards the player position

            // Check if the player is too far away from the grappling hook position
            float distance = Vector2.Distance(transform.position, target.transform.position);
            if (distance > maxDistance)
            {
                enemyisGrappled = false;
            }

        }
        else
        {
            //detect.SetActive(false);
            // Disable the line renderer when not grappling
            target = null;
            lr.enabled = false;
            sj.enabled = false;
            //Dj.enabled = false;
        }
    }

    void GrapplePullPlayer()
    {
        if (isGrappling == true)
        {
            // Update the line renderer to show the grapple line
            lineRenderer.enabled = true;
            lineRenderer.SetPosition(0, lineRenderer.transform.position);
            lineRenderer.SetPosition(1, grapplingHookPos);

            // Move the player towards the grappling hook position
            Vector2 direction = grapplingHookPos - (Vector2)transform.position;
            rb.velocity = new Vector2(direction.x * grapplingSpeed, direction.y * grapplingSpeed) * Time.deltaTime;
            rb.gravityScale = 0f;
            Vector2 newPos = Vector2.MoveTowards(transform.position, grapplingHookPos, grapplingSpeed * Time.deltaTime);
            transform.position = newPos;

            // Check if the player is too far away from the grappling hook position
            float distance = Vector2.Distance(transform.position, grapplingHookPos);
            if (distance > maxDistance)
            {
                isGrappling = false;
            }
        }
        else
        {
            rb.gravityScale = 4;
            //detect.SetActive(false);
            // Disable the line renderer when not grappling
            lineRenderer.enabled = false;
            sj.enabled = false;
            //Dj.enabled = false;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Detect"))
        {
            Debug.Log("Detect!!");
            //lineRenderer.enabled = false;
            isGrappling = false;
            linePosition.transform.position = new Vector2(1000, 1000);
        }
    }
    public void Target(GameObject targetPos)
    {
        target = targetPos;
        lr.enabled = true;
        sj.connectedBody = target.GetComponent<Rigidbody2D>();
    }
}
