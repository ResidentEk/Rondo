using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
    private BallController ballScript;
    private Vector3 fingerPosInWorld, rayOrigin;
    private RaycastHit2D hit;

    // Start is called before the first frame update
    void Start()
    {
        ballScript = GameObject.Find("Ball").GetComponent<BallController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount > 0)
        {
            foreach (Touch tap in Input.touches)
            {
                if (tap.phase == TouchPhase.Began)
                {
                    RaycastShoot(tap);
                    
                    if (hit.collider != null)
                    {
                        hit.transform.gameObject.GetComponent<PlayerController>().finger = tap.fingerId;
                        hit.transform.gameObject.GetComponent<PlayerController>().pokeOnCollider = true;
                        if (ballScript.owner != null) ballScript.passOrMove = true;
                    }

                    if (ballScript.owner != null && !ballScript.trajectory)
                    {
                        ballScript.finger = tap.fingerId;
                        ballScript.trajectory = true;
                    }
                }
            }
        }
    }

    private void RaycastShoot(Touch tap)
    {
        fingerPosInWorld = Camera.main.ScreenToWorldPoint(tap.position);
        fingerPosInWorld.z = 0;
        rayOrigin = fingerPosInWorld;
        rayOrigin.z = -5;
        hit = Physics2D.Raycast(rayOrigin, fingerPosInWorld - rayOrigin);
    }

}
