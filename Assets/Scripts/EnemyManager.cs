using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyManager : MonoBehaviour
{
    public GameObject[] enemyObject, playerObject;
    private BallController ball;
    public float speed;
    private float[] distances, distancesToOwner;
    private float min, radius;
    public float randomNumber;
    private int indexOfEnemy, indexOfPlayer;
    private Vector3 direction, goalTarget, coord, target;
    private RaycastHit2D hit;
    [SerializeField] private LayerMask mask;
    [SerializeField] private float distanceToPlayer;
    private Vector2[] directions;
    private List<GameObject> enemiesToPass;

    public int passRandom, movingRandom;
    [SerializeField] private float movingLimit, hitLimit;

    void Start()
    {
        ball = GameObject.Find("Ball").GetComponent<BallController>();
        distances = new float[4];
        distancesToOwner = new float[5];
        goalTarget = new Vector3(7.9f, 0, -2);
        directions = new Vector2[4];
        enemiesToPass = new List<GameObject>();
    }


    private void FixedUpdate()
    {
        if (ball.owner == null || (ball.owner != null && ball.owner.CompareTag("Blue"))) GetCloserToBallEnemy();

        if (ball.possession) MovingWhenFree();
        else MovingWhenOwns();
    }

    private void MovingWhenOwns()
    {
        for (int i = 0; i < enemyObject.Length; i++)
        {
            if (enemyObject[i] != ball.owner) MovingFreeEnemiesWhenOwns(i);
            else
            {
                GetCloserToOwnerPlayer();
                DecideIfOwnerNearPlayer(i);
                DecideWhenToPass(i);

                if (enemyObject[i].transform.position.x > hitLimit)
                {
                    coord = goalTarget;
                    hit = Physics2D.Linecast(enemyObject[i].transform.position, coord, mask);
                    if (hit.collider == null) MoveTheBall(goalTarget, enemyObject[i]);
                }


            }
        }
    }

    private void DecideWhenToPass(int index)
    {
        randomNumber = Random.Range(0, passRandom);
        if (randomNumber == 0)
        {
            foreach (GameObject item in enemyObject)
            {
                if (item != enemyObject[index])
                {
                    hit = Physics2D.Linecast(enemyObject[index].transform.position, item.transform.position, mask);
                    if (hit.collider == null)
                    {
                        enemiesToPass.Add(item);
                    }
                }
            }

            if (enemiesToPass.Count > 0)
            {
                randomNumber = Random.Range(0, enemiesToPass.Count);
                coord = enemiesToPass[(int)randomNumber].transform.position;
                coord.z = -2;
                MoveTheBall(coord, enemyObject[index]);
                enemiesToPass.Clear();
            }
        }
    }

    private void DecideIfOwnerNearPlayer(int index)
    {
        if (distancesToOwner[indexOfPlayer] < 1)
        {
            direction = enemyObject[index].transform.position - playerObject[indexOfPlayer].transform.position;
            enemyObject[index].transform.Translate(direction.normalized * speed * Time.fixedDeltaTime);
        }
        else MovingEnemyWithoutBall(index);
    }

    private void MovingFreeEnemiesWhenOwns(int index)
    {
        coord = ball.transform.position;
        coord.z = -1;

        hit = Physics2D.Linecast(enemyObject[index].transform.position, coord, mask);
        if (hit.collider != null) MoveCircleTrajectory(enemyObject[index], index);
        else MovingEnemyWithoutBall(index);
    }

    private void MovingEnemyWithoutBall(int index)
    {
        randomNumber = Random.Range(0, movingRandom);
        if (randomNumber == 0)
        {
            if (enemyObject[index].transform.position.x < movingLimit)
            {
                directions[index].x = 1;
            }
            else
            {
                randomNumber = Random.Range(-100, 100);
                randomNumber /= 100;
                directions[index].x = randomNumber;
            }

            randomNumber = Random.Range(-100, 100);
            randomNumber /= 100;
            directions[index].y = randomNumber;
        }
        enemyObject[index].transform.Translate(directions[index].normalized * speed * Time.fixedDeltaTime);
    }

    private void MoveCircleTrajectory(GameObject unit, int index)
    {
        randomNumber = Random.Range(0, movingRandom);
        if (randomNumber == 0)
        {
            radius = Vector3.Distance(unit.transform.position, coord);

            randomNumber = Random.Range(0, 2);
            if (randomNumber == 0)
            {
                directions[index].x = -(coord.y - unit.transform.position.y) / radius;
                directions[index].y = (coord.x - unit.transform.position.x) / radius;
            }
            else
            {
                directions[index].x = (coord.y - unit.transform.position.y) / radius;
                directions[index].y = -(coord.x - unit.transform.position.x) / radius;
            }
        }

        unit.transform.Translate(directions[index] * speed * Time.fixedDeltaTime);

    }

    public void MoveTheBall(Vector3 aim, GameObject hero)
    {
        ball.lastOwner = hero;
        ball.col.enabled = true;
        ball.owner = null;
        ball.move = true;
        ball.trajectory = false;
        ball.target = aim;
    }

    private void GetCloserToOwnerPlayer()
    {
        for (int i = 0; i < distancesToOwner.Length; i++)
        {
            distancesToOwner[i] = Vector3.Magnitude(playerObject[i].transform.position - ball.owner.transform.position);
        }

        min = distancesToOwner[0];

        foreach (float item in distancesToOwner)
        {
            if (item < min) min = item;
        }

        indexOfPlayer = (int)Array.IndexOf(distancesToOwner, min);
    }

    private void GetCloserToBallEnemy()
    {
        for (int i = 0; i < distances.Length; i++)
        {
            coord = ball.transform.position;
            coord.z = -1;

            distances[i] = Vector3.Magnitude(enemyObject[i].transform.position - coord);
        }

        min = distances[0];

        foreach (float item in distances)
        {
            if (item < min) min = item;
        }

        indexOfEnemy = (int)Array.IndexOf(distances, min);
    }

    private void MovingWhenFree()
    {
        coord = ball.transform.position;
        coord.z = -1;

        enemyObject[indexOfEnemy].transform.position = Vector3.MoveTowards(enemyObject[indexOfEnemy].transform.position, coord, speed * Time.fixedDeltaTime);

        for (int i = 0; i < enemyObject.Length; i++)
        {
            if (i != indexOfEnemy)
            {
                target = playerObject[i].transform.position;
                target.x -= distanceToPlayer;
                enemyObject[i].transform.position = Vector3.MoveTowards(enemyObject[i].transform.position, target, speed * Time.fixedDeltaTime);
            }
        }
    }

}
