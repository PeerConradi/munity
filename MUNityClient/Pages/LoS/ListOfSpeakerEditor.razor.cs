using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using System.Net.Http;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Http;
using Microsoft.JSInterop;
using MUNityClient;
using MUNityClient.Shared;
using MUNity.Models.ListOfSpeakers;
using MUNity.Extensions.LoSExtensions;
using System.Timers;

namespace MUNityClient.Pages.LoS
{
    public partial class ListOfSpeakerEditor
    {
        [Parameter]
        public string Id
        {
            get;
            set;
        }

        public ListOfSpeakers Speakerlist
        {
            get;
            set;
        }

        [Parameter]
        public bool IsOnline
        {
            get;
            set;
        }

        private MUNityClient.Shared.Bootstrap.Modal SpeakingTimeConfigModal
        {
            get;
            set;
        }

        [Parameter]
        public Services.SocketHandlers.ListOfSpeakerSocketHandler Socket
        {
            get;
            set;
        }

        private class SpeakerToAdd
        {
            public string Iso
            {
                get;
                set;
            }

            public string Name
            {
                get;
                set;
            }
        }

        private class SpeakerlistSettings
        {
            public string Speakertime
            {
                get;
                set;
            }

            public string Questiontime
            {
                get;
                set;
            }

            public SpeakerlistSettings()
            {
            }

            public SpeakerlistSettings(ListOfSpeakers source)
            {
                Speakertime = source.SpeakerTime.ToString(@"mm\:ss");
                Questiontime = source.QuestionTime.ToString(@"mm\:ss");
            }
        }

        private SpeakerlistSettings Settings
        {
            get;
            set;
        }

        private SpeakerToAdd NewSpeaker = new SpeakerToAdd();
        private SpeakerToAdd NewCommentator = new SpeakerToAdd();
        private Boolean invalidFormatSpeakerTime = false;
        private Boolean invalidFormatQuestionTime = false;
        private void SaveSettings()
        {
            var speakerTime = Speakerlist.SpeakerTime;
            var questionTime = Speakerlist.QuestionTime;
            if (TimeSpan.TryParseExact(Settings.Speakertime, @"mm\:ss", null, out speakerTime))
            {
                invalidFormatSpeakerTime = false;
                invalidFormatQuestionTime = false;
                this.Speakerlist.SpeakerTime = speakerTime;
            }
            else
            {
                // Die Eingabe der Redezeit ist ungültig!
                invalidFormatSpeakerTime = true;
            }

            if (TimeSpan.TryParseExact(Settings.Questiontime, @"mm\:ss", null, out questionTime))
            {
                this.Speakerlist.QuestionTime = questionTime;
            }
            else
            {
                // Die Eingegebene Zeit für Fragen und Kurzbemerkungen ist ungültig.
                invalidFormatQuestionTime = true;
            }
        }

        private void TimerElapsed(object sender, ElapsedEventArgs args)
        {
            this.StateHasChanged();
        }

        private EventCallback AddToSpeakers()
        {
            if (NewSpeaker.Name != null && NewSpeaker.Name.Length > 2)
            {
                this.Speakerlist.AddSpeaker(NewSpeaker.Name);
                NewSpeaker.Name = "";
            }

            return EventCallback.Empty;
        }

        private void RemoveSpeaker(Speaker speaker)
        {
            this.Speakerlist.AllSpeakers.Remove(speaker);
        }

        private EventCallback AddToQuestions()
        {
            if (NewCommentator.Name != null && NewCommentator.Name.Length > 2)
            {
                this.Speakerlist.AddQuestion(NewCommentator.Name);
                NewCommentator.Name = "";
            }

            return EventCallback.Empty;
        }

