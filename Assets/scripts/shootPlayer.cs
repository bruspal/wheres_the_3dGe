using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shootPlayer : MonoBehaviour {

    public Transform player;
    public Transform bullet;
    public float bulletSpeed = 10f;

	// Use this for initialization
	void Start () {
		
	}

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.tag == "Player") {
            Transform bulletInstance;
            Vector3 playerDirection;

            playerDirection = (player.transform.position - transform.position);
            playerDirection.Normalize();

            bulletInstance = Instantiate(bullet, transform.position, Quaternion.identity);
            bulletInstance.GetComponent<Rigidbody2D>().AddForce(playerDirection * bulletSpeed, ForceMode2D.Impulse);
            GetComponent<AudioSource>().Play();

        }
    }
}
