using UnityEngine;

public class DontDestroyBGM : MonoBehaviour
{
    private static DontDestroyBGM instance;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject); // ��ͧ�ѹ��������
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }
}
