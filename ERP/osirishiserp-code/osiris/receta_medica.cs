//  
//  receta_medica.cs
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
using Npgsql;
using Gtk;
using Glade;
using Gdk;
using GLib;

using System.Collections;

namespace osiris
{
	public class receta_medica
	{
		// Boton general para salir de las ventanas
		// Todas la ventanas en glade este boton debe estra declarado identico
		[Widget] Gtk.Button button_salir = null;
		
		[Widget] Gtk.Window receta_de_medicamento = null;
		[Widget] Gtk.TreeView treeview_listamedi = null;
		[Widget] Gtk.TreeView treeview_recetado2 = null;
		[Widget] Gtk.Entry entry_filter_medicamento = null;
		[Widget] Gtk.Entry entry_pid_paciente = null;
		[Widget] Gtk.Entry entry_nombre_paciente = null;
		[Widget] Gtk.Entry entry_edad_paciente = null;		
		[Widget] Gtk.Entry entry_numerotencion = null;
		[Widget] Gtk.Entry entry_fecha_nacimiento = null;
		[Widget] Gtk.Entry entry_sexo_paciente = null;
		[Widget] Gtk.Entry entry_id_doctor = null;
		[Widget] Gtk.Entry entry_doctor = null;
		[Widget] Gtk.Button button_quita_medrecetado = null;
		[Widget] Gtk.Entry entry_cantidad_surtir = null;
		[Widget] Gtk.SpinButton spinbutton_dosis = null;
		[Widget] Gtk.ComboBox combobox_presentacion = null;
		[Widget] Gtk.ComboBox combobox_viaadmin = null;
		[Widget] Gtk.ComboBox combobox_periodo = null;		
		[Widget] Gtk.SpinButton spinbutton_cada = null;
		[Widget] Gtk.Entry entry_periodo_final = null;
		[Widget] Gtk.Button button_recetar_med = null;
		[Widget] Gtk.Button button_agregar_med = null;
		[Widget] Gtk.TreeView treeview_lista_recetas = null;
		[Widget] Gtk.TreeView treeview_listamed_recetado = null;
		[Widget] Gtk.Button button_imprime_receta = null;
		[Widget] Gtk.Entry entry_texto_receta = null;
		[Widget] Gtk.Button button_textoreceta = null;
		[Widget] Gtk.Button button_remov_medrecetado = null;
				
		Gtk.ListStore treeViewEnginelistamedi;
		Gtk.ListStore treeViewEngineMedRecet2;
		Gtk.ListStore treeViewEngineListReceta;
		Gtk.ListStore treeViewEngineRecetado;

		Gtk.TreeModelFilter filter;
		Gtk.TextView textview_plan = null;
						
		TextBuffer bufferanalisis = new TextBuffer(null);
		TextIter insertIteranalisis;
		
		ArrayList columns = new ArrayList ();
		ArrayList arrayRecetaMedica = new ArrayList ();

		notas_medicas.struct_recetamedica RecetaMedica = new notas_medicas.struct_recetamedica();
		
		string connectionString;
		string nombrebd;
		int idsubalmacen;

		string nrorecetaselec = "0";
		
		string presenta_medic = "";
		string via_de_adminis = "";
		string periodo_de_toma = "";
		
		string[] args_args = {""};
		string[] args_presentacion = {"","TABLETA(S)","AMPULA(S)","CAPSULA(S)","GOTA(S)","CUCHARADA(S)","MILILITROS","INHALACION","OVULO","USAR","APLICAR","BOTE","UI","."};
		string[] args_tipoperiodo = {"","HORAS","DIA","SEMANA","MES"};
		string[] args_viaadministracion = {"","TOMAR","INTRA VENOSA","INTRA MUSCULAR","APLICAR RECTAL","CUTANEO","SUB-CUTANEO","ORAL","SUB-LINGUAL","TOPICA","OTICO","NASAL","OCULAR"};
		int[] args_id_array = {0,1,2,3,4,5,6,7,8,9,10,11,12,13,14};
		
