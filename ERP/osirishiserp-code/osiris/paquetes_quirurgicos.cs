//////////////////////////////////////////////////////////
// project created on 24/10/2006 at 10:20 a
// Sistema Hospitalario OSIRIS
// Monterrey - Mexico
//
// Autor    	: Ing. Juan Antonio Peña Gonzalez (gjuanzz@gmail.com) 
//				  Ing. Daniel Olivares C. (Adecuaciones y mejoras) arcangeldoc@openmailbox.org
//					Exportar a .ODS  23/12/2015 y permisos
//					Mejoras y adecuaciones Jul 2016
//                  Mejoras y Adecuaciones Feb 2017
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
// Programa		: paquetes_quirurgicos.cs
// Proposito	: Crear paquetes de productos que se aplican en cirugia
// Objeto		: paquetes_quirurgicos.cs
//////////////////////////////////////////////////////////
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
	public class paquetes_cirugias
	{
		// Boton general para salir de las ventanas Todas la ventanas en glade este boton debe estra declarado identico
		[Widget] Gtk.Button button_salir;
		
		// Para todas las busquedas este es el nombre asignado se declara una vez
		[Widget] Gtk.Entry entry_expresion;
		[Widget] Gtk.Button button_selecciona;
		[Widget] Gtk.Button button_buscar_busqueda;
		
		// Declarando ventana principal
		[Widget] Gtk.Window paquetes;
		[Widget] Gtk.Entry entry_id_cirugia;
		[Widget] Gtk.Entry entry_cirugia;
		[Widget] Gtk.Entry entry_id_especialidad; 
		[Widget] Gtk.Entry entry_descripcion_especialidad;
		[Widget] Gtk.Entry entry_dias_internamiento;
		[Widget] Gtk.Entry entry_deposito_minimo;
		[Widget] Gtk.Entry entry_precio_publico;
		[Widget] Gtk.CheckButton checkbutton_nueva_cirugia;
		[Widget] Gtk.CheckButton checkbutton_copia_productos;
		[Widget] Gtk.CheckButton checkbutton_cambiar_cirugia;
		[Widget] Gtk.CheckButton checkbutton_paquete_sino = null;
		[Widget] Gtk.CheckButton checkbutton_asignapqcx_empre = null;
		[Widget] Gtk.ComboBox combobox_tipo_paciente = null;
		[Widget] Gtk.Entry entry_id_empaseg = null;
		[Widget] Gtk.Entry entry_nombre_empaseg = null;
		[Widget] Gtk.Button button_busca_empresa = null;
		[Widget] Gtk.CheckButton checkbutton_pq_checkup = null;
		[Widget] Gtk.CheckButton checkbutton_motivo_ingreso = null;
		[Widget] Gtk.TreeView lista_de_servicios;
		[Widget] Gtk.Button button_quitar_aplicados;
		[Widget] Gtk.Button button_actualizar;
		[Widget] Gtk.Button button_buscar_cirugia;
		[Widget] Gtk.Button button_buscar_especialidad;
		[Widget] Gtk.Button button_selec_id;
		[Widget] Gtk.Button button_graba_paquete;
		[Widget] Gtk.Button button_limpiar;
		[Widget] Gtk.Button button_busca_producto;
		[Widget] Gtk.Button button_procedimiento_cobrz;
		[Widget] Gtk.Button button_paquete_sin_precios;
		[Widget] Gtk.Button button_copia_procedimiento;
		[Widget] Gtk.Button button_export_ods = null;
		[Widget] Gtk.Entry entry_subtotal_al_15;
		[Widget] Gtk.Entry entry_subtotal_al_0;
		[Widget] Gtk.Entry entry_total_iva;
		[Widget] Gtk.Entry entry_subtotal;
		[Widget] Gtk.Entry entry_total;
				
		//Declarando ventena de carga de folio
		[Widget] Gtk.Window carga_folio;
		[Widget] Gtk.Entry entry_folio;
		[Widget] Gtk.Button button_carga_productos;
		[Widget] Gtk.Statusbar statusbar_caja = null;
		
		/////// Ventana Busqueda de productos\\\\\\\\
		[Widget] Gtk.Window busca_producto;
		[Widget] Gtk.TreeView lista_de_producto = null;
		[Widget] Gtk.ComboBox combobox_tipo_admision = null;		
		[Widget] Gtk.Entry entry_cantidad_aplicada = null;
						
		TreeStore treeViewEngineBusca2;
		ListStore treeViewEngineServicio;
		
		//private ArrayList arraycargosrealizados;
		
		// Declaracion de variables publicas
		string nommedico;
		float valoriva;		
	
		int id_tipopaciente = 0;
		string idempresa = "1";
		string idaseguradora = "1";

		//Variables de admision
		int idtipointernamiento = 0;
		string descripinternamiento = "";
		bool nuevacirugia = false;
		bool copiaproductos = false;
		string tipobusqueda = "";
		bool tienepaquete = false;		
		// Sumas Totales para los calculos
		float subtotal_al_15;
		float subtotal_al_0;
		float total_iva;
		float sub_total;
		float totaldescuento;		
		bool aplico_cargos = false;
		string LoginEmpleado;			
		string connectionString;
		string nombrebd;
					
		//Declaracion de ventana de error
		protected Gtk.Window MyWinError;
		protected Gtk.Window MyWin;

		string[] args_args = {""};
		int[] args_id_array = {0,1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20};
		
		class_conexion conexion_a_DB = new class_conexion();
		class_public classpublic = new class_public();
		class_buscador classfind_data = new class_buscador();
		
		public paquetes_cirugias(string LoginEmp, string NomEmpleado, string AppEmpleado, string ApmEmpleado, string nombrebd_,string idcirugiapq_ ) 
		{
			LoginEmpleado = LoginEmp;
			connectionString = conexion_a_DB._url_servidor+conexion_a_DB._port_DB+conexion_a_DB._usuario_DB+conexion_a_DB._passwrd_user_DB;
			nombrebd = conexion_a_DB._nombrebd;			
			valoriva = float.Parse(classpublic.ivaparaaplicar);
			
			Glade.XML gxml = new Glade.XML (null, "registro_admision.glade", "paquetes", null);
			gxml.Autoconnect (this);
	        paquetes.Show();			
			button_selec_id.Clicked += new EventHandler(on_selec_id_clicked);
			entry_id_cirugia.KeyPressEvent += onKeyPressEvent_enter_id;
			button_procedimiento_cobrz.Clicked += new EventHandler(on_button_procedimiento_cobrz_clicked);
			button_paquete_sin_precios.Clicked += new EventHandler(on_button_paquete_sin_precios_clicked);
			button_actualizar.Clicked += new EventHandler(on_button_actualizar_clicked);
			button_graba_paquete.Clicked += new EventHandler(on_button_graba_paquete_clicked);
			button_buscar_cirugia.Clicked += new EventHandler(on_button_buscar_cirugia_clicked);
			button_buscar_especialidad.Clicked += new EventHandler(on_button_buscar_especialidad_clicked);
			button_copia_procedimiento.Clicked += new EventHandler(on_button_copia_procedimiento_clicked);
			button_quitar_aplicados.Clicked += new EventHandler(on_button_quitar_aplicados_clicked);
			button_busca_producto.Clicked += new EventHandler(on_button_busca_producto_clicked);
			button_limpiar.Clicked += new EventHandler(on_button_limpiar_clicked);
			checkbutton_copia_productos.Clicked += new EventHandler(on_checkbutton_copia_productos_clicked);
			checkbutton_nueva_cirugia.Clicked += new EventHandler(on_checkbutton_nueva_cirugia_clicked);
			checkbutton_cambiar_cirugia.Clicked += new EventHandler(on_checkbutton_cambiar_cirugia_clicked);
			checkbutton_asignapqcx_empre.Clicked += new EventHandler(on_checkbutton_asignapqcx_empre_clicked);
			button_export_ods.Clicked += new EventHandler(on_button_export_ods_clicked);
			button_salir.Clicked += new EventHandler(on_cierraventanas_clicked);
			button_busca_empresa.Clicked += new EventHandler(on_button_busca_empresa_clicked);
			
			// Creacion de los treeview para la pantalla	
			crea_treeview_servicio();
			entry_id_cirugia.Text = "0";
			combobox_tipo_paciente.Sensitive = false;
			entry_id_empaseg.Sensitive = false;
			entry_nombre_empaseg.Sensitive = false;
			button_busca_empresa.Sensitive = false;

			llenado_combobox(1,"",combobox_tipo_paciente,"sql","SELECT * FROM osiris_his_tipo_pacientes WHERE activo_tipo_paciente = 'true' ORDER BY descripcion_tipo_paciente;","descripcion_tipo_paciente","id_tipo_paciente",args_args,args_id_array);
			
			statusbar_caja.Pop(0);
			statusbar_caja.Push(1, "login: "+LoginEmpleado+"  |Usuario: "+NomEmpleado+" "+AppEmpleado+" "+ApmEmpleado);
			statusbar_caja.HasResizeGrip = false;
	    	
			// pone color a los entry
			entry_total.ModifyBase(StateType.Normal, new Gdk.Color(54,180,221));
			entry_total_iva.ModifyBase(StateType.Normal, new Gdk.Color(254,253,152));

			if (idcirugiapq_ != "1") {
				entry_id_cirugia.Text = idcirugiapq_;
				llenado_de_cirugia(idcirugiapq_);
			}
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
				Console.WriteLine ((string)args_colums [colum_field, 1]);
				if ((string)args_colums [colum_field, 1] == "text") {
					t [colum_field] = typeof(string);
				}
				if ((string)args_colums [colum_field, 1] == "toogle") {
					t [colum_field] = typeof(bool);
				}
				if ((string)args_colums [colum_field, 1] == "float") {
					t [colum_field] = typeof(float);
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
						if ((string)args_colums [colum_field, 1] == "float") {
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
							//toggle.Toggled += new ToggledHandler (selecciona_fila);
						}
					}
				}
				if (nombre_treeview_ == "lista_de_servicios"){
					//treeviewobject.RowActivated += on_button_ligar_erp_clicked;
					lista_de_servicios = treeviewobject;
					treeViewEngineServicio = treeViewEngine;
				}
			}
		}
		
		void on_button_export_ods_clicked(object sender, EventArgs args)
		{
			if((string) classpublic.lee_registro_de_tabla("osiris_empleado","exportar_paqqx","WHERE exportar_paqqx = 'true' AND login_empleado = '"+LoginEmpleado+"' ","exportar_paqqx","bool") == "True"){
				if ((string) entry_id_cirugia.Text == "" ){	
					MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
					MessageType.Error, 
					ButtonsType.Close, "Debe de llenar el campo de cirugia con uno \n"+
								"existente para que el  se muestre \n"+"o no a pulsado el boton ''Seleccionar''");
					msgBoxError.Run ();				msgBoxError.Destroy();
				}else{
					string query_sql = "SELECT descripcion_producto,osiris_his_tipo_admisiones.descripcion_admisiones, "+
								"id_empleado,osiris_his_cirugias_deta.eliminado,osiris_productos.aplicar_iva,osiris_his_cirugias_deta.id_tipo_admisiones,  "+
								"to_char(osiris_his_cirugias_deta.id_producto,'999999999999') AS idproducto, "+
								"to_char(osiris_his_cirugias_deta.cantidad_aplicada,'99999.99') AS cantidadaplicada, "+
								"to_char(osiris_productos.precio_producto_publico,'99999999.99') AS preciopublico," +
								"to_char(osiris_productos.precio_producto_publico * osiris_his_cirugias_deta.cantidad_aplicada,'9999999.99') AS totalpublico,"+
								"to_char(osiris_productos.costo_por_unidad,'999999999.99') AS costounitario, "+
								"to_char(osiris_productos.porcentage_ganancia,'99999.99') AS porceutilidad, "+
								"to_char(osiris_productos.costo_por_unidad * osiris_his_cirugias_deta.cantidad_aplicada,'9999999.99') AS totalcosto,"+
								"to_char(osiris_productos.costo_producto,'999999999.99') AS costoproducto, "+
								"to_char(osiris_his_cirugias_deta.fechahora_creacion,'dd-MM-yyyy HH:mi:ss') AS fechcreacion ,"+
								"to_char(osiris_his_cirugias_deta.id_secuencia,'9999999999') AS secuencia,"+
								"osiris_his_cirugias_deta.id_tipo_cirugia AS idcirugia,osiris_his_tipo_cirugias.descripcion_cirugia "+
								"FROM osiris_his_cirugias_deta,osiris_productos,osiris_his_tipo_cirugias,osiris_his_tipo_admisiones "+
								"WHERE osiris_his_cirugias_deta.id_producto = osiris_productos.id_producto "+
								"AND osiris_his_cirugias_deta.id_tipo_cirugia = osiris_his_tipo_cirugias.id_tipo_cirugia "+
								"AND osiris_his_cirugias_deta.eliminado = false "+ 
								"AND osiris_his_cirugias_deta.id_tipo_admisiones = osiris_his_tipo_admisiones.id_tipo_admisiones "+
								"AND osiris_his_cirugias_deta.id_tipo_cirugia = '"+entry_id_cirugia.Text.ToString().Trim()+"' "+
							    "AND osiris_his_cirugias_deta.eliminado = 'false' "+
								"ORDER BY osiris_productos.descripcion_producto,to_char(osiris_his_cirugias_deta.fechahora_creacion,'yyyy-mm-dd HH:mm:ss');";
					
					string[] args_names_field = {"idcirugia","descripcion_cirugia","idproducto","descripcion_producto","cantidadaplicada","costounitario","totalcosto","preciopublico","totalpublico","porceutilidad","aplicar_iva"};
					string[] args_type_field = {"float","string","string","string","float","float","float","float","float","float","string"};
					string[] args_field_text = {""};
					string[] args_more_title = {""};
					string[,] args_formulas = {{"6","=SUM(G2:G"},{"8","=SUM(I2:I"}};
					string[,] args_width = {{"1","5cm"},{"3","9cm"}};
					new osiris.class_traslate_spreadsheet(query_sql,args_names_field,args_type_field,false,args_field_text,"",false,args_more_title,args_formulas,args_width);						
				}
			}else{
				MessageDialog msgBox = new MessageDialog (MyWin,DialogFlags.Modal,
									MessageType.Info,ButtonsType.Ok,"No tiene Permiso para esta Opcion");
				msgBox.Run ();msgBox.Destroy();
			}
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
			ComboBox onComboBoxChanged = sender as ComboBox;
			if (sender == null){	return; }
			TreeIter iter;
			if (onComboBoxChanged.GetActiveIter (out iter)){
				switch (onComboBoxChanged.Name.ToString()){	
				case "combobox_tipo_paciente":
					//tipopaciente = (string) onComboBoxChanged.Model.GetValue(iter,0);
					id_tipopaciente = (int) onComboBoxChanged.Model.GetValue(iter,1);
					break;
				}
			}
		}

		void on_button_busca_empresa_clicked(object sender, EventArgs args)
		{
			// diferenciar el tipo de busqueda empresa o aseguradora
			//id_tipopaciente = 400 asegurados
			//id_tipopaciente = 102 empresas
			//id_tipopaciente = 500 municipio
			//id_tipopaciente = 100 DIF
			//id_tipopaciente = 600 Sindicato
			// Los parametros de del SQL siempre es primero cuando busca todo y la otra por expresion
			// la clase recibe tambien el orden del query
			// es importante definir que tipo de busqueda es para que los objetos caigan ahi mismo

			if(id_tipopaciente == 400 ){
				object[] parametros_objetos = {entry_id_empaseg,entry_nombre_empaseg};
				string[] parametros_sql = {"SELECT * FROM osiris_aseguradoras WHERE activa = 'true' "};
				string[] parametros_string = {};
				string[,] args_buscador1 = {{"ASEGURADORA","AND descripcion_aseguradora LIKE '%","%' "},
					{"ID ASEGURADORA","AND id_aseguradora = '","' "}};
				string[,] args_buscador2 = {{"ID ASEGURADORA","AND id_aseguradora = '","' "},
					{"ASEGURADORA","AND descripcion_aseguradora LIKE '%","%' "}};
				string[,] args_orderby = {{"",""}};
				classfind_data.buscandor(parametros_objetos,parametros_sql,parametros_string,"find_aseguradoras_citaqx",0,args_buscador1,args_buscador2,args_orderby);
				idaseguradora = entry_id_empaseg.Text.ToString().Trim();
				idempresa = "1";				
			}else{
				Console.WriteLine (id_tipopaciente.ToString ());
				object[] parametros_objetos = {entry_id_empaseg,entry_nombre_empaseg};
				string[] parametros_sql = {"SELECT * FROM osiris_empresas WHERE id_tipo_paciente = '"+id_tipopaciente.ToString().Trim()+"' AND empresa_activa = 'true' "};
				string[] parametros_string = {};
				string[,] args_buscador1 = {{"EMPRESA","AND descripcion_empresa LIKE '%","%' "},
					{"ID EMPRESA","AND id_empresa = '","' "}};
				string[,] args_buscador2 = {{"ID EMPRESA","AND id_empresa = '","' "},
					{"EMPRESA","AND descripcion_empresa LIKE '%","%' "}};
				string[,] args_orderby = {{"",""}};
				classfind_data.buscandor(parametros_objetos,parametros_sql,parametros_string,"find_empresa_citaqx",0,args_buscador1,args_buscador2,args_orderby);
				idempresa = entry_id_empaseg.Text.ToString().Trim();
				idaseguradora = "1";
			}
		}
		
		void on_selec_id_clicked(object sender, EventArgs args)
		{
			if(entry_id_cirugia.Text  == "" || entry_id_cirugia.Text == " "){
				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
				MessageType.Error,ButtonsType.Close, 
				"Debe de llenar el campo de id cirugia con uno \n"+"existente para que los datos se muestren \n");
				msgBoxError.Run ();			msgBoxError.Destroy();
			}else{
				//Console.WriteLine ("on_selec_id selecciono el ID");
				llenado_de_cirugia(entry_id_cirugia.Text );
			}
		}
		
		void on_button_actualizar_clicked(object sender, EventArgs args)
		{
			if(entry_id_cirugia.Text == "" || entry_id_cirugia.Text  == " " ){
				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
				MessageType.Error,ButtonsType.Close, 
				"Debe de llenar el campo de id cirugia con uno \n"+"existente para que los datos se muestren \n");
				msgBoxError.Run ();			msgBoxError.Destroy();
			}else{
				//Console.WriteLine ("button_actualizar selecciono el ID");
				llenado_de_cirugia( entry_id_cirugia.Text );
			}
		}
		
		void onComboBoxChanged_tipo_admision (object sender, EventArgs args)
		{
	    		ComboBox combobox_tipo_admision = sender as ComboBox;
			if (sender == null){
	    		return;
			}
	  		TreeIter iter;
	  		if (combobox_tipo_admision.GetActiveIter (out iter)){
		    	idtipointernamiento = (int) combobox_tipo_admision.Model.GetValue(iter,1);
		    	descripinternamiento = (string) combobox_tipo_admision.Model.GetValue(iter,0);
	     	}
		}
		
		void on_button_procedimiento_cobrz_clicked(object sender, EventArgs args)
		{
			if((string) classpublic.lee_registro_de_tabla("osiris_empleado","acceso_impr_pqprecios","WHERE acceso_impr_pqprecios = 'true' AND login_empleado = '"+LoginEmpleado+"' ","acceso_impr_pqprecios","bool") == "True"){
				if ((string) entry_id_cirugia.Text == "" ){	
					MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
					MessageType.Error, 
					ButtonsType.Close, "Debe de llenar el campo de cirugia con uno \n"+
								"existente para que el  se muestre \n"+"o no a pulsado el boton ''Seleccionar''");
					msgBoxError.Run ();				msgBoxError.Destroy();
				}else{
					new paquetes_reporte (int.Parse(entry_id_cirugia.Text),entry_cirugia.Text," ",nombrebd,"paquetes",
						entry_deposito_minimo.Text.Trim(),entry_dias_internamiento.Text.Trim(),"","","","","",true,"0");   // rpt_proc_cobranza.cs				
				}
			}else{
				MessageDialog msgBox = new MessageDialog (MyWinError,DialogFlags.Modal,
									MessageType.Info,ButtonsType.Ok,"No tiene Permiso para esta Opcion");
				msgBox.Run ();msgBox.Destroy();
			}
		}
		
		void on_button_paquete_sin_precios_clicked(object sender, EventArgs args)
		{
			if ((string) entry_id_cirugia.Text == "" ){	
				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
				MessageType.Error, 
				ButtonsType.Close, "Debe de llenar el campo de cirugia con uno \n"+
							"existente para que el  se muestre \n"+"o no a pulsado el boton ''Seleccionar''");
				msgBoxError.Run ();				msgBoxError.Destroy();
			}else{
				new paquetes_reporte (int.Parse(entry_id_cirugia.Text),entry_cirugia.Text," ",nombrebd,"paquetes",
					entry_deposito_minimo.Text.Trim(),entry_dias_internamiento.Text.Trim(),"","","","","",false,"0");   // rpt_proc_cobranza.cs				
			}
		}
						
		void on_button_graba_paquete_clicked(object sender, EventArgs args)
		{
			if(entry_cirugia.Text.Trim() == "" || entry_id_especialidad.Text.Trim() == "" || entry_dias_internamiento.Text.Trim() == "" ||
			  entry_precio_publico.Text.Trim() == "" || entry_deposito_minimo.Text.Trim() == "") 
			{
				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,MessageType.Info,
											ButtonsType.Close,"verifique que no existan campos en blanco");
				msgBoxError.Run ();					msgBoxError.Destroy();
			}else{
				MessageDialog msgBox = new MessageDialog (MyWin,DialogFlags.Modal,MessageType.Question,ButtonsType.YesNo,"¿ Desea grabar o actualizar esta infomacion ?");
				ResponseType miResultado = (ResponseType)msgBox.Run ();
				msgBox.Destroy();
 				if (miResultado == ResponseType.Yes){
					if(nuevacirugia == true) {
						crea_nuevacirugia();
						guarda_productos();
					}else{
						actualiza_datos_cirugia();
					}
       			}else{///termina el proceso de nuevacirugia == false
	 				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,MessageType.Info,
	 											ButtonsType.Close, "No grabo, ya que NO CARGO NADA");
					msgBoxError.Run ();			msgBoxError.Destroy();
	 			}
	 		}
		}
		
		void on_checkbutton_cambiar_cirugia_clicked(object sender, EventArgs args)
		{
			if(checkbutton_cambiar_cirugia.Active == true){
				this.entry_cirugia.IsEditable = true;
				this.entry_cirugia.ModifyText(StateType.Normal, new Gdk.Color(16,35,222));
				this.entry_id_cirugia.Sensitive = false;
			}else{
				this.entry_cirugia.IsEditable = false;
				this.entry_cirugia.ModifyText(StateType.Normal, new Gdk.Color(0,0,0));
				this.entry_id_cirugia.Sensitive = true;
			}
		}

		void on_checkbutton_asignapqcx_empre_clicked(object sender, EventArgs args)
		{
			combobox_tipo_paciente.Sensitive = checkbutton_asignapqcx_empre.Active;
			entry_id_empaseg.Sensitive = checkbutton_asignapqcx_empre.Active;
			entry_nombre_empaseg.Sensitive = checkbutton_asignapqcx_empre.Active;
			button_busca_empresa.Sensitive = checkbutton_asignapqcx_empre.Active;
		}
		
		void crea_nuevacirugia()
		{
			string[,] parametros;
			object[] paraobj;
			if(id_tipopaciente != 400 ){
				idempresa = entry_id_empaseg.Text.ToString().Trim();
				idaseguradora = "1";
			}else{	
				idaseguradora = entry_id_empaseg.Text.ToString().Trim();
				idempresa = "1";				
			}
			try{
				string idnumerocirugia = classpublic.lee_ultimonumero_registrado("osiris_his_tipo_cirugias","id_tipo_cirugia","");
				parametros = new string[,] {
										{ "id_tipo_cirugia,", "'" + idnumerocirugia + "'," },
										{ "descripcion_cirugia,", "'" + entry_cirugia.Text.ToUpper().Trim() + "'," },
										{ "tiene_paquete,", "'" + checkbutton_paquete_sino.Active.ToString() + "'," },
										{ "id_especialidad,", "'" + entry_id_especialidad.Text.ToString().Trim() + "'," },
										{ "fechahora_creacion,", "'" + DateTime.Now.ToString ("yyyy-MM-dd HH:mm:ss") + "'," },
										{ "id_quien_creo,", "'" + LoginEmpleado + "'," },
										{ "paquete_checkup", "'" + checkbutton_pq_checkup.Active.ToString() + "'," },
										};
				paraobj = new object[] { entry_id_cirugia };
				new osiris.insert_registro ("osiris_his_tipo_cirugias", parametros, paraobj);

				parametros = new string[,] {
										{ "id_empresa,", "'" +idempresa+ "'," },
										{ "id_aseguradora,", "'" + idaseguradora + "'," },
										{ "id_tipo_paciente,", "'" + id_tipopaciente.ToString().Trim() + "'," },
										{ "activo,", "'" + "true" + "'," },
										{ "valor_paquete,", "'" + float.Parse(entry_total.Text).ToString().Trim() + "'," },
										{ "id_tipo_cirugia,", "'" + idnumerocirugia + "'," },
										{ "precio_de_venta,", "'"+ float.Parse(entry_precio_publico.Text).ToString().Trim() + "'," },
										{ "deposito_minimo,", "'" + float.Parse(entry_deposito_minimo.Text).ToString().Trim() + "'," },
										{ "dias_de_internamiento", "'" + int.Parse(entry_dias_internamiento.Text).ToString().Trim() + "'," }
									};
				paraobj = new object[] { entry_id_cirugia };
				new osiris.insert_registro ("osiris_erp_preciospaquetes", parametros, paraobj);
			}catch{
				
			}
		}
		
		void actualiza_datos_cirugia()
		{
			if(id_tipopaciente != 400 ){
				idempresa = entry_id_empaseg.Text.ToString().Trim();
				idaseguradora = "1";
			}else{	
				idaseguradora = entry_id_empaseg.Text.ToString().Trim();
				idempresa = "1";				
			}
			try{
				string[,] parametros = {{ "tiene_paquete = ", "'"+checkbutton_paquete_sino.Active.ToString()+"'," },
										{ "valor_paquete = ", "'" + float.Parse(entry_total.Text).ToString().Trim() + "'," },
										{ "id_especialidad = ", "'" + entry_id_especialidad.Text.ToString().Trim() + "'," },
										{ "descripcion_cirugia =", "'" + entry_cirugia.Text.ToUpper() + "'," },
										{ "dias_internamiento = ", "'" + int.Parse(entry_dias_internamiento.Text).ToString().Trim() + "'," },
										{ "deposito_minimo = ", "'" + float.Parse(entry_deposito_minimo.Text).ToString().Trim() + "'," },
										{ "id_empresa = ", "'" + idempresa.ToString().Trim() + "'," },
										{ "id_aseguradora = ", "'" + idaseguradora.ToString().Trim() + "'," },
										{ "precio_de_venta = ", "'" + float.Parse(entry_precio_publico.Text).ToString().Trim() + "' " },
										{ "WHERE id_tipo_cirugia = '", entry_id_cirugia.Text.Trim()+ "';" }
									};
				object[] paraobj = { entry_id_cirugia };
				new osiris.update_registro ("osiris_his_tipo_cirugias", parametros, paraobj);


				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,MessageType.Info,
											ButtonsType.Close, "Los datos se guardaron con EXITO");
				msgBoxError.Run ();			msgBoxError.Destroy();
				checkbutton_nueva_cirugia.Active = false;
				checkbutton_copia_productos.Active = false;
				limpia_valores();
				llenado_de_cirugia(entry_id_cirugia.Text.ToString().Trim());
			}catch{
				
			}
		}
		
		void guarda_productos()
		{
			TreeIter iter;
			string[,] parametros;
			object[] paraobj;
			if((int) int.Parse(entry_id_cirugia.Text) > 0){  // Validando que seleccione un folio de atencion
				if (treeViewEngineServicio.GetIterFirst (out iter)){
					if ((bool)lista_de_servicios.Model.GetValue (iter,10) == false){
						//Console.WriteLine("leeo primer linea"+(string) lista_de_servicios.Model.GetValue(iter,2));
						parametros = new string[,] {{"id_producto,","'"+long.Parse((string) lista_de_servicios.Model.GetValue(iter,2)).ToString().Trim()+"',"},
												{"id_tipo_cirugia,","'"+entry_id_cirugia.Text.ToString().Trim()+"',"},
												{"cantidad_aplicada,","'"+lista_de_servicios.Model.GetValue(iter,1).ToString().Trim()+"',"},
												{"id_empleado,","'"+LoginEmpleado+"',"},
												{"fechahora_creacion,","'"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"',"},
												{"id_tipo_admisiones","'"+lista_de_servicios.Model.GetValue(iter,12).ToString().Trim()+"' "}};
						paraobj = new object []{entry_id_cirugia};
						new osiris.insert_registro("osiris_his_cirugias_deta",parametros,paraobj);
						tienepaquete = true;
					}
					while (treeViewEngineServicio.IterNext(ref iter)){
			   			if ((bool)lista_de_servicios.Model.GetValue (iter,10) == false){
							parametros = new string[,] {{"id_producto,","'"+long.Parse((string) lista_de_servicios.Model.GetValue(iter,2)).ToString().Trim()+"',"},
													{"id_tipo_cirugia,","'"+entry_id_cirugia.Text.ToString().Trim()+"',"},
													{"cantidad_aplicada,","'"+lista_de_servicios.Model.GetValue(iter,1).ToString().Trim()+"',"},
													{"id_empleado,","'"+LoginEmpleado+"',"},
													{"fechahora_creacion,","'"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"',"},
													{"id_tipo_admisiones","'"+lista_de_servicios.Model.GetValue(iter,12).ToString().Trim()+"' "}};
							paraobj = new object [] {entry_id_cirugia};
							new osiris.insert_registro("osiris_his_cirugias_deta",parametros,paraobj);
						}
					}
				}
			}else{
				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
								MessageType.Info,ButtonsType.Close, "Seleccione una Cirugia o Paquete, verifique...");
				msgBoxError.Run ();				msgBoxError.Destroy();
			}

		}
		
		void on_button_buscar_cirugia_clicked(object sender, EventArgs args)
		{
			// Los parametros de del SQL siempre es primero cuando busca todo y la otra por expresion
			// la clase recibe tambien el orden del query
			// es importante definir que tipo de busqueda es para que los objetos caigan ahi mismo
			object[] parametros_objetos = {entry_id_cirugia,entry_cirugia,paquetes};
			string[] parametros_sql = {"SELECT osiris_his_tipo_cirugias.id_tipo_cirugia,descripcion_cirugia,tiene_paquete,to_char(valor_paquete,'999999999.99') AS valorpaquetereal,"+
										"to_char(osiris_erp_preciospaquetes.precio_de_venta,'999999999.99') AS valorpaquete,osiris_his_tipo_cirugias.id_especialidad,descripcion_especialidad,"+
										"descripcion_empresa || ' ' || descripcion_aseguradora AS cliente,osiris_erp_preciospaquetes.id_empresa,osiris_erp_preciospaquetes.id_aseguradora " +
										"FROM osiris_his_tipo_cirugias,osiris_erp_preciospaquetes,osiris_his_tipo_especialidad,osiris_empresas,osiris_aseguradoras " +
										"WHERE osiris_his_tipo_cirugias.id_especialidad = osiris_his_tipo_especialidad.id_especialidad " +
										"AND osiris_his_tipo_cirugias.id_tipo_cirugia = osiris_erp_preciospaquetes.id_tipo_cirugia " +
										"AND osiris_erp_preciospaquetes.id_empresa = osiris_empresas.id_empresa " +
										"AND osiris_erp_preciospaquetes.id_aseguradora = osiris_aseguradoras.id_aseguradora " +
										"AND osiris_his_tipo_cirugias.id_tipo_cirugia > 1"};
			string[] parametros_string = {};
			string[,] args_buscador1 = {{"NOMBRE CIRUGIA","AND descripcion_cirugia LIKE '%","%' "},
										{"ID CIRUGIA","AND osiris_his_tipo_cirugias.id_tipo_cirugia ='","' "},
										{"ESPECIALIDAD","AND descripcion_especialidad LIKE '%","%' "}};
			string[,] args_buscador2 = {{"ID CIRUGIA","AND osiris_his_tipo_cirugias.id_tipo_cirugia ='","' "},
										{"ESPECIALIDAD","AND descripcion_especialidad LIKE '%","%' "},
										{"NOMBRE CIRUGIA","AND descripcion_cirugia LIKE '%","%' "}};
			string[,] args_orderby = {{"",""}};
			classfind_data.buscandor(parametros_objetos,parametros_sql,parametros_string,"find_cirugia_paquetes",0,args_buscador1,args_buscador2,args_orderby);
		}
		
		void on_button_buscar_especialidad_clicked (object sender, EventArgs args)
		{
			// Los parametros de del SQL siempre es primero cuando busca todo y la otra por expresion
			// la clase recibe tambien el orden del query
			// es importante definir que tipo de busqueda es para que los objetos caigan ahi mismo
			object[] parametros_objetos = {entry_id_especialidad,entry_descripcion_especialidad};
			string[] parametros_sql = {"SELECT * FROM osiris_his_tipo_especialidad "};
			string[] parametros_string = {};
			string[,] args_buscador1 = {{"ESPECIALIDAD","WHERE descripcion_especialidad LIKE '%","%' "},
											{"ID ESPECIALIDAD","WHERE id_especialidad = '","' "}};
			string[,] args_buscador2 = {{"ID ESPECIALIDAD","WHERE id_especialidad = '","' "},
										{"ESPECIALIDAD","WHERE descripcion_especialidad LIKE '%","%' "}};
			string[,] args_orderby = {{"",""}};
			classfind_data.buscandor(parametros_objetos,parametros_sql,parametros_string,"find_especialidad_medica",0,args_buscador1,args_buscador2,args_orderby);
		}
				
		void on_button_copia_procedimiento_clicked(object sender, EventArgs args)
		{
			Glade.XML gxml = new Glade.XML (null, "registro_admision.glade", "carga_folio", null);
			gxml.Autoconnect (this);
			carga_folio.Show();
			button_carga_productos.Clicked += new EventHandler(on_button_carga_productos_clicked);
			button_salir.Clicked += new EventHandler(on_cierraventanas_clicked);
		}
		
		void on_button_carga_productos_clicked(object sender, EventArgs args)
		{
			string id_produ = "";
			string desc_produ = "";
			string precio_produ ="";
			string iva_produ ="";
			string total_produ ="";
			string costo_unitario_producto;
			string porcentage_utilidad_producto;
			string costo_total_producto;
			string ppcant ="";
			string constante = "";
			string agrupacion = "";
			float ppcantidad = 0;
			float valor_descuento = 0;
			NpgsqlConnection conexion; 
			conexion = new NpgsqlConnection (connectionString+nombrebd);
			// Verifica que la base de datos este conectada
			try{
				treeViewEngineServicio.Clear();
				if( checkbutton_nueva_cirugia.Active == false){
					llenado_de_cirugia((string) this.entry_id_cirugia.Text.Trim());
				}
				conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand ();
				
				comando.CommandText = "SELECT descripcion_producto,osiris_his_tipo_admisiones.descripcion_admisiones, "+
							"osiris_erp_cobros_deta.eliminado,osiris_productos.aplicar_iva,osiris_erp_cobros_deta.id_tipo_admisiones,  "+
							"to_char(osiris_erp_cobros_deta.id_producto,'999999999999') AS idproducto, "+
							"to_char(osiris_erp_cobros_deta.cantidad_aplicada,'99999.99') AS cantidadaplicada, "+
							"to_char(osiris_productos.precio_producto_publico,'99999999.99') AS preciopublico,"+
							"to_char(osiris_productos.costo_por_unidad,'999999999.99') AS costoproductounitario, "+
							"to_char(osiris_productos.porcentage_ganancia,'99999.99') AS porcentageutilidad, "+
							"to_char(osiris_productos.costo_producto,'999999999.99') AS costoproducto "+
							//"to_char(osiris_his_cirugias_deta.fechahora_creacion,'dd-MM-yyyy HH:mi:ss') AS fechcreacion ,"+
							//"to_char(osiris_his_cirugias_deta.id_secuencia,'9999999999') AS secuencia "+
							"FROM "+
							"osiris_erp_cobros_deta,osiris_productos,osiris_his_tipo_admisiones "+
							"WHERE "+
							"osiris_erp_cobros_deta.id_producto = osiris_productos.id_producto "+
							"AND osiris_erp_cobros_deta.eliminado = false "+ 
							"AND osiris_erp_cobros_deta.id_tipo_admisiones = osiris_his_tipo_admisiones.id_tipo_admisiones "+
							"AND osiris_erp_cobros_deta.folio_de_servicio = '"+(string) this.entry_folio.Text +"' "+
							"ORDER BY osiris_productos.descripcion_producto,to_char(osiris_erp_cobros_deta.fechahora_creacion,'yyyy-mm-dd HH:mm:ss');";
				
				NpgsqlDataReader lector = comando.ExecuteReader ();
				//Console.WriteLine(comando.CommandText.ToString());
				float toma_cantaplicada = 0;
				ppcantidad = 0;
				float toma_subtotal = 0;
				string toma_descrip_prod;
				float calculo_del_iva_producto = 0;
				tienepaquete = false; 
				//Console.WriteLine("antes del ciclo tienepaquete = "+tienepaquete.ToString());				
				while (lector.Read()) {
					if (!(bool) lector["eliminado"]) {
						tienepaquete = true; //Console.WriteLine("ciclo: tienepaquete = "+tienepaquete.ToString());
						toma_cantaplicada = float.Parse((string) lector["cantidadaplicada"]);
						ppcantidad = toma_cantaplicada*(float.Parse((string) lector["preciopublico"]));
						if ((bool) lector["aplicar_iva"]) {
							calculo_del_iva_producto = (ppcantidad*valoriva)/100;
						}else{
							calculo_del_iva_producto = 0;
						}
						
						if ((bool) lector["aplicar_iva"]) {
							subtotal_al_15 = subtotal_al_15 + ppcantidad;
	 					}else{
	 						subtotal_al_0 = subtotal_al_0 + ppcantidad;
	 					}
	 						
	 					toma_subtotal = ppcantidad + calculo_del_iva_producto;
	 					total_iva = total_iva + calculo_del_iva_producto;
						
						toma_descrip_prod = (string) lector["descripcion_producto"];
						if(toma_descrip_prod.Length > 68) {	toma_descrip_prod = toma_descrip_prod.Substring(0,67); }  
						
						treeViewEngineServicio.AppendValues (toma_descrip_prod,
															toma_cantaplicada.ToString("F"),
															lector["idproducto"].ToString(),
															lector["preciopublico"].ToString(),
															ppcantidad.ToString("F").PadLeft(10),
															calculo_del_iva_producto.ToString("F") ,
															toma_subtotal.ToString("F"),
															this.LoginEmpleado,
															DateTime.Now.ToString("yyyy-mm-dd HH:mm:ss"),//(string) lector["fechcreacion"],//8
															lector["descripcion_admisiones"].ToString(),
															false,
															"",
															lector["id_tipo_admisiones"].ToString().Trim());
					}
				}				
				//button_busca_producto.Sensitive = true;
				// Realizando las restas
				sub_total = subtotal_al_15 + subtotal_al_0+total_iva;
				entry_subtotal_al_15.Text = subtotal_al_15.ToString("F");
 				entry_subtotal_al_0.Text = subtotal_al_0.ToString("F");
 				entry_total_iva.Text = total_iva.ToString("F");
 				entry_subtotal.Text = sub_total.ToString("F");
 				entry_total.Text = sub_total.ToString("F");
 				entry_precio_publico.Text = sub_total.ToString("F"); 								
			}catch (NpgsqlException ex){
	   			Console.WriteLine ("PostgresSQL error: {0}",ex.Message);
   				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
									MessageType.Info,ButtonsType.Close, "PostgresSQL error: {0} ",ex.Message);
				msgBoxError.Run ();		msgBoxError.Destroy();
			}
			conexion.Close ();
			carga_folio.Destroy();
		}
		
		void on_button_quitar_aplicados_clicked(object sender, EventArgs args)
		{
			TreeIter iter;
 			TreeModel model;
 			string toma_valor1;
 			string toma_valor2;
 			string prodeliminado;
			if (lista_de_servicios.Selection.GetSelected (out model, out iter)) {
 				toma_valor1 = (string) lista_de_servicios.Model.GetValue(iter,4);	//
				toma_valor2 = (string) lista_de_servicios.Model.GetValue (iter,5);  // toma el valor del iva
				
				if (!(bool) lista_de_servicios.Model.GetValue (iter,10)){
								
 					treeViewEngineServicio.Remove (ref iter);
 				 					
 					if ((float) float.Parse(toma_valor2) > 0){
 						subtotal_al_15 = subtotal_al_15 - float.Parse(toma_valor1);
 					}else{
 						subtotal_al_0 = subtotal_al_0 - float.Parse(toma_valor1);
 					}
 					total_iva = total_iva - float.Parse(toma_valor2);
					sub_total = subtotal_al_15 + subtotal_al_0 + total_iva;
					//toma_a_pagar = sub_total ;
					entry_subtotal_al_15.Text = subtotal_al_15.ToString("F");
 					entry_subtotal_al_0.Text = subtotal_al_0.ToString("F");
 					entry_total_iva.Text = total_iva.ToString("F");
 					entry_subtotal.Text = sub_total .ToString("F");
 					entry_total.Text = sub_total.ToString("F");
 					entry_precio_publico.Text = sub_total.ToString("F");
 				}else{
 					if((string) classpublic.lee_registro_de_tabla("osiris_empleado","acceso_editar_paqqx","WHERE acceso_editar_paqqx = 'true' AND login_empleado = '"+LoginEmpleado+"' ","acceso_editar_paqqx","bool") == "True"){
 						MessageDialog msgBox = new MessageDialog (MyWin,DialogFlags.Modal,
						MessageType.Question,ButtonsType.YesNo,"¿ Desea ELIMINAR del paquete este Producto ?");
						ResponseType miResultado = (ResponseType)msgBox.Run ();
						msgBox.Destroy();	 			
	 					if (miResultado == ResponseType.Yes){
							string[,] parametros = {{"eliminado = '","true',"},
													{"fechahora_eliminado = '",DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"',"},
													{"id_quien_elimino = '",LoginEmpleado+"' "},
													{"WHERE id_secuencia = '",lista_de_servicios.Model.GetValue (iter,11).ToString().Trim()+"';"}};
							object [] paraobj = {entry_id_cirugia};
							new osiris.update_registro("osiris_his_cirugias_deta",parametros,paraobj);
							llenado_de_cirugia(entry_id_cirugia.Text);
 						}
 					}else{
 						MessageDialog msgBox = new MessageDialog (MyWin,DialogFlags.Modal,
						MessageType.Error,ButtonsType.Ok,"No esta autorizado para esta opcion...");
						msgBox.Run();
						msgBox.Destroy();
 					}
 				}
 			}
		}
		
		void on_button_busca_producto_clicked (object sender, EventArgs args)
		{
			Glade.XML gxml = new Glade.XML (null, "registro_admision.glade", "busca_producto", null);
			gxml.Autoconnect (this);
			crea_treeview_busqueda();
			entry_expresion.KeyPressEvent += onKeyPressEvent_enter_exp;
			button_buscar_busqueda.Clicked += new EventHandler(on_llena_lista_producto_clicked);
			button_selecciona.Clicked += new EventHandler(this.on_selecciona_producto_clicked);
			button_salir.Clicked += new EventHandler(on_cierraventanas_clicked);
			combobox_tipo_admision.Clear();
			llena_combobox_admision();
		}
		
		void llena_combobox_admision()
		{
			CellRendererText cell2 = new CellRendererText();
			combobox_tipo_admision.PackStart(cell2, true);
			combobox_tipo_admision.AddAttribute(cell2,"text",0);
	        
			ListStore store2 = new ListStore( typeof (string), typeof (int));
			combobox_tipo_admision.Model = store2;
			
			NpgsqlConnection conexion; 
			conexion = new NpgsqlConnection (connectionString+nombrebd);
            // Verifica que la base de datos este conectada
			try{
				conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand ();
               	comando.CommandText = "SELECT * FROM osiris_his_tipo_admisiones "+
               						"WHERE cuenta_mayor = 4000 "+
               						" ORDER BY descripcion_admisiones;";
				
				NpgsqlDataReader lector = comando.ExecuteReader ();
				store2.AppendValues ("", 0);
               	while (lector.Read()){
					store2.AppendValues ((string) lector["descripcion_admisiones"], (int) lector["id_tipo_admisiones"]);
				}
			}catch (NpgsqlException ex){
				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
										MessageType.Error,ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
				msgBoxError.Run ();				msgBoxError.Destroy();
			}
			conexion.Close ();
			TreeIter iter2;
			if (store2.GetIterFirst(out iter2)){
				combobox_tipo_admision.SetActiveIter (iter2);
			}
			combobox_tipo_admision.Changed += new EventHandler (onComboBoxChanged_tipo_admision);
			
		}
		
		void crea_treeview_servicio()
		{

			object[] parametros = { lista_de_servicios, treeViewEngineServicio };
			string[,] coltreeview = {
				{ "Servicio/Producto.", "text" },
				{ "Cant.", "text" },
				{ "ID Producto", "text" },
				{ "P.Unitario", "text" },
				{ "Sub-Total", "text" },
				{ "I.V.A.", "text" },
				{ "Total", "text" },
				{ "Quien cargo", "text" },
				{ "Fecha/Hora", "text" },
				{ "Cargo a", "text" },
				{ "", "toogle" },
				{ "", "text" },
				{ "", "text" }

			};
			crea_colums_treeview (parametros, coltreeview,"lista_de_servicios");

			/*

			treeViewEngineServicio = new ListStore(typeof(string),//0 
													typeof(float),//1
													typeof(string),//2
													typeof(string),//3
													typeof(string),//4
													typeof(string),//5
													typeof(string),//6
													typeof(string),//7
													typeof(string),//8
													typeof(string),//9
													typeof(bool),//10
													typeof(string),//11
													typeof(int));//12
												
			lista_de_servicios.Model = treeViewEngineServicio;
			lista_de_servicios.RulesHint = true;
				
			TreeViewColumn col_descripcion_hc = new TreeViewColumn();
			CellRendererText cel_descripcion = new CellRendererText();
			col_descripcion_hc.Title = "Servicio/Producto"; // titulo de la cabecera de la columna, si está visible
			col_descripcion_hc.PackStart(cel_descripcion, true);
			col_descripcion_hc.AddAttribute (cel_descripcion, "text", 0);
			col_descripcion_hc.SortColumnId = (int) Column_serv.col_descripcion_hc;
			col_descripcion_hc.SetCellDataFunc(cel_descripcion, new Gtk.TreeCellDataFunc(cambia_colores_fila));
			cel_descripcion.Foreground = "darkblue";
			
			TreeViewColumn col_cantidad_hc = new TreeViewColumn();
			CellRendererText cellr1 = new CellRendererText();
			col_cantidad_hc.Title = "Cantidad"; // titulo de la cabecera de la columna, si está visible
			col_cantidad_hc.PackStart(cellr1, true);
			col_cantidad_hc.AddAttribute (cellr1, "text", 1);
			col_cantidad_hc.SortColumnId = (int) Column_serv.col_cantidad_hc;
			col_cantidad_hc.SetCellDataFunc(cellr1, new Gtk.TreeCellDataFunc(cambia_colores_fila));
			cellr1.Foreground = "darkblue";
						
			TreeViewColumn col_codigo_prod_hc = new TreeViewColumn();
			CellRendererText cellr2 = new CellRendererText();
			col_codigo_prod_hc.Title = "Codigo Prod."; // titulo de la cabecera de la columna, si está visible
			col_codigo_prod_hc.PackStart(cellr2, true);
			col_codigo_prod_hc.AddAttribute (cellr2, "text", 2);
			col_codigo_prod_hc.SortColumnId = (int) Column_serv.col_codigo_prod_hc;
			col_codigo_prod_hc.SetCellDataFunc(cellr2, new Gtk.TreeCellDataFunc(cambia_colores_fila));
			cellr2.Foreground = "darkblue";
			        
			TreeViewColumn col_precio_hc = new TreeViewColumn();
			CellRendererText cellr3 = new CellRendererText();
			col_precio_hc.Title = "P.Unitario"; // titulo de la cabecera de la columna, si está visible
			col_precio_hc.PackStart(cellr3, true);
			col_precio_hc.AddAttribute (cellr3, "text", 3);
			col_precio_hc.SortColumnId = (int) Column_serv.col_precio_hc;
			col_precio_hc.SetCellDataFunc(cellr3, new Gtk.TreeCellDataFunc(cambia_colores_fila));
			cellr3.Foreground = "darkblue";
			
			TreeViewColumn col_ppor_cantidad_hc = new TreeViewColumn();
			CellRendererText cellr4 = new CellRendererText();
			col_ppor_cantidad_hc.Title = "Sub-Total"; // titulo de la cabecera de la columna, si está visible
			col_ppor_cantidad_hc.PackStart(cellr4, true);
			col_ppor_cantidad_hc.AddAttribute (cellr4, "text", 4);
			col_ppor_cantidad_hc.SortColumnId = (int) Column_serv.col_ppor_cantidad_hc;
			col_ppor_cantidad_hc.SetCellDataFunc(cellr4, new Gtk.TreeCellDataFunc(cambia_colores_fila));
			cellr4.Foreground = "darkblue";
        
			TreeViewColumn col_iva_hc = new TreeViewColumn();
			CellRendererText cellr5 = new CellRendererText();
			col_iva_hc.Title = "I.V.A."; // titulo de la cabecera de la columna, si está visible
			col_iva_hc.PackStart(cellr5, true);
			col_iva_hc.AddAttribute (cellr5, "text", 5);
			col_iva_hc.SortColumnId = (int) Column_serv.col_iva_hc;
			col_iva_hc.SetCellDataFunc(cellr5, new Gtk.TreeCellDataFunc(cambia_colores_fila));
			cellr5.Foreground = "darkblue";
        
			TreeViewColumn col_sub_total_hc = new TreeViewColumn();
			CellRendererText cellr6 = new CellRendererText();
			col_sub_total_hc.Title = "Total"; // titulo de la cabecera de la columna, si está visible
			col_sub_total_hc.PackStart(cellr6, true);
			col_sub_total_hc.AddAttribute (cellr6, "text", 6);
			col_sub_total_hc.SortColumnId = (int) Column_serv.col_sub_total_hc;
			col_sub_total_hc.SetCellDataFunc(cellr6, new Gtk.TreeCellDataFunc(cambia_colores_fila));
			cellr6.Foreground = "darkblue";
			
			TreeViewColumn col_quien_cargo_hc = new TreeViewColumn();
			CellRendererText cellr7 = new CellRendererText();
			col_quien_cargo_hc.Title = "Quien cargo"; // titulo de la cabecera de la columna, si está visible
			col_quien_cargo_hc.PackStart(cellr7, true);
			col_quien_cargo_hc.AddAttribute (cellr7, "text", 7);//10
			col_quien_cargo_hc.SortColumnId = (int) Column_serv.col_quien_cargo_hc;
			col_quien_cargo_hc.SetCellDataFunc(cellr7, new Gtk.TreeCellDataFunc(cambia_colores_fila));
			cellr7.Foreground = "darkblue";
        
			TreeViewColumn col_fecha_hora_hc = new TreeViewColumn();
			CellRendererText cellr8 = new CellRendererText();
			col_fecha_hora_hc.Title = "Fecha/Hora"; // titulo de la cabecera de la columna, si está visible
			col_fecha_hora_hc.PackStart(cellr8, true);
			col_fecha_hora_hc.AddAttribute (cellr8, "text", 8);//11
			col_fecha_hora_hc.SortColumnId = (int) Column_serv.col_fecha_hora_hc;
			col_fecha_hora_hc.SetCellDataFunc(cellr8, new Gtk.TreeCellDataFunc(cambia_colores_fila));
			cellr8.Foreground = "darkblue";

			TreeViewColumn col_tipointernamiento_hc =  new TreeViewColumn();
			CellRendererText cellr9 = new CellRendererText();
			col_tipointernamiento_hc.Title = "Cargo a";
			col_tipointernamiento_hc.PackStart(cellr9,true);
			col_tipointernamiento_hc.AddAttribute(cellr9,"text",9);
			col_tipointernamiento_hc.SortColumnId = (int) Column_serv.col_tipointernamiento_hc;
			col_tipointernamiento_hc.SetCellDataFunc(cellr9, new Gtk.TreeCellDataFunc(cambia_colores_fila));
			cellr9.Foreground = "darkblue";
			
			lista_de_servicios.AppendColumn(col_descripcion_hc);
			lista_de_servicios.AppendColumn(col_cantidad_hc);
			lista_de_servicios.AppendColumn(col_codigo_prod_hc);
			lista_de_servicios.AppendColumn(col_precio_hc);
			lista_de_servicios.AppendColumn(col_ppor_cantidad_hc);
			lista_de_servicios.AppendColumn(col_iva_hc);
			lista_de_servicios.AppendColumn(col_sub_total_hc);
			lista_de_servicios.AppendColumn(col_quien_cargo_hc);
			lista_de_servicios.AppendColumn(col_fecha_hora_hc);
			lista_de_servicios.AppendColumn(col_tipointernamiento_hc);*/
		}
		
		void crea_treeview_busqueda()
		{
			treeViewEngineBusca2 = new TreeStore(typeof(string),
												typeof(string),
												typeof(string),
												typeof(string),
												typeof(string),
												typeof(string),
												typeof(string),
												typeof(string),
												typeof(string),
												typeof(string),
												typeof(string),
												typeof(string));
			lista_de_producto.Model = treeViewEngineBusca2;
			lista_de_producto.RulesHint = true;
			lista_de_producto.RowActivated += on_selecciona_producto_clicked;  // Doble click selecciono paciente
				
			TreeViewColumn col_idproducto = new TreeViewColumn();
			CellRendererText cellr0 = new CellRendererText();
			col_idproducto.Title = "ID Producto"; // titulo de la cabecera de la columna, si está visible
			col_idproducto.PackStart(cellr0, true);
			col_idproducto.AddAttribute (cellr0, "text", 0);    // la siguiente columna será 1 en vez de 1
			col_idproducto.SortColumnId = (int) Column_prod.col_idproducto;
			
			TreeViewColumn col_desc_producto = new TreeViewColumn();
			CellRendererText cellr1 = new CellRendererText();
			col_desc_producto.Title = "Descripcion de Producto"; // titulo de la cabecera de la columna, si está visible
			col_desc_producto.PackStart(cellr1, true);
			col_desc_producto.AddAttribute (cellr1, "text", 1);    // la siguiente columna será 1 en vez de 1
			col_desc_producto.SortColumnId = (int) Column_prod.col_desc_producto;
			//cellr0.Editable = true;   // Permite edita este campo
            
			TreeViewColumn col_precioprod = new TreeViewColumn();
			CellRendererText cellrt2 = new CellRendererText();
			col_precioprod.Title = "Precio Producto";
			col_precioprod.PackStart(cellrt2, true);
			col_precioprod.AddAttribute (cellrt2, "text", 2); // la siguiente columna será 1 en vez de 2
			col_precioprod.SortColumnId = (int) Column_prod.col_precioprod;
           
			TreeViewColumn col_ivaprod = new TreeViewColumn();
			CellRendererText cellrt3 = new CellRendererText();
			col_ivaprod.Title = "I.V.A.";
			col_ivaprod.PackStart(cellrt3, true);
			col_ivaprod.AddAttribute (cellrt3, "text", 3); // la siguiente columna será 2 en vez de 3
			col_ivaprod.SortColumnId = (int) Column_prod.col_ivaprod;
           
			TreeViewColumn col_totalprod = new TreeViewColumn();
			CellRendererText cellrt4 = new CellRendererText();
			col_totalprod.Title = "Total";
			col_totalprod.PackStart(cellrt4, true);
			col_totalprod.AddAttribute (cellrt4, "text", 4); // la siguiente columna será 3 en vez de 4
			col_totalprod.SortColumnId = (int) Column_prod.col_totalprod;
           
			TreeViewColumn col_grupoprod = new TreeViewColumn();
			CellRendererText cellrt5 = new CellRendererText();
			col_grupoprod.Title = "Grupo Producto";
			col_grupoprod.PackStart(cellrt5, true);
			col_grupoprod.AddAttribute (cellrt5, "text", 5); // la siguiente columna será 7 en vez de 8
			col_grupoprod.SortColumnId = (int) Column_prod.col_grupoprod;
          
			TreeViewColumn col_grupo1prod = new TreeViewColumn();
			CellRendererText cellrt6 = new CellRendererText();
			col_grupo1prod.Title = "Grupo1 Producto";
			col_grupo1prod.PackStart(cellrt6, true);
			col_grupo1prod.AddAttribute (cellrt6, "text", 6); // la siguiente columna será 8 en vez de 9
			col_grupo1prod.SortColumnId = (int) Column_prod.col_grupo1prod;
                        
			TreeViewColumn col_grupo2prod = new TreeViewColumn();
			CellRendererText cellrt7 = new CellRendererText();
			col_grupo2prod.Title = "Grupo2 Producto";
			col_grupo2prod.PackStart(cellrt7, true);
			col_grupo2prod.AddAttribute (cellrt7, "text", 7); // la siguiente columna será 8 en vez de 9
			col_grupo2prod.SortColumnId = (int) Column_prod.col_grupo2prod;
		
			lista_de_producto.AppendColumn(col_idproducto);
			lista_de_producto.AppendColumn(col_desc_producto);
			lista_de_producto.AppendColumn(col_precioprod);
			lista_de_producto.AppendColumn(col_ivaprod);
			lista_de_producto.AppendColumn(col_totalprod);
			lista_de_producto.AppendColumn(col_grupoprod);
			lista_de_producto.AppendColumn(col_grupo1prod);
			lista_de_producto.AppendColumn(col_grupo2prod);						
		}
		
		enum Column_prod
		{
			col_idproducto,
			col_desc_producto,
			col_precioprod,
			col_ivaprod,
			col_totalprod,
			col_descuentoprod,
			col_preciocondesc,
			col_grupoprod,
			col_grupo1prod,
			col_grupo2prod
		}
		enum Column_serv
		{
			col_descripcion_hc,
			col_cantidad_hc,
			col_codigo_prod_hc,
			col_precio_hc,
			col_ppor_cantidad_hc,
			col_iva_hc,
			col_sub_total_hc,
			col_quien_cargo_hc,
			col_fecha_hora_hc,
			col_tipointernamiento_hc
		}
		
		// Este toma los valores para llenar el encabezado del procedimiento
		// Aqui lleno el detalle de los servicios que se va aplicar para su cobro
		void llenado_de_cirugia(string idcirugia)
		{
			if(copiaproductos == false) {
				subtotal_al_15 = 0;
				subtotal_al_0 = 0;
				total_iva = 0;
				sub_total = 0;
			}
			NpgsqlConnection conexion;
			conexion = new NpgsqlConnection (connectionString+nombrebd);
	        try{
				conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand ();
	              	
				comando.CommandText = "SELECT osiris_his_tipo_cirugias.id_tipo_cirugia,"+
									"descripcion_cirugia,descripcion_especialidad,"+
									"to_char(osiris_erp_preciospaquetes.precio_de_venta,'99999999.99') AS precioventa,"+
									"to_char(osiris_erp_preciospaquetes.deposito_minimo,'99999999.99') AS depominimo,"+
									"to_char(dias_de_internamiento,'99999999') AS diasinternamiento,"+
									"tiene_paquete,paquete_checkup," +
									"osiris_erp_preciospaquetes.id_empresa,osiris_empresas.descripcion_empresa," +
									"osiris_erp_preciospaquetes.id_aseguradora,osiris_aseguradoras.descripcion_aseguradora,"+
									"to_char(osiris_his_tipo_cirugias.id_especialidad,'999999') AS idespecialidad "+
									"FROM osiris_his_tipo_cirugias,osiris_his_tipo_especialidad,osiris_erp_preciospaquetes,osiris_aseguradoras,osiris_empresas "+
					            	"WHERE osiris_his_tipo_cirugias.id_especialidad = osiris_his_tipo_especialidad.id_especialidad " +
									"AND osiris_erp_preciospaquetes.id_empresa = osiris_empresas.id_empresa " +
									"AND osiris_erp_preciospaquetes.id_aseguradora = osiris_aseguradoras.id_aseguradora " +
					            	"AND osiris_his_tipo_cirugias.id_tipo_cirugia = '"+idcirugia.ToString()+"';";
				//Console.WriteLine("query llenado cirugia: "+comando.CommandText.ToString());				
				NpgsqlDataReader lector = comando.ExecuteReader ();
				
				if(lector.Read()){
					entry_cirugia.Text = (string) lector["descripcion_cirugia"].ToString().Trim();
					entry_dias_internamiento.Text = (string) lector["diasinternamiento"].ToString().Trim();
	           		entry_id_especialidad.Text = (string) lector["idespecialidad"].ToString().Trim();
	           		entry_descripcion_especialidad.Text = (string) lector["descripcion_especialidad"].ToString().Trim();
	           		entry_precio_publico.Text = (string) lector["precioventa"].ToString().Trim();
	           		entry_deposito_minimo.Text = (string) lector["depominimo"].ToString().Trim();
					checkbutton_paquete_sino.Active = (bool) lector["tiene_paquete"];
					checkbutton_pq_checkup.Active = (bool) lector["paquete_checkup"];
					if((int) lector["id_empresa"] != 1){
						entry_id_empaseg.Text = lector["id_empresa"].ToString().Trim();
						entry_nombre_empaseg.Text = (string) lector["descripcion_empresa"];
					}else{
						entry_id_empaseg.Text = lector["id_aseguradora"].ToString().Trim();
						entry_nombre_empaseg.Text = (string) lector["descripcion_aseguradora"];
					}
	           		llenado_de_material_aplicado( idcirugia);
	           	}else{
	           		MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
							MessageType.Error, ButtonsType.Close, "NO existe la Cirugia y/o Paquete Seleccionado, verifique...");
					msgBoxError.Run (); 	msgBoxError.Destroy();
	           	}
			}catch (NpgsqlException ex){
		   		Console.WriteLine ("PostgresSQL error: {0}",ex.Message);
		   		MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
							MessageType.Error, ButtonsType.Close, "PostgresSQL error: {0} ",ex.Message);
				msgBoxError.Run (); 	msgBoxError.Destroy();
		    }
	       	conexion.Close ();
       	}
		
		// llenando el detalle de procedimiento de cobranza
		void llenado_de_material_aplicado(string idcirugia)
		{	
			
			float ppcantidad = 0;
			if(copiaproductos == false) {
				//Console.WriteLine("convierte valores a cero");
				subtotal_al_15 = 0;
				subtotal_al_0 = 0;
				total_iva = 0;
				sub_total = 0;
				treeViewEngineServicio.Clear();
			}
			///////RESTABLESCO VALORES A LOS PREDETERMINADOS\\\\\\\\\\\\\\\\\\
			//entry_id_cirugia.Sensitive = true;
			entry_cirugia.Sensitive = true;
			entry_cirugia.IsEditable = false;
			entry_id_especialidad.Sensitive = true;
			entry_descripcion_especialidad.Sensitive = true;
			entry_dias_internamiento.Sensitive = true;
			entry_deposito_minimo.Sensitive = true;
			entry_precio_publico.Sensitive = true;
	       	button_graba_paquete.Sensitive = true;
			button_procedimiento_cobrz.Sensitive = true;
			button_paquete_sin_precios.Sensitive = true;
			button_limpiar.Sensitive = true;
			button_quitar_aplicados.Sensitive = true;
			button_actualizar.Sensitive = true;
			button_copia_procedimiento.Sensitive = true;
			lista_de_servicios.Sensitive = true;
			checkbutton_copia_productos.Sensitive = true;
			checkbutton_nueva_cirugia.Sensitive = true;
			
			NpgsqlConnection conexion; 
			conexion = new NpgsqlConnection (connectionString+nombrebd);
			            
			// Verifica que la base de datos este conectada
			try{
				conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand ();
				
				comando.CommandText = "SELECT descripcion_producto,osiris_his_tipo_admisiones.descripcion_admisiones, "+
							"id_empleado,osiris_his_cirugias_deta.eliminado,osiris_productos.aplicar_iva,osiris_his_cirugias_deta.id_tipo_admisiones,  "+
							"to_char(osiris_his_cirugias_deta.id_producto,'999999999999') AS idproducto, "+
							"to_char(osiris_his_cirugias_deta.cantidad_aplicada,'99999.99') AS cantidadaplicada, "+
							"to_char(osiris_productos.precio_producto_publico,'99999999.99') AS preciopublico,"+
							"to_char(osiris_productos.costo_por_unidad,'999999999.99') AS costoproductounitario, "+
							"to_char(osiris_productos.porcentage_ganancia,'99999.99') AS porcentageutilidad, "+
							"to_char(osiris_productos.costo_producto,'999999999.99') AS costoproducto, "+
							"to_char(osiris_his_cirugias_deta.fechahora_creacion,'dd-MM-yyyy HH:mi:ss') AS fechcreacion ,"+
							"to_char(osiris_his_cirugias_deta.id_secuencia,'9999999999') AS secuencia "+
							"FROM "+
							"osiris_his_cirugias_deta,osiris_productos,osiris_his_tipo_cirugias,osiris_his_tipo_admisiones "+
							"WHERE "+
							"osiris_his_cirugias_deta.id_producto = osiris_productos.id_producto "+
							"AND osiris_his_cirugias_deta.id_tipo_cirugia = osiris_his_tipo_cirugias.id_tipo_cirugia "+
							"AND osiris_his_cirugias_deta.eliminado = false "+ 
							"AND osiris_his_cirugias_deta.id_tipo_admisiones = osiris_his_tipo_admisiones.id_tipo_admisiones "+
							"AND osiris_his_cirugias_deta.id_tipo_cirugia = '"+(string) idcirugia +"' "+
						    "AND osiris_his_cirugias_deta.eliminado = 'false' "+
							"ORDER BY osiris_productos.descripcion_producto,to_char(osiris_his_cirugias_deta.fechahora_creacion,'yyyy-mm-dd HH:mm:ss');";
				
				Console.WriteLine("aplicado  "+comando.CommandText.ToString());
				NpgsqlDataReader lector = comando.ExecuteReader ();
				float toma_cantaplicada = 0;
				ppcantidad = 0;
				float toma_subtotal = 0;
				string toma_descrip_prod;
				float calculo_del_iva_producto = 0;
				tienepaquete = false; //Console.WriteLine("antes del ciclo tienepaquete = "+tienepaquete.ToString());
				
				while (lector.Read()) {
					if (!(bool) lector["eliminado"]) {
						tienepaquete = true; //Console.WriteLine("ciclo: tienepaquete = "+tienepaquete.ToString());
						toma_cantaplicada = float.Parse((string) lector["cantidadaplicada"]);
						ppcantidad = toma_cantaplicada*(float.Parse((string) lector["preciopublico"]));
						if ((bool) lector["aplicar_iva"]) {
							calculo_del_iva_producto = (ppcantidad*valoriva)/100;
						}else{
							calculo_del_iva_producto = 0;
						}
						
						if ((bool) lector["aplicar_iva"]) {
							subtotal_al_15 = subtotal_al_15 + ppcantidad;
	 					}else{
	 						subtotal_al_0 = subtotal_al_0 + ppcantidad;
	 					}
	 						
	 					toma_subtotal = ppcantidad + calculo_del_iva_producto;
	 					total_iva = total_iva + calculo_del_iva_producto;
						
						toma_descrip_prod = (string) lector["descripcion_producto"];
						if(toma_descrip_prod.Length > 68) {	toma_descrip_prod = toma_descrip_prod.Substring(0,67);	}  
						
						treeViewEngineServicio.AppendValues (toma_descrip_prod,
															toma_cantaplicada.ToString("F"),
															lector["idproducto"].ToString().Trim(),
															lector["preciopublico"].ToString().Trim(),
															ppcantidad.ToString("F").PadLeft(10).ToString().Trim(),
															calculo_del_iva_producto.ToString("F").Trim() ,
															toma_subtotal.ToString("F").Trim(),
															lector["id_empleado"].ToString().Trim(),
															lector["fechcreacion"].ToString().Trim(),
															lector["descripcion_admisiones"].ToString().Trim(),
															true,
															lector["secuencia"].ToString().Trim().ToString().Trim(),
															lector["id_tipo_admisiones"].ToString());
					}
				}				
				button_busca_producto.Sensitive = true;
				sub_total = subtotal_al_15 + subtotal_al_0+total_iva;
				entry_subtotal_al_15.Text = subtotal_al_15.ToString("F");
 				entry_subtotal_al_0.Text = subtotal_al_0.ToString("F");
 				entry_total_iva.Text = total_iva.ToString("F");
 				entry_subtotal.Text = sub_total.ToString("F");
 				entry_total.Text = sub_total.ToString("F"); 								
			}catch (NpgsqlException ex){
	   			Console.WriteLine ("PostgresSQL error: {0}",ex.Message);
   				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
									MessageType.Info,ButtonsType.Close, "PostgresSQL error: {0} ",ex.Message);
				msgBoxError.Run ();		msgBoxError.Destroy();
			}
			conexion.Close ();
		}
		
		void on_llena_lista_producto_clicked (object sender, EventArgs args)
 		{
 			llenando_lista_de_productos();
 		}
 		
 		void llenando_lista_de_productos()
 		{
			treeViewEngineBusca2.Clear();
			float tomaprecio;
			float calculodeiva;
			float preciomasiva;
			NpgsqlConnection conexion; 
			conexion = new NpgsqlConnection (connectionString+nombrebd);
            // Verifica que la base de datos este conectada
			try{
				conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand ();               	
				comando.CommandText = "SELECT to_char(osiris_productos.id_producto,'999999999999') AS codProducto,"+
							"osiris_productos.descripcion_producto, "+
							"descripcion_grupo_producto,descripcion_grupo1_producto,descripcion_grupo2_producto, "+
							"to_char(precio_producto_publico,'99999999.99') AS preciopublico,"+
							"to_char(costo_por_unidad,'999999999.99') AS costoproductounitario, "+
							"to_char(porcentage_ganancia,'99999.99') AS porcentageutilidad, "+
							"to_char(costo_producto,'999999999.99') AS costoproducto, "+
							"osiris_grupo_producto.agrupacion, aplicar_iva "+
							"FROM osiris_productos,osiris_grupo_producto,osiris_grupo1_producto,osiris_grupo2_producto "+
							"WHERE osiris_productos.id_grupo_producto = osiris_grupo_producto.id_grupo_producto "+
							"AND osiris_productos.id_grupo1_producto = osiris_grupo1_producto.id_grupo1_producto "+
							"AND osiris_productos.id_grupo2_producto = osiris_grupo2_producto.id_grupo2_producto "+
							"AND osiris_productos.cobro_activo = 'true' "+
							"AND osiris_productos.descripcion_producto LIKE '%"+entry_expresion.Text.ToUpper().Trim()+"%' ORDER BY descripcion_producto; ";
				
				NpgsqlDataReader lector = comando.ExecuteReader ();
				while (lector.Read()){
					calculodeiva = 0;
					preciomasiva = 0;
					tomaprecio = float.Parse((string) lector["preciopublico"]);
					if ((bool) lector["aplicar_iva"]){
						calculodeiva = (tomaprecio * valoriva)/100;
					}					
					preciomasiva = tomaprecio + calculodeiva;
					treeViewEngineBusca2.AppendValues (
											(string) lector["codProducto"] ,
											(string) lector["descripcion_producto"],
											(string) lector["preciopublico"],
											calculodeiva.ToString("F").PadLeft(10),
											preciomasiva.ToString("F").PadLeft(10),
											(string) lector["descripcion_grupo_producto"],
											(string) lector["descripcion_grupo1_producto"],
											(string) lector["descripcion_grupo2_producto"],
											(string) lector["costoproductounitario"],
											(string) lector["porcentageutilidad"],
											lector["costoproducto"].ToString().Trim(),
											(string) lector["agrupacion"]);
					
				}
			}catch (NpgsqlException ex){
	   			Console.WriteLine ("PostgresSQL error: {0}",ex.Message);
   				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
									MessageType.Info,ButtonsType.Close, "PostgresSQL error: {0} ",ex.Message);
				msgBoxError.Run ();		msgBoxError.Destroy();
			}
			conexion.Close ();
		}
		
		void on_selecciona_producto_clicked (object sender, EventArgs args)
		{
			TreeModel model;
			TreeIter iterSelected;
			string iva_produ ="";
			string total_produ ="";
			string costo_unitario_producto;
			string porcentage_utilidad_producto;
			string costo_total_producto;
			string ppcant ="";
					
			float ppcantidad = 0;

 			if (lista_de_producto.Selection.GetSelected(out model, out iterSelected)){
 				//Console.WriteLine("pasavalor");
				iva_produ = (string) model.GetValue(iterSelected, 3);
				total_produ = (string) model.GetValue(iterSelected, 4);
				costo_unitario_producto = (string) model.GetValue(iterSelected, 8);
				porcentage_utilidad_producto = (string) model.GetValue(iterSelected, 9);
				// Console.WriteLine ((string) model.GetValue(iterSelected, 10));
				costo_total_producto = model.GetValue(iterSelected, 10).ToString().Trim();
				//agrupacion = (string) model.GetValue(iterSelected,11);

				//varibles numericas
				ppcantidad = float.Parse(model.GetValue(iterSelected, 2).ToString().Trim()) * float.Parse(entry_cantidad_aplicada.Text.ToString().Trim());
				float ivaproducto = float.Parse(iva_produ)*float.Parse(entry_cantidad_aplicada.Text.ToString().Trim());
				float suma_total = ppcantidad+ivaproducto;
				////////////////////////////////////////////////
				subtotal_al_15 = float.Parse(entry_subtotal_al_15.Text);
				subtotal_al_0 = float.Parse(entry_subtotal_al_0.Text);
				if ((float) float.Parse(iva_produ) > 0){
 					subtotal_al_15 = subtotal_al_15 + ppcantidad;
 				}else{
 					subtotal_al_0 = subtotal_al_0 + ppcantidad;
 				}		
 				total_iva = total_iva + ivaproducto;
				sub_total = subtotal_al_15 + subtotal_al_0+total_iva;
				
				entry_subtotal_al_15.Text = subtotal_al_15.ToString();
				entry_subtotal_al_0.Text = subtotal_al_0.ToString();
				entry_total_iva.Text = total_iva.ToString();
				entry_subtotal.Text =  sub_total.ToString();
				entry_total.Text = sub_total.ToString();
				entry_precio_publico.Text = sub_total.ToString("F");
				//entry_a_pagar.Text = sub_total.ToString();
								
				if (entry_cantidad_aplicada.Text.ToString().Trim() != ""){
					if((string) model.GetValue(iterSelected, 11) == "IMG") 
					{ idtipointernamiento = 300;		descripinternamiento = "IMAGENOLOGIA-RX"; }
					
					if((string) model.GetValue(iterSelected, 11) == "LAB") 
					{ idtipointernamiento = 400;		descripinternamiento = "LABORATORIO"; }
					
					if ((float) float.Parse(entry_cantidad_aplicada.Text.ToString().Trim()) > 0){
						if ((int) idtipointernamiento >= 20){

							//entry_cantidad_aplicada

							treeViewEngineServicio.AppendValues (
											model.GetValue (iterSelected, 1).ToString ().Trim (),
											float.Parse(entry_cantidad_aplicada.Text.ToString().Trim()),
											model.GetValue(iterSelected, 0).ToString().Trim(),
											model.GetValue(iterSelected, 2).ToString(),
											ppcantidad.ToString("F").Trim(),
											ivaproducto.ToString("F").Trim(),
											suma_total.ToString("F").Trim(),
											LoginEmpleado,
											DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
											descripinternamiento,
											false,
											idtipointernamiento);
							
							//this.combobox_tipo_admision.Clear();
							entry_cantidad_aplicada.Text = "0";
							entry_expresion.Text = "";
							entry_expresion.GrabFocus();
							//llena_combobox_admision();
						}else{
							MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
									MessageType.Error,ButtonsType.Close, 
									"seleccione un tipo de admision valido \n"+"mayor que cero, intente de nuevo");
							msgBoxError.Run ();						msgBoxError.Destroy();
						}
					}else{
						MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
										MessageType.Error,ButtonsType.Close, 
										"La cantidad que quiere aplicar debe ser \n"+"mayor que cero, intente de nuevo");
						msgBoxError.Run ();					msgBoxError.Destroy();
					}
				}else{
					MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
								MessageType.Error,ButtonsType.Close, 
								"La cantidad que quiere aplicar NO \n"+"puede quedar vacia, intente de nuevo");
					msgBoxError.Run ();				msgBoxError.Destroy();
				}
 			}
 		}
		
		void on_button_limpiar_clicked(object sender, EventArgs args)
		{
			//limpia_valores();
			entry_id_cirugia.Text = "";
			entry_cirugia.Text = "";
			entry_id_especialidad.Text = "";
			entry_descripcion_especialidad.Text = "";
			entry_dias_internamiento.Text = "0";
			entry_deposito_minimo.Text = "0";
			entry_precio_publico.Text = "0";
			button_procedimiento_cobrz.Sensitive = false;
			button_paquete_sin_precios.Sensitive = false;
			treeViewEngineServicio.Clear();
			entry_subtotal_al_0.Text = "0.00";
			entry_subtotal_al_15.Text = "0.00";
			entry_total_iva.Text = "0.00";
			entry_total.Text = "0.00";
			entry_subtotal.Text = "0.00";
			entry_precio_publico.Text = "0.00";
			checkbutton_copia_productos.Active = false;
			checkbutton_nueva_cirugia.Active = false;
		}
		
		public void limpia_valores()
		{	
			entry_cirugia.Text = "";
			entry_id_especialidad.Text = "";
			entry_descripcion_especialidad.Text = "";
			entry_dias_internamiento.Text = "0";
			entry_deposito_minimo.Text = "0";
			entry_precio_publico.Text = "0";
			if(copiaproductos == false ) { 
				treeViewEngineServicio.Clear();
			 	entry_subtotal_al_0.Text = "0.00";
				entry_subtotal_al_15.Text = "0.00";
				entry_total_iva.Text = "0.00";
				entry_total.Text = "0.00";
				entry_subtotal.Text = "0.00";
				entry_precio_publico.Text = "0.00";
			}
			checkbutton_paquete_sino.Active = false;
		}
		
		void on_checkbutton_nueva_cirugia_clicked(object sender, EventArgs args)
		{
			if(checkbutton_nueva_cirugia.Active == true){
				if((string) classpublic.lee_registro_de_tabla("osiris_empleado","acceso_crea_paqqx","WHERE acceso_crea_paqqx = 'true' AND login_empleado = '"+LoginEmpleado+"' ","acceso_crea_paqqx","bool") == "True"){
					MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
						MessageType.Question,ButtonsType.YesNo,"Quiere crear una nueva cirugia ?");
					ResponseType miResultado = (ResponseType)
					msgBoxError.Run ();			msgBoxError.Destroy();
					if (miResultado == ResponseType.Yes){
						activa_campos(true);					
						entry_cirugia.GrabFocus();
						limpia_valores();
						entry_id_cirugia.Text = classpublic.lee_ultimonumero_registrado("osiris_his_tipo_cirugias","id_tipo_cirugia","");
						nuevacirugia = true;
						checkbutton_cambiar_cirugia.Active = false;
						llenado_combobox(1,"",combobox_tipo_paciente,"sql","SELECT * FROM osiris_his_tipo_pacientes WHERE activo_tipo_paciente = 'true' ORDER BY descripcion_tipo_paciente;",
							"descripcion_tipo_paciente","id_tipo_paciente",args_args,args_id_array);
					}else{
						checkbutton_nueva_cirugia.Active = false;
						button_buscar_cirugia.Sensitive = true;
						button_selec_id.Sensitive = true;
					}
				}
			}else{
				button_buscar_cirugia.Sensitive = true;
				button_selec_id.Sensitive = true;
			}
			if(checkbutton_nueva_cirugia.Active == false){
				checkbutton_copia_productos.Active = false;
				nuevacirugia = false;
			}
		}
		
		void activa_campos(bool valor)
		{
			entry_cirugia.Sensitive = valor;
			entry_cirugia.IsEditable = valor;
			entry_id_especialidad.Sensitive = valor;
			entry_descripcion_especialidad.Sensitive = valor;
			entry_dias_internamiento.Sensitive = valor;
			entry_deposito_minimo.Sensitive = valor;
			entry_precio_publico.Sensitive = valor;			
			button_buscar_cirugia.Sensitive = !valor;
			button_selec_id.Sensitive = !valor;
			button_graba_paquete.Sensitive = valor;
			button_limpiar.Sensitive = valor;
			button_busca_producto.Sensitive = valor;
			button_graba_paquete.Sensitive = valor;
			button_procedimiento_cobrz.Sensitive = valor;
			button_paquete_sin_precios.Sensitive = valor;
			button_quitar_aplicados.Sensitive = valor;
			button_limpiar.Sensitive = valor;
			button_actualizar.Sensitive = valor;
			button_copia_procedimiento.Sensitive = valor;
			checkbutton_copia_productos.Sensitive = valor;
			//checkbutton_nueva_cirugia.Sensitive = false;
			lista_de_servicios.Sensitive = valor;
		}
		
		void on_checkbutton_copia_productos_clicked(object sender, EventArgs args)
		{
			if(entry_id_cirugia.Text != "" || entry_id_cirugia.Text != " ") {  
				TreeIter iter;
				if(checkbutton_copia_productos.Active == true){
					copiaproductos = true;		//Console.WriteLine("copiaproductos = "+copiaproductos.ToString());
					limpia_valores();
					tienepaquete = true;
					if (treeViewEngineServicio.GetIterFirst (out iter)){
						if ((bool)lista_de_servicios.Model.GetValue (iter,10) == true) {
			 				lista_de_servicios.Model.SetValue (iter,10,false);	}
						while (treeViewEngineServicio.IterNext(ref iter)) {
			    	   		if ((bool)lista_de_servicios.Model.GetValue (iter,10) == true) {
			 					lista_de_servicios.Model.SetValue (iter,10,false); }
			 			}
			 		}
			 	}else{
					copiaproductos = false;		//Console.WriteLine("copiadoproductos = "+copiaproductos.ToString());
					treeViewEngineServicio.Clear();
				}
			}else{
				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
							MessageType.Error,ButtonsType.Close,"Debe de seleccionar una cirugia \n"+"para poder copiar sus productos");
				msgBoxError.Run ();					msgBoxError.Destroy();
				copiaproductos = false;
				checkbutton_copia_productos.Active = false;
			}
		}
		
		//ACCION QUE CAMBIA EL COLOR DEL TEXTO PARA CUANDO SE GUARDA EN LA BASE DE DATOS 
		void cambia_colores_fila(Gtk.TreeViewColumn column, Gtk.CellRenderer cell, Gtk.TreeModel model, Gtk.TreeIter iter)
		{
			//descripcion_producto descrip = (descripcion_producto) model.GetValue (iter, 14);
			if ((bool)lista_de_servicios.Model.GetValue (iter,10)==true){
				(cell as Gtk.CellRendererText).Foreground = "darkblue";
			}else{
				(cell as Gtk.CellRendererText).Foreground = "red";
			}
		}
		
		// Valida entradas que solo sean numericas, se utiliza eb ventana de carga de producto
		[GLib.ConnectBefore ()]
		public void onKeyPressEvent_enter_exp(object o, Gtk.KeyPressEventArgs args)
		{	
			if (args.Event.Key.ToString() == "Return" || args.Event.Key.ToString() == "KP_Enter"){
				args.RetVal = true;			llenando_lista_de_productos();			
			}
		}
		
		// Valida entradas que solo sean numericas, se utiliza eb ventana de carga de producto
		[GLib.ConnectBefore ()]    
		public void onKeyPressEvent_enter_id(object o, Gtk.KeyPressEventArgs args)
		{
			string misDigitos = ".0123456789ﾰﾱﾲﾳﾴﾵﾶﾷﾸﾹﾮｔｒｓｑ（）";
			//Console.WriteLine(args.Event.Key.ToString());
			//Console.WriteLine(Convert.ToChar(args.Event.Key));
			//Console.WriteLine(Array.IndexOf(misDigitos.ToCharArray(), Convert.ToChar(args.Event.Key)));
			if (args.Event.Key.ToString() == "Return" || args.Event.Key.ToString() == "KP_Enter"){
				args.RetVal = true;
				llenado_de_cirugia( entry_id_cirugia.Text );			
			}
			if (Array.IndexOf(misDigitos.ToCharArray(), Convert.ToChar(args.Event.Key)) == -1){
				//Console.WriteLine(Convert.ToChar(args.Event.Key));
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