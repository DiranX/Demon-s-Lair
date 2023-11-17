using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HellBuster : MonoBehaviour
{
    public static HellBuster instance;

    public Animator animator;
    public float radius;
    public Transform attackPoint;
    public LayerMask playerLayer;


    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if(GetComponent<EnemyController>().Health <= 0)
        {
            GetComponent<EnemyController>().Health = 0;
        }
        Bursting();
    }
    private void FixedUpdate()
    {

    }
    void Bursting()
    {
        Collider2D[] playerDetected = Physics2D.OverlapCircleAll(attackPoint.position, radius, playerLayer);
        foreach (Collider2D player in playerDetected)
        {
            animator.SetTrigger("Attack");
            GetComponent<EnemyController>().TakeDamage(1000);
            this.enabled = false;
        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(attackPoint.position, radius);
    }
}
