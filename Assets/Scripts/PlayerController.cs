using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public int finger;
    public bool pokeOnCollider;
    private LineRenderer line;
    private Vector3 target, fingerPos;
    private bool move;
    public GameObject secondLineObject, bumpCollision;
    private BallController ballScript;
    private SecondLine secondLineVar;
    private int indexOfFinger;
    private RaycastHit2D hit;
    private Vector3 fingerPosInWorld, rayOrigin;

    [SerializeField]
    float speed;

    void Start()
    {
        line = GetComponent<LineRenderer>();
        secondLineVar = secondLineObject.GetComponent<SecondLine>();
        ballScript = GameObject.Find("Ball").GetComponent<BallController>();
    }

    private void Update()
    {
        if (pokeOnCollider)
        {
            DefineFinger();

            if (!move) DrawLine(fingerPos, line);
            else
            {
                if (secondLineVar.line.enabled == false) secondLineVar.line.enabled = true;
                DrawLine(fingerPos, secondLineVar.line);
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
                secondLineVar.line.enabled = false;
            }
        }
    }

    private void FingerPhaseEnded()
    {
        if (Input.touchCount > 0 && Input.GetTouch(indexOfFinger).phase == TouchPhase.Ended)
        {
            RaycastShoot();

            if (hit.collider != null && hit.collider.gameObject == this.gameObject)
            {
                pokeOnCollider = false;
                secondLineVar.line.enabled = false;
            }
            else
            {
                pokeOnCollider = false;
                move = true;
                target = Camera.main.ScreenToWorldPoint(Input.GetTouch(indexOfFinger).position);
                target.z = -1;
                secondLineVar.line.enabled = false;
            }
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

    private void DrawLine(Vector3 pos, LineRenderer direction)
    {
        direction.SetPosition(0, new Vector3(transform.position.x, transform.position.y, -0.5f));
        direction.SetPosition(1, new Vector3(pos.x, pos.y, -0.5f));
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (ballScript.owner == this.gameObject)
        {
            bumpCollision = collision.gameObject;
            StartCoroutine(TakeDealy());
        }
    }

    IEnumerator TakeDealy()
    {
        yield return null;
        ballScript.owner = bumpCollision;
    }
}

