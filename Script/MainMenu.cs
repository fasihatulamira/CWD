using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void Belajar(){
        SceneManager.LoadScene("Belajar");
    }

    public void Bermain(){
        SceneManager.LoadScene("Bermain");
    }

    public void back (){
        SceneManager.LoadScene("HomePage");
    }
}
