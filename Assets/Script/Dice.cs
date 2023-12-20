using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using SocketIOClient;
using System.Text.Json;
using System;
using Newtonsoft.Json;
using System.Threading.Tasks;

class enemyPawns 
{
    public List<GameObject> _enemyPawnList;
    public string color;
    public string enemyName;
}


public class Dice : MonoBehaviour
{
    Rigidbody rb;
    bool hasLanded;
    public static bool thrown;
    bool CanPlay;
    Vector3 initPosition;
    Quaternion initRotation;
    public static int diceValue;
    int TestdiceValue;
    public DiceSide[] diceSides;
    public PawnMovement red;
    public PawnMovement green;
    public PawnMovement yellow;
    public PawnMovement blue;
    public GameObject RedStart;
    public GameObject YellowStart;
    public GameObject GreenStart;
    public GameObject BlueStart;
    public static bool reshot;
    public static int turn;
    bool HasReset;
    bool IsMoved;
    public TextMeshProUGUI Turn;
    //bool doNotRollAgain;
    int previousDV;
    public GameObject DefaultPawnref;
    public GameObject[] RedPawnArray;
    public GameObject[] GreenPawnArray;
    public GameObject[] YellowPawnArray;
    public GameObject[] BluePawnArray;
    public bool isPlayPossible;
    public bool startBlink;
    public static int TotalRedInPlay = 0;
    public static int TotalGreenInPlay = 0;
    public static int TotalYellowInPlay = 0;
    public static int TotalBlueInPlay = 0;
    public bool singleRed;
    public bool singleGreen;
    public bool singleYellow;
    public bool singleBlue;
    GameObject temp;
    public int checkDist;
    public List<string> Exitcolor;

    [SerializeField] GameObject redNameTxt;
    [SerializeField] GameObject greenNameTxt;
    [SerializeField] GameObject yellowNameTxt;
    [SerializeField] GameObject blueNameTxt;

    Dictionary<string, enemyPawns> enemyGameObject = new Dictionary<string, enemyPawns>();


    delegate void thread();
    Queue<thread> MainThreadDispatcher = new Queue<thread>();
    SocketIO client;
    private string myGuid;
    private string myName;

    private void Start()
    {
        isPlayPossible = true;
        PawnSelection.Pawn = DefaultPawnref;
        previousDV = 0;
        HasReset = true;
        rb = GetComponent<Rigidbody>();
        initPosition = transform.position;
        initRotation = transform.rotation;
        rb.useGravity = false;
        turn = 1;
        if (LudoUiHandler.Player_2)
        {
            //skip turn 2 and 4
            for (int i = 0; i < BluePawnArray.Length; i++)
            {
                BluePawnArray[i].SetActive(false);
                GreenPawnArray[i].SetActive(false);
            }
        }
        if (LudoUiHandler.Player_3)
        {
            //skip turn 4
            for (int i = 0; i < BluePawnArray.Length; i++)
            {
                BluePawnArray[i].SetActive(false);
            }
        }

        myGuid = LudoUiHandler.GetMyGuid();
        myName = LudoUiHandler.GetMyName();
        client = LudoUiHandler.GetSocketIOClient();

        RegisterSocketIOEvent();

        //await Task.Delay(1000);
        ACtivatedPawn();

        Hashtable myTransform = new Hashtable();

        myTransform.Add("guid", myGuid);
        myTransform.Add("playername", myName);
        myTransform.Add("color", LudoUiHandler.myTag);

        //SENDING MY INFO TO OTHER PLAYER AS I JOIN
        client.EmitAsync("newPlayer", JsonConvert.SerializeObject(myTransform));


    }

    private void ACtivatedPawn()
    {
        if(LudoUiHandler.myTag == "RED")
        {
            for(int i =0;i<4;i++)
            {
                RedPawnArray[i].SetActive(true);
                redNameTxt.SetActive(true);
                redNameTxt.GetComponent<TextMeshProUGUI>().text = myName;
            }
        }
        else if (LudoUiHandler.myTag == "YELLOW")
        {
            for (int i = 0; i < 4; i++)
            {
                YellowPawnArray[i].SetActive(true);
                yellowNameTxt.SetActive(true);
                yellowNameTxt.GetComponent<TextMeshProUGUI>().text = myName;
            }
        }
        else if (LudoUiHandler.myTag == "GREEN")
        {
            for (int i = 0; i < 4; i++)
            {
                GreenPawnArray[i].SetActive(true);
                greenNameTxt.SetActive(true);
                greenNameTxt.GetComponent<TextMeshProUGUI>().text = myName;
            }
        }
        else if (LudoUiHandler.myTag == "BLUE")
        {
            for (int i = 0; i < 4; i++)
            {
                BluePawnArray[i].SetActive(true);
                blueNameTxt.SetActive(true);
                blueNameTxt.GetComponent<TextMeshProUGUI>().text = myName;
            }
        }
    }

