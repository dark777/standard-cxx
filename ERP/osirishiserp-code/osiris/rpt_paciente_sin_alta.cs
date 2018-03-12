// created on 25/01/2008 at 11:20 a
//////////////////////////////////////////////////////////////////////
// created on 21/01/2008 at 08:28 p
// Sistema Hospitalario OSIRIS
// Monterrey - Mexico
//
// Autor    	: Ing. Juan Antonio Peña Gonzalez (Programacion) gjuanzz@gmail.com
// 				  Ing. Daniel Olivares C. (Modificaciones y Ajustes) arcangeldoc@openmailbox.org 
//					    Cambio de reporte a GTKPrint 06/09/2010
//				  Jesus Buentello (Ajustes)
//				  Ing. Daniel Olivares C. Ajuste varios Jul 2016 integracion con itextSharp
// 				  
// Licencia		: GLP
//////////////////////////////////////////////////////////
//
// proyect osiris is free software; you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation; either version 2 of the License, or
// (at your option) any later version.
// 
// proyect osiris is distributed in the hope that it will be useful,
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
using Gtk;
using Npgsql;
using System.Data;
using Glade;
using System.Collections;

namespace osiris
{
	//reporte_pacientes_sin_alta
	public class reporte_pacientes_sin_alta
	{
		//declarando la ventana de rango de fechas
		[Widget] Gtk.Window rpt_ocupacion;
		[Widget] Gtk.CheckButton checkbutton_impr_todo_proce;
		[Widget] Gtk.CheckButton checkbutton_agregar_monto;
		[Widget] Gtk.Button button_imprime_rangofecha;
		[Widget] Gtk.Button button_actualiza_ocupacion = null;
		[Widget] Gtk.Button button_export_sheet = null;
		[Widget] Gtk.Button button_consultar = null;
		[Widget] Gtk.Button button_salir;
		[Widget] Gtk.TreeView lista_ocupacion;
		[Widget] Gtk.Entry entry_totalsaldos;
		[Widget] Gtk.Entry entry_totalabonos;
		[Widget] Gtk.Entry entry_total_de_pacientes;
		[Widget] Gtk.Entry entry_fecha_inicio = null;
		[Widget] Gtk.Entry entry_fecha_termino = null;
		
		string connectionString;
        string nombrebd;
		
		string idcuarto = "";
		decimal saldos = 0;
		decimal totabono = 0;
		decimal totcuenta = 0;
		decimal sumacuenta = 0;
		decimal abono = 0;
		decimal abonomuestra = 0;
		string tipo_reporte;
		
		private ListStore treeViewEngineocupacion;
		
		ArrayList columns = new ArrayList ();

		//Declaracion de ventana de error
		protected Gtk.Window MyWinError;
		protected Gtk.Window MyWin;
		
		class_conexion conexion_a_DB = new class_conexion();
		class_public classpublic = new class_public();
		
		public reporte_pacientes_sin_alta(string _nombrebd_,string tipo_reporte_)
		{
			connectionString = conexion_a_DB._url_servidor+conexion_a_DB._port_DB+conexion_a_DB._usuario_DB+conexion_a_DB._passwrd_user_DB;
			nombrebd = conexion_a_DB._nombrebd;
			tipo_reporte = tipo_reporte_;
			Glade.XML  gxml = new Glade.XML  (null, "registro_admision.glade", "rpt_ocupacion", null);
			gxml.Autoconnect  (this);	
			rpt_ocupacion.Show();
			checkbutton_impr_todo_proce.Label = "Agrega Abonos";
			entry_fecha_inicio.Text = DateTime.Now.ToString("yyyy-MM-dd");
			entry_fecha_termino.Text = DateTime.Now.ToString("yyyy-MM-dd");
			button_imprime_rangofecha.Clicked += new EventHandler(imprime_reporte);
			button_export_sheet.Clicked += new EventHandler(on_button_export_sheet_clicked);
			button_salir.Clicked += new EventHandler(on_cierraventanas_clicked);

			if (tipo_reporte_ == "ADMISIONES") {
				button_actualiza_ocupacion.Clicked += new EventHandler(on_button_actualiza_admision_clicked);
				button_consultar.Clicked += new EventHandler(on_button_actualiza_admision_clicked);
				consulta_admisiones ();
			}
			if (tipo_reporte_ == "OCUPACION") {
				checkbutton_agregar_monto.Clicked += new EventHandler(on_checkbutton_agregar_monto_clicked);
				checkbutton_impr_todo_proce.Clicked += new EventHandler(on_checkbutton_agregar_monto_clicked);
				button_actualiza_ocupacion.Clicked += new EventHandler(on_button_actualiza_ocupacion_clicked);
				consulta_ocupacion ();
			}
		}

