using UnityEngine;

public class SakanaBehavior : MonoBehaviour
{

    bool inPosition = false;
    Vector3 randPos = new Vector3(0,0,0);
    SpriteRenderer spRend;

    enum SakanaStates
    {
        idle,
        hiding,
        dying,
        breathe
        
    }

    SakanaStates state = SakanaStates.idle;

    //GameObject
    [SerializeField]
    Transform topLeft;
    [SerializeField]
    Transform bottomRight;
    [SerializeField]
    GameObject BubbleParticle;
    [SerializeField]
    ParticleSystem bubbleSystem;
    private Transform hideSpot;
    [SerializeField]
    Transform leftTip;
    [SerializeField]
    Transform rightTip;


    //Timers
    float hideDuration = 3f;
    float idleStep = 10f;
    float breatheDuration = 5f;
    float idleMoveCD = 0f;
    float idleMoveStep = 0f;
    float stateTimer = 0f;
    float lifeTimer = 100f;
    float lifeStep = 0f;

    //Others
    [SerializeField]
    AnimationCurve fishMoveCurve;
    [SerializeField]
    float lerpTime, lerpTimeMax;
    [SerializeField]
    Sprite caughtSprite;
    private bool isBreathing = false;


    void Start()
    {
        spRend = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        lifeStep += Time.deltaTime;
        if (lifeStep >= lifeTimer)
        {
            Destroy(gameObject);
        }

        switch (state)
        {
            case SakanaStates.idle:
                RunIdle();
                break;
            case SakanaStates.hiding:
                RunHiding();
                break;
            case SakanaStates.dying:
                break;
            case SakanaStates.breathe:
                RunBreathe();
                break;

        }
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

    void RunIdle()
    {
        stateTimer += Time.deltaTime;
        if (idleMoveCD == 0)
        {
            idleMoveCD = Random.Range(1, 2);

        }
        if (idleMoveStep < idleMoveCD)
        {
            idleMoveStep += Time.deltaTime;

        }
        else
        {
            if(randPos == new Vector3(0, 0, 0))
            {
                lerpTime = 0f;
                randPos = new Vector3(Random.Range(topLeft.position.x, bottomRight.position.x), Random.Range(topLeft.position.y, bottomRight.position.y));

            }
            
            if (!inPosition)
            {
                Move(randPos);
            }
            if(transform.position != randPos)
            {
                inPosition = false;
            }
            else
            {
                idleMoveCD = 0;
                idleMoveStep = 0;
                inPosition = true;
                randPos = Vector3.zero;
                lerpTime = 0;
            }

            if (stateTimer >= idleStep)
            {
                stateTimer = 0;
                state = SakanaStates.breathe;
            }
            

        }


    }

    void RunBreathe()
    {
        if (!isBreathing)
        {
            if (spRend.flipX)
            { BubbleParticle.transform.position = rightTip.position; }
            else { BubbleParticle.transform.position = leftTip.position; }
            isBreathing = true;
            bubbleSystem.Play();
            stateTimer = 0;
        }

        stateTimer += Time.deltaTime;
        if (stateTimer >= breatheDuration)
        {
            stateTimer = 0;
            bubbleSystem.Stop();
            isBreathing = false;
            state = SakanaStates.idle;
        }
    }

    void RunHiding()
    {
        if (hideSpot == null)
        {
            GameObject rock = GameObject.FindGameObjectWithTag("rock");
            if (rock != null)
                hideSpot = rock.transform;
                lerpTime = 0f;
        }

        if (hideSpot != null)
        {
            Vector3 hidePos = new Vector3(hideSpot.position.x,hideSpot.position.y);
            Vector3 currentPos = transform.position;
            Move(hidePos);
        }

        stateTimer += Time.deltaTime;
        if (stateTimer >= hideDuration)
        {
            stateTimer = 0;
            state = SakanaStates.idle;
        }
    }


    public void OnCaught()
    {
        state = SakanaStates.dying;
        spRend.sprite = caughtSprite;
        bubbleSystem.Stop();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("tako") && state != SakanaStates.hiding)
        {

            state = SakanaStates.hiding;
            stateTimer = 0;
        }
    }
}
