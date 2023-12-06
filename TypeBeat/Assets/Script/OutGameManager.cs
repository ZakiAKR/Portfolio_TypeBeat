using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

// ポーズ画面やメインのシステム以外に関わるソースコード

public class OutGameManager : MonoBehaviour
{
    // カウントダウンと全体の時間の判定を使用するために「TimerManager」を取得
    [SerializeField] 
    private TimerManager _timeSystem;

    [Space(10)]

    // 戻るボタンが押されたときに音に関しての処理を使用するため、「AudioManager」を取得
    [SerializeField] 
    private AudioManager _audioSystem;

    [Space(10)]

    // 全体の時間が終わった後にメインのシステムが続かないようにするために「InGameSystem」を取得
    [SerializeField] 
    private GameObject _typeingSystem;

    [Space(10)]

    // ESCキーが押されたときに表示するパネル
    [SerializeField] 
    private GameObject _endPanel;

    [Space(10)]

    // ポーズ画面のボタンをオブジェクトとして取得
    [SerializeField] 
    private GameObject[] _selectButton = new GameObject[3];
    // 選択されていないときのボタンの画像をオブジェクトとして取得
    [SerializeField] 
    private GameObject[] _backButton = new GameObject[3];

    // ポーズ画面が表示されたかを判定するための変数
    [HideInInspector] 
    public bool isPose;

    // Start is called before the first frame update
    void Start()
    {
        // 初期化
        _endPanel.SetActive(false);
        isPose = false;
    }

    // Update is called once per frame
    void Update()
    {
        // 選択中のボタンの情報を保存
        GameObject _button = EventSystem.current.currentSelectedGameObject;

        // カウントダウン後と全体の時間内にESCキーが使用できるようにする処理
        if (_timeSystem.isCountDown&& !_timeSystem.isFinish && Input.GetKeyDown(KeyCode.Escape))
        {
            // ポーズ画面が表示されたことを設定する
            isPose = true;

            // ゲーム内の時間を止める
            Time.timeScale = 0f;

            // BGM、SEに関しての処理
            _audioSystem.IsPose_Sound();

            // パネルを表示する
            _endPanel.SetActive(true);

            // 最初に選択状態にしておくボタンを設定する
            EventSystem.current.SetSelectedGameObject(_selectButton[0]);
        }

        // 全体の時間が終わった場合
        if (_timeSystem.isFinish)
        {
            // InGameSystemを使用できなくする
            _typeingSystem.SetActive(false);
        }

        // ボタンの演出処理
        if (_button == _selectButton[0])
        {
            // ボタンの演出の関数
            Button_Direction(0, 1, 2);
        }
        if (_button == _selectButton[1])
        {
            // ボタンの演出の関数
            Button_Direction(1, 0, 2);
        }
        if (_button == _selectButton[2])
        {
            // ボタンの演出の関数
            Button_Direction(2, 0, 1);
        }
    }

    // Escキーを押したときに出る「続きへRETUEN」のための関数
    public void OnClick_ReturnButton()
    {
        // ポーズ画面が表示されたことを設定する
        isPose = false;

        // パネルを非表示にする
        _endPanel.SetActive(false);

        // BGM、SEに関しての処理
        _audioSystem.OnClick_Button();

        // ゲーム内の時間を進める
        Time.timeScale = 1;
    }

    // 選択中と選択中以外のボタンの演出の関数
    void Button_Direction(int falseNum, int trueNum1, int trueNum2)
    {
        // 隠しの画像を非表示にする
        _backButton[falseNum].SetActive(false);

        // 隠しの画像を表示する
        _backButton[trueNum1].SetActive(true);
        _backButton[trueNum2].SetActive(true);
    }
}
