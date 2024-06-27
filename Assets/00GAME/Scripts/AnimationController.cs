using DragonBones;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameManager;

public class AnimationController : MonoBehaviour
{
    UnityArmatureComponent armature;
    [SerializeField]
    GameObject weapon;
    // Start is called before the first frame update
    
    private void Start()
    {
        armature = GetComponent<UnityArmatureComponent>();
        Observer.instance.AddListener(CONSTANT.playerState, UpdateAnim);
        GameManager.instance.currentPlayerState = GameManager.PLAYER_STATE.IDLE;
    }

    public void UpdateAnim(object playerState)
    {
        GameManager.PLAYER_STATE state = (GameManager.PLAYER_STATE)playerState;
        switch (state)
        {
            case GameManager.PLAYER_STATE.IDLE:
                weapon.SetActive(false);
                armature.animation.GotoAndPlayByTime("Idle",0,0);
                break;
            case GameManager.PLAYER_STATE.GROW:
                weapon.SetActive(false);
                armature.animation.GotoAndPlayByTime("Grow", 0, 0);
                break;
            case GameManager.PLAYER_STATE.RUN:
                weapon.SetActive(false);
                armature.animation.GotoAndPlayByTime("Run", 0, 0);
                break;
            case GameManager.PLAYER_STATE.DIE:
                weapon.SetActive(false);
                armature.animation.GotoAndPlayByTime("Die", 0, 0);
                break;
            case GameManager.PLAYER_STATE.KICK:
                weapon.SetActive(false);
                armature.animation.GotoAndPlayByTime("Kick",-1,1);
                break;
            case GameManager.PLAYER_STATE.FIGHT:
                weapon.SetActive(true);
                armature.animation.GotoAndPlayByTime("Fight", 0, 1);
               /* if(armature.animation.isCompleted)
                armature.animation.GotoAndPlayByTime("Run", 0, 0);*/
                break;
        }
    }


   /* void SetAnimation(int index)
    {
        armature.animation.ti
    }*/
  
}
