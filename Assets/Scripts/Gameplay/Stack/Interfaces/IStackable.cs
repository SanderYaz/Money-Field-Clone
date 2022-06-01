using UnityEngine;

namespace Gameplay.Stack.Interfaces
{
    public interface IStackable
    {
        void OnStacked();
        void OnUnstacked();
    }
}

