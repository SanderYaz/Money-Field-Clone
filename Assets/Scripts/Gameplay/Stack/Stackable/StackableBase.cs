using System;
using Data.Object;
using DG.Tweening;
using Gameplay.Stack.Interfaces;
using Managers;
using UnityEngine;


namespace Gameplay.Stack.Stackable
{
    public class StackableBase : MonoBehaviour, IStackable
    {
        private const float ScaleMultiplier = 0.5f;
        private const float ScaleDuration = 1f;

        private bool _isStacked;

        private Tween _scaleTween;

        public Material[] halfTransparentMats;
        public Material[] opaqueMats;

        public Renderer rend;
        public void OnStacked()
        {
            _scaleTween?.Kill(true);
            _scaleTween = transform.DOPunchScale(Vector3.one * ScaleMultiplier, ScaleDuration, 5, 1);
            gameObject.layer = LayerMask.NameToLayer("Stacked");
            _isStacked = true;
        }

        public void OnUnstacked()
        {
            var prefabInfo = GetComponent<OPrefabInfo>();
            GameManager.Instance.PoolManager.RePoolObject(prefabInfo);
            _isStacked = false;
            gameObject.layer = LayerMask.NameToLayer("Stackable");
        }

        private void Update()
        {
            if (!_isStacked) return;

            // var playerForward = GameManager.Instance.currentPlayer.transform.forward;
            //
            // Debug.Log(playerForward);
            //
            // rend.materials = playerForward.x is < 0.95f and > -0.95f && playerForward.z > 0.2f  ? halfTransparentMats : opaqueMats;

        }
    }
}