using ChatPath.Entity;
using Dapper;
using Microsoft.AspNetCore.SignalR.Protocol;
using Microsoft.AspNetCore.SignalR.Redis.Internal;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ChatPath.Models
{
    public class ChatModel
    {
        /*
         ORM olarak Dapper kullanıldı, database layer;
         */
        public List<Channel> GetChannels()
        {
            using var connection = Sql.get();
            string query = "SELECT * FROM Channel";
            List<Channel> result = connection.Query<Channel>(query).ToList();
            return result;
        }

        public Channel GetChannelByName(string channelName)
        {
            using var connection = Sql.get();
            string query = "SELECT * FROM Channel WHERE ChannelName = @ChannelName";
            Channel result = connection.Query<Channel>(query, new { ChannelName = channelName }).FirstOrDefault();//aynı isimli kanala izin verilmeyeceğini varsaydım
            return result;
        }

        public List<Message> GetMessagesByChannel(int channelID)
        {
            using var connection = Sql.get();
            string query = "SELECT * FROM Message WHERE ChannelID = @ChannelID";
            List<Message> result = connection.Query<Message>(query, new { ChannelID = channelID }).ToList();
            return result;
        }

        public int InsertMessage(Message message)
        {
            using var connection = Sql.get();
            string query = @"INSERT INTO Message (ChannelID, NickName, MessageText, Date, IsDeleted) 
                             VALUES (@ChannelID, @NickName, @MessageText, @Date, @IsDeleted)";
            int result = connection.Execute(query, new { ChannelID = message.ChannelID, NickName = message.NickName, MessageText = message.MessageText, Date = message.Date, IsDeleted = message.IsDeleted });
            return result;
        }
    }
}
