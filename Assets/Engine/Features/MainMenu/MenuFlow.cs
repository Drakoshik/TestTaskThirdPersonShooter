using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace TPS.Engine.Features.MainMenu
{
    internal sealed class MenuFlow : MonoBehaviour
    {
        [SerializeField] private Button _startButton;
        [SerializeField] private Button _exitButton;

        private void Start()
        {
            _startButton.onClick.AddListener(StartGame);
            _exitButton.onClick.AddListener(ExitGame);
        }

        private void StartGame()
        {
            SceneManager.LoadScene("GameplayScene");
        }

        private void ExitGame()
        {
            Application.Quit();
        }
    }
}
