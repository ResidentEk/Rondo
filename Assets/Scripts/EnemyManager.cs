using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyManager : MonoBehaviour
{
    public GameObject[] enemyObject, playerObject;
    public BallController ball;
    public float speed;
    private float min, rotationDirection, angle;
    public float randomNumber;
    private Vector3 goalTarget, target;
    private RaycastHit2D hit;
    [SerializeField] private LayerMask mask;
    [SerializeField] private float distanceToPlayer;
    private List<GameObject> enemiesToPass;
    private Dictionary<GameObject, float> distanceBallEnemy = new Dictionary<GameObject, float>();
    private Dictionary<GameObject, float> distanceOwnerPlayer = new Dictionary<GameObject, float>();
    private Dictionary<GameObject, Vector2> directions = new Dictionary<GameObject, Vector2>();
    private GameObject closerToBallEnemy, closerToOwnerPlayer;

    public int passRandom, movingRandom;
    [SerializeField] private float movingLimit, hitLimit;

    void Start()
    {
        goalTarget = new Vector3(7.9f, 0, -2);
        enemiesToPass = new List<GameObject>();
        rotationDirection = 1;

        for (int i = 1; i < 5; i++)
        {
            distanceBallEnemy.Add(GameObject.Find("Red team").transform.GetChild(i).gameObject, 0);
            directions.Add(GameObject.Find("Red team").transform.GetChild(i).gameObject, new Vector2(0, 0));
        }

        for (int i = 0; i < 5; i++)
        {
            distanceOwnerPlayer.Add(GameObject.Find("Blue team").transform.GetChild(i).gameObject, 0);
        }
    }


    private void FixedUpdate()
    {
        if (ball.owner == null || (ball.owner != null && ball.owner.CompareTag("Blue"))) GetCloserToBallEnemy();

        if (ball.possession) MovingWhenFree();
        else MovingWhenOwns();
    }

    private void MovingWhenOwns()
    {
        foreach (GameObject enemy in enemyObject)
        {
            if (enemy != ball.owner) MovingFreeEnemiesWhenPosession(enemy);
            else
            {
                if (enemy.transform.position.x > hitLimit)
                {
                    hit = Physics2D.Linecast(enemy.transform.position, goalTarget, mask);
                    if (hit.collider == null) MoveTheBall(goalTarget, enemy);
                }

                GetCloserToOwnerPlayer(enemy);
                DecideIfOwnerNearPlayer(enemy);
                DecideWhenToPass(enemy);
            }
        }
    }

    public void DecideWhenToPass(GameObject enemyOwner)
    {
        randomNumber = Random.Range(0, passRandom);
        if (randomNumber == 0)
        {
            foreach (GameObject unit in enemyObject)
            {
                if (unit != enemyOwner)
                {
                    hit = Physics2D.Linecast(unit.transform.position, enemyOwner.transform.position, mask);
                    if (hit.collider == null)
                    {
                        enemiesToPass.Add(unit);
                    }
                }
            }

            if (enemiesToPass.Count > 0)
            {
                randomNumber = Random.Range(0, enemiesToPass.Count);
                target = enemiesToPass[(int)randomNumber].transform.position;
                target.z = -2;
                enemiesToPass.Clear();
                MoveTheBall(target, enemyOwner);
            }
        }
    }

    private void DecideIfOwnerNearPlayer(GameObject enemy)
    {
        if (Vector2.Distance(closerToOwnerPlayer.transform.position, enemy.transform.position) < 1)
        {
            directions[enemy] = enemy.transform.position - closerToOwnerPlayer.transform.position;
            enemy.transform.Translate(directions[enemy].normalized * speed * Time.fixedDeltaTime);
        }
        else MovingEnemyWhenPosession(enemy);
    }

    private void MovingFreeEnemiesWhenPosession(GameObject enemy)
    {
        hit = Physics2D.Linecast(enemy.transform.position, ball.transform.position, mask);
        if (hit.collider != null) MoveCircleTrajectory(enemy);
        else MovingEnemyWhenPosession(enemy);
    }

    private void MovingEnemyWhenPosession(GameObject enemy)
    {
        randomNumber = Random.Range(0, movingRandom);
        if (randomNumber == 0)
        {
            if (enemy.transform.position.x < movingLimit)
            {
                randomNumber = Random.Range(-10, 10) / 10f;
                directions[enemy] = new Vector2(1, randomNumber);
            }
            else
            {
                randomNumber = Random.Range(-10, 10) / 10f;
                directions[enemy] = new Vector2(randomNumber, 0);

                randomNumber = Random.Range(-10, 10) / 10f;
                directions[enemy] += new Vector2(0, randomNumber);
            }
        }

        enemy.transform.Translate(directions[enemy].normalized * speed * Time.fixedDeltaTime);
    }

    private void MoveCircleTrajectory(GameObject enemy)
    {
        randomNumber = Random.Range(0, movingRandom);
        if (randomNumber == 0)
        {
            randomNumber = Random.Range(0, 2);
            if (randomNumber == 0) rotationDirection = 1;
            else rotationDirection = -1;

            angle = speed * Time.fixedDeltaTime * rotationDirection / Vector3.Distance(enemy.transform.position, ball.transform.position);
            angle *= 180 / 3.14f;
        }
        enemy.transform.RotateAround(ball.transform.position, Vector3.forward, angle);
    }

    public void MoveTheBall(Vector3 aim, GameObject lastOwner)
    {
        ball.lastOwner = lastOwner;
        ball.col.enabled = true;
        ball.owner = null;
        ball.move = true;
        ball.trajectory = false;
        ball.target = aim;
    }

    private void GetCloserToOwnerPlayer(GameObject enemy)
    {
        min = 20;
        foreach (GameObject item in playerObject)
        {
            distanceOwnerPlayer[item] = Vector3.Magnitude(item.transform.position - enemy.transform.position);
            if (distanceOwnerPlayer[item] < min)
            {
                min = distanceOwnerPlayer[item];
                closerToOwnerPlayer = item;
            }
        }
    }

    private void GetCloserToBallEnemy()
    {
        min = 20;
        foreach (GameObject item in enemyObject)
        {
            distanceBallEnemy[item] = Vector3.Magnitude(item.transform.position - ball.transform.position);
            if (distanceBallEnemy[item] < min)
            {
                min = distanceBallEnemy[item];
                closerToBallEnemy = item;
            }
        }
    }

    private void MovingWhenFree()
    {
        target = ball.transform.position;
        target.z = -1;

        closerToBallEnemy.transform.position = Vector3.MoveTowards(closerToBallEnemy.transform.position, target, speed * Time.fixedDeltaTime);

        for (int i = 0; i < enemyObject.Length; i++)
        {
            if (enemyObject[i] != closerToBallEnemy)
            {
                target = playerObject[i].transform.position;
                target.x -= distanceToPlayer;
                enemyObject[i].transform.position = Vector3.MoveTowards(enemyObject[i].transform.position, target, speed * Time.fixedDeltaTime);
            }
        }
    }

}
