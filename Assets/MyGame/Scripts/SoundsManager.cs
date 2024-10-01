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
}
public class SoundsManager : MonoBehaviour
{
    //Dictionary�͑�2�����̒l�̔z���[]�����R�Ȍ^�̕ϐ��ɐݒ�ł���
    Dictionary<SE, String> sounds = new Dictionary<SE, String>();
    public static Dictionary<SE, AudioClip> seList = new Dictionary<SE, AudioClip>();
    public static Dictionary<SE, AudioSource> sePlayer = new Dictionary<SE, AudioSource>();

    GameObject gameManager;

    void Start()
    {

        //�t�@�C�����͊g���q���ꂿ�Ⴂ���Ȃ�
        string folder = "Sounds/";
        SE_LoadSet(SE.oneUP, folder + "1up");
        SE_LoadSet(SE.Jump, folder + "jump");
        SE_LoadSet(SE.OnLand, folder + "land");
        SE_LoadSet(SE.punch, folder + "punch");
        SE_LoadSet(SE.flyspit, folder + "fly-spit");
        SE_LoadSet(SE.fly, folder + "fly");

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
