//
//  laboratorio_parametros.cs
//
//  Author:
//       Daniel Olivares C. <arcangeldoc@openmailbox.org>
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
	public class laboratorio_parametros
	{
		// parametros_laboratorio
		[Widget] Gtk.Window parametros_laboratorio = null;
		[Widget] Gtk.Entry entry_idproducto = null;
		[Widget] Gtk.Entry entry_descripcion_estudio = null;
		[Widget] Gtk.Entry entry_parametro = null;
		[Widget] Gtk.Entry entry_unidades = null;
		[Widget] Gtk.TextView textview_valor_refe = null;
		[Widget] Gtk.CheckButton checkbutton_aplica_valref = null;
		[Widget] Gtk.ComboBox combobox_tipo_valref = null;
		[Widget] Gtk.Button button_seleccion = null;
		[Widget] Gtk.Button button_buscar = null;
		[Widget] Gtk.Button button_agregar = null;
		[Widget] Gtk.Button button_grabar = null;
		[Widget] Gtk.Button button_quitar = null;
		[Widget] Gtk.Button button_editar = null;
		[Widget] Gtk.Button button_imprimir = null;
		[Widget] Gtk.TreeView treeview_lista_parametros = null;
		[Widget] Gtk.Entry entry_secuencia1 = null;
		[Widget] Gtk.Entry entry_secuencia2 = null;
		[Widget] Gtk.Button button_salir = null;

		// Edita Parametro
		[Widget] Gtk.Window editar_parametro = null;
		[Widget] Gtk.Entry entry_edit_idproducto = null;
		[Widget] Gtk.Entry entry_edit_descripestudio = null;
		[Widget] Gtk.Entry entry_edit_parametro = null;
		[Widget] Gtk.Entry entry_edit_unidades = null;
		[Widget] Gtk.TextView textview_edit_valrefe = null;
		[Widget] Gtk.Button button_graba_editar = null;
		[Widget] Gtk.Entry entry_idsecuencia = null;
		[Widget] Gtk.Entry entry_edit_secuencia1 = null;
		[Widget] Gtk.Entry entry_edit_secuencia2 = null;
		[Widget] Gtk.CheckButton checkbutton_edit_valref = null;

		string LoginEmpleado;
		string NomEmpleado;
		string AppEmpleado;
		string ApmEmpleado;

		string connectionString;
		string nombrebd;
		string idestudio_lab;
		string descripestudio_lab;
		string tipovalref = " ";

		string[] args_args = {""};
		string[] args_tipovalref = {"","MASCULINO","FEMENINO","NIÑO"};
		int[] args_id_array = {0,1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20};

		private ListStore treeViewEngineParametros;
		ArrayList columns = new ArrayList ();

		TextBuffer buffer = new TextBuffer(null);
		TextIter insertIter;

		//Declaracion de ventana de error
		protected Gtk.Window MyWinError;
		protected Gtk.Window MyWin;

		class_conexion conexion_a_DB = new class_conexion();
		class_buscador classfind_data = new class_buscador();
		class_public classpublic = new class_public();

		public laboratorio_parametros (string LoginEmp_, string NomEmpleado_, string AppEmpleado_, string ApmEmpleado_, string nombrebd_,string idproducto_)
		{
			connectionString = conexion_a_DB._url_servidor+conexion_a_DB._port_DB+conexion_a_DB._usuario_DB+conexion_a_DB._passwrd_user_DB;
			nombrebd = conexion_a_DB._nombrebd;
			LoginEmpleado = LoginEmp_;
			NomEmpleado = NomEmpleado_;
			AppEmpleado = AppEmpleado_;
			ApmEmpleado = ApmEmpleado_;

			Glade.XML gxml = new Glade.XML (null, "laboratorio.glade", "parametros_laboratorio", null);
			gxml.Autoconnect (this);        
			parametros_laboratorio.Show();

			entry_secuencia1.KeyPressEvent += onKeyPressEvent;
			entry_secuencia2.KeyPressEvent += onKeyPressEvent;
			entry_idproducto.KeyPressEvent += onKeyPressEvent_enter;
			button_seleccion.Clicked += new EventHandler(on_button_seleccion_clicked);
			button_buscar.Clicked += new EventHandler(on_button_buscar_clicked);
			button_agregar.Clicked += new EventHandler(on_button_agregar_clicked);
			button_grabar.Clicked += new EventHandler(on_button_grabar_clicked);
			button_quitar.Clicked += new EventHandler(on_button_quitar_clicked);
			button_editar.Clicked += new EventHandler(on_button_editar_clicked);
			button_imprimir.Clicked += new EventHandler(on_button_imprimir_clicked);
			button_salir.Clicked += new EventHandler(on_cierraventanas_clicked);
			entry_idproducto.ModifyBase (StateType.Normal, new Gdk.Color (254, 253, 152));
			entry_descripcion_estudio.ModifyBase (StateType.Normal, new Gdk.Color (101, 243, 143));

			llenado_combobox(0,"",combobox_tipo_valref,"array","","","",args_tipovalref,args_id_array,"");

			crea_treeview_parametros ();
			if (idproducto_ != "") {
				llenado_informacion_examen (idproducto_);
			}
		}

		void llenado_informacion_examen (string idproducto_)
		{
			treeViewEngineParametros.Clear ();
			entry_idproducto.Text = (string) classpublic.lee_registro_de_tabla("osiris_productos","id_producto","WHERE id_producto = '"+idproducto_+"' ","id_producto","string");
			entry_descripcion_estudio.Text = (string) classpublic.lee_registro_de_tabla("osiris_productos","descripcion_producto","WHERE id_producto = '"+idproducto_+"' ","descripcion_producto","string");
			idestudio_lab = idproducto_;
			descripestudio_lab = entry_descripcion_estudio.Text;
			NpgsqlConnection conexion; 		
			conexion = new NpgsqlConnection (connectionString+nombrebd);
			try{
				conexion.Open ();
				NpgsqlCommand comando;			
				comando = conexion.CreateCommand ();
				comando.CommandText = "SELECT osiris_his_examenes_laboratorio.id_producto,parametro,valor_referencia,unidades,id_secuencia_estudio,id_secuencia_parametros," +
					"tipo_valor_referencia,aplica_valor_referencia,descripcion_producto,osiris_his_examenes_laboratorio.activo,osiris_his_examenes_laboratorio.id_secuencia " +
					"FROM osiris_his_examenes_laboratorio,osiris_productos " +
					"WHERE osiris_his_examenes_laboratorio.id_producto = osiris_productos.id_producto " +
					"AND osiris_his_examenes_laboratorio.activo = 'true' " +
					"AND osiris_his_examenes_laboratorio.id_producto = '"+idproducto_+"'" +
					"ORDER BY id_secuencia_estudio,id_secuencia_parametros;";
				//Console.WriteLine(comando.CommandText);
				NpgsqlDataReader lector = comando.ExecuteReader ();
				if (lector.Read()){
					treeViewEngineParametros.AppendValues (
						lector["parametro"].ToString().Trim(),
						lector["valor_referencia"].ToString().Trim(),
						lector["unidades"].ToString().Trim(),
						lector["id_secuencia_estudio"].ToString().Trim(),
						lector["id_secuencia_parametros"].ToString().Trim(),
						lector["tipo_valor_referencia"].ToString().Trim(),
						(bool) lector["aplica_valor_referencia"],
						true,
						lector["id_secuencia"].ToString().Trim());
					while (lector.Read()){
						treeViewEngineParametros.AppendValues (
							lector["parametro"].ToString().Trim(),
							lector["valor_referencia"].ToString().Trim(),
							lector["unidades"].ToString().Trim(),
							lector["id_secuencia_estudio"].ToString().Trim(),
							lector["id_secuencia_parametros"].ToString().Trim(),
							lector["tipo_valor_referencia"].ToString().Trim(),
							(bool) lector["aplica_valor_referencia"],
							true,
							lector["id_secuencia"].ToString().Trim());
					}
				}
			}catch (NpgsqlException ex){
				//Console.WriteLine ("PostgresSQL error: {0}",ex.Message);
				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,MessageType.Info,ButtonsType.Close, "PostgresSQL error: {0}",ex.Message);
				msgBoxError.Run ();	msgBoxError.Destroy();
			}
			conexion.Close ();
		}

		void llenado_combobox(int tipodellenado,string descrip_defaul,object obj,string sql_or_array,string query_SQL,string name_field_desc,string name_field_id,string[] args_array,int[] args_id_array,string name_field_id2)
		{			
			Gtk.ComboBox combobox_llenado = (Gtk.ComboBox) obj;
			//Gtk.ComboBox combobox_pos_neg = obj as Gtk.ComboBox;
			combobox_llenado.Clear();
			CellRendererText cell = new CellRendererText();
			combobox_llenado.PackStart(cell, true);
			combobox_llenado.AddAttribute(cell,"text",0);	        
			ListStore store = new ListStore( typeof (string),typeof (int),typeof (int));
			combobox_llenado.Model = store;			
			if ((int) tipodellenado == 1){
				store.AppendValues ((string) descrip_defaul,0);
			}			
			if(sql_or_array == "array"){			
				for (int colum_field = 0; colum_field < args_array.Length; colum_field++){
					store.AppendValues (args_array[colum_field],args_id_array[colum_field],0);
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
							store.AppendValues ((string) lector[ name_field_desc ], (int) lector[ name_field_id],0);
						}else{
							store.AppendValues ((string) lector[ name_field_desc ], (int) lector[ name_field_id],(int) lector[ name_field_id2]);
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
				case "combobox_tipo_valref":
					tipovalref = onComboBoxChanged.Model.GetValue(iter,0).ToString().Substring(0,1);
					//idmunicipio = (int) combobox_municipios.Model.GetValue(iter,1);					
					break;
				}
			}
		}

		void on_button_seleccion_clicked (object sender, EventArgs args)
		{
			llenado_informacion_examen (entry_idproducto.Text.Trim());
		}

		void on_button_buscar_clicked (object sender, EventArgs args)
		{
			//Gtk.ComboBox hora_minutos_cita = (Gtk.ComboBox) sender;
			object[] parametros_objetos = {entry_idproducto,entry_descripcion_estudio,parametros_laboratorio};
			string[] parametros_sql = {"SELECT to_char(osiris_productos.id_producto,'999999999999') AS codProducto, "+
					"osiris_productos.descripcion_producto,osiris_productos.nombre_articulo,osiris_productos.nombre_generico_articulo, "+
					"to_char(precio_producto_publico,'99999999.99') AS preciopublico,"+
					"to_char(precio_producto_publico1,'99999999.99') AS preciopublico1,"+
					"to_char(cantidad_de_embalaje,'99999999.99') AS cantidadembalaje,"+
					"aplicar_iva,to_char(porcentage_descuento,'999.99') AS porcentagesdesc,aplica_descuento,cobro_activo,"+
					"descripcion_grupo_producto,descripcion_grupo1_producto,descripcion_grupo2_producto,to_char(costo_por_unidad,'999999999.99') AS costoproductounitario, "+
					"to_char(osiris_productos.id_grupo_producto,'9999999') AS idgrupoproducto,osiris_productos.id_grupo_producto, "+
					"to_char(osiris_grupo_producto.porcentage_utilidad_grupo,'99999.999') AS porcentageutilidadgrupo,"+
					"to_char(osiris_productos.id_grupo1_producto,'9999999') AS idgrupo1producto,osiris_productos.id_grupo1_producto,"+
					"to_char(osiris_productos.id_grupo2_producto,'9999999') AS idgrupo2producto,osiris_productos.id_grupo2_producto,"+
					"to_char(porcentage_ganancia,'99999.999') AS porcentageutilidad,to_char(costo_producto,'999999999.99') AS costoproducto,"+
					"tiene_kit,tipo_unidad_producto,osiris_productos.porcentage_iva "+
					"FROM osiris_productos,osiris_grupo_producto,osiris_grupo1_producto,osiris_grupo2_producto "+
					"WHERE osiris_productos.id_grupo_producto = osiris_grupo_producto.id_grupo_producto "+
					"AND osiris_productos.id_grupo1_producto = osiris_grupo1_producto.id_grupo1_producto "+
					"AND osiris_productos.id_grupo2_producto = osiris_grupo2_producto.id_grupo2_producto " +
					"AND osiris_productos.id_grupo_producto IN('16','17') "};
			string[] parametros_string = {};
			string[,] args_buscador1 = {{"DESCRIPCION PRODUCTO","AND osiris_productos.descripcion_producto LIKE '%","%' "},
											{"ID PRODUCTO","AND osiris_productos.id_producto = '","' "}};
			string[,] args_buscador2 = {{"ID PRODUCTO","AND osiris_productos.id_producto = '","' "},
											{"DESCRIPCION PRODUCTO","AND osiris_productos.descripcion_producto LIKE '%","%' "}};
			string[,] args_orderby = {{"",""}};
			classfind_data.buscandor(parametros_objetos,parametros_sql,parametros_string,"find_laboratorio_param",0,args_buscador1,args_buscador2,args_orderby);
		}

		void on_button_agregar_clicked (object sender, EventArgs args)
		{
			int toma_valor = int.Parse (entry_secuencia1.Text) + 1;
			treeViewEngineParametros.AppendValues (entry_parametro.Text.ToUpper(),
													textview_valor_refe.Buffer.Text.ToString().ToUpper(),
													entry_unidades.Text,
													entry_secuencia1.Text,
													entry_secuencia2.Text,
													tipovalref,
													(bool) checkbutton_aplica_valref.Active,
													false,
													"");
			tipovalref = " ";
			entry_parametro.Text = "";
			entry_unidades.Text = "";
			textview_valor_refe.Buffer.Text = "";
			entry_secuencia1.Text = toma_valor.ToString ().Trim ();
			entry_secuencia2.Text = "1";
			//llenado_combobox(0," ",combobox_tipo_valref,"array","","","",args_tipovalref,args_id_array,"");
			checkbutton_aplica_valref.Active = false;
		}

		void on_button_grabar_clicked(object sender, EventArgs args)
		{
			TreeIter iterSelected;
			string[,] parametros;
			object[] paraobj;
			if (treeViewEngineParametros.GetIterFirst (out iterSelected)) {
				if ((bool) treeview_lista_parametros.Model.GetValue (iterSelected, 7) == false) {
					parametros = new string[,] { 
						{"id_producto,","'"+idestudio_lab+"',"},
						{"parametro,","'"+treeview_lista_parametros.Model.GetValue (iterSelected, 0).ToString().ToUpper()+"',"},
						{"valor_referencia,","'"+treeview_lista_parametros.Model.GetValue (iterSelected, 1)+"',"},						
						{"aplica_valor_referencia,","'"+ treeview_lista_parametros.Model.GetValue (iterSelected, 6).ToString()+"',"},	 														
						{"id_quien_creo,","'"+LoginEmpleado +"',"},						
						{"fechahora_creacion,","'"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"',"},
						{"id_secuencia_estudio, ","'"+treeview_lista_parametros.Model.GetValue (iterSelected, 3)+"',"},
						{"id_secuencia_parametros,","'"+treeview_lista_parametros.Model.GetValue (iterSelected, 4)+"',"},
						{"tipo_valor_referencia,","'"+treeview_lista_parametros.Model.GetValue (iterSelected, 5)+"',"},
						{"unidades,","'"+treeview_lista_parametros.Model.GetValue (iterSelected, 2)+"',"},
						{"activo","'"+"true'"}
					};
					paraobj = new object[] { entry_idproducto};
					new osiris.insert_registro ("osiris_his_examenes_laboratorio", parametros, paraobj);
				}
				while (treeViewEngineParametros.IterNext (ref iterSelected)) {
					if ((bool)this.treeview_lista_parametros.Model.GetValue (iterSelected, 7) == false) {
						parametros = new string[,] { 
							{"id_producto,","'"+idestudio_lab+"',"},
							{"parametro,","'"+treeview_lista_parametros.Model.GetValue (iterSelected, 0).ToString().ToUpper()+"',"},
							{"valor_referencia,","'"+treeview_lista_parametros.Model.GetValue (iterSelected, 1)+"',"},						
							{"aplica_valor_referencia,","'"+ treeview_lista_parametros.Model.GetValue (iterSelected, 6).ToString()+"',"},	 														
							{"id_quien_creo,","'"+LoginEmpleado +"',"},						
							{"fechahora_creacion,","'"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"',"},
							{"id_secuencia_estudio, ","'"+treeview_lista_parametros.Model.GetValue (iterSelected, 3)+"',"},
							{"id_secuencia_parametros,","'"+treeview_lista_parametros.Model.GetValue (iterSelected, 4)+"',"},
							{"tipo_valor_referencia,","'"+treeview_lista_parametros.Model.GetValue (iterSelected, 5)+"',"},
							{"unidades,","'"+treeview_lista_parametros.Model.GetValue (iterSelected, 2)+"',"},
							{"activo","'"+"true'"}
						};
						paraobj = new object[] { entry_idproducto};
						new osiris.insert_registro ("osiris_his_examenes_laboratorio", parametros, paraobj);
					}
				}
				crea_treeview_parametros ();
				llenado_informacion_examen (idestudio_lab);
			}
		}

		void on_button_quitar_clicked(object sender, EventArgs args)
		{
			string[,] parametros;
			object[] paraobj;
			TreeModel model;
			TreeIter iterSelected;
			if (treeview_lista_parametros.Selection.GetSelected (out model, out iterSelected)) {
				MessageDialog msgBox = new MessageDialog (MyWin, DialogFlags.Modal,
					                       MessageType.Question, ButtonsType.YesNo, "¿ Esta quitar este concepto: " + (string)this.treeview_lista_parametros.Model.GetValue (iterSelected, 0));
				ResponseType miResultado = (ResponseType)msgBox.Run ();
				msgBox.Destroy ();
				if (miResultado == ResponseType.Yes) {					
					if ((bool)treeview_lista_parametros.Model.GetValue (iterSelected, 7) == false) {
						treeViewEngineParametros.Remove (ref iterSelected);
					} else {
						parametros = new string[,] {
							{ "activo = '", "false' " },
							{ "WHERE id_secuencia = '", treeview_lista_parametros.Model.GetValue (iterSelected, 8).ToString () + "';" }
						};
						paraobj = new object[] { entry_idproducto };
						new osiris.update_registro ("osiris_his_examenes_laboratorio", parametros, paraobj);
						crea_treeview_parametros ();
						llenado_informacion_examen (idestudio_lab);
					}
				}
			} else {
				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
					MessageType.Error, ButtonsType.Close,"Seleccione un Item, para poder quitarlo...");
				msgBoxError.Run ();						msgBoxError.Destroy();
			}
		}

		void on_button_imprimir_clicked(object sender, EventArgs args)
		{

		}

		void on_button_editar_clicked(object sender, EventArgs args)
		{
			TreeModel model;
			TreeIter iterSelected;

			if (treeview_lista_parametros.Selection.GetSelected (out model, out iterSelected)) {
				Glade.XML gxml = new Glade.XML (null, "laboratorio.glade", "editar_parametro", null);
				gxml.Autoconnect (this);        
				editar_parametro.Show ();

				buffer = textview_edit_valrefe.Buffer;
				classpublic.CreateTags(buffer);
				insertIter = buffer.StartIter;

				button_salir.Clicked += new EventHandler (on_cierraventanas_clicked);
				button_graba_editar.Clicked += new EventHandler (on_button_graba_editar_clicked);

				entry_edit_idproducto.Text = idestudio_lab;
				entry_edit_descripestudio.Text = descripestudio_lab;
				entry_edit_parametro.Text = treeview_lista_parametros.Model.GetValue (iterSelected, 0).ToString ();
				buffer.Insert (ref insertIter,(string) treeview_lista_parametros.Model.GetValue (iterSelected, 1).ToString ());
				entry_edit_unidades.Text = treeview_lista_parametros.Model.GetValue (iterSelected, 2).ToString ();
				entry_edit_secuencia1.Text = treeview_lista_parametros.Model.GetValue (iterSelected, 3).ToString ();
				entry_edit_secuencia2.Text = treeview_lista_parametros.Model.GetValue (iterSelected, 4).ToString ();
				checkbutton_edit_valref.Active = (bool) treeview_lista_parametros.Model.GetValue (iterSelected, 6);
				entry_idsecuencia.Text = treeview_lista_parametros.Model.GetValue (iterSelected, 8).ToString ();

				entry_idproducto.ModifyBase (StateType.Normal, new Gdk.Color (254, 253, 152));
				entry_descripcion_estudio.ModifyBase (StateType.Normal, new Gdk.Color (101, 243, 143));
			}
		}

		void on_button_graba_editar_clicked(object sender, EventArgs args)
		{
			string[,] parametros;
			object[] paraobj;
			MessageDialog msgBox = new MessageDialog (MyWin, DialogFlags.Modal,
				MessageType.Question, ButtonsType.YesNo, "Esta seguro de ACTUALIZAR el parametro ?");
			ResponseType miResultado = (ResponseType)msgBox.Run ();
			msgBox.Destroy ();
			if (miResultado == ResponseType.Yes) {
				parametros = new string[,] {
					{ "parametro = '",entry_edit_parametro.Text.ToUpper()+"'," },
					{ "valor_referencia = '",textview_edit_valrefe.Buffer.Text.ToString().ToUpper()+"'," },
					{ "aplica_valor_referencia = '",checkbutton_edit_valref.Active.ToString()+"'," },
					{ "id_secuencia_estudio = '",entry_edit_secuencia1.Text.ToString()+"'," },
					{ "id_secuencia_parametros = '",entry_edit_secuencia2.Text.ToString()+"'," },
					{ "unidades = '",entry_edit_unidades.Text.ToString()+"' " },
					{ "WHERE osiris_his_examenes_laboratorio.id_secuencia = '",entry_idsecuencia.Text+"';" }
				};
				paraobj = new object[] { entry_idsecuencia.Text};
				new osiris.update_registro ("osiris_his_examenes_laboratorio", parametros, paraobj);
				crea_treeview_parametros ();
				llenado_informacion_examen (entry_edit_idproducto.Text);
				editar_parametro.Destroy ();
			}	
		}

		void crea_treeview_parametros ()
		{
			treeViewEngineParametros = new ListStore(typeof(string),typeof(string),typeof(string),
													typeof(string),typeof(string),typeof(string),
													typeof(bool),typeof(bool),typeof(string));
			object[] parametros = { treeview_lista_parametros, treeViewEngineParametros };
			string[,] coltreeview = {
				{ "Parametro", "text" },
				{ "Valor Ref.", "text" },
				{ "Unidades", "text" },
				{ "Secuencia 1", "text" },
				{ "Secuencia 2", "text" },
				{ "Tipo Val.Ref.", "text" },
				{ "Aplica Val.Ref", "toogle" },
				{ "Save", "toogle" }
			};
			crea_colums_treeview (parametros, coltreeview,"");
		}

		void crea_colums_treeview(object[] args,string [,] args_colums,string tipo_reporte_)
		{
			Gtk.CellRendererText text;
			Gtk.CellRendererToggle toggle;
			Gtk.TreeViewColumn column0;
			// crea los objetos para el uso del treeview
			foreach (TreeViewColumn tvc in treeview_lista_parametros.Columns)
				treeview_lista_parametros.RemoveColumn(tvc);
			treeview_lista_parametros.Model = treeViewEngineParametros;
			treeview_lista_parametros.RulesHint = true;
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
						treeview_lista_parametros.InsertColumn (column0, colum_field);					
					}
					if((string) args_colums [colum_field, 1] == "toogle"){
						toggle = new CellRendererToggle ();
						toggle.Xalign = 0.0f;
						columns.Add (toggle);
						column0 = new TreeViewColumn ((string) args_colums [colum_field, 0], toggle,"active",colum_field);
						//column0.Sizing = TreeViewColumnSizing.Fixed;
						column0.Clickable = true;
						treeview_lista_parametros.InsertColumn (column0, colum_field);
					}
				}
			}
		}

		// Valida entradas que solo sean numericas, se utiliza eb ventana de carga de producto
		[GLib.ConnectBefore ()]   	  // Esto es indispensable para que funcione    
		void onKeyPressEvent(object o, Gtk.KeyPressEventArgs args)
		{
			//Console.WriteLine(args.Event.Key);
			//Console.WriteLine(Convert.ToChar(args.Event.Key));
			string misDigitos = "123456789ﾰﾱﾲﾳﾴﾵﾶﾷﾸﾹﾮｔｒｓｑ（）";
			if (Array.IndexOf(misDigitos.ToCharArray(), Convert.ToChar(args.Event.Key)) == -1 && args.Event.Key.ToString()  != "BackSpace"){
				args.RetVal = true;
			}
		}

		// Valida entradas que solo sean numericas, se utiliza eb ventana de carga de producto
		[GLib.ConnectBefore ()]   	  // Esto es indispensable para que funcione    
		void onKeyPressEvent_enter
		(object o, Gtk.KeyPressEventArgs args)
		{
			if (args.Event.Key.ToString() == "Return" || args.Event.Key.ToString() == "KP_Enter"){
				args.RetVal = true;
				llenado_informacion_examen (entry_idproducto.Text.Trim());
			}
			string misDigitos = "123456789ﾰﾱﾲﾳﾴﾵﾶﾷﾸﾹﾮｔｒｓｑ（）";
			if (Array.IndexOf(misDigitos.ToCharArray(), Convert.ToChar(args.Event.Key)) == -1 && args.Event.Key.ToString()  != "BackSpace"){
				args.RetVal = true;
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