using GlobalCoders.PSP.BackendApi.Base.Extensions;
using GlobalCoders.PSP.BackendApi.Data;
using GlobalCoders.PSP.BackendApi.ReservationManagment.Entities;
using GlobalCoders.PSP.BackendApi.ReservationManagment.ModelsDto;
using Microsoft.EntityFrameworkCore;

namespace GlobalCoders.PSP.BackendApi.ReservationManagment.Repositories;

public class ReservationRepository : IReservationRepository
{
    private readonly ILogger<ReservationRepository> _logger;
    private readonly IDbContextFactory<BackendContext> _contextFactory;

    public ReservationRepository(ILogger<ReservationRepository> logger, IDbContextFactory<BackendContext> contextFactory)
    {
        _logger = logger;
        _contextFactory = contextFactory;
    }

    public async Task<bool> UpdateAsync(ReservationEntity updateModel)
    {
        try
        {
            await using var context = await _contextFactory.CreateDbContextAsync();
            context.Reservations.Update(updateModel);
            return await context.SaveChangesAsync() > 0;
        }
        catch (Exception e)
        {
            _logger.LogExceptionError(e, nameof(UpdateAsync));
        }

        return false;
    }

    public async Task<bool> CreateAsync(ReservationEntity createModel)
    {
        try
        {
            await using var context = await _contextFactory.CreateDbContextAsync();
            context.Reservations.Add(createModel);
            return await context.SaveChangesAsync() > 0;
        }
        catch (Exception e)
        {
            _logger.LogExceptionError(e, nameof(CreateAsync));
        }

        return false;
    }

    public async Task<(List<ReservationEntity>, int)> GetAllAsync(ReservationFilter filter)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();

        var query = context.Reservations
            .Include(x=>x.Employee)
            .ThenInclude(x=>x.Merchant)
            .Include(x=>x.Service)
            .AsQueryable();
        
        if (!string.IsNullOrWhiteSpace(filter.Name))
        {
            query = query.Where(x => x.DisplayName.Contains(filter.Name));
        }

        if (filter.MerchantId != null)
        {
            query = query.Where(x => x.Employee!.MerchantId == filter.MerchantId);
        }
        
        var totalItems = await query.CountAsync();

        var items =  query
            .OrderBy(x=>x.DisplayName)
            .Skip((filter.Page - 1) * filter.ItemsPerPage)
            .Take(filter.ItemsPerPage)
            .ToList();

        return (items, totalItems);
    }

    public async Task<ReservationEntity?> GetAsync(Guid serviceId, Guid? merchantId)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();

        var querry =   context.Reservations
            .Include(x=>x.Employee)
            .ThenInclude(x=>x.Merchant)
            .Include(x=>x.Service)
            .Where(x => x.Id == serviceId);
       
        if (merchantId.HasValue)
        {
            querry = querry.Where(x => x.Employee != null && x.Employee.MerchantId == merchantId);
        }
         
        var service = await querry
            .FirstOrDefaultAsync();

        return service;
    }

    public async Task<bool> DeleteAsync(Guid organizationId)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();

        var entity = await context.Reservations.FirstOrDefaultAsync(x => x.Id == organizationId);

        if (entity == null)
        {
            return false;
        }

        entity.IsDeleted = true;

        return await context.SaveChangesAsync() > 0;
    }

    public async Task<bool> IsAppointmentExistsAsync(Guid serviceUserId, DateTime appointmentTime, DateTime appointmentEndDate)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();

        var entity = await context.Reservations
            .Include(x=>x.Employee)
            .FirstOrDefaultAsync(
                x => x.EmployeeId == serviceUserId
                     && ((x.ReservationTime <= appointmentTime
                          && x.ReservationEndTime >= appointmentTime) ||
                         (x.ReservationTime <= appointmentEndDate
                          && x.ReservationEndTime >= appointmentEndDate)));

        if (entity == null)
        {
            return false;
        }

        return true;
    }
}