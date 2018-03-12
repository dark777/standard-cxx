///////////////////////////////////////////////////////////
// project created on 02/06/2012
// Sistema Hospitalario OSIRIS
// Monterrey - Mexico
//
// Autor    	: Ing. Daniel Olivares
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
/////////////////////////////////////////////////////////
using System;
using Npgsql;
using System.Data;
using Gtk;
using Glade;

using System.Collections;
using OfficeOpenXml;
using System.IO;
using System.Reflection;
using System.Collections.Generic;
using System.Globalization;

namespace osiris
{
	public class tarjeta_medica
	{
		// Boton general para salir de las ventanas
		// Todas la ventanas en glade este boton debe estra declarado identico
		[Widget] Gtk.Button button_salir = null;
		
		[Widget] Gtk.Window tarjeta_medica_osiris = null;
		[Widget] Gtk.Entry entry_busqueda1 = null;
		[Widget] Gtk.Entry entry_busqueda2 = null;
		[Widget] Gtk.ComboBox combobox_busqueda1 = null;
		[Widget] Gtk.ComboBox combobox_busqueda2 = null;
		[Widget] Gtk.CheckButton checkbutton_filtro2 = null;
		[Widget] Gtk.Button button_admitir_beneficiario = null;
		[Widget] Gtk.TreeView treeview_lista_beneficiarios = null;
		ListStore treeViewEngineListbenef;
		Gtk.TreeModelFilter filter;

		// tab cargar csv
		[Widget] Gtk.Button button_cargar_archivo = null;
		[Widget] Gtk.TreeView treeview_carga_csv = null;
		[Widget] Gtk.ComboBox combobox_tipo_archivo = null;
		[Widget] Gtk.Button button_cargar_afiliados = null;
		ListStore treeViewEngineCargacsv;
		
		string LoginEmpleado;
		string NomEmpleado = "";
		string AppEmpleado = "";
		string ApmEmpleado = "";
		string connectionString;
		string nombrebd;

		string tipodebusqueda_1 = "";

		ArrayList columns = new ArrayList ();
		
		string[] args_args = {""};
		string[] args_tipobusqueda = {"","NOMINA","NOMBRES O APELLIDOS"};
		string[] args_tipoarchivocvs = {"","NOMINA","BAJAS NOMINA","EVENTUALES","BAJA EVENTUALES"};
		int[] args_id_array = {0,1,2,3,4,5,6,7,8,9,10,11,12,13,14};
			
		class_conexion conexion_a_DB = new class_conexion();
		class_public classpublic = new class_public();
		insert_registro insertreg;
		
		//Declaracion de ventana de error
		protected Gtk.Window MyWinError;
		protected Gtk.Window MyWin;
				
		public tarjeta_medica ()
		{
			connectionString = conexion_a_DB._url_servidor+conexion_a_DB._port_DB+conexion_a_DB._usuario_DB+conexion_a_DB._passwrd_user_DB;
			nombrebd = conexion_a_DB._nombrebd;
			
			Glade.XML gxml = new Glade.XML (null, "registro_admision.glade", "tarjeta_medica_osiris", null);
			gxml.Autoconnect                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                 (this);
	        tarjeta_medica_osiris.Show();

			entry_busqueda1.Changed += OnFilterEntryTextChanged;
			button_admitir_beneficiario.Clicked += new EventHandler(on_button_admitir_beneficiario_clicked);
			button_cargar_archivo.Clicked += new EventHandler(on_button_cargar_archivo_clicked);
			button_cargar_afiliados.Clicked += new EventHandler(on_button_cargar_afiliados_clicked);
			button_salir.Clicked += new EventHandler(on_cierraventanas_clicked);

			crea_treeview_lista_beneficiarios ();
			llenado_treeview_beneficiarios ();
			crea_treeview_cargacsv ();
			llenado_combobox(1,"NOMINA",combobox_busqueda1,"array","","","",args_tipobusqueda,args_id_array,"");
			llenado_combobox(0,"",combobox_tipo_archivo,"array","","","",args_tipoarchivocvs,args_id_array,"");
			tipodebusqueda_1 = "NOMINA";



			filter = new Gtk.TreeModelFilter (treeViewEngineListbenef, null);
			filter.VisibleFunc = new Gtk.TreeModelFilterVisibleFunc (FilterTree_nombre);
			treeview_lista_beneficiarios.Model = filter;
		}

