using System;
using System.Collections.Generic;
using System.Fabric;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Data.Collections;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Remoting.V2.FabricTransport.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;
using StudentDomain.Entities;
using StudentDomain.Interfaces;

namespace Student.Subjects
{
    /// <summary>
    /// An instance of this class is created for each service replica by the Service Fabric runtime.
    /// </summary>
    internal sealed class Subjects : StatefulService, ISubjectService
    {
        private ISubjectReponsitory _repo;
        public Subjects(StatefulServiceContext context)
            : base(context)
        { }

        public async Task AddSubjectAsync(Subject subject)
        {
            await _repo.AddSubjectAsync(subject);
        }

        public async Task<Subject[]> GetSubjectsAsync()
        {
            return (await _repo.GetSubjectsAsync()).ToArray();
        }
        protected override IEnumerable<ServiceReplicaListener> CreateServiceReplicaListeners()
        {
            return new[]
            {
                new ServiceReplicaListener(context =>
                    new FabricTransportServiceRemotingListener(context, this))
            };
        }
        protected override async Task RunAsync(CancellationToken cancellationToken)
        {
            _repo = new SubjectReponsitory(this.StateManager);
            var subject = new Subject
            {
                Name = "Lap trinh nang cao"
            };
            var subject2 = new Subject
            {
                Name = "C Sharp"
            };
            var subject3 = new Subject
            {
                Name = "Java"
            };
            var subject4 = new Subject
            {
                Name = "Javascript"
            };
            await _repo.AddSubjectAsync(subject);
            await _repo.AddSubjectAsync(subject2);
            await _repo.AddSubjectAsync(subject3);
            await _repo.AddSubjectAsync(subject4);
        }
    }
}
