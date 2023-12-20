using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FinalHome : MonoBehaviour
{
    [SerializeField] int TotalRed = 0;
    [SerializeField] int TotalGreen = 0;
    [SerializeField] int TotalYellow = 0;
    [SerializeField] int TotalBlue = 0;

    public static bool redOver;
    public static bool greenOver;
    public static bool yellowOver;
    public static bool blueOver;

    [SerializeField] bool redFirst;
    [SerializeField] bool greenFirst;
    [SerializeField] bool yellowFirst;
    [SerializeField] bool blueFirst;

    [SerializeField] bool redSecond;
    [SerializeField] bool greenSecond;
    [SerializeField] bool yellowSecond;
    [SerializeField] bool blueSecond;

    [SerializeField] bool firstPositionComp = false;
    [SerializeField] bool secondPositionComp = false;
    [SerializeField] bool thirdPositionComp = false;

    [SerializeField] GameObject redPositionTxt;
    [SerializeField] GameObject greenPositionTxt;
    [SerializeField] GameObject yellowPositionTxt;
    [SerializeField] GameObject bluePositionTxt;

    // Start is called before the first frame update
    void Start()
    {
        redPositionTxt.SetActive(false);
        greenPositionTxt.SetActive(false);
        yellowPositionTxt.SetActive(false);
        bluePositionTxt.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        Position();
        firstPosition();
        SecondPosition();
        thirdPosition();
        GamePlayOver();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "RedPawn")
        {
            Dice.reshot = true;
            TotalRed += 1;
        }
        if(other.tag == "GreenPawn")
        {
            Dice.reshot = true;
            TotalGreen += 1;
        }

        if (other.tag == "YellowPawn")
        {
            Dice.reshot = true;
            TotalYellow += 1;
        }

        if (other.tag == "BluePawn")
        {
            Dice.reshot = true;
            TotalBlue += 1;
        }
    }

    public void Position()
    {
        if (TotalRed >= 4)
        {
            redOver = true;
            Dice.reshot = false;
            //Debug.Log("Red is Over");
        }

        if (TotalGreen >= 4)
        {
            greenOver = true;
            Dice.reshot = false;
            //Debug.Log("Green is over");
        }

        if (TotalYellow >= 4)
        {
            yellowOver = true;
            Dice.reshot = false;
            //Debug.Log("yellow is over");
        }

        if (TotalBlue >= 4)
        {
            blueOver = true;
            Dice.reshot = false;
            //Debug.Log("Blue is Over");
        }    
    }

    public void firstPosition()
    {
        if(firstPositionComp== false)
        {
            if (redOver && !greenOver && !yellowOver && !blueOver)
            {
                //Red First
                redFirst = true;
                firstPositionComp = true;
                Debug.Log("Red is first");
                redPositionTxt.SetActive(true);
                redPositionTxt.GetComponent<TextMeshProUGUI>().text = "1st";
            }
            if (greenOver && !redOver && !yellowOver && !blueOver)
            {
                //Green First
                greenFirst = true;
                firstPositionComp = true;
                Debug.Log("Green is first");
                greenPositionTxt.SetActive(true);
                greenPositionTxt.GetComponent<TextMeshProUGUI>().text = "1st";
            }
            if (yellowOver && !greenOver && !redOver && !blueOver)
            {
                //Yellow First
                yellowFirst = true;
                firstPositionComp = true;
                Debug.Log("Yellow is first");
                yellowPositionTxt.SetActive(true);
                yellowPositionTxt.GetComponent<TextMeshProUGUI>().text = "1st";
            }
            if (blueOver && !greenOver && !yellowOver && !redOver)
            {
                //Blue First
                blueFirst = true;
                firstPositionComp = true;
                Debug.Log("Blue is first");
                bluePositionTxt.SetActive(true);
                bluePositionTxt.GetComponent<TextMeshProUGUI>().text = "1st";
            }
        }
    }

    public void SecondPosition()
    {
        if(firstPositionComp == true && secondPositionComp == false)
        {
            if (redFirst)
            {
                if (greenOver && !yellowOver && !blueOver)
                {
                    //Green second
                    greenSecond = true;
                    Debug.Log("Green is second");
                    secondPositionComp = true;
                    greenPositionTxt.SetActive(true);
                    greenPositionTxt.GetComponent<TextMeshProUGUI>().text = "2nd";
                }
                if (yellowOver && !greenOver && !blueOver)
                {
                    //Yellow second
                    yellowSecond = true;
                    Debug.Log("Yellow is second");
                    secondPositionComp = true;
                    yellowPositionTxt.SetActive(true);
                    yellowPositionTxt.GetComponent<TextMeshProUGUI>().text = "2ndnd";
                }
                if (blueOver && !greenOver && !yellowOver)
                {
                    //Blue second
                    blueSecond = true;
                    Debug.Log("Blue is second");
                    secondPositionComp = true;
                    bluePositionTxt.SetActive(true);
                    bluePositionTxt.GetComponent<TextMeshProUGUI>().text = "2nd";
                }
            }

            if (greenFirst)
            {
                if (redOver && !yellowOver && !blueOver)
                {
                    //Red second
                    redSecond = true;
                    Debug.Log("Red is second");
                    secondPositionComp = true;
                    redPositionTxt.SetActive(true);
                    redPositionTxt.GetComponent<TextMeshProUGUI>().text = "2nd";
                }
                if (yellowOver && !redOver && !blueOver)
                {
                    //Yellow second
                    yellowSecond = true;
                    Debug.Log("Yellow is second");
                    secondPositionComp = true;
                    yellowPositionTxt.SetActive(true);
                    yellowPositionTxt.GetComponent<TextMeshProUGUI>().text = "2nd";
                }
                if (blueOver && !yellowOver && !redOver)
                {
                    //Blue second
                    blueSecond = true;
                    Debug.Log("Blue is second");
                    secondPositionComp = true;
                    bluePositionTxt.SetActive(true);
                    bluePositionTxt.GetComponent<TextMeshProUGUI>().text = "2nd";
                }
            }

            if (yellowFirst)
            {
                if (redOver && !greenOver && !blueOver)
                {
                    //Red second
                    redSecond = true;
                    Debug.Log("Red is second");
                    secondPositionComp = true;
                    redPositionTxt.SetActive(true);
                    redPositionTxt.GetComponent<TextMeshProUGUI>().text = "2nd";
                }
                if (greenOver && !redOver && !blueOver)
                {
                    //Green second
                    greenSecond = true;
                    Debug.Log("Green is second");
                    secondPositionComp = true;
                    greenPositionTxt.SetActive(true);
                    greenPositionTxt.GetComponent<TextMeshProUGUI>().text = "2nd";
                }
                if (blueOver && !greenOver && !redOver)
                {
                    //Blue second
                    blueSecond = true;
                    Debug.Log("Blue is second");
                    secondPositionComp = true;
                    bluePositionTxt.SetActive(true);
                    bluePositionTxt.GetComponent<TextMeshProUGUI>().text = "2nd";
                }
            }

            if (blueFirst)
            {
                if (redOver && !greenOver && !yellowOver)
                {
                    //Red second
                    redSecond = true;
                    Debug.Log("Red is second");
                    secondPositionComp = true;
                    redPositionTxt.SetActive(true);
                    redPositionTxt.GetComponent<TextMeshProUGUI>().text = "2nd";
                }
                if (greenOver && !redOver && !yellowOver)
                {
                    //Green second
                    greenSecond = true;
                    Debug.Log("Green is second");
                    secondPositionComp = true;
                    greenPositionTxt.SetActive(true);
                    greenPositionTxt.GetComponent<TextMeshProUGUI>().text = "2nd";
                }
                if (yellowOver && !greenOver && !redOver)
                {
                    //Yellow second
                    yellowSecond = true;
                    Debug.Log("Yellow is second");
                    secondPositionComp = true;
                    yellowPositionTxt.SetActive(true);
                    yellowPositionTxt.GetComponent<TextMeshProUGUI>().text = "2nd";
                }

            }
        }
    }

    public void thirdPosition()
    {
        if(firstPositionComp && secondPositionComp && thirdPositionComp == false)
        {
            if (redFirst && greenSecond)
            {
                if (yellowOver && !blueOver)
                {
                    //yellow third
                    Debug.Log("Yellow Third");
                    thirdPositionComp = true;
                    yellowPositionTxt.SetActive(true);
                    yellowPositionTxt.GetComponent<TextMeshProUGUI>().text = "3rdrd";
                }
                if (blueOver && !yellowOver)
                {
                    //blue third
                    Debug.Log("blue Third");
                    thirdPositionComp = true;
                    bluePositionTxt.SetActive(true);
                    bluePositionTxt.GetComponent<TextMeshProUGUI>().text = "3rd";
                }
            }
            if (redFirst && yellowSecond)
            {
                if (greenOver && !blueOver)
                {
                    //green third
                    Debug.Log("green Third");
                    thirdPositionComp = true;
                    greenPositionTxt.SetActive(true);
                    greenPositionTxt.GetComponent<TextMeshProUGUI>().text = "3rd";
                }
                if (blueOver && !greenOver)
                {
                    //blue third
                    Debug.Log("blue Third");
                    thirdPositionComp = true;
                    bluePositionTxt.SetActive(true);
                    bluePositionTxt.GetComponent<TextMeshProUGUI>().text = "3rd";
                }
            }
            if (redFirst && blueSecond)
            {
                if (greenOver && !yellowOver)
                {
                    //green third
                    Debug.Log("green Third");
                    thirdPositionComp = true;
                    greenPositionTxt.SetActive(true);
                    greenPositionTxt.GetComponent<TextMeshProUGUI>().text = "3rd";
                }
                if (yellowOver && !greenOver)
                {
                    //yellow third
                    Debug.Log("Yellow Third");
                    thirdPositionComp = true;
                    yellowPositionTxt.SetActive(true);
                    yellowPositionTxt.GetComponent<TextMeshProUGUI>().text = "3rd";
                }
            }

            if(greenFirst && redSecond)
            {
                if (yellowOver && !blueOver)
                {
                    //yellow third
                    Debug.Log("Yellow Third");
                    thirdPositionComp = true;
                    yellowPositionTxt.SetActive(true);
                    yellowPositionTxt.GetComponent<TextMeshProUGUI>().text = "3rd";
                }
                if (blueOver && !yellowOver)
                {
                    //blue third
                    Debug.Log("blue Third");
                    thirdPositionComp = true;
                    bluePositionTxt.SetActive(true);
                    bluePositionTxt.GetComponent<TextMeshProUGUI>().text = "3rd";
                }
            }
            if (greenFirst && yellowSecond)
            {
                if (redOver && !blueOver)
                {
                    //red third
                    Debug.Log("red Third");
                    thirdPositionComp = true;
                    redPositionTxt.SetActive(true);
                    redPositionTxt.GetComponent<TextMeshProUGUI>().text = "3rd";
                }
                if (blueOver && !redOver)
                {
                    //blue third
                    Debug.Log("blue Third");
                    thirdPositionComp = true;
                    bluePositionTxt.SetActive(true);
                    bluePositionTxt.GetComponent<TextMeshProUGUI>().text = "3rd";
                }
            }
            if (greenFirst && blueSecond)
            {
                if (redOver && !yellowOver)
                {
                    //red third
                    Debug.Log("red Third");
                    thirdPositionComp = true;
                    redPositionTxt.SetActive(true);
                    redPositionTxt.GetComponent<TextMeshProUGUI>().text = "3rd";
                }
                if (yellowOver && !redOver)
                {
                    //yellow third
                    Debug.Log("Yellow Third");
                    thirdPositionComp = true;
                    yellowPositionTxt.SetActive(true);
                    yellowPositionTxt.GetComponent<TextMeshProUGUI>().text = "3rd";
                }
            }

            if(yellowFirst && redSecond)
            {
                if (greenOver && !blueOver)
                {
                    //green third
                    Debug.Log("green Third");
                    thirdPositionComp = true;
                    greenPositionTxt.SetActive(true);
                    greenPositionTxt.GetComponent<TextMeshProUGUI>().text = "3rd";
                }
                if (blueOver && !greenOver)
                {
                    //blue third
                    Debug.Log("blue Third");
                    thirdPositionComp = true;
                    bluePositionTxt.SetActive(true);
                    bluePositionTxt.GetComponent<TextMeshProUGUI>().text = "3rd";
                }
            }
            if (yellowFirst && greenSecond)
            {
                if (redOver && !blueOver)
                {
                    //red third
                    Debug.Log("red Third");
                    thirdPositionComp = true;
                    redPositionTxt.SetActive(true);
                    redPositionTxt.GetComponent<TextMeshProUGUI>().text = "3rd";
                }
                if (blueOver && !redOver)
                {
                    //blue third
                    Debug.Log("blue Third");
                    thirdPositionComp = true;
                    bluePositionTxt.SetActive(true);
                    bluePositionTxt.GetComponent<TextMeshProUGUI>().text = "3rd";
                }
            }
            if (yellowFirst && blueSecond)
            {
                if (redOver && !greenOver)
                {
                    //red third
                    Debug.Log("red Third");
                    thirdPositionComp = true;
                    redPositionTxt.SetActive(true);
                    redPositionTxt.GetComponent<TextMeshProUGUI>().text = "3rd";
                }
                if (greenOver && !redOver)
                {
                    //green third
                    Debug.Log("green Third");
                    thirdPositionComp = true;
                    greenPositionTxt.SetActive(true);
                    greenPositionTxt.GetComponent<TextMeshProUGUI>().text = "3rd";
                }
            }

            if(blueFirst && redSecond)
            {
                if (greenOver && !yellowOver)
                {
                    //green third
                    Debug.Log("green Third");
                    thirdPositionComp = true;
                    greenPositionTxt.SetActive(true);
                    greenPositionTxt.GetComponent<TextMeshProUGUI>().text = "3rd";
                }
                if (yellowOver && !greenOver)
                {
                    //yellow third
                    Debug.Log("Yellow Third");
                    thirdPositionComp = true;
                    yellowPositionTxt.SetActive(true);
                    yellowPositionTxt.GetComponent<TextMeshProUGUI>().text = "3rd";
                }
            }
            if (blueFirst && greenSecond)
            {
                if (redOver && !yellowOver)
                {
                    //red third
                    Debug.Log("red Third");
                    redPositionTxt.SetActive(true);
                    redPositionTxt.GetComponent<TextMeshProUGUI>().text = "3rd";
                    thirdPositionComp = true;
                }
                if (yellowOver && !redOver)
                {
                    //yellow third
                    Debug.Log("Yellow Third");
                    thirdPositionComp = true;
                    yellowPositionTxt.SetActive(true);
                    yellowPositionTxt.GetComponent<TextMeshProUGUI>().text = "3rd";
                }
            }
            if (blueFirst && yellowSecond)
            {
                if (redOver && !greenOver)
                {
                    //red third
                    Debug.Log("red Third");
                    thirdPositionComp = true;
                    redPositionTxt.SetActive(true);
                    redPositionTxt.GetComponent<TextMeshProUGUI>().text = "3rd";
                }
                if (greenOver && !redOver)
                {
                    //green third
                    Debug.Log("green Third");
                    thirdPositionComp = true;
                    greenPositionTxt.SetActive(true);
                    greenPositionTxt.GetComponent<TextMeshProUGUI>().text = "3rd";
                }
            }
        }
    }

    public void GamePlayOver()
    {
        if (firstPositionComp && secondPositionComp && thirdPositionComp)
        {
            Time.timeScale = 0f;
        }
    }
}
