using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Linq;
using System.Text.RegularExpressions;

namespace Rybu4WS.UI
{
    public partial class FormFileErrors : Form
    {
        public event EventHandler<(int line, int column)> HighlightError;

        public FormFileErrors(string filePath, IEnumerable<string> errors)
        {
            InitializeComponent();

            lblFilePath.Text = filePath;
            listBoxErrors.Items.AddRange(errors.Cast<object>().ToArray());
        }

        private void buttClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private (int line, int column)? TryParseLocation(string input)
        {
            var regex = new Regex("L: (?<line>[0-9]+) C: (?<column>[0-9]+)");
            var result = regex.Match(input);
            if (!result.Success) return null;

            return (int.Parse(result.Groups["line"].Value), int.Parse(result.Groups["column"].Value));
        }

        private void listBoxErrors_DoubleClick(object sender, EventArgs e)
        {
            var selection = (string)listBoxErrors.SelectedItem;
            if (selection == null) return;

            var location = TryParseLocation(selection);
            if (location != null)
            {
                HighlightError?.Invoke(this, (location.Value.line, location.Value.column));
            }
        }
    }
}
