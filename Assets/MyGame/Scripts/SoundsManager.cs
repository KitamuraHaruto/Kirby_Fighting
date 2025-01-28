using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SE
{
    oneUP,
    Jump,
    punch,
    OnLand,
    flyspit,
    fly,
    HardPunch,
    Punch,
    PunchHit,
    Kick,
}
public class SoundsManager : MonoBehaviour
{
    //Dictionaryは第2引数の値の配列の[]を自由な型の変数に設定できる
    Dictionary<SE, String> sounds = new Dictionary<SE, String>();
    public static Dictionary<SE, AudioClip> seList = new Dictionary<SE, AudioClip>();
    public static Dictionary<SE, AudioSource> sePlayer = new Dictionary<SE, AudioSource>();

    GameObject gameManager;

    void Start()
    {

        //ファイル名は拡張子入れちゃいけない
        string folder = "Sounds/";
        SE_LoadSet(SE.oneUP, folder + "1up");
        SE_LoadSet(SE.Jump, folder + "jump");
        SE_LoadSet(SE.OnLand, folder + "land");
        SE_LoadSet(SE.punch, folder + "punch");
        SE_LoadSet(SE.flyspit, folder + "fly-spit");
        SE_LoadSet(SE.fly, folder + "fly");
        SE_LoadSet(SE.Punch, folder + "punch-rapid");
        SE_LoadSet(SE.PunchHit, folder + "punch");
        SE_LoadSet(SE.HardPunch, folder + "beam-charged");
        SE_LoadSet(SE.Kick, folder + "hit1");

    }

    void SE_LoadSet(SE soundEnum, string fileName)
    {
        sounds[soundEnum] = fileName;
        //start
        var aa =  seList[soundEnum] = Resources.Load<AudioClip>(sounds[soundEnum]);
        sePlayer[soundEnum] = GetComponent<AudioSource>();

        //Debug.Log(aa);
    }
    
    void Update()
    {

    }

    public static void SE_Play(SE seName)
    {
        //sePlayer[seName].clip = seList[seName];
        sePlayer[seName].PlayOneShot(seList[seName]);
    }
}
