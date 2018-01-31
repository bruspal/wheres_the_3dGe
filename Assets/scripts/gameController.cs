using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class gameController : MonoBehaviour {

    public float[,] circlesDefinition = new float[,] { { 2.80f, 6.80f }, { 8.25f, 15.22f }, { 16.43f, 26.53f }, { 27.76f, 39.79f } };

    private Transform player;
    private int level = 0;
    private bool goWin = false;

	// Use this for initialization
	void Start () {
        player = GameObject.Find("player").transform;
        player.GetComponent<playerConstrainte>().setPlayground(circlesDefinition[0, 0], circlesDefinition[0, 1]);
        Physics2D.IgnoreLayerCollision(11, 11); // Ignore les collision entre les bullets
        Physics2D.IgnoreLayerCollision(11, 9);  
	}
	
	// Update is called once per frame
	void Update () {
		if (goWin) {
            StartCoroutine(coGameWin());
            goWin = false;
        }
	}

    public void levelUp() {
        if (level < 3) {
            level++;
            player.GetComponent<playerConstrainte>().levelUp(circlesDefinition[level, 0], circlesDefinition[level, 1]);
        } else {
            goWin = true;
        }
    }

    void gameWin() {
        // SceneManager.LoadScene(1);
        
    }

    public void gameOver() {
        SceneManager.LoadScene("youLose");
    }

    IEnumerator coGameWin() {
        SceneManager.LoadScene("youWin");
        yield break;

        Transform camera = GameObject.Find("Main Camera").transform;
        for (int i = 0; i < 30; i++) {
            camera.GetComponent<Camera>().orthographicSize = Mathf.Lerp(3, 30, i / 30);
            yield return new WaitForSeconds(0.2f);
        }
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene("youWin");
    }
}
