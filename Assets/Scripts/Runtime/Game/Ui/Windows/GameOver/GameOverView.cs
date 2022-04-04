using System;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using CustomSelectables;
using Runtime.Services.CommonPlayerData.Data;
using SimpleUi.Abstracts;
using TMPro;
using UnityEngine;

namespace Runtime.Game.Ui.Windows.GameOver 
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class GameOverView : UiView 
    {
        [SerializeField] private TMP_Text _levelN;
        // [SerializeField] private TMP_Text _score;
        // [SerializeField] private TMP_Text _highScore;
        [SerializeField] public CustomButton Restart;

        private int _scoreValue;
        public void Show(CommonPlayerData playerData)
        {
            _levelN.text = Enum.GetName(typeof(EScene), playerData.Level)?.Replace("_", " ");
            // UpdateHighScore(playerData.HighScore);
        }

        // public void UpdateScore(ref SignalScoreUpdate signal)
        // {
        //     _scoreValue = signal.Value;
        //     _score.text = new StringBuilder("Score: ").Append(signal.Value).ToString();
        // }
        //
        // public void UpdateHighScore(int highScore)
        // {
        //     if (highScore < _scoreValue)
        //         _highScore.text = "You have a new record!";
        //     else
        //         _highScore.text = new StringBuilder("Highs Score: ").Append(highScore).ToString();
        // }

        // public ref int GetScore() => ref _scoreValue;
    }
}