using DG.Tweening;
using UnityEngine;
using Utils.UiExtensions;

namespace Runtime.Game.Ui.Windows.InGameButtons
{
    public class ProgressBar : MonoBehaviour
    {
        [System.Serializable]
        private struct Stage
        {
            public Color color;
            [Range(0, 1)]
            public float ratio;

            public Stage(Color color, float ratio)
            {
                this.color = color;
                this.ratio = ratio;
            }
        }

        [SerializeField] private SlicedFilledImage progress;
        [SerializeField] private Stage[] stages;

        public void SetFillAmount(float ratio)
        {
            progress.fillAmount = ratio;
            progress.color = stages.Find(x => ratio <= x.ratio).color;
        }

        public void Repaint(float ratio)
        {
            progress.DOKill();
            progress.DOFillAmount(ratio, 0.1f);
            progress.DOColor(stages.Find(x => ratio <= x.ratio).color, 0.1f);
        }
    }
}