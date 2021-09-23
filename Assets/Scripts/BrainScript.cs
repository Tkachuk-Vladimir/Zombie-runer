using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrainScript : MonoBehaviour
{
    float BrainSpeed = 5f; // скорость платформы

    void FixedUpdate()
    {
        // проверка жив ли персонаж
        if (!GameObject.Find("GameControl").GetComponent<ControlScript>().gameOver && !GameObject.Find("GameControl").GetComponent<ControlScript>().isPaused)
        {
            // движение платформы
            transform.position = (Vector2)transform.position + Vector2.left * BrainSpeed * Time.deltaTime;
        }

        if (transform.position.x <= -11f)
        {
            Destroy(gameObject);
        }
    }
    
}
