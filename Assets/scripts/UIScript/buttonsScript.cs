using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class buttonsScript : MonoBehaviour {

    public void startGame() {
        SceneManager.LoadScene("Main");
    }

    public void quitGame() {
        Application.Quit();
    }

    public void goCredit() {
        SceneManager.LoadScene("credit");
    }

    public void goStart() {
        SceneManager.LoadScene("start");
    }

    public void goHelp() {
        SceneManager.LoadScene("help");
    }

}
