using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Rybu4WS.UI
{
    public partial class FormMain : Form
    {
        private string _code;
        private TrailDebugger.Debugger _debugger;
        private Dictionary<string, ServerStateControl> _serverStateControls = new Dictionary<string, ServerStateControl>();
        private Dictionary<string, AgentStateControl> _agentStateControls = new Dictionary<string, AgentStateControl>();
        private static readonly List<Color> PreDefinedAgentColors = new List<Color>() { Color.Blue, Color.Red, Color.Green, Color.Purple };

        public FormMain()
        {
            InitializeComponent();
        }

        private void buttLoad_Click(object sender, EventArgs e)
        {
            _code = File.ReadAllText(@"c:\Users\szymo\Repos\mgr\Rybu4WS.Test\IntegrationTests\deadlock.txt");
            _debugger = new TrailDebugger.Debugger(File.ReadAllText(@"c:\Users\szymo\Desktop\deadlock_dedan,First,Deadlock.XML"));


            //_code = File.ReadAllText(@"c:\Users\szymo\Repos\mgr\Rybu4WS.Test\IntegrationTests\bank.txt");
            //_debugger = new TrailDebugger.Debugger(File.ReadAllText(@"c:\Users\szymo\Desktop\banking_dedan_tester_2,First+Second,Termination.XML"));
            //_debugger = new TrailDebugger.Debugger(File.ReadAllText(@"c:\Users\szymo\Desktop\banking_dedan_tester_2,First+Second,No_term.XML"));

            txtCode.Text = _code;
            _serverStateControls.Clear();
            flowLayoutServers.Controls.Clear();
            foreach (var serverName in _debugger.GetServerNames())
            {
                var serverStateControl = new ServerStateControl() { ServerName = serverName };
                serverStateControl.UpdateVariables(_debugger.GetServerVariables(serverName));
                _serverStateControls.Add(serverName, serverStateControl);
                flowLayoutServers.Controls.Add(serverStateControl);
            }

            _agentStateControls.Clear();
            flowLayoutAgents.Controls.Clear();
            int colorIndex = 0;
            foreach (var agentName in _debugger.GetAgentNames())
            {
                var agentStateControl = new AgentStateControl() { AgentName = agentName, AgentColor = PreDefinedAgentColors[colorIndex++] };
                agentStateControl.CodeLocationSelected += AgentStateControl_CodeLocationSelected;
                agentStateControl.UpdateTrace(_debugger.GetAgentTrace(agentName));
                _agentStateControls.Add(agentName, agentStateControl);
                flowLayoutAgents.Controls.Add(agentStateControl);
            }
        }

        private void AgentStateControl_CodeLocationSelected(object sender, Language.CodeLocation e)
        {
            txtCode.Focus();
            txtCode.Select(e.StartIndex, e.EndIndex - e.StartIndex + 1);
        }

        private void buttStep_Click(object sender, EventArgs e)
        {
            if (!_debugger.CanStep())
            {
                MessageBox.Show("Cannot step more");
                return;
            }

            var (serverChanged, agentChanged) = _debugger.Step();

            if (serverChanged != null)
            {
                _serverStateControls[serverChanged].UpdateVariables(_debugger.GetServerVariables(serverChanged));
            }
            if (agentChanged != null)
            {
                _agentStateControls[agentChanged].UpdateTrace(_debugger.GetAgentTrace(agentChanged));

                UpdateVisualTraces();
            }
        }

        private void UpdateVisualTraces()
        {
            txtCode.Select(0, txtCode.Text.Length - 1);
            txtCode.SelectionBackColor = Color.White;
            txtCode.SelectionColor = Color.Black;

            foreach (var agentName in _debugger.GetAgentNames())
            {
                var trace = _debugger.GetAgentTrace(agentName);
                var color = _agentStateControls[agentName].AgentColor;

                for (int i = 0; i < trace.Count; i++)
                {
                    if (trace[i].State == TrailDebugger.AgentTraceEntry.EntryState.None) continue;
                    var codeLocation = trace[i].CodeLocation;

                    txtCode.Select(codeLocation.StartIndex, codeLocation.EndIndex - codeLocation.StartIndex + 1);
                    txtCode.SelectionBackColor = i == 0 ? color : Color.FromArgb((byte)Math.Clamp(color.R * 0.5, 32, 223), (byte)Math.Clamp(color.G * 0.5, 32, 223), (byte)Math.Clamp(color.B * 0.5, 32, 223));
                    txtCode.SelectionColor = Color.White;
                }
            }
        }
    }
}
