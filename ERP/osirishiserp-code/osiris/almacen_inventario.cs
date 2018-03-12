// created on 26/07/2007 at 03:48 p
///////////////////////////////////////////////////////////
// Sistema Hospitalario OSIRIS
// Monterrey - Mexico
//
// Autor    	: ing. Juan Antonio Peña Gonzalez (gjuanzz@gmail.com) 
// 				  Ing. Daniel Olivares C. (Programacion Base y Ajustes dic 2011)
//				 Actualizacion Marzo 2014 Limpeza de scrip
//				 Actualizacion Dic. 2016
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
using System.Data;
using Gtk;
using Glade;
using Gdk;

namespace osiris
{
	public class inventario_almacen
	{
		// Boton general para salir de las ventanas
		// Todas la ventanas en glade este boton debe estra declarado identico
		[Widget] Gtk.Button button_salir = null;
		
		// Para todas las busquedas este es el nombre asignado
		// se declara una vez
		[Widget] Gtk.Window busca_producto = null;
		[Widget] Gtk.Entry entry_expresion = null;
		[Widget] Gtk.Button button_selecciona = null;
		[Widget] Gtk.Button button_buscar_busqueda = null;
		
		[Widget] Gtk.Entry entry_lote = null;
		[Widget] Gtk.Entry entry_caducidad = null;
		
		// Declarando ventana principal
		[Widget] Gtk.Window inventario = null;
		[Widget] Gtk.Entry entry_id_almacen = null;
		[Widget] Gtk.Entry entry_almacen = null;
		[Widget] Gtk.Entry entry_ano_inventario = null;
		[Widget] Gtk.Entry entry_codigo_barras = null;
		[Widget] Gtk.Entry entry_cant_inventario = null;
		[Widget] Gtk.Button button_acepta_codbarra = null;
		[Widget] Gtk.Button button_actualiza_invvirtual = null;
		[Widget] Gtk.Button button_export_tosheet = null;
		[Widget] Gtk.ComboBox combobox_mes_inventario = null;
		[Widget] Gtk.ComboBox combobox_anaquel = null;
		[Widget] Gtk.TreeView lista_de_inventario = null;
				
		//[Widget] Gtk.ProgressBar progressbar_status_llenado;
		[Widget] Gtk.Button button_quitar_aplicados = null;
		[Widget] Gtk.Button button_actualizar = null;
		[Widget] Gtk.Button button_buscar_almacen = null;
		[Widget] Gtk.Button button_selec_id = null;
		[Widget] Gtk.Button button_graba_inventario = null;
		[Widget] Gtk.Button button_limpiar = null;
		[Widget] Gtk.Button button_busca_producto = null;
		[Widget] Gtk.Button button_reporte = null;
		
		//Declarando la barra de estado
		[Widget] Gtk.Statusbar statusbar_inventario = null;
		
		[Widget] Gtk.TreeView lista_de_producto = null;
		
		[Widget] Gtk.Entry entry_cantidad_aplicada = null;
		
		///////Ventana de Busqueda de almacenes
		[Widget] Gtk.TreeView lista_almacenes = null;
		
		private TreeStore treeViewEngineBusca;
		private TreeStore treeViewEngineBusca2;
		private TreeStore treeViewEngineInventario;
		
		//private ArrayList arraycargosrealizados;
		
		// Declaracion de variables publicas
		int idtipoalmacen = 0;	        			// Toma el valor de numero de atencion de paciente
		string almacen;
		string mesinventario= "00";
		int idanaquel = 1;
		string descripanaquel = "";
		string id_produ = "";
		string desc_produ = "";
		string costo_unitario_producto;
		string costo_producto;
		string embalaje;
		string precio_produ;
		string grupo_prod;
		string grupo1_prod;
		string grupo2_prod;
		string constante;
		
		//public float valor_descuento = 0;
		//Variables de admision
		bool copiaproductos = false;
		string tipobusqueda = "";
		
		string LoginEmpleado;
		string NomEmpleado; 
		string AppEmpleado; 
		string ApmEmpleado;
			
		string connectionString;
		string nombrebd;
		class_conexion conexion_a_DB = new class_conexion();
		class_buscador classfind_data = new class_buscador();
		CellRendererText cel_descripcion;

		string[] args_args = {""};
		string[] args_meses = {"","ENERO","FEBRERO","MARZO","ABRIL","MAYO","JUNIO","JULIO","AGOSTO","SEPTIEMBRE","OCTUBRE","NOVIEMBRE","DICIEMBRE"};
		int[] args_id_array = {0,1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20};
		
		//Declaracion de ventana de error
		protected Gtk.Window MyWinError;
		protected Gtk.Window MyWin;
				
		public inventario_almacen(string LoginEmp, string NomEmpleado_, string AppEmpleado_, string ApmEmpleado_, string nombrebd_ ) 
		{
			LoginEmpleado = LoginEmp;
			NomEmpleado = NomEmpleado_; 
			AppEmpleado = AppEmpleado_; 
			ApmEmpleado = ApmEmpleado_;
			connectionString = conexion_a_DB._url_servidor+conexion_a_DB._port_DB+conexion_a_DB._usuario_DB+conexion_a_DB._passwrd_user_DB;
			nombrebd = conexion_a_DB._nombrebd;
			
			//Console.WriteLine("antes de abrir venmtana");
			Glade.XML gxml = new Glade.XML (null,"almacen_costos_compras.glade","inventario",null);
			gxml.Autoconnect (this);
	        // Muestra ventana de Glade
			inventario.Show();
			
			button_selec_id.Clicked += new EventHandler(on_selec_id_clicked);
			entry_id_almacen.KeyPressEvent += onKeyPressEvent_enter_id;
			entry_codigo_barras.KeyPressEvent += onKeyPressEvent_codigobarras;
			button_acepta_codbarra.Clicked += new EventHandler(on_button_acepta_codbarra_clicked);
			button_actualizar.Clicked += new EventHandler(on_button_actualizar_clicked);
			button_buscar_almacen.Clicked += new EventHandler(on_button_buscar_almacen_clicked);
			button_limpiar.Clicked += new EventHandler(on_button_limpiar_clicked);
			button_busca_producto.Clicked += new EventHandler(on_button_busca_producto_clicked);
			button_quitar_aplicados.Clicked += new EventHandler(on_button_quitar_aplicados_clicked);
			button_graba_inventario.Clicked += new EventHandler(on_button_graba_inventario_clicked);
			button_reporte.Clicked += new EventHandler(on_button_reporte_clicked);
			button_actualiza_invvirtual.Clicked += new EventHandler(on_button_actualiza_invvirtual_clicked);
			button_export_tosheet.Clicked += new EventHandler(on_button_export_tosheet_clicked);
			button_salir.Clicked += new EventHandler(on_cierraventanas_clicked);

			llenado_combobox(0,"",combobox_mes_inventario,"array","","","",args_meses,args_id_array,"");
			llenado_combobox(0,"",combobox_anaquel,"sql","SELECT id_anaquel,activo,descripcion_corta || '-' || descripcion_anaquel AS descrip FROM osiris_erp_almacen_anaqueles WHERE activo = 'true' ORDER BY id_anaquel;","descrip","id_anaquel",args_args,args_id_array,"");

			crea_treeview_inventario();
			
			treeViewEngineInventario.Clear();

			//entry_id_cirugia.Sensitive = true;
			entry_almacen.Sensitive = false;
			//entry_ano_inventario.Sensitive = false;
			button_graba_inventario.Sensitive = true;
			button_reporte.Sensitive = true;
			button_quitar_aplicados.Sensitive = true;
			button_limpiar.Sensitive = true;
			lista_de_inventario.Sensitive = true;
			
			//combobox_mes_inventario.Sensitive = false;
			button_busca_producto.Sensitive = false;
			button_graba_inventario.Sensitive = false;
			button_reporte.Sensitive = false;
			button_quitar_aplicados.Sensitive = false;
			button_limpiar.Sensitive = false;
			button_actualiza_invvirtual.Sensitive = false;
			
			entry_ano_inventario.Text = DateTime.Now.ToString("yyyy");
			entry_id_almacen.Text = "0";
			
			entry_codigo_barras.GrabFocus();    // foco por default
			
			statusbar_inventario.Pop(0);
			statusbar_inventario.Push(1, "login: "+LoginEmpleado+"  |Usuario: "+NomEmpleado+" "+AppEmpleado+" "+ApmEmpleado);
			statusbar_inventario.HasResizeGrip = false;
	    	
			// pone color a los entry
			//entry_id_almacen.ModifyBase(StateType.Normal,new Gdk.Color(54,180,221));
			//Gdk.Color color1 = new Gdk.Color(54,180,221);
			//entry_id_almacen.ModifyBase(StateType.Normal, new Gdk.Color(54,180,221));
			//entry_almacen.ModifyBase(StateType.Normal, new Gdk.Color(254,253,152));
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
			if (sender == null){	return; }
			TreeIter iter;
			if (onComboBoxChanged.GetActiveIter (out iter)){
				switch (onComboBoxChanged.Name.ToString()){	
				case "combobox_mes_inventario":
					mesinventario = int.Parse(onComboBoxChanged.Model.GetValue(iter,1).ToString().Trim()).ToString("00");
					llenado_de_material_del_stock(idtipoalmacen.ToString(), entry_ano_inventario.Text.Trim(),mesinventario);
					break;
				case "combobox_anaquel":
					descripanaquel = onComboBoxChanged.Model.GetValue (iter, 0).ToString ().Trim ();
					idanaquel = int.Parse (onComboBoxChanged.Model.GetValue (iter, 1).ToString ().Trim ());
					break;
				}
			}
		}
						
