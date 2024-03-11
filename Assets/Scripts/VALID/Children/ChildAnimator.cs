using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AntoineFoucault.Utilities;

namespace BlownAway.Children
{
    public class ChildAnimator : MonoBehaviour
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private string[] _hiddenIdleNames;
        [SerializeField] private string _foundIdle;

        private void Start()
        {
            _animator.Play(_hiddenIdleNames.GetRandomItem());
        }

        public void Found()
        {
            _animator.Play(_foundIdle);
        }
    }
}