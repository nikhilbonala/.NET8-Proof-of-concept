using System.Collections.Generic;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Test1LearnNewVersion.CustomActionFilters;
using Test1LearnNewVersion.Data;
using Test1LearnNewVersion.Models.DTO;
using Test1LearnNewVersion.Models.Entities;
using Test1LearnNewVersion.Repositories;

namespace Test1LearnNewVersion.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly LearnDbContext dbContext;
        private readonly IUserRepository userRepository;
        private readonly IMapper mapper;


        //ctor controller
        public UserController(LearnDbContext dbContext,IUserRepository userRepository,IMapper mapper)
        {
            this.dbContext = dbContext;
            this.userRepository = userRepository;
            this.mapper = mapper;
        }

        //GET: /api/user/filterOn=Email&filterQuery=gmail&sortBy=Email&isAscending=true&pageNumber=1&pageSize=10
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] string? filterOn, [FromQuery] string? filterQuery, [FromQuery] string? sortBy,
            [FromQuery] bool? isAscending, [FromQuery] int pageNumber=1, [FromQuery] int pageSize=10)
        {
            // Getting data from database to domain model
            var Users = await userRepository.GetAllAsync(filterOn, filterQuery, sortBy, isAscending?? true, pageNumber, pageSize);

            // map domain models to DTO's in traditional way
            //var UsersDto = new List<userDTO>();
            //foreach (var user in Users)
            //{
            //    UsersDto.Add(new userDTO()
            //    {
            //        UserId = user.UserId,
            //        Email = user.Email,
            //        PasswordHash = user.PasswordHash,
            //        CreatedAt = user.CreatedAt
            //    });
            //}


            // map domain models to DTO's using automapper
            // mapper.Map<Type>(source);
            var UsersDto = mapper.Map<List<userDTO>>(Users);

            //return DTOs
            return Ok(UsersDto);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            //person is a domain model
            var person = await userRepository.GetByIdAsync(id);

            if (person == null)
            {
                return NotFound();
            }

            //var personDTO = new userDTO()
            //{
            //    UserId = person.UserId,
            //    Email = person.Email,
            //    PasswordHash = person.PasswordHash,
            //    CreatedAt = person.CreatedAt
            //};

            //Mapping and returning in 2 different lines

            //var personDTO = mapper.Map<userDTO>(person);

            //return Ok(personDTO);

            //doing mapping and returning in same statment
            return Ok(mapper.Map<userDTO>(person));
        }


        // post method to create
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AdduserRequestDTO adduserRequestDTO)
        {

            //added model validation in DTO's, validating them here, if it is not valid, we are returning BadRequest in else condition
            if (ModelState.IsValid)
            {
                //MAP DTO to model

                //var userDomian = new user
                //{
                //    Email = adduserRequestDTO.Email,
                //    PasswordHash = adduserRequestDTO.PasswordHash
                //};

                var userDomian = mapper.Map<user>(adduserRequestDTO);

                //await dbContext.Users.AddAsync(userDomian);
                // await dbContext.SaveChangesAsync();

                //return Ok(userDomian);
                userDomian = await userRepository.CreateAsync(userDomian);


                //var userDTO = new userDTO
                //{
                //    UserId = userDomian.UserId,
                //    Email = userDomian.Email,
                //    PasswordHash = userDomian.PasswordHash,
                //    CreatedAt = userDomian.CreatedAt
                //};

                var userDTO = mapper.Map<userDTO>(userDomian);

                //return Ok(userDTO);

                return CreatedAtAction(nameof(GetById), new { id = userDTO.UserId }, userDTO);
            }
            else
            {
                return BadRequest(ModelState);
            }

            

        }


        // put method to update user 
        [HttpPut]
        [Route("{id}")]
        [ValidateModel]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateuserRequestDTO updateuserRequestDTO)
        {
            //instead of this we can use custom action filters for validations, and write attribure [validatemode] to check model valid or not
            //if (ModelState.IsValid)
            //{
                //var userDomainModel = await dbContext.Users.FirstOrDefaultAsync(x => x.UserId == id);


                //map dto to domain to send to repository
                //var updateUser = new user
                //{
                //    Email = updateuserRequestDTO.Email,
                //    PasswordHash = updateuserRequestDTO.PasswordHash
                //};
                var updateUser = mapper.Map<user>(updateuserRequestDTO);

                var userDomainModel = await userRepository.UpdateAsync(id, updateUser);

                if (userDomainModel == null)
                {
                    return NotFound();
                }

                //userDomainModel.Email = updateuserRequestDTO.Email;
                //userDomainModel.PasswordHash = updateuserRequestDTO.PasswordHash;

                //await dbContext.SaveChangesAsync();

                //var UserDTO = new userDTO
                //{
                //    UserId = userDomainModel.UserId,
                //    Email = userDomainModel.Email,
                //    PasswordHash = userDomainModel.PasswordHash,
                //    CreatedAt = userDomainModel.CreatedAt
                //};

                var UserDTO = mapper.Map<userDTO>(userDomainModel);

                return Ok(UserDTO);
            //}
            //else 
            //{
            //    return BadRequest(ModelState);
            //}


        }


        //delete User
        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            //var userDomainModel = await dbContext.Users.FirstOrDefaultAsync(x => x.UserId == id);

            var userDomainModel = await userRepository.DeleteAsync(id);

            if (userDomainModel == null)
            {
                return NotFound();
            }

            //dbContext.Users.Remove(userDomainModel);
            //await dbContext.SaveChangesAsync();

            //return Ok("Deleted Succesfully!!");

            //to return deleted user back
            //map to dto 

            //var UserDTO = new userDTO
            //{
            //    UserId = userDomainModel.UserId,
            //    Email = userDomainModel.Email,
            //    PasswordHash = userDomainModel.PasswordHash,
            //    CreatedAt = userDomainModel.CreatedAt
            //};

            var UserDTO = mapper.Map<userDTO>(userDomainModel);
            return Ok(UserDTO);


        }


    }
}
