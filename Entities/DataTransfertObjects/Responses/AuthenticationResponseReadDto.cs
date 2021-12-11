using System;
using System.Collections.Generic;

namespace Entities.DataTransfertObjects
{
    public class AuthenticationResponseReadDto
    {
        public Dictionary<string, string> UserInfo { get; set; }
        public string Token { get; set; }
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public IEnumerable<string> ErrorDetails { get; set; }
        public DateTime? ExpireDate { get; set; }
    }
}
