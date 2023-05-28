using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Difficulty : MonoBehaviour
{
    public enum Difficulties {Easy, Medium, Hard };

    public static Difficulties level = Difficulties.Easy;
}
