//  
//  class_conexion.cs
//  
//  Author:
//       Daniel Olivares <arcangeldoc@openmailbox.org>
// 
//  Copyright (c) 2016 dolivares
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

namespace osiris
{
	/// <summary>
	/// Classe para la conexion de la base de datos
	/// </summary>
	class class_conexion
	{
		public string _url_servidor = "Server=localhost;";
		public string _port_DB = "Port=5432;";
		public string _usuario_DB = "User ID=admin;";
		public string _passwrd_user_DB = "Password=pwd;";
		public string _nombrebd = "Database=osiris;";
	}

	class class_conexion_sqlsrv
	{
		public string _url_servidor = @"Server=172.16.1.199\COMPAC;";
		public string _usuario_DB = "User ID=sa;";
		public string _passwrd_user_DB = "Password=ABcd12$$;";
		public string _nombrebd = "Database=Contpaq;";
	}
}