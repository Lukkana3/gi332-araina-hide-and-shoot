using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : NetworkBehaviour
{
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private Slider healthBar;

    private NetworkVariable<int> currentHealth = new NetworkVariable<int>();

    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            currentHealth.Value = maxHealth;
        }

        UpdateHealthBar(currentHealth.Value);

        // Subscribe ค่าที่เปลี่ยนจาก NetworkVariable
        currentHealth.OnValueChanged += (oldValue, newValue) =>
        {
            UpdateHealthBar(newValue);
        };
    }

    [ServerRpc(RequireOwnership = false)]
    public void TakeDamageServerRpc(int amount)
    {
        if (!IsServer) return;

        currentHealth.Value -= amount;
        currentHealth.Value = Mathf.Max(currentHealth.Value, 0);

        Debug.Log($"[PlayerHealth] 💔 New HP: {currentHealth.Value}");

        if (currentHealth.Value <= 0)
        {
            Die();
        }
    }

    private void UpdateHealthBar(int value)
    {
        if (healthBar != null)
        {
            healthBar.value = (float)value / maxHealth;
        }
    }

    private void Die()
    {
        Debug.Log("[PlayerHealth] ☠️ Player died.");
        // TODO: Add respawn, disable input, etc.
    }
}
