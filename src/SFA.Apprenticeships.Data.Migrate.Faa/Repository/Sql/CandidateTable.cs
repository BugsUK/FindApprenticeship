namespace SFA.Apprenticeships.Data.Migrate.Faa.Repository.Sql
{
    using System;
    using System.Collections.Generic;

    public class CandidateTable : ITableSpec
    {
        public string Name => "Candidate";
        public IEnumerable<string> PrimaryKeys => new[] { "CandidateId" };
        public IEnumerable<string> ErrorKeys => PrimaryKeys;
        public bool IdentityInsert => true;
        public decimal BatchSizeMultiplier => 1;
        public IEnumerable<ITableSpec> DependsOn { get; }
        public Action<ITableSpec, dynamic, dynamic> Transform { get; }
        public Func<ITableSpec, dynamic, dynamic, Operation, bool> CanMutate { get; }
    }
}