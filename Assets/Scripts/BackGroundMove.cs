using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGroundMove : MonoBehaviour
{
    float bgSpeed = 0.5f; // скорость платформы

    float offset;// отступ

    private void Start()
    {
        offset = GameObject.Find("GameControl").GetComponent<ControlScript>().BGHorizontalLenght;
    }
    void FixedUpdate()
    {
        // проверка жив ли персонаж
        if (!GameObject.Find("GameControl").GetComponent<ControlScript>().gameOver && !GameObject.Find("GameControl").GetComponent<ControlScript>().isPaused)
        {
            // движение платформы
            transform.position = (Vector2)transform.position + Vector2.left * bgSpeed * Time.deltaTime;

        }

        if (transform.position.x <= -15.5f)
        {
            
          transform.position = new Vector2(transform.position.x + (offset + 2.38f) * 3f, transform.position.y);// перенос впрвво на 3и длины bg
            
        }
    }
}
//transform.Translate(Vector2.left * platforSpeed * Time.deltaTime);