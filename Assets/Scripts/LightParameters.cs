using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightParameters : MonoBehaviour
{

    [SerializeField]
    Light worldLight;

    [SerializeField]
    GameObject cameraObject;
    void Start()
    {
        worldLight = GameObject.FindGameObjectWithTag("Light").GetComponent<Light>();
        cameraObject = GameObject.FindGameObjectWithTag("MainCamera");

    }

    void Update()
    {
        MeshRenderer r = GetComponent<MeshRenderer>();
        
        r.material.SetVector("camPosition", cameraObject.transform.position);

        
        r.material.SetVector("lightPosition", worldLight.transform.position);
        r.material.SetColor("lightColour", worldLight.color);

        r.material.SetFloat("lightIntensity", worldLight.intensity);
        r.material.SetFloat("lightRange", worldLight.range);

        r.material.SetFloat("specularPower", LightPlacement.specularPower);
    }
}
