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

        private Dialogue _currentDialogue;
        private int _currentTextIndex;
        private int _currentCharIndex;
        private bool _hasCurrentTextEnded;
        private Coroutine _writingCharactersCoroutine;
        private List<TextEffect> _textEffects = new List<TextEffect>();

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
        private void Update()
        {
            //dd(0, _currentCharIndex);
            PlayTextAnimation();
        }

        private void Start()
        {
            _textEffects.Add(new TextEffect(GetNormalText, Color.white, Vector3.one, 0, -1, TextEffect.LimitRangeByTypeWriter.MIN));
            _textEffects.Add(new TextEffect(GetWobblyText, Color.blue, Vector3.one, 0, 0, TextEffect.LimitRangeByTypeWriter.MAX));
        }

        private Vector3 GetWobblyText(Vector3 origin)
        {
            return new Vector3(0, Mathf.Sin(Time.time * 2f + origin.x * 0.01f) * 10f, 0);
        }

        private Vector3 GetNormalText(Vector3 origin)
        {
            return -origin;
        }

        private void HideAllCharacters()
        {
            //PlayTextAnimation(GetNormalText, Color.white, Vector3.zero);
            //dd();
        }

        private void PlayTextAnimation()
        {
            _dialogueTextboxText.ForceMeshUpdate();
            TMP_TextInfo textInfo = _dialogueTextboxText.textInfo;

            if (_textEffects == null) return;

            foreach (TextEffect effect in _textEffects)
            {
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

                    for (int j = 0; j < 4; j++)
                    {
                        int index = charInfo.vertexIndex + j;
                        Vector3 origin = meshInfo.vertices[index];
                        meshInfo.vertices[index] = origin + effect.GetText(origin);
                        meshInfo.colors32[index] = effect.Color;
                        meshInfo.vertices[index].Scale(effect.Scale);
                    }
                }
            }

            for (int i = 0; i < textInfo.meshInfo.Length; i++)
            {
                TMP_MeshInfo meshInfo = textInfo.meshInfo[i];
                meshInfo.mesh.vertices = meshInfo.vertices;
                _dialogueTextboxText.UpdateGeometry(meshInfo.mesh, i);
            }
        }

        private void dd(int rangeStart = 0, int rangeLength = -1)
        {
            _dialogueTextboxText.ForceMeshUpdate();
            TMP_TextInfo textInfo = _dialogueTextboxText.textInfo;

            rangeLength = rangeLength == -1 ? textInfo.characterCount : rangeLength;
            for (int i = rangeStart; i < textInfo.characterCount; i++)
            {
                TMP_CharacterInfo charInfo = textInfo.characterInfo[i];

                if (!charInfo.isVisible) continue;

                TMP_MeshInfo meshInfo = textInfo.meshInfo[charInfo.materialReferenceIndex];

                for (int j = 0; j < 4; j++)
                {
                    int index = charInfo.vertexIndex + j;
                    Vector3 origin = meshInfo.vertices[index];
                    if (i < rangeLength)
                    {
                        meshInfo.vertices[index] = origin + GetWobblyText(origin);
                        meshInfo.colors32[index] = Color.blue;
                        meshInfo.vertices[index].Scale(Vector3.one);
                    }
                    else
                    {
                        meshInfo.vertices[index] = origin + GetNormalText(origin);
                        meshInfo.colors32[index] = Color.white;
                        meshInfo.vertices[index].Scale(Vector3.zero);
                    }
                }
            }

            for (int i = 0; i < textInfo.meshInfo.Length; i++)
            {
                TMP_MeshInfo meshInfo = textInfo.meshInfo[i];
                meshInfo.mesh.vertices = meshInfo.vertices;
                _dialogueTextboxText.UpdateGeometry(meshInfo.mesh, i);
            }
        }
    }
}