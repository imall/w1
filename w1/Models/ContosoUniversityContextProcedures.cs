﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using w1.Models;

namespace w1.Models
{
    public partial class ContosoUniversityContext
    {
        private IContosoUniversityContextProcedures _procedures;

        public virtual IContosoUniversityContextProcedures Procedures
        {
            get
            {
                if (_procedures is null) _procedures = new ContosoUniversityContextProcedures(this);
                return _procedures;
            }
            set
            {
                _procedures = value;
            }
        }

        public IContosoUniversityContextProcedures GetProcedures()
        {
            return Procedures;
        }
    }

    public partial class ContosoUniversityContextProcedures : IContosoUniversityContextProcedures
    {
        private readonly ContosoUniversityContext _context;

        public ContosoUniversityContextProcedures(ContosoUniversityContext context)
        {
            _context = context;
        }

        public virtual async Task<List<GetMyDeptCoursesResult>> GetMyDeptCoursesAsync(string q, OutputParameter<int> returnValue = null, CancellationToken cancellationToken = default)
        {
            var parameterreturnValue = new SqlParameter
            {
                ParameterName = "returnValue",
                Direction = System.Data.ParameterDirection.Output,
                SqlDbType = System.Data.SqlDbType.Int,
            };

            var sqlParameters = new []
            {
                new SqlParameter
                {
                    ParameterName = "q",
                    Size = 100,
                    Value = q ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.NVarChar,
                },
                parameterreturnValue,
            };
            var _ = await _context.SqlQueryAsync<GetMyDeptCoursesResult>("EXEC @returnValue = [dbo].[GetMyDeptCourses] @q = @q", sqlParameters, cancellationToken);

            returnValue?.SetValue(parameterreturnValue.Value);

            return _;
        }
    }
}