    private void RegisterSocketIOEvent()
    {
        //RECEIVE EVENT AS SOON AS A NEW PLAYER SENT NEWPLAYER EVENT
        client.On("newPlayer", (SocketIOResponse enemyTransform) =>
        {
            MainThreadDispatcher.Enqueue(() =>
            {
                Hashtable o = enemyTransform.GetValue(0).Deserialize<Hashtable>();

                Dictionary<string, string> enemyData = new Dictionary<string, string>();
                foreach (DictionaryEntry kv in o)
                {
                    try
                    {
                        enemyData.Add(kv.Key.ToString(), kv.Value.ToString());
                        //Debug.Log("newPlayer: " + kv.Key.ToString() + " " + kv.Value.ToString());
                    }
                    catch (Exception e)
                    {
                        Debug.Log(e.Message);
                    }
                }

                if (enemyData["guid"] == myGuid || enemyGameObject.ContainsKey(enemyData["guid"])) return;

                
                List<GameObject> _enemyPawnList = new List<GameObject>();

                
                if(enemyData["color"] == "RED")
                {
                    for (int i = 0; i < 4; i++)
                    {
                        RedPawnArray[i].SetActive(true);
                        _enemyPawnList.Add(RedPawnArray[i]);
                        redNameTxt.SetActive(true);
                        redNameTxt.GetComponent<TextMeshProUGUI>().text = enemyData["playername"];
                    }
                }

                if (enemyData["color"] == "YELLOW")
                {
                    for (int i = 0; i < 4; i++)
                    {
                        YellowPawnArray[i].SetActive(true);
                        _enemyPawnList.Add(YellowPawnArray[i]);
                        yellowNameTxt.SetActive(true);
                        yellowNameTxt.GetComponent<TextMeshProUGUI>().text = enemyData["playername"];
                    }
                }

                if (enemyData["color"] == "GREEN")
                {
                    for (int i = 0; i < 4; i++)
                    {
                        GreenPawnArray[i].SetActive(true);
                        _enemyPawnList.Add(GreenPawnArray[i]);
                        greenNameTxt.SetActive(true);
                        greenNameTxt.GetComponent<TextMeshProUGUI>().text = enemyData["playername"];
                    }
                }

                if (enemyData["color"] == "BLUE")
                {
                    for (int i = 0; i < 4; i++)
                    {
                        BluePawnArray[i].SetActive(true);
                        _enemyPawnList.Add(BluePawnArray[i]);
                        blueNameTxt.SetActive(true);
                        blueNameTxt.GetComponent<TextMeshProUGUI>().text = enemyData["playername"];
                    }
                }
                enemyPawns _enemyPawn = new enemyPawns();

                _enemyPawn._enemyPawnList = _enemyPawnList;
                _enemyPawn.color = enemyData["color"];
                _enemyPawn.enemyName = enemyData["playername"];

                enemyGameObject.Add(enemyData["guid"], _enemyPawn);


                Hashtable myTransform = new Hashtable();
                myTransform.Add("guid", myGuid);
                myTransform.Add("color", LudoUiHandler.myTag);
                myTransform.Add("playername", myName);

                //SENDING MY INFO TO OTHER PLAYER FOR WHICH I RECEIVE THIS CALL EVENT
                client.EmitAsync("newPlayer", JsonConvert.SerializeObject(myTransform));
                

            });
        });

        //RECEIVE EVENT AS SOON AS ENEMY PAWN POSITION CHANGE
        client.On("newPosition", (enemyNewPosition) =>
        {
            MainThreadDispatcher.Enqueue(() =>
            {
                Hashtable o = enemyNewPosition.GetValue(0).Deserialize<Hashtable>();

                Dictionary<string, string> enemyData = new Dictionary<string, string>();

                foreach (DictionaryEntry kv in o)
                {
                    try
                    {
                        enemyData.Add(kv.Key.ToString(), kv.Value.ToString());
                    }
                    catch (Exception e)
                    {
                        Debug.Log(e.Message);
                    }
                }


                if (enemyData["guid"] == myGuid) return;

                enemyPawns _enemy = enemyGameObject[enemyData["guid"]];

                GameObject _enemyGameob = _enemy._enemyPawnList[int.Parse(enemyData["pawnNo"])];

                _enemyGameob.transform.position = new Vector3(
                    float.Parse(enemyData["x"]),
                    float.Parse(enemyData["y"]),
                    float.Parse(enemyData["z"])
                );

                _enemyGameob.GetComponent<PawnMovement>().moveComplete = bool.Parse(enemyData["isMoveComplete"]);
                _enemyGameob.GetComponent<PawnMovement>().moveBackComplete = bool.Parse(enemyData["isMoveBack"]);
            });

        });

        //RECEIVE EVENT AS SOON AS ENEMY DICE ROTATION CHANGE
        client.On("diceMove", (data) =>
        {
            MainThreadDispatcher.Enqueue(() =>
            {
                Hashtable o = data.GetValue(0).Deserialize<Hashtable>();

                Dictionary<string, string> enemyData = new Dictionary<string, string>();

                foreach (DictionaryEntry kv in o)
                {
                    try
                    {
                        enemyData.Add(kv.Key.ToString(), kv.Value.ToString());
                    }
                    catch (Exception e)
                    {
                        Debug.Log(e.Message);
                    }
                }


                if (enemyData["guid"] == myGuid) return;

                transform.rotation = Quaternion.Euler(new Vector3(
                    float.Parse(enemyData["w"]),
                    float.Parse(enemyData["q"]),
                    float.Parse(enemyData["p"])
                ));

                transform.position = new Vector3(
                    float.Parse(enemyData["x"]),
                    float.Parse(enemyData["y"]),
                    float.Parse(enemyData["z"])
                );

            });
        });

        //RECEIVE EVENT AS SOON AS ENEMY DICE RESET
        client.On("diceReset", (data) =>
        {
            MainThreadDispatcher.Enqueue(() =>
            {
                Hashtable o = data.GetValue(0).Deserialize<Hashtable>();

                Dictionary<string, string> enemyData = new Dictionary<string, string>();

                foreach (DictionaryEntry kv in o)
                {
                    try
                    {
                        enemyData.Add(kv.Key.ToString(), kv.Value.ToString());
                    }
                    catch (Exception e)
                    {
                        Debug.Log(e.Message);
                    }
                }

                if (enemyData["guid"] == myGuid) return;

                //Debug.Log("reset called from net");

                bool isReshot = bool.Parse(enemyData["isReshot"]);

                reshot = isReshot;

                reset();
                

            });
        });

        client.On("HandleDisconnect", (data) =>
        {
            MainThreadDispatcher.Enqueue(() =>
            {
                Hashtable h = data.GetValue(0).Deserialize<Hashtable>();

                Dictionary<string, string> playerData = new Dictionary<string, string>();

                foreach (DictionaryEntry kv in h)
                {
                    playerData.Add(kv.Key.ToString(), kv.Value.ToString());
                }

                string playerGuid = playerData["playerId"];

                if (playerGuid == myGuid) return;

                Exitcolor.Add(enemyGameObject[playerGuid].color);

                
                for (int i=0; i<4; i++)
                {
                    enemyGameObject[playerGuid]._enemyPawnList[i].SetActive(false);
                }

                enemyGameObject.Remove(playerGuid);
            });

        });
    }

