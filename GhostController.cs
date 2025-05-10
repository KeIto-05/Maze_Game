using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GhostController : MonoBehaviour
{
    Rigidbody2D rigid2D;
    MazeGenerator mazegen;
    GameObject player;
    GameObject MazeGenerator;
    const int Wall = 1;

    bool Load = false;
    float speed = 5.0f; //移動速度
    int[,] map; //迷路
    int size = TitleScene.Size;
    float distance;
    int search_range = 6;
    int mode = 1; //移動モード
    int mode_manage = 0; //モード管理用
    int foward; //向き(前方)
    Vector2Int nest;
    Vector2Int destination;

    void Start()
    {
        this.rigid2D = GetComponent<Rigidbody2D>();
        this.player = GameObject.Find("player");
        this.MazeGenerator = GameObject.Find("MazeGenerator");
        mazegen = MazeGenerator.GetComponent<MazeGenerator>();
        nest = new Vector2Int((int)transform.position.x, (int)transform.position.y);
        destination = nest;
        StartCoroutine(Loading());
    }

    IEnumerator Loading()
    {
        yield return null;
        yield return new WaitForSeconds(0.5f);
        map = mazegen.maze;
        yield return new WaitForSeconds(0.5f);
        Load = true;
    }

    void Update()
    {
        //距離を測る
        distance = (float)Math.Sqrt((Math.Pow((transform.position.x - player.transform.position.x), 2) + Math.Pow((transform.position.y - player.transform.position.x), 2)));
        if (Load)
        {
            if ((destination.x - 0.1 < transform.position.x && transform.position.x < destination.x + 0.1) && (destination.y - 0.1 < transform.position.y && transform.position.y < destination.y + 0.1))
            {
                DestChange();
            }
            Move();
        }

        //モード切り替え
        if (Math.Round(distance) < search_range && mode == 1)
        {
            mode = 0;
        }
    }


    //当たり判定
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "player")
        {
            SceneManager.LoadScene("GameOver");
        }
        else if (collision.gameObject.name != "WallPrefab(Clone)")
        {
            mode = 2;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.name == "PurifyPrefab(Clone)") Destroy(gameObject);
    }


    //移動
    void Move()
    {
        if (destination.y - 0.1 < transform.position.y && transform.position.y < destination.y + 0.1)
        {
            if (transform.position.x < destination.x)
            {
                rigid2D.linearVelocity = new Vector2(speed, 0);
                foward = 0;
            }
            else
            {
                rigid2D.linearVelocity = new Vector2(-speed, 0);
                foward = 1;
            }
        }
        else
        {
            if (transform.position.y < destination.y)
            {
                rigid2D.linearVelocity = new Vector2(0, speed);
                foward = 2;
            }
            else
            {
                rigid2D.linearVelocity = new Vector2(0, -speed);
                foward = 3;
            }
        }
    }


    //目的地変更
    void DestChange()
    {
        if (mode == 0)
        {
            int rnd_select = UnityEngine.Random.Range(0, 100);
            if (rnd_select < 95) Rnd_walk();
            else BFS();
            int rnd = UnityEngine.Random.Range(0, 10);
            if (rnd < 2) mode_manage += 1;
        }
        else if (mode == 2)
        {
            int rnd_select = UnityEngine.Random.Range(0, 10);
            if (rnd_select < 8) Return_nest();
            else Rnd_walk();
        }
        else
        {
            int rnd = UnityEngine.Random.Range(0, 10);
            if (rnd > 3) BFS();
            else Rnd_walk();
        }

        //追跡を諦める
        if (mode == 0)
        {
            int rnd = UnityEngine.Random.Range(0, 100);
            if (rnd < mode_manage)
            {
                mode = 2;
                mode_manage = 0;
            }
        }
        //追跡を再開
        if (mode == 2 && distance > search_range + 2)
        {
            int rnd = UnityEngine.Random.Range(0, 5);
            if (rnd < 4) mode = 1;
        }
    }



    //行き先決定法
    //ランダム
    void Rnd_walk()
    {
        int rnd_select = UnityEngine.Random.Range(0, 10);

        if (rnd_select < 7)
        {
            var list_dire = new List<int>();
            int x = (int)Math.Round(transform.position.x);
            int y = (int)Math.Round(transform.position.y);
            int rnd_dire;

            if (map[x + 1, y] != Wall && foward != 1) list_dire.Add(0);
            if (map[x - 1, y] != Wall && foward != 0) list_dire.Add(1);
            if (map[x, y + 1] != Wall && foward != 3) list_dire.Add(2);
            if (map[x, y - 1] != Wall && foward != 2) list_dire.Add(3);
            if (list_dire.Count > 0)
            {
                rnd_dire = UnityEngine.Random.Range(0, list_dire.Count);
                switch (list_dire[rnd_dire])
                {
                    case 0:
                        destination = new Vector2Int(x + 1, y);
                        break;
                    case 1:
                        destination = new Vector2Int(x - 1, y);
                        break;
                    case 2:
                        destination = new Vector2Int(x, y + 1);
                        break;
                    case 3:
                        destination = new Vector2Int(x, y - 1);
                        break;
                }
            }
        }
        else if (rnd_select > 7) BFS();
        else Return_nest();
    }



    //幅優先探索
    void BFS()
    {
        Vector2Int start = new Vector2Int((int)Math.Round(transform.position.x), (int)Math.Round(transform.position.y));
        Vector2Int goal = new Vector2Int((int)Math.Round(this.player.transform.position.x), (int)Math.Round(this.player.transform.position.y));
        var Q = new Queue<Vector2Int>();
        var Q_done = new List<Vector2Int>();
        var S_reverse = new Stack<Vector2Int>();
        var rootLog = new List<Vector3Int>();
        Q_done.Add(start);
        Q.Enqueue(start);
        while (Q.Count > 0)
        {
            Vector2Int v = Q.Dequeue();

            //終了条件
            if (v == goal) break;

            //vから繋がる点を探す
            if (map[v.x + 1, v.y] != Wall && !Q_done.Contains(new Vector2Int(v.x + 1, v.y)))
            {
                Q_done.Add(new Vector2Int(v.x + 1, v.y));
                Q.Enqueue(new Vector2Int(v.x + 1, v.y));
                rootLog.Add(new Vector3Int(v.x + v.y * size, v.x + 1, v.y));
            }
            if (map[v.x - 1, v.y] != Wall && !Q_done.Contains(new Vector2Int(v.x - 1, v.y)))
            {
                Q_done.Add(new Vector2Int(v.x - 1, v.y));
                Q.Enqueue(new Vector2Int(v.x - 1, v.y));
                rootLog.Add(new Vector3Int(v.x + v.y * size, v.x - 1, v.y));
            }
            if (map[v.x, v.y + 1] != Wall && !Q_done.Contains(new Vector2Int(v.x, v.y + 1)))
            {
                Q_done.Add(new Vector2Int(v.x, v.y + 1));
                Q.Enqueue(new Vector2Int(v.x, v.y + 1));
                rootLog.Add(new Vector3Int(v.x + v.y * size, v.x, v.y + 1));
            }
            if (map[v.x, v.y - 1] != Wall && !Q_done.Contains(new Vector2Int(v.x, v.y - 1)))
            {
                Q_done.Add(new Vector2Int(v.x, v.y - 1));
                Q.Enqueue(new Vector2Int(v.x, v.y - 1));
                rootLog.Add(new Vector3Int(v.x + v.y * size, v.x, v.y - 1));
            }
        }

        //ルートを辿る
        S_reverse.Push(goal);
        Vector2Int now = S_reverse.Peek();
        while (true)
        {
            for (int i = 0; i < rootLog.Count; i++)
            {
                Vector2Int seek = new Vector2Int(rootLog[i].y, rootLog[i].z);
                if (now == seek)
                {
                    now = new Vector2Int(rootLog[i].x % size, rootLog[i].x / size);
                    break;
                }
            }
            if (now == start) break;
            S_reverse.Push(now);
        }
        destination = S_reverse.Pop();
    }


    //巣に帰る
    void Return_nest()
    {
        Vector2Int start = new Vector2Int((int)Math.Round(transform.position.x), (int)Math.Round(transform.position.y));
        Vector2Int goal = nest;
        var Q = new Queue<Vector2Int>();
        var Q_done = new List<Vector2Int>();
        var S_reverse = new Stack<Vector2Int>();
        var rootLog = new List<Vector3Int>();
        Q_done.Add(start);
        Q.Enqueue(start);
        while (Q.Count > 0)
        {
            Vector2Int v = Q.Dequeue();

            //終了条件
            if (v == goal) break;

            //vから繋がる点を探す
            if (map[v.x + 1, v.y] != Wall && !Q_done.Contains(new Vector2Int(v.x + 1, v.y)))
            {
                Q_done.Add(new Vector2Int(v.x + 1, v.y));
                Q.Enqueue(new Vector2Int(v.x + 1, v.y));
                rootLog.Add(new Vector3Int(v.x + v.y * size, v.x + 1, v.y));
            }
            if (map[v.x - 1, v.y] != Wall && !Q_done.Contains(new Vector2Int(v.x - 1, v.y)))
            {
                Q_done.Add(new Vector2Int(v.x - 1, v.y));
                Q.Enqueue(new Vector2Int(v.x - 1, v.y));
                rootLog.Add(new Vector3Int(v.x + v.y * size, v.x - 1, v.y));
            }
            if (map[v.x, v.y + 1] != Wall && !Q_done.Contains(new Vector2Int(v.x, v.y + 1)))
            {
                Q_done.Add(new Vector2Int(v.x, v.y + 1));
                Q.Enqueue(new Vector2Int(v.x, v.y + 1));
                rootLog.Add(new Vector3Int(v.x + v.y * size, v.x, v.y + 1));
            }
            if (map[v.x, v.y - 1] != Wall && !Q_done.Contains(new Vector2Int(v.x, v.y - 1)))
            {
                Q_done.Add(new Vector2Int(v.x, v.y - 1));
                Q.Enqueue(new Vector2Int(v.x, v.y - 1));
                rootLog.Add(new Vector3Int(v.x + v.y * size, v.x, v.y - 1));
            }
        }

        //ルートを辿る
        S_reverse.Push(goal);
        Vector2Int now = S_reverse.Peek();
        while (true)
        {
            for (int i = 0; i < rootLog.Count; i++)
            {
                Vector2Int seek = new Vector2Int(rootLog[i].y, rootLog[i].z);
                if (now == seek)
                {
                    now = new Vector2Int(rootLog[i].x % size, rootLog[i].x / size);
                    break;
                }
            }
            if (now == start) break;
            S_reverse.Push(now);
        }
        destination = S_reverse.Pop();
    }
}