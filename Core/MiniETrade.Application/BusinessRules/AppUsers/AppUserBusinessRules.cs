﻿using MediatR;
using MiniETrade.Application.Repositories.Products;
using MiniETrade.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniETrade.Application.BusinessRules.AppUsers;

public class AppUserBusinessRules : BaseBusinessRules
{
    public static bool CheckIfPasswordMatches(string password, string passwordToCheck)
    {
        if (password != passwordToCheck) 
            throw new BusinessException("Şifreler uyuşmuyor."); //TODO-HUS magic string
        return true; 
    }
}
