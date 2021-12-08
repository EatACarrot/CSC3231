using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spin : MonoBehaviour
{
    public float distance = 5f;
    public float speed = 5f;
    public bool spinAroundX = true;
    public bool spinAroundY = false;
    public bool spinAroundZ = false;
    float time;

    // Start is called before the first frame update
    void Update()
    {
        
        time += Time.deltaTime;
        transform.localPosition = new Vector3(Mathf.Cos(time * speed) * distance * (spinAroundX ? -1 : 1), 
            Mathf.Sin(time * speed) * distance * (spinAroundY ? -1 : 1),
            Mathf.Cos(time * speed) * distance * (spinAroundZ ? -1 : 1));
        
    }
    
}
