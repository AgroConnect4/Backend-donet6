using agroApp.API.DTOs;
using agroApp.Domain.Entities;
using agroApp.API.Services;
using agroApp.Infra.Data.Repositories;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http; // Adicione o namespace para HttpContextAccessor
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text; // Adicione o namespace para Encoding
using Microsoft.Extensions.Logging; 
using Microsoft.Extensions.Configuration;

namespace agroApp.API.Services
{
    public class EventsService : IEventsService
    {
        private readonly IEventRepository _eventRepository;
        private readonly IUserRepository _userRepository;
        private readonly INotificationRepository _notificationRepository;
        private readonly UserManager<User> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<EventsService> _logger; // Adicione o Logger
        private readonly IConfiguration _configuration; // Adicione o IConfiguration
        private readonly IConnectionService _connectionService;
        private readonly INotificationService _notificationService;
        //private readonly ICommentService _commentService;
        //private readonly IShareService _shareService;
        private readonly IEventCommentService _eventCommentService; // Injeção de dependência
        private readonly IEventShareService _eventShareService; 
        private readonly IEventCommentRepository _eventCommentRepository; // Add this
        private readonly IEventShareRepository _eventShareRepository;

        public EventsService(IEventRepository eventRepository, 
                             IUserRepository userRepository,
                             UserManager<User> userManager, 
                             IHttpContextAccessor httpContextAccessor, // Injeta HttpContextAccessor
                             ILogger<EventsService> logger, // Injeta o Logger
                             IConfiguration configuration,
                            IConnectionService connectionService,
                            INotificationService notificationService,
                            INotificationRepository notificationRepository,
                            //ICommentService commentService, 
                            //IShareService shareService,
                            IEventCommentService eventCommentService, 
                            IEventShareService eventShareService,
                            IEventCommentRepository eventCommentRepository,
                            IEventShareRepository eventShareRepository)
        {
            _eventRepository = eventRepository;
            _userRepository = userRepository;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
            _configuration = configuration;
            _connectionService = connectionService;
            _notificationService = notificationService;
            _notificationRepository = notificationRepository;
            //_commentService = commentService;
            //_shareService = shareService;
            _eventCommentService = eventCommentService; 
            _eventShareService = eventShareService;
            _eventCommentRepository = eventCommentRepository;
            _eventShareRepository = eventShareRepository;
        }

