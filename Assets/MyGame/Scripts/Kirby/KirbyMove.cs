using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

//C++���ƃN���X����ł�nameSpace�łł���
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


    //Range(�ŏ��l, �ő�l)��Inspecter��ł������l�͈̔͂����߂���
    [Tooltip("�����X�s�[�h")]
    [SerializeField, Range(1f, 50f)] float moveSpeed;

    [Tooltip("�󒆂ł̈ڑ����x")]
    [SerializeField, Range(1f, 50f)] float floatMoveSpeed;

    [Tooltip("�_�b�V�����Ă��鎞�̃X�s�[�h")]
    [SerializeField] float dashSpeed = 10;

    [Tooltip("�W�����v��")]
    [SerializeField] float jumpForce;

    [Tooltip("���V���̃W�����v��")]
    [SerializeField] float floatjumpForce;


    [SerializeField, Tooltip("�����Ă��鎞�̃O���r�e�B�X�P�[��")]
    float fallGravity = 3f;

    [SerializeField, Tooltip("�����Ă��Ėc���ł����Ԃ̃O���r�e�B�X�P�[��")]
    float floatGravity = 3f;

    [SerializeField, Tooltip("����f�����Ƃ��ɏo�Ă����C�̃v���n�u")]
    public GameObject breathPrefab;

    [SerializeField, Tooltip("�n�ʂ̃��C���[")]
    LayerMask Ground;

    [SerializeField, Tooltip("�ǂ̃��C���[")]
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

    /// <summary>///  �n�ʂɗ����Ă���ǂ����̃t���O /// </summary>
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

        //�ړ��n�̏����͍U�����ɂ͍s��Ȃ�����
        if (kirbyAtaks.GetAtacking() == false�@&& !kirbyAtaks.GetAtacking() && !ho.GetHit())
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
        //�n��ɗ����Ă��鎞�̃A�j���[�V��������
        if (!isGround)
        {
           goto Anim_1_end;�@//Anim_1_end:��������Ă���228�s�ڂ�
        }
        if (IsWalking(axisH))
        {
            //SoundsManager.SE_Play(SE.OnLand);
            Anim_Walk();
            goto Anim_1_end;
            //Debug.Log("����");
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
        else//    ��
        {
            //�c���Ŗ���������ʏ�̂ɂ���
        }

    Anim_1_end:

        //moveX = axisH * 10;

        //axisH���|���邱�Ƃœ��͂��Ȃ������瓮���Ȃ��悤�ɂ���
        moveX = moveSpeed * axisH;

        //Vector2 move = new Vector2(moveX, 0); //�ŏI�I�ȓ���
        Vector2 move = new Vector2(moveX, tempVelisityY); //�ŏI�I�ȓ���

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

        if (axisH == 0 && isGround && moveState == KirbyState.Dash)   //�n�ʂɗ����Ă���Ƃ�����
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


            //Debug.Log("���f��");
            Anim_Floating_End();
            SoundsManager.SE_Play(SE.flyspit);
            //����f����������������
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

    /// <summary>/// �n�ʂɗ����Ă��邩�ǂ����𒲂ׂ� /// </summary>
    void OnGroundSearch()
    {
        //�T�[�N���L���X�g�͔��a�����������ĕǂ̔�����Ƃ��Ă��܂��̂�h���ł���

        //distance:  �u�������F�v�������Ă�邱�ƂŃp�b�g���ŉ��̈��������Ă邩�킩��₷���Ȃ�@�@�܂��������F�������Έ����̏��Ԃ��ς�����
        isGround = Physics2D.CircleCast(transform.position, 0.2f, Vector2.down, distance: 1f, Ground);
        //�߂�ǂ�����������@�@rd.velocity.y == 0�@�łƂ�̂�����
        var GroundTagName = isGround ? isGround.collider.gameObject.tag : "";
        //�T�[�N���L���X�g���n�ʂɓ���������A�����������̂̃^�O���Ƃ�


        if (GroundTagName == "Ashiba")
        {
            float v = Input.GetAxisRaw("Vertical");
            var bc = isGround.collider.gameObject.GetComponent<BoxCollider2D>();
            //isGround.collider.gameObject.GetComponent<BoxCollider2D>
            if (v < 0)
            {
                bc.isTrigger = true;
            }
            //���{�^�����������瑫�ꂪ���蔲����
        }

        if (isGround)
        {
            floating = false;
        }
    }

    private void ResetFallVelosity()
    {
        Vector2 fall = rb.velocity;      //y�����ς������Ƃ��͂�������V����Vecter3�^�̕ϐ��ɑ������
        fall.y = 0.0f;                   //y���O�ɂ���
        rb.velocity = fall;               //y��ς����ϐ�����������
    }

    /// <summary>
    /// �������x���Ǘ�����֐��ł�
    /// </summary>
    /// <param name="rb">�J�[�r�B�[���g��RigidBody</param>
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
            //�W�����v���Ă���Ƃ��ɃW�����v�_�b�V�����邽�߂̃t���O����
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
                //�����A�j���[�V�����Đ������Ă����[�v���Ȃ��̂ŗ������̃A�j���[�V���������܂Ȃ��Ƃ����Ȃ�
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
            Vector3 scale = transform.localScale;    //X�����ς������Ƃ��͂�������V����Vecter3�^�̕ϐ��ɑ������

            scale.x = axisH > 0 ? 1 : -1;   //�������Z�q�@�������@�H�@�^�̎��@�F�@�U�̎�
            transform.localScale = scale;

        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ashiba"))
        {
            //Ashiba�^�O�̑����ʂ蔲������AisTrigger��false�ɂ��A����������悤�ɂ���
            var bc = collision.gameObject.GetComponent<BoxCollider2D>();
            bc.isTrigger = false;
        }
    }



}
