using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;
using System.Text;
using System;
using System.Diagnostics;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Management;
using System.Management.Instrumentation;
using System.Windows.Input;
using System.Web;

class Solution
{
    public static int numberOfProcesses;
    public static int numberOfFoundedProcesses;
    public static int numberOfKilledProcesses;
    public static void Main(string[] args)
    {
        Console.Title = "Force Quit Program";
        Start();
    }
    
    public static void Question()
    {
        Console.WindowHeight = 25;
        Console.WindowWidth = 100;

        Console.WriteLine("type integer..." + "\n");
        int input = Convert.ToInt32(Console.ReadLine());

        for (int i = 0; i < input; i++)
        {

        }

        Console.WriteLine(CheckIsPrime(input));

        Console.ReadKey();
    }

    public static bool CheckIsPrime(int input)
    {
        bool prime = true;
        bool doneOnce = false;

        for (int i = 2; i < input; i++)
        {
            if (input % i == 0 && !doneOnce)
            {
                prime = false;
                doneOnce = true;
            }
        }

        if (prime) return true;
        else return false;
    }

    public static void SearchLyricsOnWeb(string search)
    {
        Process.Start("http://google.com/search?q=" + search);

    }


    public static void ListProcesses()
    {
        Process[] RunningProcess = Process.GetProcesses();
        List<string> ProcessNames = new List<string>();
        foreach (Process EachProcess in RunningProcess)
        {
            ProcessNames.Add(EachProcess.ProcessName);
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(EachProcess.ProcessName);
            numberOfProcesses++;
        }
    }

    public static bool AreStringsSame(string s1, string s2)
    {
        string capTestStr = s1.ToUpper();
        if (capTestStr.Contains(s2.ToUpper()))
            return true;

        return false;
    }

    public static void KillService(string serviceName)
    {
        Process[] RunningProcess = Process.GetProcesses();
        bool killService = false;
        string processName = "";

        foreach (Process EachProcess in RunningProcess)
        {
            if (AreStringsSame(EachProcess.ProcessName, serviceName))
            {
                processName = EachProcess.ProcessName;
                numberOfFoundedProcesses++;
            }
            if (AreStringsSame(EachProcess.ProcessName, serviceName) && !killService)
            {
                switch (MessageBox.Show("Are you sure you want to close the process " + "\n" + "\n" + "'" + EachProcess.ProcessName + "'" + "\n" + "\n" + "If the console has sufficient permissions, it will force close the process and any unsaved data about the process will be lost. ", "Information", MessageBoxButtons.OKCancel, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly))
                {
                    case DialogResult.OK:
                        killService = true;
                        break;
                    case DialogResult.Cancel:
                        Console.Clear();
                        Start();
                        break;
                }
            }
            if(AreStringsSame(EachProcess.ProcessName, serviceName) && killService)
            {
                EachProcess.Kill();
                numberOfKilledProcesses++;
            }
        }
        if (killService && numberOfKilledProcesses > 0)
        {
            switch (MessageBox.Show(numberOfFoundedProcesses + " Out Of " + numberOfKilledProcesses + " process named " + processName + " is force quited succesfully!", "Done!", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly))
            {
                case DialogResult.OK:
                    Environment.Exit(0);
                    break;
            }
        }
        if(!killService) switch (MessageBox.Show("Process named '" + serviceName + "' could not be found in running processes!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly))
        {
            case DialogResult.OK:
                Console.Clear();
                Start();
                break;
        }
    }



    public static string ProductName
    {
        get
        {
            AssemblyProductAttribute myProduct = (AssemblyProductAttribute)AssemblyProductAttribute.GetCustomAttribute(Assembly.GetExecutingAssembly(), typeof(AssemblyProductAttribute));
            return myProduct.Product;
        }
    }
    
    [DllImport("user32.dll")]
    public static extern IntPtr GetForegroundWindow();

    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

    [DllImport("user32.dll")]
    static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);

    /*public static string GetActiveWindowTitle()
    {
        var handle = GetForegroundWindow();
        string fileName = "";
        string name = "";
        uint pid = 0;
        GetWindowThreadProcessId(handle, out pid);

        Process p = Process.GetProcessById((int)pid);
        var processname = p.ProcessName;

        switch (processname)
        {
            case "explorer": //metro processes
            case "WWAHost":
                name = GetTitle(handle);
                return name;
            default:
                break;
        }
        string wmiQuery = string.Format("SELECT ProcessId, ExecutablePath FROM Win32_Process WHERE ProcessId LIKE '{0}'", pid.ToString());
        var pro = new ManagementObjectSearcher(wmiQuery).Get().Cast<ManagementObject>().FirstOrDefault();
        fileName = (string)pro["ExecutablePath"];
        // Get the file version
        FileVersionInfo myFileVersionInfo = FileVersionInfo.GetVersionInfo(fileName != null ? fileName : "");
        // Get the file description
        name = myFileVersionInfo.FileDescription;
        if (name == "")
            name = GetTitle(handle);

        return name;
    }*/

    private static string GetActiveWindowTitleSpotify()
    {
        const int nChars = 256;
        StringBuilder Buff = new StringBuilder(nChars);
        IntPtr handle = GetForegroundWindow();

        if (GetWindowText(handle, Buff, nChars) > 0)
        {
            return Buff.ToString();
        }
        return null;
    }

    public static void Start()
    {
        /*
        //To get Product name - not necessarily the same as the executeable
        string appName = Application.ProductName;
        MessageBox.Show(appName);
        //To find the name of the executable
        appName = Path.GetFileName(Application.ExecutablePath);
        MessageBox.Show(appName);
        Console.WriteLine(ProductName);
        
        int limit = 100;
        int i = 0;
        while(i<limit)
        {
            limit++;
        }*/

        numberOfFoundedProcesses = 0;
        numberOfKilledProcesses = 0;
        numberOfProcesses = 0;

        ListProcesses();

        Console.ResetColor();

        Console.WriteLine("\n" + "Please write one of those " + numberOfProcesses + " services to force quit: " + "\n");

        string processName = Console.ReadLine();

        if (processName != null)
        {
            if (processName == "refresh")
            {
                Console.Clear();
                Start();
            }
            if (processName == "quit") Environment.Exit(0);
        }

        if (processName != null) KillService(processName);
        else
        {
            switch (MessageBox.Show("Please Enter A Process.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly))
            {
                case DialogResult.OK:
                    Console.Clear();
                    Start();
                    break;
            }
        }
        Console.ReadKey();
        return;
    }
}
