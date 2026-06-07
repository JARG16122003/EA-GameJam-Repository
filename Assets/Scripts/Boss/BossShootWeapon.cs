using System.Collections;
using UnityEngine;

public class BossShootWeapon : MonoBehaviour
{
    [SerializeField] 
    private GameObject weaponLeftSide;
    [SerializeField] 
    private GameObject weaponRightSide;
    [SerializeField] 
    private GameObject weaponCenterSide;

    [Header("Side Weapons - Timing")]
    [SerializeField] 
    private float sideMinBurstDuration = 0.4f;
    [SerializeField] 
    private float sideMaxBurstDuration = 1.2f;
    [SerializeField] 
    private float sideMinCooldown = 0.5f;
    [SerializeField] 
    private float sideMaxCooldown = 1.8f;

    [Header("Center Weapon - Timing")]
    [SerializeField] 
    private float centerMinBurstDuration = 0.6f;
    [SerializeField] 
    private float centerMaxBurstDuration = 1.5f;
    [SerializeField] 
    private float centerMinCooldown = 2.5f;
    [SerializeField] 
    private float centerMaxCooldown = 5f;

    // Probabilidad de que ambas armas laterales disparen al mismo tiempo (0-1)
    [Header("Behavior")]
    [Range(0f, 1f)]
    [SerializeField] 
    private float bothSidesSimultaneousChance = 0.5f;

    private IWeapon interfaceWeaponLeft;
    private IWeapon interfaceWeaponRight;
    private IWeapon interfaceWeaponCenter;

    private Transform player;
    private bool isBossActive = false;

    void Start()
    {
        if (weaponLeftSide != null) interfaceWeaponLeft = weaponLeftSide.GetComponent<IWeapon>();
        if (weaponRightSide != null) interfaceWeaponRight = weaponRightSide.GetComponent<IWeapon>();
        if (weaponCenterSide != null) interfaceWeaponCenter = weaponCenterSide.GetComponent<IWeapon>();

        PlayerManager.onPlayerDead += StopAllWeapons;
    }

    private void OnDestroy()
    {
        PlayerManager.onPlayerDead -= StopAllWeapons;
    }

    public void SetPlayerPosition(Transform playerPosition)
    {
        player = playerPosition;

        if (player != null && !isBossActive)
        {
            isBossActive = true;
            StartCoroutine(SideWeaponsRoutine());
            StartCoroutine(CenterWeaponRoutine());
        }
    }

    public void StopAllWeapons()
    {
        PlayerManager.onPlayerDead -= StopAllWeapons;

        isBossActive = false;
        StopAllCoroutines();

        interfaceWeaponLeft?.StopShooting();
        interfaceWeaponRight?.StopShooting();
        interfaceWeaponCenter?.StopShooting();
    }

    private IEnumerator SideWeaponsRoutine()
    {
        while (isBossActive)
        {
            float cooldown = Random.Range(sideMinCooldown, sideMaxCooldown);
            yield return new WaitForSeconds(cooldown);


            bool simultaneous = Random.value <= bothSidesSimultaneousChance;

            if (simultaneous)
            {
                yield return StartCoroutine(FireBothSides());
            }
            else
            {
                bool leftFirst = Random.value > 0.5f;

                yield return StartCoroutine(FireOneSide(leftFirst ? interfaceWeaponLeft : interfaceWeaponRight));

                yield return new WaitForSeconds(Random.Range(0.1f, 0.4f));

                yield return StartCoroutine(FireOneSide(leftFirst ? interfaceWeaponRight : interfaceWeaponLeft));
            }
        }
    }


    private IEnumerator FireBothSides()
    {
        interfaceWeaponLeft?.Shoot();
        interfaceWeaponRight?.Shoot();

        float duration = Random.Range(sideMinBurstDuration, sideMaxBurstDuration);
        yield return new WaitForSeconds(duration);

        interfaceWeaponLeft?.StopShooting();
        interfaceWeaponRight?.StopShooting();
    }

    private IEnumerator FireOneSide(IWeapon weapon)
    {
        weapon?.Shoot();

        float duration = Random.Range(sideMinBurstDuration, sideMaxBurstDuration);
        yield return new WaitForSeconds(duration);

        weapon?.StopShooting();
    }

    private IEnumerator CenterWeaponRoutine()
    {
        while (isBossActive)
        {
            // Cooldown más largo antes de que dispare el centro
            float cooldown = Random.Range(centerMinCooldown, centerMaxCooldown);
            yield return new WaitForSeconds(cooldown);

            interfaceWeaponCenter.Shoot();

            float duration = Random.Range(centerMinBurstDuration, centerMaxBurstDuration);
            yield return new WaitForSeconds(duration);

            interfaceWeaponCenter.StopShooting();
        }
    }

    
}
