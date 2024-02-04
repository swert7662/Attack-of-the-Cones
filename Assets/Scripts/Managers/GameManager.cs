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

    private Dictionary<string, float> _cooldowns = new Dictionary<string, float>();

    private void Awake()
    {
        Instance = this;
        if (_gameManagerData == null) { Debug.LogError("GameManagerData not found by GameManager"); }
        _gameManagerData.IsPaused = false;
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

    //Cool downs should be replaced with SO and updated in WeaponPowerUpManager
    public bool IsCooldownElapsed(string abilityName, float cooldownDuration)
    {
        // This if checks if the abilityName is not in the dictionary or if the cooldown has elapsed
        // then it will add the abilityName to the dictionary and return true
        if (!_cooldowns.ContainsKey(abilityName) || Time.time >= _cooldowns[abilityName])
        {
            //Debug.Log($"{abilityName} is ready to go!");
            _cooldowns[abilityName] = Time.time + cooldownDuration;
            return true;
        }
        //Debug.Log($"{abilityName} is on cooldown");
        return false;
    }
}
