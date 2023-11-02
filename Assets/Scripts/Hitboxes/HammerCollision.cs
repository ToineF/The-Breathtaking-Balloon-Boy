using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HammerCollision : MonoBehaviour
{
    [SerializeField] private LayerMask _wallLayer;
    private float _collisionTime = 2;
    private Collider _collider;

    private void OnEnable()
    {
        _collider = null;
        StartCoroutine(Disable());
    }

    IEnumerator Disable()
    {
        yield return new WaitForSeconds(_collisionTime);
        gameObject.SetActive(false);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (((int)Mathf.Pow(2,other.gameObject.layer) & _wallLayer.value) <= 0) return;
        _collider = other.collider;
        Debug.Log(other.gameObject.name);
        // Return _collider HERE
        gameObject.SetActive(false);
    }
}
