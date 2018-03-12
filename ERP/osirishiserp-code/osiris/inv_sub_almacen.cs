////////////////////////////////////////////////////////////
// created on 05/03/2008 at 04:30 p
// Sistema Hospitalario OSIRIS
// Monterrey - Mexico
//
// Autor    	: Tec. Homero Montoya Galvan (Programacion)
//				  Ing. Daniel Olivares (Preprogramacion)
// 				  
// Licencia		: GLP
// S.O. 		: GNU/Linux
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
using System.IO;
using Gtk;
using Npgsql;
using System.Data;
using Glade;
using System.Xml;
namespace osiris
{
	public class inventario_sub_almacen
	{
		// Declarando ventana de inventario sub almacen
		[Widget] Gtk.Window inventario_sub_almacenes;
		[Widget] Gtk.ComboBox combobox_tipo_almacen;
		[Widget] Gtk.ComboBox combobox_grupo;
		[Widget] Gtk.ComboBox combobox_grupo1;
		[Widget] Gtk.ComboBox combobox_grupo2;
		[Widget] Gtk.ComboBox combobox_almacen_destino;
		[Widget] Gtk.RadioButton radiobutton_con_stock = null;
		[Widget] Gtk.RadioButton radiobutton_sin_stock = null;
		[Widget] Gtk.RadioButton radiobutton_todo_stock = null;
		[Widget] Gtk.TreeView lista_almacenes;
		[Widget] Gtk.Button button_actualizar;
		[Widget] Gtk.Button button_imprimir;
		[Widget] Gtk.Button button_salir;
		[Widget] Gtk.Button button_enviar;
		[Widget] Gtk.Button button_quitar;
		[Widget] Gtk.Button button_es_stock = null;
		[Widget] Gtk.Button button_imprime_traspaso = null;
		[Widget] Gtk.Button button_export_ods = null;
		[Widget] Gtk.Entry entry_mes_copy = null;
		[Widget] Gtk.Entry entry_ano_copy = null;
		[Widget] Gtk.Label label_almacen_destino;
		[Widget] Gtk.CheckButton checkbutton_herramientas = null;
		[Widget] Gtk.CheckButton checkbutton_todoscopy_invfisico = null;
		[Widget] Gtk.ComboBox combobox_herramientas = null;

		[Widget] Gtk.Entry entry_filter = null;
		[Widget] Gtk.Statusbar statusbar_inv_sub_hosp;
		
		string connectionString;
		string nombrebd;
		string LoginEmpleado;
		string NomEmpleado;
		string AppEmpleado;
		string ApmEmpleado;		
		
		int idsubalmacen;
		int idsubalacen_original;
		int idalmacendestino;
		string descsubalmacen;
		int tipoalmacen;
				
		int idtipogrupo = 0;
		int idtipogrupo1 = 0;
		int idtipogrupo2 = 0;
		int idalmacen = 0;
		string descripgrupo = "";
		string descripgrupo1 =  "";
		string descripgrupo2 = "";
		int tipoherramienta = 0;
		string descripcionalmacen = "";
		string entryfilter_productos = "";
		
		string query_sql = "SELECT osiris_catalogo_almacenes.id_almacen,osiris_catalogo_almacenes.id_secuencia," +
							"to_char(osiris_productos.id_producto,'999999999999') AS idproducto,"+
							"osiris_productos.descripcion_producto, "+
							"to_char(osiris_catalogo_almacenes.stock,'999999999999.99') AS stock,"+
							"to_char(osiris_catalogo_almacenes.minimo_stock,'999999999999.99') AS minstock,"+
							"to_char(osiris_catalogo_almacenes.maximo,'999999999999.99') AS maxstock,"+
							"to_char(osiris_catalogo_almacenes.punto_de_reorden,'999999999999.99') AS reorden,"+
							"to_char(osiris_catalogo_almacenes.fechahora_ultimo_surtimiento,'yyyy-MM-dd HH24:mi:ss') AS fechsurti, "+
							"osiris_productos.id_grupo_producto AS idgrupoproducto,descripcion_grupo_producto,descripcion_grupo1_producto,descripcion_grupo2_producto,"+
							"to_char(osiris_productos.precio_producto_publico,'99999999.99') AS preciopublico,"+
							"to_char(osiris_productos.costo_por_unidad,'999999999.99') AS costoproductounitario,"+
							"to_char(osiris_productos.costo_producto,'999999999.99') AS costoproducto,"+
							"to_char(osiris_productos.cantidad_de_embalaje,'999999999.99') AS embalaje, "+
							"to_char(osiris_productos.porcentage_ganancia,'99999.99') AS porcentageganancia, "+
							"osiris_catalogo_almacenes.tiene_stock "+
							"FROM osiris_catalogo_almacenes,osiris_productos,osiris_grupo_producto,osiris_grupo1_producto,osiris_grupo2_producto "+
							"WHERE osiris_catalogo_almacenes.id_producto = osiris_productos.id_producto "+ 
							"AND osiris_productos.id_grupo_producto = osiris_grupo_producto.id_grupo_producto "+
							"AND osiris_productos.cobro_activo = 'true' "+
							"AND osiris_productos.id_grupo1_producto = osiris_grupo1_producto.id_grupo1_producto "+
							"AND osiris_productos.id_grupo2_producto = osiris_grupo2_producto.id_grupo2_producto "+
							"AND osiris_grupo_producto.agrupacion_4 = 'true' "+
							"AND osiris_catalogo_almacenes.eliminado = 'false' ";
		string query_grupo = " ";
		string query_grupo1 = " ";
		string query_grupo2 = " ";
		string query_stock = " ";
		string tiporeporte = "STOCK";
		string titulo = "REPORTE DE STOCK HOSPITALIZACION";
		
		//Declaracion de ventana de error
		protected Gtk.Window MyWinError;
		protected Gtk.Window MyWin;
		
		private ListStore treeViewEngineBusca;
		private ListStore treeViewEngineBusca2;
		Gtk.TreeModelFilter filter;
		
		//declaracion de columnas y celdas de treeview de busqueda
		TreeViewColumn col_descrip;		CellRendererText cellrt0;
		TreeViewColumn col_existencia;	CellRendererText cellrt1;
		TreeViewColumn col_codigo;		CellRendererText cellrt2;
		TreeViewColumn col_minimo;		CellRendererText cellrt3;
		TreeViewColumn col_maximo;		CellRendererText cellrt4;
		TreeViewColumn col_reorden;		CellRendererText cellrt5;
		TreeViewColumn col_fecha;		CellRendererText cellrt6;
		TreeViewColumn col_embalaje;	CellRendererText cellrt7;
		TreeViewColumn col_enviar;       //public CellRendererText cellrt8;
		TreeViewColumn col_cantenviar;   //public CellRendererText cellrt9;	
		TreeViewColumn col_descripcion;  CellRendererText cellrt8;
		TreeViewColumn col_costo;
		TreeViewColumn col_cantidad;
		TreeViewColumn col_precio;
		TreeViewColumn col_quitar;		CellRendererToggle cel_quitar;
		TreeViewColumn col_es_stock;	CellRendererToggle cel_es_stock;
		TreeViewColumn col_envioinventario;

		Gtk.TreeViewColumn col_12;		Gtk.CellRendererCombo cellrt12;
		Gtk.TreeViewColumn col_13;		Gtk.CellRendererCombo cellrt13;
		Gtk.TreeViewColumn col_14;		Gtk.CellRendererCombo cellrt14;

		Gtk.ListStore cell_combox_store_mes;
		Gtk.ComboBox combobox_mes_inv;

		Gtk.ListStore cell_combox_store_ano;
		Gtk.ComboBox combobox_ano_inv;

		Gtk.ListStore cell_combox_anaqueles;
		Gtk.ComboBox combobox_anaqueles;

		class_conexion conexion_a_DB = new class_conexion();
		class_public classpublic = new class_public();
		
		string[] args_args = {"","AJUSTE DE INVENTARIO","TRASPASO SIN AUT. ENTRE SUB-ALMACENES","AJUSTE MAX/MIN/REORDEN","COPIAR INVENTARIO GENERAL","EDITAR ANAQUELES"};
		int[] args_id_array = {0,1,2,3,4,5,6,7,8};

