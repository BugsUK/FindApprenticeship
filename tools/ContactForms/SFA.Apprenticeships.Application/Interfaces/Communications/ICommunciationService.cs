﻿namespace SFA.Apprenticeships.Application.Interfaces.Communications
{
    using Domain.Entities;

    public interface ICommunciationService
    {
        void SendMessageToHelpdesk(EmployerEnquiry enquiryData);
    }

}