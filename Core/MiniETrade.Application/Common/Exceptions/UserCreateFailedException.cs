using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniETrade.Application.Common.Exceptions
{
    public class UserCreateFailedException : Exception
    {
        public UserCreateFailedException() : base("Kullanıcı oluşturulurken beklenmeyen bir hatayla karşılaşıldı!")
        {}

        public UserCreateFailedException(string? message) : base(message)
        {}

        public UserCreateFailedException(string? message, Exception? innerException) : base(message, innerException)
        {}

        public UserCreateFailedException(IEnumerable<IdentityError> error) : this() //TODO-HUS burada Identity kütüphanesibe bağımlılık söz konusu sanki.
        {}
    }
}