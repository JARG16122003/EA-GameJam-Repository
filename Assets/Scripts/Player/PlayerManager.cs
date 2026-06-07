using System;
using UnityEngine;

public enum PlayerState
{
    playing,
    onCinematic,
    dead
}

public class PlayerManager : MonoBehaviour
{
    public static event Action onPlayerDead;
    public PlayerState currentState { get; private set; }

    private CharacterAttributes characterAttributes;

    private void Start()
    {
        currentState = PlayerState.playing;

        characterAttributes = GetComponent<CharacterAttributes>();
        if (characterAttributes != null) characterAttributes.onCharacterDead += ChangeStateToDead;
    }

    private void OnDestroy()
    {
        onPlayerDead?.Invoke();
    }

    private void ChangeStateToDead()
    {
        characterAttributes.onCharacterDead -= ChangeStateToDead;
        currentState = PlayerState.dead;
    }

}