		class_conexion conexion_a_DB = new class_conexion();
		class_public classpublic = new class_public();
		
		protected Gtk.Window MyWinError;
		
		public receta_medica (string LoginEmp, string NomEmpleado_, string AppEmpleado_, string ApmEmpleado_,
		                      string folioservicio_,string iddoctor_,string nombredoctor_,
		                      string pidpaciente_,string nombrepaciente_,string edadpaciente_,string fechanac_,
			string sexopaciente_,int idsubalmacen_,object textview_plan_,ArrayList arrayRecetaMedica_)
		{
			connectionString = conexion_a_DB._url_servidor+conexion_a_DB._port_DB+conexion_a_DB._usuario_DB+conexion_a_DB._passwrd_user_DB;
			nombrebd = conexion_a_DB._nombrebd;
			Glade.XML gxml = new Glade.XML (null, "hospitalizacion.glade", "receta_de_medicamento", null);
			gxml.Autoconnect (this);
			receta_de_medicamento.Show();

			arrayRecetaMedica = arrayRecetaMedica_;
			
			idsubalmacen = idsubalmacen_;
			entry_pid_paciente.Text = (string) pidpaciente_;
			entry_nombre_paciente.Text = (string) nombrepaciente_;
			entry_numerotencion.Text = (string) folioservicio_;
			entry_id_doctor.Text = (string) iddoctor_;
			entry_doctor.Text = (string) nombredoctor_;
			entry_edad_paciente.Text = (string) edadpaciente_;
			entry_numerotencion.IsEditable = false;
			entry_sexo_paciente.Text = (string) sexopaciente_;
			entry_cantidad_surtir.Text = "1";
			entry_fecha_nacimiento.Text = fechanac_;

			textview_plan = (object) textview_plan_ as Gtk.TextView;
						
			entry_filter_medicamento.Changed += OnFilterEntryTextChanged;
			entry_cantidad_surtir.KeyPressEvent += onKeyPressEvent_numeric;
			button_recetar_med.Clicked += new EventHandler(on_button_recetar_med_clicked);
			button_salir.Clicked += new EventHandler(on_cierraventanas_clicked);
			button_quita_medrecetado.Clicked += new EventHandler(on_button_quita_medrecetado_clicked);
			button_agregar_med.Clicked += new EventHandler(on_selectrow_dbl_click);
			button_textoreceta.Clicked += new EventHandler(on_button_textoreceta_click);
			// tab Recetar Realizadas
			button_imprime_receta.Clicked += new EventHandler(on_button_imprime_receta_clicked);
			button_remov_medrecetado.Clicked += new EventHandler(on_button_remov_medrecetado_clicked);
			
			llenado_combobox(0,"",combobox_presentacion,"array","","","",args_presentacion,args_id_array,"");
			llenado_combobox(0,"",combobox_viaadmin,"array","","","",args_viaadministracion,args_id_array,"");
			llenado_combobox(0,"",combobox_periodo,"array","","","",args_tipoperiodo,args_id_array,"");
			entry_numerotencion.ModifyBase(StateType.Normal, new Gdk.Color(170,192,255));
			entry_id_doctor.ModifyBase(StateType.Normal, new Gdk.Color(170,192,255));
			entry_doctor.ModifyBase(StateType.Normal, new Gdk.Color(170,192,255));
			entry_pid_paciente.ModifyBase(StateType.Normal, new Gdk.Color(170,192,255));
			entry_nombre_paciente.ModifyBase(StateType.Normal, new Gdk.Color(170,192,255));
			entry_edad_paciente.ModifyBase(StateType.Normal, new Gdk.Color(170,192,255));
			entry_fecha_nacimiento.ModifyBase(StateType.Normal, new Gdk.Color(170,192,255));
			entry_sexo_paciente.ModifyBase(StateType.Normal, new Gdk.Color(170,192,255));
			entry_cantidad_surtir.ModifyBase(StateType.Normal, new Gdk.Color(238,228,15));
			crea_treeview_medicamentos();
			crea_treeview_medrecetado();
			llenado_treeview_medicamentos();
			
			crea_treeview_recetasmed();
			llenado_treeview_listareceta();

			crea_treeview_recetarealizada ();

			// llenado de los medicamentos recetados
			for (int i = 0; i < arrayRecetaMedica.Count; ++i) {
				RecetaMedica = (notas_medicas.struct_recetamedica) arrayRecetaMedica [i];
				treeViewEngineMedRecet2.AppendValues (
					RecetaMedica.col00_receta,
					RecetaMedica.col01_receta,
					RecetaMedica.col02_receta,
					RecetaMedica.col03_receta,
					RecetaMedica.col03_receta);
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
				
		void onComboBoxChanged_llenado(object sender, EventArgs args)
		{
			ComboBox onComboBoxChanged = sender as ComboBox;
			if (sender == null){	return; }
			TreeIter iter;
			if (onComboBoxChanged.GetActiveIter (out iter)){
				switch (onComboBoxChanged.Name.ToString()){	
				case "combobox_presentacion":
					presenta_medic = (string) onComboBoxChanged.Model.GetValue(iter,0);
					break;
				case "combobox_periodo":
					periodo_de_toma = (string) onComboBoxChanged.Model.GetValue(iter,0);
					break;
				case "combobox_viaadmin":
					via_de_adminis = (string) onComboBoxChanged.Model.GetValue(iter,0);
					break;
				}
			}			
		}
		
		void on_button_recetar_med_clicked(object sender, EventArgs args)
		{
			bufferanalisis = textview_plan.Buffer;
			insertIteranalisis = bufferanalisis.StartIter;
			bufferanalisis.Clear();
			bufferanalisis = textview_plan.Buffer;
			insertIteranalisis = bufferanalisis.StartIter;

			Gtk.TreeIter iter2;
			if(treeViewEngineMedRecet2.GetIterFirst (out iter2)){				
				bufferanalisis.Insert(ref insertIteranalisis, (string) treeview_recetado2.Model.GetValue(iter2,0)+"\n");
				while(treeViewEngineMedRecet2.IterNext(ref iter2)){
					bufferanalisis.Insert(ref insertIteranalisis, (string) treeview_recetado2.Model.GetValue(iter2,0)+"\n");
				}
			}
			receta_de_medicamento.Destroy();
		}
		
		void crea_treeview_medicamentos()
		{
			Gtk.CellRendererText text;
			treeViewEnginelistamedi = new ListStore(typeof(string),typeof(string),typeof(string),typeof(string),
				                                          typeof(string),typeof(string));
			treeview_listamedi.Model = treeViewEnginelistamedi;
			treeview_listamedi.RulesHint = true;
			//treeview_listamedi.Selection.Mode = SelectionMode.Multiple;
			treeview_listamedi.RowActivated += on_selectrow_dbl_click;
			
			// column for holiday names
			text = new CellRendererText ();
			text.Xalign = 0.0f;
			columns.Add (text);
			TreeViewColumn column0 = new TreeViewColumn("Medicamento", text,"text", 0);
			column0.Resizable = true;
			column0.SortColumnId = 0;
			text.Width = 380;
			treeview_listamedi.InsertColumn (column0, 0);
			
			text = new CellRendererText ();
			text.Xalign = 0.0f;
			columns.Add (text);
			TreeViewColumn column1 = new TreeViewColumn("Stock", text,"text", 1);
			column1.Resizable = true;
			column1.SortColumnId = 1;
			treeview_listamedi.InsertColumn (column1, 1);
			
			text = new CellRendererText ();
			text.Xalign = 0.0f;
			columns.Add (text);
			TreeViewColumn column2 = new TreeViewColumn("Codigo", text,"text", 2);
			column2.Resizable = true;
			column2.SortColumnId = 1;
			treeview_listamedi.InsertColumn (column2, 2);
		}
		
		void crea_treeview_medrecetado()
		{
			Gtk.CellRendererText text;
			foreach (TreeViewColumn tvc in treeview_listamed_recetado.Columns)
				treeview_listamed_recetado.RemoveColumn(tvc);
			treeViewEngineRecetado = new ListStore(typeof(string),typeof(string),typeof(string),typeof(string),typeof(string));
			treeview_listamed_recetado.Model = treeViewEngineRecetado;
			treeview_listamed_recetado.RulesHint = true;
						
			// column for holiday names
			text = new CellRendererText ();
			text.Xalign = 0.0f;
			columns.Add (text);
			TreeViewColumn column0 = new TreeViewColumn("Medicamento Recetado", text,"text", 0);
			column0.Resizable = true;
			column0.SortColumnId = 0;
			treeview_listamed_recetado.InsertColumn (column0, 0);

			// column for holiday names
			text = new CellRendererText ();
			text.Xalign = 0.0f;
			columns.Add (text);
			TreeViewColumn column1 = new TreeViewColumn("ID Producto", text,"text", 1);
			column1.Resizable = true;
			column1.SortColumnId = 1;
			treeview_listamed_recetado.InsertColumn (column1, 1);

			// column for holiday names
			text = new CellRendererText ();
			text.Xalign = 0.0f;
			columns.Add (text);
			TreeViewColumn column2 = new TreeViewColumn("Cant.Sol.", text,"text", 2);
			column2.Resizable = true;
			column2.SortColumnId = 2;
			treeview_listamed_recetado.InsertColumn (column2, 2);
		}

		void on_button_remov_medrecetado_clicked(object sender, EventArgs args)
		{
			// nrorecetaselec
			// verificar si el producto esta surtido para poder eliminarlo del la receta y si tiene solicitud
			// (string) model.GetValue (iterSelected, 4)  // numero de solicitud


			TreeModel model;
			TreeIter iterSelected;
			if (treeview_listamed_recetado.Selection.GetSelected (out model, out iterSelected)) {
				treeViewEngineRecetado.Remove (ref iterSelected);
			}
		}

		void on_movecursor_cliente(object sender, EventArgs args)
		{
			TreeModel model;
			TreeIter iterSelected;
			if (treeview_lista_recetas.Selection.GetSelected(out model, out iterSelected)){
				crea_treeview_medrecetado();
				llenado_medicamento_recetado ((string) model.GetValue (iterSelected, 0));
			}
		}

		void llenado_medicamento_recetado(string numeroreceta_)
		{
			nrorecetaselec = numeroreceta_;
			NpgsqlConnection conexion;
			conexion = new NpgsqlConnection (connectionString+nombrebd);
			// Verifica que la base de datos este conectada
			try{
				conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand ();	          	
				comando.CommandText = "SELECT * FROM osiris_his_receta_medica WHERE folio_de_receta = '"+numeroreceta_+"';";
				//Console.WriteLine(comando.CommandText);
				NpgsqlDataReader lector = comando.ExecuteReader ();
				while (lector.Read()){
						treeViewEngineRecetado.AppendValues(
						lector["descripcion_prescripcion"].ToString(),
						lector["id_producto"].ToString(),
						lector["cantidad_recetada"].ToString(),
						lector["id_secuencia"].ToString(),
						lector["folio_de_solicitud"].ToString());
				}
			}catch (NpgsqlException ex){
				Console.WriteLine ("PostgresSQL error: {0}",ex.Message);
				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
					MessageType.Info,ButtonsType.Close, "PostgresSQL error: {0} ",ex.Message);
				msgBoxError.Run ();					msgBoxError.Destroy();
			}
			conexion.Close ();
		}
		
		void crea_treeview_recetasmed()
		{
			Gtk.CellRendererText text;
			treeViewEngineListReceta = new ListStore(typeof(string),typeof(string));
			treeview_lista_recetas.Model = treeViewEngineListReceta;
			treeview_lista_recetas.RulesHint = true;
			treeview_lista_recetas.MoveCursor += on_movecursor_cliente;
			treeview_lista_recetas.RowActivated += on_movecursor_cliente;
						
			// column for holiday names
			text = new CellRendererText ();
			text.Xalign = 0.0f;
			columns.Add (text);
			TreeViewColumn column0 = new TreeViewColumn("Numero Receta", text,"text", 0);
			column0.Resizable = true;
			column0.SortColumnId = 0;
			treeview_lista_recetas.InsertColumn (column0, 0);
		}

		void crea_treeview_recetarealizada()
		{
			Gtk.CellRendererText text;
			treeViewEngineMedRecet2 = new ListStore(typeof(string),typeof(string),typeof(string),typeof(string),typeof(string));
			treeview_recetado2.Model = treeViewEngineMedRecet2;
			treeview_recetado2.RulesHint = true;

			// column for holiday names
			text = new CellRendererText ();
			text.Xalign = 0.0f;
			columns.Add (text);
			TreeViewColumn column0 = new TreeViewColumn("Medicamento Recetado", text,"text", 0);
			column0.Resizable = true;
			column0.SortColumnId = 0;
			treeview_recetado2.InsertColumn (column0, 0);
		}
				
		void llenado_treeview_medicamentos()
		{
			string acceso_a_grupos = classpublic.lee_registro_de_tabla("osiris_almacenes","id_almacen"," WHERE osiris_almacenes.id_almacen = '"+idsubalmacen.ToString().Trim()+"' ","acceso_grupo_prodsoap","string");
			NpgsqlConnection conexion;
			conexion = new NpgsqlConnection (connectionString+nombrebd);
			// Verifica que la base de datos este conectada
			try{
				conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand ();	          	
				comando.CommandText = "SELECT osiris_catalogo_almacenes.id_almacen,osiris_catalogo_almacenes.id_secuencia," +
							"to_char(osiris_productos.id_producto,'999999999999') AS idproducto,"+
							"osiris_productos.descripcion_producto, "+
							"to_char(osiris_catalogo_almacenes.stock,'999999999999.99') AS stock,"+
							"to_char(osiris_catalogo_almacenes.minimo_stock,'999999999999.99') AS minstock,"+
							"to_char(osiris_catalogo_almacenes.maximo,'999999999999.99') AS maxstock,"+
							"to_char(osiris_catalogo_almacenes.punto_de_reorden,'999999999999.99') AS reorden,"+
							"to_char(osiris_catalogo_almacenes.fechahora_ultimo_surtimiento,'yyyy-MM-dd HH24:mi:ss') AS fechsurti, "+
							"osiris_productos.id_grupo_producto AS idgrupoproducto,descripcion_grupo_producto,"+ //descripcion_grupo1_producto,descripcion_grupo2_producto, "+
							"to_char(osiris_productos.precio_producto_publico,'99999999.99') AS preciopublico,"+
							"to_char(osiris_productos.costo_por_unidad,'999999999.99') AS costoproductounitario,"+
							"to_char(osiris_productos.costo_producto,'999999999.99') AS costoproducto,"+
							"to_char(osiris_productos.cantidad_de_embalaje,'999999999.99') AS embalaje, "+
							"to_char(osiris_productos.porcentage_ganancia,'99999.99') AS porcentageganancia, "+
							"osiris_catalogo_almacenes.tiene_stock "+
							"FROM osiris_catalogo_almacenes,osiris_productos,osiris_grupo_producto "+ //,osiris_grupo1_producto,osiris_grupo2_producto "+
							"WHERE osiris_catalogo_almacenes.id_producto = osiris_productos.id_producto "+ 
							"AND osiris_productos.id_grupo_producto = osiris_grupo_producto.id_grupo_producto "+
							"AND osiris_productos.cobro_activo = 'true' "+
							"AND osiris_grupo_producto.agrupacion_4 = 'true' "+
							"AND osiris_catalogo_almacenes.eliminado = 'false' "+
							"AND osiris_catalogo_almacenes.id_almacen = '"+idsubalmacen.ToString().Trim()+"' " +
							"AND osiris_productos.id_grupo_producto IN("+acceso_a_grupos+") "+
							"ORDER BY descripcion_producto;";
				//Console.WriteLine(comando.CommandText);
				NpgsqlDataReader lector = comando.ExecuteReader ();
				while (lector.Read()){
					treeViewEnginelistamedi.AppendValues(lector["descripcion_producto"].ToString(),
					                                     "",
					                                     lector["idproducto"].ToString(),
					                                     lector["preciopublico"].ToString(),
					                                     lector["costoproductounitario"].ToString());
				}
			}catch (NpgsqlException ex){
		   		Console.WriteLine ("PostgresSQL error: {0}",ex.Message);
		   		MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
							MessageType.Info,ButtonsType.Close, "PostgresSQL error: {0} ",ex.Message);
				msgBoxError.Run ();					msgBoxError.Destroy();
			}
			conexion.Close ();
			filter = new Gtk.TreeModelFilter (treeViewEnginelistamedi, null);
			filter.VisibleFunc = new Gtk.TreeModelFilterVisibleFunc (FilterTree);
			treeview_listamedi.Model = filter;
		}
		
