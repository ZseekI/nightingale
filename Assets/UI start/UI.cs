using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // �ѧ��ѹ�������
    public void StartGame()
    {
        SceneManager.LoadScene("Start"); // ᷹ "GameScene" ���ª��ͩҡ���ͧ�س
    }

    // �ѧ��ѹ�͡�ҡ��
    public void ExitGame()
    {
        Application.Quit(); // �����ԧ������ѹ���� Standalone ���� Build �͡������
        Debug.Log("Game is exiting..."); // ����Ѻ��Ǩ�ͺ�͹���ͺ� Unity Editor
    }
}
