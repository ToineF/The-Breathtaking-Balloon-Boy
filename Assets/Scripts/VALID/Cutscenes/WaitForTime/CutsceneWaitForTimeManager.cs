using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace BlownAway.Cutscenes
{
    public class CutsceneWaitForTimeManager : MonoBehaviour
    {
        public Action OnTimerEnd;

        private float _currentTimer;

        public void StartTimer(float timer)
        {
            _currentTimer = timer;
        }

        private void Update()
        {
            if (_currentTimer <= 0) return;
            _currentTimer -= Time.deltaTime;
            if (_currentTimer <= 0)
                OnTimerEnd?.Invoke();
        }
    }
}
