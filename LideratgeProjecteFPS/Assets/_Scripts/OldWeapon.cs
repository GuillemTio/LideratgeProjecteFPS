using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;



public class OldWeapon : MonoBehaviour
{
    public bool Aiming => m_Aiming;
    public bool Reloading => m_Reloading;
    public int CurrentAmmo => m_CurrentAmmo;
    public int TotalAmmo => m_TotalAmmo;

    [SerializeField] Camera m_RaycastCamera;
    [SerializeField] FPSController m_FPSController;

    [Header ("Input")]
    [SerializeField] KeyCode m_ShootKeyCode = KeyCode.Mouse0;
    [SerializeField] KeyCode m_AimKeyCode = KeyCode.Mouse1;
    [SerializeField] KeyCode m_ReloadKeyCode = KeyCode.R;

    [Header("OldWeapon")]
    [SerializeField] int m_MaxAmmo;
    [SerializeField] int m_MagazineCapacity;
    [SerializeField] float m_FireRate;
    [SerializeField] float m_Range;
    [SerializeField] LayerMask m_LayerMask;
    int m_TotalAmmo;
    int m_CurrentAmmo;
    float m_LastTimeShoot;
    bool m_Reloading;
    private bool m_Aiming;

    [Header("Dispersion")]
    [SerializeField] Crosshair m_Crosshair;
    [Space]
    [SerializeField] List<CDispersionModifiers> m_DispersionModifiersList = new();
    [SerializeField] float m_AddDispPerShot;
    [SerializeField] float m_MaxDispersion;
    [SerializeField] float m_MinDispersion;
    float ModifiedMaxDisp => m_MaxDispersion / (m_Aiming? 2:1);
    float ModifiedMinDisp => m_MinDispersion / (m_Aiming ? 2 : 1);
    [Space]
    float m_CurrentDispersion;
    float m_TargetDispersion;
    [Space]
    [SerializeField] float m_RecoverySpeed;
    [SerializeField] float m_AddingSpeed;

    [Header("Recoil")]
    [SerializeField] float m_MinRecoil;
    [SerializeField] float m_MaxRecoil;
    [SerializeField] float m_RecoilConeAnlge;

    [Header("Animation and FX")]
    [SerializeField] Animator m_Animator;
    [SerializeField] Animator m_PlayerCameraAnimator;
    [SerializeField] AnimationClip m_IdleAnimation;
    [SerializeField] AnimationClip m_RunAnimation;
    [SerializeField] AnimationClip m_ReloadAnimation;
    [SerializeField] AnimationClip m_ShootAnimation;
    bool CanPlayShootAnimation => m_CurrentAmmo > 0 && Input.GetKey(m_ShootKeyCode);
    [Space]
    [SerializeField] ParticleSystem m_MuzzleFlashParticles;
    [SerializeField] ParticleSystem m_CartridgeEjectParticles;
    [SerializeField] GameObject m_DecalPrefab;
    [SerializeField] int m_MaxDecalInScene;
    private CPoolElements m_DecalsPool;

    enum AnimStates {Idle = 0, Run, Reload, Shoot}
    void Start()
    {
        // m_DecalsPool = new(m_DecalPrefab, m_MaxDecalInScene, GameManager.GetGameManager().m_DestroyObjects.transform);
        m_TotalAmmo = m_MaxAmmo;
        m_CurrentAmmo = m_MagazineCapacity;
        m_CurrentDispersion = ModifiedMinDisp;
        m_TargetDispersion = m_CurrentDispersion;

        // GameManager.GetGameManager().AddRestartElement(this);
    }

    void Update()
    {
        m_Aiming = CanAim();

        if (Input.GetKey(m_ShootKeyCode))
            TryShoot();
        if (Input.GetKey(m_ReloadKeyCode))
            TryReload();

        HandleDispersionModifiers();
        UpdateAnimation();
    }

    private bool CanAim()
    {
        return !m_Reloading && Input.GetKey(m_AimKeyCode);
    }

    private void UpdateAnimation()
    {
        AnimStates l_State = AnimStates.Idle;

        if (CanPlayShootAnimation)
            l_State = AnimStates.Shoot;
        else 
        { 
            // if (m_FPSController.Sprinting)
                l_State = AnimStates.Run;
        }
        if (m_Reloading)
            l_State = AnimStates.Reload;
        m_Animator.SetInteger("state", ((int)l_State));

        m_Animator.SetBool("aiming", m_Aiming);
        m_PlayerCameraAnimator.SetBool("aiming", m_Aiming);
    }

    private void HandleDispersionModifiers()
    {
        m_TargetDispersion = ModifiedMinDisp;
        // foreach (CDispersionModifiers dispModifier in m_DispersionModifiersList)
            // m_TargetDispersion += dispModifier.GetDispersionAdded(m_FPSController);

        m_TargetDispersion = Mathf.Clamp(m_TargetDispersion, ModifiedMinDisp, ModifiedMaxDisp);
        
        if (m_Aiming)
            m_TargetDispersion = ModifiedMinDisp;

        if (m_CurrentDispersion > m_TargetDispersion)
            m_CurrentDispersion = Mathf.MoveTowards(m_CurrentDispersion, m_TargetDispersion, m_RecoverySpeed*Time.deltaTime);
        else if (m_CurrentDispersion < m_TargetDispersion)
            m_CurrentDispersion = Mathf.MoveTowards(m_CurrentDispersion, m_TargetDispersion, m_AddingSpeed*Time.deltaTime);

        m_CurrentDispersion = Mathf.Clamp(m_CurrentDispersion, ModifiedMinDisp, ModifiedMaxDisp);

        m_Crosshair?.UpdateCrosshairUI(m_CurrentDispersion);
    }
    private void TryShoot()
    {
        // if (m_CurrentAmmo <= 0 && m_FPSController.AutoReload)
            TryReload();
        if (CanShoot())
            Shoot();
        else if (m_CurrentAmmo <= 0)
            return; // HACER SONIDO DE NO BALAS, ENCASQUILLADAS
    }

