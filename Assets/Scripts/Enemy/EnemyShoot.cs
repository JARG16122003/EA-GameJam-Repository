using UnityEngine;

public class EnemyShoot : MonoBehaviour
{
    [SerializeField]
    private GameObject currentWeapon;

    [Header("Distances")]
    [SerializeField]
    private float engageableRange = 6f;    // Distancia a la que empieza a disparar
    



    private Transform player;
    private IWeapon interfaceWeapon;
    private bool isAlreadyShooting = false;

    void Start()
    {
        if (currentWeapon == null) return;
        interfaceWeapon = currentWeapon.GetComponent<IWeapon>();
    }

    void Update()
    {
        if (player == null) return;

        float distanceBetweenPlayer = Vector2.Distance(transform.position, player.position);

        if(distanceBetweenPlayer <= engageableRange && !isAlreadyShooting)
        {
            isAlreadyShooting = true;
            interfaceWeapon.Shoot();
        }
        else if(distanceBetweenPlayer > engageableRange && isAlreadyShooting)
        {
            isAlreadyShooting = false;
            interfaceWeapon.StopShooting();
        }
    }

    public void SetPlayerPosition(Transform playerPosition)
    {
        player = playerPosition;
    }

}
