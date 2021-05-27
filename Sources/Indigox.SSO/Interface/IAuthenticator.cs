﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Indigox.SSO.Interface
{
    public interface IAuthenticator
    {
        IAuthentication Authenticate(ICredentials credentials);
    }
}
