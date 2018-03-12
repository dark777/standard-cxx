//
//  movimiento_mensual.cs
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
	public class movimiento_mensual
	{
		// Boton general para salir de las ventanas
		// Todas la ventanas en glade este boton debe estra declarado identico
		[Widget] Gtk.Button button_salir;

		// Para todas las busquedas este es el nombre asignado
		// se declara una vez
		[Widget] Gtk.Entry entry_expresion;
		[Widget] Gtk.Button button_selecciona;
		[Widget] Gtk.Button button_buscar_busqueda;
		//[Widget] Gtk.Button button_imprimir_busqueda;

		// Declarando ventana de consulta de producto
		[Widget] Gtk.Window movimiento_prod_mensual;
		[Widget] Gtk.Entry entry_descrip_producto;
		[Widget] Gtk.Button button_busca_producto;
		[Widget] Gtk.ComboBox combobox_anos;
		[Widget] Gtk.TreeView lista_producto_seleccionados;
		[Widget] Gtk.TreeView lista_resumen_productos;

		[Widget] Gtk.CheckButton checkbutton_ene_costos;
		[Widget] Gtk.CheckButton checkbutton_feb_costos;
		[Widget] Gtk.CheckButton checkbutton_mar_costos;
		[Widget] Gtk.CheckButton checkbutton_abr_costos;
		[Widget] Gtk.CheckButton checkbutton_may_costos;
		[Widget] Gtk.CheckButton checkbutton_jun_costos;
		[Widget] Gtk.CheckButton checkbutton_jul_costos;
		[Widget] Gtk.CheckButton checkbutton_ago_costos;
		[Widget] Gtk.CheckButton checkbutton_sep_costos;
		[Widget] Gtk.CheckButton checkbutton_oct_costos;
		[Widget] Gtk.CheckButton checkbutton_nov_costos;
		[Widget] Gtk.CheckButton checkbutton_dic_costos;
		[Widget] Gtk.CheckButton checkbutton_todo_ano;
		[Widget] Gtk.ComboBox combobox_grupo = null;
		[Widget] Gtk.ComboBox combobox_grupo1 = null;
		[Widget] Gtk.ComboBox combobox_grupo2 = null;
		[Widget] Gtk.Button button_crealist_prod = null;
		[Widget] Gtk.CheckButton checkbutton_grupo = null;
		[Widget] Gtk.CheckButton checkbutton_grupo1 = null;
		[Widget] Gtk.CheckButton checkbutton_grupo2 = null;
		[Widget] Gtk.CheckButton checkbutton_rango_fecha = null;
		[Widget] Gtk.Entry entry_fecha_inicio = null;
		[Widget] Gtk.Entry entry_fecha_termino = null;
		[Widget] Gtk.Button button_lista_prodcargos = null;
		[Widget] Gtk.Button button_consultar_costos = null;
		[Widget] Gtk.Button button_quitar_producto = null;
		[Widget] Gtk.Button button_limpiar = null;
		[Widget] Gtk.Button button_imprimir_costos = null;
		[Widget] Gtk.Button button_export_sheet = null;

		Gtk.TreeView treeviewobject = null;
		Gtk.ListStore treeViewEngine = null;
		ArrayList columns = new ArrayList ();

		string busqueda = "";
		string connectionString;
		string nombrebd;
		string LoginEmpleado;
		string NomEmpleado;
		string AppEmpleado;
		string ApmEmpleado;
		string ano_seleccionado = DateTime.Now.ToString("yyyy");

		private ListStore treeViewEngineBusca2;		// Para la busqueda de Productos
		private ListStore treeViewEngineProdSelec;	// Lista de Productos seleccionados
		private ListStore treeViewEngineResumen;	// Lista de Productos seleccionados

		//Declaracion de ventana de error
		protected Gtk.Window MyWinError;
		protected Gtk.Window MyWin;

		string[] args_args = {""};
		int[] args_id_array = {0,1,2,3,4,5,6,7,8,9,10};
		string[] args_anos = { "2010", "2011", "2012", "2013", "2014", "2015", "2016", "2017", "2018", "2019", "2020", "2021" };
		int idgrupo0 = 1;
		int idgrupo1 = 1;
		int idgrupo2 = 1;
		string query_grupo_prod = "";
		string query_grupo_prod1 = "";
		string query_grupo_prod2 = "";


		class_conexion conexion_a_DB = new class_conexion();
		class_public classpublic = new class_public();

		public movimiento_mensual(string LoginEmp_, string NomEmpleado_, string AppEmpleado_, string ApmEmpleado_, string nombrebd_ )
		{
			LoginEmpleado = LoginEmp_;
			NomEmpleado = NomEmpleado_;
			AppEmpleado = AppEmpleado_;
			ApmEmpleado = ApmEmpleado_;
			connectionString = conexion_a_DB._url_servidor + conexion_a_DB._port_DB + conexion_a_DB._usuario_DB + conexion_a_DB._passwrd_user_DB;
			nombrebd = conexion_a_DB._nombrebd;

			Glade.XML gxml = new Glade.XML (null, "costos.glade", "movimiento_prod_mensual", null);
			gxml.Autoconnect (this);
			// Muestra ventana de Glade
			movimiento_prod_mensual.Show ();
			// Creacion de los treeview en pantalla
			crea_treeview_prodselec ();
			crea_treeview_resumen_mensual ();

			//button_busca_producto.Clicked += new EventHandler(on_button_busca_producto_clicked);
			button_consultar_costos.Clicked += new EventHandler (on_button_consultar_costos_clicked);			
			button_quitar_producto.Clicked += new EventHandler (on_button_quitar_producto_clicked);
			checkbutton_todo_ano.Clicked += new EventHandler (on_checkbutton_todo_ano_clicked);
			button_limpiar.Clicked += new EventHandler (on_button_limpiar_clicked);
			//button_imprimir_costos.Clicked += new EventHandler(on_button_imprimir_costos_clicked);
			button_crealist_prod.Clicked += new EventHandler (on_button_crealist_prod_clicked);
			checkbutton_grupo.Clicked += new EventHandler (on_checkbutton_grupo_clicked);
			checkbutton_grupo1.Clicked += new EventHandler (on_checkbutton_grupo1_clicked);
			checkbutton_grupo2.Clicked += new EventHandler (on_checkbutton_grupo2_clicked);
			checkbutton_rango_fecha.Clicked += new EventHandler (on_checkbutton_rango_fecha_clicked);
			button_lista_prodcargos.Clicked += new EventHandler (on_button_lista_prodcargos_clicked);
			button_export_sheet.Clicked += new EventHandler (on_button_export_sheet_clicked);
			button_salir.Clicked += new EventHandler (on_cierraventanas_clicked);

			checkbutton_todo_ano.Active = true;
			combobox_grupo.Sensitive = false;
			combobox_grupo1.Sensitive = false;
			combobox_grupo2.Sensitive = false;
			entry_fecha_inicio.Sensitive = false;
			entry_fecha_termino.Sensitive = false;
			entry_fecha_inicio.Text = DateTime.Now.ToString ("yyyy-MM-dd");
			entry_fecha_termino.Text = DateTime.Now.ToString ("yyyy-MM-dd");

			llenado_combobox(1,DateTime.Now.ToString("yyyy"),combobox_anos,"array","","","",args_args,args_id_array);
		}

		void on_button_export_sheet_clicked(object sender, EventArgs args)
		{
			string[,] args_formulas;
			string[,] args_width;
			string[,] args_name_type_active;
			string[] args_field_text;
			string[] args_more_title;


			args_name_type_active = new string[,]{ 
				{ "Codigo Prod.", "string","active" },
				{ "Descripcion Producto", "string","active" },
				{"Cos.Unitario","string","active"},
				{"ENE","float","active"},
				{"FEB","float","active"},
				{"MAR","float","active"},
				{"ABR","float","active"},
				{"MAY","float","active"},
				{"JUN","float","active"},
				{"JUL","float","active"},
				{"AGO","float","active"},
				{"SEP","float","active"},
				{"OCT","float","active"},
				{"NOV","float","active"},
				{"DIC","float","active"},
				{"Total Aplicado","float","active"},
				{"Total $ Venta","float","active"},
				{"Precio $ Venta","float","active"},
				{"Promedio Consumo","float","active"},
				{"Grupo Producto","string","active"},
				{"Grupo1 Producto","string","active"},
				{"Grupo2 Producto","string","active"},
				};
				args_field_text = new string[] {"string","string","string","string","string","string","string","string","string","string","string","string"};
				args_more_title = new string[] {""};
				args_formulas = new string[,] {{"",""}};
				args_width = new string[,] {{"1","8cm"}};
			new osiris.class_traslate_ods_array (lista_resumen_productos,treeViewEngineResumen,args_name_type_active,false,args_field_text,"",false,args_more_title,args_formulas,args_width,"REPORTE DE OCUPACION","RAGO DE FECHA  DESDE :"+entry_fecha_inicio.Text+"  HASTA:"+entry_fecha_termino.Text);
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
			//int idgrupo0 = 1;
			//int idgrupo1 = 1;
			//int idgrupo2 = 1;
			ComboBox onComboBoxChanged = sender as ComboBox;
			if (sender == null){	return; }
			TreeIter iter;
			if (onComboBoxChanged.GetActiveIter (out iter)){
				switch (onComboBoxChanged.Name.ToString()){	
				case "combobox_grupo":
					idgrupo0 = (int)onComboBoxChanged.Model.GetValue (iter, 1);
					query_grupo_prod = "AND osiris_erp_cobros_deta.id_producto = '" + idgrupo0.ToString ().Trim () + "' ";
					break;
				case "combobox_grupo1":
					idgrupo1 = (int) onComboBoxChanged.Model.GetValue(iter,1);
					query_grupo_prod1 = "";
					break;
				case "combobox_grupo2":
					idgrupo2 = (int) onComboBoxChanged.Model.GetValue(iter,1);
					query_grupo_prod2 = "";
					break;
				case "combobox_anos":
					ano_seleccionado = (string) onComboBoxChanged.Model.GetValue(iter,0);
					break;
				}
				//Console.WriteLine(query_departamento);
			}
		}

		void on_checkbutton_rango_fecha_clicked(object sender, EventArgs args)
		{
			checkbutton_todo_ano.Active = (bool) !checkbutton_rango_fecha.Active;
			entry_fecha_inicio.Sensitive = (bool) checkbutton_rango_fecha.Active;
			entry_fecha_termino.Sensitive = (bool) checkbutton_rango_fecha.Active;
		}

		void on_checkbutton_grupo_clicked(object sender, EventArgs args)
		{
			combobox_grupo.Sensitive = checkbutton_grupo.Active;
			if ((bool)checkbutton_grupo.Active) {
				llenado_combobox(0,"",combobox_grupo,"sql","SELECT * FROM osiris_grupo_producto ORDER BY descripcion_grupo_producto;","descripcion_grupo_producto","id_grupo_producto",args_args,args_id_array);
			}
		}

		void on_checkbutton_grupo1_clicked(object sender, EventArgs args)
		{
			combobox_grupo1.Sensitive = checkbutton_grupo1.Active;
			if ((bool)checkbutton_grupo1.Active) {
				llenado_combobox(0,"",combobox_grupo1,"sql","SELECT * FROM osiris_grupo1_producto ORDER BY descripcion_grupo1_producto;","descripcion_grupo1_producto","id_grupo1_producto",args_args,args_id_array);
			}
		}

		void on_checkbutton_grupo2_clicked(object sender, EventArgs args)
		{
			combobox_grupo2.Sensitive = checkbutton_grupo2.Active;
			if ((bool)checkbutton_grupo2.Active) {
				llenado_combobox(0,"",combobox_grupo2,"sql","SELECT * FROM osiris_grupo2_producto ORDER BY descripcion_grupo2_producto;","descripcion_grupo2_producto","id_grupo2_producto",args_args,args_id_array);
			}
		}

		void on_button_crealist_prod_clicked(object sender, EventArgs args)
		{
			query_grupo_prod = "";
			if((bool) checkbutton_grupo.Active == true){
				query_grupo_prod = " AND osiris_grupo_producto.id_grupo_producto = '"+idgrupo0.ToString().Trim()+"' ";
				llenado_grupos_seleccionados();
			}
		}

		void on_button_busca_producto_clicked (object sender, EventArgs args)
		{
			
		}

		void on_checkbutton_todo_ano_clicked (object sender, EventArgs args)
		{
			checkbutton_ene_costos.Active = (bool) checkbutton_todo_ano.Active;
			checkbutton_feb_costos.Active = (bool) checkbutton_todo_ano.Active;
			checkbutton_mar_costos.Active = (bool) checkbutton_todo_ano.Active;
			checkbutton_abr_costos.Active = (bool) checkbutton_todo_ano.Active;
			checkbutton_may_costos.Active = (bool) checkbutton_todo_ano.Active;
			checkbutton_jun_costos.Active = (bool) checkbutton_todo_ano.Active;
			checkbutton_jul_costos.Active = (bool) checkbutton_todo_ano.Active;
			checkbutton_ago_costos.Active = (bool) checkbutton_todo_ano.Active;
			checkbutton_sep_costos.Active = (bool) checkbutton_todo_ano.Active;
			checkbutton_oct_costos.Active = (bool) checkbutton_todo_ano.Active;
			checkbutton_nov_costos.Active = (bool) checkbutton_todo_ano.Active;
			checkbutton_dic_costos.Active = (bool) checkbutton_todo_ano.Active;
		}

		void on_button_consultar_costos_clicked (object sender, EventArgs args)
		{
			int contador_de_meses = 0;
			string meses_seleccionados = "";
			string productos_seleccionado = "";
			string query_anomeses_rangofecha = "";
			if (checkbutton_rango_fecha.Active == true) {
				query_anomeses_rangofecha = "AND to_char(osiris_erp_cobros_deta.fechahora_creacion,'yyyy-MM-dd') >= '"+entry_fecha_inicio.Text+"' "+
					"AND to_char(osiris_erp_cobros_deta.fechahora_creacion,'yyyy-MM-dd') <= '"+entry_fecha_termino.Text+"' ";
			} else {
				if (checkbutton_ene_costos.Active) {
					meses_seleccionados += "'"+ano_seleccionado + "-01',";
				}
				if (checkbutton_feb_costos.Active) {
					meses_seleccionados += "'"+ano_seleccionado + "-02',";
				}
				if (checkbutton_mar_costos.Active) {
					meses_seleccionados += "'"+ano_seleccionado + "-03',";
				}
				if (checkbutton_abr_costos.Active) {
					meses_seleccionados += "'"+ano_seleccionado + "-04',";
				}
				if (checkbutton_may_costos.Active) {
					meses_seleccionados += "'"+ano_seleccionado + "-05',";
				}
				if (checkbutton_jun_costos.Active) {
					meses_seleccionados += "'"+ano_seleccionado + "-06',";
				}
				if (checkbutton_jul_costos.Active) {
					meses_seleccionados += "'"+ano_seleccionado + "-07',";
				}
				if (checkbutton_ago_costos.Active) {
					meses_seleccionados += "'"+ano_seleccionado + "-08',";
				}
				if (checkbutton_sep_costos.Active) {
					meses_seleccionados += "'"+ano_seleccionado + "-09',";
				}
				if (checkbutton_oct_costos.Active) {
					meses_seleccionados += "'"+ano_seleccionado + "-10',";
				}
				if (checkbutton_nov_costos.Active) {
					meses_seleccionados += "'"+ano_seleccionado + "-11',";
				}
				if (checkbutton_dic_costos.Active) {
					meses_seleccionados += "'"+ano_seleccionado + "-12'";
				}
				query_anomeses_rangofecha = "AND to_char(osiris_erp_cobros_deta.fechahora_creacion,'yyyy-MM') IN(" + meses_seleccionados + ") ";
			}
			// Validando que tenga algun producto seleccionado en la lista
			treeViewEngineResumen.Clear();
			TreeIter iter;
			if (treeViewEngineProdSelec.GetIterFirst (out iter)) {
				llenado_treeview_por_producto ((string) lista_producto_seleccionados.Model.GetValue (iter, 0),query_anomeses_rangofecha);
				while (treeViewEngineProdSelec.IterNext (ref iter)) {
					llenado_treeview_por_producto ((string) lista_producto_seleccionados.Model.GetValue (iter, 0),query_anomeses_rangofecha);
				}
			}
		}

		void llenado_treeview_por_producto (string idproducto_,string queryanomesesrangofecha_)
		{
			float var_01_ene = 0; float var_02_ene = 0; string var_03_ene = "0";
			float var_01_feb = 0; float var_02_feb = 0; string var_03_feb = "0";
			float var_01_mar = 0; float var_02_mar = 0; string var_03_mar = "0";
			float var_01_abr = 0; float var_02_abr = 0; string var_03_abr = "0";
			float var_01_may = 0; float var_02_may = 0; string var_03_may = "0";
			float var_01_jun = 0; float var_02_jun = 0; string var_03_jun = "0";
			float var_01_jul = 0; float var_02_jul = 0; string var_03_jul = "0";
			float var_01_ago = 0; float var_02_ago = 0; string var_03_ago = "0";
			float var_01_sep = 0; float var_02_sep = 0; string var_03_sep = "0";
			float var_01_oct = 0; float var_02_oct = 0; string var_03_oct = "0";
			float var_01_nov = 0; float var_02_nov = 0; string var_03_nov = "0";
			float var_01_dic = 0; float var_02_dic = 0; string var_03_dic = "0";
			string descrip_producto = "";
			string descrip_grupo = "";
			string descrip_grupo1 = "";
			string descrip_grupo2 = "";
			string toma_precio_producto = "0.00";
			float cuenta_meses_activados = 0;
			float ventatotal_ciclo = 0;
			NpgsqlConnection conexion; 
			conexion = new NpgsqlConnection (connectionString+nombrebd);
			try{
				conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand ();
				comando.CommandText = "SELECT to_char(osiris_erp_cobros_deta.fechahora_creacion,'yyyy-MM') AS anomes_actual,"+
					"SUM(osiris_erp_cobros_deta.cantidad_aplicada) AS totaldeproductos,"+
					"osiris_erp_cobros_deta.id_producto,osiris_productos.descripcion_producto,descripcion_grupo_producto,descripcion_grupo1_producto,descripcion_grupo2_producto "+
					"FROM osiris_erp_cobros_deta,osiris_productos,osiris_grupo_producto,osiris_grupo1_producto,osiris_grupo2_producto "+
					"WHERE osiris_erp_cobros_deta.id_producto =  osiris_productos.id_producto "+
					"AND osiris_productos.id_grupo_producto = osiris_grupo_producto.id_grupo_producto "+
					"AND osiris_productos.id_grupo1_producto = osiris_grupo1_producto.id_grupo1_producto "+
					"AND osiris_productos.id_grupo2_producto = osiris_grupo2_producto.id_grupo2_producto "+
					"AND osiris_erp_cobros_deta.eliminado = 'false' "+
					"AND osiris_productos.id_producto = '"+ idproducto_.Trim()+ "' "+ 
					queryanomesesrangofecha_+
					"GROUP BY to_char(osiris_erp_cobros_deta.fechahora_creacion,'yyyy-MM'),osiris_erp_cobros_deta.id_producto,osiris_productos.descripcion_producto,descripcion_grupo_producto,descripcion_grupo1_producto,descripcion_grupo2_producto "+
					"ORDER BY to_char(osiris_erp_cobros_deta.fechahora_creacion,'yyyy-MM');";
				//Console.WriteLine(comando.CommandText.ToString());
				NpgsqlDataReader lector = comando.ExecuteReader ();
				while (lector.Read()){
					descrip_producto = lector["descripcion_producto"].ToString();
					descrip_grupo = lector["descripcion_grupo_producto"].ToString();
					descrip_grupo1 = lector["descripcion_grupo1_producto"].ToString();
					descrip_grupo2 = lector["descripcion_grupo2_producto"].ToString();
					toma_precio_producto = "0"; //(string) lector["precioproducto"].ToString();
					if (lector["anomes_actual"].ToString().Trim() == ano_seleccionado+"-01"){
						//var_01_ene = float.Parse(lector["totaldeproductos"].ToString().Trim()) * float.Parse(lector["precioproducto"].ToString().Trim());
						//var_02_ene = float.Parse(lector["totaldeproductos"].ToString().Trim()) * float.Parse(lector["totalpreciocosto"].ToString().Trim());
						var_03_ene = lector["totaldeproductos"].ToString().Trim();
					}
					if (lector["anomes_actual"].ToString().Trim() == ano_seleccionado+"-02"){
						//var_01_feb = float.Parse(lector["totaldeproductos"].ToString().Trim()) * float.Parse(lector["precioproducto"].ToString().Trim());
						//var_02_feb = float.Parse(lector["totaldeproductos"].ToString().Trim()) * float.Parse(lector["totalpreciocosto"].ToString().Trim());
						var_03_feb = lector["totaldeproductos"].ToString().Trim();
					}
					if (lector["anomes_actual"].ToString().Trim() == ano_seleccionado+"-03"){
						//var_01_mar = float.Parse(lector["totaldeproductos"].ToString().Trim()) * float.Parse(lector["precioproducto"].ToString().Trim());
						//var_02_mar = float.Parse(lector["totaldeproductos"].ToString().Trim()) * float.Parse(lector["totalpreciocosto"].ToString().Trim());
						var_03_mar = lector["totaldeproductos"].ToString().Trim();
					}
					if (lector["anomes_actual"].ToString().Trim() == ano_seleccionado+"-04"){
						//var_01_abr = float.Parse(lector["totaldeproductos"].ToString().Trim()) * float.Parse(lector["precioproducto"].ToString().Trim());
						//var_02_abr = float.Parse(lector["totaldeproductos"].ToString().Trim()) * float.Parse(lector["totalpreciocosto"].ToString().Trim());
						var_03_abr = lector["totaldeproductos"].ToString().Trim();
					}
					if (lector["anomes_actual"].ToString().Trim() == ano_seleccionado+"-05"){
						//var_01_may = float.Parse(lector["totaldeproductos"].ToString().Trim()) * float.Parse(lector["precioproducto"].ToString().Trim());
						//var_02_may = float.Parse(lector["totaldeproductos"].ToString().Trim()) * float.Parse(lector["totalpreciocosto"].ToString().Trim());
						var_03_may = lector["totaldeproductos"].ToString().Trim();
					}
					if (lector["anomes_actual"].ToString().Trim() == ano_seleccionado+"-06"){
						//var_01_jun = float.Parse(lector["totaldeproductos"].ToString().Trim()) * float.Parse(lector["precioproducto"].ToString().Trim());
						//var_02_jun = float.Parse(lector["totaldeproductos"].ToString().Trim()) * float.Parse(lector["totalpreciocosto"].ToString().Trim());
						var_03_jun = lector["totaldeproductos"].ToString().Trim();
					}
					if ((string) lector["anomes_actual"] == ano_seleccionado+"-07"){
						//var_01_jul = float.Parse(lector["totaldeproductos"].ToString().Trim()) * float.Parse(lector["precioproducto"].ToString().Trim());
						//var_02_jul = float.Parse(lector["totaldeproductos"].ToString().Trim()) * float.Parse(lector["totalpreciocosto"].ToString().Trim());
						var_03_jul = lector["totaldeproductos"].ToString().Trim();
					}
					if ((string) lector["anomes_actual"] == ano_seleccionado+"-08"){
						//var_01_ago = float.Parse(lector["totaldeproductos"].ToString().Trim()) * float.Parse(lector["precioproducto"].ToString().Trim());
						//var_02_ago = float.Parse(lector["totaldeproductos"].ToString().Trim()) * float.Parse(lector["totalpreciocosto"].ToString().Trim());
						var_03_ago = lector["totaldeproductos"].ToString().Trim();
					}
					if ((string) lector["anomes_actual"] == ano_seleccionado+"-09"){
						//var_01_sep = float.Parse(lector["totaldeproductos"].ToString().Trim()) * float.Parse(lector["precioproducto"].ToString().Trim());
						//var_02_sep = float.Parse(lector["totaldeproductos"].ToString().Trim()) * float.Parse(lector["totalpreciocosto"].ToString().Trim());
						var_03_sep = lector["totaldeproductos"].ToString().Trim();
					}
					if ((string) lector["anomes_actual"] == ano_seleccionado+"-10"){
						//var_01_oct = float.Parse(lector["totaldeproductos"].ToString().Trim()) * float.Parse(lector["precioproducto"].ToString().Trim());
						//var_02_oct = float.Parse(lector["totaldeproductos"].ToString().Trim()) * float.Parse(lector["totalpreciocosto"].ToString().Trim());
						var_03_oct = lector["totaldeproductos"].ToString().Trim();
					}
					if ((string) lector["anomes_actual"] == ano_seleccionado+"-11"){
						//var_01_nov = float.Parse(lector["totaldeproductos"].ToString().Trim()) * float.Parse(lector["precioproducto"].ToString().Trim());
						//var_02_nov = float.Parse(lector["totaldeproductos"].ToString().Trim()) * float.Parse(lector["totalpreciocosto"].ToString().Trim());
						var_03_nov = lector["totaldeproductos"].ToString().Trim();
					}
					if ((string) lector["anomes_actual"] == ano_seleccionado+"-12"){
						//var_01_dic = float.Parse(lector["totaldeproductos"].ToString().Trim()) * float.Parse(lector["precioproducto"].ToString().Trim());
						//var_02_dic = float.Parse(lector["totaldeproductos"].ToString().Trim()) * float.Parse(lector["totalpreciocosto"].ToString().Trim());
						var_03_dic = lector["totaldeproductos"].ToString().Trim();
					}
				}

				if (this.checkbutton_ene_costos.Active == true){
					cuenta_meses_activados += 1;
				}
				if (this.checkbutton_feb_costos.Active == true){
					cuenta_meses_activados += 1;
				}
				if (this.checkbutton_mar_costos.Active == true){
					cuenta_meses_activados += 1;
				}
				if (this.checkbutton_abr_costos.Active == true){
					cuenta_meses_activados += 1;
				}
				if (this.checkbutton_may_costos.Active == true){
					cuenta_meses_activados += 1;
				}
				if (this.checkbutton_jun_costos.Active == true){
					cuenta_meses_activados += 1;
				}
				if (this.checkbutton_jul_costos.Active == true){
					cuenta_meses_activados += 1;
				}
				if (this.checkbutton_ago_costos.Active == true ){
					cuenta_meses_activados += 1;
				}
				if (this.checkbutton_sep_costos.Active == true ){
					cuenta_meses_activados += 1;
				}
				if (this.checkbutton_oct_costos.Active == true){									
					cuenta_meses_activados += 1;
				}
				if (this.checkbutton_nov_costos.Active == true){
					cuenta_meses_activados += 1;
				}
				if (this.checkbutton_dic_costos.Active == true){
					cuenta_meses_activados += 1;
				}

				if((float.Parse(var_03_ene)+float.Parse(var_03_feb)+float.Parse(var_03_mar)+
					float.Parse(var_03_abr)+float.Parse(var_03_may)+float.Parse(var_03_jun)+
					float.Parse(var_03_jul)+float.Parse(var_03_ago)+float.Parse(var_03_sep)+
					float.Parse(var_03_oct)+float.Parse(var_03_nov)+float.Parse(var_03_dic)) > 0){

					treeViewEngineResumen.AppendValues(	idproducto_,
													descrip_producto,
													" ",
													float.Parse(var_03_ene).ToString("F"),
													float.Parse(var_03_feb).ToString("F"),
													float.Parse(var_03_mar).ToString("F"),
													float.Parse(var_03_abr).ToString("F"),
													float.Parse(var_03_may).ToString("F"),
													float.Parse(var_03_jun).ToString("F"),
													float.Parse(var_03_jul).ToString("F"),
													float.Parse(var_03_ago).ToString("F"),
													float.Parse(var_03_sep).ToString("F"),
													float.Parse(var_03_oct).ToString("F"),
													float.Parse(var_03_nov).ToString("F"),
													float.Parse(var_03_dic).ToString("F"),
													(float.Parse(var_03_ene)+float.Parse(var_03_feb)+float.Parse(var_03_mar)+
													float.Parse(var_03_abr)+float.Parse(var_03_may)+float.Parse(var_03_jun)+
													float.Parse(var_03_jul)+float.Parse(var_03_ago)+float.Parse(var_03_sep)+
													float.Parse(var_03_oct)+float.Parse(var_03_nov)+float.Parse(var_03_dic)).ToString("F"),
													ventatotal_ciclo.ToString("F"),
													toma_precio_producto,
													float.Parse((string) Convert.ToString((float.Parse(var_03_ene)+float.Parse(var_03_feb)+float.Parse(var_03_mar)+
													float.Parse(var_03_abr)+float.Parse(var_03_may)+float.Parse(var_03_jun)+
													float.Parse(var_03_jul)+float.Parse(var_03_ago)+float.Parse(var_03_sep)+
													float.Parse(var_03_oct)+float.Parse(var_03_nov)+float.Parse(var_03_dic))/cuenta_meses_activados)).ToString("F"),
													descrip_grupo,
													descrip_grupo1,
													descrip_grupo2);
				}
			}catch (NpgsqlException ex){
				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
					MessageType.Error, 
					ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
				msgBoxError.Run ();
				msgBoxError.Destroy();
			}
			conexion.Close ();
		}

		void on_button_imprimir_costos_clicked(object sender, EventArgs args)
		{
			TreeIter iter;
			if (this.treeViewEngineResumen.GetIterFirst (out iter)){
				new osiris.imprime_consumo_productos (this.lista_resumen_productos,this.treeViewEngineResumen,this.ano_seleccionado);
			}else{
				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
					MessageType.Error, 
					ButtonsType.Close, "NO existen nada para imprimir");
				msgBoxError.Run ();
				msgBoxError.Destroy();
			}
		}

		void llenado_grupos_seleccionados()
		{
			NpgsqlConnection conexion; 
			conexion = new NpgsqlConnection (connectionString+nombrebd);

			// Verifica que la base de datos este conectada
			try{
				conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand ();

				comando.CommandText = "SELECT to_char(osiris_productos.id_producto,'999999999999') AS codProducto,"+
					"osiris_productos.descripcion_producto,to_char(precio_producto_publico,'99999999.99') AS preciopublico,"+
					"to_char(precio_producto_publico1,'99999999.99') AS preciopublico1,"+
					"aplicar_iva,to_char(porcentage_descuento,'999.99') AS porcentagesdesc,aplica_descuento,"+
					"descripcion_grupo_producto,descripcion_grupo1_producto,descripcion_grupo2_producto,to_char(costo_por_unidad,'999999999.99') AS costoproductounitario, "+
					"to_char(porcentage_ganancia,'99999.99') AS porcentageutilidad,to_char(costo_producto,'999999999.99') AS costoproducto, "+
					"osiris_grupo_producto.agrupacion "+
					"FROM osiris_productos,osiris_grupo_producto,osiris_grupo1_producto,osiris_grupo2_producto "+
					"WHERE osiris_productos.id_grupo_producto = osiris_grupo_producto.id_grupo_producto "+
					"AND osiris_productos.id_grupo1_producto = osiris_grupo1_producto.id_grupo1_producto "+
					"AND osiris_productos.id_grupo2_producto = osiris_grupo2_producto.id_grupo2_producto "+
					"AND cobro_activo = 'true' "+
					query_grupo_prod+
					"ORDER BY osiris_productos.id_grupo_producto,descripcion_producto; ";
				//Console.WriteLine(comando.CommandText.ToString());
				NpgsqlDataReader lector = comando.ExecuteReader ();					
				while (lector.Read()){
					treeViewEngineProdSelec.AppendValues ((string) lector["codProducto"] ,
						(string) lector["descripcion_producto"]);
				}
			}catch (NpgsqlException ex){
				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
					MessageType.Error, 
					ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
				msgBoxError.Run ();
				msgBoxError.Destroy();
			}
			conexion.Close ();					
		}

		void on_button_lista_prodcargos_clicked (object sender, EventArgs args)
		{
			NpgsqlConnection conexion; 
			conexion = new NpgsqlConnection (connectionString+nombrebd);

			// Verifica que la base de datos este conectada
			try{
				conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand ();
				comando.CommandText = "SELECT to_char(osiris_productos.id_producto,'999999999999') AS codProducto,"+
					"osiris_productos.descripcion_producto,to_char(precio_producto_publico,'99999999.99') AS preciopublico,"+
					"to_char(precio_producto_publico1,'99999999.99') AS preciopublico1,"+
					"aplicar_iva,to_char(osiris_erp_cobros_deta.porcentage_descuento,'999.99') AS porcentagesdesc,aplica_descuento,"+
					"descripcion_grupo_producto,descripcion_grupo1_producto,descripcion_grupo2_producto,to_char(costo_por_unidad,'999999999.99') AS costoproductounitario, "+
					"to_char(porcentage_ganancia,'99999.99') AS porcentageutilidad,to_char(costo_producto,'999999999.99') AS costoproducto, "+
					"osiris_grupo_producto.agrupacion "+
					"FROM osiris_productos,osiris_grupo_producto,osiris_grupo1_producto,osiris_grupo2_producto,osiris_erp_cobros_deta "+
					"WHERE osiris_productos.id_grupo_producto = osiris_grupo_producto.id_grupo_producto "+
					"AND osiris_productos.id_grupo1_producto = osiris_grupo1_producto.id_grupo1_producto "+
					"AND osiris_productos.id_grupo2_producto = osiris_grupo2_producto.id_grupo2_producto "+
					"AND osiris_erp_cobros_deta.id_producto = osiris_productos.id_producto " +
					//"AND cobro_activo = 'true' "+
					"AND osiris_erp_cobros_deta.eliminado = 'false' "+
					//"AND osiris_productos.id_grupo_producto IN ('10','11','12','13','14','15','16','17','21') " +
					query_grupo_prod+
					"AND to_char(osiris_erp_cobros_deta.fechahora_creacion,'yyyy-MM-dd') >= '"+entry_fecha_inicio.Text+"' "+
					"AND to_char(osiris_erp_cobros_deta.fechahora_creacion,'yyyy-MM-dd') <= '"+entry_fecha_termino.Text+"' "+
					"GROUP BY osiris_productos.id_producto,osiris_productos.descripcion_producto,precio_producto_publico,precio_producto_publico1,aplicar_iva,osiris_erp_cobros_deta.porcentage_descuento,aplica_descuento," +
					"descripcion_grupo_producto,descripcion_grupo1_producto,descripcion_grupo2_producto,costo_por_unidad,porcentage_ganancia,costo_producto,osiris_grupo_producto.agrupacion; ";
				//"ORDER BY osiris_productos.id_grupo_producto,descripcion_producto; ";
				//Console.WriteLine(comando.CommandText.ToString());
				NpgsqlDataReader lector = comando.ExecuteReader ();					
				while (lector.Read()){
					treeViewEngineProdSelec.AppendValues ((string) lector["codProducto"] ,
						(string) lector["descripcion_producto"]);
				}
			}catch (NpgsqlException ex){
				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
					MessageType.Error, 
					ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
				msgBoxError.Run ();
				msgBoxError.Destroy();
			}
			conexion.Close ();			
		}

		void on_button_quitar_producto_clicked (object o, EventArgs args)
		{
			TreeIter iter;
			TreeModel model;
			if (lista_producto_seleccionados.Selection.GetSelected (out model, out iter)) {
				treeViewEngineProdSelec.Remove (ref iter);
			}
		}

		void on_button_limpiar_clicked(object o, EventArgs args)
		{
			treeViewEngineProdSelec.Clear();
		}

		void crea_colums_treeview(object [] paraobj_, string [] nameobject_,string [,] args_colums)
		{
			Gtk.CellRendererText text;
			Gtk.CellRendererToggle toggle;

			treeviewobject = (object) paraobj_[0] as Gtk.TreeView;
			treeViewEngine = (object) paraobj_[1] as Gtk.ListStore;

			foreach (TreeViewColumn tvc in treeviewobject.Columns)
				treeviewobject.RemoveColumn(tvc);

			treeViewEngine = new ListStore(typeof(string),typeof(string),typeof(string),typeof(string),typeof(string),
											typeof(string),typeof(string),typeof(string),typeof(string),typeof(string),
											typeof(string),typeof(string),typeof(string),typeof(string),typeof(string),
											typeof(string),typeof(string),typeof(string),typeof(string),typeof(string),
											typeof(string),typeof(string),typeof(string),typeof(string),typeof(string));
			treeviewobject.Model = treeViewEngine;
			treeviewobject.RulesHint = true;
			treeviewobject.Selection.Mode = SelectionMode.Multiple;
			if(args_colums.GetUpperBound(0) >= 0){
				for (int colum_field = 0; colum_field <= args_colums.GetUpperBound (0); colum_field++) {
					if((string) args_colums [colum_field, 1] == "text"){
						// column for holiday names
						text = new CellRendererText ();
						text.Xalign = 0.0f;
						columns.Add (text);
						TreeViewColumn column0 = new TreeViewColumn((string) args_colums [colum_field, 0], text,"text", colum_field);
						column0.Resizable = true;
						//column0.SortColumnId = colum_field;
						treeviewobject.InsertColumn (column0, colum_field);					
					}
					if((string) args_colums [colum_field, 1] == "toogle"){

					}
				}
			}
			if (nameobject_[0] == "lista_producto_seleccionados") {
				lista_producto_seleccionados = treeviewobject;
			}
			if (nameobject_[1] == "treeViewEngineProdSelec") {
				treeViewEngineProdSelec = treeViewEngine;
			}
			if (nameobject_[0] == "lista_resumen_productos") {
				lista_resumen_productos = treeviewobject;
			}
			if (nameobject_[1] == "treeViewEngineResumen") {
				treeViewEngineResumen = treeViewEngine;
			}
		}

		void crea_treeview_prodselec()
		{
			object[] paraobj = { lista_producto_seleccionados, treeViewEngineProdSelec};
			string[] nameobject = {"lista_producto_seleccionados", "treeViewEngineProdSelec"};
			string[,] coltreeview = { 
				{ "Codigo Prod.", "text" },
				{ "Descripcion Producto", "text" }
			};
			crea_colums_treeview (paraobj, nameobject, coltreeview);
		}

		void crea_treeview_resumen_mensual()
		{
			object[] paraobj = { lista_resumen_productos, treeViewEngineResumen};
			string[] nameobject = {"lista_resumen_productos", "treeViewEngineResumen"};
			string[,] coltreeview = { 
				{ "Codigo Prod.", "text" },
				{ "Descripcion Producto", "text" },
				{"Cos.Unitario","text"},
				{"ENE","text"},
				{"FEB","text"},
				{"MAR","text"},
				{"ABR","text"},
				{"MAY","text"},
				{"JUN","text"},
				{"JUL","text"},
				{"AGO","text"},
				{"SEP","text"},
				{"OCT","text"},
				{"NOV","text"},
				{"DIC","text"},
				{"Total Aplicado","text"},
				{"Total $ Venta","text"},
				{"Precio $ Venta","text"},
				{"Promedio Consumo","text"},
				{"Grupo Producto","text"},
				{"Grupo1 Producto","text"},
				{"Grupo2 Producto","text"},
			};
			crea_colums_treeview (paraobj, nameobject, coltreeview);

		}

		// cierra ventanas emergentes
		void on_cierraventanas_clicked (object sender, EventArgs args)
		{
			Widget win = (Widget) sender;
			win.Toplevel.Destroy();
		}
	}	
}