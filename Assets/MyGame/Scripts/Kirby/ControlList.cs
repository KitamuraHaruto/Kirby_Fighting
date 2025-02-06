using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum LeftorRight
{
    Left, Right 
};
public class ControlList : MonoBehaviour
{
    List<GamePadClass> inputList = new List<GamePadClass>();

    [SerializeField] int postponementFlame = 15;

    bool recordFrag = false;

    //public List<GamePadClass> InputList() => inputList;

    //private void Awake()
    //{
    //    InputRecord();
    //}

    void Start()
    {
        //InputRecord();
        StartCoroutine(moveableCoroutine());
    }

    IEnumerator moveableCoroutine()
    {
        recordFrag = false;
        yield return new WaitForSeconds(0.1f);
        recordFrag = true;
    }



    void Update()
    {
        InputRecord();
        //Debug.Log(inputList.Count);
    }


    public bool Dash_Left() { return DashFrag(LeftorRight.Left); }
    public bool Dash_Right() { return DashFrag(LeftorRight.Right); }

    /// <summary>
    /// �_�b�V������Ƃ��̏\���L�[��񉟂���������o����
    /// </summary>
    /// <param name="leftorRight">���o����������w�肷��</param>
    /// <returns>��񉟂��̓�������o�o������true��Ԃ�</returns>
    bool DashFrag(LeftorRight leftorRight)
    {
        if (!recordFrag)
        {
            Debug.Log("srefweraeraeraeregr");
            return false;
        }

        var currentInput = inputList[0];

        //Debug.Log(currentInput.movekey);

        if(leftorRight == LeftorRight.Left && !currentInput.Left){  return false; }

        if (leftorRight == LeftorRight.Right && !currentInput.Right) { return false; }

        ///�`�F�b�N��������ɂ���ă`�F�b�N����ϐ��͕ς��
        bool checkDirection = false;

        //�_�b�V����t�t���[�����ɑ��̓��͂������ĂȂ���
        //0(���͂Ȃ�)����������2�񉟂��_�b�V������

        //��񉟂��̍ۂ̈��{�^���𗣂�����������m����
        bool check = false;



        for (int i = 1; postponementFlame > i; i++)
        {
            if (leftorRight == LeftorRight.Left) { checkDirection = inputList[i].Left; }

            if (leftorRight == LeftorRight.Right) { checkDirection = inputList[i].Right; }

            //�֌W�Ȃ������ɓ��͂���Ă�����_�b�V������C�������Ƃ݂Ȃ�false�ɂ���
            if (!checkDirection && !inputList[i].Neutral)
            {
                //Debug.LogWarning("false");
                return false;
            }

            //��񉟂��̍ۂ̈��{�^���𗣂�����������m����
            if (inputList[i].Neutral && check == false)
            {
                check = true;
            }

            if (checkDirection && check == true)
            {
                //Debug.LogWarning("true");
                return true;
            }

        }
        //Debug.LogWarning("false");
        return false;

    }


    public void InputRecord()
    {
        var input = GamePadClass.GetInputData();
        //inputList.Add(input);
        //0�Ԗڂɒǉ�
        inputList.Insert(0, input);
        //Debug.Log(input.movekey + " " + inputList.Count);
        
        //�T�b�ԂƂ邩��300��
        if(inputList.Count >= 60 * 5)
        {
            //�Ō�̔z��̔ԍ�������
            inputList.RemoveAt(inputList.Count -1);
        }

    }


}

public class GamePadClass
{
    /// <summary>
    /// ���o�[�̔ԍ����L�^�@�O�͓��͂Ȃ��̂Ƃ�
    /// </summary>
    public int movekey = 0;

    //public bool right { get { return movekey == 6; }}
    //�����_���ŏ�����get�̎����ȒP�ɂ�����

    //�ԍ��Ŕ��f����̂͂���Ȃ̂�bool�^�łƂ��悤�ɂ�����
    public bool Left => movekey == 4;
    public bool Right => movekey == 6;
    public bool Up  => movekey == 8;
    public bool Down => movekey == 2;

    public bool Neutral => movekey == 0;

    float xInput = 0, YInput = 0;

    /// <summary>
    /// ����:�L�[���͂̏��͋L�^����̂Ő錾���邾���ő��v�ł�
    /// </summary>
    public GamePadClass()
    {
        
    }


    /// <summary>
    /// ���ݓ��͂��Ă���\���L�[�̏����擾����
    /// </summary>
    /// <returns></returns>
    public static GamePadClass GetInputData()
    {
        //static�ɂ���ꍇ�ϐ��S�Ă��ÓI�ɂȂ��ĂđS�Ă����Ŋ������Ă���K�v������

        
        GamePadClass gamePadClass = new GamePadClass();

        int movekey = 0;

        float xInput = Input.GetAxisRaw("Horizontal");
        float YInput = Input.GetAxisRaw("Vertical");

        if (xInput > 0.7) { movekey = 6; }
        if (xInput < -0.7) { movekey = 4; }
        if (YInput > 0.7) { movekey = 8; }
        if (YInput < -0.7) { movekey = 2; }

        gamePadClass.movekey = movekey;

        return gamePadClass;
    }

    /// <summary>
    /// ������������Ȃ�
    /// </summary>
    //public void CurrentInput ()
    //{
    //    xInput = Input.GetAxisRaw("Horizontal");
    //    YInput = Input.GetAxisRaw("Vertical");

    //    if(xInput > 0.7) { Right = true;} 
    //    if(xInput < -0.7) { Left = true;} 
    //    if(YInput > 0.7) { up = true;} 
    //    if(YInput < -0.7) { down = true;} 

    //}

}
