using Rybu4WS.TrailDebugger;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Rybu4WS.UI
{
    public partial class AgentStateControl : UserControl
    {
        public string AgentName
        {
            get => lblAgentName.Text;
            set => lblAgentName.Text = value;
        }

        public Color AgentColor
        {
            get => lblAgentName.ForeColor;
            set => lblAgentName.ForeColor = value;
        }

        public bool IsNext
        {
            get => lblNext.Visible;
            set => lblNext.Visible = value;
        }

        public event EventHandler<Language.CodeLocation> CodeLocationSelected;

        public AgentStateControl()
        {
            InitializeComponent();
        }

        public void UpdateTrace(IReadOnlyList<TrailDebugger.AgentTraceEntry> executionTrace)
        {
            while (listTrace.Items.Count > executionTrace.Count)
            {
                listTrace.Items.RemoveAt(0);
            }
            while (listTrace.Items.Count < executionTrace.Count)
            {
                listTrace.Items.Insert(0, new ListViewItem(new string[4]));
            }

            for (int i = 0; i < executionTrace.Count; i++)
            {
                listTrace.Items[i].Tag = executionTrace[i];
                var str = GetAgentTraceEntryColumns(executionTrace[i]);
                for (int j = 0; j < listTrace.Items[i].SubItems.Count; j++)
                {
                    listTrace.Items[i].SubItems[j].Text = str[j];
                }
            }
        }

        private void listTrace_DoubleClick(object sender, EventArgs e)
        {
            if (listTrace.SelectedItems.Count != 1) return;

            var selected = listTrace.SelectedItems[0].Tag as AgentTraceEntry;
            if (!selected.CodeLocation.HasValue) return;

            CodeLocationSelected(this, selected.CodeLocation.Value);
        }

        private string[] GetAgentTraceEntryColumns(AgentTraceEntry traceEntry)
        {
            var result = new string[4];
            if (traceEntry.State == AgentTraceEntry.EntryState.None) return result;

            if (traceEntry.CodeLocation.HasValue)
            {
                result[0] = traceEntry.CodeLocation.Value.StartLine.ToString();
                result[1] = (traceEntry.CodeLocation.Value.StartColumn + 1).ToString();
            }

            result[2] = traceEntry.ServerName;

            result[3] = traceEntry.State switch
            {
                AgentTraceEntry.EntryState.Pre => "PRE",
                AgentTraceEntry.EntryState.Post => "POST",
                AgentTraceEntry.EntryState.At => "AT",
                AgentTraceEntry.EntryState.MissingCode => "UNKNOWN NEXT STEP",
                AgentTraceEntry.EntryState.Calling => $"CALLING {traceEntry.CallingActionName}",
                AgentTraceEntry.EntryState.Returned => $"RETURNED {traceEntry.ReturnValue}",
                AgentTraceEntry.EntryState.Terminating => $"TERMINATING",
                _ => throw new NotImplementedException()
            };

            return result;
        }
    }
}
