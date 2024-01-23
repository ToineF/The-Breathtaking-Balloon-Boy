using System;
using UnityEngine;
using BlownAway.Character.States;
using BlownAway.Character.Inputs;
using BlownAway.Camera;

namespace BlownAway.Character
{
    public class CharacterManager : Singleton<CharacterManager> /// CLEAN UP THIS MESS OF A CLASS
    {
        [field: SerializeField] public Rigidbody CharacterRigidbody { get; set; }
        [field: SerializeField] public Transform CharacterTransform { get; set; }


        // Inputs
        [field: SerializeField, Tooltip("The reference to the class that contains the inputs of the character")] public CharacterInputsManager Inputs { get; private set; }

        // Camera
        [field: SerializeField, Tooltip("The reference to the class that contains the logic about the camera")] public CameraManager CameraManager { get; private set; }

        // Movement
        [field: SerializeField, Tooltip("The reference to the class that contains the movement of the player")] public CharacterMovementManager MovementManager { get; private set; }


    }
}