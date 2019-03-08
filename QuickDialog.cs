using Microsoft.Bot.Builder.Dialogs;
using System.Threading;
using System.Threading.Tasks;

namespace QuickTestBot_CSharp
{
    public class QuickDialog : ComponentDialog
    {
        public QuickDialog() : base(nameof(QuickDialog))
        {
            WaterfallStep[] waterfallSteps = new WaterfallStep[]
            {
                FirstStepAsync,
                SecondStepAsync,
                EndAsync,
            };
            AddDialog(new WaterfallDialog($"{nameof(QuickDialog)}_waterfall", waterfallSteps));
            AddDialog(new ChoicePrompt(nameof(ChoicePrompt)));
            AddDialog(new TextPrompt(nameof(TextPrompt)));
            AddDialog(new NumberPrompt<long>(nameof(NumberPrompt<long>)));
            AddDialog(new DateTimePrompt(nameof(DateTimePrompt)));
            AddDialog(new ConfirmPrompt(nameof(ConfirmPrompt)));
            AddDialog(new AttachmentPrompt(nameof(AttachmentPrompt)));
        }

        private static async Task<DialogTurnResult> FirstStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken = default(CancellationToken))
        {
            await stepContext.Context.SendActivityAsync("Beginning QuickDialog...");
            return await stepContext.NextAsync();
        }

        private static async Task<DialogTurnResult> SecondStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await stepContext.NextAsync();
        }

        private static async Task<DialogTurnResult> EndAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await stepContext.EndDialogAsync();
        }
    }
}
