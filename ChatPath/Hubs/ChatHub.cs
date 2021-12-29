
using ChatPath.Entity;
using ChatPath.Models;
using ChatPath.Helper;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.SignalR.Protocol;
using Microsoft.AspNetCore.SignalR.Redis.Internal;
using System.Collections.Generic;
using System;

namespace ChatPath.Hubs
{
    public class ChatHub : Hub
    {
        public void Subscribe(string channel)
        {
            Groups.AddToGroupAsync(Context.ConnectionId, channel);
        }

        public void Unsubscribe(string channel)
        {
            Groups.RemoveFromGroupAsync(Context.ConnectionId, channel);
        }

        public void SendMsg(string channel, string nickName, string msg)
        {
            try
            {
                ChatModel model = new();
                Channel chnl = model.GetChannelByName(channel);
                Encryption enc = new();
                //Mesaj encrypt edildikten sonra db'ye kayıt
                int result = model.InsertMessage(new Message
                {
                    ChannelID = chnl.ChannelID,
                    NickName = nickName,
                    Date = DateTime.Now,
                    IsDeleted = false,
                    MessageText = enc.EncryptText(msg)
                });

                // mesajı redis ile iletme
                using var connection = Redis.get();
                var sub = connection.GetSubscriber();
                var protocol = new JsonHubProtocol();
                var redisProtocol = new RedisProtocol(new List<JsonHubProtocol>() { protocol });
                var bytes = redisProtocol.WriteInvocation("ReceiveMsg", new[] { channel, nickName, msg });
                sub.Publish($"ChatPath.Hubs.ChatHub:group:{channel}", bytes);
            }
            catch (Exception)
            {
                try
                {   
                    //redis ile iletme esnasında hata alınırsa, sıradan signalR metoduyla mesaj iletme 
                    Clients.Group(channel).SendAsync("ReceiveMsg", channel, nickName, msg);
                }
                catch (Exception)
                {
                    Clients.Group(channel).SendAsync("ErrorMsg", "");
                }
            }
            //if (result == 1)
            //{
            //    Clients.Group(channel).SendAsync("ReceiveMsg", channel, nickName, msg);
            //}
            //else
            //{
            //    Clients.Group(channel).SendAsync("ErrorMsg", msg);
            //}
        }
    }
}