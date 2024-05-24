using UnityEngine;
using UnityEngine.EventSystems;

namespace BlownAway.Character
{
    public class SubMenu : CharacterSubComponent
    {
        [field: Header("Current Menu")]
        [field: SerializeField] public CanvasGroup CanvasGroup { get; private set; }
        [field: SerializeField] public GameObject FirstSelectedButton { get; private set; }

        [field: Header("Top Menu")]
        [field: SerializeField] public CanvasGroup TopCanvasGroup { get; private set; }
        [field: SerializeField] public GameObject TopSelectedButton { get; private set; }

        public void OpenSubMenu(SubMenu submenu)
        {
            OpenMenu(submenu.CanvasGroup, submenu.FirstSelectedButton);
            CloseMenu(CanvasGroup);
        }

        public void CloseSubMenu()
        {
            if (TopCanvasGroup != null) OpenMenu(TopCanvasGroup, TopSelectedButton);
            CloseMenu(CanvasGroup);
        }

        protected void OpenMenu(CanvasGroup group, GameObject firstSelected = null)
        {
            group.interactable = true;
            group.alpha = 1;
            group.blocksRaycasts = true;
            if (firstSelected != null) EventSystem.current.SetSelectedGameObject(firstSelected);
        }

        protected void CloseMenu(CanvasGroup group, GameObject firstSelected = null)
        {
            group.interactable = false;
            group.alpha = 0;
            group.blocksRaycasts = false;
            if (firstSelected != null) EventSystem.current.SetSelectedGameObject(firstSelected);
        }
    }
}