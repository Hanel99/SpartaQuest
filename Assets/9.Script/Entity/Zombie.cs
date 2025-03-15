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





    void StateChange(ZombieState afterState, float duration = 0f)
    {
        switch (afterState)
        {
            case ZombieState.Move:
                if (zombieState == ZombieState.Die)
                    return;
                stateChangeCoroutine = StartCoroutine(Co_StateChange(afterState, duration));
                break;

            case ZombieState.Jump:
                if (zombieState != ZombieState.Move)
                    return;
                stateChangeCoroutine = StartCoroutine(Co_StateChange(afterState, duration));

                break;
            case ZombieState.Die:
                stateChangeCoroutine = StartCoroutine(Co_StateChange(afterState, duration));
                break;
        }
    }

    IEnumerator Co_StateChange(ZombieState afterState, float duration)
    {
        if (duration > 0) yield return new WaitForSeconds(duration);

        zombieState = afterState;
    }

    void Jump()
    {
        if (zombieState != ZombieState.Move)
            return;

        StateChange(ZombieState.Jump);
        rb.AddForce(Vector2.up * jumpPower);

        //원래 이렇게 하면 안되는데... 착지 시점을 알면 시전해야 함
        StateChange(ZombieState.Move, 1f);
    }


    public void Die()
    {
        StateChange(ZombieState.Die);

        // 죽는 시퀀스 실행
        Destroy(gameObject);
    }

    public void Damage(float damage)
    {
        hp -= damage;
        if (hp <= 0)
            Die();
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

    public void FrontTriggerEvent(GameObject collision)
    {
        if (collision.CompareTag("Zombie"))
        {
            Jump();
        }
        else if (collision.gameObject.CompareTag("Hero") && transform.localPosition.y <= 0)
        {
            // 벽에 붙어있는 아래에 있는 좀비는 뒤로 물러나도록 함
            rb.AddForce(Vector2.right * jumpPower);
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
            // 일정 위치 이상으로 올라갔을 경우 아래로 내려오는 힘을 가함        
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
