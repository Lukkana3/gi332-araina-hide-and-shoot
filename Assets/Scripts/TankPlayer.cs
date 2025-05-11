using Unity.Netcode;
using UnityEngine;
using Unity.Cinemachine;

public class TankPlayer : NetworkBehaviour
{
    [Header("References")]
    //[SerializeField] private CinemachineCamera virtualCamera;
    [field: SerializeField] public Health Health { get; private set; }


    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            // เซ็ตตำแหน่งหลังจาก Scene โหลดเสร็จแล้ว → ปลอดภัย
            Vector3 spawnPos = SpawnPoint.GetRandomSpawnPos();
            transform.position = spawnPos;
            //Debug.Log($"[TankPlayer] Spawned at {spawnPos}");
            //Debug.Log($"🎯 OnNetworkSpawn | IsHost: {IsHost} | IsClient: {IsClient} | IsOwner: {IsOwner} | Player name: {gameObject.name}");
        }

    }
}