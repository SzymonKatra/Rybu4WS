using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace Rybu4WS.UI
{
    public class DedanRunner
    {
        public class DedanResult
        {
            public bool Succeed { get; set; }

            public string ErrorMessage { get; set; }

            public bool TrailExists { get; set; }

            public string InputPath { get; set; }

            public string TrailPath { get; set; }
        }

        public enum VerificationMode
        {
            Deadlock,
            Termination,
            PossibleTermination
        }

        private static readonly string DedanPath = "dedan\\DedAn.exe";
        private static readonly string DedanInputFilePath = "_dedaninput.txt";
        private static readonly string DedanOutputFilePath = "_trail.XML";

        public static DedanResult Verify(string dedanCode, VerificationMode mode)
        {
            if (File.Exists(DedanInputFilePath)) File.Delete(DedanInputFilePath);
            if (File.Exists(DedanOutputFilePath)) File.Delete(DedanOutputFilePath);

            File.WriteAllText(DedanInputFilePath, dedanCode);

            var flags = mode switch
            {
                VerificationMode.Deadlock => "-A",
                VerificationMode.Termination => "-E",
                VerificationMode.PossibleTermination => "-I",
                _ => throw new NotImplementedException()
            };

            var processStartInfo = new ProcessStartInfo(DedanPath, $"-C -MT:{DedanInputFilePath} {flags}");
            processStartInfo.RedirectStandardError = true;
            processStartInfo.RedirectStandardOutput = true;
            processStartInfo.CreateNoWindow = true;
            processStartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            var process = new Process();
            process.StartInfo = processStartInfo;
            process.Start();

            process.WaitForExit();

            string output = process.StandardOutput.ReadToEnd();
            string errorOutput = process.StandardError.ReadToEnd();

            return new DedanResult()
            {
                Succeed = string.IsNullOrWhiteSpace(errorOutput) && File.Exists(DedanOutputFilePath),
                ErrorMessage = errorOutput,
                TrailExists = File.Exists(DedanOutputFilePath),
                InputPath = DedanInputFilePath,
                TrailPath = DedanOutputFilePath
            };
        }
    }
}
