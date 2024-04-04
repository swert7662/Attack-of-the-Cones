using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private GameManagerData _gameManagerData;
    [SerializeField] private GameObject _mainMenuCanvasGO;

    [SerializeField] private GameObject _menuInitialSelectionGO;
    //[SerializeField] private GameObject _settingsMenuCanvasGO;

    //private bool isPaused;

    private void Start()
    {
        _mainMenuCanvasGO.SetActive(false);
        //_settingsMenuCanvasGO.SetActive(false);
    }

    private void Update()
    {
        if (InputManager.Instance.MenuOpenCloseInput)
        {
            if (!_gameManagerData.IsPaused)
            {
                OpenMainMenu();
            }
            else
            {
                CloseAllMenus();
            }
        }
    }

    public void Pause()
    {
        _gameManagerData.IsPaused = true;
        InputManager.Instance.SwitchToUIControls();
        Time.timeScale = 0;        
    }

    public void Unpause()
    {
        _gameManagerData.IsPaused = false;
        InputManager.Instance.SwitchToGameplayControls();
        Time.timeScale = 1;        
    }

    private void OpenMainMenu()
    {
        Pause();
        _mainMenuCanvasGO.SetActive(true);
        //_settingsMenuCanvasGO.SetActive(false);

        SetInitialSelection(_menuInitialSelectionGO);
    }

    private void SetInitialSelection(GameObject initialSelectionGO)
    {
        EventSystem.current.SetSelectedGameObject(initialSelectionGO);
    }

    private void OpenSettingsMenu()
    {
        _mainMenuCanvasGO.SetActive(false);
        //_settingsMenuCanvasGO.SetActive(true);
    }

    public void CloseAllMenus()
    {
        Unpause();
        _mainMenuCanvasGO.SetActive(false);
        //_settingsMenuCanvasGO.SetActive(false);
    }
}
