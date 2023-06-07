using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zoom : MonoBehaviour
{
    public SpriteRenderer field;
    // Start is called before the first frame update
    void Start()
    {
        float orthoSize = field.bounds.size.x * Screen.height / Screen.safeArea.width * 0.5f;
        Camera.main.orthographicSize = orthoSize;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
