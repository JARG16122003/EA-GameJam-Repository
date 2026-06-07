using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.InputSystem;

public class EscenarioDestruir : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private Tilemap tilemap;
    [SerializeField] private Transform objetivoCamara; // GameObject vacío que actúa de target
    [SerializeField] private GameObject explosionPrefab;

    [Header("Destrucción")]
    [SerializeField] private float tiempoEntreFilas = 0.08f;
    [SerializeField] private int destruccionesExtraPorFila = 40;
    [SerializeField] private float probabilidadDestruccion = 0.65f;

    [Header("Explosiones")]
    [SerializeField] private int explosionesPorFila = 15;
    [SerializeField] private int rangoVerticalExplosion = 3;

    private bool destruccionActiva;
    private int enemigosRestantes;
    private BoundsInt bounds;

    private void Start()
    {
        if (tilemap == null)
        {
            Debug.LogError("No se asignó ningún Tilemap.");
            return;
        }

        bounds = tilemap.cellBounds;
        InicializarEnemigos();
    }

    private void Update()
    {
        if (Keyboard.current != null &&
            Keyboard.current.spaceKey.wasPressedThisFrame &&
            !destruccionActiva)
        {
            Debug.Log("Destrucción iniciada manualmente.");
            IniciarDestruccion();
        }
    }

    private void InicializarEnemigos()
    {
        EnemyManager[] enemies = FindObjectsOfType<EnemyManager>();
        enemigosRestantes = enemies.Length;

        foreach (EnemyManager enemy in enemies)
        {
            CharacterAttributes attributes =
                enemy.GetComponent<CharacterAttributes>();

            if (attributes != null)
                attributes.onCharacterDead += OnEnemyDead;
        }

        Debug.Log("Robots encontrados: " + enemigosRestantes);
    }

    private void OnEnemyDead()
    {
        enemigosRestantes--;
        Debug.Log("Robots restantes: " + enemigosRestantes);

        if (enemigosRestantes <= 0 && !destruccionActiva)
            IniciarDestruccion();
    }

    private void IniciarDestruccion()
    {
        if (destruccionActiva) return;
        destruccionActiva = true;
        StartCoroutine(BarridoVerticalIrregular());
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