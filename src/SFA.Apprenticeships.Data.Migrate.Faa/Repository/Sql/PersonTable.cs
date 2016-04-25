namespace SFA.Apprenticeships.Data.Migrate.Faa.Repository.Sql
{
    using System;
    using System.Collections.Generic;

    public class PersonTable : ITableSpec
    {
        public string Name => "Person";
        public IEnumerable<string> PrimaryKeys => new[] { "PersonId" };
        public IEnumerable<string> ErrorKeys => PrimaryKeys;
        public bool IdentityInsert => false;
        public decimal BatchSizeMultiplier => 1;
        public IEnumerable<ITableSpec> DependsOn { get; }
        public Action<ITableSpec, dynamic, dynamic> Transform { get; }
        public Func<ITableSpec, dynamic, dynamic, Operation, bool> CanMutate { get; }
    }
}