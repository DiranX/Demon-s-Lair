using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeMechanic : MonoBehaviour
{
    public static MeleeMechanic instance;

    public Animator animator;
    public Transform attackPoint;
    public float attackRange;
    public bool isAttacking = false;

    public float bulletForce;
    private int ammoAmmount;
    public bool isShoot;
    public Transform shootingPoint;
    public GameObject bulletPrefab;
    public GameObject[] ammo;

    public float swordDamage;
    public AudioClip slash1;
    public AudioClip slash2;
    public AudioClip slash3;
    public float gunDamage;
    public AudioClip gunShoot;

    public AudioSource playerAudio;
    private Animator anim;

    public LayerMask enemylayer;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        anim = GetComponent<Animator>();
        for (int i = 0; i <= 5; i++)
        {
            ammo[i].gameObject.SetActive(true); ;
        }
        ammoAmmount = 6;
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerStatus.instance.playerStamina > 15)
        {
            AttackInput();
        }
        if (PlayerStatus.instance.playerStamina > 10 && ammoAmmount > 0)
        {
            ShootInput();
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            ammoAmmount = 6;
            for (int i = 0; i <=  5; i++)
            {
                ammo[i].gameObject.SetActive(true);
            }
        }
    }

    public void ShootInput()
    {
        if (Input.GetMouseButtonDown(1) && !isShoot)
        {
            PlayerStatus.instance.StaminaBar(5);
            isShoot = true;
            GameObject bullet = Instantiate(bulletPrefab, shootingPoint.position, shootingPoint.rotation);
            bullet.GetComponent<Rigidbody2D>().velocity = new Vector2(transform.localScale.x, 0).normalized * bulletForce;
            playerAudio.PlayOneShot(gunShoot, PlayerMove.instance.volume);
            ammoAmmount -= 1;
            ammo[ammoAmmount].gameObject.SetActive(false);
        }
    }

    public void AttackInput()
    {
        if (Input.GetMouseButtonDown(0) && !isAttacking)
        {
            PlayerStatus.instance.StaminaBar(15);
            isAttacking = true;
            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemylayer);

            foreach (Collider2D enemy in hitEnemies)
            {
                enemy.GetComponent<EnemyController>().TakeDamage(swordDamage);
            }
        }
    }
    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
        {
            return;
        }

        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}

