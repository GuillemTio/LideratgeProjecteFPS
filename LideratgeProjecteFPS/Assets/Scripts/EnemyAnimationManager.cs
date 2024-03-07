using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimationManager : MonoBehaviour
{
    Animator animator;
    [SerializeField] SimpleEnemy simpleEnemyScript;
    [SerializeField] EnemyBehaviour enemyBehaviourScript;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void DieAnimationEnd()
    {
        simpleEnemyScript.DieAnimationEnd();
    }

    public void StopEnemy()
    {
        enemyBehaviourScript.stopped = true;
    }
}
