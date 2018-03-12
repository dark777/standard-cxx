//
//  catalogo_empresas.cs
//
//  Author:
//       dolivares <arcangeldoc@gmail.com>
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
using Npgsql;
using System.Data;
using Glade;
using System.Collections;

namespace osiris
{
	public class catalogo_empresas
	{
		[Widget] Gtk.Window catalogo_instempres = null;
		[Widget] Gtk.CheckButton checkbutton_nueva_instempr = null;

		[Widget] Gtk.CheckButton checkbutton_activa_empresa = null;
		[Widget] Gtk.Entry entry_idinstempr = null;
		[Widget] Gtk.Entry entry_nombre_instempr = null;
		[Widget] Gtk.Entry entry_rfc_instempr = null;
		[Widget] Gtk.Button button_busca_empresa = null;
		[Widget] Gtk.Entry entry_direccion_empresa = null;
		[Widget] Gtk.Entry entry_numero_empresa = null;
		[Widget] Gtk.Entry entry_colonia_empresa = null;
		[Widget] Gtk.Entry entry_codpostal = null;
		[Widget] Gtk.Entry entry_telefono1 = null;
		[Widget] Gtk.Entry entry_telefono2 = null;
		[Widget] Gtk.Entry entry_paginaweb = null;
		[Widget] Gtk.Entry entry_nombrecontaco = null;
		[Widget] Gtk.Entry entry_emailcontacto = null;
		[Widget] Gtk.Entry entry_nombre_comecial = null;
		[Widget] Gtk.ComboBox combobox_estado = null;
		[Widget] Gtk.ComboBox combobox_municipios = null;
		[Widget] Gtk.CheckButton checkbutton_listaprecios = null;
		[Widget] Gtk.CheckButton checkbutton_aplicaiva = null;
		[Widget] Gtk.CheckButton checkbutton_sol_recetamed = null;
		[Widget] Gtk.ComboBox combobox_tipo_paciente = null;

		[Widget] Gtk.Button button_guardar = null;
		[Widget] Gtk.ToggleButton togglebutton_editar = null;
		[Widget] Gtk.Button button_salir = null;

		//tab
		[Widget] Gtk.TreeView treeview_lista_empresas = null;

		private TreeStore treeViewEngineEmpresas;
		ArrayList columns = new ArrayList ();

		string LoginEmpleado;
		string NomEmpleado;
		string AppEmpleado;
		string ApmEmpleado;
		string NomEmpleados;	
		string nombrebd;
		string connectionString;

		string estado = "";
		int idestado = 1;
		string municipios = "";
		int id_tipopaciente = 0;

		string[] args_args = {""};
		int[] args_id_array = {0,1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20};

		class_conexion conexion_a_DB = new class_conexion();
		class_public classpublic = new class_public();
		class_buscador classfind_data = new class_buscador();

		//Declaracion de ventana de error
		protected Gtk.Window MyWin;

		public catalogo_empresas (string LoginEmp, string NomEmpleado_, string AppEmpleado_, string ApmEmpleado_, string nombrebd_,int idempresa_)
		{
			LoginEmpleado = LoginEmp;
			NomEmpleado = NomEmpleado_;
			AppEmpleado = AppEmpleado_;
			ApmEmpleado = ApmEmpleado_;
			nombrebd = conexion_a_DB._nombrebd;
			connectionString = conexion_a_DB._url_servidor+conexion_a_DB._port_DB+conexion_a_DB._usuario_DB+conexion_a_DB._passwrd_user_DB;

			Glade.XML gxml = new Glade.XML (null, "catalogos.glade", "catalogo_instempres", null);
			gxml.Autoconnect (this);
	        // Muestra ventana de Glade
			catalogo_instempres.Show();

			llenado_lista_empresas ();

			button_busca_empresa.Clicked += new EventHandler (on_button_busca_empresa_clicked);
			button_guardar.Clicked += new EventHandler (on_button_guardar_clicked);
			checkbutton_nueva_instempr.Clicked += new EventHandler (on_checkbutton_nueva_instempr_clicked);
			button_salir.Clicked += new EventHandler(on_cierraventanas_clicked);
			togglebutton_editar.Clicked += new EventHandler (on_togglebutton_editar_clicked);

			llenado_combobox(1,"",combobox_tipo_paciente,"sql","SELECT * FROM osiris_his_tipo_pacientes WHERE id_tipo_paciente IN('102','200') AND activo_tipo_paciente = 'true' ORDER BY descripcion_tipo_paciente;","descripcion_tipo_paciente","id_tipo_paciente",args_args,args_id_array);
			llenado_combobox(1,"",combobox_estado,"sql","SELECT * FROM osiris_estados ORDER BY descripcion_estado;","descripcion_estado","id_estado",args_args,args_id_array);
			if (idempresa_ != 1) {
				llenado_informacion_empresa(idempresa_);
			}
		}

