using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PausePanel : MonoBehaviour
{
    [SerializeField] private PlayerStats player;
    [SerializeField] private GameManagerData _gameManagerData;
    [SerializeField] private GameObject _pauseScreen;
    [SerializeField] private GameEvent pausedEvent;

    private void Update()
    {
        if ((player.IsAlive && !_gameManagerData.IsPaused) || (_gameManagerData.IsPaused && _pauseScreen.gameObject.activeInHierarchy))
        {
            if (Input.GetKeyDown(KeyCode.Escape)) { pausedEvent.Raise(); }
        }
    }

    public void TogglePause(bool PauseWithPauseScreen)
    {
        _gameManagerData.IsPaused = !_gameManagerData.IsPaused;
        Time.timeScale = _gameManagerData.IsPaused ? 0f : 1f;
        if (PauseWithPauseScreen) { _pauseScreen.gameObject.SetActive(_gameManagerData.IsPaused); }
    }
}
