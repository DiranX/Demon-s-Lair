using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public static EnemyController instance;

    public float Health;
    public float walkSpeed;
    public float hookSpeed;
    public bool flip;
    public float detectRange;
    public GameObject detect;
    public Transform playerPos;
    public LayerMask playerlayer;
    public Animator anim;
    private AudioSource enemySound;
    public AudioClip hurt;
    public AudioClip die;
    public float volume;
    [SerializeField] private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }
    // Update is called once per frame
    void Update()
    {
        enemySound = GetComponent<AudioSource>();
        StartCoroutine(Flip());
        if (PlayerStatus.instance.isDie)
        {
            anim.enabled = false;
            this.enabled = false;
        }
        //Physics2D.IgnoreLayerCollision(7, 7);
        DetectPlayer();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Detect"))
        {
            GrapplingHook.instance.Target(gameObject);
            Vector2 direction = playerPos.position - transform.position;
            rb.velocity = new Vector2(direction.x * hookSpeed, direction.y * hookSpeed) * Time.deltaTime;
            Vector2 newPos = Vector2.MoveTowards(transform.position, GrapplingHook.instance.lr.transform.position, hookSpeed * Time.deltaTime);
            transform.position = newPos;
            GrapplingHook.instance.linePosition.transform.position = new Vector2(1000, 1000);
        }
        if (collision.gameObject.CompareTag("Bullet"))
        {
            TakeDamage(MeleeMechanic.instance.gunDamage);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Enemy"))
        {
            GrapplingHook.instance.enemyisGrappled = false;
        }
        if (collision.gameObject.CompareTag("Bullet"))
        {
            TakeDamage(MeleeMechanic.instance.gunDamage);
        }
    }
    void DetectPlayer()
    {
        Vector2 Direction = playerPos.position - transform.position;
        Collider2D[] enemyDetect = Physics2D.OverlapCircleAll(detect.transform.position, detectRange, playerlayer);
        RaycastHit2D DetectPlayer = Physics2D.Raycast(detect.transform.position, Vector2.left, detectRange, playerlayer);

        if(DetectPlayer.collider == null)
        {
            anim.SetFloat("Walk", Mathf.Abs(0));
        }

        foreach (Collider2D player in enemyDetect)
        {
            rb.AddForce(Direction * walkSpeed * Time.deltaTime);
            anim.SetFloat("Walk", Mathf.Abs(walkSpeed));
        }
    }
    private void OnDrawGizmosSelected()
    {
        if (detect == null) return;

        Gizmos.DrawWireSphere(detect.transform.position, detectRange);
    }

    IEnumerator Flip()
    {
        Vector2 scale = transform.localScale;
        if (playerPos.position.x > transform.position.x)
        {
            yield return new WaitForSeconds(.1f);
            scale.x = Mathf.Abs(scale.x) * (flip ? -1 : 1);
        }
        else
        {
            yield return new WaitForSeconds(.1f);
            scale.x = Mathf.Abs(scale.x) * -1 * (flip ? -1 : 1);

        }
        transform.localScale = scale;
    }

    public void TakeDamage(float damage)
    {
        Health -= damage;
        anim.SetTrigger("Hurt");

        if(Health <= 0)
        {
            Die();
            Health = 0;
        }
        enemySound.PlayOneShot(hurt, volume);
    }
    void Die()
    {
        anim.SetBool("isDead", true);
        enemySound.PlayOneShot(die, volume);
        SceneManagement.instance.KillCount(1);
        GrapplingHook.instance.enemyisGrappled = false;
        this.enabled = false;
        GetComponent<Collider2D>().enabled = false;
        rb.gravityScale = 1;
    }
}
