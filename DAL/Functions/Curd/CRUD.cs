using DAL.DataContext;
using DAL.Functions.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Functions.Curd
{
    public class CRUD : ICRUD   
    {
        public async Task<T> Create<T>(T objectForDb) where T : class
        {
            try
            {
                using (var context = new DatabaseContext(DatabaseContext.Options.DataBaseOptions))
                {
                    await context.AddAsync<T>(objectForDb);
                    await context.SaveChangesAsync();
                    return objectForDb;
                }
            }
            catch
            {
                throw;
            }


        }



        public async Task<T> Read<T>(Int64 entityId) where T : class
        {
            try
            {
                using (DatabaseContext context = new DatabaseContext(DatabaseContext.Options.DataBaseOptions))
                {
                    T result = await context.FindAsync<T>(entityId);
                    return result;
                }
            }
            catch
            {
                throw;
            }
        }

        public async Task<List<T>> ReadAll<T>() where T : class
        {
            try
            {
                using (DatabaseContext context = new DatabaseContext(DatabaseContext.Options.DataBaseOptions))
                {
                    var result = await context.Set<T>().ToListAsync();
                    return result;
                }
            }
            catch
            {
                throw;
            }
        }

        public async Task<T> Update<T>(T objectToUpdate, Int64 entityId) where T : class
        {
            try
            {
                using (var context = new DatabaseContext(DatabaseContext.Options.DataBaseOptions))
                {
                    var objectFound = await context.FindAsync<T>(entityId);
                    if (objectFound != null)
                    {
                        context.Entry(objectFound).CurrentValues.SetValues(objectToUpdate);
                        await context.SaveChangesAsync();
                    }
                    return objectFound;
                }
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> Delete<T>(Int64 entityId) where T : class
        {
            try
            {
                using (DatabaseContext context = new DatabaseContext(DatabaseContext.Options.DataBaseOptions))
                {
                    T recordToDelete = await context.FindAsync<T>(entityId);
                    if (recordToDelete != null)
                    {
                        context.Remove(recordToDelete);
                        await context.SaveChangesAsync();
                        return true;
                    }
                    return false;
                }
            }
            catch
            {
                throw;
            }
        }

    }
}
