using Evento.Infrastructure.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace Evento.Infrastructure.Services
{
    public interface IJwtHandler
    {
        JwtDto CreateTokem(Guid userId, string role);

    }
}
