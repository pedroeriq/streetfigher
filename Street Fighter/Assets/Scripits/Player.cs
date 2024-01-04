using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed;
    public float jumpForce;
    public float forwardJumpForce;
    public float lateralForce;
    public float speedHadouken = 11;

    public GameObject hadouken;
    public Transform pontoDeTiro;
    public Player2 Player1;

    private bool forwardJump;
    private bool isJumping;
    private bool isAttacking;
    private bool applyingLateralForce; // Variável para rastrear se a força lateral está sendo aplicada
    private Animator anim;
    private Rigidbody2D rig;

    void Start()
    {
        anim = GetComponent<Animator>();
        rig = GetComponent<Rigidbody2D>();
        Player1 = FindObjectOfType<Player2>();
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

        // Adiciona a força lateral ao cálculo da velocidade apenas durante o forward jump
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

        if (movement == 0 && !isJumping && isAttacking == false)
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
                applyingLateralForce = true; // Ativa a aplicação de força lateral
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
                applyingLateralForce = true; // Ativa a aplicação de força lateral
            }
        }
    }

    private IEnumerator Atacar()
    {
        if (Input.GetKeyDown(KeyCode.G) && !isJumping)
        {
            isAttacking = true;
            anim.SetInteger("transition", 9);
            yield return new WaitForSecondsRealtime(0.4f);
            isAttacking = false;
        }
        if (Input.GetKeyDown(KeyCode.F) && !isJumping)
        {
            isAttacking = true;
            anim.SetInteger("transition", 8);
            yield return new WaitForSecondsRealtime(0.6f);
            isAttacking = false;
        }
        if (Input.GetKey(KeyCode.Y) && Input.GetKey(KeyCode.U) && Input.GetKey(KeyCode.F) && !isJumping)
        {
            if (Input.GetKey(KeyCode.Y) && Input.GetKey(KeyCode.U) && Input.GetKey(KeyCode.F) && !isJumping)
            {
                isAttacking = true;
                anim.SetInteger("transition", 10);
                yield return new WaitForSecondsRealtime(0.58f);
                isAttacking = false;
                GameObject bullet = Instantiate(hadouken, pontoDeTiro.position, pontoDeTiro.rotation);
                Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();

                // Obter a direção do jogador
                Vector3 playerDirection = transform.right * transform.localScale.x;

                rb.velocity = playerDirection * speedHadouken;
                Destroy(bullet, 2f);
            }
        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 3)
        {
            isJumping = false;
            forwardJump = false;
            applyingLateralForce = false; // Desativa a aplicação de força lateral ao pousar
        }
    }
    void LookAtPlayer()
    {
        if (Player1 != null)
        {
            Vector3 direction = Player1.transform.position - transform.position;
        
            // Verifica se o inimigo está à direita ou à esquerda
            if (direction.x > 0)
            {
                // Se o jogador estiver à direita do jogador principal
                transform.eulerAngles = new Vector3(0, 0, 0);
            }
            else
            {
                // Se o jogador estiver à esquerda do jogador principal
                transform.eulerAngles = new Vector3(0, 180, 0);
            }
        }
    }
}