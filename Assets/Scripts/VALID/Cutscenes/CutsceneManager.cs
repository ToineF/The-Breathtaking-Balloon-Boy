using System;
using System.Collections.Generic;
using UnityEngine;

namespace BlownAway.Cutscenes
{
    public class CutsceneManager : MonoBehaviour
    {
        //public int CurrentSequenceIndex
        //{
        //    get => _currentSequenceIndex; private set
        //    {
        //        if (_currentInteractionSequence == null) return;

        //        _currentSequenceIndex = Mathf.Clamp(value, 0, _currentInteractionSequence.SequenceElements.Length - 1);

        //        if (value < 0 || value > _currentInteractionSequence.SequenceElements.Length - 1) EndSequence();
        //        else ReadSequenceElement();
        //    }
        //}

        //private InteractionSequence _currentInteractionSequence;
        //private int _currentSequenceIndex;

        //public void StartNewSequence(InteractionSequence sequence)
        //{
        //    if (MainGame.Instance.Player)
        //        MainGame.Instance.Player.CanMove = false;

        //    _currentInteractionSequence = sequence;
        //    CurrentSequenceIndex = 0;
        //}

        //private void GoToSequenceAt(int index)
        //{
        //    CurrentSequenceIndex = index;
        //}

        //public void GoToNextSequenceElement()
        //{
        //    GoToSequenceAt(CurrentSequenceIndex + 1);
        //}

        //private void EndSequence()
        //{
        //    if (MainGame.Instance.Player)
        //        MainGame.Instance.Player.CanMove = true;
        //}

        //private void ReadSequenceElement()
        //{
        //    InteractionElement interactionElement = _currentInteractionSequence.SequenceElements[CurrentSequenceIndex];

        //    Dialogue dialogue = interactionElement as Dialogue;
        //    if (dialogue != null)
        //    {
        //        ReadDialogue(dialogue);
        //    }

        //    InventoryItemData itemData = interactionElement as InventoryItemData;
        //    if (itemData != null)
        //    {
        //        SpawnInventoryItem(itemData);
        //    }

        //    InteractionImageAppear imageAppear = interactionElement as InteractionImageAppear;
        //    if (imageAppear != null)
        //    {
        //        ShowImage(imageAppear);
        //    }
        //}

        //private void ReadDialogue(Dialogue dialogue)
        //{
        //    MainGame.Instance.DialogueUI.SetActive(true);
        //    MainGame.Instance.DialogueManager.SetNewDialogue(dialogue);
        //    MainGame.Instance.DialogueManager.OnDialogueEnd += EndDialogue;
        //}

        //private void EndDialogue()
        //{
        //    MainGame.Instance.DialogueUI.SetActive(false);
        //    MainGame.Instance.DialogueManager.OnDialogueEnd -= EndDialogue;
        //    GoToNextSequenceElement();
        //}

        //private void SpawnInventoryItem(InventoryItemData itemData)
        //{
        //    MainGame.Instance.Inventory.AddItem(itemData);
        //    MainGame.Instance.Inventory.OnItemAddition += EndItemSpawn;
        //}

        //private void EndItemSpawn()
        //{
        //    MainGame.Instance.Inventory.OnItemAddition -= EndItemSpawn;
        //    GoToNextSequenceElement();
        //}

        //private void ShowImage(InteractionImageAppear imageAppear)
        //{
        //    MainGame.Instance.InteractionImageManager.gameObject.SetActive(true);
        //    MainGame.Instance.InteractionImageManager.SetImage(imageAppear.Sprite);
        //    MainGame.Instance.InteractionImageManager.OnCLick += HideImage;
        //}

        //private void HideImage()
        //{
        //    MainGame.Instance.InteractionImageManager.gameObject.SetActive(false);
        //    GoToNextSequenceElement();
        //}
    }

    [Serializable]
    public class CutsceneElement
    {
    }

    public class CutsceneDialogue : CutsceneElement
    {
        [SerializeField] public Dialogue _dialogue;
    }

    public class CutsceneCamera : CutsceneElement
    {
        [SerializeField] public Transform _targetTransform;
        [SerializeField] public float _travellingTime;
        [SerializeField] public DG.Tweening.Ease _travellingEase;

    }
    public class CutsceneWaitForTime : CutsceneElement
    {
        [SerializeField] public float _waitTIme;
    }

    public class Cutscene
    {
        [BF_SubclassList.SubclassList(typeof(CutsceneElement)), SerializeField] private CutsceneElementWrapper _cutscene;
    }

    [Serializable]
    public class CutsceneElementWrapper
    {
        [SerializeReference] public List<CutsceneElement> List;
    }
}