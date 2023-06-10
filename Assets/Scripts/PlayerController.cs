using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public int finger;
    public bool pokeOnCollider;
    private LineRenderer line;
    private Vector3 target, fingerPos;
    private bool move, outOfBounds;
    public GameObject secondLineObject, bumpCollision;
    private BallController ball;
    private SecondLine secondLine;
    private int indexOfFinger;
    private RaycastHit2D hit;
    private Vector2 hitPoint;

    [SerializeField]
    private float speed;
    [SerializeField]
    private LayerMask mask;


    void Start()
    {
        line = GetComponent<LineRenderer>();
        secondLine = secondLineObject.GetComponent<SecondLine>();
        ball = GameObject.Find("Ball").GetComponent<BallController>();
    }

    private void Update()
    {
        if (pokeOnCollider)
        {
            DefineFinger();

            if (!move) LinecastShoot(line);
            else
            {
                if (secondLine.line.enabled == false) secondLine.line.enabled = true;
                LinecastShoot(secondLine.line);
            }

            FingerPhaseEnded();
        }

        if (move) DrawLine(target, line);
    }



    private void FixedUpdate()
    {
        if (move)
        {
            transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.fixedDeltaTime);
            if (transform.position == target)
            {
                move = false;
                secondLine.line.enabled = false;
            }
        }

    }

    private void LinecastShoot(LineRenderer segment)
    {
        hit = Physics2D.Linecast(transform.position, fingerPos, mask);
        if (hit.collider != null)
        {
            DrawLine(hit.point, segment);
            hitPoint = hit.point;
            outOfBounds = true;
        }
        else
        {
            DrawLine(fingerPos, segment);
            outOfBounds = false;
        }

    }

    private void FingerPhaseEnded()
    {
        if (Input.GetTouch(indexOfFinger).phase == TouchPhase.Ended)
        {
            hit = Physics2D.Raycast(fingerPos, Vector3.forward);

            if (hit.collider != null && hit.collider.gameObject != this.gameObject || hit.collider == null)
            {
                if (outOfBounds) target = hitPoint;
                else target = Camera.main.ScreenToWorldPoint(Input.GetTouch(indexOfFinger).position);

                move = true;
                target.z = -1;
            }

            secondLine.line.enabled = false;
            pokeOnCollider = false;
        }
    }

    private void DefineFinger()
    {
        for (int i = 0; i < Input.touchCount; i++)
        {
            if (Input.touches[i].fingerId == finger)
            {
                fingerPos = Camera.main.ScreenToWorldPoint(Input.touches[i].position);
                indexOfFinger = i;
                return;
            }
        }
    }

    private void DrawLine(Vector3 pos, LineRenderer objectDraw)
    {
        objectDraw.SetPosition(0, new Vector3(transform.position.x, transform.position.y, -0.5f));
        objectDraw.SetPosition(1, new Vector3(pos.x, pos.y, -0.5f));
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (ball.owner == this.gameObject && collision.gameObject.CompareTag("Red"))
        {
            bumpCollision = collision.gameObject;
            StartCoroutine(TakeDealy());
        }
    }

    IEnumerator TakeDealy()
    {
        yield return null;
        ball.owner = bumpCollision;
        ball.possession = false;
        ball.trajectory = false;
        ball.line.enabled = false;
    }
}

