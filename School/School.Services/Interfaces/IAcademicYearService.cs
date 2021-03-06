﻿namespace School.Services.Interfaces
{
    using System;
    using School.Models;

    public interface IAcademicYearService : IRepositoryService<AcademicYear>
    {
        AcademicYear GetById(Guid id);

        void Add(AcademicYear academicYear, int highestGrade);

        bool AcademicYearExistsInDb(DateTime startDate, DateTime endDate);

        bool AcademicYearUniqueOnEdit(Guid id, DateTime startDate, DateTime endDate);

        void HardDelete(AcademicYear academicYear);
    }
}
