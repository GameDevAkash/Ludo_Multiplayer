using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using StreamChat.Core;
using StreamChat.Libs.Auth;
using StreamChat.Core.StatefulModels;
using System.Threading.Tasks;
using System;
using Newtonsoft.Json;
using UnityEngine.UI;
using TMPro;

public class GameUI : MonoBehaviour
{
    [SerializeField] GameObject ChatRootView;
    [SerializeField] GameObject ChatInputField;
    [SerializeField] GameObject ChatMessageListViewContent;
    [SerializeField] GameObject MessageViewLocalUserVariant;
    [SerializeField] GameObject MessageView;

    string userToken;
    string channelId;
    string API_KEY = "tkxbebheh2g2";
    string uri = "http://127.0.0.1:3001/api/stream/get_token";

    UnityWebRequest webRequest;
    IStreamChatClient client;
    IStreamChannel channel;

    cube cubeInstance;
    string roomid;

    private void Start()
    {
        cubeInstance = GameObject.Find("Network").GetComponent<cube>();
        roomid = UI.GetRoomid();
        channelId = roomid;

        WWWForm form = new WWWForm();

        form.AddField("userid", cubeInstance.myGuid);
        Debug.Log("myRoomid+channelID: " + roomid);
        form.AddField("roomid", roomid);

        webRequest = UnityWebRequest.Post(uri, form);

        UnityWebRequestAsyncOperation Request = webRequest.SendWebRequest();
        Request.completed += GameUI_completed;

    }

    private async void GameUI_completed(AsyncOperation obj)
    {
        string[] pages = uri.Split('/');
        int page = pages.Length - 1;

        switch (webRequest.result)
        {
            case UnityWebRequest.Result.ConnectionError:
            case UnityWebRequest.Result.DataProcessingError:
                Debug.LogError(pages[page] + ": Error: " + webRequest.error);
                return;
            case UnityWebRequest.Result.ProtocolError:
                Debug.LogError(pages[page] + ": HTTP Error: " + webRequest.error);
                return;
            case UnityWebRequest.Result.Success:
                Debug.Log(pages[page] + ":\nReceived: " + webRequest.downloadHandler.text);
                break;
        }

        Response response = JsonConvert.DeserializeObject<Response>(webRequest.downloadHandler.text);

        Debug.Log($"recv token: {response.token}");
        userToken = response.token;

        client = StreamChatClient.CreateDefaultClient();

        AuthCredentials credential = new AuthCredentials(API_KEY, cubeInstance.myGuid, userToken);

        IStreamLocalUserData userdata = await client.ConnectUserAsync(credential);

        channel = await client.GetOrCreateChannelWithIdAsync(ChannelType.Gaming, channelId: channelId);
        //await channel.SendNewMessageAsync("Hello world! from "+ userToken);

        channel.MessageReceived += StreamMessageReceive;

    }

    private void StreamMessageReceive(IStreamChannel channel, IStreamMessage message)
    {
        if (message.User.Id == cubeInstance.myGuid) return;

        GameObject messageView = Instantiate(MessageView);
        messageView.transform.Find("Column/Message").GetComponent<TMP_Text>().text = message.Text;

        messageView.transform.SetParent(ChatMessageListViewContent.transform);

        Debug.Log(message.Text);
    }

    public void OnMessageButtonClick()
    {
        if (ChatRootView.activeSelf)
        {
            ChatRootView.SetActive(false);
        }
        else
            ChatRootView.SetActive(true);
    }
    public void SendStreamMessage()
    {
        string message = ChatInputField.GetComponent<TMP_InputField>().text;

        GameObject messageView = Instantiate(MessageViewLocalUserVariant);
        messageView.transform.Find("Column/Message").GetComponent<TMP_Text>().text = message;
        messageView.transform.SetParent(ChatMessageListViewContent.transform);

        channel.SendNewMessageAsync(message);
        Debug.Log(message);
    }

}



class Response
{
    public string token;
    public string channelId;
}