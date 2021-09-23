using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoyMove : MonoBehaviour
{
    [SerializeField] KeyCode JumpButton;
    [SerializeField] KeyCode DownButton;
    [SerializeField] bool jumpAllowed = false;

    public float jumpForce = 10f;

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
        if (Input.GetKey(JumpButton) && jumpAllowed)
        {
            Jump();
        }

        if (Input.GetKey(DownButton) && jumpAllowed)
        {
            Down();
        }
    }

    public void Jump()
    {
     // включаем анимацию   
     anim.SetTrigger("isUp");

     // включам тригер, то есть становиться прозрачным для колайдеров
     cc.isTrigger = true;

     // придаюм скорость в верх
     rb.velocity = new Vector2(0, jumpForce);
     //transform.position = Vector2.MoveTowards(transform.position, new Vector2(transform.position.x, transform.position.y + 10f), jumpForce * Time.fixedDeltaTime);
     
        // выключаем разрешение на прыжок
        //jumpAllowed = false;      
    }

    public void Down()
    {
      //transform.position = Vector2.MoveTowards(transform.position,target,jumpForce);
      //rb.velocity = new Vector2(0, -jumpForce);
        cc.isTrigger = true;
        jumpAllowed = false;    
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
     if(collision.gameObject.CompareTag("Platform"))
        {
          jumpAllowed = true;    
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
     if(collision.gameObject.CompareTag("Platform"))
        {
         // cc.isTrigger = false;
          jumpAllowed = true;
        }
    }
}
//rb.AddForce(transform.up * jumpForce);
//transform.Translate(Vector2.up * jumpForce * Time.deltaTime, Space.World);

// transform.position = Vector2.MoveTowards(transform.position, new Vector2(0, 1), jumpForce * Time.fixedDeltaTime);//, jumpForce * Time.deltaTime);
//rb.AddForce(new Vector2(0, 70), ForceMode2D.Impulse);
//rb.MovePosition(transform.position + transform.up * jumpForce);// * Time.fixedDeltaTime);
//transform.Translate(Vector2.up * jumpForce * Time.deltaTime);
//rb.AddForce(transform.up * jampForce * Time.fixedDeltaTime);

//rb.MovePosition(transform.position * transform.up * jumpForce);//Time.fixedDeltaTime);
//transform.Translate(Vector2.down * 1.3f);// * Time.deltaTime);
// rb.AddForce(transform.up * jampForce);
//cc.isTrigger = true;