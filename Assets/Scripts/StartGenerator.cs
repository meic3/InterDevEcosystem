using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StartGenerator : MonoBehaviour
{
    [SerializeField]
    GameObject fishPrefab;
    [SerializeField]
    GameObject rodPrefab;

    [SerializeField]
    Transform topleftlimit, bottomrightLimit;


    [SerializeField]
    float initialFishDelayMin = 3f;
    [SerializeField]
    float initialFishDelayMax = 8f;
    [SerializeField]
    float regularFishInterval = 20f;

    private int fishCount = 0;


    //void Awake()
    //{
    //    CreateStartCreatures();
    //}
    void Start()
    {
        StartCoroutine(SpawnInitialFish());
    }

    IEnumerator SpawnInitialFish()
    {
        int initialFishAmount = Random.Range(2, 5);

        for (int i = 0; i < initialFishAmount; i++)
        {
            SpawnFish();
            yield return new WaitForSeconds(Random.Range(initialFishDelayMin, initialFishDelayMax));
        }

        StartCoroutine(SpawnContinuousFish());
    }

    IEnumerator SpawnContinuousFish()
    {
        while (true)
        {
            yield return new WaitForSeconds(regularFishInterval);
            SpawnFish();
        }
    }

    void SpawnFish()
    {
        Vector3 startPos = new Vector3(
            Random.Range(topleftlimit.position.x, bottomrightLimit.position.x),
            Random.Range(topleftlimit.position.y, bottomrightLimit.position.y),
            0f
        );

        Instantiate(fishPrefab, startPos, Quaternion.identity);
        fishCount++;
        if (fishCount % 3 == 0)
        {
            SpawnRod();
        }
    }

    void SpawnRod()
    {
        Vector3 startPos = new Vector3(
            Random.Range(topleftlimit.position.x, bottomrightLimit.position.x),
            12,12);

        Instantiate(rodPrefab, startPos, Quaternion.identity);
    }
}