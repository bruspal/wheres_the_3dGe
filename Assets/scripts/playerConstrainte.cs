using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class playerConstrainte : MonoBehaviour {

    public Transform playfieldLocation;
    public Text energyText;
    public Text networkText;
    public gameController gc;

    public float innerRadius = 0;
    public float outerRadius = 0;

    private float energy = 100f;
    private int netStrength = 0;
    private int safeFrames = 2; // nombre de frame d'invulnaribilité
    private bool safe = false; // joueur invulnérable ?
    private bool safeStarted = false;
    private bool gameIsOver = false; // jeu fini que le joueur gagne ou non

	// Use this for initialization
	void Start () {
        energyText.text = Mathf.Round(energy).ToString();
        // networkText.text = netStrength.ToString();
        // setPlayground(3f, 6.8f);
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        float xAxis = Input.GetAxis("Horizontal");
        float yAxis = Input.GetAxis("Vertical");
        GetComponent<Rigidbody2D>().AddForce(new Vector2(xAxis, yAxis), ForceMode2D.Impulse);
        playerConstraint(playfieldLocation.position, innerRadius, outerRadius);
        if (safe && ! safeStarted) { // On entre en mode safe (frame invulnaribilité)
            StartCoroutine(safePlayerMode());
        }
        if (updateEnergy(-1 * Time.deltaTime) <= 0) {
            gc.GetComponent<gameController>().gameOver();
        }
        
        // gestion anim
        Vector3 playerVelocity = GetComponent<Rigidbody2D>().velocity;
        Debug.Log(playerVelocity);

        if (playerVelocity.magnitude <= 0.3) {
            GetComponent<Animator>().SetBool("goRight", false);
            GetComponent<Animator>().SetBool("goLeft", false);
            GetComponent<Animator>().SetBool("goUp", false);
            GetComponent<Animator>().SetBool("goDown", false);
        } else {
            if (playerVelocity.x > 1) GetComponent<Animator>().SetBool("goRight", true);
                else GetComponent<Animator>().SetBool("goRight", false);
            if (playerVelocity.x < -1) GetComponent<Animator>().SetBool("goLeft", true);
                else GetComponent<Animator>().SetBool("goLeft", false);
            if (playerVelocity.y > 1) {
                GetComponent<Animator>().SetBool("goUp", true);
            } else {
                Debug.Log("Go Up");
                GetComponent<Animator>().SetBool("goUp", false);
            }
            if (playerVelocity.y < -1) GetComponent<Animator>().SetBool("goDown", true);
                else GetComponent<Animator>().SetBool("goDown", false);
        }
    }

    /*
     * Limite les deplacements dans un anneau compris entre innerRadius et outterRadius positionné a conterPosition
     */
    private void playerConstraint(Vector3 centerPosition, float innerRadius, float outerRadius) {
        float outterDistance = Vector3.Distance(transform.position, centerPosition);
        float innerDistance = Vector3.Distance(transform.position, centerPosition);

        if (outterDistance > outerRadius) {
            Vector3 fromOriginToObject = transform.position - centerPosition;
            fromOriginToObject *= outerRadius / outterDistance;
            transform.position = centerPosition + fromOriginToObject;
        }
        if (innerDistance < innerRadius) {
            Vector3 fromOriginToObject = transform.position - centerPosition;
            fromOriginToObject *= innerRadius / innerDistance;
            transform.position = centerPosition + fromOriginToObject;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        // collision 'enemy'
        if ( ! safe) {
            if (collision.gameObject.tag == "wave") {
                updateEnergy(-10);
                GetComponent<Rigidbody2D>().AddForce(collision.transform.GetComponent<Rigidbody2D>().velocity * 20, ForceMode2D.Impulse);
                safe = true;
            }
            if (collision.gameObject.tag == "bullet") {
                updateEnergy(-10);
                GetComponent<Rigidbody2D>().AddForce(collision.transform.GetComponent<Rigidbody2D>().velocity * 8, ForceMode2D.Impulse);
                Object.Destroy(collision.gameObject);
                safe = true;
            }

        }
    }

    public float updateEnergy(float value) {
        energy = Mathf.Min(energy += value, 100);
        energyText.text = Mathf.Round(energy).ToString();
        return energy;
    }

    public void setPlayground(float innerRadius, float outerRadius) {
        this.innerRadius = innerRadius;
        this.outerRadius = outerRadius;
        transform.position = new Vector3(0, 1.0f + playfieldLocation.position.x - transform.position.x, transform.position.z);
    }

    public void levelUp(float innerRadius, float outerRadius) {
        energy = Mathf.Min(energy * 110 / 100, 100);
        setPlayground(innerRadius, outerRadius);
    }

    public void gameOver() {
        gameIsOver = true;
    }

    IEnumerator safePlayerMode() {
        Color spriteAlpha;
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        safeStarted = true;
        for (int i = 0; i < safeFrames; i++) {
            float actualAlpha = 1;
            while (actualAlpha >= 0) {
                sr.color = new Color(1f, 1f, 1f, actualAlpha -= 0.1f);
                yield return new WaitForEndOfFrame();
            }
            actualAlpha = 0;
            while (actualAlpha <= 1) {
                sr.color = new Color(1f, 1f, 1f, actualAlpha += 0.1f);
                yield return new WaitForEndOfFrame();
            }
        }
        safe = false;
        safeStarted = false;
        yield break;
    }

}
