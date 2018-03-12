//
//  resultado_lab.cs
//
//  Author:
//       dolivares <>
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

using Npgsql;
using System.Data;
using Gtk;
using Glade;
using System.Collections;

namespace osiris
{
	public class resultados_lab
	{
		// Boton general para salir de las ventanas
		// Todas la ventanas en glade este boton debe estra declarado identico
		[Widget] Gtk.Button button_salir;

		// Declarando ventana de Resultados
		[Widget] Gtk.Window resultados_laboratorio;
		[Widget] Gtk.TreeView lista_de_resultados;
		[Widget] Gtk.Button button_guardar;
		[Widget] Gtk.Button button_imprimir;
		[Widget] Gtk.Button button_validar_examen;
		[Widget] Gtk.ComboBox combobox_quimicos_aut;
		[Widget] Gtk.ComboBox combobox_tipo_examen;
		[Widget] Gtk.CheckButton checkbutton_valor_referencia;
		[Widget] Gtk.CheckButton checkbutton_parametros;
		[Widget] Gtk.Entry entry_fecha_solicitud_res;
		[Widget] Gtk.Entry entry_hora_solicitud_res;
		[Widget] Gtk.Entry entry_folio_laboratorio_res;
		[Widget] Gtk.Entry entry_observaciones;
		[Widget] Gtk.Entry entry_fecha_validado;
		[Widget] Gtk.Statusbar statusbar_reslab;

		public string LoginEmpleado;
		public string NomEmpleados;
		public string connectionString;
		public string nombrebd;

		public int folioservicio = 0;							// Toma el valor de numero de atencion de paciente
		public int PidPaciente = 0;								// Toma la actualizacion del pid del paciente
		public string edadpac;
		public string fecha_nacimiento;
		public string nombpaciente = "";
		public string tipo_paciente = "";
		public string dir_pac;
		public string empresapac = "";
		public string observaciones = "";
		public string idsecuencia = "";
		public string id_produ = "";
		public string tipoexamen = "";

		public string quimicoaut = "";
		public string idquimico = "";
		public string cedulaquimico = "";

		public string numerosolicitud;
		public string fechasolicitud;
		public string sexopaciente;
		public string procedencia;
		public string medicotratante;
		public string nombre_estudio;
		public bool resultados_editables = false;
		public bool actualizaresultado = false;

		public Gtk.ListStore treeViewEngineresultados;
		public Gtk.CellRendererText cellr0;
		public Gtk.CellRendererText cellr1;
		public Gtk.CellRendererText cellr2;
		public Gtk.CellRendererText cellr3;
		public Gtk.CellRendererToggle cellr5;

		//Declaracion de ventana de error
		protected Gtk.Window MyWinError;
		protected Gtk.Window MyWin;

		string[] args_args = {""};
		string[] args_tipo_estudio = {"","ORDINARIO","URGENTE"};
		int[] args_id_array = {0,1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20};

		class_conexion conexion_a_DB = new class_conexion();

