using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PausePanel : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private GameManagerData _gameManagerData;
    [SerializeField] private GameObject _pauseScreen;
    [SerializeField] private GameEvent pausedEvent;

    private void Update()
    {
        if (player.IsAlive)
        {
            if (Input.GetKeyDown(KeyCode.Escape)) { pausedEvent.Raise(); }
        }
    }

    public void TogglePause()
    {
        _gameManagerData.IsPaused = !_gameManagerData.IsPaused;
        Time.timeScale = _gameManagerData.IsPaused ? 0f : 1f;
        _pauseScreen.gameObject.SetActive(_gameManagerData.IsPaused);
    }
}
