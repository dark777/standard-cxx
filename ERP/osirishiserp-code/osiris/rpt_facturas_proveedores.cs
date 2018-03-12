//  
//  rpt_facturas_proveedores.cs
//  
//  Author:
//       Daniel Olivares C. arcangeldoc@gmail.com
// 
//  Copyright (c) 2013 dolivares
// 
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
// 
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
// 
//  You should have received a copy of the GNU General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.
using System;
using Gtk;
using Npgsql;
using Glade;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;

namespace osiris
{
	public class rpt_facturas_proveedores
	{
		string query_consulta = "";
		
		string connectionString;
		string nombrebd;
		
		class_public classpublic = new class_public();
		class_conexion conexion_a_DB = new class_conexion();
		
		/// <summary>
		/// Initializes a new instance of the <see cref="osiris.rpt_facturas_proveedores"/> class.
		/// </summary>
		/// <param name="tipo_reporte">
		/// Tipo_reporte.
		/// </param>
		/// <param name="query_consulta">
		/// Query_consulta.
		/// </param>
		public rpt_facturas_proveedores (string tipo_reporte,string query_consulta_)
		{
			connectionString = conexion_a_DB._url_servidor+conexion_a_DB._port_DB+conexion_a_DB._usuario_DB+conexion_a_DB._passwrd_user_DB;
			nombrebd = conexion_a_DB._nombrebd;
			query_consulta = query_consulta_;
			Console.WriteLine(tipo_reporte);			
		}		
	}
}

