using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameSceneManager : MonoBehaviour
{
    GameObject Score;
    GameObject Stage;
    GameObject MP;
    GameObject Norma;

    void Start()
    {
        this.Score = GameObject.Find("Score");
        this.Stage = GameObject.Find("Stage");
        this.MP = GameObject.Find("MP");
        this.Norma = GameObject.Find("Norma");
    }

    void Update()
    {
        this.Score.GetComponent<Text>().text = ($"Score　{TitleScene.Score}");
        this.Stage.GetComponent<Text>().text = ($"Stage　{TitleScene.Level}");
        this.MP.GetComponent<Text>().text = ($"           　　　×{PlayerController.purify}");
        this.Norma.GetComponent<Text>().text = ($"ノルマ　{TitleScene.Gold}　/　{MazeGenerator.norma}");
    }
}
