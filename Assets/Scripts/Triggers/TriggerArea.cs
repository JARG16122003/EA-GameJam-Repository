using UnityEngine;
using System;

public class TriggerArea : MonoBehaviour
{
    public event Action<Transform> onPlayerEnter;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        Debug.Log("Jugador Entrando");

        Transform PlayerLocation = collision.gameObject.transform;

        onPlayerEnter?.Invoke(PlayerLocation);
    }
}
