using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyAction
{
    public EnemyMethod method;
    public float time;
    public float speed;
    public float jumpForce;
    

    /// <summary>
    /// 敵が行う行動と行う時間を指定する
    /// </summary>
    /// <param name="method">動きの種類</param>
    /// <param name="time">その動きを行う時間</param>
    public EnemyAction(EnemyMethod method, float time = 2f)
    {
        this.method = method;
        this.time = time;
    }

    public EnemyAction(EnemyMethod method)
    {
        this.method = method;
    }

}
