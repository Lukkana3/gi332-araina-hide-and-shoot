using Unity.Netcode;
using UnityEngine;

public class PlayerCameraActivator : NetworkBehaviour
{
    private void Start()
    {
        if (!IsOwner)
        {
            // �Դ���ͧ��������ͧ����ͧ
            gameObject.SetActive(false);
        }
    }
}
