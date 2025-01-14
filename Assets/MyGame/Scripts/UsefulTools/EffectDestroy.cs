using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectDestroy : MonoBehaviour
{
    public void OnAnimationEnd()
    {
        //Debug.Log("アニメーション終了"); 
        // アニメーション終了時に行いたい処理をここに追加します 
        Destroy(gameObject);
    }
}
