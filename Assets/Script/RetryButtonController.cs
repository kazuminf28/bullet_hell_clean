using UnityEngine;
using UnityEngine.SceneManagement;

public class RetryButtonController : MonoBehaviour
{
    // Retryボタン用
    public void OnRetryButton()
    {
        SceneManager.LoadScene("GameScene");
    }

    // Returnボタン用
    public void OnReturnButton()
    {
        SceneManager.LoadScene("TitleScene");
    }
}