        private Guid GetUserIdFromToken()
{
    var httpContextAccessor = _httpContextAccessor; //Assuming you have this injected.
    var configuration = _configuration; // Assuming you have this injected.
    var logger = _logger; // Assuming you have this injected.

    if (httpContextAccessor == null || httpContextAccessor.HttpContext == null)
    {
        logger.LogError("HttpContextAccessor is null or HttpContext is null.");
        throw new InvalidOperationException("Cannot access HTTP context.");
    }

    var authorizationHeader = httpContextAccessor.HttpContext.Request.Headers["Authorization"];
    if (!authorizationHeader.Any())
    {
        logger.LogError("Authorization header not found.");
        throw new UnauthorizedAccessException("Authorization header is missing.");
    }

    var token = authorizationHeader.FirstOrDefault().Split(" ").LastOrDefault();
    if (string.IsNullOrEmpty(token))
    {
        logger.LogError("JWT token not found in Authorization header.");
        throw new UnauthorizedAccessException("JWT token is missing.");
    }

    try
    {
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = configuration["Jwt:Issuer"],
            ValidAudience = configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"])),
            ClockSkew = TimeSpan.Zero
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        SecurityToken validatedToken;
        var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out validatedToken);

        var userIdClaim = principal.FindFirst(ClaimTypes.NameIdentifier);
        if (userIdClaim == null || string.IsNullOrEmpty(userIdClaim.Value))
        {
            logger.LogError("User ID claim not found in JWT token.");
            throw new UnauthorizedAccessException("User ID claim is missing.");
        }

        Guid userId;
        if (Guid.TryParse(userIdClaim.Value, out userId))
        {
            return userId;
        }
        else
        {
            logger.LogError("Invalid User ID format in JWT token.");
            throw new UnauthorizedAccessException("Invalid User ID format in JWT token.");
        }
    }
    catch (SecurityTokenException ex)
    {
        logger.LogError(ex, "Error validating JWT token: {Message}", ex.Message);
        throw new UnauthorizedAccessException("Invalid JWT token.");
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "Unexpected error retrieving User ID from JWT token.");
        throw;
    }
}

        public async Task<List<EventDto>> GetAllEventsAsync()
        {
            var events = await _eventRepository.GetAllEventsAsync();

            // Usando Task.WhenAll para aguardar todos os eventos:
            var eventDtos = await Task.WhenAll(events.Select(async e => new EventDto
            {
                Id = e.Id,
                Name = e.Name,
                ImageUrl = e.ImageUrl,
                // Obtemos o organizador do evento:
                OrganizerId = e.UserId,
                StartDateTime = e.StartDateTime,
                EndDateTime = e.EndDateTime,
                Location = e.Location,
                Description = e.Description,
                ProductImages = e.ProductImages
            }));

            return eventDtos.ToList();
        }

        public async Task<EventDto> GetEventByIdAsync(Guid id)
        {
            var @event = await _eventRepository.GetEventByIdAsync(id);
            if (@event == null)
            {
                return null;
            }

            return new EventDto
            {
                Id = @event.Id,
                Name = @event.Name,
                ImageUrl = @event.ImageUrl,
                // Obtemos o organizador do evento:
                OrganizerId = @event.UserId, 
                EndDateTime = @event.EndDateTime,
                Location = @event.Location,
                Description = @event.Description,
                ProductImages = @event.ProductImages
            };
        }

        public async Task<EventDto> CreateEventAsync(CreateEventDto createEventDto)
        {
            // Obter o ID do usuário do token JWT
            var userId = GetUserIdFromToken();

            var @event = new Event
            {
                Name = createEventDto.Name,
                ImageUrl = createEventDto.ImageUrl,
                UserId = userId, // Use o ID do usuário do token
                StartDateTime = createEventDto.StartDateTime,
                EndDateTime = createEventDto.EndDateTime,
                Location = createEventDto.Location,
                Description = createEventDto.Description,
                ProductImages = createEventDto.ProductImages
            };

            var createdEvent = await _eventRepository.AddEventAsync(@event);

            await SendEventNotifications(createdEvent);

            return new EventDto
            {
                Id = createdEvent.Id,
                Name = createdEvent.Name,
                ImageUrl = createdEvent.ImageUrl,
                OrganizerId = createdEvent.UserId,
                StartDateTime = createdEvent.StartDateTime,
                EndDateTime = createdEvent.EndDateTime,
                Location = createdEvent.Location,
                Description = createdEvent.Description,
                ProductImages = createdEvent.ProductImages
            };
        }

        public async Task UpdateEventAsync(UpdateEventDto updateEventDto, Guid id)
        {
            // Obtenha o UserId do token JWT
            var userId = GetUserIdFromToken();

            var existingEvent = await _eventRepository.GetEventByIdAsync(id);

            if (existingEvent == null)
            {
                return; // Ou lance uma exceção
            }

            existingEvent.Name = updateEventDto.Name;
            existingEvent.ImageUrl = updateEventDto.ImageUrl;
            existingEvent.StartDateTime = updateEventDto.StartDateTime;
            existingEvent.EndDateTime = updateEventDto.EndDateTime;
            existingEvent.Location = updateEventDto.Location;
            existingEvent.Description = updateEventDto.Description;
            existingEvent.ProductImages = updateEventDto.ProductImages;

            // Atualize o UserId do evento com o ID do usuário autenticado
            existingEvent.UserId = userId; // Use o ID do usuário do token

            await _eventRepository.UpdateEventAsync(existingEvent);
        }

        public async Task DeleteEventAsync(Guid id)
        {
            await _eventRepository.DeleteEventAsync(id);
        }

        //In EventsService.cs
        public async Task SendEventNotifications(Event @event)
        {
            var user = await _userManager.FindByIdAsync(@event.UserId.ToString());
            if (user != null)
            {
                var connectedUsers = await _connectionService.GetConnectedUsersAsync(@event.UserId);
                foreach (var connectedUser in connectedUsers)
                {
                    await _notificationService.SendEventNotificationAsync(connectedUser, @event); // Use SendEventNotificationAsync
                }
            }
            else
            {
                _logger.LogError("User not found for event notifications.");
            }
        }

        private async Task SendNotification(User connectedUser, Event @event)
        {
            var notification = new Notification
            {
                UserId = connectedUser.Id,
                Message = $"O usuário {@event.User.UserName} (ou {@event.User.Email}) criou um novo evento."
            };

            await _notificationRepository.AddAsync(notification);
        }

       public async Task<Guid> CreateCommentAsync(Guid eventId, Guid userId, string content)
        {
            var comment = new EventComment
            {
                EventId = eventId,
                UserId = userId,
                Content = content,
                CreatedAt = DateTime.UtcNow
            };

            await _eventCommentRepository.AddAsync(comment); // Use the injected repository
            return comment.Id;
        }

        public async Task<Guid> ShareEventAsync(Guid eventId, Guid userId)
        {
            var share = new EventShare
            {
                EventId = eventId,
                UserId = userId,
                SharedAt = DateTime.UtcNow
            };

            await _eventShareRepository.AddAsync(share);  // Use the injected repository
            return share.Id;
        }
    }
}