using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Frog : Enemy
{
    [SerializeField] private float leftCap;
    [SerializeField] private float rightCap;
    [SerializeField] private float jumpLength = 10f;
    [SerializeField] private float jumpHeight = 15f;
    [SerializeField] private LayerMask ground;
    private Collider2D col;

    private bool facingLeft = true;

    protected override void Start() {

        base.Start();
        col = GetComponent<Collider2D>();
    }

    private void Update()
    {
        //transition from jump to fall
        Yo();
      //transition from fall to idle
    }

    private void Yo() 
    {
        //transition
        if (anim.GetBool("Jumping")) 
        {
            if (rb.velocity.y < -.1) 
            {
                anim.SetBool("Falling", true);
                anim.SetBool("Jumping", false);
            }
        }

        //fall to idle
        if (col.IsTouchingLayers(ground) && anim.GetBool("Falling")) 
        {
            anim.SetBool("Falling", false);
        }
    
    }

    private void Move()
    {
        if (facingLeft)
        {
            //Test to see if we are beyond left cap
            //if not we are going to turn around
            if (transform.position.x > leftCap)
            {
                //makesure sprite is facing right location
                if (transform.localScale.x != 1)
                {
                    transform.localScale = new Vector3(1, 1);
                }

                //Test to see if the frog is on the ground, then jump
                if (col.IsTouchingLayers(ground))
                {
                    //Jump
                    rb.velocity = new Vector2(-jumpLength, jumpHeight);
                    anim.SetBool("Jumping", true);
                }
            }
            else
            {
                facingLeft = false;
            }

        }

        else 
        {
            //Test to see if we are beyond left cap
            //if not we are going to turn around
            if (transform.position.x < rightCap)
            {
                //makesure sprite is facing right location
                if (transform.localScale.x != -1)
                {
                    transform.localScale = new Vector3(-1, 1);
                }

                //Test to see if the frog is on the ground, then jump
                if (col.IsTouchingLayers(ground))
                {
                    //Jump
                    rb.velocity = new Vector2(jumpLength, jumpHeight);
                    anim.SetBool("Jumping", true);
                    
                }
            }
            else
            {
                facingLeft = true;
            }
        }
    }




}
