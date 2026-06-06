using System;
using UnityEngine;

public interface IWeapon
{
    void Shoot();

    void StopShooting();

    event Action onWeaponShot;
}
