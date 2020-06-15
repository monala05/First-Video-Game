using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    //Start() variables
    private Rigidbody2D rb;
    private Animator anim;

    //Finite State Machine
    public enum State { idle, running, jumping, falling, hurt }
    private State state = State.idle;
    private Collider2D coll;
    private int score = 0;
    //Inspector variables
    [SerializeField] private LayerMask ground;
    [SerializeField] private float speed = 5f;
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private int cherries = 0;
    [SerializeField] private TextMeshProUGUI cherryText;
    [SerializeField] private float hurtForce = 10f;
    [SerializeField] private AudioSource cherry;
    [SerializeField] private AudioSource footstep;
    [SerializeField] private int health;
    [SerializeField] private TextMeshProUGUI healthAmount;
    [SerializeField] private TextMeshProUGUI scoreAmount;
    


    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        coll = GetComponent<CircleCollider2D>();
        healthAmount.text = health.ToString();
        scoreAmount.text = score.ToString();
    }

    // Update is called once per frame
    private void Update()
    {
        if (state != State.hurt)
        {
            Movement();
        }
        AnimationState();
        anim.SetInteger("state", (int)state);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Collectable"))
        {
            cherry.Play();
            Destroy(collision.gameObject);
            cherries += 1;
            score += 100;
            cherryText.text = cherries.ToString();
            scoreAmount.text = score.ToString();
        }

        if (collision.CompareTag("PowerUp")) 
        {
            Destroy(collision.gameObject);
            speed += 5f;
            jumpForce += 5f;
            GetComponent<SpriteRenderer>().color = Color.Lerp(Color.red, Color.yellow, Mathf.PingPong(Time.time, 1));
            StartCoroutine(ResetPower());
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Enemy" ) 
        {
            Enemy enemy = other.gameObject.GetComponent<Enemy>();
            if (state == State.falling)
            {
                enemy.JumpedOn();
                score += 500;
                scoreAmount.text = score.ToString();
                Jump();
            }
            else
            {
                healthLoss();  //Deals with the health system
                if (other.gameObject.transform.position.x > transform.position.x)
                {
                    //Enemy is to the righttherfore i should be damaged and moved left
                    rb.velocity = new Vector2(-hurtForce, rb.velocity.y);
                    //Destroy(this.gameObject);
                }
                else
                {
                    // Enemy is to the left, i should be going to the right when damaged
                    rb.velocity = new Vector2(hurtForce, rb.velocity.y);
                    //Destroy(this.gameObject);
                }
            }

        }

    }

    private void healthLoss()
    {
        state = State.hurt;
        health -= 1;
        healthAmount.text = health.ToString();
        if (health <= 0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    private void Movement()
    {
        float hDirection = Input.GetAxis("Horizontal");

        if (hDirection < 0)
        {
            rb.velocity = new Vector2(-speed, rb.velocity.y);
            transform.localScale = new Vector2(-1, 1);
        }

        //Moving right
        else if (hDirection > 0)
        {
            rb.velocity = new Vector2(speed, rb.velocity.y);
            transform.localScale = new Vector2(1, 1);
        }

        //Jumping
        if (Input.GetButtonDown("Jump") && coll.IsTouchingLayers(ground))
        {
            Jump();

        }
    }

    private void Jump() 
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        state = State.jumping;
    }

    private void AnimationState() 
    {

        if (state == State.jumping)
        {
            if (rb.velocity.y < .1f)
            {
                state = State.falling;
            }
        }
        else if (state == State.hurt)
        {
            if (Mathf.Abs(rb.velocity.x) < .1f) 
            {
                state = State.idle;
            }
        }

        else if (state == State.falling)
        {
            if (coll.IsTouchingLayers(ground))
            {
                state = State.idle;
            }
        }

        else if (Mathf.Abs(rb.velocity.x) > 2f)
        {
            //moving
            state = State.running;
        }
        else
        {
            state = State.idle;
        }
    }

    private void Footstep() 
    {
        footstep.Play();
    }

    private IEnumerator ResetPower() 
    {
        yield return new WaitForSeconds(10);
        jumpForce -= 5f;
        speed -= 5f;
        GetComponent<SpriteRenderer>().color = Color.white;
    }
}
