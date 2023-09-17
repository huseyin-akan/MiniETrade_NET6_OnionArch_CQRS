﻿using MiniETrade.Application.Common.Abstractions.Localization;
using MiniETrade.Domain.Exceptions;
using MiniETrade.Domain.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniETrade.Application.BusinessRules.AppUsers;

public class AppUserBusinessRules : BaseBusinessRules
{

    public bool CheckIfPasswordMatches(string password, string passwordToCheck)
    {
        if (password != passwordToCheck) 
            throw new BusinessException(Messages.PasswordDoesntMatch); 
        return true; 
    }
}