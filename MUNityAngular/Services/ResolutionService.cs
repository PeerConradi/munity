using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MUNityAngular.Util.Extenstions;
using System.Threading;
using Microsoft.AspNetCore.SignalR;

namespace MUNityAngular.Services
{
    public class ResolutionService : IDisposable
    {

        public IHubContext<Hubs.ResolutionHub, Hubs.ITypedResolutionHub> HubContext { get; set; }

        private List<Models.ResolutionModel> resolutions = new List<Models.ResolutionModel>();

        private Timer _saveTimer;

        private Stack<Models.ResolutionModel> _saveStack = new Stack<Models.ResolutionModel>();

        public Models.ResolutionModel CreateResolution(bool isPublicReadable = false, bool isPublicWriteable = false, string userid = "anon")
        {
            var resolution = new Models.ResolutionModel();
            resolutions.Add(resolution);
            //Add To database
            SaveResolutionInDatabase(resolution, isPublicReadable, isPublicWriteable, userid);
            //Create file
            return resolution;
        }

        private string Save(Models.ResolutionModel resolution)
        {
            var resolutionDirectory = DataHandlers.Database.SettingsHandler.GetResolutionDir;
            var filePath = System.IO.Path.Combine(resolutionDirectory, resolution.ID + ".json");

            if (!System.IO.File.Exists(filePath))
            {
                var stream = System.IO.File.Create(filePath);
                stream.Close();
            }
            System.IO.File.WriteAllText(filePath, resolution.ToJson());
            if (HubContext != null)
                HubContext.Clients.Group(resolution.ID).ResolutionSaved(DateTime.Now);
            return filePath;
        }

        private void SaveStack(object state)
        {
            int stackCount = _saveStack.Count;
            while (stackCount > 0)
            {
                var resolution = _saveStack.Pop();
                Save(resolution);
                stackCount = _saveStack.Count;
            }
        }

        private Task StartSaveTask(CancellationToken stoppingToken)
        {
            _saveTimer = new Timer(SaveStack, null, TimeSpan.Zero,
                TimeSpan.FromSeconds(5));
            return Task.CompletedTask;
        }

        private Task StopSaveTask(CancellationToken stoppingToken)
        {
            _saveTimer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void RequestSave(Models.ResolutionModel resolution)
        {
            if (!_saveStack.Contains(resolution))
                _saveStack.Push(resolution);
        }

        public Models.ResolutionModel GetResolution(string id)
        {
            var resolution = resolutions.FirstOrDefault(n => n.ID == id);
            if (resolution == null)
            {
                var resolutionDirectory = DataHandlers.Database.SettingsHandler.GetResolutionDir;
                var filePath = System.IO.Path.Combine(resolutionDirectory, id + ".json");
                if (System.IO.File.Exists(filePath))
                {
                    resolution = GetResolutionFromJson(System.IO.File.ReadAllText(filePath));
                    resolutions.Add(resolution);
                }
                else
                {
                    return null;
                }
            }
            return resolution;
        }

        public Models.ResolutionModel GetResolutionFromJson(string json)
        {
            var resolution = Newtonsoft.Json.JsonConvert.DeserializeObject<Models.ResolutionModel>(json);
            return resolution;
        }

        public string GetResolutionJsonFromFile(string id)
        {
            var resolutionDirectory = DataHandlers.Database.SettingsHandler.GetResolutionDir;
            var filePath = System.IO.Path.Combine(resolutionDirectory, id + ".json");
            if (System.IO.File.Exists(filePath))
            {
                return System.IO.File.ReadAllText(filePath);
            }
            else
            {
                return null;
            }
        }

        private bool SaveResolutionInDatabase(Models.ResolutionModel resolution, bool pread, bool pwrite, string userid = "anon")
        {
            using (var connection = DataHandlers.Database.Connector.Connection)
            {
                var cmdStr = "INSERT INTO resolution (id, name, user, creationdate, lastchangeddate, onlinecode,ispublicread,ispublicwrite) VALUES (" +
                    "@id, @name, @user, @creationdate, @lastchangeddate, @onlinecode, @pread, @pwrite);";
                connection.Open();
                var cmd = new MySqlCommand(cmdStr, connection);
                cmd.Parameters.AddWithValue("@id", resolution.ID);
                cmd.Parameters.AddWithValue("@name", resolution.Topic);
                cmd.Parameters.AddWithValue("@user", userid);
                cmd.Parameters.AddWithValue("@creationdate", DateTime.Now);
                cmd.Parameters.AddWithValue("@lastchangeddate", DateTime.Now);
                cmd.Parameters.AddWithValue("@onlinecode", new Random().Next(10000000, 99999999));
                cmd.Parameters.AddWithValue("@pread", pread);
                cmd.Parameters.AddWithValue("@pwrite", pwrite);
                cmd.ExecuteNonQuery();
                return true;
            }
        }

        public void Dispose()
        {
            _saveTimer?.Dispose();
        }

        public ResolutionService()
        {
            StartSaveTask(CancellationToken.None);
        }
    }

    
}
