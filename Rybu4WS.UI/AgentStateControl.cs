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

        public void UpdateExecutionTrace(IReadOnlyList<Language.CodeLocation?> executionTrace)
        {
            while (listTrace.Items.Count > executionTrace.Count)
            {
                listTrace.Items.RemoveAt(0);
            }
            while (listTrace.Items.Count < executionTrace.Count)
            {
                listTrace.Items.Insert(0, new ListViewItem());
            }

            for (int i = 0; i < executionTrace.Count; i++)
            {
                listTrace.Items[i].Text = executionTrace[i]?.ToString() ?? "";
            }
        }

        private void listTrace_DoubleClick(object sender, EventArgs e)
        {
            if (listTrace.SelectedItems.Count != 1) return;

            var selected = listTrace.SelectedItems[0];
            if (string.IsNullOrEmpty(selected.Text)) return;

            CodeLocationSelected(this, Language.CodeLocation.Parse(selected.Text));
        }
    }
}
