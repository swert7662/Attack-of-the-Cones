using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public GameObject _pauseScreen;
    public Timer _timer;
    public GameObject _playerGameObject;
    public Transform _playerTransform;
    public LayerMask _enemyLayer;

    private Dictionary<string, float> _cooldowns = new Dictionary<string, float>();
    private bool _isPaused = false;
    private XPBar _xpBar;

    private EntityManager entityManager;
    private Entity entity;

    void Awake()
    {
        // Singleton pattern
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
    }

    private void Start()
    {
        _playerGameObject = GameObject.FindGameObjectWithTag("Player");
        _playerTransform = _playerGameObject.transform;
        _enemyLayer = LayerMask.GetMask("Enemy");
        _xpBar = FindObjectOfType<XPBar>();
    }

    private void Update() 
    { 
        if (Input.GetKeyDown(KeyCode.Escape)) { TogglePause(); }

        if (entity != Entity.Null && entityManager != null)
        {
            entityManager.SetComponentData(entity, new TargetPosition { Position = _playerTransform.position });
        }
    }

    public void StartGame()
    {
        Debug.Log("Start Game Called");
        SceneManager.LoadScene("Game");
    }

    public void QuitGame()
    {         
        Debug.Log("Quit Called");
        Application.Quit();
    }

    public void TogglePause()
    {
        Debug.Log("Toggle Pause Called");
        _isPaused = !_isPaused;
        _timer.ToggleTimer(_isPaused);
        Time.timeScale = _isPaused ? 0f : 1f;
        _pauseScreen.gameObject.SetActive(_isPaused);
    }

    public void AddXP(int xp)
    {
        if (_xpBar != null)
        {
            _xpBar.UpdateXP(xp);
        }
    }

    public bool IsCooldownElapsed(string abilityName, float cooldownDuration)
    {
        // This if checks if the abilityName is not in the dictionary or if the cooldown has elapsed
        // then it will add the abilityName to the dictionary and return true
        if (!_cooldowns.ContainsKey(abilityName) || Time.time >= _cooldowns[abilityName])
        {
            Debug.Log($"{abilityName} is ready to go!");
            _cooldowns[abilityName] = Time.time + cooldownDuration;
            return true;
        }
        Debug.Log($"{abilityName} is on cooldown");
        return false;
    }
}
