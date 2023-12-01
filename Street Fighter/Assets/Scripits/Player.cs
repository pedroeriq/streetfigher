using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Player : MonoBehaviour
{
    public float speed;
    public float jumpForce;
    public float forwardJumpForce;
    
    private bool forwardJump;
    private bool isJumping;
    private Animator anim;
    private Rigidbody2D rig;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        rig = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Jump();
        ForwardJump();
    }

    void Move()
    {
        float movement = Input.GetAxis("Horizontal");
        rig.velocity = new Vector2(movement * speed, rig.velocity.y);
        
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
            if (!isJumping )
            {
                anim.SetInteger("transition", 1);
            }
            transform.eulerAngles = new Vector3(0, 180, 0);
        }

        if (movement == 0 && !isJumping)
        {
            anim.SetInteger("transition", 0);
        }
    }
    void Jump()
    {
        if (Input.GetButtonDown("Jump"))
        {
            if(!isJumping)
            {
                anim.SetInteger("transition", 2);
                rig.AddForce(new Vector2(0,jumpForce), ForceMode2D.Impulse);
                isJumping = true;
            }
        }
    }

    private void ForwardJump()
    {
        if(Input.GetKey(KeyCode.D) && Input.GetKey(KeyCode.Space))
        {
            if (!forwardJump)
            {
                anim.SetInteger("transition", 3);
                rig.AddForce(new Vector2(0,forwardJumpForce), ForceMode2D.Impulse);
                forwardJump = true;
            }
        }

        if (Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.Space))
        {
            if (!forwardJump)
            {
                anim.SetInteger("transition", 3);
                rig.AddForce(new Vector2(0,forwardJumpForce), ForceMode2D.Impulse);
                forwardJump = true;
            }
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
   {
       if (collision.gameObject.layer == 3)
       {
           isJumping = false;
           forwardJump = false;
       }
   }
}