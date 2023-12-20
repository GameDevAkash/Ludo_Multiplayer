using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketIOClient;
using Newtonsoft.Json;
using System.Text.Json;
using System;
using System.Threading.Tasks;
using UnityEngine.UIElements;

public class cube : MonoBehaviour
{
    private Vector3 dir;
    private float moveSpeed = 10f;
    SocketIO io;

    [SerializeField] GameObject mycube;

    public string myGuid;

    Dictionary<string, GameObject> enemyGameObject = new Dictionary<string, GameObject>();

    [SerializeField] GameObject enemy;

    delegate void MainThread();
    Queue<MainThread> MainThreadDispatcher = new Queue<MainThread>();

    void Start()
    {
        myGuid = UI.GetMyGuid();
        dir = new Vector3();
        io = UI.GetSocketIOClient();

        RegisterSocketEvent();

        Hashtable myTransform = new Hashtable();
        myTransform.Add("x", mycube.transform.position.x);
        myTransform.Add("y", mycube.transform.position.y);
        myTransform.Add("z", mycube.transform.position.z);
        myTransform.Add("guid", myGuid);

        //await Task.Delay(4000);
        io.EmitAsync("newPlayer", JsonConvert.SerializeObject(myTransform));

    }

    private void RegisterSocketEvent()
    {
        io.On("newPlayer", (enemyTransform) =>
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
                    }
                    catch (Exception e)
                    {
                        Debug.Log(e.Message);
                    }
                }

                if (enemyData["guid"] == myGuid || enemyGameObject.ContainsKey(enemyData["guid"])) return;

                GameObject _enemy = Instantiate(enemy);

                _enemy.transform.position = new Vector3(
                    float.Parse(enemyData["x"]),
                    float.Parse(enemyData["y"]),
                    float.Parse(enemyData["z"])
                );

                enemyGameObject.Add(enemyData["guid"], _enemy);

                Hashtable myTransform = new Hashtable();
                myTransform.Add("x", mycube.transform.position.x);
                myTransform.Add("y", mycube.transform.position.y);
                myTransform.Add("z", mycube.transform.position.z);
                myTransform.Add("guid", myGuid);

                io.EmitAsync("newPlayer", JsonConvert.SerializeObject(myTransform));
            });
        });

        io.On("newPosition", (enemyNewPosition) =>
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

                GameObject _enemy = enemyGameObject[enemyData["guid"]];

                _enemy.transform.position = new Vector3(
                    float.Parse(enemyData["x"]),
                    float.Parse(enemyData["y"]),
                    float.Parse(enemyData["z"])
                );
            });

        });

        io.On("newRotation", (enemyNewRotation) =>
        {
            MainThreadDispatcher.Enqueue(() =>
            {
                Hashtable o = enemyNewRotation.GetValue(0).Deserialize<Hashtable>();

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

                GameObject _enemy = enemyGameObject[enemyData["guid"]];

                _enemy.transform.rotation = Quaternion.Euler(new Vector3(
                    float.Parse(enemyData["p"]),
                    float.Parse(enemyData["w"]),
                    float.Parse(enemyData["q"])
                ));

            });
        });

        io.On("HandleDisconnect", (data) =>
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

                Destroy(enemyGameObject[playerGuid]);
                enemyGameObject.Remove(playerGuid);
            });

        });

    }

    void Update()
    {
        if (Input.GetKey(KeyCode.W)) dir.z = 1;
        if (Input.GetKey(KeyCode.S)) dir.z = -1;
        if (Input.GetKey(KeyCode.A)) dir.x = -1;
        if (Input.GetKey(KeyCode.D)) dir.x = 1;


        mycube.transform.position += dir.normalized * Time.deltaTime * moveSpeed;

        if (dir != Vector3.zero)
        {
            Hashtable myTransform = new Hashtable();
            myTransform.Add("x", mycube.transform.position.x);
            myTransform.Add("y", mycube.transform.position.y);
            myTransform.Add("z", mycube.transform.position.z);
            myTransform.Add("guid", myGuid);

            io.EmitAsync("newPosition", JsonConvert.SerializeObject(myTransform));
        }

        dir = Vector3.zero;

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            mycube.transform.Rotate(
                mycube.transform.TransformDirection(Vector3.up), -Time.deltaTime * 200);
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            mycube.transform.Rotate(
                mycube.transform.TransformDirection(Vector3.up), Time.deltaTime * 200);
        }

        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow))
        {
            Hashtable myRotation = new Hashtable();
            myRotation.Add("w", mycube.transform.rotation.eulerAngles.y);
            myRotation.Add("p", mycube.transform.rotation.eulerAngles.x);
            myRotation.Add("q", mycube.transform.rotation.eulerAngles.z);
            myRotation.Add("guid", myGuid);

            io.EmitAsync("newRotation", JsonConvert.SerializeObject(myRotation));
        }




        while (MainThreadDispatcher.Count > 0)
        {
            MainThread _thread = MainThreadDispatcher.Dequeue();

            _thread();

        }

    }
}
