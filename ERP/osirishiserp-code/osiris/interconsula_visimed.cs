//  
//  interconsula_visimed.cs
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
using Gtk;
using Gdk;
using Glade;
using Npgsql;
using System.Collections;
using System.Data;
using System.Collections.Generic;

namespace osiris
{
	public class interconsula_visimed
	{
		//Declarando ventana
		[Widget] Gtk.Window visita_medica  = null;
		[Widget] Gtk.Entry entry_numerotencion = null;
		[Widget] Gtk.Entry entry_pid_paciente = null;
		[Widget] Gtk.Entry entry_nombre_paciente = null;
		[Widget] Gtk.Entry entry_edad_paciente = null;		
		[Widget] Gtk.Entry entry_fecha_nacimiento = null;
		[Widget] Gtk.TextView textview_motivo_ingreso = null;
		[Widget] Gtk.Entry entry_sexo_paciente = null;
		[Widget] Gtk.Entry entry_id_doctor = null;
		[Widget] Gtk.Entry entry_doctor = null;
		[Widget] Gtk.Entry entry_especialidad_doctor = null;
		[Widget] Gtk.Entry entry_fechavistita = null;
		[Widget] Gtk.ComboBox combobox_nropaseqx = null;
		[Widget] Gtk.ComboBox combobox_hora_cita = null;
		[Widget] Gtk.ComboBox combobox_minutos_cita = null;
		[Widget] Gtk.ComboBox combobox_tipovisita = null;
		[Widget] Gtk.TreeView treeview_consulta_cargo = null;
		[Widget] Gtk.ComboBox combobox_tipo_anestesia = null;
		[Widget] Gtk.TreeView treeview_lista_vismed = null;
		[Widget] Gtk.TreeView treeview_cargos_vismed = null;

		[Widget] Gtk.Entry entry_id_medtratante = null;
		[Widget] Gtk.Entry entry_nom_medtratante = null;
		[Widget] Gtk.Button button_busca_doctor = null;
		[Widget] Gtk.Entry entry_espe_medtratante = null;
		[Widget] Gtk.Button button_grabar = null;
		[Widget] Gtk.Button button_quitar_concepcargo = null;

		[Widget] Gtk.Button button_busca_cargovismed = null;

		[Widget] Gtk.TextView textview_motivo_visita = null;
		
		[Widget] Gtk.CheckButton checkbutton_consulta = null;
		[Widget] Gtk.CheckButton checkbutton_consulta_urgencias = null;
		[Widget] Gtk.CheckButton checkbutton_visita_hospital = null;
		[Widget] Gtk.CheckButton checkbutton_cuidados_intermedio = null;
		[Widget] Gtk.CheckButton checkbutton_quirofano = null;
		[Widget] Gtk.CheckButton checkbutton_cuidados_intesivos = null;
		[Widget] Gtk.CheckButton checkbutton_cuidado_neonatal = null;
		[Widget] Gtk.CheckButton checkbutton_rehabiltacion = null;
		[Widget] Gtk.CheckButton checkbutton_anestesia = null;
		[Widget] Gtk.CheckButton checkbutton_ayudanteqx = null;
		[Widget] Gtk.CheckButton checkbutton_otro = null;

		[Widget] Gtk.Button button_rpt_notainterconsulta = null;
		[Widget] Gtk.Button button_rpt_visitamedica = null;

		[Widget] Gtk.Button button_salir = null;
		
		string connectionString;
		string nombrebd;
		int idsubalmacen;
		string LoginEmpleado;
		string idtipodepaciente;

		ListStore treeViewEngineConsCargo;
		ListStore treeViewEngineListVisita;
		ListStore treeViewEngineCargosVismed;

		TextBuffer buffer = new TextBuffer(null);
		TextIter insertIter;

		ArrayList columns = new ArrayList ();

		string hora_cita_qx = "";
		string minutos_cita_qx = "";
		string tipo_visita_medica = "";
		string nropaseqxurg = "0";
		string anestesia_aplicada = "";
		bool validapaseqxurg;
		string[] args_args = {""};
		string[] args_tipo_visita = {"","VISITA MEDICA","INTERCONSULTA","PROCED. MEDICO URGENCIAS","ANESTESIA","QUIROFANO","AYUDANTE-QUIROFANO"};
		string[] args_tipoanestesia = {"","INTRACAM","INTRACAM + SEDACION","RETROBULVAR","RETROBULVAR + SEDACION","TOPICA","NO TOPICA","EPIDURAL O RAQUEA","LOCAL + SEDACION","GENERAL","ENDOVENOSA","TOPICA + LOCAL","TOPICA + SEDACION"};
		int[] args_id_array = {0,1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20};

		int idtipointernamiento;

		class_conexion conexion_a_DB = new class_conexion();
		class_public classpublic = new class_public();
		class_buscador classfind_data = new class_buscador();
		
		protected Gtk.Window MyWinError;
		protected Gtk.Window MyWin;
		
