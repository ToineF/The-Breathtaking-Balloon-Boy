using BlownAway.Character;

namespace BlownAway.City
{
    public class Shop : SubMenu
    {
        public void Open()
        {
            OpenMenu(CanvasGroup, FirstSelectedButton);
        }
    }
}