		void on_togglebutton_editar_clicked(object sender, EventArgs args)
		{
			activa_campos ((bool) togglebutton_editar.Active);
		}

		void llenado_combobox(int tipodellenado,string descrip_defaul,object obj,string sql_or_array,string query_SQL,string name_field_desc,string name_field_id,string[] args_array,int[] args_id_array)
		{			
			Gtk.ComboBox combobox_llenado = (Gtk.ComboBox) obj;
			//Gtk.ComboBox combobox_pos_neg = obj as Gtk.ComboBox;
			combobox_llenado.Clear();
			CellRendererText cell = new CellRendererText();
			combobox_llenado.PackStart(cell, true);
			combobox_llenado.AddAttribute(cell,"text",0);	        
			ListStore store = new ListStore( typeof (string),typeof (int));
			combobox_llenado.Model = store;			
			if ((int) tipodellenado == 1){
				store.AppendValues ((string) descrip_defaul,0);
			}			
			if(sql_or_array == "array"){			
				for (int colum_field = 0; colum_field < args_array.Length; colum_field++){
					store.AppendValues (args_array[colum_field],args_id_array[colum_field]);
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
						store.AppendValues ((string) lector[ name_field_desc ], (int) lector[ name_field_id]);
					}
				}catch (NpgsqlException ex){
					MessageDialog msgBoxError = new MessageDialog (MyWin,DialogFlags.DestroyWithParent,
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
				case "combobox_tipo_paciente":
					//tipopaciente = (string) onComboBoxChanged.Model.GetValue(iter,0);
					id_tipopaciente = (int) onComboBoxChanged.Model.GetValue(iter,1);
					break;
				case "combobox_estado":
					estado = (string) combobox_estado.Model.GetValue(iter,0);
					idestado = (int) combobox_estado.Model.GetValue(iter,1);
					llenado_combobox(0,"",combobox_municipios,"sql","SELECT * FROM osiris_municipios WHERE id_estado = '"+idestado.ToString()+"' "+
						"ORDER BY descripcion_municipio;","descripcion_municipio","id_municipio",args_args,args_id_array);
					break;
				case "combobox_municipios":
					municipios = (string) combobox_municipios.Model.GetValue(iter,0);
					//idmunicipio = (int) combobox_municipios.Model.GetValue(iter,1);					
					break;
				}
			}
		}

