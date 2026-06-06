using System;
using UnityEngine;


public class EnemyChasePlayer : MonoBehaviour
{


    [Header("Movement")]
    [SerializeField]
    private float speed = 4f;

    [Header("Distances")]
    [SerializeField]
    private float chaseRange = 8f;    // Distancia a la que empieza a perseguir
    [SerializeField]
    private float stopDistance = 2f;  // Distancia mínima — no se pega al jugador

    private TriggerArea currentArea;

    private Rigidbody2D rigidbodyEnemy;
    private EnemyAimController aimController;
    private Transform player;

    void Awake()
    {
        rigidbodyEnemy = GetComponent<Rigidbody2D>();

        aimController = GetComponentInChildren<EnemyAimController>();

    }

    void Start()
    {
        
    }

    void FixedUpdate()
    {
        
        ChasePlayerLogic();
    }

    public void SetPlayerPosition(Transform playerPosition)
    {
        player = playerPosition;
    }

    private void ChasePlayerLogic()
    {
        if (player == null)
        {
            rigidbodyEnemy.linearVelocity = Vector2.zero;
            return;
        }

        float distanceBetweenPlayer = Vector2.Distance(rigidbodyEnemy.position, player.position);

        if (distanceBetweenPlayer <= chaseRange && distanceBetweenPlayer > stopDistance)
        {
            // Perseguir: está en rango pero aún lejos
            Vector2 direction = ((Vector2)player.position - rigidbodyEnemy.position).normalized;
            rigidbodyEnemy.linearVelocity = direction * speed;
        }
        else if (distanceBetweenPlayer <= stopDistance)
        {
            // Demasiado cerca: frenar completamente
            rigidbodyEnemy.linearVelocity = Vector2.zero;
        }
        else
        {
            // Fuera de rango: no hacer nada (idle)
            rigidbodyEnemy.linearVelocity = Vector2.zero;
        }
    }


}
