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

    // Start is called before the first frame update
    void Start()
    {
        ball = GameObject.Find("Ball").GetComponent<BallController>();
        step = 1;
    }

    // Update is called once per frame
    void Update()
    {
        switch (step)
        {
            case 1:
                if (ball.owner == blue)
                {
                    dialogueBox.text = "Отлично! Вы завладели мячом!";
                    Invoke("TakeAway", 3);
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
    }


}
