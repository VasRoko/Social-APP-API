using System.Collections.Generic;
using System.Threading.Tasks;
using SocialApp.Domain;
using SocialApp.Domain.Dtos;

namespace SocialApp.Business.Interface
{
    public interface IMessageManager
    {
        Task<Message> GetMessage(int Id);
        Task<Message> CreateMessage(int userid, MessageForCreactionDto messageForCreaction);
        MessageForReturnDto MessageForCreationRetun(Message message);
        Task<PageList<Message>> GetMessageForUser(int userId, MessageParams messageParams);
        IEnumerable<MessageForReturnDto> MapMessagesForUser(PageList<Message> messages);
        Task<IEnumerable<MessageForReturnDto>> GetMessageThred(int userId, int recipientId);
        Task<string> DeleteMessage(int id, int userid);
    }
}
