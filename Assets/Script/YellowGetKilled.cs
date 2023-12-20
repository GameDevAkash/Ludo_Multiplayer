using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YellowGetKilled : MonoBehaviour
{
    private Vector3 initialPos;
    private Vector3 initialScale;
    public int CheckDist;
    Dice dice;
    private void Start()
    {
        initialPos = transform.position;
        dice = GameObject.Find("dice").GetComponent<Dice>();
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "GreenPawn" || other.tag == "BluePawn" || other.tag == "RedPawn")
        {
            
            if  (!gameObject.GetComponent<PawnMovement>().safeHome)
            {
                Dice.reshot = true;
                if (other.GetComponent<PawnMovement>().moveComplete && other.GetComponent<PawnMovement>().moveBackComplete)
                {
                    if (Dice.turn != 3)
                    {
                        
                        Dice.TotalYellowInPlay -= 1;
                        gameObject.GetComponent<PawnMovement>().MoveBack();
                        //Debug.Log(Dice.turn);
                        gameObject.GetComponent<PawnMovement>().CanPlay = false;
                        gameObject.GetComponent<PawnMovement>().firstSix = false;

                        gameObject.GetComponent<PawnMovement>().call = true;


                    }
                }   
            }

        }

    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "GreenPawn" || other.tag == "BluePawn" || other.tag == "YellowPawn")
        {
            Dice.reshot = false;
            if (CheckDist == other.GetComponent<PawnMovement>().Dist)
            {
                gameObject.transform.localScale = new Vector3(25, 25, 25);
            }
        }

    }

}
