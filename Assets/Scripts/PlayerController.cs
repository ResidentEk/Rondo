using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public int finger;
    public bool pokeOnCollider;
    private LineRenderer line;
    private Vector3 target;
    private bool move;

    [SerializeField]
    float speed;


    // Start is called before the first frame update
    void Start()
    {
        line = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    private void Update()
    {
        DragPlayer();

        if (pokeOnCollider && Input.GetTouch(finger).phase == TouchPhase.Ended)
        {
            pokeOnCollider = false;
            move = true;
            target = Camera.main.ScreenToWorldPoint(Input.GetTouch(finger).position);
            target.z = -1;
        }

        if (move) DrawLine(target);
    }

    private void FixedUpdate()
    {
        if (move)
        {
            transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.fixedDeltaTime);
            if (transform.position == target) move = false;
        }

    }

    private void DragPlayer()
    {
        if (pokeOnCollider)
        {
            DrawLine(Input.GetTouch(finger).position);
        }
    }
    private void DrawLine(Vector3 pos)
    {
        line.SetPosition(0, new Vector3(transform.position.x, transform.position.y, -0.5f));

        Vector3 tapVector3;

        if (move) tapVector3 = pos;
        else tapVector3 = Camera.main.ScreenToWorldPoint(pos);

        tapVector3.z = -0.5f;
        line.SetPosition(1, tapVector3);
    }




}

