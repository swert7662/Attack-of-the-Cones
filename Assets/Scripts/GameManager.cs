using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public Canvas _canvasGameObject;
    public GameObject _playerGameObject;
    public Transform _playerTransform;
    public LayerMask _enemyLayer;

    private Dictionary<string, float> _cooldowns = new Dictionary<string, float>();
    private bool _isPaused = false;

    void Awake()
    {
        _playerGameObject = GameObject.FindGameObjectWithTag("Player");
        _playerTransform = _playerGameObject.transform;
        _enemyLayer = LayerMask.GetMask("Enemy");
        
        // Singleton pattern
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Update() { if (Input.GetKeyDown(KeyCode.Escape)) { TogglePause(); } }

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
        Time.timeScale = _isPaused ? 0f : 1f;
        _canvasGameObject.gameObject.SetActive(_isPaused);
    }

    public bool IsCooldownElapsed(string abilityName, float cooldownDuration)
    {
        // This if checks if the abilityName is not in the dictionary or if the cooldown has elapsed
        // then it will add the abilityName to the dictionary and return true
        if (!_cooldowns.ContainsKey(abilityName) || Time.time >= _cooldowns[abilityName])
        {
            _cooldowns[abilityName] = Time.time + cooldownDuration;
            return true;
        }

        return false;
    }
}
