using System.Collections.Generic;
using Gameplay.Stack.Stackable;
using UnityEngine;


public interface IStacker
{
    List<StackableBase> Stacks { get; set; }
    Transform StackHolder { get; set; }
    Transform FollowTarget { get; set; }

    void Stack(StackableBase stackable);

    void Unstack(StackableBase stackable);

    void HandleStackMovement();
}