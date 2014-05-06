﻿using System.Collections.Generic;

namespace SFA.Apprenticeships.Repository.Elasticsearch.Entities
{
    /// <summary>
    /// Do not change property names!
    /// </summary>
    public class Hit<T> where T : new()
    {
        public string Total { get; set; }
        public string Max_Score { get; set; }
        public List<Item<T>> Hits { get; set; }
    }
}
