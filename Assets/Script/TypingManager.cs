using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;

// �^�C�s���O�����̃\�[�X�R�[�h

// �C���X�y�N�^�[�ォ�當�����ώ�o����悤�ɂ���
[Serializable]
public class Question
{
    public string mondai;
    public string romaji;
}

public class TypingManager : MonoBehaviour
{
    //�uQuestion�v�N���X�̃C���X�^���X�𐶐�����
    [SerializeField] private Question[] _questions = new Question[51];

    [Space(10)]

    // ���̕�����\������text���擾
    [SerializeField] private TextMeshProUGUI _textMondai;
    // �𓚂̕�����\������text���擾
    [SerializeField] private TextMeshProUGUI _textRomaji;

    [Space(10)]

    // ���Ɖ𓚂�\������Ԋu�̕ϐ�
    public float intervalWaitMoji;

    // �^�C�s���O�̏�Ԃ��i�[���郊�X�g�̕ϐ�
    private List<char> _kaitou = new List<char>();
    // ���X�g�̔z��̗v�f���Ŏg�p����Ă���ϐ�
    private int _kaitouIndex = 0;

    // �ł��������̐����v������ϐ�
    private int _count;
    // �ԈႦ�đł��������̐��𐔂���ϐ�
    private int _miss;

    // Start is called before the first frame update
    void Start()
    {
        // ������
        CountManager.countMojiNum = 0;
        CountManager.missMojiNum = 0;
        CountManager.kaitouMojiNum = 0;
    }

    // Update is called once per frame
    void Update()
    {
        // �ł��������̐����Ǘ��p�̕ϐ��ɑ��
        CountManager.countMojiNum = _count;

        // �ԈႦ�đł��������̐����Ǘ��p�̕ϐ��ɑ��
        CountManager.missMojiNum = _miss;
    }

    // �L�[���͎��ɌĂяo�����C�x���g�֐�
    private void OnGUI()
    {
        // ���͂��ꂽ��񂪃L�[���������Ƃ��ɓ��͂̏ꍇ
        if (Event.current.type == EventType.KeyDown)
        {
            // ���͂��ꂽ�L�[�R�[�h��ϊ����āA�ϊ��������������������������肵�����ʂɐ����ď������ς��
            switch (InputKey(GetChange_KeyCode(Event.current.keyCode)))
            {
                case 1:
                case 2:
                    // �����̕�����ł��������v��
                    _count++;

                    // �P�v�f�������Z���邱�Ƃł��̕������󔒂������ꍇ�A�������������ĐV���������o���B����ȊO�͕����̐F��ς���B
                    _kaitouIndex++;

                    // ����̕������󔒂������ꍇ�̏���
                    if (_kaitou[_kaitouIndex] == ' ')
                    {
                        // ���̏������̊֐����Ăяo��
                        Initi_Question();
                    }
                    else
                    {
                        // �����̐F��ς���
                        _textRomaji.text = Generate_Romaji();
                    }
                    break;
                case 3:
                    // �ԈႢ�̕�����ł��������v��
                    _miss++;
                    break;
            }
        }
    }