		public inventario_sub_almacen(string LoginEmp_, string NomEmpleado_, string AppEmpleado_, 
									string ApmEmpleado_, string nombrebd_, int _idsubalmacen_,
									string _descsubalmacen_,int tipoalmacen_,bool tipoacceso_)
		{
			LoginEmpleado = LoginEmp_;
			NomEmpleado = NomEmpleado_;
			AppEmpleado = AppEmpleado_;
			ApmEmpleado = ApmEmpleado_;
			idsubalmacen = _idsubalmacen_;
			idsubalacen_original = _idsubalmacen_;
			descsubalmacen =_descsubalmacen_;
			tipoalmacen = tipoalmacen_;   // 1 = inventario sub-almacenes   2 = Traspasos de Sub-Almacenes   3 = AMBOS para almacen general
			connectionString = conexion_a_DB._url_servidor+conexion_a_DB._port_DB+conexion_a_DB._usuario_DB+conexion_a_DB._passwrd_user_DB;
			nombrebd = conexion_a_DB._nombrebd;
			Glade.XML gxml = new Glade.XML (null, "hospitalizacion.glade", "inventario_sub_almacenes", null);
			gxml.Autoconnect (this);
			inventario_sub_almacenes.Show();
			//Console.WriteLine(descsubalmacen+"   almacen");
			button_salir.Clicked += new EventHandler(on_cierraventanas_clicked);
			button_imprimir.Clicked += new EventHandler(imprime_reporte_stock);
			button_imprime_traspaso.Clicked += new EventHandler(imprime_reporte_traspaso);
			button_actualizar.Clicked += new EventHandler(actualizar);
			radiobutton_con_stock.Clicked += new EventHandler(on_radiobutton_inv_clicked);
			radiobutton_sin_stock.Clicked += new EventHandler(on_radiobutton_inv_clicked);
			radiobutton_todo_stock.Clicked += new EventHandler(on_radiobutton_inv_clicked);
			button_enviar.Clicked += new EventHandler(on_button_enviar_articulos_clicked);
			button_quitar.Clicked += new EventHandler(on_button_quitar_clicked);
			button_export_ods.Clicked += new EventHandler(on_button_export_ods_clicked);
			checkbutton_herramientas.Clicked += new EventHandler(on_checkbutton_herramientas_clicked);
			//entry_numero_de_traspaso.KeyPressEvent += onKeyPressEvent_numero_traspaso;
			combobox_almacen_destino.Sensitive = false;
			//entry_numero_de_traspaso.Text = "0";
			statusbar_inv_sub_hosp.Pop(0);
			statusbar_inv_sub_hosp.Push(1, "login: "+LoginEmpleado+"  |Usuario: "+NomEmpleado+" "+AppEmpleado+" "+ApmEmpleado);
			statusbar_inv_sub_hosp.HasResizeGrip = false;
			if(idsubalacen_original == 1){
				combobox_tipo_almacen.Sensitive = true;
			}else{
				combobox_tipo_almacen.Sensitive = false;
			}
			checkbutton_todoscopy_invfisico.Sensitive = false;
			entry_mes_copy.Sensitive = false;
			entry_ano_copy.Sensitive = false;
			switch (tipoalmacen){	
				case 1:
					//INVENTARIO SUB ALMACENES
					label_almacen_destino.Hide();
					combobox_almacen_destino.Hide();
					button_enviar.Hide();
					query_stock = " AND osiris_catalogo_almacenes.stock > 0";
					crea_treeview_inventarios();
					entry_filter.Changed += OnFilterEntryTextChanged;
					llenado_combobox(1,"",combobox_grupo,"sql","SELECT * FROM osiris_grupo_producto ORDER BY descripcion_grupo_producto;","descripcion_grupo_producto","id_grupo_producto",args_args,args_id_array);
					llenado_combobox(1,"",combobox_grupo1,"sql","SELECT * FROM osiris_grupo1_producto ORDER BY descripcion_grupo1_producto;","descripcion_grupo1_producto","id_grupo1_producto",args_args,args_id_array);
					llenado_combobox(1,"",combobox_grupo2,"sql","SELECT * FROM osiris_grupo2_producto ORDER BY descripcion_grupo2_producto;","descripcion_grupo2_producto","id_grupo2_producto",args_args,args_id_array);
					llenado_combobox(1,descsubalmacen,combobox_tipo_almacen,"sql","SELECT id_almacen,descripcion_almacen,sub_almacen FROM osiris_almacenes WHERE sub_almacen = 'true' ORDER BY descripcion_almacen;","descripcion_almacen","id_almacen",args_args,args_id_array);
					llenado_combobox(1,"",combobox_almacen_destino,"sql","SELECT id_almacen,descripcion_almacen,sub_almacen FROM osiris_almacenes WHERE sub_almacen = 'true' ORDER BY descripcion_almacen;","descripcion_almacen","id_almacen",args_args,args_id_array);
					combobox_tipo_almacen.Sensitive = tipoacceso_;
					checkbutton_herramientas.Sensitive = false;
					combobox_herramientas.Sensitive = false;
					button_enviar.Sensitive = tipoacceso_;
					button_quitar.Sensitive = tipoacceso_;
					button_es_stock.Sensitive = tipoacceso_;
					button_imprime_traspaso.Sensitive = tipoacceso_;
				break;
				case 2:
					//TRASPASO DE SUB ALMACENES
					combobox_tipo_almacen.Sensitive = false;
					combobox_grupo.Sensitive = false;
					combobox_grupo1.Sensitive = false;
					combobox_grupo2.Sensitive = false;
					button_actualizar.Hide();
					button_quitar.Hide();
					query_stock = " AND osiris_catalogo_almacenes.stock > 0";
					crea_treeview_traspaso();
					entry_filter.Changed += OnFilterEntryTextChanged;
					llenado_combobox(1,"",combobox_grupo,"sql","SELECT * FROM osiris_grupo_producto ORDER BY descripcion_grupo_producto;","descripcion_grupo_producto","id_grupo_producto",args_args,args_id_array);
					llenado_combobox(1,"",combobox_grupo1,"sql","SELECT * FROM osiris_grupo1_producto ORDER BY descripcion_grupo1_producto;","descripcion_grupo1_producto","id_grupo1_producto",args_args,args_id_array);
					llenado_combobox(1,"",combobox_grupo2,"sql","SELECT * FROM osiris_grupo2_producto ORDER BY descripcion_grupo2_producto;","descripcion_grupo2_producto","id_grupo2_producto",args_args,args_id_array);
					llenado_combobox(1,descsubalmacen,combobox_tipo_almacen,"sql","SELECT id_almacen,descripcion_almacen,sub_almacen FROM osiris_almacenes WHERE sub_almacen = 'true' ORDER BY descripcion_almacen;","descripcion_almacen","id_almacen",args_args,args_id_array);
					llenado_combobox(1,"",combobox_almacen_destino,"sql","SELECT id_almacen,descripcion_almacen,sub_almacen FROM osiris_almacenes WHERE sub_almacen = 'true' ORDER BY descripcion_almacen;","descripcion_almacen","id_almacen",args_args,args_id_array);
				break;
				case 3:
					// AJUSTE DE INVENTARIO, TRASPASOS no aplica al almacen general, INVENTARIO GENERAL
					button_quitar.Hide();
					query_stock = " AND osiris_catalogo_almacenes.stock > 0";
					crea_treeview_traspaso();
					entry_filter.Changed += OnFilterEntryTextChanged;
					llenado_combobox(1,"",combobox_grupo,"sql","SELECT * FROM osiris_grupo_producto ORDER BY descripcion_grupo_producto;","descripcion_grupo_producto","id_grupo_producto",args_args,args_id_array);
					llenado_combobox(1,"",combobox_grupo1,"sql","SELECT * FROM osiris_grupo1_producto ORDER BY descripcion_grupo1_producto;","descripcion_grupo1_producto","id_grupo1_producto",args_args,args_id_array);
					llenado_combobox(1,"",combobox_grupo2,"sql","SELECT * FROM osiris_grupo2_producto ORDER BY descripcion_grupo2_producto;","descripcion_grupo2_producto","id_grupo2_producto",args_args,args_id_array);
					llenado_combobox(1,descsubalmacen,combobox_tipo_almacen,"sql","SELECT id_almacen,descripcion_almacen,sub_almacen FROM osiris_almacenes WHERE sub_almacen = 'true' ORDER BY descripcion_almacen;","descripcion_almacen","id_almacen",args_args,args_id_array);
					llenado_combobox(1,"",combobox_almacen_destino,"sql","SELECT id_almacen,descripcion_almacen,sub_almacen FROM osiris_almacenes WHERE sub_almacen = 'true' ORDER BY descripcion_almacen;","descripcion_almacen","id_almacen",args_args,args_id_array);
					llenado_combobox(0,"",combobox_herramientas,"array","","","",args_args,args_id_array);
					combobox_herramientas.Sensitive = false;
				break;
			}
			llenando_busqueda_productos();
		}

		void on_radiobutton_inv_clicked(object sender, EventArgs args)
		{
			Gtk.RadioButton radiobutton_opciones = (Gtk.RadioButton) sender;
			switch (radiobutton_opciones.Name.ToString()){
				case "radiobutton_con_stock":
					query_stock = " AND osiris_catalogo_almacenes.stock > 0";
					llenando_busqueda_productos();
					break;
				case "radiobutton_sin_stock":
					query_stock = " AND osiris_catalogo_almacenes.stock <= 0";
					llenando_busqueda_productos();
					break;
				case "radiobutton_todo_stock":
					query_stock = " ";
				llenando_busqueda_productos();
					break;
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
				case "combobox_tipo_almacen":
					idsubalmacen = (int) combobox_tipo_almacen.Model.GetValue(iter,1);
					descripcionalmacen = (string) combobox_tipo_almacen.Model.GetValue(iter,0);
					if(descripcionalmacen== ""){
						idsubalmacen = idsubalacen_original;
					}
					entry_filter.Text = "";
		    		llenando_busqueda_productos();
					break;
				case "combobox_almacen_destino":
					idalmacendestino = (int) this.combobox_almacen_destino.Model.GetValue(iter,1);
					break;
				case "combobox_grupo":
			    	idtipogrupo = (int) combobox_grupo.Model.GetValue(iter,1);
			    	descripgrupo = (string) combobox_grupo.Model.GetValue(iter,0);
			    	query_grupo = "AND osiris_productos.id_grupo_producto = '"+idtipogrupo.ToString()+"' ";
					if(descripgrupo.Trim() == ""){
						query_grupo = " ";
					}
			    	llenando_busqueda_productos();
					break;
				case "combobox_grupo1":
					idtipogrupo1 = (int) combobox_grupo1.Model.GetValue(iter,1);
		    		descripgrupo1 = (string) combobox_grupo1.Model.GetValue(iter,0);
		    		query_grupo1 = "AND osiris_productos.id_grupo1_producto = '"+idtipogrupo1.ToString()+"' ";
					if(descripgrupo1.Trim() == ""){
						query_grupo1 = " ";
					}
		    		llenando_busqueda_productos();
					break;
				case "combobox_grupo2":
					idtipogrupo2 = (int) combobox_grupo2.Model.GetValue(iter,1);
		    		descripgrupo2 = (string) combobox_grupo2.Model.GetValue(iter,0);
		    		query_grupo2 = "AND osiris_productos.id_grupo2_producto = '"+idtipogrupo2.ToString()+"' ";
					if(descripgrupo2.Trim() == ""){
						query_grupo2 = " ";
					}
		    		llenando_busqueda_productos();
					break;
				case "combobox_herramientas":
					tipoherramienta = (int) combobox_herramientas.Model.GetValue(iter,1);
					if(tipoherramienta == 2){
						combobox_almacen_destino.Sensitive = true;
					}else{
						combobox_almacen_destino.Sensitive = false;
					}
					if(tipoherramienta == 4){
						checkbutton_todoscopy_invfisico.Sensitive = true;
						entry_mes_copy.Sensitive = true;
						entry_ano_copy.Sensitive = true;
					}else{
						checkbutton_todoscopy_invfisico.Sensitive = false;
						entry_mes_copy.Sensitive = false;
						entry_ano_copy.Sensitive = false;
					}
					break;
				}
			}
		}
		
		void on_button_export_ods_clicked(object sender, EventArgs args)
		{
			string consulta_sql = query_sql+
								query_grupo+
								query_stock+
								" AND osiris_catalogo_almacenes.id_almacen = '"+idsubalmacen.ToString().Trim()+"' "+
								"ORDER BY osiris_productos.id_grupo_producto,osiris_productos.descripcion_producto;";
			string[] args_names_field = {"idproducto","descripcion_producto","stock","minstock","maxstock","reorden","descripcion_grupo_producto"};
			string[] args_type_field = {"string","string","float","float","float","float","string"};
			string[] args_field_text = {""};
			string[] args_more_title = {""};
			string[,] args_formulas = {{"",""}};
			string[,] args_width = {{"1","10cm"},{"6","4cm"}};
			new osiris.class_traslate_spreadsheet(consulta_sql,args_names_field,args_type_field,false,args_field_text,"",false,args_more_title,args_formulas,args_width);
		}
		