		void llenado_treeview_beneficiarios()
		{
			int secuencia_registros = 1;
			NpgsqlConnection conexion;
			conexion = new NpgsqlConnection (connectionString+nombrebd);
			// Verifica que la base de datos este conectada
			try{
				conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand ();	          	
				comando.CommandText = "SELECT * FROM osiris_erp_afiliados_enca; ;";
				//Console.WriteLine(comando.CommandText);
				NpgsqlDataReader lector = comando.ExecuteReader ();
				while (lector.Read()){
					treeViewEngineListbenef.AppendValues(secuencia_registros.ToString().Trim(),
					                                     lector["numero_nomina_afiliado"].ToString().Trim(),
					                                    lector["nombre_completo"].ToString().Trim(),
														lector["fecha_nacimiento_afiliado"].ToString().Trim()
					);
					secuencia_registros++;
				}
			}catch (NpgsqlException ex){
				Console.WriteLine ("PostgresSQL error: {0}",ex.Message);
				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
					MessageType.Info,ButtonsType.Close, "PostgresSQL error: {0} ",ex.Message);
				msgBoxError.Run ();					msgBoxError.Destroy();
			}
			conexion.Close ();
				
		}

		private void OnFilterEntryTextChanged (object o, System.EventArgs args)
		{
			filter.Refilter ();	
		}

		private bool FilterTree_nombre (Gtk.TreeModel model, Gtk.TreeIter iter)
		{
			string contactname = model.GetValue (iter, 2).ToString ();
			if (entry_busqueda1.Text.ToUpper() == ""){
				return true;
			}			
			if (contactname.IndexOf (entry_busqueda1.Text.ToUpper()) > -1){
				return true;
			}			
			if (model.IterHasChild(iter)) {
				bool filerBool = false;
				investigateChildNodes_nombre(model, iter); //method checking if currently investigated node has any child fulfilling filter contitions
				return filerBool;
			}
			return false;
		}

		private void investigateChildNodes_nombre(TreeModel model, TreeIter iter) 
		{       
			TreeIter childIter;
			model.IterChildren(out childIter, iter); 
			bool filerBool;
			do{
				if (model.GetValue(childIter, 2).ToString().IndexOf(entry_busqueda1.Text) > -1)
					filerBool = true;

				if (model.IterHasChild(childIter))
					investigateChildNodes_nombre(model, childIter);

			}while (model.IterNext(ref childIter));
		}

		private bool FilterTree_nomina (Gtk.TreeModel model, Gtk.TreeIter iter)
		{
			string contactname = model.GetValue (iter, 1).ToString ();
			if (entry_busqueda1.Text.ToUpper() == ""){
				return true;
			}			
			if (contactname.IndexOf (entry_busqueda1.Text.ToUpper()) > -1){
				return true;
			}			
			if (model.IterHasChild(iter)) {
				bool filerBool = false;
				investigateChildNodes_nomina(model, iter); //method checking if currently investigated node has any child fulfilling filter contitions
				return filerBool;
			}
			return false;
		}

		private void investigateChildNodes_nomina(TreeModel model, TreeIter iter) 
		{       
			TreeIter childIter;
			model.IterChildren(out childIter, iter); 
			bool filerBool;
			do{
				if (model.GetValue(childIter, 1).ToString().IndexOf(entry_busqueda1.Text) > -1)
					filerBool = true;

				if (model.IterHasChild(childIter))
					investigateChildNodes_nomina(model, childIter);

			}while (model.IterNext(ref childIter));
		}

		void crea_treeview_lista_beneficiarios ()
		{
			object[] parametros = { treeview_lista_beneficiarios, treeViewEngineListbenef };
			string[,] coltreeview = {
				{ "#", "text" },
				{ "Nomina", "text" },
				{ "Nombre Afiliado", "text" },
				{ "Fech.Nac.", "text" },
				{ "Edad", "text" },
				{ "Departamento", "text" },
				{ "Afiliados", "text" },
				{ "Grupo", "text" },
				{ "Sexo", "text" },
				{ "Parentesco", "text" },
				{ "Puesto", "text" },
				{ "id_table", "text" }
			};
			crea_colums_treeview (parametros, coltreeview,"treeview_lista_beneficiarios");
		}

