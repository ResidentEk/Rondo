using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Tutorial : MonoBehaviour
{
    private BallController ball;
    public GameObject blue, blue2, red;
    public TextMeshProUGUI dialogueBox;
    private int step;


    void Start()
    {
        ball = GameObject.Find("Ball").GetComponent<BallController>();
        ball.transform.position = new Vector3(-2.2f, 1.65f, -2);
        step = 1;
    }


    void Update()
    {

        switch (step)
        {
            case 1:
                if (ball.owner == blue)
                {
                    dialogueBox.text = "Отлично! Вы завладели мячом!";
                    Invoke("TakeAway", 2);
                }
                break;

            case 2:
                if (ball.owner == blue)
                {
                    dialogueBox.text = "Прекрасно! Чтобы перместить мяч, коснитесь любой области поля. Забейте гол, чтобы начать обучение заново.";
                    Invoke("MoveTheBall", 2);
                }
                break;
        }

    }

    private void TakeAway()
    {
        step = 2;
        dialogueBox.text = "Чтобы отобрать мяч, коснитесь синим игроком красного";
        red.SetActive(true);
        ball.owner = red;

    }

    private void MoveTheBall()
    {
        blue2.SetActive(true);
        step = 3;
    }


}
