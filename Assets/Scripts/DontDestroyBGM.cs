using UnityEngine;

public class DontDestroyBGM : MonoBehaviour
{
    private static DontDestroyBGM instance;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject); // ป้องกันไม่ให้ซ้ำ
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }
}
