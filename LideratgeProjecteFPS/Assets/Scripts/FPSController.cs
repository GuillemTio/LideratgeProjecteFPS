using System;
using UnityEngine;
public class FPSController : MonoBehaviour
{
    float m_Yaw;
    float m_Pitch;
    public Transform m_PitchController;

    //public float m_HealthStart;
    //float m_HealthCurrent;
    //public float m_ShieldStart;
    //float m_ShieldCurrent;

    public bool m_YawInverted;
    public bool m_PitchInverted;
    public float m_MinPitch;
    public float m_MaxPitch;
    public float m_PitchSpeed;
    public float m_YawSpeed;

    [Header("NewMovement")]
    public float maxSpeed;
    public float maxSprintSpeed;
    public float acceleration;
    public float jumpForce;
    private Vector3 currentMovementForce;

    [Header("OldMovement")]
    public float m_Speed;
    public float m_VerticalSpeed;
    public float m_SprintSpeed;
    public float m_JumpSpeed;
    public float m_LastTimeOnFloor;

    Vector3 m_StartPosition;
    Quaternion m_StartRotation;

    public Camera m_Camera;

    CharacterController m_CharacterController;

    [Header("Input")]

    public KeyCode m_LeftKeyCode = KeyCode.A;
    public KeyCode m_RightKeyCode = KeyCode.D;
    public KeyCode m_UpKeyCode = KeyCode.W;
    public KeyCode m_DownKeyCode = KeyCode.S;
    public KeyCode m_JumpKeyCode = KeyCode.Space;
    public KeyCode m_SprintKeyCode = KeyCode.LeftShift;
    public KeyCode m_ReloadKeyCode = KeyCode.R;
    public KeyCode m_EnterKeyCode = KeyCode.KeypadEnter;
    public int m_ShootMouseButton = 0;

    [Header("DebugInput")]
    bool m_AngleLocked = false;
    bool m_AimLocked = true;
    public KeyCode m_DebugLockAngleKeyCode = KeyCode.I;
    public KeyCode m_DebugLockKeyCode = KeyCode.O;

    private void Awake()
    {
        m_CharacterController = GetComponent<CharacterController>();

        /*
        if (GameController.GetGameController().m_Player == null)
        {
            GameController.GetGameController().m_Player = this;
            GameObject.DontDestroyOnLoad(gameObject);
            m_StartPosition = transform.position;
            m_StartRotation = transform.rotation;
            SetAmmo();
            SetHealthShield();
            SetScore();

            m_Yaw = transform.rotation.eulerAngles.y;
        }
        else
        {
            GameController.GetGameController().m_Player.SetStartPosition(transform);
            GameObject.Destroy(this.gameObject);
        }
        */
    }

