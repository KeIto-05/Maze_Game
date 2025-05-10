using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScene : MonoBehaviour
{
    public static int Size = 31;
    public static int Level = 1;
    public static int Score = 0;
    public static int Gold = 0;

    int on, on2;
    float speed_move = 0.001f;
    float speed = 0.005f;
    SpriteRenderer sr;
    float cla;

    void Start()
    {
        PlayerController.purify = 1;
        Size = 31;
        Level = 1;
        Score = 0;
        Gold = 0;

        sr = GetComponent<SpriteRenderer>();
        cla = sr.color.a;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            SceneManager.LoadScene("GameScene");
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SceneManager.LoadScene("Ranking");
        }

        //ロゴを動かす
        transform.Translate(0, speed_move, 0);
        on = (on + 1) % 90;
        if (on == 0)
        {
            speed_move *= -1;
        }

        //透明度変更
        cla -= speed;
        sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, cla);
        on2 = (on2 + 1) % 120;
        if(on2 == 0)
        {
            speed *= -1;
        }
    }

   
}