		void consulta_admisiones()
		{
			object[] parametros = { lista_ocupacion, treeViewEngineocupacion };
			string[,] coltreeview = {
				{ "Fecha Hora", "text" },
				{ "Nro. Atencion", "text" },
				{ "Nro. Nomina", "text" },
				{ "Nombre Paciente", "text" },
				{ "Edad", "text" },
				{ "Admitido a", "text" },
				{ "Tipo Paciente", "text" },
				{ "Moptivo de Ingreso", "text" },
				{ "Nombre Medico", "text" },
				{ "Admitido Por", "text" },
				{ "Expediente ", "text" },
				{ "Empresa o Aseguradora", "text" },
				{ "Subjetivo", "text" },
				{ "Objetivo", "text" },
				{ "Analisis", "text" },
				{ "Plan", "text" },
			};
			crea_colums_treeview (parametros, coltreeview,"ADMISIONES");
		}
		
		void consulta_ocupacion()
		{
			object[] parametros = { lista_ocupacion, treeViewEngineocupacion};
			string[,] coltreeview = { 
				{ "Nombre Paciente", "text" },
				{ "Nro. Atencion", "text" },
				{ "Nro. Nomina", "text" },
				{ "Fecha Ingreso", "text" },
				{ "Saldo", "text" },
				{ "Abonos", "text" },
				{ "a Pagar", "text" },
				{ "Medico Tratante", "text" },
				{ "Habitacion", "text" },
				{ "Diagnostico", "text" },
				{ "Tipo Paciente", "text" },
				{ "Empresa o Aseguradora", "text" }
			};
			crea_colums_treeview (parametros, coltreeview,"OCUPACION");
		}

		void on_button_export_sheet_clicked(object sender, EventArgs args)
		{
			string[,] args_formulas;
			string[,] args_width;
			string[,] args_name_type_active;
			string[] args_field_text;
			string[] args_more_title;

			if (tipo_reporte == "OCUPACION") {
				args_name_type_active = new string[,] {
					{"Nombre Paciente","string","active"},
					{"Nro. Atencion","float","active"},
					{"Nro. Nomina","string","active"},
					{"Fecha Ingreso","string","active"},
					{"Saldo","float","active"},
					{"Abonos","float","active"},
					{"a Pagar","float","active"},
					{"Medico Tratante","string","active"},
					{"Habitacion","string","active"},
					{"Diagnostico","string","active"},
					{"Tipo Paciente","string","active"},
					{"Empresa o Aseguradora","string","active"}
				};
				args_field_text = new string[] {"string","string","string","string","string","string","string","string","string","string","string","string"};
				args_more_title = new string[] {""};
				args_formulas = new string[,] {{"",""}};
				args_width = new string[,] {{"0","8cm"},{"7","9cm"},{"9","8cm"},{"11","8cm"}};
				new osiris.class_traslate_ods_array (lista_ocupacion,treeViewEngineocupacion,args_name_type_active,false,args_field_text,"",false,args_more_title,args_formulas,args_width,"REPORTE DE OCUPACION","RAGO DE FECHA  DESDE :"+entry_fecha_inicio.Text+"  HASTA:"+entry_fecha_termino.Text);
			}
			if (tipo_reporte == "ADMISIONES") {
				args_name_type_active = new string[,] {
					{"Fecha Hora","string","active"},
					{"Nro. Atencion","float","active"},
					{"Nro. Nomina","float","active"},
					{"Nombre Paciente","string","active"},
					{"Edad","string","active"},
					{"Admitido a","string","active"},
					{"Tipo Paciente","string","active"},
					{"Moptivo de Ingreso","string","active"},
					{"Nombre Medico","string","active"},
					{"Admitido Por","string","active"},
					{"Expediente","float","active"},
					{"Empresa o Aseguradora","string","active"},
					{"Subjetivo","string","active"},
					{"Objetivo","string","active"},
					{"Analisis","string","active"},
					{"Plan","string","active"}
				};
				args_field_text = new string[] {"string","string","string","string","string","string","string","string","string","string","string","string","string","string","string","string"};
				args_more_title = new string[] {""};
				args_formulas = new string[,] {{"",""}};
				args_width = new string[,] {{"3","8cm"},{"7","9cm"},{"8","8cm"},{"11","8cm"},{"12","10cm"},{"13","10cm"},{"14","10cm"},{"15","10cm"}};
				new osiris.class_traslate_ods_array (lista_ocupacion,treeViewEngineocupacion,args_name_type_active,false,args_field_text,"",false,args_more_title,args_formulas,args_width,"REPORTE DE ADMISIONES","RAGO DE FECHA  DESDE :"+entry_fecha_inicio.Text+"  HASTA:"+entry_fecha_termino.Text);
			}
		}

