using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightPlacement : MonoBehaviour
{
    [SerializeField]
    GameObject Sun;

    [SerializeField]
    GameObject Sky;
    
    [SerializeField]
    static public float specularPower = 6f;

    float time = 70f;
    float meteorTime = 0f;

    public float timeFlowSpeed = 0.3f;

    public float distance;

    [SerializeField]
    GameObject prefab;


    // Update is called once per frame
    void Update()
    {
        //timers
        time += Time.deltaTime * timeFlowSpeed;
        meteorTime += Time.deltaTime;

        //rotating the directional light to create the effect of a primitive day night cycle
        Sky.transform.rotation = Quaternion.Euler(time, 0, 0);
        Sun.transform.position = new Vector3(0,
               Mathf.Sin(Sky.transform.rotation.eulerAngles.x * Mathf.Deg2Rad) * distance,
               Mathf.Cos(Sky.transform.rotation.eulerAngles.x * Mathf.Deg2Rad) * distance);

        //shooting out asteroids
        if (meteorTime >= Random.Range(1f, 2f)){
            Instantiate(prefab, transform);
            meteorTime = 0f;
        }
    }
}
