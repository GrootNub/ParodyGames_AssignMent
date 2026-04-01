using UnityEngine;
using System;

public class GameTimer : MonoBehaviour
{
    public float timeRemaining = 120f;

    public event Action OnTimeUp;

    void Update()
    {
        timeRemaining -= Time.deltaTime;

        if (timeRemaining <= 0)
        {
            OnTimeUp?.Invoke();
            enabled = false;
        }
    }
}