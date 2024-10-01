using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.SymbolStore;
using UnityEditor;
using UnityEngine;

public class PlayerMoveByVelosity : MonoBehaviour
{
    private GameObject player;
    private Transform playerTf;
    private Animator animator;
    private AudioSource AS ;

    [SerializeField] AudioClip jumpSE; 
    [SerializeField] AudioClip dashSE;
    [SerializeField] AudioClip graidSE;
    [SerializeField] float dashSpeed = 10;
    [SerializeField] float glaidSpeed = 10;
    [Header("���󎞂̗����X�s�[�h")]
    [SerializeField] float glaidFallSpeed = 5;
    public bool DashFrag;
    bool graid = false;
    private float h;
    [SerializeField] private bool DashJumpFrag;
    [SerializeField] private float DashTime;
    [SerializeField] float EndDashTime;

    //Range(�ŏ��l, �ő�l)��Inspecter��ł������l�͈̔͂����߂���
    [SerializeField, Range(1f, 50f)] float moveSpeed;
    [SerializeField] float jumpForce;
    [SerializeField] LayerMask Ground;
    [SerializeField] LayerMask wall;

    private Rigidbody2D rd;
    private TrailRenderer tl;
    float moveX;

    void Start()
    {
        AS = GetComponent<AudioSource>();
        tl = GetComponent<TrailRenderer>();
        rd = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        animator.SetBool("Dash", DashFrag);
        animator.SetBool("Graid", graid);

        //distance:  �u�������F�v�������Ă�邱�ƂŃp�b�g���ŉ��̈��������Ă邩�킩��₷���Ȃ�@�@�܂��������F�������Έ����̏��Ԃ��ς�����
        RaycastHit2D isGround = Physics2D.CircleCast(transform.position, 1.0f, Vector2.down, distance: 0.4f, Ground);
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

        h = Input.GetAxisRaw("Horizontal");  


        var tempVeloisityY = rd.velocity.y; var tempVelisityX = rd.velocity.x;
        //Jump(tempVelisityX, tempVeloisityY, isGround);

        RaycastHit2D isWall = Physics2D.CircleCast(transform.position, 0.5f, new Vector2(h, 0), 0.3f, wall);

        if (!isWall)
        {
            MoveHorizontal(h, tempVeloisityY, isGround);
        }

        FlipHorizontal(h);


        if (isGround && Input.GetButtonDown("Jump"))
        {
            ResetFallVelosity();
            AS.PlayOneShot(jumpSE);
            rd.velocity = new Vector2(tempVelisityX, jumpForce);
            DashJumpFrag = DashFrag;
            //�W�����v���Ă���Ƃ��ɃW�����v�_�b�V�����邽�߂̃t���O����
        }

        graid = false;
        if(rd.velocity.y < 0 && Input.GetButton("Jump"))
        {
            Gliding();
        }


    }

    private void MoveHorizontal(float axisH, float tempVelisityY, bool isGround)
    {

        moveX = axisH * 10;�@

        if (DashJumpFrag)�@�@
        {
            DashJump(axisH, moveX ,isGround);
        }
        else if (DashFrag)�@�@//���ʂɒn�ʂ��_�b�V�����Ă���Ƃ�
        {
            //�摜�̃X�P�[���̌����̕����Ƀ_�b�V������@�@�������̉摜�p
            moveX = dashSpeed * (transform.localScale.x * -1);
            DashTime += Time.deltaTime;
        }

        Vector2 move = new Vector2(moveX, tempVelisityY); //�ŏI�I�ȓ���

        if (isGround && Input.GetButtonDown("Dash"))
        {
            AS.PlayOneShot(dashSE);
            DashFrag = true;
            move.y = 0.0f;
        }
        rd.velocity = move;

        if (DashTime > EndDashTime)
        {
            DashTime = 0.0f;
            DashFrag = false;
        }

        if (DashFrag)
        {
            tl.emitting = true;
        }
        else
        {
            TrailStop();
        }


    }

    void TrailStop()
    {
        tl.emitting = false;
    }

    void DashJump(float axisH, float moveX ,bool isGround )
    {
            if (Mathf.Abs(axisH) > 0.5f)
            {
                //�摜�̃X�P�[���̌����̕����Ƀ_�b�V������@�@�������̉摜�p
                moveX = dashSpeed * (axisH < 0.0f ? -1.0f : 1.0f);
            }
            else
            {
                //�摜�̃X�P�[���̌����̕����Ƀ_�b�V������@�@�������̉摜�p
                moveX = dashSpeed * (transform.localScale.x * -1);
            }
            DashTime += Time.deltaTime;

            if(rd.velocity.y < 0 && isGround)�@�@//�_�b�V���W�����v�I���̏���
            {
                DashJumpFrag = false;
            }

    }


    private void ResetFallVelosity()
    {
        Vector2 fall = rd.velocity;      //y�����ς������Ƃ��͂�������V����Vecter3�^�̕ϐ��ɑ������
        fall.y = 0.0f;                   //y���O�ɂ���
        rd.velocity = fall;               //y��ς����ϐ�����������
    }

    private void Gliding()
    {
        //AS.PlayOneShot(graidSE);
        Vector3 gridMoveHorizontal = new Vector3(glaidSpeed * -transform.localScale.x, -glaidFallSpeed, 0);
        rd.velocity = gridMoveHorizontal;
        //Debug.Log("Gliding");
        graid = true;
    }


    void FlipHorizontal(float axisH)
    {
        if (Mathf.Abs(axisH) > 0.1f)
        {
            Vector3 scale = transform.localScale;    //X�����ς������Ƃ��͂�������V����Vecter3�^�̕ϐ��ɑ������

            scale.x = axisH > 0 ? -1 : 1;   //�������Z�q�@�������@�H�@�^�̎��@�F�@�U�̎�
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
