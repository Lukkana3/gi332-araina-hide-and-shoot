using Unity.Netcode;
using UnityEngine;

public class Health : NetworkBehaviour
{
    [SerializeField] private int maxHealth = 100;

    // ให้ public เพื่อให้ HealthDisplay.cs เรียกใช้ได้
    public int MaxHealth => maxHealth;

    // ให้ public เพื่อให้ HealthDisplay.cs เรียกได้
    public NetworkVariable<int> CurrentHealth = new NetworkVariable<int>();

    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            CurrentHealth.Value = maxHealth;
        }
    }

    // ฟังก์ชันนี้ถูกเรียกจาก DealDamageOnContact.cs
    public void TakeDamage(int damage)
    {
        if (!IsServer) return;

        CurrentHealth.Value -= damage;

        if (CurrentHealth.Value <= 0)
        {
            CurrentHealth.Value = 0;
            HandleDeath();
        }
    }

    private void HandleDeath()
    {
        //Debug.Log($"🪦 Player {OwnerClientId} died.");
        GameOverManager.Instance.PlayerDied(OwnerClientId);
    }
    public void Heal(int amount)
    {
        if (!IsServer) return;

        CurrentHealth.Value += amount;
        if (CurrentHealth.Value > maxHealth)
            CurrentHealth.Value = maxHealth;

        //Debug.Log($"💚 Healed {amount}, now HP = {CurrentHealth.Value}");
    }
}






