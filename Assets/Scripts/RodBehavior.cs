using UnityEngine;

public class RodBehavior : MonoBehaviour
{
    enum RodStates
    {
        none,
        inSea,
        moving,
        swinging,
        pulling
    }


    [SerializeField]
    public float sinkSpeed = 2f;
    [SerializeField]
    public float moveSpeed = 2f;
    [SerializeField]
    public float swingSpeed = 1f;
    [SerializeField]
    public float swingAmount = 1f;
    [SerializeField]
    public float minY = 11f;
    [SerializeField]
    Transform rodTip;
    [SerializeField]
    public float holdTime = 1.5f;
    [SerializeField]
    public float actionCD = 5f;
    [SerializeField]
    float offScreenY = 13f;

    private RodStates state = RodStates.none;
    private Vector3 startPosition;
    private GameObject hookedFish;
    private float actionStep = 0;



    void Start()
    {
        startPosition = transform.position;

    }

    void Update()
    {
        switch (state)
        {
            case RodStates.none:
                RunNone();
                break;
            case RodStates.inSea:
                RunInSea();
                break;
            case RodStates.moving:
                RunMoving();
                break;
            case RodStates.swinging:
                RunSwinging();
                break;
            case RodStates.pulling:
                RunPulling();
                break;
        }
    }

    void RunNone()
    {
        if (transform.position.y > minY)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                new Vector3(transform.position.x, minY, transform.position.z),
                sinkSpeed * Time.deltaTime
            );
        }
        else
        {
            state = RodStates.inSea;
        }
    }

    void RunInSea()
    {
        if (actionStep <= actionCD)
        {
            actionStep += Time.deltaTime;
        }
        else
        {
            actionStep = 0;
            int randNum = Random.Range(0, 2);
            Debug.Log(randNum);
            if (randNum == 0)
            {
                state = RodStates.moving;
            }
            if (randNum == 1)
            {
                state = RodStates.swinging;
            }
        }
    }

    void RunMoving()
    {
        float newY = minY + Mathf.Sin(Time.time * moveSpeed) * 0.5f;
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }

    void RunSwinging()
    {
        float newX = startPosition.x + Mathf.Sin(Time.time * swingSpeed) * swingAmount;
        transform.position = new Vector3(newX, transform.position.y, transform.position.z);
    }


    void RunPulling()
    {
        if (transform.position.y < offScreenY)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                new Vector3(transform.position.x, offScreenY, transform.position.z),
                sinkSpeed * Time.deltaTime * 2f
            );

            if (hookedFish != null)
                hookedFish.transform.position = rodTip.position;
        }   else 
        {
            if (hookedFish != null)
            {
                Destroy(hookedFish);
                hookedFish = null;
            }

            Destroy(gameObject);
        }
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("fish") && state != RodStates.pulling)
        {
            hookedFish = other.gameObject;
            hookedFish.GetComponent<SakanaBehavior>()?.OnCaught();
            state = RodStates.pulling;

        }
    }
}


