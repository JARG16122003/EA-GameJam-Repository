using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

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
        if (characterAttributes != null) characterAttributes.onCharacterDead += DeadSetup;
    }

    private void DeadSetup()
    {
        onPlayerDead?.Invoke();
        characterAttributes.onCharacterDead -= DeadSetup;
        currentState = PlayerState.dead;

        StartCoroutine(RestartLevel());
    }

    private IEnumerator RestartLevel()
    {
        yield return new WaitForSeconds(5.0f);
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }

}
