using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopWatch : MonoBehaviour
{

    bool isStart = false;
    float startTime = 0;

    public void CountStart()
    {
        startTime = Time.time;
        isStart = true;
    }

    public float GetTime()
    {
        return Time.time - startTime;
    }

    /// <summary>
    /// �w�肵���b���̊Ԋu��true��Ԃ�
    /// </summary>
    /// <param name="interval">�Ԋu�̕b��</param>
    /// <returns>�C���^�[�o�����Ԃ��z���Ă��邩�ǂ���</returns>
    public bool HitTiming(float interval)
    {
        if(isStart == false)
        {
            CountStart();
            //�N���������̍ŏ��̃q�b�g
            return true;
        }

        if (GetTime() > interval)
        {
            CountStart();
            return true;
        }
            
        //���̎��_�ŃC���^�[�o���z���ĂȂ��̂��m�肷��
        return false;
    }
}
