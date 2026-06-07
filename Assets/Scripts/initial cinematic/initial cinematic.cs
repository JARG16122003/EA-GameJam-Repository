using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class IntroTimelineManager : MonoBehaviour
{
    [SerializeField]
    private PlayableDirector director;

    private bool skipped = false;

    private void Start()
    {
        director.stopped += OnTimelineFinished;
    }

    private void Update()
    {
        if (skipped) return;

        bool skip =
            (Keyboard.current != null &&
             Keyboard.current.anyKey.wasPressedThisFrame)
            ||
            (Mouse.current != null &&
             Mouse.current.leftButton.wasPressedThisFrame)
            ||
            (Gamepad.current != null &&
             Gamepad.current.buttonSouth.wasPressedThisFrame);

        if (skip)
        {
            skipped = true;

            director.Stop();

            SceneManager.LoadScene("main menu");
        }
    }

    private void OnTimelineFinished(PlayableDirector pd)
    {
        if (skipped) return;

        SceneManager.LoadScene("main menu");
    }

    private void OnDestroy()
    {
        if (director != null)
            director.stopped -= OnTimelineFinished;
    }
}