    private void Update()
    {
        if (Exitcolor.Contains("RED") && turn == 1)
        {
            turn = 2;
        }
        else if (Exitcolor.Contains("GREEN") && turn == 2)
        {
            turn = 3;
        }
        else if (Exitcolor.Contains("YELLOW") && turn == 3)
        {
            turn = 4;
        }
        else if (Exitcolor.Contains("BLUE") && turn == 4)
        {
            turn = 1;
        }
        while (MainThreadDispatcher.Count > 0)
        {
            thread _thread = MainThreadDispatcher.Dequeue();
            _thread();
        }

        //MoveComplete();
        //Debug.Log(DefaultPawnref.GetComponent<PawnMovement>().moveComplete);
        turnTextChanger();
        
        if (rb.IsSleeping() && hasLanded && thrown ) 
        {
            checkWhetherOnlyOnePawnCanPlay();
            //rb.useGravity = false; 
            DiceMovements();
            //Debug.Log(diceValue + " = Dice value");
           
            

            FirstSix();
            six();
            testFunction();
            BlinkIt();
        }

        if(PawnSelection.Pawn != null)
        {
            if (PawnSelection.Pawn.GetComponent<PawnMovement>() != null)
            {
                if (turn == 1)
                {
                    callReset("RedPawn");
                }
                if (turn == 2)
                {
                    callReset("GreenPawn");
                }
                if (turn == 3)
                {
                    callReset("YellowPawn");
                }
                if (turn == 4)
                {
                    callReset("BluePawn");
                }
            }
        }

        else if (rb.IsSleeping() && hasLanded && diceValue != 1 && diceValue != 2 && diceValue != 3 && diceValue != 4 && diceValue != 5 && diceValue != 6)
        {
            rollAgain();
        }

        if (temp != null)
        {
            if (temp.GetComponent<PawnMovement>() != null)
            {
                if (temp.tag == "RedPawn" || temp.tag == "GreenPawn" || temp.tag == "YellowPawn" || temp.tag == "BluePawn")
                {

                    if (temp.GetComponent<PawnMovement>().moveComplete && rb.IsSleeping() && hasLanded && thrown && temp.GetComponent<PawnMovement>().MovePossible)
                    {
                        Debug.Log("reset called from temp temp.GetComponent<PawnMovement>().MovePossible=" + temp.GetComponent<PawnMovement>().MovePossible);
                        reset();
                        EmitResetToServer(reshot);
                        temp = null;
                    }
                }
            }
        }

        if (thrown)
        {
            SyncDiceTransformWithEnemy();
        }

    }

