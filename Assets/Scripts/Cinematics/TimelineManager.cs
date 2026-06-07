using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class TimelineManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField]
    private GameObject cinemachine;
    [SerializeField]
    private GameObject player;
    [SerializeField] 
    private Camera mainCamera;
    [SerializeField]
    private PlayableDirector directorCinematics;
    [SerializeField]
    private string scene;


    [SerializeField]
    [Range(0f, 10f)]
    private float sizeCamera = 5.6f;
    [SerializeField]
    private bool shouldChangeScene = true;

    void Start()
    {
        if(shouldChangeScene)
        {
            directorCinematics.stopped += OnTimelineFinished;
        }
        
    }

    public void StartCinematic()
    {
        if(cinemachine != null) cinemachine.SetActive(false);
        if(player != null) player.SetActive(false);
        if (mainCamera != null) mainCamera.orthographicSize = sizeCamera;
        if(directorCinematics != null) directorCinematics.Play();
    }

    private void OnTimelineFinished(PlayableDirector managerScene)
    {
        directorCinematics.stopped -= OnTimelineFinished;
        SceneManager.LoadScene(scene);
    }

}
