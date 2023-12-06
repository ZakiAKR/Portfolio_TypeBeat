using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// シーン遷移に関するソースコード

public class TranstionManager : MonoBehaviour
{
    // SEを流してからシーン遷移に移るまでの時間を保存する変数
    [SerializeField]
    private float seWaitTime;

    private void Start()
    {
        // ゲーム内の時間を進める
        Time.timeScale = 1;
    }

    public void To_Title()
    {
        // SEを鳴らした後時間を置くためのコルーチン
        StartCoroutine(Sound_SceneSE(0));
    }

    public void To_Main()
    {
        // SEを鳴らした後時間を置くためのコルーチン
        StartCoroutine(Sound_SceneSE(1));
    }

    public void To_Result()
    {
        // SEを鳴らした後時間を置くためのコルーチン
        StartCoroutine(Sound_SceneSE(2));
    }

    public void End_Game()
    {
        // SEを鳴らした後時間を置くためのコルーチン
        StartCoroutine(Sound_EndSE());
    }


    // SEを鳴らした後時間を置くためのコルーチン（シーン遷移用）
    private IEnumerator Sound_SceneSE(int numScene)
    {
        // ゲーム内の時間を進める
        Time.timeScale = 1;

        // SEが鳴り終わるまでの待ちの時間
        yield return new WaitForSeconds(seWaitTime);

        // 指定のシーンに遷移する
        SceneManager.LoadScene(numScene);
    }


    // SEを鳴らした後時間を置くためのコルーチン（ゲーム終了用）
    private IEnumerator Sound_EndSE()
    {
        // ゲーム内の時間を進める
        Time.timeScale = 1;

        // SEが鳴り終わるまでの待ちの時間
        yield return new WaitForSeconds(seWaitTime);

        // ゲームを閉じる処理
        Application.Quit();
    }
}
