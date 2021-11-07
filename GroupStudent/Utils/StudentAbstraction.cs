using GroupStudent.Interfaces;
using Microsoft.ServiceFabric.Actors;
using Microsoft.ServiceFabric.Actors.Client;
using Microsoft.ServiceFabric.Services.Client;
using Microsoft.ServiceFabric.Services.Remoting.Client;
using Microsoft.ServiceFabric.Services.Remoting.V2.FabricTransport.Client;
using StudentActor.Interfaces;
using StudentDomain.Interfaces;
using System;

namespace GroupStudent.Utils
{
    public abstract class StudentAbstraction
    {
        protected IStudentActor _student;
        protected IGroupStudent _groupStudent;
        protected ISubjectService _subject;
        public StudentAbstraction(string id, string fabricUri, bool isGroup = false)
        {
            var actor = new ActorId(id);
            if(!isGroup) _student = ActorProxy.Create<IStudentActor>(actor, new Uri(fabricUri));
            else _groupStudent = ActorProxy.Create<IGroupStudent>(actor, new Uri(fabricUri));
            if (string.IsNullOrEmpty(id))
            {
                var proxyFactory = new ServiceProxyFactory(x => new FabricTransportServiceRemotingClientFactory());
                _subject = proxyFactory.CreateServiceProxy<ISubjectService>(new Uri(fabricUri), new ServicePartitionKey(0));
            }
        }
        public IStudentActor GetStudent() => _student;
        public IGroupStudent GetGroupStudent() => _groupStudent;
        public ISubjectService GetSubjectStateful() => _subject;
    }
}
