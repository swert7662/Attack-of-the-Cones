using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PausePanel : MonoBehaviour
{
    [SerializeField] private PlayerStats player;
    [SerializeField] private GameManagerData _gameManagerData;
    [SerializeField] private GameObject _pauseScreen;
    [SerializeField] private GameEvent pausedEvent;
    //private MainControls mainControls;

    private void Awake()
    {
        //mainControls = new MainControls();

        //mainControls.UI.Pause.performed += ctx => TogglePause();
    }

    private void OnEnable()
    {
        //mainControls.Enable();
    }

    private void OnDisable()
    {
        //mainControls.Disable();
    }
    public void TogglePause()
    {
        _gameManagerData.IsPaused = !_gameManagerData.IsPaused;
        Time.timeScale = _gameManagerData.IsPaused ? 0f : 1f;
        _pauseScreen.SetActive(_gameManagerData.IsPaused); 

        if (_gameManagerData.IsPaused)
        {
            // Optional: Select a default button when the pause menu is opened
            // This requires having an EventSystem in your scene and a selectable button on your pause menu
            // Example:
            // yourDefaultButton.Select();
        }

        // Raise the paused event if needed
        pausedEvent?.Raise();
    }

    /*
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
    */
}
