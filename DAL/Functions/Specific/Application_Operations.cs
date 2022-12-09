using DAL.DataContext;
using DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Functions.Specific
{
    internal class Application_Operations 
    {
        public async Task<Application> AddFullApplication(Int64 gradeId, Int64 applicationStatusId, Int32 schoolYear, string firstName, string surname, DateTime birthDate, string email, string contactNumber)
        {
            try
            {
                using (var context = new DatabaseContext(DatabaseContext.Options.DataBaseOptions))
                {
                    using (var transaction = await context.Database.BeginTransactionAsync())
                    {
                       
                        try
                        {
                           
                            var applicant = new Applicants
                            {
                                Applicant_Name = firstName,
                                Applicant_Surname = surname,
                                Applicant_BirthDate = birthDate,
                                Contact_Email = email,
                                Contact_Number = contactNumber
                            };
                            var trackingApplicant = await context.applicants.AddAsync(applicant);
                            
                            await context.SaveChangesAsync();

                            
                            var application = new Application
                            {
                                Applicant_ID = applicant.Applicant_ID,
                                ApplicationStatus_ID = applicationStatusId,
                                Grade_ID = gradeId,
                                SchoolYear = schoolYear

                            };
                            var trackingApplication = await context.application.AddAsync(application);
                            await context.SaveChangesAsync();
                            
                            await transaction.CommitAsync();
                            application.Applicants = applicant;
                           
                            return application;
                        }
                        catch
                        {
                           
                            await transaction.RollbackAsync();
                            throw;
                        }
                    }
                }
            }
            catch
            {
                throw;
            }
        }

     

    }
}
