using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class IntroTimelineManager : MonoBehaviour
{
    [SerializeField]
    private PlayableDirector director;

    private void Start()
    {
        director.stopped += OnTimelineFinished;
    }

    private void OnTimelineFinished(PlayableDirector pd)
    {
        SceneManager.LoadScene("main menu");
    }

    private void OnDestroy()
    {
        director.stopped -= OnTimelineFinished;
    }
}
