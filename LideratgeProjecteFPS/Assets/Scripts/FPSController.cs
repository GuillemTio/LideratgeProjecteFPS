using System.Collections;
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

    [Header("Movement")]
    public float m_MoveSpeed;
    public Rigidbody m_RigidBody;
    public GameObject m_PlayerBody;
    public float m_PlayerHeight;
    public LayerMask m_GroundLayer;
    bool m_IsOnGround;
    public float m_JumpForce;

    Vector3 m_StartPosition;
    Quaternion m_StartRotation;

    public Camera m_Camera;

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


        //Debug.Log(m_RigidBody.velocity.magnitude);

        m_IsOnGround = Physics.Raycast(m_PlayerBody.transform.position, Vector3.down, m_PlayerHeight / 2 + 0.2f, m_GroundLayer);

        if (m_IsOnGround)
        {
            if (Input.GetKeyDown(m_JumpKeyCode))
            {
                m_RigidBody.AddForce(transform.up * m_JumpForce, ForceMode.Impulse);
            }
        }


    }

    private void FixedUpdate()
    {
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


        m_RigidBody.AddForce(l_Movement * m_MoveSpeed, ForceMode.Force);

        //SPEED CONTROL
        Vector3 l_FlatVel = new Vector3(m_RigidBody.velocity.x, 0, m_RigidBody.velocity.z);
        if (l_FlatVel.magnitude > m_MoveSpeed)
        {
            Vector3 l_LimitedVel = l_FlatVel.normalized * m_MoveSpeed;
            m_RigidBody.velocity = new Vector3(l_LimitedVel.x, m_RigidBody.velocity.y, l_LimitedVel.z);
        }
    }



    public void RestartLevel()
    {
        //m_CharacterController.enabled = false;
        transform.position = m_StartPosition;
        transform.rotation = m_StartRotation;
        m_Yaw = transform.rotation.eulerAngles.y;
        m_Pitch = 0.0f;
        //m_CharacterController.enabled = true;
    }

    void SetStartPosition(Transform startTransform)
    {
        m_StartPosition = startTransform.position;
        m_StartRotation = startTransform.rotation;
        //m_CharacterController.enabled = false;
        transform.position = m_StartPosition;
        transform.rotation = m_StartRotation;
        m_Yaw = transform.rotation.eulerAngles.y;
        m_Pitch = 0.0f;
        //m_CharacterController.enabled = true;
    }
    
    public void AddTorque(Vector2 torque, float duration)
    {
        StartCoroutine(AddTorqueOverTime(torque, duration));
    }

    private IEnumerator AddTorqueOverTime(Vector2 torque, float duration)
    {
        var l_TorqueAdded = torque;
        var l_Timer = duration;
        var l_Speed = torque / duration;
        Debug.Log("PICH BEFORE: " + m_Pitch);
        var l_AddedPitch = 0f;
        var l_AddedYaw = 0f;
        while (l_Timer > 0)
        {
            m_Pitch -= l_AddedPitch;
            m_Yaw -= l_AddedYaw;
            
            m_Pitch += l_Speed.y * Time.deltaTime;
            m_Yaw += l_Speed.x * Time.deltaTime;

            // m_Pitch += l_Speed.y * Time.deltaTime;
            // m_Yaw += l_Speed.x * Time.deltaTime;
            l_Timer -= Time.deltaTime;
            
            // var l_TimeFraction = Mathf.Clamp01(l_Timer / duration);
            // l_TorqueAdded = Vector2.Lerp(Vector2.zero, torque, l_TimeFraction);
            // Debug.Log("Timer: " + l_Timer + " Torque: " + l_TorqueAdded.magnitude);
            yield return null;
        }
        Debug.Log("PICH AFTER: " + m_Pitch);

    }



}
