using System;
using Gameplay.Field;
using UnityEngine;

namespace Gameplay.Player
{
    public class PlayerTrigger : MonoBehaviour
    {
        private PlayerController _controller;
        private float _timer;
        private const float MoneyGiveWaitTime = 0.05f;
        public float collectRadius;

        public void Initialize(PlayerController playerController)
        {
            _controller = playerController;
        }


        private void OnTriggerExit(Collider other)
        {
        }

        private Field.Field _enteredField;

        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<SingleField>())
            {
               
            }

            if (other.GetComponentInParent<Field.Field>() && other.gameObject.layer == LayerMask.NameToLayer("Field"))
            {
                var field = other.GetComponentInParent<Field.Field>();
                _enteredField = field;
                _timer = 0;
            }
        }

        private void Update ()
        {
            var hitColliders = Physics.OverlapSphere(transform.position, collectRadius);
            foreach (var col in hitColliders)
            {
                var hitCollider = col.gameObject;
                if (!hitCollider.GetComponent<SingleField>()) continue;
                
                var singleField = hitCollider.GetComponent<SingleField>();
                if (!singleField.growth || _controller.Stacker.IsMax()) continue;
                hitCollider.GetComponent<SingleField>().Crop(_controller);
            }
        }
        void OnDrawGizmos ()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere (transform.position, collectRadius);
        }


        private void OnTriggerStay(Collider other)
        {
            if (other.GetComponentInParent<Field.Field>() && other.gameObject.layer == LayerMask.NameToLayer("Field") && _enteredField == other.GetComponentInParent<Field.Field>())
            {
                if (_controller.Movement.IsMoving) return;


                if (_controller.Stacker.Stacks.Count > 0)
                {
                    _timer += Time.deltaTime;

                    if (_timer > MoneyGiveWaitTime)
                    {
                        _timer = 0;
                        if (_enteredField.tookMoney == _enteredField.unlockPrice) return;
                        
                        _controller.GiveMoneyToField(_enteredField);
                    }
                }
            }
        }
    }
}