using UnityEngine;

public class CollectibleCube : MonoBehaviour
{
    public static int collected = 0;
    public static int total = 0;

    void Start()
    {
        total++;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            collected++;
            Destroy(gameObject);
        }
    }
}