    private void SyncDiceTransformWithEnemy()
    {
        Hashtable diceTransform = new Hashtable();
        diceTransform.Add("w", transform.eulerAngles.x);
        diceTransform.Add("q", transform.eulerAngles.y);
        diceTransform.Add("p", transform.eulerAngles.z);
        diceTransform.Add("x", transform.position.x);
        diceTransform.Add("y", transform.position.y);
        diceTransform.Add("z", transform.position.z);
        diceTransform.Add("guid", myGuid);

        //SENDING MY INFO TO OTHER PLAYER AFTER I ROLL DICE
        client.EmitAsync("diceMove", JsonConvert.SerializeObject(diceTransform));
    }

    public void SyncPawnTransformWithEnemy(GameObject pawnObj)
    {
        int pawnNo = int.Parse(pawnObj.name.Split("_")[1])-1;

        Hashtable pawnTransform = new Hashtable();
        pawnTransform.Add("x", pawnObj.transform.position.x);
        pawnTransform.Add("y", pawnObj.transform.position.y);
        pawnTransform.Add("z", pawnObj.transform.position.z);
        pawnTransform.Add("pawnNo", pawnNo);
        pawnTransform.Add("guid", myGuid);
        pawnTransform.Add("isMoveComplete", pawnObj.GetComponent<PawnMovement>().moveComplete);
        pawnTransform.Add("isMoveBack", pawnObj.GetComponent<PawnMovement>().moveBackComplete);
        //SENDING MY INFO TO OTHER PLAYER AFTER I CHANGE PAWN POSITION
        client.EmitAsync("newPosition", JsonConvert.SerializeObject(pawnTransform));
    }

    public void callReset(string pawnTag)
    {
        if (PawnSelection.Pawn.tag == pawnTag && PawnSelection.Pawn.GetComponent<PawnMovement>().CanPlay)
        {
            //if ((PawnSelection.Pawn.GetComponent<PawnMovement>().Dist + diceValue < PawnSelection.Pawn.GetComponent<PawnMovement>().MovePoints.Length))
            {
                if (PawnSelection.Pawn.GetComponent<PawnMovement>().moveComplete && rb.IsSleeping() && hasLanded && thrown)
                {
                    Debug.Log("reset called from CallReset");
                    reset();
                    EmitResetToServer(reshot);
                    startBlink = false;
                }
            }
        }
    }

    public void RollDice()
    {
        if(LudoUiHandler.myTag == "RED" && turn ==1 || LudoUiHandler.myTag == "GREEN"&& turn == 2 || LudoUiHandler.myTag == "YELLOW" && turn == 3 || LudoUiHandler.myTag == "BLUE" && turn == 4)
        {
            if (!thrown && !hasLanded)
            {
                thrown = true;
                hasLanded = true;
                rb.useGravity = true;
                //doNotRollAgain = false;
                rb.AddTorque(UnityEngine.Random.Range(0, 500), UnityEngine.Random.Range(0, 500), UnityEngine.Random.Range(0, 500));
            }          
        }            
    }

    public void turnTextChanger()
    {
        if (turn == 1)
        {
            Turn.text = redNameTxt.GetComponent<TextMeshProUGUI>().text + " Turn";
            Turn.color = Color.red;
        }
            
        if (turn == 2) 
        {
            Turn.text = greenNameTxt.GetComponent<TextMeshProUGUI>().text + " Turn";
            Turn.color = Color.green;
        }
            
        if (turn == 3)
        {
            Turn.text = yellowNameTxt.GetComponent<TextMeshProUGUI>().text + " Turn";
            Turn.color = Color.yellow;
        }
            
        if (turn == 4)
        {
            Turn.text = blueNameTxt.GetComponent<TextMeshProUGUI>().text + " Turn";
            Turn.color = Color.blue;
        }
            
    }

    private void EmitResetToServer(bool isReshot)
    {
        Hashtable diceResetInfo = new Hashtable();
        diceResetInfo.Add("guid", myGuid);
        diceResetInfo.Add("isReshot", isReshot.ToString());

        //SENDING MY INFO TO OTHER PLAYER AFTER MY DICE RESET
        client.EmitAsync("diceReset", JsonConvert.SerializeObject(diceResetInfo));
    }

