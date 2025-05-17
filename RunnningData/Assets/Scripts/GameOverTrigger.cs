using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverTrigger : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Obstacle"))
        {
            SceneManager.LoadScene("GameOver");
        }
    }
}
