using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] private PlayerHealth m_PlayerHealth;
    [SerializeField] private GameObject m_Screen;
    private bool m_GameOver; 
    private void Awake()
    {
        m_Screen.SetActive(false);
    }

    
    
    private void OnEnable()
    {
        m_PlayerHealth.OnDead += OnDead;
    }
    
    private void OnDisable()
    {
        m_PlayerHealth.OnDead -= OnDead;
    }

    private void OnDead()
    {
        m_Screen.SetActive(true);
        m_GameOver = true;
        Time.timeScale = 0f;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (m_GameOver)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                Time.timeScale = 1f;
                string currentSceneName = SceneManager.GetActiveScene().name;
                SceneManager.LoadScene(currentSceneName);
            }
        }
    }
}