		void crea_treeview_traspaso()
		{			
			cell_combox_store_mes = new ListStore(typeof (string));
			combobox_mes_inv = new Gtk.ComboBox(cell_combox_store_mes);
			combobox_mes_inv.AppendText("01");
			combobox_mes_inv.AppendText("02");
			combobox_mes_inv.AppendText("03");
			combobox_mes_inv.AppendText("04");
			combobox_mes_inv.AppendText("05");
			combobox_mes_inv.AppendText("06");
			combobox_mes_inv.AppendText("07");
			combobox_mes_inv.AppendText("08");
			combobox_mes_inv.AppendText("09");
			combobox_mes_inv.AppendText("10");
			combobox_mes_inv.AppendText("11");
			combobox_mes_inv.AppendText("12");
			combobox_mes_inv.Active = 0;

			cell_combox_store_ano = new ListStore(typeof (string));
			combobox_ano_inv = new Gtk.ComboBox(cell_combox_store_ano);
			combobox_ano_inv.AppendText(Convert.ToString(int.Parse(DateTime.Now.ToString("yyyy"))-1));
			combobox_ano_inv.AppendText(DateTime.Now.ToString("yyyy"));
			combobox_ano_inv.AppendText(Convert.ToString(int.Parse(DateTime.Now.ToString("yyyy"))+1));
			combobox_ano_inv.Active = 0;

			cell_combox_anaqueles = new ListStore(typeof (string));
			combobox_anaqueles = new Gtk.ComboBox(cell_combox_anaqueles);
			combobox_anaqueles.AppendText("Anaquel 01");
			combobox_anaqueles.AppendText("Anaquel 02");
			combobox_anaqueles.AppendText("Anaquel 03");
			combobox_anaqueles.AppendText("Anaquel 04");
			combobox_anaqueles.AppendText("Anaquel 05");
			combobox_anaqueles.AppendText("Anaquel 06");
			combobox_anaqueles.AppendText("Anaquel 07");
			combobox_anaqueles.AppendText("Anaquel 08");
			combobox_anaqueles.AppendText("Anaquel 09");
			combobox_anaqueles.AppendText("Anaquel 10");
			combobox_anaqueles.AppendText("Anaquel 11");
			combobox_anaqueles.AppendText("Anaquel 12");
			combobox_anaqueles.Active = 0;

			treeViewEngineBusca2 = new ListStore(typeof(bool),
												typeof(string),
			                                    typeof(string),
			                                    typeof(string),
			                                    typeof(string),
			                                    typeof(string),
			                                    typeof(string),
			                                    typeof(string),
			                                    typeof(string),
			                                    typeof(string),
												typeof(int),
												typeof(bool),
												typeof(string),
												typeof(string),
												typeof(string),
												typeof(string),
												typeof(string));
			
			lista_almacenes.Model = treeViewEngineBusca2;			
			lista_almacenes.RulesHint = true;

			//lista_almacenes.RowActivated += on_selecciona_almacen_clicked;  // Doble click selecciono paciente
			
			TreeViewColumn col_surtir = new TreeViewColumn();
			CellRendererToggle cel_surtir = new CellRendererToggle();
			col_surtir.Title = "Selec."; // titulo de la cabecera de la columna, si está visible
			col_surtir.PackStart(cel_surtir, true);
			col_surtir.AddAttribute (cel_surtir, "active", 0);
			cel_surtir.Activatable = true;
			cel_surtir.Toggled += selecciona_fila;
			col_surtir.SortColumnId = (int) Col_traspaso.col_surtir;
			
			TreeViewColumn col_autorizado = new TreeViewColumn();
			CellRendererText cel_autorizado = new CellRendererText();
			col_autorizado.Title = "Aut./Ajuste"; // titulo de la cabecera de la columna, si está visible
			col_autorizado.PackStart(cel_autorizado, true);
			col_autorizado.AddAttribute (cel_autorizado, "text", 1);
			col_autorizado.SortColumnId = (int) Col_traspaso.col_autorizado;
			cel_autorizado.Editable = true;
			cel_autorizado.Edited += NumberCellEdited_Autorizado;
			
			col_existencia = new TreeViewColumn();
			cellrt1 = new CellRendererText();
			col_existencia.Title = "Existencia"; // titulo de la cabecera de la columna, si está visible
			col_existencia.PackStart(cellrt1, true);
			col_existencia.AddAttribute (cellrt1, "text", 2);    // la siguiente columna será 1 en vez de 1
			col_existencia.SortColumnId = (int) Col_traspaso.col_existencia;
			
			col_codigo = new TreeViewColumn();
			cellrt2 = new CellRendererText();
			col_codigo.Title = "Codigo";
			col_codigo.PackStart(cellrt2, true);
			col_codigo.AddAttribute (cellrt2, "text", 3); // la siguiente columna será 1 en vez de 2
			col_codigo.SortColumnId = (int) Col_traspaso.col_codigo;
			
			col_descripcion = new TreeViewColumn();
			cellrt8 = new CellRendererText();
			col_descripcion.Title = "Descripcion";
			col_descripcion.PackStart(cellrt8, true);
			col_descripcion.AddAttribute (cellrt8, "text", 4);
			col_descripcion.SortColumnId = (int) Col_traspaso.col_descripcion;
			cellrt8.Width = 400;
			
			col_minimo = new TreeViewColumn();
			cellrt3 = new CellRendererText();
			col_minimo.Title = "Minimo Stock";
			col_minimo.PackStart(cellrt3, true);
			col_minimo.AddAttribute (cellrt3, "text", 7);
			col_minimo.SortColumnId = (int) Col_traspaso.col_minimo;
			cellrt3.Editable = true;
			cellrt3.Edited += NumberCellEdited_minimo;
			
			col_maximo = new TreeViewColumn();
			cellrt4 = new CellRendererText();
			col_maximo.Title = "Maximo Stock";
			col_maximo.PackStart(cellrt4, true);
			col_maximo.AddAttribute (cellrt4, "text", 8);
			col_maximo.SortColumnId = (int) Col_traspaso.col_maximo;
			cellrt4.Editable = true;
			cellrt4.Edited += NumberCellEdited_maximo;
			
			col_reorden = new TreeViewColumn();
			cellrt5 = new CellRendererText();
			col_reorden.Title = "Punto de Reorden";
			col_reorden.PackStart(cellrt5, true);
			col_reorden.AddAttribute (cellrt5, "text", 9);
			col_reorden.SortColumnId = (int) Col_traspaso.col_reorden;
			cellrt5.Editable = true;
			cellrt5.Edited += NumberCellEdited_reorden;

			TreeViewColumn col_envioinventario = new TreeViewColumn();
			CellRendererToggle cel_envioinventario = new CellRendererToggle();
			col_envioinventario.Title = "Envio a Inv."; // titulo de la cabecera de la columna, si está visible
			col_envioinventario.PackStart(cel_envioinventario, true);
			col_envioinventario.AddAttribute (cel_envioinventario, "active", 11);
			cel_envioinventario.Activatable = true;
			cel_envioinventario.Toggled += selecciona_fila11;
			col_envioinventario.SortColumnId = (int) Col_traspaso.col_envioinventario;

			// ComboBox dentro del treeview
			col_12 = new TreeViewColumn();
			cellrt12 = new CellRendererCombo();
			col_12.Title = "Mes Inv.";
			col_12.PackStart(cellrt12, true);
			col_12.AddAttribute(cellrt12, "text", 12);
			col_12.Clickable = false;
			col_12.Sizing = Gtk.TreeViewColumnSizing.Autosize;
			cellrt12.Editable = true;
			cellrt12.Edited += OnEdited_mes;
			cellrt12.HasEntry = false;
			cellrt12.TextColumn = 0;
			cellrt12.Model = cell_combox_store_mes;
			cellrt12.WidthChars = 20;

			// ComboBox dentro del treeview
			col_13 = new TreeViewColumn();
			cellrt13 = new CellRendererCombo();
			col_13.Title = "Año Inv.";
			col_13.PackStart(cellrt13, true);
			col_13.AddAttribute(cellrt13, "text", 13);
			col_13.Clickable = false;
			col_13.Sizing = Gtk.TreeViewColumnSizing.Autosize;
			cellrt13.Editable = true;
			cellrt13.Edited += OnEdited_ano;
			cellrt13.HasEntry = false;
			cellrt13.TextColumn = 0;
			cellrt13.Model = cell_combox_store_ano;
			cellrt13.WidthChars = 20;

			// ComboBox dentro del treeview
			col_14 = new TreeViewColumn();
			cellrt14 = new CellRendererCombo();
			col_14.Title = "Anaquel";
			col_14.PackStart(cellrt14, true);
			col_14.AddAttribute(cellrt14, "text", 14);
			col_14.Clickable = false;
			col_14.Sizing = Gtk.TreeViewColumnSizing.Autosize;
			cellrt14.Editable = true;
			cellrt14.Edited += OnEdited_anaqueles;
			cellrt14.HasEntry = false;
			cellrt14.TextColumn = 0;
			cellrt14.Model = cell_combox_anaqueles;
			cellrt14.WidthChars = 20;

			lista_almacenes.AppendColumn(col_surtir);
			lista_almacenes.AppendColumn(col_autorizado);
			lista_almacenes.AppendColumn(col_existencia);
			lista_almacenes.AppendColumn(col_codigo);
			lista_almacenes.AppendColumn(col_descripcion);
			lista_almacenes.AppendColumn(col_minimo);
			lista_almacenes.AppendColumn(col_maximo);
			lista_almacenes.AppendColumn(col_reorden);
			lista_almacenes.AppendColumn(col_envioinventario);
			lista_almacenes.AppendColumn(col_12);
			lista_almacenes.AppendColumn(col_13);
			lista_almacenes.AppendColumn(col_14);
		}

		void TextCellDataFunc_mes(Gtk.TreeViewColumn tree_column,Gtk.CellRenderer cell,Gtk.TreeModel tree_model,Gtk.TreeIter iter)
		{
			Gtk.CellRendererCombo crc = cell as Gtk.CellRendererCombo;
			crc.Text = (string) lista_almacenes.Model.GetValue (iter,12);
			//crc.Text = (string) filter.Model.GetValue (iter,12);
		}

		void TextCellDataFunc_ano(Gtk.TreeViewColumn tree_column,Gtk.CellRenderer cell,Gtk.TreeModel tree_model,Gtk.TreeIter iter)
		{
			Gtk.CellRendererCombo crc = cell as Gtk.CellRendererCombo;
			crc.Text = (string) lista_almacenes.Model.GetValue (iter,13);
			//crc.Text = (string) filter.Model.GetValue (iter,13);
		}

		void TextCellDataFunc_anaquel(Gtk.TreeViewColumn tree_column,Gtk.CellRenderer cell,Gtk.TreeModel tree_model,Gtk.TreeIter iter)
		{
			Gtk.CellRendererCombo crc = cell as Gtk.CellRendererCombo;
			crc.Text = (string) lista_almacenes.Model.GetValue (iter,14);
			//crc.Text = (string) filter.Model.GetValue (iter,13);
		}

		void OnEdited_mes(object sender, Gtk.EditedArgs args)
		{
			Gtk.TreeIter iter;
			Gtk.TreePath path = new TreePath (args.Path);
			if (filter.Model.GetIter (out iter, path) == false)
				return;
			filter.Model.SetValue(iter, 12, args.NewText );
		}

		void OnEdited_ano(object sender, Gtk.EditedArgs args)
		{
			Gtk.TreeIter iter;
			Gtk.TreePath path = new TreePath (args.Path);
			if (filter.Model.GetIter (out iter, path) == false)
				return;
			filter.Model.SetValue(iter, 13, args.NewText );
		}

		void OnEdited_anaqueles(object sender, Gtk.EditedArgs args)
		{
			Gtk.TreeIter iter;
			Gtk.TreePath path = new TreePath (args.Path);
			if (filter.Model.GetIter (out iter, path) == false)
				return;
			filter.Model.SetValue(iter, 14, args.NewText );
		}

		enum Col_traspaso
		{
			col_surtir,
			col_autorizado,
			col_existencia,
			col_codigo,
			col_descripcion,
			col_minimo,
			col_maximo,
			col_reorden,
			col_envioinventario
		}
		
