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

    private bool _isPaused = false;

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
}
