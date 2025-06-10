using System;

using freezetimer.switchboard;

namespace utility
{
  public static class settings
  {
    static string _content = "";
    static string _path = @"./settings";

    public static string[] _prms; // parameters

    public static void set()
    {
      try
      {
        read(); parse();
      }
      catch (Exception e)
      {
        writer.write("settings file error\n" + e.Message, color.red);
      }
    }

    public static void set(string key, int val)
    {
      for (int i = 0; i < _prms.Length; i++)
      {
        string[] _prse = _prms[i].Split(' ');
        for (int j = 0; j < _prse.Length; j++)
        {
          if (_prse[j] == key)
          {
            _prse[j + 2] = val.ToString();

            _prms[i] = string.Join(" ", _prse);
            _prms[i] += " \n";
            break;
          }
          
        }
      }
    }

    public static void read()
    {
      _content = "";

      using (FileStream _fs = new FileStream(_path, FileMode.Open, FileAccess.Read))
      using (TextReader _sr = new StreamReader(_fs))
      {
        string line;
        while ((line = _sr.ReadLine()) != null)
        {
          _content += line + '\n';
        }

        _sr.Close();
        _fs.Close();
      }

      string _chk = _content;

    }

    public static void parse()
    {
      if (_content.Length > 0)
      {
        _prms = _content.Split('\n');

        for (int i = 0; i < _prms.Length; i++)
        {
          string[] _prse = _prms[i].Split(' ');

          if (_prse[0] == "LIBERAL" && _prse.Length > 2) { flags.LIBERAL = int.Parse(_prse[2]); }


        }
      }
    }

    public static void update()
    {
      using (FileStream _fs = new FileStream(_path, FileMode.Open, FileAccess.Write))
      using (StreamWriter _sw = new StreamWriter(_fs))
      {
        _sw.Write(string.Join('\n', _prms));

        _sw.Close();
        _fs.Close();
      }

      writer.write("file @'./settings' updated.", color.green);
      
    }
  }
}