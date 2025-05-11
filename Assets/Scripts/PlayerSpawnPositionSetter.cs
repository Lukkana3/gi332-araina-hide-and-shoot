using Unity.Netcode;
using UnityEngine;
using System.Collections;

public class PlayerSpawnPositionSetter : NetworkBehaviour
{
    public override void OnNetworkSpawn()
    {
        if (IsOwner)
        {
            StartCoroutine(WaitForSpawnPointThenMove());
        }
    }

    private IEnumerator WaitForSpawnPointThenMove()
    {
        // รอจนกว่ามีจุด SpawnPoint อย่างน้อย 1 จุด
        while (SpawnPoint.Count <= 0)
        {
            yield return null;
        }

        Vector3 spawnPos = SpawnPoint.GetRandomSpawnPos();
        transform.position = spawnPos;
        //Debug.Log($"✅ PlayerSpawnPositionSetter moved player to: {spawnPos}");
    }
}
