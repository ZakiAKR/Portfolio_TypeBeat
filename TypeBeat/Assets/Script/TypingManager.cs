using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;

// タイピング部分のソースコード

// インスペクター上から文字列を変種出来るようにする
[Serializable]
public class Question
{
    public string mondai;
    public string romaji;
}

public class TypingManager : MonoBehaviour
{
    //「Question」クラスのインスタンスを生成する
    [SerializeField] 
    private Question[] _questions = new Question[51];

    [Space(10)]

    // 問題の文字を表示するtextを取得
    [SerializeField] 
    private TextMeshProUGUI _textMondai;
    // 解答の文字を表示するtextを取得
    [SerializeField] 
    private TextMeshProUGUI _textRomaji;

    [Space(10)]

    // 問題と解答を表示する間隔の変数
    public float intervalWaitMoji;

    // タイピングの状態を格納するリストの変数
    private List<char> _kaitou = new List<char>();
    // リストの配列の要素数で使用されている変数
    private int _kaitouIndex = 0;

    // 打った文字の数を計測する変数
    private int _count;
    // 間違えて打った文字の数を数える変数
    private int _miss;

    // Start is called before the first frame update
    void Start()
    {
        // 初期化
        CountManager.countMojiNum = 0;
        CountManager.missMojiNum = 0;
        CountManager.kaitouMojiNum = 0;
    }

    // Update is called once per frame
    void Update()
    {
        // 打った文字の数を管理用の変数に代入
        CountManager.countMojiNum = _count;

        // 間違えて打った文字の数を管理用の変数に代入
        CountManager.missMojiNum = _miss;
    }

    // キー入力時に呼び出されるイベント関数
    private void OnGUI()
    {
        // 入力された情報がキーを押したときに入力の場合
        if (Event.current.type == EventType.KeyDown)
        {
            // 入力されたキーコードを変換して、変換した文字が正しい文字が判定した結果に酔って処理が変わる
            switch (InputKey(GetChange_KeyCode(Event.current.keyCode)))
            {
                case 1:
                case 2:
                    // 正解の文字を打った数を計測
                    _count++;

                    // １つ要素数を加算することでこの文字が空白だった場合、問題を初期化して新しい問題を出す。それ以外は文字の色を変える。
                    _kaitouIndex++;

                    // 特定の文字が空白だった場合の処理
                    if (_kaitou[_kaitouIndex] == ' ')
                    {
                        // 問題の初期化の関数を呼び出し
                        Initi_Question();
                    }
                    else
                    {
                        // 文字の色を変える
                        _textRomaji.text = Generate_Romaji();
                    }
                    break;
                case 3:
                    // 間違いの文字を打った数を計測
                    _miss++;
                    break;
            }
        }
    }