        protected override async Task OnInitializedAsync()
        {
            this.IsOnline = await this.listOfSpeakerService.IsListOfSpeakersOnline(this.Id);
            if (IsOnline)
            {
                this.Speakerlist = await this.listOfSpeakerService.GetFromApi(this.Id);
                // Check if the socket is already passed as a Parameter
                if (Socket != null)
                    Socket = await listOfSpeakerService.Subscribe(Speakerlist);
                if (Socket != null)
                {
                    Socket.SpeakerListChanged += OnSpeakerlistChanged;
                }
            }
            else
            {
                this.Speakerlist = await this.listOfSpeakerService.GetListOfSpeakers(this.Id);
            }

            if (this.Speakerlist != null)
            {
                this.Settings = new SpeakerlistSettings(this.Speakerlist);
                // Wir bauen jetzt für den ersten Entwurf einen Timer, welcher
                // jede Sekunde neu Zeichnet. Der Grund dafür ist, dass die
                // Redeliste nicht selber mit einem Timer arbeitet sondern
                // Startzeiten mit der aktuellen Uhrzeit Synct und hierdurch
                // die Redezeiten berechnet.
                // Es ist theoretisch möglich sich von der Redeliste ein StateChanged Event geben zu lassen
                // dann kann der Timer pausiert werden, wenn ohnehin gerade niemand spricht, aber ich halte
                // eine aktualisierung der Ansicht (kein Serverping) nicht für zu viel. ~Peer
                var timer = new Timer(1000);
                timer.Elapsed += TimerElapsed;
                timer.Start();
                Speakerlist.PropertyChanged += StoreOnListChanged;
                Speakerlist.AllSpeakers.CollectionChanged += StoreOnCollectionChanged;
                base.OnInitialized();
            }
        }

        private void OnSpeakerlistChanged(object sender, ListOfSpeakers newList)
        {
            if (this.Speakerlist.CompareTo(newList) != 0)
            {
                this.Speakerlist = newList;
                Speakerlist.PropertyChanged += StoreOnListChanged;
                Speakerlist.AllSpeakers.CollectionChanged += StoreOnCollectionChanged;
                StateHasChanged();
            }
        }

        private async void StoreOnListChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (IsOnline)
            {
                // The syncing when a speaker has been added will happen in StoreOnCollectionChanged
                if (e.PropertyName == nameof(MUNity.Models.ListOfSpeakers.ListOfSpeakers.SpeakerTime) || 
                    e.PropertyName == nameof(MUNity.Models.ListOfSpeakers.ListOfSpeakers.QuestionTime) || 
                    e.PropertyName == nameof(MUNity.Models.ListOfSpeakers.ListOfSpeakers.CurrentSpeaker) || 
                    e.PropertyName == nameof(MUNity.Models.ListOfSpeakers.ListOfSpeakers.CurrentQuestion) || 
                    e.PropertyName == nameof(MUNity.Models.ListOfSpeakers.ListOfSpeakers.Status) || 
                    e.PropertyName == nameof(MUNity.Models.ListOfSpeakers.ListOfSpeakers.StartQuestionTime) || 
                    e.PropertyName == nameof(MUNity.Models.ListOfSpeakers.ListOfSpeakers.StartSpeakerTime) || 
                    e.PropertyName == nameof(MUNity.Models.ListOfSpeakers.ListOfSpeakers.ListClosed) || 
                    e.PropertyName == nameof(MUNity.Models.ListOfSpeakers.ListOfSpeakers.QuestionsClosed))
                {
                    this.listOfSpeakerService.SyncSpeakerlist(this.Speakerlist);
                }
            }
            else
            {
                await this.listOfSpeakerService.StoreListOfSpeakers(this.Speakerlist);
            }
        }

        private async void StoreOnCollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs args)
        {
            if (IsOnline)
            {
                this.listOfSpeakerService.SyncSpeakerlist(this.Speakerlist);
            }
            else
            {
                await this.listOfSpeakerService.StoreListOfSpeakers(this.Speakerlist);
            }
        }

        private async Task OpenReaderSpectatorView()
        {
            // Das her ist auch mal ein beispiel, wie man eine JavaScript Methode schnell aufrufen kann :)
            string url = "/los/read/" + this.Speakerlist.ListOfSpeakersId;
            await jsRuntime.InvokeAsync<object>("open", url, "_blank");
        }
    }
}