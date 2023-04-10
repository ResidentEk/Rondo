using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{
    public GameObject owner;
    public Collider2D col;
    private LineRenderer line;
    public int finger, indexOfFinger;
    public bool trajectory, passOrMove;
    public Vector3 target, fingerPos;
    public bool move, outOfBounds;
    public GameObject lastOwner;
    private RaycastHit2D hit;
    private Vector2 hitPoint;
    public bool possession;

    [SerializeField]
    float speed;
    [SerializeField]
    private LayerMask mask;

    void Start()
    {
        col = GetComponent<Collider2D>();
        line = GetComponent<LineRenderer>();
        possession = true;
    }

    void Update()
    {
        if (owner != null && owner.CompareTag("Blue")) possession = true;
        else if (owner != null && owner.CompareTag("Red")) possession = false;
        else if(!move) possession = true; 

        if (owner != null)
        {
            transform.position = new Vector3(owner.transform.position.x, owner.transform.position.y, transform.position.z);

            if (trajectory && Input.touchCount > 0)
            {
                DefineFinger();
                if (line.enabled == false) line.enabled = true;
                if (Input.GetTouch(indexOfFinger).phase == TouchPhase.Ended) TouchEnded();

                hit = Physics2D.Linecast(transform.position, fingerPos, mask);
                if (hit.collider != null)
                {
                    outOfBounds = true;
                    DrawLine(hit.point);
                    hitPoint = hit.point;
                }
                else
                {
                    DrawLine(fingerPos);
                    outOfBounds = false;
                }
            }

            if (passOrMove && Input.touchCount > 0) DefinePassOrMove();
        }

        if (move) DrawLine(target);
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
        if (outOfBounds)
        {
            target = hitPoint;
            outOfBounds = false;
        }
        else target = Camera.main.ScreenToWorldPoint(Input.GetTouch(indexOfFinger).position);
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
        fingerPos = Camera.main.ScreenToWorldPoint(Input.GetTouch(indexOfFinger).position);
        hit = Physics2D.Raycast(fingerPos, Vector3.forward);
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
