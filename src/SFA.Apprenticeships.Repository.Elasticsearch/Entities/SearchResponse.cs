﻿using System.Collections.Generic;

namespace SFA.Apprenticeships.Repository.Elasticsearch.Entities
{
    /// <summary>
    /// Do not change property names
    /// </summary>
    public class SearchResponse<T> where T : new()
    {
        public string Took { get; set; }
        public bool Timed_Out { get; set; }
        public List<Hit<T>> Hits { get; set; }
    }
}
