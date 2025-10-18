using UnityEngine;

public class ChinBehavior : MonoBehaviour
{
    [SerializeField] ParticleSystem bubbleSystem;
    SpriteRenderer spRend;

    enum ChinStates
    {
        breathing,
        hiding,
        stretching,
        leaving,
        idling
    }

    ChinStates state = ChinStates.idling;

    //Sprites
    [SerializeField] Sprite spriteHide, spriteIdle, spriteStretch, spriteHole;

    //Timers
    [SerializeField]
    float idleSwitchCD = 10f;
    private float idleSwitchStep = 0f;


    //others
    [SerializeField]
    float leaveSpeed = 1.5f;
    private bool isLeaving = false;


    void Start()
    {
        spRend = GetComponent<SpriteRenderer>();

    }

    void Update()
    {
        if (state != ChinStates.breathing)
            bubbleSystem.Pause();

        idleSwitchStep += Time.deltaTime;

        switch (state)
        {
            case ChinStates.idling:
                RunIdle();
                break;
            case ChinStates.breathing:
                RunBreathe();
                break;
            case ChinStates.hiding:
                RunHide();
                break;
            case ChinStates.stretching:
                RunStretch();
                break;
            case ChinStates.leaving:
                RunLeave();
                break;
        }

        if (isLeaving)
        {
            transform.position += Vector3.up * leaveSpeed * Time.deltaTime;
        }


        if (idleSwitchStep >= idleSwitchCD)
        {
            ChangeState(ChinStates.idling);
        }
    }

    void RunIdle()
    {
        spRend.sprite = spriteIdle;

        if (idleSwitchStep >= idleSwitchCD)
        {
            int randNum = Random.Range(0, 3);
            if (randNum == 0)
                ChangeState(ChinStates.breathing);
            else if (randNum == 1)
                ChangeState(ChinStates.stretching);
            else
                ChangeState(ChinStates.hiding);
        }
    }

    void RunBreathe()
    {
        spRend.sprite = spriteIdle;
        if (!bubbleSystem.isPlaying)
            bubbleSystem.Play();
    }

    void RunHide()
    {
        spRend.sprite = spriteHide;
    }


    void RunStretch()
    {
        spRend.sprite = spriteStretch;

        
        if (Random.Range(0, 10) < 2)
        {
            state = ChinStates.leaving;
            spRend.sprite = spriteHole;
            isLeaving = true;
            return;
        }

        idleSwitchStep += Time.deltaTime;
        if (idleSwitchStep >= idleSwitchCD)
        {
            idleSwitchStep = 0;
            state = ChinStates.idling;
        }
    }

    void RunLeave()
    {
        spRend.sprite = spriteHole;
        isLeaving = true;
        if (transform.position.y > 10f)
        {
            Destroy(gameObject);
        }
    }

    void ChangeState(ChinStates newState)
    {
        state = newState;
        idleSwitchStep = 0f;
    }

}