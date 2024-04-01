using UnityEngine;
using DG.Tweening;

namespace BlownAway.Cutscenes
{
    public class CutsceneCameraManager : MonoBehaviour
    {
        public void SetNewCamera(CutsceneCamera camera)
        {
            camera.TargetTransform.gameObject.SetActive(false);
            camera.TargetTransform.gameObject.SetActive(true);
        }
    }
}
