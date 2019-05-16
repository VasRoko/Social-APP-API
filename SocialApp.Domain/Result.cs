using System;
using System.Collections.Generic;
using System.Text;

namespace SocialApp.Domain
{
    public class Result
    {
        public bool isValid { get; set; } = false;
        public string Message { get; set; }
        public object Data { get; set; }
    }
}
