using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : NetworkBehaviour
{
    [SerializeField] private int maxHealth = 100;
    private NetworkVariable<int> currentHealth = new NetworkVariable<int>();

    [SerializeField] private Slider healthBar;

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
        if (!IsServer) return;

        currentHealth.Value -= damage;
        if (currentHealth.Value <= 0)
        {
            currentHealth.Value = 0;
            Die();
        }

        UpdateHealthBarClientRpc(currentHealth.Value);
    }

    [ClientRpc]
    private void UpdateHealthBarClientRpc(int newHealth)
    {
        if (healthBar != null)
        {
            healthBar.value = (float)newHealth / maxHealth;
        }
    }

    private void Die()
    {
        Debug.Log("Player Died!");
        // ·ÓÅÒÂ Player ËÃ×Í ÃÕà«çµµÓáË¹è§
    }

    private void UpdateHealthBar()
    {
        if (healthBar != null)
        {
            healthBar.value = (float)currentHealth.Value / maxHealth;
        }
    }
}
