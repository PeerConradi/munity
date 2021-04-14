using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.EntityFrameworkCore;
using MUNity.Models.Simulation;
using MUNity.Schema.Simulation;
using MUNity.Schema.Simulation.Voting;
using MUNityCore.DataHandlers.EntityFramework;
using MUNityCore.Extensions.CastExtensions;
using MUNityCore.Models.Simulation;
using MUNityCore.Models.Simulation.Presets;
using MUNitySchema.Schema.Simulation.Resolution;

namespace MUNityCore.Services
{
    public class FrontendSimulationService
    {
        public event EventHandler<SimulationTabs> TabChanged;

        private int _currentSimulationId = -1;
        public int CurrentSimulationId
        {
            get => _currentSimulationId;
            set 
            {
                if (_currentSimulationId != value)
                {
                    _currentSimulationId = value;
                    CurrentSimulationIdChanged?.Invoke(this, value);
                }
            }
        }

        public enum SimulationTabs
        {
            Overview,
            Agenda,
            Voting,
            Resolutions
        }

        private SimulationTabs _currentTab;
        public SimulationTabs CurrentTab 
        { 
            get => _currentTab;
            set
            {
                if (this._currentTab != value)
                {
                    this._currentTab = value;
                    TabChanged?.Invoke(this, value);
                }
            }
        }

        public string CurrentUserToken { get; set; }

        public event EventHandler<int> CurrentSimulationIdChanged;

        private readonly MunityContext _context;

        public void CurrentTabByString(string pageName)
        {
            switch (pageName.ToLower())
            {
                case "start":
                    this.CurrentTab = SimulationTabs.Overview;
                    break;
                case "agenda":
                    this.CurrentTab = SimulationTabs.Agenda;
                    break;
                case "voting":
                    this.CurrentTab = SimulationTabs.Voting;
                    break;
                case "resolution":
                    this.CurrentTab = SimulationTabs.Resolutions;
                    break;
                default:
                    break;
            }
        }
    }

}