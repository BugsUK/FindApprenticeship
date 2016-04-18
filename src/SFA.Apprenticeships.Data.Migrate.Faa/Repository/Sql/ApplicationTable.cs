namespace SFA.Apprenticeships.Data.Migrate.Faa.Repository.Sql
{
    using System;
    using System.Collections.Generic;
    using Migrate;

    public class ApplicationTable : ITableSpec
    {
        public string Name => "Application";
        public IEnumerable<string> PrimaryKeys => new []{"ApplicationId"};
        public decimal BatchSizeMultiplier => 1;
        public IEnumerable<ITableSpec> DependsOn { get; }
        public Action<ITableSpec, dynamic, dynamic> Transform { get; }
        public Func<ITableSpec, dynamic, dynamic, Operation, bool> CanMutate { get; }
    }
}