
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : NetworkBehaviour
{
    [SerializeField] private int maxHealth = 100;
    private NetworkVariable<int> currentHealth = new NetworkVariable<int>();

    [SerializeField] private Slider healthBar; // Slider ที่ลากจาก Inspector ได้
    private void Start()
    {
        if (healthBar == null)
        {
            healthBar = GetComponentInChildren<Slider>();
            Debug.LogWarning("HealthBar assigned automatically: " + healthBar);
        }
    }

    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            currentHealth.Value = maxHealth;
        }
        UpdateHealthBar();
    }

    [ServerRpc]
    public void TakeDamageServerRpc(int damage)
    {
        if (IsServer)
        {
            currentHealth.Value -= damage;
            if (currentHealth.Value < 0)
                currentHealth.Value = 0;

            Debug.Log($"[Server] Player took damage. New HP: {currentHealth.Value}");

            UpdateHealthBarClientRpc(currentHealth.Value);
        }
    }

    [ClientRpc]
    private void UpdateHealthBarClientRpc(int newHealth)
    {
        if (healthBar != null)
            healthBar.value = (float)newHealth / maxHealth;
    }


    private void Die()
    {
        Debug.Log("Player Died!");
        // TODO: กำหนดสิ่งที่เกิดขึ้นเมื่อผู้เล่นตาย
    }

    private void UpdateHealthBar()
    {
        if (healthBar != null)
        {
            healthBar.value = (float)currentHealth.Value / maxHealth;
        }
    }
}
