using UnityEngine;

public class PlayerLightController : MonoBehaviour
{
    public Transform player;
    public Transform lightTransform;

    void Update()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = (mousePos - player.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        lightTransform.rotation = Quaternion.Euler(0, 0, angle);
    }
}
