using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Rendering;

// ���ԂɊւ���\�[�X�R�[�h

public class TimerManager : MonoBehaviour
{
    // �J�E���g�_�E����\��������text���擾
    [SerializeField] private TextMeshProUGUI countDownText;
    // �J�E���g�_�E����\��������Text�̃I�u�W�F�N�g���擾
    [SerializeField] private GameObject countObj;

    [Space(10)]

    // �S�̂̎��Ԃ�\��������text���擾
    [SerializeField] private TextMeshProUGUI lifeText;

    [Space(10)]

    // �I����\��������text���擾
    [SerializeField] private TextMeshProUGUI overText;
    // �I����\��������text�̃I�u�W�F�N�g���擾
    [SerializeField] private GameObject overObj;

    [Space(10)]

    // ��╪�̎��Ԃ̕�����\������text���擾
    [SerializeField] private TextMeshProUGUI mondaiTimeText;

    [Space(10)]

    // �V�[���J�ڂ����邽�߂ɁuTranstionManager�v���擾
    [SerializeField] public TranstionManager transScene;

    // ���C���̃V�X�e���̊֐����g�p���邽�߁A�uTypingManager�v���擾
    [SerializeField] public TypingManager typeSystem;

    [Space(10)]

    // Start�̕\������
    public float startWaitTime;
    // Finish�̕\������
    public float finishWaitTime;

    [Space(10)]

    // Start�ɓ���ۂ̉��o���I��������ǂ����̔���
    [HideInInspector]public bool isCountDown;

    // �S�̂̎��Ԃ��I��������ǂ����̔���
    [HideInInspector]public bool isFinish;

    // ���Ԍv������p�̕ϐ�
    private float _countDown = 3f;
    // �J�E���g�_�E���̐�����\�����邽�߂̕ϐ�
    private int _count;

    [Space(10)]

    // �S�̂̎��Ԃ��v��
    public float lifeTime;
    // �S�̂̎��Ԃ̐�����\�����邽�߂̕ϐ�
    private int _life;

    [Space(10)]

    // ��╪�̎��Ԃ̒l��ێ����邽�߂̕ϐ�
    public static float typeTime = 8;
    // ��╪�̎��Ԃ𑪒肷�邽�߂̕ϐ�
    [HideInInspector] public static float _typeTime;
    // ��╪�̎��Ԃ̒l��text�ŕ\�����邽�߂̕ϐ�
    private int _type;


    // Start is called before the first frame update
    void Start()
    {
        // ������
        countObj.SetActive(true);
        overObj.SetActive(false);
        _count = 0;
        _life = 0;
        isCountDown = false;
        isFinish = false;
        _typeTime = 0;
    }

    // Update is called once per frame
    void Update()
    {
        // �J�E���g�_�E���̏���
        if (_countDown >= 0)
        {
            // ���Ԃ��v�����Ďc�莞�Ԃ�\�����鏈��
            // ���Ԃ����炷
            _countDown -= Time.deltaTime;

            // text�ɕ\�����邽�߂�int�^�ɕϊ�
            _count = (int)_countDown + 1;

            // �c�莞�Ԃ�\��
            countDownText.text = _count.ToString();
        }
        // _isCountDown�̔����t����̂́uStart�v���\������Ă��鎞�ɑS�̂̎��Ԃ��v������Ȃ��悤�ɂ��邽��
        if (!isCountDown && _countDown <= 0)
        {
            // Start��\�����邽�߂̃R���[�`��
            StartCoroutine(Delay_StartText());
        }

        // �S�̂̎��Ԃ̏���
        // _isFinish�̔����t���Ă���̂́uFinish�v���\������Ă���Ƃ��ɑS�̂̎��Ԃ��v������Ȃ��悤�ɂ��邽��
        if (isCountDown && !isFinish)
        {
            // ���Ԃ��v�����Ďc�莞�Ԃ�\�����鏈��
            // ���Ԃ����炷
            lifeTime -= Time.deltaTime;

            // text�ɕ\�����邽�߂�int�^�ɕϊ�
            _life = (int)lifeTime;

            // �c�莞�Ԃ�\��
            lifeText.text = _life.ToString();
        }
        if (lifeTime <= 0)
        {
            // �S�̂̎��Ԃ��I������
            isFinish = true;

            // Finish��\�����邽�߂̃R���[�`��
            StartCoroutine(Delay_OverText());
        }

        // �J�E���g�_�E����ł��A�S�̂̎��ԓ��ł̏�����������
        if (isCountDown && !isFinish)
        {
            if (_typeTime > 0)
            {
                // ���Ԃ��v�����Ďc�莞�Ԃ�\�����鏈��
                // ���Ԃ����炷
                _typeTime -= Time.deltaTime;

                // text�ɕ\�����邽�߂�int�^�ɕϊ�
                _type = (int)_typeTime;

                // �c�莞�Ԃ�\��
                mondaiTimeText.text = _type.ToString();
            }
            if (_typeTime <= 0)
            {
                // ���̏������̊֐����Ăяo��
                typeSystem.Initi_Question();
            }
        }

    }

    // Start��\�����邽�߂̃R���[�`��
    private IEnumerator Delay_StartText()
    {
        // text�ɁuStart�v��\��
        countDownText.text = "START!!";

        // text����莞�ԕ\�������܂܂ɂ���
        yield return new WaitForSeconds(startWaitTime);

        // text�̃I�u�W�F�N�g���\���ɂ���
        countObj.SetActive(false);

        // �J�E���g�_�E�����I������
        isCountDown = true;
    }

    // Finish��\�����邽�߂̃R���[�`��
    private IEnumerator Delay_OverText()
    {
        // text�̃I�u�W�F�N�g��\���ɂ���
        overObj.SetActive(true);

        // text�ɁuFinish�v��\��
        overText.text = "FINISH!!";

        // text����莞�ԕ\�������܂܂ɂ���
        yield return new WaitForSeconds(finishWaitTime);

        // ���U���g��ʂ�J�ڂ���
        transScene.To_Result();
    }
}
