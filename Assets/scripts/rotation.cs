using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotation : MonoBehaviour {
    public float minSpeed;
    public float maxSpeed;
    public float deltaRadius;
    public float deltaSpeed;


    private float angle;
    private float actualSpeed;

	// Use this for initialization
	void Start () {
        angle = Random.Range(0f, 360f);
        transform.RotateAround(transform.parent.position, Vector3.back, angle);
        actualSpeed = Random.Range(minSpeed, maxSpeed);
    }

    // Update is called once per frame
    void Update () {
        angle = ((angle + actualSpeed) % 360) * Time.deltaTime;
        // transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, deltaRadius);
        transform.RotateAround(transform.parent.position, Vector3.back, angle);
	}


}
