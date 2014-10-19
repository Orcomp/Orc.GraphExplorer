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
        private IStatusService _statusService;
        public MainViewModel()
        {
            StatusMessage.Register(this, OnStatusMessage);
        }

        protected override void Initialize()
        {
            base.Initialize();
            _statusService = ServiceLocator.Default.ResolveType<IStatusService>();            
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
