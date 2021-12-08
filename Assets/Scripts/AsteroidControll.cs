using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AsteroidControll : MonoBehaviour
{
    MeshRenderer r;
    Texture2D tex;
    Rigidbody rb;

    public int width = 256;
    public int height = 256;
    public float scale = 5f;
    public float timeToDie = 10f;


    float time = 0f;

    public float noiseEffectShader = 0.3f;
    // Start is called before the first frame update
    void Start()
    {
        float offsetX = Random.Range(0f, 99999f);
        float offsetY = Random.Range(0f, 99999f);
        tex = CreateNoise.GenNoiseTexture(width, height, scale, offsetX, offsetY);

        r = GetComponent<MeshRenderer>();
        r.material.SetTexture("_NoiseTexture", tex);
        r.material.SetFloat("_NoiseEffect", noiseEffectShader);
        rb = GetComponent<Rigidbody>();
        rb.velocity = new Vector3(Random.Range(1f, 10f), 0, Random.Range(1f, 10f));
    }

    

    // Update is called once per frame
    void Update()
    {
        //rotating the asteroid while it is moving 
        Matrix4x4 rotx = new Matrix4x4(
            new Vector4(1, 0, 0, 0),
            new Vector4(0, Mathf.Cos(transform.position.x), -Mathf.Sin(transform.position.x), 0),
            new Vector4(0, Mathf.Sin(transform.position.x), Mathf.Cos(transform.position.x), 0),
            new Vector4(0, 0, 0, 1));
        Matrix4x4 rotz = new Matrix4x4(
            new Vector4(Mathf.Cos(-transform.position.z), -Mathf.Sin(-transform.position.z), 0, 0),
            new Vector4(Mathf.Sin(-transform.position.z), Mathf.Cos(-transform.position.z), 0, 0),
            new Vector4(0, 0, 1, 0),
            new Vector4(0, 0, 0, 1));

        r.material.SetMatrix("_RotX", rotx);
        r.material.SetMatrix("_RotZ", rotz);

        //removieng it after some time
        time += Time.deltaTime;
        if(time >= timeToDie)
        {
            Destroy(gameObject);
        }
    }
}
