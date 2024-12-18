using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.models.users.dtos;

namespace api.models.users.Mapping
{
    public static class UserMapping
    {
        public static UserDto ToDto(this User model)
        {
            return new UserDto
            {
                First_name = model.First_name,
                Last_Name = model.Last_Name,
                email = model.Email,
                Username = model.UserName,
                Age = model.Age,
                Bio = model.Bio,
                Gender = model.Gender
            };
        }
    }
}