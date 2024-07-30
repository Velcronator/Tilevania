using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class LevelExit : MonoBehaviour
{
    // Time to wait before loading the next level
    [SerializeField] private float waitTime = 2f;

    // You can specify the name of the next level in the inspector
    public string nextLevelName;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            StartCoroutine(WaitAndLoadNextLevel());
        }
    }

    private IEnumerator WaitAndLoadNextLevel()
    {
        // Wait for the specified time period
        yield return new WaitForSeconds(waitTime);

        // Load the next level
        if (!string.IsNullOrEmpty(nextLevelName))
        {
            SceneManager.LoadScene(nextLevelName);
        }
        else
        {
            // If no next level is specified, load the next scene in the build settings order
            int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
            int nextSceneIndex = currentSceneIndex + 1;

            if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
            {
                SceneManager.LoadScene(nextSceneIndex);
            }
            else
            {
                SceneManager.LoadScene(0);
            }
        }
    }
}
