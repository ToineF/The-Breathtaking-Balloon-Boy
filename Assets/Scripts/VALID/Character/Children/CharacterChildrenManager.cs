using BlownAway.Character;
using BlownAway.Character.Data;
using UnityEngine;

public class CharacterChildrenManager : MonoBehaviour
{
    public CharacterManager Manager { get; set; }


    [field:SerializeField] public int ChildrenCount {  get; private set; }
    [field:SerializeField] public CharacterChildrenUpgradesData CharacterUpgradesData {get; private set; }

    private void Start()
    {
        UpdateData();
    }

    public void AddChild()
    {
        ChildrenCount++;
        UpdateData();
    }

    private void UpdateData()
    {
        if (CharacterUpgradesData.CharacterDatas.Length <= 0) return;

        int targetData = Mathf.Clamp(ChildrenCount, 0, CharacterUpgradesData.CharacterDatas.Length - 1);
        Manager.Data = CharacterUpgradesData.CharacterDatas[targetData];
    }
}
