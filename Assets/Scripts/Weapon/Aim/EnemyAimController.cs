using UnityEngine;

public class EnemyAimController : WeaponAim
{
    private Transform playerPosition;

    // Update is called once per frame
    void Update()
    {
        if (playerPosition == null) return;
        AimWeaponAtDirection(playerPosition.position);
    }

    public void SetPlayerPosition(Transform position) {  playerPosition = position; }

}
