using BlownAway.Player;
using UnityEngine;

[RequireComponent (typeof(Collider))]
public class AirRefreshBonus : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (!other.TryGetComponent(out BalloonBoyController character)) return;

        character.RefreshAir();
        Destroy(gameObject);
    }
}
