using UnityEngine;
using UnityEngine.SceneManagement;

public class ReturnToMenuButton : MonoBehaviour
{
    public void BackToMenu()
    {
        // ปิดการเชื่อมต่อ Multiplayer (ถ้ามี)
        if (Unity.Netcode.NetworkManager.Singleton.IsListening)
        {
            Unity.Netcode.NetworkManager.Singleton.Shutdown();
        }

        // โหลดซีนเมนู
        SceneManager.LoadScene("Menu"); // 🔁 เปลี่ยนชื่อซีนให้ตรงกับของคุณ
    }
}
