using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.InputSystem;


using NUnit.Framework;

public class EscenarioDestruir : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private Tilemap tilemap;
    [SerializeField] private Transform objetivoCamara; // GameObject vacío que actúa de target
    [SerializeField] private GameObject explosionPrefab;
    [SerializeField] private TriggerArea triggerArea;
    [SerializeField] private TimelineManager timelineManager;

    [Header("Destrucción")]
    [SerializeField] private float tiempoEntreFilas = 0.08f;
    [SerializeField] private int destruccionesExtraPorFila = 40;
    [SerializeField] private float probabilidadDestruccion = 0.65f;

    [Header("Explosiones")]
    [SerializeField] private int explosionesPorFila = 15;
    [SerializeField] private int rangoVerticalExplosion = 3;

    
    private bool destruccionActiva;
    [SerializeField]
    private int enemigosRestantes;
    private BoundsInt bounds;

    [SerializeField]
    private List<EnemyManager> enemies = new List<EnemyManager>();
    [SerializeField]
    private GameObject jefe;

    private void Start()
    {
        if (tilemap == null)
        {
            Debug.LogError("No se asignó ningún Tilemap.");
            return;
        }

        timelineManager = GetComponent<TimelineManager>();
        bounds = tilemap.cellBounds;
        InicializarEnemigos();
        InicializarJefe();

        if(triggerArea != null)
        {
            triggerArea.onPlayerEnter += IniciarDestruccion;
        }
    }


    private void InicializarEnemigos()
    {
        enemies.AddRange(FindObjectsByType<EnemyManager>(FindObjectsSortMode.None));
        enemigosRestantes = enemies.Count;

        foreach (EnemyManager enemy in enemies)
        {
            CharacterAttributes attributes =
                enemy.GetComponent<CharacterAttributes>();

            if (attributes != null)
                attributes.onCharacterDead += OnEnemyDead;
        }

        Debug.Log("Robots encontrados: " + enemigosRestantes);
    }

    private void InicializarJefe()
    {
        if (jefe == null) return;
        enemigosRestantes = 1;
        CharacterAttributes attributes = jefe.GetComponent<CharacterAttributes>();
        attributes.onCharacterDead += OnEnemyDead;
    }

    private void OnEnemyDead()
    {
        Debug.Log($"OnEnemyDead llamado en frame {Time.frameCount}");
        //enemigosRestantes = enemies.Count;
        enemigosRestantes--;
        Debug.Log("Robots restantes: " + enemigosRestantes);

        if (enemigosRestantes <= 0 && !destruccionActiva)
            IniciarDestruccion(transform);
    }

    private void IniciarDestruccion(Transform transform)
    {
        if (destruccionActiva) return;
        destruccionActiva = true;
        BorrarEnemigosSi();
        IniciarCinematica();
        StartCoroutine(BarridoVerticalIrregular());
    }

    private void BorrarEnemigosSi()
    {
        if (enemigosRestantes <= 0) return;
        foreach(EnemyManager enemy in enemies)
        {
            if(enemy != null)
            {
                Destroy(enemy.gameObject);
            }
        }
    }

    private void IniciarCinematica()
    {
        if (timelineManager != null) timelineManager.StartCinematic();
    }
    private IEnumerator BarridoVerticalIrregular()
    {
        int centroX = (bounds.xMin + bounds.xMax) / 2;

        for (int y = bounds.yMin; y <= bounds.yMax; y++)
        {
            // Mueve el target directamente — la cámara lo sigue sola con su Lerp
            if (objetivoCamara != null)
            {
                Vector3 worldFila =
                    tilemap.GetCellCenterWorld(new Vector3Int(centroX, y, 0));

                objetivoCamara.position = new Vector3(
                    worldFila.x,
                    worldFila.y,
                    objetivoCamara.position.z);
            }

            // Destrucción principal
            for (int x = bounds.xMin; x < bounds.xMax; x++)
            {
                Vector3Int pos = new Vector3Int(x, y, 0);

                if (tilemap.HasTile(pos) && Random.value < probabilidadDestruccion)
                {
                    Vector3 worldPos = tilemap.GetCellCenterWorld(pos);
                    tilemap.SetTile(pos, null);
                    EliminarObjetos(worldPos);
                }
            }

            // Destrucción extra caótica
            for (int i = 0; i < destruccionesExtraPorFila; i++)
            {
                int randomX = Random.Range(bounds.xMin, bounds.xMax);
                int minY = Mathf.Max(bounds.yMin, y - 1);
                int maxY = Mathf.Min(bounds.yMax, y + 4);
                int randomY = Random.Range(minY, maxY);

                Vector3Int randomPos = new Vector3Int(randomX, randomY, 0);

                if (tilemap.HasTile(randomPos))
                {
                    Vector3 worldPos = tilemap.GetCellCenterWorld(randomPos);
                    tilemap.SetTile(randomPos, null);
                    EliminarObjetos(worldPos);
                }
            }

            GenerarExplosiones(y);

            yield return new WaitForSeconds(tiempoEntreFilas);
        }

        Debug.Log("Destrucción del escenario completada.");
    }

    private void EliminarObjetos(Vector3 posicion)
    {
        Collider2D[] objetos = Physics2D.OverlapCircleAll(posicion, 1.5f);

        foreach (Collider2D obj in objetos)
        {
            if (obj == null) continue;
            if (!obj.CompareTag("eliminar")) continue;

            Destroy(obj.gameObject);
        }
    }

    private void GenerarExplosiones(int y)
    {
        if (explosionPrefab == null) return;

        for (int i = 0; i < explosionesPorFila; i++)
        {
            int randomX = Random.Range(bounds.xMin, bounds.xMax);
            int minY = Mathf.Max(bounds.yMin, y - rangoVerticalExplosion);
            int maxY = Mathf.Min(bounds.yMax, y + rangoVerticalExplosion);
            int randomY = Random.Range(minY, maxY);

            Vector3 posicion =
                tilemap.GetCellCenterWorld(new Vector3Int(randomX, randomY, 0));

            GameObject explosion =
                Instantiate(explosionPrefab, posicion, Quaternion.identity);

            Destroy(explosion, 2f);
        }
    }
}