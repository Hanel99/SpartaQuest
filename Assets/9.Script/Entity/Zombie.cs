using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : MonoBehaviour
{

    public float hp = 100f;
    public float attackDamage = 10f;
    public ZombieState zombieState = ZombieState.Move;


    public float speed;
    public float jumpPower;


    // private
    Rigidbody2D rb;
    Collider2D frontCollider;
    Collider2D upCollider;


    float realSpeed = 0f;
    Vector3 beforePosition;
    Coroutine stateChangeCoroutine;
    float tempTime = 0f;
    float checkTime = 0.5f;







    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        frontCollider = transform.Find("Front").GetComponent<BoxCollider2D>();
        // upCollider = transform.Find("Up").GetComponent<BoxCollider2D>();
        beforePosition = transform.position;
        stateChangeCoroutine = null;
    }

    void Update()
    {
        tempTime += Time.deltaTime;
        if (tempTime > checkTime)
        {
            tempTime = 0f;
            CheckTrigger();
            // UpdateSpeed();
        }


        if (zombieState == ZombieState.Move)
        {
            // CheckMovement();
            // beforePosition = transform.position;

            // Jump();
            // UpdateSpeed();
            // CheckForZombieInFront();
        }
    }


    void CheckTrigger()
    {
        // Debug.Log("@@@ Check trigger");
        StartCoroutine(Co_CheckTrigger());
    }

    IEnumerator Co_CheckTrigger()
    {
        frontCollider.gameObject.SetActive(false);
        yield return new WaitForSeconds(0.1f);
        frontCollider.gameObject.SetActive(true);
    }

    public void FrontTriggerEvent(GameObject obj)
    {
        if (obj.CompareTag("Zombie"))
        {
            Jump();
        }
    }

    // void UpdateSpeed()
    // {
    //     realSpeed = Mathf.Abs(transform.position.x - beforePosition.x);
    //     beforePosition = transform.position;

    //     Debug.Log($"{this.name} speed : {realSpeed}");

    //     if (realSpeed < 1f)
    //     {
    //         Jump();
    //     }
    // }

    // void CheckForZombieInFront()
    // {
    //     RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.left, 1f);
    //     if (hit.collider != null && hit.collider.name.Contains("Zombie"))
    //     {
    //         Jump();
    //     }
    // }

    void OnDestroy()
    {
        InGameManager.instance.zombies.Remove(this);
    }



    private void FixedUpdate()
    {
        rb.velocity = new Vector2(-1 * speed, rb.velocity.y);

        // Debug.Log($"@@@ {this.name} localposition y : {transform.localPosition.y} -> {1 + 7 * (transform.localPosition.y + 3.39f)}");
        rb.mass = Mathf.Max(1f, 1f + 20f * (transform.localPosition.y + 3.39f));
        jumpPower = Mathf.Max(450f, 450f + 8000f * (transform.localPosition.y + 3.39f));
    }
    void Jump()
    {
        if (zombieState != ZombieState.Move)
            return;

        zombieState = ZombieState.Jump;
        rb.AddForce(Vector2.up * jumpPower);

        // speed = 3f;
        // rb.mass = 1f;
        StateChange(ZombieState.Move, 1f);
    }


    void StateChange(ZombieState afterState, float duration)
    {
        stateChangeCoroutine = StartCoroutine(Co_StateChange(ZombieState.Move, 1f));
    }
    IEnumerator Co_StateChange(ZombieState afterState, float duration)
    {
        yield return new WaitForSeconds(duration);
        zombieState = afterState;
        // speed = 3f;
        // rb.mass = 5f;
    }



    void OnTriggerEnter2D(Collider2D collision)
    {
        // Debug.Log($"@@@ {this.name} trigger enter -> {collision.name}");
        if (collision.gameObject.CompareTag("Zombie"))
        {
            // rb.mass = 1f;
            Jump();
        }
        else if (collision.gameObject.CompareTag("Hero") && transform.localPosition.y > 0)
        {
            Debug.Log($"@@@ {this.name} trigger enter -> {collision.name}");
            rb.AddForce(Vector2.left * Vector2.down * jumpPower);

            // speed = 0.1f;
            // rb.mass = 1f;
        }
    }

    // void OnTriggerExit2D(Collider2D other)
    // {
    //     speed = 4f;
    //     rb.mass = 5f;
    // }



    // void OnCollisionEnter2D(Collision2D collision)
    // {
    //     if (collision.gameObject.CompareTag("Zombie"))
    //     {
    //         Debug.Log($"{this.name} Zombie");

    //         speed = 0.5f;
    //         rb.mass = 0.1f;
    //         Jump();
    //     }
    //     else if (collision.gameObject.CompareTag("Border"))
    //     {
    //         Debug.Log($"{this.name} border ");

    //         speed = 3f;
    //         rb.mass = 1f;
    //         // Jump();
    //     }
    //     else if (collision.gameObject.CompareTag("Hero"))
    //     {
    //         Debug.Log($"{this.name} hero");

    //         speed = -1f;
    //         rb.mass = 0.1f;
    //     }


    // }

    // void OnCollisionExit2D(Collision2D collision)
    // {
    //     speed = 5f;
    //     rb.mass = 1f;
    // }


}
