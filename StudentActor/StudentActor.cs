using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Actors;
using Microsoft.ServiceFabric.Actors.Runtime;
using Microsoft.ServiceFabric.Actors.Client;
using StudentActor.Interfaces;
using StudentDomain;

namespace StudentActor
{
    /// <remarks>
    /// This class represents an actor.
    /// Every ActorID maps to an instance of this class.
    /// The StatePersistence attribute determines persistence and replication of actor state:
    ///  - Persisted: State is written to disk and replicated.
    ///  - Volatile: State is kept in memory only and replicated.
    ///  - None: State is kept in memory only and not replicated.
    /// </remarks>
    [StatePersistence(StatePersistence.Persisted)]
    [ActorService(Name = "StudentActorService")]
    internal class StudentActor : Actor, IStudentActor
    {
        public StudentActor(ActorService actorService, ActorId actorId) 
            : base(actorService, actorId)
        {
        }
        public async Task<Student> GetStudentAsync(Guid studentId, CancellationToken cancellationToken)
        {
            var student = await StateManager.GetStateAsync<Student>(studentId.ToString(), cancellationToken);
            return student;
        }

        public async Task<Student> PostStudentAsync(Student student, CancellationToken cancellationToken)
        {
            try
            {
                await StateManager.AddOrUpdateStateAsync(student.Id, student, (k, v) => student, cancellationToken);
                return student;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
        public async Task DeleteAsync(Guid studentId, CancellationToken cancellationToken)
        {
            try
            {
                await StateManager.RemoveStateAsync(studentId.ToString(), cancellationToken);
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
    }
}
