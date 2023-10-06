using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.Rendering.Universal;

// ���U���g��ʂɊւ���\�[�X�R�[�h

public class ResultManager : MonoBehaviour
{
    [Space(10)]

    // �ł��������̐���\������text���擾
    [SerializeField] private TextMeshProUGUI _mojiNum;
    // �ԈႦ�đł�������\������text���擾
    [SerializeField] private TextMeshProUGUI _missNum;
    // �]����\������text���擾
    [SerializeField] private TextMeshProUGUI _evalation;

    [Space(10)]

    //�u�߂�v�Ƃ����{�^�����I�u�W�F�N�g�Ƃ��Ď擾
    [SerializeField] private GameObject _selectButton;

    [Space(10)]

    // �����̐���\������҂��̎��Ԃ�ݒ肷��ϐ�
    public float firstWaitTime;
    // �]���̕�����\������҂��̎��Ԃ�ݒ肷��ϐ�
    public float secondWaitTime;

    // �g��k������傫����ݒ肷��ϐ�
    public float scallSize;
    // �{�^���̑傫�����ő�ɂȂ鎞�Ԃ�ݒ肷��ϐ�
    public float maxTime;
    // �{�^���̊g��k���̃X�s�[�h
    public float moveSpeed;
    // ���Ԃ�ۑ�����ϐ�
    private float time=0;
    // �g��k����؂�ւ��锻�������ϐ�
    private bool enlarge = true;

    // �ł��������̐���ۑ�����ϐ�
    private int _countMoji;
    // �ԈႦ�đł��������̐���ۑ�����ϐ�
    private int _missMoji;
    // �𓚂̕����̐���ۑ�����ϐ�
    private int _kaitouMoji;
    // �����𓦂��������v�Z���ĕۑ�����ϐ�
    private float _notMoji;
    // �v�Z���ʂ�ۑ�����ϐ�
    private int _totalPoint;

    [Space(10)]

    // �]���̊�ƂȂ銄�����`����ϐ�
    [Header("Valuation Ratio")]
    public float[] valuation = new float[3];

    // �𓚂̕������Ɗ������v�Z�������ʂ�ۑ�����ϐ�
    private int[] _ratioPoint=new int[3];

    // Start is called before the first frame update
    void Start()
    {
        // �ł��������̐����Q�Ƃ��ĕۑ��p�̕ϐ��ɑ��
        _countMoji = CountManager.countMojiNum;

        // �ԈႢ�̕����̐����Q�Ƃ��ĕۑ��p�̕ϐ��ɑ��
        _missMoji = CountManager.missMojiNum;

        // �𓚂̕����̐����Q�Ƃ��ĕۑ��p�̕ϐ��ɑ��
        _kaitouMoji = CountManager.kaitouMojiNum;

        // ������\�����Ȃ��悤�ɂ��鏈��
        _mojiNum.text = "";
        _missNum.text = "";
        _evalation.text = "";

        // �u�߂�v�̃{�^����I����Ԃɂ��Ă���
        EventSystem.current.SetSelectedGameObject(_selectButton);

        // �𓚂̕�����łĂȂ��������������v�Z
        _notMoji = _kaitouMoji - _countMoji;

        // �𓚂̕��������犄��o����������ۑ��p�̕ϐ��ɑ��
        for (int i = 0; i < valuation.Length; i++)
        {
            _ratioPoint[i] = (int)(_kaitouMoji * valuation[i]);
        }

    }

    // Update is called once per frame
    void Update()
    {
        // �����̉��o������R���[�`��
        StartCoroutine(Set_Text());

        // �{�^�����g��k�����鉉�o�̏���
        Scaling_Button(_selectButton);
    }

    // �\�����镶�������Ԃ��󂯂ĕ\�����鉉�o�̃R���[�`��
    private IEnumerator Set_Text()
    {
        // �ł���������\������܂ł̑҂��̎���
        yield return new WaitForSeconds(firstWaitTime);

        // �ł��������̐���\������
        _mojiNum.text = _countMoji.ToString();

        // �ԈႦ�đł���������\������܂ł̑҂��̎���
        yield return new WaitForSeconds(firstWaitTime);

        // �ԈႦ�đł��������̐���\������
        _missNum.text = (_missMoji+(int)_notMoji).ToString();

        // �]����\������܂ł̑҂��̎���
        yield return new WaitForSeconds(secondWaitTime);

        // �]�������̏���
        // �ł��������ƊԈႦ�đł��������̐��Ɠ���������1.2�{�̐����v�Z
        _totalPoint = _countMoji - (_missMoji + (int)(_notMoji * 1.2f));

        // �]���𕶎��ŕ\��
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

    // �{�^�����g��k�����鉉�o�̊֐�
    void Scaling_Button(GameObject image)
    {
        // ���������炩�ɂ��鏈��
        scallSize = Time.deltaTime * moveSpeed;

        // �g��k�������ԂŐ؂�ւ��鏈��
        if (time < 0) { enlarge = true; }
        if (time > maxTime) { enlarge = false; }

        // �I�u�W�F�N�g�̑傫����ς��鏈��
        if (enlarge)
        {
            // �g�傷�邽�߂Ɏ��Ԃ𑝂₷
            time += Time.deltaTime;

            // �I�u�W�F�N�g�̑傫����傫������
            image.transform.localScale += new Vector3(scallSize, scallSize, scallSize);
        }
        else
        {
            // �k�����邽�߂Ɏ��Ԃ����炷
            time -= Time.deltaTime;

            // �I�u�W�F�N�g�̑傫��������������
            image.transform.localScale -= new Vector3(scallSize, scallSize, scallSize);
        }
    }

}
