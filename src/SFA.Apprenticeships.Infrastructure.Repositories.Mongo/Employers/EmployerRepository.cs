﻿namespace SFA.Apprenticeships.Infrastructure.Repositories.Mongo.Employers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Common;
    using Common.Configuration;
    using Domain.Entities.Raa.Parties;
    using Domain.Raa.Interfaces.Repositories;
    using Domain.Raa.Interfaces.Repositories.Models;
    using Entities;
    using MongoDB.Bson;
    using MongoDB.Driver.Builders;
    using MongoDB.Driver.Linq;
    using Application.Interfaces;

    public class EmployerRepository : GenericMongoClient2<MongoEmployer>, IEmployerReadRepository, IEmployerWriteRepository
    {
        private readonly IMapper _mapper;
        private readonly ILogService _logger;

        public EmployerRepository(IConfigurationService configurationService, IMapper mapper, ILogService logger)
        {
            var config = configurationService.Get<MongoConfiguration>();
            Initialise(config.EmployersDb, "employers");
            _mapper = mapper;
            _logger = logger;
        }

        public Employer GetById(int employerId, bool currentOnly = true)
        {
            _logger.Debug("Called Mongodb to get employer with Id={0}", employerId);

            var mongoEntity = Collection.AsQueryable().SingleOrDefault(e => e.EmployerId == employerId);

            return mongoEntity == null ? null : _mapper.Map<MongoEmployer, Employer>(mongoEntity);
        }

        public Employer GetByEdsUrn(string edsUrn, bool currentOnly = true)
        {
            _logger.Debug("Called Mongodb to get employer with edsUrn={0}", edsUrn);

            var mongoEntity = Collection.AsQueryable().SingleOrDefault(e => e.EdsUrn == edsUrn);

            return mongoEntity == null ? null : _mapper.Map<MongoEmployer, Employer>(mongoEntity);
        }

        public List<Employer> GetByIds(IEnumerable<int> employerIds, bool currentOnly = true)
        {
            var mongoEntities = Collection.Find(Query.In("EmployerId", new BsonArray(employerIds)));

            return mongoEntities.Select(e => _mapper.Map<MongoEmployer, Employer>(e)).ToList();
        }

        public IEnumerable<Employer> Search(EmployerSearchParameters searchParameters)
        {
            throw new NotImplementedException();
        }

        public Employer Save(Employer employer)
        {
            _logger.Debug("Called Mongodb to save employer with ERN={0}", employer.EdsUrn);

            if (employer.EmployerGuid == Guid.Empty)
            {
                employer.EmployerGuid = Guid.NewGuid();
                employer.EmployerId = employer.EmployerGuid.GetHashCode();
            }

            var mongoEntity = _mapper.Map<Employer, MongoEmployer>(employer);

            Collection.Save(mongoEntity);

            _logger.Debug("Saved employer to Mongodb with ERN={0}", employer.EdsUrn);

            return _mapper.Map<MongoEmployer, Employer>(mongoEntity);
        }
    }
}
