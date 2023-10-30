using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalloonStateManager : MonoBehaviour
{
    private BalloonBaseState _currentState;
    private BalloonNone _balloonNone = new BalloonNone();
    private BalloonFlower _balloonFlower = new BalloonFlower();

    private void Start()
    {
        _currentState = _balloonNone;
        _currentState.StartState(this);
    }

    private void Update()
    {
        _currentState.UpdateState(this);
    }

    public void SwitchState(BalloonBaseState state)
    {
        _currentState = state;
        _currentState.StartState(this);
    }
}