		public resultados_lab(bool _resultados_editables_,string _LoginEmpleado_,string _NomEmpleados_,string _id_produ_,
			string _idsecuencia_,string _nombrebd_,string _dir_pac_,string _edadpac_,string _empresapac_,
			int _PidPaciente_,string _entry_nombre_paciente_,int _folioservicio_,
			string _fecha_nacimiento_,string _entry_tipo_paciente_,
			string _fechasolicitud_,string _numerosolicitud_,string _sexopaciente_,string _procedencia_,
			string _medicotratante_,string _nombre_estudio_)
		{
			//Guardo valores a variables
			resultados_editables = _resultados_editables_;
			LoginEmpleado = _LoginEmpleado_;
			NomEmpleados = _NomEmpleados_;
			id_produ = _id_produ_;
			idsecuencia = _idsecuencia_;
			dir_pac = _dir_pac_; 
			edadpac = _edadpac_;
			empresapac = _empresapac_;
			PidPaciente = _PidPaciente_;
			nombpaciente = _entry_nombre_paciente_;
			folioservicio = _folioservicio_;
			fecha_nacimiento = _fecha_nacimiento_; 
			tipo_paciente = _entry_tipo_paciente_;
			numerosolicitud = _numerosolicitud_;
			fechasolicitud = _fechasolicitud_;
			sexopaciente = _sexopaciente_ ;
			procedencia = _procedencia_;
			medicotratante = _medicotratante_;
			nombre_estudio = _nombre_estudio_;
			connectionString = conexion_a_DB._url_servidor+conexion_a_DB._port_DB+conexion_a_DB._usuario_DB+conexion_a_DB._passwrd_user_DB;
			nombrebd = conexion_a_DB._nombrebd;
			///////////////////ventana de resultados de examenes/////////////////

			//valores y subprogramas de la ventana
			Glade.XML gxml = new Glade.XML (null, "laboratorio.glade", "resultados_laboratorio", null);
			gxml.Autoconnect (this);		resultados_laboratorio.Show();
			if(this.resultados_editables == false) {
				button_guardar.Sensitive = false;
				//button_imprimir.Sensitive = false;
				//combobox_quimicos_aut.Sensitive = false;
				//combobox_tipo_examen.Sensitive = false;
				button_validar_examen.Sensitive = false;			
			}

			entry_fecha_solicitud_res.Text = fechasolicitud.Substring(0,10);
			entry_hora_solicitud_res.Text = fechasolicitud.Substring(11,5);			
			entry_folio_laboratorio_res.Text = numerosolicitud;

			if(numerosolicitud.Trim() == "0"){
				entry_fecha_solicitud_res.Text = DateTime.Now.ToString("yyyy-MM-dd");
				entry_hora_solicitud_res.Text = DateTime.Now.ToString("HH:mm");			
				entry_folio_laboratorio_res.Text = "";
				entry_fecha_solicitud_res.IsEditable = true;
				entry_hora_solicitud_res.IsEditable = true;			
				entry_folio_laboratorio_res.IsEditable = true;
				button_validar_examen.Sensitive = false;
			}			
			button_guardar.Clicked += new EventHandler(on_button_guardar_resultado_clicked);
			button_salir.Clicked += new EventHandler(on_cierraventanas_clicked); 
			button_validar_examen.Clicked += new EventHandler(on_button_validar_examen_clicked);
			button_imprimir.Clicked += new EventHandler(on_button_imprimir_clicked);

			llenado_combobox(0,"",combobox_tipo_examen,"array","","","",args_tipo_estudio,args_id_array,"");
			llenado_combobox(1,"",combobox_quimicos_aut,"sql","SELECT osiris_empleado.id_empleado,cedula_profesional,nombre1_empleado || ' ' || nombre2_empleado || ' ' || apellido_paterno_empleado || ' ' || apellido_materno_empleado AS nombre_completo,osiris_empleado_detalle.id_puesto FROM osiris_empleado,osiris_empleado_detalle " +
				"WHERE osiris_empleado.id_empleado = osiris_empleado_detalle.id_empleado " +
				"AND osiris_empleado_detalle.id_puesto = '43';","nombre_completo","id_empleado",args_args,args_id_array,"cedula_profesional");

			//CREACION DE LISTAS Y TREEVIEW
			crea_treeview_resultados();
			llenado_de_treeview_resultados();

			statusbar_reslab.Pop(0);
			statusbar_reslab.Push(1, "login: "+LoginEmpleado+"  |Usuario: "+NomEmpleados);
			statusbar_reslab.HasResizeGrip = false;
		}

