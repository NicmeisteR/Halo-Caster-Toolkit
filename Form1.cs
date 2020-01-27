﻿using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Halo_Streamer_Tools
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            startStop.BackColor = Color.LawnGreen;
        }

        private bool start = true;
        private string _outputPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop).ToString();
        private int _seconds = 5;
        private CancellationTokenSource cancelSource;


        private string ApiKey = "";
        private int MaxXp = 50000000;

        private string redOne;
        private string redTwo;
        private string redThree;
        private string redFour;

        private int StartKills = 0;
        private int StartWins = 0;
        private int StartHeadshots = 0;
        private int StartXp = 0;

        private void startStop_Click(object sender, EventArgs e)
        {
          redOne = txt_redOne.Text;
          redTwo = txt_redOne.Text;
          redThree = txt_redOne.Text;
          redFour = txt_redOne.Text;

      if (start == true)
            {
                Start();
                cancelSource = new CancellationTokenSource();
                var timespan = TimeSpan.FromSeconds(60);
                var task = WatcherTask(timespan, cancelSource.Token);
                task.Start();
                //var input = Console.ReadLine();
                startStop.Text = "Stop";
                startStop.BackColor = Color.Red;
                start = false;
            }
            else
            {
                cancelSource.Cancel();
                startStop.Text = "Start";
                startStop.BackColor = Color.Green;
                start = true;
            }
        }

        string Get(string url)
        {

            var client = new RestClient($"https://www.haloapi.com/{url}");
            var request = new RestRequest(Method.GET);
            request.AddHeader("ocp-apim-subscription-key", ApiKey);
            IRestResponse response = client.Execute(request);
            return response.Content;

        }

        public void Start()
        {
            var ResponseObjects = JsonConvert.DeserializeObject<dynamic>(Get($"stats/h5/servicerecords/arena?players={redOne},{redTwo},{redThree},{redFour}"));
            StartWins = ResponseObjects.Results[0].Result.ArenaStats.TotalGamesWon;
            StartKills = ResponseObjects.Results[0].Result.ArenaStats.TotalKills;
            StartHeadshots = ResponseObjects.Results[0].Result.ArenaStats.TotalHeadshots ;
            StartXp = ResponseObjects.Results[0].Result.Xp;
        }

        void ShowStats()
        {
            var ResponseObject = JsonConvert.DeserializeObject<dynamic>(Get($"stats/h5/servicerecords/arena?players={redOne},{redTwo},{redThree},{redFour}"));

            var Kills = $"Kills: {ResponseObject.Results[0].Result.ArenaStats.TotalKills - StartKills}";
            var Wins = $"Wins: {ResponseObject.Results[0].Result.ArenaStats.TotalGamesWon - StartWins}";
            var Headshots = $"Headshots: {ResponseObject.Results[0].Result.ArenaStats.TotalHeadshots - StartHeadshots}";
            var Xp = $"XP: {ResponseObject.Results[0].Result.Xp - StartXp}";

            //if (lbl_winstotal.InvokeRequired)
            //{
            //    lbl_winstotal.Invoke(new MethodInvoker(delegate { lbl_winstotal.Text = Wins; }));
            //}
            //else
            //{
            //    lbl_winstotal.Text = Wins;
            //}

            //if (lbl_kills.InvokeRequired)
            //{
            //    lbl_kills.Invoke(new MethodInvoker(delegate { lbl_kills.Text = Kills; }));
            //}
            //else
            //{
            //    lbl_kills.Text = Kills;
            //}

            //if (lbl_headshots.InvokeRequired)
            //{
            //    lbl_headshots.Invoke(new MethodInvoker(delegate { lbl_headshots.Text = Headshots; }));
            //}
            //else
            //{
            //    lbl_headshots.Text = Headshots;
            //}

            //if (lbl_xp.InvokeRequired)
            //{
            //    lbl_xp.Invoke(new MethodInvoker(delegate { lbl_xp.Text = Xp; }));
            //}
            //else
            //{
            //    lbl_xp.Text = Xp;
            //}

            if (label3.InvokeRequired)
            {
                label3.Invoke(new MethodInvoker(delegate { label3.Text = "Last Update: " + DateTime.Now.ToString("h:mm:ss tt"); }));
            }
            else
            {
                label3.Text = DateTime.Now.ToString("h:mm:ss tt");
            }

            //Console.WriteLine($"{kills}\n{wins}\n{headshots}");
            //Console.WriteLine(DateTime.Now.ToString("h:mm:ss tt") + " " + Kills);
            File.WriteAllText(_outputPath + "/data.json", Kills);
            File.WriteAllText(_outputPath + "/kills.txt", Kills);
            File.WriteAllText(_outputPath + "/wins.txt", Wins);
            File.WriteAllText(_outputPath + "/headshots.txt", Headshots);
            File.WriteAllText(_outputPath + "/xp.txt", Xp);
        }

        Task WatcherTask(TimeSpan seconds, CancellationToken cancellationToken)
        {
            return new Task(async () =>
            {
                while (true)
                {
                    if (cancellationToken.IsCancellationRequested)
                    {
                        return;
                    }
                    ShowStats();
                    await Task.Delay(seconds);
                }
            }, cancellationToken);
        }
    }
}
