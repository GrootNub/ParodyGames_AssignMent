using UnityEngine;
using System;

public class GravityManager : MonoBehaviour
{
    public static GravityManager Instance;

    public Vector3 CurrentGravity { get; private set; } = Vector3.down;
    public float gravityStrength = 20f;

    public event Action<Vector3> OnGravityChanged;

    void Awake()
    {
        Instance = this;
    }

    public void SetGravity(Vector3 direction)
    {
        CurrentGravity = direction.normalized;

        Physics.gravity = CurrentGravity * gravityStrength;

        OnGravityChanged?.Invoke(CurrentGravity);
    }
}