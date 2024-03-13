using BlownAway.Character;
using BlownAway.Character.Data;
using UnityEngine;

public class CharacterChildrenManager : CharacterSubComponent
{
    [field:SerializeField] public int ChildrenCount {  get; private set; }
    [field:SerializeField] public CharacterChildrenUpgradesData CharacterUpgradesData {get; private set; }

    protected override void StartScript(CharacterManager manager)
    {
        UpdateData();
    }

    public void AddChild()
    {
        ChildrenCount++;
        UpdateData();
    }

    public void SetChildren(int number)
    {
        ChildrenCount = number;
        UpdateData();
    }

    private void UpdateData()
    {
        if (CharacterUpgradesData.CharacterDatas.Length <= 0) return;

        int targetData = Mathf.Clamp(ChildrenCount, 0, CharacterUpgradesData.CharacterDatas.Length - 1);
        Manager.Data = CharacterUpgradesData.CharacterDatas[targetData];
    }
}
