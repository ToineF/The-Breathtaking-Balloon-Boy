using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public Transform Transform;

    private void OnTriggerEnter(Collider other)
    {
        CheckpointManager.Instance.ChangeCurrentCheckPoint(this);
    }
}
