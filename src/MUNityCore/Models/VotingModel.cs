﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MUNityCore.Models.Conference;

namespace MUNityCore.Models
{
    public class VotingModel
    {
        public string ID { get; set; }

        public enum EVotingOptions
        {
            Positiv,
            Negativ,
            Abstention,
            Withdraw,
        }

        public bool VotingDone { get; private set; }

        public ObservableCollection<Delegation> Positiv { get; set; }

        public ObservableCollection<Delegation> Negativ { get; set; }

        public ObservableCollection<Delegation> Abstention { get; set; }

        private Committee Committee { get; set; }

        public ObservableCollection<Delegation> RemainingVotes { get; set; }

        public bool CanAbstent { get; set; }

        public double PercentagePositiv { get; set; }

        private int _totalVoters;
        public int TotalVoters
        {
            get => _totalVoters;
            set
            {
                _totalVoters = value;
                CalculatePercentages();
            }
        }

        private int _proCount;
        public int ProCount
        {
            get => _proCount;
            set
            {
                _proCount = value;
                CalculatePercentages();
            }
        }

        private int _contraCount;
        public int ContraCount
        {
            get => _contraCount;
            set
            {
                _contraCount = value;
                CalculatePercentages();
            }
        }

        private int _abstentionCount;
        public int AbstentionCount
        {
            get => _abstentionCount;
            set
            {
                _abstentionCount = value;  
                CalculatePercentages();
            }
        }

        public double PercentageNegativ { get; set; }

        public double PercentageAbstention { get; set; }

        public bool AbsolutMajority { get; set; }

        public bool TwoThirdsMajority { get; set; }

        public bool Valid { get; set; }

        public VotingModel(Committee committee, string id = null)
        {
            Committee = committee;
            Positiv = new ObservableCollection<Delegation>();
            Negativ = new ObservableCollection<Delegation>();
            Abstention = new ObservableCollection<Delegation>();
            RemainingVotes = new ObservableCollection<Delegation>();
            Positiv.CollectionChanged += VoteChanged;
            Negativ.CollectionChanged += VoteChanged;
            Abstention.CollectionChanged += VoteChanged;
            RemainingVotes.CollectionChanged += VoteChanged;


            ID = id ?? Guid.NewGuid().ToString();

            TotalVoters = RemainingVotes.Count;
        }

        private void VoteChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            CalculatePercentages();
            if (RemainingVotes.Count == 0)
                VotingDone = true;

            TotalVoters = Positiv.Count + Negativ.Count + Abstention.Count + RemainingVotes.Count;
            ProCount = Positiv.Count;
            ContraCount = Negativ.Count;
            AbstentionCount = Abstention.Count;
        }

        public void CalculatePercentages()
        {
            //Catch divide by zero
            if (TotalVoters == 0)
            {
                Valid = false;
                return;
            }
                

            PercentagePositiv = (ProCount / (double)TotalVoters) * 100;
            PercentageNegativ = (ContraCount / (double)TotalVoters) * 100;
            PercentageAbstention = (AbstentionCount / (double)TotalVoters) * 100;

            AbsolutMajority = (ProCount > TotalVoters / 2);
            TwoThirdsMajority = (ProCount > ((TotalVoters / 3) * 2));
            ValidateVotes();
        }

        private void ValidateVotes()
        {
            Valid = (ProCount + ContraCount + AbstentionCount <= TotalVoters);
        }

        /*
        public void Vote(DelegationModel delegation, EVotingOptions option)
        {
            if (Committee.MyDelegations.Contains(delegation))
            {
                RemainingVotes.Remove(delegation);
                Positiv.Remove(delegation);
                Negativ.Remove(delegation);
                Abstention.Remove(delegation);
                switch (option)
                {
                    case EVotingOptions.Positiv:
                        Positiv.Add(delegation);
                        break;
                    case EVotingOptions.Negativ:
                        Negativ.Add(delegation);
                        break;
                    case EVotingOptions.Abstention:
                        if (CanAbstent)
                        {
                            Abstention.Add(delegation);
                        }
                        break;
                    case EVotingOptions.Withdraw:
                        RemainingVotes.Add(delegation);
                        break;
                    default:
                        break;
                }
                CalculatePercentages();
            }
        }
        */
    }
}
