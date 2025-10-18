using UnityEngine;

public class KumaBehavior : MonoBehaviour
{

    bool inPosition = false;
    Vector3 randPos = new Vector3(0, 0, 0);
    SpriteRenderer spRend;

    enum KumaStates
    {
        idle,
        child,
        breeding,
        hiding
    }

    KumaStates state = KumaStates.child;

    //GameObject
    [SerializeField]
    GameObject eggPrefab;
    [SerializeField]
    Transform topLeft;
    [SerializeField]
    Transform bottomRight;
    private Transform hideSpot;
    private Transform sango;
    [SerializeField] Sprite childSprite;
    [SerializeField] Sprite idleSprite;


    //Timers
    float idleMoveCD = 0;
    float idleMoveStep = 0;
    float lifeTimer = 300f;
    float lifeStep = 0f;

    //Others
    [SerializeField]
    AnimationCurve fishMoveCurve;
    [SerializeField]
    float lerpTime, lerpTimeMax;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spRend = GetComponent<SpriteRenderer>();
    }

    void OnEnable()
    {
        TakoBehaviour.OnTakoStartMoving += GoHide;
        TakoBehaviour.OnTakoStopMoving += StartBreeding;
    }

    //void OnDisable()
    //{
    //    TakoBehaviour.OnTakoStartMoving -= GoHide;
    //    TakoBehaviour.OnTakoStopMoving -= StartBreeding;
    //}


    // Update is called once per frame
    void Update()
    {
        lifeStep += Time.deltaTime;
        if (lifeStep >= lifeTimer)
        {
            Destroy(gameObject);
        }

        switch (state)
        {
            case KumaStates.idle:
                RunIdle();
                break;
            case KumaStates.child:
                RunChild();
                break;
            case KumaStates.breeding:
                RunBreeding();
                break;
            case KumaStates.hiding:
                RunHiding();
                break;

        }
    }

    void GoHide()
    {
        state = KumaStates.hiding;
    }

    void StartBreeding()
    {
        state = KumaStates.breeding;
    }


    void Move(Vector3 target)
    {
        Vector3 currentPos = transform.position;

        if (target.x > currentPos.x)
        { spRend.flipX = true; }
        else { spRend.flipX = false; }
        lerpTime += Time.deltaTime;
        float percent = fishMoveCurve.Evaluate(lerpTime / lerpTimeMax);
        Vector3.Lerp(transform.position, target, percent);
        Vector3 newPos = Vector3.Lerp(currentPos, target, percent);
        transform.position = newPos;

    }

    void RunChild()
    {
        spRend.sprite = childSprite;
        if (lifeStep >= 5)
        {
            state = KumaStates.idle;
        }
        if (idleMoveCD == 0)
        {
            idleMoveCD = Random.Range(0, 4);

        }
        if (idleMoveStep < idleMoveCD)
        {
            idleMoveStep += Time.deltaTime;

        }
        else
        {
            if (randPos == new Vector3(0, 0, 0))
            {
                randPos = new Vector3(Random.Range(topLeft.position.x, bottomRight.position.x), Random.Range(topLeft.position.y, bottomRight.position.y));

            }

            if (!inPosition)
            {
                Move(randPos);
            }
            if (transform.position != randPos)
            {
                inPosition = false;
            }
            else
            {
                idleMoveCD = 0;
                idleMoveStep = -5;
                inPosition = true;
                randPos = Vector3.zero;
                lerpTime = 0;

            }

            
        }
    }

        void RunIdle()
        {
        spRend.sprite = idleSprite;

        if (idleMoveCD == 0)
            {
                idleMoveCD = Random.Range(3, 7);

            }
            if (idleMoveStep < idleMoveCD)
            {
                idleMoveStep += Time.deltaTime;

            }
            else
            {
                if (randPos == new Vector3(0, 0, 0))
                {
                    randPos = new Vector3(Random.Range(topLeft.position.x, bottomRight.position.x), Random.Range(topLeft.position.y, bottomRight.position.y));

                }

                if (!inPosition)
                {
                    Move(randPos);
                }
                if (transform.position != randPos)
                {
                    inPosition = false;
                }
                else
                {
                    idleMoveCD = 0;
                    idleMoveStep = -5;
                    inPosition = true;
                    randPos = Vector3.zero;
                    lerpTime = 0;

                }

                //Vector3 randPos = new Vector3(Random.Range(topLeft.position.x,bottomRight.position.x),Random.Range(topLeft.position.y,bottomRight.position.y));

            }


        }

        void RunHiding()
        {
            if (hideSpot == null)
            {
                GameObject sango = GameObject.FindGameObjectWithTag("sango");
                if (sango != null)
                    hideSpot = sango.transform;
                lerpTime = 0f;
            }

            if (hideSpot != null)
            {
                Vector3 hidePos = new Vector3(hideSpot.position.x, hideSpot.position.y);
                Vector3 currentPos = transform.position;
                Move(hidePos);
            }

        }

    void RunBreeding()
    {
        if (sango == null)
        {
            GameObject sangoObj = GameObject.FindGameObjectWithTag("sango");
            if (sangoObj != null)
                sango = sangoObj.transform;
        }

        if (sango != null)
        {
            Vector3 sangoPos = new Vector3(sango.position.x, hideSpot.position.y);
            Vector3 currentPos = transform.position;
            Move(sangoPos);

            if (Vector3.Distance(transform.position, sango.position) < 0.05f)
            {
                Instantiate(eggPrefab, sango.position, Quaternion.identity);
                idleMoveCD = 0;
                state = KumaStates.idle;

            }
        }
    }



}
