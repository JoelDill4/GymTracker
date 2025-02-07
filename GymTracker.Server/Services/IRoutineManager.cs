namespace GymTracker.Server.Services
{
    public interface IRoutineManager
    {
        public List<Routine> GetRoutines();

        public Routine? GetRoutine(string name);

        public Routine EditRoutine(string name, string newName);

        public Routine CreateRoutine(string name);

        public void DeleteRoutine(string name);
    }
}
