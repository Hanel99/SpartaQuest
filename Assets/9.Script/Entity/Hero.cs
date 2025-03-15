using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : MonoBehaviour
{
    public float hp = 100f;
    public float attackDamage = 10f;

    public HeroState heroState = HeroState.Idle;
    public HeroType heroType = HeroType.Hero;


    void Start()
    {

    }

    public void Attack()
    {
        if (heroState != HeroState.Idle)
            return;

        ChangeState(HeroState.Attack);

        // 가장 가까운 좀비 서치
        // 좀비 공격 시퀀스 실행
    }

    public void Die()
    {
        ChangeState(HeroState.Die);

        //gameover 처리
    }

    public void ChangeState(HeroState state)
    {
        heroState = state;
    }
}
