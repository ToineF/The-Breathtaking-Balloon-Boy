using UnityEngine;

namespace BlownAway.Character.Data
{
    [CreateAssetMenu(fileName = "CharacterUpgradesData", menuName = "CharacterData/All Upgrades Data")]
    public class CharacterChildrenUpgradesData : ScriptableObject
    {
        [field:SerializeField] public CharacterControllerData[] CharacterDatas {get; private set; }
        [field:SerializeField] public CharacterControllerData GodModeData {get; private set; }

    }
}
