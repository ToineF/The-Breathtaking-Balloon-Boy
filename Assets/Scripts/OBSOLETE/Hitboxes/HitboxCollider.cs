using UnityEngine;

namespace BlownAway.Hitbox
{
    public abstract class HitboxCollider : MonoBehaviour
    {
        virtual protected void OnCollisionEnter(Collision other)
        {

        }

        virtual protected void OnCollisionStay(Collision other)
        {

        }

        virtual protected void OnCollisionExit(Collision other)
        {

        }
    }
}