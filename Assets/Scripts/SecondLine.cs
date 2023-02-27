using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecondLine : MonoBehaviour
{
    public LineRenderer line;

    // Start is called before the first frame update
    void Start()
    {
        line = GetComponent<LineRenderer>();
    }

}
