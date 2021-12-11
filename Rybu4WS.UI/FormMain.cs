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
        private string _codePath;
        private string _trailPath;
        private Language.System _loadedSystem;
        private TrailDebugger.Debugger _debugger;
        private Dictionary<string, ServerStateControl> _serverStateControls = new Dictionary<string, ServerStateControl>();
        private Dictionary<string, AgentStateControl> _agentStateControls = new Dictionary<string, AgentStateControl>();
        private static readonly List<Color> PreDefinedAgentColors = new List<Color>() { Color.Blue, Color.Red, Color.Green, Color.Purple, Color.Silver, Color.Magenta, Color.Beige, Color.SaddleBrown };

        public FormMain()
        {
            InitializeComponent();
        }

        private void UpdateDebuggerUI(IEnumerable<string> servers, IEnumerable<string> agents)
        {
            if (servers.Any())
            {
                foreach (var server in servers)
                {
                    _serverStateControls[server].UpdateVariables(_debugger.GetServerVariables(server));
                }
            }
            if (agents.Any())
            {
                foreach (var agent in agents)
                {
                    _agentStateControls[agent].UpdateTrace(_debugger.GetAgentTrace(agent));
                }

                UpdateVisualTraces();
            }

            var nextAgent = _debugger.GetNextAgent();
            foreach (var item in _agentStateControls)
            {
                item.Value.IsNext = item.Key == nextAgent;
            }
        }

        private void AgentStateControl_CodeLocationSelected(object sender, Language.CodeLocation e)
        {
            txtCode.Focus();
            txtCode.Select(e.StartIndex, e.EndIndex - e.StartIndex + 1);
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
                    if (!trace[i].CodeLocation.HasValue) continue;
                    var codeLocation = trace[i].CodeLocation.Value;

                    txtCode.Select(codeLocation.StartIndex, codeLocation.EndIndex - codeLocation.StartIndex + 1);
                    txtCode.SelectionBackColor = i == 0 ? color : Color.FromArgb((byte)Math.Clamp(color.R * 0.5, 32, 223), (byte)Math.Clamp(color.G * 0.5, 32, 223), (byte)Math.Clamp(color.B * 0.5, 32, 223));
                    txtCode.SelectionColor = Color.White;
                }
            }
        }

        private IEnumerable<string> ReadAllLines(Stream stream)
        {
            using (var reader = new StreamReader(stream))
            {
                while (!reader.EndOfStream)
                {
                    yield return reader.ReadLine();
                }
            }
        }

        private void buttSaveDedanModel_Click(object sender, EventArgs e)
        {
            var fileDialog = new SaveFileDialog();
            fileDialog.FileName = $"{Path.GetFileNameWithoutExtension(_codePath)}_dedan.txt";
            fileDialog.Filter = CreateDialogFilter(hasTxt: true);
            fileDialog.ShowDialog();
            if (string.IsNullOrEmpty(fileDialog.FileName)) return;

            string dedanCode = ConvertToDedan();

            if (dedanCode != null)
            {
                File.WriteAllText(fileDialog.FileName, dedanCode);
            }
        }

        private string ConvertToDedan()
        {
            Func<(string dedanResult, List<string> errors)> convertFunc = () =>
            {
                var errorStream = new MemoryStream();
                var stateMachine = StateMachine.Converter.ConvertToStateMachine(_loadedSystem, errorStream);

                errorStream.Flush();
                if (errorStream.Length > 0)
                {
                    errorStream.Seek(0, SeekOrigin.Begin);
                    var errors = ReadAllLines(errorStream);

                    return (null, errors.ToList());
                }

                return (stateMachine.ToDedan(), null);
            };

            var result = FormProcessing<(string dedanResult, List<string> errors)>.Start(convertFunc, "Converting to DedAn model in progres...", this);
            if (result.errors != null)
            {
                var formErrors = new FormFileErrors(_codePath, result.errors);
                formErrors.HighlightError += FormErrors_HighlightError;
                formErrors.Show();
                return null;
            }

            return result.dedanResult;
        }

        private void buttLoadCode_Click(object sender, EventArgs e)
        {
            var path = OpenFile(CreateDialogFilter(hasTxt: true));
            if (path == null) return;

            LoadCode(path);
        }

        private void LoadCode(string path)
        {
            _code = File.ReadAllText(path).Replace("\r\n", "\n");
            _codePath = path;

            txtCode.Text = _code;
            lblCodePath.Text = _codePath.Length > 50 ? "..." + _codePath.Substring(_codePath.Length - 50) : _codePath;

            var errorStream = new MemoryStream();
            _loadedSystem = Language.Parser.Rybu4WS.Parse(_code, errorStream);
            errorStream.Flush();
            if (errorStream.Length > 0)
            {
                errorStream.Seek(0, SeekOrigin.Begin);
                var errors = ReadAllLines(errorStream);

                var formErrors = new FormFileErrors(path, errors);
                formErrors.HighlightError += FormErrors_HighlightError;
                formErrors.Show();
                return;
            }

            buttSaveDedanModel.Enabled = true;
            buttLoadDedanTrail.Enabled = true;
            buttVerifyDeadlock.Enabled = true;
            buttVerifyTermination.Enabled = true;
            buttReloadCode.Enabled = true;
            _debugger = null;
            lblDedanTrailPath.Text = "...";
            buttReloadDedanTrail.Enabled = false;
            buttDebuggerReset.Enabled = false;
            buttDebuggerStep.Enabled = false;

            _serverStateControls.Clear();
            flowLayoutServers.Controls.Clear();
            _agentStateControls.Clear();
            flowLayoutAgents.Controls.Clear();
        }

        private void FormErrors_HighlightError(object sender, (int line, int column) e)
        {
            this.Focus();
            txtCode.Focus();
            txtCode.Select(txtCode.GetFirstCharIndexFromLine(e.line - 1) + e.column - 1, 0);
        }

        private void buttDebuggerStep_Click(object sender, EventArgs e)
        {
            var (serverChanged, agentChanged) = _debugger.Step();
            UpdateDebuggerUI(
                serverChanged != null ? new[] { serverChanged } : Enumerable.Empty<string>(),
                agentChanged != null ? new[] { agentChanged } : Enumerable.Empty<string>());
            buttDebuggerStep.Enabled = _debugger.CanStep();
        }

        private void buttDebuggerReset_Click(object sender, EventArgs e)
        {
            _debugger.Reset();
            UpdateDebuggerUI(_debugger.GetServerNames(), _debugger.GetAgentNames());
            buttDebuggerStep.Enabled = _debugger.CanStep();
        }

        private void buttLoadDedanTrail_Click(object sender, EventArgs e)
        {
            var path = OpenFile(CreateDialogFilter(hasXml: true));
            if (path == null) return;

            LoadDedanTrail(path);
        }

        private void LoadDedanTrail(string path)
        {
            _trailPath = path;

            _debugger = new TrailDebugger.Debugger(File.ReadAllText(path));
            lblDedanTrailPath.Text = _trailPath.Length > 50 ? "..." + _trailPath.Substring(_trailPath.Length - 50) : _trailPath;

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
                var agentStateControl = new AgentStateControl() { AgentName = agentName, AgentColor = PreDefinedAgentColors[colorIndex++], IsNext = false };
                agentStateControl.CodeLocationSelected += AgentStateControl_CodeLocationSelected;
                agentStateControl.UpdateTrace(_debugger.GetAgentTrace(agentName));
                _agentStateControls.Add(agentName, agentStateControl);
                flowLayoutAgents.Controls.Add(agentStateControl);
            }

            UpdateVisualTraces();

            buttDebuggerReset.Enabled = true;
            buttDebuggerStep.Enabled = true;
            buttReloadDedanTrail.Enabled = true;
        }

        private string OpenFile(string dialogFilter)
        {
            var fileDialog = new OpenFileDialog();
            fileDialog.Filter = dialogFilter;
            fileDialog.ShowDialog();
            if (string.IsNullOrEmpty(fileDialog.FileName) || !File.Exists(fileDialog.FileName)) return null;

            return fileDialog.FileName;
        }

        private string CreateDialogFilter(bool hasTxt = false, bool hasXml = false)
        {
            var filters = new List<(string name, string filter)>();
            if (hasTxt)
            {
                filters.Add(("Txt files (*.txt)", "*.txt"));
            }
            if (hasXml)
            {
                filters.Add(("Xml files (*.xml)", "*.xml"));
            }
            filters.Add(("All files (*.*)", "*.*"));
            return string.Join("|", filters.Select(x => string.Join("|", x.name, x.filter)));
        }

        private void txtCode_SelectionChanged(object sender, EventArgs e)
        {
            var line = txtCode.GetLineFromCharIndex(txtCode.SelectionStart);
            var column = txtCode.SelectionStart - txtCode.GetFirstCharIndexFromLine(line);
            lblCursorPos.Text = $"L: {line + 1} C: {column + 1}";
        }

        private void buttReloadCode_Click(object sender, EventArgs e)
        {
            LoadCode(_codePath);
        }

        private void buttReloadDedanTrail_Click(object sender, EventArgs e)
        {
            LoadDedanTrail(_trailPath);
        }

        private void buttVerify_Click(object sender, EventArgs e)
        {
            ctxMenuStripVerify.Show(Cursor.Position);
        }

        private void ConvertAndRunDedan(DedanRunner.VerificationMode mode)
        {
            var dedanCode = ConvertToDedan();
            if (dedanCode == null) return;

            Func<DedanRunner.DedanResult> func = () =>
            {
                var result = DedanRunner.Verify(dedanCode, mode);
                return result;
            };

            var result = FormProcessing<DedanRunner.DedanResult>.Start(func, "DedAn verification in progress...", this);
            if (!result.Succeed)
            {
                MessageBox.Show(result.ErrorMessage ?? "", "DedAn error");

                if (result.TrailExists)
                {
                    var mboxResult = MessageBox.Show("Some error occurred, but trail has been generated by DedAn. Do you want to continue and load it?", "DedAn generated trail", MessageBoxButtons.YesNo);
                    if (mboxResult == DialogResult.Yes)
                    {
                        LoadDedanTrail(result.TrailPath);
                    }
                }
            }
            else
            {
                LoadDedanTrail(result.TrailPath);
            }
        }

        private void toolStripItemDeadlock_Click(object sender, EventArgs e)
        {
            ConvertAndRunDedan(DedanRunner.VerificationMode.Deadlock);
        }

        private void toolStripItemTermination_Click(object sender, EventArgs e)
        {
            ConvertAndRunDedan(DedanRunner.VerificationMode.Termination);
        }

        private void toolStripItemPossibleTermination_Click(object sender, EventArgs e)
        {
            ConvertAndRunDedan(DedanRunner.VerificationMode.PossibleTermination);
        }
    }
}
