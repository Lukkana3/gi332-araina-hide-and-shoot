using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class HealingZone : NetworkBehaviour
{
    [SerializeField] private int healAmount = 20;
    [SerializeField] private float requiredStayTime = 0f;
    [SerializeField] private float cooldownTime = 60f;

    private Dictionary<ulong, float> enterTime = new();
    private Dictionary<ulong, float> lastHealTime = new();

    private void OnTriggerStay2D(Collider2D other)
    {
        if (!IsServer) return;

        // พยายามดึง NetworkObject จากตัวลูกหรือพ่อ
        NetworkObject netObj = other.GetComponent<NetworkObject>();
        if (netObj == null)
            netObj = other.GetComponentInParent<NetworkObject>();
        if (netObj == null) return;

        ulong clientId = netObj.OwnerClientId;

        // พยายามดึง Health จากลูกหรือพ่อ
        Health health = other.GetComponent<Health>();
        if (health == null)
            health = other.GetComponentInParent<Health>();
        if (health == null) return;

        // บันทึกเวลาเข้า
        if (!enterTime.ContainsKey(clientId))
            enterTime[clientId] = Time.time;

        float timeInZone = Time.time - enterTime[clientId];
        float lastHealed = lastHealTime.ContainsKey(clientId) ? lastHealTime[clientId] : -cooldownTime;

        if (timeInZone >= requiredStayTime && Time.time - lastHealed >= cooldownTime)
        {
            health.Heal(healAmount);
            lastHealTime[clientId] = Time.time;
            enterTime[clientId] = Time.time + 999f; // กันฮีลซ้ำทันที
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        NetworkObject netObj = other.GetComponent<NetworkObject>();
        if (netObj == null)
            netObj = other.GetComponentInParent<NetworkObject>();
        if (netObj == null) return;

        ulong clientId = netObj.OwnerClientId;

        if (enterTime.ContainsKey(clientId))
            enterTime.Remove(clientId);
    }
}
