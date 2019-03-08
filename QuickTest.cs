using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace QuickTestBot_CSharp
{
    public class QuickTest
    {
        private DialogContext _dc;
        private readonly CancellationToken _cancellationToken;

        public QuickTest(DialogContext dc, CancellationToken cancellationToken)
        {

        }

        public static async Task OnWelcomeAsync(DialogContext dc, CancellationToken cancellationToken)
        {
            await dc.Context.SendActivityAsync("Executing welcome test...");
            await dc.BeginDialogAsync(nameof(QuickDialog));
        }

        public static async Task OnMessageAsync(DialogContext dc, CancellationToken cancellationToken)
        {
            await dc.Context.SendActivityAsync("Executing on message test...");
        }
    }
}
