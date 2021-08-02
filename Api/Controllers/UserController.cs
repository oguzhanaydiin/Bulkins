using Bulkins.Dal;
using Bulkins.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Bulkins.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private UnitOfWork _UnitOfWork { get; set; }
        private IConfiguration Configuration;

        public UserController(IConfiguration _configuration)
        {
            _UnitOfWork = new UnitOfWork(new DatabaseContext());
            Configuration = _configuration;
        }

        /// <summary>
        ///  GET ALL USERS IN DB
        /// </summary>
        /// <returns></returns>
        // GET: api/<UserController>
        [HttpGet("getAllUsers")]
        public IActionResult Get()
        {
            var users = _UnitOfWork.UserRepository.GetAll();
            if (!users.Any())
            {
                return Ok(new CommonResponseModel<IEnumerable<User>>(users, "204", "Eleman yok"));
            }
            else
            {
                return Ok(new CommonResponseModel<IEnumerable<User>>(users, "200", "Basarili"));
            }


        }

        /// <summary>
        /// GET USERS WITH A RANGE 
        /// </summary>
        /// <param name="range"></param>
        /// <returns></returns>
        // GET: api/<UserController>
        [HttpGet("getUsersBulk")]
        public IActionResult GetUsersBulk(int range)
        {
            var users = _UnitOfWork.UserRepository.GetTopUsers(range);
            if (!users.Any())
            {
                return BadRequest(new CommonResponseModel<IEnumerable<User>>(null, "204", "Eleman yok"));
            }
            else if(users.Count()<range){
                return BadRequest(new CommonResponseModel<IEnumerable<User>>(null, "204", "Yeterli eleman yok"));
            }
            else
            {
                return Ok(new CommonResponseModel<IEnumerable<User>>(users, "200", "Basarili"));
            }


        }

        /// <summary>
        /// GET USER BY SINGLE ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // GET api/<UserController>/5
        [HttpGet("getUserById")]
        public IActionResult Get(int id)
        {
            var user = _UnitOfWork.UserRepository.GetById(id);
            if (user == null)
            {
                return Ok(new CommonResponseModel<User>(null, "204", "Eleman yok"));
            }
            else
            {
                return Ok(new CommonResponseModel<User>(user, "200", "Basarili"));
            }
        }

        /// <summary>
        /// ADD A USER BY AUTOMATIC NAME AND SURNAME
        /// </summary>
        /// <returns></returns>
        // POST api/<UserController>/
        [HttpPost("addUser")]
        public IActionResult Post()
        {

            TimeSpan t = DateTime.UtcNow - new DateTime(1970, 1, 1);
            int epochTime = (int)t.TotalSeconds;

            var user = new User()
            {

                CreateDate = DateTime.Now,

                IsActive = true,
                Name = epochTime.ToString(),
                Surname = epochTime.ToString()


            };

            _UnitOfWork.UserRepository.Add(user);
            _UnitOfWork.Complete();
            return Ok(new CommonResponseModel<User>(user, "200", "Basariyla eklendi"));

        }

        /// <summary>
        /// ADD USERS BULK BY GIVEN INPUT
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // POST api/<UserController>/
        [HttpPost("addUsersBulk")]
        public IActionResult AddUsersBulk(int range)
        {
            TimeSpan t = DateTime.UtcNow - new DateTime(1970, 1, 1);
            int epochTime = (int)t.TotalSeconds;

            List<User> users = new List<User>();
            for (int count = 0; count < range; count++)
            {
                epochTime = epochTime + 1;
                var user = new User()
                {

                    CreateDate = DateTime.Now,

                    IsActive = true,
                    Name = epochTime.ToString(),
                    Surname = epochTime.ToString()


                };

                users.Add(user);
            }

            _UnitOfWork.UserRepository.BulkInsert(users);
            _UnitOfWork.Complete();

            return Ok();
        }

        /// <summary>
        /// UPDATES THE GIVEN DATA WITH ID,  NAME TO "DEGIS"
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        // POST api/<UserController>/
        [HttpPost("updateUserWithId")]
        public IActionResult UpdateUserWithId(int id)
        {

            var user = _UnitOfWork.UserRepository.GetById(id);

            if (user != null)
            {
                string value = this.Configuration.GetSection("AppSettings")["UpdateValue"];


                _UnitOfWork.UserRepository.Update(user, value);
                _UnitOfWork.Complete();
                return Ok(new CommonResponseModel<User>(user, "200", "Basariyla guncellendi"));
            }
            else
            {
                return BadRequest(new CommonResponseModel<User>(null, "204", "Data bulunamadi"));
            }

            
        }


        /// <summary>
        /// UPDATES GIVEN NUMBER OF DATA'S NAMES TO "DEGIS"
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        // POST api/<UserController>/
        [HttpPost("updateUsersBulk")]
        public IActionResult UpdateUsersBulk(int count)
        {

            var returnValue = _UnitOfWork.UserRepository.BulkUpdate(count);
            if(returnValue == 0)
            {
                _UnitOfWork.Complete();

                return Ok(new CommonResponseModel<User>(null, "200", "Basariyla guncellendi"));
            }
            else
            {
                return BadRequest(new CommonResponseModel<User>(null, "204", "Bu kadar kayit yok"));
            }

            
        }

        /// <summary>
        /// DELETE A USER BY ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // DELETE api/<UserController>/
        [HttpDelete("deleteById")]
        public IActionResult Delete(int id)
        {
            var user = _UnitOfWork.UserRepository.GetById(id);
            if (user == null)
            {
                return Ok(new CommonResponseModel<User>(null, "204", "Eleman yok"));
            }
            else
            {
                _UnitOfWork.UserRepository.Remove(id);
                _UnitOfWork.Complete();
                return Ok(new CommonResponseModel<User>(user, "200", "Basarili"));
            }
            
        }

        /// <summary>
        /// DELETE BULK OF DATA
        /// </summary>
        /// <returns></returns>
        // DELETE api/<UserController>/
        [HttpDelete("deleteBulk")]
        public IActionResult DeleteBulk(int count)
        {
            var users = _UnitOfWork.UserRepository.GetTopUsers(count);

            if (users.Count() < count)
            {
                return BadRequest(new CommonResponseModel<User>(null, "204", "Bu kadar kayit yok"));
            }
            else
            {
                _UnitOfWork.UserRepository.RemoveRange(users);
                _UnitOfWork.Complete();

                return Ok(new CommonResponseModel<User>(null, "200", "Basariyla silindiler"));
            }
            
        }

        /// <summary>
        /// DELETE ALL DATA
        /// </summary>
        /// <returns></returns>
        // DELETE api/<UserController>/
        [HttpDelete("deleteAllData")]
        public IActionResult DeleteAllData()
        {
            var users = _UnitOfWork.UserRepository.GetAll();
            _UnitOfWork.UserRepository.RemoveRange(users);
            _UnitOfWork.Complete();

            return Ok(new CommonResponseModel<User>(null, "200", "Basariyla silindiler"));
        }

    }


}
