using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MUNityCore.Services
{
    public class MainViewService
    {
        private int _loadingState { get; set; }

        private bool _showSidebar;

        public event EventHandler<int> LoadingStateChanged;

        public event EventHandler<bool> ShowSidebarChanged;

        public int LoadingState
        {
            get => _loadingState;
            set
            {
                if (_loadingState != value)
                {
                    _loadingState = value;
                    LoadingStateChanged?.Invoke(this, value);
                }
            }
        }

        public bool ShowSidebar
        {
            get => _showSidebar;
            set
            {
                if (value != _showSidebar)
                {
                    _showSidebar = value;
                    ShowSidebarChanged?.Invoke(this, value);
                }
            }
        }
    }
}
