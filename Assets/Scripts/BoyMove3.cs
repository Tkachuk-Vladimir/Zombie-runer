using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoyMove3 : MonoBehaviour
{
    //инициализация клавишь управления
    [SerializeField] KeyCode JumpButton;
    [SerializeField] KeyCode DownButton;
    [SerializeField] bool jumpAllowed = false; // разрешение на прыжок

    // [System.Serializable]
    [Header("Задать параметры")]
    public float dx; //отступ от первого player  по оси х
    public int n = 1; // номер этажа // floor number
    public ParticleSystem StoneParticale;

    float[] Yarray = { -2.3f, -0.29f, 1.72f };// массив положений персонажа
    float jumpForce = 10f;  // сила прыжка
    float deltaJump = 0.7f;

    // поля свойств персонажа
    Rigidbody2D rb;
    CapsuleCollider2D cc;
    Animator anim;  // поле аниматор

    // инициализировали поле Audio
    public AudioSource audioPlayer;
    public AudioClip clipJump; // подключаем звуки
    public AudioClip clipDown; // подключаем звуки
    public AudioClip clipHome;
    public AudioClip clipApple; // подключаем звуки
    public AudioClip clipBrain; // подключаем звуки
    public AudioClip clipStone; // подключаем звуки
    public AudioClip speedUp;
    public AudioClip speedSloow;


    void Awake()
    {
        // получаем доступ к свойствам персонажа
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        cc = GetComponent<CapsuleCollider2D>();
        audioPlayer = GetComponent<AudioSource>();
    }

    // задаётся стартовое положение boy
    void Start()
    {
        //set start position
        transform.position = new Vector2(-2f + dx, -2.3f);

        //включаем animationController
        anim.enabled = false;  
    }

    /////////////////////////////////////////////////////
    /////////////////////////////////////////////////////
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
    
    ///////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////
   
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
               if (transform.position.x >= -10f)//условие остановки 
                {
                    // движение влево 
                    transform.Translate(Vector2.right * Time.deltaTime * -1.5f);
                } 
        }
    }
    /////////////////////////////////////////////////////
    /////////////////////////////////////////////////////
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
            audioPlayer.clip = clipJump; // установили - выбрали звук
            audioPlayer.Play();     // включаем проигрыватель 


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
                    audioPlayer.clip = clipJump; // установили - выбрали звук
                    audioPlayer.Play();     // включаем проигрыватель 
                }
            }
        }

    }
    /////////////////////////////////////////////////////
    /////////////////////////////////////////////////////
    public void Down()
    {
        if (n > 1)
        {   //выключаем разрешение на пры
            jumpAllowed = false;

            //прыгаем на следующий этаж
            n--;

            // включаем анимацию падение   
            anim.SetTrigger("isDown");

            //высота прыжка над платформой
            deltaJump = 0.7f;

            //включаем звук прыжка
            audioPlayer.clip = clipDown; // установили - выбрали звук
            audioPlayer.Play();     // включаем проигрыватель 
        }

    }
    /////////////////////////////////////////////////////
    /////////////////////////////////////////////////////
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Brain")
        {
            //включаем звук 
            audioPlayer.clip = clipBrain; // установили - выбрали звук
            audioPlayer.Play();     // включаем проигрыватель

            //удаляем мозг,который столкнулся с зомби
            Destroy(collision.gameObject);

            //подвигаем зомби на 0.5 влево 
            transform.position = (Vector2)transform.position - new Vector2(0.5f, 0f);
        }


        if (collision.tag == "Stone")
        {
            //включаем звук 
            audioPlayer.clip = clipStone; // установили - выбрали звук
            audioPlayer.Play();     // включаем проигрыватель

            // появление частичек разрушения
            Instantiate(StoneParticale, collision.transform.position, Quaternion.identity);

            // удаление частиц после проигрывания
            Destroy(GameObject.Find("StoneParticle System(Clone)"), 1f);
           
            //удаляем stone,который столкнулся с boy
            Destroy(collision.gameObject);

            //подвигаем player на 0.5 влево 
            transform.position = (Vector2)transform.position - new Vector2(0.5f, 0f);
        }

        if (collision.tag == "Apple")
        {
            //включаем звук
            audioPlayer.PlayOneShot(clipApple, 1f);
            //audioPlayer.clip = clipApple; // установили - выбрали звук
            //audioPlayer.volume = 1f;      // set volume
            //audioPlayer.Play();           // включаем проигрыватель

            audioPlayer.PlayOneShot(speedUp, 0.5f);
            // audioPlayer.clip = speedUp;   // установили - выбрали звук
            //audioPlayer.volume = 0.5f;    // set volume
            //audioPlayer.Play();           // включаем проигрыватель

            //удаляем мозг,который столкнулся с зомби
            Destroy(collision.gameObject);

            //подвигаем зомби на 0.5 вправо 
            transform.position = (Vector2)transform.position + new Vector2(0.5f, 0f);
        }

        if (collision.tag == "Home")
        {
            //включаем звук 
            audioPlayer.clip = clipHome; // установили - выбрали звук
            audioPlayer.Play();     // включаем проигрыватель

            ControlScript.instance.GameOver();
            ControlScript.instance.HumanWin();
            gameObject.SetActive(false);

            // устанавливаем звук и включае его
            audioPlayer.clip = clipHome;
            audioPlayer.Play();
        }

        if (collision.tag == "Zombie")
        {
            ControlScript.instance.GameOver();
            ControlScript.instance.ZombieWin();

            // включаем анимацию смерти   
            anim.SetTrigger("isDeath");
        }

        if (collision.tag == "Switch")
        {
            // Остановка генерации мозгов и яблок
            ControlScript.instance.StopSpawnObject();

            // автоматически переводим на верхний уровень
            n = 3;
        }
        if (collision.tag == "Player")
        {
            if (transform.position.x < collision.transform.position.x)
            {
                collision.transform.position = Vector2.MoveTowards(collision.transform.position, new Vector2(4f,0f), jumpForce * Time.fixedDeltaTime);
            }
        }         
    }
}