    void Start()
    {

        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
#if UNITY_EDITOR
        if (Input.GetKeyDown(m_DebugLockAngleKeyCode))
            m_AngleLocked = !m_AngleLocked;
        if (Input.GetKeyDown(m_DebugLockKeyCode))
        {
            if (Cursor.lockState == CursorLockMode.Locked)
                Cursor.lockState = CursorLockMode.None;
            else
                Cursor.lockState = CursorLockMode.Locked;
            m_AimLocked = Cursor.lockState == CursorLockMode.Locked;
        }
#endif

        float l_HorizontalMovement = Input.GetAxis("Mouse X");
        float l_VerticalMovement = Input.GetAxis("Mouse Y");

        if (m_AngleLocked)
        {
            l_HorizontalMovement = 0f;
            l_VerticalMovement = 0f;
        }

        float l_YawInverted = m_YawInverted ? -1f : 1f;
        float l_PitchInverted = m_PitchInverted ? -1f : 1f;

        m_Yaw = m_Yaw + m_YawSpeed * l_HorizontalMovement * Time.deltaTime * l_YawInverted;
        m_Pitch = m_Pitch + m_PitchSpeed * l_VerticalMovement * Time.deltaTime * l_PitchInverted;
        m_Pitch = Mathf.Clamp(m_Pitch, m_MinPitch, m_MaxPitch);

        transform.rotation = Quaternion.Euler(0.0f, m_Yaw, 0.0f);
        m_PitchController.localRotation = Quaternion.Euler(m_Pitch, 0.0f, 0.0f);

        float l_YawInRadians = m_Yaw * Mathf.Deg2Rad;
        float l_Yaw90InRadians = (m_Yaw + 90) * Mathf.Deg2Rad;

        Vector3 l_Forward = new Vector3(Mathf.Sin(l_YawInRadians), 0, Mathf.Cos(l_YawInRadians));
        Vector3 l_Right = new Vector3(Mathf.Sin(l_Yaw90InRadians), 0, Mathf.Cos(l_Yaw90InRadians));


        Vector3 movementDirection = Vector3.zero;

        if (Input.GetKey(m_LeftKeyCode))
        {
            movementDirection -= l_Right;
        }
        if (Input.GetKey(m_RightKeyCode))
        {
            movementDirection += l_Right;
        }
        if (Input.GetKey(m_UpKeyCode))
        {
            movementDirection += l_Forward;
        }
        if (Input.GetKey(m_DownKeyCode))
        {
            movementDirection -= l_Forward;
        }

        movementDirection.Normalize();

        Vector3 inputMovementForce = movementDirection * acceleration * Time.deltaTime;
        if(inputMovementForce.x == 0 && currentMovementForce.x != 0 )
        {
            if (currentMovementForce.x > 0)
            {
                inputMovementForce.x = Math.Max(0, currentMovementForce.x - acceleration);
            }
            else if (currentMovementForce.x < 0)
            {
                inputMovementForce.x = Math.Min(0, currentMovementForce.x + acceleration);
            }
        }

        if (inputMovementForce.y == 0 && currentMovementForce.y != 0)
        {
            if (currentMovementForce.y > 0)
            {
                inputMovementForce.y = Math.Max(0, currentMovementForce.y - acceleration);
            }
            else if (currentMovementForce.x < 0)
            {
                inputMovementForce.y = Math.Min(0, currentMovementForce.y + acceleration);
            }
        }

        currentMovementForce += inputMovementForce;
        //Debug.Log(currentMovementForce);
        m_CharacterController.Move(currentMovementForce);

        //limitar velocidad segun el vector que se genera

        /* float l_Speed = m_Speed;

        m_LastTimeOnFloor += Time.deltaTime;

        if (Input.GetKeyDown(m_JumpKeyCode) && m_VerticalSpeed == 0)
        {
            m_VerticalSpeed = m_JumpSpeed;
        }

        if (Input.GetKey(m_SprintKeyCode))
        {
            l_Speed = m_SprintSpeed;
        }

        float l_YawInverted = m_YawInverted ? -1f : 1f;
        float l_PitchInverted = m_PitchInverted ? -1f : 1f;

        m_Yaw = m_Yaw + m_YawSpeed * l_HorizontalMovement * Time.deltaTime * l_YawInverted;
        m_Pitch = m_Pitch + m_PitchSpeed * l_VerticalMovement * Time.deltaTime * l_PitchInverted;
        m_Pitch = Mathf.Clamp(m_Pitch, m_MinPitch, m_MaxPitch);

        transform.rotation = Quaternion.Euler(0.0f, m_Yaw, 0.0f);
        m_PitchController.localRotation = Quaternion.Euler(m_Pitch, 0.0f, 0.0f);

        float l_YawInRadians = m_Yaw * Mathf.Deg2Rad;
        float l_Yaw90InRadians = (m_Yaw + 90) * Mathf.Deg2Rad;

        Vector3 l_Forward = new Vector3(Mathf.Sin(l_YawInRadians), 0, Mathf.Cos(l_YawInRadians));
        Vector3 l_Right = new Vector3(Mathf.Sin(l_Yaw90InRadians), 0, Mathf.Cos(l_Yaw90InRadians));

        Vector3 l_Movement = Vector3.zero;

        if (Input.GetKey(m_LeftKeyCode))
        {
            l_Movement = -l_Right;
        }
        else if (Input.GetKey(m_RightKeyCode))
        {
            l_Movement = l_Right;
        }

        if (Input.GetKey(m_UpKeyCode))
        {
            l_Movement += l_Forward;
        }
        else if (Input.GetKey(m_DownKeyCode))
        {
            l_Movement -= l_Forward;
        }

        l_Movement.Normalize();

        l_Movement *= l_Speed * Time.deltaTime;

        m_VerticalSpeed = m_VerticalSpeed + Physics.gravity.y * Time.deltaTime;
        l_Movement.y = m_VerticalSpeed * Time.deltaTime;


        CollisionFlags l_CollisionFlags = m_CharacterController.Move(l_Movement);
        if ((l_CollisionFlags & CollisionFlags.CollidedBelow) != 0)
            m_VerticalSpeed = 0f;
        m_LastTimeOnFloor = 0.0f;
        if ((l_CollisionFlags & CollisionFlags.CollidedBelow) != 0 && m_VerticalSpeed > 0f)
            m_VerticalSpeed = 0f;


        //m_CharacterController.Move(l_Movement);

        if (Input.GetKeyDown(m_JumpKeyCode) && m_VerticalSpeed == 0.0f)
        {
            m_VerticalSpeed = m_JumpSpeed;
        }
        */


    }




    public void RestartLevel()
    {
        m_CharacterController.enabled = false;
        transform.position = m_StartPosition;
        transform.rotation = m_StartRotation;
        m_Yaw = transform.rotation.eulerAngles.y;
        m_Pitch = 0.0f;
        m_CharacterController.enabled = true;
    }

    void SetStartPosition(Transform startTransform)
    {
        m_StartPosition = startTransform.position;
        m_StartRotation = startTransform.rotation;
        m_CharacterController.enabled = false;
        transform.position = m_StartPosition;
        transform.rotation = m_StartRotation;
        m_Yaw = transform.rotation.eulerAngles.y;
        m_Pitch = 0.0f;
        m_CharacterController.enabled = true;
    }



}
