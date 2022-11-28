using UnityEngine;

public class DontDestroyOnLoad : MonoBehaviour {

    public static DontDestroyOnLoad instance;

    private void Awake() {
        instance = this;
        DontDestroyOnLoad(gameObject);
    }
}
