using Unity.Netcode;
using UnityEngine;

public class PlayerShooting : NetworkBehaviour
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private InputReader inputReader;

    private void Awake()
    {
        if (inputReader == null)
        {
            inputReader = FindObjectOfType<InputReader>();
            if (inputReader == null)
            {
                Debug.LogError("[PlayerShooting] ❌ InputReader not found in scene!");
            }
            else
            {
                Debug.Log("[PlayerShooting] ✅ InputReader found automatically.");
            }
        }
        else
        {
            Debug.Log("[PlayerShooting] ✅ InputReader assigned via Inspector.");
        }
    }

    private void OnEnable()
    {
        if (inputReader != null)
        {
            inputReader.PrimaryFireEvent += HandleFire;
            Debug.Log("[PlayerShooting] ✅ Subscribed to PrimaryFireEvent.");
        }
        else
        {
            Debug.LogError("[PlayerShooting] ❌ InputReader is null on Enable.");
        }
    }

    private void OnDisable()
    {
        if (inputReader != null)
        {
            inputReader.PrimaryFireEvent -= HandleFire;
            Debug.Log("[PlayerShooting] 🔌 Unsubscribed from PrimaryFireEvent.");
        }
    }

    private void HandleFire(bool isFiring)
    {
        if (!IsOwner)
        {
            Debug.Log("[PlayerShooting] ⛔ Not owner, ignore fire input.");
            return;
        }

        if (!isFiring) return;

        Debug.Log("[PlayerShooting] 🔫 Firing requested!");

        // บอก Server ให้สร้างกระสุน
        FireServerRpc(firePoint.position, firePoint.up);
    }

    [ServerRpc]
    private void FireServerRpc(Vector2 position, Vector2 direction)
    {
        Debug.Log($"[Server] 🔨 Spawning bullet at {position} with direction {direction}");

        GameObject bullet = Instantiate(bulletPrefab, position, Quaternion.identity);
        NetworkObject bulletNetworkObject = bullet.GetComponent<NetworkObject>();
        bulletNetworkObject.Spawn();

        Bullet bulletScript = bullet.GetComponent<Bullet>();
        if (bulletScript != null)
        {
            bulletScript.SetDirectionServerRpc(direction);
            Debug.Log("[Server] ✅ Bullet direction set.");
        }
        else
        {
            Debug.LogError("[Server] ❌ Bullet script not found on bullet prefab!");
        }
    }
}
