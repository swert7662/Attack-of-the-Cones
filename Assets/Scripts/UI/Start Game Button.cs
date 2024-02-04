using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartGameButton : MonoBehaviour
{
    void Start() => GetComponent<Button>().onClick.AddListener(StartGame);

    public void StartGame() => SceneManager.LoadScene("Game");
}
