using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public static class KitamuraMethod 
{


    /// <summary>
    /// 引数のVecter3を向きを変えずに値を置き換える
    /// </summary>
    /// <param name="dir">ベクトル</param>
    /// <param name="xValue"></param>
    /// <param name="zValue"></param>
    /// <returns>置き換わったベクトルが正規化された値</returns>
    public static Vector3 VectorReplaced(Vector3 dir, float xValue, float zValue)
    {
        Vector3 vector3 = dir;
        if (dir.x > 0) { vector3.x = xValue; }
        if (dir.x < 0) { vector3.x = -xValue; }
        if (dir.z > 0) { vector3.z = zValue; }
        if (dir.z < 0) { vector3.z = -zValue; }

        return vector3.normalized;
    }

    /// <summary>
    /// 引数のVecter3を向きを変えずに値を置き換える
    /// </summary>
    /// /// <returns>置き換わったベクトルが正規化された値</returns>

    public static Vector3 VectorReplaced2D(Vector3 dir, float Value)
    {
        Vector3 vector3 = dir;
        if (dir.x > 0) { vector3.x = Value; }
        if (dir.x < 0) { vector3.x = -Value; }
        if (dir.y > 0) { vector3.y = Value; }
        if (dir.y < 0) { vector3.y = -Value; }

        return vector3.normalized;
    }


    /// <summary>
    /// 二点の方向のベクトルを求める正規化もやります。
    /// </summary>
    /// <param name="startPos"></param>
    /// <param name="goalPos"></param>
    /// <returns></returns>
    public static Vector3 DirCalculation( Vector3 startPos,Vector3 goalPos )
    {
        Vector3 dir = goalPos - startPos;
        return dir.normalized;
    }


    /// <summary>
    /// 引数の値を0以下にさせないようにする
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static float NotbelowZero( float value )
    {
        //Mathf.Max(value, 0);
        if (value <= 0)
        {
            return 0;
        }

        return value;
    }


    public static GameObject FindPlayer()
    {
        return GameObject.FindGameObjectWithTag("Player");
    }

    /// <summary>
    /// シーンを読み込みます
    /// </summary>
    /// <param name="sceneName"></param>
    public static void LoadScene( string sceneName ,float second)
    {
        //StartCoroutine(LoadSceneCoroutine(sceneName, second));
    }

    public static IEnumerator LoadSceneCoroutine(string sceneName, float sec)
    {
        yield return new WaitForSeconds(sec);
        SceneManager.LoadScene(sceneName);
    }


    /// <summary>
    /// レイヤーを設定する
    /// </summary>
    /// <param name="needSetChildrens">子にもレイヤー設定を行うか</param>
    public static void SetLayer(this GameObject gameObject, int layerNo, bool needSetChildrens = true)
    {
        if (gameObject == null)
        {
            return;
        }
        gameObject.layer = layerNo;

        //子に設定する必要がない場合はここで終了
        if (!needSetChildrens)
        {
            return;
        }

        //子のレイヤーにも設定する
        foreach (Transform childTransform in gameObject.transform)
        {
            SetLayer(childTransform.gameObject, layerNo, needSetChildrens);
        }
    }

}
