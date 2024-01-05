using BlownAway.Player;
using UnityEngine;

namespace BlownAway.GPE
{
    public class Hoop : MonoBehaviour
    {

        [SerializeField] float _force;
        [SerializeField] [Range(0,1)] float _acceleration;
        [SerializeField] [Range(0,1)] float _deceleration;

        private bool _isPlayerIn;

        private void OnTriggerEnter(Collider other)
        {
            if (!other.TryGetComponent(out CharacterControllerTest characterController)) return;
            if (_isPlayerIn) return;
            _isPlayerIn = true;

            //int direction = Mathf.Sign(characterController.transform.position.z - transform.position.z);
            Vector3 targetDir = characterController.transform.position - transform.position;
            float angle = Vector3.Angle(targetDir, transform.up);
            float direction = angle > 90 ? 1 : -1;
            Debug.Log(angle);

            characterController.AddAdditionalForce(gameObject, direction * _force * transform.up, _acceleration);

        }

        private void OnTriggerExit(Collider other)
        {
            if (!other.TryGetComponent(out CharacterControllerTest characterController)) return;
            if (!_isPlayerIn) return;
            _isPlayerIn = false;


            characterController.AddAdditionalForce(gameObject, Vector3.zero, _deceleration);

        }
    }
}