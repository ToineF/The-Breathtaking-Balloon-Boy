using UnityEngine;

namespace BlownAway.Hitbox
{
    public abstract class HitboxTrigger : MonoBehaviour
    {
        virtual protected void OnTriggerEnter(Collider other)
        {

        }

        virtual protected void OnTriggerStay(Collider other)
        {

        }

        virtual protected void OnTriggerExit(Collider other)
        {

        }
    }
}