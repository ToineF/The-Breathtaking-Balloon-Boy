using UnityEngine;

public class Propeller : MonoBehaviour
{
    [SerializeField] private float _turnSpeed;
    [SerializeField] private Vector3 _vector3Up;

    void Update()
    {
        transform.Rotate(_vector3Up * _turnSpeed * Time.deltaTime);
    }
}
