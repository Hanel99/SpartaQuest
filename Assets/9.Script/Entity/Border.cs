using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Border : MonoBehaviour
{
    BoxCollider2D boxCollider2D;

    void Start()
    {
        boxCollider2D = gameObject.GetComponent<BoxCollider2D>();
    }

    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Destroy(other.gameObject);
    }
}
