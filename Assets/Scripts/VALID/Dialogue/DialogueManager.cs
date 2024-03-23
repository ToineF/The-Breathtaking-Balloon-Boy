using UnityEngine;
using TMPro;
using System.Collections;
using System;
using AntoineFoucault.Utilities;
using UnityEngine.UI;
using System.Collections.Generic;

namespace BlownAway.Cutscenes
{
    public class DialogueManager : MonoBehaviour
    {
        public Action OnDialogueEnd;

        [SerializeField] AudioClip _dialogueContinueSound;

        public int CurrentTextIndex
        {
            get => _currentTextIndex; set
            {
                if (_currentDialogue == null) return;

                if (_hasCurrentTextEnded)
                {
                    _currentTextIndex = Mathf.Clamp(value, 0, _currentDialogue.Texts.Length - 1);
                    AudioManager.Instance?.PlayClip(_dialogueContinueSound);
                }

                if ((value < 0 || value > _currentDialogue.Texts.Length - 1) && _hasCurrentTextEnded)
                    EndDialogue();
                else
                    StartNewText();

            }
        }

        [SerializeField] private TMP_Text _dialogueTextboxText;
        [SerializeField] private Image _dialogueTextbox;
        [SerializeField] private float _characterApparitionSpeed;

        [Header("Text Effects")]
        [SerializeField] private TextEffectData _hiddenEffectData;
        [SerializeField] private TextEffectData _baseEffectData;

        private Dialogue _currentDialogue;
        private int _currentTextIndex;
        private int _currentCharIndex;
        private bool _hasCurrentTextEnded;
        private Coroutine _writingCharactersCoroutine;
        private List<TextEffectData> _textEffects = new List<TextEffectData>();

        private void Awake()
        {
            _hasCurrentTextEnded = true;
        }

        public void SetNewDialogue(Dialogue newDialogue)
        {
            _currentDialogue = newDialogue;
            CurrentTextIndex = 0;
            _dialogueTextbox.color = newDialogue.CharacterData.DialogueBoxColor;
            _dialogueTextboxText.color = newDialogue.CharacterData.DialogueTextColor;
        }

        private void GoToTextAt(int index)
        {
            CurrentTextIndex = index;
        }

        public void GoToNextText()
        {
            GoToTextAt(CurrentTextIndex + 1);
        }

        private void StartNewText()
        {
            string currentText = _currentDialogue.Texts[CurrentTextIndex];

            if (!_hasCurrentTextEnded)
            {
                FullyWriteText();
            }
            else
            {
                _writingCharactersCoroutine = StartCoroutine(WriteEachCharacter(_dialogueTextboxText, currentText, _characterApparitionSpeed, _currentDialogue.CharacterData));
            }
        }

        public IEnumerator WriteEachCharacter(TMP_Text dialogueTextbox, string finalText, float timeBetweenCharacters, DialogueCharacterData characterData)
        {
            _hasCurrentTextEnded = false;
            dialogueTextbox.text = finalText;
            _currentCharIndex = 0;
            for (int i = 0; i < finalText.Length; i++)
            {
                _currentCharIndex = i;
                char c = finalText[i];

                if (characterData.SoundsTalk.Length > 0)
                {
                    if (i % Math.Max(1, characterData.TalkFrequency) == 0 && char.IsLetterOrDigit(c))
                    {
                        AudioClip sound = characterData.SoundsTalk.GetRandomItem();
                        AudioManager.Instance?.PlayClip(sound);
                    }
                }


                yield return new WaitForSeconds(timeBetweenCharacters);
            }
            _hasCurrentTextEnded = true;
            _currentCharIndex = -1;
        }

        public void FullyWriteText()
        {
            if (_writingCharactersCoroutine != null)
                StopCoroutine(_writingCharactersCoroutine);
            _currentCharIndex = -1;
            _hasCurrentTextEnded = true;
        }

        private void EndDialogue()
        {
            OnDialogueEnd?.Invoke();
        }


        // Text Animation
        private void Start()
        {
            _textEffects.Add(_hiddenEffectData);
            _textEffects.Add(_baseEffectData);
        }

        private void Update()
        {
            PlayTextAnimation();
        }

        private void PlayTextAnimation()
        {
            _dialogueTextboxText.ForceMeshUpdate();
            TMP_TextInfo textInfo = _dialogueTextboxText.textInfo;

            if (_textEffects == null) return;

            foreach (TextEffectData data in _textEffects)
            {
                TextEffect effect = data.TextEffect;

                int minRange = effect.MinRange;
                int maxRange = effect.MaxRange;
                if (effect.TypeWriterRange == TextEffect.LimitRangeByTypeWriter.MIN) minRange = _currentCharIndex;
                if (effect.TypeWriterRange == TextEffect.LimitRangeByTypeWriter.MAX) maxRange = _currentCharIndex;
                if (minRange == -1) minRange = textInfo.characterCount;
                if (maxRange == -1) maxRange = textInfo.characterCount;
                for (int i = minRange; i < maxRange; i++)
                {
                    TMP_CharacterInfo charInfo = textInfo.characterInfo[i];

                    if (!charInfo.isVisible) continue;

                    TMP_MeshInfo meshInfo = textInfo.meshInfo[charInfo.materialReferenceIndex];
                    Vector3 centerPoint = new Vector2((meshInfo.vertices[charInfo.vertexIndex].x + meshInfo.vertices[charInfo.vertexIndex + 2].x)/2, meshInfo.vertices[charInfo.vertexIndex].y);
                    Vector3 charData = data.CharMathDisplacement.GetTotalFunction(centerPoint);

                    for (int j = 0; j < 4; j++)
                    {
                        int index = charInfo.vertexIndex + j;
                        Vector3 origin = meshInfo.vertices[index];
                        meshInfo.vertices[index] = origin + data.VertexMathDisplacement.GetTotalFunction(origin) + charData;
                        meshInfo.colors32[index] = effect.Colors[j];
                    }
                }
            }

            for (int i = 0; i < textInfo.meshInfo.Length; i++)
            {
                TMP_MeshInfo meshInfo = textInfo.meshInfo[i];
                meshInfo.mesh.vertices = meshInfo.vertices;
                meshInfo.mesh.colors32 = meshInfo.colors32;
                _dialogueTextboxText.UpdateGeometry(meshInfo.mesh, i);
            }
        }

    }
}