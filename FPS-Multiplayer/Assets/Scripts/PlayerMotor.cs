using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Rigidbody))]

public class PlayerMotor : MonoBehaviour {
    [SerializeField]
    private Camera cam;

    private Vector3 velocity = Vector3.zero;
    private Vector3 rotation = Vector3.zero;
    private float Camerarotation =0f;
    private float CurrentCameraRotationX=0f;
    private Vector3 Thrustforce = Vector3.zero;

    [SerializeField]
    private float cameraRotationLimit = 85f;



    private Rigidbody rb;
   
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        
    }
	//Get a movement vector
    public void Move(Vector3 _velocity)
    {
        velocity = _velocity;


    }
    //Get Rotation Vector
    public void Rotate(Vector3 _rotation)
    {
        rotation = _rotation;


    }
    public void CameraRotate(float _rotation)
    {
        Camerarotation = _rotation;


    }
    public void ApplyThrust(Vector3 _thrusterforce)
    {
        Thrustforce = _thrusterforce;
    }
    //Run Ever Physics Iteration
    void FixedUpdate()
    {
        PerformMovement();
        PerformRotation();

    }

    //Perform movement based on velocity variable
    void PerformMovement()
    {
        if(velocity!= Vector3.zero)
        {
            rb.MovePosition(rb.position + velocity * Time.fixedDeltaTime);
        }
        if (Thrustforce != Vector3.zero)
        {
            rb.AddForce(Thrustforce * Time.fixedDeltaTime, ForceMode.Acceleration);
        }
    }
    void PerformRotation()
    {
        rb.MoveRotation(rb.rotation * Quaternion.Euler(rotation));
        if(cam != null)
        {
            //Set our Rotation And Clap
            CurrentCameraRotationX -= Camerarotation;
            CurrentCameraRotationX = Mathf.Clamp(CurrentCameraRotationX, -cameraRotationLimit, cameraRotationLimit);

            //Apply to camra
            cam.transform.localEulerAngles = new Vector3(CurrentCameraRotationX, 0f, 0f);
        }
    }
   

}
