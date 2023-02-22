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
    public GameObject secondLineObject;
    private SecondLine secondLineVar;
    private int indexOfFinger;

    [SerializeField]
    float speed;


    // Start is called before the first frame update
    void Start()
    {
        line = GetComponent<LineRenderer>();
        secondLineVar = secondLineObject.GetComponent<SecondLine>();
    }

    // Update is called once per frame
    private void Update()
    {
        DrawLines();
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void MovePlayer()
    {
        if (move)
        {
            transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.fixedDeltaTime);
            if (transform.position == target) move = false;
        }
    }

    private void DrawLines()
    {
        if (pokeOnCollider)
        {
            for (int i = 0; i < Input.touchCount; i++)
            {
                if (Input.touches[i].fingerId == finger)
                {
                    fingerPos = Camera.main.ScreenToWorldPoint(Input.touches[i].position);
                    indexOfFinger = i;
                }
            }

            if (!move) DrawFirstLine(fingerPos);
            else
            {
                if (secondLineVar.line.enabled == false) secondLineVar.line.enabled = true;
                DrawSecondLine(fingerPos);
            }

            if (Input.GetTouch(indexOfFinger).phase == TouchPhase.Ended)
            {
                pokeOnCollider = false;
                move = true;
                target = Camera.main.ScreenToWorldPoint(Input.GetTouch(indexOfFinger).position);
                target.z = -1;
                secondLineVar.line.enabled = false;
            }
        }

        if (move)
        {
            DrawFirstLine(target);
        }

    }

    private void DrawFirstLine(Vector3 pos)
    {
        line.SetPosition(0, new Vector3(transform.position.x, transform.position.y, -0.5f));
        pos.z = -0.5f;
        line.SetPosition(1, pos);

    }

    private void DrawSecondLine(Vector3 pos)
    {
        secondLineVar.line.SetPosition(0, new Vector3(transform.position.x, transform.position.y, -0.5f));
        pos.z = -0.5f;
        secondLineVar.line.SetPosition(1, pos);
    }


}

