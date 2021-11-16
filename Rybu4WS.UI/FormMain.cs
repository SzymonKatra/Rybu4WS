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
        private Dictionary<string, ServerStateControl> _serverStateControls;
        private Dictionary<string, AgentStateControl> _agentStateControls;
        private static readonly List<Color> PreDefinedAgentColors = new List<Color>() { Color.Red, Color.Blue, Color.Green, Color.Yellow };

        public FormMain()
        {
            InitializeComponent();
        }

        private void buttLoad_Click(object sender, EventArgs e)
        {
            _code = File.ReadAllText(@"c:\Users\szymo\Repos\mgr\Rybu4WS.Test\IntegrationTests\bank.txt");
            _debugger = new TrailDebugger.Debugger(File.ReadAllText(@"c:\Users\szymo\Desktop\banking_dedan_tester_2,First+Second,Termination.XML"));

            txtCode.Text = _code;
            _serverStateControls = new Dictionary<string, ServerStateControl>();
            foreach (var serverName in _debugger.GetServerNames())
            {
                var serverStateControl = new ServerStateControl() { ServerName = serverName };
                serverStateControl.UpdateVariables(_debugger.GetServerVariables(serverName));
                _serverStateControls.Add(serverName, serverStateControl);
                flowLayoutServers.Controls.Add(serverStateControl);
            }

            _agentStateControls = new Dictionary<string, AgentStateControl>();
            int colorIndex = 0;
            foreach (var agentName in _debugger.GetAgentNames())
            {
                var agentStateControl = new AgentStateControl() { AgentName = agentName, AgentColor = PreDefinedAgentColors[colorIndex++] };
                agentStateControl.CodeLocationSelected += AgentStateControl_CodeLocationSelected;
                agentStateControl.UpdateExecutionTrace(_debugger.GetAgentExecutionTrace(agentName));
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
                _agentStateControls[agentChanged].UpdateExecutionTrace(_debugger.GetAgentExecutionTrace(agentChanged));

                UpdateVisualTraces();
            }
        }

        private void UpdateVisualTraces()
        {
            txtCode.Select(0, txtCode.Text.Length - 1);
            txtCode.SelectionBackColor = Color.Transparent;

            foreach (var agentName in _debugger.GetAgentNames())
            {
                var trace = _debugger.GetAgentExecutionTrace(agentName);
                var color = _agentStateControls[agentName].AgentColor;

                foreach (var item in trace)
                {
                    if (!item.HasValue) continue;
                    txtCode.Select(item.Value.StartIndex, item.Value.EndIndex - item.Value.StartIndex + 1);
                    txtCode.SelectionBackColor = color;
                }
            }
        }
    }
}
