using UnityEngine;

public abstract class BaseCharacterState : MonoBehaviour
{
    public PlayerBrain.State State;

    public abstract void EnterState();
    public abstract void UpdateState();
    public abstract void ExitState();
}
