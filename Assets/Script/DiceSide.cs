using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceSide : MonoBehaviour
{
    public bool onGround;
    public static bool ifAnySideOnGround;
    public int sideValue;

    private void OnTriggerStay(Collider other)
    {
        if(other.tag == "Ground")
        {
            onGround = true;
            ifAnySideOnGround = true;
            //Debug.Log(Dice.diceValue);
        }
    }
    private void OnTriggerExit(Collider other)
    { 
        if (other.tag == "Ground")
        {
            onGround = false;
            ifAnySideOnGround = false;
        }   
    }

    public bool OnGround()
    {
        return onGround;
    }
}
