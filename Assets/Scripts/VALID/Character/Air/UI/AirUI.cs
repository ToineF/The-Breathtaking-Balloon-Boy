
namespace BlownAway.Character.Air
{
    public class AirUI : RadialUI
    {
        void Update()
        {
            FillAmount = GameManager.Instance.CharacterManager.AirManager.CurrentAir;
        }
    }
}