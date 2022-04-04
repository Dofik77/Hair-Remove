using System;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Runtime.UI.QuitConcentPopUp;
using Signals;
using SimpleUi.Signals;
using Zenject;

namespace Utils.UiExtensions
{
    public static partial class UiExtensions
    {
        public static void OpenQuestionPopUp(this SignalBus signalBus, string title, Action<bool> action)
        {
            signalBus.Fire(new SignalQuestionChoice(title, action));
            signalBus.OpenWindow<ConsentWindow>();
        }
        
        public static TweenerCore<float, float, FloatOptions> DOFillAmount(this SlicedFilledImage target, float endValue, float duration)
        {
            if (endValue > 1) endValue = 1;
            else if (endValue < 0) endValue = 0;
            var t = DOTween.To(() => target.fillAmount, x => target.fillAmount = x, endValue, duration);
            t.SetTarget(target);
            return t;
        }
    }
}