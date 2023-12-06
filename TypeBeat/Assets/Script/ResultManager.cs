using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.Rendering.Universal;

// リザルト画面に関するソースコード

public class ResultManager : MonoBehaviour
{
    [Space(10)]

    // 打った文字の数を表示するtextを取得
    [SerializeField] 
    private TextMeshProUGUI _mojiNum;
    // 間違えて打った数を表示するtextを取得
    [SerializeField] 
    private TextMeshProUGUI _missNum;
    // 評価を表示するtextを取得
    [SerializeField] 
    private TextMeshProUGUI _evalation;

    [Space(10)]

    //「戻る」というボタンをオブジェクトとして取得
    [SerializeField] 
    private GameObject _selectButton;

    [Space(10)]

    // 文字の数を表示する待ちの時間を設定する変数
    [SerializeField]
    private float firstWaitTime;
    // 評価の文字を表示する待ちの時間を設定する変数
    [SerializeField]
    private float secondWaitTime;

    // ボタンの大きさが最大になる時間を設定する変数
    [SerializeField]
    private float maxTime;
    // ボタンの拡大縮小のスピード
    [SerializeField]
    private float moveSpeed;
    // 時間を保存する変数
    private float _time = 0;
    // 拡大縮小を切り替える判定をする変数
    private bool enlarge = true;

    // 打った文字の数を保存する変数
    private int _countMoji;
    // 間違えて打った文字の数を保存する変数
    private int _missMoji;
    // 解答の文字の数を保存する変数
    private int _kaitouMoji;
    // 文字を逃した数を計算して保存する変数
    private float _notMoji;

    [Space(10)]

    // 評価の基準となる割合を定義する変数
    [Header("Valuation Ratio")]
    [SerializeField]
    private float[] valuation = new float[3];

    // 解答の文字数と割合を計算した結果を保存する変数
    private int[] _ratioPoint=new int[3];

    [SerializeField]
    private float missPoint;

    // Start is called before the first frame update
    void Start()
    {
        // 打った文字の数を参照して保存用の変数に代入
        _countMoji = CountManager.countMojiNum;

        // 間違いの文字の数を参照して保存用の変数に代入
        _missMoji = CountManager.missMojiNum;

        // 解答の文字の数を参照して保存用の変数に代入
        _kaitouMoji = CountManager.kaitouMojiNum;

        // 文字を表示しないようにする処理
        _mojiNum.text = "";
        _missNum.text = "";
        _evalation.text = "";

        // 「戻る」のボタンを選択状態にしておく
        EventSystem.current.SetSelectedGameObject(_selectButton);

        // 解答の文字を打てなかった文字数を計算
        _notMoji = _kaitouMoji - _countMoji;

        // 解答の文字数から割り出した割合を保存用の変数に代入
        for (int i = 0; i < valuation.Length; i++)
        {
            _ratioPoint[i] = (int)(_kaitouMoji * valuation[i]);
        }

    }

    // Update is called once per frame
    void Update()
    {
        // 文字の演出をするコルーチン
        StartCoroutine(Set_Text());

        // ボタンが拡大縮小する演出の処理
        Scaling_Button(_selectButton);
    }

    // 表示する文字を時間を空けて表示する演出のコルーチン
    private IEnumerator Set_Text()
    {
        // 打った文字を表示するまでの待ちの時間
        yield return new WaitForSeconds(firstWaitTime);

        // 打った文字の数を表示する
        _mojiNum.text = _countMoji.ToString();

        // 間違えて打った文字を表示するまでの待ちの時間
        yield return new WaitForSeconds(firstWaitTime);

        // 間違えて打った文字の数を表示する
        _missNum.text = (_missMoji+(int)_notMoji).ToString();

        // 評価を表示するまでの待ちの時間
        yield return new WaitForSeconds(secondWaitTime);

        // 評価する基準の処理
        // 打った文字から間違えて打った文字（等倍）の数と逃した数(1.2倍)の数を足したものと計算
        int _totalPoint = _countMoji - (_missMoji + (int)(_notMoji * missPoint));

        // 評価を文字で表示
        if (_ratioPoint[0] <= _totalPoint)
        {
            _evalation.text = "A";
        }
        else if (_ratioPoint[0] > _totalPoint && _ratioPoint[1] <= _totalPoint)
        {
            _evalation.text = "B";
        }
        else if (_ratioPoint[1] > _totalPoint && _ratioPoint[2] <= _totalPoint)
        {
            _evalation.text = "C";
        }
        else if(_ratioPoint[2] > _totalPoint)
        {
            _evalation.text = "D";
        }
    }

    // ボタンを拡大縮小する演出の関数
    void Scaling_Button(GameObject image)
    {
        // 動きを滑らかにする処理
        float scallSize = Time.deltaTime * moveSpeed;

        // 拡大縮小を時間で切り替える処理
        if (_time < 0) { enlarge = true; }
        if (_time > maxTime) { enlarge = false; }

        // オブジェクトの大きさを変える処理
        if (enlarge)
        {
            // 拡大するために時間を増やす
            _time += Time.deltaTime;

            // オブジェクトの大きさを大きくする
            image.transform.localScale += new Vector3(scallSize, scallSize, scallSize);
        }
        else
        {
            // 縮小するために時間を減らす
            _time -= Time.deltaTime;

            // オブジェクトの大きさを小さくする
            image.transform.localScale -= new Vector3(scallSize, scallSize, scallSize);
        }
    }

}