		void on_checkbutton_agregar_monto_clicked(object sender, EventArgs args)
		{
			consulta_ocupacion ();
		}
		
		void on_button_actualiza_ocupacion_clicked(object sender, EventArgs args)
		{
			consulta_ocupacion ();
		}

		void on_button_actualiza_admision_clicked(object sender, EventArgs args)
		{
			consulta_admisiones ();
		}
				
		void imprime_reporte(object sender, EventArgs args)
		{
			new osiris.rpt_ocupacion_hospitalaria(treeViewEngineocupacion,sumacuenta,totabono);
		}
		
				
		// Valida entradas que solo sean numericas, se utiliza eb ventana de
		//de rangos de fechas
		[GLib.ConnectBefore ()]   	  // Esto es indispensable para que funcione    
		public void onKeyPressEvent(object o, Gtk.KeyPressEventArgs args)
		{
			string misDigitos = ".0123456789ﾰﾱﾲﾳﾴﾵﾶﾷﾸﾹﾮ）（ｔｒｓｑ ";
			if (Array.IndexOf(misDigitos.ToCharArray(), Convert.ToChar(args.Event.Key)) == -1 && args.Event.Key.ToString()  != "BackSpace"){
				args.RetVal = true;
			}
		}
		
		void on_cierraventanas_clicked (object sender, EventArgs args)
		{
			Widget win = (Widget) sender;
			win.Toplevel.Destroy();
		}


		void crea_colums_treeview(object[] args,string [,] args_colums,string tipo_reporte_)
		{
			Gtk.CellRendererText text;
			Gtk.CellRendererToggle toggle;
			Gtk.TreeViewColumn column0;
			// crea los objetos para el uso del treeview
			foreach (TreeViewColumn tvc in lista_ocupacion.Columns)
				lista_ocupacion.RemoveColumn(tvc);
			treeViewEngineocupacion = new ListStore(typeof(string),typeof(string),typeof(string),
				typeof(string),typeof(string),typeof(string),typeof(string),typeof(string),typeof(string),
				typeof(string),typeof(string),typeof(string),typeof(string),typeof(string),typeof(string),
				typeof(string),typeof(string),typeof(string),typeof(string),typeof(string),typeof(string),
				typeof(bool),typeof(bool));
			lista_ocupacion.Model = treeViewEngineocupacion;
			lista_ocupacion.RulesHint = true;
			lista_ocupacion.Selection.Mode = SelectionMode.Multiple;
			if(args_colums.GetUpperBound(0) >= 0){
				for (int colum_field = 0; colum_field <= args_colums.GetUpperBound (0); colum_field++) {
					if((string) args_colums [colum_field, 1] == "text"){
						// column for holiday names
						text = new CellRendererText ();
						text.Xalign = 0.0f;
						columns.Add (text);
						column0 = new TreeViewColumn((string) args_colums [colum_field, 0], text,"text", colum_field);
						column0.Resizable = true;
						//column0.SortColumnId = colum_field;
						lista_ocupacion.InsertColumn (column0, colum_field);					
					}
					if((string) args_colums [colum_field, 1] == "toogle"){
						toggle = new CellRendererToggle ();
						toggle.Xalign = 0.0f;
						columns.Add (toggle);
						column0 = new TreeViewColumn ((string) args_colums [colum_field, 0], toggle,"active",colum_field);
						column0.Sizing = TreeViewColumnSizing.Fixed;
						column0.Clickable = true;
						lista_ocupacion.InsertColumn (column0, colum_field);

					}
				}
			}
			if (tipo_reporte_ == "ADMISIONES") {
				llenando_lista_de_admisiones ();
			}
			if (tipo_reporte_ == "OCUPACION") {
				llenando_lista_de_ocupacion ();
			}
		}