    public void reset()
    {
        

        TotalBlueInPlay = 0;
        TotalGreenInPlay = 0;
        TotalRedInPlay = 0;
        TotalYellowInPlay = 0;
        //Players turn
        if(!reshot && LudoUiHandler.Player_4 && !LudoUiHandler.Player_3 && !LudoUiHandler.Player_2)
        {
            turn += 1;
            if (turn == 5)
                turn = 1;
        }

        else if(!reshot && !LudoUiHandler.Player_4 && LudoUiHandler.Player_3 && !LudoUiHandler.Player_2)
        {
            turn += 1;
            if (turn == 4)
                turn = 1;
        }

        else if (!reshot && !LudoUiHandler.Player_4 && !LudoUiHandler.Player_3 && LudoUiHandler.Player_2)
        {
            turn += 1;
            if (turn == 2)
                turn = 3;
            if (turn == 4)
                turn = 1;
        }

        if (FinalHome.redOver && turn == 1)
        {
            turn = 2;
        }
        if (FinalHome.greenOver && turn == 2)
        {
            turn = 3;
        }
        if (FinalHome.yellowOver && turn == 3)
        {
            turn = 4;
        }
        if (FinalHome.blueOver && turn == 4)
        {
            turn = 1;
        }
        
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        transform.SetPositionAndRotation(initPosition, initRotation);
        thrown = false;
        hasLanded = false;
        rb.useGravity = false;
        startBlink = false;
        HasReset = true;
    }

    public void rollAgain()
    {
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        transform.SetPositionAndRotation(initPosition, initRotation);
        thrown = false;
        hasLanded = false;
        rb.useGravity = false;
        startBlink = false;
        HasReset = true;
        EmitResetToServer(reshot);
        thrown = true;
        rb.useGravity = true;
        rb.AddTorque(UnityEngine.Random.Range(0, 500), UnityEngine.Random.Range(0, 500), UnityEngine.Random.Range(0, 500));
    }

  
    private void DiceMovements()
    {
        diceValue = previousDV;

        foreach (DiceSide side in diceSides)
        {          
            if (side.OnGround() && HasReset)
            {                
                diceValue = side.sideValue;
                
                previousDV = diceValue;

                //Uncomment below for red pawn Movement
                if (turn == 1)
                {
                    if (singleRed)
                    {
                        SingleOnMove(RedPawnArray);
                    }
                    else if  (PawnSelection.Pawn.GetComponent<PawnMovement>() != null)
                    {
                        //checkDist = PawnSelection.Pawn.GetComponent<PawnMovement>().Dist;
                        //checkDist += diceValue;
                        //if (checkDist <= 43)
                        {
                            if (PawnSelection.Pawn.GetComponent<PawnMovement>().CanPlay)
                            {
                                if (PawnSelection.Pawn.gameObject.tag == "RedPawn")// && (PawnSelection.Pawn.GetComponent<PawnMovement>().Dist + diceValue) < PawnSelection.Pawn.GetComponent<PawnMovement>().MovePoints.Length
                                {
                                    PawnSelection.Pawn.GetComponent<PawnMovement>().moveComplete = false;
                                    //diceValue = previousDV;
                                    Debug.Log(diceValue);
                                    startBlink = false;
                                    OnMove();
                                }
                            }
                        }
                    }
                }
                else if (turn == 2)
                {
                    if (singleGreen)
                    {
                        SingleOnMove(GreenPawnArray);
                    }
                    else if (PawnSelection.Pawn.GetComponent<PawnMovement>() != null)
                    {
                        //checkDist = PawnSelection.Pawn.GetComponent<PawnMovement>().Dist;
                        //checkDist += diceValue;
                        //if (checkDist <= 43)
                        {
                            if (PawnSelection.Pawn.GetComponent<PawnMovement>().CanPlay)
                            {
                                if (PawnSelection.Pawn.gameObject.tag == "GreenPawn")
                                {
                                    PawnSelection.Pawn.GetComponent<PawnMovement>().moveComplete = false;
                                    startBlink = false;
                                    OnMove();
                                }
                            }
                        } 
                    }
                        
                }

                //Uncomment below for yellow pawn Movement
                else if (turn == 3)
                {
                    if (singleYellow)
                    {
                        SingleOnMove(YellowPawnArray);
                    }
                    else if (PawnSelection.Pawn.GetComponent<PawnMovement>() != null)
                    {
                        //checkDist = PawnSelection.Pawn.GetComponent<PawnMovement>().Dist;
                        //checkDist += diceValue;
                        //if (checkDist <= 43)
                        {
                            if (PawnSelection.Pawn.GetComponent<PawnMovement>().CanPlay)
                            {
                                if (PawnSelection.Pawn.gameObject.tag == "YellowPawn")
                                {
                                    PawnSelection.Pawn.GetComponent<PawnMovement>().moveComplete = false;
                                    startBlink = false;
                                    OnMove();
                                }
                            }
                        }
                    }
                }

                //Uncomment below for blue pawn Movement
                else if (turn == 4)
                {
                    if (singleBlue)
                    {
                        SingleOnMove(BluePawnArray);
                    }
                    else if (PawnSelection.Pawn.GetComponent<PawnMovement>() != null)
                    {
                        //checkDist = PawnSelection.Pawn.GetComponent<PawnMovement>().Dist;
                        //checkDist += diceValue;
                        //if (checkDist <= 43)
                        {
                            if (PawnSelection.Pawn.GetComponent<PawnMovement>().CanPlay)
                            {
                                if (PawnSelection.Pawn.gameObject.tag == "BluePawn")
                                {
                                    PawnSelection.Pawn.GetComponent<PawnMovement>().moveComplete = false;
                                    startBlink = false;
                                    OnMove();

                                }
                            }
                        } 
                    }
                }
            }
        }
    }

