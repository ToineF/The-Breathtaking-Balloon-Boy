using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace BlownAway.Character
{
    public class SubMenu : CharacterSubComponent
    {
        [field: Header("Current Menu")]
        [field: SerializeField] public CanvasGroup CanvasGroup { get; private set; }
        [field: SerializeField] public GameObject FirstSelectedButton { get; private set; }

        [field: Header("Top Menu")]
        [field: SerializeField] public SubMenu TopSubmenu { get; private set; }
        [field: SerializeField] public GameObject TopSelectedButton { get; private set; }
        [field: SerializeField] public bool CanBeClosed { get; private set; } = true;

        public void OpenSubMenu(SubMenu submenu)
        {
            OpenMenu(submenu, submenu.FirstSelectedButton);
            CloseMenu(this);
            //submenu.Inputs.UnityUI.Cancel.performed += TryCloseSubMenu;
        }

        public void CloseSubMenu()
        {
            if (TopSubmenu != null) OpenMenu(TopSubmenu, TopSelectedButton);
            CloseMenu(this);
            //Inputs.UnityUI.Cancel.performed -= TryCloseSubMenu;
        }

        protected void OpenMenu(SubMenu submenu, GameObject firstSelected = null)
        {
            submenu.CanvasGroup.interactable = true;
            submenu.CanvasGroup.alpha = 1;
            submenu.CanvasGroup.blocksRaycasts = true;
            if (firstSelected != null) EventSystem.current.SetSelectedGameObject(firstSelected);
            Debug.LogError("Open submenu : " +  submenu.gameObject.name);
            submenu.Inputs.UnityUI.Cancel.performed += submenu.TryCloseSubMenu;
        }

        protected void CloseMenu(SubMenu submenu, GameObject firstSelected = null)
        {
            submenu.CanvasGroup.interactable = false;
            submenu.CanvasGroup.alpha = 0;
            submenu.CanvasGroup.blocksRaycasts = false;
            if (firstSelected != null) EventSystem.current.SetSelectedGameObject(firstSelected);
            Debug.LogError("Close submenu : " + submenu.gameObject.name);
            submenu.Inputs.UnityUI.Cancel.performed -= submenu.TryCloseSubMenu;
        }

        #region Cancel Input
        // Inputs
        public PlayerInputs Inputs { get; private set; }
        public static bool CanPressCancel { get; set; } = true;

        private void Awake()
        {
            Inputs = new PlayerInputs();
        }

        private void OnEnable()
        {
            Inputs.Enable();
            Inputs.UnityUI.Cancel.canceled += PressCancel;

        }
        private void OnDisable()
        {
            Inputs.Disable();
            Inputs.UnityUI.Cancel.canceled -= PressCancel;
        }

        private void TryCloseSubMenu(InputAction.CallbackContext context)
        {
            //if (CanvasGroup.alpha == 0) return;
            if (!CanBeClosed) return;
            if (!CanPressCancel) return;


            Debug.Log("Close : " + gameObject.name);
            CanPressCancel = false;
            CloseSubMenu();
        }

        private void PressCancel(InputAction.CallbackContext context)
        {
            CanPressCancel = true;
        }
        #endregion
    }
}