    //���͂����������𔻒肷��֐�
    int InputKey(char inputMoji)
    {
        // char �ϐ��� = �v�f���� N �܂��� N �ȏ�̏ꍇ ? (true) N �O�̕����̏�� : (false)null
        // ���͂��Ă镶���̂P�O�̉𓚂̕����̏���ۑ�����ϐ�
        char prevChar = _kaitouIndex >= 1 ? _kaitou[_kaitouIndex - 1] : '\0';
        // ���͂��Ă镶���̂Q�O�̉𓚂̕����̏���ۑ�����ϐ�
        char prevChar2 = _kaitouIndex >= 2 ? _kaitou[_kaitouIndex - 2] : '\0';

        // �𓚂ɋL�ڂ���Ă��镶���̏���ۑ�����ϐ�
        char currentMoji = _kaitou[_kaitouIndex];

        // ���͂��Ă镶���̂P��̉𓚂̕����̏���ۑ�����ϐ�
        char nextChar = _kaitou[_kaitouIndex + 1];
        // char �ϐ��� = N ��̕������󔒂������ꍇ ? (true) �� : (false) N ��̕����̏��
        // ���͂��Ă镶���̂Q��̉𓚂̕����̏���ۑ�����ϐ�
        char nextChar2 = nextChar == ' ' ? ' ' : _kaitou[_kaitouIndex + 2];
        //���͂��Ă��镶���̂R��̉𓚂̕����̏���ۑ�����ϐ�
        char nextChar3 = nextChar2 == ' ' ? ' ' : _kaitou[_kaitouIndex + 3];

        // ���͂������ꍇ
        if (inputMoji == '\0')
        {
            return 0;
        }

        // ���͂��������ꍇ
        if (inputMoji == currentMoji)
        {
            return 1;
        }

        // ��O����
        // �u���v�� i ->  yi
        // �ui�v�̕ꉹ���g�������Ɓuyi�v���g�������̋�ʂ����邽�߁A�P�O�̕������u�Ȃ��v�ua�v�ui�v�uu�v�ue�v�uo�v�un�v�Ɍ��肵�āA�ui�v�̑O�Ɂuy�v��}������
        if (inputMoji == 'y' && currentMoji == 'i' && (prevChar=='\0'|| prevChar == 'a'|| prevChar == 'i' || prevChar == 'u' || prevChar == 'e' || prevChar == 'o' || prevChar == 'n'))
        {
            _kaitou.Insert(_kaitouIndex, 'y');

            // �𓚂̕����̐��ɉ��Z
            CountManager.kaitouMojiNum++;
            return 2;
        }
        // �u���v�� u  -> wu whu
        // �uu�v�̕ꉹ�g�������Ƌ�ʂ����邽�߁A�P�O�̕������u�Ȃ��v�ua�v�ui�v�uu�v�ue�v�uo�v�un�v�Ɍ��肵�āA�uu�v�̑O�Ɂuw�v��}������
        if (inputMoji == 'w' && currentMoji == 'u' && (prevChar == '\0' || prevChar == 'a' || prevChar == 'i' || prevChar == 'u' || prevChar == 'e' || prevChar == 'o' || prevChar == 'n'))
        {
            _kaitou.Insert(_kaitouIndex, 'w');

            // �𓚂̕����̐��ɉ��Z
            CountManager.kaitouMojiNum++;
            return 2;
        }
        // �u���v�� wu  -> whu
        // �P�O�̕������uw�v�Ɍ��肵�āA�uu�v�̑O�Ɂuh�v��}������@���uqwu�v�uswu�v�utwu�v�ufwu�v�ugwu�v�udwu�v�Ƌ�ʂ��邽�߁A�Q�O�̕������u�Ȃ��v�ua�v�ui�v�uu�v�ue�v�uo�v�un�v�ƌ��肷��
        if (inputMoji == 'h' && currentMoji == 'u' && prevChar == 'w' && (prevChar2 == '\0' || prevChar2 == 'a' || prevChar2 == 'i' || prevChar2 == 'u' || prevChar2 == 'e' || prevChar2 == 'o' || prevChar2 == 'n'))
        {
            _kaitou.Insert(_kaitouIndex, 'h');

            // �𓚂̕����̐��ɉ��Z
            CountManager.kaitouMojiNum++;
            return 2;
        }

        // �u���v�� ka -> ca
        // �u���v�� ku -> cu 
        // �u���v�� ko -> co
        // �ukka�v�ukku�v�ukko�v�Ƌ�ʂ��邽�߁A�P�O�̕������uk�v�ȊO�łP���̕������ua�v�uu�v�uo�v�Ɍ��肵�āA�uk�v���uc�v�ƒu��������
        if (inputMoji == 'c' && currentMoji == 'k' && prevChar != 'k' && (nextChar == 'a' || nextChar == 'u' || nextChar == 'o'))
        {
            _kaitou[_kaitouIndex] = 'c';
            return 2;
        }
        // �u���v�� ku -> qu
        // �ukku�v�Ƌ�ʂ��邽�߁A�P�O�̕������uk�v�ȊO�łP���̕������uu�v�Ɍ��肵�āA�uk�v���uq�v�ƒu��������
        if (inputMoji == 'q' && currentMoji == 'k' && prevChar != 'k' && nextChar == 'u')
        {
            _kaitou[_kaitouIndex] = 'q';
            return 2;
        }
        // �u�����v�� kka -> cca
        // �u�����v�� kku -> ccu
        // �u�����v�� kko -> cco
        // �P���̕������uk�v�ɁA�Q���̕������ua�v�uu�v�uo�v�Ɍ��肵�āA�ukk�v���ucc�v�ƒu��������
        if (inputMoji == 'c' && currentMoji == 'k' && nextChar == 'k' && (nextChar2 == 'a' || nextChar2 == 'u' || nextChar2 == 'o'))
        {
            _kaitou[_kaitouIndex] = 'c';
            _kaitou[_kaitouIndex + 1] = 'c';
            return 2;
        }
        // �u�����v�� kku -> qqu
        // �P���̕������uk�v�ɁA�Q���̕������uu�v�Ɍ��肵�āA�ukk�v���uqq�v�ƒu��������
        if (inputMoji == 'q' && currentMoji == 'k' && nextChar == 'k' && nextChar2 == 'u')
        {
            _kaitou[_kaitouIndex] = 'q';
            _kaitou[_kaitouIndex + 1] = 'q';
            return 2;
        }

        // �u���v�� si -> shi
        // �P�O�̕������us�v�Ɍ��肵�āA�ui�v�̑O�Ɂuh�v��}������@���utsi�v�Ƌ�ʂ��邽�߁A�Q�O�̕������u�Ȃ��v�ua�v�ui�v�uu�v�ue�v�uo�v�un�v�ƌ��肷��
        if (inputMoji == 'h' && currentMoji == 'i' && prevChar == 's' && (prevChar2 == '\0' || prevChar2 == 'a' || prevChar2 == 'i' || prevChar2 == 'u' || prevChar2 == 'e' || prevChar2 == 'o' || prevChar2 == 'n'))
        {
            _kaitou.Insert(_kaitouIndex, 'h');

            // �𓚂̕����̐��ɉ��Z
            CountManager.kaitouMojiNum++;
            return 2;
        }
        // �u���v�� si -> ci 
        // �u���v�� se -> ce
        //  �P���̕������ui�v�ue�v�Ɍ��肵�āA�us�v���uc�v�ƒu��������@���utsi�v�utse�v�Ƌ�ʂ��邽�߁A�P�O�̕������u�Ȃ��v�ua�v�ui�v�uu�v�ue�v�uo�v�un�v�ƌ��肷��
        if (inputMoji == 'c' && currentMoji == 's' && (nextChar == 'i' || nextChar == 'e') && (prevChar == '\0' || prevChar == 'a' || prevChar == 'i' || prevChar == 'u' || prevChar == 'e' || prevChar == 'o' || prevChar == 'n'))
        {
            _kaitou[_kaitouIndex] = 'c';
            return 2;
        }
        // �u�����v�� ssi -> cci 
        // �u�����v�� sse -> cce
        // �P���̕������us�v�ɂQ���̕������ui�v�ue�v�Ɍ��肵�āA�uss�v���ucc�v�ƒu��������
        if (inputMoji == 'c' && currentMoji == 's' && nextChar == 's' && (nextChar2 == 'i' || nextChar2 == 'e'))
        {
            _kaitou[_kaitouIndex] = 'c';
            _kaitou[_kaitouIndex + 1] = 'c';
            return 2;
        }


        // �u���v�� ti -> chi
        // �utti�v�Ƌ�ʂ��邽�߁A�P�O�̕������ut�v�ȊO�łP���̕������ui�v�Ɍ��肵�āA�ut�v���uc�v�ƒu�������āui�v�̑O�Ɂuh�v��}������
        if (inputMoji == 'c' && currentMoji == 't' && prevChar != 't' && nextChar == 'i')
        {
            _kaitou[_kaitouIndex] = 'c';
            _kaitou.Insert(_kaitouIndex + 1, 'h');

            // �𓚂̕����̐��ɉ��Z
            CountManager.kaitouMojiNum++;
            return 2;
        }
        // �u�����v�� tti -> cchi
        // �P���̕������ut�v�ɂQ���̕������ui�v�Ɍ��肵�āA�utt�v���ucc�v�ƒu�������āui�v�̑O�Ɂuh�v��}������
        if (inputMoji == 'c' && currentMoji == 't' && nextChar == 't' && nextChar2 == 'i')
        {
            _kaitou[_kaitouIndex] = 'c';
            _kaitou[_kaitouIndex + 1] = 'c';
            _kaitou.Insert(_kaitouIndex + 2, 'h');

            // �𓚂̕����̐��ɉ��Z
            CountManager.kaitouMojiNum++;
            return 2;
        }
        // �u�v�� tu -> tsu
        // �P�O�̕������ut�v�ɂP���̕������uu�v�Ɍ��肵�āA�uu�v�̑O�Ɂus�v��}������@���uxtu�v�Ƌ�ʂ��邽�߁A�Q�O�̕������u�Ȃ��v�ua�v�ui�v�uu�v�ue�v�uo�v�un�v�ƌ��肷��
        if (inputMoji == 's' && currentMoji == 'u' && prevChar == 't' &&(prevChar2 == '\0' || prevChar2 == 'a' || prevChar2 == 'i' || prevChar2 == 'u' || prevChar2 == 'e' || prevChar2 == 'o' || prevChar2 == 'n'))
        {
            _kaitou.Insert(_kaitouIndex, 's');

            // �𓚂̕����̐��ɉ��Z
            CountManager.kaitouMojiNum++;
            return 2;
        }

        // �u�Ӂv�� hu -> fu
        //  �P���̕������uu�v�Ɍ��肵�āA�uh�v���uf�v�ƒu��������@���ushu�v�uchu�v�uwhu�v�uthu�v�udhu�v�Ƌ�ʂ��邽�߁A�P�O�̕������u�Ȃ��v�ua�v�ui�v�uu�v�ue�v�uo�v�un�v�ƌ��肷��
        if (inputMoji == 'f' && currentMoji == 'h' && prevChar != 'h' && nextChar == 'u'&& (prevChar == '\0' || prevChar == 'a' || prevChar == 'i' || prevChar == 'u' || prevChar == 'e' || prevChar == 'o' || prevChar == 'n'))
        {
            _kaitou[_kaitouIndex] = 'f';
            return 2;
        }
        // �u���Ӂv�� hhu -> ffu
        // �P���̕������uh�v�ɁA�Q���̕������uu�v�Ɍ��肵�āA�uhh�v���uff�v�ƒu��������
        if (inputMoji == 'f' && currentMoji == 'h' && nextChar == 'h' && nextChar2 == 'u' )
        {
            _kaitou[_kaitouIndex] = 'f';
            _kaitou[_kaitouIndex + 1] = 'f';
            return 2;
        }

        // �u��v�� n -> nn
        // �una�v�uni�v�unu�v�une�v�uno�v�uya�v�uyi�v�uyu�v�uye�v�uyo�v�Ƌ�ʂ����邽�߁A���͂��悤�Ƃ��Ă���𓚂̕������ua�v�ui�v�uu�v�ue�v�uo�v�uy�v�ȊO�łP�O�̕������un�v�Ɍ��肵�āA���̕����̑O�Ɂun�v��}������@���unn�v�Ƌ�ʂ��邽�߁A�Q�O�̕������un�v�ȊO�Ɍ��肷��
        if (inputMoji == 'n' && currentMoji != 'a' && currentMoji != 'i' && currentMoji != 'u' && currentMoji != 'e' && currentMoji != 'o' && currentMoji != 'y' && prevChar == 'n' && prevChar2 != 'n')
        {
            _kaitou.Insert(_kaitouIndex, 'n');

            // �𓚂̕����̐��ɉ��Z
            CountManager.kaitouMojiNum++;
            return 2;
        }
        // �u��v�� n -> xn
        // �una�v�uni�v�unu�v�une�v�uno�v�uya�v�uyi�v�uyu�v�uye�v�uyo�v�Ƌ�ʂ��邽�߁A�P���̕������ua�v�ui�v�uu�v�ue�v�uo�v�uy�v�ȊO�łP�O�̕������un�v�ȊO�Ɍ��肵�āA�un�v�̑O�Ɂux�v��}������
        if (inputMoji == 'x' && currentMoji == 'n' && prevChar != 'n' && nextChar != 'a' && nextChar != 'i' && nextChar != 'u' && nextChar != 'e' && nextChar != 'o' && nextChar != 'y')
        {
            _kaitou.Insert(_kaitouIndex, 'x');
            
            // �𓚂̕����̐��ɉ��Z
            CountManager.kaitouMojiNum++;
            return 2;
        }

        // �u���v�� ji -> zi
        // �ujji�v�Ƌ�ʂ��邽�߁A�P�O�̕������uj�v�ȊO�ɂP���̕������ui�v�Ɍ��肵�āA�uj�v���uz�v�ɒu��������
        if (inputMoji == 'z' && currentMoji == 'j' && prevChar!='j'&&nextChar == 'i')
        {
            _kaitou[_kaitouIndex] = 'z';
            return 2;
        }
        // �u�����v�� jyi -> zyi
        // �ujjyi�v�Ƌ�ʂ��邽�߁A�P�O�̕������uj�v�ȊO�ɂP���̕������uy�v�ƂQ���̕������ui�v�Ɍ��肵�āA�uj�v���uz�v�ɒu��������
        if (inputMoji == 'z' && currentMoji == 'j' && prevChar!='j'&&nextChar == 'y' && nextChar2 == 'i')
        {
            _kaitou[_kaitouIndex] = 'z';
            return 2;
        }
        // �u�����v�� je -> jye 
        // �u����v�� ja -> jya 
        // �u����v�� ju -> jyu 
        // �u����v�� jo -> jyo 
        // �P�O�̕������uj�v�Ɍ��肵�āA�ua�v�uu�v�ue�v�uo�v�̑O�Ɂuy�v��}������
        if (inputMoji == 'y' && (currentMoji == 'a' || currentMoji == 'u' || currentMoji == 'e' || currentMoji == 'o') && prevChar == 'j')
        {
            _kaitou.Insert(_kaitouIndex, 'y');
            
            // �𓚂̕����̐��ɉ��Z
            CountManager.kaitouMojiNum++;
            return 2;
        }
        // �u�����v�� je -> zye
        // �u����v�� ja -> zya
        // �u����v�� ju -> zyu
        // �u����v�� jo -> zyo
        // �ujja�v�ujju�v�ujje�v�ujjo�v�Ƌ�ʂ����邽�߁A�P�O�̕������uj�v�ȊO�ɂP���̕������ua�v�uu�v�ue�v�uo�v�Ɍ��肵�āA�uj�v���uz�v�ɒu�������āua�v�uu�v�ue�v�uo�v�̑O�Ɂuy�v��}������
        if (inputMoji == 'z' && currentMoji == 'j' && prevChar!='j'&&(nextChar == 'a' || nextChar == 'u' || nextChar == 'e' || nextChar == 'o'))
        {
            _kaitou[_kaitouIndex] = 'z';
            _kaitou.Insert(_kaitouIndex+1, 'y');
            
            // �𓚂̕����̐��ɉ��Z
            CountManager.kaitouMojiNum++;
            return 2;
        }
        // �u�����v�� jji -> zzi
        // �P���̕������uj�v�ƂQ���̕������ui�v�Ɍ��肵�āA�ujj�v���uzz�v�ɒu��������
        if (inputMoji == 'z' && currentMoji == 'j' && nextChar == 'j' && nextChar2 == 'i')
        {
            _kaitou[_kaitouIndex] = 'z';
            _kaitou[_kaitouIndex + 1] = 'z';
            return 2;
        }
        // �u�������v�� jjyi -> zzyi
        // �P���̕������uj�v�ƂQ���̕������uy�v�ƂR���̕������ui�v�Ɍ��肵�āA�ujj�v���uzz�v�ɒu��������
        if (inputMoji == 'z' && currentMoji == 'j' && nextChar == 'j' && nextChar2 == 'y' && nextChar3 == 'i')
        {
            _kaitou[_kaitouIndex] = 'z';
            _kaitou[_kaitouIndex + 1] = 'z';
            return 2;
        }
        // �u�������v�� jje -> zzye
        // �u������v�� jja -> zzya
        // �u������v�� jju -> zzyu
        // �u������v�� jjo -> zzyo
        // �P���̕������uj�v�ƂQ���̕������ua�v�uu�v�ue�v�uo�v�Ɍ��肵�āA�ujj�v���uzz�v�ɒu�������āua�v�uu�v�ue�v�uo�v�̑O�Ɂuy�v��}������
        if (inputMoji == 'z' && currentMoji == 'j' && nextChar == 'j' && (nextChar2 == 'a' || nextChar2 == 'u' || nextChar2 == 'e' || nextChar2 == 'o'))
        {
            _kaitou[_kaitouIndex] = 'z';
            _kaitou[_kaitouIndex + 1] = 'z';
            _kaitou.Insert(_kaitouIndex + 2, 'y');
            
            // �𓚂̕����̐��ɉ��Z
            CountManager.kaitouMojiNum++;
            return 2;
        }

        // �u���v�� la -> xa
        // �u���v�� li -> xi 
        // �u���v�� lu -> xu
        // �u���v�� le -> xe 
        // �u���v�� lo -> xo
        // �u���v�� lyi -> xyi
        // �u���v�� lye -> xye
        // �u��v�� lya -> xya
        // �u��v�� lyu -> xyu
        // �u��v�� lyo -> xyo
        // �u���v�� ltu -> xtu
        // �ulla�v�ulli�v�ullu�v�ulle�v�ullo�v�ullya�v�ullyi�v�ullyu�v�ullye�v�ullyo�v�ultu�v�Ƌ�ʂ��邽�߁A�P�O�̕������ul�v�ȊO�Ɍ��肵�āA�ul�v���ux�v�ɒu��������
        if (inputMoji == 'x' && currentMoji == 'l' && prevChar != 'l'&& nextChar!='l' )
        {
            _kaitou[_kaitouIndex] = 'x';
            return 2;
        }
        // �u�����v�� lla -> xxa
        // �u�����v�� lli -> xxi
        // �u�����v�� llu -> xxu
        // �u�����v�� lle -> xxe
        // �u�����v�� llo -> xxo
        // �u�����v�� llyi -> xxyi
        // �u�����v�� llye -> xxye
        // �u����v�� llya -> xxya
        // �u����v�� llyu -> xxyu
        // �u����v�� llyo -> xxyo
        // �u�����v�� lltu -> xxtu
        // �P���̕������ul�v�ƌ��肵�āA�ull�v���uxx�v�ɒu��������
        if (inputMoji == 'x' && currentMoji == 'l' && nextChar == 'l')
        {
            _kaitou[_kaitouIndex] = 'x';
            _kaitou[_kaitouIndex+1] = 'x';
            return 2;
        }

        // �u�����v�� qa -> qwa
        // �u�����v�� qi -> qwi 
        // �u�����v�� qe -> qwe 
        // �u�����v�� qo -> qwo
        // �P�O�̕������uq�v�Ɍ��肵�āA�ua�v�ui�v�ue�v�uo�v�̑O�Ɂuw�v��}������
        if (inputMoji == 'w' && (currentMoji == 'a' || currentMoji == 'i' || currentMoji == 'e' || currentMoji == 'o') && prevChar == 'q')
        {
            _kaitou.Insert(_kaitouIndex, 'w');

            // �𓚂̕����̐��ɉ��Z
            CountManager.kaitouMojiNum++;
            return 2;
        }

        // �u�����v�� qa -> kwa
        // �uqqa�v�Ƌ�ʂ��邽�߁A�P�O�̕������uq�v�ȊO�ɂP���̕������ua�v�Ɍ��肵�āA�uq�v���uk�v�ɒu�������āua�v�̑O�Ɂuw�v��}������
        if (inputMoji == 'k' && currentMoji == 'q' && prevChar != 'q' && nextChar == 'a' )
        {
            _kaitou[_kaitouIndex] = 'k';
            _kaitou.Insert(_kaitouIndex + 1, 'w');

            // �𓚂̕����̐��ɉ��Z
            CountManager.kaitouMojiNum++;
            return 2;
        }

        // �u�����v�� qi -> qyi
        // �u�����v�� qe -> qye
        // �P�O�̕������uq�v�Ɍ��肵�āA�ui�v�ue�v�̑O�Ɂuy�v��}������
        if (inputMoji == 'y' && (currentMoji == 'i' || currentMoji == 'e') && prevChar == 'q')
        {
            _kaitou.Insert(_kaitouIndex, 'y');
            CountManager.kaitouMojiNum++;
            return 2;
        }

        // �u�������v�� qqa -> kkwa
        // �P���̕������uq�v�ɂQ���̕������ua�v�Ɍ��肵�āA�uqq�v���ukk�v�ɒu�������āua�v�̑O�Ɂuw�v��}������
        if (inputMoji == 'k' && currentMoji == 'q' && nextChar == 'q' && nextChar2 == 'a')
        {
            _kaitou[_kaitouIndex] = 'k';
            _kaitou[_kaitouIndex+1] = 'k';
            _kaitou.Insert(_kaitouIndex + 2, 'w');

            // �𓚂̕����̐��ɉ��Z
            CountManager.kaitouMojiNum++;
            return 2;
        }

        // �u�����v�� she -> sye
        // �u����v�� sha -> sya
        // �u����v�� shu -> syu
        // �u����v�� sho -> syo
        // �P�O�̕������us�v�ɂP���̕������ua�v�uu�v�ue�v�uo�v�Ɍ��肵�āA�uh�v���uy�v�ɒu��������
        if (inputMoji == 'y' && currentMoji == 'h' && prevChar == 's' && (nextChar == 'a' || nextChar == 'u' || nextChar == 'e' || nextChar == 'o'))
        {
            _kaitou[_kaitouIndex] = 'y';
            return 2;
        }

        // �u�����v�� tyi -> cyi
        // �u�����v�� tye -> cye 
        // �u����v�� tya -> cya 
        // �u����v�� tyu -> cyu 
        // �u����v�� tyo -> cyo 
        // �uttya�v�uttyi�v�uttyu�v�uttye�v�uttyo�v�̋�ʂ����邽�߁A�P�O�̕������ut�v�ȊO�ɂP���̕������uy�v�ƂQ���̕������ua�v�ui�v�uu�v�ue�v�uo�v�Ɍ��肵�āA�ut�v���uc�v�ɒu��������
        if (inputMoji == 'c' && currentMoji == 't' && prevChar!='t'&&nextChar == 'y' && (nextChar2 == 'a' || nextChar2 == 'i' || nextChar2 == 'u' || nextChar2 == 'e' || nextChar2 == 'o'))
        {
            _kaitou[_kaitouIndex] = 'c';
            return 2;
        }
        // �u�����v�� cye -> che�@
        // �u����v�� cya -> cha
        // �u����v�� cyu -> chu
        // �u����v�� cyo -> cho
        // �P�O�̕������uc�v�ɂP���̕������ua�v�uu�v�ue�v�uo�v�Ɍ��肵�āA�uy�v���uh�v�ɒu��������
        if (inputMoji == 'h' && currentMoji == 'y' && prevChar == 'c' && (nextChar == 'a' || nextChar == 'u' || nextChar == 'e' || nextChar == 'o'))
        {
            _kaitou[_kaitouIndex] = 'h';
            return 2;
        }
        // �u�������v�� ttyi -> ccyi
        // �u�������v�� ttye -> ccye 
        // �u������v�� ttya -> ccya 
        // �u������v�� ttyu -> ccyu 
        // �u������v�� ttyo -> ccyo 
        // �P���̕������ut�v�ƂQ���̕������uy�v�Ɍ��肵�āA�utt�v���ucc�v�ɒu��������
        if (inputMoji == 'c' && currentMoji == 't' && nextChar == 't' && nextChar2 == 'y')
        {
            _kaitou[_kaitouIndex] = 'c';
            _kaitou[_kaitouIndex+1] = 'c';
            return 2;
        }

        // �u�ӂ��v�� fa -> fwa
        // �u�ӂ��v�� fi -> fwi
        // �u�ӂ��v�� fe -> fwe 
        // �u�ӂ��v�� fo -> fwo
        // �P�O�̕������uf�v�Ɍ��肵�āA�ua�v�ui�v�ue�v�uo�v�̑O�Ɂuw�v��}������
        if (inputMoji == 'w' && (currentMoji == 'a' || currentMoji == 'i' || currentMoji == 'e' || currentMoji == 'o') && prevChar == 'f')
        {
            _kaitou.Insert(_kaitouIndex, 'w');

            // �𓚂̕����̐��ɉ��Z
            CountManager.kaitouMojiNum++;
            return 2;
        }
        // �u�ӂ��v�� fi -> fyi
        // �u�ӂ��v�� fe -> fye
        // �P�O�̕������uf�v�Ɍ��肵�āA�ui�v�̑O�Ɂuy�v��}������
        if (inputMoji == 'y' && (currentMoji == 'i' || currentMoji == 'e') && prevChar == 'f')
        {
            _kaitou.Insert(_kaitouIndex, 'y');

            // �𓚂̕����̐��ɉ��Z
            CountManager.kaitouMojiNum++;
            return 2;
        }

        // ���͂��Ԉ���Ă���ꍇ
        return 3;
    }

