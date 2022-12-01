using System.Collections;
using UnityEngine;

public class ManaCrystal : MonoBehaviour {

    #region Fields
    public int Value { get; private set; }
    private enum Colors {
        Red, Green, Blue, Purple
    }
    [SerializeField] private Colors color;

    private Vector3 spawnLocation; // might delete

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
    #endregion

    #region CollisionMethods
    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Player")) {
            if (GameManager.ActiveLevel != null) {
                GameManager.ActiveLevel.UpdateCrystals(Value);
            }
            else {
                Debug.Log("TestLevel: collected " + color.ToString() + "(" + Value + ") crystal.");
            }
            
            if (collectSfx != null) {
                audioSource.PlayOneShot(collectSfx);
            }
            else {
                Debug.LogWarning("Warning! " + name + ": missing collectSfx.");
            }

            StartCoroutine(DelayDestroy());
        }
    }

    private IEnumerator DelayDestroy() {
        yield return new WaitForSeconds(0.05f);
        Destroy(gameObject);
    }
    #endregion
}
