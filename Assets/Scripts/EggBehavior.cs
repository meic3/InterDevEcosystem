using UnityEngine;

public class EggBehavior : MonoBehaviour
{
    public enum EggTypes
    {
        kumaEgg
    }

    public EggTypes eggType;

    [SerializeField]
    Sprite kumaEggSprite;

    [SerializeField]
    GameObject kumaPrefab;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    [SerializeField]
    float spawnTime = 5f;
    float spawnStep;

    void Start()
    {
        GetComponent<SpriteRenderer>().sprite = SetupEgg();
    }

    // Update is called once per frame
    void Update()
    {
        if (spawnStep >= spawnTime)
        {
            hatchEgg();
        }
        else { spawnStep += Time.deltaTime; }


    }

    Sprite SetupEgg()
    {
        switch (eggType)
        {
            case EggTypes.kumaEgg:
                return kumaEggSprite;
            default:
                return null;
        }
    }

    void hatchEgg()
    {
        switch (eggType)
        {
            case EggTypes.kumaEgg:
                GameObject newSpider = Instantiate(kumaPrefab, transform.position, Quaternion.identity);
                Destroy(gameObject);
                break;
            default:
                break;
        }
    }
}
