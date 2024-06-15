using UnityEngine;
using UnityEngine.EventSystems;

public class EventSystemDebug : StandaloneInputModule
{
    private void Update()
    {
        Debug.LogError("Object: " + GetHovered());
    }

    public GameObject GetHovered()
    {
        var mouseEvent = GetLastPointerEventData(-1);
        if (mouseEvent == null)
            return null;
        return mouseEvent.pointerCurrentRaycast.gameObject;
    }

}
