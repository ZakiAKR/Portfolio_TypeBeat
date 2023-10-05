using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TitleButtonManager : MonoBehaviour
{
    // �^�C�g����ʂ̃{�^�����I�u�W�F�N�g�Ƃ��Ď擾
    [SerializeField] GameObject[] _selectButton=new GameObject[2];
    
    [Space(10)]

    // �g��k������傫����ݒ肷��ϐ�
    public float scallSize;
    // �{�^���̑傫�����ő�ɂȂ鎞�Ԃ�ݒ肷��ϐ�
    public float maxTime;
    // �{�^���̊g��k���̃X�s�[�h
    public float moveSpeed;
    // ���Ԃ�ۑ�����ϐ�
    private float time;
    // �g��k����؂�ւ��锻�������ϐ�
    private bool enlarge = true;

    // �{�^���̏����̑傫����ۑ����邽�߂̕ϐ�
    private Vector3[] _seleceScale = new Vector3[2];

    // �I�𒆂̃{�^���̏���ۑ����邽�߂̕ϐ�
    private GameObject _button;

    // Start is called before the first frame update
    void Start()
    {
        //�uStart�v�{�^����I����Ԃɂ���
        EventSystem.current.SetSelectedGameObject(_selectButton[0]);

        // ���ׂẴ{�^���̏����̑傫����ۑ�����
        for (int i = 0; i < _selectButton.Length; i++)
        {
            _seleceScale[i] = _selectButton[i].transform.localScale;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // �I�𒆂̃{�^���̏���ۑ�����
        _button = EventSystem.current.currentSelectedGameObject;

        // �{�^���̉��o����
        if (_button == _selectButton[0])
        {
            // �{�^���̑傫�����g��k�����鏈��
            Button_Scaling(_selectButton[0]);
            
            // �I������Ă��Ȃ��{�^���������̑傫���ɒ�������
            _selectButton[1].transform.localScale = Reset_ImageScale(_seleceScale[1]);
        }
        if (_button == _selectButton[1])
        {
            // �{�^���̑傫�����g��k�����鏈��
            Button_Scaling(_selectButton[1]);

            // �I������Ă��Ȃ��{�^���������̑傫���ɒ�������
            _selectButton[0].transform.localScale = Reset_ImageScale(_seleceScale[0]);
        }
    }

    // �{�^���̑傫�����g��k�����鉉�o�̊֐�
    void Button_Scaling(GameObject image)
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

    // �{�^���̑傫���������̑傫���ɖ߂��֐�
    Vector3 Reset_ImageScale(Vector3 afterObj)
    {
        return afterObj;
    }
}
