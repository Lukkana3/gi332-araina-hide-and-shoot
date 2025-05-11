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
            // ���¡��ͧ价����˹觢ͧ����Ф�
            mainCamera.transform.position = transform.position + offset;
        }
    }

    private void LateUpdate()
    {
        if (!IsOwner || mainCamera == null) return;

        // �ѻവ���˹觡��ͧ�����������
        mainCamera.transform.position = transform.position + offset;
    }
}
