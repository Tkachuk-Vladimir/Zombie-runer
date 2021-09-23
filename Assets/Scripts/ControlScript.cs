using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ControlScript : MonoBehaviour
{
    public static ControlScript instance;

    //жив или нет boy
    public bool gameOver;
    public bool stopSwawnObject;
    public bool isPaused;

    // доступ к префабам
    public GameObject Platform, Brain, Apple, Home, Menu, PauseObject,BGObject;

    // координаты появления платформ
    //float Ytop = 0.8f;
    //float Ymiddle = -1.2f ;
    //float Ydown = -3.2f;

    // массив координата появления Platform
    float[] YarrayPlatform = { 0.8f, -1.2f, -3.2f };
    float x_position_platform = -10f;

    float x_position_backGround = -6f;

    float PlatformHorizontalLenght; // длина BoxCollider2D Platform
    public float BGHorizontalLenght; // длина BoxCollider2D BG

    // переменная таймер
    float timerBrain,timerApple; 
    int randomTimeBrain,randomTimeApple;
  
    // массив координата появления Brain
    float[] Yarray = { 1.5f, -0.5f, -2.5f };

    Animator HomeAnim, MenuAnim;

    GameObject Player;
    GameObject Zombie;

    void Awake() // checking working code ControlScript
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        MenuAnim = Menu.GetComponent<Animator>();
    }

    void Start()
    {
        // берём длину BGobject
        BGHorizontalLenght = BGObject.GetComponent<BoxCollider2D>().size.x; 

        //появление 3х BgObject
        for(int countBG = 0; countBG < 3; countBG++)
        {
         if(countBG == 1) // второй bg будет отзеркален
         {
           BGObject.GetComponent<SpriteRenderer>().flipX = true; // отзеркаливание вклчить
         }

          Instantiate(BGObject, new Vector2(x_position_backGround, BGObject.transform.position.y), Quaternion.identity); // создаётся элемент
          x_position_backGround = x_position_backGround + BGHorizontalLenght + 2.38f;// отступаю вправо на длину bg;
          BGObject.GetComponent<SpriteRenderer>().flipX = false; // отзеркаливание выключить
        }
    

        Player = GameObject.Find("Player"); // find Player
        Zombie = GameObject.Find("Zombies");// find Zombies

        gameOver = false;

        // включам кнопку Паузы, она появляется
        PauseObject.SetActive(false);

        //двигаю клавишу Play вправо
        //Menu.transform.GetChild(2).position =  - Vector2.up; 

        // выключаем клавишу RePlay
        Menu.transform.GetChild(3).gameObject.SetActive(false);

        // пока не нажали на клавишу play всё остановленно
        // сразу пауза
        isPaused = true;

        // получаем доступ к BoxCollider2D
        PlatformHorizontalLenght = Platform.GetComponent<BoxCollider2D>().size.x;

        // генерация платформ
        InstantiatePlatform();

        // 
        stopSwawnObject = false;


        HomeAnim = GameObject.Find("Home(Clone)").GetComponent<Animator>();
      

    }

    public void GameOver()
    { 
        // выключам кнопку Паузы, она изчезает
        PauseObject.SetActive(false);

        gameOver = true;

        //Menu.SetActive(true);

        // выключаем клавишу Play
        Menu.transform.GetChild(2).gameObject.SetActive(false);
        // включаем клавишу RePlay
        Menu.transform.GetChild(3).gameObject.SetActive(true);
        // включаем клавишу ExitPlay
        Menu.transform.GetChild(4).gameObject.SetActive(true);
        // выключаем клавишу в меню
        Menu.transform.GetChild(5).gameObject.SetActive(true);
        //включаем анимацию кнопок из меню
        MenuAnim.SetTrigger("MenuOn");
    }

    public void HumanWin()
    {
        // вклчаем анимацию закрыть дверь
        HomeAnim.SetTrigger("isClosed");
        // включаем картинку humanWin
        Menu.transform.GetChild(6).gameObject.SetActive(true);
    }

    public void ZombieWin()
    {
        // включаем клартинку zombieWin
        Menu.transform.GetChild(7).gameObject.SetActive(true);
    }

    public void StopSpawnObject()
    {
        //
        stopSwawnObject = true;

        // включаем анимацию открыть дверь
        HomeAnim.SetTrigger("isOpen");
    }

    void Update()
    {
        if (gameOver && Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        /* if(Player.GetComponent<Transform>().position.x < Zombie.GetComponent<Transform>().position.x)
         {
             Zombie.GetComponent<ZombiMove>().n = Player.GetComponent<BoyMove3>().n; 
            //Zombie.transform.Translate(Vector2.right * Time.deltaTime * -1.1f);
            //Zombie.GetComponent<Animator>().SetTrigger("isUp");
         }*/
        Zombie.GetComponent<ZombiMove>().n = Player.GetComponent<BoyMove3>().n;

    }

    void FixedUpdate()
    {
      //SpawnPlatfom();
      InstantiateBrain();
      InstantiateApple();
    }

    // public void BgControl()
    //{
    //    Instantiate(BGObject,new Vector2(BGObject.transform.position.x + BGHorizontalLenght, BGObject.transform.position.y), Quaternion.identity);
    //}

    void InstantiatePlatform()
    {
        for (int k = 0; k < 3; k++)
        {
            for (int i = 0; i < 45; i++)
            {
                Instantiate(Platform, new Vector2(x_position_platform, YarrayPlatform[k]), Quaternion.identity);
                x_position_platform = x_position_platform + PlatformHorizontalLenght / 2.6f;

                // Условие создания домика
                if (k == 0 && i == 35)
                {
                    Instantiate(Home, new Vector2(x_position_platform, YarrayPlatform[k] + 0.1f), Quaternion.identity);
                }
            }
            x_position_platform = -10f;
        }
    }

    void InstantiateBrain()
    {
        if (gameOver || isPaused) // остановка генерации если или gameOver или пауза
        {
            return;
        }
        if ( stopSwawnObject == true)// 
        {
            return;
        }

        timerBrain += Time.deltaTime; // делаем таймер

        if (timerBrain >= randomTimeBrain)
        {
            // функция создания gameObject
            Instantiate(Brain, new Vector2(10f, Yarray[Random.Range(0,3)]), Quaternion.identity);
            // reset timer;
            timerBrain = 0f;
            randomTimeBrain = Random.Range(2, 5);
        }
    }
    void InstantiateApple()
    {
        if (gameOver || isPaused)// проверка жива 
        {
            return;
        }
        if (stopSwawnObject == true)// провер
        {
            return;
        }
        timerApple += Time.deltaTime; // делаем таймер

        if (timerApple >= randomTimeApple)
        {
            // функция создания gameObject
            Instantiate(Apple, new Vector2(10f, Yarray[Random.Range(0, 3)]), Quaternion.identity);
            // reset timer;
            timerApple = 0f;
            randomTimeApple = Random.Range(5, 8);
        }
    }

    public void PlayButton()
    {
        // включам кнопку Паузы, она появляется
        PauseObject.SetActive(true);

        // включаем игру
        //gameOver = false;
        isPaused = true;

        //анимацией выключаем меню - кнопки
        MenuAnim.SetTrigger("MenuOff");

        // выключаем меню
        //Menu.SetActive(false);

        // выключаем клавишу Play
        Menu.transform.GetChild(2).gameObject.SetActive(false);
        // выключаем клавишу RePlay
        Menu.transform.GetChild(3).gameObject.SetActive(false);
        // выключаем клавишу Exit
        Menu.transform.GetChild(4).gameObject.SetActive(false);
    }

    public void ExitButton()
    {
        Application.Quit();// exit game
    }

    public void ReplayButton()
    {
        // Перезагрузка сцены
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void MenuButton()
    {
        // Переход в стартовую сцену
        SceneManager.LoadScene(0);
    }

    public void PauseButton(){

        if (isPaused == true)// если сейчас пауза, выключаем кнопки и тп
        {
            // то включаем время
            //Time.timeScale = 1; 

            // включаем игру
           // gameOver = false;

            // устанавливаем isPause в состояние выкл
            isPaused = false;

            // включам кнопку Паузы, она появляется
            PauseObject.SetActive(true);
            //анимацией выключаем меню - кнопки
            MenuAnim.SetTrigger("MenuOff");
            // выключаем клавишу Play
            Menu.transform.GetChild(2).gameObject.SetActive(false);
            // выключаем клавишу RePlay
            Menu.transform.GetChild(3).gameObject.SetActive(false);
            // выключаем клавишу Exit
            Menu.transform.GetChild(4).gameObject.SetActive(false);
            // выключаем клавишу в меню
            Menu.transform.GetChild(5).gameObject.SetActive(false);
            // выключаем image pause
            Menu.transform.GetChild(8).gameObject.SetActive(false);
        }
        else
        {
            // выключам кнопку Паузы, она изчезает
            PauseObject.SetActive(false);

            //Time.timeScale = 0; // выключаем время

            isPaused = true;

            // выключаем игру
            //gameOver = true;

            // включаем клавишу Play
            Menu.transform.GetChild(2).gameObject.SetActive(true);
            // включаем клавишу RePlay
            Menu.transform.GetChild(3).gameObject.SetActive(true);
            // включаем клавишу ExitPlay
            Menu.transform.GetChild(4).gameObject.SetActive(true);
            // включаем клавишу Вменю
            Menu.transform.GetChild(5).gameObject.SetActive(true);
            // вsключаем image pause
            Menu.transform.GetChild(8).gameObject.SetActive(true);

            // выключаем меню
            // Menu.SetActive(true);

            MenuAnim.SetTrigger("MenuOn");

        }

    }
}
