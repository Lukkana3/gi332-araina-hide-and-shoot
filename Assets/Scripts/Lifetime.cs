using UnityEngine;



public class Lifetime : MonoBehaviour

{

    [SerializeField] private float lifetime = 3f;



    private void Start()

    {

        Destroy(gameObject, lifetime);

    }

}