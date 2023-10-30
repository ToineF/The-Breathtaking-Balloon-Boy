using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BalloonBaseState
{
    public abstract void StartState(BalloonStateManager balloonStateManager);
    public abstract void UpdateState(BalloonStateManager balloonStateManager);
}
