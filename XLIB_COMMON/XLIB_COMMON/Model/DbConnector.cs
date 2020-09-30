using IMPLEA.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Odbc;
using System.Linq;
using System.Web;
using XLIB_COMMON.Interface;

namespace XLIB_COMMON.Model
{
    public class DbConnector
    {
        private OdbcConnection conn;
        private OdbcCommand odbcCmd;
        private ILogger logger;
        int timeout = 30; //[s]
        //private string dBServer;
        //private string dBDataBaseName;
        //private string dBUser;
        //private string dBPwd;
        private string connectionString;
        private string logPrefix;

        public DbConnector(ILogger logger, string connectionString)
        {
            this.logger = logger;
            //this.dBServer = dBServer;
            //this.dBDataBaseName = dBDataBaseName;
            //this.dBUser = dBUser;
            //this.dBPwd = dBPwd;
            this.connectionString = connectionString;
            this.logPrefix = ""; //logPrefix;
        }

        public OdbcConnection connect()
        {
            //string connectionString = "Driver={SQL Server};" +
            //                            "Server=" + dBServer + ";" +
            //                            "Trusted_Connection=no;" +
            //                            "Database=" + dBDataBaseName + ";" +
            //                            "Uid=" + dBUser + ";" +
            //                            "Pwd=" + dBPwd + ";";

            //string t = "Driver={SQL Server};server=dbserver;database=dbdatabasename;uid=dbuser;pwd=dbpwd;trusted_connection=no;";
            conn = new OdbcConnection(connectionString);
            conn.ConnectionTimeout = timeout;
            return conn;
        }
        public void disconnect()
        {
            conn.Close();
        }

        public DataTable runCommand(string query)
        {
            DataTable dtSqlData = new DataTable();

            connect();
            OdbcDataAdapter adapter = new OdbcDataAdapter(query, conn);

            adapter.SelectCommand.CommandTimeout = timeout;

            try
            {
                conn.Open();
                adapter.Fill(dtSqlData);
            }
            catch (Exception ex)
            {
                logger.SaveLog(ex.Message, logPrefix);
                if (ex.InnerException != null)
                {
                    logger.SaveLog(ex.InnerException.Message, logPrefix);
                }
                logger.SaveLog(query, logPrefix);
            }

            disconnect();
            return dtSqlData;
        }
        public DataTable runStoredProcedure(string SPName, params Object[] Parameters)
        {
            DataTable dtTemp = new DataTable();
            DataRow dr;
            String parameter;
            int i;

            parameter = "";
            for (i = 0; i < Parameters.Count(); i++)
            {
                if (i == 0)
                    parameter += "'" + Convert.ToString(Parameters[i]) + "'";
                else
                    parameter += ", '" + Convert.ToString(Parameters[i]) + "'";
            }

            try
            {
                connect();
                odbcCmd = new OdbcCommand("EXEC " + SPName + " " + parameter, conn);
                conn.Open();
            }
            catch (Exception ex)
            {
                logger.SaveLog(ex.Message, logPrefix);
                logger.SaveLog("EXEC " + SPName + " " + parameter, logPrefix);
                return dtTemp;
            }

            OdbcDataReader myReader = odbcCmd.ExecuteReader();

            //creating columns
            i = 0;
            while (i < myReader.FieldCount)
            {
                dtTemp.Columns.Add(myReader.GetName(i));
                i++;
            }

            //getting rows
            while (myReader.Read())
            {
                dr = dtTemp.NewRow();
                i = 0;
                while (i < myReader.FieldCount)
                {

                    if (Convert.IsDBNull(myReader.GetValue(i)) == true)
                        dr.SetField(i, 0);
                    else
                        dr.SetField(i, myReader.GetValue(i));

                    i++;
                }
                dtTemp.Rows.Add(dr);
            }

            // converting time from INT to DateTime & formating time
            disconnect();

            return dtTemp;
        }

        public DataTable runSqlQuery(string query)
        {
            DataTable dtTemp = new DataTable();
            DataRow dr;
            //String parameter;
            int i;

            try
            {
                connect();
                odbcCmd = new OdbcCommand(query, conn);
                conn.Open();
            }
            catch (Exception ex)
            {
                logger.SaveLog(ex.Message, logPrefix);
                logger.SaveLog(query, logPrefix);
                return dtTemp;
            }

            OdbcDataReader myReader = odbcCmd.ExecuteReader();

            //creating columns
            i = 0;
            while (i < myReader.FieldCount)
            {
                dtTemp.Columns.Add(myReader.GetName(i));
                i++;
            }

            //getting rows
            while (myReader.Read())
            {
                dr = dtTemp.NewRow();
                i = 0;
                while (i < myReader.FieldCount)
                {

                    if (Convert.IsDBNull(myReader.GetValue(i)) == true)
                        dr.SetField(i, 0);
                    else
                        dr.SetField(i, myReader.GetValue(i));

                    i++;
                }
                dtTemp.Rows.Add(dr);
            }

            // converting time from INT to DateTime & formating time
            disconnect();

            return dtTemp;
        }

        public int RunInsertOrUpdateCommand(string query)
        {
            int id = 0;
            try
            {
                connect();
                odbcCmd = new OdbcCommand(query + "; SELECT SCOPE_IDENTITY()", conn);
                conn.Open();
                OdbcDataReader myReader = odbcCmd.ExecuteReader();

                while (myReader.Read())
                {
                    if (!(myReader.GetValue(0) is DBNull))
                    {
                        id = Convert.ToInt32(myReader.GetValue(0));
                    }
                }
                odbcCmd.Dispose();
                disconnect();
            }
            catch (Exception ex)
            {
                logger.SaveLog(ex.Message, logPrefix);
                if (ex.InnerException != null)
                {
                    logger.SaveLog(ex.InnerException.Message, logPrefix);
                }
                logger.SaveLog(query, logPrefix);
            }
            return id;
        }
        private string flattenParameters(params Object[] Parameters)
        {
            String parameter;
            int i;

            parameter = "";
            for (i = 0; i < Parameters.Count(); i++)
            {
                if (i == 0)
                    parameter += "'" + Convert.ToString(Parameters[i]) + "'";
                else
                    parameter += ", '" + Convert.ToString(Parameters[i]) + "'";
            }

            return parameter;
        }

        //public List<class> SqlQuery(string sql, params object[] parameters)
        //{
        //}
        //public Db<TElement> SqlQuery<TElement>(string sql, params object[] parameters)
        //{
        //    return new Db<TElement>();
        //}

        public List<t2> SqlQuery2<t2>(string sql) where t2 : class
        {
            List<t2> list = DataTableHelper.DataTableColumnToList<t2>(runSqlQuery(sql));
            return list;
        }
        public List<t2> SqlQuery<t2>(string sql) where t2 : class, new()
        {
            List<t2> list = DataTableHelper.DataTableToList<t2>(runCommand(sql));
            return list;
        }
    }

}