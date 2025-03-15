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
    Collider2D frontCollider; //앞 오브젝트가 뭔지 판단할 콜라이더


    Coroutine stateChangeCoroutine;
    float tempTime = 0f; //타이머
    float colliderCheckTime = 0.5f; //콜라이더 체크 기준 시간







    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        frontCollider = transform.Find("Front").GetComponent<BoxCollider2D>();
        stateChangeCoroutine = null;
    }

    void Update()
    {
        tempTime += Time.deltaTime;
        if (tempTime > colliderCheckTime)
        {
            tempTime = 0f;
            CheckTrigger();
        }
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(-1 * speed, rb.velocity.y);

        rb.mass = Mathf.Max(1f, 1f + 20f * (transform.localPosition.y + 3.39f));
        jumpPower = Mathf.Max(450f, 450f + 8000f * (transform.localPosition.y + 3.39f));
    }

    void OnDestroy()
    {
        InGameManager.instance.zombies.Remove(this);
    }





    void StateChange(ZombieState afterState, float duration)
    {
        stateChangeCoroutine = StartCoroutine(Co_StateChange(afterState, duration));
    }
    IEnumerator Co_StateChange(ZombieState afterState, float duration)
    {
        yield return new WaitForSeconds(duration);
        zombieState = afterState;
    }

    void Jump()
    {
        if (zombieState != ZombieState.Move)
            return;

        zombieState = ZombieState.Jump;
        rb.AddForce(Vector2.up * jumpPower);

        StateChange(ZombieState.Move, 1f);
    }





    #region  트리거 이벤트

    void CheckTrigger()
    {
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



    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Zombie"))
        {
            speed = 5f;
            Jump();
        }
        else if (collision.gameObject.CompareTag("Hero") && transform.localPosition.y > 0)
        {
            rb.AddForce(Vector2.down * jumpPower);
        }
    }


    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Border"))
            return;

        if (zombieState != ZombieState.Move)
            return;

        speed = 0.5f;
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        speed = 5f;
    }

    #endregion
}
