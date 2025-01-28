using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public enum EnemyMethod
{
    Walk,Jump,Atack,Away,AirAtack,Frip,Run,Chase
}

[RequireComponent(typeof(StopWatch))]
public partial class EnemyScript : MonoBehaviour
{
    BoxCollider2D col;
    public BoxCollider2D GetCol() => col;

    Rigidbody2D rb; StopWatch sw;
    HitableOBJ ho; Animator animator;

    IEnemyMove enemyData;

    [SerializeField, Tooltip("地面のレイヤー")]
    LayerMask Ground;

    RaycastHit2D isGround;

    //[HideInInspector] public bool isHit = false;

    /// <summary> 今読み込んでるactionDataの番号 </summary>
    int currentDataNum = 0;

    /// <summary> 今読み込んでるactionDataの処理を終えてるかどうか </summary>
    bool moveDone = true;

    /// <summary> actionDataの処理を行うかどうか </summary>
    bool readMoveData = true;

    void Start()
    {
        ho = GetComponent<HitableOBJ>();
        col = GetComponent<BoxCollider2D>();
        sw = GetComponent<StopWatch>();
        rb = GetComponent<Rigidbody2D>();
        enemyData = GetComponent<IEnemyMove>();
        animator = GetComponent<Animator>();
    }


    void Update()
    {
        OnGroundSearch();
        DownAnimation();

        //今やってる処理を終わらせていたら
        if (moveDone)
        {
            ReadMoveData();
        }

    }

    public void DownAnimation()
    {
        if (ho.GetHit())
        {
            Debug.Log("Hit");
            if (!isGround)
            {
                Anim_Damage_Start();
                //animator.Play("Damage_Start");
            }
            else if (isGround)
            {
                Vector3 currentVelosity = rb.velocity; currentVelosity.x = 0f;
                rb.velocity = currentVelosity;
                if (moveState != EnemyState.Down)
                {
                    Anim_Down();
                }
            }

        }
        else if (moveState == EnemyState.Down || moveState == EnemyState.Damage_Start)
        {
            //Anim_Idle();
            //Anim_Down();
        }
    }


    /// <summary>
    /// 
    /// </summary>
    void ReadMoveData()
    {
        if (readMoveData)
        {
            moveDone = false;
            switch (enemyData.actionData()[currentDataNum].method)
            {
                case EnemyMethod.Walk:
                    StartCoroutine(Walk(enemyData.actionData()[currentDataNum], enemyData.Movespeed()));
                    break;

                case EnemyMethod.Jump:
                    Jump(enemyData.JumpPower());
                    break;

                case EnemyMethod.Atack:
                    break;

                case EnemyMethod.Away:
                    break;

                case EnemyMethod.Frip:
                    Frip();
                    break;

            }

            currentDataNum++;
            currentDataNum = currentDataNum % enemyData.actionData().Count;
        }
    }


    void NextReadData()
    {

    }


    //-----------------------動きのメソッド------------------------------------------------
    //

    IEnumerator Walk(EnemyAction actionData, float speed)
    {
        Anim_Walk();
        Vector2 dir = new Vector2(speed * transform.localScale.x, rb.velocity.y);
        rb.velocity = dir;
        //Debug.Log(rb.velocity);
        yield return new WaitForSeconds(actionData.time);
        moveDone = true;
    }

    void Jump(float jumpForce)
    {
        if (isGround)
        {
            
            Vector2 Force = new Vector2(0, jumpForce);
            rb.velocity = Force;
        }
        moveDone = true;
    }

    void Atack()
    {
        moveDone = true;
    }

    /// <summary>
    /// 自身のスケール
    /// </summary>
    void Frip()
    {
        //Debug.Log("Frip");
        Vector3 value = transform.localScale;
        value.x *= -1;
        transform.localScale = value;
        moveDone = true;
    }

    //IEnumerator ChaceCoroutine()
    //{

    //}


    /// <summary>/// 地面に立っているかどうかを調べる /// </summary>
    void OnGroundSearch()
    {
        //サークルキャストは半径を小さくして壁の判定もとってしまうのを防いでいる

        //distance:  「引数名：」を書いてやることでパット見で何の引数書いてるかわかりやすくなる　　また引数名：を書けば引数の順番も変えられる
        isGround = Physics2D.CircleCast(transform.position, 0.2f, Vector2.down, distance: 1f, Ground);
        //めんどくさかったら　　rd.velocity.y == 0　でとるのもあり
        var GroundTagName = isGround ? isGround.collider.gameObject.tag : "";
        //サークルキャストが地面に当たったら、当たったもののタグをとる


        if (GroundTagName == "Ashiba")
        {
            float v = Input.GetAxisRaw("Vertical");
            var bc = isGround.collider.gameObject.GetComponent<BoxCollider2D>();
            //isGround.collider.gameObject.GetComponent<BoxCollider2D>
            if (v < 0)
            {
                bc.isTrigger = true;
            }
            //下ボタンを押したら足場がすり抜ける
        }

    }

}
