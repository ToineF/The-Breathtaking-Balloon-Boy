using UnityEngine;
using static UnityEditor.PlayerSettings;

public class CheckpointManager : MonoBehaviour
{
    public static CheckpointManager Instance;

    private Checkpoint _currentCheckPoint;

    private void Awake()
    {
        Instance = this;
    }

    public void ChangeCurrentCheckPoint(Checkpoint checkPoint)
    {
        _currentCheckPoint = checkPoint;
    }

    private void SetPosition(GameObject objectToMove)
    {
        objectToMove.transform.position = _currentCheckPoint.Transform.position;
    }

    private void Update()
    {
        if (_currentCheckPoint == null) return;
        if (Input.GetKeyDown(KeyCode.K))
        {
            Debug.LogWarning(_currentCheckPoint.Transform.position);
            Physics.autoSyncTransforms = true;
            CharacterControllerTest.Instance.enabled = false;
            CharacterControllerTest.Instance.transform.position = _currentCheckPoint.Transform.position; // chara contreoller can't move transform
            CharacterControllerTest.Instance.enabled = true;
            Physics.autoSyncTransforms = false;
        }
    }
}
