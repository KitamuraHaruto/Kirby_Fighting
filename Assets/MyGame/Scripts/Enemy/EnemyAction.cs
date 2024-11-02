using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyAction
{
    public EnemyMethod method;
    public float time;


    /// <summary>
    /// �G���s���s���ƍs�����Ԃ��w�肷��
    /// </summary>
    /// <param name="method"></param>
    /// <param name="time"></param>
    public EnemyAction(EnemyMethod method, float time = 2f)
    {
        this.method = method;
        this.time = time;
    }
}
