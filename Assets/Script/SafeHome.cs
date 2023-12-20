using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SafeHome : MonoBehaviour
{
    //public GameObject[] PawnArray;
    List<GameObject> _Pawns = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (_Pawns.Count == 3)
            foreach (GameObject obk in _Pawns)
                obk.transform.localScale = new Vector3(12, 12, 12);
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.tag == "GreenPawn" || other.tag == "RedPawn" || other.tag == "YellowPawn" || other.tag == "BluePawn")
        {


            _Pawns.Add(other.gameObject);
            //Debug.Log(_Pawns.Count);



            if (_Pawns.Count == 1)
            {
                //Debug.Log("000");
                _Pawns[0].gameObject.transform.localScale = new Vector3(25, 25, 25);
            }
            if (_Pawns.Count == 2)
            {
                for (int i = 0; i < _Pawns.Count; i++)
                {
                    _Pawns[i].gameObject.transform.localScale = new Vector3(16, 16, 16);

                }
                _Pawns[0].gameObject.transform.position = gameObject.transform.position + new Vector3(-0.1f, -0.1f, 0);
                _Pawns[1].gameObject.transform.position = gameObject.transform.position + new Vector3(0.2f, 0.1f, 0);
            }
            else if (_Pawns.Count == 3)
            {
                //for (int i = 0; i < _Pawns.Count; i++)
                // {
                //     _Pawns[i].transform.localScale = new Vector3 (5,5,5);

                // }
                
                _Pawns[0].gameObject.transform.position = gameObject.transform.position + new Vector3(0, 0.2f,0 );
                _Pawns[1].gameObject.transform.position = gameObject.transform.position + new Vector3(-0.2f, -0.1f,0 );
                _Pawns[2].gameObject.transform.position = gameObject.transform.position + new Vector3(0.2f, -0.1f, 0);
            }
            else if (_Pawns.Count == 4)
            {
                for (int i = 0; i < _Pawns.Count; i++)
                {
                    _Pawns[i].gameObject.transform.localScale = new Vector3(10, 10, 10);
                }
                _Pawns[0].gameObject.transform.position = gameObject.transform.position + new Vector3(-0.1f, -0.1f,0 );
                _Pawns[1].gameObject.transform.position = gameObject.transform.position + new Vector3(0.1f, 0.1f,0 );
                _Pawns[2].gameObject.transform.position = gameObject.transform.position + new Vector3(-0.1f, 0.1f,0 );
                _Pawns[3].gameObject.transform.position = gameObject.transform.position + new Vector3(0.1f, -0.1f,0 );
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        //Debug.Log("Exited");
        if (other.tag == "GreenPawn" || other.tag == "RedPawn" || other.tag == "YellowPawn" || other.tag == "BluePawn")
        {
            other.transform.localScale = new Vector3(25, 25, 25);
            _Pawns.Remove(other.gameObject);

            if (_Pawns.Count == 1)
            {
                _Pawns[0].gameObject.transform.localScale = new Vector3(25, 25, 25);
                _Pawns[0].gameObject.transform.position = gameObject.transform.position;

            }

            if (_Pawns.Count == 2)
            {
                for (int i = 0; i < _Pawns.Count; i++)
                {
                    _Pawns[i].gameObject.transform.localScale = new Vector3(16, 16, 16);

                }
                _Pawns[0].gameObject.transform.position = gameObject.transform.position + new Vector3(-0.1f, 0, -0.1f);
                _Pawns[1].gameObject.transform.position = gameObject.transform.position + new Vector3(0.1f, 0, 0.1f);
            }
            else if (_Pawns.Count == 3)
            {
                for (int i = 0; i < _Pawns.Count; i++)
                {
                    _Pawns[i].gameObject.transform.localScale = new Vector3(12, 12, 12);
                }
                _Pawns[0].gameObject.transform.position = gameObject.transform.position + new Vector3(-0.2f, 0, -0.2f);
                _Pawns[1].gameObject.transform.position = gameObject.transform.position + new Vector3(0.2f, 0, 0.2f);
                _Pawns[2].gameObject.transform.position = gameObject.transform.position;
            }
        }
    }
}
