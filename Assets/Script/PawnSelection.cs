using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PawnSelection : MonoBehaviour
{
    public static GameObject Pawn;
    
    // Start is called before the first frame update
    void Start()
    {
        //P_Selected = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            //Debug.Log("Mouse clicked");
            PawnSelect(Input.mousePosition);
        }
    }

    public void PawnSelect(Vector3 fromPosition)
    {
        
        Ray ray = Camera.main.ScreenPointToRay(fromPosition);

        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            
            //GameObject x = hit.collider.gameObject;
            //if(x.tag =="RedPawn" || x.tag == "GreenPawn" || x.tag == "YellowPawn" || x.tag == "BluePawn")
            //{
            //    Pawn = x;
            //}
            Pawn = hit.collider.gameObject;
            //Debug.Log(Pawn);
            //Debug.Log(Pawn.name);

        }

    }



}
