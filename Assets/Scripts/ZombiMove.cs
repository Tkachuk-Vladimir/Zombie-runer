using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombiMove : MonoBehaviour
{
    //инициализация клавишь управления
    [SerializeField] KeyCode JumpButton;
    [SerializeField] KeyCode DownButton;
    [SerializeField] bool jumpAllowed = false; // разрешение на прыжок

    
    public float[] Yarray = { -2.3f, -0.29f, 1.72f };// массив положений персонажа
    public int n = 1; // номер этажа // floor number
    public float dx; //отступ от первого Zombie  по оси х

    float jumpForce = 10f;// сила прыжка
    float deltaJump = 0.7f;

    // поля свойств персонажа
    Rigidbody2D rb;
    CapsuleCollider2D cc;
    Animator anim;  // поле аниматор

    // инициализировали поле Audio
    public AudioSource audioZombie;
    public AudioClip clipJump; // подключаем звуки
    public AudioClip clipDown; // подключаем звуки
    public AudioClip clipApple; // подключаем звуки
    public AudioClip clipBrain; // подключаем звуки
    public AudioClip clipStone; // подключаем звуки
    public AudioClip clipPlayer; // подключаем звуки

    void Awake()
    {
        // получаем доступ к свойствам персонажа
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        cc = GetComponent<CapsuleCollider2D>();
        audioZombie = GetComponent<AudioSource>();
    }

    // задаётся стартовое положение boy
    void Start()
    {
        //set start position
        transform.position = new Vector2(-6f + dx, -2.3f);
    }

    // в этой функции опрашиваются клавиши
    void Update()
    {
        if (Input.GetKeyDown(JumpButton))
        {
            Jump();
        }

        if (Input.GetKeyDown(DownButton))
        {
            Down();
        }

    }

    // в этой функции прописывается все движения
    void FixedUpdate()
    {
        if (!ControlScript.instance.gameOver) // если жив
        {
            if (!GameObject.Find("GameControl").GetComponent<ControlScript>().isPaused) // если нет паузы
            {
                anim.enabled = true;  //включаем animationController

                if (jumpAllowed)
                {
                    // the upward movement
                    transform.position = Vector2.MoveTowards(transform.position, new Vector2(transform.position.x, Yarray[n - 1] + deltaJump), jumpForce * Time.fixedDeltaTime);
                    //if it reaches the top point
                    if (transform.position.y >= Yarray[n - 1] + deltaJump)
                    {
                        jumpAllowed = false;
                    }
                }
                else
                {   // drops to the floor
                    transform.position = Vector2.MoveTowards(transform.position, new Vector2(transform.position.x, Yarray[n - 1]), jumpForce * Time.fixedDeltaTime);
                }
            }
            else // если жив и пауза
            {
                anim.enabled = false; // выключаем анимацию
            }
        }
        else // если мёртв
        {
           // anim.enabled = false; // выключаем анимацию
        }
    }

    public void Jump()
    {
        if (n < 3)
        {

            //прыгаем на следующий этаж
            n++;

            // включаем анимацию //turn on animation  
            anim.SetTrigger("isUp");

            //получаем разрешение на прыжок
            jumpAllowed = true;

            //высота прыжка над платформой
            deltaJump = 0.7f;

            //включаем звук прыжка
            audioZombie.clip = clipJump; // установили - выбрали звук
            audioZombie.Play();     // включаем проигрыватель 


        }
        else // if he's already on the 3rd floor
        {
            if (n == 3)
            {
                if (transform.position.y <= Yarray[n - 1])
                {
                    //получаем разрешение на прыжок
                    jumpAllowed = true;

                    // включаем анимацию //turn on animation  
                    anim.SetTrigger("isUp");

                    deltaJump = 2f;

                    //включаем звук прыжка
                    audioZombie.clip = clipJump; // установили - выбрали звук
                    audioZombie.Play();     // включаем проигрыватель 
                }
            }
        }
    }

    public void Down()
    {
        if (n > 1)
        {
            //прыгаем на следующий этаж
            n--;

            // включаем анимацию падение   
            anim.SetTrigger("isDown");

            //высота прыжка над платформой
            deltaJump = 0.7f;

            //включаем звук прыжка
            audioZombie.clip = clipDown; // установили - выбрали звук
            audioZombie.Play();     // включаем проигрыватель 
        }
        

    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Brain")
        {
            //включаем звук столкновения с Мозгом
            audioZombie.clip = clipBrain; // установили - выбрали звук
            audioZombie.Play();     // включаем проигрыватель

            //удаляем мозг,который столкнулся с зомби
            Destroy(collision.gameObject);

            //подвигаем зомби на 0.5 вправо 
            transform.position = (Vector2)transform.position + new Vector2(0.5f,0f);

        }
        if (collision.tag == "Apple")
        {
            //включаем звук столкновения с Мозгом
            audioZombie.clip = clipApple; // установили - выбрали звук
            audioZombie.Play();     // включаем проигрыватель

            //удаляем мозг,который столкнулся с зомби
            Destroy(collision.gameObject);

            //подвигаем зомби на 0.5 вправо 
            transform.position = (Vector2)transform.position - new Vector2(0.3f, 0f);

        }
        if (collision.tag == "Player")
        {
            //включаем звук столкновения с Мозгом
            audioZombie.clip = clipPlayer; // установили - выбрали звук
            audioZombie.Play();     // включаем проигрыватель

            // включаем анимацию   
            anim.SetTrigger("isEat");

        }
     }
}

