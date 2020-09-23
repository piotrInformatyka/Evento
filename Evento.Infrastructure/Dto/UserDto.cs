using System;
using System.Collections.Generic;
using System.Text;

namespace Evento.Infrastructure.Dto
{
   public class UserDto
    {
        public string Role { get;  set; }
        public string Name { get;  set; }
        public string Email { get;  set; }
        public string Password { get;  set; }
        public DateTime CreatedAt { get;  set; }
    }
}
