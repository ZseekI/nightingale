using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // ฟังก์ชันเริ่มเกม
    public void StartGame()
    {
        SceneManager.LoadScene("Start"); // แทน "GameScene" ด้วยชื่อฉากเกมของคุณ
    }

    // ฟังก์ชันออกจากเกม
    public void ExitGame()
    {
        Application.Quit(); // ใช้ได้จริงเมื่อรันเกมเป็น Standalone หรือ Build ออกมาแล้ว
        Debug.Log("Game is exiting..."); // สำหรับตรวจสอบตอนทดสอบใน Unity Editor
    }
}
