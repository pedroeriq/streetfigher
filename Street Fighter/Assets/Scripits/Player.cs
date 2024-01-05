using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public float speed;
    public float jumpForce;
    public float forwardJumpForce;
    public float lateralForce;
    public float speedHadouken = 11;
    public AudioSource Soucer;
    public AudioClip[] Clip;

    public GameObject hadouken;
    public Transform pontoDeTiro;
    public Player2 Player1;

    public int maxHealth = 100;
    public int currentHealth;

    public Slider healthSlider; // Referência para o Slider da barra de vida

    private bool forwardJump;
    private bool isJumping;
    private bool isAttacking;
    private bool applyingLateralForce;
    private Animator anim;
    private Rigidbody2D rig;
    private bool canUseHadouken = true;
    private bool isDefending = false;

    void Start()
    {
        anim = GetComponent<Animator>();
        rig = GetComponent<Rigidbody2D>();
        Player1 = FindObjectOfType<Player2>();
        Soucer = GetComponent<AudioSource>();

        currentHealth = maxHealth;

        // Encontre e atribua automaticamente o Slider se ele não estiver atribuído
        if (healthSlider == null)
        {
            healthSlider = GameObject.FindObjectOfType<Slider>();
        }

        // Define o valor máximo do Slider como a vida máxima do jogador
        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHealth;
            healthSlider.value = currentHealth; // Atualiza o valor inicial do Slider
        }
    }

    void Update()
    {
        Move();
        Jump();
        ForwardJump();

        if (isAttacking == false)
        {
            StartCoroutine("Atacar");
        }

        LookAtPlayer();
        if (Input.GetKeyDown(KeyCode.E))
        {
            isDefending = true;
            anim.SetInteger("transition", 12); // Entra na animação de defesa
        }
        else if (Input.GetKeyUp(KeyCode.E))
        {
            isDefending = false;
            anim.SetInteger("transition", 0); // Retorna à animação normal
        }

        if (!isDefending && currentHealth <= 0)
        {
            Play(4);
            anim.SetInteger("transition", 11);

            // Checa a direção para onde o jogador está olhando
            if (transform.eulerAngles.y == 0)
            {
                rig.AddForce(new Vector2(-5f, 0f),
                    ForceMode2D.Impulse); // Olhando para a direita, aplicando força para a esquerda
            }
            else
            {
                rig.AddForce(new Vector2(5f, 0f),
                    ForceMode2D.Impulse); // Olhando para a esquerda, aplicando força para a direita
            }

            Play(4);
            Destroy(gameObject, 1f);
            SceneManager.LoadScene(0);
        }
    }

    void Move()
    {
        float movement = Input.GetAxis("Horizontal");

        if (Input.GetKey(KeyCode.A))
        {
            movement = -1;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            movement = 1;
        }
        else
        {
            movement = 0;
        }

        if (applyingLateralForce)
        {
            rig.velocity = new Vector2((movement * speed) + lateralForce * Mathf.Sign(rig.velocity.x), rig.velocity.y);
        }
        else
        {
            rig.velocity = new Vector2(movement * speed, rig.velocity.y);
        }

        if (movement > 0)
        {
            if (!isJumping)
            {
                anim.SetInteger("transition", 1);
            }

            transform.eulerAngles = new Vector3(0, 0, 0);
        }

        if (movement < 0)
        {
            if (!isJumping)
            {
                anim.SetInteger("transition", 1);
            }

            transform.eulerAngles = new Vector3(0, 180, 0);
        }

        if (movement == 0 && !isJumping && !isDefending && isAttacking == false)
        {
            anim.SetInteger("transition", 0);
        }
    }

    void Jump()
    {
        if (Input.GetKey(KeyCode.W))
        {
            if (!isJumping)
            {
                anim.SetInteger("transition", 2);
                rig.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
                isJumping = true;
            }
        }
    }

    private void ForwardJump()
    {
        if (Input.GetKey(KeyCode.D) && Input.GetKey(KeyCode.W))
        {
            if (!forwardJump)
            {
                anim.SetInteger("transition", 3);
                rig.AddForce(new Vector2(0, forwardJumpForce), ForceMode2D.Impulse);
                rig.AddForce(new Vector2(lateralForce * Mathf.Sign(rig.velocity.x), 0), ForceMode2D.Impulse);
                forwardJump = true;
                applyingLateralForce = true;
            }
        }

        if (Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.W))
        {
            if (!forwardJump)
            {
                anim.SetInteger("transition", 3);
                rig.AddForce(new Vector2(0, forwardJumpForce), ForceMode2D.Impulse);
                rig.AddForce(new Vector2(-lateralForce * Mathf.Sign(rig.velocity.x), 0), ForceMode2D.Impulse);
                forwardJump = true;
                applyingLateralForce = true;
            }
        }
    }

    private IEnumerator Atacar()
    {
        if (Input.GetKeyDown(KeyCode.G) && !isJumping)
        {
            isAttacking = true;
            anim.SetInteger("transition", 9);
            Play(1);
            yield return new WaitForSecondsRealtime(0.4f);
            isAttacking = false;
        }

        if (Input.GetKeyDown(KeyCode.F) && !isJumping)
        {
            isAttacking = true;
            anim.SetInteger("transition", 8);
            Play(1);
            yield return new WaitForSecondsRealtime(0.6f);
            isAttacking = false;
        }

        if (Input.GetKey(KeyCode.Y) && Input.GetKey(KeyCode.U) && Input.GetKey(KeyCode.F) && isJumping == false)
        {
            if (canUseHadouken)
            {
                canUseHadouken = false;
                isAttacking = true;
                anim.SetInteger("transition", 10);
                Play(0);
                yield return new WaitForSecondsRealtime(0.58f);
                isAttacking = false;
                GameObject bullet = Instantiate(hadouken, pontoDeTiro.position, pontoDeTiro.rotation);
                Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
                Vector3 playerDirection = transform.right * transform.localScale.x;
                rb.velocity = playerDirection * speedHadouken;
                Destroy(bullet, 2f);
                StartCoroutine(HadoukenCooldown());
            }
        }
    }

    private void Play(int numero)
    {
        Soucer.clip = Clip[numero];
        Soucer.Play();
    }

    private IEnumerator HadoukenCooldown()
    {
        yield return new WaitForSeconds(2);
        canUseHadouken = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 3 && Mathf.Abs(rig.velocity.y) < 0.01f)
        {
            isJumping = false;
            forwardJump = false;
            applyingLateralForce = false;
        }
    }

    void LookAtPlayer()
    {
        if (Player1 != null)
        {
            Vector3 direction = Player1.transform.position - transform.position;

            if (direction.x > 0)
            {
                transform.eulerAngles = new Vector3(0, 0, 0);
            }
            else
            {
                transform.eulerAngles = new Vector3(0, 180, 0);
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("hadouken"))
        {
            if (!isDefending) // Verifica se o jogador não está defendendo
            {
                TakeDamage(15);
                Play(2);
            }
            Destroy(collision.gameObject);
        }
        if (collision.CompareTag("atack"))
        {
            if (!isDefending) // Verifica se o jogador não está defendendo
            {
                TakeDamage(5);
                Play(2);
            }
        }
    }

    void TakeDamage(int damage)
    {
        if (!isDefending) // Verifica se o jogador não está defendendo
        {
            currentHealth -= damage;

            if (healthSlider != null)
            {
                healthSlider.value = currentHealth;
            }

        }
    }
}