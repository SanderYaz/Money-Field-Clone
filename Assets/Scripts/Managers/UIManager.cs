using UI.UIs;
using UnityEngine;

namespace Managers
{
    public class UIManager : MonoBehaviour
    {
        public InGameUI inGameUI;
        public EconomyUI economyUI;



        public void Initialize()
        {
            inGameUI.Initialize();
            economyUI.Initialize();
        }

    
    }
}
