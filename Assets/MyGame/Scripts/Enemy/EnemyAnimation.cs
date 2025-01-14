using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


public enum EnemyState
{
    Idle, Walk, Jump_Start, Jump_Falling,
    DoorExit,
    Dash,
    Dash_Stop,
    Damage_Start,
    Down,

}

public partial class EnemyScript : MonoBehaviour
{
    /// <summary>　必要に応じてアニメーションを再生させる関数　 </summary>
    /// <param name="animName">アニメーションコントローラーのクリップ名</param>
    /// <param name="enforcementPlay"> trueで強制的に最初から再生させる</param>
    void Anim_Action(string animName, bool enforcementPlay = false)
    {
        //AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        AnimatorClipInfo clipInfo = animator.GetCurrentAnimatorClipInfo(0)[0];
        //[0]は一個分なので配列でない
        string clipName = clipInfo.clip.name;


        if (clipName != animName || enforcementPlay)
        {
            //Debug.Log(clipName);
            animator.Play(animName);
        }
        if (clipName == null)
        {
            animator.Play(animName);
        }
    }

    /// <summary>
    /// 現在の敵自身の状態
    /// </summary>
    EnemyState moveState;

    void Anim_Walk() { Anim_Action("Walk"); moveState = EnemyState.Walk; }
    void Anim_Dash() { Anim_Action("Dash"); moveState = EnemyState.Dash; }
    void Anim_Dash_Stop() { Anim_Action("DashStop"); moveState = EnemyState.Dash_Stop; }
    public void Anim_Idle() { Anim_Action("Idle"); moveState = EnemyState.Idle; }
    void Anim_Damage_Start() { Anim_Action("Damage_Start"); moveState = EnemyState.Damage_Start; }
    void Anim_Down() { Anim_Action("Down"); moveState = EnemyState.Down; }



}
