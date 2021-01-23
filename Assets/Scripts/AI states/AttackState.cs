using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : IEnemyState
{
    Enemy e;
    public int id => 2;
    float timer = 0;
    public void Enter(Enemy enemy)
    {
        e = enemy;
        e.running = true;
        timer = 0;
    }

    public void Execute()
    {
        timer += Time.deltaTime;

        if (e.target != null)
        {
            if(timer > 0.35f)
            {
                e.fire = e.dist <= e.weapon.attackDist;
            }
               
        }
        else
        {
            e.ChangeState(new SearchState());
        }
    }

    public void ExecuteFixed()
    {
        if (e.target != null)
        {
            e.lastKnowPosition = e.target.position;

            if (e.dist > 0.1f)
            {
                e.LookAt(e.lastKnowPosition);
            }

            if (e.dist > 0.1f && e.dist > e.weapon.attackDist * 0.75f)
            {
                e.Move(new Vector2(e.pathDir.x, e.pathDir.y));
            }
        }
    }

    public void Exit()
    {
        e.fire = false;
    }

    public void OnTriggerEnter2D(Collider2D o)
    {

    }
}
