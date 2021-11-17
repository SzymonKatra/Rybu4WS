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
                listTrace.Items.Insert(0, new ListViewItem(new string[3]));
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
            if (selected.State == AgentTraceEntry.EntryState.None) return;

            CodeLocationSelected(this, selected.CodeLocation);
        }

        private string[] GetAgentTraceEntryColumns(AgentTraceEntry traceEntry)
        {
            var result = new string[3];
            if (traceEntry.State == AgentTraceEntry.EntryState.None) return result;

            result[0] = traceEntry.CodeLocation.StartLine.ToString();
            result[1] = traceEntry.CodeLocation.EndLine.ToString();
            result[2] = traceEntry.State switch
            {
                AgentTraceEntry.EntryState.Pre => "PRE",
                AgentTraceEntry.EntryState.At => "AT",
                AgentTraceEntry.EntryState.MissingCode => "MISSING CODE AFTER",
                _ => throw new NotImplementedException()
            };

            return result;
        }
    }
}
