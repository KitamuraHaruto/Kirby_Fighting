using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour
{
    bool enterFrag = false;
    GameObject player;
    KirbyMove kirby;

    [SerializeField] string sceneName;

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
