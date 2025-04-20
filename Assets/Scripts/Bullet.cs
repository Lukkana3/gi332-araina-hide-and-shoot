using Unity.Netcode;
using UnityEngine;

public class Bullet : NetworkBehaviour
{
    [SerializeField] private float speed = 100f;
    [SerializeField] private float lifeTime = 5f;

    private readonly NetworkVariable<Vector2> direction = new NetworkVariable<Vector2>();

    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            Invoke(nameof(DestroyBullet), lifeTime);
            Debug.Log("[Bullet] 🟢 Bullet spawned on Server.");
        }
    }

    [ServerRpc]
    public void SetDirectionServerRpc(Vector2 dir)
    {
        direction.Value = dir;
        Debug.Log($"[Bullet] 📡 Direction set via ServerRpc: {dir}");
    }

    private void Update()
    {
        if (!IsSpawned) return;

        transform.position += (Vector3)(direction.Value * speed * Time.deltaTime);
    }

    private void DestroyBullet()
    {
        if (IsServer)
        {
            Debug.Log("[Bullet] 🔴 Bullet destroyed.");
            NetworkObject.Despawn();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!IsServer) return;

        Debug.Log($"[Bullet] 💥 Hit: {other.gameObject.name}");

        if (other.CompareTag("Player"))
        {
            Debug.Log("[Bullet] 🎯 Hit Player!");

            if (other.TryGetComponent<NetworkObject>(out var targetNetObj))
            {
                if (TryGetComponent<NetworkObject>(out var bulletNetObj))
                {
                    if (targetNetObj.TryGetComponent<PlayerHealth>(out var playerHealth))
                    {
                        playerHealth.TakeDamageServerRpc(10);
                        Debug.Log("[Bullet] ➖ Called TakeDamageServerRpc(10)");
                    }
                    else
                    {
                        Debug.LogWarning("[Bullet] ⚠️ PlayerHealth component not found on Player!");
                    }
                }
                else
                {
                    Debug.LogWarning("[Bullet] ⚠️ Bullet missing NetworkObject.");
                }
            }
            else
            {
                Debug.LogWarning("[Bullet] ⚠️ Player target missing NetworkObject.");
            }
        }

        // ถ้าโดนกำแพง
        if (other.gameObject.layer == LayerMask.NameToLayer("Wall"))
        {
            Debug.Log("[Bullet] 🧱 Hit Wall. Destroying bullet.");
        }

        DestroyBullet(); // กระสุนหายเมื่อชนอะไรก็ตาม
    }
}
