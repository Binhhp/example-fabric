using StudentDomain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace StudentDomain.Interfaces
{
    public interface ISubjectReponsitory
    {
        Task<IEnumerable<Subject>> GetSubjectsAsync();
        Task AddSubjectAsync(Subject subject);
    }
}
