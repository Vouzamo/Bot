using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using Vouzamo.Bot.App.Dialogs;

namespace Vouzamo.Bot.App.Controllers
{
    [BotAuthentication]
    public class MessagesController : ApiController
    {
        /// <summary>
        /// POST: api/Messages
        /// Receive a message from a user and reply to it
        /// </summary>
        public async Task<HttpResponseMessage> Post([FromBody]Activity activity)
        {
            if (activity.Type == ActivityTypes.Message)
            {
                var connector = new ConnectorClient(new Uri(activity.ServiceUrl));

                Activity reply;

                if (activity.Text.StartsWith("echo "))
                {
                    await Conversation.SendAsync(activity, () => new EchoDialog());
                }

                if (IsQuestion(activity.Text.ToLower()))
                {
                    reply = activity.CreateReply("You asked a question.");
                }
                else if(IsCommand(activity.Text.ToLower()))
                {
                    reply = activity.CreateReply("You issued a command.");
                }
                else
                {
                    reply = activity.CreateReply("You made a statement.");
                }

                await connector.Conversations.ReplyToActivityAsync(reply);
            }
            else
            {
                HandleSystemMessage(activity);
            }
            var response = Request.CreateResponse(HttpStatusCode.OK);
            return response;
        }

        private bool IsQuestion(string message)
        {
            // Does the message contain interrogative words? This is naive and machine learning would yield improvements
            return message.Contains("?") || message.Contains("who") || message.Contains("what") ||
                   message.Contains("where") || message.Contains("when") || message.Contains("why") ||
                   message.Contains("how");
        }

        private bool IsCommand(string message)
        {
            // Does the message start with a verb and contain no subject or past tense?
            return (message.Contains("do") || message.Contains("get")) && (!message.Contains("i") || !message.Contains("you"));
        }

        private Activity HandleSystemMessage(Activity message)
        {
            if (message.Type == ActivityTypes.DeleteUserData)
            {
                // Implement user deletion here
                // If we handle user deletion, return a real message
            }
            else if (message.Type == ActivityTypes.ConversationUpdate)
            {
                // Handle conversation state changes, like members being added and removed
                // Use Activity.MembersAdded and Activity.MembersRemoved and Activity.Action for info
                // Not available in all channels
            }
            else if (message.Type == ActivityTypes.ContactRelationUpdate)
            {
                // Handle add/remove from contact lists
                // Activity.From + Activity.Action represent what happened
            }
            else if (message.Type == ActivityTypes.Typing)
            {
                // Handle knowing tha the user is typing
            }
            else if (message.Type == ActivityTypes.Ping)
            {
            }

            return null;
        }
    }
}