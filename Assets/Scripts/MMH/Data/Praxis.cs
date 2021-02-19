namespace MMH.Data
{
    public class Praxis
    {
        public bool Decided;

        public Type.Behavior.Movement Movement;


        public Praxis()
        {
            Decided = false;

            Movement = Type.Behavior.Movement.None;
        }
    }
}
