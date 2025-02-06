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
    /// ダッシュするときの十字キー二回押す動作を検出する
    /// </summary>
    /// <param name="leftorRight">検出する方向を指定する</param>
    /// <returns>二回押しの動作を検出出来たらtrueを返す</returns>
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

        ///チェックする方向によってチェックする変数は変わる
        bool checkDirection = false;

        //ダッシュ受付フレーム内に他の入力が入ってなくて
        //0(入力なし)があったら2回押しダッシュ成功

        //二回押しの際の一回ボタンを離した操作を検知する
        bool check = false;



        for (int i = 1; postponementFlame > i; i++)
        {
            if (leftorRight == LeftorRight.Left) { checkDirection = inputList[i].Left; }

            if (leftorRight == LeftorRight.Right) { checkDirection = inputList[i].Right; }

            //関係ない方向に入力されていたらダッシュする気が無いとみなしfalseにする
            if (!checkDirection && !inputList[i].Neutral)
            {
                //Debug.LogWarning("false");
                return false;
            }

            //二回押しの際の一回ボタンを離した操作を検知する
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
        //0番目に追加
        inputList.Insert(0, input);
        //Debug.Log(input.movekey + " " + inputList.Count);
        
        //５秒間とるから300位
        if(inputList.Count >= 60 * 5)
        {
            //最後の配列の番号を消す
            inputList.RemoveAt(inputList.Count -1);
        }

    }


}

public class GamePadClass
{
    /// <summary>
    /// レバーの番号を記録　０は入力なしのとき
    /// </summary>
    public int movekey = 0;

    //public bool right { get { return movekey == 6; }}
    //ラムダ式で書けばgetの式を簡単にかける

    //番号で判断するのはあれなのでbool型でとれるようにしたい
    public bool Left => movekey == 4;
    public bool Right => movekey == 6;
    public bool Up  => movekey == 8;
    public bool Down => movekey == 2;

    public bool Neutral => movekey == 0;

    float xInput = 0, YInput = 0;

    /// <summary>
    /// 注意:キー入力の情報は記録するので宣言するだけで大丈夫です
    /// </summary>
    public GamePadClass()
    {
        
    }


    /// <summary>
    /// 現在入力している十字キーの情報を取得する
    /// </summary>
    /// <returns></returns>
    public static GamePadClass GetInputData()
    {
        //staticにする場合変数全てが静的になってて全てここで完結している必要がある

        
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
    /// 消すかもしれない
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
