using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Actors;
using Microsoft.ServiceFabric.Actors.Runtime;
using Microsoft.ServiceFabric.Actors.Client;
using StudentDomain.Constants;
using StudentActor.Interfaces;
using StudentDomain;
using GroupStudent.Interfaces;
using GroupStudent.Utils;

namespace GroupStudent
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
    internal class GroupStudentActor : Actor, IGroupStudent
    {
        /// <summary>
        /// Initializes a new instance of GroupStudentService
        /// </summary>
        /// <param name="actorService">The Microsoft.ServiceFabric.Actors.Runtime.ActorService that will host this actor instance.</param>
        /// <param name="actorId">The Microsoft.ServiceFabric.Actors.ActorId for this actor instance.</param>
        public GroupStudentActor(ActorService actorService, ActorId actorId) 
            : base(actorService, actorId)
        {
        }

        private Func<Guid, IStudentActor> actorCreate = (Guid studentId)
          => (new StudentActorCreate(studentId.ToString(), FabricUriConstants.UriStudentActor)).GetStudent();
        private async Task FakeDataAsync()
        {
            int count = 1;
            while (count < 20)
            {
                var student = new Student
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = Faker.Name.FullName(),
                    Age = Faker.RandomNumber.Next(100)
                };
                await StateManager.TryAddStateAsync(student.Id, student.Id);
                await AddOrUpdateStudentAsync(student);
                count++;
            }
        }
        protected override Task OnActivateAsync()
        {
            ActorEventSource.Current.ActorMessage(this, "Actor activated.");
            return FakeDataAsync();
        }
        public async Task<Student> GetCurrentStudentAsync(Guid studentId)
        {
            var student = await actorCreate(studentId).GetStudentAsync(studentId, new CancellationToken());
            return student;
        }

        public async Task<Stack<Student>> GetListStudentAsync()
        {
            try
            {
                var states = (List<string>)await StateManager.GetStateNamesAsync();
                Stack<Student> students = new Stack<Student>();
                states.ForEach(st => Task.Run(async () => {
                    string studentState = await StateManager.GetStateAsync<string>(st, new CancellationToken());
                    var stud = await actorCreate(new Guid(studentState)).GetStudentAsync(new Guid(studentState), new CancellationToken());
                }));
                return students;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task DeleteStudentAsync(Guid studentId)
        {
            Task deleteStateGroupStudent = new Task(() => StateManager.RemoveStateAsync(studentId.ToString()));
            Task deleteStateStudent = new Task(() => actorCreate(studentId).DeleteAsync(studentId, new CancellationToken()));
            await Task.WhenAll(deleteStateStudent, deleteStateGroupStudent);
        }

        public async Task<Student> AddOrUpdateStudentAsync(Student student)
        {
            try
            {
                _ = await actorCreate(new Guid(student.Id)).PostStudentAsync(student, new CancellationToken());
                return student;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
