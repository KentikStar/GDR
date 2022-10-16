using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thorn : MonoBehaviour
{
    GameManag gameManag;

    void Start()
    {
        gameManag = Camera.main.GetComponent<GameManag>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        gameManag.EndGame();
    }
}
