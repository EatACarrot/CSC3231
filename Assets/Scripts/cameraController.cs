using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraController : MonoBehaviour
{
    private Camera cam;
    [SerializeField]
    Transform playa;
    [SerializeField]
    float followSpeed;
    public float rotationOnX;
    public float rotationOnY;
    public float turnSpeed = 2.0f;

    void Start()
    {
        
        cam = gameObject.GetComponent<Camera>();

        //hiding the cursor
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void LookADirection()
    {
        //rotating camera with the mouse
        float mouseY = Input.GetAxis("Mouse Y") * Time.deltaTime * turnSpeed;
        float mouseX = Input.GetAxis("Mouse X") * Time.deltaTime * turnSpeed;
        rotationOnX -= mouseY;
        rotationOnY += mouseX;
        transform.localEulerAngles = new Vector3(rotationOnX, rotationOnY, 0f);

    }
    void TargetAPosition()
    {
        //following the player 
        transform.position = Vector3.Lerp(transform.position, playa.position, followSpeed);

    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        LookADirection();
        TargetAPosition();
    }
}
