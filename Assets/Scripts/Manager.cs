using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
    

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        CheckWhichPlayerTap();
    }

    void CheckWhichPlayerTap()
    {
        if (Input.touchCount > 0)
        {
            foreach (Touch tap in Input.touches)
            {
                if (tap.phase == TouchPhase.Began)
                {
                    Vector3 fingerPosInWorld = Camera.main.ScreenToWorldPoint(tap.position);
                    fingerPosInWorld.z = -1;
                    Vector3 rayOrigin = fingerPosInWorld;
                    rayOrigin.z = -5;
                    RaycastHit2D hit = Physics2D.Raycast(rayOrigin, fingerPosInWorld - rayOrigin);
                    if (hit.collider != null)
                    {
                        hit.transform.gameObject.GetComponent<PlayerController>().finger = tap.fingerId;
                        hit.transform.gameObject.GetComponent<PlayerController>().pokeOnCollider = true;
                    }
                }
            }
        }
    }
}
