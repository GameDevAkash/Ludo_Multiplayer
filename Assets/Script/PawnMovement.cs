using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PawnMovement : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject[] MovePoints;
    [SerializeField] float speed;
    public int Dist = 0;
    public bool call=true;
    public bool callBack = true;
    public int i=1;
    public bool firstSix;
    public bool CanPlay;
    public Vector3 initialPos;
    public bool moveComplete;
    public bool moveBackComplete;
    public bool safeHome;
    public Color startColor;
    public bool MoveCalled;
    public bool MovePossible;

    Dice dice;


    private void Start()
    {
        MovePossible = true;
        MoveCalled = false;
        startColor = GetComponent<Renderer>().material.color;
        safeHome = true;
        moveComplete = true;
        moveBackComplete = true;
        firstSix = false;
        CanPlay = false;
        initialPos = transform.position;

        dice = GameObject.Find("dice").GetComponent<Dice>();

    }
    private void Update()
    {
        SafeHomeForPawn();
        ReachedFinalHome();
        PawnScaleExceptSafeHome();
        if(CanPlay)
        {
            if (Dist + Dice.diceValue <= MovePoints.Length)
            {
                MovePossible = true;
            }
            else if (!(Dist + Dice.diceValue < MovePoints.Length))
            {
                MovePossible = false;
            }
        }
    }
    public void Move(int moveLoc)
    {
        

        if (Dist<=MovePoints.Length)
        {
            if (Dist + moveLoc < MovePoints.Length)
            {
                Dist += moveLoc;
                MoveCalled = true;
                StartCoroutine(CallMove());
                //Dice.HasReset = false;
                //CanPlay = true;
            }
        }

        //Debug.Log(transform.tag + " " +  transform.position.ToString());
        
    }

    public void MoveBack()
    {
            StartCoroutine(CallMoveBack());
    }

    public IEnumerator CallMoveBack()
    {
        if (callBack)
        {
            
            callBack = false;
            for (i= Dist; i >= 0; i--)
            {
                moveBackComplete = false;
                transform.position = Vector3.MoveTowards(transform.position, MovePoints[i].transform.position, 1f);
                //Debug.Log(Dice.turn);
                if (i==0)
                {
                    transform.position = initialPos;
                }
                 dice.SyncPawnTransformWithEnemy(transform.gameObject);
                yield return new WaitForSeconds(.3f);
            }
            moveBackComplete = true;
            dice.SyncPawnTransformWithEnemy(transform.gameObject);
            i = 1;
            Dist = 0;
            //Debug.Log(moveComplete.ToString());
            callBack = true;
        }
    }
    //call this move function in ienumerator then call that function in dice

    public IEnumerator CallMove()
    {
        if (call)
        {
            //Debug.Log(Dist);
            call = false; 
            for (; i <= Dist; i++)
            {
                //Dice.diceValue--;
                moveComplete = false;
                transform.position = Vector3.MoveTowards(transform.position, MovePoints[i].transform.position, 1f);
                dice.SyncPawnTransformWithEnemy(transform.gameObject);
                //Debug.Log(Dice.turn);
                yield return new WaitForSeconds(.4f);
               
                //Debug.Log(moveComplete.ToString());
            }
            moveComplete = true;
            dice.SyncPawnTransformWithEnemy(transform.gameObject);
            //Debug.Log(moveComplete.ToString());
            call = true;
        }
    }

    public void SafeHomeForPawn()
    {
        if (Dist == 0 || Dist == 6 || Dist == 10 || Dist == 16 || Dist == 20 || Dist == 26 || Dist == 30 || Dist == 36 || Dist == 43)
        {
            //gameObject.transform.localScale = new Vector3(1,1,1);
            safeHome = true;
        }
        else
        {
            safeHome = false;
        }
    }

    public void ReachedFinalHome()
    {
        if(moveComplete && Dist == 43)
        {
            CanPlay = false;
            //gameObject.SetActive(false);
        }
    }

    public void PawnScaleExceptSafeHome()
    {
        if(!safeHome)
        {
            //transform.localScale = new Vector3(25, 25, 25);
        }
    }
}
