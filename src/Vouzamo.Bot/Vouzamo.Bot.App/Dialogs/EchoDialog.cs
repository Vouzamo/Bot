using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;

namespace Vouzamo.Bot.App.Dialogs
{
    [Serializable]
    public class EchoDialog : IDialog
    {
        public async Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageReceivedAsync);
        }

        public async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> argument)
        {
            var message = await argument;

            var text = message.Text.Substring(5, message.Text.Length - 5);

            await context.PostAsync(text);

            context.Wait(MessageReceivedAsync);
        }
    }
}