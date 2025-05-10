using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ScoreManager : MonoBehaviour
{
    int[] newsta_score = new int[2];
    int[,] sta_score = new int[10, 2];

    public GameObject Ranking1;
    public GameObject Ranking2;

    void Start()
    {
        //最新スコア取得
        newsta_score[0] = TitleScene.Level - 1;
        newsta_score[1] = TitleScene.Score;

        //ランキング取得
        //1
        sta_score[0, 0] = PlayerPrefs.GetInt("STAGE1", 0);
        sta_score[0, 1] = PlayerPrefs.GetInt("SCORE1", 0);
        //2
        sta_score[1, 0] = PlayerPrefs.GetInt("STAGE2", 0);
        sta_score[1, 1] = PlayerPrefs.GetInt("SCORE2", 0);
        //3
        sta_score[2, 0] = PlayerPrefs.GetInt("STAGE3", 0);
        sta_score[2, 1] = PlayerPrefs.GetInt("SCORE3", 0);
        //4
        sta_score[3, 0] = PlayerPrefs.GetInt("STAGE4", 0);
        sta_score[3, 1] = PlayerPrefs.GetInt("SCORE4", 0);
        //5
        sta_score[4, 0] = PlayerPrefs.GetInt("STAGE5", 0);
        sta_score[4, 1] = PlayerPrefs.GetInt("SCORE5", 0);
        //6
        sta_score[5, 0] = PlayerPrefs.GetInt("STAGE6", 0);
        sta_score[5, 1] = PlayerPrefs.GetInt("SCORE6", 0);
        //7
        sta_score[6, 0] = PlayerPrefs.GetInt("STAGE7", 0);
        sta_score[6, 1] = PlayerPrefs.GetInt("SCORE7", 0);
        //8
        sta_score[7, 0] = PlayerPrefs.GetInt("STAGE8", 0);
        sta_score[7, 1] = PlayerPrefs.GetInt("SCORE8", 0);
        //9
        sta_score[8, 0] = PlayerPrefs.GetInt("STAGE9", 0);
        sta_score[8, 1] = PlayerPrefs.GetInt("SCORE9", 0);
        //10
        sta_score[9, 0] = PlayerPrefs.GetInt("STAGE10", 0);
        sta_score[9, 1] = PlayerPrefs.GetInt("SCORE10", 0);


        //スコア比較
        if(newsta_score[0] > sta_score[9, 0] || (newsta_score[0] == sta_score[9, 0] && newsta_score[1] > sta_score[9, 0]))
        {
            sta_score[9, 0] = newsta_score[0];
            sta_score[9, 1] = newsta_score[1];

            for(int i = 9; i > 0; i--)
            {
                if(sta_score[i, 0] > sta_score[i - 1, 0] || (sta_score[i, 0] == sta_score[i - 1, 0] && sta_score[i, 1] > sta_score[i - 1, 1]))
                {
                    newsta_score[0] = sta_score[i - 1, 0];
                    newsta_score[1] = sta_score[i - 1, 1];
                    sta_score[i - 1, 0] = sta_score[i, 0];
                    sta_score[i - 1, 1] = sta_score[i, 1];
                    sta_score[i, 0] = newsta_score[0];
                    sta_score[i, 1] = newsta_score[1];
                }
            }

            //スコア保存
            //1
            PlayerPrefs.SetInt("STAGE1", sta_score[0, 0]);
            PlayerPrefs.SetInt("SCORE1", sta_score[0, 1]);
            //2
            PlayerPrefs.SetInt("STAGE2", sta_score[1, 0]);
            PlayerPrefs.SetInt("SCORE2", sta_score[1, 1]);
            //3
            PlayerPrefs.SetInt("STAGE3", sta_score[2, 0]);
            PlayerPrefs.SetInt("SCORE3", sta_score[2, 1]);
            //4
            PlayerPrefs.SetInt("STAGE4", sta_score[3, 0]);
            PlayerPrefs.SetInt("SCORE4", sta_score[3, 1]);
            //5
            PlayerPrefs.SetInt("STAGE5", sta_score[4, 0]);
            PlayerPrefs.SetInt("SCORE5", sta_score[4, 1]);
            //6
            PlayerPrefs.SetInt("STAGE6", sta_score[5, 0]);
            PlayerPrefs.SetInt("SCORE6", sta_score[5, 1]);
            //7
            PlayerPrefs.SetInt("STAGE7", sta_score[6, 0]);
            PlayerPrefs.SetInt("SCORE7", sta_score[6, 1]);
            //8
            PlayerPrefs.SetInt("STAGE8", sta_score[7, 0]);
            PlayerPrefs.SetInt("SCORE8", sta_score[7, 1]);
            //9
            PlayerPrefs.SetInt("STAGE9", sta_score[8, 0]);
            PlayerPrefs.SetInt("SCORE9", sta_score[8, 1]);
            //10
            PlayerPrefs.SetInt("STAGE10", sta_score[9, 0]);
            PlayerPrefs.SetInt("SCORE10", sta_score[9, 1]);

            PlayerPrefs.Save();
        }


        //ランキング表示
        Ranking1.GetComponent<Text>().text = ($"1．Stage {sta_score[0, 0]}　　Score {sta_score[0, 1]}\r\n\r\n\r\n2．Stage {sta_score[1, 0]}　　Score {sta_score[1, 1]}\r\n\r\n\r\n3．Stage {sta_score[2, 0]}　　Score {sta_score[2, 1]}\r\n\r\n\r\n4．Stage {sta_score[3, 0]}　　Score {sta_score[3, 1]}\r\n\r\n\r\n5．Stage {sta_score[4, 0]}　　Score {sta_score[4, 1]}");
        Ranking2.GetComponent<Text>().text = ($"6．Stage {sta_score[5, 0]}　　Score {sta_score[5, 1]}\r\n\r\n\r\n7．Stage {sta_score[6, 0]}　　Score {sta_score[6, 1]}\r\n\r\n\r\n8．Stage {sta_score[7, 0]}　　Score {sta_score[7, 1]}\r\n\r\n\r\n9．Stage {sta_score[8, 0]}　　Score {sta_score[8, 1]}\r\n\r\n\r\n10．Stage {sta_score[9, 0]}　　Score {sta_score[9, 1]}");

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) || Input.anyKeyDown)
        {
            SceneManager.LoadScene("TitleScene");
        }
    }
}
