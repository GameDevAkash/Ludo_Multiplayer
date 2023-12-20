using Newtonsoft.Json;
using SocketIOClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.Json;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class LudoUiHandler : MonoBehaviour
{
    public static bool Player_4 = false;
    public static bool Player_3 = false;
    public static bool Player_2 = true;

    static SocketIO client = new SocketIO("http://10.2.0.205:1622/game/ludo"/*"http://127.0.0.1:1622/game/ludo"*/);//https://glearning.dedicateddevelopers.us/game/ludo
    static string roomid;
    private bool canStartGame = false;
    private static string myGuid;
    public static string myTag;
    private static string myName;
    Dictionary<string, string> gameData = new Dictionary<string, string>();

    public static string myPlayerID;
    [SerializeField] GameObject CreateRoom_InputField;
    [SerializeField] GameObject CreateRoom_Button;
    [SerializeField] GameObject JoinRoom_InputField;
    [SerializeField] GameObject JoinRoom_Button;
    [SerializeField] GameObject SearchingPlayerPanel;
    [SerializeField] GameObject WaitingForFriendPanel;
    [SerializeField] GameObject StartButton;
    [SerializeField] GameObject playerName_InputField;
    int MaxPlayer = 2;

    [SerializeField] TextMeshProUGUI playIDDebug;

    private void Start()
    {
        //Player_2 = true;
        //Player_3 = false;
        //Player_4 = false;

        client.ConnectAsync();

        if(playIDDebug != null)
        {
            //Debug.Log(myName);
            playIDDebug.text=  myName;
            //playIDDebug.text = myPlayerID;


            //if (int.Parse(myPlayerID) == 0)
            //{
            //    Dice.turn = 1;
            //}
            //else if (int.Parse(myPlayerID) == 1)
            //{
            //    Dice.turn = 3;
            //}

        }
        else if(playIDDebug == null)
        {
            myGuid = Guid.NewGuid().ToString();
        }
        client.On("canStart", (Data) =>
        {
            Hashtable o = Data.GetValue(0).Deserialize<Hashtable>();


            foreach (DictionaryEntry kv in o)
            {
                gameData.Add(kv.Key.ToString(), kv.Value.ToString());
                //Debug.Log(kv.Key.ToString() + " " + kv.Value.ToString());
            }

            roomid = gameData["roomId"];
            MaxPlayer = int.Parse(gameData["maxPlayer"]);

            canStartGame = true;

            myPlayerID = gameData["playerIndex"];

            
            //Debug.Log("canStart");
        });
    }
    public static SocketIO GetSocketIOClient()
    {
        return client;
    }

    public static string GetMyGuid()
    {
        return myGuid;
    }
    public static string GetMyName()
    {
        return myName;
    }
    private void Update()
    {
        if (canStartGame)
        {
            try
            {
                //if (!LudoUiHandler.Player_4 && !LudoUiHandler.Player_3 && LudoUiHandler.Player_2)
                if (MaxPlayer == 2)
                {
                    Player_2 = true;
                    Player_4 = false;
                    Player_3 = false;
                    //Debug.Log("2 player called");
                    switch (int.Parse(gameData["playerIndex"]))
                    {
                        case 0:
                            //Dice.turn = 1;
                            myTag = "RED";
                            break;
                        case 1:
                            //Dice.turn = 3;
                            myTag = "YELLOW";
                            break;
                        case 2:
                            //Dice.turn = 3;
                            break;
                        case 3:
                            //Dice.turn = 4;
                            break;

                    }
                }
                //if (!LudoUiHandler.Player_4 && LudoUiHandler.Player_3 && !LudoUiHandler.Player_2)
                if (MaxPlayer == 3)
                {
                    Player_3 = true;
                    Player_2 = false;
                    Player_4 = false;
                    //Debug.Log("3 player called" + "playerIndex" + int.Parse(gameData["playerIndex"]));
                    switch (int.Parse(gameData["playerIndex"]))
                    {
                        case 0:
                            //Dice.turn = 1;
                            myTag = "RED";
                            Debug.Log(myTag);
                            break;
                        case 1:
                            //Dice.turn = 3;
                            myTag = "GREEN";
                            Debug.Log(myTag);
                            break;
                        case 2:
                            //Dice.turn = 3;
                            myTag = "YELLOW";
                            Debug.Log(myTag);
                            break;
                        case 3:
                            //Dice.turn = 4;
                            break;

                    }
                }
                //if (LudoUiHandler.Player_4 && !LudoUiHandler.Player_3 && !LudoUiHandler.Player_2)
                if (MaxPlayer == 4)
                {
                    Player_4 = true;
                    Player_3 = false;
                    Player_2 = false;
                    //Debug.Log("4 player called");
                    switch (int.Parse(gameData["playerIndex"]))
                    {
                        case 0:
                            //Dice.turn = 1;
                            myTag = "RED";
                            Debug.Log(myTag);
                            break;
                        case 1:
                            //Dice.turn = 3;
                            myTag = "GREEN";
                            Debug.Log(myTag);
                            break;
                        case 2:
                            //Dice.turn = 3;
                            myTag = "YELLOW";
                            Debug.Log(myTag);
                            break;
                        case 3:
                            //Dice.turn = 4;
                            myTag = "BLUE";
                            break;

                    }
                }
                //Ludo_2Player();
                SceneManager.LoadScene("Ludo");
            }
            catch (Exception e)
            {
                Debug.Log(e.Message);
            }
            canStartGame = false;
        }
    }
    public void Ludo_4Player()
    {
        Player_4 = true;
        Player_3 = false;
        Player_2 = false;
        MaxPlayer = 4;
        //Debug.Log(Player_4.ToString() + Player_3 + Player_2);
        //StartSearchingForMatch(4);
        //SceneManager.LoadScene("Ludo");
    }

    public void Ludo_3Player()
    {
        Player_3 = true;
        Player_2 = false;
        Player_4 = false;
        MaxPlayer = 3;
        //Debug.Log(Player_4.ToString() + Player_3 + Player_2);
        //StartSearchingForMatch(3);
    }
    public void Ludo_2Player()
    {
        Player_2 = true;
        Player_4 = false;
        Player_3 = false;
        MaxPlayer = 2;
        //Debug.Log(Player_4.ToString() + Player_3 + Player_2);
        //StartSearchingForMatch(2);
    }
    public void BackBtn()
    {
        SceneManager.LoadScene("LudoMainMenu");
    }
    public void ApplicationQuit()
    {
        Application.Quit();
    }

    //public void StartSearchingForMatch(int numberOfPlayers)
    //{
    //    Hashtable data = new Hashtable();
    //    data.Add("guid", myGuid);
    //    data.Add("maxPlayer", numberOfPlayers);
    //    client.EmitAsync("start", JsonConvert.SerializeObject(data));
    //}
    public void startSearchingForMatch()
    {

        Hashtable data = new Hashtable();
        data.Add("guid", myGuid);
        data.Add("maxPlayer", MaxPlayer);
        data.Add("playername", myName);//Add player name

        client.EmitAsync("start", JsonConvert.SerializeObject(data));

        StartButton.GetComponent<Button>().interactable = false;
    }

    public void givePlayerName()
    {
        myName = playerName_InputField.GetComponent<TMP_InputField>().text;
    }
    public void CreateRoomByName()
    {
        string roomName = CreateRoom_InputField.GetComponent<TMP_InputField>().text;

        Hashtable roomData = new Hashtable();
        roomData.Add("guid", myGuid);
        roomData.Add("roomName", roomName);
        roomData.Add("customRoomFlag", "create");
        roomData.Add("maxPlayer", MaxPlayer);
        roomData.Add("playername", myName);
        Debug.Log(roomData["maxPlayer"]);

        client.EmitAsync("start", JsonConvert.SerializeObject(roomData));

        WaitingForFriendPanel.SetActive(true);
        StartButton.GetComponent<Button>().interactable = false;

        CreateRoom_Button.GetComponent<Button>().interactable = false;

    }

    public void JoinRoomByName()
    {
        string roomName = JoinRoom_InputField.GetComponent<TMP_InputField>().text;

        Hashtable roomData = new Hashtable();

        roomData.Add("guid", myGuid);
        roomData.Add("roomName", roomName);
        roomData.Add("customRoomFlag", "join");
        roomData.Add("playername", myName);

        client.EmitAsync("start", JsonConvert.SerializeObject(roomData));

        SearchingPlayerPanel.SetActive(true);
        StartButton.GetComponent<Button>().interactable = false;

        JoinRoom_Button.GetComponent<Button>().interactable = false;
    }
    private void OnApplicationQuit()
    {
        client.Dispose();
        client.DisconnectAsync();
    }

}
