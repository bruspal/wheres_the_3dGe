using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class netCollider : MonoBehaviour {

    public Transform waveSignal;
    public Color BeepColor;

    private GameObject player;
    private gameController gc;
    private bool disabled = false;
    private bool playerOnTrigger = false;
    private bool colliderLocked = false; // tant que true, on ne peux plus utiliser le collider
    private bool coroutineSemaphore = false;

    private AudioSource beepAudio;
    private AudioSource loadAudio;
    private AudioSource cancelAudio;

    // Start est appelé juste avant qu'une méthode Update soit appelée pour la première fois
    private void Start() {
        gc = (gameController)GameObject.Find("gameController").GetComponent<gameController>();
        transform.RotateAround(transform.parent.position, Vector3.back, Random.Range(0f, 360f));
        AudioSource[] temp = GetComponents<AudioSource>();
        beepAudio = temp[0];
        loadAudio = temp[1];
        cancelAudio = temp[2];
    }



    void OnTriggerEnter2D(Collider2D collision) {
        if (disabled) return;
        if (collision.gameObject.tag == "wave") {
            waveSignal.GetComponent<SpriteRenderer>().color = new Color(1f, 0f, 0f, 1f);
            beepAudio.Play();
        }
        if (collision.gameObject.tag == "Player") {
            playerOnTrigger = true;
            if ( ! coroutineSemaphore) {
                loadAudio.Play();
                StartCoroutine(activateArc());
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        waveSignal.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
        if (collision.gameObject.tag == "Player") {
            playerOnTrigger = false;
            if ( ! disabled && coroutineSemaphore) {
                loadAudio.Stop();
                cancelAudio.Play();
            }
        }
    }

    IEnumerator activateArc() {
        coroutineSemaphore = true;
        SpriteRenderer sprite = transform.GetComponentInChildren<SpriteRenderer>();
        float inc = 0.5f;
        while (inc > 0 && inc < 1) {
            if (playerOnTrigger) {
                inc += 0.05f;
            } else {
                inc -= 0.30f;
            }
            sprite.color = new Color(1f, 1f, 1f, inc);
            yield return new WaitForSeconds(0.2f);
        }
        if (inc >= 1) {
            gc.levelUp();
            transform.RotateAround(transform.parent.position, Vector3.back, 0);
            disabled = true;
        } else {
            Transform player = GameObject.FindGameObjectWithTag("Player").transform;
            // bloquer le trigger pendant qq secondes ek colliderLocked
            for (int i = 20; i > 0; i--) {
                sprite.color = new Color(1f, 0f, 0f, i * 0.05f);
                player.GetComponent<playerConstrainte>().updateEnergy(-0.5f);
                yield return new WaitForSeconds(0.2f);
            }
            sprite.color = new Color(1f, 1f, 1f, 0f);
        }
        coroutineSemaphore = false;
    }
}
