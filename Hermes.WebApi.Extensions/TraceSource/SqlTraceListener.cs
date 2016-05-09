using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;

namespace Hermes.WebApi.Extensions.TraceSource
{
	public class SqlTraceListener : HermesTraceListener
	{
		protected readonly SqlConnection Connection;

		public SqlTraceListener(string initializeData)
		{
			var connectionStringSetting = ConfigurationManager.ConnectionStrings[initializeData];

			var connectionString = connectionStringSetting != null
				? connectionStringSetting.ConnectionString
				: initializeData;

			Connection = new SqlConnection(connectionString);
		}

		public override void Write(object o)
		{
			LogEntry entry = o as LogEntry;
			if (entry == null)
				return;
			using (SqlCommand command = SqlTraceListener.CreateCommand(entry))
			{
				command.Connection = this.Connection;
				lock (this.Connection)
				{
					try
					{
						this.Connection.Open();
						command.ExecuteScalar();
					}
					finally
					{
						this.Connection.Close();
					}
				}
			}
		}

		private static SqlCommand CreateCommand(LogEntry entry)
		{
			SqlCommand sqlCommand1 = new SqlCommand();
			sqlCommand1.CommandType = CommandType.StoredProcedure;
			sqlCommand1.CommandText = "dbo.spWriteLog";
			SqlCommand sqlCommand2 = sqlCommand1;
			object obj1 = (object)DBNull.Value;
			object obj2 = (object)DBNull.Value;
			object obj3 = (object)DBNull.Value;
			if (entry.Exception != null)
			{
				string str = entry.Exception.ToString();
				obj1 = str.Length > 4000 ? (object)str.Substring(0, 4000) : (object)str;
				obj2 = string.IsNullOrEmpty(entry.Exception.Source) ? (object)"InnerException" : (object)entry.Exception.Source;
				obj3 = string.IsNullOrEmpty(entry.Exception.StackTrace) ? (object)string.Empty : (object)entry.Exception.StackTrace;
			}
			SqlParameter[] sqlParameterArray1 = new SqlParameter[10];
			SqlParameter[] sqlParameterArray2 = sqlParameterArray1;
			int index1 = 0;
			SqlParameter sqlParameter1 = new SqlParameter();
			sqlParameter1.ParameterName = "@SourceApplication";
			sqlParameter1.SqlDbType = SqlDbType.VarChar;
			sqlParameter1.Size = 100;
			sqlParameter1.Value = (object)entry.AssemblyFullName;
			SqlParameter sqlParameter2 = sqlParameter1;
			sqlParameterArray2[index1] = sqlParameter2;
			SqlParameter[] sqlParameterArray3 = sqlParameterArray1;
			int index2 = 1;
			SqlParameter sqlParameter3 = new SqlParameter();
			sqlParameter3.ParameterName = "@SourceClass";
			sqlParameter3.SqlDbType = SqlDbType.VarChar;
			sqlParameter3.Size = 200;
			sqlParameter3.Value = obj2;
			SqlParameter sqlParameter4 = sqlParameter3;
			sqlParameterArray3[index2] = sqlParameter4;
			SqlParameter[] sqlParameterArray4 = sqlParameterArray1;
			int index3 = 2;
			SqlParameter sqlParameter5 = new SqlParameter();
			sqlParameter5.ParameterName = "@LogTime";
			sqlParameter5.SqlDbType = SqlDbType.DateTime;
			sqlParameter5.Value = (object)DateTime.Now;
			SqlParameter sqlParameter6 = sqlParameter5;
			sqlParameterArray4[index3] = sqlParameter6;
			SqlParameter[] sqlParameterArray5 = sqlParameterArray1;
			int index4 = 3;
			SqlParameter sqlParameter7 = new SqlParameter();
			sqlParameter7.ParameterName = "@Severity";
			sqlParameter7.SqlDbType = SqlDbType.TinyInt;
			sqlParameter7.Value = (object)(byte)SqlTraceListener.GetSeverity(entry.Severity);
			SqlParameter sqlParameter8 = sqlParameter7;
			sqlParameterArray5[index4] = sqlParameter8;
			SqlParameter[] sqlParameterArray6 = sqlParameterArray1;
			int index5 = 4;
			SqlParameter sqlParameter9 = new SqlParameter();
			sqlParameter9.ParameterName = "@AdditionalMessage";
			sqlParameter9.SqlDbType = SqlDbType.Text;
			sqlParameter9.Value = (object)entry.Message;
			SqlParameter sqlParameter10 = sqlParameter9;
			sqlParameterArray6[index5] = sqlParameter10;
			SqlParameter[] sqlParameterArray7 = sqlParameterArray1;
			int index6 = 5;
			SqlParameter sqlParameter11 = new SqlParameter();
			sqlParameter11.ParameterName = "@ExceptionMessage";
			sqlParameter11.SqlDbType = SqlDbType.VarChar;
			sqlParameter11.Size = 4000;
			sqlParameter11.Value = obj1;
			SqlParameter sqlParameter12 = sqlParameter11;
			sqlParameterArray7[index6] = sqlParameter12;
			SqlParameter[] sqlParameterArray8 = sqlParameterArray1;
			int index7 = 6;
			SqlParameter sqlParameter13 = new SqlParameter();
			sqlParameter13.ParameterName = "@StackTrace";
			sqlParameter13.SqlDbType = SqlDbType.Text;
			sqlParameter13.Value = obj3;
			SqlParameter sqlParameter14 = sqlParameter13;
			sqlParameterArray8[index7] = sqlParameter14;
			SqlParameter[] sqlParameterArray9 = sqlParameterArray1;
			int index8 = 7;
			SqlParameter sqlParameter15 = new SqlParameter();
			sqlParameter15.ParameterName = "@CategoryNames";
			sqlParameter15.SqlDbType = SqlDbType.Text;
			sqlParameter15.Value = entry.Categories != null ? (object)string.Join("|", entry.Categories) : (object)DBNull.Value;
			SqlParameter sqlParameter16 = sqlParameter15;
			sqlParameterArray9[index8] = sqlParameter16;
			SqlParameter[] sqlParameterArray10 = sqlParameterArray1;
			int index9 = 8;
			SqlParameter sqlParameter17 = new SqlParameter();
			sqlParameter17.ParameterName = "@TraceLevel";
			sqlParameter17.SqlDbType = SqlDbType.TinyInt;
			sqlParameter17.Value = (object)(byte)entry.Severity;
			SqlParameter sqlParameter18 = sqlParameter17;
			sqlParameterArray10[index9] = sqlParameter18;
			SqlParameter[] sqlParameterArray11 = sqlParameterArray1;
			int index10 = 9;
			SqlParameter sqlParameter19 = new SqlParameter();
			sqlParameter19.ParameterName = "@MachineName";
			sqlParameter19.SqlDbType = SqlDbType.NVarChar;
			sqlParameter19.Size = 32;
			sqlParameter19.Value = (object)entry.MachineName;
			SqlParameter sqlParameter20 = sqlParameter19;
			sqlParameterArray11[index10] = sqlParameter20;
			SqlParameter[] values = sqlParameterArray1;
			sqlCommand2.Parameters.AddRange(values);
			return sqlCommand2;
		}

		private static SqlTraceListener.MessageType GetSeverity(TraceEventType traceLevel)
		{
			SqlTraceListener.MessageType messageType;
			switch (traceLevel)
			{
				case TraceEventType.Critical:
					messageType = SqlTraceListener.MessageType.Failure;
					break;

				case TraceEventType.Error:
					messageType = SqlTraceListener.MessageType.Error;
					break;

				case TraceEventType.Warning:
					messageType = SqlTraceListener.MessageType.Warning;
					break;

				case TraceEventType.Information:
					messageType = SqlTraceListener.MessageType.Informational;
					break;

				default:
					messageType = SqlTraceListener.MessageType.Verbose;
					break;
			}
			return messageType;
		}

		private enum MessageType
		{
			Informational = 1,
			Failure = 2,
			Warning = 3,
			Error = 4,
			Verbose = 5,
		}
	}
}