using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class BallController : MonoBehaviour
{
    public GameObject owner;
    public Collider2D col;
    public LineRenderer line;
    public int finger, indexOfFinger;
    public bool trajectory, passOrMove;
    public Vector3 target, fingerPos;
    public bool move, outOfBounds;
    public GameObject lastOwner;
    private RaycastHit2D hit;
    private Vector2 hitPoint;
    public bool possession;
    public GameObject blueGameObject, redGameObject;
    private TextMeshProUGUI blueText, redText;
    public static int blueScore = 0, redScore = 0;
    private bool goal;

    [SerializeField]
    float speed;
    [SerializeField]
    private LayerMask mask;

    void Start()
    {
        col = GetComponent<Collider2D>();
        line = GetComponent<LineRenderer>();
        possession = true;
        blueText = blueGameObject.GetComponent<TextMeshProUGUI>();
        redText = redGameObject.GetComponent<TextMeshProUGUI>();
        blueText.text = blueScore.ToString();
        redText.text = redScore.ToString();
    }

    void Update()
    {
        CheckIfGoal();

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

                if (passOrMove && Input.touchCount > 0) DefinePassOrMove();
            }
        }
        else if (move) DrawLine(target);
    }

    private void CheckIfGoal()
    {
        if (transform.position.x < -7.9f && !goal)
        {
            Invoke("RestartScene", 2);
            blueScore = int.Parse(blueText.text);
            blueScore++;
            blueText.text = blueScore.ToString();
            goal = true;
        }
        else if (transform.position.x > 7.26 && !goal)
        {
            Invoke("RestartScene", 2);
            redScore = int.Parse(redText.text);
            redScore++;
            redText.text = redScore.ToString();
            goal = true;
        }
    }

    private void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
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
                possession = true;
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
            passOrMove = false;

            if (collision.gameObject.CompareTag("Blue")) possession = true;
            else possession = false;
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
