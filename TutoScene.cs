using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class TutoScene : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) || Input.anyKeyDown)
        {
            SceneManager.LoadScene("TitleScene");
        }
        
    }
}
