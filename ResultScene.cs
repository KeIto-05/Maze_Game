using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ResultScene : MonoBehaviour
{
    GameObject GoldScore;
    GameObject enemyBonus;
    public GameObject StageBonus;
    GameObject TotalScore;

    public GameObject Enelgyimg;
    public GameObject ClearBonus;


    public static float EnemyBonus;
    float MazeBonus = 1 + (float)TitleScene.Level / 20;
    void Start()
    {
        this.GoldScore = GameObject.Find("GoldScore");
        this.enemyBonus = GameObject.Find("EnemyBonus");
        this.TotalScore = GameObject.Find("TotalScore");

        if (TitleScene.Level % 5 == 0 || TitleScene.Level == 1) PlayerController.purify += 1;
        else
        {
            Enelgyimg.SetActive(false);
            ClearBonus.SetActive(false);
        }
        if (TitleScene.Level % 2 == 0 && TitleScene.Size < 75) TitleScene.Size += 2;

        TitleScene.Level += 1;
        TitleScene.Score += (int)Mathf.Round(TitleScene.Gold * 100 * MazeBonus * EnemyBonus);

        //UI表示
        this.GoldScore.GetComponent<Text>().text = ($"：100×{TitleScene.Gold}");
        this.enemyBonus.GetComponent<Text>().text = ("EnemyBonus：×" + EnemyBonus.ToString());
        this.StageBonus.GetComponent<Text>().text = ("StageBonus：×" + MazeBonus.ToString());
        this.TotalScore.GetComponent<Text>().text = ($"TotalScore：{TitleScene.Score}");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            TitleScene.Gold = 0;
            SceneManager.LoadScene("GameScene");
        }
    }
}
