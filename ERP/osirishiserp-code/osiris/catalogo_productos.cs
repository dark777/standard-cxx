///////////////////////////////////////////////////////////
// created on 27/12/2007 at 05:14 p
// Sistema Hospitalario OSIRIS
// Monterrey - Mexico
//
// Autor    	: Ing. Juan Antonio Peña (Programacion)
//                Ing. Hector vargas (Diseño Glade)
//				  Ing. Daniel Olivares (Programacion Ajustes Varios)
//                Tec. Homero Montoya (Ajustes Varios)
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
using Npgsql;
using Gtk;
using System.Data;
using Glade;
using Gdk;
using System.Collections;

namespace osiris
{
	public class catalogo_productos_nuevos
	{
		// //Declarando ventana de Productos Homero Prueba "Ventana Principal"
		[Widget] Gtk.Window catalogo_productos;
		[Widget] Gtk.CheckButton checkbutton_nuevo_producto;
		[Widget] Gtk.Entry entry_codigo_producto;
		[Widget] Gtk.Button button_selec_prod = null;
		[Widget] Gtk.CheckButton checkbutton_producto_anidado;
		[Widget] Gtk.CheckButton checkbutton_costounico;
		[Widget] Gtk.ComboBox combobox_grupo;
		[Widget] Gtk.ComboBox combobox_grupo1;
		[Widget] Gtk.ComboBox combobox_grupo2;
		[Widget] Gtk.Entry entry_descripcion;
		[Widget] Gtk.Entry entry_nombre_articulo;
		[Widget] Gtk.Entry entry_nombre_generico;
		[Widget] Gtk.ComboBox combobox_tipo_unidad;
		[Widget] Gtk.Entry entry_costo;
		[Widget] Gtk.Entry entry_embalaje;
		[Widget] Gtk.CheckButton checkbutton_apl_iva;
		[Widget] Gtk.CheckButton checkbutton_prod_activo;
		[Widget] Gtk.CheckButton checkbutton_cambia_utilidad;
		[Widget] Gtk.Entry entry_precio_unitario;
		[Widget] Gtk.Entry entry_porciento_utilidad;
		[Widget] Gtk.Entry entry_utilidad;
		[Widget] Gtk.Entry entry_iva;
		[Widget] Gtk.CheckButton checkbutton_descuento;
		[Widget] Gtk.Entry entry_descuento_en_porciento;
		[Widget] Gtk.Entry entry_descuento_en_pesos;
		[Widget] Gtk.Entry entry_precio_publico;
		[Widget] Gtk.Entry precio_sin_iva;
		[Widget] Gtk.TreeView treeview_precios_convenios;
		[Widget] Gtk.TreeView treeview_productos_anidados;
		[Widget] Gtk.Entry entry_producto;
		[Widget] Gtk.Entry entry_precio_venta = null;
		[Widget] Gtk.Entry entry_porcentage_iva = null;
		[Widget] Gtk.Button button_guarda_preconvenios = null;
		
		//Declarando la barra de estado
		[Widget] Gtk.Statusbar statusbar_catalogo_productos;
		
		//Declarando botones
		[Widget] Gtk.Button button_guardar;
		[Widget] Gtk.ToggleButton button_editar;
		[Widget] Gtk.Button button_calcular;
		[Widget] Gtk.Button button_salir;
		//[Widget] Gtk.Button button_seleccionar;
		[Widget] Gtk.Button button_busca_producto;
		[Widget] Gtk.Button button_busca_producto_anidado;
		[Widget] Gtk.Button button_quitar;
		
		//Declaracion de ventana de busqueda de productos
		[Widget] Gtk.Window busca_producto;
		[Widget] Gtk.RadioButton radiobutton_nombre;
		[Widget] Gtk.RadioButton radiobutton_codigo;
		[Widget] Gtk.TreeView lista_de_producto;
		[Widget] Gtk.TreeView busqueda;
		[Widget] Gtk.Label label_cantidad;
		[Widget] Gtk.Entry entry_cantidad_aplicada;
		[Widget] Gtk.HBox hbox3 = null;
		[Widget] Gtk.Label label_pack = null;
		[Widget] Gtk.Entry entry_embalaje_pack = null;
		[Widget] Gtk.Label label_desc_proveedor = null;
		[Widget] Gtk.Entry entry_producto_proveedor = null;
		[Widget] Gtk.Label label_codprod_proveedor = null;
		[Widget] Gtk.Entry entry_codprod_proveedor = null;
		[Widget] Gtk.Label label390 = null;
 		[Widget] Gtk.ComboBox combobox_tipo_unidad2 = null;

		// tab codigo de barras
		[Widget] Gtk.Entry entry_codigo_barras = null;
		[Widget] Gtk.Entry entry_idmarca = null;
		[Widget] Gtk.Entry entry_marcaproducto = null;
		[Widget] Gtk.Button button_busca_marcaprod = null;
		[Widget] Gtk.Button button_agrega_marca = null;
		[Widget] Gtk.Button button_graba_codigobarra = null;
		[Widget] Gtk.Button button_quitar_codigobarra = null;
		[Widget] Gtk.TreeView treeview_lista_codbarra = null;
		[Widget] Gtk.Button button_export_codbarra = null;
		
		// Lista de Precio
		[Widget] Gtk.Button button_export = null;
		[Widget] Gtk.CheckButton checkbutton_grupo_export = null;
		[Widget] Gtk.ComboBox combobox_grupo_export = null;
		[Widget] Gtk.CheckButton checkbutton_grupo1_export = null;
		[Widget] Gtk.ComboBox combobox_grupo1_export = null;
		[Widget] Gtk.CheckButton checkbutton_grupo2_export = null;
		[Widget] Gtk.ComboBox combobox_grupo2_export = null;		
		
		// Para todas las busquedas este es el nombre asignado  se declara una vez
		[Widget] Gtk.Entry entry_expresion;
		[Widget] Gtk.Button button_selecciona;
		[Widget] Gtk.Button button_buscar_busqueda;
		[Widget] Gtk.TreeView lista_de_producto_anidado;
		
		// Declaracion de variables publicas
		long idproduct = 0;
		long lastproduct = 0;
		long newidproduct = 0;
		long newidsecuencia = 0;
		int idtipogrupo = 0;
		int idtipogrupo1 = 0;
		int idtipogrupo2 = 0;
		int idtipogrupo_export = 0;
		int idtipogrupo1_export = 0;
		int idtipogrupo2_export = 0;		
		string descripgrupo = "";
		string descripgrupo1 =  "";
		string descripgrupo2 = "";
		string apldesc;
		bool aplicariva_producto;
	 	bool cobroactivo_producto;
	 	string costounico;
	 	string tiposeleccion = "";
	 	decimal descuento = 0;
	 	decimal precio_uni = 0;
	 	bool aplica_descuento;
	 	string tipounidadproducto = "";
				
		// Almacena los valores anterios para guardar los cuando actualiza algun precio, o descripcion
	 	decimal precio_unitario_anterior = 0;
	 	decimal precio_costo_anterior = 0;
		decimal utilidad_anterior = 0;
		string valor_anterior_descuento;
		
		//VARIABLES PARA CARGAR DATOS
	 	string codigoproducto ="";
	 			
		//Variables Principales
		string connectionString;		
		string nombrebd;
		string LoginEmpleado;
		string NomEmpleado;
		string AppEmpleado;
		string ApmEmpleado;
		string id_producto;
		
		TreeStore treeViewEngineBusca2;
		TreeStore treeViewEngineBusca3;
		TreeStore treeViewEnginePreciosConvenios;
		TreeStore treeViewEngineCodigobarra;

		Gtk.TreeView treeviewobject = null;
		Gtk.TreeStore treeViewEngine = null;
		ArrayList columns = new ArrayList ();

		
		//declaracion de columnas y celdas de treeview de los precios de Clientes 
		TreeViewColumn col_seleccion; 		CellRendererToggle cellrt_seleccion;
		TreeViewColumn col_clie_aseg;		CellRendererText cellr30;
		TreeViewColumn col_utilidad;		CellRendererText cellr31;
		TreeViewColumn col_precio_sin_iva;	CellRendererText cellrt32;
		TreeViewColumn col_porc_utilidad;	CellRendererText cellrt33;
		TreeViewColumn col_iva;				CellRendererText cellrt34;
		TreeViewColumn col_precio_final;	CellRendererText cellrt35;
		TreeViewColumn col_key;				CellRendererText cellrt36;
		TreeViewColumn col_precioref;		CellRendererText cellrt37;
		
		//declaracion de columnas y celdas de treeview de busqueda
		TreeViewColumn col_idproducto;		CellRendererText cellr0;
		TreeViewColumn col_desc_producto;	CellRendererText cellr1;
		TreeViewColumn col_precioprod;		CellRendererText cellrt2;
		TreeViewColumn col_ivaprod;			CellRendererText cellrt3;
		TreeViewColumn col_totalprod;		CellRendererText cellrt4;
		TreeViewColumn col_descuentoprod;	CellRendererText cellrt5;
		TreeViewColumn col_preciocondesc;	CellRendererText cellrt6;
		TreeViewColumn col_grupoprod;		CellRendererText cellrt7;
		TreeViewColumn col_grupo1prod;		CellRendererText cellrt8;
		TreeViewColumn col_grupo2prod;		CellRendererText cellrt9;
		TreeViewColumn col_costoprod_uni;	CellRendererText cellrt12;
		TreeViewColumn col_aplica_iva;		CellRendererText cellrt19;
		TreeViewColumn col_cobro_activo;		CellRendererText cellrt20;
		
		//Declaracion de ventana de error
		protected Gtk.Window MyWinError;
		protected Gtk.Window MyWin;


		string[] args_args = {""};
		string[] args_tipounidad = {"","PIEZA","KILO","LITRO","GRAMO","METRO","CENTIMETRO","CAJA","PULGADA","FRASCO","GALON","BOLSA","PULGADA"};
		int[] args_id_array = {0,1,2,3,4,5,6,7,8,9,10,11,12,13,14,15};
		
		class_conexion conexion_a_DB = new class_conexion();
		class_public classpublic = new class_public();
		class_buscador classfind_data = new class_buscador();
						
