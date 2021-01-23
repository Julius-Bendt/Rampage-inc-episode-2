using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState : IEnemyState
{

    Enemy e;
    private int currentPoint = 0;

    public int id => 1;

    private bool coroutinelook = true, rotating = false;
    private const float TIME = 1.5f, ROTATEVAL = 45;
    private const float IDLETIME = 2;

    public void Enter(Enemy enemy)
    {
        e = enemy;
        e.running = false;
    }

    public void Execute()
    {
        if (e.target != null)
            e.ChangeState(new AttackState());
        else if (e.lastKnowPosition.magnitude != 0)
            e.ChangeState(new SearchState());

        if(e.PatrolPoints != null)
        {
            if (e.PatrolPoints.Length > 1)
            {
                if (Vector2.Distance(e.transform.position, e.PatrolPoints[currentPoint].position) < 0.5)
                {
                    currentPoint++;

                    if (currentPoint > e.PatrolPoints.Length - 1)
                    {
                        if (!rotating)
                            e.StartCoroutine(LookRotate());

                        currentPoint = 0;
                    }
                }

                e.nextPatrolPos = e.PatrolPoints[currentPoint].position;
            }
            else
            {
                if (!rotating)
                    e.StartCoroutine(LookRotate());
            }
        }
    }

    public void ExecuteFixed()
    {
        if(e.PatrolPoints.Length > 1 && !rotating)
        {
            e.Move(new Vector2(e.pathDir.x, e.pathDir.y));
            e.LookAt(e._path);
        }

    }

    public void Exit()
    {

    }

    public void OnTriggerEnter2D(Collider2D o)
    {

    }

    private IEnumerator LookRotate()
    {
        rotating = true;
        yield return new WaitForSeconds(IDLETIME);

        Quaternion startRot = e.transform.rotation;
        Quaternion left = Quaternion.Euler(0, 0, ROTATEVAL + startRot.eulerAngles.z);
        Quaternion right = Quaternion.Euler(0, 0, -ROTATEVAL + startRot.eulerAngles.z);
        float elapsedTime = 0;

        while (elapsedTime < TIME && coroutinelook)
        {
            elapsedTime += Time.deltaTime;

            e.transform.rotation = Quaternion.Lerp(startRot, left, elapsedTime / TIME);
            yield return null;
        }
        yield return new WaitForSeconds(0.75f);

        elapsedTime = 0;
        while (elapsedTime < TIME * 2 && coroutinelook)
        {
            elapsedTime += Time.deltaTime;

            e.transform.rotation = Quaternion.Lerp(left, right, elapsedTime / TIME);
            yield return null;

        }
        yield return new WaitForSeconds(0.75f);


        elapsedTime = 0;
        while (elapsedTime < TIME * 2 && coroutinelook)
        {
            elapsedTime += Time.deltaTime;

            e.transform.rotation = Quaternion.Lerp(right, startRot, elapsedTime / TIME);
            yield return null;

        }
        rotating = false;
        yield return null;
    }
}
