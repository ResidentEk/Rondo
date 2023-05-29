using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
    private BallController ball;
    private Vector3 fingerPos;
    private RaycastHit2D hit;


    void Start()
    {
        ball = GameObject.Find("Ball").GetComponent<BallController>();
    }


    void Update()
    {
        if (Input.touchCount > 0)
        {
            foreach (Touch tap in Input.touches)
            {
                if (tap.phase == TouchPhase.Began)
                {
                    RaycastShoot(tap);

                    if (hit.collider != null && hit.collider.CompareTag("Blue"))
                    {
                        hit.transform.gameObject.GetComponent<PlayerController>().finger = tap.fingerId;
                        hit.transform.gameObject.GetComponent<PlayerController>().pokeOnCollider = true;
                        if (ball.owner != null) ball.passOrMove = true;
                    }

                    if (ball.owner != null && ball.owner.CompareTag("Blue") && !ball.trajectory)
                    {
                        if (hit.collider == null || !hit.collider.CompareTag("Button"))
                        {
                            ball.finger = tap.fingerId;
                            ball.trajectory = true;
                        }                 
                    }
                }
            }
        }
    }

    private void RaycastShoot(Touch tap)
    {
        fingerPos = Camera.main.ScreenToWorldPoint(tap.position);
        hit = Physics2D.Raycast(fingerPos, Vector3.forward);
    }

}