    public void FirstSix()
    {
        
        if (diceValue == 6)
        {
            
            if (PawnSelection.Pawn.GetComponent<PawnMovement>() != null)
            {
                if (turn == 1 && !PawnSelection.Pawn.GetComponent<PawnMovement>().firstSix && HasReset)
                {
                    DefaultPawnref.GetComponent<PawnMovement>().moveComplete = false;
                    if (PawnSelection.Pawn.gameObject.tag == "RedPawn")
                    {
                        PawnSelection.Pawn.GetComponent<PawnMovement>().transform.position = RedStart.transform.position;
                        FirstSixBool();
                        //Debug.Log("RED"+ " " +RedStart.transform.position);
                        SyncPawnTransformWithEnemy(PawnSelection.Pawn);
                        DefaultPawnref.GetComponent<PawnMovement>().moveComplete = true;
                    }

                }
                if (turn == 2 && !PawnSelection.Pawn.GetComponent<PawnMovement>().firstSix && HasReset)
                {
                    if (PawnSelection.Pawn.gameObject.tag == "GreenPawn")
                    {
                        PawnSelection.Pawn.GetComponent<PawnMovement>().transform.position = GreenStart.transform.position;
                        //Debug.Log("GREEN" + " " + GreenStart.transform.position);
                        SyncPawnTransformWithEnemy(PawnSelection.Pawn);
                        FirstSixBool();
                    }

                }
                if (turn == 3 && !PawnSelection.Pawn.GetComponent<PawnMovement>().firstSix && HasReset)
                {
                    if (PawnSelection.Pawn.gameObject.tag == "YellowPawn")
                    {
                        PawnSelection.Pawn.GetComponent<PawnMovement>().transform.position = YellowStart.transform.position;
                        //Debug.Log("YELLOW" + " " + YellowStart.transform.position);
                        SyncPawnTransformWithEnemy(PawnSelection.Pawn);
                        FirstSixBool();
                    }
                }
                if (turn == 4 && !PawnSelection.Pawn.GetComponent<PawnMovement>().firstSix && HasReset)
                {
                    if (PawnSelection.Pawn.gameObject.tag == "BluePawn")
                    {
                        PawnSelection.Pawn.GetComponent<PawnMovement>().transform.position = BlueStart.transform.position;
                        //Debug.Log("BLUE" + " " + BlueStart.transform.position);
                        SyncPawnTransformWithEnemy(PawnSelection.Pawn);
                        FirstSixBool();
                    }
                }
            }
                
        }
    }

    
    public void six()
    {
        if (diceValue == 6)
        {
            startBlink = true;
            if(turn ==1)
            {
                for(int i =0;i<RedPawnArray.Length;i++)
                {
                    if (!RedPawnArray[i].GetComponent<PawnMovement>().CanPlay)
                        PawnBlink(RedPawnArray[i]);
                }
            }
            else if (turn == 2)
            {
                for (int i = 0; i < GreenPawnArray.Length; i++)
                {
                    if (!GreenPawnArray[i].GetComponent<PawnMovement>().CanPlay)
                        PawnBlink(GreenPawnArray[i]);
                }
            }
            else if (turn == 3)
            {
                for (int i = 0; i < YellowPawnArray.Length; i++)
                {
                    if (!YellowPawnArray[i].GetComponent<PawnMovement>().CanPlay)
                        PawnBlink(YellowPawnArray[i]);
                }
            }
            else if (turn == 4)
            {
                for (int i = 0; i < BluePawnArray.Length; i++)
                {
                    if (!BluePawnArray[i].GetComponent<PawnMovement>().CanPlay)
                        PawnBlink(BluePawnArray[i]);
                }
            }
            reshot = true;           
        }
    }

    public void OnMove()
    {
        
        DefaultPawnref = PawnSelection.Pawn;
        PawnSelection.Pawn.GetComponent<PawnMovement>().Move(diceValue);        
        if (diceValue != 6)
            reshot = false;
        if(PawnSelection.Pawn.GetComponent<PawnMovement>().MoveCalled)
        {
            HasReset = false;
            Debug.Log(HasReset);
            PawnSelection.Pawn.GetComponent<PawnMovement>().MoveCalled = false;
        }  
    }

    

