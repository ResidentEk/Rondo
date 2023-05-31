using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
    private BallController ball;
    private Vector3 fingerPos;
    private RaycastHit2D hit;
    private Dictionary<GameObject, PlayerController> playerScript = new Dictionary<GameObject, PlayerController>();
    private Transform players;
    public GameObject pauseMenu;

    void Start()
    {
        ball = GameObject.Find("Ball").GetComponent<BallController>();
        players = GameObject.Find("Blue team").transform;

        for (int i = 0; i < 5; i++)
        {
            playerScript.Add(players.GetChild(i).gameObject, players.GetChild(i).GetComponent<PlayerController>());
        }

    }


    void Update()
    {
        if (Input.touchCount > 0 && !pauseMenu.activeSelf)
        {
            foreach (Touch tap in Input.touches)
            {
                if (tap.phase == TouchPhase.Began)
                {
                    RaycastShoot(tap);

                    if (hit.collider != null && hit.collider.CompareTag("Blue"))
                    {
                        playerScript[hit.collider.gameObject].finger = tap.fingerId;
                        playerScript[hit.collider.gameObject].pokeOnCollider = true;
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
