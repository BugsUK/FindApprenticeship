﻿namespace SFA.Apprenticeships.Infrastructure.Repositories.Mongo.Applications
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Domain.Entities.Applications;
    using MongoDB.Driver.Builders;
    using SFA.Infrastructure.Interfaces;
    using MongoDB.Bson;
    using MongoDB.Driver;
    using System.Collections;
    using Application.Interfaces;
    using Domain.Entities.Collections.Generic;

    public class CommonApplicationRepository
    {
        private readonly ILogService _logger;
        private readonly MongoCollection _collection;
        public CommonApplicationRepository(ILogService logger, MongoCollection collection)
        {
            _logger = logger;
            _collection = collection;
        }

        private class ApplicationCounts : Tuple<int, int>, IApplicationCounts
        {
            public ApplicationCounts(int allCount, int newCount) : base(allCount, newCount)
            { }

            public int AllApplications { get { return base.Item1; } }
            public int NewApplications { get { return base.Item2; } }
        }

        /* Working example from RoboMongo
        db.apprenticeships.mapReduce(
function() {
    emit(NumberInt(this.Vacancy._id), { allCount: 1, newCount: (this.DateLastViewed === null && this.Status == 30 ? 1 : 0) });
},
function(key, counts) {
    var reducedCounts = { allCount: 0, newCount: 0 };
    for (var i = 0; i < counts.length; i++) {
        reducedCounts.allCount += counts[i].allCount;
        reducedCounts.newCount += counts[i].newCount;
    }
    return reducedCounts;
},
{
    query: { "Vacancy._id" : { "$in" : [445650] } },
    finalize: function(key, val) { return { allCount: NumberInt(val.allCount), newCount: NumberInt(val.newCount) } },
    out: { replace: "leo_output" }
}
);
*/

        public IReadOnlyDictionary<int, IApplicationCounts> GetCountsForVacancyIds(IEnumerable<int> vacancyIds)
        {
            _logger.Debug($"Calling repository to get applications count for vacancy with {vacancyIds.Count()} ids: {string.Join(", ", vacancyIds.Take(10))}");

#pragma warning disable 618 // Deliberately using obsolete method as I think it has come back in the current driver
            var mapReduceResult = _collection.MapReduce(
                Query.And(Query.In("Vacancy._id", vacancyIds.Select(v => (BsonValue)v)), Query.GTE("Status", ApplicationStatuses.Submitted)), @"
                function() {
                    emit(NumberInt(this.Vacancy._id), { allCount: 1, newCount: (this.DateLastViewed === null && this.Status == " + (int)ApplicationStatuses.Submitted + @" ? 1 : 0) });
                }
", @"
                function(key, counts) {
                    var reducedCounts = { allCount: 0, newCount: 0 };
                    for (var i = 0; i < counts.length; i++) {
                        reducedCounts.allCount += counts[i].allCount;
                        reducedCounts.newCount += counts[i].newCount;
                    }
                    return reducedCounts;
                }
            ");
#pragma warning restore 618

            var results = new Dictionary<int, IApplicationCounts>();
            foreach (var bson in mapReduceResult.GetResults())
            {
                var vacancyId = bson["_id"].AsInt32;
                var allCount = (int)bson["value"]["allCount"].AsDouble;
                var newCount = (int)bson["value"]["newCount"].AsDouble;

                // TODO: This should be allCount, newCount. This is a deliberate bug introduced for compatibility with previous implementation
                results.Add(vacancyId, new ApplicationCounts(allCount, newCount));
            }

            return new SparseDictionary<int, IApplicationCounts>(results, new ApplicationCounts(0,0));
        }
    }
}
