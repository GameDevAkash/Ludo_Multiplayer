using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public Color startColor;
    public Color endColor;
    [Range(0, 10)]
    public float speed;

    public Renderer ren;
    // Start is called before the first frame update
    void Awake()
    {
        ren = GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
