using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemFloat : MonoBehaviour {

    private float minY;
    private float maxY;
    private float t;
    private int direction;

    // Start is called before the first frame update
    void Start() {
        maxY = transform.position.y + 0.1f;
        minY = transform.position.y;
        direction = 1;
        t = 0f;
    }

    // Update is called once per frame
    void FixedUpdate() {
        if (t >= 1) {
            direction = -1;
        }
        if (t <= 0) {
            direction = 1;
        }
        t += Time.fixedDeltaTime * direction;
        transform.position = Vector3.Lerp(new Vector3(0f, minY, 0f), new Vector3(0f, maxY, 0f), t);
        transform.Rotate(0f, 0f, 2f);
    }

}
