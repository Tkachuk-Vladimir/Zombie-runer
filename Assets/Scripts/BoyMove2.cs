using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoyMove2 : MonoBehaviour
{
    [SerializeField] KeyCode JumpButton;
    [SerializeField] KeyCode DownButton;
    [SerializeField] bool jumpAllowed = false;
    [SerializeField] bool downAllowed = false;

    [SerializeField] float jumpForce = 15f;
    [SerializeField] float downForce = 0f;

    Rigidbody2D rb;
    CapsuleCollider2D cc;
    Animator anim;  // поле аниматор

    Vector2 target;

    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        cc = GetComponent<CapsuleCollider2D>();

        target = new Vector2(transform.position.x, transform.position.y + 3f);
    }

    void FixedUpdate()
    {

        if (Input.GetKeyDown(JumpButton))
        {
            // включаем анимацию   
            anim.SetTrigger("isUp");
            jumpAllowed = true;
        }

        Jump();

        if (Input.GetKeyDown(DownButton))
        {
            downAllowed = true;
        }

        Down();
    }

    public void Jump()
    {
        if (jumpAllowed)
        {
            // придаюм скорость в верх
            transform.position =  new Vector2(transform.position.x, transform.position.y + jumpForce * Time.deltaTime);
            jumpForce--;

            if (jumpForce <= -7f)
            {
                jumpForce = 15f;
                // выключаем разрешение на прыжок
                jumpAllowed = false;
            }
        
        }
    }

    public void Down()
    {
        if (downAllowed)
        {
            // придаюм скорость в верх
            transform.position = new Vector2(transform.position.x, transform.position.y + downForce * Time.deltaTime);
            downForce--;

            if (downForce <= -14.11f)
            {
                downForce = 0f;
                // выключаем разрешение на прыжок
                downAllowed = false;
            }
        }
    }
}


//transform.Translate(new Vector2(0f,2f));// * Time.deltaTime);// нужен
//transform.position = Vector2.MoveTowards(transform.position, new Vector2(transform.position.x, transform.position.y + 10f), jumpForce * Time.fixedDeltaTime);