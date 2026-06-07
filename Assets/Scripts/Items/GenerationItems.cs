using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GenerationItems : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> prefabsItemToSpawn = new List<GameObject>();
    [SerializeField]
    private List<Transform> spawnLocations = new List<Transform>();
    [SerializeField]
    private TriggerArea triggerArea;
    [SerializeField]
    private float intervalsBetweenSpawns = 5.0f;

    private Transform CurrentLocation;
    private GameObject currentItem;
    private bool isPlaying = false;
    private int currentIndexPrefab = 0;

    void Start()
    {
        if (triggerArea != null) triggerArea.onPlayerEnter += InitializeGenerator;
        BossManager.onBossDead += StopGenerator;       
        
    }

    private void OnDestroy()
    {
        BossManager.onBossDead -= StopGenerator;
    }

    public void InitializeGenerator(Transform player)
    {
        triggerArea.onPlayerEnter -= InitializeGenerator;

        isPlaying = true;
        StartCoroutine(GenerateItems());
        CurrentLocation = spawnLocations[0];
        SpawnItem(prefabsItemToSpawn[currentIndexPrefab]);
    }

    public void StopGenerator()
    {
        BossManager.onBossDead -= StopGenerator;
        isPlaying = false;
        StopCoroutine(GenerateItems());
    }

    private IEnumerator GenerateItems()
    {
        while(isPlaying)
        {
            yield return new WaitForSeconds(intervalsBetweenSpawns);
            GameObject item = ChooseItem();
            CurrentLocation = ChooseLocation();
            SpawnItem(item);
        }
    }

    private GameObject ChooseItem()
    {
        int indexItem;
        do
        {
            indexItem = Random.Range(0, prefabsItemToSpawn.Count);

        } while (currentIndexPrefab == indexItem);

        currentIndexPrefab = indexItem;

        return prefabsItemToSpawn[indexItem];
    }

    private Transform ChooseLocation()
    {
        int indexLocation;

        do
        {
            indexLocation = Random.Range(0, spawnLocations.Count);
        } while (CurrentLocation == spawnLocations[indexLocation] );

        return spawnLocations[indexLocation];
    }

    private void SpawnItem(GameObject item)
    {
        Instantiate(item, CurrentLocation.position, Quaternion.identity);
    }

}
