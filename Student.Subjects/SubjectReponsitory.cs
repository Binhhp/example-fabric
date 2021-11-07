using Microsoft.ServiceFabric.Data;
using Microsoft.ServiceFabric.Data.Collections;
using StudentDomain.Entities;
using StudentDomain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Student.Subjects
{
    public class SubjectReponsitory : ISubjectReponsitory
    {
        private readonly IReliableStateManager _stateManager;
        public SubjectReponsitory(IReliableStateManager stateManager)
        {
            _stateManager = stateManager;
        }
        public async Task AddSubjectAsync(Subject subject)
        {
            IReliableDictionary<Guid, Subject> subjects = 
                await _stateManager.GetOrAddAsync<IReliableDictionary<Guid, Subject>>("subjects");
            using(ITransaction tx = _stateManager.CreateTransaction())
            {
                await subjects.AddOrUpdateAsync(tx, subject.Id, subject, (id, value) => subject);
                await tx.CommitAsync();
            }
        }

        public async Task<IEnumerable<Subject>> GetSubjectsAsync()
        {
            IReliableDictionary<Guid, Subject> subjects =
                await _stateManager.GetOrAddAsync<IReliableDictionary<Guid, Subject>>("subjects");
            var result = new List<Subject>();
            using(ITransaction tx = _stateManager.CreateTransaction())
            {
                var allSubjects = await subjects.CreateEnumerableAsync(tx, EnumerationMode.Unordered);
                using(var enumrator = allSubjects.GetAsyncEnumerator())
                {
                    while(await enumrator.MoveNextAsync(CancellationToken.None))
                    {
                        var data = enumrator.Current;
                        result.Add(data.Value);
                    }
                }
            }
            return result;
        }
    }
}
