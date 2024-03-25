using UnityEngine;

public abstract class WendigoBaseState
{
    public abstract void EnterState(WendigoStateManager stateManager);
    public abstract void UpdateState(WendigoStateManager stateManager);
}

