using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


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
    /// 2点のベクトルの向きに引数の値を置き換える
    /// </summary>
    /// /// <returns>置き換わったベクトルが正規化された値</returns>

    public static Vector3 VectorReplaced2D(Vector3 myPosition,Vector3 target, float Value = 1, bool Upper = false)
    {
        Vector3 dir = Vector3.zero;
        if (myPosition.x > target.x) { dir.x = Value; Debug.Log("プラス" + dir.x); }
        if (myPosition.x < target.x) { dir.x = -Value; Debug.Log("マイナス" + dir.x); }
        if (myPosition.y > target.y) { dir.y = Value; Debug.Log("プラス" + dir.y); }
        if (myPosition.y < target.y && Upper == false) { dir.y = -Value; Debug.Log("マイナス" + dir.y); }
        else if (Upper == true) { dir.y = Value; Debug.Log(dir.y); }
      
        //Debug.Log(dir.normalized);
        return dir;
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


    public static IEnumerator FadeOut(Image blackScreen, float fadeTime)
    {

        //経過時間
        float elapsedTime = 0;

        //フェードアウトさせるImageの色を取得
        Color fadeColor = blackScreen.color;

        //経過時間がフェードアウト時間に達するまでループ
        while (elapsedTime < fadeTime)
        {
            //経過時間の更新
            elapsedTime += Time.deltaTime;

            //Alpha値(透明度)を更新
            fadeColor.a = Mathf.Clamp01(elapsedTime / fadeTime);

            //更新された色をImageに適用
            blackScreen.color = fadeColor;

            //次のフレームまで一時停止
            yield return null;
        }

    }

    public static IEnumerator FadeOutSceneChange(Image blackScreen, float fadeTime, string sceneName)
    {

        //経過時間
        float elapsedTime = 0;

        //フェードアウトさせるImageの色を取得
        Color fadeColor = blackScreen.color;

        //経過時間がフェードアウト時間に達するまでループ
        while (elapsedTime < fadeTime)
        {
            //経過時間の更新
            elapsedTime += Time.deltaTime;

            //Alpha値(透明度)を更新
            fadeColor.a = Mathf.Clamp01(elapsedTime / fadeTime);

            //更新された色をImageに適用
            blackScreen.color = fadeColor;

            //次のフレームまで一時停止
            yield return null;
        }

        //フェードアウトの処理が終わったらシーンを切り替える
        SceneManager.LoadScene(sceneName);

    }




}