		void llenado_treeview_listareceta()
		{
			NpgsqlConnection conexion;
			conexion = new NpgsqlConnection (connectionString+nombrebd);
			// Verifica que la base de datos este conectada
			try{
				conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand ();	          	
				comando.CommandText = "SELECT DISTINCT ON (folio_de_receta) osiris_his_receta_medica.folio_de_receta, * FROM osiris_his_receta_medica WHERE folio_de_servicio = '"+entry_numerotencion.Text.Trim()+"';";;
				//Console.WriteLine(comando.CommandText);
				NpgsqlDataReader lector = comando.ExecuteReader ();
				while (lector.Read()){
					treeViewEngineListReceta.AppendValues(lector["folio_de_receta"].ToString());
					//Console.WriteLine(lector["folio_de_receta"].ToString());
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
		
		// Valida entradas que solo sean numericas, se utiliza eb ventana de carga de producto
		[GLib.ConnectBefore ()]   	  // Esto es indispensable para que funcione    
		private void onKeyPressEvent_numeric(object o, Gtk.KeyPressEventArgs args)
		{
			//Console.WriteLine(args.Event.Key);
			//Console.WriteLine(Convert.ToChar(args.Event.Key));
			string misDigitos = "123456789ﾱﾲﾳﾴﾵﾶﾷﾸﾹﾮｔｒｓｑ（）";
			if (Array.IndexOf(misDigitos.ToCharArray(), Convert.ToChar(args.Event.Key)) == -1 && args.Event.Key.ToString()  != "BackSpace"){
				args.RetVal = true;
			}
		}
		
		private bool FilterTree (Gtk.TreeModel model, Gtk.TreeIter iter)
		{
			string contactname = model.GetValue (iter, 0).ToString ();
			if (entry_filter_medicamento.Text.ToUpper() == ""){
			    return true;
			}			
			if (contactname.IndexOf (entry_filter_medicamento.Text.ToUpper()) > -1){
			    return true;
			}			
			if (model.IterHasChild(iter)) {
			    bool filerBool = false;
			    investigateChildNodes(model, iter); //method checking if currently investigated node has any child fulfilling filter contitions
			    return filerBool;
			}
			return false;
		}
		
		private void investigateChildNodes(TreeModel model, TreeIter iter) 
		{       
		    TreeIter childIter;
		    model.IterChildren(out childIter, iter); 
			bool filerBool;
		    do{
		        if (model.GetValue(childIter, 0).ToString().IndexOf(entry_filter_medicamento.Text) > -1)
		            filerBool = true;
		
		        if (model.IterHasChild(childIter))
		            investigateChildNodes(model, childIter);
		
		    }while (model.IterNext(ref childIter));
		}
		
		void on_selectrow_dbl_click(object sender, EventArgs args)
		{
			TreeModel model;
			TreeIter iterSelected;
			if (treeview_listamedi.Selection.GetSelected(out model, out iterSelected)){
				if(float.Parse(spinbutton_dosis.Text) > 0 && via_de_adminis != "" && presenta_medic != "" && float.Parse(spinbutton_cada.Text) > 0 && periodo_de_toma != "" && entry_periodo_final.Text != ""){
					treeViewEngineMedRecet2.AppendValues(model.GetValue (iterSelected, 0).ToString ()+" "+via_de_adminis+" "+spinbutton_dosis.Text+" "+presenta_medic+" CADA "+spinbutton_cada.Text+" "+periodo_de_toma+" POR "+entry_periodo_final.Text.ToUpper());				

					RecetaMedica = new notas_medicas.struct_recetamedica(
						model.GetValue (iterSelected, 0).ToString ()+" "+via_de_adminis+" "+spinbutton_dosis.Text+" "+presenta_medic+" CADA "+spinbutton_cada.Text+" "+periodo_de_toma+" POR "+entry_periodo_final.Text.ToUpper(),
						model.GetValue (iterSelected, 2).ToString (),
						entry_cantidad_surtir.Text.ToString(),
						model.GetValue (iterSelected, 3).ToString (),
						model.GetValue (iterSelected, 4).ToString ()
					);
					arrayRecetaMedica.Add(RecetaMedica);


					/*treeViewEngineMedRecet.AppendValues (model.GetValue (iterSelected, 0).ToString ()+" "+via_de_adminis+" "+spinbutton_dosis.Text+" "+presenta_medic+" CADA "+spinbutton_cada.Text+" "+periodo_de_toma+" POR "+entry_periodo_final.Text.ToUpper(),
					                                    model.GetValue (iterSelected, 2).ToString (),
														entry_cantidad_surtir.Text.ToString(),
					                                     model.GetValue (iterSelected, 3).ToString (),
					                                     model.GetValue (iterSelected, 4).ToString ());*/
				}
			}
		}

		void on_button_textoreceta_click(object sender, EventArgs args)
		{
			if(entry_texto_receta.Text != ""){
				treeViewEngineMedRecet2.AppendValues(entry_texto_receta.Text.ToUpper());
				RecetaMedica = new notas_medicas.struct_recetamedica(
									entry_texto_receta.Text.ToUpper(),
									"0",
									"0",
									"0",
									"0"
								);
				arrayRecetaMedica.Add(RecetaMedica);
			}
		}
		
		void on_button_quita_medrecetado_clicked(object sender, EventArgs args)
		{
			TreeIter iter2;
			TreeModel model2;
			if (treeview_recetado2.Selection.GetSelected (out model2, out iter2)) {
				int position = treeViewEngineMedRecet2.GetPath (iter2).Indices[0];
				treeViewEngineMedRecet2.Remove (ref iter2);
				arrayRecetaMedica.RemoveAt (position);
			}
		}

		void removeselectedrows(TreeView treeView, ListStore listStore)
		{
			TreeIter iter;
			TreePath[] treePath = treeView.Selection.GetSelectedRows();
			for (int i  = treePath.Length; i > 0; i--){
				listStore.GetIter(out iter, treePath[(i - 1)]);
				string value = (string)listStore.GetValue(iter, 0);
				//Console.WriteLine("Removing: " + value);
				listStore.Remove(ref iter);
			}
		}
		
		void on_button_imprime_receta_clicked(object sender, EventArgs args)
		{
			TreeIter iter;
 			TreeModel model;
			if(treeview_lista_recetas.Selection.GetSelected (out model, out iter)) {
				new osiris.rpt_receta_medica((string) treeview_lista_recetas.Model.GetValue(iter,0),int.Parse(entry_numerotencion.Text),int.Parse(entry_pid_paciente.Text),entry_nombre_paciente.Text,
				                             entry_fecha_nacimiento.Text,entry_edad_paciente.Text,entry_sexo_paciente.Text);
			}
		}
		
		// cierra ventanas emergentes
		void on_cierraventanas_clicked(object sender, EventArgs args)
		{
			Widget win = (Widget) sender;
			win.Toplevel.Destroy();
		}
	}
}