    public void FirstSixBool()
    {
        DefaultPawnref = PawnSelection.Pawn;       
        reshot = true;
        PawnSelection.Pawn.GetComponent<PawnMovement>().CanPlay = true;
        PawnSelection.Pawn.GetComponent<PawnMovement>().firstSix = true;
        HasReset = false;
        diceValue = 0;
    }
    public void testFunction()
    {       
        if (turn == 1)
        {
            
            for (int i = 0; i < RedPawnArray.Length; i++)
            {    
                if(RedPawnArray[i].activeSelf)
                {
                    if (!RedPawnArray[i].GetComponent<PawnMovement>().CanPlay)
                    {
                        isPlayPossible = false;
                    }
                    if (RedPawnArray[i].GetComponent<PawnMovement>().CanPlay && !RedPawnArray[i].GetComponent<PawnMovement>().MovePossible)
                    {
                        isPlayPossible = false;
                    }
                    if (RedPawnArray[i].GetComponent<PawnMovement>().CanPlay && RedPawnArray[i].GetComponent<PawnMovement>().MovePossible)
                    {
                        //if(!RedPawnArray[i].GetComponent<PawnMovement>().MovePossible)
                        //    isPlayPossible = false;
                        //else if(RedPawnArray[i].GetComponent<PawnMovement>().MovePossible)
                        //    isPlayPossible = true;
                        isPlayPossible = true;
                        break;
                    }
                }
            }
            if (!isPlayPossible && diceValue != 6)
            {
                Debug.Log("reset called from here dice value =" + diceValue);
                reset();
                EmitResetToServer(reshot);
                reshot = false;
            }
            else if (isPlayPossible)
            {

            }
        }

        else if (turn == 2)
        {
            
            for (int i = 0; i < GreenPawnArray.Length; i++)
            {
                if (GreenPawnArray[i].activeSelf)
                {
                    if (!GreenPawnArray[i].GetComponent<PawnMovement>().CanPlay)
                    {
                        isPlayPossible = false;
                    }
                    if (GreenPawnArray[i].GetComponent<PawnMovement>().CanPlay && !GreenPawnArray[i].GetComponent<PawnMovement>().MovePossible)
                    {
                        isPlayPossible = false;
                    }
                    if (GreenPawnArray[i].GetComponent<PawnMovement>().CanPlay && GreenPawnArray[i].GetComponent<PawnMovement>().MovePossible)
                    {
                        //if (!GreenPawnArray[i].GetComponent<PawnMovement>().MovePossible)
                        //    isPlayPossible = false;
                        //else if (GreenPawnArray[i].GetComponent<PawnMovement>().MovePossible)
                        //    isPlayPossible = true;
                        isPlayPossible = true;
                        break;
                    }
                }                   
            }
            if (!isPlayPossible && diceValue != 6)
            {             
                reset();
                EmitResetToServer(reshot);
                //Debug.Log("Reset Functioncalled from here");
                reshot = false;
            }
        }

        else if (turn == 3)
        {
           
            for (int i = 0; i < YellowPawnArray.Length; i++)
            {
                if (YellowPawnArray[i].activeSelf)
                {
                    if (!YellowPawnArray[i].GetComponent<PawnMovement>().CanPlay)
                    {
                        isPlayPossible = false;
                    }
                    if (YellowPawnArray[i].GetComponent<PawnMovement>().CanPlay && !YellowPawnArray[i].GetComponent<PawnMovement>().MovePossible)
                    {
                        isPlayPossible = false;
                    }
                    if (YellowPawnArray[i].GetComponent<PawnMovement>().CanPlay && YellowPawnArray[i].GetComponent<PawnMovement>().MovePossible)
                    {
                        //if (!YellowPawnArray[i].GetComponent<PawnMovement>().MovePossible)
                        //    isPlayPossible = false;
                        //else if (YellowPawnArray[i].GetComponent<PawnMovement>().MovePossible)
                        //    isPlayPossible = true;
                        isPlayPossible = true;
                        break;
                    }
                }
                    
            }
            if (!isPlayPossible && diceValue != 6)
            {               
                reset();
                EmitResetToServer(reshot);
                reshot = false;
            }
        }

        else if (turn == 4)
        {
           
            for (int i = 0; i < BluePawnArray.Length; i++)
            {
                if (BluePawnArray[i].activeSelf)
                {
                    if (!BluePawnArray[i].GetComponent<PawnMovement>().CanPlay)
                    {
                        isPlayPossible = false;
                    }
                    if (BluePawnArray[i].GetComponent<PawnMovement>().CanPlay && !BluePawnArray[i].GetComponent<PawnMovement>().MovePossible)
                    {
                        isPlayPossible = false;
                    }
                    if (BluePawnArray[i].GetComponent<PawnMovement>().CanPlay && BluePawnArray[i].GetComponent<PawnMovement>().MovePossible)
                    {
                        //if (!BluePawnArray[i].GetComponent<PawnMovement>().MovePossible)
                        //    isPlayPossible = false;
                        //else if (BluePawnArray[i].GetComponent<PawnMovement>().MovePossible)
                        //    isPlayPossible = true;
                        isPlayPossible = true;
                        break;
                    }
                }
                    
            }
            if (!isPlayPossible && diceValue != 6)
            {
                reset();
                EmitResetToServer(reshot);
                reshot = false;
            }
        }

    }

