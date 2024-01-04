using System.Collections;
using UnityEngine;

public class Player2 : MonoBehaviour
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
    public Player player;
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
        player = FindObjectOfType<Player>();
        Soucer = GetComponent<AudioSource>();
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

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            movement = -1;
        }
        else if (Input.GetKey(KeyCode.RightArrow))
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

        // Ajusta a escala para olhar na direção correta
        if (movement > 0)
        {
            if (!isJumping)
            {
                anim.SetInteger("transition", 1);
            }
            //transform.localScale = new Vector3(1, 1, 1); // Mantém a escala normal
        }
        else if (movement < 0)
        {
            if (!isJumping)
            {
                anim.SetInteger("transition", 1);
            }
            //transform.localScale = new Vector3(-1, 1, 1); // Inverte a escala para olhar para a esquerda
        }

        if (movement == 0 && !isJumping && isAttacking == false)
        {
            anim.SetInteger("transition", 0);
        }
    }

    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (!isJumping && Mathf.Abs(rig.velocity.y) < 0.01f)
            {
                anim.SetInteger("transition", 2);
                rig.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
                isJumping = true;
            }
        }
    }
    private void ForwardJump()
    {
        if (Input.GetKey(KeyCode.RightArrow) && Input.GetKey(KeyCode.UpArrow))
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

        if (Input.GetKey(KeyCode.LeftArrow) && Input.GetKey(KeyCode.UpArrow))
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
        if (Input.GetKeyDown(KeyCode.Keypad5) && !isJumping)
        {
            isAttacking = true;
            anim.SetInteger("transition", 8);
            Play(1);
            yield return new WaitForSecondsRealtime(0.35f);
            isAttacking = false;
        }
        else if (Input.GetKeyDown(KeyCode.Keypad4) && !isJumping)
        {
            isAttacking = true;
            anim.SetInteger("transition", 9);
            Play(1);
            yield return new WaitForSecondsRealtime(0.54f);
            isAttacking = false;
        }
        else if (Input.GetKey(KeyCode.DownArrow) && Input.GetKey(KeyCode.Keypad6) && Input.GetKey(KeyCode.Keypad5) && !isJumping)
        {
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
        }
    }
    private void Play(int numero)
    {
        Soucer.clip = Clip[numero];
        Soucer.Play();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 3 && Mathf.Abs(rig.velocity.y) < 0.01f)
        {
            isJumping = false;
            forwardJump = false;
            applyingLateralForce = false; // Desativa a aplicação de força lateral ao pousar
        }
    }
    void LookAtPlayer()
    {
        if (player != null)
        {
            Vector3 direction = player.transform.position - transform.position;
        
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