		void on_button_quitar_clicked (object sender, EventArgs args)		
		{
			string[,] parametros;
			object[] paraobj;
			if((string) classpublic.lee_registro_de_tabla("osiris_empleado","acceso_elimina_prodalmacen","WHERE acceso_elimina_prodalmacen = 'true' AND login_empleado = '"+LoginEmpleado+"' ","acceso_elimina_prodalmacen","bool") == "True"){
				MessageDialog msgBox2 = new MessageDialog (MyWin,DialogFlags.Modal,
						                 MessageType.Question,ButtonsType.YesNo,"¿ Esta seguro de Borrar los Materiales Seleccionados?");
				ResponseType miResultado2 = (ResponseType)msgBox2.Run ();
				msgBox2.Destroy();					
				if (miResultado2 == ResponseType.Yes){
					TreeIter iterSelected;
					if(this.treeViewEngineBusca.GetIterFirst (out iterSelected)){
						if((bool) filter.Model.GetValue (iterSelected,8) == true){
							parametros = new string[,] {
								{ "eliminado = '","true' " },
								{ "WHERE id_almacen = '",idsubalmacen.ToString ()+"' "},
								{ "AND id_producto = '",filter.Model.GetValue (iterSelected, 2).ToString ().Trim ()+"' "}
							};
							paraobj = new object[] { entry_filter};
							new osiris.update_registro ("osiris_catalogo_almacenes", parametros, paraobj);
						}
						if ((bool) filter.Model.GetValue (iterSelected,9) == true){
							parametros = new string[,] {
								{ "tiene_stock = '","true' " },
								{ "WHERE id_almacen = '",idsubalmacen.ToString ()+"' "},
								{ "AND id_producto = '",filter.Model.GetValue (iterSelected, 2).ToString ().Trim ()+"' "}
							};
							paraobj = new object[] { entry_filter};
							new osiris.update_registro ("osiris_catalogo_almacenes", parametros, paraobj);
						}
					}
					while (treeViewEngineBusca.IterNext(ref iterSelected)){
						if ((bool) filter.Model.GetValue (iterSelected,8) == true){
							parametros = new string[,] {
								{ "eliminado = '","true' " },
								{ "WHERE id_almacen = '",idsubalmacen.ToString ()+"' "},
								{ "AND id_producto = '",filter.Model.GetValue (iterSelected, 2).ToString ().Trim ()+"' "}
							};
							paraobj = new object[] { entry_filter};
							new osiris.update_registro ("osiris_catalogo_almacenes", parametros, paraobj);
						}
						if ((bool) filter.Model.GetValue (iterSelected,9) == true){
							parametros = new string[,] {
								{ "tiene_stock = '","true' " },
								{ "WHERE id_almacen = '",idsubalmacen.ToString ()+"' "},
								{ "AND id_producto = '",filter.Model.GetValue (iterSelected, 2).ToString ().Trim ()+"' "}
							};
							paraobj = new object[] { entry_filter};
							new osiris.update_registro ("osiris_catalogo_almacenes", parametros, paraobj);
						}
					}
				}
				llenando_busqueda_productos();
			}else{
				MessageDialog msgBox = new MessageDialog (MyWin,DialogFlags.Modal,
					MessageType.Info,ButtonsType.Ok,"No tiene Permiso para Borrar Articulos");
				msgBox.Run ();msgBox.Destroy();
			}
		}

		// Cuando seleccion el treeview de cargos extras para cargar los productos  
		void selecciona_fila(object sender, ToggledArgs args)
		{
			TreeIter iter;
			if (filter.Model.GetIter(out iter, new TreePath (args.Path))) {
				bool old = (bool) filter.Model.GetValue(iter,0);
				filter.Model.SetValue(iter,0,!old);				
			}			
			
			//TreeModel model;
			//TreeIter iterSelected;
			//if (lista_almacenes.Selection.GetSelected(out model, out iterSelected)){
			//	bool old = (bool) model.GetValue (iterSelected, 0);
			//	filter.Model.SetValue(iterSelected,0,!old);
			//}						
		}

		void NumberCellEdited_Autorizado(object o, EditedArgs args)
		{
			Gtk.TreeIter iter;
			bool esnumerico = false;
			int var_paso = 0;
			int largo_variable = args.NewText.ToString().Length;
			string toma_variable = args.NewText.ToString();
			filter.Model.GetIter (out iter, new Gtk.TreePath (args.Path));
			while (var_paso < largo_variable){		
				if ((string) toma_variable.Substring(var_paso,1).ToString() == "." || 
					(string) toma_variable.Substring(var_paso,1).ToString() == "0" ||
					(string) toma_variable.Substring(var_paso,1).ToString() == "1" || 
					(string) toma_variable.Substring(var_paso,1).ToString() == "2" ||
					(string) toma_variable.Substring(var_paso,1).ToString() == "3" ||
					(string) toma_variable.Substring(var_paso,1).ToString() == "4" ||
					(string) toma_variable.Substring(var_paso,1).ToString() == "5" || 
					(string) toma_variable.Substring(var_paso,1).ToString() == "6" || 
					(string) toma_variable.Substring(var_paso,1).ToString() == "7" || 
					(string) toma_variable.Substring(var_paso,1).ToString() == "8" ||
					(string) toma_variable.Substring(var_paso,1).ToString() == "9") {
					esnumerico = true;
				}else{
					esnumerico = false;
					var_paso = largo_variable;
				}
				var_paso += 1;
			}
			if (esnumerico == true){		
				filter.Model.SetValue(iter,(int) Col_traspaso.col_autorizado,args.NewText);
				bool old = (bool) filter.Model.GetValue (iter,0);
				filter.Model.SetValue(iter,0,!old);
			}
		}

		void NumberCellEdited_minimo(object o, EditedArgs args)
		{
			Gtk.TreeIter iter;
			bool esnumerico = false;
			int var_paso = 0;
			int largo_variable = args.NewText.ToString().Length;
			string toma_variable = args.NewText.ToString();
			filter.Model.GetIter (out iter, new Gtk.TreePath (args.Path));
			while (var_paso < largo_variable){				
				if ((string) toma_variable.Substring(var_paso,1).ToString() == "." || 
					(string) toma_variable.Substring(var_paso,1).ToString() == "0" ||
					(string) toma_variable.Substring(var_paso,1).ToString() == "1" || 
					(string) toma_variable.Substring(var_paso,1).ToString() == "2" ||
					(string) toma_variable.Substring(var_paso,1).ToString() == "3" ||
					(string) toma_variable.Substring(var_paso,1).ToString() == "4" ||
					(string) toma_variable.Substring(var_paso,1).ToString() == "5" || 
					(string) toma_variable.Substring(var_paso,1).ToString() == "6" || 
					(string) toma_variable.Substring(var_paso,1).ToString() == "7" || 
					(string) toma_variable.Substring(var_paso,1).ToString() == "8" ||
					(string) toma_variable.Substring(var_paso,1).ToString() == "9") {
					esnumerico = true;
				}else{
					esnumerico = false;
					var_paso = largo_variable;
				}
				var_paso += 1;
			}
			if (esnumerico == true){		
				filter.Model.SetValue(iter,7,args.NewText);
				//bool old = (bool) filter.Model.GetValue (iter,7);
				//filter.Model.SetValue(iter,7,!old);
			}
		}

		void NumberCellEdited_maximo(object o, EditedArgs args)
		{
			Gtk.TreeIter iter;
			bool esnumerico = false;
			int var_paso = 0;
			int largo_variable = args.NewText.ToString().Length;
			string toma_variable = args.NewText.ToString();
			filter.Model.GetIter (out iter, new Gtk.TreePath (args.Path));
			while (var_paso < largo_variable){				
				if ((string) toma_variable.Substring(var_paso,1).ToString() == "." || 
					(string) toma_variable.Substring(var_paso,1).ToString() == "0" ||
					(string) toma_variable.Substring(var_paso,1).ToString() == "1" || 
					(string) toma_variable.Substring(var_paso,1).ToString() == "2" ||
					(string) toma_variable.Substring(var_paso,1).ToString() == "3" ||
					(string) toma_variable.Substring(var_paso,1).ToString() == "4" ||
					(string) toma_variable.Substring(var_paso,1).ToString() == "5" || 
					(string) toma_variable.Substring(var_paso,1).ToString() == "6" || 
					(string) toma_variable.Substring(var_paso,1).ToString() == "7" || 
					(string) toma_variable.Substring(var_paso,1).ToString() == "8" ||
					(string) toma_variable.Substring(var_paso,1).ToString() == "9") {
					esnumerico = true;
				}else{
					esnumerico = false;
					var_paso = largo_variable;
				}
				var_paso += 1;
			}
			if (esnumerico == true){		
				filter.Model.SetValue(iter,8,args.NewText);
				//bool old = (bool) filter.Model.GetValue (iter,8);
				//filter.Model.SetValue(iter,8,!old);
			}
		}

		void NumberCellEdited_reorden(object o, EditedArgs args)
		{
			Gtk.TreeIter iter;
			bool esnumerico = false;
			int var_paso = 0;
			int largo_variable = args.NewText.ToString().Length;
			string toma_variable = args.NewText.ToString();
			filter.Model.GetIter (out iter, new Gtk.TreePath (args.Path));
			while (var_paso < largo_variable){				
				if ((string) toma_variable.Substring(var_paso,1).ToString() == "." || 
					(string) toma_variable.Substring(var_paso,1).ToString() == "0" ||
					(string) toma_variable.Substring(var_paso,1).ToString() == "1" || 
					(string) toma_variable.Substring(var_paso,1).ToString() == "2" ||
					(string) toma_variable.Substring(var_paso,1).ToString() == "3" ||
					(string) toma_variable.Substring(var_paso,1).ToString() == "4" ||
					(string) toma_variable.Substring(var_paso,1).ToString() == "5" || 
					(string) toma_variable.Substring(var_paso,1).ToString() == "6" || 
					(string) toma_variable.Substring(var_paso,1).ToString() == "7" || 
					(string) toma_variable.Substring(var_paso,1).ToString() == "8" ||
					(string) toma_variable.Substring(var_paso,1).ToString() == "9") {
					esnumerico = true;
				}else{
					esnumerico = false;
					var_paso = largo_variable;
				}
				var_paso += 1;
			}
			if (esnumerico == true){		
				filter.Model.SetValue(iter,9,args.NewText);
				//bool old = (bool) filter.Model.GetValue (iter,8);
				//filter.Model.SetValue(iter,8,!old);
			}
		}

		void selecciona_fila11(object sender, ToggledArgs args)
		{
			TreeIter iter;
			TreePath path = new TreePath (args.Path);
			if (filter.Model.GetIter(out iter, path)) {
				bool old = (bool) filter.Model.GetValue(iter,11);
				filter.Model.SetValue(iter,11,!old);				
			}
		}
		
		void selecciona_fila2(object obj, ToggledArgs args)
		{
			Gtk.CellRendererToggle check_toggle = (Gtk.CellRendererToggle) obj;
			// Gtk.ComboBox combobox_almacen_origen = obj as ComboBox;
			// Gtk.RadioButton radiobutton_filtros = (Gtk.RadioButton) obj;
			TreeIter iter;
			TreePath path = new TreePath (args.Path);
			if (filter.Model.GetIter (out iter, path)){
				bool old = (bool) filter.Model.GetValue (iter,8);
				//lista_almacenes.Model.SetValue(iter,8,!old);
				filter.Model.SetValue(iter,8,!old);
			}	
		}
		
		void selecciona_fila3(object obj, ToggledArgs args)
		{
			Gtk.CellRendererToggle check_toggle = (Gtk.CellRendererToggle) obj;
			// Gtk.ComboBox combobox_almacen_origen = obj as ComboBox;
			// Gtk.RadioButton radiobutton_filtros = (Gtk.RadioButton) obj;
			TreeIter iter;
			TreePath path = new TreePath (args.Path);
			if (filter.Model.GetIter (out iter, path)){				
				bool old = (bool) filter.Model.GetValue (iter,9);
				//lista_almacenes.Model.SetValue(iter,9,!old);
				filter.Model.SetValue(iter,9,!old);
			}	
		}
						
