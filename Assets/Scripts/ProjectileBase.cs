using UnityEngine;

public class ProjectileBase : MonoBehaviour
{
    [SerializeField] protected float speed = 20f;
    protected Vector3 direction;

    public void SetDirection(Vector3 dir)
    {
        direction = dir.normalized;
    }

    protected virtual void Update()
    {
        transform.position += direction * speed * Time.deltaTime;
    }
}
