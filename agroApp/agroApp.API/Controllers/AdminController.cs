using agroApp.API.DTOs;
using agroApp.API.Services;
using agroApp.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using agroApp.Infra.Data.Repositories;

namespace agroApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class AdminController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly IUserRepository _userRepository;

        public AdminController(UserManager<User> userManager, IUserRepository userRepository)
        {
            _userManager = userManager;
            _userRepository = userRepository;
        }

        [HttpPost]
public async Task<IActionResult> CreateOrUpdateAdmin([FromBody] CreateOrUpdateAdminDto adminDto)
{
    var user = await _userRepository.GetByEmailAsync(adminDto.Email);

    if (user == null)
    {
        // Cria um novo administrador
        user = new User { UserName = adminDto.Username, Email = adminDto.Email };
        var createResult = await _userManager.CreateAsync(user, adminDto.Password);
        if (!createResult.Succeeded) return BadRequest(createResult.Errors);

        // Adiciona à role "Admin"
        var addToRoleResult = await _userManager.AddToRoleAsync(user, "Admin");
        if (!addToRoleResult.Succeeded) return BadRequest(addToRoleResult.Errors);

        return Ok();
    }
    else
    {
        // Atualiza um administrador existente
        //Atualizar outras propriedades se necessário

        // Verifica se a senha foi fornecida para atualizar
        if (!string.IsNullOrEmpty(adminDto.Password))
        {
            // Define a nova senha
            var changePasswordResult = await _userManager.ChangePasswordAsync(user, adminDto.CurrentPassword, adminDto.Password);
            if (!changePasswordResult.Succeeded) return BadRequest(changePasswordResult.Errors);
        }


        //Manipulação de roles - Usar o UserManager para segurança
        if (adminDto.RemoveAdminRole)
        {
             await _userManager.RemoveFromRoleAsync(user, "Admin"); //Remove role Admin
             if(adminDto.AddUserRole) await _userManager.AddToRoleAsync(user, "User"); //Adiciona role User se necessário
        }
        else if (adminDto.AddAdminRole)
        {
            await _userManager.AddToRoleAsync(user, "Admin"); // Adiciona a role Admin
        }
        else if (adminDto.AddUserRole)
        {
            await _userManager.AddToRoleAsync(user, "User"); //Adiciona role User
        }
        //Remove Role User caso necessário
        if(adminDto.RemoveUserRole) await _userManager.RemoveFromRoleAsync(user, "User");


        return Ok();
    }
}
    }
}