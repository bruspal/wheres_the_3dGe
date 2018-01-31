using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class followPlayer : MonoBehaviour {

    public float xLimit = 1f;
    public float yLimit = 1f;
    public float recenterSpeed = 50f;

    private Transform player;

	// Use this for initialization
	void Start () {
        player = GameObject.FindGameObjectWithTag("Player").transform;
	}
	
	// Update is called once per frame
	void Update () {
        /*
        float playerSpeed = player.transform.GetComponent<Rigidbody2D>().velocity.magnitude;
        float camX = transform.position.x;
        float camY = transform.position.y;
        if (checkX()) {
            camX = Mathf.Lerp(player.position.x, camX, recenterSpeed * Time.deltaTime);
        }
        if (checkY()) {
            camY = Mathf.Lerp(player.position.y, camY, recenterSpeed * Time.deltaTime);
        }
        transform.position = new Vector3(camX, camY, transform.position.z);

        if (playerSpeed <= 0.1) {
            StartCoroutine(recenter());
        } else {
            StopCoroutine("recenter");
        }
        */
        transform.position = new Vector3(player.position.x, player.position.y, transform.position.z);
    }

    bool checkX() {
        return Mathf.Abs(player.position.x - transform.position.x) > xLimit;
    }

    bool checkY() {
        return Mathf.Abs(player.position.y - transform.position.y) > xLimit;
    }
    /* recentrage de la camera */
    IEnumerator recenter() {
        yield return new WaitForSeconds(0.2f);
        Vector3 fromCameratoPlayer = transform.position - player.position;
        fromCameratoPlayer.z = transform.position.z;
        while (transform.position.x != player.position.x && transform.position.y != player.position.y) {
            // Debug.Log(fromCameratoPlayer);
            // transform.position += fromCameratoPlayer / 10;
            yield return new WaitForSeconds(2f);
        }
        yield break;
    }
}
