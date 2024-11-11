using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class TraceNavigate : MonoBehaviour
{
    public void Trace (){
        SceneManager.LoadScene("TestDraw");
    }
}
