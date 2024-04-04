using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance;

    [SerializeField] private GameManagerData _gameManagerData;

    public bool MenuOpenCloseInput { get; private set; }

    private PlayerInput _playerInput;
    private InputAction _menuOpenCloseAction;
    private GameObject lastSelectedObject = null;
    private bool lastInputWasMouse = false;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        _playerInput = GetComponent<PlayerInput>();
        GetActions();
    }

    private void GetActions()
    {
        _menuOpenCloseAction = _playerInput.actions["MenuOpenClose"];
    }

    private void Update()
    {
        MenuOpenCloseInput = _menuOpenCloseAction.WasPressedThisFrame();
        CheckMouseInput();
    }

    private void CheckMouseInput()
    {
        // Check for mouse input
        if (Mouse.current.delta.ReadValue() != Vector2.zero || Mouse.current.leftButton.wasPressedThisFrame)
        {
            if (!lastInputWasMouse)
            {
                lastSelectedObject = EventSystem.current.currentSelectedGameObject;
                EventSystem.current.SetSelectedGameObject(null);
                lastInputWasMouse = true;
            }
        }

        // Check for gamepad or keyboard input
        // Here we use the leftStick's delta as a proxy for gamepad input and any key press for keyboard
        else if ((Gamepad.current != null && Gamepad.current.leftStick.ReadValue().magnitude > 0.1f) 
                    || Keyboard.current.anyKey.wasPressedThisFrame)
        {
            if (lastInputWasMouse)
            {
                // Only reset the selected object if there's a last selected object to return to
                if (lastSelectedObject != null)
                {
                    EventSystem.current.SetSelectedGameObject(lastSelectedObject);
                }
                lastInputWasMouse = false;
            }
        }
    }

    public void SwitchToGameplayControls()
    {
        Debug.Log("Switching to gameplay controls");
        _playerInput.SwitchCurrentActionMap("Player Controls");
        GetActions();
    }

    public void SwitchToUIControls()
    {
        Debug.Log("Switching to UI controls");
        _playerInput.SwitchCurrentActionMap("UI");
        GetActions();
    }
}
