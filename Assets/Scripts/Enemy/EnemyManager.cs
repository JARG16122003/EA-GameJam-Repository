using UnityEngine;

enum EnemyState
{
    Idle,
    Chasing,
    Dead
}
public class EnemyManager : MonoBehaviour
{
    [Header("Reference Area")]
    [SerializeField]
    private TriggerArea currentArea;
    
    private EnemyAimController aimControllerComponent;
    private EnemyChasePlayer enemyChaseComponent;
    private EnemyShoot enemyShootComponent;

    void Start()
    {
        SuscribeEvent();
        GetComponents();
    }

    private void SuscribeEvent()
    {
        if (currentArea == null) return;
        currentArea.onPlayerEnter += InitializeTransforms;
    }

    private void GetComponents()
    {
        enemyChaseComponent = GetComponent<EnemyChasePlayer>();
        aimControllerComponent = GetComponentInChildren<EnemyAimController>();
        enemyShootComponent = GetComponent<EnemyShoot>();
    }

    private void InitializeTransforms(Transform transformPlayer)
    {
        currentArea.onPlayerEnter -= InitializeTransforms;

        if (enemyChaseComponent != null)
        {
            enemyChaseComponent.SetPlayerPosition(transformPlayer);
        }

        if(aimControllerComponent)
        {
            aimControllerComponent.SetPlayerPosition(transformPlayer);
        }

        if(enemyShootComponent)
        {
            enemyShootComponent.SetPlayerPosition(transformPlayer);
        }


    }
}
