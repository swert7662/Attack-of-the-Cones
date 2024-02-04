using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuitGameButton : MonoBehaviour
{
    void Start() => GetComponent<Button>().onClick.AddListener(QuitGame);
    public void QuitGame() => Application.Quit();
}
