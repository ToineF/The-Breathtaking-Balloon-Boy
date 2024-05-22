namespace BlownAway.Character.States
{
    public abstract class CharacterBaseState
    {
        public abstract void EnterState(CharacterManager manager, CharacterBaseState previousState);
        public abstract void UpdateState(CharacterManager manager);
        public abstract void FixedUpdateState(CharacterManager manager);
        public abstract void LateUpdateState(CharacterManager manager);
        public abstract void ExitState(CharacterManager manager);

        public abstract bool IsMovable { get; }
    }
}