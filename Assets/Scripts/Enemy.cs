using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private BallController ball;
    public GameObject bumpCollision;


    void Start()
    {
        ball = GameObject.Find("Ball").GetComponent<BallController>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (ball.owner == this.gameObject && collision.gameObject.CompareTag("Blue"))
        {
            bumpCollision = collision.gameObject;
            StartCoroutine(TakeDealy());
        }
    }

    IEnumerator TakeDealy()
    {
        yield return null;
        ball.owner = bumpCollision;
        ball.possession = true;
        ball.passOrMove = false;
    }

}
