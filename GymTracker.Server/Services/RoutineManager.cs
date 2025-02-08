using Microsoft.EntityFrameworkCore;

namespace GymTracker.Server.Services
{
    public class RoutineManager : IRoutineManager
    {
        private readonly AppDbContext context;

        public RoutineManager(AppDbContext context) 
        {
            this.context = context;
        }

        public List<Routine> GetRoutines()
        {
            List<Routine> routines = context.Routine
                                      .ToList();

            return routines;
        }

        public Routine? GetRoutine(Guid id)
        {
            Routine? routine = context.Routine.FirstOrDefault(r => r.Id == id);

            return routine;
        }

        public Routine UpsertRoutine(Routine routine)
        {
            if (routine.Id != Guid.Empty)
            {
                var existingRoutine = context.Routine.AsNoTracking().FirstOrDefault(r => r.Id == routine.Id);

                if (existingRoutine == null)
                {
                    throw new ArgumentException($"No routine found with ID: {routine.Id}");
                }

                context.Routine.Update(routine);
                context.SaveChanges();
                return routine;
            }

            context.Routine.Add(routine);
            context.SaveChanges();
            return routine;
        }

        public void DeleteRoutine(Guid id)
        {
            Routine? routine = this.GetRoutine(id);

            if (routine == null)
            {
                throw new ArgumentException($"A routine with the name: \"{id}\" does not exist");
            }

            context.Routine.Remove(routine);
            context.SaveChanges();
        }
    }
}
