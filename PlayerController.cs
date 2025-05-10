using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Rigidbody2D rigid2D;
    float speed = 4.5f;
    Vector2[] vel;
    public int dire;
    static public int purify;

    public GameObject PurifyPrefab;

    void Start()
    {
        this.rigid2D = GetComponent<Rigidbody2D>();
        vel = new[] { new Vector2(0, 0), new Vector2(speed, 0), new Vector2(-speed, 0), new Vector2(0, speed), new Vector2(0, -speed) };
    }

    void Update()
    {
        //移動
        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        {
            dire = 1;
        }
        else if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
        {
            dire = 2;
        }
        else if (Input.GetKey(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
        {
            dire = 3;
        }
        else if (Input.GetKey(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
        {
            dire = 4;
        }
        if (dire <= 2)
        {
            if (Math.Round(transform.position.y) - 0.1 <= transform.position.y && transform.position.y <= Math.Round(transform.position.y) + 0.1)
            {
                rigid2D.linearVelocity = vel[dire];
            }
        }
        else
        {
            if (Math.Round(transform.position.x) - 0.1 <= transform.position.x && transform.position.x <= Math.Round(transform.position.x) + 0.1)
            {
                rigid2D.linearVelocity = vel[dire];
            }
        }

        //アイテム使用
        if(Input.GetKeyDown(KeyCode.Space) && purify > 0)
        {
            StartCoroutine(use_Purify());
            purify -= 1;
        }
    }


    IEnumerator use_Purify()
    {
        GameObject Purify = Instantiate(PurifyPrefab) as GameObject;
        Purify.transform.position = new Vector3(transform.position.x, transform.position.y, 0);
        yield return new WaitForSeconds(0.25f);
        Destroy(Purify);
    }
}
