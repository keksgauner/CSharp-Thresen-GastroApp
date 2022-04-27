﻿using System;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace Thresen___GastroApp
{
    public class GastroApp
    {
        string db_user, db_pass, db_host, db_name; //Datenbank-Verbindungsdaten

        public void SQL_GetLogin(string user, string pass, string host, string name)
        {
            db_user = user; //Setzen der Verbindungsdaten
            db_pass = pass;
            db_host = host;
            db_name = name;
        }


        public bool SQL_Login(string user, string pass)
        {
            bool login = false; //Ergebnis der Login-Prüfung

            //MySQL Connection
            MySqlConnection conn = new MySqlConnection("Server=" + db_host + ";Database=" + db_name + ";Uid=" + db_user + ";Pwd=" + db_pass + ";"); //Verbindungsstring
            MySqlCommand cmd = new MySqlCommand("SELECT * FROM Accounts WHERE Name = '" + user + "' AND BINARY Password = '" + pass + "'", conn);   //Überprüfen der Login-Daten in der Datenbank
            try
            {
                conn.Open();    //Öffnen der Verbindung
                MySqlDataReader reader = cmd.ExecuteReader();   //Ausführen des SQL-Commands
                if (reader.HasRows) //Prüfung ob ein Datensatz gefunden wurde
                {
                    login = true;   //Login erfolgreich
                }
                else
                {
                    login = false;  //Login fehlgeschlagen
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);    //Fehlermeldung ausgeben wenn ein Fehler auftritt
            }
            finally
            {
                conn.Close();   //Schließen der Verbindung
            }
            return login;   //Ergebnis der Login-Prüfung zurückgeben
        }

        public void SQL_SaveLogin(string user, string pass, string host, string name, string username)
        {
            //Save Login to File
            string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            //create Folder if not exists
            if (!System.IO.Directory.Exists(path + "\\GastroApp"))
            {
                System.IO.Directory.CreateDirectory(path + "\\GastroApp");
            }
            //create File and write to it
            System.IO.File.WriteAllText(path + "\\GastroApp\\lastlogin.dat", "### Logindaten der Letzen Verbindung ### \n!!! DO NOT CHANGE ANYTHING !!! \n" + host + "\n" + name + "\n" + user + "\n" + pass + "\n" + username);
        }
        public string[] SQL_LoadLogin()
        {
            string[] lines;
            //Check if File exists
            string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            if (System.IO.File.Exists(path + "\\GastroApp\\lastlogin.dat"))
            {
                //Read File
                lines = System.IO.File.ReadAllLines(path + "\\GastroApp\\lastlogin.dat");
                return lines;
            }
            //set first index of lines to "empty"
            lines = new string[1];
            lines[0] = "empty";
            //return lines
            return lines;
        }
        public void exit()
        {
            //Sicherstellung der Beendung des Programmes
            DialogResult result = MessageBox.Show("Bist du sicher, dass du das Programm beenden willst?", "Beenden", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

    }
}
