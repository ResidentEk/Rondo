using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Keeper : MonoBehaviour
{
    private BallController ball;
    private EnemyManager enemyScript;
    private Vector3 target;
    public GameObject bumpCollision;
    private RaycastHit2D hit;
    [SerializeField] private LayerMask mask;
    private List<GameObject> enemiesToPass;

    void Start()
    {
        ball = GameObject.Find("Ball").GetComponent<BallController>();
        enemyScript = GameObject.Find("EnemyManager").GetComponent<EnemyManager>();
        enemiesToPass = new List<GameObject>();
    }


    void Update()
    {

    }

    private void FixedUpdate()
    {
        if (ball.owner != this.gameObject)
        {
            target = ball.target;
            target.x = transform.position.x;
            target.z = -1;

            if (ball.move)
            {
                if (ball.target.y > 0.6f) target.y = 0.6f;
                else if (ball.target.y > 0.6f) target.y = 0.6f;
            }
            else
            {
                if (ball.transform.position.y > 0.6f) target.y = 0.6f;
                else if (ball.transform.position.y < -0.6f) target.y = -0.6f;
            }
            transform.position = Vector3.MoveTowards(transform.position, target, enemyScript.speed * Time.fixedDeltaTime);
        }
        else DecideWhenToPass();
      
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (ball.owner == this.gameObject && collision.gameObject.name != "Border" && collision.gameObject.tag != this.gameObject.tag)
        {
            bumpCollision = collision.gameObject;
            StartCoroutine(TakeDealy());
        }
    }

    IEnumerator TakeDealy()
    {
        yield return null;
        ball.owner = bumpCollision;
    }


    private void DecideWhenToPass()
    {
        enemyScript.randomNumber = Random.Range(0, enemyScript.passRandom);
        if (enemyScript.randomNumber == 0)
        {
            foreach (GameObject item in enemyScript.enemyObject)
            {
                hit = Physics2D.Linecast(transform.position, item.transform.position, mask);
                if (hit.collider == null)
                {
                    enemiesToPass.Add(item);
                }

            }

            if (enemiesToPass.Count > 0)
            {
                enemyScript.randomNumber = Random.Range(0, enemiesToPass.Count);
                enemyScript.MoveTheBall(enemiesToPass[(int)enemyScript.randomNumber].transform.position, this.gameObject);
                enemiesToPass.Clear();
            }           
        }
    }
}
