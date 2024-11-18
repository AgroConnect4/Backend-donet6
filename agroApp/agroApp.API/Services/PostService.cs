using agroApp.Domain.Entities;
using agroApp.Infra.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using agroApp.API.DTOs;
using Microsoft.AspNetCore.Http;
using agroApp.API.Services;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity; // Adicione o namespace para UserManager
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;

namespace agroApp.API.Services
{
    public class PostService : IPostService
    {
        private readonly ILogger<PostService> _logger;
        private readonly IPostRepository _postRepository;
        private readonly IConfiguration _configuration;
        private readonly IUserRepository _userRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<User> _userManager; // Injete UserManager
        private readonly IConnectionService _connectionService;
        private readonly INotificationService _notificationService;
        private readonly INotificationRepository _notificationRepository;
        //private readonly ICommentRepository _commentRepository;
        //private readonly IShareRepository _shareRepository;
        private readonly IPostCommentRepository _postCommentRepository;
        private readonly IPostShareRepository _postShareRepository;   
        
        public PostService(IPostRepository postRepository,
        IConfiguration configuration,
        IUserRepository userRepository,
        IHttpContextAccessor httpContextAccessor,
        ILogger<PostService> logger,
        UserManager<User> userManager,
        IConnectionService connectionService,
        INotificationService notificationService,
        INotificationRepository notificationRepository,
        //ICommentRepository commentRepository,
        //IShareRepository shareRepository
        IPostCommentRepository postCommentRepository,
        IPostShareRepository postShareRepository
        )
        {
            _logger = logger;
            _configuration = configuration;
            _postRepository = postRepository ?? throw new ArgumentNullException(nameof(postRepository));
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
            _userManager = userManager; 
            _connectionService = connectionService;
            _notificationService = notificationService;
            _notificationRepository = notificationRepository;
            //_commentRepository = commentRepository;
            //_shareRepository = shareRepository;
            _postCommentRepository = postCommentRepository; // Assign the repository
            _postShareRepository = postShareRepository;
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

        public async Task<Guid> CreatePostAsync(CreatePostDto request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request), "Os dados de criação de post não podem ser nulos.");
            }

            // Obter o usuário logado
            Guid userId = GetUserIdFromToken();

            // Validar se o usuário existe
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
            {
                throw new Exception("Usuário não encontrado.");
            }

            // Criar o post
            var post = new Post
            {
                UserId = userId,
                Content = request.Content,
                Title = request.Title,
                ImageUrl = request.ImageUrl,
                CreatedAt = DateTime.UtcNow,
                Categories = request.Categories // Atribui diretamente as categorias
            };

            // Salvar o post
            await _postRepository.AddAsync(post);

            await SendNotifications(post);

            return post.Id;
        }

        public async Task<Post> GetPostByIdAsync(Guid postId)
        {
            var post = await _postRepository.GetByIdAsync(postId);
            if (post == null)
            {
                throw new KeyNotFoundException("Post não encontrado.");
            }
            return post;
        }

        public async Task<List<Post>> GetAllPostsByUserIdAsync(Guid userId)
        {
            return await _postRepository.GetAllByUserIdAsync(userId);
        }

        public async Task<Post> UpdatePostAsync(Guid postId, UpdatePostDto request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request), "Os dados de atualização não podem ser nulos.");
            }

            var post = await _postRepository.GetByIdAsync(postId);
            if (post == null)
            {
                throw new KeyNotFoundException("Post não encontrado.");
            }

            post.Content = request.Content;
            post.Title = request.Title;
            post.ImageUrl = request.ImageUrl;
            post.EditedAt = DateTime.UtcNow;
            post.Categories = request.Categories;

            return await _postRepository.UpdateAsync(post);
        }

        public async Task DeletePostAsync(Guid postId)
        {
            var post = await _postRepository.GetByIdAsync(postId);
            if (post == null)
            {
                throw new KeyNotFoundException("Post não encontrado.");
            }
            await _postRepository.DeleteAsync(postId);
        }

        public async Task SendNotifications(Post post)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(post.UserId.ToString());
                if (user != null)
                {
                    var connectedUsers = await _connectionService.GetConnectedUsersAsync(post.UserId);
                    foreach (var connectedUser in connectedUsers)
                    {
                        await _notificationService.SendPostNotificationAsync(connectedUser, post); // Use SendPostNotificationAsync
                    }
                }
                else
                {
                    _logger.LogError("User not found for post notifications.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao enviar notificações para o post com ID {PostId}.", post.Id);
            }
        }

        public async Task CreateCommentAsync(Guid postId, Guid userId, string content)
        {
            var comment = new PostComment
            {
                PostId = postId, // Atribuição direta
                UserId = userId,
                Content = content,
                CreatedAt = DateTime.UtcNow
            };

            await _postCommentRepository.AddAsync(comment);
        }

        public async Task SharePostAsync(Guid postId, Guid userId)
        {
            var share = new PostShare
            {
                PostId = postId, // Atribuição direta
                UserId = userId,
                SharedAt = DateTime.UtcNow
            };

            await _postShareRepository.AddAsync(share);
        }

        public async Task<int> GetCommentsCountAsync(Guid postId)
        {
            var post = await _postRepository.GetByIdAsync(postId);
            if (post == null)
            {
                throw new KeyNotFoundException("Post não encontrado.");
            }
            return post.Comments.Count;
        }

        public async Task<int> GetSharesCountAsync(Guid postId)
        {
            var post = await _postRepository.GetByIdAsync(postId);
            if (post == null)
            {
                throw new KeyNotFoundException("Post não encontrado.");
            }
            return post.Shares.Count;
        }

        public async Task<List<Post>> GetAllPostsByCategoryNameAsync(string categoryName)
        {
            return await _postRepository.GetAllPostsByCategoryNameAsync(categoryName);
        }
    }
}
