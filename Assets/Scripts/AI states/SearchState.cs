using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SearchState : IEnemyState
{
    Enemy e;

    public int id => 0;

    private bool coroutinelook = true, rotating = false;
    private const float TIME = 0.75f, ROTATEVAL = 80f;

    public void Enter(Enemy enemy)
    {
        e = enemy;
        e.running = true;
    }

    public void Execute()
    {
        if (e.target != null)
        {
            e.ChangeState(new AttackState());
        }
        else
        {
            if(!rotating && e.dist < 0.25f)
            {
                rotating = true;
                e.StartCoroutine(LookRotate());
            }

            
        }
    }

    public void ExecuteFixed()
    {
        if (e.dist > 0.1f)
        {
            if (e.lastKnowPosition.magnitude == 0)
                e.lastKnowPosition = e.startPos;

            e.LookAt(e.lastKnowPosition);
            e.Move(new Vector2(e.pathDir.x, e.pathDir.y));
        }
    }

    public void Exit()
    {
        e.running = false;
        e.lastKnowPosition = Vector2.zero;
        coroutinelook = false;
    }

    private IEnumerator LookRotate()
    {
        Quaternion startRot = e.transform.rotation;
        Quaternion left = Quaternion.Euler(0, 0, ROTATEVAL + startRot.eulerAngles.z);
        Quaternion right = Quaternion.Euler(0, 0, -ROTATEVAL + startRot.eulerAngles.z);
        float elapsedTime = 0;

        while(elapsedTime < TIME && coroutinelook)
        {
            elapsedTime += Time.deltaTime;

            e.transform.rotation = Quaternion.Lerp(startRot, left, elapsedTime / TIME);
            yield return null;
        }

        elapsedTime = 0;
        while (elapsedTime < TIME*2 && coroutinelook)
        {
            elapsedTime += Time.deltaTime;

            e.transform.rotation = Quaternion.Lerp(left, right, elapsedTime / TIME);
            yield return null;

        }

        e.ChangeState(new PatrolState());
        yield return null;
    }

    public void OnTriggerEnter2D(Collider2D o)
    {

    }
}
