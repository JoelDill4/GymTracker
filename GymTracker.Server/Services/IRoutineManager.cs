namespace GymTracker.Server.Services
{
    public interface IRoutineManager
    {
        public List<Routine> GetRoutines();

        public Routine? GetRoutine(Guid id);

        public Routine UpsertRoutine(Routine routine);

        public void DeleteRoutine(Guid id);
    }
}
