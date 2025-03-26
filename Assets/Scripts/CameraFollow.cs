using Unity.Netcode;
using UnityEngine;

public class CameraFollow : NetworkBehaviour
{
    private Camera mainCamera;

    public Vector3 offset = new Vector3(0, 0, -10);

    private void Start()
    {
        if (!IsOwner) return;

        mainCamera = Camera.main;

        if (mainCamera != null)
        {
            // ย้ายกล้องไปที่ตำแหน่งของตัวละคร
            mainCamera.transform.position = transform.position + offset;
        }
    }

    private void LateUpdate()
    {
        if (!IsOwner || mainCamera == null) return;

        // อัปเดตตำแหน่งกล้องให้ตามผู้เล่น
        mainCamera.transform.position = transform.position + offset;
    }
}
