using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Keeper : MonoBehaviour
{
    private BallController ball;
    private EnemyManager enemyScript;
    private Vector3 target;
    public GameObject bumpCollision;

    void Start()
    {
        ball = GameObject.Find("Ball").GetComponent<BallController>();
        enemyScript = GameObject.Find("EnemyManager").GetComponent<EnemyManager>();
    }

    private void FixedUpdate()
    {
        if (ball.owner != this.gameObject)
        {
            target = transform.position;
            if (ball.move)
            {
                if (ball.target.y > 0.8f) target.y = 0.8f;
                else if (ball.target.y < -0.8f) target.y = -0.8f;
                else target.y = ball.target.y;
            }
            else
            {
                if (ball.transform.position.y > 0.8f) target.y = 0.8f;
                else if (ball.transform.position.y < -0.8f) target.y = -0.8f;
                else target.y = ball.transform.position.y;
            }
            transform.position = Vector3.MoveTowards(transform.position, target, enemyScript.speed * Time.fixedDeltaTime);
        }
        else enemyScript.DecideWhenToPass(this.gameObject);
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

        if (ball.owner.CompareTag("Blue")) ball.possession = true;
        else ball.possession = false;
    }

}
