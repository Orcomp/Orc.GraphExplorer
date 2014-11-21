namespace Orc.GraphExplorer.Demo.ViewModels
{
    using System;

    using Catel;
    using Catel.IoC;
    using Catel.MVVM;
    using Catel.Services;

    using Orc.GraphExplorer.Messages;

    using Orchestra.Services;

    public class MainViewModel : ViewModelBase
    {
        private readonly IStatusService _statusService;
        public MainViewModel(IStatusService statusService)
        {
            _statusService = statusService;
            StatusMessage.Register(this, OnStatusMessage);
        }

        private void OnStatusMessage(StatusMessage message)
        {
            if (_statusService == null)
            {
                return;
            }

            _statusService.UpdateStatus(message.Data);
        }
    }
}
