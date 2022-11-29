using UnityEngine;

public class Artifact : MonoBehaviour
{
    #region Fields
    private AudioSource audioSource;
    [SerializeField] private AudioClip collectSfx;
    #endregion

    #region UnityMethods
    private void Awake() {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null) {
            throw new System.Exception("Error! " + name + ": missing AudioSource.");
        }
    }
    #endregion

    #region CollisionMethods
    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Player")) {
            if (GameManager.ActiveLevel != null) {
                GameManager.ActiveLevel.UpdateArtifacts();
            }
            else {
                Debug.Log("TestLevel: collected artifact.");
            }
            audioSource.PlayOneShot(collectSfx);
            Destroy(gameObject);
        }
    }
    #endregion
}
