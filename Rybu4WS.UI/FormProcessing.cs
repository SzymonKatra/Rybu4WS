using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Rybu4WS.UI
{
    public partial class FormProcessing<TResult> : Form
    {
        private Func<TResult> _func;
        private CancellationTokenSource _cancellationTokenSource;
        private TResult _result;

        public FormProcessing(string message, Func<TResult> func)
        {
            InitializeComponent();
            progressBar.MarqueeAnimationSpeed = 10;

            lblMessage.Text = message;
            _func = func;
            _cancellationTokenSource = new CancellationTokenSource();
        }

        protected override async void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            var task = Task.Run(_func, _cancellationTokenSource.Token);
            _result = await task;

            this.Close();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            if (_result == null)
            {
                e.Cancel = true;
            }
            base.OnClosing(e);
        }

        private void buttCancel_Click(object sender, EventArgs e)
        {
            _cancellationTokenSource.Cancel();
        }

        public static TResult Start(Func<TResult> func, string message, Form owner)
        {
            var form = new FormProcessing<TResult>(message, func);
            form.ShowDialog(owner);
            return form._result;
        }
    }
}