    private bool CanShoot()
    {
        return !m_Reloading && m_CurrentAmmo > 0 && Time.time - m_LastTimeShoot >= m_FireRate;
    }


    private void Shoot()
    {
        Ray l_Ray = CreateDispersedRay();
        //Ray l_Ray = m_RaycastCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0.0f));
        if (Physics.Raycast(l_Ray, out RaycastHit l_RaycastHit, m_Range, m_LayerMask))
            HandleHit(l_RaycastHit);
        m_CurrentDispersion += m_AddDispPerShot / (m_Aiming ? 2 : 1);
        ApplyRecoil();
        PlayParticles();
        m_CurrentAmmo--;
        m_LastTimeShoot = Time.time;
    }

    private void PlayParticles()
    {
        m_MuzzleFlashParticles.Play();
        if(!m_Aiming)
            m_CartridgeEjectParticles.Play();
    }

    private void ApplyRecoil()
    {
        float l_RecoilAmmount = Random.Range(m_MinRecoil, m_MaxRecoil) / (m_Aiming? 2 : 1);

        float l_RandomRotationAngle = Random.Range(-m_RecoilConeAnlge / 2, m_RecoilConeAnlge / 2);

        Vector2 l_RecoilDir = Quaternion.AngleAxis(l_RandomRotationAngle, Vector3.forward) * Vector2.down;
        Vector2 l_Torque = l_RecoilDir.normalized * l_RecoilAmmount;

        // m_FPSController.AddTorque(l_Torque, m_FireRate);
    }

    private Ray CreateDispersedRay()
    {
        Vector3 l_CameraCenter = new(0.50f, 0.5f, 0.0f);
        Vector3 l_DispersionOffset = m_Crosshair.GetRandomPointInsideCrosshair();
        l_DispersionOffset.x /= m_RaycastCamera.pixelWidth;
        l_DispersionOffset.y /= m_RaycastCamera.pixelHeight;

        return m_RaycastCamera.ViewportPointToRay(l_CameraCenter + l_DispersionOffset);
    }

    private void HandleHit(RaycastHit l_RaycastHit)
    {
        // // ICanBeShot l_ICanBeShot = l_RaycastHit.transform.GetComponent<ICanBeShot>();
        // l_ICanBeShot?.HandleShot();
        // if (l_RaycastHit.transform.tag != "Mobile")
        // {
        //     CreateDecal(l_RaycastHit.point, l_RaycastHit.normal);
        // }
    }

    private void CreateDecal(Vector3 point, Vector3 normal)
    {
        if (m_DecalsPool == null) return;
        GameObject l_HitParticles = m_DecalsPool.GetNextElement();
        l_HitParticles.transform.SetPositionAndRotation(point, Quaternion.LookRotation(normal));
        // l_HitParticles.transform.parent = GameManager.GetGameManager().m_DestroyObjects.transform;
        l_HitParticles.GetComponent<ParticleSystem>().Play();
    }

    public void TryReload()
    {
        if (CanReload())
        {
            StartCoroutine(ReloadCoroutine());
        }
        else { } // Hacer sonido de no poder recargar
    }

    private IEnumerator ReloadCoroutine()
    {
        m_Reloading = true;
        yield return new WaitForSeconds(m_ReloadAnimation.length / m_Animator.speed);
        Reload();
        m_Reloading = false;
    }

    private void Reload()
    {
        int l_BulletsToReload = m_MagazineCapacity - m_CurrentAmmo;

        if (m_TotalAmmo > l_BulletsToReload)
        {
            m_TotalAmmo -= l_BulletsToReload;
            m_CurrentAmmo += l_BulletsToReload;
        }
        else
        {
            m_CurrentAmmo += m_TotalAmmo;
            m_TotalAmmo = 0;
        }

    }

    private bool CanReload()
    {
        return m_CurrentAmmo < m_MagazineCapacity && m_TotalAmmo > 0 && !m_Reloading;
    }

    public void AddTotalAmmo(int ammoCount)
    {
        m_TotalAmmo += ammoCount;

        if (m_TotalAmmo > m_MaxAmmo) m_TotalAmmo = m_MaxAmmo;
    }

    public bool IsFullCapacity()
    {
        return m_TotalAmmo >= m_MaxAmmo;
    }

    public void RestartElement()
    {
        m_CurrentAmmo = m_MagazineCapacity;
        m_TotalAmmo = m_MaxAmmo;
        // m_DecalsPool = new (m_DecalPrefab, m_MaxDecalInScene, GameManager.GetGameManager().m_DestroyObjects.transform);
    }
}
