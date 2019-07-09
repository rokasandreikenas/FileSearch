using System;
using System.IO;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Diagnostics;
using System.Threading;

namespace _1uzduotis
{
    public partial class Form1 : Form
    {

        private int progress;
        private string path = "";
        private string fileOrDirName = "";
        private Stopwatch watch = new Stopwatch();

        Thread thread;
        List<string> filesAndDirectories = new List<string>();


        public Form1()
        {
            InitializeComponent();
        }


        private void button2_Click_1(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                path = folderBrowserDialog1.SelectedPath;

            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            fileOrDirName = textBox1.Text;
            
        }

        private void button1_Click(object sender, EventArgs e)
        {          
            thread = new Thread(new ParameterizedThreadStart(FindFilesAndFolders));
            thread.Start(path);

            Thread thread2 = new Thread(new ThreadStart(DisplayResultsInRealtime));
            thread2.Start();                  
        }

        void FindFilesAndFolders(object dir)
        {
                string directory = dir.ToString();
                string[] directories = Directory.GetDirectories(directory);

                foreach (string direct in directories)
                {
                try
                {
                    string[] files = Directory.GetFiles(direct);
                    foreach (string file in files)
                    {
                        if (file.Contains(fileOrDirName))
                        {

                            filesAndDirectories.Add(file);

                        }
                    }
                    FindFilesAndFolders(direct);
                }

                catch (Exception )
                {
                    continue;
                }
                   
                }
            
        }

        void DisplayResultsInRealtime()
        {
            Thread.Sleep(100);
                while(thread.ThreadState == System.Threading.ThreadState.Running)
                {
                    this.BeginInvoke((MethodInvoker)delegate
                    {
                    foreach (string file in filesAndDirectories)
                        {
                            if (!listBox1.Items.Contains(file))
                                listBox1.Items.Add(file);
                        }
                        
                    });
               
                Thread.Sleep(100);
                } 
        }
    }
}

