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
            Debug.Log("[Bullet] ✅ Spawned on server.");
        }
    }

    [ServerRpc]
    public void SetDirectionServerRpc(Vector2 dir)
    {
        direction.Value = dir;
        Debug.Log($"[Bullet] 📡 Direction set to {dir}");
    }

    private void Update()
    {
        if (!IsSpawned) return;

        transform.position += (Vector3)(direction.Value * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!IsServer) return;

        GameObject target = other.gameObject;

        // ✅ ถ้าโดน Body หรือชิ้นส่วนลูกของ Player
        if (!target.CompareTag("Player") && target.transform.root.CompareTag("Player"))
        {
            target = target.transform.root.gameObject;
        }

        Debug.Log($"[Bullet] 💥 Hit: {target.name}");

        if (target.CompareTag("Player"))
        {
            Debug.Log("[Bullet] 🎯 Hit Player!");

            if (target.TryGetComponent<NetworkObject>(out var targetNetObj))
            {
                if (targetNetObj.TryGetComponent<PlayerHealth>(out var playerHealth))
                {
                    playerHealth.TakeDamageServerRpc(10);
                    Debug.Log("[Bullet] ➖ Called TakeDamageServerRpc(10)");
                }
                else
                {
                    Debug.LogWarning("[Bullet] ⚠️ PlayerHealth not found!");
                }
            }
            else
            {
                Debug.LogWarning("[Bullet] ⚠️ Target missing NetworkObject!");
            }
        }

        DestroyBullet();
    }

    private void DestroyBullet()
    {
        if (IsServer)
        {
            NetworkObject.Despawn();
            Debug.Log("[Bullet] 🔴 Destroyed.");
        }
    }
}
