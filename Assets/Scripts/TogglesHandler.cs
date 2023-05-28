using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TogglesHandler : MonoBehaviour
{
    public GameObject DifficultyToggles;

    private void Start()
    {
        DifficultyToggles.transform.GetChild((int)Difficulty.level).GetComponent<Toggle>().isOn = true;
    }

    public void SetEasyDifficulty(bool isOn)
    {
        if (isOn) Difficulty.level = Difficulty.Difficulties.Easy;
        EnemyManager.distanceToPlayer = 3;
        EnemyManager.passRandom = 150;
    }

    public void SetMediumDifficulty(bool isOn)
    {
        if (isOn) Difficulty.level = Difficulty.Difficulties.Medium;
        EnemyManager.distanceToPlayer = 3;
        EnemyManager.passRandom = 100;
    }

    public void SetHardDifficulty(bool isOn)
    {
        if (isOn) Difficulty.level = Difficulty.Difficulties.Hard;
        EnemyManager.distanceToPlayer = 2;
        EnemyManager.passRandom = 50;
    }
}
