using UnityEngine;
using TMPro;
using DG.Tweening;
using System.Collections;

namespace BlownAway.Player
{
    public class ChildrenManager : MonoBehaviour
    {
        [SerializeField] TMP_Text _childrenCountText;

        [Header("Feedbacks")]
        [SerializeField] private float _textFadeAppearTime;
        [SerializeField] private float _textStayTime;
        [SerializeField] private float _textFadeDisapearTime;
        [SerializeField] private Vector3 _textPunchDirection;
        [SerializeField] private float _textPunchDuration;

        private int _childrenCount;

        private void Start()
        {
            _childrenCount = 0;
            _childrenCountText.text = _childrenCount.ToString();
            _childrenCountText.DOFade(0, 0);
        }

        public void AddChild()
        {
            _childrenCount++;
            UpdateText();
        }

        private void UpdateText()
        {
            _childrenCountText.DOKill();
            _childrenCountText.text = _childrenCount.ToString();
            StartCoroutine(AnimateText());
        }

        private IEnumerator AnimateText()
        {
            _childrenCountText.DOFade(1, _textFadeAppearTime);
            _childrenCountText.transform.DOPunchPosition(_textPunchDirection, _textPunchDuration);
            yield return new WaitForSeconds(_textStayTime);
            _childrenCountText.DOFade(0, _textFadeDisapearTime);
        }
    }
}