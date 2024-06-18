using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using UnityEngine;

public class LocalFunc
{
    public static MySqlConnection conn;

    public static DataSet GetMysqlQuery(string sql)
    {
        MySqlConnectionStringBuilder builder = new MySqlConnectionStringBuilder();
        builder.UserID = "root";
        builder.Password = "P@ss1234";
        builder.Server = "101.132.190.13";
        builder.Database = "carddemo";
        conn = new MySqlConnection(builder.ConnectionString);
        conn.Open();
        MySqlCommand cmd = new MySqlCommand(sql, conn);
        MySqlDataAdapter sda = new MySqlDataAdapter(cmd);

        DataSet ds = new DataSet();
        sda.Fill(ds);
        conn.Close();
        return ds;
    }

    public static int ExecuteMysqlQuery(string sql)
    {
        MySqlConnectionStringBuilder builder = new MySqlConnectionStringBuilder();
        builder.UserID = "root";
        builder.Password = "P@ss1234";
        builder.Server = "101.132.190.13";
        builder.Database = "carddemo";
        conn = new MySqlConnection(builder.ConnectionString);
        conn.Open();
        MySqlCommand cmd = new MySqlCommand(sql, conn);
        int a = 0;
        try
        {
            a = cmd.ExecuteNonQuery();
        }
        catch (Exception e)
        {
            return 0;
        }
        finally
        {
            conn.Close();

        }
        return a;
    }


    public static string UseCmd(string cmd)
    {
        Process p;
        ProcessStartInfo psi;
        p = new Process();
        psi = new ProcessStartInfo("C:/Windows/System32/conhost.exe");
        psi.RedirectStandardInput = true;
        psi.RedirectStandardOutput = true;
        psi.RedirectStandardError = true;
        psi.UseShellExecute = false;
        psi.CreateNoWindow = true;
        p.StartInfo = psi;

        p.Start();
        p.StandardInput.WriteLine(cmd);
        p.StandardInput.AutoFlush = true;

        StreamReader reader = p.StandardOutput;
        string output = reader.ReadLine();

        while (!reader.EndOfStream)
        {
            output += "\r\n" + reader.ReadLine();
        }
        p.WaitForExit();
        p.Close();

        return output;
    }

    public static int StartExe(string path)
    {
        Process p;
        ProcessStartInfo psi;
        p = new Process();
        psi = new ProcessStartInfo(path);
        p.StartInfo = psi;

        p.Start();


        return p.Id;
    }
}
