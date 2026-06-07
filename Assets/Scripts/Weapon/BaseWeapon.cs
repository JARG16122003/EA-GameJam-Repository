using System;
using System.Collections;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;
using Random = UnityEngine.Random;

public class BaseWeapon : MonoBehaviour , IWeapon
{
    public event Action onWeaponShot;

    [SerializeField]
    private GameObject bulletPrefab;

    [SerializeField]
    private Transform muzzleWeapon;

    [SerializeField]
    private bool isBurstFire = false;

    [SerializeField]
    private float RateFire = 0.5f;

    [SerializeField]
    private float WeaponPower = 20.0f;

    private bool isShooting = false;

    public AudioSource shootAudioSource; 
    public AudioClip shoot;

    public void Shoot()
    {
        if (isShooting) return;
        isShooting = true;

        StartCoroutine(StartSpawnBullets());
    }

    public void StopShooting()
    {
        
        StopAllCoroutines();
        isShooting = false;
    }
    
    private IEnumerator StartSpawnBullets()
    {
        SpawnBullets();
        onWeaponShot?.Invoke();
        yield return new WaitForSeconds(RateFire);
        StartCoroutine(StartSpawnBullets());
    }

    private void SpawnBullets()
    {
        if (!CanShoot()) return;

        shootAudioSource.pitch = Random.Range(0.7f, 1.3f);
        shootAudioSource.PlayOneShot(shoot);

        GameObject bullet = Instantiate(bulletPrefab, muzzleWeapon.position, muzzleWeapon.rotation);

        Rigidbody2D rigidbodyBullet = bullet.GetComponent<Rigidbody2D>();

        rigidbodyBullet.linearVelocity = muzzleWeapon.right * WeaponPower;

        Destroy(bullet, 3.0f);
    }

    private bool CanShoot()
    {
        return bulletPrefab != null && muzzleWeapon != null;
    }

}
