using UnityEngine;

public class FogOfWarController : MonoBehaviour
{
    public Material fogMaterial;
    public Transform player;

    void Update()
    {
        if (fogMaterial == null || player == null) return;

        Vector2 playerPos = Camera.main.WorldToViewportPoint(player.position);
        fogMaterial.SetVector("_Center", new Vector4(playerPos.x, playerPos.y, 0, 0));
    }
}
