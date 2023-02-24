using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{
    public GameObject owner;
    private Collider2D col;

    // Start is called before the first frame update
    void Start()
    {
        col = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (owner != null)
        {
            transform.position = new Vector3(owner.transform.position.x, owner.transform.position.y, transform.position.z);
        }           
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        col.enabled = false;
        owner = collision.gameObject;
    }
}
