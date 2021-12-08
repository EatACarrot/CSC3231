using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    Transform cam;

    [SerializeField]
    float turnSpeed;
    Rigidbody rb;
    [SerializeField]
    float moveSpeed = 10.0f;
    

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void TargetAPosition()
    {
        //rotating the player mesh with camera's rotation
        Vector3 myrot = transform.rotation.eulerAngles;
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(myrot.x, cam.rotation.eulerAngles.y, myrot.z), turnSpeed);
        rb.velocity += transform.forward * Input.GetAxis("Vertical") * moveSpeed;
        rb.velocity += transform.right * Input.GetAxis("Horizontal") * moveSpeed;
        if (Input.GetKey(KeyCode.LeftShift))
        {
            rb.velocity += -transform.up * moveSpeed;
        }
        else if (Input.GetKey(KeyCode.Space))
        {
            rb.velocity += transform.up * moveSpeed;
        }
    }

    void Update()
    {
        TargetAPosition();
    }


}
