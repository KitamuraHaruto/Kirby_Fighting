using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyDataList
{
    //敵の動きは全部ここで作る


    public static List<EnemyAction> waddleDee = new List<EnemyAction>
    {
        new EnemyAction(EnemyMethod.Walk, 2.5f),
        new EnemyAction(EnemyMethod.Walk, 2.5f),
    };




}

