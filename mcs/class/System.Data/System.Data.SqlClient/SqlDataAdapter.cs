//
// System.Data.SqlClient.SqlDataAdapter.cs
//
// Author:
//   Rodrigo Moya (rodrigo@ximian.com)
//   Daniel Morgan (danmorg@sc.rr.com)
//   Tim Coleman (tim@timcoleman.com)
//
// (C) Ximian, Inc 2002
// Copyright (C) 2002 Tim Coleman
//

using System;
using System.ComponentModel;
using System.Data;
using System.Data.Common;

namespace System.Data.SqlClient
{
	/// <summary>
	/// Represents a set of command-related properties that are used 
	/// to fill the DataSet and update a data source, all this 
	/// from a SQL database.
	/// </summary>
	public sealed class SqlDataAdapter : DbDataAdapter 
	{
		#region Constructors
		
		public SqlDataAdapter () 	
			: this (new SqlCommand ())
		{
		}

		public SqlDataAdapter (SqlCommand selectCommand) 
		{
			this.deleteCommand = new SqlCommand ();
			this.insertCommand = new SqlCommand ();
			this.selectCommand = selectCommand;
			this.updateCommand = new SqlCommand ();
			this.isDirty = true;
		}

		public SqlDataAdapter (string selectCommandText, SqlConnection selectConnection) 
			: this (new SqlCommand (selectCommandText, selectConnection))
		{ 
		}

		public SqlDataAdapter (string selectCommandText, string selectConnectionString)
			: this (selectCommandText, new SqlConnection (selectConnectionString))
		{
		}

		#endregion

		#region Properties

		public new SqlCommand DeleteCommand {
			get { return (SqlCommand)deleteCommand; }
			set { deleteCommand = value; }
		}

		public new SqlCommand InsertCommand {
			get { return (SqlCommand)insertCommand; }
			set { insertCommand = value; }
		}

		public new SqlCommand SelectCommand {
			get { return (SqlCommand)selectCommand; }
			set { 
				this.isDirty = true;
				selectCommand = value; 
			}
		}

		public new SqlCommand UpdateCommand {
			get { return (SqlCommand)updateCommand; }
			set { updateCommand = value; }
		}

		#endregion // Properties

		#region Methods

		[MonoTODO]
		protected override RowUpdatedEventArgs CreateRowUpdatedEvent (DataRow dataRow, IDbCommand command, StatementType statementType, DataTableMapping tableMapping) 
		{
			throw new NotImplementedException ();
		}


		[MonoTODO]
		protected override RowUpdatingEventArgs CreateRowUpdatingEvent (DataRow dataRow, IDbCommand command, StatementType statementType, DataTableMapping tableMapping) 
		{
			throw new NotImplementedException ();
		}

		protected override void OnRowUpdated (RowUpdatedEventArgs value) 
		{
			throw new NotImplementedException ();
		}

		protected override void OnRowUpdating (RowUpdatingEventArgs value) 
		{
			throw new NotImplementedException ();
		}

		#endregion // Methods

		#region Events and Delegates

		public event SqlRowUpdatedEventHandler RowUpdated;
		public event SqlRowUpdatingEventHandler RowUpdating;

		#endregion // Events and Delegates

	}
}
