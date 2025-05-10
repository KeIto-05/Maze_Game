using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class TotoButton : MonoBehaviour
{
    public void OnClick()
    {
        SceneManager.LoadScene("TutorialScene");
    }
}
