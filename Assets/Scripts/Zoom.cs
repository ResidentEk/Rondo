using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zoom : MonoBehaviour
{
    public SpriteRenderer field;
    private float offset;

    // Start is called before the first frame update
    void Start()
    {
        float orthoSize = field.bounds.size.x * Screen.height / Screen.safeArea.width * 0.5f;
        Camera.main.orthographicSize = orthoSize;

        offset = (Screen.safeArea.width - Screen.width) * 0.35f / 100;
        transform.position += new Vector3(offset, 0, 0);
    }


}
