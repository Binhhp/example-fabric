using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Actors;
using Microsoft.ServiceFabric.Actors.Remoting.FabricTransport;
using Microsoft.ServiceFabric.Services.Remoting;
using StudentDomain;

[assembly: FabricTransportActorRemotingProvider(RemotingListenerVersion = RemotingListenerVersion.V2_1, RemotingClientVersion = RemotingClientVersion.V2_1)]
namespace StudentActor.Interfaces
{
    /// <summary>
    /// This interface defines the methods exposed by an actor.
    /// Clients use this interface to interact with the actor that implements it.
    /// </summary>
    public interface IStudentActor : IActor
    {
        Task<Student> GetStudentAsync(Guid studentId, CancellationToken cancellationToken);
        Task<Student> PostStudentAsync(Student student, CancellationToken cancellationToken);
        Task DeleteAsync(Guid studentId, CancellationToken cancellationToken);
    }
}
