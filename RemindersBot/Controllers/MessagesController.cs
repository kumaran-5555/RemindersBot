using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Microsoft.Bot.Connector;
using Microsoft.Bot.Connector.Utilities;
using Newtonsoft.Json;

namespace RemindersBot
{
    [BotAuthentication]
    public class MessagesController : ApiController
    {
        /// <summary>
        /// POST: api/Messages
        /// Receive a message from a user and reply to it
        /// </summary>
        public async Task<Message> Post([FromBody]Message message)
        {
            if (message.Type == "Message")
            {
                return HandleAppMessages(message);
                
            }
            else
            {
                return HandleSystemMessage(message);
            }
        }

        private Message HandleAppMessages(Message message)
        {

            var state = message.GetBotUserData<BotStates>("State");

            var response = message.Text;

            


            if (state == null || state.CurrentTopLevelState == BotStates.BotStateTopLevel.NewUser)
            {


                // present prompt
                var reply = message.CreateReplyMessage(TopLevelCommands.CommandsObj.GeneratePrompt());

                reply.SetBotUserData("State", new BotStates(BotStates.BotStateTopLevel.GetCommand, null));

                return reply;

            }
            else if(state.CurrentTopLevelState == BotStates.BotStateTopLevel.GetCommand)
            {

                Object temp;
                TopLevelCommands.CommandsObj.Validate(response, out temp);

                ConstantCommands.ConstantCommandsList rval;
                
                rval = (ConstantCommands.ConstantCommandsList)temp;
                

                if(rval == ConstantCommands.ConstantCommandsList.AddAlaram)
                {

                    var reply = message.CreateReplyMessage("Enter alaram name");

                    reply.SetBotUserData("State", new BotStates(BotStates.BotStateTopLevel.InAdd, BotStates.BotStateSecondLevel.Add_GetName));
                    // create temporary alarm object
                    reply.SetBotUserData("Alarm", new Alarm());

                    return reply;
                }
                else if(rval == ConstantCommands.ConstantCommandsList.DeleteAlarams)
                {
                    return message.CreateReplyMessage("Delete");
                }
                else if(rval == ConstantCommands.ConstantCommandsList.ViewAlarms)
                {
                    return message.CreateReplyMessage("View");
                }
                
            }
            else if(state.CurrentTopLevelState == BotStates.BotStateTopLevel.InAdd)
            {
                if(state.CurrentSecondLevelState == BotStates.BotStateSecondLevel.Add_GetName)
                {
                    var alarm = message.GetBotUserData<Alarm>("Alarm");

                    if(alarm == null)
                    {
                        throw new Exception("Invalid alarm state");
                    }

                    if (!Alarm.ValidateName(message.Text))
                    {
                        throw new Exception("Invalid alarm name");
                    }

                    alarm.

                }
            }


            return null;

        }

        private Message HandleSystemMessage(Message message)
        {
            if (message.Type == "Ping")
            {
                Message reply = message.CreateReplyMessage();
                reply.Type = "Ping";
                return reply;
            }
            else if (message.Type == "DeleteUserData")
            {
                // Implement user deletion here
                // If we handle user deletion, return a real message
            }
            else if (message.Type == "BotAddedToConversation")
            {
                // initiate state
                Message reply = message.CreateReplyMessage("Hi, I can help you with reminders.\n\n" + TopLevelCommands.CommandsObj.GeneratePrompt());
                reply.Type = "Message";
                reply.SetBotUserData("State", new BotStates(BotStates.BotStateTopLevel.GetCommand, null));

                return reply;
            }
            else if (message.Type == "BotRemovedFromConversation")
            {
            }
            else if (message.Type == "UserAddedToConversation")
            {
            }
            else if (message.Type == "UserRemovedFromConversation")
            {
            }
            else if (message.Type == "EndOfConversation")
            {
                Message reply = message.CreateReplyMessage("Good bye ...");

                reply.SetBotUserData("State", null);

                return reply;
            }

            return null;
        }
    }

    
}