﻿using apigenerica.primitivas;
using Microsoft.AspNetCore.Mvc;

namespace controlescolar.api.Controllers;
[ApiController]
public class EntidadGenericaN1Controller : ControladorGenericoN1
{
    private ILogger<EntidadGenericaN1Controller> _logger;

    public EntidadGenericaN1Controller (ILogger<EntidadGenericaN1Controller> logger, IHttpContextAccessor httpContextAccessor) : base(logger, httpContextAccessor)
    {
        this._logger = logger;
    }
}