    //入力が正しいかを判定する関数
    int InputKey(char inputMoji)
    {
        // char 変数名 = 要素数が N または N 以上の場合 ? (true) N 前の文字の情報 : (false)null
        // 入力してる文字の１つ前の解答の文字の情報を保存する変数
        char prevChar = _kaitouIndex >= 1 ? _kaitou[_kaitouIndex - 1] : '\0';
        // 入力してる文字の２つ前の解答の文字の情報を保存する変数
        char prevChar2 = _kaitouIndex >= 2 ? _kaitou[_kaitouIndex - 2] : '\0';

        // 解答に記載されている文字の情報を保存する変数
        char currentMoji = _kaitou[_kaitouIndex];

        // 入力してる文字の１つ先の解答の文字の情報を保存する変数
        char nextChar = _kaitou[_kaitouIndex + 1];
        // char 変数名 = N つ先の文字が空白だった場合 ? (true) 空白 : (false) N つ先の文字の情報
        // 入力してる文字の２つ先の解答の文字の情報を保存する変数
        char nextChar2 = nextChar == ' ' ? ' ' : _kaitou[_kaitouIndex + 2];
        //入力している文字の３つ先の解答の文字の情報を保存する変数
        char nextChar3 = nextChar2 == ' ' ? ' ' : _kaitou[_kaitouIndex + 3];

        // 入力が無い場合
        if (inputMoji == '\0')
        {
            return 0;
        }

        // 入力が正しい場合
        if (inputMoji == currentMoji)
        {
            return 1;
        }

        // 例外処理
        // 「い」＝ i ->  yi
        // 「i」の母音を使う文字と「yi」を使う文字の区別をするため、１つ前の文字を「なし」「a」「i」「u」「e」「o」「n」に限定して、「i」の前に「y」を挿入する
        if (inputMoji == 'y' && currentMoji == 'i' && (prevChar=='\0'|| prevChar == 'a'|| prevChar == 'i' || prevChar == 'u' || prevChar == 'e' || prevChar == 'o' || prevChar == 'n'))
        {
            _kaitou.Insert(_kaitouIndex, 'y');

            // 解答の文字の数に加算
            CountManager.kaitouMojiNum++;
            return 2;
        }
        // 「う」＝ u  -> wu whu
        // 「u」の母音使う文字と区別をするため、１つ前の文字を「なし」「a」「i」「u」「e」「o」「n」に限定して、「u」の前に「w」を挿入する
        if (inputMoji == 'w' && currentMoji == 'u' && (prevChar == '\0' || prevChar == 'a' || prevChar == 'i' || prevChar == 'u' || prevChar == 'e' || prevChar == 'o' || prevChar == 'n'))
        {
            _kaitou.Insert(_kaitouIndex, 'w');

            // 解答の文字の数に加算
            CountManager.kaitouMojiNum++;
            return 2;
        }
        // 「う」＝ wu  -> whu
        // １つ前の文字を「w」に限定して、「u」の前に「h」を挿入する　※「qwu」「swu」「twu」「fwu」「gwu」「dwu」と区別するため、２つ前の文字を「なし」「a」「i」「u」「e」「o」「n」と限定する
        if (inputMoji == 'h' && currentMoji == 'u' && prevChar == 'w' && (prevChar2 == '\0' || prevChar2 == 'a' || prevChar2 == 'i' || prevChar2 == 'u' || prevChar2 == 'e' || prevChar2 == 'o' || prevChar2 == 'n'))
        {
            _kaitou.Insert(_kaitouIndex, 'h');

            // 解答の文字の数に加算
            CountManager.kaitouMojiNum++;
            return 2;
        }

        // 「か」＝ ka -> ca
        // 「く」＝ ku -> cu 
        // 「こ」＝ ko -> co
        // 「kka」「kku」「kko」と区別するため、１つ前の文字を「k」以外で１つ次の文字を「a」「u」「o」に限定して、「k」を「c」と置き換える
        if (inputMoji == 'c' && currentMoji == 'k' && prevChar != 'k' && (nextChar == 'a' || nextChar == 'u' || nextChar == 'o'))
        {
            _kaitou[_kaitouIndex] = 'c';
            return 2;
        }
        // 「く」＝ ku -> qu
        // 「kku」と区別するため、１つ前の文字を「k」以外で１つ次の文字を「u」に限定して、「k」を「q」と置き換える
        if (inputMoji == 'q' && currentMoji == 'k' && prevChar != 'k' && nextChar == 'u')
        {
            _kaitou[_kaitouIndex] = 'q';
            return 2;
        }
        // 「っか」＝ kka -> cca
        // 「っく」＝ kku -> ccu
        // 「っこ」＝ kko -> cco
        // １つ次の文字を「k」に、２つ次の文字を「a」「u」「o」に限定して、「kk」を「cc」と置き換える
        if (inputMoji == 'c' && currentMoji == 'k' && nextChar == 'k' && (nextChar2 == 'a' || nextChar2 == 'u' || nextChar2 == 'o'))
        {
            _kaitou[_kaitouIndex] = 'c';
            _kaitou[_kaitouIndex + 1] = 'c';
            return 2;
        }
        // 「っく」＝ kku -> qqu
        // １つ次の文字を「k」に、２つ次の文字を「u」に限定して、「kk」を「qq」と置き換える
        if (inputMoji == 'q' && currentMoji == 'k' && nextChar == 'k' && nextChar2 == 'u')
        {
            _kaitou[_kaitouIndex] = 'q';
            _kaitou[_kaitouIndex + 1] = 'q';
            return 2;
        }

        // 「し」＝ si -> shi
        // １つ前の文字を「s」に限定して、「i」の前に「h」を挿入する　※「tsi」と区別するため、２つ前の文字を「なし」「a」「i」「u」「e」「o」「n」と限定する
        if (inputMoji == 'h' && currentMoji == 'i' && prevChar == 's' && (prevChar2 == '\0' || prevChar2 == 'a' || prevChar2 == 'i' || prevChar2 == 'u' || prevChar2 == 'e' || prevChar2 == 'o' || prevChar2 == 'n'))
        {
            _kaitou.Insert(_kaitouIndex, 'h');

            // 解答の文字の数に加算
            CountManager.kaitouMojiNum++;
            return 2;
        }
        // 「し」＝ si -> ci 
        // 「せ」＝ se -> ce
        //  １つ次の文字を「i」「e」に限定して、「s」を「c」と置き換える　※「tsi」「tse」と区別するため、１つ前の文字を「なし」「a」「i」「u」「e」「o」「n」と限定する
        if (inputMoji == 'c' && currentMoji == 's' && (nextChar == 'i' || nextChar == 'e') && (prevChar == '\0' || prevChar == 'a' || prevChar == 'i' || prevChar == 'u' || prevChar == 'e' || prevChar == 'o' || prevChar == 'n'))
        {
            _kaitou[_kaitouIndex] = 'c';
            return 2;
        }
        // 「っし」＝ ssi -> cci 
        // 「っせ」＝ sse -> cce
        // １つ次の文字を「s」に２つ次の文字を「i」「e」に限定して、「ss」を「cc」と置き換える
        if (inputMoji == 'c' && currentMoji == 's' && nextChar == 's' && (nextChar2 == 'i' || nextChar2 == 'e'))
        {
            _kaitou[_kaitouIndex] = 'c';
            _kaitou[_kaitouIndex + 1] = 'c';
            return 2;
        }


        // 「ち」＝ ti -> chi
        // 「tti」と区別するため、１つ前の文字を「t」以外で１つ次の文字を「i」に限定して、「t」を「c」と置き換えて「i」の前に「h」を挿入する
        if (inputMoji == 'c' && currentMoji == 't' && prevChar != 't' && nextChar == 'i')
        {
            _kaitou[_kaitouIndex] = 'c';
            _kaitou.Insert(_kaitouIndex + 1, 'h');

            // 解答の文字の数に加算
            CountManager.kaitouMojiNum++;
            return 2;
        }
        // 「っち」＝ tti -> cchi
        // １つ次の文字を「t」に２つ次の文字を「i」に限定して、「tt」を「cc」と置き換えて「i」の前に「h」を挿入する
        if (inputMoji == 'c' && currentMoji == 't' && nextChar == 't' && nextChar2 == 'i')
        {
            _kaitou[_kaitouIndex] = 'c';
            _kaitou[_kaitouIndex + 1] = 'c';
            _kaitou.Insert(_kaitouIndex + 2, 'h');

            // 解答の文字の数に加算
            CountManager.kaitouMojiNum++;
            return 2;
        }
        // 「つ」＝ tu -> tsu
        // １つ前の文字を「t」に１つ次の文字を「u」に限定して、「u」の前に「s」を挿入する　※「xtu」と区別するため、２つ前の文字を「なし」「a」「i」「u」「e」「o」「n」と限定する
        if (inputMoji == 's' && currentMoji == 'u' && prevChar == 't' &&(prevChar2 == '\0' || prevChar2 == 'a' || prevChar2 == 'i' || prevChar2 == 'u' || prevChar2 == 'e' || prevChar2 == 'o' || prevChar2 == 'n'))
        {
            _kaitou.Insert(_kaitouIndex, 's');

            // 解答の文字の数に加算
            CountManager.kaitouMojiNum++;
            return 2;
        }

        // 「ふ」＝ hu -> fu
        //  １つ次の文字を「u」に限定して、「h」を「f」と置き換える　※「shu」「chu」「whu」「thu」「dhu」と区別するため、１つ前の文字を「なし」「a」「i」「u」「e」「o」「n」と限定する
        if (inputMoji == 'f' && currentMoji == 'h' && prevChar != 'h' && nextChar == 'u'&& (prevChar == '\0' || prevChar == 'a' || prevChar == 'i' || prevChar == 'u' || prevChar == 'e' || prevChar == 'o' || prevChar == 'n'))
        {
            _kaitou[_kaitouIndex] = 'f';
            return 2;
        }
        // 「っふ」＝ hhu -> ffu
        // １つ次の文字を「h」に、２つ次の文字を「u」に限定して、「hh」を「ff」と置き換える
        if (inputMoji == 'f' && currentMoji == 'h' && nextChar == 'h' && nextChar2 == 'u' )
        {
            _kaitou[_kaitouIndex] = 'f';
            _kaitou[_kaitouIndex + 1] = 'f';
            return 2;
        }

        // 「ん」＝ n -> nn
        // 「na」「ni」「nu」「ne」「no」「ya」「yi」「yu」「ye」「yo」と区別をするため、入力しようとしている解答の文字が「a」「i」「u」「e」「o」「y」以外で１つ前の文字を「n」に限定して、次の文字の前に「n」を挿入する　※「nn」と区別するため、２つ前の文字を「n」以外に限定する
        if (inputMoji == 'n' && currentMoji != 'a' && currentMoji != 'i' && currentMoji != 'u' && currentMoji != 'e' && currentMoji != 'o' && currentMoji != 'y' && prevChar == 'n' && prevChar2 != 'n')
        {
            _kaitou.Insert(_kaitouIndex, 'n');

            // 解答の文字の数に加算
            CountManager.kaitouMojiNum++;
            return 2;
        }
        // 「ん」＝ n -> xn
        // 「na」「ni」「nu」「ne」「no」「ya」「yi」「yu」「ye」「yo」と区別するため、１つ次の文字を「a」「i」「u」「e」「o」「y」以外で１つ前の文字を「n」以外に限定して、「n」の前に「x」を挿入する
        if (inputMoji == 'x' && currentMoji == 'n' && prevChar != 'n' && nextChar != 'a' && nextChar != 'i' && nextChar != 'u' && nextChar != 'e' && nextChar != 'o' && nextChar != 'y')
        {
            _kaitou.Insert(_kaitouIndex, 'x');
            
            // 解答の文字の数に加算
            CountManager.kaitouMojiNum++;
            return 2;
        }

        // 「じ」＝ ji -> zi
        // 「jji」と区別するため、１つ前の文字を「j」以外に１つ次の文字を「i」に限定して、「j」を「z」に置き換える
        if (inputMoji == 'z' && currentMoji == 'j' && prevChar!='j'&&nextChar == 'i')
        {
            _kaitou[_kaitouIndex] = 'z';
            return 2;
        }
        // 「じぃ」＝ jyi -> zyi
        // 「jjyi」と区別するため、１つ前の文字を「j」以外に１つ次の文字を「y」と２つ次の文字を「i」に限定して、「j」を「z」に置き換える
        if (inputMoji == 'z' && currentMoji == 'j' && prevChar!='j'&&nextChar == 'y' && nextChar2 == 'i')
        {
            _kaitou[_kaitouIndex] = 'z';
            return 2;
        }
        // 「じぇ」＝ je -> jye 
        // 「じゃ」＝ ja -> jya 
        // 「じゅ」＝ ju -> jyu 
        // 「じょ」＝ jo -> jyo 
        // １つ前の文字を「j」に限定して、「a」「u」「e」「o」の前に「y」を挿入する
        if (inputMoji == 'y' && (currentMoji == 'a' || currentMoji == 'u' || currentMoji == 'e' || currentMoji == 'o') && prevChar == 'j')
        {
            _kaitou.Insert(_kaitouIndex, 'y');
            
            // 解答の文字の数に加算
            CountManager.kaitouMojiNum++;
            return 2;
        }
        // 「じぇ」＝ je -> zye
        // 「じゃ」＝ ja -> zya
        // 「じゅ」＝ ju -> zyu
        // 「じょ」＝ jo -> zyo
        // 「jja」「jju」「jje」「jjo」と区別をするため、１つ前の文字を「j」以外に１つ次の文字を「a」「u」「e」「o」に限定して、「j」を「z」に置き換えて「a」「u」「e」「o」の前に「y」を挿入する
        if (inputMoji == 'z' && currentMoji == 'j' && prevChar!='j'&&(nextChar == 'a' || nextChar == 'u' || nextChar == 'e' || nextChar == 'o'))
        {
            _kaitou[_kaitouIndex] = 'z';
            _kaitou.Insert(_kaitouIndex+1, 'y');
            
            // 解答の文字の数に加算
            CountManager.kaitouMojiNum++;
            return 2;
        }
        // 「っじ」＝ jji -> zzi
        // １つ次の文字を「j」と２つ次の文字を「i」に限定して、「jj」を「zz」に置き換える
        if (inputMoji == 'z' && currentMoji == 'j' && nextChar == 'j' && nextChar2 == 'i')
        {
            _kaitou[_kaitouIndex] = 'z';
            _kaitou[_kaitouIndex + 1] = 'z';
            return 2;
        }
        // 「っじぃ」＝ jjyi -> zzyi
        // １つ次の文字を「j」と２つ次の文字を「y」と３つ次の文字を「i」に限定して、「jj」を「zz」に置き換える
        if (inputMoji == 'z' && currentMoji == 'j' && nextChar == 'j' && nextChar2 == 'y' && nextChar3 == 'i')
        {
            _kaitou[_kaitouIndex] = 'z';
            _kaitou[_kaitouIndex + 1] = 'z';
            return 2;
        }
        // 「っじぇ」＝ jje -> zzye
        // 「っじゃ」＝ jja -> zzya
        // 「っじゅ」＝ jju -> zzyu
        // 「っじょ」＝ jjo -> zzyo
        // １つ次の文字を「j」と２つ次の文字を「a」「u」「e」「o」に限定して、「jj」を「zz」に置き換えて「a」「u」「e」「o」の前に「y」を挿入する
        if (inputMoji == 'z' && currentMoji == 'j' && nextChar == 'j' && (nextChar2 == 'a' || nextChar2 == 'u' || nextChar2 == 'e' || nextChar2 == 'o'))
        {
            _kaitou[_kaitouIndex] = 'z';
            _kaitou[_kaitouIndex + 1] = 'z';
            _kaitou.Insert(_kaitouIndex + 2, 'y');
            
            // 解答の文字の数に加算
            CountManager.kaitouMojiNum++;
            return 2;
        }

        // 「ぁ」＝ la -> xa
        // 「ぃ」＝ li -> xi 
        // 「ぅ」＝ lu -> xu
        // 「ぇ」＝ le -> xe 
        // 「ぉ」＝ lo -> xo
        // 「ぃ」＝ lyi -> xyi
        // 「ぇ」＝ lye -> xye
        // 「ゃ」＝ lya -> xya
        // 「ゅ」＝ lyu -> xyu
        // 「ょ」＝ lyo -> xyo
        // 「っ」＝ ltu -> xtu
        // 「lla」「lli」「llu」「lle」「llo」「llya」「llyi」「llyu」「llye」「llyo」「ltu」と区別するため、１つ前の文字を「l」以外に限定して、「l」を「x」に置き換える
        if (inputMoji == 'x' && currentMoji == 'l' && prevChar != 'l'&& nextChar!='l' )
        {
            _kaitou[_kaitouIndex] = 'x';
            return 2;
        }
        // 「っぁ」＝ lla -> xxa
        // 「っぃ」＝ lli -> xxi
        // 「っぅ」＝ llu -> xxu
        // 「っぇ」＝ lle -> xxe
        // 「っぉ」＝ llo -> xxo
        // 「っぃ」＝ llyi -> xxyi
        // 「っぇ」＝ llye -> xxye
        // 「っゃ」＝ llya -> xxya
        // 「っゅ」＝ llyu -> xxyu
        // 「っょ」＝ llyo -> xxyo
        // 「っっ」＝ lltu -> xxtu
        // １つ次の文字を「l」と限定して、「ll」を「xx」に置き換える
        if (inputMoji == 'x' && currentMoji == 'l' && nextChar == 'l')
        {
            _kaitou[_kaitouIndex] = 'x';
            _kaitou[_kaitouIndex+1] = 'x';
            return 2;
        }

        // 「くぁ」＝ qa -> qwa
        // 「くぃ」＝ qi -> qwi 
        // 「くぇ」＝ qe -> qwe 
        // 「くぉ」＝ qo -> qwo
        // １つ前の文字を「q」に限定して、「a」「i」「e」「o」の前に「w」を挿入する
        if (inputMoji == 'w' && (currentMoji == 'a' || currentMoji == 'i' || currentMoji == 'e' || currentMoji == 'o') && prevChar == 'q')
        {
            _kaitou.Insert(_kaitouIndex, 'w');

            // 解答の文字の数に加算
            CountManager.kaitouMojiNum++;
            return 2;
        }

        // 「くぁ」＝ qa -> kwa
        // 「qqa」と区別するため、１つ前の文字を「q」以外に１つ次の文字を「a」に限定して、「q」を「k」に置き換えて「a」の前に「w」を挿入する
        if (inputMoji == 'k' && currentMoji == 'q' && prevChar != 'q' && nextChar == 'a' )
        {
            _kaitou[_kaitouIndex] = 'k';
            _kaitou.Insert(_kaitouIndex + 1, 'w');

            // 解答の文字の数に加算
            CountManager.kaitouMojiNum++;
            return 2;
        }

        // 「くぃ」＝ qi -> qyi
        // 「くぇ」＝ qe -> qye
        // １つ前の文字を「q」に限定して、「i」「e」の前に「y」を挿入する
        if (inputMoji == 'y' && (currentMoji == 'i' || currentMoji == 'e') && prevChar == 'q')
        {
            _kaitou.Insert(_kaitouIndex, 'y');
            CountManager.kaitouMojiNum++;
            return 2;
        }

        // 「っくぁ」＝ qqa -> kkwa
        // １つ次の文字を「q」に２つ次の文字を「a」に限定して、「qq」を「kk」に置き換えて「a」の前に「w」を挿入する
        if (inputMoji == 'k' && currentMoji == 'q' && nextChar == 'q' && nextChar2 == 'a')
        {
            _kaitou[_kaitouIndex] = 'k';
            _kaitou[_kaitouIndex+1] = 'k';
            _kaitou.Insert(_kaitouIndex + 2, 'w');

            // 解答の文字の数に加算
            CountManager.kaitouMojiNum++;
            return 2;
        }

        // 「しぇ」＝ she -> sye
        // 「しゃ」＝ sha -> sya
        // 「しゅ」＝ shu -> syu
        // 「しょ」＝ sho -> syo
        // １つ前の文字を「s」に１つ次の文字を「a」「u」「e」「o」に限定して、「h」を「y」に置き換える
        if (inputMoji == 'y' && currentMoji == 'h' && prevChar == 's' && (nextChar == 'a' || nextChar == 'u' || nextChar == 'e' || nextChar == 'o'))
        {
            _kaitou[_kaitouIndex] = 'y';
            return 2;
        }

        // 「ちぃ」＝ tyi -> cyi
        // 「ちぇ」＝ tye -> cye 
        // 「ちゃ」＝ tya -> cya 
        // 「ちゅ」＝ tyu -> cyu 
        // 「ちょ」＝ tyo -> cyo 
        // 「ttya」「ttyi」「ttyu」「ttye」「ttyo」の区別をするため、１つ前の文字を「t」以外に１つ次の文字を「y」と２つ次の文字を「a」「i」「u」「e」「o」に限定して、「t」を「c」に置き換える
        if (inputMoji == 'c' && currentMoji == 't' && prevChar!='t'&&nextChar == 'y' && (nextChar2 == 'a' || nextChar2 == 'i' || nextChar2 == 'u' || nextChar2 == 'e' || nextChar2 == 'o'))
        {
            _kaitou[_kaitouIndex] = 'c';
            return 2;
        }
        // 「ちぇ」＝ cye -> che　
        // 「ちゃ」＝ cya -> cha
        // 「ちゅ」＝ cyu -> chu
        // 「ちょ」＝ cyo -> cho
        // １つ前の文字を「c」に１つ次の文字を「a」「u」「e」「o」に限定して、「y」を「h」に置き換える
        if (inputMoji == 'h' && currentMoji == 'y' && prevChar == 'c' && (nextChar == 'a' || nextChar == 'u' || nextChar == 'e' || nextChar == 'o'))
        {
            _kaitou[_kaitouIndex] = 'h';
            return 2;
        }
        // 「っちぃ」＝ ttyi -> ccyi
        // 「っちぇ」＝ ttye -> ccye 
        // 「っちゃ」＝ ttya -> ccya 
        // 「っちゅ」＝ ttyu -> ccyu 
        // 「っちょ」＝ ttyo -> ccyo 
        // １つ次の文字を「t」と２つ次の文字を「y」に限定して、「tt」を「cc」に置き換える
        if (inputMoji == 'c' && currentMoji == 't' && nextChar == 't' && nextChar2 == 'y')
        {
            _kaitou[_kaitouIndex] = 'c';
            _kaitou[_kaitouIndex+1] = 'c';
            return 2;
        }

        // 「ふぁ」＝ fa -> fwa
        // 「ふぃ」＝ fi -> fwi
        // 「ふぇ」＝ fe -> fwe 
        // 「ふぉ」＝ fo -> fwo
        // １つ前の文字を「f」に限定して、「a」「i」「e」「o」の前に「w」を挿入する
        if (inputMoji == 'w' && (currentMoji == 'a' || currentMoji == 'i' || currentMoji == 'e' || currentMoji == 'o') && prevChar == 'f')
        {
            _kaitou.Insert(_kaitouIndex, 'w');

            // 解答の文字の数に加算
            CountManager.kaitouMojiNum++;
            return 2;
        }
        // 「ふぃ」＝ fi -> fyi
        // 「ふぇ」＝ fe -> fye
        // １つ前の文字を「f」に限定して、「i」の前に「y」を挿入する
        if (inputMoji == 'y' && (currentMoji == 'i' || currentMoji == 'e') && prevChar == 'f')
        {
            _kaitou.Insert(_kaitouIndex, 'y');

            // 解答の文字の数に加算
            CountManager.kaitouMojiNum++;
            return 2;
        }

        // 入力が間違っている場合
        return 3;
    }