		void on_button_guardar_clicked(object sender, EventArgs args)
		{
			if (checkbutton_nueva_instempr.Active == true) {
				MessageDialog msgBox = new MessageDialog (MyWin,DialogFlags.Modal,MessageType.Question,
					ButtonsType.YesNo,"¿ Esta seguro(a) de GUARDAR este Convenio/Empresa al Catalogo ?");
				ResponseType miResultado = (ResponseType)msgBox.Run ();
				msgBox.Destroy();
				if (miResultado == ResponseType.Yes) {
					string[,] parametros = {
						{ "descripcion_empresa,", "'" + entry_nombre_instempr.Text.ToUpper () + "'," },
						{ "rfc_cliente_empresa,", "'" + entry_rfc_instempr.Text.ToUpper () + "'," },
						{ "direccion_empresa,", "'" + entry_direccion_empresa.Text.ToUpper ().Trim () + "'," },
						{ "numero_direccion_empresa,", "'" + entry_numero_empresa.Text.ToUpper ().Trim () + "'," },
						{ "colonia_empresa,", "'" + entry_colonia_empresa.Text.ToUpper ().Trim () + "'," },
						{ "estado_empresa,", "'" + estado + "'," },
						{ "municipio_empresa,", "'" + municipios + "'," },
						{ "codigo_postal_empresa,", "'" + entry_codpostal.Text.Trim () + "'," },
						{ "telefono1_empresa,", "'" + entry_telefono1.Text.Trim () + "'," },
						{ "telefono2_empresa,", "'" + entry_telefono2.Text.Trim () + "'," },
						{ "web_empresa,", "'" + entry_paginaweb.Text.Trim () + "'," },
						{ "nombre_asesor_sindical,", "'" + entry_nombrecontaco.Text.ToUpper().Trim () + "'," },
						{ "email_empresa,", "'" + entry_emailcontacto.Text.ToLower().Trim () + "'," },
						{ "lista_de_precio,", "'" + checkbutton_listaprecios.Active.ToString().Trim() + "'," },
						{ "servicio_medico_iva,", "'" + checkbutton_aplicaiva.Active.ToString().Trim() + "'," },
						{ "solicitud_receta_medica,", "'" + checkbutton_sol_recetamed.Active.ToString().Trim() + "'," },
						{ "empresa_activa,", "'" + checkbutton_activa_empresa.Active.ToString().Trim() + "'," },
						{ "id_tipo_documento,", "'" + "1" + "'," },	
						{ "nombre_comercial_empresa,", "'" + entry_nombre_comecial.Text.ToString ().Trim () + "'," },
						{ "id_tipo_paciente", "'" + id_tipopaciente.ToString ().Trim () + "' " }
					};
					object[] paraobj = { entry_idinstempr };
					new osiris.insert_registro ("osiris_empresas", parametros, paraobj);
				}
			} else {
				if (togglebutton_editar.Active == true) {
					MessageDialog msgBox = new MessageDialog (MyWin,DialogFlags.Modal,MessageType.Question,
						ButtonsType.YesNo,"¿ Esta seguro(a) de GUARDA los cambio esta Empresa al Catalogo ?");
					ResponseType miResultado = (ResponseType)msgBox.Run ();
					msgBox.Destroy();
					if(miResultado == ResponseType.Yes){
						string[,] parametros = {
							{ "descripcion_empresa = ", "'"+entry_nombre_instempr.Text.ToUpper()+"'," },
							{ "rfc_cliente_empresa = ", "'" + entry_rfc_instempr.Text.ToUpper() + "', " },
							{ "direccion_empresa = ", "'" + entry_direccion_empresa.Text.ToUpper ().Trim () + "', " },
							{ "numero_direccion_empresa = ", "'" + entry_numero_empresa.Text.ToUpper ().Trim () + "', " },
							{ "colonia_empresa = ", "'" + entry_colonia_empresa.Text.ToUpper ().Trim () + "', " },
							{ "estado_empresa = ", "'" + estado + "', " },
							{ "municipio_empresa = ", "'" + municipios + "', " },
							{ "codigo_postal_empresa = ", "'" + entry_codpostal.Text.Trim () + "', " },
							{ "telefono1_empresa = ", "'" + entry_telefono1.Text.Trim () + "', " },
							{ "telefono2_empresa = ", "'" + entry_telefono2.Text.Trim () + "', " },
							{ "web_empresa = ", "'" + entry_paginaweb.Text.Trim () + "', " },
							{ "nombre_asesor_sindical = ", "'" + entry_nombrecontaco.Text.ToUpper().Trim () + "', " },
							{ "email_empresa = ", "'" + entry_emailcontacto.Text.ToLower().Trim () + "', " },
							{ "lista_de_precio = ", "'" + checkbutton_listaprecios.Active.ToString().Trim() + "', " },
							{ "servicio_medico_iva = ", "'" + checkbutton_aplicaiva.Active.ToString().Trim() + "', " },
							{ "solicitud_receta_medica = ", "'" + checkbutton_sol_recetamed.Active.ToString().Trim() + "', " },
							{ "empresa_activa = ", "'" + checkbutton_activa_empresa.Active.ToString().Trim() + "', " },
							{ "lista_de_precio = ", "'" + checkbutton_listaprecios.Active.ToString().Trim() + "', " },
							{ "id_tipo_documento = ", "'" + "1" + "', " },
							{ "id_tipo_paciente = ", "'" + id_tipopaciente.ToString ().Trim () + "'," },
							{ "nombre_comercial_empresa = ", "'" + entry_nombre_comecial.Text.ToString ().Trim () + "' " },
							{ "WHERE id_empresa = '", entry_idinstempr.Text.Trim()+ "';" }
						};
						object[] paraobj = { entry_idinstempr };
						new osiris.update_registro ("osiris_empresas", parametros, paraobj);
					}					
				}
			}
		}

