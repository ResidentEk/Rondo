using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{
    public GameObject owner;
    private Collider2D col;
    private LineRenderer line;
    public int finger, indexOfFinger;
    public bool trajectory;
    private Vector3 fingerPos, target, fingerPosInWorld, rayOrigin;
    private bool move;
    private GameObject lastOwner;
    public bool passOrMove;
    private RaycastHit2D hit;

    [SerializeField]
    float speed;

    void Start()
    {
        col = GetComponent<Collider2D>();
        line = GetComponent<LineRenderer>();
    }

    void Update()
    {
        if (owner != null)
        {
            transform.position = new Vector3(owner.transform.position.x, owner.transform.position.y, transform.position.z);

            if (trajectory && Input.touchCount > 0) DrawTrajectory();

            if (passOrMove && Input.touchCount > 0) DefinePassOrMove();
        }

        if (move) DrawLine(target);
    }

    private void DrawTrajectory()
    {
        DefineFinger();

        if (line.enabled == false) line.enabled = true;

        DrawLine(fingerPos);

        if (Input.GetTouch(indexOfFinger).phase == TouchPhase.Ended) TouchEnded();

    }

    private void DefineFinger()
    {
        for (int i = 0; i < Input.touchCount; i++)
        {
            if (Input.touches[i].fingerId == finger)
            {
                fingerPos = Camera.main.ScreenToWorldPoint(Input.touches[i].position);
                indexOfFinger = i;
                i = Input.touchCount;
            }
        }
    }

    private void DrawLine(Vector3 point)
    {
        line.SetPosition(0, new Vector3(transform.position.x, transform.position.y, -0.5f));
        line.SetPosition(1, new Vector3(point.x, point.y, -0.5f));
    }

    private void TouchEnded()
    {
        lastOwner = owner;
        col.enabled = true;
        owner = null;
        move = true;
        trajectory = false;
        target = Camera.main.ScreenToWorldPoint(Input.GetTouch(indexOfFinger).position);
        target.z = -2;
    }

    private void DefinePassOrMove()
    {
        RaycastShoot();

        if (hit.collider == null || hit.collider.gameObject == owner)
        {
            passOrMove = false;
            trajectory = false;
            line.enabled = false;
        }
    }





    private void RaycastShoot()
    {
        fingerPosInWorld = Camera.main.ScreenToWorldPoint(Input.GetTouch(indexOfFinger).position);
        fingerPosInWorld.z = 0;
        rayOrigin = fingerPosInWorld;
        rayOrigin.z = -5;
        hit = Physics2D.Raycast(rayOrigin, fingerPosInWorld - rayOrigin);
    }



    private void FixedUpdate()
    {
        if (move)
        {
            transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.fixedDeltaTime);
            if (transform.position == target)
            {
                move = false;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject != lastOwner)
        {
            col.enabled = false;
            owner = collision.gameObject;
            line.enabled = false;
            move = false;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject == lastOwner)
        {
            lastOwner = null;
        }
    }
}
