// created on 24/05/2007 at 06:08 p// Sistema Hospitalario OSIRIS
// Monterrey - Mexico
//
// Autor    	: Daniel Olivares - arcangeldoc@openmailbox.org (Programacion Mono)
//				  Daniel Olivares - arcangeldoc@openmailbox.org (Diseño de Pantallas Glade)
// 				  
// Licencia		: GLP
// S.O. 		: GNU/Linux Ubuntu 6.06 LTS (Dapper Drake)
//////////////////////////////////////////////////////////
//
// proyect osiris is free software; you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation; either version 2 of the License, or
// (at your option) any later version.
// 
// proyect osirir is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with Foobar; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
// 
//////////////////////////////////////////////////////////
// Programa		: 
// Proposito	:  
// Objeto		: 
//////////////////////////////////////////////////////////	
using System;
using Npgsql;
using Gtk;
using Glade;
using System.Data;
using System.Data.SqlClient;

namespace osiris
{
	public class herramientas_del_sistemas
	{
		// Boton general para salir de las ventanas
		// Todas la ventanas en glade este boton debe estra declarado identico
		[Widget] Gtk.Button button_salir;
		
		// Declarando ventana principal de Hospitalizacion
		[Widget] Gtk.Window menu_herramientas;
		[Widget] Gtk.Button button_catalogos;
		[Widget] Gtk.Button button_cambios_tabla;
		[Widget] Gtk.Button button_actualiza1;
		[Widget] Gtk.Button button_actualiza2 = null;
		[Widget] Gtk.Button button_conexion_contpaq = null;
		
		//Declaracion de ventana de error
		protected Gtk.Window MyWinError;
		protected Gtk.Window MyWin;
		
		string LoginEmpleado;
		string NomEmpleado;
		string AppEmpleado;
		string ApmEmpleado;
		string connectionString;
		string nombrebd;
		int secuencia = 0;
		int admision = 0;
		class_conexion conexion_a_DB = new class_conexion();
		class_conexion_sqlsrv conexion_sqlsrv = new class_conexion_sqlsrv ();
		
		public herramientas_del_sistemas(string LoginEmp_, string NomEmpleado_, string AppEmpleado_, string ApmEmpleado_, string nombrebd_) 
		{
			LoginEmpleado = LoginEmp_;
			NomEmpleado = NomEmpleado_;
			AppEmpleado = AppEmpleado_;
			ApmEmpleado = ApmEmpleado_;
			connectionString = conexion_a_DB._url_servidor+conexion_a_DB._port_DB+conexion_a_DB._usuario_DB+conexion_a_DB._passwrd_user_DB;
			nombrebd = conexion_a_DB._nombrebd; 
			
			Glade.XML gxml = new Glade.XML (null, "herramientas.glade", "menu_herramientas", null);
			gxml.Autoconnect (this);        
			menu_herramientas.Show();

			button_catalogos.Clicked += new EventHandler(on_button_catalogos_clicked);
			button_actualiza1.Clicked += new EventHandler(on_button_actualiza1_clicked);
			button_actualiza2.Clicked += new EventHandler(on_button_actualiza2_clicked);
			button_cambios_tabla.Clicked += new EventHandler(on_button_cmb_tab_clicked);
			button_conexion_contpaq.Clicked += new EventHandler(on_button_conexion_contpaq_clicked);
			button_salir.Clicked += new EventHandler(on_cierraventanas_clicked);			
		}
		
		// Asignando numero de factura a los procedimientos
		void on_button_catalogos_clicked(object sender, EventArgs args)
		{
			string tipo_catalogo = verifica_acceso();
			if(tipo_catalogo == "SISTEMAS" ) {
				new osiris.catalogos_generales("menu",LoginEmpleado,NomEmpleado,AppEmpleado,ApmEmpleado,nombrebd); }
			if(tipo_catalogo == "MEDICOS" ){
				new osiris.catalogos_generales("medicos",LoginEmpleado,NomEmpleado,AppEmpleado,ApmEmpleado,nombrebd);}
			if(tipo_catalogo == "NO" ){
				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
										MessageType.Error,ButtonsType.Close,"NO esta autorizado para accesar");
				  						msgBoxError.Run ();			msgBoxError.Destroy(); }
			// cierra la ventana despues que almaceno la informacion en variables
			Widget win = (Widget) sender;
			win.Toplevel.Destroy();
		}

