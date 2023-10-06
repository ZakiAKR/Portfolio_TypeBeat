using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TitleButtonManager : MonoBehaviour
{
    // タイトル画面のボタンをオブジェクトとして取得
    [SerializeField] GameObject[] _selectButton=new GameObject[2];
    
    [Space(10)]

    // 拡大縮小する大きさを設定する変数
    public float scallSize;
    // ボタンの大きさが最大になる時間を設定する変数
    public float maxTime;
    // ボタンの拡大縮小のスピード
    public float moveSpeed;
    // 時間を保存する変数
    private float time;
    // 拡大縮小を切り替える判定をする変数
    private bool enlarge = true;

    // ボタンの初期の大きさを保存するための変数
    private Vector3[] _seleceScale = new Vector3[2];

    // 選択中のボタンの情報を保存するための変数
    private GameObject _button;

    // Start is called before the first frame update
    void Start()
    {
        //「Start」ボタンを選択状態にする
        EventSystem.current.SetSelectedGameObject(_selectButton[0]);

        // すべてのボタンの初期の大きさを保存する
        for (int i = 0; i < _selectButton.Length; i++)
        {
            _seleceScale[i] = _selectButton[i].transform.localScale;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // 選択中のボタンの情報を保存する
        _button = EventSystem.current.currentSelectedGameObject;

        // ボタンの演出処理
        if (_button == _selectButton[0])
        {
            // ボタンの大きさを拡大縮小する処理
            Button_Scaling(_selectButton[0]);
            
            // 選択されていないボタンを初期の大きさに直す処理
            _selectButton[1].transform.localScale = Reset_ImageScale(_seleceScale[1]);
        }
        if (_button == _selectButton[1])
        {
            // ボタンの大きさを拡大縮小する処理
            Button_Scaling(_selectButton[1]);

            // 選択されていないボタンを初期の大きさに直す処理
            _selectButton[0].transform.localScale = Reset_ImageScale(_seleceScale[0]);
        }
    }

    // ボタンの大きさを拡大縮小する演出の関数
    void Button_Scaling(GameObject image)
    {
        // 動きを滑らかにする処理
        scallSize = Time.deltaTime * moveSpeed;

        // 拡大縮小を時間で切り替える処理
        if (time < 0) { enlarge = true; }
        if (time > maxTime) { enlarge = false; }

        // オブジェクトの大きさを変える処理
        if (enlarge)
        {
            // 拡大するために時間を増やす
            time += Time.deltaTime;

            // オブジェクトの大きさを大きくする
            image.transform.localScale += new Vector3(scallSize, scallSize, scallSize);
        }
        else
        {
            // 縮小するために時間を減らす
            time -= Time.deltaTime;

            // オブジェクトの大きさを小さくする
            image.transform.localScale -= new Vector3(scallSize, scallSize, scallSize);
        }
    }

    // ボタンの大きさを初期の大きさに戻す関数
    Vector3 Reset_ImageScale(Vector3 afterObj)
    {
        return afterObj;
    }
}
