using UnityEngine;

public class IKFootPlacement : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private float _distanceToGround;
    [SerializeField] private LayerMask _groundLayer;

    private void OnAnimatorIK(int layerIndex)
    {
        if (!_animator) return;

        _animator.SetIKPositionWeight(AvatarIKGoal.LeftFoot, 1f);
        _animator.SetIKRotationWeight(AvatarIKGoal.LeftFoot, 1f);

        // Left foot
        RaycastHit hit;
        Ray ray = new Ray(_animator.GetIKPosition(AvatarIKGoal.LeftFoot) + Vector3.up, Vector3.down);
        if (Physics.Raycast(ray, out hit, _distanceToGround + 1f, _groundLayer))
        {
            Vector3 footPosition = hit.point;
            footPosition.y += _distanceToGround;
            _animator.SetIKPosition(AvatarIKGoal.LeftFoot, footPosition);
            _animator.SetIKRotation(AvatarIKGoal.LeftFoot, Quaternion.LookRotation(transform.forward, hit.normal)); ;
        }
    }
}