    // 入力されたキーコードをchar型に変換する関数
    char GetChange_KeyCode(KeyCode keyCode)
    {
        switch (keyCode)
        {
            case KeyCode.A:
                return 'a';
            case KeyCode.B:
                return 'b';
            case KeyCode.C:
                return 'c';
            case KeyCode.D:
                return 'd';
            case KeyCode.E:
                return 'e';
            case KeyCode.F:
                return 'f';
            case KeyCode.G:
                return 'g';
            case KeyCode.H:
                return 'h';
            case KeyCode.I:
                return 'i';
            case KeyCode.J:
                return 'j';
            case KeyCode.K:
                return 'k';
            case KeyCode.L:
                return 'l';
            case KeyCode.M:
                return 'm';
            case KeyCode.N:
                return 'n';
            case KeyCode.O:
                return 'o';
            case KeyCode.P:
                return 'p';
            case KeyCode.Q:
                return 'q';
            case KeyCode.R:
                return 'r';
            case KeyCode.S:
                return 's';
            case KeyCode.T:
                return 't';
            case KeyCode.U:
                return 'u';
            case KeyCode.V:
                return 'v';
            case KeyCode.W:
                return 'w';
            case KeyCode.X:
                return 'x';
            case KeyCode.Y:
                return 'y';
            case KeyCode.Z:
                return 'z';
            case KeyCode.Minus:
                return '-';
            case KeyCode.Space:
                return ' ';
            default:
                return '\0';
        }
    }

