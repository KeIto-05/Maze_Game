using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeGenerator : MonoBehaviour
{
    public GameObject WallPrefab;
    public GameObject GoldPrefab;
    public GameObject GoalPrefab;
    public GameObject[] EnemyPrefab = new GameObject[4];
    public GameObject EnergyPrefab;

    int size = TitleScene.Size;
    public int[,] maze;
    const int Path = 0;
    const int Wall = 1;
    public static int norma;


    void Start()
    {
        int rnd_select = UnityEngine.Random.Range(0, 3);
        maze = new int[size, size];

        //生成法選択
        if (rnd_select == 0)
        {
            Maze_stick();
        }
        else if (rnd_select == 1)
        {
            Maze_extend();
        }
        else
        {
            Maze_dig();
        }

        //迷路生成
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                if (maze[i, j] == Wall)
                {
                    GameObject wallBrock = Instantiate(WallPrefab) as GameObject;
                    wallBrock.transform.position = new Vector3(i, j, 0);
                }
            }
        }

        //ゴール決定，ノルマを課す
        maze[size - 2, size - 2] = 2;
        Impose_quota();

        //エナジー配置
        int rnd_ene = UnityEngine.Random.Range(0, 4);
        if(rnd_ene == 0)
        {
            while (true)
            {
                int x = UnityEngine.Random.Range(1, size - 1);
                int y = UnityEngine.Random.Range(1, size - 1);

                if (maze[x, y] == Path)
                {
                    GameObject energy = Instantiate(EnergyPrefab) as GameObject;
                    energy.transform.position = new Vector3(x, y, 0);
                    maze[x, y] = 2;
                    break;
                }
            }
        }
        for(int i = 0; i < size / 10 - 3; i++)
        {
            rnd_ene = UnityEngine.Random.Range(0, 100);
            if(rnd_ene == 0)
            {
                while (true)
                {
                    int x = UnityEngine.Random.Range(1, size - 1);
                    int y = UnityEngine.Random.Range(1, size - 1);

                    if (maze[x, y] == Path)
                    {
                        GameObject energy = Instantiate(EnergyPrefab) as GameObject;
                        energy.transform.position = new Vector3(x, y, 0);
                        maze[x, y] = 2;
                        break;
                    }
                }
            }
        }

        //ゴールド配置
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                if (maze[i, j] == Path)
                {
                    int rnd = UnityEngine.Random.Range(0, 10);
                    if (rnd < 5 && !(i == size - 2 && j == size - 2))
                    {
                        GameObject gold = Instantiate(GoldPrefab) as GameObject;
                        gold.transform.position = new Vector3(i, j, 0);
                    }
                }
            }
        }


        //エネミー配置
        ResultScene.EnemyBonus = 1;
        Spawn_Enemy();
    }



    //ノルマ確認
    private void Update()
    {
        if(TitleScene.Gold >= norma && maze[size - 2, size - 2] == 2)
        {
            maze[size - 2, size - 2] = 3;
            GetComponent<AudioSource>().Play();
            GameObject goalflag = Instantiate(GoalPrefab) as GameObject;
            goalflag.transform.position = new Vector3(size - 2, size - 2, 0);
        }
    }



    //棒倒し法
    void Maze_stick()
    {
        //壁で囲う
        for (int i = 0; i < size; i++)
        {
            maze[i, 0] = 1;
            maze[i, size - 1] = Wall;
            maze[0, i] = 1;
            maze[size - 1, i] = Wall;
        }

        //偶数座標に棒を生成
        for (int i = 2; i < size - 1; i += 2)
        {
            for (int j = 2; j < size - 1; j += 2)
            {
                maze[i, j] = Wall;

                //棒を倒す
                int rnd_roop = UnityEngine.Random.Range(0, 100);
                if (rnd_roop > 9)
                {
                    while (true)
                    {
                        int range = 3;
                        if (j == 2) range = 4;
                        int rnd_dire = UnityEngine.Random.Range(0, range);
                        int x = i;
                        int y = j;

                        switch (rnd_dire)
                        {
                            case 0:
                                x++;
                                break;

                            case 1:
                                x--;
                                break;

                            case 2:
                                y++;
                                break;

                            case 3:
                                y--;
                                break;
                        }

                        if (maze[x, y] != Wall)
                        {
                            maze[x, y] = Wall;
                            break;
                        }
                    }
                }
            }
        }
    }





    //壁伸ばし法
    void Maze_extend()
    {
        var list_wall = new List<Vector2Int>();
        var list_now = new List<Vector2Int>();

        //壁で囲う
        for (int i = 0; i < size; i++)
        {
            maze[i, 0] = 1;
            maze[i, size - 1] = Wall;
            maze[0, i] = 1;
            maze[size - 1, i] = Wall;
        }

        //リストアップ
        for (int i = 2; i < size - 1; i += 2)
        {
            for (int j = 2; j < size - 1; j += 2)
            {
                list_wall.Add(new Vector2Int(i, j));
            }
        }

        int start = UnityEngine.Random.Range(0, list_wall.Count);
        int start_x = list_wall[start].x;
        int start_y = list_wall[start].y;

        //壁を伸ばす
        Extend(start_x, start_y);

        //ループを作る
        for (int i = 1; i < size - 1; i++)
        {
            for (int j = 1; j < size - 1; j++)
            {
                if (maze[i, j] == Wall)
                {
                    if ((maze[i + 1, j] == Path && maze[i - 1, j] == Path && maze[i, j + 1] == Wall && maze[i, j - 1] == Wall) || (maze[i + 1, j] == Wall && maze[i - 1, j] == Wall && maze[i, j + 1] == Path && maze[i, j - 1] == Path))
                    {
                        int rnd_roop = UnityEngine.Random.Range(0, 100);
                        if (rnd_roop < 10) maze[i, j] = Path;
                    }
                }
            }
        }

        void Extend(int x, int y)
        {
            var list_extend = new List<int>();
            if (maze[x, y] == Path)
            {
                list_now.Add(new Vector2Int(x, y));
                list_wall.Remove(new Vector2Int(x, y));
                maze[x, y] = Wall;
                //伸ばせる方向を探す
                if (!list_now.Contains(new Vector2Int(x + 2, y))) list_extend.Add(0);
                if (!list_now.Contains(new Vector2Int(x - 2, y))) list_extend.Add(1);
                if (!list_now.Contains(new Vector2Int(x, y + 2))) list_extend.Add(2);
                if (!list_now.Contains(new Vector2Int(x, y - 2))) list_extend.Add(3);

                //壁を伸ばす
                if (list_extend.Count > 0)
                {
                    int rnd_extend = UnityEngine.Random.Range(0, list_extend.Count);
                    switch (list_extend[rnd_extend])
                    {
                        case 0:
                            maze[x + 1, y] = Wall;
                            Extend(x + 2, y);
                            break;
                        case 1:
                            maze[x - 1, y] = Wall;
                            Extend(x - 2, y);
                            break;
                        case 2:
                            maze[x, y + 1] = Wall;
                            Extend(x, y + 2);
                            break;
                        case 3:
                            maze[x, y - 1] = Wall;
                            Extend(x, y - 2);
                            break;
                    }
                }
                else
                {
                    list_now.Clear();
                    if (list_wall.Count > 0)
                    {
                        int rnd_next = UnityEngine.Random.Range(0, list_wall.Count);
                        Extend(list_wall[rnd_next].x, list_wall[rnd_next].y);
                    }
                }
            }
            else
            {
                list_now.Clear();
                if (list_wall.Count > 0)
                {
                    int rnd_next = UnityEngine.Random.Range(0, list_wall.Count);
                    Extend(list_wall[rnd_next].x, list_wall[rnd_next].y);
                }
            }

        }
    }





    //穴掘り法
    void Maze_dig()
    {
        int[,] maze_dig = new int[size + 2, size + 2];
        var list_path = new List<Vector2Int>();

        //壁で埋める
        for (int i = 1; i <= size; i++)
        {
            for (int j = 1; j <= size; j++)
            {
                maze_dig[i, j] = Wall;
            }
        }

        int start_x = UnityEngine.Random.Range(1, size / 2) * 2;
        int start_y = UnityEngine.Random.Range(1, size / 2) * 2;

        //穴掘りスタート
        Dig(start_x, start_y);

        //迷路をコピー
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                maze[i, j] = maze_dig[i + 1, j + 1];
            }
        }

        //ループを作る
        for (int i = 1; i < size - 1; i++)
        {
            for (int j = 1; j < size - 1; j++)
            {
                if (maze[i, j] == Wall)
                {
                    if ((maze[i + 1, j] == Path && maze[i - 1, j] == Path && maze[i, j + 1] == Wall && maze[i, j - 1] == Wall) || (maze[i + 1, j] == Wall && maze[i - 1, j] == Wall && maze[i, j + 1] == Path && maze[i, j - 1] == Path))
                    {
                        int rnd_roop = UnityEngine.Random.Range(0, 100);
                        if (rnd_roop < 10) maze[i, j] = Path;
                    }
                }
            }
        }

        //穴を掘る
        void Dig(int x, int y)
        {
            var list_dig = new List<int>();

            if (x % 2 == 0 && y % 2 == 0 && !list_path.Contains(new Vector2Int(x, y))) list_path.Add(new Vector2Int(x, y));

            //掘り進められる方向を探す
            if (maze_dig[x + 2, y] == Wall) list_dig.Add(0);
            if (maze_dig[x - 2, y] == Wall) list_dig.Add(1);
            if (maze_dig[x, y + 2] == Wall) list_dig.Add(2);
            if (maze_dig[x, y - 2] == Wall) list_dig.Add(3);

            //掘り進める
            if (list_dig.Count > 0)
            {
                int rnd_dig = UnityEngine.Random.Range(0, list_dig.Count);
                switch (list_dig[rnd_dig])
                {
                    case 0:
                        maze_dig[x + 1, y] = Path;
                        maze_dig[x + 2, y] = Path;
                        Dig(x + 2, y);
                        break;
                    case 1:
                        maze_dig[x - 1, y] = Path;
                        maze_dig[x - 2, y] = Path;
                        Dig(x - 2, y);
                        break;
                    case 2:
                        maze_dig[x, y + 1] = Path;
                        maze_dig[x, y + 2] = Path;
                        Dig(x, y + 2);
                        break;
                    case 3:
                        maze_dig[x, y - 1] = Path;
                        maze_dig[x, y - 2] = Path;
                        Dig(x, y - 2);
                        break;
                }
            }
            else
            {
                list_path.Remove(new Vector2Int(x, y));
                if (list_path.Count > 0)
                {
                    int rnd_next = UnityEngine.Random.Range(0, list_path.Count);
                    Dig(list_path[rnd_next].x, list_path[rnd_next].y);
                }
            }
        }
    }



    //敵生成
    void Spawn_Enemy()
    {
        int x, y, num_enemy;

        //確定敵生成
        num_enemy = (int)Mathf.Round((size * size - 16 * 16) / 300);
        for (int i = 0; i < num_enemy; i++)
        {
            while (true)
            {
                x = UnityEngine.Random.Range(1, size - 1);
                y = UnityEngine.Random.Range(1, size - 1);

                if (maze[x, y] == Path && (x > 16 || y > 16))
                {
                    if(i == 0)
                    {
                        GameObject enemy = Instantiate(EnemyPrefab[0]) as GameObject;
                        enemy.transform.position = new Vector3(x, y, 0);
                        break;
                    }
                    else
                    {
                        int rnd = UnityEngine.Random.Range(0, 4);
                        GameObject enemy = Instantiate(EnemyPrefab[rnd]) as GameObject;
                        enemy.transform.position = new Vector3(x, y, 0);
                        if (rnd != 3) ResultScene.EnemyBonus += 0.01f;
                        break;
                    }   
                }
            }
        }

        //ランダム敵生成
        num_enemy = (int)Mathf.Round((size * size - 16 * 16) / 700);
        for(int i = 0; i < num_enemy; i++)
        {
            int rnd_enemy = UnityEngine.Random.Range(0, 3);
            if(rnd_enemy > TitleScene.Level % 2)
            {
                while (true)
                {
                    x = UnityEngine.Random.Range(1, size - 1);
                    y = UnityEngine.Random.Range(1, size - 1);

                    if (maze[x, y] == Path && (x > 16 || y > 16))
                    {
                        
                        int rnd = UnityEngine.Random.Range(0, 4);
                        GameObject enemy = Instantiate(EnemyPrefab[rnd]) as GameObject;
                        enemy.transform.position = new Vector3(x, y, 0);
                        if (rnd != 3) ResultScene.EnemyBonus += 0.01f;
                        break;
                        
                    }
                }
            }
        }
    }



    //ノルマ決定
    void Impose_quota()
    {
        float[] MapPerGold = new float[] {0.22f, 0.235f, 0.25f, 0.265f , 0.3f};
        int rnd_norma = UnityEngine.Random.Range(0, 100);
        int rnd_MPG = UnityEngine.Random.Range(0, 5);

        if (rnd_norma < 10) norma = (int)(size * size * MapPerGold[rnd_MPG] * 0.15);
        else if (rnd_norma < 20) norma = (int)(size * size * MapPerGold[rnd_MPG] * 0.19);
        else if (rnd_norma < 55) norma = (int)(size * size * MapPerGold[rnd_MPG] * 0.20);
        else if (rnd_norma < 65) norma = (int)(size * size * MapPerGold[rnd_MPG] * 0.21);
        else if (rnd_norma < 75) norma = (int)(size * size * MapPerGold[rnd_MPG] * 0.22);
        else if (rnd_norma < 82) norma = (int)(size * size * MapPerGold[rnd_MPG] * 0.23);
        else if (rnd_norma < 89) norma = (int)(size * size * MapPerGold[rnd_MPG] * 0.24);
        else if (rnd_norma < 94) norma = (int)(size * size * MapPerGold[rnd_MPG] * 0.25);
        else if (rnd_norma < 97) norma = (int)(size * size * MapPerGold[rnd_MPG] * 0.28);
        else if (rnd_norma < 99) norma = (int)(size * size * MapPerGold[rnd_MPG] * 0.3);
        else norma = (int)(size * size * MapPerGold[rnd_MPG] * 0.35);
    }
}