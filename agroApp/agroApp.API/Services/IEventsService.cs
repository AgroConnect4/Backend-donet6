using agroApp.API.DTOs;
using System.Threading.Tasks;
using agroApp.Domain.Entities;
using System;
using System.Collections.Generic;

namespace agroApp.API.Services
{
    public interface IEventsService
    {
        Task<List<EventDto>> GetAllEventsAsync();
        Task<EventDto> GetEventByIdAsync(Guid id);
        Task<EventDto> CreateEventAsync(CreateEventDto createEventDto);
        Task UpdateEventAsync(UpdateEventDto updateEventDto, Guid id);
        Task DeleteEventAsync(Guid id);
    }
}