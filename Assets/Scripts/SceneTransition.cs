using UnityEngine;
// Required namespace for changing scenes
using UnityEngine.SceneManagement; 

public class SceneTransition : MonoBehaviour
{
    [SerializeField] private string sceneToLoad;

    // Use Collider2D if you are making a 2D game
    private void OnTriggerEnter(Collider other) 
    {
        // Check if the object entering the trigger has the "Player" tag
        if (other.CompareTag("Player"))
        {
            SceneManager.LoadScene(sceneToLoad);
        }
    }
}
