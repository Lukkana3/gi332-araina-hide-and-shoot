using UnityEngine;

public class DestroySelfOnContact : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        //Debug.Log("🔥 Projectile hit: " + other.name);
        Destroy(gameObject);
    }
}

