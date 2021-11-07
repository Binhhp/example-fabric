using Microsoft.ServiceFabric.Services.Remoting;
using StudentDomain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace StudentDomain.Interfaces
{
    public interface ISubjectService : IService
    {
        Task<Subject[]> GetSubjectsAsync();
        Task AddSubjectAsync(Subject subject);
    }
}