		void on_button_actualiza1_clicked(object sender, EventArgs args)
		{
			if(LoginEmpleado == "DOLIVARES" || LoginEmpleado == "ADMIN"){
				string numerodeatencion = "";
				string idtipopaciente = "";
				NpgsqlConnection conexion; 
				conexion = new NpgsqlConnection (connectionString+nombrebd);
				//Verifica que la base de datos este conectada
				try{
					conexion.Open ();
					NpgsqlCommand comando; 
					comando = conexion.CreateCommand ();
					comando.CommandText = "SELECT osiris_erp_cobros_enca.folio_de_servicio FROM osiris_erp_cobros_enca;";
					NpgsqlDataReader lector = comando.ExecuteReader ();
					while(lector.Read()){
						numerodeatencion = (string) lector["folio_de_servicio"].ToString().Trim();
						NpgsqlConnection conexion2; 
						conexion2 = new NpgsqlConnection (connectionString+nombrebd);
						try{
							conexion2.Open ();
							NpgsqlCommand comando2; 
							comando2 = conexion2.CreateCommand ();
							comando2.CommandText = "SELECT osiris_erp_movcargos.descripcion_diagnostico_movcargos,osiris_erp_movcargos.id_tipo_paciente FROM osiris_erp_movcargos WHERE folio_de_servicio = '"+numerodeatencion+"';";
							NpgsqlDataReader lector2 = comando2.ExecuteReader ();
							if(lector2.Read()){
								idtipopaciente = (string) lector2["id_tipo_paciente"].ToString().Trim();
								Console.WriteLine(numerodeatencion);
							}
						}catch(NpgsqlException ex){
							MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
								MessageType.Error,ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
							msgBoxError.Run ();        msgBoxError.Destroy();
						}
						conexion2.Close();

						NpgsqlConnection conexion3; 
						conexion3 = new NpgsqlConnection (connectionString+nombrebd);
						try{
							conexion3.Open ();
							NpgsqlCommand comando3; 
							comando3 = conexion3.CreateCommand ();
							comando3.CommandText = "UPDATE osiris_erp_cobros_enca " +
								"SET id_tipo_paciente = '"+idtipopaciente+"' " +
								"WHERE folio_de_servicio = '"+numerodeatencion+"';";
							comando3.ExecuteNonQuery();
							comando3.Dispose();    
						}catch(NpgsqlException ex){
							MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
								MessageType.Error,ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
							msgBoxError.Run ();        msgBoxError.Destroy();
						}
						conexion3.Close();
					}
				}catch(NpgsqlException ex){
					Console.WriteLine("PostgresSQL error: {0}",ex.Message);
					MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
						MessageType.Error,ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
					msgBoxError.Run ();        msgBoxError.Destroy();
				}
				conexion.Close();
			}else{ MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
				MessageType.Error,ButtonsType.Close,"NO esta autorizado para accesar");
				msgBoxError.Run ();			msgBoxError.Destroy(); 
			}
		}
		
		
		void on_button_actualiza2_clicked(object sender, EventArgs args)
        {
           
		}
		
		void on_button_cmb_tab_clicked(object sender, EventArgs args)
		{
			
			
		}

		void on_button_conexion_contpaq_clicked(object sender, EventArgs args)
		{
			string connectionString = conexion_sqlsrv._url_servidor + conexion_sqlsrv._nombrebd + conexion_sqlsrv._usuario_DB + conexion_sqlsrv._passwrd_user_DB; 
				
				//@"Server=192.168.1.1\SQLENU64;" +
				//"Database=NameDataBase;" +
				//"User ID=sa;" +
				//"Password=";

			SqlConnection conexion; 
			conexion = new SqlConnection (connectionString);

			// Verifica que la base de datos este conectada
			try{
				conexion.Open();
				SqlCommand comando; 
				comando = conexion.CreateCommand ();
				comando.CommandText = @"SELECT * FROM dbo.Clientes;";
				SqlDataReader lector = comando.ExecuteReader ();
				while (lector.Read()){				
					Console.WriteLine(lector["Cliente"].ToString().Trim()+" "+lector["Nombre"].ToString().Trim());
				}
			}catch (SqlException ex){
				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
					MessageType.Error,ButtonsType.Close,"SQL Server error: {0}",ex.Message);
				msgBoxError.Run ();        msgBoxError.Destroy();;	
			}
			conexion.Close ();
		}
				
		public string verifica_acceso()
		{
			string varpaso = "SISTEMAS";
			/*
			if(LoginEmpleado == "DOLIVARES" || LoginEmpleado == "ADMIN" ){
				varpaso = "SISTEMAS";
				return varpaso;
			}
			if(LoginEmpleado == "DOLIVARES" || LoginEmpleado == "ADMIN" || LoginEmpleado == "ANAMARIA"){
			  varpaso = "MEDICOS";
			  return varpaso;
			}
			if(LoginEmpleado == "DOLIVARES" || LoginEmpleado == "ADMIN" || LoginEmpleado == "SZALETAGONZALEZ"){
				varpaso = "RH"; 
				return varpaso;
			}
			*/
			return varpaso;
		}
		
		void on_cierraventanas_clicked (object sender, EventArgs args)
		{
			Widget win = (Widget) sender;
			win.Toplevel.Destroy();
		}
	}
}