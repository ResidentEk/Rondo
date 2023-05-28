using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Quit : MonoBehaviour
{
    public void QuitGame()
    {
        BallController.blueScore = 0;
        BallController.redScore = 0;
        SceneManager.LoadScene("Menu");
    }
}
