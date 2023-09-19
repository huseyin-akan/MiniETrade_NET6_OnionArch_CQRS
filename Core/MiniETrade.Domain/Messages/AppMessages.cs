using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace MiniETrade.Domain.Messages;

public static class AppMessages
{
    public static string PasswordDoesntMatch = "PasswordDoesntMatch";

    public static string UserNotFound = "UserNotFound";
    public static string UsernameAlreadyRegistered = "UsernameAlreadyRegistered";
    public static string EmailAlreadyRegistered = "EmailAlreadyRegistered";
    public static string LoginUnsuccessful = "LoginUnsuccessful";

    //Validation Messages
    public static string InvalidEmailAddress = "InvalidEmailAddress";
    public static string UsernameOrEmailEmpty = "UsernameOrEmailEmpty";
    public static string PasswordEmpty = "PasswordEmpty";
    public static string PasswordMinLength = "PasswordMinLength";


    public static string ProductNotAvailable = "ProductNotAvailable";
    public static string DataNotFound = "DataNotFound";
}