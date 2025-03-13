using Unity.Netcode;
using UnityEngine;

public class PlayerShooting : NetworkBehaviour
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private InputReader inputReader; // ตรวจสอบว่า inputReader มีค่าหรือไม่

    private void Awake()
    {
        // ตรวจสอบว่า inputReader ถูกกำหนดค่าหรือไม่
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

        // เรียกให้เซิร์ฟเวอร์สร้างกระสุน
        FireServerRpc(firePoint.position, firePoint.up);
    }

    [ServerRpc]
    private void FireServerRpc(Vector2 position, Vector2 direction)
    {
        GameObject bullet = Instantiate(bulletPrefab, position, Quaternion.identity);
        NetworkObject bulletNetworkObject = bullet.GetComponent<NetworkObject>();
        bulletNetworkObject.Spawn(); // ซิงค์กระสุนไปยังไคลเอนต์ทุกตัว

        Bullet bulletScript = bullet.GetComponent<Bullet>();
        bulletScript.SetDirectionServerRpc(direction);
    }
}