		void on_checkbutton_nueva_instempr_clicked(object sender, EventArgs args)
		{
			if (checkbutton_nueva_instempr.Active == true) {
				MessageDialog msgBox = new MessageDialog (MyWin,DialogFlags.Modal,MessageType.Question,
					ButtonsType.YesNo,"¿ Esta seguro(a) de AGREGAR otro Convenio/Empresa?");
				ResponseType miResultado = (ResponseType)msgBox.Run ();
				msgBox.Destroy();
				if (miResultado == ResponseType.Yes) {
					activa_campos (true);
				} else {
					activa_campos (false);
				}
			}
		}

		void on_button_busca_empresa_clicked(object sender, EventArgs args)
		{
			if (id_tipopaciente > 0) {
				object[] parametros_objetos = { catalogo_instempres };
				string[] parametros_sql = { "SELECT * FROM osiris_empresas WHERE id_tipo_paciente = '" + id_tipopaciente.ToString ().Trim () + "' " };
				string[] parametros_string = { };
				string[,] args_buscador1 = {
										{ "EMPRESA", "AND descripcion_empresa LIKE '%", "%' " },
										{ "ID EMPRESA", "AND id_empresa = '", "' " },
										{ "RFC", "AND rfc_cliente_empresa = '", "' " }
										};
				string[,] args_buscador2 = {
										{ "ID EMPRESA", "AND id_empresa = '", "' " },
										{ "EMPRESA", "AND descripcion_empresa LIKE '%", "%' " },
										{ "RFC", "AND rfc_cliente_empresa = '", "' " }
										};
				string[,] args_orderby = { { "", "" } };
				classfind_data.buscandor (parametros_objetos, parametros_sql, parametros_string, "find_empresa_catalogo", 0, args_buscador1, args_buscador2, args_orderby);
			} else {
				MessageDialog msgBoxError = new MessageDialog (MyWin,DialogFlags.DestroyWithParent,
					MessageType.Error,ButtonsType.Close, "Debe seleccionar un TIPO DE PACIENTE, verifique...");
				msgBoxError.Run ();				msgBoxError.Destroy();
			}
		}

