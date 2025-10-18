using UnityEngine;
using System;

public class TakoBehaviour : MonoBehaviour
{

    public static event Action OnTakoStartMoving;
    public static event Action OnTakoStopMoving;

    enum OctoState
    {
        waiting,
        swimmingToMid,
        spitting
    }


    //gameobjects
    [SerializeField] Transform leftPoint;
    [SerializeField] Transform rightPoint;
    [SerializeField] Transform midPoint;
    [SerializeField] GameObject inkOverlay;

    //floats
    float moveSpeed = 3f;
    float waitTime = 20f;
    float inkDuration = 10f;
    private float timer = 0f;
    private float spitTimer = 0f;

    private OctoState state = OctoState.waiting;

    void Start()
    {
        transform.position = leftPoint.position;
        if (inkOverlay != null) inkOverlay.SetActive(false);
    }

    void Update()
    {
        switch (state)
        {
            case OctoState.waiting:
                RunWaiting();
                break;
            case OctoState.swimmingToMid:
                RunSwimmingToMid();
                break;
            case OctoState.spitting:
                RunSpitting();
                break;
        }
    }

    void RunWaiting()
    {
        timer += Time.deltaTime;
        if (timer >= waitTime)
        {
            timer = 0f;


            transform.position = leftPoint.position;
            state = OctoState.swimmingToMid;
            OnTakoStartMoving?.Invoke();
        }
    }

    void RunSwimmingToMid()
    {
        Vector3 target = midPoint.position;
        transform.position = Vector3.MoveTowards(transform.position, target, moveSpeed * Time.deltaTime);

        if (transform.position == target)
        {
            state = OctoState.spitting;
            spitTimer = 0f;
            if (inkOverlay != null) inkOverlay.SetActive(true);
        }
    }

    void RunSpitting()
    {
        Vector3 farTarget = rightPoint.position;
        transform.position = Vector3.MoveTowards(transform.position, farTarget, moveSpeed * Time.deltaTime);
        spitTimer += Time.deltaTime;
        if (spitTimer >= inkDuration)
        {
            if (inkOverlay != null) inkOverlay.SetActive(false);
            transform.position = farTarget;
            state = OctoState.waiting;
            timer = 0f;
            spitTimer = 0f;
            OnTakoStopMoving?.Invoke();
        }
    }
}

