using GlobalCoders.PSP.BackendApi.Base.Factories;
using GlobalCoders.PSP.BackendApi.Base.ModelsDto;
using GlobalCoders.PSP.BackendApi.EmployeeManagment.Entities;
using GlobalCoders.PSP.BackendApi.ReservationManagment.Factories;
using GlobalCoders.PSP.BackendApi.ReservationManagment.ModelsDto;
using GlobalCoders.PSP.BackendApi.ReservationManagment.Repositories;
using GlobalCoders.PSP.BackendApi.ServicesManagement.Services;

namespace GlobalCoders.PSP.BackendApi.ReservationManagment.Services;


public class ReservationService : IReservationService
{
    private readonly IServicesService _servicesService;
    private readonly IReservationRepository _reservationRepository;

    public ReservationService(IServicesService servicesService,
        IReservationRepository reservationRepository)
    {
        _servicesService = servicesService;
        _reservationRepository = reservationRepository;
    }
    public async Task<ReservationResponseModel?> GetAsync(Guid reservationId, Guid? merchantId)
    {
        var entity = await _reservationRepository.GetAsync(reservationId, merchantId);
        
        if(entity == null)
        {
            return null;
        }
        
        return ReservationResponseModelFactory.Create(entity);
    }

    public async Task<BasePagedResponse<ReservationListModel>?> GetAllAsync(ReservationFilter filter)
    {
        var entities = await _reservationRepository.GetAllAsync(filter);
        
        var models = entities.items.Select(ReservationListModelFactory.Create).ToList();

        return BasePagedResopnseFactory.Create(models, filter, entities.totalItems);
    }

    public async Task<(bool, string)> CreateAsync(ReservationCreateModel reservationCreateModel, EmployeeEntity serviceUser)
    {
        var service = await _servicesService.GetAsync(reservationCreateModel.Service);

        if(service == null)
        {
            return (false, "Service not found");
        }

        var (isAvailable, message ) = await IsTimeAvailableForEmployeeAsync(serviceUser, reservationCreateModel.AppointmentTime, service.DurationMin);
       
        if(!isAvailable)
        {
            return (false, message);
        }

        var createModel = ReservationEntityFactory.Create(reservationCreateModel, service, serviceUser);
        
        return (await _reservationRepository.CreateAsync(createModel), "Failed to create reservation");
    }

    public async Task<List<TimeSlot>?> GetTimeSlotsAsync(TimeSlotRequest request, EmployeeEntity user)
    {
        var employScheduleForDay = user.WorkingSchedule.FirstOrDefault(x => x.DayOfWeek == request.DateTime.DayOfWeek);
        
        if(employScheduleForDay == null)
        {
            return null;
        }
        
        var (reservationsForDay, totalReservations) = await _reservationRepository.GetAllAsync(ReservationFilterFactory.
            CreateForAllItems(request.EmployeeId,
                GetDateWithTime(request.DateTime, employScheduleForDay.StartTime),
                GetDateWithTime(request.DateTime, employScheduleForDay.EndTime)
                ));

        var timeSlots = new List<TimeSlot>();
        if (totalReservations == 0)
        {
            timeSlots.Add(
                TimeSlotFactory.Create(
                    GetDateWithTime(request.DateTime, employScheduleForDay.StartTime),
                    GetDateWithTime(request.DateTime, employScheduleForDay.EndTime)
                ));
        }

        var processingTime = GetDateWithTime(request.DateTime, employScheduleForDay.StartTime);
        foreach (var reservation in reservationsForDay)
        {
            timeSlots.Add(
                TimeSlotFactory.Create(
                    processingTime,
                    reservation.ReservationTime
                ));

            processingTime = reservation.ReservationEndTime;
        }
        
        timeSlots.Add(
            TimeSlotFactory.Create(
                processingTime,
                GetDateWithTime(request.DateTime, employScheduleForDay.EndTime)
            ));

        
        if(request.MinimumDurationMin == null ||  request.MinimumDurationMin <= 0)
        {
            return timeSlots.Where(x => x.DurationMin >= 1).ToList();
        }

        return timeSlots.Where(x => x.DurationMin >= request.MinimumDurationMin).ToList();
    }

    private static DateTime GetDateWithTime(DateTime requestDateTime, TimeSpan startTime)
    {
        return new DateTime(
            requestDateTime.Year,
            requestDateTime.Month,
            requestDateTime.Day,
            startTime.Hours,
            startTime.Minutes,
            startTime.Seconds, 
            DateTimeKind.Utc);
    }

    private async Task<(bool, string)> IsTimeAvailableForEmployeeAsync(EmployeeEntity serviceUser, DateTime appointmentTime, int serviceDurationMin)
    {
        if(serviceUser.WorkingSchedule.Count == 0)
        {
            return (false, "Employee has no working schedule");
        }
        
        var schedule = serviceUser.WorkingSchedule.ToList();
        var scheduleForAppointmentDate = schedule.Find(x=>x.DayOfWeek == appointmentTime.DayOfWeek);

        if (scheduleForAppointmentDate == null)
        {
            return (false, "Employee is not working on this day");
        }

        var appointmentEndDate = appointmentTime.AddMinutes(serviceDurationMin);
        if(appointmentEndDate.TimeOfDay > scheduleForAppointmentDate.EndTime)
        {
            return (false, "Employee is not available at this time");
        }
        
        if(appointmentTime.TimeOfDay < scheduleForAppointmentDate.StartTime)
        {
            return (false, "Employee is not available at this time");
        }

        if (!await _reservationRepository.IsAppointmentExistsAsync(serviceUser.Id, appointmentTime, appointmentEndDate))
        {
            return (true, string.Empty);
        }
        
        return (false, "Employee has anther appointment at this time");
    }
}