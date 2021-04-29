using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MUNityCore.Services
{
    public class MainViewService
    {
        private int _loadingState { get; set; }

        public event EventHandler<int> LoadingStateChanged;

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
    }
}
