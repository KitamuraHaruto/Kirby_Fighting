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
    /// <summary>�@�K�v�ɉ����ăA�j���[�V�������Đ�������֐��@ </summary>
    /// <param name="animName">�A�j���[�V�����R���g���[���[�̃N���b�v��</param>
    /// <param name="enforcementPlay"> true�ŋ����I�ɍŏ�����Đ�������</param>
    void Anim_Action(string animName, bool enforcementPlay = false)
    {
        //AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        AnimatorClipInfo clipInfo = animator.GetCurrentAnimatorClipInfo(0)[0];
        //[0]�͈���Ȃ̂Ŕz��łȂ�
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
    /// ���݂̓G���g�̏��
    /// </summary>
    EnemyState moveState;

    void Anim_Walk() { Anim_Action("Walk"); moveState = EnemyState.Walk; }
    void Anim_Dash() { Anim_Action("Dash"); moveState = EnemyState.Dash; }
    void Anim_Dash_Stop() { Anim_Action("DashStop"); moveState = EnemyState.Dash_Stop; }
    public void Anim_Idle() { Anim_Action("Idle"); moveState = EnemyState.Idle; }
    void Anim_Damage_Start() { Anim_Action("Damage_Start"); moveState = EnemyState.Damage_Start; }
    void Anim_Down() { Anim_Action("Down"); moveState = EnemyState.Down; }



}
