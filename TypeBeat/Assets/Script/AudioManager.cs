using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

// 音に関するソースコード

public class AudioManager : MonoBehaviour
{
    //「SEAudioSource」を取得
    [SerializeField] private AudioSource _seAudioSource;
    // SEの音源を取得
    [SerializeField] private AudioClip[] seSound = new AudioClip[28];

    [Space(10)]

    //「BGMAudioSource」を取得
    [SerializeField] private AudioSource _bgmAudioSource;

    [Space(10)]

    // ポーズ画面のBGMを流すために、「PoseAudioSource」を取得
    [SerializeField] private AudioSource _poseAudioSource;

    [Space(10)]

    // Audioの音量を調節するための変数
    public float soundVolume;
    // Audioの音量を１で固定するための変数
    public float maxVolume = 1f;

    // Update is called once per frame
    void Update()
    {
        // アルファベット'A'～'Z'と'ー'と'Space'キーのSE
        if (Input.GetKey(KeyCode.A)) { Sound_StartSorce(0, soundVolume); }
        if (Input.GetKeyUp(KeyCode.A)) { Sound_EndScorce(); }
        if (Input.GetKey(KeyCode.B)) { Sound_StartSorce(1, soundVolume); }
        if (Input.GetKeyUp(KeyCode.B)) { Sound_EndScorce(); }
        if (Input.GetKey(KeyCode.C)) { Sound_StartSorce(2, soundVolume); }
        if (Input.GetKeyUp(KeyCode.C)) { Sound_EndScorce(); }
        if (Input.GetKey(KeyCode.D)) { Sound_StartSorce(3, soundVolume); }
        if (Input.GetKeyUp(KeyCode.D)) { Sound_EndScorce(); }
        if (Input.GetKey(KeyCode.E)) { Sound_StartSorce(4, soundVolume); }
        if (Input.GetKeyUp(KeyCode.E)) { Sound_EndScorce(); }
        if (Input.GetKey(KeyCode.F)) { Sound_StartSorce(5, soundVolume); }
        if (Input.GetKeyUp(KeyCode.F)) { Sound_EndScorce(); }
        if (Input.GetKey(KeyCode.G)) { Sound_StartSorce(6, soundVolume); }
        if (Input.GetKeyUp(KeyCode.G)) { Sound_EndScorce(); }
        if (Input.GetKey(KeyCode.H)) { Sound_StartSorce(7, soundVolume); }
        if (Input.GetKeyUp(KeyCode.H)) { Sound_EndScorce(); }
        if (Input.GetKey(KeyCode.I)) { Sound_StartSorce(8, soundVolume); }
        if (Input.GetKeyUp(KeyCode.I)) { Sound_EndScorce(); }
        if (Input.GetKey(KeyCode.J)) { Sound_StartSorce(9, soundVolume); }
        if (Input.GetKeyUp(KeyCode.J)) { Sound_EndScorce(); }
        if (Input.GetKey(KeyCode.K)) { Sound_StartSorce(10, soundVolume); }
        if (Input.GetKeyUp(KeyCode.K)) { Sound_EndScorce(); }
        if (Input.GetKey(KeyCode.L)) { Sound_StartSorce(11, soundVolume); }
        if (Input.GetKeyUp(KeyCode.L)) { Sound_EndScorce(); }
        if (Input.GetKey(KeyCode.M)) { Sound_StartSorce(12, soundVolume); }
        if (Input.GetKeyUp(KeyCode.M)) { Sound_EndScorce(); }
        if (Input.GetKey(KeyCode.N)) { Sound_StartSorce(13, soundVolume); }
        if (Input.GetKeyUp(KeyCode.N)) { Sound_EndScorce(); }
        if (Input.GetKey(KeyCode.O)) { Sound_StartSorce(14, soundVolume); }
        if (Input.GetKeyUp(KeyCode.O)) { Sound_EndScorce(); }
        if (Input.GetKey(KeyCode.P)) { Sound_StartSorce(15, soundVolume); }
        if (Input.GetKeyUp(KeyCode.P)) { Sound_EndScorce(); }
        if (Input.GetKey(KeyCode.Q)) { Sound_StartSorce(16, soundVolume); }
        if (Input.GetKeyUp(KeyCode.Q)) { Sound_EndScorce(); }
        if (Input.GetKey(KeyCode.R)) { Sound_StartSorce(17, soundVolume); }
        if (Input.GetKeyUp(KeyCode.R)) { Sound_EndScorce(); }
        if (Input.GetKey(KeyCode.S)) { Sound_StartSorce(18, soundVolume); }
        if (Input.GetKeyUp(KeyCode.S)) { Sound_EndScorce(); }
        if (Input.GetKey(KeyCode.T)) { Sound_StartSorce(19, soundVolume); }
        if (Input.GetKeyUp(KeyCode.T)) { Sound_EndScorce(); }
        if (Input.GetKey(KeyCode.U)) { Sound_StartSorce(20, soundVolume); }
        if (Input.GetKeyUp(KeyCode.U)) { Sound_EndScorce(); }
        if (Input.GetKey(KeyCode.V)) { Sound_StartSorce(21, soundVolume); }
        if (Input.GetKeyUp(KeyCode.V)) { Sound_EndScorce(); }
        if (Input.GetKey(KeyCode.W)) { Sound_StartSorce(22, soundVolume); }
        if (Input.GetKeyUp(KeyCode.W)) { Sound_EndScorce(); }
        if (Input.GetKey(KeyCode.X)) { Sound_StartSorce(23, soundVolume); }
        if (Input.GetKeyUp(KeyCode.X)) { Sound_EndScorce(); }
        if (Input.GetKey(KeyCode.Y)) { Sound_StartSorce(24, soundVolume); }
        if (Input.GetKeyUp(KeyCode.Y)) { Sound_EndScorce(); }
        if (Input.GetKey(KeyCode.Z)) { Sound_StartSorce(25, soundVolume); }
        if (Input.GetKeyUp(KeyCode.Z)) { Sound_EndScorce(); }
        if (Input.GetKey(KeyCode.Minus)) { Sound_StartSorce(26, soundVolume); }
        if (Input.GetKeyUp(KeyCode.Minus)) { Sound_EndScorce(); }
        if (Input.GetKey(KeyCode.Space)) { Sound_StartSorce(27, maxVolume); }
    }

    // ボタンが押されたときに決定音を鳴らす関数
    public void On_ClickButtonSE()
    {
        Sound_StartSorce(27, maxVolume);
    }

    // 音を鳴らす関数
    void Sound_StartSorce(int num, float volume)
    {
        // 音のボリュームを指定
        _seAudioSource.volume = volume;

        // 指定した音を鳴らす処理
        _seAudioSource.PlayOneShot(seSound[num]);
    }

    // 音を止める関数
    void Sound_EndScorce()
    {
        // 鳴っている音を止める処理
        _seAudioSource.Stop();
    }

    // ポーズ画面が表示されているときの音に関する処理の関数
    public void IsPose_Sound()
    {
        // ポーズ画面のBGMを再生する
        _poseAudioSource.Play();

        // SEとBGMの音量を０にする
        _seAudioSource.Stop();
        _bgmAudioSource.Stop();
    }

    // ポーズ画面の戻るボタンが押されたときの音に関する処理の関数
    public void OnClick_Button()
    {
        // ポーズ画面のBGMを止める
        _poseAudioSource.Stop();

        // SEとBGMの音量を上げる
        _seAudioSource.Play();
        _seAudioSource.volume = soundVolume;
        _bgmAudioSource.Play();
    }
}
