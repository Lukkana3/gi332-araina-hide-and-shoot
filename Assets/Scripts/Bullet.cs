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
            Invoke(nameof(DestroyBullet), lifeTime);
    }

    [ServerRpc]
    public void SetDirectionServerRpc(Vector2 dir)
    {
        direction.Value = dir;
    }

    private void Update()
    {
        if (!IsSpawned) return;
        transform.position += (Vector3)(direction.Value * speed * Time.deltaTime);
    }

    private void DestroyBullet()
    {
        if (IsServer)
            NetworkObject.Despawn();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!IsServer) return;

        Debug.Log("Bullet hit: " + other.gameObject.name);

        // ตรวจสอบว่าชน Player หรือไม่
        if (other.CompareTag("Player"))
        {
            Debug.Log("Hit Player!");

            if (other.TryGetComponent<PlayerHealth>(out PlayerHealth playerHealth))
            {
                playerHealth.TakeDamageServerRpc(10); // ลด HP 10
            }
        }

        // ตรวจสอบว่าชนกำแพงหรือไม่ (Layer "Wall")
        if (other.gameObject.layer == LayerMask.NameToLayer("Wall"))
        {
            Debug.Log("Bullet hit Wall! Destroying bullet.");
            DestroyBullet(); // ทำลายกระสุน
        }

        // ทำลายกระสุนหลังจากโดน Player หรือ กำแพง
        DestroyBullet();
    }
}
