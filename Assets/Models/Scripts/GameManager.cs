using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        if (CollectibleCube.collected >= CollectibleCube.total)
        {
            Debug.Log("YOU WIN");
        }
    }

    public void GameOver()
    {
        Debug.Log("GAME OVER");
    }
}