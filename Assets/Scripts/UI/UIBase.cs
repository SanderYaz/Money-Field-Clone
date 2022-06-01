
using Interfaces;
using UnityEngine;

namespace UI
{
    public class UIBase : MonoBehaviour, IUI
    {
        public void Hide()
        {
            gameObject.SetActive(false);
        }

        public void Show()
        {

            OnShow();
        }


        protected virtual void OnShow()
        {


            gameObject.SetActive(true);
        }
    }
}