		void crea_treeview_inventarios()
		{			
			treeViewEngineBusca = new ListStore(typeof(string),
												typeof(string),
												typeof(string),
												typeof(string),
												typeof(string),
												typeof(string),
												typeof(string),
												typeof(string),
												typeof(bool),
			                                    typeof(bool));
			lista_almacenes.Model = treeViewEngineBusca;
			
			lista_almacenes.RulesHint = true;
			
			//lista_almacenes.RowActivated += on_selecciona_almacen_clicked;  // Doble click selecciono paciente
			
			col_descrip = new TreeViewColumn();
			cellrt0 = new CellRendererText();
			col_descrip.Title = "Descripcion";
			col_descrip.PackStart(cellrt0, true);
			col_descrip.AddAttribute (cellrt0, "text", 0); // la siguiente columna será 1 en vez de 2
			col_descrip.Resizable = true;
			cellrt0.Width = 400;
			col_descrip.SortColumnId = (int) Col_inv_sub_almacen.col_descrip;
						
			col_cantidad = new TreeViewColumn();
			cellrt1 = new CellRendererText();
			col_cantidad.Title = "Existencia"; // titulo de la cabecera de la columna, si está visible
			col_cantidad.PackStart(cellrt1, true);
			col_cantidad.AddAttribute (cellrt1, "text", 1);    // la siguiente columna será 1 en vez de 1
			col_cantidad.SortColumnId = (int) Col_inv_sub_almacen.col_cantidad;
						
			col_codigo = new TreeViewColumn();
			cellrt2 = new CellRendererText();
			col_codigo.Title = "Codigo";
			col_codigo.PackStart(cellrt2, true);
			col_codigo.AddAttribute (cellrt2, "text", 2); // la siguiente columna será 1 en vez de 2
			col_codigo.SortColumnId = (int) Col_inv_sub_almacen.col_codigo;
			
			col_minimo = new TreeViewColumn();
			cellrt3 = new CellRendererText();
			col_minimo.Title = "Minimo Stock";
			col_minimo.PackStart(cellrt3, true);
			col_minimo.AddAttribute (cellrt3, "text", 3); // la siguiente columna será 1 en vez de 2
			col_minimo.SortColumnId = (int) Col_inv_sub_almacen.col_minimo;
			
			col_maximo = new TreeViewColumn();
			cellrt4 = new CellRendererText();
			col_maximo.Title = "Maximo Stock";
			col_maximo.PackStart(cellrt4, true);
			col_maximo.AddAttribute (cellrt4, "text", 4); // la siguiente columna será 1 en vez de 2
			col_maximo.SortColumnId = (int) Col_inv_sub_almacen.col_maximo;
			
			col_reorden = new TreeViewColumn();
			cellrt5 = new CellRendererText();
			col_reorden.Title = "Punto de Reorden";
			col_reorden.PackStart(cellrt5, true);
			col_reorden.AddAttribute (cellrt5, "text", 5); // la siguiente columna será 1 en vez de 2
			col_reorden.SortColumnId = (int) Col_inv_sub_almacen.col_reorden;
						
			col_fecha = new TreeViewColumn();
			cellrt6 = new CellRendererText();
			col_fecha.Title = "Fec. Ultimo Surtido";
			col_fecha.PackStart(cellrt6, true);
			col_fecha.AddAttribute (cellrt6, "text", 6); // la siguiente columna será 1 en vez de 2
			col_fecha.SortColumnId = (int) Col_inv_sub_almacen.col_fecha;
			
			col_embalaje = new TreeViewColumn();
			cellrt7 = new CellRendererText();
			col_embalaje.Title = "Embalaje";
			col_embalaje.PackStart(cellrt7, true);
			col_embalaje.AddAttribute (cellrt7, "text", 7); // la siguiente columna será 1 en vez de 2
			col_embalaje.SortColumnId = (int) Col_inv_sub_almacen.col_embalaje;
			
			col_quitar = new TreeViewColumn();
			cel_quitar = new CellRendererToggle();
			col_quitar.Title = "Borrar"; // titulo de la cabecera de la columna, si está visible
			col_quitar.PackStart(cel_quitar, true);
			col_quitar.AddAttribute (cel_quitar, "active", 8);
			cel_quitar.Activatable = true;
			cel_quitar.Toggled += selecciona_fila2;
			col_quitar.SortColumnId = (int) Col_inv_sub_almacen.col_quitar;
			
			col_es_stock = new TreeViewColumn();
			cel_es_stock = new CellRendererToggle();
			col_es_stock.Title = "Es de Stock"; // titulo de la cabecera de la columna, si está visible
			col_es_stock.PackStart(cel_es_stock, true);
			col_es_stock.AddAttribute (cel_es_stock, "active", 9);
			cel_es_stock.Activatable = true;
			cel_es_stock.Toggled += selecciona_fila3;
			col_es_stock.SortColumnId = (int) Col_inv_sub_almacen.col_es_stock;
			
			lista_almacenes.AppendColumn(col_descrip);
			lista_almacenes.AppendColumn(col_cantidad);
			lista_almacenes.AppendColumn(col_codigo);
			lista_almacenes.AppendColumn(col_minimo);
			lista_almacenes.AppendColumn(col_maximo);
			lista_almacenes.AppendColumn(col_reorden);
			lista_almacenes.AppendColumn(col_fecha);
			lista_almacenes.AppendColumn(col_embalaje);
			lista_almacenes.AppendColumn(col_quitar);
			lista_almacenes.AppendColumn(col_es_stock);						
		}
		
		void OnFilterEntryTextChanged (object o, System.EventArgs args)
		{
			// Since the filter text changed, tell the filter to re-determine which rows to display
			filter.Refilter ();
		}
	 
		bool FilterTree (Gtk.TreeModel model, Gtk.TreeIter iter)
		{
			if (entry_filter.Text == ""){
			    return true;
			}
			string contactname = model.GetValue (iter, 0).ToString ();
			if (contactname.IndexOf (entry_filter.Text) > -1){
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
		        if (model.GetValue(childIter, 0).ToString().IndexOf(entry_filter.Text) > -1)
		            filerBool = true;
		
		        if (model.IterHasChild(childIter))
		            investigateChildNodes(model, childIter);
		
		    }while (model.IterNext(ref childIter));
		}
		
		bool FilterTree2 (Gtk.TreeModel model, Gtk.TreeIter iter)
		{
			if (entry_filter.Text == "")
				return true;
			entryfilter_productos = entry_filter.Text.ToString().Trim();
			string ProductName = model.GetValue (iter, 4).ToString ();
			//string ProductName = filter.Model.GetValue (iter, 4).ToString ();
			if (ProductName.IndexOf (entry_filter.Text.ToString().ToUpper()) > -1)
				return true;
			else
				return false;	
		}
		
		enum Col_inv_sub_almacen
		{
			col_descrip,
			col_cantidad,
			col_codigo,
			col_minimo,
			col_maximo,
			col_reorden,
			col_fecha,
			col_embalaje,
			col_quitar,col_es_stock
		}
		
		void actualizar(object sender, EventArgs args)
		{
			llenando_busqueda_productos();
		}	
		
