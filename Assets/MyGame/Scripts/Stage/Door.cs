using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Door : MonoBehaviour
{
    bool enterFrag = false;
    GameObject player;
    KirbyMove kirby;

    [SerializeField] string sceneName;
    [SerializeField] Image blackScreen;
    [SerializeField] float fadeTime = 1f;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        kirby = player.GetComponent<KirbyMove>();
    }

    void Update()
    {
        if (enterFrag && kirby.InputV() == 1)
        {
            Debug.Log("•”‰®ˆÚ“®");
            StartCoroutine(KitamuraMethod.FadeOutSceneChange(blackScreen, fadeTime, sceneName));
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            //Debug.LogWarning("enter");
            enterFrag = true;

        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            //Debug.LogWarning("exit");
            enterFrag = false;
        }
    }

    void DoorExit()
    {
        kirby.Anim_DoorExit();

    }
}
