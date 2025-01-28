using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PSoft
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] private string levelToLoadName = "S_Dev_Basic"; // ToDo: Temporary. Set to proper level when implemented.

        public void OnStartButtonPressed()
        {
            // SceneManager.LoadScene(1); ToDo: Could also just use build index.
            SceneManager.LoadScene(levelToLoadName); // ToDo: I believe I need to add to build settings to load it.
        }

        public void OnQuitButtonPressed()
        {
            Application.Quit();

            // Application.Quit() only works in build. This is to quit editor play.
            if (Application.isEditor)
                EditorApplication.isPlaying = false;
        }
        
    }
}
