using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Demon : MonoBehaviour
{
    public static Demon instance;

    public float attackRange;
    public Animator anim;
    public Transform attackPoint;
    public Image healthbar;
    public GameObject Skull;
    public GameObject winText;

    public LayerMask playerMask;

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
            winText.SetActive(true);
            PlayerStatus.instance.playerAudio.Stop();
            Destroy(Skull);
        }
        healthbar.fillAmount = GetComponent<EnemyController>().Health / 500;
        if(GetComponent<EnemyController>().Health <= 150)
        {
            Skull.SetActive(true);
        }
        StartCoroutine(Attacking());
    }

    IEnumerator Attacking()
    {
            yield return new WaitForSeconds(1);
        Collider2D[] detectPlayer = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, playerMask);
        foreach (Collider2D item in detectPlayer)
        {
            anim.SetTrigger("Attack");
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
