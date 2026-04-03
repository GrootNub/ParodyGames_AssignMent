using UnityEngine;

public class CubeSpawner : MonoBehaviour
{
    public GameObject cubePrefab;
    public int cubeCount = 10;

    public Vector3 spawnAreaMin;
    public Vector3 spawnAreaMax;

    public Transform ground;

    void Start()
    {
        SpawnCubes();
    }

    
    void SpawnCubes()
    {
        Vector3 center = ground.position;
        Vector3 size = ground.localScale * 10f; 

        for (int i = 0; i < cubeCount; i++)
        {
            float x = Random.Range(center.x - size.x / 2, center.x + size.x / 2);
            float z = Random.Range(center.z - size.z / 2, center.z + size.z / 2);

            Vector3 pos = new Vector3(x, center.y + 0.5f, z);

            Instantiate(cubePrefab, pos, Quaternion.identity);
        }
    }
}