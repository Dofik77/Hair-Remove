using System;
using System.Diagnostics.CodeAnalysis;
using CustomSelectables;
using DG.Tweening;
using SimpleUi.Abstracts;
using TMPro;
using UnityEngine;

namespace Runtime.Game.Ui.Windows.LevelComplete 
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class LevelCompleteView : UiView 
    {
        [SerializeField] private RectTransform _container;
        [SerializeField] private TMP_Text _levelN;
        
        [SerializeField] public CustomButton NextLevel;
        
        [SerializeField] public TMP_Text Currency;
        [SerializeField] public TMP_Text RewardValue;
        [SerializeField] private RectTransform _currencyIcon;
        [SerializeField] private RectTransform _rewardIcon;
        
        
        public void Show(EScene currentLevel)
        {
            gameObject.SetActive(true);
            _levelN.text = Enum.GetName(typeof(EScene), currentLevel)?.Replace("_", " ");
        }

        public void GetReward(int result)
        {
            var newIcon = Instantiate(_currencyIcon, _rewardIcon.TransformPoint(_rewardIcon.rect.center), Quaternion.identity,  _container);
            newIcon.DOMove(_currencyIcon.TransformPoint(_currencyIcon.rect.center), 0.5f).SetEase(Ease.Linear).OnComplete(() =>
            {
                Currency.text = result.ToString();
                newIcon.gameObject.SetActive(false);
            });
        }
        
        
        public void UpdateCurrency(ref int value)
        {
            Currency.text = value.ToString();
        }
    }
}