		void llenando_busqueda_productos()
		{
			if(this.tipoalmacen==1){
				treeViewEngineBusca.Clear(); // Limpia el treeview cuando realiza una nueva busqueda
			}
			if(this.tipoalmacen==2){
				treeViewEngineBusca2.Clear(); // Limpia el treeview cuando realiza una nueva busqueda
			}
			if(this.tipoalmacen==3){
				treeViewEngineBusca2.Clear(); // Limpia el treeview cuando realiza una nueva busqueda
			}
			if(tipoalmacen == 1){
				filter = new Gtk.TreeModelFilter (treeViewEngineBusca, null);
				filter.VisibleFunc = new Gtk.TreeModelFilterVisibleFunc (FilterTree);
				lista_almacenes.Model = filter;
			}
			if(tipoalmacen == 2){
				filter = new Gtk.TreeModelFilter (treeViewEngineBusca2, null);
				filter.VisibleFunc = new Gtk.TreeModelFilterVisibleFunc (FilterTree2);
				lista_almacenes.Model = filter;
			}
			if(tipoalmacen == 3){
				filter = new Gtk.TreeModelFilter (treeViewEngineBusca2, null);
				filter.VisibleFunc = new Gtk.TreeModelFilterVisibleFunc (FilterTree2);
				lista_almacenes.Model = filter;
			}
			NpgsqlConnection conexion;
			conexion = new NpgsqlConnection (connectionString+nombrebd);
	       
			// Verifica que la base de datos este conectada
			try{
				conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand ();	          	
				comando.CommandText = query_sql+
							query_grupo+
							//query_grupo1+
							//query_grupo2+
							query_stock+
							" AND osiris_catalogo_almacenes.id_almacen = '"+idsubalmacen.ToString().Trim()+"' "+
							"ORDER BY descripcion_producto;";
				//Console.WriteLine(comando.CommandText);
				NpgsqlDataReader lector = comando.ExecuteReader ();
				while (lector.Read()){
					if(tipoalmacen==1){
						treeViewEngineBusca.AppendValues ((string) lector["descripcion_producto"],
														(string) lector["stock"], 
														(string) lector["idproducto"],
														(string) lector["minstock"],
														(string) lector["maxstock"],
														(string) lector["reorden"],
														(string) lector["fechsurti"],
														(string) lector["embalaje"],
														false,
						                                false);
													
						col_descrip.SetCellDataFunc(cellrt0, new Gtk.TreeCellDataFunc(cambia_colores_fila_productos));
						col_cantidad.SetCellDataFunc(cellrt1, new Gtk.TreeCellDataFunc(cambia_colores_fila_productos));
						col_codigo.SetCellDataFunc(cellrt2, new Gtk.TreeCellDataFunc(cambia_colores_fila_productos));
						col_minimo.SetCellDataFunc(cellrt3, new Gtk.TreeCellDataFunc(cambia_colores_fila_productos));
						col_maximo.SetCellDataFunc(cellrt4, new Gtk.TreeCellDataFunc(cambia_colores_fila_productos));
						col_reorden.SetCellDataFunc(cellrt5, new Gtk.TreeCellDataFunc(cambia_colores_fila_productos));
						col_fecha.SetCellDataFunc(cellrt6, new Gtk.TreeCellDataFunc(cambia_colores_fila_productos));
						col_embalaje.SetCellDataFunc(cellrt7, new Gtk.TreeCellDataFunc(cambia_colores_fila_productos));						
					}
				
					if(tipoalmacen==2){
						treeViewEngineBusca2.AppendValues (false,
							                                   "0",
							                                   (string) lector["stock"],
							                                   (string) lector["idproducto"],
							                                   (string) lector["descripcion_producto"],
							                                   (string) lector["costoproductounitario"],
							                                   (string) lector["preciopublico"],
						                                   		"",
						                                   		"");
					}
				
					if(tipoalmacen==3){
						treeViewEngineBusca2.AppendValues (false,
							                                   "0",
							                                   (string) lector["stock"],
							                                   (string) lector["idproducto"],
							                                   (string) lector["descripcion_producto"],
							                                   (string) lector["costoproductounitario"],
							                                   (string) lector["preciopublico"],
						                                   		(string) lector["minstock"],
																(string) lector["maxstock"],
																(string) lector["reorden"],
																(int) lector["id_secuencia"],
																false,
																DateTime.Now.ToString("MM"),
																DateTime.Now.ToString("yyyy"),
																"Anaquel 01");
						col_12.SetCellDataFunc (cellrt12,new Gtk.TreeCellDataFunc (TextCellDataFunc_mes));
						col_13.SetCellDataFunc (cellrt13,new Gtk.TreeCellDataFunc (TextCellDataFunc_ano));
						col_14.SetCellDataFunc (cellrt14,new Gtk.TreeCellDataFunc (TextCellDataFunc_anaquel));
					}
				}				
			}catch (NpgsqlException ex){
		   		Console.WriteLine ("PostgresSQL error: {0}",ex.Message);
		   		MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
							MessageType.Info,ButtonsType.Close, "PostgresSQL error: {0} ",ex.Message);
				msgBoxError.Run ();					msgBoxError.Destroy();
			}
			conexion.Close ();
		}
		
		// funcion para cambiar el color de una fila y columna
		void cambia_colores_fila_productos(Gtk.TreeViewColumn column, Gtk.CellRenderer cell, Gtk.TreeModel model, Gtk.TreeIter iter)
		{
			//descripcion_producto descrip = (descripcion_producto) model.GetValue (iter, 14);
			if ( float.Parse((string) this.lista_almacenes.Model.GetValue (iter,1)) > 0 ){
				(cell as Gtk.CellRendererText).Foreground = "black";
			}else{
				(cell as Gtk.CellRendererText).Foreground = "red";
			}
		}
		
		// funcion para cambiar el color de una fila y columna
		void cambia_colores_fila_productos2(Gtk.TreeViewColumn column, Gtk.CellRenderer cell, Gtk.TreeModel model, Gtk.TreeIter iter)
		{
			//descripcion_producto descrip = (descripcion_producto) model.GetValue (iter, 14);
			if ( float.Parse((string) this.lista_almacenes.Model.GetValue (iter,2)) > 0 ){
				(cell as Gtk.CellRendererText).Foreground = "black";
			}else{
				(cell as Gtk.CellRendererText).Foreground = "red";
			}
		}
		
		void on_button_enviar_articulos_clicked(object sender, EventArgs args)
		{
			string[,] parametros;
			object[] paraobj;
			if (tipoherramienta == 1){
				// ajuste de inventario
				if((string) classpublic.lee_registro_de_tabla("osiris_empleado","acceso_ajuste_inv","WHERE acceso_ajuste_inv = 'true' AND login_empleado = '"+LoginEmpleado+"' ","acceso_ajuste_inv","bool") == "True"){
					MessageDialog msgBox2 = new MessageDialog (MyWin,DialogFlags.Modal,
						MessageType.Question,ButtonsType.YesNo,"¿ Esta seguro de Realizar el Ajuste de los Materiales Seleccionados?");
					ResponseType miResultado2 = (ResponseType)msgBox2.Run ();
					msgBox2.Destroy();					
					if (miResultado2 == ResponseType.Yes){
						entry_filter.Text = "";
						TreeIter iterSelected2;								
						if (treeViewEngineBusca2.GetIterFirst (out iterSelected2)){
							if ((bool) filter.Model.GetValue (iterSelected2,0) == true){
								//if (decimal.Parse((string) this.lista_almacenes.Model.GetValue (iterSelected2,1)) != 0 ){
								NpgsqlConnection conexion;
								conexion = new NpgsqlConnection (connectionString+nombrebd);
								try{
									conexion.Open ();
									NpgsqlCommand comando; 
									comando = conexion.CreateCommand();
									comando.CommandText = "UPDATE osiris_catalogo_almacenes SET stock = '"+(string) filter.Model.GetValue(iterSelected2,1)+"',"+
										"historial_ajustes = historial_ajustes || '"+LoginEmpleado.Trim()+";"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss").Trim()+";"+
										Convert.ToString((string) filter.Model.GetValue (iterSelected2,2)).Trim()+";"+
										Convert.ToString((string) filter.Model.GetValue (iterSelected2,1)).Trim()+"\n' "+
										"WHERE id_almacen = '"+idsubalmacen.ToString()+"' "+
										"AND eliminado = 'false' " +
										"AND id_producto = '"+(string) filter.Model.GetValue(iterSelected2,3)+"' ;";
									//Console.WriteLine(comando.CommandText.ToString());
									comando.ExecuteNonQuery();
									comando.Dispose();
									// realiza un insert en el inventario fisico solo si marca la casilla
									if ((bool) filter.Model.GetValue (iterSelected2,11) == true){
										NpgsqlConnection conexion1;
										conexion1 = new NpgsqlConnection (connectionString+nombrebd);
										try{
											conexion1.Open ();
											NpgsqlCommand comando1; 
											comando1 = conexion.CreateCommand();
											comando1.CommandText = "INSERT INTO osiris_inventario_almacenes(" +
												"id_producto," +
												"id_almacen," +
												"stock," +
												"fechahora_alta," +
												"id_quien_creo," +
												"ano_inventario," +
												"mes_inventario," +
												"lote," +
												"caducidad," +
												"codigo_barras," +
												"ajuste_inventario," +
												"stock_antes_ajuste) " +
												"VALUES ('"+
												filter.Model.GetValue(iterSelected2,3).ToString().Trim()+"','"+
												idsubalmacen.ToString()+"','"+
												filter.Model.GetValue(iterSelected2,1).ToString().Trim()+"','"+
												DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss").Trim()+"','"+
												LoginEmpleado.Trim()+"','"+
												filter.Model.GetValue(iterSelected2,13).ToString().Trim()+"','"+
												filter.Model.GetValue(iterSelected2,12).ToString().Trim()+"','"+
												"','"+
												"','"+
												"','"+
												"true','"+
												(string) filter.Model.GetValue(iterSelected2,2)+"')";
											Console.WriteLine(comando1.CommandText.ToString());
											comando1.ExecuteNonQuery();					comando1.Dispose();

										}catch (NpgsqlException ex){
											MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
												MessageType.Error, 
												ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
											msgBoxError.Run (); msgBoxError.Destroy();
										}
										conexion1.Close();
									}
								}catch (NpgsqlException ex){
									MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
										MessageType.Error, 
										ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
									msgBoxError.Run (); msgBoxError.Destroy();
								}
								conexion.Close();
							}
						}

						while (treeViewEngineBusca2.IterNext(ref iterSelected2)){
							if ((bool) filter.Model.GetValue (iterSelected2,0) == true){
								//if (decimal.Parse((string) this.lista_almacenes.Model.GetValue (iterSelected2,2)) != 0 ){
								NpgsqlConnection conexion;
								conexion = new NpgsqlConnection (connectionString+nombrebd);
								try{
									conexion.Open ();
									NpgsqlCommand comando; 
									comando = conexion.CreateCommand();
									comando.CommandText = "UPDATE osiris_catalogo_almacenes SET stock = '"+(string) filter.Model.GetValue(iterSelected2,1)+"',"+
										"historial_ajustes = historial_ajustes || '"+LoginEmpleado.Trim()+";"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss").Trim()+";"+
										Convert.ToString((string) filter.Model.GetValue (iterSelected2,2)).Trim()+";"+
										Convert.ToString((string) filter.Model.GetValue (iterSelected2,1)).Trim()+"\n' "+
										"WHERE id_almacen = '"+idsubalmacen.ToString()+"' "+
										"AND eliminado = 'false' " +
										"AND id_producto = '"+(string) filter.Model.GetValue(iterSelected2,3)+"' ;";
									comando.ExecuteNonQuery();
									comando.Dispose();
									// realiza un insert en el inventario fisico solo si marca la casilla
									if ((bool) filter.Model.GetValue (iterSelected2,11) == true){
										NpgsqlConnection conexion1;
										conexion1 = new NpgsqlConnection (connectionString+nombrebd);
										try{
											conexion1.Open ();
											NpgsqlCommand comando1; 
											comando1 = conexion.CreateCommand();
											comando1.CommandText = "INSERT INTO osiris_inventario_almacenes(" +
												"id_producto," +
												"id_almacen," +
												"stock," +
												"fechahora_alta," +
												"id_quien_creo," +
												"ano_inventario," +
												"mes_inventario," +
												"lote," +
												"caducidad," +
												"codigo_barras," +
												"ajuste_inventario," +
												"stock_antes_ajuste) " +
												"VALUES ('"+
												filter.Model.GetValue(iterSelected2,3).ToString().Trim()+"','"+
												idsubalmacen.ToString()+"','"+
												filter.Model.GetValue(iterSelected2,1).ToString().Trim()+"','"+
												DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss").Trim()+"','"+
												LoginEmpleado.Trim()+"','"+
												filter.Model.GetValue(iterSelected2,13).ToString().Trim()+"','"+
												filter.Model.GetValue(iterSelected2,12).ToString().Trim()+"','"+
												"','"+
												"','"+
												"','"+
												"true','"+
												(string) filter.Model.GetValue(iterSelected2,2)+"')";
											Console.WriteLine(comando1.CommandText.ToString());
											comando1.ExecuteNonQuery();					comando1.Dispose();

										}catch (NpgsqlException ex){
											MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
												MessageType.Error, 
												ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
											msgBoxError.Run (); msgBoxError.Destroy();
										}
										conexion1.Close();
									}

								}catch (NpgsqlException ex){
									MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
										MessageType.Error, 
										ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
									msgBoxError.Run ();
								}
								conexion.Close();
							}
						}
						llenando_busqueda_productos();							
					}
				}else{
					MessageDialog msgBox = new MessageDialog (MyWin,DialogFlags.Modal,
						MessageType.Info,ButtonsType.Ok,"No tiene Permiso para Ajustar el Inventario de Articulos");
					msgBox.Run ();msgBox.Destroy();					
				}
			}

			if (tipoherramienta == 2){
				// Traspasos de inventarios
				if((string) classpublic.lee_registro_de_tabla("osiris_empleado","acceso_tras_almacenes","WHERE acceso_traspasos_inv = 'true' AND login_empleado = '"+LoginEmpleado+"' ","acceso_traspasos_inv","bool") == "True"){
					//if (idsubalmacen != 1){
						MessageDialog msgBox = new MessageDialog (MyWin,DialogFlags.Modal,
							                         MessageType.Question,ButtonsType.YesNo,"¿ Esta seguro de traspasar los Materiales Seleccionados?");
						ResponseType miResultado = (ResponseType)msgBox.Run ();
						msgBox.Destroy();					
						if (miResultado == ResponseType.Yes){
							TreeIter iterSelected;
							if (this.treeViewEngineBusca2.GetIterFirst (out iterSelected)){
								if ((bool) filter.Model.GetValue (iterSelected,0) == true){
									if (decimal.Parse((string) filter.Model.GetValue (iterSelected,2)) > 0 ){
										NpgsqlConnection conexion;
										conexion = new NpgsqlConnection (connectionString+nombrebd);
					 					try{
											conexion.Open ();
											NpgsqlCommand comando; 
											comando = conexion.CreateCommand();
											comando.CommandText = "SELECT id_producto,id_almacen,stock FROM osiris_catalogo_almacenes "+
														"WHERE id_almacen = '"+this.idalmacendestino.ToString()+"' "+
															"AND eliminado = 'false' "+														
															"AND id_producto = '"+(string) filter.Model.GetValue(iterSelected,3)+"' ;";
										
											NpgsqlDataReader lector = comando.ExecuteReader ();
											if(lector.Read()){
												NpgsqlConnection conexion5;
												conexion5 = new NpgsqlConnection (connectionString+nombrebd);
												try{
													conexion5.Open ();
													NpgsqlCommand comando5; 
													comando5 = conexion5.CreateCommand();
													comando5.CommandText = "UPDATE osiris_catalogo_almacenes SET stock = stock - '"+(string)filter.Model.GetValue(iterSelected,1)+"' "+
														"WHERE id_almacen = '"+this.idsubalmacen.ToString()+"' "+
														"AND eliminado = 'false'" +
														"AND id_producto = '"+(string) filter.Model.GetValue(iterSelected,3)+"' ;";
														
													comando5.ExecuteNonQuery();
													comando5.Dispose();
													conexion5.Close();
													
													NpgsqlConnection conexion1; 
													conexion1 = new NpgsqlConnection (connectionString+nombrebd);
													try{
														conexion1.Open ();
														NpgsqlCommand comando1;
														comando1 = conexion1.CreateCommand();
														comando1.CommandText = "UPDATE osiris_catalogo_almacenes SET stock = stock + '"+(string) filter.Model.GetValue(iterSelected,1)+"' "+
																"WHERE id_almacen = '"+this.idalmacendestino.ToString()+"' "+
																"AND eliminado = 'false'" +
																"AND id_producto = '"+(string) filter.Model.GetValue(iterSelected,3)+"' ;";
																	
														comando1.ExecuteNonQuery();
														comando1.Dispose();
														conexion1.Close();
																
														NpgsqlConnection conexion4; 
														conexion4 = new NpgsqlConnection (connectionString+nombrebd);
														try{
															conexion4.Open ();
															NpgsqlCommand comando4; 
															comando4 = conexion4.CreateCommand ();
															comando4.CommandText = "INSERT INTO osiris_his_solicitudes_deta("+
																				"folio_de_solicitud,"+
																				"id_producto,"+
																				"cantidad_autorizada,"+
																				"fechahora_traspaso,"+
																				"id_quien_traspaso,"+
																				"id_almacen_origen,"+
																				"id_almacen,"+
																				"costo_por_unidad,"+
																				"cantidad_solicitada,"+
																				"precio_producto_publico,"+
																				"numero_de_traspaso,"+
																				"traspaso ) "+
																				"VALUES ("+
																				"0,'"+
																				(string) filter.Model.GetValue(iterSelected,3)+"','"+//+" ,'"+
																				(string) filter.Model.GetValue(iterSelected,1)+"','"+
																				DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"','"+
																				LoginEmpleado+"','"+
																				idsubalmacen.ToString()+"','"+
																				idalmacendestino.ToString()+"' ,'"+
																				(string) filter.Model.GetValue(iterSelected,5)+"','"+
																				(string) filter.Model.GetValue(iterSelected,1)+"','"+
																				(string) filter.Model.GetValue(iterSelected,6)+"','"+
																	//(string) entry_numero_de_traspaso.Text.ToUpper()+"','"+	
																				"true');";
																comando4.ExecuteNonQuery();
																comando4.Dispose();															
															}catch (NpgsqlException ex){
																MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
																                                               MessageType.Error,ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
																msgBoxError.Run ();				msgBoxError.Destroy();
															}
															conexion4.Close();
														}catch (NpgsqlException ex){
															MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
															                                               MessageType.Error, 
															                                               ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
															msgBoxError.Run ();
														}
														conexion1.Close();
													}catch (NpgsqlException ex){
														MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
												                                               MessageType.Error, 
												                                              ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
														msgBoxError.Run ();
													}
													conexion.Close();
												}else{
													NpgsqlConnection conexion1; 
													conexion1 = new NpgsqlConnection (connectionString+nombrebd);
							 						try{
														conexion1.Open ();
														NpgsqlCommand comando1; 
														comando1 = conexion1.CreateCommand();
														comando1.CommandText = "INSERT INTO osiris_catalogo_almacenes("+
																					"id_almacen,"+
																					"id_producto,"+
																					"stock,"+
																					"id_quien_creo,"+
																					"fechahora_alta,"+
																					"fechahora_ultimo_surtimiento)"+
																					"VALUES ('"+
																					this.idalmacendestino.ToString()+"','"+
																					(string) filter.Model.GetValue (iterSelected,3)+"','"+
																					(string) filter.Model.GetValue (iterSelected,1)+"','"+
																					this.LoginEmpleado+"','"+
																					DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"','"+
																					DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"');";
														comando1.ExecuteNonQuery();
														comando1.Dispose();
													}catch (NpgsqlException ex){
											   			MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
																		MessageType.Error, 
																		ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
														msgBoxError.Run ();
														msgBoxError.Destroy();
													}
													conexion1.Close();
													NpgsqlConnection conexion6; 
													conexion6 = new NpgsqlConnection (connectionString+nombrebd);
													try{
														conexion6.Open ();
														NpgsqlCommand comando1;
														comando1 = conexion6.CreateCommand();
														comando1.CommandText = "UPDATE osiris_catalogo_almacenes SET stock = '"+(string) filter.Model.GetValue(iterSelected,1)+"' "+
																		"WHERE id_almacen = '"+idalmacendestino.ToString()+"' "+
																		"AND eliminado = 'false'" +
																		"AND id_producto = '"+(string) filter.Model.GetValue(iterSelected,3)+"' ;";
																				
														comando1.ExecuteNonQuery();
														comando1.Dispose();
														conexion6.Close();
																
														NpgsqlConnection conexion7;
														conexion7 = new NpgsqlConnection (connectionString+nombrebd);
													try{
															conexion7.Open ();
														NpgsqlCommand comando7; 
														comando7 = conexion7.CreateCommand();
														comando7.CommandText = "UPDATE osiris_catalogo_almacenes SET stock = stock - '"+(string) filter.Model.GetValue(iterSelected,1)+"' "+
																		"WHERE id_almacen = '"+this.idsubalmacen.ToString()+"' "+
																		"AND eliminado = 'false'" +
																		"AND id_producto = '"+(string) filter.Model.GetValue(iterSelected,3)+"' ;";
														comando7.ExecuteNonQuery();
														comando7.Dispose();
															conexion7.Close();
																	
															NpgsqlConnection conexion4; 
															conexion4 = new NpgsqlConnection (connectionString+nombrebd);
															
															try{
																conexion4.Open ();
																NpgsqlCommand comando4; 
																comando4 = conexion4.CreateCommand ();
																comando4.CommandText = "INSERT INTO osiris_his_solicitudes_deta("+
																						"folio_de_solicitud,"+
																						"id_producto,"+
																						"cantidad_autorizada,"+
																						"fechahora_traspaso,"+
																						"id_quien_traspaso,"+
																						"id_almacen_origen,"+
																						"id_almacen,"+
																						"costo_por_unidad,"+
																						"cantidad_solicitada,"+
																						"precio_producto_publico,"+
																						"numero_de_traspaso,"+
																						"traspaso ) "+
																						"VALUES ("+
																						"0,'"+
																						(string) filter.Model.GetValue(iterSelected,3)+"','"+//+" ,'"+
																						(string) filter.Model.GetValue(iterSelected,1)+"','"+
																						DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"','"+
																						LoginEmpleado+"','"+
																						idsubalmacen.ToString()+"','"+
																						idalmacendestino.ToString()+"' ,'"+
																						(string) filter.Model.GetValue(iterSelected,5)+"','"+
																						(string) filter.Model.GetValue(iterSelected,1)+"','"+
																						(string) filter.Model.GetValue(iterSelected,6)+"','"+
																//(string) entry_numero_de_traspaso.Text.ToUpper()+"','"+	
																						"true');";
																comando4.ExecuteNonQuery();
																comando4.Dispose();
															}catch (NpgsqlException ex){
																MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
																		                                               MessageType.Error,ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
																msgBoxError.Run ();				msgBoxError.Destroy();
															}
															conexion4.Close();
														}catch (NpgsqlException ex){
													   		MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
																				MessageType.Error, 
																				ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
															msgBoxError.Run ();
															msgBoxError.Destroy();
														}
													}catch (NpgsqlException ex){
														MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
																				MessageType.Error, 
																				ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
														msgBoxError.Run ();					msgBoxError.Destroy();
													}
												}
										}catch (NpgsqlException ex){
												MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
												                                               MessageType.Error, 
												                                               ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
												msgBoxError.Run (); msgBoxError.Destroy();
										}
									}
								}	
							}
							while (treeViewEngineBusca2.IterNext(ref iterSelected)){
									if ((bool) filter.Model.GetValue (iterSelected,0) == true){
										if (decimal.Parse((string) filter.Model.GetValue (iterSelected,2)) > 0 ){
											NpgsqlConnection conexion;
											conexion = new NpgsqlConnection (connectionString+nombrebd);
											try{
												conexion.Open ();
												NpgsqlCommand comando; 
												comando = conexion.CreateCommand();
												comando.CommandText = "SELECT id_producto,id_almacen,stock FROM osiris_catalogo_almacenes "+
														"WHERE id_almacen = '"+idalmacendestino.ToString()+"' "+
														"AND eliminado = 'false' "+														
														"AND id_producto = '"+(string) filter.Model.GetValue(iterSelected,3)+"' ;";
									
												NpgsqlDataReader lector = comando.ExecuteReader ();
												if(lector.Read()){
													NpgsqlConnection conexion5;
													conexion5 = new NpgsqlConnection (connectionString+nombrebd);
													try{
														conexion5.Open ();
														NpgsqlCommand comando5; 
														comando5 = conexion5.CreateCommand();
														comando5.CommandText = "UPDATE osiris_catalogo_almacenes SET stock = stock - '"+(string) filter.Model.GetValue(iterSelected,1)+"' "+
																"WHERE id_almacen = '"+this.idsubalmacen.ToString()+"' "+
															"AND eliminado = 'false'" +
															"AND id_producto = '"+(string) filter.Model.GetValue(iterSelected,3)+"' ;";
													
														comando5.ExecuteNonQuery(); 	comando5.Dispose();
														conexion5.Close();
												
														NpgsqlConnection conexion1; 
														conexion1 = new NpgsqlConnection (connectionString+nombrebd);
														try{
															conexion1.Open ();
															NpgsqlCommand comando1;
															comando1 = conexion1.CreateCommand();
															comando1.CommandText = "UPDATE osiris_catalogo_almacenes SET stock = stock + '"+(string) filter.Model.GetValue(iterSelected,1)+"' "+
																	"WHERE id_almacen = '"+idalmacendestino.ToString()+"' "+
																	"AND eliminado = 'false'" +
																"AND id_producto = '"+(string) filter.Model.GetValue(iterSelected,3)+"' ;";
																
															comando1.ExecuteNonQuery();
															comando1.Dispose();
															conexion1.Close();
															
															NpgsqlConnection conexion4; 
															conexion4 = new NpgsqlConnection (connectionString+nombrebd);
															try{
																conexion4.Open ();
																NpgsqlCommand comando4; 
																comando4 = conexion4.CreateCommand ();
																comando4.CommandText = "INSERT INTO osiris_his_solicitudes_deta("+
																			"folio_de_solicitud,"+
																			"id_producto,"+
																			"cantidad_autorizada,"+
																			"fechahora_traspaso,"+
																			"id_quien_traspaso,"+
																			"id_almacen_origen,"+
																			"id_almacen,"+
																			"costo_por_unidad,"+
																			"cantidad_solicitada,"+
																			"precio_producto_publico,"+
																			"numero_de_traspaso,"+
																			"traspaso ) "+
																			"VALUES ("+
																			"0,'"+
																			(string) filter.Model.GetValue(iterSelected,3)+"','"+//+" ,'"+
																			(string) filter.Model.GetValue(iterSelected,1)+"','"+
																			DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"','"+
																			LoginEmpleado+"','"+
																			idsubalmacen.ToString()+"','"+
																			idalmacendestino.ToString()+"' ,'"+
																			(string) filter.Model.GetValue(iterSelected,5)+"','"+
																			(string) filter.Model.GetValue(iterSelected,1)+"','"+
																			(string) filter.Model.GetValue(iterSelected,6)+"','"+
														//(string) this.entry_numero_de_traspaso.Text.ToUpper()+"','"+	
																			"true');";
																comando4.ExecuteNonQuery();
																comando4.Dispose();															
															}catch (NpgsqlException ex){
																MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
															                                               MessageType.Error,ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
																msgBoxError.Run ();				msgBoxError.Destroy();
															}
															conexion4.Close();
														}catch (NpgsqlException ex){
															MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
														                                               MessageType.Error, 
														                                               ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
															msgBoxError.Run (); msgBoxError.Destroy();
														}
														conexion1.Close();
													}catch (NpgsqlException ex){
														MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
											                                               MessageType.Error, 
											                                              ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
													msgBoxError.Run (); msgBoxError.Destroy();
													}
													conexion.Close();
												}else{
													NpgsqlConnection conexion1; 
													conexion1 = new NpgsqlConnection (connectionString+nombrebd);
						 							try{
														conexion1.Open ();
														NpgsqlCommand comando1; 
														comando1 = conexion1.CreateCommand();
														comando1.CommandText = "INSERT INTO osiris_catalogo_almacenes("+
																				"id_almacen,"+
																				"id_producto,"+
																				"stock,"+
																				"id_quien_creo,"+
																				"fechahora_alta,"+
																				"fechahora_ultimo_surtimiento)"+
																				"VALUES ('"+
																				idalmacendestino.ToString()+"','"+
																				(string) filter.Model.GetValue (iterSelected,3)+"','"+
																				(string) filter.Model.GetValue (iterSelected,1)+"','"+
																				this.LoginEmpleado+"','"+
																				DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"','"+
																				DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"');";
														comando1.ExecuteNonQuery();					comando1.Dispose();
													}catch (NpgsqlException ex){
										   				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
																	MessageType.Error, 
																	ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
														msgBoxError.Run ();					msgBoxError.Destroy();
													}
													conexion1.Close();
													NpgsqlConnection conexion6; 
													conexion6 = new NpgsqlConnection (connectionString+nombrebd);
													try{
														conexion6.Open ();
														NpgsqlCommand comando1;
														comando1 = conexion6.CreateCommand();
														comando1.CommandText = "UPDATE osiris_catalogo_almacenes SET stock = '"+(string) filter.Model.GetValue(iterSelected,1)+"' "+
																	"WHERE id_almacen = '"+idalmacendestino.ToString()+"' "+
																	"AND eliminado = 'false'" +
																	"AND id_producto = '"+(string) filter.Model.GetValue(iterSelected,3)+"' ;";
																			
														comando1.ExecuteNonQuery();
														comando1.Dispose();
														conexion6.Close();
															
														NpgsqlConnection conexion7;
														conexion7 = new NpgsqlConnection (connectionString+nombrebd);
														try{
															conexion7.Open ();
															NpgsqlCommand comando7; 
															comando7 = conexion7.CreateCommand();
															comando7.CommandText = "UPDATE osiris_catalogo_almacenes SET stock = stock - '"+(string) filter.Model.GetValue(iterSelected,1)+"' "+
																	"WHERE id_almacen = '"+this.idsubalmacen.ToString()+"' "+
																	"AND eliminado = 'false'" +
																	"AND id_producto = '"+(string) filter.Model.GetValue(iterSelected,3)+"' ;";
															comando7.ExecuteNonQuery();
															comando7.Dispose();
															conexion7.Close();
															
															NpgsqlConnection conexion4; 
															conexion4 = new NpgsqlConnection (connectionString+nombrebd);
															try{
																conexion4.Open ();
																NpgsqlCommand comando4; 
																comando4 = conexion4.CreateCommand ();
																comando4.CommandText = "INSERT INTO osiris_his_solicitudes_deta("+
																					"folio_de_solicitud,"+
																					"id_producto,"+
																					"cantidad_autorizada,"+
																					"fechahora_traspaso,"+
																					"id_quien_traspaso,"+
																					"id_almacen_origen,"+
																					"id_almacen,"+
																					"costo_por_unidad,"+
																					"cantidad_solicitada,"+
																					"precio_producto_publico,"+
																					"numero_de_traspaso,"+
																					"traspaso ) "+
																					"VALUES ("+
																					"0,'"+
																					(string) filter.Model.GetValue(iterSelected,3)+"','"+//+" ,'"+
																					(string) filter.Model.GetValue(iterSelected,1)+"','"+
																					DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"','"+
																					LoginEmpleado+"','"+
																					idsubalmacen.ToString()+"','"+
																					idalmacendestino.ToString()+"' ,'"+
																					(string) filter.Model.GetValue(iterSelected,5)+"','"+
																					(string) filter.Model.GetValue(iterSelected,1)+"','"+
																					(string) filter.Model.GetValue(iterSelected,6)+"','"+
																//(string) this.entry_numero_de_traspaso.Text.ToUpper()+"','"+	
																					"true');";
																comando4.ExecuteNonQuery();
																comando4.Dispose();
															}catch (NpgsqlException ex){
																MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
																	                                               MessageType.Error,ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
																msgBoxError.Run ();				msgBoxError.Destroy();
															}
															conexion4.Close();
														}catch (NpgsqlException ex){
												   			MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
																			MessageType.Error, 
																			ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
															msgBoxError.Run ();
															msgBoxError.Destroy();
														}
													}catch (NpgsqlException ex){
														MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
																			MessageType.Error, 
																			ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
														msgBoxError.Run ();
														msgBoxError.Destroy();
													}
													conexion6.Close();
												}
											}catch (NpgsqlException ex){
												MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
											                                               MessageType.Error, 
											                                               ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
												msgBoxError.Run ();                                               
										}
									}
								}
							}
						}
						llenando_busqueda_productos();								
					//}else{
					//	MessageDialog msgBox = new MessageDialog (MyWin,DialogFlags.Modal,
					//				MessageType.Info,ButtonsType.Ok,"El Almacen General no puede realizar traspasos, solo envios directos y surtir mediante solicitud de sub-almacenes");
					//	msgBox.Run ();msgBox.Destroy();
					//}
				}else{
					MessageDialog msgBox = new MessageDialog (MyWin,DialogFlags.Modal,
						MessageType.Info,ButtonsType.Ok,"No tiene Permiso para Realizar Traspasos entre Almacenes y Sub-Almacenes");
					msgBox.Run ();msgBox.Destroy();
				}
			}
			// Ajustes maximo minimo y reorden
			if (tipoherramienta == 3){				
				if((string) classpublic.lee_registro_de_tabla("osiris_empleado","acceso_modif_maxminreo","WHERE acceso_modif_maxminreo = 'true' AND login_empleado = '"+LoginEmpleado+"' ","acceso_modif_maxminreo","bool") == "True"){
					MessageDialog msgBox2 = new MessageDialog (MyWin,DialogFlags.Modal,
						MessageType.Question,ButtonsType.YesNo,"¿ Esta seguro de Realizar el Ajuste de Maximos, Mimimos y Reorden de los prpductos Seleccionados ?");
					ResponseType miResultado2 = (ResponseType)msgBox2.Run ();
					msgBox2.Destroy();					
					if (miResultado2 == ResponseType.Yes){
						entry_filter.Text = "";
						TreeIter iterSelected2;								
						if (treeViewEngineBusca2.GetIterFirst (out iterSelected2)) {
							if ((bool)filter.Model.GetValue (iterSelected2, 0) == true) {
								parametros = new string[,] {
									{ "historial_ajustes = historial_ajustes || 'AJUSTE MAX-MIN-REO;",LoginEmpleado.Trim () + ";" + DateTime.Now.ToString ("yyyy-MM-dd HH:mm:ss").Trim ()+
										filter.Model.GetValue (iterSelected2, 7).ToString ().Trim () + ";" +
										filter.Model.GetValue (iterSelected2, 8).ToString ().Trim () + ";" +
										filter.Model.GetValue (iterSelected2, 9).ToString ().Trim ()+"\n'," },
									{ "maximo = '",filter.Model.GetValue (iterSelected2, 8).ToString ().Trim ()+"'," },
									{ "minimo_stock = '",filter.Model.GetValue (iterSelected2, 7).ToString ().Trim ()+"'," },
									{ "punto_de_reorden = '",filter.Model.GetValue (iterSelected2, 9).ToString ().Trim ()+"' " },
									{ "WHERE id_almacen = '",idsubalmacen.ToString ()+"' "},
									{ "AND eliminado = '","false' "},
									{ "AND id_producto = '",filter.Model.GetValue (iterSelected2, 3).ToString ().Trim ()+"' "}
								};
								paraobj = new object[] { entry_filter};
								new osiris.update_registro ("osiris_catalogo_almacenes", parametros, paraobj);
							}
							while (treeViewEngineBusca2.IterNext (ref iterSelected2)) {
								if ((bool)filter.Model.GetValue (iterSelected2, 0) == true) {
									parametros = new string[,] {
										{ "historial_ajustes = historial_ajustes || 'AJUSTE MAX-MIN-REO;",LoginEmpleado.Trim () + ";" + DateTime.Now.ToString ("yyyy-MM-dd HH:mm:ss").Trim ()+
											filter.Model.GetValue (iterSelected2, 7).ToString ().Trim () + ";" +
											filter.Model.GetValue (iterSelected2, 8).ToString ().Trim () + ";" +
											filter.Model.GetValue (iterSelected2, 9).ToString ().Trim ()+"\n'," },
										{ "maximo = '",filter.Model.GetValue (iterSelected2, 8).ToString ().Trim ()+"'," },
										{ "minimo_stock = '",filter.Model.GetValue (iterSelected2, 7).ToString ().Trim ()+"'," },
										{ "punto_de_reorden = '",filter.Model.GetValue (iterSelected2, 9).ToString ().Trim ()+"' " },
										{ "WHERE id_almacen = '",idsubalmacen.ToString ()+"' "},
										{ "AND eliminado = '","false' "},
										{ "AND id_producto = '",filter.Model.GetValue (iterSelected2, 3).ToString ().Trim ()+"' "}
									};
									paraobj = new object[] { entry_filter};
									new osiris.update_registro ("osiris_catalogo_almacenes", parametros, paraobj);
								}
							}
						}
						llenando_busqueda_productos();							
					}
				}else{
					MessageDialog msgBox = new MessageDialog (MyWin,DialogFlags.Modal,
						MessageType.Info,ButtonsType.Ok,"No tiene Permiso para Hacer cambios en los minimos, maximos y reorden");
					msgBox.Run ();msgBox.Destroy();
				}
			}
		}	
		
		void imprime_reporte_traspaso(object sender, EventArgs args)
		{
			
		}
				
		void imprime_reporte_stock(object sender, EventArgs args)
		{
			new osiris.inventario_almacen_reporte (idsubalmacen,descripcionalmacen,"01","0000",
													LoginEmpleado,NomEmpleado,AppEmpleado,ApmEmpleado,nombrebd,"inventario_actual",
							query_sql+query_grupo+query_stock+" AND osiris_catalogo_almacenes.id_almacen = '"+idsubalmacen.ToString().Trim()+"' "+"ORDER BY osiris_productos.id_grupo_producto,osiris_productos.descripcion_producto;","","","","","","");
		}

		// Valida entradas que solo sean numericas, se utiliza eb ventana de
		// carga de producto
		[GLib.ConnectBefore ()]   	  // Esto es indispensable para que funcione    
      	public void onKeyPressEvent(object o, Gtk.KeyPressEventArgs args)
		{
			//Console.WriteLine(args.Event.Key);
			//Console.WriteLine(Convert.ToChar(args.Event.Key));
			string misDigitos = ".0123456789ﾰﾱﾲﾳﾴﾵﾶﾷﾸﾹﾮｔｒｓｑ（）";
			if (Array.IndexOf(misDigitos.ToCharArray(), Convert.ToChar(args.Event.Key)) == -1 && args.Event.Key.ToString()  != "BackSpace"){
				args.RetVal = true;
			}
		}

		void on_checkbutton_herramientas_clicked(object sender, EventArgs args)
		{
			combobox_herramientas.Sensitive = checkbutton_herramientas.Active;
		}
		
		// Valida entradas que solo sean numericas, se utiliza eb ventana de
		// carga de producto
		[GLib.ConnectBefore ()]   	  // Esto es indispensable para que funcione
		public void onKeyPressEvent_numero_traspaso(object o, Gtk.KeyPressEventArgs args)
		{ 
			if (args.Event.Key.ToString() == "Return" || args.Event.Key.ToString() == "KP_Enter"){
				args.RetVal = true;
			}
			string misDigitos = ".0123456789ﾰﾱﾲﾳﾴﾵﾶﾷﾸﾹﾮｔｒｓｑ（）";
			if (Array.IndexOf(misDigitos.ToCharArray(), Convert.ToChar(args.Event.Key)) == -1 && args.Event.Key.ToString()  != "BackSpace"){
				args.RetVal = true;
			}
		}
		
		void on_cierraventanas_clicked (object sender, EventArgs args)
		{
			Widget win = (Widget) sender;
			win.Toplevel.Destroy();
		}
	}	
}