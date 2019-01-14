using System;
using System.Collections.Generic;
using System.Text;

namespace SocialApp.Domain.Dtos
{
    public class MessageForCreactionDto
    {
        public int SenderId { get; set; }
        public int RecipientId { get; set; }
        public DateTime MessageSent { get; set; }
        public string Content { get; set; }

        public MessageForCreactionDto()
        {
            MessageSent = DateTime.Now;
        }
    }
}
