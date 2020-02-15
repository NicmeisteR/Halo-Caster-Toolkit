using Newtonsoft.Json;
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

    public List<TeamElement> RedTeamStart = new List<TeamElement>();
    public List<TeamElement> BlueTeamStart = new List<TeamElement>();

    public List<TeamElement> RedTeamCurrent = new List<TeamElement>();
    public List<TeamElement> BlueTeamCurrent = new List<TeamElement>();

    public List<Stats> Combined = new List<Stats>();
    private int StartWins = 0;
    private int StartHeadshots = 0;
    private int StartXp = 0;

    // All the player data is stored in here:
    List<Data> _data = new List<Data>();
    List<string> RedTeam;
    List<string> BlueTeam;

    private void startStop_Click(object sender, EventArgs e)
    {

      RedTeam = new List<string>(new string[] { txt_redOne.Text, txt_redTwo.Text, txt_redThree.Text, txt_redFour.Text });
      BlueTeam = new List<string>(new string[] { txt_blueOne.Text, txt_blueTwo.Text, txt_blueThree.Text, txt_blueFour.Text });

      //foreach (string player in BlueTeam)
      //{
      //  _data.Add(new Data()
      //  {
      //    Name = player,
      //    ID = "2",
      //  });

      //  File.WriteAllText(_outputPath + $"/{player}_data.json", JsonConvert.SerializeObject(_data));
      //}

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
      var ResponseObjects = JsonConvert.DeserializeObject<dynamic>(Get($"stats/h5/servicerecords/custom?players={RedTeam[0]},{RedTeam[1]},{RedTeam[2]},{RedTeam[3]},{BlueTeam[0]},{BlueTeam[1]},{BlueTeam[2]},{BlueTeam[3]}"));

      RedTeamStart.Add(new TeamElement(
        ResponseObjects.Results[0].Result.PlayerId.Gamertag,
        ResponseObjects.Results[0].Result.CustomStats.TotalKills,
        ResponseObjects.Results[0].Result.CustomStats.TotalDeaths,
        ResponseObjects.Results[0].Result.CustomStats.TotalAssists,
        ResponseObjects.Results[0].Result.CustomStats.TotalWeaponDamage
      ));
      RedTeamStart.Add(new TeamElement(
        ResponseObjects.Results[1].Result.PlayerId.Gamertag,
        ResponseObjects.Results[1].Result.CustomStats.TotalKills,
        ResponseObjects.Results[1].Result.CustomStats.TotalDeaths,
        ResponseObjects.Results[1].Result.CustomStats.TotalAssists,
        ResponseObjects.Results[1].Result.CustomStats.TotalWeaponDamage
      ));
      RedTeamStart.Add(new TeamElement(
        ResponseObjects.Results[2].Result.PlayerId.Gamertag,
        ResponseObjects.Results[2].Result.CustomStats.TotalKills,
        ResponseObjects.Results[2].Result.CustomStats.TotalDeaths,
        ResponseObjects.Results[2].Result.CustomStats.TotalAssists,
        ResponseObjects.Results[2].Result.CustomStats.TotalWeaponDamage
      ));
      RedTeamStart.Add(new TeamElement(
        ResponseObjects.Results[3].Result.PlayerId.Gamertag,
        ResponseObjects.Results[3].Result.CustomStats.TotalKills,
        ResponseObjects.Results[3].Result.CustomStats.TotalDeaths,
        ResponseObjects.Results[3].Result.CustomStats.TotalAssists,
        ResponseObjects.Results[3].Result.CustomStats.TotalWeaponDamage
      ));

      BlueTeamStart.Add(new TeamElement (
        ResponseObjects.Results[4].Result.PlayerId.Gamertag,
        ResponseObjects.Results[4].Result.CustomStats.TotalKills,
        ResponseObjects.Results[4].Result.CustomStats.TotalDeaths,
        ResponseObjects.Results[4].Result.CustomStats.TotalAssists,
        ResponseObjects.Results[4].Result.CustomStats.TotalWeaponDamage
      ));
      BlueTeamStart.Add(new TeamElement(
        ResponseObjects.Results[5].Result.PlayerId.Gamertag,
        ResponseObjects.Results[5].Result.CustomStats.TotalKills,
        ResponseObjects.Results[5].Result.CustomStats.TotalDeaths,
        ResponseObjects.Results[5].Result.CustomStats.TotalAssists,
        ResponseObjects.Results[5].Result.CustomStats.TotalWeaponDamage
      ));
      BlueTeamStart.Add(new TeamElement(
        ResponseObjects.Results[6].Result.PlayerId.Gamertag,
        ResponseObjects.Results[6].Result.CustomStats.TotalKills,
        ResponseObjects.Results[6].Result.CustomStats.TotalDeaths,
        ResponseObjects.Results[6].Result.CustomStats.TotalAssists,
        ResponseObjects.Results[6].Result.CustomStats.TotalWeaponDamage
      ));
      BlueTeamStart.Add(new TeamElement(
        ResponseObjects.Results[7].Result.PlayerId.Gamertag,
        ResponseObjects.Results[7].Result.CustomStats.TotalKills,
        ResponseObjects.Results[7].Result.CustomStats.TotalDeaths,
        ResponseObjects.Results[7].Result.CustomStats.TotalAssists,
        ResponseObjects.Results[7].Result.CustomStats.TotalWeaponDamage
      ));

      RedTeamCurrent.Add(new TeamElement(
        ResponseObjects.Results[0].Result.PlayerId.Gamertag,
        ResponseObjects.Results[0].Result.CustomStats.TotalKills,
        ResponseObjects.Results[0].Result.CustomStats.TotalDeaths,
        ResponseObjects.Results[0].Result.CustomStats.TotalAssists,
        ResponseObjects.Results[0].Result.CustomStats.TotalWeaponDamage
      ));
      RedTeamCurrent.Add(new TeamElement(
        ResponseObjects.Results[1].Result.PlayerId.Gamertag,
        ResponseObjects.Results[1].Result.CustomStats.TotalKills,
        ResponseObjects.Results[1].Result.CustomStats.TotalDeaths,
        ResponseObjects.Results[1].Result.CustomStats.TotalAssists,
        ResponseObjects.Results[1].Result.CustomStats.TotalWeaponDamage
      ));
      RedTeamCurrent.Add(new TeamElement(
        ResponseObjects.Results[2].Result.PlayerId.Gamertag,
        ResponseObjects.Results[2].Result.CustomStats.TotalKills,
        ResponseObjects.Results[2].Result.CustomStats.TotalDeaths,
        ResponseObjects.Results[2].Result.CustomStats.TotalAssists,
        ResponseObjects.Results[2].Result.CustomStats.TotalWeaponDamage
      ));
      RedTeamCurrent.Add(new TeamElement(
        ResponseObjects.Results[3].Result.PlayerId.Gamertag,
        ResponseObjects.Results[3].Result.CustomStats.TotalKills,
        ResponseObjects.Results[3].Result.CustomStats.TotalDeaths,
        ResponseObjects.Results[3].Result.CustomStats.TotalAssists,
        ResponseObjects.Results[3].Result.CustomStats.TotalWeaponDamage
      ));

      BlueTeamCurrent.Add(new TeamElement(
        ResponseObjects.Results[4].Result.PlayerId.Gamertag,
        ResponseObjects.Results[4].Result.CustomStats.TotalKills,
        ResponseObjects.Results[4].Result.CustomStats.TotalDeaths,
        ResponseObjects.Results[4].Result.CustomStats.TotalAssists,
        ResponseObjects.Results[4].Result.CustomStats.TotalWeaponDamage
      ));
      BlueTeamCurrent.Add(new TeamElement(
        ResponseObjects.Results[5].Result.PlayerId.Gamertag,
        ResponseObjects.Results[5].Result.CustomStats.TotalKills,
        ResponseObjects.Results[5].Result.CustomStats.TotalDeaths,
        ResponseObjects.Results[5].Result.CustomStats.TotalAssists,
        ResponseObjects.Results[5].Result.CustomStats.TotalWeaponDamage
      ));
      BlueTeamCurrent.Add(new TeamElement(
        ResponseObjects.Results[6].Result.PlayerId.Gamertag,
        ResponseObjects.Results[6].Result.CustomStats.TotalKills,
        ResponseObjects.Results[6].Result.CustomStats.TotalDeaths,
        ResponseObjects.Results[6].Result.CustomStats.TotalAssists,
        ResponseObjects.Results[6].Result.CustomStats.TotalWeaponDamage
      ));
      BlueTeamCurrent.Add(new TeamElement(
        ResponseObjects.Results[7].Result.PlayerId.Gamertag,
        ResponseObjects.Results[7].Result.CustomStats.TotalKills,
        ResponseObjects.Results[7].Result.CustomStats.TotalDeaths,
        ResponseObjects.Results[7].Result.CustomStats.TotalAssists,
        ResponseObjects.Results[7].Result.CustomStats.TotalWeaponDamage
      ));


      Combined.Add(new Stats(
        RedTeamCurrent,
        BlueTeamCurrent
      ));


      File.WriteAllText(_outputPath + $"/RedTeamStart.json", JsonConvert.SerializeObject(RedTeamStart));
      File.WriteAllText(_outputPath + $"/BlueTeamStart_data.json", JsonConvert.SerializeObject(BlueTeamStart));

      StartWins = ResponseObjects.Results[0].Result.CustomStats.TotalGamesWon;
      //StartKills = ResponseObjects.Results[0].Result.CustomStats.TotalKills;
      StartHeadshots = ResponseObjects.Results[0].Result.CustomStats.TotalHeadshots;
      StartXp = ResponseObjects.Results[0].Result.Xp;
    }

    void ShowStats()
    {
      var ResponseObjects = JsonConvert.DeserializeObject<dynamic>(Get($"stats/h5/servicerecords/custom?players={RedTeam[0]},{RedTeam[1]},{RedTeam[2]},{RedTeam[3]},{BlueTeam[0]},{BlueTeam[1]},{BlueTeam[2]},{BlueTeam[3]}"));


      RedTeamCurrent[0].Kills = ResponseObjects.Results[0].Result.CustomStats.TotalKills - RedTeamStart[0].Kills;
      RedTeamCurrent[0].Deaths = ResponseObjects.Results[0].Result.CustomStats.TotalDeaths - RedTeamStart[0].Deaths;
      RedTeamCurrent[0].Assists = ResponseObjects.Results[0].Result.CustomStats.TotalAssists - RedTeamStart[0].Assists;
      RedTeamCurrent[0].Damage = ResponseObjects.Results[0].Result.CustomStats.TotalWeaponDamage - RedTeamStart[0].Damage;

      RedTeamCurrent[1].Kills = ResponseObjects.Results[1].Result.CustomStats.TotalKills - RedTeamStart[1].Kills;
      RedTeamCurrent[1].Deaths = ResponseObjects.Results[1].Result.CustomStats.TotalDeaths - RedTeamStart[1].Deaths;
      RedTeamCurrent[1].Assists = ResponseObjects.Results[1].Result.CustomStats.TotalAssists - RedTeamStart[1].Assists;
      RedTeamCurrent[1].Damage = ResponseObjects.Results[1].Result.CustomStats.TotalWeaponDamage - RedTeamStart[1].Damage;
                     
      RedTeamCurrent[2].Kills = ResponseObjects.Results[2].Result.CustomStats.TotalKills - RedTeamStart[2].Kills;
      RedTeamCurrent[2].Deaths = ResponseObjects.Results[2].Result.CustomStats.TotalDeaths - RedTeamStart[2].Deaths;
      RedTeamCurrent[2].Assists = ResponseObjects.Results[2].Result.CustomStats.TotalAssists - RedTeamStart[2].Assists;
      RedTeamCurrent[2].Damage = ResponseObjects.Results[2].Result.CustomStats.TotalWeaponDamage - RedTeamStart[2].Damage;

      RedTeamCurrent[3].Kills = ResponseObjects.Results[3].Result.CustomStats.TotalKills - RedTeamStart[3].Kills;
      RedTeamCurrent[3].Deaths = ResponseObjects.Results[3].Result.CustomStats.TotalDeaths - RedTeamStart[3].Deaths;
      RedTeamCurrent[3].Assists = ResponseObjects.Results[3].Result.CustomStats.TotalAssists - RedTeamStart[3].Assists;
      RedTeamCurrent[3].Damage = ResponseObjects.Results[3].Result.CustomStats.TotalWeaponDamage - RedTeamStart[3].Damage;

      BlueTeamCurrent[0].Kills = ResponseObjects.Results[4].Result.CustomStats.TotalKills - BlueTeamStart[0].Kills;
      BlueTeamCurrent[0].Deaths = ResponseObjects.Results[4].Result.CustomStats.TotalDeaths - BlueTeamStart[0].Deaths;
      BlueTeamCurrent[0].Assists = ResponseObjects.Results[4].Result.CustomStats.TotalAssists - BlueTeamStart[0].Assists;
      BlueTeamCurrent[0].Damage = ResponseObjects.Results[4].Result.CustomStats.TotalWeaponDamage - BlueTeamStart[0].Damage;

      BlueTeamCurrent[1].Kills = ResponseObjects.Results[5].Result.CustomStats.TotalKills - BlueTeamStart[1].Kills;
      BlueTeamCurrent[1].Deaths = ResponseObjects.Results[5].Result.CustomStats.TotalDeaths - BlueTeamStart[1].Deaths;
      BlueTeamCurrent[1].Assists = ResponseObjects.Results[5].Result.CustomStats.TotalAssists - BlueTeamStart[1].Assists;
      BlueTeamCurrent[1].Damage = ResponseObjects.Results[5].Result.CustomStats.TotalWeaponDamage - BlueTeamStart[1].Damage;

      BlueTeamCurrent[2].Kills = ResponseObjects.Results[6].Result.CustomStats.TotalKills - BlueTeamStart[2].Kills;
      BlueTeamCurrent[2].Deaths = ResponseObjects.Results[6].Result.CustomStats.TotalDeaths - BlueTeamStart[2].Deaths;
      BlueTeamCurrent[2].Assists = ResponseObjects.Results[6].Result.CustomStats.TotalAssists - BlueTeamStart[2].Assists;
      BlueTeamCurrent[2].Damage = ResponseObjects.Results[6].Result.CustomStats.TotalWeaponDamage - BlueTeamStart[2].Damage;

      BlueTeamCurrent[3].Kills = ResponseObjects.Results[6].Result.CustomStats.TotalKills - BlueTeamStart[3].Kills;
      BlueTeamCurrent[3].Deaths = ResponseObjects.Results[6].Result.CustomStats.TotalDeaths - BlueTeamStart[3].Deaths;
      BlueTeamCurrent[3].Assists = ResponseObjects.Results[6].Result.CustomStats.TotalAssists - BlueTeamStart[3].Assists;
      BlueTeamCurrent[3].Damage = ResponseObjects.Results[6].Result.CustomStats.TotalWeaponDamage - BlueTeamStart[3].Damage;

      Combined[0].Redteam = RedTeamCurrent;
      Combined[0].Blueteam = BlueTeamCurrent;

      File.WriteAllText(_outputPath + $"/TeamStats.json", JsonConvert.SerializeObject(Combined));
      //var Kills = $"Kills: {ResponseObjects.Results[0].Result.CustomStats.TotalKills - StartKills}";
      //var Wins = $"Wins: {ResponseObjects.Results[0].Result.CustomStats.TotalGamesWon - StartWins}";
      //var Headshots = $"Headshots: {ResponseObjects.Results[0].Result.CustomStats.TotalHeadshots - StartHeadshots}";
      //var Xp = $"XP: {ResponseObjects.Results[0].Result.Xp - StartXp}";

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
      //File.WriteAllText(_outputPath + "/data.json", Kills);
      //File.WriteAllText(_outputPath + "/kills.txt", Kills);
      //File.WriteAllText(_outputPath + "/wins.txt", Wins);
      //File.WriteAllText(_outputPath + "/headshots.txt", Headshots);
      //File.WriteAllText(_outputPath + "/xp.txt", Xp);
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
  class Data
  {
    public string Name;
    public string ID;
  }
}
