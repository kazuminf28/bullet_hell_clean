using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleButtonController : MonoBehaviour
{
    // Startボタン用
    public void OnStartButton()
    {
        SceneManager.LoadScene("GameScene");
    }

    // Quitボタン用
    public void OnQuitButton()
    {
        Application.Quit();

        // エディタ上で確認したい場合
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