    // 問題の初期化の関数
    public void Initi_Question()
    {
        // textの表示を消す
        _textMondai.text = "";
        _textRomaji.text = "";

        // 乱数で数値を生成
        int _random = UnityEngine.Random.Range(0, _questions.Length);

        // Questionクラスに配列を追加
        Question question = _questions[_random];

        // 要素数を初期化
        _kaitouIndex = 0;

        // リストの中身を空にする
        _kaitou.Clear();

        // Question.romaji（String型）をChar型の配列に変換
        char[] characters = question.romaji.ToCharArray();

        // Questionクラスの配列を_kaitouリストに追加する
        foreach (char character in characters)
        {
            _kaitou.Add(character);
        }

        // 文字列の最期に空白を追加して、「タイピングの終わり」を示す
        _kaitou.Add(' ');

        // 一問分の時間を初期化
        TimerManager._typeTime = TimerManager.typeTime;

        // 問題と解答の表示するタイミングをずらすためのコルーチン
        StartCoroutine(Display_Wait(question));

        // 解答で出された文字の数を加算していく
        CountManager.kaitouMojiNum += (_kaitou.Count - 1);
    }

    // 入力前と入力後の文字の色を変化して表示
    string Generate_Romaji()
    {
        // 文字の色をタグ機能で指定
        string text = "<style=typed>";

        // _kaitouリスト分処理を繰り返す
        for (int i = 0; i < _kaitou.Count; i++)
        {
            // 文字が空白あったら処理を飛ばす
            if (_kaitou[i] == ' ')
            {
                break;
            }
            // リストの要素数を合っていた場合に色を変える
            if (i == _kaitouIndex)
            {
                text += "</style><style=untyped>";
            }

            // 文字を代入する
            text += _kaitou[i];
        }
        // 文字の色を変える
        text += "</style>";

        return text;
    }

    // 問題と解答を表示する間隔を空けるための関数
    private IEnumerator Display_Wait(Question question)
    {
        yield return new WaitForSeconds(intervalWaitMoji);

        //問題を表示する
        _textMondai.text = question.mondai;

        yield return new WaitForSeconds(intervalWaitMoji);

        // 文字の色を半透明色にする
        _textRomaji.text = Generate_Romaji();
    }
}
