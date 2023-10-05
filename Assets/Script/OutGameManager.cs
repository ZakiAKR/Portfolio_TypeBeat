using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

// �|�[�Y��ʂ⃁�C���̃V�X�e���ȊO�Ɋւ��\�[�X�R�[�h

public class OutGameManager : MonoBehaviour
{
    // �J�E���g�_�E���ƑS�̂̎��Ԃ̔�����g�p���邽�߂ɁuTimerManager�v���擾
    [SerializeField] public TimerManager _timeSystem;

    [Space(10)]

    // �߂�{�^���������ꂽ�Ƃ��ɉ��Ɋւ��Ă̏������g�p���邽�߁A�uAudioManager�v���擾
    [SerializeField] public AudioManager _audioSystem;

    [Space(10)]

    // �S�̂̎��Ԃ��I�������Ƀ��C���̃V�X�e���������Ȃ��悤�ɂ��邽�߂ɁuInGameSystem�v���擾
    [SerializeField] private GameObject _typeingSystem;

    [Space(10)]

    // ESC�L�[�������ꂽ�Ƃ��ɕ\������p�l��
    [SerializeField] private GameObject _endPanel;

    [Space(10)]

    // �|�[�Y��ʂ̃{�^�����I�u�W�F�N�g�Ƃ��Ď擾
    [SerializeField] private GameObject[] _selectButton = new GameObject[3];
    // �I������Ă��Ȃ��Ƃ��̃{�^���̉摜���I�u�W�F�N�g�Ƃ��Ď擾
    [SerializeField] private GameObject[] _backButton = new GameObject[3];

    // �|�[�Y��ʂ��\�����ꂽ���𔻒肷�邽�߂̕ϐ�
    [HideInInspector] public bool isPose;

    // �I�𒆂̃{�^���̏���ۑ����Ă������߂̕ϐ�
    private GameObject _button;

    // Start is called before the first frame update
    void Start()
    {
        // ������
        _endPanel.SetActive(false);
        isPose = false;
    }

    // Update is called once per frame
    void Update()
    {
        // �I�𒆂̃{�^���̏���ۑ�
        _button = EventSystem.current.currentSelectedGameObject;

        // �J�E���g�_�E����ƑS�̂̎��ԓ���ESC�L�[���g�p�ł���悤�ɂ��鏈��
        if (_timeSystem.isCountDown&& !_timeSystem.isFinish && Input.GetKeyDown(KeyCode.Escape))
        {
            // �|�[�Y��ʂ��\�����ꂽ���Ƃ�ݒ肷��
            isPose = true;

            // �Q�[�����̎��Ԃ��~�߂�
            Time.timeScale = 0f;

            // BGM�ASE�Ɋւ��Ă̏���
            _audioSystem.IsPose_Sound();

            // �p�l����\������
            _endPanel.SetActive(true);

            // �ŏ��ɑI����Ԃɂ��Ă����{�^����ݒ肷��
            EventSystem.current.SetSelectedGameObject(_selectButton[0]);
        }

        // �S�̂̎��Ԃ��I������ꍇ
        if (_timeSystem.isFinish)
        {
            // InGameSystem���g�p�ł��Ȃ�����
            _typeingSystem.SetActive(false);
        }

        // �{�^���̉��o����
        if (_button == _selectButton[0])
        {
            // �{�^���̉��o�̊֐�
            Button_Direction(0, 1, 2);
        }
        if (_button == _selectButton[1])
        {
            // �{�^���̉��o�̊֐�
            Button_Direction(1, 0, 2);
        }
        if (_button == _selectButton[2])
        {
            // �{�^���̉��o�̊֐�
            Button_Direction(2, 0, 1);
        }
    }

    // Esc�L�[���������Ƃ��ɏo��u������RETUEN�v�̂��߂̊֐�
    public void OnClick_ReturnButton()
    {
        // �|�[�Y��ʂ��\�����ꂽ���Ƃ�ݒ肷��
        isPose = false;

        // �p�l�����\���ɂ���
        _endPanel.SetActive(false);

        // BGM�ASE�Ɋւ��Ă̏���
        _audioSystem.OnClick_Button();

        // �Q�[�����̎��Ԃ�i�߂�
        Time.timeScale = 1;
    }

    // �I�𒆂ƑI�𒆈ȊO�̃{�^���̉��o�̊֐�
    void Button_Direction(int falseNum, int trueNum1, int trueNum2)
    {
        // �B���̉摜���\���ɂ���
        _backButton[falseNum].SetActive(false);

        // �B���̉摜��\������
        _backButton[trueNum1].SetActive(true);
        _backButton[trueNum2].SetActive(true);
    }
}
