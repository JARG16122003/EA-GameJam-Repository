using UnityEngine;

public class UserInterfaceManager : MonoBehaviour
{
    [SerializeField]
    private Animator animatorGameOver;

    void Start()
    {
        PlayerManager.onPlayerDead += ShowGameOver;
    }

    private void ShowGameOver()
    {
        PlayerManager.onPlayerDead -= ShowGameOver;
        StartAnimation("anim_ShowGameOver", animatorGameOver);
    }

    private void StartAnimation(string nameAnimation, Animator animator)
    {
        if (animator != null)
        {
            animator.CrossFade(nameAnimation, 0.2f);
        }
    }
}