		public interconsula_visimed (string LoginEmpleado_, string NomEmpleado_, string AppEmpleado_, string ApmEmpleado_,
		                      		string folioservicio_,string pidpaciente_,string nombrepaciente_,string edadpaciente_,string fechanac_,
			string sexopaciente_,int idsubalmacen_,string iddoctor_,string nombredoctor_,string especidoctor_,int idtipointernamiento_,bool validapaseqxurg_)
		{
			connectionString = conexion_a_DB._url_servidor+conexion_a_DB._port_DB+conexion_a_DB._usuario_DB+conexion_a_DB._passwrd_user_DB;
			nombrebd = conexion_a_DB._nombrebd;
			LoginEmpleado = LoginEmpleado_;
			idtipointernamiento = idtipointernamiento_; 
			validapaseqxurg = validapaseqxurg_;
			Glade.XML gxml = new Glade.XML (null, "hospitalizacion.glade", "visita_medica", null);
			gxml.Autoconnect                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                 (this);
	        visita_medica.Show();

			buffer = textview_motivo_ingreso.Buffer;
			classpublic.CreateTags(buffer);
			insertIter = buffer.StartIter;
			
			entry_pid_paciente.Text = pidpaciente_;
			entry_nombre_paciente.Text = nombrepaciente_;
			entry_edad_paciente.Text = edadpaciente_;
			entry_numerotencion.Text = folioservicio_;
			entry_fecha_nacimiento.Text = fechanac_;
			entry_sexo_paciente.Text = sexopaciente_;
			entry_fechavistita.Text = DateTime.Now.ToString("yyyy-MM-dd");

			entry_id_medtratante.Text = (string)classpublic.lee_registro_de_tabla ("osiris_erp_cobros_enca", "id_medico_tratante", "WHERE folio_de_servicio = '" + folioservicio_ + "'", "id_medico_tratante", "string");
			entry_nom_medtratante.Text = (string)classpublic.lee_registro_de_tabla ("osiris_his_medicos", "nombre_medico", "WHERE id_medico = '" + entry_id_medtratante.Text + "'", "nombre_medico", "string");
			entry_espe_medtratante.Text = (string)classpublic.lee_registro_de_tabla ("osiris_his_tipo_especialidad", "descripcion_especialidad", "WHERE id_especialidad = '" + 
											(string)classpublic.lee_registro_de_tabla ("osiris_his_medicos", "id_especialidad", "WHERE id_medico = '" + entry_id_medtratante.Text + "'", "id_especialidad", "string") + 
											"'", "descripcion_especialidad", "string");
			idtipodepaciente = (string)classpublic.lee_registro_de_tabla ("osiris_erp_cobros_enca,osiris_his_tipo_pacientes", "osiris_erp_cobros_enca.id_tipo_paciente", "WHERE osiris_erp_cobros_enca.id_tipo_paciente = osiris_his_tipo_pacientes.id_tipo_paciente AND folio_de_servicio = '" + folioservicio_ + "'", "osiris_erp_cobros_enca.id_tipo_paciente", "string");

			buffer.Insert (ref insertIter,(string) classpublic.lee_registro_de_tabla ("osiris_erp_movcargos","descripcion_diagnostico_movcargos", "WHERE folio_de_servicio = '" + folioservicio_ + "'", "descripcion_diagnostico_movcargos", "string"));

			llenado_combobox(0,"",combobox_tipovisita,"array","","","",args_tipo_visita,args_id_array,"");
			llenado_combobox(0,"",combobox_tipo_anestesia,"array","","","",args_tipoanestesia,args_id_array,"");
			llenado_combobox(1,"0",combobox_nropaseqx,"sql","SELECT to_char(id_secuencia,'99999999999') AS idsecuencia,id_secuencia,folio_de_servicio FROM osiris_erp_pases_qxurg WHERE folio_de_servicio = '"+folioservicio_+"' AND eliminado = 'false' ORDER BY id_secuencia;","idsecuencia","id_secuencia",args_args,args_id_array,"id_secuencia");

			entry_id_doctor.ModifyBase(StateType.Normal, new Gdk.Color(255,243,169)); // Color Amarillo
			entry_doctor.ModifyBase(StateType.Normal, new Gdk.Color(255,243,169)); // Color Amarillo
			entry_especialidad_doctor.ModifyBase(StateType.Normal, new Gdk.Color(255,243,169)); // Color Amarillo

			combobox_tipo_anestesia.Sensitive = false;

			if (entry_id_medtratante.Text != "1") {
				entry_id_doctor.Sensitive = true;
				entry_doctor.Sensitive = true;
				entry_especialidad_doctor.Sensitive = true;
				button_busca_doctor.Sensitive = true;
			} else {
				entry_id_doctor.Sensitive = false;
				entry_doctor.Sensitive = false;
				entry_especialidad_doctor.Sensitive = false;
				button_busca_doctor.Sensitive = false;
				button_grabar.Sensitive = false;
				checkbutton_consulta.Sensitive = false;
				checkbutton_consulta_urgencias.Sensitive = false;
				checkbutton_visita_hospital.Sensitive = false;
				checkbutton_cuidados_intermedio.Sensitive = false;
				checkbutton_quirofano.Sensitive = false;
				checkbutton_cuidados_intesivos.Sensitive = false;
				checkbutton_cuidado_neonatal.Sensitive = false;
				checkbutton_rehabiltacion.Sensitive = false;
				checkbutton_anestesia.Sensitive = false;
				checkbutton_ayudanteqx.Sensitive = false;
				checkbutton_otro.Sensitive = false;
				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
					MessageType.Error, 
					ButtonsType.Close, "Debe Asignar el MEDICO TRATANTE, para poder realizar una INTERCONSULTA o una VISITA MEDICA, verifique porfavor...");
				msgBoxError.Run ();						msgBoxError.Destroy();
			}
			llena_horas_citas();
			llenado_lista_visitas ();
			crea_treeview_cargosconsulta ();
			crea_treeview_listVisita ();
			llenado_lista_visitas ();
			crea_treeview_cargosvismed ();
			button_busca_doctor.Clicked += new EventHandler(on_button_busca_doctor_clicked);
			button_grabar.Clicked += new EventHandler(on_button_grabar_clicked);
			button_salir.Clicked += new EventHandler(on_cierraventanas_clicked);
			checkbutton_consulta.Clicked += new EventHandler(on_checkbutton_clicked);
			checkbutton_consulta_urgencias.Clicked += new EventHandler(on_checkbutton_clicked);
			checkbutton_visita_hospital.Clicked += new EventHandler(on_checkbutton_clicked);
			checkbutton_cuidados_intermedio.Clicked += new EventHandler(on_checkbutton_clicked);
			checkbutton_quirofano.Clicked += new EventHandler(on_checkbutton_clicked);
			checkbutton_cuidados_intesivos.Clicked += new EventHandler(on_checkbutton_clicked);
			checkbutton_cuidado_neonatal.Clicked += new EventHandler(on_checkbutton_clicked);
			checkbutton_rehabiltacion.Clicked += new EventHandler(on_checkbutton_clicked);
			checkbutton_anestesia.Clicked += new EventHandler(on_checkbutton_clicked);
			checkbutton_ayudanteqx.Clicked += new EventHandler(on_checkbutton_clicked);
			checkbutton_otro.Clicked += new EventHandler(on_checkbutton_clicked);
			button_busca_cargovismed.Clicked += new EventHandler(on_button_busca_cargovismed_clicked);
			button_quitar_concepcargo.Clicked += new EventHandler(on_button_quitar_concepcargo_clicked);
			button_rpt_notainterconsulta.Clicked += new EventHandler(on_button_rpt_notainterconsulta_clicked);
			button_rpt_visitamedica.Clicked += new EventHandler(on_button_rpt_visitamedica_clicked);
			button_grabar.Sensitive = false;
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
				case "combobox_tipovisita":
					tipo_visita_medica = (string) onComboBoxChanged.Model.GetValue (iter, 0);
					if ((string)onComboBoxChanged.Model.GetValue (iter, 1) == "ANESTESIA") {
						combobox_tipo_anestesia.Sensitive = true;
					} else {
						combobox_tipo_anestesia.Sensitive = false;
					}
					break;
				case "combobox_nropaseqx":
					nropaseqxurg = (string) onComboBoxChanged.Model.GetValue(iter,0);
					break;
				case "combobox_tipo_anestesia":
					anestesia_aplicada = (string) onComboBoxChanged.Model.GetValue (iter, 1);
					break;
				}
			}
		}

		void on_checkbutton_clicked(object sender, EventArgs args)
		{
			CheckButton onCheckBoxChanged = sender as CheckButton;
			if(sender == null){return;}			
			switch (onCheckBoxChanged.Name.ToString()){
			case "checkbutton_consulta":
				activa_botton_grabar_edit(onCheckBoxChanged.Active);
				break;
			case "checkbutton_consulta_urgencias":
				activa_botton_grabar_edit(onCheckBoxChanged.Active);
				break;
			case "checkbutton_visita_hospital":
				activa_botton_grabar_edit(onCheckBoxChanged.Active);
				break;
			case "checkbutton_cuidados_intermedio":
				activa_botton_grabar_edit(onCheckBoxChanged.Active);
				break;
			case "checkbutton_quirofano":
				activa_botton_grabar_edit(onCheckBoxChanged.Active);
				break;
			case "checkbutton_cuidados_intesivos":
				activa_botton_grabar_edit(onCheckBoxChanged.Active);
				break;
			case "checkbutton_cuidado_neonatal":
				activa_botton_grabar_edit(onCheckBoxChanged.Active);
				break;
			case "checkbutton_rehabiltacion":
				activa_botton_grabar_edit(onCheckBoxChanged.Active);
				break;
			case "checkbutton_anestesia":
				activa_botton_grabar_edit (onCheckBoxChanged.Active);
				combobox_tipo_anestesia.Sensitive = (bool)checkbutton_anestesia.Active;
				anestesia_aplicada = "";
				if (checkbutton_anestesia.Active == true) {
					llenado_combobox(0,"",combobox_tipo_anestesia,"array","","","",args_tipoanestesia,args_id_array,"");
				}
				break;
			case "checkbutton_ayudanteqx":
				activa_botton_grabar_edit(onCheckBoxChanged.Active);
				break;
			case "checkbutton_otro":
				activa_botton_grabar_edit(onCheckBoxChanged.Active);
				break;
			}
		}

		void activa_botton_grabar_edit(bool activa_los_checkbutton)
		{
			if((bool) checkbutton_consulta.Active == true
				|| (bool) checkbutton_consulta_urgencias.Active == true
				|| (bool) checkbutton_visita_hospital.Active == true
				|| (bool) checkbutton_cuidados_intermedio.Active == true
				|| (bool) checkbutton_quirofano.Active == true
				|| (bool) checkbutton_cuidados_intesivos.Active == true
				|| (bool) checkbutton_cuidado_neonatal.Active == true
				|| (bool) checkbutton_rehabiltacion.Active == true
				|| (bool) checkbutton_anestesia.Active == true
				|| (bool) checkbutton_ayudanteqx.Active == true
				|| (bool) checkbutton_otro.Active == true){
				button_grabar.Sensitive = true;
			}else{
				button_grabar.Sensitive = false;
			}
		}
		
		void on_button_busca_doctor_clicked(object sender, EventArgs args)
		{
			object[] parametros_objetos = {entry_id_doctor,entry_doctor,entry_especialidad_doctor,treeview_consulta_cargo,treeViewEngineConsCargo,entry_numerotencion};
			string[] parametros_sql = {"SELECT nombre_medico,id_medico,osiris_his_medicos.id_especialidad,descripcion_especialidad,id_productos_consulta " +
										"FROM osiris_his_medicos,osiris_his_tipo_especialidad " +
										"WHERE medico_activo = 'true' " +
										"AND osiris_his_medicos.id_especialidad = osiris_his_tipo_especialidad.id_especialidad "};
			string[] parametros_string = {entry_numerotencion.Text.Trim()};
			string[,] args_buscador1 = {{"NOMBRE O APELLIDOS","AND nombre_medico LIKE '%","%'"},
										{"ID MEDICO","AND id_medico = '","'"},
										{"ESPECIALIDAD","AND descripcion_especialidad LIKE '%","%'"}};
			string[,] args_buscador2 = {{"ID MEDICO","AND id_medico = '","'"},
										{"NOMBRE O APELLIDOS","AND nombre_medico LIKE '%","%'"},
										{"ESPECIALIDAD","AND descripcion_especialidad LIKE '%","%'"}};
			string[,] args_orderby = {{"",""}};

			classfind_data.buscandor(parametros_objetos,parametros_sql,parametros_string,"find_medico_visitamedica",0,args_buscador1,args_buscador2,args_orderby);
		}

		void on_button_busca_cargovismed_clicked(object sender, EventArgs args)
		{
			// find_busca_cargovismed
		}

		void llenado_lista_visitas()
		{
			crea_treeview_listVisita ();
			NpgsqlConnection conexion; 
			conexion = new NpgsqlConnection (connectionString+nombrebd);
			// Verifica que la base de datos este conectada
			try{
				conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand ();               	
				comando.CommandText = "SELECT fechahora_abono,id_abono " +
					"FROM osiris_erp_honorarios_medicos " +
					"WHERE folio_de_servicio = '"+entry_numerotencion.Text.Trim() +"';";
				//Console.WriteLine(comando.CommandText);
				NpgsqlDataReader lector = comando.ExecuteReader ();
				while(lector.Read()){
					treeViewEngineListVisita.AppendValues(lector["fechahora_abono"].ToString().Trim(),
						lector["id_abono"].ToString().Trim(),
						"",
						""
					);
				}
			}catch (NpgsqlException ex){
				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
					MessageType.Error,ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
				msgBoxError.Run ();				msgBoxError.Destroy();
			}
			conexion.Close ();
		}

		void crea_treeview_cargosconsulta()
		{
			object[] parametros = { treeview_consulta_cargo, treeViewEngineConsCargo };
			string[,] coltreeview = {
				{ "Selec.", "toogle" },
				{ "Cant.", "text" },
				{ "ID Producto", "text" },
				{ "Descripcion", "text" },
				{ "", "text" },
				{ "", "text" },
				{ "", "text" },
				{ "", "text" },
				{ "", "text" },
				{ "", "text" },
				{ "", "text" },
				{ "", "text" }
			};
			crea_colums_treeview (parametros, coltreeview,"treeview_consulta_cargo");
		}

		void crea_treeview_listVisita ()
		{
			object[] parametros = { treeview_lista_vismed, treeViewEngineListVisita };
			string[,] coltreeview = {
				{ "Fecha", "text" },
				{ "Nro. Visita", "text" },
				{ "Nombre Doctor", "text" },
				{ "Especialidad", "text" }

			};
			crea_colums_treeview (parametros, coltreeview,"treeview_lista_vismed");
		}

		void crea_treeview_cargosvismed ()
		{
			object[] parametros = { treeview_cargos_vismed, treeViewEngineCargosVismed };
			string[,] coltreeview = {
				{ "Cant.", "text" },
				{ "ID Producto", "text" },
				{ "Descripcion", "text" },
				{ "ID", "text" }
			};
			crea_colums_treeview (parametros, coltreeview,"treeview_cargos_vismed");
		}

		void crea_colums_treeview(object[] args,string [,] args_colums,string nombre_treeview_)
		{
			//var columns_renderertext = new List<Gtk.CellRendererText>();
			//var columns_renderertoggle = new List<Gtk.CellRendererToggle>();
			Gtk.CellRendererText text;
			Gtk.CellRendererToggle toggle;
			Gtk.TreeView treeviewobject = null;
			Gtk.ListStore treeViewEngine = null;
			ArrayList columns = new ArrayList ();

			treeviewobject = (object) args[0] as Gtk.TreeView;
			treeViewEngine = (object) args[1] as Gtk.ListStore;

			var columns_treeview = new List<TreeViewColumn>();

			foreach (TreeViewColumn tvc in treeviewobject.Columns)
				treeviewobject.RemoveColumn(tvc);

			Type[] t = new Type[args_colums.GetUpperBound (0)+1];
			for (int colum_field = 0; colum_field <= args_colums.GetUpperBound (0); colum_field++) {
				if ((string)args_colums [colum_field, 1] == "text") {
					t [colum_field] = typeof(string);
				}
				if ((string)args_colums [colum_field, 1] == "toogle") {
					t [colum_field] = typeof(bool);
				}
			}
			treeViewEngine = new Gtk.ListStore(t);
			//treeViewEngine = liststore_;

			treeviewobject.Model = treeViewEngine;
			treeviewobject.RulesHint = true;
			//treeviewobject.Selection.Mode = SelectionMode.Multiple;
			if (args_colums.GetUpperBound (0) >= 0) {
				for (int colum_field = 0; colum_field <= args_colums.GetUpperBound (0); colum_field++) {
					if ((string)args_colums [colum_field, 0] != "") {
						if ((string)args_colums [colum_field, 1] == "text") {
							// column for holiday names
							text = new CellRendererText ();
							text.Xalign = 0.0f;
							columns.Add (text);
							columns_treeview.Add (new TreeViewColumn ((string)args_colums [colum_field, 0], text, "text", colum_field));
							//columns_renderertext.Add (new CellRendererText ());	
							//columns_renderertext [colum_field+1].Xalign = 0.0f;
							//columns.Add (columns_renderertext [colum_field+1]);
							//columns_treeview.Add (new TreeViewColumn ((string)args_colums [colum_field, 0], columns_renderertext [colum_field], "text", colum_field));
							columns_treeview [colum_field].Resizable = true;
							treeviewobject.InsertColumn (columns_treeview [colum_field], colum_field);					
						}
						if ((string)args_colums [colum_field, 1] == "toogle") {
							// column for holiday names
							toggle = new CellRendererToggle ();
							toggle.Xalign = 0.0f;
							columns.Add (toggle);
							columns_treeview.Add (new TreeViewColumn ((string)args_colums [colum_field, 0], toggle, "active", colum_field));
							//columns_renderertoggle.Add(new CellRendererToggle());
							//columns_renderertoggle[colum_field].Xalign = 0.0f;
							//columns.Add (columns_renderertoggle[colum_field]);
							//columns_treeview.Add (new TreeViewColumn ((string)args_colums [colum_field, 0], columns_renderertoggle[colum_field], "active", colum_field));
							columns_treeview [colum_field].Resizable = true;
							treeviewobject.InsertColumn (columns_treeview [colum_field], colum_field);
							toggle.Toggled += new ToggledHandler (selecciona_fila);
						}
					}
				}
				if (nombre_treeview_ == "treeview_consulta_cargo"){
					//treeviewobject.RowActivated += on_button_ligar_erp_clicked;
					treeview_consulta_cargo = treeviewobject;
					treeViewEngineConsCargo = treeViewEngine;
				}
				if (nombre_treeview_ == "treeview_lista_vismed") {
					treeview_lista_vismed = treeviewobject;
					treeViewEngineListVisita = treeViewEngine;
				}
				if (nombre_treeview_ == "treeview_cargos_vismed") {
					treeview_cargos_vismed = treeviewobject;
					treeViewEngineCargosVismed = treeViewEngine;
				}
			}
		}

		// Cuando seleccion el treeview de cargos extras para cargar los productos  
		void selecciona_fila(object sender, ToggledArgs args)
		{
			TreeIter iter;
			if (treeview_consulta_cargo.Model.GetIter (out iter, new TreePath (args.Path))) {
				bool old = (bool)treeview_consulta_cargo.Model.GetValue (iter, 0);
				treeview_consulta_cargo.Model.SetValue (iter, 0, !old);
			}
		}

		void llena_horas_citas()
		{
			combobox_hora_cita.Clear ();
			CellRendererText cell2 = new CellRendererText ();
			combobox_hora_cita.PackStart (cell2, true);
			combobox_hora_cita.AddAttribute (cell2, "text", 0);

			ListStore store2 = new ListStore (typeof(string), typeof(int));
			combobox_hora_cita.Model = store2;
			for (int i = (int)classpublic.horario_cita_inicio; i < (int)classpublic.horario_24_horas + 1; i++) {				
				store2.AppendValues ((string)i.ToString ("00").Trim ());
			}
			combobox_hora_cita.Changed += new EventHandler (onComboBoxChanged_hora_minutos_cita);

			combobox_minutos_cita.Clear ();
			CellRendererText cell3 = new CellRendererText ();
			combobox_minutos_cita.PackStart (cell3, true);
			combobox_minutos_cita.AddAttribute (cell3, "text", 0);

			ListStore store3 = new ListStore (typeof(string), typeof(int));
			combobox_minutos_cita.Model = store3;

			for (int i = (int)0; i < 60; i = i + (int) classpublic.intervalo_minutos) {				
				store3.AppendValues ((string)i.ToString ("00").Trim ());
			}
			combobox_minutos_cita.Changed += new EventHandler (onComboBoxChanged_hora_minutos_cita);
		}

		void onComboBoxChanged_hora_minutos_cita(object sender, EventArgs args)
		{
			//Gtk.ComboBox hora_minutos_cita = (Gtk.ComboBox) sender;
			Gtk.ComboBox hora_minutos_cita = sender as Gtk.ComboBox;			
			if (sender == null){
				return;
			}
			TreeIter iter;
			if (hora_minutos_cita.GetActiveIter (out iter)){
				if(hora_minutos_cita.Name.ToString() == "combobox_hora_cita"){				
					hora_cita_qx = (string) hora_minutos_cita.Model.GetValue(iter,0);
				}			
				if(hora_minutos_cita.Name.ToString() == "combobox_minutos_cita"){
					minutos_cita_qx = (string) hora_minutos_cita.Model.GetValue(iter,0);
				}
			}
		}

		void on_button_grabar_clicked(object sender, EventArgs args)
		{
			string[,] parametros;
			object[] paraobj;
			float valoriva = float.Parse(classpublic.ivaparaaplicar);
			// validaciones
			//
			//nropaseqxurg

			if (nropaseqxurg != "0") {
				if (entry_id_doctor.Text != "") {
					if (hora_cita_qx != "" && minutos_cita_qx != "") { 
						if (tipo_visita_medica != "") {
							if (textview_motivo_visita.Buffer.Text.ToUpper () != "") {
								MessageDialog msgBox = new MessageDialog (MyWin, DialogFlags.Modal, MessageType.Question, ButtonsType.YesNo, "¿ La Solicitud de Interconsulta o Visita Medica  ?");
								ResponseType miResultado = (ResponseType)msgBox.Run ();
								msgBox.Destroy ();
								if (miResultado == ResponseType.Yes) {
									string motivosprede = "";
									// actualizar los registros en los pases de servicios medicos URG/QX/
									switch (tipo_visita_medica) {
									case "VISITA MEDICA":
										motivosprede = motivosprede + "/"+tipo_visita_medica;
										parametros = new string[,] {
											{ "id_medico_vismed = ", "'" + entry_id_doctor.Text.ToString ().Trim () + "'," },
											{ "nombre_medico_vismed = ", "'" + entry_doctor.Text.Trim () + "' " },
											{ "WHERE id_secuencia = '", nropaseqxurg.ToString ().Trim () + "';" }
										};
										paraobj = new [] { entry_numerotencion };
										new osiris.update_registro ("osiris_erp_pases_qxurg", parametros, paraobj);
										break;
									case "INTERCONSULTA":
										motivosprede = motivosprede + "/"+tipo_visita_medica;
										parametros = new string[,] {
											{ "id_medico_vismed = ", "'" + entry_id_doctor.Text.ToString ().Trim () + "'," },
											{ "nombre_medico_vismed = ", "'" + entry_doctor.Text.Trim () + "' " },
											{ "WHERE id_secuencia = '", nropaseqxurg.ToString ().Trim () + "';" }
										};
										paraobj = new [] { entry_numerotencion };
										new osiris.update_registro ("osiris_erp_pases_qxurg", parametros, paraobj);										
										break;
									case "PROCED. MEDICO URGENCIAS":
										motivosprede = motivosprede + "/"+tipo_visita_medica;
										parametros = new string[,] {
											{ "id_cirujano = ", "'" + entry_id_doctor.Text.ToString ().Trim () + "'," },
											{ "nombre_cirujano = ", "'" + entry_doctor.Text.Trim () + "' " },
											{ "WHERE id_secuencia = '", nropaseqxurg.ToString ().Trim () + "';" }
										};
										paraobj = new [] { entry_numerotencion };
										new osiris.update_registro ("osiris_erp_pases_qxurg", parametros, paraobj);
										break;
									case "ANESTESIA":
										motivosprede = motivosprede + "/"+tipo_visita_medica;
										parametros = new string[,] {
											{ "id_anestesiologo = ", "'" + entry_id_doctor.Text.ToString ().Trim () + "'," },
											{ "nombre_anestesiologo = ", "'" + entry_doctor.Text.Trim () + "' " },
											{ "WHERE id_secuencia = '", nropaseqxurg.ToString ().Trim () + "';" }
										};
										paraobj = new [] { entry_numerotencion };
										new osiris.update_registro ("osiris_erp_pases_qxurg", parametros, paraobj);
										break;
									case "QUIROFANO":
										motivosprede = motivosprede + "/"+tipo_visita_medica;
										parametros = new string[,] {
											{ "id_cirujano = ", "'" + entry_id_doctor.Text.ToString ().Trim () + "'," },
											{ "nombre_cirujano = ", "'" + entry_doctor.Text.Trim () + "' " },
											{ "WHERE id_secuencia = '", nropaseqxurg.ToString ().Trim () + "';" }
										};
										paraobj = new [] { entry_numerotencion };
										new osiris.update_registro ("osiris_erp_pases_qxurg", parametros, paraobj);
										break;
									case "AYUDANTE-QUIROFANO":
										motivosprede = motivosprede + "/"+tipo_visita_medica;
										parametros = new string[,] {
											{ "id_ayudante = ", "'" + entry_id_doctor.Text.ToString ().Trim () + "'," },
											{ "nombre_ayudante = ", "'" + entry_doctor.Text.Trim () + "' " },
											{ "WHERE id_secuencia = '", nropaseqxurg.ToString ().Trim () + "';" }
										};
										paraobj = new [] { entry_numerotencion };
										new osiris.update_registro ("osiris_erp_pases_qxurg", parametros, paraobj);
										break;
									}

									// llave principal nropaseqxurg el cual se genera en la caja				
									parametros = new string[,] {
										{ "id_medico,", "'" + entry_id_doctor.Text.ToString ().Trim () + "'," },
										{ "monto_del_abono,", "'" + "0" + "'," },
										{ "folio_de_servicio,", "'" + entry_numerotencion.Text.Trim () + "'," },
										{ "pid_paciente,", "'" + entry_pid_paciente.Text.Trim () + "'," },
										{ "id_paseqxurg,", "'" + nropaseqxurg.ToString ().Trim () + "'," },
										{ "fechahora_abono,", "'" + DateTime.Now.ToString ("yyyy-MM-dd HH:mm:ss") + "'," },
										{ "fecha_visita,", "'" + DateTime.Now.ToString ("yyyy-MM-dd") + "'," },
										{ "hora_visita,", "'" + hora_cita_qx + ":" + minutos_cita_qx + "'," },
										{ "id_quien_abono,", "'" + LoginEmpleado + "'," },
										{ "motivo_visita,", "'" + textview_motivo_visita.Buffer.Text.ToUpper () + "'," },
										{ "id_medico_tratante,", "'" + entry_id_medtratante.Text.Trim () + "'," },
										{ "id_tipo_paciente,", "'" + idtipodepaciente + "'," },
										{ "tipo_de_visita,", "'" + tipo_visita_medica + "'," },
										{ "motivos_predeterminado,", "'" + motivosprede + "'," },
										{ "tipo_de_anestesia,", "'" + anestesia_aplicada + "'," }, 
										{ "descripcion_diagnostico_movcargos,",	"'" + textview_motivo_ingreso.Buffer.Text.ToString ().ToUpper () + "', "},
										{ "tipvisita_consulta,", "'" + checkbutton_consulta.Active.ToString () + "'," },
										{ "tipvisita_consulta_urg,", "'" + checkbutton_consulta_urgencias.Active.ToString () + "'," },
										{ "tipvisita_hospital,", "'" + checkbutton_visita_hospital.Active.ToString () + "'," },
										{ "tipvisita_cuidados_intermedio,", "'" + checkbutton_visita_hospital.Active.ToString () + "'," },
										{ "tipvisita_quirofano,", "'" + checkbutton_visita_hospital.Active.ToString () + "'," },
										{ "tipvisita_cuidados_intensivo,", "'" + checkbutton_visita_hospital.Active.ToString () + "'," },
										{ "tipvisita_cuidado_neonatal,", "'" + checkbutton_visita_hospital.Active.ToString () + "'," },
										{ "tipvisita_rehabilitacion,", "'" + checkbutton_visita_hospital.Active.ToString () + "'," },
										{ "tipvisita_anestesia,", "'" + checkbutton_visita_hospital.Active.ToString () + "'," },
										{ "tipvisita_ayudante_quirofano", "'" + checkbutton_visita_hospital.Active.ToString () + "'" }
									};
									paraobj = new object[] { entry_numerotencion };
									new osiris.insert_registro ("osiris_erp_honorarios_medicos", parametros, paraobj);


									// Realiza el cargo en detalle de conceptos de los servicios realizados
									TreeIter iter;
									if (treeViewEngineConsCargo.GetIterFirst (out iter)){
										//Console.WriteLine (treeview_consulta_cargo.Model.GetValue(iter,1).ToString().Trim());
										if ((bool)treeview_consulta_cargo.Model.GetValue (iter, 0) == true) {
											parametros = new string[,] {
												{ "id_paseqxurg,", "'" + nropaseqxurg.ToString ().Trim () + "'," },
												{ "id_producto,", "'" + treeview_consulta_cargo.Model.GetValue (iter, 2).ToString ().Trim () + "'," },
												{ "folio_de_servicio,", "'" + entry_numerotencion.Text.Trim () + "'," },
												{ "pid_paciente,", "'" + entry_pid_paciente.Text.Trim () + "'," },
												{ "cantidad_aplicada,", "'" + treeview_consulta_cargo.Model.GetValue (iter, 1).ToString ().Trim () + "'," },
												{ "id_tipo_admisiones,", "'" + idtipointernamiento.ToString ().Trim () + "'," },
												{ "precio_producto,", "'" + treeview_consulta_cargo.Model.GetValue (iter, 4).ToString ().Trim () + "'," },
												{ "iva_producto,", "'" + treeview_consulta_cargo.Model.GetValue (iter, 5).ToString ().Trim () + "'," },
												{ "precio_costo_unitario,", "'" + treeview_consulta_cargo.Model.GetValue (iter, 9).ToString ().Trim () + "'," },
												{ "porcentage_utilidad,", "'" + treeview_consulta_cargo.Model.GetValue (iter, 10).ToString ().Trim () + "'," },
												{ "porcentage_descuento,", "'" + treeview_consulta_cargo.Model.GetValue (iter, 7).ToString ().Trim () + "'," },
												{ "id_empleado,", "'" + LoginEmpleado + "'," },
												{ "fechahora_creacion,", "'" + DateTime.Now.ToString ("yyyy-MM-dd HH:mm:ss") + "'," },
												{ "porcentage_iva,", "'" + valoriva.ToString ().Trim () + "'," },
												{ "folio_interno_dep,", "'" + nropaseqxurg.ToString ().Trim () + "'," },
												{ "precio_costo", "'" + treeview_consulta_cargo.Model.GetValue (iter, 11).ToString ().Trim () + "' " }
											};
											paraobj = new object[] { entry_numerotencion };
											new osiris.insert_registro ("osiris_erp_cobros_deta", parametros, paraobj);
										}
										while (treeViewEngineConsCargo.IterNext(ref iter)){
											if ((bool)treeview_consulta_cargo.Model.GetValue (iter, 0) == true) {
												parametros = new string[,] {
													{ "id_paseqxurg,", "'" + nropaseqxurg.ToString ().Trim () + "'," },
													{ "id_producto,", "'" + treeview_consulta_cargo.Model.GetValue (iter, 2).ToString ().Trim () + "'," },
													{ "folio_de_servicio,", "'" + entry_numerotencion.Text.Trim () + "'," },
													{ "pid_paciente,", "'" + entry_pid_paciente.Text.Trim () + "'," },
													{ "cantidad_aplicada,", "'" + treeview_consulta_cargo.Model.GetValue (iter, 1).ToString ().Trim () + "'," },
													{ "id_tipo_admisiones,", "'" + idtipointernamiento.ToString ().Trim () + "'," },
													{ "precio_producto,", "'" + treeview_consulta_cargo.Model.GetValue (iter, 4).ToString ().Trim () + "'," },
													{ "iva_producto,", "'" + treeview_consulta_cargo.Model.GetValue (iter, 5).ToString ().Trim () + "'," }, { "precio_costo_unitario,", "'" + treeview_consulta_cargo.Model.GetValue (iter, 9).ToString ().Trim () + "'," },
												{ "porcentage_utilidad,", "'" + treeview_consulta_cargo.Model.GetValue (iter, 10).ToString ().Trim () + "'," },
												{ "porcentage_descuento,", "'" + treeview_consulta_cargo.Model.GetValue (iter, 7).ToString ().Trim () + "'," },
												{ "id_empleado,", "'" + LoginEmpleado + "'," },
												{ "fechahora_creacion,", "'" + DateTime.Now.ToString ("yyyy-MM-dd HH:mm:ss") + "'," },
												{ "porcentage_iva,", "'" + valoriva.ToString ().Trim () + "'," },
												{ "folio_interno_dep,", "'" + nropaseqxurg.ToString ().Trim () + "'," },
												{ "precio_costo", "'" + treeview_consulta_cargo.Model.GetValue (iter, 11).ToString ().Trim () + "' " }
											};
											paraobj = new object[] { entry_numerotencion };
												new osiris.insert_registro ("osiris_erp_cobros_deta", parametros, paraobj);
											}
										}
									}													
								}
							} else {
								MessageDialog msgBoxError = new MessageDialog (MyWinError, DialogFlags.DestroyWithParent,
									MessageType.Error, ButtonsType.Close, "Debe ingresar un MOTIVO de la Visita, verifique...");
								msgBoxError.Run ();
								msgBoxError.Destroy ();
							}
						} else {
							MessageDialog msgBoxError = new MessageDialog (MyWinError, DialogFlags.DestroyWithParent,
								                           MessageType.Error, ButtonsType.Close, "Debe elegir el TIPO DE VISITA, verifique...");
							msgBoxError.Run ();
							msgBoxError.Destroy ();
						}
					} else {
						MessageDialog msgBoxError = new MessageDialog (MyWinError, DialogFlags.DestroyWithParent,
							                           MessageType.Error, ButtonsType.Close, "Seleccione el HORARIO DE VISITA, verifique...");
						msgBoxError.Run ();
						msgBoxError.Destroy ();
					}
				} else {
					MessageDialog msgBoxError = new MessageDialog (MyWinError, DialogFlags.DestroyWithParent,
						MessageType.Error, ButtonsType.Close, "Debe Seleccionar el MEDICO QUE REALIZO LA VISITA, verifique...");
					msgBoxError.Run ();
					msgBoxError.Destroy ();
				}
			} else {
				MessageDialog msgBoxError = new MessageDialog (MyWinError, DialogFlags.DestroyWithParent,
					MessageType.Error, ButtonsType.Close, "Debe Seleccionar UN PASE de Servicio Medico, verifique...");
				msgBoxError.Run ();
				msgBoxError.Destroy ();
			}
		}

		void on_button_quitar_concepcargo_clicked(object sender, EventArgs args)
		{
			TreeIter iterSelected;
			if (treeViewEngineConsCargo.GetIterFirst (out iterSelected)) {
				MessageDialog msgBox = new MessageDialog (MyWin,DialogFlags.Modal,MessageType.Question,ButtonsType.YesNo,"¿ Esta seguro de QUITAR el CARGO  ?");
				ResponseType miResultado = (ResponseType) msgBox.Run ();
				msgBox.Destroy();
				if (miResultado == ResponseType.Yes) {
					treeViewEngineConsCargo.Remove (ref iterSelected);
				}
			}
		}

		void on_button_rpt_notainterconsulta_clicked(object sender, EventArgs args)
		{
			TreeIter iterSelected;
			if (treeViewEngineListVisita.GetIterFirst (out iterSelected)) {
				new osiris.rpt_hoja_de_interconsulta ("1");
			}
		}

		void on_button_rpt_visitamedica_clicked(object sender, EventArgs args)
		{
			TreeIter iterSelected;
			if (treeViewEngineListVisita.GetIterFirst (out iterSelected)) {
				new osiris.rpt_control_visita_medica ("1",1);
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