		void llenando_lista_de_admisiones()
		{
			string descri_empresa_aseguradora = "";
			int totaldepaciente = 0;

			string s_subjetivo_px = "";
			string o_objetivo_px = "";
			string a_analisis_px = "";
			string p_plan_px = "";
			string motivo_de_ingreso = "";
			string query_rango_fechas = "AND to_char(osiris_erp_cobros_enca.fechahora_creacion,'yyyy-MM-dd') >= '"+entry_fecha_inicio.Text.ToString()+"' "+
				"AND to_char(osiris_erp_cobros_enca.fechahora_creacion,'yyyy-MM-dd') <= '"+entry_fecha_termino.Text.ToString()+"' ";

			NpgsqlConnection conexion; 
			conexion = new NpgsqlConnection (connectionString+nombrebd);
			// Verifica que la base de datos este conectada
			try{
				conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand ();
				comando.CommandText ="SELECT osiris_erp_cobros_enca.folio_de_servicio,"+							
					"to_char(osiris_erp_cobros_enca.folio_de_servicio,'9999999999') AS foliodeatencion,id_empleado_admision,"+
					"to_char(osiris_erp_cobros_enca.pid_paciente,'9999999999') AS pidpaciente,osiris_his_paciente.nomina_paciente,"+
					"nombre1_paciente || ' ' || nombre2_paciente || ' ' || apellido_paterno_paciente || ' ' || apellido_materno_paciente AS nombre_completo, "+
					"to_char(to_number(to_char(age('2008-01-26 13:30:39',osiris_his_paciente.fecha_nacimiento_paciente),'yyyy') ,'9999'),'9999') AS edad, "+
					"to_char(to_number(to_char(age('2008-01-26 01:30:39',osiris_his_paciente.fecha_nacimiento_paciente),'MM'),'99'),'99') AS mesesedad, "+
					"osiris_erp_cobros_enca.nombre_medico_encabezado, "+
					"osiris_erp_cobros_enca.id_medico,nombre_medico, "+
					"osiris_erp_cobros_enca.id_medico_tratante,nombre_medico,"+
					"osiris_erp_cobros_enca.nombre_medico_tratante,"+
					"osiris_erp_cobros_enca.id_aseguradora,descripcion_aseguradora,"+
					"osiris_erp_cobros_enca.id_empresa,descripcion_empresa,"+
					"to_char(osiris_erp_cobros_enca.id_cuarto,'999999999') AS cuarto, "+
					"to_char(osiris_erp_cobros_enca.total_abonos,'99999999.99') AS totabonos, "+
					"to_char(osiris_erp_cobros_enca.fechahora_creacion,'dd-MM-yyyy HH24:mi') AS fecha_ingreso, "+
					"to_char(osiris_erp_cobros_enca.total_procedimiento,'99999999.99') AS totalproc,"+
					"osiris_erp_cobros_enca.id_habitacion,to_char(osiris_his_habitaciones.numero_cuarto,'999999999') AS numerocuarto,"+
					"osiris_his_habitaciones.descripcion_cuarto,osiris_his_habitaciones.id_tipo_admisiones AS idtipoadmisiones_habitacion,"+
					"osiris_his_habitaciones.descripcion_cuarto_corta,"+
					"osiris_erp_cobros_enca.id_tipo_paciente AS idtipopaciente, descripcion_tipo_paciente " +
					//"osiris_his_informacion_medica.s_subjetivo,osiris_his_informacion_medica.o_objetivo,osiris_his_informacion_medica.a_analisis,osiris_his_informacion_medica.p_plan "+
					"FROM osiris_erp_cobros_enca,osiris_his_paciente,osiris_his_tipo_pacientes,osiris_aseguradoras,"+
					"osiris_his_habitaciones,osiris_empresas,osiris_his_medicos "+
					"WHERE osiris_erp_cobros_enca.pid_paciente = osiris_his_paciente.pid_paciente "+
					"AND osiris_erp_cobros_enca.id_aseguradora = osiris_aseguradoras.id_aseguradora "+
					"AND osiris_erp_cobros_enca.id_tipo_paciente = osiris_his_tipo_pacientes.id_tipo_paciente "+
					"AND osiris_erp_cobros_enca.id_empresa = osiris_empresas.id_empresa "+
					"AND osiris_his_medicos.id_medico = osiris_erp_cobros_enca.id_medico "+
					//"AND osiris_his_medicos.id_medico = osiris_erp_cobros_enca.id_medico_tratante "+
					//"AND osiris_his_informacion_medica.folio_de_servicio = osiris_erp_cobros_enca.folio_de_servicio "+
					//"AND osiris_erp_cobros_enca.reservacion = 'false' "+
					//"AND osiris_erp_cobros_enca.alta_paciente = 'false' "+
					"AND osiris_erp_cobros_enca.cancelado = 'false' "+
					//"AND osiris_erp_cobros_enca.id_tipo_admisiones > '16' "+
					/*
					"AND osiris_erp_cobros_enca.id_tipo_admisiones <> '940' "+
					"AND osiris_erp_cobros_enca.id_tipo_admisiones <> '930' "+
					"AND osiris_erp_cobros_enca.id_tipo_admisiones <> '920' "+
					"AND osiris_erp_cobros_enca.id_tipo_admisiones <> '950' "+
					"AND osiris_erp_cobros_enca.id_tipo_admisiones <> '300' "+
					"AND osiris_erp_cobros_enca.id_tipo_admisiones <> '400' "+*/

					"AND osiris_erp_cobros_enca.id_habitacion = osiris_his_habitaciones.id_habitacion "+
					query_rango_fechas+
					"ORDER BY osiris_erp_cobros_enca.folio_de_servicio ;";		
				Console.WriteLine(comando.CommandText);
				NpgsqlDataReader lector = comando.ExecuteReader ();
				while (lector.Read()){
					totaldepaciente += 1;
					if((int) lector ["id_aseguradora"] > 1){
						descri_empresa_aseguradora =  (string) lector["descripcion_aseguradora"];
					}else{
						descri_empresa_aseguradora =  (string) lector["descripcion_empresa"];						
					}
					motivo_de_ingreso = (string) classpublic.lee_registro_de_tabla("osiris_erp_movcargos","folio_de_servicio","WHERE folio_de_servicio = '"+lector["folio_de_servicio"].ToString().Trim()+"' ","descripcion_diagnostico_movcargos","string");
					s_subjetivo_px = (string) classpublic.lee_registro_de_tabla("osiris_his_informacion_medica","s_subjetivo","WHERE folio_de_servicio = '"+lector["folio_de_servicio"].ToString().Trim()+"' ","s_subjetivo","string");
					o_objetivo_px = (string) classpublic.lee_registro_de_tabla("osiris_his_informacion_medica","o_objetivo","WHERE folio_de_servicio = '"+lector["folio_de_servicio"].ToString().Trim()+"' ","o_objetivo","string");
					a_analisis_px = (string) classpublic.lee_registro_de_tabla("osiris_his_informacion_medica","a_analisis","WHERE folio_de_servicio = '"+lector["folio_de_servicio"].ToString().Trim()+"' ","a_analisis","string");
					p_plan_px = (string) classpublic.lee_registro_de_tabla("osiris_his_informacion_medica","p_plan","WHERE folio_de_servicio = '"+lector["folio_de_servicio"].ToString().Trim()+"' ","p_plan","string");
					treeViewEngineocupacion.AppendValues (lector["fecha_ingreso"].ToString().Trim(),
						lector["foliodeatencion"].ToString().Trim(),
						lector["nomina_paciente"].ToString().Trim(),
						lector["nombre_completo"].ToString().Trim(),
						lector["edad"].ToString().Trim()+"/"+lector["mesesedad"].ToString().Trim(),
						"",
						lector["descripcion_tipo_paciente"].ToString().Trim(),
						motivo_de_ingreso,
						lector["nombre_medico_encabezado"].ToString().Trim(),
						lector["id_empleado_admision"].ToString().Trim(),
						lector["pidpaciente"].ToString().Trim(),
						descri_empresa_aseguradora,
						s_subjetivo_px,
						o_objetivo_px,
						a_analisis_px,
						p_plan_px);
				}
				entry_total_de_pacientes.Text = totaldepaciente.ToString().Trim();
			}catch (NpgsqlException ex){
				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
					MessageType.Error,ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
				msgBoxError.Run ();			msgBoxError.Destroy();
			}
			conexion.Close ();
		}

