using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    GameObject Result;

    int[] newsta_score = new int[2];
    int[] sta_score10 = new int[2];

    void Start()
    {
        Result = GameObject.Find("Result");
        Result.GetComponent<Text>().text = ($"Stage {TitleScene.Level - 1}　　Score {TitleScene.Score}");

        newsta_score[0] = TitleScene.Level - 1;
        newsta_score[1] = TitleScene.Score;
        sta_score10[0] = PlayerPrefs.GetInt("STAGE10", 0);
        sta_score10[1] = PlayerPrefs.GetInt("SCORE10", 0);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (newsta_score[0] > sta_score10[0] || (newsta_score[0] == sta_score10[0] && newsta_score[1] > sta_score10[0]))
            {
                SceneManager.LoadScene("Ranking");
            }
            else
            {
                SceneManager.LoadScene("TitleScene");
            }
        }
    }
}
