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

        public Routine? GetRoutine(string name)
        {
            Routine? routine = context.Routine.FirstOrDefault(r => r.Name == name);

            return routine;
        }

        public Routine EditRoutine(string name, string newName)
        {
            Routine? routine = this.GetRoutine(name);

            if (routine == null)
            {
                throw new ArgumentException($"Ya existe una rutina con el nombre \"{name}\".");
            }

            routine.Name = newName;

            context.Routine.Update(routine);

            return routine;
        }

        public Routine CreateRoutine(string name)
        {
            bool repeatedRoutine = (this.GetRoutine(name) != null);

            if (repeatedRoutine)
            {
                throw new ArgumentException($"Ya existe una rutina con el nombre \"{name}\".");
            }

            Routine newRoutine = new Routine(name);

            context.Routine.Add(newRoutine);
            context.SaveChanges();

            return newRoutine;
        }

        public void DeleteRoutine(string name)
        {
            Routine? routine = this.GetRoutine(name);

            if (routine == null)
            {
                throw new ArgumentException($"No existe una rutina con el nombre \"{name}\".");
            }

            context.Routine.Remove(routine);
        }
    }
}
