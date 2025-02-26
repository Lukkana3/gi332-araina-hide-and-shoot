using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class GameNetworkManager : MonoBehaviour
{
    [SerializeField] private Button hostButton;
    [SerializeField] private Button clientButton;

    private void Start()
    {
        // ����������Ѻ�ѧ��ѹ����� Host ���� Client
        hostButton.onClick.AddListener(StartHost);
        clientButton.onClick.AddListener(StartClient);
    }

    private void StartHost()
    {
        Debug.Log("Starting Host...");
        NetworkManager.Singleton.StartHost();
    }

    private void StartClient()
    {
        Debug.Log("Starting Client...");
        NetworkManager.Singleton.StartClient();
    }
}