		void on_selec_id_clicked(object sender, EventArgs args)
		{
			if(entry_id_almacen.Text  == "" || entry_id_almacen.Text == " "){
				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
				MessageType.Error,ButtonsType.Close, 
				"Debe de llenar el campo de id almacen con uno \n"+"existente para que los datos se muestren \n");
				msgBoxError.Run ();			msgBoxError.Destroy();
			}else{
				//Console.WriteLine ("on_selec_id selecciono el ID");
				idtipoalmacen = int.Parse(entry_id_almacen.Text.ToString());
				llenado_de_material_del_stock(entry_id_almacen.Text.ToString(),entry_ano_inventario.Text.Trim(),mesinventario.ToString().Trim());
				
			}
		}
		
		// Valida entradas que solo sean numericas, se utiliza la ventana de
		// carga de almacen
		[GLib.ConnectBefore ()]   	  // Esto es indispensable para que funcione    
		void onKeyPressEvent_enter_id(object o, Gtk.KeyPressEventArgs args)
		{
			//Console.WriteLine(args.Event.Key);
			if (args.Event.Key.ToString() == "Return" || args.Event.Key.ToString() == "KP_Enter"){
				args.RetVal = true;
				if(entry_id_almacen.Text.Trim()  == "" ){
					MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
					MessageType.Error,ButtonsType.Close, 
					"Debe de llenar el campo de id almacen con uno \n"+"existente para que los datos se muestren ");
					msgBoxError.Run ();			msgBoxError.Destroy();
				}else{
					//Console.WriteLine ("actualiza selecciono el ID");
					idtipoalmacen = int.Parse(entry_id_almacen.Text.ToString());
					//tipo_de_inventario();
					llenado_de_material_del_stock(entry_id_almacen.Text.ToString(),entry_ano_inventario.Text.Trim(),mesinventario);
				}			
			}
			string misDigitos = ".0123456789ﾰﾱﾲﾳﾴﾵﾶﾷﾸﾹﾮｔｒｓｑ（）";
			if (Array.IndexOf(misDigitos.ToCharArray(), Convert.ToChar(args.Event.Key)) == -1 && args.Event.Key.ToString()  != "BackSpace")
			{
				//Console.WriteLine(Convert.ToChar(args.Event.Key));
				args.RetVal = true;
			}
		}
		// Valida entradas que solo sean numericas, se utiliza la ventana de
		// carga de almacen
		[GLib.ConnectBefore ()]   	  // Esto es indispensable para que funcione  
		void onKeyPressEvent_codigobarras(object o, Gtk.KeyPressEventArgs args)
		{
			if (idanaquel > 1) {
				if (args.Event.Key.ToString () == "Return" || args.Event.Key.ToString () == "KP_Enter") {
					//Console.WriteLine("hizo el enter");
					NpgsqlConnection conexion; 
					conexion = new NpgsqlConnection (connectionString + nombrebd);
				            
					// Verifica que la base de datos este conectada
					try {
						conexion.Open ();
						NpgsqlCommand comando; 
						comando = conexion.CreateCommand ();
					
						comando.CommandText = "SELECT to_char(osiris_productos.id_producto,'999999999999') AS codProducto," +
						"osiris_productos.descripcion_producto," +
						"descripcion_grupo_producto,descripcion_grupo1_producto,descripcion_grupo2_producto," +
						"to_char(precio_producto_publico,'99999999.99') AS preciopublico," +
						"to_char(costo_por_unidad,'999999999.99') AS costoproductounitario," +
						"to_char(costo_producto,'999999999.99') AS costoproducto," +
						"to_char(cantidad_de_embalaje,'999999999.99') AS embalaje," +
						"id_codigo_barras,marca,presentacion " +
						"FROM osiris_productos,osiris_grupo_producto,osiris_grupo1_producto,osiris_grupo2_producto,osiris_erp_codigos_barra " +
						"WHERE osiris_productos.id_grupo_producto = osiris_grupo_producto.id_grupo_producto " +
						"AND osiris_productos.id_grupo1_producto = osiris_grupo1_producto.id_grupo1_producto " +
						"AND osiris_productos.id_grupo2_producto = osiris_grupo2_producto.id_grupo2_producto " +
						"AND osiris_erp_codigos_barra.id_producto = osiris_productos.id_producto " +
						"AND osiris_productos.cobro_activo = 'true' " +
						"AND id_codigo_barras = '" + entry_codigo_barras.Text.Trim () + "';";
											
						//Console.WriteLine(comando.CommandText.ToString());
						NpgsqlDataReader lector = comando.ExecuteReader ();				
						if (lector.Read ()) {
							entry_codigo_barras.Text = "";
							entry_codigo_barras.GrabFocus ();
							treeViewEngineInventario.AppendValues (
								lector ["codProducto"].ToString ().Trim (),
								lector ["descripcion_producto"].ToString ().Trim (),
								entry_cant_inventario.Text.Trim (),
								(string)lector ["costoproductounitario"],
								(string)lector ["costoproducto"],
								(string)lector ["embalaje"],
								(string)lector ["preciopublico"],
								(string)lector ["descripcion_grupo_producto"],
								(string)lector ["descripcion_grupo1_producto"],
								(string)lector ["descripcion_grupo2_producto"],
								(string)LoginEmpleado,
								DateTime.Now.ToString ("yyyy-MM-dd HH:mm:ss"),	
								false,
								"0",
								(string)entry_lote.Text.Trim (),
								(string)entry_caducidad.Text.Trim (),
								(string)lector ["id_codigo_barras"],
								(string)lector ["marca"],
								descripanaquel,
								idanaquel.ToString().Trim());							
						} else {
							// llamr a clase para el enlase del codigo de osiris
						}
					} catch (NpgsqlException ex) {
						Console.WriteLine ("PostgresSQL error: {0}", ex.Message);
						MessageDialog msgBoxError = new MessageDialog (MyWinError, DialogFlags.DestroyWithParent,
							                            MessageType.Warning, ButtonsType.Ok, "PostgresSQL error: {0}", ex.Message);
						msgBoxError.Run ();						msgBoxError.Destroy ();
					}
					conexion.Close ();				
				}
			} else {
				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
					MessageType.Error,ButtonsType.Close,"Debe selecciona un Anaquel, verifique...");
				msgBoxError.Run ();					msgBoxError.Destroy();	
			}
		}

		void on_button_acepta_codbarra_clicked(object sender, EventArgs args)
		{
			//Console.WriteLine("hizo el enter");
			NpgsqlConnection conexion; 
			conexion = new NpgsqlConnection (connectionString+nombrebd);

			// Verifica que la base de datos este conectada
			try{
				conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand ();

				comando.CommandText = "SELECT to_char(osiris_productos.id_producto,'999999999999') AS codProducto,"+
					"osiris_productos.descripcion_producto,"+
					"descripcion_grupo_producto,descripcion_grupo1_producto,descripcion_grupo2_producto,"+
					"to_char(precio_producto_publico,'99999999.99') AS preciopublico,"+
					"to_char(costo_por_unidad,'999999999.99') AS costoproductounitario,"+
					"to_char(costo_producto,'999999999.99') AS costoproducto,"+
					"to_char(cantidad_de_embalaje,'999999999.99') AS embalaje,"+
					"id_codigo_barras,marca,presentacion "+
					"FROM osiris_productos,osiris_grupo_producto,osiris_grupo1_producto,osiris_grupo2_producto,osiris_erp_codigos_barra "+
					"WHERE osiris_productos.id_grupo_producto = osiris_grupo_producto.id_grupo_producto "+
					"AND osiris_productos.id_grupo1_producto = osiris_grupo1_producto.id_grupo1_producto "+
					"AND osiris_productos.id_grupo2_producto = osiris_grupo2_producto.id_grupo2_producto "+
					"AND osiris_erp_codigos_barra.id_producto = osiris_productos.id_producto "+
					"AND osiris_productos.cobro_activo = 'true' "+
					"AND id_codigo_barras = '"+entry_codigo_barras.Text.Trim()+"';";

				//Console.WriteLine(comando.CommandText.ToString());
				NpgsqlDataReader lector = comando.ExecuteReader ();				
				if(lector.Read()){
					entry_codigo_barras.Text = "";
					entry_codigo_barras.GrabFocus();
					treeViewEngineInventario.AppendValues(
						(string) lector["codProducto"].ToString().Trim(),
						(string) lector["descripcion_producto"].ToString().Trim(),
						entry_cant_inventario.Text.Trim(),
						(string) lector["costoproductounitario"],
						(string) lector["costoproducto"],
						(string) lector["embalaje"],
						(string) lector["preciopublico"],
						(string) lector["descripcion_grupo_producto"],
						(string) lector["descripcion_grupo1_producto"],
						(string) lector["descripcion_grupo2_producto"],
						(string) LoginEmpleado,
						(string) DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),	
						false,
						"0",
						(string) entry_lote.Text.Trim(),
						(string) entry_caducidad.Text.Trim(),
						(string) lector["id_codigo_barras"],
						(string) lector["marca"],
						descripanaquel,
						idanaquel.ToString().Trim());							
				}else{
					// llamr a clase para el enlase del codigo de osiris
				}
			}catch (NpgsqlException ex) { Console.WriteLine ("PostgresSQL error: {0}",ex.Message);
				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
					MessageType.Warning, ButtonsType.Ok, "PostgresSQL error: {0}",ex.Message);
				msgBoxError.Run ();
				msgBoxError.Destroy();
			}
		conexion.Close ();
		}
		
		void on_button_actualizar_clicked(object sender, EventArgs args)
		{
			llenado_de_material_del_stock(entry_id_almacen.Text.ToString(),entry_ano_inventario.Text.Trim(),mesinventario);
		}
														
		void llenado_de_material_del_stock(string idalmacen_,string anoinventario_,string mesinvenario_)
		{	
			llenado_de_almacen(idalmacen_);
			treeViewEngineInventario.Clear();
			
			NpgsqlConnection conexion; 
			conexion = new NpgsqlConnection (connectionString+nombrebd);
			            
			// Verifica que la base de datos este conectada
			try{
				conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand ();
				
				comando.CommandText = "SELECT descripcion_producto, "+
							"id_quien_creo,osiris_productos.aplicar_iva,osiris_inventario_almacenes.id_almacen,  "+
							"descripcion_grupo_producto,descripcion_grupo1_producto,descripcion_grupo2_producto, "+
							"to_char(osiris_inventario_almacenes.id_producto,'999999999999') AS idproducto,"+
							"to_char(osiris_inventario_almacenes.stock,'999999.99') AS stock, "+
							"to_char(osiris_inventario_almacenes.id_secuencia,'999999999999') AS idsecuencia,"+
							"eliminado,lote,caducidad,codigo_barras,"+
							//"to_char(osiris_catalogo_almacenes."+mesinventario.ToString()+",'99999.99') AS stock, "+
							"to_char(osiris_productos.costo_por_unidad,'999999999.99') AS costoproductounitario, "+
							"to_char(osiris_productos.costo_producto,'999999999.99') AS costoproducto, "+
							"to_char(osiris_productos.precio_producto_publico,'99999999.99') AS preciopublico,"+
							"to_char(osiris_productos.cantidad_de_embalaje,'999999999.99') AS embalaje, "+
							"to_char(osiris_inventario_almacenes.fechahora_alta,'dd-MM-yyyy HH:mi:ss') AS fechcreacion,osiris_inventario_almacenes.id_anaquel,descripcion_corta || '-' || descripcion_anaquel AS descripanaqel "+
							"FROM "+
							"osiris_inventario_almacenes,osiris_productos,osiris_almacenes,osiris_grupo_producto,osiris_grupo1_producto,osiris_grupo2_producto,osiris_erp_almacen_anaqueles "+
							"WHERE osiris_inventario_almacenes.id_producto = osiris_productos.id_producto "+ 
							"AND osiris_productos.id_grupo_producto = osiris_grupo_producto.id_grupo_producto "+
							"AND osiris_productos.id_grupo1_producto = osiris_grupo1_producto.id_grupo1_producto "+
							"AND osiris_productos.id_grupo2_producto = osiris_grupo2_producto.id_grupo2_producto "+
							"AND osiris_inventario_almacenes.id_almacen = osiris_almacenes.id_almacen "+
							"AND osiris_inventario_almacenes.id_anaquel = osiris_erp_almacen_anaqueles.id_anaquel "+
							"AND osiris_inventario_almacenes.id_almacen = '"+(string) idalmacen_ +"' "+
							"AND osiris_inventario_almacenes.ano_inventario = '"+anoinventario_+"' "+
							"AND osiris_inventario_almacenes.mes_inventario = '"+mesinvenario_+"' "+
							"AND eliminado = 'false' "+
							"ORDER BY to_char(osiris_inventario_almacenes.fechahora_alta,'dd-MM-yyyy HH:mi:ss');";
				Console.WriteLine(comando.CommandText.ToString());
				NpgsqlDataReader lector = comando.ExecuteReader ();				
				while (lector.Read()){
					//if(float.Parse((string) lector["stock"]) != 0){
						treeViewEngineInventario.AppendValues (
							(string) lector["idproducto"],
							(string) lector["descripcion_producto"],
							(string) lector["stock"],
							(string) lector["costoproductounitario"],
							(string) lector["costoproducto"],
							(string) lector["embalaje"],
							(string) lector["preciopublico"],
							(string) lector["descripcion_grupo_producto"],
							(string) lector["descripcion_grupo1_producto"],
							(string) lector["descripcion_grupo2_producto"],
							(string) lector["fechcreacion"],
							(string) lector["id_quien_creo"],
							true,
							(string) lector["idsecuencia"],
							(string) lector["lote"],
							(string) lector["caducidad"],
							(string) lector["codigo_barras"],
							"marca",
							lector["descripanaqel"].ToString().Trim(),
							lector["id_anaquel"].ToString().Trim());
					
				}
				button_busca_producto.Sensitive = true;
				// Realizando las restas
			}catch (NpgsqlException ex) { Console.WriteLine ("PostgresSQL error: {0}",ex.Message);
				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
								MessageType.Warning, ButtonsType.Ok, "PostgresSQL error: {0}",ex.Message);
								msgBoxError.Run ();
								msgBoxError.Destroy();
			}
			conexion.Close ();
		}
		
		void on_button_buscar_almacen_clicked(object sender, EventArgs args)
		{
			
			// Los parametros de del SQL siempre es primero cuando busca todo y la otra por expresion
			// la clase recibe tambien el orden del query
			// es importante definir que tipo de busqueda es para que los objetos caigan ahi mismo
			object[] parametros_objetos = {entry_id_almacen,entry_almacen};
			string[] parametros_sql = {"SELECT * FROM osiris_almacenes "};
			string[] parametros_string = {};
			string[,] args_buscador1 = {{"ALMACEN","WHERE descripcion_almacen LIKE '%","%' "},
										{"ID ALMACEN","WHERE id_almacen = '","' "}};
			string[,] args_buscador2 = {{"ID ALMACEN","WHERE id_almacen = '","' "},
										{"ALMACEN","WHERE descripcion_almacen LIKE '%","%' "}};
			string[,] args_orderby = {{"",""}};
			classfind_data.buscandor(parametros_objetos,parametros_sql,parametros_string,"find_almacen_inventario",0,args_buscador1,args_buscador2,args_orderby);
		}
		
		void llenado_de_almacen(string idalmacen)
		{
			treeViewEngineInventario.Clear();
			//entry_id_cirugia.Sensitive = true;
			entry_almacen.Sensitive = true;
			entry_ano_inventario.Sensitive = true;
			entry_ano_inventario.GrabFocus();
			combobox_mes_inventario.Sensitive = true;
			button_graba_inventario.Sensitive = true;
			button_reporte.Sensitive = true;
			button_quitar_aplicados.Sensitive = true;
			button_limpiar.Sensitive = true;
			lista_de_inventario.Sensitive = true;
			button_actualiza_invvirtual.Sensitive = true;
			button_busca_producto.Sensitive = true;
			
			NpgsqlConnection conexion;
			conexion = new NpgsqlConnection (connectionString+nombrebd);
	           
			try{
				conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand ();
	              	
				comando.CommandText = "SELECT * "+
									"FROM osiris_almacenes  "+
					            	"WHERE id_almacen = '"+idalmacen.ToString()+"' ;";
				//Console.WriteLine("query llenado cirugia: "+comando.CommandText.ToString());				
				NpgsqlDataReader lector = comando.ExecuteReader ();
				
				if(lector.Read()){
					idtipoalmacen = int.Parse(idalmacen);
					entry_almacen.Text = (string) lector["descripcion_almacen"];
				}
			}catch (NpgsqlException ex){
		   		Console.WriteLine ("PostgresSQL error: {0}",ex.Message);
		   		MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
							MessageType.Error, ButtonsType.Close, "PostgresSQL error: {0} ",ex.Message);
				msgBoxError.Run (); 	msgBoxError.Destroy();
		    }
	       	conexion.Close ();
       	}
		
		void on_button_limpiar_clicked(object sender, EventArgs args)
		{
			//limpia_valores();
			entry_id_almacen.Text = "";
			entry_almacen.Text = "";
			entry_ano_inventario.Text = "";
			button_reporte.Sensitive = false;
			treeViewEngineInventario.Clear();
			combobox_mes_inventario.Sensitive = false;
			button_busca_producto.Sensitive = false;
			button_graba_inventario.Sensitive = false;
			button_reporte.Sensitive = false;
			button_quitar_aplicados.Sensitive = false;
			button_actualiza_invvirtual.Sensitive = false;
			button_limpiar.Sensitive = false;
		}
		
		void on_button_busca_producto_clicked (object sender, EventArgs args)
		{
			Glade.XML gxml = new Glade.XML (null, "almacen_costos_compras.glade", "busca_producto", null);
			gxml.Autoconnect (this);
			crea_treeview_busqueda();
			entry_expresion.KeyPressEvent += onKeyPressEvent_enter_expresion;
			button_buscar_busqueda.Clicked += new EventHandler(on_llena_lista_producto_clicked);
			button_selecciona.Clicked += new EventHandler(on_selecciona_producto_clicked);
			button_salir.Clicked += new EventHandler(on_cierraventanas_clicked);
			entry_cantidad_aplicada.KeyPressEvent += onKeyPressEvent;
		}
		
		[GLib.ConnectBefore ()]   	  // Esto es indispensable para que funcione    
		void onKeyPressEvent_enter_expresion(object o, Gtk.KeyPressEventArgs args)
		{
			//Console.WriteLine(args.Event.Key);
			if (args.Event.Key.ToString() == "Return" || args.Event.Key.ToString() == "KP_Enter"){
				args.RetVal = true;
				llenando_busqueda_productos();
			}
		}
		
		// Valida entradas que solo sean numericas, se utiliza eb ventana de carga de producto
		[GLib.ConnectBefore ()]   	  // Esto es indispensable para que funcione    
		public void onKeyPressEvent(object o, Gtk.KeyPressEventArgs args)
		{
			//Console.WriteLine(args.Event.Key);
			//Console.WriteLine(Convert.ToChar(args.Event.Key));
			string misDigitos = "-.0123456789ﾰﾱﾲﾳﾴﾵﾶﾷﾸﾹﾮｔｒｓｑ（）";
			if (Array.IndexOf(misDigitos.ToCharArray(), Convert.ToChar(args.Event.Key)) == -1 && args.Event.Key.ToString()  != "BackSpace"){
				args.RetVal = true;
			}
		}
		
		void on_llena_lista_producto_clicked (object sender, EventArgs args)
 		{
 			llenando_busqueda_productos();
 		}
 			
 		void llenando_busqueda_productos()
 		{
 			treeViewEngineBusca2.Clear(); // Limpia el treeview cuando realiza una nueva busqueda
			
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
							"to_char(costo_producto,'999999999.99') AS costoproducto, "+
							"to_char(cantidad_de_embalaje,'999999999.99') AS embalaje "+
							"FROM osiris_productos,osiris_grupo_producto,osiris_grupo1_producto,osiris_grupo2_producto "+
							"WHERE osiris_productos.id_grupo_producto = osiris_grupo_producto.id_grupo_producto "+
							"AND osiris_productos.id_grupo1_producto = osiris_grupo1_producto.id_grupo1_producto "+
							"AND osiris_productos.id_grupo2_producto = osiris_grupo2_producto.id_grupo2_producto "+
							"AND osiris_grupo_producto.agrupacion4 = 'ALM' "+
							"AND osiris_productos.cobro_activo = 'true' "+
							"AND osiris_productos.descripcion_producto LIKE '%"+entry_expresion.Text.ToUpper().Trim()+"%' ORDER BY descripcion_producto; ";
				
				NpgsqlDataReader lector = comando.ExecuteReader ();
				while (lector.Read()){
					treeViewEngineBusca2.AppendValues ((string) lector["codProducto"] , 
														(string) lector["descripcion_producto"],
														//stock
														(string) lector["costoproductounitario"],
														(string) lector["costoproducto"],
														(string) lector["embalaje"],
														(string) lector["preciopublico"],
														(string) lector["descripcion_grupo_producto"],
														(string) lector["descripcion_grupo1_producto"],
														(string) lector["descripcion_grupo2_producto"]);
				}
			}catch (NpgsqlException ex) { Console.WriteLine ("PostgresSQL error: {0}",ex.Message); }
			conexion.Close ();
		}
		
		void on_selecciona_producto_clicked (object sender, EventArgs args)
		{
			TreeModel model;
			TreeIter iterSelected;

 			if (lista_de_producto.Selection.GetSelected(out model, out iterSelected)){
 				//Console.WriteLine("pasavalor");
 				id_produ = (string) model.GetValue(iterSelected, 0);
				desc_produ = (string) model.GetValue(iterSelected, 1);
				costo_unitario_producto = (string) model.GetValue(iterSelected, 2);
				costo_producto = (string) model.GetValue(iterSelected, 3);
				embalaje = (string) model.GetValue(iterSelected, 4);
				precio_produ  = (string) model.GetValue(iterSelected, 5);
				grupo_prod = (string) model.GetValue(iterSelected, 6);
				grupo1_prod = (string) model.GetValue(iterSelected, 7);
				grupo2_prod = (string) model.GetValue(iterSelected,8);
				constante = entry_cantidad_aplicada.Text;
					
				if ((float) float.Parse(constante) > 0){
					treeViewEngineInventario.AppendValues (
												(string) id_produ,
												(string) desc_produ,
												(string) constante,
												(string) costo_unitario_producto,
												(string) costo_producto,
												(string) embalaje,
												(string) precio_produ,
												(string) grupo_prod,
												(string) grupo1_prod,
												(string) grupo2_prod,
												(string) DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
												LoginEmpleado.Trim(),
												false,
												(string) "0",
					                            (string) entry_lote.Text.Trim(),
					                            (string) entry_caducidad.Text.Trim(),
					                            "",
												"",
												descripanaquel,
												idanaquel.ToString().Trim());
					entry_cantidad_aplicada.Text = "0";
					entry_expresion.Text = "";
					entry_expresion.GrabFocus();
				}else{
					MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
												MessageType.Error,ButtonsType.Close, 
											"La cantidad que quiere aplicar debe ser \n"+"distinta a cero, intente de nuevo");
					msgBoxError.Run ();					msgBoxError.Destroy();
				}
			}
		}
		
		void guarda_productos()
		{
			string[,] parametros;
			object[] paraobj;
			TreeIter iter;
			if ((int) idtipoalmacen > 0){
				if (treeViewEngineInventario.GetIterFirst (out iter)){
					if ((bool)lista_de_inventario.Model.GetValue (iter, 12) == false) {
						parametros = new string[,] {
							{ "id_producto,", "'" + lista_de_inventario.Model.GetValue (iter, 0).ToString ().Trim () + "'," },
							{ "id_almacen,", "'" + idtipoalmacen + "'," },
							{ "stock,", "'" + lista_de_inventario.Model.GetValue (iter, 2).ToString ().Trim () + "'," },
							{ "mes_inventario,", "'" + mesinventario + "'," },
							{ "ano_inventario,", "'" + entry_ano_inventario.Text + "'," },
							{ "fechahora_alta,", "'" + DateTime.Now.ToString ("yyyy-MM-dd HH:mm:ss") + "'," },
							{ "lote,", "'" + lista_de_inventario.Model.GetValue (iter, 14).ToString ().Trim () + "'," },
							{ "caducidad,", "'" + lista_de_inventario.Model.GetValue (iter, 15).ToString ().Trim () + "'," },
							{ "codigo_barras,", "'" + lista_de_inventario.Model.GetValue (iter, 16).ToString ().Trim () + "'," },
							{ "id_quien_creo", "'" + LoginEmpleado + "' " }
						};
						paraobj = new object[] { entry_id_almacen };
						new osiris.insert_registro ("osiris_inventario_almacenes", parametros, paraobj);
					}
					while (treeViewEngineInventario.IterNext (ref iter)) {
						if ((bool)lista_de_inventario.Model.GetValue (iter, 12) == false) {
							parametros = new string[,] {
								{ "id_producto,", "'" + lista_de_inventario.Model.GetValue (iter, 0).ToString ().Trim () + "'," },
								{ "id_almacen,", "'" + idtipoalmacen + "'," },
								{ "stock,", "'" + lista_de_inventario.Model.GetValue (iter, 2).ToString ().Trim () + "'," },
								{ "mes_inventario,", "'" + mesinventario + "'," },
								{ "ano_inventario,", "'" + entry_ano_inventario.Text + "'," },
								{ "fechahora_alta,", "'" + DateTime.Now.ToString ("yyyy-MM-dd HH:mm:ss") + "'," },
								{ "lote,", "'" + lista_de_inventario.Model.GetValue (iter, 14).ToString ().Trim () + "'," },
								{ "caducidad,", "'" + lista_de_inventario.Model.GetValue (iter, 15).ToString ().Trim () + "'," },
								{ "codigo_barras,", "'" + lista_de_inventario.Model.GetValue (iter, 16).ToString ().Trim () + "'," },
								{ "id_quien_creo", "'" + LoginEmpleado + "' " }
							};
							paraobj = new object[] { entry_id_almacen };
							new osiris.insert_registro ("osiris_inventario_almacenes", parametros, paraobj);
						}
					}							
					MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,MessageType.Info,ButtonsType.Close, "Los datos se guardaron con EXITO");
					msgBoxError.Run ();			msgBoxError.Destroy();
					entry_id_almacen.Text = idtipoalmacen.ToString();
					llenado_de_material_del_stock(entry_id_almacen.Text, entry_ano_inventario.Text.Trim(),mesinventario);
				}
			}
		}
		
		void on_button_quitar_aplicados_clicked(object sender, EventArgs args)
		{
			TreeIter iter;
 			TreeModel model;
			string[,] parametros;
			object[] paraobj;
 			string prodeliminado;
 			if (lista_de_inventario.Selection.GetSelected (out model, out iter)) {
 				if (!(bool) lista_de_inventario.Model.GetValue (iter,12)){					
					treeViewEngineInventario.Remove (ref iter);					
 				}else{
 					prodeliminado = (string) lista_de_inventario.Model.GetValue (iter,1);
 					//if (LoginEmpleado =="DOLIVARES" || LoginEmpleado =="ADMIN" ){
 						MessageDialog msgBox = new MessageDialog (MyWin,DialogFlags.Modal,
						MessageType.Question,ButtonsType.YesNo,"¿ Desea eliminar "+prodeliminado+" del paquete este producto ?");
						ResponseType miResultado = (ResponseType) msgBox.Run ();
						msgBox.Destroy();	 			
	 					if (miResultado == ResponseType.Yes){
							parametros = new string[,] {
								{ "eliminado = '","true'," },
								{ "id_quien_elimino = '",LoginEmpleado+"'," },
								{ "fechahora_eliminado = '",DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"' " },
							{ "WHERE id_secuencia = '",lista_de_inventario.Model.GetValue (iter,13).ToString().Trim()+"';" }
							};
							paraobj = new object[] { entry_id_almacen };
							new osiris.update_registro ("osiris_inventario_almacenes", parametros, paraobj);
							llenado_de_material_del_stock(entry_id_almacen.Text, entry_ano_inventario.Text.Trim(),mesinventario);

 						}
 					/*}else{
 						MessageDialog msgBox = new MessageDialog (MyWin,DialogFlags.Modal,
						MessageType.Error,ButtonsType.Ok,"No esta autorizado para esta opcion...");
						msgBox.Run();					msgBox.Destroy();
 					}*/
 				}
 			}
		}
		
		
		void on_button_graba_inventario_clicked(object sender, EventArgs args)
		{
			if(entry_almacen.Text.Trim() == "" || entry_ano_inventario.Text == "" || this.mesinventario == "" ||
			   entry_id_almacen.Text.Trim()  == "" || entry_ano_inventario.Text == "") {
				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,MessageType.Info,
											ButtonsType.Close,"verifique que no existan campos en blanco");
				msgBoxError.Run ();					msgBoxError.Destroy();
			}else{
				MessageDialog msgBox = new MessageDialog (MyWin,DialogFlags.Modal,
									MessageType.Question,ButtonsType.YesNo,
									"¿ Desea grabar o actualizar esta infomacion ?");
				ResponseType miResultado = (ResponseType)
				msgBox.Run ();			msgBox.Destroy();
 				if (miResultado == ResponseType.Yes){
					guarda_productos();					
				}else{
	 				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,MessageType.Info,
	 											ButtonsType.Close, "No grabo, ya que NO CARGO NADA");
					msgBoxError.Run ();			msgBoxError.Destroy();
	 			}
	 		}
		}		
			
		void on_button_reporte_clicked(object sender, EventArgs args)
		{
			if ((string) this.entry_id_almacen.Text.Trim() == "" ){	
				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
				MessageType.Error, 
				ButtonsType.Close, "Debe de llenar el campo de almacen con uno \n"+
							"existente para que el  se muestre \n"+"o no a pulsado el boton ''Seleccionar''");
				msgBoxError.Run ();
				msgBoxError.Destroy();
			}else{
				new osiris.inventario_almacen_reporte (int.Parse(entry_id_almacen.Text.ToString()),entry_almacen.Text,mesinventario,entry_ano_inventario.Text,
													LoginEmpleado,NomEmpleado,AppEmpleado,ApmEmpleado,nombrebd,"inventario_fisico","","","","","","","");
			}
		}

		void on_button_actualiza_invvirtual_clicked(object sender, EventArgs args)
		{
			string[,] parametros;
			object[] paraobj;
			MessageDialog msgBox = new MessageDialog (MyWin,DialogFlags.Modal,MessageType.Warning,ButtonsType.YesNo,
				"¿ Esta Seguro de actualizar el INVENTARIO FISICO al VIRTUAL ? \n"+
				"Una vez aceptado se no podra revertir esta actualizacion.");
			ResponseType miResultado = (ResponseType)
				msgBox.Run ();			msgBox.Destroy();
			if (miResultado == ResponseType.Yes) {

				NpgsqlConnection conexion; 
				conexion = new NpgsqlConnection (connectionString+nombrebd);

				// Verifica que la base de datos este conectada
				try{
					conexion.Open ();
					NpgsqlCommand comando; 
					comando = conexion.CreateCommand ();

					comando.CommandText = "SELECT SUM(stock) AS totalstock,osiris_inventario_almacenes.id_producto,descripcion_producto " +
						"FROM osiris_inventario_almacenes,osiris_almacenes,osiris_productos,osiris_grupo_producto,osiris_grupo1_producto,osiris_grupo2_producto " +
						"WHERE osiris_inventario_almacenes.id_producto = osiris_productos.id_producto " +
						"AND osiris_productos.id_grupo_producto = osiris_grupo_producto.id_grupo_producto " +
						"AND osiris_productos.id_grupo1_producto = osiris_grupo1_producto.id_grupo1_producto " +
						"AND osiris_productos.id_grupo2_producto = osiris_grupo2_producto.id_grupo2_producto " +
						"AND osiris_inventario_almacenes.id_almacen = osiris_almacenes.id_almacen " +
						"AND osiris_inventario_almacenes.id_almacen = '1' " +
						"AND osiris_inventario_almacenes.ano_inventario = '2016' " +
						"AND osiris_inventario_almacenes.mes_inventario = '12' " +
						"AND eliminado = 'false' " +
						"GROUP BY osiris_inventario_almacenes.id_producto,descripcion_producto " +
						"ORDER BY totalstock DESC";
					Console.WriteLine(comando.CommandText.ToString());
					NpgsqlDataReader lector = comando.ExecuteReader ();				
					while (lector.Read()){						
						/*parametros = new string[,] {
							{ "historial_ajustes = historial_ajustes || 'AJUSTE POR INV. FISICO;",LoginEmpleado.Trim () + ";" + DateTime.Now.ToString ("yyyy-MM-dd HH:mm:ss").Trim ()+"\n',"},
							{ "stock = '",lector["totalstock"].ToString ().Trim ()+"' " },
							{ "WHERE id_almacen = '","1' "},
							{ "AND eliminado = '","false' "},
							{ "AND id_producto = '",lector["id_producto"].ToString ().Trim ()+"' "}
						};
						paraobj = new object[] { entry_id_almacen};
						new osiris.update_registro ("osiris_catalogo_almacenes", parametros, paraobj);*/
					}
					button_busca_producto.Sensitive = true;
					// Realizando las restas
				}catch (NpgsqlException ex) { Console.WriteLine ("PostgresSQL error: {0}",ex.Message);
					MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
						MessageType.Warning, ButtonsType.Ok, "PostgresSQL error: {0}",ex.Message);
					msgBoxError.Run ();
					msgBoxError.Destroy();
				}
				conexion.Close ();


				
			}
		}

		void on_button_export_tosheet_clicked(object sender, EventArgs args)
		{
			string consulta_sql = "SELECT SUM(stock) AS totalstock," +
				"to_char(osiris_productos.id_producto,'999999999999') AS idproducto,"+
				"descripcion_producto,descripcion_grupo_producto,"+
				"to_char(precio_producto_publico,'99999999.99') AS preciopublico,"+
				"to_char(costo_por_unidad,'999999999.99') AS costoproductounitario, "+
				"to_char(costo_producto,'999999999.99') AS costoproducto, "+
				"to_char(cantidad_de_embalaje,'999999999.99') AS embalaje "+
				"FROM osiris_inventario_almacenes,osiris_almacenes,osiris_productos,osiris_grupo_producto,osiris_grupo1_producto,osiris_grupo2_producto " +
				"WHERE osiris_inventario_almacenes.id_producto = osiris_productos.id_producto " +
				"AND osiris_productos.id_grupo_producto = osiris_grupo_producto.id_grupo_producto " +
				"AND osiris_productos.id_grupo1_producto = osiris_grupo1_producto.id_grupo1_producto " +
				"AND osiris_productos.id_grupo2_producto = osiris_grupo2_producto.id_grupo2_producto " +
				"AND osiris_inventario_almacenes.id_almacen = osiris_almacenes.id_almacen " +
				"AND osiris_inventario_almacenes.id_almacen = '"+idtipoalmacen+"' " +
				"AND osiris_inventario_almacenes.ano_inventario = '"+entry_ano_inventario.Text+"' " +
				"AND osiris_inventario_almacenes.mes_inventario = '"+mesinventario+"' " +
				"AND eliminado = 'false' " +
				"GROUP BY idproducto,descripcion_producto,descripcion_grupo_producto,preciopublico,costoproducto,costoproductounitario,embalaje " +
				"ORDER BY totalstock DESC";
			string[] args_names_field = {"idproducto","descripcion_producto","totalstock","descripcion_grupo_producto","preciopublico","costoproducto","costoproductounitario","embalaje"};
			string[] args_type_field = {"string","string","float","string","float","float","float","float"};
			string[] args_field_text = {""};
			string[] args_more_title = {""};
			string[,] args_formulas = {{"",""}};
			string[,] args_width = {{"1","8cm"},{"3","4cm"}};
			new osiris.class_traslate_spreadsheet(consulta_sql,args_names_field,args_type_field,false,args_field_text,"",false,args_more_title,args_formulas,args_width);

		}
				
		void crea_treeview_inventario()
		{
			treeViewEngineInventario = new TreeStore(typeof(string),
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
													typeof(string),
			                                        typeof(string),
			                                        typeof(string),
			                                        typeof(string),
			                                        typeof(string),
													typeof(string),
													typeof(string));
			lista_de_inventario.Model = treeViewEngineInventario;
			lista_de_inventario.RulesHint = true;
				
			TreeViewColumn col_idproducto = new TreeViewColumn();
			CellRendererText cellr0 = new CellRendererText();
			col_idproducto.Title = "ID Producto";
			col_idproducto.PackStart(cellr0, true);
			col_idproducto.AddAttribute (cellr0, "text", 0);    // la siguiente columna será 1 en vez de 1
			col_idproducto.SortColumnId = (int) Column_inv.col_idproducto;
			col_idproducto.SetCellDataFunc(cellr0, new Gtk.TreeCellDataFunc(cambia_colores_fila));
			
			TreeViewColumn col_desc_producto = new TreeViewColumn();
			CellRendererText cellr1 = new CellRendererText();
			col_desc_producto.Title = "Descripcion de Producto";
			col_desc_producto.PackStart(cellr1, true);
			col_desc_producto.AddAttribute (cellr1, "text", 1);
			col_desc_producto.SortColumnId = (int) Column_inv.col_desc_producto;
			col_desc_producto.SetCellDataFunc(cellr1, new Gtk.TreeCellDataFunc(cambia_colores_fila));
			col_desc_producto.Resizable = true;
			cellr1.Width = 400;
			
			TreeViewColumn col_stock = new TreeViewColumn();
			CellRendererText cellrt2 = new CellRendererText();
			col_stock.Title = "stock";
			col_stock.PackStart(cellrt2, true);
			col_stock.AddAttribute (cellrt2, "text", 2);
			col_stock.SortColumnId = (int) Column_inv.col_stock;
			col_stock.SetCellDataFunc(cellrt2,new Gtk.TreeCellDataFunc(cambia_colores_fila));
            
			TreeViewColumn col_precioprod = new TreeViewColumn();
			CellRendererText cellrt3 = new CellRendererText();
			col_precioprod.Title = "Costo Unit.";
			col_precioprod.PackStart(cellrt3, true);
			col_precioprod.AddAttribute (cellrt3, "text", 3);
			col_precioprod.SortColumnId = (int) Column_inv.col_precioprod;
			col_precioprod.SetCellDataFunc(cellrt3,new Gtk.TreeCellDataFunc(cambia_colores_fila));
           
			TreeViewColumn col_costo_prod = new TreeViewColumn();
			CellRendererText cellrt4 = new CellRendererText();
			col_costo_prod.Title = "Costo Prod";
			col_costo_prod.PackStart(cellrt4, true);
			col_costo_prod.AddAttribute (cellrt4, "text", 4);
			col_costo_prod.SortColumnId = (int) Column_inv.col_costo_prod;
			col_costo_prod.SetCellDataFunc(cellrt4, new Gtk.TreeCellDataFunc(cambia_colores_fila));
			
			TreeViewColumn col_embalaje = new TreeViewColumn();
			CellRendererText cellrt5 = new CellRendererText();
			col_embalaje.Title = "Embalaje";
			col_embalaje.PackStart(cellrt5, true);
			col_embalaje.AddAttribute (cellrt5, "text", 5);
			col_embalaje.SortColumnId = (int) Column_inv.col_embalaje;
			col_embalaje.SetCellDataFunc(cellrt5, new Gtk.TreeCellDataFunc(cambia_colores_fila));
			
			TreeViewColumn col_precio_pub = new TreeViewColumn();
			CellRendererText cellrt6 = new CellRendererText();
			col_precio_pub.Title = "Precio Publico";
			col_precio_pub.PackStart(cellrt6, true);
			col_precio_pub.AddAttribute (cellrt6, "text", 6);
			col_precio_pub.SortColumnId = (int) Column_inv.col_precio_pub;
			col_precio_pub.SetCellDataFunc(cellrt6, new Gtk.TreeCellDataFunc(cambia_colores_fila));
           
			TreeViewColumn col_grupoprod = new TreeViewColumn();
			CellRendererText cellrt7 = new CellRendererText();
			col_grupoprod.Title = "Grupo Producto";
			col_grupoprod.PackStart(cellrt7, true);
			col_grupoprod.AddAttribute (cellrt7, "text", 7);
			col_grupoprod.SortColumnId = (int) Column_inv.col_grupoprod;
			col_grupoprod.SetCellDataFunc(cellrt7, new Gtk.TreeCellDataFunc(cambia_colores_fila));
          
			TreeViewColumn col_grupo1prod = new TreeViewColumn();
			CellRendererText cellrt8 = new CellRendererText();
			col_grupo1prod.Title = "Grupo1 Producto";
			col_grupo1prod.PackStart(cellrt8, true);
			col_grupo1prod.AddAttribute (cellrt8, "text", 8);
			col_grupo1prod.SortColumnId = (int) Column_inv.col_grupo1prod;
			col_grupo1prod.SetCellDataFunc(cellrt8, new Gtk.TreeCellDataFunc(cambia_colores_fila));
                        
			TreeViewColumn col_grupo2prod = new TreeViewColumn();
			CellRendererText cellrt9 = new CellRendererText();
			col_grupo2prod.Title = "Grupo2 Producto";
			col_grupo2prod.PackStart(cellrt9, true);
			col_grupo2prod.AddAttribute (cellrt9, "text", 9);
			col_grupo2prod.SortColumnId = (int) Column_inv.col_grupo2prod;
			col_grupo2prod.SetCellDataFunc(cellrt9, new Gtk.TreeCellDataFunc(cambia_colores_fila));
			
			TreeViewColumn col_fecha_hora = new TreeViewColumn();
			CellRendererText cellrt10 = new CellRendererText();
			col_fecha_hora.Title = "Fecha de creacion";
			col_fecha_hora.PackStart(cellrt10, true);
			col_fecha_hora.AddAttribute (cellrt10, "text", 10);
			col_fecha_hora.SortColumnId = (int) Column_inv.col_fecha_hora;
			col_fecha_hora.SetCellDataFunc(cellrt10, new Gtk.TreeCellDataFunc(cambia_colores_fila));
						
			TreeViewColumn col_empleado = new TreeViewColumn();
			CellRendererText cellrt11 = new CellRendererText();
			col_empleado.Title = "Empleado";
			col_empleado.PackStart(cellrt11, true);
			col_empleado.AddAttribute (cellrt11, "text", 11);
			col_empleado.SortColumnId = (int) Column_inv.col_empleado;
			col_empleado.SetCellDataFunc(cellrt11, new Gtk.TreeCellDataFunc(cambia_colores_fila));
			
			TreeViewColumn col_lote = new TreeViewColumn();
			CellRendererText cellrt14 = new CellRendererText();
			col_lote.Title = "Lote";
			col_lote.PackStart(cellrt14, true);
			col_lote.AddAttribute (cellrt14, "text", 14);
			col_lote.SortColumnId = (int) Column_inv.col_lote;
			col_lote.SetCellDataFunc(cellrt14, new Gtk.TreeCellDataFunc(cambia_colores_fila));
			
			TreeViewColumn col_caducidad = new TreeViewColumn();
			CellRendererText cellrt15 = new CellRendererText();
			col_caducidad.Title = "Caducidad";
			col_caducidad.PackStart(cellrt15, true);
			col_caducidad.AddAttribute (cellrt15, "text", 15);
			col_caducidad.SortColumnId = (int) Column_inv.col_caducidad;
			col_caducidad.SetCellDataFunc(cellrt15, new Gtk.TreeCellDataFunc(cambia_colores_fila));
			
			TreeViewColumn col_codigobarra = new TreeViewColumn();
			CellRendererText cellrt16 = new CellRendererText();
			col_codigobarra.Title = "Cod.Barras";
			col_codigobarra.PackStart(cellrt16, true);
			col_codigobarra.AddAttribute (cellrt16, "text", 16);
			//col_codigobarra.SortColumnId = (int) Column_inv.col_caducidad;
			col_codigobarra.SetCellDataFunc(cellrt16, new Gtk.TreeCellDataFunc(cambia_colores_fila));
						
			TreeViewColumn col_marca = new TreeViewColumn();
			CellRendererText cellrt17 = new CellRendererText();
			col_marca.Title = "Marca";
			col_marca.PackStart(cellrt17, true);
			col_marca.AddAttribute (cellrt17, "text", 17);
			//col_marca.SortColumnId = (int) Column_inv.marca;
			col_marca.SetCellDataFunc(cellrt17, new Gtk.TreeCellDataFunc(cambia_colores_fila));

			TreeViewColumn col_anaquel = new TreeViewColumn();
			CellRendererText cellrt18 = new CellRendererText();
			col_anaquel.Title = "Anaquel";
			col_anaquel.PackStart(cellrt18, true);
			col_anaquel.AddAttribute (cellrt18, "text", 18);
			//col_marca.SortColumnId = (int) Column_inv.col_caducidad;
			col_anaquel.SetCellDataFunc(cellrt18, new Gtk.TreeCellDataFunc(cambia_colores_fila));
			
			lista_de_inventario.AppendColumn(col_idproducto);
			lista_de_inventario.AppendColumn(col_codigobarra);
			lista_de_inventario.AppendColumn(col_desc_producto);
			lista_de_inventario.AppendColumn(col_marca);
			lista_de_inventario.AppendColumn(col_stock);
			lista_de_inventario.AppendColumn(col_precioprod);	
			lista_de_inventario.AppendColumn(col_costo_prod);
			lista_de_inventario.AppendColumn(col_embalaje);
			lista_de_inventario.AppendColumn(col_precio_pub);
			lista_de_inventario.AppendColumn(col_grupoprod);
			lista_de_inventario.AppendColumn(col_grupo1prod);
			lista_de_inventario.AppendColumn(col_grupo2prod);	
			lista_de_inventario.AppendColumn(col_fecha_hora);
			lista_de_inventario.AppendColumn(col_empleado);
			lista_de_inventario.AppendColumn(col_lote);
			lista_de_inventario.AppendColumn(col_caducidad);
			lista_de_inventario.AppendColumn(col_anaquel);
		}
		
		enum Column_inv
		{
			col_idproducto,
			col_desc_producto,
			col_stock,
			col_precioprod,
			col_costo_prod,
			col_embalaje,
			col_precio_pub,
			col_grupoprod,
			col_grupo1prod,
			col_grupo2prod,
			col_fecha_hora,
			col_empleado,
			col_lote,
			col_caducidad,
			col_anaquel
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
												typeof(string));
			lista_de_producto.Model = treeViewEngineBusca2;
			lista_de_producto.RulesHint = true;
			
			lista_de_producto.RowActivated += on_selecciona_producto_clicked;  // Doble click selecciono paciente
				
			TreeViewColumn col_idproducto = new TreeViewColumn();
			CellRendererText cellr0 = new CellRendererText();
			col_idproducto.Title = "ID Producto";
			col_idproducto.PackStart(cellr0, true);
			col_idproducto.AddAttribute (cellr0, "text", 0);
			col_idproducto.SortColumnId = (int) Column_prod.col_idproducto;
			
			TreeViewColumn col_desc_producto = new TreeViewColumn();
			CellRendererText cellr1 = new CellRendererText();
			col_desc_producto.Title = "Descripcion de Producto";
			col_desc_producto.PackStart(cellr1, true);
			col_desc_producto.AddAttribute (cellr1, "text", 1);
			col_desc_producto.SortColumnId = (int) Column_prod.col_desc_producto;
			//cellr0.Editable = true;   // Permite edita este campo
            
			TreeViewColumn col_precioprod = new TreeViewColumn();
			CellRendererText cellrt2 = new CellRendererText();
			col_precioprod.Title = "Costo Unit.";
			col_precioprod.PackStart(cellrt2, true);
			col_precioprod.AddAttribute (cellrt2, "text", 2);
			col_precioprod.SortColumnId = (int) Column_prod.col_precioprod;
           
           	TreeViewColumn col_costo_prod = new TreeViewColumn();
			CellRendererText cellrt3 = new CellRendererText();
			col_costo_prod.Title = "Costo Prod";
			col_costo_prod.PackStart(cellrt3, true);
			col_costo_prod.AddAttribute (cellrt3, "text", 3);
			col_costo_prod.SortColumnId = (int) Column_prod.col_costo_prod;
			
			TreeViewColumn col_embalaje = new TreeViewColumn();
			CellRendererText cellrt4 = new CellRendererText();
			col_embalaje.Title = "Embalaje";
			col_embalaje.PackStart(cellrt4, true);
			col_embalaje.AddAttribute (cellrt4, "text", 4);
			col_embalaje.SortColumnId = (int) Column_prod.col_embalaje;
			
			TreeViewColumn col_precio_pub = new TreeViewColumn();
			CellRendererText cellrt5 = new CellRendererText();
			col_precio_pub.Title = "Precio Publico";
			col_precio_pub.PackStart(cellrt5, true);
			col_precio_pub.AddAttribute (cellrt5, "text", 5);
			col_precio_pub.SortColumnId = (int) Column_prod.col_precio_pub;
           
			TreeViewColumn col_grupoprod = new TreeViewColumn();
			CellRendererText cellrt6 = new CellRendererText();
			col_grupoprod.Title = "Grupo Producto";
			col_grupoprod.PackStart(cellrt6, true);
			col_grupoprod.AddAttribute (cellrt6, "text", 6);
			col_grupoprod.SortColumnId = (int) Column_prod.col_grupoprod;
          
			TreeViewColumn col_grupo1prod = new TreeViewColumn();
			CellRendererText cellrt7 = new CellRendererText();
			col_grupo1prod.Title = "Grupo1 Producto";
			col_grupo1prod.PackStart(cellrt7, true);
			col_grupo1prod.AddAttribute (cellrt7, "text", 7);
			col_grupo1prod.SortColumnId = (int) Column_prod.col_grupo1prod;
                        
			TreeViewColumn col_grupo2prod = new TreeViewColumn();
			CellRendererText cellrt8 = new CellRendererText();
			col_grupo2prod.Title = "Grupo2 Producto";
			col_grupo2prod.PackStart(cellrt8, true);
			col_grupo2prod.AddAttribute (cellrt8, "text", 8);
			col_grupo2prod.SortColumnId = (int) Column_prod.col_grupo2prod;
			
			lista_de_producto.AppendColumn(col_idproducto);
			lista_de_producto.AppendColumn(col_desc_producto);
			lista_de_producto.AppendColumn(col_precioprod);
			lista_de_producto.AppendColumn(col_costo_prod);
			lista_de_producto.AppendColumn(col_embalaje);
			lista_de_producto.AppendColumn(col_precio_pub);
			lista_de_producto.AppendColumn(col_grupoprod);
			lista_de_producto.AppendColumn(col_grupo1prod);
			lista_de_producto.AppendColumn(col_grupo2prod);						
		}
		
		enum Column_prod
		{
			col_idproducto,
			col_desc_producto,
			col_precioprod,
			col_costo_prod,
			col_embalaje,
			col_precio_pub,
			col_grupoprod,
			col_grupo1prod,
			col_grupo2prod
		}
		
		//ACCION QUE CAMBIA EL COLOR DEL TEXTO PARA CUANDO SE GUARDA EN LA BASE DE DATOS 
		void cambia_colores_fila(Gtk.TreeViewColumn column, Gtk.CellRenderer cell, Gtk.TreeModel model, Gtk.TreeIter iter)
		{
			//descripcion_producto descrip = (descripcion_producto) model.GetValue (iter, 14);
			if ((bool)lista_de_inventario.Model.GetValue (iter,12)==true){
				(cell as Gtk.CellRendererText).Foreground = "darkblue";
			}else{
				(cell as Gtk.CellRendererText).Foreground = "red";
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