		void llenado_informacion_empresa(int idempresa_)
		{
			NpgsqlConnection conexion; 
			conexion = new NpgsqlConnection (connectionString+nombrebd);
			// Verifica que la base de datos este conectada
			try{
				conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand ();
				comando.CommandText = "SELECT id_empresa,descripcion_empresa,numero_direccion_empresa,telefono1_empresa,telefono2_empresa," +
									"email_empresa,web_empresa,colonia_empresa,estado_empresa,rfc_cliente_empresa,nombre_asesor_sindical," +
									"id_empresa,osiris_empresas.lista_de_precio,osiris_empresas.id_tipo_paciente,porcentage_descuento,osiris_empresas.id_tipo_documento," +
									"servicio_medico_iva,codigo_postal_empresa,municipio_empresa,empresa_activa,solicitud_receta_medica "+
									"FROM osiris_empresas,osiris_his_tipo_pacientes " +
									"WHERE osiris_empresas.id_tipo_paciente = osiris_his_tipo_pacientes.id_tipo_paciente " +
									"AND id_empresa = '"+idempresa_.ToString().Trim()+"';";
				//Console.WriteLine(comando.CommandText.ToString());
				NpgsqlDataReader lector = comando.ExecuteReader ();
				if (lector.Read()) {
					checkbutton_activa_empresa.Active = (bool) lector["empresa_activa"];
					entry_nombre_instempr.Text = lector["descripcion_empresa"].ToString().Trim();
					entry_rfc_instempr.Text = lector["rfc_cliente_empresa"].ToString().Trim();
					entry_numero_empresa.Text = lector["numero_direccion_empresa"].ToString().Trim();
					entry_colonia_empresa.Text = lector["colonia_empresa"].ToString().Trim();

					checkbutton_activa_empresa.Active = (bool) lector["empresa_activa"];
					checkbutton_listaprecios.Active = (bool) lector["lista_de_precio"];
					checkbutton_aplicaiva.Active = (bool) lector["servicio_medico_iva"];
					checkbutton_sol_recetamed.Active = (bool) lector["solicitud_receta_medica"];

					llenado_combobox(1,lector["estado_empresa"].ToString().Trim(),combobox_estado,"sql","SELECT * FROM osiris_estados ORDER BY descripcion_estado;","descripcion_estado","id_estado",args_args,args_id_array);
					llenado_combobox(1,lector["municipio_empresa"].ToString().Trim(),combobox_municipios,"sql","SELECT * FROM osiris_municipios WHERE id_estado = '"+idestado.ToString()+"' "+
						"ORDER BY descripcion_municipio;","descripcion_municipio","id_municipio",args_args,args_id_array);

					activa_campos(false);
					
				}
			}catch (NpgsqlException ex){
				Console.WriteLine ("PostgresSQL error: {0}",ex.Message);
				MessageDialog msgBoxError = new MessageDialog (MyWin,DialogFlags.DestroyWithParent,
						MessageType.Info,ButtonsType.Close, "PostgresSQL error: {0} ",ex.Message);
				msgBoxError.Run ();		msgBoxError.Destroy();
			}
			conexion.Close ();
		}

		void activa_campos(bool activacampos_)
		{
			checkbutton_activa_empresa.Sensitive = activacampos_;
			entry_idinstempr.Sensitive = activacampos_;
			entry_nombre_instempr.Sensitive = activacampos_;
			entry_rfc_instempr.Sensitive = activacampos_;
			entry_direccion_empresa.Sensitive = activacampos_;
			entry_numero_empresa.Sensitive = activacampos_;
			entry_colonia_empresa.Sensitive = activacampos_;
			entry_codpostal.Sensitive = activacampos_;
			entry_telefono1.Sensitive = activacampos_;
			entry_telefono2.Sensitive = activacampos_;
			entry_paginaweb.Sensitive = activacampos_;
			entry_nombrecontaco.Sensitive = activacampos_;
			entry_emailcontacto.Sensitive = activacampos_;
			combobox_estado.Sensitive = activacampos_;
			combobox_municipios.Sensitive = activacampos_;
			checkbutton_listaprecios.Sensitive = activacampos_;
			checkbutton_aplicaiva.Sensitive = activacampos_;
			checkbutton_sol_recetamed.Sensitive = activacampos_;
			combobox_tipo_paciente.Sensitive = activacampos_;
			togglebutton_editar.Sensitive = activacampos_;
		}

