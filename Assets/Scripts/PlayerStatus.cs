using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatus : MonoBehaviour
{
    public static PlayerStatus instance;
    public float playerHealth;
    public float playerStamina;
    public float staminaChargeRate;
    public float staminaCoolDown;
    public bool isDie;
    public Image staminaUi;
    public Image HealthaUi;
    public GameObject collider;
    public GameObject gameOverUi;
    public AudioClip hurt;
    public AudioClip die;
    public AudioClip bgm;
    public AudioSource playerAudio;
    public float volume;

    [SerializeField] private float IFrameDuration;
    [SerializeField] private int flashNumber;
    [SerializeField] SpriteRenderer sprite;
    [SerializeField] private Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        playerAudio.PlayOneShot(bgm, volume);
    }

    // Update is called once per frame
    void Update()
    {
        HealthaUi.fillAmount = playerHealth / 250;
        Die();
        StartCoroutine(StaminaCharge());
        staminaUi.fillAmount = playerStamina / 100;
        if (playerStamina < 0) playerStamina = -0;

    }

    public void HealthBar(float damageToHp)
    {
        playerHealth -= damageToHp;
        StartCoroutine(AfterDamageIFrame());
        playerAudio.PlayOneShot(hurt, PlayerMove.instance.volume);
        anim.SetTrigger("Hurt");
    }
    public void StaminaBar(float StaminaCost)
    {
        playerStamina -= StaminaCost;
    }
    public void Die()
    {
        if(playerHealth <= 0)
        {
            isDie = true;
            playerAudio.PlayOneShot(die, PlayerMove.instance.volume);
            anim.SetBool("isDead", true);
            this.enabled = false;
            gameObject.GetComponent<PlayerMove>().enabled = false;
            gameObject.GetComponent<GrapplingHook>().enabled = false;
            gameObject.GetComponent<MeleeMechanic>().enabled = false;
            gameObject.GetComponent<BoxCollider2D>().enabled = false;
            collider.GetComponent<BoxCollider2D>().enabled = true;
            gameOverUi.SetActive(true);
            playerAudio.Stop();
            Debug.Log("Game Over");
        }
    }
    private IEnumerator StaminaCharge()
    {
        while(playerStamina < 100)
        {
            yield return new WaitForSeconds(staminaCoolDown);
            playerStamina += staminaChargeRate * 0.8f * Time.deltaTime;
            if(playerStamina > 100)
            {
                playerStamina = 100;
            }
        }
    }

    IEnumerator AfterDamageIFrame()
    {
        Physics2D.IgnoreLayerCollision(3, 7, true);
        for (int i = 0; i < flashNumber; i++)
        {
            sprite.color = new Color(1,0,0,0.5f);
            yield return new WaitForSeconds(IFrameDuration / (flashNumber*2));
            sprite.color = Color.white;
            yield return new WaitForSeconds(IFrameDuration / (flashNumber * 2));
        }
        Physics2D.IgnoreLayerCollision(3, 7, false);
    }
}
