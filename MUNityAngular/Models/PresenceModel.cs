using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MUNityAngular.Models
{
    public class PresenceModel
    {
        public string ID { get; set; }

        public ObservableCollection<DelegationModel> Present { get; set; }

        public ObservableCollection<DelegationModel> Absent { get; set; }

        public ObservableCollection<DelegationModel> Remaining { get; set; }

        public int FiftyPlusOne { get; set; }

        public int TwoThirds { get; set; }

        public uint Total { get; set; }

        public double PresentPercentage { get; set; }

        public double AbsentPercentage { get; set; }
        public uint Known { get; set; }
        public DateTime Date { get; set; }

        public double KnownPercentage { get; set; }

        private CommitteeModel Committee;

        public PresenceModel(CommitteeModel committee, string id = null)
        {
            Committee = committee;

            

            Present = new ObservableCollection<DelegationModel>();
            Absent = new ObservableCollection<DelegationModel>();
            Remaining = new ObservableCollection<DelegationModel>();
            ID = id ?? Guid.NewGuid().ToString();
            Date = DateTime.Now;
            foreach (var delegation in Committee.MyDelegations.OrderBy(n => n.Name))
            {
                Remaining.Add(delegation);
            }

            //Committee.Delegations.CollectionChanged += Delegations_CollectionChanged;
            Total = (uint)(Present.Count + Absent.Count + Remaining.Count);
            Present.CollectionChanged += AnyCollectionChanged;
            Absent.CollectionChanged += AnyCollectionChanged;

        }

        private void AnyCollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            PresentPercentage = (Present.Count > 0) ? Present.Count / (double)Total * 100 : 0;
            AbsentPercentage = (Absent.Count > 0) ? Absent.Count / (double)Total * 100 : 0;
            Known = (uint)(Absent.Count + Present.Count);
            KnownPercentage = (Total > 0) ? Known / (double)Total * 100 : 0;
            if (Present.Count % 2 == 0)
            {
                //If dividable by two then it should be the Count / 2 plus 1 Vote
                FiftyPlusOne = (int)Math.Ceiling((double)Present.Count / 2) + 1;
            } else
            {
                FiftyPlusOne = (int)Math.Ceiling((double)Present.Count / 2);
            }
            
            TwoThirds = (int)Math.Ceiling(Present.Count * (double)( 2.0 / 3.0));
        }

        private void Delegations_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case System.Collections.Specialized.NotifyCollectionChangedAction.Add:
                    foreach(var o in e.NewItems.OfType<Models.DelegationModel>())
                    {
                        Remaining.Add(o);
                    }
                        
                    break;
                case System.Collections.Specialized.NotifyCollectionChangedAction.Remove:
                    break;
                case System.Collections.Specialized.NotifyCollectionChangedAction.Replace:
                    break;
                case System.Collections.Specialized.NotifyCollectionChangedAction.Move:
                    break;
                case System.Collections.Specialized.NotifyCollectionChangedAction.Reset:
                    break;
                default:
                    break;
            }
            Total = (ushort)(Present.Count + Absent.Count + Remaining.Count);

        }

        public void SetPresent(DelegationModel delegation)
        {
            Remaining.Remove(delegation);
            Present.Add(delegation);
        }

        public void SetAbsent(DelegationModel delegation)
        {
            Remaining.Remove(delegation);
            Absent.Add(delegation);
        }

        public void SetRemaining(DelegationModel delegation)
        {
            if (Present.Contains(delegation) || Absent.Contains(delegation))
            {
                Present.Remove(delegation);
                Absent.Remove(delegation);
                Remaining.Add(delegation);
                Remaining.OrderBy(n => n.Name);
            }
            else
            {
                Console.WriteLine("ERROR: Tried to set a delegation to Remaining that wasnt in Present or Absent");
            }
        }

        public void Reset()
        {
            Present.Clear();
            Absent.Clear();
            Remaining.Clear();
            foreach (var delegation in Committee.MyDelegations)
            {
                Remaining.Add(delegation);
            }
        }
    }
}
