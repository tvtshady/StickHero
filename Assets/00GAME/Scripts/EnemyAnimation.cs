using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimation : MonoBehaviour
{
    EnemyController controller;
    Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        controller = GetComponent<EnemyController>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateAnimation(controller.state);
    }

    public void UpdateAnimation(EnemyController.ENEMY_STATE state)
    {
        switch (state)
        {
            case EnemyController.ENEMY_STATE.FLY:
                animator.SetBool("isDie",false);
                break;
            case EnemyController.ENEMY_STATE.DIE:
                animator.SetBool("isDie", true);
                break;
        }
    }

}
