using Unity.Netcode;
using UnityEngine;

public class PlayerCameraActivator : NetworkBehaviour
{
    private void Start()
    {
        if (!IsOwner)
        {
            // ปิดกล้องถ้าไม่ใช่ของตัวเอง
            gameObject.SetActive(false);
        }
    }
}
