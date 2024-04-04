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

    [SerializeField] private GameManagerData _gameManagerData;
    [SerializeField] private EnemyStats _enemyStats;
    [SerializeField] private EnemyStats _enemyBaseStats;


    private Dictionary<string, float> _cooldowns = new Dictionary<string, float>();

    private void Awake()
    {
        Instance = this;

        if (_gameManagerData == null) { Debug.LogError("GameManagerData not found by GameManager"); }
        if (_enemyStats == null) { Debug.LogError("EnemyStats not found by GameManager"); }

        _gameManagerData.IsPaused = false;
        _enemyStats.UpdateStats(_enemyBaseStats);    
    }
    public void UpdateEnemyStats(Component sender, object data)
    {
        if (data is EnemyStats)
        {
            _enemyStats.UpdateStats((EnemyStats)data);
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

    public void RetryGame()
    {
        Debug.Log("Retry Called");
        SceneManager.LoadScene("Game");
    }
}
