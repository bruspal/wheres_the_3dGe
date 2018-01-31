using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class batteryController : MonoBehaviour {

    public Transform toto;

    private Transform player;

    void Start() {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }
    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.tag == "Player") {
            player.GetComponent<playerConstrainte>().updateEnergy(10);
            Object.Destroy(gameObject);
        }
    }
}
