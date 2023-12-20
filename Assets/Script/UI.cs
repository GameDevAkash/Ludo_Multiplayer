using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using SocketIOClient;
using System;
using UnityEngine.UI;
using System.Text.Json;
using TMPro;
using System.Net.Sockets;
using System.Text;
using System.Net;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;

public class UI : MonoBehaviour
{
    static SocketIO client = new SocketIO("http://127.0.0.1:1622/game");
    static string roomid;

    bool canStartGame = false;
    private static string myGuid;

    [SerializeField] GameObject SearchingPlayerPanel;
    [SerializeField] GameObject StartButton;
    [SerializeField] GameObject playerID;
    [SerializeField] GameObject CreateRoom_InputField;
    [SerializeField] GameObject CreateRoom_Button;
    [SerializeField] GameObject JoinRoom_InputField;
    [SerializeField] GameObject JoinRoom_Button;
    [SerializeField] GameObject WaitingForFriendPanel;

    private void Awake()
    {
        //Set screen size for Standalone
#if UNITY_STANDALONE
        Screen.SetResolution(564, 960, false);
        Screen.fullScreen = false;
#endif
    }

    private async void Start()
    {

        myGuid = Guid.NewGuid().ToString();

        playerID.GetComponent<TextMeshProUGUI>().text = "working";
        await client.ConnectAsync();

        Debug.Log(client.Id);

        client.On("canStart", (Data) =>
        {
            Hashtable o = Data.GetValue(0).Deserialize<Hashtable>();

            Dictionary<string, string> gameData = new Dictionary<string, string>();

            foreach (DictionaryEntry kv in o)
            {
                gameData.Add(kv.Key.ToString(), kv.Value.ToString());
                Debug.Log(kv.Key.ToString() + " " + kv.Value.ToString());
            }

            roomid = gameData["roomId"];
            canStartGame = true;

            Debug.Log("canStart");
        });
    }

    private void Update()
    {
        if (canStartGame)
        {
            Debug.Log("game scene");
            try
            {
                SceneManager.LoadScene("SampleScene");
            }
            catch (Exception e)
            {
                Debug.Log(e.Message);
            }
            canStartGame = false;
        }
    }

    public static SocketIO GetSocketIOClient()
    {
        return client;
    }
    public static string GetRoomid()
    {
        return roomid;
    }
    public void StartSearchingForMatch()
    {
        Hashtable data = new Hashtable();
        data.Add("guid", myGuid);

        client.EmitAsync("start", JsonConvert.SerializeObject(data));
        SearchingPlayerPanel.SetActive(true);
        StartButton.GetComponent<Button>().interactable = false;
    }

    public void CreateRoomByName()
    {
        string roomName = CreateRoom_InputField.GetComponent<TMP_InputField>().text;

        Hashtable roomData = new Hashtable();
        roomData.Add("guid", myGuid);
        roomData.Add("roomName", roomName);
        roomData.Add("customRoomFlag", "create");

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

        client.EmitAsync("start", JsonConvert.SerializeObject(roomData));

        SearchingPlayerPanel.SetActive(true);
        StartButton.GetComponent<Button>().interactable = false;

        JoinRoom_Button.GetComponent<Button>().interactable = false;
    }

    public static string GetMyGuid()
    {
        return myGuid;
    }

}
