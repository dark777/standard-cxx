//  
//  credito_cobranza.cs
//  
//  Author:
//       Daniel Olivares <arcangeldoc@openmailbox.org>
// 
//  Copyright (c) 2014 dolivares
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
using Npgsql;
using System.Data;
using Gtk;

namespace osiris
{
	public partial class credito_cobranza : Gtk.Window
	{
		
		Gtk.TreeStore treeViewEngineGestza;
		
		string LoginEmpleado;
		string NomEmpleado;
		string AppEmpleado;
		string ApmEmpleado;
		string nombrebd;
		
		protected Gtk.Window MyWinError;
		
		class_conexion conexion_a_DB = new class_conexion();
		class_public classpublic = new class_public();
		
		public credito_cobranza (string LoginEmp_, string NomEmpleado_, string AppEmpleado_, string ApmEmpleado_, string nombrebd_ ) : 
				base(Gtk.WindowType.Toplevel)
		{
			//connectionString = conexion_a_DB._url_servidor + conexion_a_DB._port_DB + conexion_a_DB._usuario_DB + conexion_a_DB._passwrd_user_DB;
			nombrebd = conexion_a_DB._nombrebd;
			LoginEmpleado = LoginEmp_;
			NomEmpleado = NomEmpleado_;
			AppEmpleado = AppEmpleado_;
			ApmEmpleado = ApmEmpleado_;			
			this.Build ();
		}

		protected void on_cierraventanas_clicked (object sender, System.EventArgs e)
		{
			Destroy();
		}

		protected void on_gestioncobranza_clicked (object sender, System.EventArgs e)
		{
			new osiris.gestion_cobranza(LoginEmpleado,NomEmpleado,AppEmpleado,ApmEmpleado,nombrebd);
		}

		protected void on_button_reporte_abonos_clicked (object sender, System.EventArgs e)
		{
			new osiris.reporte_de_abonos(nombrebd,"corte_caja_pagares",LoginEmpleado,0,0);
		}

		protected void on_button_exportar_pagares_clicked (object sender, System.EventArgs e)
		{
			if((string) classpublic.lee_registro_de_tabla("osiris_empleado","exportar_pagares","WHERE exportar_pagares = 'true' AND login_empleado = '"+LoginEmpleado+"' ","exportar_pagares","bool") == "True"){
				new osiris.rptAdmision(nombrebd,"archivo","PAGARES");  // rpt_rep1_admision.cs
			}else{
				MessageDialog msgBox = new MessageDialog (MyWinError,DialogFlags.Modal,
									MessageType.Info,ButtonsType.Ok,"No tiene Permiso para esta Opcion");
				msgBox.Run ();msgBox.Destroy();
			}
		}

		protected void on_button_venc_pagare_clicked (object sender, System.EventArgs e)
		{
			if((string) classpublic.lee_registro_de_tabla("osiris_empleado","exportar_pagares","WHERE exportar_pagares = 'true' AND login_empleado = '"+LoginEmpleado+"' ","exportar_pagares","bool") == "True"){
				new osiris.rptAdmision(nombrebd,"archivo","PAGARES_X_VENCER");  // rpt_rep1_admision.cs
			}else{
				MessageDialog msgBox = new MessageDialog (MyWinError,DialogFlags.Modal,
									MessageType.Info,ButtonsType.Ok,"No tiene Permiso para esta Opcion");
				msgBox.Run ();msgBox.Destroy();
			}
		}

		protected void on_button_pagares_vigentes_clicked (object sender, System.EventArgs e)
		{
			if((string) classpublic.lee_registro_de_tabla("osiris_empleado","exportar_pagares","WHERE exportar_pagares = 'true' AND login_empleado = '"+LoginEmpleado+"' ","exportar_pagares","bool") == "True"){
				new osiris.rptAdmision(nombrebd,"archivo","PAGARES_VIGENTE");  // rpt_rep1_admision.cs
			}else{
				MessageDialog msgBox = new MessageDialog (MyWinError,DialogFlags.Modal,
									MessageType.Info,ButtonsType.Ok,"No tiene Permiso para esta Opcion");
				msgBox.Run ();msgBox.Destroy();
			}
		}

		protected void on_button_pagares_liquidados_clicked (object sender, System.EventArgs e)
		{
			new osiris.reporte_de_abonos(nombrebd,"pagares_liquidados",LoginEmpleado,0,0);
		}
	}
}