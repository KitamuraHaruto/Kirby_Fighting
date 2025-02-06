using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

//C++だとクラス作らんでもnameSpaceでできる
public class PlayerAnimString
{
    public const string idle = "Idle",walk = "Walk";
}

[RequireComponent(typeof(StopWatch))]
public partial class KirbyMove : MonoBehaviour
{
    private GameObject player;
    private Transform playerTf;
    private Animator animator;
    private AudioSource AS ;

    private ControlList controlList;
    

    bool DashFrag;
    bool isDash = false;
    bool isFall = false;
    bool isWalking = false;
    private float h;
    private float v;
    public bool moveable;
    public float InputV()
    {
        return v;
    }


    //Range(最小値, 最大値)でInspecter上でいじれる値の範囲を決められる
    [Tooltip("歩きスピード")]
    [SerializeField, Range(1f, 50f)] float moveSpeed;

    [Tooltip("空中での移送速度")]
    [SerializeField, Range(1f, 50f)] float floatMoveSpeed;

    [Tooltip("ダッシュしている時のスピード")]
    [SerializeField] float dashSpeed = 10;

    [Tooltip("ジャンプ力")]
    [SerializeField] float jumpForce;

    [Tooltip("浮遊中のジャンプ力")]
    [SerializeField] float floatjumpForce;


    [SerializeField, Tooltip("落ちている時のグラビティスケール")]
    float fallGravity = 3f;

    [SerializeField, Tooltip("落ちていて膨らんでいる状態のグラビティスケール")]
    float floatGravity = 3f;

    [SerializeField, Tooltip("息を吐いたときに出てくる空気のプレハブ")]
    public GameObject breathPrefab;

    [SerializeField, Tooltip("地面のレイヤー")]
    LayerMask Ground;

    [SerializeField, Tooltip("壁のレイヤー")]
    LayerMask wall;

    private Rigidbody2D rb;
    private HitableOBJ ho;

    float moveX;
    bool floating = false;
    public bool GetFloating() => floating; 

    KirbyAtacks kirbyAtaks;
    

    public static KirbyState moveState = KirbyState.Idle;
    public KirbyState GetMoveState() => moveState;
    
    //public string Movestate => moveState.ToString();

    /// <summary>///  地面に立っているどうかのフラグ /// </summary>
    RaycastHit2D isGround;
    public bool GetIsground() => isGround;

    float tempVeloisityY, tempVelisityX;

    StopWatch stopWatch;

    void Start()
    {
        moveable = true;
        AS = GetComponent<AudioSource>();
        ho = GetComponent<HitableOBJ>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        stopWatch = GetComponent<StopWatch>();
        kirbyAtaks = GetComponent<KirbyAtacks>();
        controlList = GetComponent<ControlList>();
        Application.targetFrameRate = 60;

        //StartCoroutine(moveableCoroutine());
    }

    IEnumerator moveableCoroutine()
    {
        moveable = false;
        yield return new WaitForSeconds(0.1f);
        moveable = true;
    }


    void Update()
    {
        OnGroundSearch();
        //animator.SetBool("Dash", DashFrag);

        h = moveable ? Input.GetAxisRaw("Horizontal") : 0f;
        v = moveable ? Input.GetAxisRaw("Vertical") : 0f;

        //移動系の処理は攻撃中には行わないつもり
        if (kirbyAtaks.GetAtacking() == false　&& !kirbyAtaks.GetAtacking() && !ho.GetHit())
        {
            //Debug.Log("moveable");
            AtackMethod();
            FlipHorizontal(h);
            Jump();
            tempVeloisityY = rb.velocity.y; tempVelisityX = rb.velocity.x;
            MoveHorizontal(h, tempVeloisityY, isGround);
            ReduceFallSpeed(rb);

        }

        DownAnimation();

    }

    void DownAnimation()
    {
        if (ho.GetHit())
        {
            //Debug.Log("moveable");
            if (!isGround && moveState != KirbyState.KirbyDamage_Start)
            {
                Anim_Damage_Start();
            }
            else if (isGround)
            {
                Vector3 currentVelosity = rb.velocity;  currentVelosity.x = 0f;
                rb.velocity = currentVelosity;
                if (moveState != KirbyState.Down && ho.GetDownFrag())
                {
                    Debug.LogWarning(ho.GetDownFrag() + "Down");
                    Anim_Down();
                }
            }
            
        }
        else if(moveState == KirbyState.Down || moveState == KirbyState.KirbyDamage_Start)
        {
            Anim_Idle();
        }
    }

    bool IsWalking(float axisH) => Mathf.Abs(axisH) != 0 && moveState != KirbyState.Walk && moveState != KirbyState.Jump_Start
            && moveState != KirbyState.Floating_End && moveState != KirbyState.Dash && isGround;


