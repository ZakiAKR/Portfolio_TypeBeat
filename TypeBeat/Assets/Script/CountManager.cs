using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// タイピングの打った文字と間違えて打った文字の数を管理するソースコード

public class CountManager : MonoBehaviour
{
    // 打った文字の数を管理する変数
    [HideInInspector] 
    public static int countMojiNum;

    // 間違えて打った文字の数を管理する変数
    [HideInInspector] 
    public static int missMojiNum;

    // 解答として出された文字の数を管理する変数
    [HideInInspector] 
    public static int kaitouMojiNum;
}
