using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(PlayerMotor))]
[RequireComponent(typeof(ConfigurableJoint))]
public class PlayerController : MonoBehaviour {
    [SerializeField]
    private float speed = 5f;
    [SerializeField]
    private float LookSensitivity = 1.5f;
    
    [SerializeField]
    private float thrustForce = 1000f;

    [SerializeField]
    private float thrusterFuelBurnSpeed = 1f;

    [SerializeField]
    private float thrusterFuelRegenSpeed = 0.3f;

    private float thrusterFuelAmount = 1f;

    public float GetThrusterFuelAmount()
    {
        return thrusterFuelAmount;
    }

    [Header("Spring Settings")]
   
    [SerializeField]
    private float jointSpring=20f;
    [SerializeField]
    private float jointMaxForce = 40f;

    private PlayerMotor motor;
    private ConfigurableJoint cj;


    [SerializeField]
    private Animator animator;

    [SerializeField]
    private LayerMask enviromentMask;
    void Start()
    {
        motor = GetComponent<PlayerMotor>();
        cj = GetComponent<ConfigurableJoint>();
        animator = GetComponent<Animator>();
        SetJointSettings(jointSpring);
    }
    void Update()
    {
        if (PauseMenu.isOn)
            return;
        //Setting Target Position For Spring
        RaycastHit hit;
        if(Physics.Raycast(transform.position,Vector3.down,out hit, 100f,enviromentMask))
        {
            cj.targetPosition = new Vector3(0f, -hit.point.y, 0f);
        }
        else
        {
            cj.targetPosition = new Vector3(0f, 0f, 0f);
        }

        //Calculate Movement Velocity as a 3D vector
        float _xMov = Input.GetAxis("Horizontal");
        float _zMov = Input.GetAxis("Vertical");

        
        Vector3 _movHorizontal = transform.right * _xMov;
        Vector3 _movVertical = transform.forward * _zMov;
        // Final Movement Vector
        Vector3 _velocity = (_movHorizontal + _movVertical).normalized * speed;
        //Animate Movement
        animator.SetFloat("ForwardVelocity", _zMov);
        
        
        //Apply Movement
        motor.Move(_velocity);
        //Calculate rotation as a 3D vector(turning around)
        float _yRot = Input.GetAxisRaw("Mouse X");
        
        Vector3 rotation = new Vector3(0f, _yRot, 0f) * LookSensitivity ;
        //Apply Rotation
        motor.Rotate(rotation);

        //Calculate Camer rotation as a 3D vector(turning around)
        float _xRot = Input.GetAxisRaw("Mouse Y");

        float Camerarotation = _xRot * LookSensitivity;
        //Apply Rotation
        motor.CameraRotate(Camerarotation);

        Vector3 _thrustForce = Vector3.zero;
        //Apply Thrust Force
        if (Input.GetButton("Jump") && thrusterFuelAmount > 0f)
        {
            thrusterFuelAmount -= thrusterFuelBurnSpeed * Time.deltaTime;

            if (thrusterFuelAmount >= 0.01f)
            {
                _thrustForce = Vector3.up * thrustForce;
                SetJointSettings(0f);
            }
        }
        else
        {
            thrusterFuelAmount += thrusterFuelRegenSpeed * Time.deltaTime;
            SetJointSettings(jointSpring);
        }
        thrusterFuelAmount = Mathf.Clamp(thrusterFuelAmount, 0f, 1f);

        //Apply Jump
        motor.ApplyThrust(_thrustForce);
    }
    private void SetJointSettings(float _jointSpring)
    {
        cj.yDrive = new JointDrive {  positionSpring = _jointSpring, maximumForce = jointMaxForce };  
    }

}
