using System;
using System.Diagnostics.CodeAnalysis;
using CustomSelectables;
using Runtime.Services.CommonPlayerData.Data;
using SimpleUi.Abstracts;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime.Game.Ui.Windows.InGameButtons
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    [SuppressMessage("ReSharper", "RedundantDefaultMemberInitializer")]
    public class InGameButtonsView : UiView
    {
        [SerializeField] public Image JoystickButton;
        [SerializeField] public Image JoystickOrigin;
        [SerializeField] private TMP_Text _levelN;
        [SerializeField] public CustomButton InGameMenuButton;
        [SerializeField] public CustomButton RestartGameButton;

        // [SerializeField] private TMP_Text _score;
        // [SerializeField] private TMP_Text _highScore;
        // [SerializeField] private TMP_Text _lifeCount;

        // [SerializeField] public RectTransform Vignette;
        // [SerializeField] public float _vignetteDuration = 0.3f;
        // [SerializeField] public int _maxHp = 100;
        // [SerializeField] private ProgressBar _hpBar;
        // [SerializeField] private RectTransform _hpBarRect;
        
        // [SerializeField] private TMP_Text _currency;
        
        

        // private RectTransform _joystickButtonRT;
        // private RectTransform _joystickOriginRT;
        // private int _lastHp;
        // private Color _color1 = new Color(0.347f, 0.964f, 0.185f, 1f);
        // private Color _color2 = new Color(0.965f, 0.922f, 0.185f, 1f);
        // private Color _color3 = new Color(0.965f, 0.185f, 0.34f, 1f);
        // private Color _currentColor;

        public void Show(CommonPlayerData playerData)
        {
            _levelN.text = Enum.GetName(typeof(EScene), playerData.Level)?.Replace("_", " ");
            // _joystickButtonRT = JoystickButton.GetComponent<RectTransform>();
            // _joystickOriginRT = JoystickOrigin.GetComponent<RectTransform>();
            // _lastHp = _maxHp;
            // SetHightScore(playerData.HighScore);
            // UpdateCurrency(ref playerData.Money);
        }

        // public void UpdateJoystick(ref SignalJoystickUpdate signal)
        // {
        //     JoystickButton.gameObject.SetActive(signal.IsPressed);
        //     JoystickOrigin.gameObject.SetActive(signal.IsPressed);
        //     if (signal.IsPressed)
        //     {
        //         _joystickButtonRT.anchoredPosition = signal.ButtonPosition;
        //         _joystickOriginRT.anchoredPosition = signal.OriginPosition;
        //     }
        // } //touc pad on view 

        // public void UpdateCurrency(ref int value)
        // {
        //     _currency.text = value.ToString();
        // }
        
        // public void UpdateLifeCount(ref SignalLifeCountUpdate signal)
        // {
        //     _lifeCount.text = new StringBuilder("x").Append(signal.Count).ToString();
        // }

        // public void SetHightScore(int value) => _highScore.text = value.ToString();
        
        // public void UpdateScore(ref SignalScoreUpdate signal) => _score.text = signal.Value.ToString();
        

        // public void UpdateHpBar(ref SignalHpBarUpdate signal)
        // {
        //     if (signal.Hp < _lastHp)
        //     {
        //         Vignette.DOKill();
        //         Vignette.gameObject.SetActive(true);
        //         Vignette.DOScale(Vector3.one * 3, _vignetteDuration).SetEase(Ease.Linear);
        //         Vignette.DOScale(Vector3.one * 15, _vignetteDuration).SetEase(Ease.Linear).SetDelay(_vignetteDuration)
        //             .OnComplete(() => Vignette.gameObject.SetActive(false));
        //     }
        //
        //     if (signal.Hp > 70)
        //         _currentColor = _color1;
        //     else if (signal.Hp < 30)
        //         _currentColor = _color3;
        //     else
        //         _currentColor = _color2;
        //
        //     _hpBar.Repaint(signal.Hp.Remap01(_maxHp), _currentColor);
        //     _hpBarRect.anchoredPosition = signal.Position;
        //     _lastHp = signal.Hp;
        // }
    }
}