		void llenado_combobox(int tipodellenado,string descrip_defaul,object obj,string sql_or_array,string query_SQL,string name_field_desc,string name_field_id,string[] args_array,int[] args_id_array,string name_field_id2)
		{			
			Gtk.ComboBox combobox_llenado = (Gtk.ComboBox) obj;
			//Gtk.ComboBox combobox_pos_neg = obj as Gtk.ComboBox;
			combobox_llenado.Clear();
			CellRendererText cell = new CellRendererText();
			combobox_llenado.PackStart(cell, true);
			combobox_llenado.AddAttribute(cell,"text",0);	        
			ListStore store = new ListStore( typeof (string),typeof (string),typeof (string));
			combobox_llenado.Model = store;			
			if ((int) tipodellenado == 1){
				store.AppendValues ((string) descrip_defaul,"0","0");
			}			
			if(sql_or_array == "array"){			
				for (int colum_field = 0; colum_field < args_array.Length; colum_field++){
					store.AppendValues (args_array[colum_field],args_id_array[colum_field].ToString().Trim(),"");
				}
			}
			if(sql_or_array == "sql"){			
				NpgsqlConnection conexion; 
				conexion = new NpgsqlConnection (connectionString+nombrebd);
				// Verifica que la base de datos este conectada
				try{
					conexion.Open ();
					NpgsqlCommand comando; 
					comando = conexion.CreateCommand ();
					comando.CommandText = query_SQL;
					NpgsqlDataReader lector = comando.ExecuteReader ();
					while (lector.Read()){
						if(name_field_id2 == ""){
							store.AppendValues (lector[ name_field_desc ].ToString().Trim(), lector[ name_field_id].ToString().Trim(),"");
						}else{
							store.AppendValues (lector[ name_field_desc ].ToString().Trim(), lector[ name_field_id].ToString().Trim(),lector[ name_field_id2].ToString().Trim());
						}
					}
				}catch (NpgsqlException ex){
					MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
						MessageType.Error,ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
					msgBoxError.Run ();				msgBoxError.Destroy();
				}
				conexion.Close ();
			}			
			TreeIter iter;
			if (store.GetIterFirst(out iter)){
				combobox_llenado.SetActiveIter (iter);
			}
			combobox_llenado.Changed += new EventHandler (onComboBoxChanged_llenado);			
		}

		void onComboBoxChanged_llenado (object sender, EventArgs args)
		{
			ComboBox onComboBoxChanged = sender as ComboBox;
			if (sender == null){	return; }
			TreeIter iter;
			if (onComboBoxChanged.GetActiveIter (out iter)){
				switch (onComboBoxChanged.Name.ToString()){	
				case "combobox_quimicos_aut":
					quimicoaut = (string) combobox_quimicos_aut.Model.GetValue(iter,0);						
					cedulaquimico = (string) combobox_quimicos_aut.Model.GetValue(iter,2);
					idquimico = (string) combobox_quimicos_aut.Model.GetValue(iter,1);
					Console.WriteLine(quimicoaut+", "+idquimico+", "+cedulaquimico);
					break;
				case "combobox_tipo_examen":
					tipoexamen = (string) combobox_tipo_examen.Model.GetValue(iter,0);
					break;
				}
			}
		}

		void crea_treeview_resultados()
		{
			treeViewEngineresultados = new ListStore(
				typeof(string),
				typeof(string),
				typeof(string),
				typeof(string),
				typeof(string),
				typeof(bool));
			lista_de_resultados.Model = treeViewEngineresultados;
			lista_de_resultados.RulesHint = true;

			TreeViewColumn col_parametro = new TreeViewColumn();
			cellr0 = new CellRendererText();
			col_parametro.Title = "Parametro"; // titulo de la cabecera de la columna, si está visible
			col_parametro.PackStart(cellr0, true);
			col_parametro.AddAttribute (cellr0, "text", 0);
			col_parametro.SortColumnId = (int) Column_resu.col_parametro;
			col_parametro.Resizable = true;
			cellr0.Editable = true;
			cellr0.Edited += NumberCellEditedresultado0;

			TreeViewColumn col_resultado = new TreeViewColumn();
			cellr1 = new CellRendererText();
			col_resultado.Title = "Resultado"; // titulo de la cabecera de la columna, si está visible
			col_resultado.PackStart(cellr1, true);
			col_resultado.AddAttribute (cellr1, "text", 1);
			col_resultado.SortColumnId = (int) Column_resu.col_resultado;
			col_resultado.Resizable = true;
			cellr1.Editable = true;
			cellr1.Edited += NumberCellEditedresultado1;

			TreeViewColumn col_vr = new TreeViewColumn();
			cellr2 = new CellRendererText();
			col_vr.Title = "Valor de Referencia"; // titulo de la cabecera de la columna, si está visible
			col_vr.PackStart(cellr2, true);
			col_vr.AddAttribute (cellr2, "text", 2);
			col_vr.SortColumnId = (int) Column_resu.col_vr;
			col_vr.Resizable = true;
			cellr2.Editable = true;
			cellr2.Edited += NumberCellEditedresultado2;

			TreeViewColumn col_unidades = new TreeViewColumn();
			cellr3 = new CellRendererText();
			col_unidades.Title = "Unidades"; // titulo de la cabecera de la columna, si está visible
			col_unidades.PackStart(cellr3, true);
			col_unidades.AddAttribute (cellr3, "text", 3);
			col_unidades.SortColumnId = (int) Column_resu.col_unidades;
			col_unidades.Resizable = true;

			TreeViewColumn col_selecciona = new TreeViewColumn();
			cellr5 = new CellRendererToggle();
			col_selecciona.Title = "Selecciona"; // titulo de la cabecera de la columna, si está visible
			col_selecciona.PackStart(cellr5, true);
			col_selecciona.AddAttribute (cellr5, "active", 5);
			col_selecciona.Resizable = true;
			cellr5.Activatable = true;		
			cellr5.Toggled += selecciona_fila;			

			lista_de_resultados.AppendColumn(col_parametro);
			lista_de_resultados.AppendColumn(col_resultado);
			lista_de_resultados.AppendColumn(col_vr);
			lista_de_resultados.AppendColumn(col_unidades);
			lista_de_resultados.AppendColumn(col_selecciona);

			if(this.resultados_editables == false){
				cellr0.Editable = false;	
				cellr1.Editable = false;		
				cellr2.Editable = false;
			}
		}

