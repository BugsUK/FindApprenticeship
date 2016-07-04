select top 100 v.VacancyId, v.VacancyOwnerRelationshipIdfrom   vacancy vjoin   vacancyownerrelationship voron     vor.VacancyOwnerRelationshipId = v.VacancyOwnerRelationshipIdwhere  v.vacancystatusid = 2and    vor.statustypeid <> 4UPDATE vacancyownerrelationshipSET    EditedInRaa  = 1,       StatusTypeId = 4WHERE  VacancyOwnerRelationshipId IN (139891, 356820)select top 100 v.VacancyId, v.VacancyOwnerRelationshipId
from   vacancy v
join   vacancyownerrelationship vor
on     vor.VacancyOwnerRelationshipId = v.VacancyOwnerRelationshipId
where  v.vacancystatusid = 2
and    vor.statustypeid <> 4

UPDATE vacancyownerrelationship
SET    EditedInRaa  = 1,
       StatusTypeId = 4
WHERE  VacancyOwnerRelationshipId IN (139891, 356820)
