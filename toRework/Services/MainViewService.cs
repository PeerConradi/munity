using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MUNity.Services
{
    public class MainViewService
    {
        private int _loadingState { get; set; }

        private bool _showSidebar;

        public event EventHandler<int> LoadingStateChanged;

        public event EventHandler<bool> ShowSidebarChanged;

        public event EventHandler<bool> ShowSidebarLageViewChanged;

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

        private bool _showSidbarLageView = true;
        public bool ShowSidebarLargeView
        {
            get => _showSidbarLageView;
            set
            {
                if (_showSidbarLageView != value)
                {
                    _showSidbarLageView = value;
                    ShowSidebarLageViewChanged?.Invoke(this, value);
                }
            }
        }
    }
}
