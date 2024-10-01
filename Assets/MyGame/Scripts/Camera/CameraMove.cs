using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    private Transform tf;
    private Transform playerTf;
    [SerializeField] float cameraBottom = 1f;
    [SerializeField] float cameraSpeed = 2f;
    [SerializeField] private bool moveY = false;  //�c�X�N���[���ɂ������Ƃ�
    [SerializeField] private bool moveX = true;
    [Header("Rigidbody�n�̈ړ��Ȃ�Fixed��")]
    public UpdateType updatetype = UpdateType.Fixed;

    void Start()
    {
        tf = GetComponent<Transform>();
        var player = GameObject.FindGameObjectWithTag("Player");
        playerTf = player.GetComponent<Transform>();
    }

    //�σt���[�����[�g
    void Update()
    {
        //�J������X���W���v���C���[��X���W�Ɠ����ɂ�����@  
        //���ꂾ�ƃJ�����������ɂ��Ă���̂Ő����₷��
        //tf.position = new Vector3(playerTf.position.x, tf.position.y, tf.position.z);


        //�[�����ɗ�����~�߂���
        if (tf.position.x < 0)
        {
            tf.position = new Vector3(0, transform.position.y, tf.position.z);
        }

        if (tf.position.y <= cameraBottom)
        {
            tf.position = new Vector3(tf.position.x, cameraBottom, tf.position.z);
        }

    }

    //�σt���[�����[�g�iUpdate����Ɏ��s�����j�v���C���[��Translate�œ������Ȃ�
    private void LateUpdate()
    {
        if(updatetype == UpdateType.Late) 
        {
            cameraMove();
        }
    }

    //�Œ�t���[�����[�g(�v���C���[��RigidBody�ňړ�������Ȃ�)   Time.deltatime�������Ȃ��Ă���
    //����g���ƃv���C���[�̓������J�N�J�N���Ȃ�
    private void FixedUpdate()
    {
        if(updatetype == UpdateType.Fixed)
        {
            cameraMove();
        }
    }

    private void cameraMove()
    {
        var move = playerTf.position - tf.position;
        
        if (playerTf == null)
        {
            move = transform.position;
        }

        //�������Z�q�@�������@�H�@�@�^�̎��@�F�@�U�̎�
        move.y = moveY ? move.y : 0;
        move.x = moveX ? move.x : 0;
        move.z = 0;   //Z�����Ɉړ����ĉf��Ȃ��Ȃ�̂�h������
                                                      //Late�̎��@:  Fixed�̎�
        float time = updatetype == UpdateType.Late ? Time.deltaTime : 0.032f;
        tf.Translate(move * cameraSpeed * time);
    }

    public enum UpdateType
    { 
        Late,
        Fixed,
    }

}
