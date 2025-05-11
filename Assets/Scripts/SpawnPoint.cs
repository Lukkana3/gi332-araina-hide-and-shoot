using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    private static List<SpawnPoint> spawnPoints = new List<SpawnPoint>();
    private static HashSet<SpawnPoint> usedSpawnPoints = new HashSet<SpawnPoint>();

    public static int Count => spawnPoints.Count; // ✅ เพิ่มบรรทัดนี้

    private void OnEnable()
    {
        if (!spawnPoints.Contains(this))
        {
            spawnPoints.Add(this);
            //Debug.Log($"SpawnPoint registered at: {transform.position}");
        }
    }

    private void OnDisable()
    {
        spawnPoints.Remove(this);
        usedSpawnPoints.Remove(this);
    }

    public static Vector3 GetRandomSpawnPos()
    {
        var available = spawnPoints.FindAll(p => !usedSpawnPoints.Contains(p));

        if (available == null || available.Count == 0)
        {
            //Debug.LogWarning("⚠️ No available spawn points left, reusing from all");
            available = new List<SpawnPoint>(spawnPoints);
            usedSpawnPoints.Clear();
        }

        if (available.Count == 0)
        {
            //Debug.LogError("❌ No spawn points available at all!");
            return Vector3.zero; // fallback
        }

         var point = available[Random.Range(0, available.Count)];
        usedSpawnPoints.Add(point);
        return point.transform.position;
    }

}