		void crea_treeview_cargacsv()
		{
			object[] parametros = { treeview_carga_csv, treeViewEngineCargacsv };
			string[,] coltreeview = {
				{ "#", "text" },
				{ "Nomina", "text" },
				{ "Nombre Afiliado", "text" },
				{ "Fech.Nac.", "text" },
				{ "Edad", "text" },
				{ "Departamento", "text" },
				{ "Afiliados", "text" },
				{ "Grupo", "text" },
				{ "Sexo", "text" },
				{ "Parentesco", "text" },
				{ "Puesto", "text" }
			};
			crea_colums_treeview (parametros, coltreeview,"treeview_carga_csv");
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
							if (colum_field == 1) {
								text.CellBackgroundGdk = new Gdk.Color (135, 193, 243);
							}
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
							//toggle.Toggled += new ToggledHandler (selecciona_fila);
						}
					}
				}
				if (nombre_treeview_ == "treeview_carga_csv"){
					treeview_carga_csv = treeviewobject;
					treeViewEngineCargacsv = treeViewEngine;
					// doble clic en la fila no toma el numero de la columna

					//treeview_inventario_mov.MoveCursor += row_change;
					//treeview_inventario_mov.MoveColumnAfter += row_change;
				}
				if (nombre_treeview_ == "treeview_lista_beneficiarios") {
					treeview_lista_beneficiarios = treeviewobject;
					treeViewEngineListbenef = treeViewEngine;
					treeview_lista_beneficiarios.RowActivated += on_button_admitir_beneficiario_clicked;
				}
			}
		}

		void on_button_admitir_beneficiario_clicked(object sender, EventArgs args)
		{
			TreeModel model;
			TreeIter iterSelected;
			if (treeview_lista_beneficiarios.Selection.GetSelected(out model, out iterSelected)){
				new osiris.admitir_beneficiario();
			}
		}
		
		void llenado_combobox(int tipodellenado,string descrip_defaul,object obj,string sql_or_array,string query_SQL,string name_field_desc,string name_field_id,string[] args_array,int[] args_id_array,string name_field_id2)
		{			
			Gtk.ComboBox combobox_llenado = (Gtk.ComboBox) obj;
			//Gtk.ComboBox combobox_pos_neg = obj as Gtk.ComboBox;
			//Console.WriteLine((string) combobox_llenado.GetType().ToString());
			combobox_llenado.Clear();
			CellRendererText cell = new CellRendererText();
			combobox_llenado.PackStart(cell, true);
			combobox_llenado.AddAttribute(cell,"text",0);	        
			ListStore store = new ListStore( typeof (string),typeof (int),typeof(bool));
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
						store.AppendValues ((string) lector[name_field_desc ], (int) lector[name_field_id],false);
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
			if (sender == null){return; }
			TreeIter iter;
			if (onComboBoxChanged.GetActiveIter (out iter)){
				switch (onComboBoxChanged.Name.ToString()){	
				case "combobox_busqueda1":
					tipodebusqueda_1 = (string)onComboBoxChanged.Model.GetValue (iter, 0);
					if (tipodebusqueda_1 == "NOMBRES O APELLIDOS") {
						filter = new Gtk.TreeModelFilter (treeViewEngineListbenef, null);
						filter.VisibleFunc = new Gtk.TreeModelFilterVisibleFunc (FilterTree_nombre);
						treeview_lista_beneficiarios.Model = filter;
					}
					if (tipodebusqueda_1 == "NOMINA") {
						tipodebusqueda_1 = (string)onComboBoxChanged.Model.GetValue (iter, 0);
						filter = new Gtk.TreeModelFilter (treeViewEngineListbenef, null);
						filter.VisibleFunc = new Gtk.TreeModelFilterVisibleFunc (FilterTree_nomina);
						treeview_lista_beneficiarios.Model = filter;

					}
					break;
				case "combobox_busqueda2":

					break;
				}
			}
		}

		void on_button_cargar_afiliados_clicked (object sender, EventArgs args)
		{
			string[,] parametros;
			object[] paraobj;
			TreeIter iter;
			MessageDialog msgBox = new MessageDialog (MyWin,DialogFlags.Modal,
				MessageType.Question,ButtonsType.YesNo,	"¿ Desea actualizar la infomacion ?");
			ResponseType miResultado = (ResponseType) msgBox.Run ();
			msgBox.Destroy();
			if (miResultado == ResponseType.Yes) {
				if (treeViewEngineCargacsv.GetIterFirst (out iter)) {
					parametros = new string[,] {
						{ "numero_afiliacion,", "'" + treeview_carga_csv.Model.GetValue (iter, 1).ToString ().Trim () + "'," },
						{ "serie_afiliacion,", "'" + treeview_carga_csv.Model.GetValue (iter, 6).ToString ().Trim () + "'," },
						{ "fechahora_creacion,", "'" + DateTime.Now.ToString ("yyyy-MM-dd HH:mm:ss") + "'," },
						{ "id_quien_creo,", "'" + LoginEmpleado + "'," },
						{ "fecha_nacimiento_afiliado,", "'" + treeview_carga_csv.Model.GetValue (iter, 3).ToString ().Trim () + "'," },
						{ "puesto_afiliado,", "'" + treeview_carga_csv.Model.GetValue (iter, 10).ToString ().Trim () + "'," },
						{ "depto_dondelabora_afiliado,", "'" + treeview_carga_csv.Model.GetValue (iter, 5).ToString ().Trim () + "'," },
						{ "numero_nomina_afiliado,", "'" + treeview_carga_csv.Model.GetValue (iter, 1).ToString ().Trim () + "'," },
						{ "edad_afiliado,", "'" + treeview_carga_csv.Model.GetValue (iter, 4).ToString ().Trim () + "'," },
						{ "sexo_afiliado,", "'" + treeview_carga_csv.Model.GetValue (iter, 8).ToString ().Trim () + "'," },
						{ "grupo_afiliado,", "'" + treeview_carga_csv.Model.GetValue (iter, 7).ToString ().Trim () + "'," },
						{ "parentesco_afiliado,", "'" + treeview_carga_csv.Model.GetValue (iter, 9).ToString ().Trim () + "'," },
						{ "nombre_completo", "'" + treeview_carga_csv.Model.GetValue (iter, 2).ToString ().Trim () + "' " }
					};
					paraobj = new object[] {entry_busqueda1 };
					insertreg = new insert_registro("osiris_erp_afiliados_enca", parametros, paraobj);
					while (treeViewEngineCargacsv.IterNext (ref iter)) {
						parametros = new string[,] {
						{ "numero_afiliacion,", "'" + treeview_carga_csv.Model.GetValue (iter, 1).ToString ().Trim () + "'," },
						{ "serie_afiliacion,", "'" + treeview_carga_csv.Model.GetValue (iter, 6).ToString ().Trim () + "'," },
						{ "fechahora_creacion,", "'" + DateTime.Now.ToString ("yyyy-MM-dd HH:mm:ss") + "'," },
						{ "id_quien_creo,", "'" + LoginEmpleado + "'," },
						{ "fecha_nacimiento_afiliado,", "'" + treeview_carga_csv.Model.GetValue (iter, 3).ToString ().Trim () + "'," },
						{ "puesto_afiliado,", "'" + treeview_carga_csv.Model.GetValue (iter, 10).ToString ().Trim () + "'," },
						{ "depto_dondelabora_afiliado,", "'" + treeview_carga_csv.Model.GetValue (iter, 5).ToString ().Trim () + "'," },
						{ "numero_nomina_afiliado,", "'" + treeview_carga_csv.Model.GetValue (iter, 1).ToString ().Trim () + "'," },
						{ "edad_afiliado,", "'" + treeview_carga_csv.Model.GetValue (iter, 4).ToString ().Trim () + "'," },
						{ "sexo_afiliado,", "'" + treeview_carga_csv.Model.GetValue (iter, 8).ToString ().Trim () + "'," },
						{ "grupo_afiliado,", "'" + treeview_carga_csv.Model.GetValue (iter, 7).ToString ().Trim () + "'," },
						{ "parentesco_afiliado,", "'" + treeview_carga_csv.Model.GetValue (iter, 9).ToString ().Trim () + "'," },
						{ "nombre_completo", "'" + treeview_carga_csv.Model.GetValue (iter, 2).ToString ().Trim () + "' " }
					};
						paraobj = new object[] { entry_busqueda1 };
						insertreg = new insert_registro("osiris_erp_afiliados_enca", parametros, paraobj);
					
					}
				}
			}	
		}

		void on_button_cargar_archivo_clicked(object sender, EventArgs args)
		{
			Gtk.FileChooserDialog select_file_csv = new Gtk.FileChooserDialog("Select file CSV for Open",
				tarjeta_medica_osiris,
				FileChooserAction.Open,
				"Cancel",ResponseType.Cancel,
				"Accept",ResponseType.Accept);

			Gtk.FileFilter filter = new Gtk.FileFilter();
			//filter.AddPattern("*.*");
			filter.AddPattern("*.csv");
			filter.AddPattern("*.CSV");

			select_file_csv.AddFilter(filter);		
			int resp = select_file_csv.Run();
			select_file_csv.Hide();			
			if (resp == (int)ResponseType.Accept) {
				Console.WriteLine (select_file_csv.Filename);
				lee_archivo_csv (select_file_csv.Filename);
			}
			select_file_csv.Destroy();
		}
		
		// cierra ventanas emergentes
		void on_cierraventanas_clicked (object sender, EventArgs args)
		{
			Widget win = (Widget) sender;
			win.Toplevel.Destroy();
		}

		void lee_archivo_csv(string file_and_path)
		{
			using (var rd = new StreamReader(@file_and_path)){
				while (!rd.EndOfStream){
					var splits = rd.ReadLine().Split(';');
					treeViewEngineCargacsv.AppendValues (splits [0], 
														splits [1], 
														splits [2],
														splits [3],
														splits [4],					
														splits [5],
														splits [6],
														splits [7],
														splits [8],
														splits [9],
														splits [10]);						
				}
			}
		}
	}
		
	
	public class admitir_beneficiario
	{
		// Boton general para salir de las ventanas
		// Todas la ventanas en glade este boton debe estra declarado identico
		[Widget] Gtk.Button button_salir = null;
		
		[Widget] Gtk.Window admision_beneficiario = null;
		[Widget] Gtk.Entry entry_nombre1 = null;

		[Widget] Gtk.Entry entry_nro_afiliacio = null;
		[Widget] Gtk.Entry entry_puesto = null;
		[Widget] Gtk.Entry entry_departamento = null;
		[Widget] Gtk.CheckButton checkbutton_es_primeravez = null;
		[Widget] Gtk.ComboBox combobox_tipo_paciente = null;
		[Widget] Gtk.ComboBox combobox_tipo_admision = null;
		[Widget] Gtk.Entry entry_id_medico = null;
		[Widget] Gtk.Entry entry_nombre_medico = null;
		[Widget] Gtk.Entry entry_especialidad_medico = null;
		[Widget] Gtk.Entry entry_tel_medico = null;
		[Widget] Gtk.Entry entry_cedula_medico = null;
		[Widget] Gtk.Button button_busca_medpcontacto = null;
		
		
		public admitir_beneficiario()
		{
			Glade.XML gxml = new Glade.XML (null, "registro_admision.glade", "admision_beneficiario", null);
			gxml.Autoconnect                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                 (this);
	        admision_beneficiario.Show();			
			
			button_busca_medpcontacto.Clicked += new EventHandler(on_button_busca_medicos_clicked);
			button_salir.Clicked += new EventHandler(on_cierraventanas_clicked);
			
			checkbutton_es_primeravez.Sensitive = false;
			// verificar si se a atendido 
			//checkbutton_es_primeravez.Active = 
		}
		
		void on_button_busca_medicos_clicked(object sender, EventArgs args)
		{
			
		}
		
		// cierra ventanas emergentes
		void on_cierraventanas_clicked(object sender, EventArgs args)
		{
			Widget win = (Widget) sender;
			win.Toplevel.Destroy();
		}
	}		
}