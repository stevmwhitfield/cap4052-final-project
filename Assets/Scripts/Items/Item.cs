using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour {

    #region Fields
    private int hoverDirection;

    private float minY;
    private float maxY;
    private float t;
    #endregion

    #region UnityMethods
    private void Start() {
        maxY = transform.position.y + 0.1f;
        minY = transform.position.y;
        hoverDirection = 1;
        t = 0f;
    }

    private void FixedUpdate() {
        if (t >= 1) {
            hoverDirection = -1;
        }
        if (t <= 0) {
            hoverDirection = 1;
        }
        t += Time.fixedDeltaTime * hoverDirection;
        transform.position = Vector3.Lerp(new Vector3(transform.position.x, minY, transform.position.z), new Vector3(transform.position.x, maxY, transform.position.z), t);
        transform.Rotate(0f, 2f, 0f);
    }
    #endregion

}