		void llenado_lista_empresas ()
		{
			object[] parametros = { treeview_lista_empresas, treeViewEngineEmpresas };
			string[,] coltreeview = {
				{ "ID", "text" },
				{ "Nombre Empresa", "text" },
				{ "Direccion Empresa", "text" },
				{ "Numero", "text" },
				{ "Colonia", "text" },
				{ "Municipio", "text" },
				{ "Estado", "text" },
				{ "CP.", "text" },
			};
			crea_colums_treeview (parametros, coltreeview,"");

			NpgsqlConnection conexion; 
			conexion = new NpgsqlConnection (connectionString+nombrebd);
			// Verifica que la base de datos este conectada
			try{
				conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand ();
				comando.CommandText = "SELECT id_empresa,descripcion_empresa,telefono1_empresa,telefono2_empresa," +
					"email_empresa,web_empresa,colonia_empresa,estado_empresa,rfc_cliente_empresa,nombre_asesor_sindical," +
					"id_empresa,osiris_empresas.lista_de_precio,osiris_empresas.id_tipo_paciente,porcentage_descuento,osiris_empresas.id_tipo_documento," +
					"servicio_medico_iva,codigo_postal_empresa,municipio_empresa,empresa_activa "+
					"FROM osiris_empresas,osiris_his_tipo_pacientes " +
					"WHERE osiris_empresas.id_tipo_paciente = osiris_his_tipo_pacientes.id_tipo_paciente " +
					"ORDER BY osiris_empresas.id_empresa;";
				NpgsqlDataReader lector = comando.ExecuteReader ();
				//Console.WriteLine(comando.CommandText.ToString());
				while (lector.Read()) {
					treeViewEngineEmpresas.AppendValues(lector["id_empresa"].ToString().Trim(),
						lector["descripcion_empresa"].ToString().Trim());
				}
			}catch (NpgsqlException ex){
				Console.WriteLine ("PostgresSQL error: {0}",ex.Message);
				MessageDialog msgBoxError = new MessageDialog (MyWin,DialogFlags.DestroyWithParent,
					MessageType.Info,ButtonsType.Close, "PostgresSQL error: {0} ",ex.Message);
				msgBoxError.Run ();		msgBoxError.Destroy();
			}
			conexion.Close ();
		}

		void crea_colums_treeview(object[] args,string [,] args_colums,string tipo_reporte_)
		{
			Gtk.CellRendererText text;
			Gtk.CellRendererToggle toggle;
			Gtk.TreeViewColumn column0;
			// crea los objetos para el uso del treeview
			foreach (TreeViewColumn tvc in treeview_lista_empresas.Columns)
				treeview_lista_empresas.RemoveColumn(tvc);
			treeViewEngineEmpresas = new TreeStore(typeof(string),typeof(string),typeof(string),
													typeof(string),typeof(string),typeof(string),typeof(string),typeof(string),typeof(string),
													typeof(string),typeof(string),typeof(string),typeof(string),typeof(string),typeof(string),
													typeof(string),typeof(string),typeof(string),typeof(string),typeof(string),typeof(string),
													typeof(bool),typeof(bool));
			treeview_lista_empresas.Model = treeViewEngineEmpresas;
			treeview_lista_empresas.RulesHint = true;
			treeview_lista_empresas.Selection.Mode = SelectionMode.Multiple;
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
						treeview_lista_empresas.InsertColumn (column0, colum_field);					
					}
					if((string) args_colums [colum_field, 1] == "toogle"){
						toggle = new CellRendererToggle ();
						toggle.Xalign = 0.0f;
						columns.Add (toggle);
						column0 = new TreeViewColumn ((string) args_colums [colum_field, 0], toggle,"active",colum_field);
						column0.Sizing = TreeViewColumnSizing.Fixed;
						column0.Clickable = true;
						treeview_lista_empresas.InsertColumn (column0, colum_field);
					}
				}
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