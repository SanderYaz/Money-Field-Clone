using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Gameplay.Player;
using Gameplay.Stack.Stackable;
using Interfaces;
using Managers;
using NaughtyAttributes;
using UnityEngine;
using Util;

namespace Gameplay.Stack
{
    public class Stacker : MonoBehaviour, IStacker
    {
        [field: SerializeField] public Transform StackHolder { get; set; }
        [field: SerializeField] public Transform FollowTarget { get; set; }

        public List<StackableBase> Stacks { get; set; }
        private PlayerController _controller;

        public float speed;
        public float yOffset = 0.5f;

        public Transform[] _xzTransforms;

        public List<StackableBase> asdsadsa;

        private int _currentColumn;
        private int _currentRow;

        public void Initialize(PlayerController playerController)
        {
            DOTween.SetTweensCapacity(1000, 100);
            Stacks = new List<StackableBase>();
            _controller = playerController;
        }

        private void FixedUpdate()
        {
            if (GameManager.Instance == null) return;
            asdsadsa = Stacks;
            HandleStackMovement();
        }

        public bool IsMax()
        {
            return _currentRow > _controller.capacityLevels[_controller.CurrentCapacityLevel].maxYCount;
        }

        public void Stack(StackableBase stackable)
        {
            if (IsMax() || Stacks.Contains(stackable)) return;

            Stacks.Add(stackable);
            stackable.transform.SetParent(StackHolder);
            _currentRow = (Stacks.Count / _xzTransforms.Length) + 1;
            HandleStackScale();
        }

        public void Unstack(StackableBase stackable)
        {
            if (GameManager.Instance == null) return;
            if (!Stacks.Contains(stackable)) return;


            var stackIndex = GetStackIndex(stackable);

            for (int i = stackIndex; i < Stacks.Count; i++)
            {
                // Stacks[i].OnUnstacked();
                _currentRow = (Stacks.Count / _xzTransforms.Length) + 1;
                Stacks.Remove(Stacks[i]);
            }
        }

        private int GetStackIndex(StackableBase stack)
        {
            var index = -1;

            for (int i = 0; i < Stacks.Count; i++)
            {
                if (Stacks[i] != stack) continue;
                index = i;
                break;
            }

            return index;
        }

        public void HandleStackMovement()
        {
            // if (Stacks.Count <= 0) return;

            _currentColumn = 0;
            speed = _controller.capacityLevels[_controller.CurrentCapacityLevel].maxYCount / 1.2f;
            var s = 0;
            for (int i = 0; i < Stacks.Count; i++)
            {
                _currentColumn = i % _xzTransforms.Length;

                s = i / _xzTransforms.Length;


                Stacks[i].transform.position =
                    Vector3.Lerp(Stacks[i].transform.position,
                        _xzTransforms[_currentColumn].position + Vector3.up * s * yOffset,
                        (Time.fixedDeltaTime * speed) * (_controller.capacityLevels[_controller.CurrentCapacityLevel].maxYCount - s + 1));

                var extraEuler = Quaternion.Euler(0, 0, 90);
                Stacks[i].transform.rotation = Quaternion.Lerp(Stacks[i].transform.rotation,
                    Quaternion.LookRotation(transform.forward) * extraEuler,
                    Time.fixedDeltaTime * speed * (_controller.capacityLevels[_controller.CurrentCapacityLevel].maxYCount - s));
            }

            if (Stacks.Count <= 0)
            {
                _controller.moneyDisplayTransform.gameObject.SetActive(false);
                var z = _controller.moneyDisplayFillBar.size;
                z.x = 0;
                _controller.moneyDisplayFillBar.size = z;
                return;
            }
            _controller.moneyDisplayTransform.gameObject.SetActive(true);
            var transforms = Stacks.Select(f => f.transform).ToArray();
            var centerOfMoneys = Extensions.FindCenterOfTransforms(transforms);
            centerOfMoneys.y = Stacks.Last().transform.position.y + 1;
            _controller.moneyDisplayTransform.position = Vector3.Lerp(_controller.moneyDisplayTransform.position, centerOfMoneys, Time.fixedDeltaTime * 9);

            var size = _controller.moneyDisplayFillBar.size;
            size.x = (float) _currentRow / (_controller.capacityLevels[_controller.CurrentCapacityLevel].maxYCount-1);
            size.x = Mathf.Clamp(size.x, 0, 1);
            _controller.moneyDisplayFillBar.size = size;
        }


        private void HandleStackScale()
        {
            Stacks.Last().OnStacked();
        }

        public void SetXZTransforms(Transform[] transforms)
        {
            _xzTransforms = transforms;
        }
    }
}