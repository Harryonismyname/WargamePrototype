using UnityEngine;

public interface IAction
{
    string Name {  get; } 
    int Cost { get; }
    bool Declare(Vector3 point);
    bool Perform(Vector3 point);
}
