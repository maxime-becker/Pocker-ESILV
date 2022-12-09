using System.Reflection;

namespace PockerApp;

public class Logs
{
    private static readonly string? data = "Pocker";

    public Logs(string logMessage)
    {
        File.Delete(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\" + "log.txt");
        LogWrite(logMessage);
    }

    public void LogWrite(string logMessage)
    {
        var mExePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!;
        try
        {
            using var w = File.AppendText(mExePath + "\\" + "log.txt");
            Log(logMessage, w);
        }
        catch (Exception)
        {
            // ignored
        }
    }

    private void Log(string logMessage, TextWriter txtWriter)
    {
        try
        {
            txtWriter.Write("\r\nLog Entry : ");
            txtWriter.WriteLine("{0} {1}", DateTime.Now.ToLongTimeString(),
                DateTime.Now.ToLongDateString());
            txtWriter.WriteLine(data);
            txtWriter.WriteLine("  $ {0}", logMessage);
            txtWriter.WriteLine("-------------------------------");
        }
        catch (Exception)
        {
            // ignored
        }
    }
}