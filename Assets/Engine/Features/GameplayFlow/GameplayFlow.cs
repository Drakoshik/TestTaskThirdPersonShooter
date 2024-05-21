using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TPS.Engine.Features.GameplayFlow
{
    internal sealed class GameplayFlow : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _losesText;
        [SerializeField] private TextMeshProUGUI _winsText;
        [SerializeField] private TextMeshProUGUI _endText;
        [SerializeField] private GameObject _endPanel;

        [SerializeField] private int _loses;
        [SerializeField] private int _wins;

        private void Awake()
        {
            Cursor.visible = false;
            _endPanel.SetActive(false);
            _endText.text = "";
            Time.timeScale = 1;
            if (PlayerPrefs.HasKey("Wins"))
            {
                _wins = PlayerPrefs.GetInt("Wins");
                _winsText.text = "Wins: " + _wins;
            }
            else
            {
                _wins = 0;
                _winsText.text = "Wins: 0";
            }

            if (PlayerPrefs.HasKey("Loses"))
            {
                _loses = PlayerPrefs.GetInt("Loses");
                _losesText.text = "Loses: " + _loses;
            }
            else
            {
                _loses = 0;
                _losesText.text = "Loses: 0";
            }

        }

        public void GameOver()
        {
            _loses++;
            PlayerPrefs.SetInt("Loses", _loses);
            _endPanel.SetActive(true);
            _endText.text = "You lose!";
            WaitForRestart(destroyCancellationToken).Forget();
        }

        public void Win()
        {
            _wins++;
            PlayerPrefs.SetInt("Wins", _wins);
            _endPanel.SetActive(true);
            _endText.text = "You win!";
            WaitForRestart(destroyCancellationToken).Forget();
        }

        private async UniTaskVoid WaitForRestart(CancellationToken cancellation = default)
        {
            while (cancellation.IsCancellationRequested is not true)
            {
                Time.timeScale = 0.5f;
                await UniTask.Delay(TimeSpan.FromSeconds(1), cancellationToken: cancellation);
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }
    }
}