    public void BlinkIt()
    {
        startBlink = true;
        if (turn == 1)
        {
            for (int i = 0; i < RedPawnArray.Length; i++)
            {
                if (RedPawnArray[i].GetComponent<PawnMovement>().CanPlay && RedPawnArray[i].GetComponent<PawnMovement>().MovePossible)
                    PawnBlink(RedPawnArray[i]);
                else
                    continue;
            }
        }
        else if (turn == 2)
        {
            for (int i = 0; i < GreenPawnArray.Length; i++)
            {
                if (GreenPawnArray[i].GetComponent<PawnMovement>().CanPlay && GreenPawnArray[i].GetComponent<PawnMovement>().MovePossible)
                    PawnBlink(GreenPawnArray[i]);
                else
                    continue;
            }
        }
        else if (turn == 3)
        {
            for (int i = 0; i < YellowPawnArray.Length; i++)
            {
                if (YellowPawnArray[i].GetComponent<PawnMovement>().CanPlay && YellowPawnArray[i].GetComponent<PawnMovement>().MovePossible)
                    PawnBlink(YellowPawnArray[i]);
                else
                    continue;
            }
        }
        else if (turn == 4)
        {
            for (int i = 0; i < BluePawnArray.Length; i++)
            {
                if (BluePawnArray[i].GetComponent<PawnMovement>().CanPlay && BluePawnArray[i].GetComponent<PawnMovement>().MovePossible)
                    PawnBlink(BluePawnArray[i]);
                else
                    continue;
            }
        }
    }

    public void PawnBlink(GameObject Pawn)
    {
        //Debug.Log(startBlink);
        if (startBlink)
            Pawn.GetComponent<Test>().ren.material.color = Color.Lerp(Pawn.GetComponent<Test>().startColor, Pawn.GetComponent<Test>().endColor, Mathf.PingPong(Time.time * Pawn.GetComponent<Test>().speed, 1));

        else if (!startBlink)
            Pawn.GetComponent<Test>().ren.material.color = Pawn.GetComponent<Test>().startColor;
    }

    public void checkWhetherOnlyOnePawnCanPlay()
    {
        if (turn == 1)
        {
            for (int i = 0; i < RedPawnArray.Length; i++)
            {
                if (RedPawnArray[i].GetComponent<PawnMovement>().CanPlay)
                {
                    TotalRedInPlay += 1;
                }
            }
            if (TotalRedInPlay == 1)
            {
                singleRed = true;
            }
            else
                singleRed = false;
        }
        else if (turn == 2)
        {
            for (int i = 0; i < GreenPawnArray.Length; i++)
            {
                if (GreenPawnArray[i].GetComponent<PawnMovement>().CanPlay)
                {
                    TotalGreenInPlay += 1;
                }
            }
            if (TotalGreenInPlay == 1)
            {
                singleGreen = true;
            }
            else
                singleGreen = false;
        }
        else if (turn == 3)
        {
            for (int i = 0; i < YellowPawnArray.Length; i++)
            {
                if (YellowPawnArray[i].GetComponent<PawnMovement>().CanPlay)
                {
                    TotalYellowInPlay += 1;
                }
            }
            if (TotalYellowInPlay == 1)
            {
                singleYellow = true;
            }
            else
                singleYellow = false;
        }
        else if (turn == 4)
        {
            for (int i = 0; i < BluePawnArray.Length; i++)
            {
                if (BluePawnArray[i].GetComponent<PawnMovement>().CanPlay)
                {
                    TotalBlueInPlay += 1;
                }
            }
            if (TotalBlueInPlay == 1)
            {
                singleBlue = true;
            }
            else
                singleBlue = false;
        }
    }

    public void SingleOnMove(GameObject[] Array)
    {
        if(diceValue != 6)
        {
            for (int i = 0; i < Array.Length; i++)
            {
                if (Array[i].GetComponent<PawnMovement>().CanPlay && Array[i].GetComponent<PawnMovement>().MovePossible)
                {
                    Array[i].GetComponent<PawnMovement>().Move(diceValue);
                    temp = Array[i];
                    break;
                }
            }
            //Debug.Log(temp);
            if (diceValue != 6)
                reshot = false;
            HasReset = false;

        }
    }

    private void OnApplicationQuit()
    {
        client.Dispose();
        client.DisconnectAsync();
    }
}
