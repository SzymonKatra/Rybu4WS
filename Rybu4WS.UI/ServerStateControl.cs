using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Linq;

namespace Rybu4WS.UI
{
    public partial class ServerStateControl : UserControl
    {
        public string ServerName
        {
            get => lblServerName.Text;
            set => lblServerName.Text = value;
        }

        public ServerStateControl()
        {
            InitializeComponent();
        }

        public void UpdateVariables(IReadOnlyDictionary<string, string> variables)
        {
            foreach (var variable in variables)
            {
                var lvi = FindListViewItemForVariableOrDefault(variable.Key);
                if (lvi == null)
                {
                    lvi = new ListViewItem(new string[] { variable.Key, variable.Value });
                    listVariables.Items.Add(lvi);
                }
                lvi.SubItems[1].Text = variable.Value;
            }
        }

        private ListViewItem FindListViewItemForVariableOrDefault(string variableName)
        {
            foreach (var item in listVariables.Items.Cast<ListViewItem>())
            {
                if (item.SubItems[0].Text == variableName) return item;
            }
            return null;
        }
    }
}