		void llenando_lista_de_ocupacion()
		{
			string descri_empresa_aseguradora = "";
			int totaldepaciente = 0;
			string foliodeservicio;
			saldos = 0;
			decimal abonomuestra = 0;
			totabono = 0;
			totcuenta = 0;
			sumacuenta = 0;
			abono = 0;

			NpgsqlConnection conexion; 
			conexion = new NpgsqlConnection (connectionString+nombrebd);
			// Verifica que la base de datos este conectada
			try{
				conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand ();
				comando.CommandText ="SELECT DISTINCT(osiris_erp_movcargos.folio_de_servicio),"+							
					"to_char(osiris_erp_movcargos.folio_de_servicio,'9999999999') AS foliodeatencion, "+
					"to_char(osiris_erp_cobros_enca.pid_paciente,'9999999999') AS pidpaciente, "+
					"nombre1_paciente || ' ' || nombre2_paciente || ' ' || apellido_paterno_paciente || ' ' || apellido_materno_paciente AS nombre_completo, "+
					"to_char(to_number(to_char(age('2008-01-26 13:30:39',osiris_his_paciente.fecha_nacimiento_paciente),'yyyy') ,'9999'),'9999') AS edad, "+
					"to_char(to_number(to_char(age('2008-01-26 01:30:39',osiris_his_paciente.fecha_nacimiento_paciente),'MM'),'99'),'99') AS mesesedad, "+
					"osiris_erp_cobros_enca.nombre_medico_encabezado, "+
					"osiris_erp_cobros_enca.id_medico,nombre_medico, "+
					"osiris_erp_cobros_enca.id_medico_tratante,nombre_medico,"+
					"osiris_erp_cobros_enca.nombre_medico_tratante,"+
					"osiris_erp_cobros_enca.id_aseguradora,descripcion_aseguradora,"+
					"osiris_erp_cobros_enca.id_empresa,descripcion_empresa,"+
					"to_char(osiris_erp_cobros_enca.id_cuarto,'999999999') AS cuarto, "+
					"to_char(osiris_erp_cobros_enca.total_abonos,'99999999.99') AS totabonos, "+
					"osiris_erp_movcargos.descripcion_diagnostico_movcargos,"+
					"to_char(osiris_erp_cobros_enca.fechahora_creacion,'dd-MM-yyyy HH24:mi') AS fecha_ingreso, "+
					"to_char(osiris_erp_cobros_enca.total_procedimiento,'99999999.99') AS totalproc,"+
					"osiris_erp_cobros_enca.id_habitacion,to_char(osiris_his_habitaciones.numero_cuarto,'999999999') AS numerocuarto,"+
					"osiris_his_habitaciones.descripcion_cuarto,osiris_his_habitaciones.id_tipo_admisiones AS idtipoadmisiones_habitacion,"+
					"osiris_his_habitaciones.descripcion_cuarto_corta,"+
					"osiris_erp_movcargos.id_tipo_paciente AS idtipopaciente, descripcion_tipo_paciente "+
					"FROM osiris_erp_cobros_enca,osiris_his_paciente,osiris_erp_movcargos,osiris_his_tipo_pacientes,osiris_aseguradoras,"+
					"osiris_his_habitaciones,osiris_empresas,osiris_his_medicos "+
					"WHERE osiris_erp_cobros_enca.pid_paciente = osiris_his_paciente.pid_paciente "+
					"AND osiris_erp_movcargos.folio_de_servicio = osiris_erp_cobros_enca.folio_de_servicio "+
					"AND osiris_erp_cobros_enca.id_aseguradora = osiris_aseguradoras.id_aseguradora "+
					"AND osiris_erp_movcargos.id_tipo_paciente = osiris_his_tipo_pacientes.id_tipo_paciente "+
					"AND osiris_erp_cobros_enca.id_empresa = osiris_empresas.id_empresa "+
					//"AND osiris_his_medicos.id_medico = osiris_erp_cobros_enca.id_medico "+
					"AND osiris_his_medicos.id_medico = osiris_erp_cobros_enca.id_medico_tratante "+
					"AND osiris_erp_cobros_enca.reservacion = 'false' "+
					"AND osiris_erp_cobros_enca.alta_paciente = 'false' "+
					"AND osiris_erp_cobros_enca.cancelado = 'false' "+
					//"AND osiris_erp_movcargos.id_tipo_admisiones > '16' "+
					/*
					"AND osiris_erp_movcargos.id_tipo_admisiones <> '940' "+
					"AND osiris_erp_movcargos.id_tipo_admisiones <> '930' "+
					"AND osiris_erp_movcargos.id_tipo_admisiones <> '920' "+
					"AND osiris_erp_movcargos.id_tipo_admisiones <> '950' "+
					"AND osiris_erp_movcargos.id_tipo_admisiones <> '300' "+
					"AND osiris_erp_movcargos.id_tipo_admisiones <> '400' "+
					*/
					"AND osiris_erp_cobros_enca.id_habitacion = osiris_his_habitaciones.id_habitacion "+
					"ORDER BY osiris_erp_movcargos.folio_de_servicio ;";		
				//Console.WriteLine(comando.CommandText);
				NpgsqlDataReader lector = comando.ExecuteReader ();
				while (lector.Read()){
					foliodeservicio = (string) lector["foliodeatencion"];
					totaldepaciente += 1;
					Console.WriteLine(","+foliodeservicio.ToString().Trim());

					if (this.checkbutton_agregar_monto.Active == true){
						NpgsqlConnection conexion1;
						conexion1 = new NpgsqlConnection (connectionString+nombrebd);
						// Verifica que la base de datos este conectada
						try{
							conexion1.Open ();
							NpgsqlCommand comando1; 
							comando1 = conexion1.CreateCommand ();
							comando1.CommandText ="SELECT to_char(folio_de_servicio,'9999999999') AS foliodeatencion,"+
								"to_char(sum(cantidad_aplicada),'9999999999.99') AS totaldeproductos,"+
								"to_char(sum(precio_producto * cantidad_aplicada),'9999999999.99') AS totalpreciopublico,"+
								"to_char(sum(precio_costo_unitario * cantidad_aplicada),'9999999999.99') AS totalpreciocosto "+
								"FROM osiris_erp_cobros_deta "+
								"WHERE eliminado = 'false' "+
								"AND folio_de_servicio = '"+foliodeservicio+"' "+
								"GROUP BY folio_de_servicio; ";
							NpgsqlDataReader lector1 = comando1.ExecuteReader ();
							totcuenta = 0;
							saldos = 0;
							if(lector1.Read()){
								totcuenta = decimal.Parse((string) lector1["totalpreciopublico"]);
								sumacuenta += decimal.Parse((string) lector1["totalpreciopublico"]);
								entry_totalsaldos.Text = sumacuenta.ToString();
								abono = decimal.Parse((string) lector["totabonos"]);
								saldos = totcuenta - abono;
							}
						}catch (NpgsqlException ex){
							MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
								MessageType.Error,ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
							msgBoxError.Run ();			msgBoxError.Destroy();
						}
						conexion1.Close ();
					}else{
						totcuenta = 0;
						sumacuenta = 0;
						abono = 0 ;
						saldos = 0;
						entry_totalsaldos.Text = sumacuenta.ToString();
					}
					if (this.checkbutton_impr_todo_proce.Active == true){
						totabono += decimal.Parse((string) lector["totabonos"]);
						entry_totalabonos.Text = totabono.ToString();
						abono = decimal.Parse((string) lector["totabonos"]);
						if (this.checkbutton_agregar_monto.Active == false){
							saldos = 0;
						}else{
							saldos = totcuenta - abono;
						}
					}else{
						abonomuestra = 0 ;
						abono = 0;
						saldos = 0;
						totabono = 0 ;
						entry_totalabonos.Text = totabono.ToString();
					}

					if((int) lector ["id_aseguradora"] > 1){
						descri_empresa_aseguradora =  (string) lector["descripcion_aseguradora"];
					}else{
						descri_empresa_aseguradora =  (string) lector["descripcion_empresa"];						
					}
					idcuarto = (string) lector["numerocuarto"]+"("+(string) lector["descripcion_cuarto_corta"]+")";
					treeViewEngineocupacion.AppendValues ((string) lector["nombre_completo"],
						(string) lector["foliodeatencion"],
						(string) lector["pidpaciente"],
						(string) lector["fecha_ingreso"],
						totcuenta.ToString(),
						abono.ToString(),
						saldos.ToString(),
						(string) lector["nombre_medico_tratante"],
						idcuarto.Trim(),
						(string) lector["descripcion_diagnostico_movcargos"],
						(string) lector["descripcion_tipo_paciente"],
						descri_empresa_aseguradora);
				}
				entry_total_de_pacientes.Text = totaldepaciente.ToString().Trim();
			}catch (NpgsqlException ex){
				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
					MessageType.Error,ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
				msgBoxError.Run ();			msgBoxError.Destroy();
			}
			conexion.Close ();
		}
	}	
}