using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace QuickTestBot_CSharp
{
    public class QuickTestBot_CSharpBot : IBot
    {
        private readonly IStatePropertyAccessor<DialogState> _dialogStateAccessor;
        private readonly ConversationState _conversationState;
        public QuickTestBot_CSharpBot(ConversationState conversationState)
        {
            _conversationState = conversationState ?? throw new ArgumentNullException(nameof(conversationState));
            _dialogStateAccessor = _conversationState.CreateProperty<DialogState>(nameof(DialogState));

            Dialogs = new DialogSet(_dialogStateAccessor);
            Dialogs.Add(new QuickDialog());
        }

        private DialogSet Dialogs { get; set; }

        public async Task OnTurnAsync(ITurnContext turnContext, CancellationToken cancellationToken = default(CancellationToken))
        {
            var activity = turnContext.Activity;

            var dc = await Dialogs.CreateContextAsync(turnContext);

            if (activity.Type == ActivityTypes.Message)
            {
                // Ensure that message is a postBack (like a submission from Adaptive Cards
                var channelData = JObject.Parse(dc.Context.Activity.ChannelData.ToString());
                if (channelData.ContainsKey("postback"))
                {
                    var postbackActivity = dc.Context.Activity;
                    // Convert the user's Adaptive Card input into the input of a Text Prompt
                    // Must be sent as a string
                    postbackActivity.Text = postbackActivity.Value.ToString();
                    await dc.Context.SendActivityAsync(postbackActivity);
                }
                else
                {
                    await QuickTest.OnMessageAsync(dc, cancellationToken);
                }
            }

            var dialogResult = await dc.ContinueDialogAsync();

            if (!dc.Context.Responded)
            {
                switch (dialogResult.Status)
                {
                    case DialogTurnStatus.Empty:
                    case DialogTurnStatus.Waiting:
                        break;
                    case DialogTurnStatus.Complete:
                        await dc.EndDialogAsync();
                        break;
                    default:
                        await dc.CancelAllDialogsAsync();
                        break;

                }
            }

            if (activity.Type == ActivityTypes.ConversationUpdate)
            {
                if (activity.MembersAdded != null)
                {
                    foreach (var member in activity.MembersAdded)
                    {
                        if (member.Name != "Bot" && member.Name != null)
                        {
                            await QuickTest.OnWelcomeAsync(dc, cancellationToken);
                        }
                    }
                }
            }

            await _conversationState.SaveChangesAsync(turnContext);
        }
    }
}
