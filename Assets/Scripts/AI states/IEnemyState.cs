using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnemyState
{

    int id { get; }
    void Enter(Enemy enemy); //Start
    void Execute(); //Update

    void ExecuteFixed(); //Fixed update

    void Exit(); // leave
    void OnTriggerEnter2D(Collider2D o);
}
