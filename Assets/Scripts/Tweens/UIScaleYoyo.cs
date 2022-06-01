using DG.Tweening;
using UnityEngine;

namespace Tweens
{
    public class UIScaleYoyo : MonoBehaviour
    {
        public Ease ease;
        private void OnEnable()
        {
            transform.DOKill();
            StartTween();
        }

        private void StartTween()
        {
            transform.localScale = Vector3.one;
            transform.DOScale(Vector3.one * 1.2f, 0.6f).SetLoops(-1, LoopType.Yoyo).SetEase(ease);
        }
    }
}
