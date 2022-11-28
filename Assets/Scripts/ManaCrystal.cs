using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManaCrystal : MonoBehaviour {

    #region Fields
    public int Value { get; private set; }
    private enum Colors {
        Red, Green, Blue, Purple
    }
    [SerializeField] private Colors color;

    private Vector3 spawnLocation;

    private AudioSource audioSource;
    [SerializeField] private AudioClip collectSfx;
    #endregion

    #region UnityMethods
    private void Start() {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null ) {
            throw new System.Exception("Error! " + name + ": missing AudioSource.");
        }

        if (color == Colors.Blue) {
            Value = 1;
        }

        if (color == Colors.Green) {
            Value = 5;
        }

        if (color == Colors.Red) {
            Value = 10;
        }

        if (color == Colors.Purple) {
            Value = 20;
        }
    }

    private void Update() {

    }
    #endregion

    #region CollisionMethods
    private void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.CompareTag("Player")) {
            GameManager.AddManaCrystal(Value);
            audioSource.PlayOneShot(collectSfx);
            Destroy(gameObject);
        }
    }
    #endregion
}
