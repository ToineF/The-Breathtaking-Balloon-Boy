using UnityEngine;
using DG.Tweening;
using System.Collections;

public class ChildAnimatedCircle : MonoBehaviour
{
    [Header("Animation")]
    [SerializeField] private Animator _animator;
    [SerializeField] private string _isWalkingName;

    [Header("Params")]
    [SerializeField] private float _walkRadius;
    [SerializeField] private float _walkSpeed;
    [SerializeField] private float _idleTimeMin;
    [SerializeField] private float _idleTimeMax;
    [SerializeField] private float _rotationSmooth;
    [SerializeField] private Ease _walkEase;

    private Vector3 _startPosition;
    private DG.Tweening.Tween _walkTween;


    private void Start()
    {
        _startPosition = transform.position;
        StartWalkingTowardsNewPosition();
    }

    private void StartWalkingTowardsNewPosition()
    {
        _animator.SetBool(_isWalkingName, true);
        Vector3 targetOffset = Random.insideUnitSphere;
        targetOffset = new Vector3(targetOffset.x, 0, targetOffset.y);
        Vector3 targetPosition = _startPosition + targetOffset * _walkRadius;
        float distance = Vector3.Distance(transform.position, targetPosition);

        _walkTween = transform.DOMove(targetPosition, distance / _walkSpeed).OnComplete(StopWalking).SetEase(_walkEase);
        var angle = Mathf.Atan2(targetPosition.x - transform.position.x, targetPosition.z - transform.position.z);   //radians
                                                                                                                               // you need to devide by PI, and MULTIPLY by 180:
        var degrees = 180 * angle / Mathf.PI;  //degrees
        //round number, avoid decimal f
        transform.eulerAngles = Vector3.up * degrees;

        Debug.Log(distance);
        Debug.Log(_walkSpeed);
        Debug.Log(distance / _walkSpeed);
    }


    private void StopWalking()
    {
        _animator.SetBool(_isWalkingName, false);
        StartCoroutine(WaitIdle());
    }

    private IEnumerator WaitIdle()
    {
        yield return new WaitForSeconds(Random.Range(_idleTimeMin, _idleTimeMax));

        StartWalkingTowardsNewPosition();
    }

    private void OnDestroy()
    {
        _walkTween.Kill();
    }
}
