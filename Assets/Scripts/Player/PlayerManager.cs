using System;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static event Action onPlayerDead;

    private void OnDestroy()
    {
        onPlayerDead?.Invoke();
    }
}
