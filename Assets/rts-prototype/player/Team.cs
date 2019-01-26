namespace DefaultNamespace
{
    public class Team
    {
        public short TeamNumber { get; private set; }
        public short PlayerControllerId { get; private set; }
        public string Name { get; private set; }

        public Team(short teamNumber, short playerControllerId, string name)
        {
            TeamNumber = teamNumber;
            PlayerControllerId = playerControllerId;
            Name = name;
        }
    }
}