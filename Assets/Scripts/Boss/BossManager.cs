using System;
using UnityEngine;

public class BossManager : MonoBehaviour
{
    public static event Action onBossDead;

    [Header("Reference Area")]
    [SerializeField]
    private TriggerArea currentArea;
    [SerializeField]
    private EnemyAimController aimRightWeapon;
    [SerializeField]
    private EnemyAimController aimLeftWeapon;
    [SerializeField]
    private EnemyAimController aimCenterWeapon;

    private BossShootWeapon bossWeapons;

    void Start()
    {
        currentArea.onPlayerEnter += InitializeBoss;

        bossWeapons = GetComponent<BossShootWeapon>();

    }

    private void OnDestroy()
    {
        onBossDead?.Invoke();
    }

    private void InitializeBoss(Transform player)
    {
        currentArea.onPlayerEnter -= InitializeBoss;
        if (bossWeapons != null) bossWeapons.SetPlayerPosition(player);
        if(aimRightWeapon != null) aimRightWeapon.SetPlayerPosition(player);
        if(aimLeftWeapon != null) aimLeftWeapon.SetPlayerPosition(player);
        if(aimCenterWeapon != null) aimCenterWeapon.SetPlayerPosition(player);
    }

}