    private void MoveHorizontal(float axisH, float tempVelisityY, bool isGround)
    {
        //地上に立っている時のアニメーション処理
        if (!isGround)
        {
           goto Anim_1_end;　//Anim_1_end:が書かれている228行目に
        }
        if (IsWalking(axisH))
        {
            //SoundsManager.SE_Play(SE.OnLand);
            Anim_Walk();
            goto Anim_1_end;
            //Debug.Log("歩き");
        }

        if (moveState == KirbyState.Jump_FloatFalling || moveState == KirbyState.Jump_FloatJump
            || moveState == KirbyState.Floating_Start)
        {
            Breath();
            goto Anim_1_end;
        }

        if ((moveState == KirbyState.Walk)
            && Mathf.Abs(axisH) == 0 && !ho.GetHit())
        {
            //|| moveState == KirbyState.Floating_End
            Anim_Idle();
            goto Anim_1_end;
        }
        else//    空中
        {
            //膨らんで無かったら通常のにする
        }

    Anim_1_end:

        //moveX = axisH * 10;

        //axisHを掛けることで入力しなかったら動かないようにする
        moveX = moveSpeed * axisH;

        //Vector2 move = new Vector2(moveX, 0); //最終的な動き
        Vector2 move = new Vector2(moveX, tempVelisityY); //最終的な動き

        if (isGround && (controlList.Dash_Left() || controlList.Dash_Right() || moveState == KirbyState.Dash) )
        {

            if (moveState != KirbyState.Jump_Start)
            {
                Anim_Dash();
            }

            if (transform.localScale.x > 0)
            { move.x = dashSpeed; }
            else
            { move.x = -dashSpeed; }

            Debug.Log(move.x);

        }

        if (axisH == 0 && isGround && moveState == KirbyState.Dash)   //地面に立っているとき限定
        {
            Anim_Dash_Stop();
        }


        rb.velocity = move;
    }

    bool dashFrag = false;

    void Dash()
    {
        if(moveState != KirbyState.Dash)
        {

        }
    }
    IEnumerator DashFragCoroutine()
    {
        yield return new WaitForSeconds(0.1f);
    }


    public void Breath()
    {
        if (moveState == KirbyState.Jump_FloatJump || moveState == KirbyState.Jump_FloatFalling
        || moveState == KirbyState.Floating_Start)
        {
            GameObject breath = Instantiate(breathPrefab,transform.position, Quaternion.identity);
            BreathMove aaa = breath.GetComponent<BreathMove>();

            //Debug.Log(aaa);
            StartCoroutine(aaa.BreathCoroutine(transform.localScale.x));


            //Debug.Log("息吐き");
            Anim_Floating_End();
            SoundsManager.SE_Play(SE.flyspit);
            //息を吐く処理をさせたい
        }

    }

    public void BreathEnd()
    {
        if (!isGround)
        {
            Anim_Jump_StartFall();
        }
        if (isGround && !ho.GetHit())
        {
            Anim_Idle();
        }
    }

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

        if (isGround)
        {
            floating = false;
        }
    }

    private void ResetFallVelosity()
    {
        Vector2 fall = rb.velocity;      //yだけ変えたいときはいったん新しいVecter3型の変数に代入する
        fall.y = 0.0f;                   //yを０にする
        rb.velocity = fall;               //yを変えた変数を代入し直す
    }

    /// <summary>
    /// 落下速度を管理する関数です
    /// </summary>
    /// <param name="rb">カービィー自身のRigidBody</param>
    private void ReduceFallSpeed(Rigidbody2D rb)
    {
        if(floating)
        {
            //if(moveState != KirbyState.Floating_Start || moveState != KirbyState.Jump_FloatJump)
            //{
            //    Anim_While_floating();
            //}
            rb.gravityScale = floatGravity;
        }
        else
        {
            rb.gravityScale = fallGravity;
        }

    }


    void AtackMethod()
    {
        if(!isGround && Input.GetButtonDown("Atack"))
        {
            floating = false;
            Breath();
        }
    }

    void Jump()
    {
        if (isGround && Input.GetButtonDown("Jump"))
        {
            //Debug.Log("jump");
            ResetFallVelosity();
            //rb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
            rb.velocity = new Vector2(tempVelisityX, jumpForce);
            //ジャンプしているときにジャンプダッシュするためのフラグ発動
            Anim_Jump_Start();
            SoundsManager.SE_Play(SE.Jump);
            return;
        }
        if (!isGround && Input.GetButtonDown("Jump"))
        {
            ResetFallVelosity();
            rb.velocity = new Vector2(tempVelisityX, floatjumpForce);
            SoundsManager.SE_Play(SE.fly);
            floating = true;
            if ((moveState == KirbyState.While_floating || moveState == KirbyState.Floating_Start
                 || moveState == KirbyState.Jump_FloatFalling || moveState == KirbyState.Jump_FloatJump)
                 && !ho.GetHit())
            {
                //同じアニメーション再生させてもループしないので落下中のアニメーションを挟まないといけない
                Anim_FloatJump();
                return;
            }
            else { Anim_Floating_Start(); }
        }

        if(!isGround && Mathf.Abs(rb.velocity.y) <= 0.5 
            && moveState != KirbyState.While_floating && moveState != KirbyState.Floating_End
            && !floating && !ho.GetHit())
        {
            Anim_Jump_StartFall();
            return;
        }


        if ( isGround && moveState == KirbyState.Jump_StartFall && !ho.GetHit())
        { Anim_Idle();  return; }
    }


    void FlipHorizontal(float axisH)
    {
        if (Mathf.Abs(axisH) > 0.1f)
        {
            Vector3 scale = transform.localScale;    //Xだけ変えたいときはいったん新しいVecter3型の変数に代入する

            scale.x = axisH > 0 ? 1 : -1;   //条件演算子　条件式　？　真の時　：　偽の時
            transform.localScale = scale;

        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ashiba"))
        {
            //Ashibaタグの足場を通り抜けたら、isTriggerをfalseにし、もう一回乗れるようにする
            var bc = collision.gameObject.GetComponent<BoxCollider2D>();
            bc.isTrigger = false;
        }
    }



}
