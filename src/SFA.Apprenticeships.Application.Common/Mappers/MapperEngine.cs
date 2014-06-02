﻿using CuttingEdge.Conditions;

namespace SFA.Apprenticeships.Application.Common.Mappers
{
    using System;
    using AutoMapper;
    using AutoMapper.Mappers;

    public abstract class MapperEngine : IMapper
    {
        private readonly IMappingEngine _mappingEngine;

        protected MapperEngine()
        {
            Mapper = new ConfigurationStore(new TypeMapFactory(), MapperRegistry.Mappers);
            _mappingEngine = new MappingEngine(Mapper);
            Initialize();
        }

        public ConfigurationStore Mapper { get; private set; }

        public object Map(object source, Type sourceType, Type destinationType)
        {
            Condition.Requires("source").IsNotNull();

            var map = _mappingEngine.ConfigurationProvider.FindTypeMapFor(sourceType, destinationType);

            if (map != null)
            {
                return _mappingEngine.Map(source, sourceType, destinationType);
            }

            throw new InvalidOperationException("No mapping configuration registered for mapping " 
                                                + sourceType.FullName +
                                                " to " 
                                                + destinationType.FullName);
        }

        public TDestination Map<TSource, TDestination>(TSource sourceObject)
        {
            return (TDestination) Map(sourceObject, typeof (TSource), typeof (TDestination));
        }

        public abstract void Initialize();
    }
}