		enum Column_resu{
			col_parametro,
			col_resultado,
			col_vr,
			col_unidades
		}

		// Cuando seleccion el treeview de cargos extras para cargar los productos  
		void selecciona_fila(object sender, ToggledArgs args)
		{
			TreeIter iter;
			TreePath path = new TreePath (args.Path);
			if (lista_de_resultados.Model.GetIter (out iter, path)) {
				bool old = (bool) lista_de_resultados.Model.GetValue (iter,5);
				lista_de_resultados.Model.SetValue(iter,5,!old);
			}	
		}

		void llenado_de_treeview_resultados()
		{
			bool acceso_resultados = true;
			this.treeViewEngineresultados.Clear();
			NpgsqlConnection conexion; 		
			conexion = new NpgsqlConnection (connectionString+nombrebd);
			try{
				conexion.Open ();
				NpgsqlCommand comando;			comando = conexion.CreateCommand ();
				if( this.numerosolicitud != "0"){
					comando.CommandText = "SELECT parametro,valor_referencia,resultado,unidades,"+
						"to_char(fechahora_captura,'yyyy-MM-dd') AS fechacap,to_char(fechahora_captura,'HH24:mi') AS horacap, "+
						"to_char(folio_laboratorio,'999999999') AS foliolab,to_char(id_secuencia,'999999999') AS secuencia,"+
						"validado,to_char(fechahora_validacion,'dd-MM-yyyy HH24:mi') AS fechavalidacion,observaciones_de_examen "+
						"FROM osiris_his_resultados_laboratorio "+
						"WHERE osiris_his_resultados_laboratorio.id_producto = '"+id_produ.Trim()+"' "+
						"AND osiris_his_resultados_laboratorio.folio_laboratorio = '"+numerosolicitud.Trim()+"' "+
						"ORDER BY osiris_his_resultados_laboratorio.id_secuencia;"; 
					Console.WriteLine(comando.CommandText);
					NpgsqlDataReader lector = comando.ExecuteReader ();
					if (lector.Read()){
						if(this.resultados_editables == false){
							if ((bool) lector ["validado"] == false){
								acceso_resultados = false;
							}
						}
						if(acceso_resultados == true){
							treeViewEngineresultados.AppendValues(
								(string) lector ["parametro"],
								(string) lector ["resultado"],
								(string) lector ["valor_referencia"],
								(string) lector ["unidades"],
								(string) lector ["secuencia"],
								true);
							entry_observaciones.Text = (string) lector ["observaciones_de_examen"];
							if ((bool) lector ["validado"] == true){
								this.button_validar_examen.Sensitive = false;
								this.entry_fecha_validado.Text = (string) lector ["fechavalidacion"]; 
							}
							while (lector.Read()){
								this.treeViewEngineresultados.AppendValues(
									(string) lector ["parametro"],
									(string) lector ["resultado"],
									(string) lector ["valor_referencia"],
									(string) lector ["unidades"],
									(string) lector ["secuencia"],
									true);
							}
							actualizaresultado = true;
						}else{
							MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
								MessageType.Info, ButtonsType.Close, "Este estudio no se Encuentra Validado por Laboratorio");
							msgBoxError.Run ();			msgBoxError.Destroy();
						}
					}else{
						// check only si existe el resultado sino llena con los parametros
						if(this.resultados_editables == true){
							NpgsqlConnection conexion1; 		conexion1 = new NpgsqlConnection (connectionString+nombrebd);
							try{
								conexion1.Open ();
								NpgsqlCommand comando1;			comando1 = conexion1.CreateCommand ();
								comando1.CommandText = "SELECT parametro,valor_referencia,unidades,to_char(id_secuencia,'999999999') AS secuencia "+
									"FROM osiris_his_examenes_laboratorio "+
									"WHERE osiris_his_examenes_laboratorio.id_producto = '"+id_produ+"' "+
									"ORDER BY osiris_his_examenes_laboratorio.id_secuencia_estudio,osiris_his_examenes_laboratorio.id_secuencia_parametros; "; 
								NpgsqlDataReader lector1 = comando1.ExecuteReader ();
								Console.WriteLine(comando1.CommandText);
								if (lector1.Read()){
									this.treeViewEngineresultados.AppendValues(
										(string) lector1 ["parametro"],
										" -- ",
										(string) lector1 ["valor_referencia"],
										(string) lector1 ["unidades"],
										(string) lector1 ["secuencia"],
										"false");

									while (lector1.Read()){
										this.treeViewEngineresultados.AppendValues(
											(string) lector1 ["parametro"],
											" -- ",
											(string) lector1 ["valor_referencia"],
											(string) lector1 ["unidades"],
											(string) lector1 ["secuencia"],
											"false");
									}
								}else{
									//resultados_laboratorio.Destroy();
									MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,MessageType.Info,ButtonsType.Close, "El estudio no contiene RESULTADOS CARGADOS\n hagaselo saber al Administrador del Sistema");
									msgBoxError.Run ();	msgBoxError.Destroy();
								}
							}catch (NpgsqlException ex){
								//Console.WriteLine ("PostgresSQL error: {0}",ex.Message);
								MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,MessageType.Info,ButtonsType.Close, "PostgresSQL error: {0}",ex.Message);
								msgBoxError.Run ();	msgBoxError.Destroy();
							}
							conexion1.Close ();
							actualizaresultado = false;
						}
					}
					conexion.Close();
				}
			}catch (NpgsqlException ex){
				//Console.WriteLine ("PostgresSQL error: {0}",ex.Message);
				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,MessageType.Info,ButtonsType.Close, "PostgresSQL error: {0}",ex.Message);
				msgBoxError.Run ();	msgBoxError.Destroy();
			}
			conexion.Close ();
		}

		void NumberCellEditedresultado0 (object o, EditedArgs args) {
			Gtk.TreeIter iter;
			this.treeViewEngineresultados.GetIter (out iter, new Gtk.TreePath (args.Path));
			this.treeViewEngineresultados.SetValue(iter,(int) Column_resu.col_parametro,args.NewText);
		}

		void NumberCellEditedresultado1 (object o, EditedArgs args)	{
			Gtk.TreeIter iter;
			this.treeViewEngineresultados.GetIter (out iter, new Gtk.TreePath (args.Path));
			this.treeViewEngineresultados.SetValue(iter,(int) Column_resu.col_resultado,args.NewText);
		}

		void NumberCellEditedresultado2 (object o, EditedArgs args) {
			Gtk.TreeIter iter;
			this.treeViewEngineresultados.GetIter (out iter, new Gtk.TreePath (args.Path));
			this.treeViewEngineresultados.SetValue(iter,(int) Column_resu.col_vr,args.NewText);
		}

		// Valida entradas se utiliza en ventana de resultados
		[GLib.ConnectBefore ()]   	  // Esto es indispensable para que funcione    
		public void onKeyPressEvent_fechahora_solicitud(object o, Gtk.KeyPressEventArgs args)
		{
			//Console.WriteLine(Convert.ToChar(args.Event.KeyValue));	Console.WriteLine(args.Event.Key);
			string misDigitos = ".0123456789ﾰﾱﾲﾳﾴﾵﾶﾷﾸﾹﾮｔｒｓｑ（ﾭ￢:";
			if (Array.IndexOf(misDigitos.ToCharArray(), Convert.ToChar(args.Event.Key)) == -1 && args.Event.Key.ToString()  != "BackSpace")
			{	args.RetVal = true;		}
		}

		void on_button_guardar_resultado_clicked(object sender, EventArgs args)
		{
			bool camposcompletos = Verificacion_de_valores();
			TreeIter iter;
			//if(idquimico == ""){
			if (camposcompletos == true) {
				if(actualizaresultado == false){
					MessageDialog msgBox = new MessageDialog (MyWin,DialogFlags.Modal,MessageType.Question,ButtonsType.YesNo,"¿ Desea grabar esta infomacion ?");
					ResponseType miResultado = (ResponseType)msgBox.Run ();		msgBox.Destroy();
					if (miResultado == ResponseType.Yes){
						NpgsqlConnection conexion; 		conexion = new NpgsqlConnection (connectionString+nombrebd);
						// Verifica que la base de datos este conectada
						try{
							conexion.Open ();
							NpgsqlCommand comando; 		comando = conexion.CreateCommand ();
							if (treeViewEngineresultados.GetIterFirst (out iter)){
								comando.CommandText ="INSERT INTO osiris_his_resultados_laboratorio("+
									"id_producto,"+
									"folio_laboratorio,"+
									"folio_de_servicio,"+
									"pid_paciente,"+
									"parametro,"+
									"resultado,"+
									"valor_referencia,"+
									"unidades,"+
									"id_quien_capturo,"+
									"fechahora_captura,"+
									"id_quimico,"+
									"id_quien_creo,"+
									"fechahora_creacion,"+
									"observaciones_de_examen) "+
									"VALUES ('"+
									id_produ+"','"+
									entry_folio_laboratorio_res.Text.Trim()+"','"+
									folioservicio+"','"+//folio
									(int) PidPaciente+"','"+//pid
									(string) lista_de_resultados.Model.GetValue(iter,0)+"','"+
									(string) lista_de_resultados.Model.GetValue(iter,1)+"','"+
									(string) lista_de_resultados.Model.GetValue(iter,2)+"','"+
									(string) lista_de_resultados.Model.GetValue(iter,3)+"','"+
									LoginEmpleado+"','"+
									DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"','"+
									(string) idquimico+"','"+
									LoginEmpleado+"','"+
									DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"','"+
									entry_observaciones.Text.Trim()+
									"');";
								//Console.WriteLine("query de graba resultado "+comando.CommandText);
								comando.ExecuteNonQuery(); 					comando.Dispose();
								while (treeViewEngineresultados.IterNext(ref iter)){
									comando.CommandText ="INSERT INTO osiris_his_resultados_laboratorio("+
										"id_producto,"+
										"folio_laboratorio,"+
										"folio_de_servicio,"+
										"pid_paciente,"+
										"parametro,"+
										"resultado,"+
										"valor_referencia,"+
										"unidades,"+
										"id_quien_capturo,"+
										"fechahora_captura,"+
										"id_quimico,"+
										"id_quien_creo,"+
										"fechahora_creacion,"+
										"observaciones_de_examen) "+
										"VALUES ('"+
										id_produ+"','"+
										entry_folio_laboratorio_res.Text.Trim()+"','"+
										folioservicio+"','"+//folio
										(int) PidPaciente+"','"+//pid
										(string) lista_de_resultados.Model.GetValue(iter,0)+"','"+
										(string) lista_de_resultados.Model.GetValue(iter,1)+"','"+
										(string) lista_de_resultados.Model.GetValue(iter,2)+"','"+
										(string) lista_de_resultados.Model.GetValue(iter,3)+"','"+
										LoginEmpleado+"','"+
										DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"','"+
										(string)idquimico+"','"+
										LoginEmpleado+"','"+
										DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"','"+
										entry_observaciones.Text.Trim()+
										"');";
									//Console.WriteLine("query de graba resultado 2: "+comando.CommandText);
									comando.ExecuteNonQuery();  					comando.Dispose();
								}
								conexion.Close ();

								// Valida que no tenga numero de folio para laboratorio
								// esto significa que el cargo se realizo desde el modulo de caja
								if(numerosolicitud.Trim() == "0"){
									NpgsqlConnection conexion1; 		conexion1 = new NpgsqlConnection (connectionString+nombrebd);
									// Verifica que la base de datos este conectada
									try{
										conexion1.Open ();
										NpgsqlCommand comando1; 		comando1 = conexion1.CreateCommand();
										comando1.CommandText = "UPDATE osiris_erp_cobros_deta "+
											"SET folio_interno_dep = '"+this.entry_folio_laboratorio_res.Text.Trim()+"',"+
											"fechahora_solicitud = '"+this.entry_fecha_solicitud_res.Text.Trim()+" "+this.entry_hora_solicitud_res.Text.Trim()+":00' "+
											"WHERE id_secuencia = '"+idsecuencia.Trim()+"' ;";
										//Console.WriteLine(comando1.CommandText);
										comando1.ExecuteNonQuery(); 					comando.Dispose();
										numerosolicitud = entry_folio_laboratorio_res.Text;
										button_validar_examen.Sensitive = true;
									}catch (NpgsqlException ex){
										//Console.WriteLine ("PostgresSQL error: {0}",ex.Message);
										MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,MessageType.Info,ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
										msgBoxError.Run ();	msgBoxError.Destroy();
									}
									conexion1.Close ();
								}
								actualizaresultado = true;
								llenado_de_treeview_resultados();
							}
						}catch (NpgsqlException ex){
							//Console.WriteLine ("PostgresSQL error: {0}",ex.Message);
							MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,MessageType.Info,ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
							msgBoxError.Run ();	msgBoxError.Destroy();
						}
						conexion.Close ();
					}
					//button_guardar.Sensitive = false;
				}else{
					// ENTRO AQUI PARA ACTUALIZAR DATOS si es que ya se han guardado
					MessageDialog msgBox = new MessageDialog (MyWin,DialogFlags.Modal,MessageType.Question,ButtonsType.YesNo,"¿ Desea Actualizar esta infomacion ?");
					ResponseType miResultado = (ResponseType)msgBox.Run ();		msgBox.Destroy();
					if (miResultado == ResponseType.Yes){
						NpgsqlConnection conexion; 		conexion = new NpgsqlConnection (connectionString+nombrebd);
						// Verifica que la base de datos este conectada
						try{
							conexion.Open ();
							NpgsqlCommand comando; 		comando = conexion.CreateCommand ();
							if (treeViewEngineresultados.GetIterFirst (out iter)){
								if((string) lista_de_resultados.Model.GetValue(iter,1) != (string) lista_de_resultados.Model.GetValue(iter,4)){
									comando.CommandText = "UPDATE osiris_his_resultados_laboratorio "+
										"SET id_producto = '"+id_produ+"',"+
										"folio_de_servicio = '"+folioservicio+"',"+
										"pid_paciente = '"+(int) PidPaciente+"',"+
										"parametro = '"+(string) lista_de_resultados.Model.GetValue(iter,0)+"',"+
										"resultado = '"+(string) lista_de_resultados.Model.GetValue(iter,1)+"',"+
										"valor_referencia = '"+(string) lista_de_resultados.Model.GetValue(iter,2)+"',"+
										"id_quien_capturo = '"+LoginEmpleado+"',"+
										"fechahora_captura = '"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"',"+
										"id_quimico = '"+(string)idquimico+"', "+
										"historial_cambios = historial_cambios || '"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+" Resultado = "+(string) lista_de_resultados.Model.GetValue(iter,4)+"\n' "+
										"WHERE id_secuencia = '"+(string) (lista_de_resultados.Model.GetValue(iter,4)).ToString().Trim()+"' ;";
									comando.ExecuteNonQuery(); 					comando.Dispose();
								}
								while (treeViewEngineresultados.IterNext(ref iter)){
									if((string) lista_de_resultados.Model.GetValue(iter,1) != (string) lista_de_resultados.Model.GetValue(iter,4)){
										comando.CommandText = "UPDATE osiris_his_resultados_laboratorio "+
											"SET id_producto = '"+id_produ+"',"+
											"folio_de_servicio = '"+folioservicio+"',"+
											"pid_paciente = '"+(int) PidPaciente+"',"+
											"parametro = '"+(string) lista_de_resultados.Model.GetValue(iter,0)+"',"+
											"resultado = '"+(string) lista_de_resultados.Model.GetValue(iter,1)+"',"+
											"valor_referencia = '"+(string) lista_de_resultados.Model.GetValue(iter,2)+"',"+
											"id_quien_capturo = '"+LoginEmpleado+"',"+
											"fechahora_captura = '"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"',"+
											"id_quimico = '"+(string)idquimico+"', "+
											"historial_cambios = historial_cambios || '"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+" Resultado = "+(string) lista_de_resultados.Model.GetValue(iter,4)+"\n' "+
											"WHERE id_secuencia = '"+(string) (lista_de_resultados.Model.GetValue(iter,4)).ToString().Trim()+"' ;";
										comando.ExecuteNonQuery();  					comando.Dispose();
									}
								}								
							}
						}catch (NpgsqlException ex){
							Console.WriteLine ("PostgresSQL error: {0}",ex.Message);
							MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,MessageType.Info,ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
							msgBoxError.Run ();	msgBoxError.Destroy();
						}
						conexion.Close ();
					}
				}
			}else{
				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
					MessageType.Info,ButtonsType.Close,"Seleccione un Quimico o verifique que los campos \n de fecha, hora y folio no se encuentren en blanco");
				msgBoxError.Run ();	msgBoxError.Destroy();
			}
		}

		void on_button_validar_examen_clicked(object sender, EventArgs args)
		{
			MessageDialog msgBox = new MessageDialog (MyWin,DialogFlags.Modal,MessageType.Question,ButtonsType.YesNo,"¿ Sr. "+NomEmpleados+" Esta Seguro de Validar este Examen ?");
			ResponseType miResultado = (ResponseType)msgBox.Run ();		msgBox.Destroy();
			if (miResultado == ResponseType.Yes){
				string[,] parametros;
				object[] paraobj;
				parametros = new string[,] {
					{ "validado = '","true'," },
					{ "id_quimico_validacion = '",LoginEmpleado+"'," },
					{ "fechahora_validacion = '",DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"' " },
					{ "WHERE osiris_his_resultados_laboratorio.id_producto = '",id_produ+"' " },
					{ "AND osiris_his_resultados_laboratorio.folio_laboratorio = '",numerosolicitud.Trim()+"';"}
				};
				paraobj = new object[] { entry_folio_laboratorio_res};
				new osiris.update_registro ("osiris_his_resultados_laboratorio", parametros, paraobj);

				parametros = new string[,] {
					{ "validado = '","true'," },
					{ "fechahora_validacion = '",DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"' " },
					{ "WHERE osiris_his_solicitudes_labrx.id_producto = '",id_produ+"' " },
					{ "AND osiris_his_solicitudes_labrx.id_tipo_admisiones2 = '","400' " },
					{ "AND osiris_his_solicitudes_labrx.folio_de_solicitud = '",numerosolicitud.Trim()+"';"}
				};
				paraobj = new object[] { entry_folio_laboratorio_res};
				new osiris.update_registro ("osiris_his_solicitudes_labrx", parametros, paraobj);
			}
		}

		void on_button_imprimir_clicked(object sender, EventArgs args)
		{
			if( idquimico != ""){
				new osiris.imprime_resultadolab(int.Parse(numerosolicitud),id_produ,400);
			}else{
				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
					MessageType.Info,ButtonsType.Close,"Seleccione un Quimico...");
				msgBoxError.Run ();	msgBoxError.Destroy();
			}
		}

		public bool Verificacion_de_valores()
		{
			bool valor;
			if(entry_fecha_solicitud_res.Text.Trim() == "" || entry_folio_laboratorio_res.Text.Trim() == "" ||
				entry_hora_solicitud_res.Text.Trim() == "" || idquimico.ToString().Trim() == "0"){
				valor = false;
				return valor;
			}else{
				valor = true;
				return valor;
			}
		}

		// cierra ventanas emergentes
		void on_cierraventanas_clicked (object sender, EventArgs args)
		{
			Widget win = (Widget) sender;
			win.Toplevel.Destroy();
		}
	}
}