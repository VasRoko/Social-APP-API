using System;
using System.Collections.Generic;
using System.Text;

namespace SocialApp.Domain.Dtos
{
    public class UserForUpdateDto
    {
        public string Description { get; set; }
        public string LookingFor { get; set; }
        public string Interests { get; set; }
        public string City { get; set; }
        public string Country { get; set; }

    }
}
