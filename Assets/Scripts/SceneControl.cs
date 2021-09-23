using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; 

public class SceneControl : MonoBehaviour
{
  public bool accesScenes;
  GameObject Menu;

  int setLevel = 0; //  в этой переменной будет храниться номер сцены

  private void Start()
 {
    Menu = GameObject.Find("Menu");    
 }

  public void LoadMainScene() // функция для клавиши Play, загрузка выбранного уровня 
  {
    accesScenes = true; // активация клавишь

    Menu.transform.GetChild(5).gameObject.SetActive(true); // 1p button
    Menu.transform.GetChild(6).gameObject.SetActive(true); // 2p button
    Menu.transform.GetChild(7).gameObject.SetActive(true); // 4p button
    //SceneManager.LoadScene(setLevel);
  }

  public void ExitButton()
  {
    Application.Quit();// exit game
  }

  public void OnePerson()
  {
    setLevel = 1; // установка уровня
    SceneManager.LoadScene(setLevel);// загрузка уровня 1p
    }
  public void TwoPerson()
  {
    setLevel = 2; //установка уровня
    SceneManager.LoadScene(setLevel);// загрузка уровня 2p
    }
  public void FoutPerson()
  {
    setLevel = 3; //установка уровня
    SceneManager.LoadScene(setLevel);// загрузка уровня 4p
    }
}
