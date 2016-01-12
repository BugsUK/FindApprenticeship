namespace SFA.Apprenticeships.Infrastructure.Repositories.Vacancies.Sql.Mappers
{
    using System;
    using System.Collections.Generic;
    using AutoMapper;
    using AutoMapper.Mappers;
    using Domain.Entities.Vacancies.ProviderVacancies.Apprenticeship;
    using SFA.Infrastructure.Interfaces;
    using Vacancy = SFA.Apprenticeships.NewDB.Domain.Entities.Vacancy;
    using Reference = SFA.Apprenticeships.NewDB.Domain.Entities;
    using Domain.Entities.Vacancies.ProviderVacancies;
    using System.Collections;
    using System.Linq.Expressions;

    // TODO: Copied because I don't want to depend on SFA.Apprenticeships.Infrastructure.Common.Mappers because this depends on lots of other things
    // But don't want to move it to SFA.Infrastructure project because that would then depend on AutoMapper
    public abstract class MapperEngine : IMapper
    {
        private readonly IMappingEngine _mappingEngine;

        protected MapperEngine()
        {
            Mapper = new ConfigurationStore(new TypeMapFactory(), MapperRegistry.Mappers);
            _mappingEngine = new MappingEngine(Mapper);
            Initialise();
        }

        public ConfigurationStore Mapper { get; private set; }

        public TDestination Map<TSource, TDestination>(TSource sourceObject)
        {
            return _mappingEngine.Map<TDestination>(sourceObject);
        }

        public abstract void Initialise();
    }

    public class ApprenticeshipVacancyMappers : MapperEngine
    {
        public override void Initialise()
        {
            var wageTypeMap = new CodeEnumMap<WageType>
            {
                { "X", WageType.ApprenticeshipMinimumWage } // TODO
            };

            var durationTypeMap = new CodeEnumMap<DurationType>
            {
                { "X", DurationType.Months } // TODO
            };

            var trainingTypeMap = new CodeEnumMap<TrainingType>
            {
                { "F", TrainingType.Frameworks },
                { "S", TrainingType.Standards },
                { null, TrainingType.Unknown }, // TODO: null = blank??
            };

            var apprenticeshipLevelMap = new CodeEnumMap<ApprenticeshipLevel>
            {
                { "2", ApprenticeshipLevel.Intermediate },
                // TODO
            };

            var vacancyStatusMap = new CodeEnumMap<ProviderVacancyStatuses>
            {
                { "LIV", ProviderVacancyStatuses.Live },
                // TODO
            };

            Mapper.CreateMap<ApprenticeshipVacancy, Vacancy.Vacancy>()
                .MapMemberFrom(v => v.WorkingWeekText, av => av.WorkingWeek)
                .MapMemberFrom(v => v.WageTypeCode, av => wageTypeMap.EnumToCode[av.WageType])
                .MapMemberFrom(v => v.WageValue, av => av.Wage)
                // TODO: WageUnit
                .MapMemberFrom(v => v.DurationTypeCode, av => durationTypeMap.EnumToCode[av.DurationType])
                .MapMemberFrom(v => v.DurationValue, av => av.Duration)
                .MapMemberFrom(v => v.AV_InterviewStartDate, av => av.InterviewStartDate)
                .MapMemberFrom(v => v.PossibleStartDateDate, av => av.PossibleStartDate)
                .MapMemberFrom(v => v.TrainingTypeCode, av => trainingTypeMap.EnumToCode[av.TrainingType])
                .MapMemberFrom(v => v.VacancyStatusCode, av => vacancyStatusMap.EnumToCode[av.Status])
                .MapMemberFrom(v => v.LevelCode, av => apprenticeshipLevelMap.EnumToCode[av.ApprenticeshipLevel])
                .MapMemberFrom(v => v.LevelCodeComment, av => av.ApprenticeshipLevelComment)
                .MapMemberFrom(v => v.VacancyId, av => av.EntityId)

                .ForMember(v => v.WageValue, opt => opt.MapFrom(av => av.Wage));

            Mapper.CreateMap<Vacancy.Vacancy, ApprenticeshipVacancy>()
                .MapMemberFrom(av => av.WorkingWeek, v => v.WorkingWeekText)
                .MapMemberFrom(av => av.WageType, v => wageTypeMap.CodeToEnum[v.WageTypeCode])
                .MapMemberFrom(av => av.Wage, v => v.WageValue)
                // TODO: WageUnit
                .MapMemberFrom(av => av.DurationType, v => durationTypeMap.CodeToEnum[v.DurationTypeCode])
                .MapMemberFrom(av => av.Duration, v => v.DurationValue)
                .MapMemberFrom(av => av.InterviewStartDate, v => v.AV_InterviewStartDate)
                .MapMemberFrom(av => av.PossibleStartDate, v => v.PossibleStartDateDate)
                .MapMemberFrom(av => av.TrainingType, v => trainingTypeMap.CodeToEnum[v.TrainingTypeCode])
                .MapMemberFrom(av => av.Status, v => vacancyStatusMap.CodeToEnum[v.VacancyStatusCode])
                .MapMemberFrom(av => av.ApprenticeshipLevel, v => apprenticeshipLevelMap.CodeToEnum[v.LevelCode])
                .MapMemberFrom(av => av.ApprenticeshipLevelComment, v => v.LevelCodeComment)
                .MapMemberFrom(av => av.EntityId, v => v.VacancyId)

                // Need to map the following separately
                .ForMember(v => v.Ukprn, opt => opt.Ignore())
                .ForMember(v => v.ProviderSiteEmployerLink, opt => opt.Ignore())
                .ForMember(v => v.FrameworkCodeName, opt => opt.Ignore())

                // VGA -> temp
                .ForMember(v => v.FrameworkCodeNameComment, opt => opt.Ignore())
                .ForMember(v => v.WorkingWeekComment, opt => opt.Ignore())
                .ForMember(v => v.AdditionalLocationInformation, opt => opt.Ignore())
                .ForMember(v => v.LocationAddresses, opt => opt.Ignore())
                .ForMember(v => v.IsEmployerLocationMainApprenticeshipLocation, opt => opt.Ignore())
                .ForMember(v => v.NumberOfPositions, opt => opt.Ignore())
                .ForMember(v => v.WageUnit, opt => opt.Ignore())
                .ForMember(v => v.DateSubmitted, opt => opt.Ignore())
                .ForMember(v => v.DateStartedToQA, opt => opt.Ignore())
                .ForMember(v => v.DateCreated, opt => opt.Ignore())
                .ForMember(v => v.DateUpdated, opt => opt.Ignore())
                .MapMemberFrom(av => av.OfflineVacancy, v => v.IsDirectApplication)
                .MapMemberFrom(av => av.OfflineApplicationUrl, v=> v.DirectApplicationUrl)
                .MapMemberFrom(av => av.OfflineApplicationUrlComment, v=> v.DirectApplicationUrlComment)
                .MapMemberFrom(av => av.OfflineApplicationInstructions, v=> v.DirectApplicationInstructions)
                .MapMemberFrom(av => av.OfflineApplicationInstructionsComment, v=> v.DirectApplicationInstructionsComment)
                
                
                // END VGA

                .ForMember(v => v.Wage, opt => opt.MapFrom(av => av.WageValue));
        }
    }

    public static class IMappingExpressionExtensions
    {
        public static IMappingExpression<TSource, TDestination> MapMemberFrom<TSource, TDestination, TMember>(
            this IMappingExpression<TSource, TDestination> mappingExpression,
            Expression<Func<TDestination, object>> destinationMember,
            Expression<Func<TSource, TMember>> mapFunction)
        {
            return mappingExpression.ForMember(destinationMember, opt => opt.MapFrom<TMember>(mapFunction));
        }
    }

    public class CodeEnumMap<T> : IEnumerable<KeyValuePair<string, T>>
    {
        private Map<string, T> _map;

        public CodeEnumMap()
        {
            _map = new Map<string, T>();
        }

        public void Add(string codeValue, T enumValue)
        {
            _map.Add(codeValue ?? "**NULL**", enumValue); // TODO: Outside the otherwise
        }

        public IEnumerator<KeyValuePair<string, T>> GetEnumerator()
        {
            return _map.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _map.GetEnumerator(); // TODO: Call the right one
        }

        public Map<string, T>.Indexer<string, T> CodeToEnum { get { return _map.Forward; } }
        public Map<string, T>.Indexer<T, string> EnumToCode { get { return _map.Reverse; } }
    }

    public class Map<T1, T2> : IEnumerable<KeyValuePair<T1, T2>>
    {
        private Dictionary<T1, T2> _forward = new Dictionary<T1, T2>();
        private Dictionary<T2, T1> _reverse = new Dictionary<T2, T1>();

        public Map()
        {
            this.Forward = new Indexer<T1, T2>(_forward);
            this.Reverse = new Indexer<T2, T1>(_reverse);
        }

        public class Indexer<T3, T4>
        {
            private Dictionary<T3, T4> _dictionary;
            public Indexer(Dictionary<T3, T4> dictionary)
            {
                _dictionary = dictionary;
            }
            public T4 this[T3 index]
            {
                get { return _dictionary[index]; }
                set { _dictionary[index] = value; }
            }
        }

        public void Add(T1 t1, T2 t2)
        {
            _forward.Add(t1, t2);
            _reverse.Add(t2, t1);
        }

        public IEnumerator<KeyValuePair<T1, T2>> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        public Indexer<T1, T2> Forward { get; private set; }
        public Indexer<T2, T1> Reverse { get; private set; }

        public T2 this[T1 index]
        {
            get { return Forward[index]; }
        }
    }
}
