﻿using Sage.Accounting.NominalLedger;
using Sage.Accounting.SystemManager;
using Sage.MMS.SAA.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NormalClient
{
	public partial class Form1 : Form
	{
		private Sage.Accounting.Application _app = null;

		private Company _company = null;

		private void Connect()
		{
			if (_app == null)
			{
				_app = new Sage.Accounting.Application();
			}

			if (_company == null)
			{
				List<Company> companies = SAAClientAPI.CompaniesGet();
				_company = companies.First();
				SAAClientAPI.ConnectCompany(_company);
			}
		}

		public Form1()
		{
			InitializeComponent();
		}

		private void button1_Click(object sender, EventArgs e)
		{
			try
			{
				Connect();
				Go();
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}

		}

		private void Go()
		{
			Departments items = DepartmentsFactory.Factory.CreateNew();

			StringBuilder sb = new StringBuilder();

			foreach (Department item  in items)
			{
				sb.AppendLine(item.Code + ", " + item.Name);
			}

			richTextBox1.Text = sb.ToString();

		}

	}
}
