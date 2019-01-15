using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using SocialApp.Business.Interface;
using SocialApp.DataAccess.Interfaces;
using SocialApp.Domain;
using SocialApp.Domain.Dtos;

namespace SocialApp.Business
{
    public class MessageManager : IMessageManager
    {
        private readonly ISocialAppDataAccess _dataAccess;
        private readonly ISocialAppBusiness _business;
        private readonly IMapper _mapper;

        public MessageManager(ISocialAppDataAccess dataAccess, ISocialAppBusiness business, IMapper mapper)
        {
            _dataAccess = dataAccess;
            _mapper = mapper;
            _business = business;
        }

        public async Task<Message> GetMessage(int Id)
        {
            return await _dataAccess.GetMessage(Id);
        }

        public async Task<Message> CreateMessage(int userid, MessageForCreactionDto messageForCreaction)
        {
            messageForCreaction.SenderId = userid;
            var recipient = await _business.GetUser(userid, userid);

            if (recipient == null)
            {
                return null;
            }

            var message = _mapper.Map<Message>(messageForCreaction);
            _dataAccess.Add(message);

            if (await _dataAccess.SaveAll())
            {
                return message;
            }
            else
            {
                throw new Exception("Creating the message failed on save");
            }
        }

        public MessageForReturnDto MessageForCreationRetun(Message message)
        {
            return _mapper.Map<MessageForReturnDto>(message);
        }

        public async Task<PageList<Message>> GetMessageForUser(int userId, MessageParams messageParams)
        {
            messageParams.UserId = userId;
            return await _dataAccess.GetMessageForUser(messageParams);
        }

        public IEnumerable<MessageForReturnDto> MapMessagesForUser(PageList<Message> messages)
        {
            return _mapper.Map<IEnumerable<MessageForReturnDto>>(messages);
        }

        public async Task<IEnumerable<MessageForReturnDto>> GetMessageThred(int userId, int recipientId)
        {
            var messages = await _dataAccess.GetMessageThred(userId, recipientId);
            return _mapper.Map<IEnumerable<MessageForReturnDto>>(messages);
        }

        public async Task<string> DeleteMessage(int id, int userid)
        {
            var message = await _dataAccess.GetMessage(id);

            if (message.SenderId == userid)
            {
                message.SenderDeleted = true;
            }
            else
            {
                message.RecipientDeleted = true;
            }

            if (message.SenderDeleted && message.RecipientDeleted)
            {
                _dataAccess.Delete(message);
            }

            if (await _dataAccess.SaveAll())
            {
                return null;
            }

            throw new Exception("Error deleting  the message");

        }

        public async Task<string> MerkMessageAsRead(int userid, int id)
        {
            var message = _dataAccess.GetMessage(id);
            if (message.Result.RecipientId != userid)
            {
                return null;
            }

            message.Result.IsRead = true;
            message.Result.DateRead = DateTime.Now;

            if (await _dataAccess.SaveAll())
            {
                return "Ok";
            }

            throw new Exception("Error reading the message");
        }
    }
}
