using System.Collections;
using UnityEngine;

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

    private bool forwardJump;
    private bool isJumping;
    private bool isAttacking;
    private bool applyingLateralForce; 
    private Animator anim;
    private Rigidbody2D rig;
    private bool canUseHadouken = true;

    void Start()
    {
        anim = GetComponent<Animator>();
        rig = GetComponent<Rigidbody2D>();
        Player1 = FindObjectOfType<Player2>();
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 3)
        {
            isJumping = false;
            forwardJump = false;
            applyingLateralForce = false;
        }
    }
    private IEnumerator HadoukenCooldown()
    {
        yield return new WaitForSeconds(2);
        canUseHadouken = true;
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
}