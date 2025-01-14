using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KirbyAtacks : MonoBehaviour
{
    
    bool atack = false;
    public bool GetAtacking() => atack;

    bool ButtomHold = false;
    bool tonori = false;
    bool floatState = false;

    Animator animator;　KirbyMove KirbyMove;
    Rigidbody2D rb;
    HitableOBJ ho;

    [SerializeField] GameObject jabPrefab; 
    [SerializeField] GameObject hardPunchPrefab; 
    [SerializeField] float tonoriSpeed = 4f;

    void Start()
    {
        ho = GetComponent<HitableOBJ>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        KirbyMove = GetComponent<KirbyMove>();
    }

    
    void Update()
    {
        if(Input.GetButtonDown("Atack") && !atack && !ho.GetHit())
        {
            if (KirbyMove.GetIsground())
            {
                Atack_Jab();
            }
            else if(!FloatState())
            {
                Tonori_Kick();
            }
        }
        ButtonHoldCount();
    }

    void Atack_Jab()
    {
        atack = true;
        StartCoroutine(KoutyokuCoroutine(0.2f));
        Anim_Action("Jab", true);
        LaunchOBJ(jabPrefab);
    }

    void Tonori_Kick()
    {
        atack = true;
        tonori = true;
        rb.velocity = new Vector3(tonoriSpeed * transform.localScale.x,-tonoriSpeed * 2,0);
        Anim_Action("Tonori", true);

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //とんおり攻撃をして地面とかオブジェクトに当たった時の処理
        if (tonori)
        {
            atack = false;
            tonori = false;
            KirbyMove.Anim_Idle();
        }
    }


    void Atack_HardPunch()
    {
        string animationName = "";

        if (KirbyMove.GetIsground())
        {
            animationName = "HardPunch";
        }
        else if (!KirbyMove.GetIsground())
        {
            animationName = "HardPunch_Air";
        }

        Debug.Log("hardAtack");
        atack = true;
        Anim_Action(animationName, true);
        LaunchOBJ(hardPunchPrefab);
        StartCoroutine(KoutyokuCoroutine(0.7f));
    }


    void LaunchOBJ(GameObject obj)
    {
        GameObject breath = Instantiate(obj, transform.position, Quaternion.identity);
        BreathMove aaa = breath.GetComponent<BreathMove>();

        //Debug.Log(aaa);
        StartCoroutine(aaa.BreathCoroutine(transform.localScale.x));


    }


    IEnumerator KoutyokuCoroutine( float koutyokuTime)
    {
        //Debug.Log("fwaefafertiuyyh");
        yield return new WaitForSeconds(koutyokuTime);
        atack = false;
        KirbyMove.Anim_Idle();
    }

    float buttonHoldTime = 0;
    void ButtonHoldCount()
    {
        if (Input.GetButton("Atack"))
        {
            //Debug.Log(buttonHoldTime);
            buttonHoldTime += Time.deltaTime;
        }

        if (Input.GetButtonUp("Atack"))
        {
            if(buttonHoldTime >= 1f)
            {
                Atack_HardPunch();
            }

            buttonHoldTime = 0;
        }

    }

    /// <summary>
    /// カービィが息を含んだstateかどうかを取得する　とんおり用
    /// </summary>
    /// <returns></returns>
    bool FloatState()
    {
        var state = KirbyMove.GetMoveState();

        
        if(state == KirbyState.Jump_FloatFalling || state == KirbyState.While_floating || state == KirbyState.Jump_FloatJump
            || state == KirbyState.Floating_Start || state == KirbyState.Floating_End)
        {
            return true;
        }
        else { return false; }

    }

    void Anim_Action(string animName, bool enforcementPlay = false)
    {
        //AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        AnimatorClipInfo clipInfo = animator.GetCurrentAnimatorClipInfo(0)[0];
        //[0]は一個分なので配列でない
        string clipName = clipInfo.clip.name;


        if (clipName != animName || enforcementPlay)
        {
            //Debug.Log(clipName);
            animator.Play(animName);
        }
        if (clipName == null)
        {
            animator.Play(animName);
        }
    }

}
