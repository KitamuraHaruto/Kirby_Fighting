using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum KirbyState
{
    Idle, Walk, Jump_Start, Jump_Falling, Jump_FloatFalling,
    Jump_StartFall,
    Floating_Start,
    While_floating,
    Floating_End,
    Jump_FloatEndFalling,
    Jump_FloatJump,
    DoorExit,
    Dash,
    Dash_Stop,
    KirbyDamage_Start,
    Down,
}

public partial class KirbyMove : MonoBehaviour
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

    string aa = PlayerAnimString.idle;

    //Action Anim_Walk = () => { };   �����ʂŕ������߂������郉���_��void���Ƃ���  ALT+Enter
    void Anim_Walk() { Anim_Action("Walk"); moveState = KirbyState.Walk; }
    void Anim_Dash() { Anim_Action("Dash"); moveState = KirbyState.Dash; }
    void Anim_Dash_Stop() { Anim_Action("DashStop"); moveState = KirbyState.Dash_Stop; }
    public void Anim_Idle() { Anim_Action("Idle"); moveState = KirbyState.Idle; }
    void Anim_Jump_Start() { Anim_Action("Jump_Start"); moveState = KirbyState.Jump_Start; }
    void Anim_Jump_Falling() { Anim_Action("Jump_Falling"); moveState = KirbyState.Jump_Falling; }
    void Anim_Jump_StartFall() { Anim_Action("Jump_StartFall"); moveState = KirbyState.Jump_StartFall; }
    public void Anim_Jump_FloatFalling() { Anim_Action("Jump_FloatFalling"); moveState = KirbyState.Jump_FloatFalling; }
    void Anim_Jump_FloatEndFalling() { Anim_Action("Jump_FloatEndFalling "); moveState = KirbyState.Jump_FloatEndFalling; }
    void Anim_Floating_Start() { Anim_Action("Floating_Start"); moveState = KirbyState.Floating_Start; }
    public void Anim_DoorExit() { Anim_Action("DoorExit"); moveState = KirbyState.DoorExit; }
    //���V���̎�p�^�p�^�̃A�j���[�V����
    void Anim_FloatJump() { Anim_Action("Jump_FloatJump"); moveState = KirbyState.Jump_FloatJump; }
    void Anim_Floating_End() { Anim_Action("Floating_End"); moveState = KirbyState.Floating_End; }

    //void Anim_Jump() => animator.SetBool("jumping", isGround && Input.GetButtonDown("Jump"));
    void Anim_Float() => animator.SetBool("floating", floating);
    void Anim_Damage_Start() { Anim_Action("KirbyDamage_Start"); moveState = KirbyState.KirbyDamage_Start; }

    void Anim_Down() { Anim_Action("KirbyDown"); moveState = KirbyState.Down; }


}
