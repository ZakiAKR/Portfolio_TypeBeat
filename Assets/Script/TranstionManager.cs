using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// �V�[���J�ڂɊւ���\�[�X�R�[�h

public class TranstionManager : MonoBehaviour
{
    // SE�𗬂��Ă���V�[���J�ڂɈڂ�܂ł̎��Ԃ�ۑ�����ϐ�
    public float seWaitTime;

    private void Start()
    {
        // �Q�[�����̎��Ԃ�i�߂�
        Time.timeScale = 1;
    }

    public void To_Title()
    {
        // �Q�[�����̎��Ԃ�i�߂�
        Time.timeScale = 1;

        // SE��炵���㎞�Ԃ�u�����߂̃R���[�`��
        StartCoroutine(Sound_SceneSE(0));
    }

    public void To_Main()
    {
        // �Q�[�����̎��Ԃ�i�߂�
        Time.timeScale = 1;

        // SE��炵���㎞�Ԃ�u�����߂̃R���[�`��
        StartCoroutine(Sound_SceneSE(1));
    }

    public void To_Result()
    {
        // �Q�[�����̎��Ԃ�i�߂�
        Time.timeScale = 1;

        // SE��炵���㎞�Ԃ�u�����߂̃R���[�`��
        StartCoroutine(Sound_SceneSE(2));
    }

    public void End_Game()
    {
        // �Q�[�����̎��Ԃ�i�߂�
        Time.timeScale = 1;

        // SE��炵���㎞�Ԃ�u�����߂̃R���[�`��
        StartCoroutine(Sound_EndSE());
    }


    // SE��炵���㎞�Ԃ�u�����߂̃R���[�`���i�V�[���J�ڗp�j
    private IEnumerator Sound_SceneSE(int numScene)
    {
        // SE����I���܂ł̑҂��̎���
        yield return new WaitForSeconds(seWaitTime);

        // �w��̃V�[���ɑJ�ڂ���
        SceneManager.LoadScene(numScene);
    }


    // SE��炵���㎞�Ԃ�u�����߂̃R���[�`���i�Q�[���I���p�j
    private IEnumerator Sound_EndSE()
    {
        // SE����I���܂ł̑҂��̎���
        yield return new WaitForSeconds(seWaitTime);

        // �Q�[������鏈��
        Application.Quit();
    }
}