    // ���͂��ꂽ�L�[�R�[�h��char�^�ɕϊ�����֐�
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

    // ���̏������̊֐�
    public void Initi_Question()
    {
        // text�̕\��������
        _textMondai.text = "";
        _textRomaji.text = "";

        // �����Ő��l�𐶐�
        int _random = UnityEngine.Random.Range(0, _questions.Length);

        // Question�N���X�ɔz���ǉ�
        Question question = _questions[_random];

        // �v�f����������
        _kaitouIndex = 0;

        // ���X�g�̒��g����ɂ���
        _kaitou.Clear();

        // Question.romaji�iString�^�j��Char�^�̔z��ɕϊ�
        char[] characters = question.romaji.ToCharArray();

        // Question�N���X�̔z���_kaitou���X�g�ɒǉ�����
        foreach (char character in characters)
        {
            _kaitou.Add(character);
        }

        // ������̍Ŋ��ɋ󔒂�ǉ����āA�u�^�C�s���O�̏I���v������
        _kaitou.Add(' ');

        // ��╪�̎��Ԃ�������
        TimerManager._typeTime = TimerManager.typeTime;

        // ���Ɖ𓚂̕\������^�C�~���O�����炷���߂̃R���[�`��
        StartCoroutine(Display_Wait(question));

        // �𓚂ŏo���ꂽ�����̐������Z���Ă���
        CountManager.kaitouMojiNum += (_kaitou.Count - 1);
    }

    // ���͑O�Ɠ��͌�̕����̐F��ω����ĕ\��
    string Generate_Romaji()
    {
        // �����̐F���^�O�@�\�Ŏw��
        string text = "<style=typed>";

        // _kaitou���X�g���������J��Ԃ�
        for (int i = 0; i < _kaitou.Count; i++)
        {
            // �������󔒂������珈�����΂�
            if (_kaitou[i] == ' ')
            {
                break;
            }
            // ���X�g�̗v�f���������Ă����ꍇ�ɐF��ς���
            if (i == _kaitouIndex)
            {
                text += "</style><style=untyped>";
            }

            // ������������
            text += _kaitou[i];
        }
        // �����̐F��ς���
        text += "</style>";

        return text;
    }

    // ���Ɖ𓚂�\������Ԋu���󂯂邽�߂̊֐�
    private IEnumerator Display_Wait(Question question)
    {
        yield return new WaitForSeconds(intervalWaitMoji);

        //����\������
        _textMondai.text = question.mondai;

        yield return new WaitForSeconds(intervalWaitMoji);

        // �����̐F�𔼓����F�ɂ���
        _textRomaji.text = Generate_Romaji();
    }
}
