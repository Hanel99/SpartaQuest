using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerObject : MonoBehaviour
{
    Zombie zombie; //이벤트 전송할 부모 좀비

    void Start()
    {
        zombie = gameObject.transform.parent.GetComponent<Zombie>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        zombie.FrontTriggerEvent(other.gameObject);
    }
}
