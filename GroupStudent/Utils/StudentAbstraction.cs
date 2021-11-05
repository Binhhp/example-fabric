using GroupStudent.Interfaces;
using Microsoft.ServiceFabric.Actors;
using Microsoft.ServiceFabric.Actors.Client;
using StudentActor.Interfaces;
using System;

namespace GroupStudent.Utils
{
    public abstract class StudentAbstraction
    {
        protected IStudentActor _student;
        protected IGroupStudent _groupStudent;
        public StudentAbstraction(string id, string fabricUri)
        {
            var actor = new ActorId(id);
            _student = ActorProxy.Create<IStudentActor>(actor, new Uri(fabricUri));
        }
        public IStudentActor GetStudent() => _student;
        public IGroupStudent GetGroupStudent() => _groupStudent;
    }
}
