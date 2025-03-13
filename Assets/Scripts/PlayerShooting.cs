using Unity.Netcode;
using UnityEngine;

public class PlayerShooting : NetworkBehaviour
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private InputReader inputReader; // ��Ǩ�ͺ��� inputReader �դ���������

    private void Awake()
    {
        // ��Ǩ�ͺ��� inputReader �١��˹�����������
        if (inputReader == null)
        {
            inputReader = Object.FindFirstObjectByType<InputReader>();
            if (inputReader == null)
            {
                Debug.LogError("InputReader not found in the scene! Please assign it in the Inspector.");
            }
        }
    }

    private void OnEnable()
    {
        if (inputReader != null)
            inputReader.PrimaryFireEvent += HandleFire;
        else
            Debug.LogError("InputReader is missing in PlayerShooting!");
    }

    private void OnDisable()
    {
        if (inputReader != null)
            inputReader.PrimaryFireEvent -= HandleFire;
    }

    private void HandleFire(bool isFiring)
    {
        if (!isFiring || !IsOwner) return;

        // ���¡���������������ҧ����ع
        FireServerRpc(firePoint.position, firePoint.up);
    }

    [ServerRpc]
    private void FireServerRpc(Vector2 position, Vector2 direction)
    {
        GameObject bullet = Instantiate(bulletPrefab, position, Quaternion.identity);
        NetworkObject bulletNetworkObject = bullet.GetComponent<NetworkObject>();
        bulletNetworkObject.Spawn(); // �ԧ�����ع��ѧ���͹��ء���

        Bullet bulletScript = bullet.GetComponent<Bullet>();
        bulletScript.SetDirectionServerRpc(direction);
    }
}