		public catalogo_productos_nuevos (string LoginEmp, string NomEmpleado_, string AppEmpleado, string ApmEmpleado, string nombrebd_ )
		{
			LoginEmpleado = LoginEmp;
			NomEmpleado = NomEmpleado_;
			connectionString = conexion_a_DB._url_servidor+conexion_a_DB._port_DB+conexion_a_DB._usuario_DB+conexion_a_DB._passwrd_user_DB;
			nombrebd = conexion_a_DB._nombrebd;
						
			//Direcciona al glade
			Glade.XML gxml = new Glade.XML (null, "almacen_costos_compras.glade", "catalogo_productos", null);
			gxml.Autoconnect (this);
			// Muestra ventana de Glade
			catalogo_productos.Show();
        	
			// Sale de la ventana
			button_salir.Clicked += new EventHandler(on_cierraventanas_clicked);
			button_guardar.Clicked += new EventHandler(on_guarda_producto_clicked);
			button_editar.Clicked +=  new EventHandler(on_editar_productos_clicked);
			button_busca_producto.Clicked += new EventHandler(on_button_busca_producto_clicked);
			button_calcular.Clicked += new EventHandler(on_button_calcula_utilidad_clicked);
			checkbutton_nuevo_producto.Clicked += new EventHandler(on_checkbutton_nuevo_producto_clicked);
			checkbutton_apl_iva.Clicked += new EventHandler(on_checkbutton_apl_iva_clicked);
			checkbutton_descuento.Clicked += new EventHandler(on_checkbutton_descuento_clicked);
			checkbutton_cambia_utilidad.Clicked += new EventHandler(on_checkbutton_cambia_utilidad_clicked);
			checkbutton_producto_anidado.Clicked += new EventHandler(on_checkbutton_producto_anidado_cliked);
			checkbutton_apl_iva.Clicked += new EventHandler(on_checkbutton_apl_iva_cliked);
			button_busca_producto_anidado.Clicked += new EventHandler(on_button_busca_producto_anidado_clicked);
			button_guarda_preconvenios.Clicked += new EventHandler(on_button_guarda_preconvenios_clicked);
			button_export.Clicked += new EventHandler(on_button_export_clicked);
			entry_codigo_barras.KeyPressEvent += onKeyPressEvent_codbarra;
			entry_costo.KeyPressEvent += onKeyPressEvent_numeric;
			entry_embalaje.KeyPressEvent += onKeyPressEvent_numeric;
			entry_precio_venta.KeyPressEvent += onKeyPressEvent_numeric;
			entry_porciento_utilidad.KeyPressEvent += onKeyPressEvent_numeric;
			entry_codigo_producto.KeyPressEvent += onKeyPressEvent_codprod;
			button_selec_prod.Clicked += new EventHandler(on_button_selec_prod_clicked);
			button_busca_marcaprod.Clicked += new EventHandler(on_button_busca_marcaprod_clicked);
			button_agrega_marca.Clicked += new EventHandler(on_button_agrega_marca_clicked);
			button_graba_codigobarra.Clicked += new EventHandler(on_button_graba_codigobarra_clicked);
			button_quitar_codigobarra.Clicked += new EventHandler(on_button_quitar_codigobarra_clicked);
			button_export_codbarra.Clicked += new EventHandler(on_button_export_codbarra_clicked);
            			
			//button_seleccionar.Clicked += new EventHandler(on_selecciona_producto_clicked);
			//Desactiva y activa los Botones
			button_guardar.Sensitive = false;
			button_editar.Sensitive = false;
			button_calcular.Sensitive = false;
			button_busca_producto_anidado.Sensitive = false;
			button_quitar.Sensitive = false;
			
			//Desactiva y activa los entrys, checkbuttons, radiobuttons
			checkbutton_producto_anidado.Sensitive = false;
			checkbutton_costounico.Sensitive = false;
			combobox_grupo.Sensitive = false;
			combobox_grupo1.Sensitive = false;
			combobox_grupo2.Sensitive = false;
			entry_descripcion.Sensitive = false;
			entry_nombre_articulo.Sensitive = false;
			entry_nombre_generico.Sensitive = false;
			combobox_tipo_unidad.Sensitive = false;
			entry_costo.Sensitive = false;
			entry_embalaje.Sensitive = false;
			checkbutton_apl_iva.Sensitive = false;
			checkbutton_prod_activo.Sensitive = false;
			checkbutton_cambia_utilidad.Sensitive = false;
			entry_precio_unitario.Sensitive = false;
			entry_porciento_utilidad.Sensitive = false;
			entry_utilidad.Sensitive = false;
			entry_iva.Sensitive = false;
			checkbutton_descuento.Sensitive = false;
			entry_descuento_en_porciento.Sensitive = false;
			entry_descuento_en_pesos.Sensitive = false;
			entry_precio_publico.Sensitive = false;
			precio_sin_iva.Sensitive = false;
			crea_treeview_precios_convenios();
			treeview_productos_anidados.Sensitive = false;
			entry_precio_venta.Sensitive = false;
			entry_porcentage_iva.Sensitive = false;
			entry_codigo_barras.Sensitive = false;
			entry_codigo_producto.IsEditable = true;
			entry_idmarca.Sensitive = false;
			entry_marcaproducto.Sensitive = false;
			button_busca_marcaprod.Sensitive = false;
			
			llenado_combobox(0,"",combobox_grupo_export,"sql","SELECT * FROM osiris_grupo_producto ORDER BY descripcion_grupo_producto;","descripcion_grupo_producto","id_grupo_producto",args_args,args_id_array);
			llenado_combobox(0,"",combobox_grupo1_export,"sql","SELECT * FROM osiris_grupo1_producto ORDER BY descripcion_grupo1_producto;","descripcion_grupo1_producto","id_grupo1_producto",args_args,args_id_array);
			llenado_combobox(0,"",combobox_grupo2_export,"sql","SELECT * FROM osiris_grupo2_producto ORDER BY descripcion_grupo2_producto;","descripcion_grupo2_producto","id_grupo2_producto",args_args,args_id_array);

			entry_costo.ModifyBase(StateType.Normal, new Gdk.Color(255,243,169)); // Color Amarillo
			entry_embalaje.ModifyBase(StateType.Normal, new Gdk.Color(255,243,169)); // Color Amarillo
			entry_precio_venta.ModifyBase(StateType.Normal, new Gdk.Color(255,243,169)); // Color Amarillo

			crea_treeview_codbarra ();

			//object [] parametros = {treeview_lista_codbarra,treeViewEngineCodigobarra};
			//string[,] coltreeview = { { "Codigo Barra", "text" }, { "Nombre Comercial", "text" } };
			//crea_colums_treeview (parametros, coltreeview);

			//new osiris.crea_colums_treeview(parametros, coltreeview);

			// crea un columna en la tabla de productos
			//comando.CommandText = "ALTER TABLE osiris_productos ADD COLUMN precio_producto_<tipo de px><empr aseg etc> numeric(13,5) SET DEFAULT 0.00;";
			//Console.WriteLine(comando.CommandText);
			//comando.ExecuteNonQuery();    	    	       	comando.Dispose();
			
			// crea un columna tipo bool
			//comando.CommandText = "ALTER TABLE osiris_his_tipo_admisiones ADD COLUMN pase_servicio_medico bool SET DEFAULT false;";
			//Console.WriteLine(comando.CommandText);
			//comando.ExecuteNonQuery();    	    	       	comando.Dispose();
			
			// crea una columna tipo integer
			//ALTER TABLE the_table ADD COLUMN col_name TYPE integer;
			
			// crea un campo tipo text
			// ALTER TABLE osiris_erp_cobros_enca ADD COLUMN logs_cambios text DEFAULT ''::bpchar;
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
					case "combobox_tipo_unidad":
						tipounidadproducto = (string) onComboBoxChanged.Model.GetValue(iter,0);
						break;
					case "combobox_grupo":
						idtipogrupo = (int) combobox_grupo.Model.GetValue(iter,1);
			    		descripgrupo = (string) combobox_grupo.Model.GetValue(iter,0);
			    		if((bool) this.checkbutton_nuevo_producto.Active == true){
			    			ultimo_id_prod(idtipogrupo,idtipogrupo1,idtipogrupo2);
			    		}
						break;
					case "combobox_grupo1":
						idtipogrupo1 = (int) combobox_grupo1.Model.GetValue(iter,1);
			    		descripgrupo1 = (string) combobox_grupo1.Model.GetValue(iter,0);
			    		if((bool) this.checkbutton_nuevo_producto.Active == true){
			    			ultimo_id_prod(idtipogrupo,idtipogrupo1,idtipogrupo2);
			    		}
						break;
					case "combobox_grupo2":
						idtipogrupo2 = (int) combobox_grupo2.Model.GetValue(iter,1);
			    		descripgrupo2 = (string) combobox_grupo2.Model.GetValue(iter,0);
			    		if((bool) this.checkbutton_nuevo_producto.Active == true){
			    			ultimo_id_prod(idtipogrupo,idtipogrupo1,(int) combobox_grupo2.Model.GetValue(iter,1));
			    		}
						break;
					case "combobox_grupo_export":
						idtipogrupo_export = (int) combobox_grupo_export.Model.GetValue(iter,1);
						break;
					case "combobox_grupo1_export":
						idtipogrupo1_export = (int) combobox_grupo1_export.Model.GetValue(iter,1);
						break;
					case "combobox_grupo2_export":
						idtipogrupo2_export = (int) combobox_grupo2_export.Model.GetValue(iter,1);
						break;				
				}
			}
		}

		void on_button_busca_marcaprod_clicked(object sender, EventArgs args)
		{
			//osiris_erp_marca_productos
			object[] parametros_objetos = {entry_idmarca,entry_marcaproducto};
			string[] parametros_sql = {"SELECT * FROM osiris_erp_marca_productos WHERE activa = 'true' "};
			string[] parametros_string = {};
			string[,] args_buscador1 = {{"MARCA","AND descripcion_marca LIKE '%","%' "},
										{"ID MARCA","AND id_marca_producto = '","' "}};
			string[,] args_buscador2 = {{"ID MARCA","AND id_marca_producto = '","' "},
										{"MARCA","AND descripcion_marca LIKE '%","%' "}};
			string[,] args_orderby = {{"",""}};
			classfind_data.buscandor(parametros_objetos,parametros_sql,parametros_string,"find_marcaproducto",0,args_buscador1,args_buscador2,args_orderby);
		}

		void on_button_agrega_marca_clicked(object sender, EventArgs args)
		{
			string[] parametros = {NomEmpleado,LoginEmpleado};
			new osiris.catalogo_marcas_productos(parametros);	
		}

		void on_button_graba_codigobarra_clicked(object sender, EventArgs args)
		{
			if (entry_codigo_barras.Text.Trim () != "") {
				if ((string)classpublic.lee_registro_de_tabla ("osiris_erp_codigos_barra", "id_codigo_barras", "WHERE id_codigo_barras = '" + entry_codigo_barras.Text.Trim () + "' AND eliminado = 'false' ", "id_codigo_barras", "string") == "") {
					string[,] parametros = {{ "id_codigo_barras,", "'" + entry_codigo_barras.Text.Trim () + "'," },
						{ "id_producto,", "'" + entry_codigo_producto.Text + "'," },
						{ "marca,", "'" + entry_marcaproducto.Text + "'," },
						{ "id_quien_creo,", "'" + LoginEmpleado + "'," },
						{ "fechahora_creacion,", "'" + DateTime.Now.ToString ("yyyy-MM-dd HH:mm:ss") + "'," },
						{ "presentacion,", "'" + tipounidadproducto + "'," },
						{ "id_marca_producto", "'" + entry_idmarca.Text + "' " }
					};
					
					object[] paraobj = { entry_codigo_barras };
					new osiris.insert_registro ("osiris_erp_codigos_barra", parametros, paraobj);
				} else {
					MessageDialog msgBoxError = new MessageDialog (MyWinError, DialogFlags.DestroyWithParent,
						                            MessageType.Error, ButtonsType.Close, "Codigo de Barra ya existe en la base de datos");
					msgBoxError.Run ();					msgBoxError.Destroy ();
				}

			} else {
				MessageDialog msgBoxError = new MessageDialog (MyWinError, DialogFlags.DestroyWithParent,
					MessageType.Error, ButtonsType.Close, "NO hay ningun codigo de barra capturado");
				msgBoxError.Run ();				msgBoxError.Destroy ();
			}
			llenado_codigobarra (idproduct.ToString().Trim());
		}
		
		void on_button_quitar_codigobarra_clicked(object sender, EventArgs args)
		{
			TreeModel model;
			TreeIter iterSelected;
			if (treeview_lista_codbarra.Selection.GetSelected (out model, out iterSelected)) {
				string[,] parametros = {{ "eliminado = ", "'true'," },
					{ "fechahora_eliminado = ", "'" + DateTime.Now.ToString ("yyyy-MM-dd HH:mm:ss") + "'," },
					{ "id_quien_elimino = ", "'" + LoginEmpleado + "' " },
					{ "WHERE id_codigo_barras = '", treeview_lista_codbarra.Model.GetValue (iterSelected,0)+ "';" }
				};
				object[] paraobj = { entry_codigo_barras };
				new osiris.update_registro ("osiris_erp_codigos_barra", parametros, paraobj);

				llenado_codigobarra (idproduct.ToString ().Trim ());
			} else {
				MessageDialog msgBoxError = new MessageDialog (MyWinError, DialogFlags.DestroyWithParent,
					MessageType.Error, ButtonsType.Close, "Seleccione un Codigo de Barra para eliminar de la lista...");
				msgBoxError.Run ();				msgBoxError.Destroy ();
			}
		}

		void on_button_export_codbarra_clicked(object sender, EventArgs args)
		{
			string query_sql = "SELECT id_codigo_barras,osiris_erp_codigos_barra.id_producto,descripcion_producto,marca,presentacion,descripcion_grupo_producto " +
								"FROM osiris_erp_codigos_barra,osiris_productos,osiris_grupo_producto " +
								"WHERE osiris_erp_codigos_barra.id_producto = osiris_productos.id_producto " +
								"AND osiris_productos.id_grupo_producto = osiris_grupo_producto.id_grupo_producto " +
								"AND osiris_erp_codigos_barra.eliminado = 'false';";

			string[] args_names_field = {"id_codigo_barras","id_producto","descripcion_producto","marca","presentacion","descripcion_grupo_producto"};
			string[] args_type_field = {"string","string","string","string","string","string"};
			string[] args_field_text = {""};
			string[] args_more_title = {""};
			string[,] args_formulas = {{"",""}};
			string[,] args_width = {{"",""}};
			new osiris.class_traslate_spreadsheet(query_sql,args_names_field,args_type_field,false,args_field_text,"",false,args_more_title,args_formulas,args_width);
		}

		void crea_treeview_codbarra ()
		{
			treeViewEngineCodigobarra = new TreeStore (typeof(string),
											typeof(string));
			treeview_lista_codbarra.Model = treeViewEngineCodigobarra;
			treeview_lista_codbarra.RulesHint = true;

			col_idproducto = new TreeViewColumn ();
			cellr0 = new CellRendererText ();
			col_idproducto.Title = "Codigo Barras";
			col_idproducto.PackStart (cellr0, true);
			col_idproducto.AddAttribute (cellr0, "text", 0);
			//col_idproducto.SortColumnId = (int) Column_prod.col_idproducto;

			col_desc_producto = new TreeViewColumn ();
			cellr1 = new CellRendererText ();
			col_desc_producto.Title = "Nombre Comercial";
			col_desc_producto.PackStart (cellr1, true);
			col_desc_producto.AddAttribute (cellr1, "text", 1);
			col_desc_producto.Resizable = true;

			treeview_lista_codbarra.AppendColumn(col_idproducto);  // 0
			treeview_lista_codbarra.AppendColumn(col_desc_producto); // 1
		}

		void crea_colums_treeview(object[] args,string [,] args_colums)
		{
			Gtk.CellRendererText text;
			Gtk.CellRendererToggle toggle;
			treeviewobject = (object) args[0] as Gtk.TreeView;
			treeViewEngine = (object) args[1] as Gtk.TreeStore;

			//foreach (TreeViewColumn tvc in treeviewobject.Columns)
			//	treeviewobject.RemoveColumn(tvc);

			treeViewEngine = new TreeStore(typeof(string),typeof(string),typeof(string),typeof(string),typeof(string),typeof(string),typeof(string),typeof(string),typeof(string),typeof(bool),typeof(bool));
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
			args[0] = treeviewobject;
			args[1] = treeViewEngine;
		}

		void llenado_codigobarra(string idproducto_)
		{
			treeViewEngineCodigobarra.Clear ();
			NpgsqlConnection conexion;
			conexion = new NpgsqlConnection (connectionString+nombrebd);
			// Verifica que la base de datos este conectada
			try{
				conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand ();
				comando.CommandText = "SELECT * FROM osiris_erp_codigos_barra WHERE id_producto = '"+idproducto_+"' " +
					"AND eliminado = 'false';";
				//Console.WriteLine(comando.CommandText);
				NpgsqlDataReader lector = comando.ExecuteReader ();
				while(lector.Read()){
					treeViewEngineCodigobarra.AppendValues(lector["id_codigo_barras"].ToString().Trim(),
					lector["marca"].ToString().Trim());
				}
			}catch (NpgsqlException ex){
				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
					MessageType.Warning, ButtonsType.Ok, "PostgresSQL error: {0}",ex.Message);
				msgBoxError.Run ();
				msgBoxError.Destroy();
			}
			conexion.Close ();
		}

		void on_checkbutton_cambia_utilidad_clicked(object sender, EventArgs args)
		{
			if (checkbutton_cambia_utilidad.Active == true){
				entry_porciento_utilidad.Sensitive = false;
			}else{
				entry_porciento_utilidad.Sensitive = true;
			}
		}	
		
		// Cuando seleccion el treeview de cargos extras para cargar los productos  
		void selecciona_fila(object sender, ToggledArgs args)
		{
			TreeIter iter;
			TreePath path = new TreePath (args.Path);
			if (treeview_precios_convenios.Model.GetIter (out iter, path)){
				bool old = (bool) treeview_precios_convenios.Model.GetValue (iter,0);
				treeview_precios_convenios.Model.SetValue(iter,0,!old);
			}	
		}
		
		//Alta de nuevo Producto
		void on_checkbutton_nuevo_producto_clicked (object sender, EventArgs args)
		{
			valor_anterior_descuento = "0.00";
			if(checkbutton_nuevo_producto.Active == true){ 
				MessageDialog msgBox = new MessageDialog (MyWin,DialogFlags.Modal,
				MessageType.Question,ButtonsType.YesNo,"¿ Esta seguro de querer crear un nuevo producto?");
				ResponseType miResultado = (ResponseType)
				msgBox.Run ();				msgBox.Destroy();
		 		if (miResultado == ResponseType.Yes){
		 			llenado_combobox(0,"",combobox_grupo,"sql","SELECT * FROM osiris_grupo_producto ORDER BY descripcion_grupo_producto;","descripcion_grupo_producto","id_grupo_producto",args_args,args_id_array);
					llenado_combobox(0,"",combobox_grupo1,"sql","SELECT * FROM osiris_grupo1_producto ORDER BY descripcion_grupo1_producto;","descripcion_grupo1_producto","id_grupo1_producto",args_args,args_id_array);
					llenado_combobox(0,"",combobox_grupo2,"sql","SELECT * FROM osiris_grupo2_producto ORDER BY descripcion_grupo2_producto;","descripcion_grupo2_producto","id_grupo2_producto",args_args,args_id_array);
					llenado_combobox(0,"",combobox_tipo_unidad,"array","","","",args_tipounidad,args_id_array);
					limpia_textos(true);
					activa_campos(true);
					button_calcular.Sensitive = true;
					entry_precio_venta.Sensitive = true;
					checkbutton_producto_anidado.Active = false;
					checkbutton_cambia_utilidad.Active = false;
					checkbutton_costounico.Active = false;
					checkbutton_apl_iva.Active = false;
					checkbutton_prod_activo.Active = false;
					checkbutton_descuento.Active = false;
					checkbutton_producto_anidado.Sensitive = true;
					checkbutton_costounico.Sensitive = true;
					checkbutton_apl_iva.Sensitive = true;
					checkbutton_prod_activo.Sensitive = true;
					checkbutton_cambia_utilidad.Sensitive = true;
					checkbutton_descuento.Sensitive = true;
					button_guardar.Sensitive = true;
					combobox_tipo_unidad.Sensitive = true;
					entry_costo.Text= "0.00";
					entry_descripcion.Text = "";
					entry_descuento_en_porciento.Text ="0.00";
					entry_descuento_en_pesos.Text ="0.00";
					entry_embalaje.Text = "0.00";
					entry_nombre_articulo.Text = "";
					entry_nombre_generico.Text = "";
					entry_iva.Text = "0.00";
					entry_porciento_utilidad.Text = "0.000";
					entry_precio_publico.Text = "0.00";
					entry_precio_unitario.Text ="0.00";
					entry_utilidad.Text ="0.00";
					precio_sin_iva.Text = "0.00";
					entry_precio_venta.Text = "0.00";
					entry_porcentage_iva.Text = "0.00";
					entry_codigo_producto.IsEditable = false;
				}else{
					checkbutton_nuevo_producto.Active = false;
					entry_codigo_producto.IsEditable = true;
				}
			}
			if(checkbutton_nuevo_producto.Active == false){ 
				activa_campos(false);
				this.button_guardar.Sensitive = false;
				checkbutton_nuevo_producto.Sensitive = true;
				entry_codigo_producto.IsEditable = true;
			}
		}
				
		void limpia_textos(bool valor)
		{
			entry_codigo_producto.Text = "";
			entry_descripcion.Text = "";
			entry_nombre_articulo.Text = "";
			entry_nombre_generico.Text = "";
			entry_porciento_utilidad.Text = "0";
		}
		
		void on_guarda_producto_clicked(object sender, EventArgs args)
		{
			if(checkbutton_nuevo_producto.Active == true){
				calculando_utilidad();
				MessageDialog msgBox = new MessageDialog (MyWin,DialogFlags.Modal,MessageType.Question,
										ButtonsType.YesNo,"¿ Desea grabar esta infomacion ?");
				ResponseType miResultado = (ResponseType)msgBox.Run ();
				msgBox.Destroy();
 				if (miResultado == ResponseType.Yes){
					if(entry_porciento_utilidad.Text == ""){
						entry_porciento_utilidad.Text = "0.0";
					}

					NpgsqlConnection conexion; 
					conexion = new NpgsqlConnection (connectionString+nombrebd);
					// Verifica que la base de datos este conectada
					try{
						conexion.Open ();
						NpgsqlCommand comando; 
						comando = conexion.CreateCommand ();
	 					comando.CommandText = "INSERT INTO osiris_productos("+
													"id_producto,"+
									 				"descripcion_producto,"+
									 				"id_grupo_producto,"+
									 				"id_grupo1_producto,"+
									 				"id_grupo2_producto,"+
									 				"precio_producto_publico, "+
									 				"costo_producto,"+
									 				"costo_por_unidad,"+
									 				"porcentage_ganancia,"+
									 				"id_quienlocreo_producto,"+
									 				"fechahora_creacion,"+
									 				"aplicar_iva,"+
									 				"cobro_activo,"+
									 				"aplica_descuento,"+
									 				"nombre_articulo,"+
									 				"nombre_generico_articulo,"+
									 				"cantidad_de_embalaje,"+
									 				"porcentage_descuento, "+
									 				"tipo_unidad_producto,"+
								                    "tiene_kit,"+
									 				"costo_unico," +
									 				"porcentage_iva" +
									 				") VALUES ('"+
									 				(string) entry_codigo_producto.Text.ToUpper()+"','"+
									 				(string) entry_descripcion.Text.ToUpper()+"','"+
									 				idtipogrupo.ToString()+"','"+
									 				idtipogrupo1.ToString()+"','"+
									 				idtipogrupo2.ToString()+"','"+
									 				(string) precio_sin_iva.Text.ToUpper() +"','"+
									 				(string) entry_costo.Text.ToUpper()+"','"+
									 				(string) entry_precio_unitario.Text.ToUpper()+"','"+
									 				(string) entry_porciento_utilidad.Text.ToUpper()+"','"+
									 				LoginEmpleado+"','"+
									 				DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"','"+
									 				(bool) checkbutton_apl_iva.Active+"','"+
									 				(bool) checkbutton_prod_activo.Active+"','"+
									 				(bool) checkbutton_descuento.Active+"','"+
									 				(string) entry_nombre_articulo.Text.ToUpper()+"','"+
									 				(string) entry_nombre_generico.Text.ToUpper()+"','"+
									 				(string) entry_embalaje.Text.ToUpper()+"','"+
									 				(string) entry_descuento_en_porciento.Text.ToUpper()+"','"+
									 				tipounidadproducto+"','"+
								                    (bool) this.checkbutton_producto_anidado.Active+"','"+
									 				(bool) checkbutton_costounico.Active+"','"+
													entry_porcentage_iva.Text.Trim()+"');";
						comando.ExecuteNonQuery();         	comando.Dispose();
						//Console.WriteLine(comando.CommandText);
						checkbutton_nuevo_producto.Active = false;
						MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
										MessageType.Info,ButtonsType.Close,"El producto se guardo con exito");
						msgBoxError.Run ();					msgBoxError.Destroy();

						checkbutton_nuevo_producto.Sensitive = true;
						button_editar.Sensitive = true;
						button_calcular.Sensitive = false;
						button_editar.Active = false;
						lista_precios_adicionales(entry_codigo_producto.Text.ToUpper(),(bool) checkbutton_apl_iva.Active,entry_porcentage_iva.Text.Trim());
												
						//verifica_codigo_barra();
						//lista_codigos_barra(entry_codigo_producto.Text);
						
					}catch(NpgsqlException ex){
	   					MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
										MessageType.Error,ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
						msgBoxError.Run ();					msgBoxError.Destroy();
					}
					conexion.Close ();
				}
			}else{
				//calculando_utilidad();
				if((bool) valida_actualizacion_info()){
				
					MessageDialog msgBox = new MessageDialog (MyWin,DialogFlags.Modal,MessageType.Question,
											ButtonsType.YesNo,"¿ Desea Actualizar esta infomacion ?");
					ResponseType miResultado = (ResponseType)
					msgBox.Run ();							msgBox.Destroy();
					if (miResultado == ResponseType.Yes){
						NpgsqlConnection conexion; 
						conexion = new NpgsqlConnection (connectionString+nombrebd);
		    	     	// Verifica que la base de datos este conectada
		    	     	try{
							conexion.Open ();
							NpgsqlCommand comando; 
							comando = conexion.CreateCommand ();
							comando.CommandText = "UPDATE osiris_productos SET "+
								"descripcion_producto = '"+(string) entry_descripcion.Text.Trim().ToUpper()+"',"+
								"id_grupo_producto = '"+idtipogrupo.ToString()+"',"+
								"id_grupo1_producto = '"+idtipogrupo1.ToString()+"',"+
								"id_grupo2_producto = '"+idtipogrupo2.ToString()+"',"+
								"precio_producto_publico = '"+(string) precio_sin_iva.Text.Trim().ToUpper()+"',"+
								"costo_producto = '"+(string) entry_costo.Text.Trim().ToUpper()+"',"+
								"costo_por_unidad = '"+(string) entry_precio_unitario.Text.Trim().ToUpper()+"',"+
								"porcentage_ganancia = '"+(string) entry_porciento_utilidad.Text.Trim().ToUpper()+"',"+
								"id_quienlocreo_producto = '"+LoginEmpleado+"',"+
								"aplicar_iva = '"+(bool) checkbutton_apl_iva.Active+"',"+
								"cobro_activo = '"+(bool) checkbutton_prod_activo.Active+"',"+
								"tiene_kit = '"+(bool) this.checkbutton_producto_anidado.Active+"',"+
								"aplica_descuento = '"+(bool) checkbutton_descuento.Active+"',"+
								"nombre_articulo = '"+(string) entry_nombre_articulo.Text.Trim().ToUpper()+"',"+
								"nombre_generico_articulo = '"+(string) entry_nombre_generico.Text.Trim().ToUpper()+"', "+
								"cantidad_de_embalaje = '"+(string) entry_embalaje.Text.Trim().ToUpper()+"',"+
								"porcentage_descuento = '"+(string) entry_descuento_en_porciento.Text.Trim().ToUpper()+"',"+
								"costo_unico = '"+(bool) checkbutton_costounico.Active+"',"+
								"tipo_unidad_producto = '"+(string) tipounidadproducto+"',"+
								"porcentage_iva = '"+entry_porcentage_iva.Text.Trim()+"',"+
								"historial_de_cambios = historial_de_cambios || '"+LoginEmpleado+";"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+";"+
			 					precio_unitario_anterior.ToString().Trim()+";"+
			 					precio_costo_anterior.ToString().Trim()+";"+
			 					utilidad_anterior.ToString().Trim()+"\n' "+
								"WHERE id_producto =  '"+codigoproducto+"' ;";
							//Console.WriteLine(comando.CommandText);
	 						comando.ExecuteNonQuery();    	    	       	comando.Dispose();
							      	
	    	    	       	MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
											MessageType.Info,ButtonsType.Close,"Los datos se actualizaron con exito");			
							msgBoxError.Run ();					msgBoxError.Destroy();
							lista_precios_adicionales(entry_codigo_producto.Text.ToString().Trim(),(bool) checkbutton_apl_iva.Active,entry_porcentage_iva.Text.Trim());
							//lista_codigos_barra();
							button_guardar.Sensitive = true;
							button_editar.Sensitive = true;
							button_calcular.Sensitive = false;
							button_busca_producto_anidado.Sensitive = false;
							button_quitar.Sensitive = false;
							checkbutton_producto_anidado.Sensitive = false;
							combobox_grupo.Sensitive = false;
							combobox_grupo1.Sensitive = false;
							combobox_grupo2.Sensitive = false;
							entry_descripcion.Sensitive = false;
							entry_nombre_articulo.Sensitive = false;
							entry_nombre_generico.Sensitive = false;
							entry_codigo_barras.Sensitive = false;
							entry_idmarca.Sensitive = false;
							entry_marcaproducto.Sensitive = false;
							button_busca_marcaprod.Sensitive = false;
							entry_costo.Sensitive = false;
							entry_embalaje.Sensitive = false;
							checkbutton_apl_iva.Sensitive = false;
							checkbutton_prod_activo.Sensitive = false;
							entry_precio_unitario.Sensitive = false;
							entry_porciento_utilidad.Sensitive = false;
							entry_utilidad.Sensitive = false;
							entry_iva.Sensitive = false;
							entry_descuento_en_pesos.Sensitive = false;
							entry_iva.Sensitive = false;
							entry_utilidad.Sensitive = false;
							entry_descuento_en_porciento.Sensitive = false;
							entry_descuento_en_pesos.Sensitive = false;
							//entry_precio_publico.Sensitive = false;
							entry_producto.Sensitive = false;
							checkbutton_costounico.Sensitive = false;
							checkbutton_cambia_utilidad.Active = false;
							checkbutton_descuento.Sensitive = false;
							checkbutton_cambia_utilidad.Sensitive = false;
							button_editar.Active = false;
						}catch(NpgsqlException ex){
							MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
											MessageType.Error,ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
							msgBoxError.Run ();					msgBoxError.Destroy();
		       			}
	       				conexion.Close ();
	       			}
				}
			}
		}
		
		bool valida_actualizacion_info()
		{
			bool error_verificacion = true;
			if((string) tipounidadproducto == ""){
				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
											MessageType.Error,ButtonsType.Close,"Verifique el TIPO DE UNIDAD del PRODUCTO");			
				msgBoxError.Run ();					msgBoxError.Destroy();
				error_verificacion = false;
			}
			if(entry_embalaje.Text == ""){
				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
											MessageType.Error,ButtonsType.Close,"Verifique el EMBALAJE del PRODUCTO");			
				msgBoxError.Run ();					msgBoxError.Destroy();
				error_verificacion = false;
			}else{
				if(float.Parse(entry_embalaje.Text) <= 0){
					MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
											MessageType.Error,ButtonsType.Close,"Verifique el EMBALAJE del PRODUCTO debe ser mayor a CERO");			
					msgBoxError.Run ();					msgBoxError.Destroy();
					error_verificacion = false;
				}
			}
			
			return error_verificacion;
		}
					
		void on_button_busca_producto_clicked(object sender, EventArgs a)
		{
			buscando_producto("producto");
		}
		
		void on_button_busca_producto_anidado_clicked(object sender, EventArgs a)
		{
			buscando_producto("anidado");
		}
		
		void buscando_producto(string tipobusqueda)
		{
			tiposeleccion = tipobusqueda;
			
			Glade.XML gxml = new Glade.XML (null, "almacen_costos_compras.glade", "busca_producto", null);
			gxml.Autoconnect (this);
			busca_producto.Show();
			label_cantidad.Hide();
			label_pack.Hide();
			entry_embalaje_pack.Hide();
			label_desc_proveedor.Hide();
			entry_producto_proveedor.Hide();
			label_codprod_proveedor.Hide();
			entry_codprod_proveedor.Hide();
			hbox3.Hide();
			entry_cantidad_aplicada.Hide();
			label390.Hide();
			combobox_tipo_unidad2.Hide();
			crea_treeview_busqueda();
			
			button_buscar_busqueda.Clicked += new EventHandler(on_llena_lista_producto_clicked);
			entry_expresion.KeyPressEvent += onKeyPressEvent_enter;
			button_selecciona.Clicked += new EventHandler(on_selecciona_producto_clicked);
			button_salir.Clicked += new EventHandler(on_cierraventanas_clicked);
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
													typeof(string),
													typeof(string),
													typeof(string),
													typeof(string),
													typeof(string),
													typeof(string),
													typeof(string),
													typeof(string),
													typeof(bool),
													typeof(bool),
													typeof(bool),
													typeof(string),
			                                        typeof(bool),
													typeof(string),
			                                     	typeof(string));
				lista_de_producto.Model = treeViewEngineBusca2;
			
				lista_de_producto.RulesHint = true;
			
				lista_de_producto.RowActivated += on_selecciona_producto_clicked;  // Doble click selecciono producto*/
				
				col_idproducto = new TreeViewColumn();
				cellr0 = new CellRendererText();
				
				col_idproducto.Title = "ID Producto";
				col_idproducto.PackStart(cellr0, true);
				col_idproducto.AddAttribute (cellr0, "text", 0);
				//col_idproducto.SortColumnId = (int) Column_prod.col_idproducto;
			
				col_desc_producto = new TreeViewColumn();
				cellr1 = new CellRendererText();
				col_desc_producto.Title = "Descripcion de Producto";
				col_desc_producto.PackStart(cellr1, true);
				col_desc_producto.AddAttribute (cellr1, "text", 1);
				col_desc_producto.Resizable = true;
				cellr1.Width = 450;
				//col_desc_producto.SortColumnId = (int) Column_prod.col_desc_producto;
				
				col_precioprod = new TreeViewColumn();
				cellrt2 = new CellRendererText();
				col_precioprod.Title = "Precio Producto";
				col_precioprod.PackStart(cellrt2, true);
				col_precioprod.AddAttribute (cellrt2, "text", 2);
				//col_precioprod.SortColumnId = (int) Column_prod.col_precioprod;
            
				col_ivaprod = new TreeViewColumn();
				cellrt3 = new CellRendererText();
				col_ivaprod.Title = "I.V.A.";
				col_ivaprod.PackStart(cellrt3, true);
				col_ivaprod.AddAttribute (cellrt3, "text", 3);
				//col_ivaprod.SortColumnId = (int) Column_prod.col_ivaprod;
            
				col_totalprod = new TreeViewColumn();
				cellrt4 = new CellRendererText();
				col_totalprod.Title = "Total";
				col_totalprod.PackStart(cellrt4, true);
				col_totalprod.AddAttribute (cellrt4, "text", 4);
				//col_totalprod.SortColumnId = (int) Column_prod.col_totalprod;
            
				col_descuentoprod = new TreeViewColumn();
				cellrt5 = new CellRendererText();
				col_descuentoprod.Title = "% Descuento";
				col_descuentoprod.PackStart(cellrt5, true);
				col_descuentoprod.AddAttribute (cellrt5, "text", 5);
				//col_descuentoprod.SortColumnId = (int) Column_prod.col_descuentoprod;
      
				col_preciocondesc = new TreeViewColumn();
				cellrt6 = new CellRendererText();
				col_preciocondesc.Title = "Precio con Desc.";
				col_preciocondesc.PackStart(cellrt6, true);
				col_preciocondesc.AddAttribute (cellrt6, "text", 6);
				//col_preciocondesc.SortColumnId = (int) Column_prod.col_preciocondesc;
            
				col_grupoprod = new TreeViewColumn();
				cellrt7 = new CellRendererText();
				col_grupoprod.Title = "Grupo Producto";
				col_grupoprod.PackStart(cellrt7, true);
				col_grupoprod.AddAttribute (cellrt7, "text", 7);
				//col_grupoprod.SortColumnId = (int) Column_prod.col_grupoprod;
            
				col_grupo1prod = new TreeViewColumn();
				cellrt8 = new CellRendererText();
				col_grupo1prod.Title = "Grupo1 Producto";
				col_grupo1prod.PackStart(cellrt8, true);
				col_grupo1prod.AddAttribute (cellrt8, "text", 8);
				//col_grupo1prod.SortColumnId = (int) Column_prod.col_grupo1prod;
                        
				col_grupo2prod = new TreeViewColumn();
				cellrt9 = new CellRendererText();
				col_grupo2prod.Title = "Grupo2 Producto";
				col_grupo2prod.PackStart(cellrt9, true);
				col_grupo2prod.AddAttribute (cellrt9, "text", 9);
				//col_grupo2prod.SortColumnId = (int) Column_prod.col_grupo2prod;
				
				col_costoprod_uni = new TreeViewColumn();
				cellrt12 = new CellRendererText();
				col_costoprod_uni.Title = "Precio Unitario";
				col_costoprod_uni.PackStart(cellrt12, true);
				col_costoprod_uni.AddAttribute (cellrt12, "text", 12);
				//col_costoprod_uni.SortColumnId = (int) Column_prod.col_costoprod_uni;
				
				col_aplica_iva = new TreeViewColumn();
				cellrt19 = new CellRendererText();
				col_aplica_iva.Title = "Iva Activo?";
				col_aplica_iva.PackStart(cellrt19, true);
				col_aplica_iva.AddAttribute (cellrt19, "text", 19);
				//col_aplica_iva.SortColumnId = (int) Column_prod.col_aplica_iva;
				
				col_cobro_activo = new TreeViewColumn();
				cellrt20 = new CellRendererText();
				col_cobro_activo.Title = "Prod. Activo?";
				col_cobro_activo.PackStart(cellrt20, true);
				col_cobro_activo.AddAttribute (cellrt20, "text", 20);
				//col_cobro_activo.SortColumnId = (int) Column_prod.col_cobro_activo;
				
				lista_de_producto.AppendColumn(col_idproducto);  // 0
				lista_de_producto.AppendColumn(col_desc_producto); // 1
				lista_de_producto.AppendColumn(col_precioprod);	//2
				lista_de_producto.AppendColumn(col_ivaprod);	// 3
				lista_de_producto.AppendColumn(col_totalprod); // 4
				lista_de_producto.AppendColumn(col_descuentoprod); //5
				lista_de_producto.AppendColumn(col_preciocondesc); // 6
				lista_de_producto.AppendColumn(col_grupoprod);	//7
				lista_de_producto.AppendColumn(col_grupo1prod);	//8
				lista_de_producto.AppendColumn(col_grupo2prod);	//9
				lista_de_producto.AppendColumn(col_costoprod_uni); //12
				lista_de_producto.AppendColumn(col_aplica_iva);//19
				lista_de_producto.AppendColumn(col_cobro_activo);//20
		}
		
		void on_button_calcula_utilidad_clicked(object sender, EventArgs a)
		{
			calculando_utilidad();
		}
			
		void calculando_utilidad()
		{
			//entry_precio_venta
			if(entry_porciento_utilidad.Text == ""){
				entry_porciento_utilidad.Text = "0.0";
			}
			precio_uni = 0;
			precio_unitario_anterior = decimal.Parse(this.entry_precio_publico.Text,System.Globalization.NumberStyles.Float, new System.Globalization.CultureInfo( "es-MX" ));
			precio_costo_anterior = decimal.Parse(this.entry_precio_unitario.Text,System.Globalization.NumberStyles.Float, new System.Globalization.CultureInfo( "es-MX" ));
			
			if(decimal.Parse(entry_costo.Text) != 0 && decimal.Parse(entry_embalaje.Text) != 0){
				if(decimal.Parse(entry_precio_venta.Text) == 0){
					decimal cost_prod = decimal.Parse(entry_costo.Text); 
					decimal embalaje_prod = decimal.Parse(entry_embalaje.Text);
					precio_uni = cost_prod/embalaje_prod;
					decimal porc_utilidad = 0;
					if (checkbutton_cambia_utilidad.Active == true){
						//this.entry_porciento_utilidad.Sensitive = false;
						porc_utilidad = busca_porcentage_utilidad(precio_uni);
					}else{
						//this.entry_porciento_utilidad.Sensitive = true;
						porc_utilidad = decimal.Parse(entry_porciento_utilidad.Text);
					}
					decimal utilidad_prod = precio_uni*porc_utilidad/100;
					decimal precio_pub = precio_uni+utilidad_prod;
					entry_utilidad.Text = utilidad_prod.ToString("F").Replace(",",".");
					entry_precio_unitario.Text = precio_uni.ToString("F").Replace(",",".");
					entry_precio_publico.Text = precio_pub.ToString("F").Replace(",",".");				
					entry_porciento_utilidad.Text = porc_utilidad.ToString("###0.0###").Replace(",",".");
					
					decimal calculo_iva = 0;
					decimal preciopublico = 0;
					decimal descuentocero = 0;
					decimal descuento = decimal.Parse(entry_descuento_en_porciento.Text); 
					decimal porcientodescuento = 0;
					decimal valoriva = decimal.Parse(classpublic.ivaparaaplicar);
					
					if (this.checkbutton_descuento.Active == true){
						descuento = (precio_uni * descuento)/100;
						entry_descuento_en_pesos.Text = descuento.ToString("F").Replace(",",".");				
					}else{
						entry_descuento_en_pesos.Text = descuentocero.ToString("F").Replace(",",".");
						entry_descuento_en_porciento.Text = descuentocero.ToString("F").Replace(",",".");
					}
					if(checkbutton_apl_iva.Active == true){
						calculo_iva = (precio_pub*valoriva)/100;
						preciopublico = calculo_iva + precio_pub;
						entry_iva.Text = calculo_iva.ToString("F").Replace(",",".");					
						entry_precio_publico.Text = preciopublico.ToString("F").Replace(",",".");
						//entry_precio_venta.Text = preciopublico.ToString("F").Replace(",",".");
					}else{
						preciopublico = precio_pub;
						entry_iva.Text = calculo_iva.ToString("F").Replace(",",".");
						entry_precio_publico.Text = "0";
						//entry_precio_venta.Text = precio_pub.ToString("F").Replace(",",".");
					}				
					precio_sin_iva.Text = precio_pub.ToString("F").Replace(",",".");

				}else{
					if(decimal.Parse(entry_precio_venta.Text) >= decimal.Parse(entry_precio_unitario.Text)){
						decimal cost_prod = decimal.Parse(entry_costo.Text); 
						decimal embalaje_prod = decimal.Parse(entry_embalaje.Text);
						decimal porc_utilidad = 0;
						decimal valoriva = decimal.Parse(classpublic.ivaparaaplicar);
						precio_uni = cost_prod/embalaje_prod;
						if(checkbutton_apl_iva.Active == true){
							porc_utilidad = (((decimal.Parse(entry_precio_venta.Text) / ((valoriva/100)+1)) - precio_uni)/precio_uni)*100;
						}else{
							porc_utilidad = (((decimal.Parse(entry_precio_venta.Text) - precio_uni) / precio_uni) * 100);
						}
						checkbutton_cambia_utilidad.Active = false;
						decimal utilidad_prod = precio_uni*porc_utilidad/100;
						decimal precio_pub = precio_uni+utilidad_prod;
						entry_utilidad.Text = utilidad_prod.ToString("F").Replace(",",".");
						entry_precio_unitario.Text = precio_uni.ToString("F").Replace(",",".");
						entry_precio_publico.Text = precio_pub.ToString("F").Replace(",",".");				
						entry_porciento_utilidad.Text = porc_utilidad.ToString("##0.0###").Replace(",",".");

						decimal calculo_iva = 0;
						decimal preciopublico = 0;
						decimal descuentocero = 0;
						decimal descuento = decimal.Parse(entry_descuento_en_porciento.Text); 
						decimal porcientodescuento = 0;

						if (this.checkbutton_descuento.Active == true){
							descuento = (precio_uni * descuento)/100;
							this.entry_descuento_en_pesos.Text = descuento.ToString("F").Replace(",",".");				
						}else{
							this.entry_descuento_en_pesos.Text = descuentocero.ToString("F").Replace(",",".");
							this.entry_descuento_en_porciento.Text = descuentocero.ToString("F").Replace(",",".");
						}
						if(this.checkbutton_apl_iva.Active == true){
							calculo_iva = (precio_pub*valoriva)/100;
							preciopublico = calculo_iva + precio_pub;
							entry_iva.Text = calculo_iva.ToString("F").Replace(",",".");					
							entry_precio_publico.Text = preciopublico.ToString("F").Replace(",",".");
						}else{
							preciopublico = precio_pub;
							entry_iva.Text = calculo_iva.ToString("F").Replace(",",".");
							entry_precio_publico.Text = "0";
						}				
						precio_sin_iva.Text = precio_pub.ToString("F").Replace(",",".");
					}else{
						MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
							MessageType.Info, 
							ButtonsType.Close, "El Precio de Venta no puede ser menor que el Costro Unitario");
						msgBoxError.Run ();
						msgBoxError.Destroy();
					}
				}
			}else{
				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
								MessageType.Info, 
								ButtonsType.Close, "Escriba el costo y embalaje del producto");
								msgBoxError.Run ();
								msgBoxError.Destroy();
			}
		}
		
	    void on_checkbutton_apl_iva_clicked(object sender, EventArgs a)
		{
			if( this.checkbutton_apl_iva.Active == true){
				aplicariva_producto = true;
			}else{
				aplicariva_producto = false;
			}
		}
	    
	    void on_checkbutton_descuento_clicked(object sender, EventArgs a)
		{
			if( this.checkbutton_descuento.Active == true){
				//aplica_descuento = true;
				this.entry_descuento_en_porciento.Text = valor_anterior_descuento;
			}else{
				//aplica_descuento = false;
				this.entry_descuento_en_porciento.Text = "0.00";
			}
		}
		
		// llena la lista de productos
 		void on_llena_lista_producto_clicked (object sender, EventArgs args)
 		{
 			llena_la_lista_de_productos();
 		}
		
		void llena_la_lista_de_productos()
 		{
 			treeViewEngineBusca2.Clear(); // Limpia el treeview cuando realiza una nueva busqueda
			string query_tipo_busqueda = "";
			if(radiobutton_nombre.Active == true) {
				query_tipo_busqueda = "AND osiris_productos.descripcion_producto LIKE '%"+entry_expresion.Text.ToUpper().Trim()+"%' ORDER BY descripcion_producto; "; }
			if(radiobutton_codigo.Active == true) {
				query_tipo_busqueda = "AND to_char(osiris_productos.id_producto,'999999999999') LIKE '%"+entry_expresion.Text.Trim()+"%'  ORDER BY id_producto; ";
			}
			NpgsqlConnection conexion;
			conexion = new NpgsqlConnection (connectionString+nombrebd);
            // Verifica que la base de datos este conectada
			try{
				conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand ();
               	comando.CommandText = "SELECT to_char(osiris_productos.id_producto,'999999999999') AS codProducto, "+
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
							"AND osiris_productos.id_grupo2_producto = osiris_grupo2_producto.id_grupo2_producto "+
							query_tipo_busqueda;
				//Console.WriteLine(comando.CommandText);
				NpgsqlDataReader lector = comando.ExecuteReader ();
				float calculodeiva;
				float preciomasiva;
				float preciocondesc;
				float tomaprecio;
				float tomadescue;
				float valoriva;   // = float.Parse(classpublic.ivaparaaplicar);
							
				while(lector.Read()){
					valoriva = float.Parse(lector["porcentage_iva"].ToString().Trim());
					calculodeiva = 0;
					preciomasiva = 0;
					tomaprecio = float.Parse((string) lector["preciopublico"],System.Globalization.NumberStyles.Float,new System.Globalization.CultureInfo("es-MX"));
					tomadescue = float.Parse((string) lector["porcentagesdesc"],System.Globalization.NumberStyles.Float,new System.Globalization.CultureInfo("es-MX"));
					preciocondesc = tomaprecio;
					if ((bool) lector["aplicar_iva"]){
						calculodeiva = (tomaprecio * valoriva)/100;
					}
					if ((bool) lector["aplica_descuento"]){
						preciocondesc = tomaprecio-((tomaprecio*tomadescue)/100);
					}
					preciomasiva = tomaprecio + calculodeiva;
					 
					treeViewEngineBusca2.AppendValues (
									(string) lector["codProducto"],
									(string) lector["descripcion_producto"],
									(string) lector["preciopublico"],
									calculodeiva.ToString("F").PadLeft(10).Replace(",","."),
									preciomasiva.ToString("F").PadLeft(10).Replace(",","."),
									(string) lector["porcentagesdesc"],
									preciocondesc.ToString("F").PadLeft(10).Replace(",","."),
									(string) lector["descripcion_grupo_producto"],
									(string) lector["descripcion_grupo1_producto"],
									(string) lector["descripcion_grupo2_producto"],
									(string) lector["nombre_articulo"],
									(string) lector["nombre_articulo"],
									(string) lector["costoproductounitario"],
									(string) lector["porcentageutilidad"],
									(string) lector["costoproducto"],
									(string) lector["cantidadembalaje"],
									(string) lector["idgrupoproducto"],
									(string) lector["idgrupo1producto"],
									(string) lector["idgrupo2producto"],
									(bool) lector["aplicar_iva"],
									(bool) lector["cobro_activo"],
									(bool) lector["aplica_descuento"],
									(string) lector["preciopublico1"],
					                (bool) lector["tiene_kit"],
									(string) lector["tipo_unidad_producto"],
									lector["porcentage_iva"].ToString().Trim());
					
					col_idproducto.SetCellDataFunc(cellr0, new Gtk.TreeCellDataFunc(cambia_colores_fila));
					col_desc_producto.SetCellDataFunc(cellr1, new Gtk.TreeCellDataFunc(cambia_colores_fila));
					col_precioprod.SetCellDataFunc(cellrt2, new Gtk.TreeCellDataFunc(cambia_colores_fila));
					col_ivaprod.SetCellDataFunc(cellrt3, new Gtk.TreeCellDataFunc(cambia_colores_fila));
					col_totalprod.SetCellDataFunc(cellrt4, new Gtk.TreeCellDataFunc(cambia_colores_fila));
					col_descuentoprod.SetCellDataFunc(cellrt5, new Gtk.TreeCellDataFunc(cambia_colores_fila));
					col_preciocondesc.SetCellDataFunc(cellrt6, new Gtk.TreeCellDataFunc(cambia_colores_fila));
					col_grupoprod.SetCellDataFunc(cellrt7, new Gtk.TreeCellDataFunc(cambia_colores_fila));
					col_grupo1prod.SetCellDataFunc(cellrt8, new Gtk.TreeCellDataFunc(cambia_colores_fila));
					col_grupo2prod.SetCellDataFunc(cellrt9, new Gtk.TreeCellDataFunc(cambia_colores_fila));
					col_costoprod_uni.SetCellDataFunc(cellrt12, new Gtk.TreeCellDataFunc(cambia_colores_fila));
					col_aplica_iva.SetCellDataFunc(cellrt19, new Gtk.TreeCellDataFunc(cambia_colores_fila));
					col_cobro_activo.SetCellDataFunc(cellrt20, new Gtk.TreeCellDataFunc(cambia_colores_fila));
				}
			}catch (NpgsqlException ex){
	   			MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
								MessageType.Warning, ButtonsType.Ok, "PostgresSQL error: {0}",ex.Message);
								msgBoxError.Run ();
								msgBoxError.Destroy();
			}
			conexion.Close ();
		}
		
		void cambia_colores_fila(Gtk.TreeViewColumn column, Gtk.CellRenderer cell, Gtk.TreeModel model, Gtk.TreeIter iter)
		{
			if ((bool)lista_de_producto.Model.GetValue (iter,20)==true) { 
				if ((bool)lista_de_producto.Model.GetValue (iter,19)==true){
					(cell as Gtk.CellRendererText).Foreground = "blue";
				}else{ 
					(cell as Gtk.CellRendererText).Foreground = "black"; }
			}else{
				(cell as Gtk.CellRendererText).Foreground = "red";
			}
		}
		
		void on_selecciona_producto_clicked (object sender, EventArgs args)
		{	
			entry_porciento_utilidad.Text = "0";
			entry_utilidad.Text = "0";
			entry_precio_venta.Text = "0";
			treeViewEnginePreciosConvenios.Clear();
								
			if(tiposeleccion == "anidado"){
								
				
			}else{
				TreeModel model;
				TreeIter iterSelected;
				if (lista_de_producto.Selection.GetSelected(out model, out iterSelected)){
					selecciona_producto((string) model.GetValue(iterSelected, 0),"AND osiris_productos.id_producto = '"+(string) model.GetValue(iterSelected, 0)+"' ");
					
					//cierra la ventana despues que almaceno la informacion en variables
					Widget win = (Widget) sender;
					win.Toplevel.Destroy();
				}
			}
		}
		
		void crea_treeview_precios_convenios()
		{
			treeViewEnginePreciosConvenios = new TreeStore(typeof(bool),
								typeof(string),
								typeof(string),
								typeof(string),
								typeof(string),
								typeof(string),
								typeof(bool),
			                    typeof(string),
			                    typeof(string));
			treeview_precios_convenios.Model = treeViewEnginePreciosConvenios;
			//Nombre de la ventana donde se muestra en el glade(treeview_precios_convenios)
			treeview_precios_convenios.RulesHint = true;
		
			//treeview_precios_convenios.RowActivated += on_selecciona_producto_clicked;  // Doble click selecciono producto
			
			col_seleccion = new TreeViewColumn();
			cellrt_seleccion = new CellRendererToggle();
			col_seleccion.Title = "Seleccion";
			col_seleccion.PackStart(cellrt_seleccion, true);
			col_seleccion.AddAttribute (cellrt_seleccion, "active", 0);
			cellrt_seleccion.Activatable = true;
			cellrt_seleccion.Toggled += selecciona_fila;
			col_seleccion.SortColumnId = (int) Column_princ.col_seleccion;			
			
			col_clie_aseg = new TreeViewColumn();
			cellr30 = new CellRendererText();
			col_clie_aseg.Title = "Convenio";
			col_clie_aseg.PackStart(cellr30, true);
			col_clie_aseg.AddAttribute (cellr30, "text", 1);
			col_clie_aseg.SortColumnId = (int) Column_princ.col_clie_aseg;
			
			col_precio_sin_iva = new TreeViewColumn();
			cellrt32 = new CellRendererText();
			col_precio_sin_iva.Title = "Precio Sin IVA";
			col_precio_sin_iva.PackStart(cellrt32, true);
			col_precio_sin_iva.AddAttribute (cellrt32, "text", 2);
			col_precio_sin_iva.SortColumnId = (int) Column_princ.col_precio_sin_iva;
			
			col_iva = new TreeViewColumn();
			cellrt34 = new CellRendererText();
			col_iva.Title = "IVA";
			col_iva.PackStart(cellrt34, true);
			col_iva.AddAttribute (cellrt34, "text", 3);
			col_iva.SortColumnId = (int) Column_princ.col_iva;
			
			col_precio_final = new TreeViewColumn();
			cellrt35 = new CellRendererText();
			cellrt35.Editable = true;
			cellrt35.Edited += NumberCellEdited_precioprod;
			col_precio_final.Title = "Precio Total";
			col_precio_final.PackStart(cellrt35, true);
			col_precio_final.AddAttribute (cellrt35, "text", 4);
			col_precio_final.SortColumnId = (int) Column_princ.col_precio_final;
			
			col_key = new TreeViewColumn();
			cellrt36 = new CellRendererText();
			col_key.Title = "Key";
			col_key.PackStart(cellrt36, true);
			col_key.AddAttribute (cellrt36, "text", 5);
			col_key.SortColumnId = (int) Column_princ.col_key;
			
			col_precioref = new TreeViewColumn();
			cellrt37 = new CellRendererText();
			col_precioref.Title = "Ref. Precio";
			col_precioref.PackStart(cellrt37, true);
			col_precioref.AddAttribute (cellrt37, "text", 8);
			col_precioref.SortColumnId = (int) Column_princ.col_precioref;
       
			treeview_precios_convenios.AppendColumn(col_seleccion);
			treeview_precios_convenios.AppendColumn(col_clie_aseg);
			treeview_precios_convenios.AppendColumn(col_precio_sin_iva);
			treeview_precios_convenios.AppendColumn(col_iva);
			treeview_precios_convenios.AppendColumn(col_precio_final);
			treeview_precios_convenios.AppendColumn(col_key);
			treeview_precios_convenios.AppendColumn(col_precioref);
		}
		
		enum Column_princ
		{
			col_seleccion,
			col_clie_aseg,
			col_precio_sin_iva,
			col_iva,
			col_precio_final,
			col_key,
			col_precioref
		}
				
		void lista_precios_adicionales(string codigoproducto, bool aplicariva, string porcentageiva)
		{
			treeViewEnginePreciosConvenios.Clear();
			NpgsqlConnection conexion;
			conexion = new NpgsqlConnection (connectionString+nombrebd);
            // Verifica que la base de datos este conectada
			try{
				conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand ();
            	comando.CommandText = "SELECT *,descripcion_empresa,osiris_empresas.id_tipo_paciente AS idtipopaciente," +
            				"osiris_empresas.id_empresa AS idempresa,sub_lista_precios,descripcion_tipo_paciente " +
             				"FROM osiris_his_tipo_pacientes,osiris_empresas " +
               				"WHERE osiris_his_tipo_pacientes.id_tipo_paciente = osiris_empresas.id_tipo_paciente " +
               				"AND osiris_empresas.lista_de_precio = 'true' " +
							"AND osiris_his_tipo_pacientes.lista_de_precio = 'true' " +
               				"ORDER BY osiris_his_tipo_pacientes.id_tipo_paciente,osiris_empresas.descripcion_empresa;";
						
				//Console.WriteLine(comando.CommandText);
				NpgsqlDataReader lector = comando.ExecuteReader ();
				string concatenacion = "";
				float preciocosto_producto = 0;
				float calculodeiva = 0;
				float precio_producto = 0;
				int toma_tipopaciente = 0;
								
				while (lector.Read()){
					if(toma_tipopaciente != (int) lector["idtipopaciente"]){
						concatenacion = "precio_producto_"+ Convert.ToString((int) lector["idtipopaciente"]).ToString().Trim();
						preciocosto_producto = float.Parse((string) classpublic.lee_registro_de_tabla("osiris_productos","id_producto","WHERE id_producto = '"+codigoproducto+"' ",concatenacion,"string"));
						calculodeiva = 0;
						if(preciocosto_producto >= 0){
							if((bool) aplicariva == true){
								if(preciocosto_producto > 0){
									calculodeiva = (preciocosto_producto * float.Parse(porcentageiva))/100;
								}
							}							
						}
						precio_producto = preciocosto_producto + calculodeiva;
						treeViewEnginePreciosConvenios.AppendValues(false,
						                                            lector["descripcion_tipo_paciente"].ToString(),
						                                            preciocosto_producto.ToString("F"),
						                                            calculodeiva.ToString("F"),
						                                            precio_producto.ToString("F"),
						                                            concatenacion,
						                                            aplicariva,
						                                            porcentageiva,
						                                            precio_producto.ToString("F"));
						toma_tipopaciente = (int) lector["idtipopaciente"];
					}
					
					if((bool) lector["sub_lista_precios"] == true){
						concatenacion = "precio_producto_" + Convert.ToString((int) lector["idtipopaciente"]).ToString().Trim()+Convert.ToString((int) lector["idempresa"]).ToString().Trim(); 
						preciocosto_producto = float.Parse((string) classpublic.lee_registro_de_tabla("osiris_productos","id_producto","WHERE id_producto = '"+codigoproducto+"' ",concatenacion,"string"));
						calculodeiva = 0;
						if((string) classpublic.lee_registro_de_tabla("osiris_empresas","servicio_medico_iva","WHERE servicio_medico_iva = 'true' AND id_empresa = '"+lector["idempresa"].ToString().Trim()+"' ","servicio_medico_iva","bool") == "True"){
							if(preciocosto_producto > 0){
								porcentageiva = classpublic.ivaparaaplicar;
								calculodeiva = (preciocosto_producto * float.Parse(porcentageiva))/100;
							}
						}
						precio_producto = preciocosto_producto + calculodeiva;
						treeViewEnginePreciosConvenios.AppendValues(false,
						                                            lector["descripcion_empresa"].ToString(),
						                                            preciocosto_producto.ToString("F"),
						                                            calculodeiva.ToString("F"),
						                                            precio_producto.ToString("F"),
						                                            concatenacion,
						                                            aplicariva,
						                                            porcentageiva,
						                                            precio_producto.ToString("F"));
					}													
				}
				
				comando.CommandText = "SELECT *,descripcion_aseguradora,osiris_aseguradoras.id_tipo_paciente AS idtipopaciente," +
							"osiris_aseguradoras.id_aseguradora AS idempresa,sub_lista_precios,descripcion_tipo_paciente " +
             				"FROM osiris_his_tipo_pacientes,osiris_aseguradoras " +
               				"WHERE osiris_his_tipo_pacientes.lista_de_precio = 'true' " +
               				"AND osiris_his_tipo_pacientes.id_tipo_paciente = osiris_aseguradoras.id_tipo_paciente " +
               				"AND osiris_aseguradoras.lista_de_precio = 'true' " +
               				"ORDER BY osiris_aseguradoras.descripcion_aseguradora;";
						
				//Console.WriteLine(comando.CommandText);
				lector = comando.ExecuteReader ();
				while (lector.Read()){
					calculodeiva = 0;
					concatenacion = "precio_producto_"+ Convert.ToString((int) lector["idtipopaciente"]).ToString().Trim();
					preciocosto_producto = float.Parse((string) classpublic.lee_registro_de_tabla("osiris_productos","id_producto","WHERE id_producto = '"+codigoproducto+"' ",concatenacion,"string"));
					//Console.WriteLine(concatenacion);
					if(preciocosto_producto >= 0){
						if((bool) aplicariva == true){
							if(preciocosto_producto > 0){
								calculodeiva = (preciocosto_producto * float.Parse(porcentageiva))/100;
							}
						}
					}
					precio_producto = preciocosto_producto + calculodeiva;
					treeViewEnginePreciosConvenios.AppendValues(false,
					                                            lector["descripcion_tipo_paciente"].ToString(),
					                                            preciocosto_producto.ToString("F"),
					                                            calculodeiva.ToString("F"),
					                                            precio_producto.ToString("F"),
					                                            concatenacion,
					                                            aplicariva,
					                                            porcentageiva,
					                                            precio_producto.ToString("F"));
					if((bool) lector["sub_lista_precios"] == true){
						concatenacion = "precio_producto_" + Convert.ToString((int) lector["idtipopaciente"]).ToString().Trim()+Convert.ToString((int) lector["idempresa"]).ToString().Trim(); 
						preciocosto_producto = float.Parse((string) classpublic.lee_registro_de_tabla("osiris_productos","id_producto","WHERE id_producto = '"+codigoproducto+"' ",concatenacion,"string"));
						calculodeiva = 0;
						if((string) classpublic.lee_registro_de_tabla("osiris_aseguradoras","servicio_medico_iva","WHERE servicio_medico_iva = 'true' AND id_aseguradora = '"+lector["idempresa"].ToString().Trim()+"' ","servicio_medico_iva","bool") == "True"){
							if(preciocosto_producto > 0){
								porcentageiva = classpublic.ivaparaaplicar;
								calculodeiva = (preciocosto_producto * float.Parse(porcentageiva))/100;
								aplicariva = true;
							}
						}
						precio_producto = preciocosto_producto + calculodeiva;
						treeViewEnginePreciosConvenios.AppendValues(false,
						                                            lector["descripcion_aseguradora"].ToString(),
						                                            preciocosto_producto.ToString("F"),
						                                            calculodeiva.ToString("F"),
						                                            precio_producto.ToString("F"),
						                                            concatenacion,
						                                            aplicariva,
						                                            porcentageiva,
						                                            precio_producto.ToString("F"));
					}													
				}
			}catch (NpgsqlException ex){
	   				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
						MessageType.Warning, ButtonsType.Ok, "PostgresSQL error: {0}",ex.Message);
						msgBoxError.Run ();
						msgBoxError.Destroy();
			}
			conexion.Close();
		}
						
		void on_editar_productos_clicked (object sender, EventArgs args)
		{
			bool activar;
			if (button_editar.Active == true){				
				activa_campos(true);
				button_guardar.Sensitive = true;
				this.button_calcular.Sensitive = true;
			}else{
				activa_campos(false);
				button_guardar.Sensitive = false;
				this.button_calcular.Sensitive = false;
			}
		}
		
		void NumberCellEdited_precioprod(object o, EditedArgs args)
		{
			//Gtk.CellRendererText onRendererChanged = o as Gtk.CellRendererText;
			//Console.WriteLine(onRendererChanged.ToString());
			Gtk.TreeIter iter;
			bool esnumerico = false;
			int var_paso = 0;
			int largo_variable = args.NewText.ToString().Length;
			string toma_variable = args.NewText.ToString();
			float precio_coniva = 0;
			float valoriva = 0;
			float preciosiniva = 0;
			float porcentageiva = 0;
						
			treeViewEnginePreciosConvenios.GetIter (out iter, new Gtk.TreePath (args.Path));
			
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
				porcentageiva = float.Parse((string) treeViewEnginePreciosConvenios.GetValue(iter,7));
				treeViewEnginePreciosConvenios.SetValue(iter,0,true);
				treeViewEnginePreciosConvenios.SetValue(iter,4,float.Parse((string) args.NewText).ToString("F"));
				precio_coniva = float.Parse(args.NewText);				
				preciosiniva = precio_coniva / ((porcentageiva/100)+1);
				valoriva = precio_coniva - preciosiniva;
				treeViewEnginePreciosConvenios.SetValue(iter,2,preciosiniva.ToString("F"));
				treeViewEnginePreciosConvenios.SetValue(iter,3,valoriva.ToString("F"));
			}
 		}
		
		void on_button_guarda_preconvenios_clicked(object sender, EventArgs args)
		{
			TreeIter iterSelected;
			MessageDialog msgBox = new MessageDialog (MyWin,DialogFlags.Modal,MessageType.Question,
										ButtonsType.YesNo,"¿ Desea Almacenar estos precios ?");
			ResponseType miResultado = (ResponseType)
			msgBox.Run ();							msgBox.Destroy();
			if (miResultado == ResponseType.Yes){
				NpgsqlConnection conexion; 
				conexion = new NpgsqlConnection (connectionString+nombrebd);
	    	    // Verifica que la base de datos este conectada
	    	    try{
					conexion.Open ();
					NpgsqlCommand comando; 
					comando = conexion.CreateCommand ();
					//this.treeViewEngineProductosaComprar.GetSortColumnId(out iterSelected,			
					if (treeViewEnginePreciosConvenios.GetIterFirst(out iterSelected)){						
						if((bool) treeViewEnginePreciosConvenios.GetValue(iterSelected,0)){
							comando.CommandText = "UPDATE osiris_productos SET "+treeViewEnginePreciosConvenios.GetValue(iterSelected,5).ToString()+"='"+treeViewEnginePreciosConvenios.GetValue(iterSelected,2).ToString()+"'," +
								"historial_mov_otrosprecios = historial_mov_otrosprecios || '"+LoginEmpleado+";"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+";"+treeViewEnginePreciosConvenios.GetValue(iterSelected,8).ToString()+";"+treeViewEnginePreciosConvenios.GetValue(iterSelected,4).ToString()+"\n' "+
								"WHERE id_producto = '"+entry_codigo_producto.Text.Trim()+"';";
							//Console.WriteLine(comando.CommandText);
							comando.ExecuteNonQuery();    	    	       	comando.Dispose();
						}
						while (treeViewEnginePreciosConvenios.IterNext(ref iterSelected)){
							if((bool) treeViewEnginePreciosConvenios.GetValue(iterSelected,0)){
								comando.CommandText = "UPDATE osiris_productos SET "+treeViewEnginePreciosConvenios.GetValue(iterSelected,5).ToString()+"='"+treeViewEnginePreciosConvenios.GetValue(iterSelected,2).ToString()+"'," +
									"historial_mov_otrosprecios = historial_mov_otrosprecios || '"+LoginEmpleado+";"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+";"+treeViewEnginePreciosConvenios.GetValue(iterSelected,8).ToString()+";"+treeViewEnginePreciosConvenios.GetValue(iterSelected,4).ToString()+"\n' "+
									"WHERE id_producto = '"+entry_codigo_producto.Text.Trim()+"';";
								//Console.WriteLine(comando.CommandText);
								comando.ExecuteNonQuery();    	    	       	comando.Dispose();
							}
						}
					}
				}catch (NpgsqlException ex){
	   				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
						MessageType.Warning, ButtonsType.Ok, "PostgresSQL error: {0}",ex.Message);
						msgBoxError.Run ();
						msgBoxError.Destroy();
				}
				conexion.Close();
			}
		}
		
		void activa_campos(bool valor)
		{	 
			checkbutton_nuevo_producto.Sensitive = valor;
			checkbutton_producto_anidado.Sensitive = valor;
			combobox_grupo.Sensitive = valor;
			combobox_grupo1.Sensitive = valor;
			combobox_grupo2.Sensitive = valor;
			entry_descripcion.Sensitive = valor;
			entry_nombre_articulo.Sensitive = valor;
			entry_nombre_generico.Sensitive = valor;
			entry_costo.Sensitive = valor;
			entry_embalaje.Sensitive = valor;
			checkbutton_apl_iva.Sensitive = valor;
			checkbutton_prod_activo.Sensitive = valor;
			entry_precio_unitario.Sensitive = valor;
			entry_porciento_utilidad.Sensitive = valor;
			entry_utilidad.Sensitive = valor;
			entry_iva.Sensitive = valor;
			entry_descuento_en_porciento.Sensitive = valor;
			entry_descuento_en_pesos.Sensitive = valor;
			entry_precio_publico.Sensitive = valor;
			entry_producto.Sensitive = valor;
			checkbutton_costounico.Sensitive = valor;
			checkbutton_descuento.Sensitive = valor;
			checkbutton_cambia_utilidad.Sensitive = valor;
			precio_sin_iva.Sensitive = valor;
			combobox_tipo_unidad.Sensitive = valor;
			button_busca_producto_anidado.Sensitive = valor;
			button_quitar.Sensitive = valor;
			entry_precio_venta.Sensitive = valor;
			entry_porcentage_iva.Sensitive = valor;
			entry_codigo_barras.Sensitive = valor;
			entry_idmarca.Sensitive = valor;
			entry_marcaproducto.Sensitive = valor;
			button_busca_marcaprod.Sensitive = valor;
		}
		
		void on_checkbutton_producto_anidado_cliked(object sender, EventArgs args)
		{
			entry_producto.Sensitive = checkbutton_producto_anidado.Active;
			button_busca_producto_anidado.Sensitive = checkbutton_producto_anidado.Active;
			button_quitar.Sensitive = checkbutton_producto_anidado.Active;
		}
		
		void on_checkbutton_apl_iva_cliked(object sender, EventArgs args)
		{
			if((bool) checkbutton_apl_iva.Active == true){
				entry_porcentage_iva.Text = classpublic.ivaparaaplicar;	
			}else{
				entry_porcentage_iva.Text = "0.00";
			}
		}
		
		void llenado_productos_anidados()
		{
			
		}
		
		void ultimo_id_prod(int idtipogrupo_,int idtipogrupo1_, int idtipogrupo2_)
		{
			long primera = Convert.ToInt64(idtipogrupo_ ) * 10000000000;
			long segundo = Convert.ToInt64(idtipogrupo1_) * 100000000;
			long tercero = Convert.ToInt64(idtipogrupo2_) * 100000;
			idproduct = (primera+segundo+tercero);
			lastproduct = idproduct+100000;
			
			NpgsqlConnection conexion; 
			conexion = new NpgsqlConnection (connectionString+nombrebd );
			// Verifica que la base de datos este conectada
			try{
				conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand ();

				// asigna el numero de folio de ingreso de paciente (FOLIO)
				comando.CommandText = "SELECT to_char(id_producto,'999999999999') AS idproducto "+ 
									"FROM osiris_productos "+
									"WHERE id_producto > '"+idproduct.ToString()+"'"+
									"AND id_producto < '"+lastproduct.ToString()+"'"+
									"ORDER BY id_producto DESC LIMIT 1;";
				//Console.WriteLine(comando.CommandText.ToString());
				NpgsqlDataReader lector = comando.ExecuteReader ();
				if ((bool) lector.Read()){
					newidproduct = long.Parse((string) lector["idproducto"]) + 1;
					lector.Close ();
				}else{
					newidproduct = idproduct + 1;
					lector.Close ();
				}
				// Actualiza entry 
				this.entry_codigo_producto.Text = newidproduct.ToString();
			}catch (NpgsqlException ex){
	   			MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
				MessageType.Error, ButtonsType.Close,"PostgresSQL error: {0}",ex.Message );
				msgBoxError.Run ();			msgBoxError.Destroy();
			}
			conexion.Close ();
		}					
		
		// Activa el enter en la busqueda de los productos
		[GLib.ConnectBefore ()]   	  // Esto es indispensable para que funcione
		void onKeyPressEvent_enter(object o, Gtk.KeyPressEventArgs args)
		{
			//Console.WriteLine(args.Event.Key);
			if (args.Event.Key.ToString() == "Return" || args.Event.Key.ToString() == "KP_Enter"){
				args.RetVal = true;
				llena_la_lista_de_productos();				
			}
		}
		
		decimal busca_porcentage_utilidad (decimal precio_uni)
		{
			decimal porcentageutilidad = 0;
			
			NpgsqlConnection conexion; 
			conexion = new NpgsqlConnection (connectionString+nombrebd );
            
			// Verifica que la base de datos este conectada
			try{
				conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand ();

				// asigna el numero de folio de ingreso de paciente (FOLIO)
				comando.CommandText = "SELECT precio_costo_inicial,precio_costo_final,porcentage_de_ganancia "+ 
									"FROM osiris_erp_tabla_ganancia "+
									"WHERE precio_costo_final >= '"+precio_uni.ToString()+"'"+
									"AND precio_costo_inicial <= '"+precio_uni.ToString()+"' "+
									"LIMIT 1;";
				//Console.WriteLine(comando.CommandText.ToString());
				
				NpgsqlDataReader lector = comando.ExecuteReader ();
				if ((bool) lector.Read()){
					porcentageutilidad  = (decimal) lector["porcentage_de_ganancia"];
				}
			}catch (NpgsqlException ex){
	   			MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
				MessageType.Error, ButtonsType.Close,"PostgresSQL error: {0}",ex.Message );
				msgBoxError.Run ();			msgBoxError.Destroy();
			}
			conexion.Close();
			
			return (porcentageutilidad);
		}

		// Valida entradas que solo sean numericas, se utiliza en ventana
		// Principal cuando selecciona el folio de productos
		// Ademas controla la tecla ENTRER para ver el procedimiento
		[GLib.ConnectBefore ()]   	  // Esto es indispensable para que funcione    
		void onKeyPressEvent_numeric(object o, Gtk.KeyPressEventArgs args)
		{
			string misDigitos = ".0123456789ﾰﾱﾲﾳﾴﾵﾶﾷﾸﾹﾮｔｒｓｑ（）";
			if (Array.IndexOf(misDigitos.ToCharArray(), Convert.ToChar(args.Event.Key)) == -1 && args.Event.Key.ToString()  != "BackSpace"){
				args.RetVal = true;
			}
		}
		
		[GLib.ConnectBefore ()]   	  // Esto es indispensable para que funcione    
		void onKeyPressEvent_codbarra(object o, Gtk.KeyPressEventArgs args)
		{
			if (args.Event.Key == Gdk.Key.Return || args.Event.Key == Gdk.Key.KP_Enter || args.Event.Key == Gdk.Key.Tab){
				
			}
		}
		
		[GLib.ConnectBefore ()]   	  // Esto es indispensable para que funcione    
		void onKeyPressEvent_codprod(object o, Gtk.KeyPressEventArgs args)
		{
			if (args.Event.Key.ToString() == "Return" || args.Event.Key.ToString() == "KP_Enter"){
				selecciona_producto (entry_codigo_producto.Text.Trim(),"AND osiris_productos.id_producto = '"+entry_codigo_producto.Text.Trim()+"' ");
				args.RetVal = true;
			}
			
		}
		
		void on_button_selec_prod_clicked(object sender, EventArgs args)
		{
			selecciona_producto (entry_codigo_producto.Text.Trim(),"AND osiris_productos.id_producto = '"+entry_codigo_producto.Text.Trim()+"' ");
		}
		
		void selecciona_producto (string codigo_producto_,string query_tipo_busqueda_)
		{
			string toma_valor;
			NpgsqlConnection conexion;
			conexion = new NpgsqlConnection (connectionString+nombrebd);
            // Verifica que la base de datos este conectada
			try{
				conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand ();
               	comando.CommandText = "SELECT to_char(osiris_productos.id_producto,'999999999999') AS codProducto, "+
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
							"AND osiris_productos.id_grupo2_producto = osiris_grupo2_producto.id_grupo2_producto "+
							query_tipo_busqueda_;
				//Console.WriteLine(comando.CommandText);
				NpgsqlDataReader lector = comando.ExecuteReader ();
				float calculodeiva;
				float preciomasiva;
				float preciocondesc;
				float tomaprecio;
				float tomadescue;
				float valoriva;   // = float.Parse(classpublic.ivaparaaplicar);
										
				if(lector.Read()){
					entry_precio_venta.Text ="0";
					valoriva = float.Parse(lector["porcentage_iva"].ToString().Trim());
					calculodeiva = 0;
					preciomasiva = 0;
					tomaprecio = float.Parse((string) lector["preciopublico"],System.Globalization.NumberStyles.Float,new System.Globalization.CultureInfo("es-MX"));
					tomadescue = float.Parse((string) lector["porcentagesdesc"],System.Globalization.NumberStyles.Float,new System.Globalization.CultureInfo("es-MX"));
					preciocondesc = tomaprecio;
					if ((bool) lector["aplicar_iva"]){
						calculodeiva = (tomaprecio * valoriva)/100;
					}
					if ((bool) lector["aplica_descuento"]){
						preciocondesc = tomaprecio-((tomaprecio*tomadescue)/100);
					}
					preciomasiva = tomaprecio + calculodeiva;
					
					/*
					treeViewEngineBusca2.AppendValues (
									(string) lector["codProducto"], //0
									(string) lector["descripcion_producto"], //1
									(string) lector["preciopublico"], //2
									calculodeiva.ToString("F").PadLeft(10).Replace(",","."), //3
									preciomasiva.ToString("F").PadLeft(10).Replace(",","."), //4
									(string) lector["porcentagesdesc"], //5
									preciocondesc.ToString("F").PadLeft(10).Replace(",","."), //6
									(string) lector["descripcion_grupo_producto"], //7
									(string) lector["descripcion_grupo1_producto"], //8
									(string) lector["descripcion_grupo2_producto"], //9
									(string) lector["nombre_articulo"], //10
									(string) lector["nombre_articulo"], //11
									(string) lector["costoproductounitario"],//12
									(string) lector["porcentageutilidad"], //13
									(string) lector["costoproducto"], //14
									(string) lector["cantidadembalaje"], //15
									(string) lector["idgrupoproducto"], // 16
									(string) lector["idgrupo1producto"], // 17
									(string) lector["idgrupo2producto"], // 18
									(bool) lector["aplicar_iva"], //19
									(bool) lector["cobro_activo"], //20
									(bool) lector["aplica_descuento"], //21
									(string) lector["preciopublico1"], //22
					                (bool) lector["tiene_kit"], // 23
									(string) lector["tipo_unidad_producto"], //24
									lector["porcentage_iva"].ToString().Trim()); //25
					*/

					idproduct = long.Parse(lector["codProducto"].ToString().Trim());
					lista_precios_adicionales(lector["codProducto"].ToString().Trim(),(bool) lector["aplicar_iva"],lector["porcentage_iva"].ToString().Trim());
					utilidad_anterior = decimal.Parse((string) lector["costoproducto"]);
					entry_descripcion.Text = (string) lector["descripcion_producto"];
					entry_nombre_articulo.Text = (string) lector["nombre_articulo"]; 
					entry_nombre_generico.Text = (string) lector["nombre_articulo"];
					toma_valor = lector["codProducto"].ToString().Trim();
					entry_codigo_producto.Text = lector["codProducto"].ToString().Trim();					
					codigoproducto = lector["codProducto"].ToString().Trim();
					toma_valor = (string) lector["preciopublico"];
					//this.entry_precio_publico.Text = toma_valor.TrimStart();
					precio_sin_iva.Text = toma_valor.TrimStart();
					valor_anterior_descuento = (string) lector["porcentagesdesc"];
					entry_descuento_en_porciento.Text = valor_anterior_descuento.TrimStart();
					toma_valor = (string) lector["costoproductounitario"];					
					entry_precio_unitario.Text = toma_valor.TrimStart();
					toma_valor = (string) lector["porcentageutilidad"];
					entry_porciento_utilidad.Text = toma_valor.TrimStart();
					toma_valor = (string) lector["costoproducto"];
					entry_costo.Text = toma_valor.TrimStart();
					toma_valor = (string) lector["cantidadembalaje"];
					entry_embalaje.Text = toma_valor.TrimStart();
					// Lineas especial para municipio de San Nicolas
					toma_valor = (string) lector["preciopublico1"];
					//this.entry_precios_costunitario_sannico.Text = toma_valor.Trim();
					descripgrupo = (string) lector["descripcion_grupo_producto"];
					descripgrupo1 = (string) lector["descripcion_grupo1_producto"];
					descripgrupo2 = (string) lector["descripcion_grupo2_producto"];
					toma_valor = (string) lector["costoproducto"];					
					//llenado_grupo("selecciona",descripgrupo,idtipogrupo);
					llenado_combobox(1,descripgrupo,combobox_grupo,"sql","SELECT * FROM osiris_grupo_producto ORDER BY descripcion_grupo_producto;","descripcion_grupo_producto","id_grupo_producto",args_args,args_id_array);
					llenado_combobox(1,descripgrupo1,combobox_grupo1,"sql","SELECT * FROM osiris_grupo1_producto ORDER BY descripcion_grupo1_producto;","descripcion_grupo1_producto","id_grupo1_producto",args_args,args_id_array);
					llenado_combobox(1,descripgrupo2,combobox_grupo2,"sql","SELECT * FROM osiris_grupo2_producto ORDER BY descripcion_grupo2_producto;","descripcion_grupo2_producto","id_grupo2_producto",args_args,args_id_array);
					llenado_combobox(1,(string) lector["tipo_unidad_producto"],combobox_tipo_unidad,"array","","","",args_tipounidad,args_id_array);
					checkbutton_apl_iva.Active = (bool) lector["aplicar_iva"];
					checkbutton_prod_activo.Active = (bool) lector["cobro_activo"];
					checkbutton_cambia_utilidad.Active = false;
					idtipogrupo = int.Parse((string) lector["idgrupoproducto"]);
					idtipogrupo1 = int.Parse((string) lector["idgrupo1producto"]);
					idtipogrupo2 = int.Parse((string) lector["idgrupo2producto"]);
					tipounidadproducto = (string) lector["tipo_unidad_producto"];
					calculando_utilidad();					 					
					if ((bool) lector["aplica_descuento"] == true){ 
						this.checkbutton_descuento.Active = true;
						apldesc = "true";
					}else{
						apldesc = "false";	
					}
					checkbutton_producto_anidado.Active = (bool) lector["tiene_kit"];
					entry_porcentage_iva.Text = float.Parse(lector["porcentage_iva"].ToString()).ToString("F");;
					
					// validacion de producto anidado o kit de producto 
					if ((bool) lector["tiene_kit"]){
						this.treeview_productos_anidados.Sensitive = true;
						llenado_productos_anidados();
					}else{
						this.treeview_productos_anidados.Sensitive = false;
						//this.treeViewEngineBusca3.Clear();
					}
					entry_porciento_utilidad.Sensitive = false;
					button_editar.Sensitive = true;
					checkbutton_nuevo_producto.Active = false;
    	    	    checkbutton_nuevo_producto.Sensitive = true;
					button_editar.Sensitive = true;

					llenado_codigobarra(lector["codProducto"].ToString().Trim());
					
										
				}
			}catch (NpgsqlException ex){
	   			MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
								MessageType.Warning, ButtonsType.Ok, "PostgresSQL error: {0}",ex.Message);
								msgBoxError.Run ();
								msgBoxError.Destroy();
			}
			conexion.Close ();
		}		
		
		void on_button_export_clicked(object sender, EventArgs args)
		{
			string sql_precios_clientes = "";
			string query_sql_grupo = "";
			string query_sql_grupo1 = "";
			string query_sql_grupo2 = "";
			string[] args_names_field;
			string[] args_type_field;
			
			string ordern_lista = "ORDER BY osiris_productos.id_grupo_producto,descripcion_producto;";
			if ((string)classpublic.nombre_empresa == "CLINICA ANAHUAC") {			
				sql_precios_clientes = ",to_char(precio_producto_400,'999999999.99') AS aseguradora," +
				"to_char(precio_producto_2002,'999999999.99') AS internamiento," +
				"to_char(precio_producto_2003,'999999999.99') AS venta_publico," +
				"to_char(precio_producto_1028,'999999999.99') AS banorte," +
				"to_char(precio_producto_1026,'999999999.99') AS KBR," +
				"to_char(precio_producto_1027,'999999999.99') AS merkafon," +
				"to_char(precio_producto_5004,'999999999.99') AS apodaca," +
				"to_char(precio_producto_1025,'999999999.99') AS takata," +
				"to_char(precio_producto_10210,'999999999.99') AS vitamedica_banamex," +
				"to_char(precio_producto_1029,'999999999.99') AS vitamedica_bancomer," +
				"to_char(precio_producto_10211,'999999999.99') AS suspe ";
				args_names_field = new string[] {
					"codProducto",
					"descripcion_producto",
					"costoproducto",
					"cantidadembalaje",
					"costoproductounitario",
					"precioosiris",
					"porcentageutilidad",
					"venta_publico",
					"aseguradora",
					"internamiento",
					"banorte",
					"KBR",
					"merkafon",
					"apodaca",
					"takata",
					"vitamedica_banamex",
					"vitamedica_bancomer",
					"suspe",
					"descripcion_grupo_producto",
					"descripcion_grupo1_producto",
					"descripcion_grupo2_producto"
				};
				args_type_field = new string[] {
					"string",
					"string",
					"float",
					"float",
					"float",
					"float",
					"float",
					"float",
					"float",
					"float",
					"float",
					"float",
					"float",
					"float",
					"float",
					"float",
					"float",
					"float",
					"string",
					"string",
					"string"
				};
			} else if ((string)classpublic.nombre_empresa == "CONTROL DE CLINICAS S.C.") {
				args_names_field = new string[] {"codProducto","descripcion_producto","costoproducto","cantidadembalaje","costoproductounitario","precioosiris","porcentageutilidad","descripcion_grupo_producto","descripcion_grupo1_producto","descripcion_grupo2_producto","inst_empresa"};
				args_type_field = new string[] {"string","string","float","float","float","float","float","string","string","string","float"};
				sql_precios_clientes = ",precio_producto_102 AS inst_empresa ";

			} else {
				args_names_field = new string[] {"codProducto","descripcion_producto","costoproducto","cantidadembalaje","costoproductounitario","precioosiris","porcentageutilidad","descripcion_grupo_producto","descripcion_grupo1_producto","descripcion_grupo2_producto"};
				args_type_field = new string[] {"string","string","float","float","float","float","float","string","string","string"};
			}
			
			
			string query_sql = "SELECT to_char(osiris_productos.id_producto,'999999999999') AS codProducto, "+
							"osiris_productos.descripcion_producto,osiris_productos.nombre_articulo,osiris_productos.nombre_generico_articulo, "+
							"to_char(precio_producto_publico,'99999999.99') AS precioosiris,"+
							"to_char(precio_producto_publico1,'99999999.99') AS preciopublico1,"+
							"to_char(cantidad_de_embalaje,'99999999.99') AS cantidadembalaje,"+
							"aplicar_iva,to_char(porcentage_descuento,'999.99') AS porcentagesdesc,aplica_descuento,cobro_activo,"+
							"descripcion_grupo_producto,descripcion_grupo1_producto,descripcion_grupo2_producto,to_char(costo_por_unidad,'999999999.99') AS costoproductounitario, "+
							"to_char(osiris_productos.id_grupo_producto,'9999999') AS idgrupoproducto,osiris_productos.id_grupo_producto, "+
							"to_char(osiris_grupo_producto.porcentage_utilidad_grupo,'99999.999') AS porcentageutilidadgrupo,"+
							"to_char(osiris_productos.id_grupo1_producto,'9999999') AS idgrupo1producto,osiris_productos.id_grupo1_producto,"+
							"to_char(osiris_productos.id_grupo2_producto,'9999999') AS idgrupo2producto,osiris_productos.id_grupo2_producto,"+
							"to_char(porcentage_ganancia,'99999.999') AS porcentageutilidad,to_char(costo_producto,'999999999.99') AS costoproducto,"+
						    "tiene_kit,tipo_unidad_producto,osiris_productos.porcentage_iva,cobro_activo " +
							sql_precios_clientes+				
							"FROM osiris_productos,osiris_grupo_producto,osiris_grupo1_producto,osiris_grupo2_producto "+
							"WHERE osiris_productos.id_grupo_producto = osiris_grupo_producto.id_grupo_producto "+
							"AND osiris_productos.id_grupo1_producto = osiris_grupo1_producto.id_grupo1_producto "+
							"AND osiris_productos.id_grupo2_producto = osiris_grupo2_producto.id_grupo2_producto " +
							//"AND precio_producto_10211 > 0 "+
							"AND osiris_productos.cobro_activo = 'true' ";
							
			if(checkbutton_grupo_export.Active != false || checkbutton_grupo1_export.Active != false || checkbutton_grupo2_export.Active != false){
				if(checkbutton_grupo_export.Active){
					query_sql_grupo = "AND osiris_productos.id_grupo_producto = '"+idtipogrupo_export.ToString().Trim()+"' ";
					//query_sql_grupo = "AND osiris_productos.id_grupo_producto IN ('4','5','6','7','10','11','12','13','14','15','16','17') ";
				}
				if(checkbutton_grupo1_export.Active){
					query_sql_grupo1 = "AND osiris_productos.id_grupo1_producto = '"+idtipogrupo1_export.ToString().Trim()+"' ";
				}
				if(checkbutton_grupo2_export.Active){
					query_sql_grupo2 = "AND osiris_productos.id_grupo2_producto2 = '"+idtipogrupo2_export.ToString().Trim()+"' ";
				}
			}
			string[] args_field_text = {""};
			string[] args_more_title = {""};
			string[,] args_formulas = {{"",""}};
			string[,] args_width = {{"0","2.54cm"},{"1","10cm"}};
			new osiris.class_traslate_spreadsheet(query_sql+query_sql_grupo+query_sql_grupo1+query_sql_grupo2+ordern_lista,args_names_field,args_type_field,false,args_field_text,"",false,args_more_title,args_formulas,args_width);	
		}
		
		// cierra ventanas emergentes
		void on_cierraventanas_clicked (object sender, EventArgs args)
		{
				Widget win = (Widget) sender;
				win.Toplevel.Destroy();
		}
	}
}