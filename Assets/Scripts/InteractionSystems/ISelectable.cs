using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISelectable
{
    GameObject GameObject { get; }
    void OnSelect();
    void OnDeselect();
    List<IAction> Actions { get; }
}
