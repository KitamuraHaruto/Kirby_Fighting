using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectDestroy : MonoBehaviour
{
    public void OnAnimationEnd()
    {
        //Debug.Log("�A�j���[�V�����I��"); 
        // �A�j���[�V�����I�����ɍs�����������������ɒǉ����܂� 
        Destroy(gameObject);
    }
}
