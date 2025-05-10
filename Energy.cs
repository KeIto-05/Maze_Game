using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Energy : MonoBehaviour
{
    Rigidbody2D rigid2D;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "player")
        {
            PlayerController.purify++;
            Destroy(gameObject);
        }
    }
}
