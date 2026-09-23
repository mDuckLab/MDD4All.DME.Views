using Microsoft.AspNetCore.Components;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MDD4All.DME.Views.Notifications
{
    public partial class NotificationToast
    {
        [Parameter]
        public string Message { get; set; } = "";

        // Errors stay long enough to be read even when the user was looking elsewhere.
        [Parameter]
        public bool IsError { get; set; }

        [Parameter]
        public EventCallback OnDismiss { get; set; }

        private const int InfoDurationInMilliseconds = 4000;
        private const int ErrorDurationInMilliseconds = 12000;

        private string _shownMessage = "";

        private CancellationTokenSource? _hideCancellation;

        public string CssClass
        {
            get
            {
                if (this.IsError)
                {
                    return "notification notificationError";
                }

                return "notification notificationInfo";
            }
        }

        // How long something stays visible is a display concern, so the timer lives here and not
        // in the view model. A new message restarts it, an empty one stops it.
        protected override void OnParametersSet()
        {
            if (this.Message == _shownMessage)
            {
                return;
            }

            _shownMessage = this.Message;

            this.CancelPendingHide();

            if (string.IsNullOrEmpty(this.Message))
            {
                return;
            }

            int duration = InfoDurationInMilliseconds;

            if (this.IsError)
            {
                duration = ErrorDurationInMilliseconds;
            }

            _hideCancellation = new CancellationTokenSource();

            this.HideAfter(duration, _hideCancellation.Token);
        }

        private async void HideAfter(int milliseconds, CancellationToken cancellationToken)
        {
            try
            {
                await Task.Delay(milliseconds, cancellationToken);
            }
            catch (TaskCanceledException)
            {
                // Replaced by a newer message or dismissed by hand - nothing left to do.
                return;
            }

            await this.InvokeAsync(this.Dismiss);
        }

        private async Task Dismiss()
        {
            this.CancelPendingHide();

            _shownMessage = "";

            await this.OnDismiss.InvokeAsync();
        }

        private void CancelPendingHide()
        {
            if (_hideCancellation != null)
            {
                _hideCancellation.Cancel();
                _hideCancellation.Dispose();
                _hideCancellation = null;
            }
        }

        public override void Dispose()
        {
            base.Dispose();

            this.CancelPendingHide();
        }
    }
}
