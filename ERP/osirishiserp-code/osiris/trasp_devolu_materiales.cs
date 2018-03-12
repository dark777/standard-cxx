//  
//  devolucion_materiales.cs
//  
//  Author:
//       dolivares <arcangeldoc@openmailbox.org>
// 
//  Copyright (c) 2013 dolivares
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

using GLib;
using System.Collections;

namespace osiris
{
	public class trasp_devolu_materiales
	{
		// Boton general para salir de las ventanas Todas la ventanas en glade este boton debe estra declarado identico
		[Widget] Gtk.Button button_salir;

		// Para todas las busquedas este es el nombre asignado se declara una vez
		[Widget] Gtk.Entry entry_expresion;
		[Widget] Gtk.Button button_selecciona;
		[Widget] Gtk.Button button_buscar_busqueda;
		
		// Declarando ventana principal de Hospitalizacion
		[Widget] Gtk.Window devoluciones_traspasos = null;
		[Widget] Gtk.Notebook notebook5 = null;
		[Widget] Gtk.CheckButton checkbutton_nueva_solicitud = null;
		[Widget] Gtk.Entry entry_numero_traspdev = null;
		[Widget] Gtk.Button button_sel_traspdevo = null;
		[Widget] Gtk.ComboBox combobox_tipo_traspdev = null;
		[Widget] Gtk.Entry entry_fecha_traspdev = null; 
		[Widget] Gtk.ComboBox combobox_almacen_destino = null;
		[Widget] Gtk.Entry entry_motivo = null;
		[Widget] Gtk.ComboBox combobox_tipo_almacen = null;
		[Widget] Gtk.RadioButton radiobutton_dev_stock = null;
		[Widget] Gtk.RadioButton radiobutton_dev_solic = null;
		[Widget] Gtk.Entry entry_numero_solicitud = null;
		[Widget] Gtk.Entry entry_fecha_solicitud = null;
		[Widget] Gtk.Entry entry_tipo_solicitud = null;
		[Widget] Gtk.Entry entry_status_solicitud = null;
		[Widget] Gtk.Entry entry_folio_servicio = null;
		[Widget] Gtk.Entry entry_pid_paciente = null;
		[Widget] Gtk.Entry entry_nombre_paciente = null;
		[Widget] Gtk.Entry entry_id_cirugia = null;
		[Widget] Gtk.Entry entry_cirugia = null;
		[Widget] Gtk.Entry entry_diagnostico = null;
		[Widget] Gtk.Entry entry_observacion = null;
		[Widget] Gtk.TreeView treeview_trasp_devol = null;
		[Widget] Gtk.Button button_guardar = null;
		[Widget] Gtk.Button button_busca_producto = null;
		[Widget] Gtk.Button button_quitar_productos = null;
		[Widget] Gtk.Entry entry_idproducto = null;
		[Widget] Gtk.Entry entry_descproducto = null;
		[Widget] Gtk.Button button_imprimir_tradev = null;
		[Widget] Gtk.Statusbar statusbar_dev_sol = null;

		// tab para recibir el productos
		[Widget] Gtk.ComboBox combobox_origen_trasp = null;
		[Widget] Gtk.ComboBox combobox_destino_trasp = null;
		[Widget] Gtk.TreeView treeview_autoriza_devotras = null;
		[Widget] Gtk.Button button_aceptar_devolucion = null;
		
		/////// Ventana Busqueda de productos\\\\\\\\
		[Widget] Gtk.TreeView lista_de_producto;
		//[Widget] Gtk.Button button_agrega_extra;
		[Widget] Gtk.Entry entry_cantidad_aplicada;
		[Widget] Gtk.Label label_titulo_cantidad;
		
		string LoginEmpleado;
		string NomEmpleado;
		string AppEmpleado;
		string ApmEmpleado;
		string nombrebd;
		string connectionString;
		int idalmacen;
		int idalacendestino;
		string descsubalmacen;
		string tipo_busqueda;
		float valoriva;
		bool tipoacceso;

		int tipo_devo_trasp;
		string name_devo_trasp = "";
		string[] args_args = {""};
		string[] args_tiposolicitud ={"","DEVOLUCION","TRASPASO","CADUCOS"};
		int[] args_id_array = {0,1,2,3,4,5,6,7,8};

		//Declaracion de ventana de error y pregunta
		protected Gtk.Window MyWinError;
		protected Gtk.Window MyWin;

		private TreeStore treeviewEnginetraspdevol;
		private TreeStore treeviewEngineAcptraspdevol;
		private TreeStore treeViewEngineBusca2;

		ArrayList columns = new ArrayList ();
		
		class_conexion conexion_a_DB = new class_conexion();
		//class_buscador classfind_data = new class_buscador();
		class_public classpublic = new class_public();
		/// <summary>
		/// Initializes a new instance of the <see cref="osiris.trasp_devolu_materiales"/> class.
		/// </summary>
		/// <param name='LoginEmp'>
		/// Login empl.
		/// </param>
		/// <param name='NomEmpleado'>
		/// Nom empleado.
		/// </param>
		/// <param name='AppEmpleado'>
		/// App empleado.
		/// </param>
		/// <param name='ApmEmpleado'>
		/// Apm empleado.
		/// </param>
		/// <param name='nombrebd'>
		/// Nombrebd.
		/// </param>
		/// <param name='idalmacen'>
		/// Idalmacen_.
		/// </param>
		/// <param name='descsubalmacen'>
		/// Descsubalmacen_.
		/// </param>
		/// <param name='tipo_devo_trasp'>
		/// Tipo_devo_trasp.
		/// </param>
		/// <param name='name_devo_trasp'>
		/// Name_devo_trasp.
		/// </param>
		/// <param name='tipoacceso'>
		/// Tipoacceso.
		/// </param>
		/// <param name='idalacendestino'>
		/// Idalacendestino.
		/// </param>
		public trasp_devolu_materiales( string LoginEmp_, string NomEmpleado_, string AppEmpleado_, 
			string ApmEmpleado_, string nombrebd_, int idalmacen_, string descsubalmacen_,
			int tipo_devo_trasp_,string name_devo_trasp_,bool tipoacceso_,int idalacendestino_)
		{
			LoginEmpleado = LoginEmp_;
			NomEmpleado = NomEmpleado_;
			AppEmpleado = AppEmpleado_;
			ApmEmpleado = ApmEmpleado_;
			nombrebd = conexion_a_DB._nombrebd;
			idalmacen = idalmacen_;
			descsubalmacen = descsubalmacen_;
			tipo_devo_trasp = tipo_devo_trasp_;
			name_devo_trasp = name_devo_trasp_;
			tipoacceso = tipoacceso_;
			valoriva = float.Parse(classpublic.ivaparaaplicar);
			idalacendestino = idalacendestino_;
			connectionString = conexion_a_DB._url_servidor+conexion_a_DB._port_DB+conexion_a_DB._usuario_DB+conexion_a_DB._passwrd_user_DB;
			Glade.XML gxml = new Glade.XML (null, "almacen_costos_compras.glade", "devoluciones_traspasos", null);
			gxml.Autoconnect (this);
			devoluciones_traspasos.Show();
						
			checkbutton_nueva_solicitud.Clicked += new EventHandler(on_checkbutton_nueva_solicitud_clicked);
			radiobutton_dev_stock.Clicked += new EventHandler(on_radiobutton_stocksoli_clicked);
			button_guardar.Clicked += new EventHandler(on_button_guardar_clicked);
			radiobutton_dev_solic.Clicked += new EventHandler(on_radiobutton_stocksoli_clicked);
			button_busca_producto.Clicked += new EventHandler(on_button_busca_producto_clicked);
			button_sel_traspdevo.Clicked += new EventHandler(on_button_sel_traspdevo_clicked);
			button_quitar_productos.Clicked += new EventHandler(on_button_quitar_productos_clicked);
			button_imprimir_tradev.Clicked += new EventHandler(on_button_imprimir_tradev_clicked);
			button_salir.Clicked += new EventHandler(on_cierraventanas_clicked);

			entry_numero_solicitud.Sensitive = false;
			combobox_tipo_traspdev.Sensitive = false;
			combobox_almacen_destino.Sensitive = false;
			button_busca_producto.Sensitive = false;
			button_quitar_productos.Sensitive = false;
			entry_idproducto.Sensitive = false;
			entry_descproducto.Sensitive = false;
			entry_motivo.IsEditable = false;
			button_guardar.Sensitive = false;

			llenado_combobox(1,name_devo_trasp,combobox_tipo_traspdev,"array","","","",args_tiposolicitud,args_id_array,"");
						
			combobox_tipo_almacen.Sensitive = tipoacceso;
			entry_numero_traspdev.GrabFocus();
			
			
			// pestaña para marcar la devolucion
			//button_cargar_devoltra.Clicked += new EventHandler(on_button_cargar_devoltra_clicked);
			
			
			llenado_combobox(0,"",combobox_destino_trasp,"sql","SELECT id_almacen,descripcion_almacen,sub_almacen FROM osiris_almacenes WHERE sub_almacen = 'true' AND recibe_devolucion = 'true' ORDER BY descripcion_almacen;","descripcion_almacen","id_almacen",args_args,args_id_array,"");
			if(tipoacceso_){
				llenado_combobox(1,"",combobox_origen_trasp,"sql","SELECT id_almacen,descripcion_almacen,sub_almacen FROM osiris_almacenes WHERE sub_almacen = 'true' AND recibe_devolucion = 'false' ORDER BY descripcion_almacen;","descripcion_almacen","id_almacen",args_args,args_id_array,"");
				notebook5.CurrentPage = 1;
			}else{
				llenado_combobox(1,descsubalmacen,combobox_tipo_almacen,"sql","SELECT id_almacen,descripcion_almacen,sub_almacen FROM osiris_almacenes WHERE sub_almacen = 'true' ORDER BY descripcion_almacen;","descripcion_almacen","id_almacen",args_args,args_id_array,"");
				button_aceptar_devolucion.Sensitive = false;
				llenado_combobox(0,"",combobox_origen_trasp,"sql","SELECT id_almacen,descripcion_almacen,sub_almacen FROM osiris_almacenes WHERE sub_almacen = 'true' AND recibe_devolucion = 'false' AND id_almacen ='"+idalmacen.ToString().Trim()+"' ORDER BY descripcion_almacen;","descripcion_almacen","id_almacen",args_args,args_id_array,"");
				
			}
				
			Pango.FontDescription fontdesc = Pango.FontDescription.FromString("Arial 10");  // Cambia el tipo de Letra
			fontdesc.Weight = Pango.Weight.Bold; // Letra Negrita
			//entry_numero_traspdev.
			entry_numero_traspdev.ModifyBase(StateType.Normal, new Gdk.Color(255,243,169)); // Color Amarillo
			entry_motivo.ModifyBase(StateType.Normal, new Gdk.Color(255,243,169)); // Color Amarillo
			entry_numero_solicitud.ModifyBase(StateType.Normal, new Gdk.Color(255,243,169)); // Color Amarillo
			entry_motivo.ModifyFont(fontdesc);  // Arial y Negrita
			statusbar_dev_sol.Pop(0);
			statusbar_dev_sol.Push(1, "login: "+LoginEmpleado+"  |Usuario: "+NomEmpleado+" "+AppEmpleado+" "+ApmEmpleado);
			statusbar_dev_sol.HasResizeGrip = false;
			crea_treeview_devtrasp();
			crea_treeview_aceptadevtrasp();
		}

		void llenado_combobox(int tipodellenado,string descrip_defaul,object obj,string sql_or_array,string query_SQL,string name_field_desc,string name_field_id,string[] args_array,int[] args_id_array,string name_field_id2)
		{			
			Gtk.ComboBox combobox_llenado = (Gtk.ComboBox) obj;
			//Gtk.ComboBox combobox_pos_neg = obj as Gtk.ComboBox;
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
						store.AppendValues ((string) lector[ name_field_desc ], (int) lector[ name_field_id],false);
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
				case "combobox_tipo_traspdev":
					name_devo_trasp = (string) onComboBoxChanged.Model.GetValue(iter,0);
					break;
				case "combobox_almacen_destino":
					idalacendestino = (int) onComboBoxChanged.Model.GetValue(iter,1);
					break;
				case "combobox_origen_trasp":
					idalmacen = (int) (int) onComboBoxChanged.Model.GetValue(iter,1);
					break;
				}
			}
		}
		
		void on_checkbutton_nueva_solicitud_clicked(object sender, EventArgs args)
	    {
	    	if (checkbutton_nueva_solicitud.Active == true){
				MessageDialog msgBox = new MessageDialog (MyWin,DialogFlags.Modal,
					MessageType.Question,ButtonsType.YesNo,"¿ Esta seguro de CREAR una Nueva SOLICITUD de "+name_devo_trasp+"?");
				ResponseType miResultado = (ResponseType)msgBox.Run ();
				msgBox.Destroy();
		 		if (miResultado == ResponseType.Yes){
					treeviewEnginetraspdevol.Clear();
					button_sel_traspdevo.Sensitive = false;
					button_busca_producto.Sensitive = true;
					button_quitar_productos.Sensitive = true;
					entry_idproducto.Sensitive = true;
					entry_descproducto.Sensitive = true;
					entry_numero_traspdev.IsEditable = false;
					button_guardar.Sensitive = true;
					entry_motivo.Text = "";
					entry_motivo.IsEditable = true;
					entry_numero_traspdev.Text = (string) classpublic.lee_ultimonumero_registrado("osiris_erp_movtraspdevo","folio_de_traspdevo","WHERE id_tipo_mov = '"+tipo_devo_trasp.ToString().Trim()+"' AND id_almacen = '"+idalmacen.ToString().Trim()+"' ");
					entry_fecha_traspdev.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
					if(name_devo_trasp == "DEVOLUCION"){
						llenado_combobox(1,"ALMACEN GENERAL",combobox_almacen_destino,"sql","SELECT id_almacen,descripcion_almacen,sub_almacen FROM osiris_almacenes WHERE sub_almacen = 'true' ORDER BY descripcion_almacen;","descripcion_almacen","id_almacen",args_args,args_id_array,"");
						combobox_almacen_destino.Sensitive = false;
					}
					if(name_devo_trasp == "TRASPASO"){
						llenado_combobox(1,"ALMACEN GENERAL",combobox_almacen_destino,"sql","SELECT id_almacen,descripcion_almacen,sub_almacen FROM osiris_almacenes WHERE sub_almacen = 'true' ORDER BY descripcion_almacen;","descripcion_almacen","id_almacen",args_args,args_id_array,"");
						combobox_almacen_destino.Sensitive = true;
					}
					if(name_devo_trasp == "CADUCOS"){
						llenado_combobox(1,"CADUCADOS",combobox_almacen_destino,"sql","SELECT id_almacen,descripcion_almacen,sub_almacen FROM osiris_almacenes WHERE sub_almacen = 'true' ORDER BY descripcion_almacen;","descripcion_almacen","id_almacen",args_args,args_id_array,"");
						combobox_almacen_destino.Sensitive = true;
					}
				}else{
					button_guardar.Sensitive = false;
					entry_fecha_traspdev.Text = "";
					button_sel_traspdevo.Sensitive = true;
					checkbutton_nueva_solicitud.Active = false;
					entry_idproducto.Sensitive = false;
					entry_descproducto.Sensitive = false;
					button_busca_producto.Sensitive = false;
					button_quitar_productos.Sensitive = false;
					entry_numero_traspdev.IsEditable = true;
					entry_motivo.IsEditable = false;
				}
			}else{
				button_sel_traspdevo.Sensitive = true;
				entry_idproducto.Sensitive = false;
				entry_descproducto.Sensitive = false;
				button_busca_producto.Sensitive = false;
				button_quitar_productos.Sensitive = false;
				entry_numero_traspdev.IsEditable = true;
				entry_motivo.IsEditable = false;
				button_guardar.Sensitive = false;
			}
		}
		
		void on_radiobutton_stocksoli_clicked(object sender, EventArgs args)
		{
			Gtk.RadioButton radiobutton_sender = sender as Gtk.RadioButton;
			if(radiobutton_sender.Active == true){
				if(radiobutton_sender.Name.ToString() == "radiobutton_dev_stock"){
					Console.WriteLine("STOCK");
					entry_numero_solicitud.Sensitive = false;
				}
				if(radiobutton_sender.Name.ToString() == "radiobutton_dev_solic"){
					Console.WriteLine("DEVO");
					entry_numero_solicitud.Sensitive = true;
				}
			}
		}

		void on_button_guardar_clicked(object sender, EventArgs args)
		{
			TreeIter iter;
			//treeview_trasp_devol.RemoveColumn(tvc);
			//treeviewEnginetraspdevol
			if (treeviewEnginetraspdevol.GetIterFirst (out iter)){
				if(entry_motivo.Text !=""){
					MessageDialog msgBox = new MessageDialog (MyWin,DialogFlags.Modal,
								MessageType.Question,ButtonsType.YesNo,"¿ Esta seguro de "+name_devo_trasp+" los Materiales Seleccionados?");
					ResponseType miResultado = (ResponseType)msgBox.Run ();			msgBox.Destroy();
					if (miResultado == ResponseType.Yes){
						NpgsqlConnection conexion;
						conexion = new NpgsqlConnection (connectionString+nombrebd);
						try{
							entry_numero_traspdev.Text = (string) classpublic.lee_ultimonumero_registrado("osiris_erp_movtraspdevo","folio_de_traspdevo","WHERE id_tipo_mov = '"+tipo_devo_trasp.ToString().Trim()+"' AND id_almacen = '"+idalmacen.ToString().Trim()+"' ");
							entry_motivo.Text.ToUpper();
							conexion.Open ();
							NpgsqlCommand comando; 
							comando = conexion.CreateCommand ();
							if ((bool) treeview_trasp_devol.Model.GetValue (iter,0) == true){
								comando.CommandText = "INSERT INTO osiris_erp_movtraspdevo (" +
								                      "folio_de_traspdevo," +
								                      "motivo_traspdevo," +
								                      "id_producto," +
								                      "id_almacen," +
								                      "id_almacen2," +
								                      "fechahora_creacion," +
								                      "id_empleado," +
								                      "cantidad_traspdevo," +
								                      "precio_costo," +
								                      "precio_costo_unitario," +
								                      "porcentage_ganancia," +
								                      "porcentage_descuento," +
								                      "porcentage_iva," +
								                      "cantidad_de_embalaje," +
								                      "precio_producto_publico," +
								                      "id_tipo_mov," +
								                      "stock_almacen_origen) "+
								                      "VALUES ('" +
								                      entry_numero_traspdev.Text.Trim()+"','" +
								                      entry_motivo.Text.ToUpper().Trim()+"','"+
								                      treeview_trasp_devol.Model.GetValue(iter,2).ToString().Trim()+"','"+
								                      idalmacen.ToString()+"','"+
								                      idalacendestino+"','"+
								                      DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"','"+
								                      LoginEmpleado+"','"+
								                      treeview_trasp_devol.Model.GetValue(iter,1).ToString().Trim()+"','"+
								                      treeview_trasp_devol.Model.GetValue(iter,4).ToString().Trim()+"','"+
								                      treeview_trasp_devol.Model.GetValue(iter,5).ToString().Trim()+"','"+
								                      treeview_trasp_devol.Model.GetValue(iter,8).ToString().Trim()+"','"+
								                      treeview_trasp_devol.Model.GetValue(iter,9).ToString().Trim()+"','"+
								                      valoriva.ToString().Trim()+"','"+
								                      treeview_trasp_devol.Model.GetValue(iter,6).ToString().Trim()+"','"+
								                      treeview_trasp_devol.Model.GetValue(iter,10).ToString().Trim()+"','"+
								                      tipo_devo_trasp.ToString()+"','"+
								                      treeview_trasp_devol.Model.GetValue(iter,7).ToString().Trim()+"')";
								Console.WriteLine(comando.CommandText);
								comando.ExecuteNonQuery();				comando.Dispose();
							}
							while ((bool) treeview_trasp_devol.Model.IterNext(ref iter) == true){
								comando.CommandText = "INSERT INTO osiris_erp_movtraspdevo (" +
								                      "folio_de_traspdevo," +
								                      "motivo_traspdevo," +
								                      "id_producto," +
								                      "id_almacen," +
								                      "id_almacen2," +
								                      "fechahora_creacion," +
								                      "id_empleado," +
								                      "cantidad_traspdevo," +
								                      "precio_costo," +
								                      "precio_costo_unitario," +
								                      "porcentage_ganancia," +
								                      "porcentage_descuento," +
								                      "porcentage_iva," +
								                      "cantidad_de_embalaje," +
								                      "precio_producto_publico," +
								                      "id_tipo_mov," +
								                      "stock_almacen_origen) "+
								                      "VALUES ('" +
								                      entry_numero_traspdev.Text.Trim()+"','" +
								                      entry_motivo.Text.ToUpper().Trim()+"','"+
								                      treeview_trasp_devol.Model.GetValue(iter,2).ToString().Trim()+"','"+
								                      idalmacen.ToString()+"','"+
								                      idalacendestino+"','"+
								                      DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"','"+
								                      LoginEmpleado+"','"+
								                      treeview_trasp_devol.Model.GetValue(iter,1).ToString().Trim()+"','"+
								                      treeview_trasp_devol.Model.GetValue(iter,4).ToString().Trim()+"','"+
								                      treeview_trasp_devol.Model.GetValue(iter,5).ToString().Trim()+"','"+
								                      treeview_trasp_devol.Model.GetValue(iter,8).ToString().Trim()+"','"+
								                      treeview_trasp_devol.Model.GetValue(iter,9).ToString().Trim()+"','"+
								                      valoriva.ToString().Trim()+"','"+
								                      treeview_trasp_devol.Model.GetValue(iter,6).ToString().Trim()+"','"+
								                      treeview_trasp_devol.Model.GetValue(iter,10).ToString().Trim()+"','"+
								                      tipo_devo_trasp.ToString()+"','"+
								                      treeview_trasp_devol.Model.GetValue(iter,7).ToString().Trim()+"')";
								//Console.WriteLine(comando.CommandText);
								comando.ExecuteNonQuery();					comando.Dispose();
							}
						}catch (NpgsqlException ex){
							MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
								MessageType.Error,ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
							msgBoxError.Run ();				msgBoxError.Destroy();
						}
						conexion.Close();
					}
				}else{
					MessageDialog msgBox = new MessageDialog (MyWin,DialogFlags.Modal,
						MessageType.Info,ButtonsType.Ok,"Debe escribir el Motivo, para efectuar este proceso.");										
					msgBox.Run ();	msgBox.Destroy();
				}
			}else{
				MessageDialog msgBox = new MessageDialog (MyWin,DialogFlags.Modal,
					MessageType.Info,ButtonsType.Ok,"No existe productos que procesar, verifique");										
				msgBox.Run ();	msgBox.Destroy();
			}
		}

		void on_button_busca_producto_clicked(object sender, EventArgs args)
		{
			Glade.XML gxml = new Glade.XML (null, "hospitalizacion.glade", "busca_producto", null);
			gxml.Autoconnect (this);			
			crea_treeview_busqueda("producto");			
			button_buscar_busqueda.Clicked += new EventHandler(on_llena_lista_producto_clicked);
			button_selecciona.Clicked += new EventHandler(on_selecciona_producto_clicked);
			button_salir.Clicked += new EventHandler(on_cierraventanas_clicked); // esta sub-clase esta en hscmty.cs
			label_titulo_cantidad.Text = "Cant. Dev./Trasp.";	
			entry_expresion.KeyPressEvent += onKeyPressEvent_entry_expresion;			
			// Validando que sen solo numeros
			entry_cantidad_aplicada.KeyPressEvent += onKeyPressEvent;
		}

		[GLib.ConnectBefore ()]   	  // Esto es indispensable para que funcione    
		public void onKeyPressEvent_entry_expresion(object o, Gtk.KeyPressEventArgs args)
		{
			//Console.WriteLine(Convert.ToChar(args.Event.KeyValue));
			//Console.WriteLine(args.Event.Key);
			if (args.Event.Key.ToString() == "Return" || args.Event.Key.ToString() == "KP_Enter"){
				args.RetVal = true;
				llenando_lista_de_productos();			
			}
		}

		void on_button_imprimir_tradev_clicked(object sender, EventArgs args)
		{
			string query_sql = "SELECT cantidad_traspdevo AS cant_devol,osiris_erp_movtraspdevo.folio_de_traspdevo AS nro_"+name_devo_trasp+"," +
				"to_char(osiris_erp_movtraspdevo.id_producto,'999999999999') AS idproducto," +
				"to_char(osiris_erp_movtraspdevo.fechahora_creacion,'yyyy-MM-dd') AS fechacreacion,to_char(osiris_erp_movtraspdevo.fechahora_autorizacion,'yyyy-MM-dd') AS fechaautoriza," +
				"motivo_traspdevo," +
				"osiris_productos.descripcion_producto AS descripcion_prod,osiris_almacenes.descripcion_almacen AS descrip_almacen " +
				"FROM osiris_erp_movtraspdevo,osiris_productos,osiris_almacenes " +
				"WHERE osiris_erp_movtraspdevo.id_producto = osiris_productos.id_producto " +
				"AND osiris_erp_movtraspdevo.id_almacen = osiris_almacenes.id_almacen " +
				"AND osiris_erp_movtraspdevo.id_almacen = '"+idalmacen.ToString().Trim()+"' " +
				"AND osiris_erp_movtraspdevo.folio_de_traspdevo = '"+entry_numero_traspdev.Text.ToString().Trim()+"';";
			string[] args_names_field = {"fechacreacion","nro_"+name_devo_trasp,"cant_devol","idproducto","descripcion_prod","descrip_almacen","motivo_traspdevo","fechaautoriza"};
			string[] args_type_field = {"string","string","float","string","string","string","string","string"};
			string[] args_field_text = {""};
			string[] args_more_title = {""};
			string[,] args_formulas = {{"",""}};
			string[,] args_width = {{"",""}};
			new osiris.class_traslate_spreadsheet(query_sql,args_names_field,args_type_field,false,args_field_text,"",false,args_more_title,args_formulas,args_width);
		}

		void on_button_sel_traspdevo_clicked(object sender, EventArgs args)
		{
			llenado_de_traspsoli();
		}

		void llenado_de_traspsoli()
		{
			NpgsqlConnection conexion; 
			conexion = new NpgsqlConnection (connectionString+nombrebd);
			// Verifica que la base de datos este conectada
			try{
				treeviewEnginetraspdevol.Clear();
				conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand ();               	
				comando.CommandText = "SELECT cantidad_traspdevo AS cant_devol,osiris_erp_movtraspdevo.folio_de_traspdevo AS nro_"+name_devo_trasp+"," +
					"to_char(osiris_erp_movtraspdevo.id_producto,'999999999999') AS idproducto," +
					"to_char(osiris_erp_movtraspdevo.fechahora_creacion,'yyyy-MM-dd HH24:MI:SS') AS fechahoracreacion,motivo_traspdevo," +
					"osiris_productos.descripcion_producto AS descripcion_prod,osiris_almacenes.descripcion_almacen AS descrip_almacen " +
					"FROM osiris_erp_movtraspdevo,osiris_productos,osiris_almacenes " +
					"WHERE osiris_erp_movtraspdevo.id_producto = osiris_productos.id_producto " +
					"AND osiris_erp_movtraspdevo.id_almacen = osiris_almacenes.id_almacen " +
					"AND osiris_erp_movtraspdevo.id_almacen = '"+idalmacen.ToString().Trim()+"' " +
					"AND osiris_erp_movtraspdevo.folio_de_traspdevo = '"+entry_numero_traspdev.Text.ToString().Trim()+"';";
				//Console.WriteLine(comando.CommandText);
				NpgsqlDataReader lector = comando.ExecuteReader ();
				if(lector.Read()){
					entry_fecha_traspdev.Text = lector["fechahoracreacion"].ToString().Trim();
					entry_motivo.Text = lector["motivo_traspdevo"].ToString().Trim();
					treeviewEnginetraspdevol.AppendValues (true,
					                                       lector["cant_devol"].ToString().Trim(),
					                                       lector["idproducto"].ToString().Trim(),
					                                       lector["descripcion_prod"].ToString().Trim(),
					                                       "");
					while (lector.Read()){
						treeviewEnginetraspdevol.AppendValues (true,
					                                       lector["cant_devol"].ToString().Trim(),
					                                       lector["idproducto"].ToString().Trim(),
					                                       lector["descripcion_prod"].ToString().Trim(),
					                                       "");
					}
				}
			}catch (NpgsqlException ex){
				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
					MessageType.Error,ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
				msgBoxError.Run ();				msgBoxError.Destroy();
			}
			conexion.Close ();
			
		}

		void crea_treeview_busqueda(string tipo_busqueda)
		{
			if (tipo_busqueda == "producto"){
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
					typeof(string));

				lista_de_producto.Model = treeViewEngineBusca2;			
				lista_de_producto.RulesHint = true;			
				lista_de_producto.RowActivated += on_selecciona_producto_clicked;

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
				col_desc_producto.Resizable = true;
				cellr1.Width = 15;

				TreeViewColumn col_grupoprod = new TreeViewColumn();
				CellRendererText cellrt2 = new CellRendererText();
				col_grupoprod.Title = "Grupo Producto";//Precio Producto
				col_grupoprod.PackStart(cellrt2, true);
				col_grupoprod.AddAttribute (cellrt2, "text", 2);
				col_grupoprod.SortColumnId = (int) Column_prod.col_grupoprod;

				TreeViewColumn col_grupo1prod = new TreeViewColumn();
				CellRendererText cellrt3 = new CellRendererText();
				col_grupo1prod.Title = "Grupo1 Producto";
				col_grupo1prod.PackStart(cellrt3, true);
				col_grupo1prod.AddAttribute (cellrt3, "text", 3);
				col_grupo1prod.SortColumnId = (int) Column_prod.col_grupo1prod;

				TreeViewColumn col_grupo2prod = new TreeViewColumn();
				CellRendererText cellrt4 = new CellRendererText();
				col_grupo2prod.Title = "Grupo2 Producto";
				col_grupo2prod.PackStart(cellrt4, true);
				col_grupo2prod.AddAttribute (cellrt4, "text", 4);
				col_grupo2prod.SortColumnId = (int) Column_prod.col_grupo2prod;

				TreeViewColumn col_stock = new TreeViewColumn();
				CellRendererText cellrt5 = new CellRendererText();
				col_stock.Title = "Stock";	//Total
				col_stock.PackStart(cellrt5, true);
				col_stock.AddAttribute (cellrt5, "text", 13);
				col_stock.SortColumnId = (int) Column_prod.col_stock;

				lista_de_producto.AppendColumn(col_stock);
				lista_de_producto.AppendColumn(col_idproducto);
				lista_de_producto.AppendColumn(col_desc_producto);
				lista_de_producto.AppendColumn(col_grupoprod);
				lista_de_producto.AppendColumn(col_grupo1prod);
				lista_de_producto.AppendColumn(col_grupo2prod);
			}
		}

		enum Column_prod
		{
			col_idproducto,
			col_desc_producto,
			col_grupoprod,
			col_grupo1prod,
			col_grupo2prod,
			col_stock
		}

		// llena la lista de productos
		void on_llena_lista_producto_clicked (object sender, EventArgs args)
		{
			llenando_lista_de_productos();
		}

		void llenando_lista_de_productos()
		{
			string acceso_a_grupos = classpublic.lee_registro_de_tabla("osiris_almacenes","id_almacen"," WHERE osiris_almacenes.id_almacen = '"+this.idalmacen.ToString().Trim()+"' ","acceso_grupo_producto","int");
			treeViewEngineBusca2.Clear();
			NpgsqlConnection conexion; 
			conexion = new NpgsqlConnection (connectionString+nombrebd);
			// Verifica que la base de datos este conectada
			try{
				conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand ();               	
				comando.CommandText = "SELECT to_char(osiris_productos.id_producto,'999999999999') AS codProducto,"+
				                      "osiris_productos.descripcion_producto,to_char(precio_producto_publico,'99999999.99') AS preciopublico,"+
				                      "aplicar_iva,to_char(porcentage_descuento,'999.99') AS porcentagesdesc,aplica_descuento," +
				                      "osiris_productos.cantidad_de_embalaje,"+
				                      "descripcion_grupo_producto,descripcion_grupo1_producto,descripcion_grupo2_producto,to_char(costo_por_unidad,'999999999.99') AS costoproductounitario, "+
				                      "to_char(porcentage_ganancia,'99999.99') AS porcentageutilidad,to_char(costo_producto,'999999999.99') AS costoproducto, "+
				                      "to_char(stock,'999999999.99') AS stock_subalmacen "+
				                      "FROM osiris_productos,osiris_catalogo_almacenes,osiris_grupo_producto,osiris_grupo1_producto,osiris_grupo2_producto "+
				                      "WHERE osiris_productos.id_grupo_producto = osiris_grupo_producto.id_grupo_producto "+
				                      "AND osiris_productos.id_producto = osiris_catalogo_almacenes.id_producto "+
				                      "AND osiris_catalogo_almacenes.id_almacen = '"+idalmacen.ToString().Trim()+"' "+
				                      "AND osiris_catalogo_almacenes.eliminado = 'false' "+	
				                      "AND osiris_productos.id_grupo1_producto = osiris_grupo1_producto.id_grupo1_producto "+
				                      "AND osiris_productos.id_grupo2_producto = osiris_grupo2_producto.id_grupo2_producto "+
				                      "AND osiris_productos.cobro_activo = true "+
				                      //"AND osiris_grupo_producto.agrupacion IN(= 'MD1' "+
				                      "AND osiris_productos.id_grupo_producto IN("+acceso_a_grupos+") "+
				                      "AND osiris_productos.descripcion_producto LIKE '%"+entry_expresion.Text.ToUpper()+"%' ORDER BY descripcion_producto; ";
				//Console.WriteLine(comando.CommandText);
				NpgsqlDataReader lector = comando.ExecuteReader ();
				float tomaprecio;
				float calculodeiva;
				float preciomasiva;
				float tomadescue;
				float preciocondesc;							
				while (lector.Read()){
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
						(string) lector["descripcion_grupo_producto"],
						(string) lector["descripcion_grupo1_producto"],
						(string) lector["descripcion_grupo2_producto"],
						(string) lector["preciopublico"],
						calculodeiva.ToString("F").PadLeft(10),
						preciomasiva.ToString("F").PadLeft(10),
						(string) lector["porcentagesdesc"],
						preciocondesc.ToString("F").PadLeft(10),
						(string) lector["costoproductounitario"],
						(string) lector["porcentageutilidad"],
						(string) lector["costoproducto"],
						(string) lector["stock_subalmacen"],
						lector["cantidad_de_embalaje"].ToString());

				}
			}catch (NpgsqlException ex){
				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
					MessageType.Error,ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
				msgBoxError.Run ();				msgBoxError.Destroy();
			}
			conexion.Close ();
		}

		void on_selecciona_producto_clicked (object sender, EventArgs args)
		{
			TreeModel model;
			TreeIter iterSelected;

			if (lista_de_producto.Selection.GetSelected(out model, out iterSelected)){
				if((float) float.Parse((string) entry_cantidad_aplicada.Text) > 0){
					if(float.Parse((string) lista_de_producto.Model.GetValue (iterSelected,13)) > 0 ){
						entry_idproducto.Text = (string) model.GetValue(iterSelected, 0);
						entry_descproducto.Text = (string) model.GetValue(iterSelected, 1);
						treeviewEnginetraspdevol.AppendValues (true,
										entry_cantidad_aplicada.Text,
										(string) model.GetValue(iterSelected, 0),
										(string) model.GetValue(iterSelected, 1),
										(string) model.GetValue(iterSelected, 12),
										(string) model.GetValue(iterSelected, 10),
										(string) model.GetValue(iterSelected, 14),
										(string) model.GetValue(iterSelected, 13),
										(string) model.GetValue(iterSelected, 11),
										(string) model.GetValue(iterSelected, 8),
										(string) model.GetValue(iterSelected, 5));
						entry_cantidad_aplicada.Text = "0";
					}else{
						MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
							MessageType.Error,ButtonsType.Close, "El producto seleccionado  NO \n"+
							"tiene EXISTENCIA en el Sub-Almacen, verifique...");
						msgBoxError.Run ();			msgBoxError.Destroy();
					}
				}else{
					MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
						MessageType.Error,ButtonsType.Close, 
						"La cantidad que quiere solicitar debe ser \n"+"distinta a cero, intente de nuevo");
					msgBoxError.Run ();					msgBoxError.Destroy();
				}
			}
		}

		void crea_treeview_devtrasp()
		{
			Gtk.CellRendererText text_;
			Gtk.CellRendererToggle toggle_;
			foreach (TreeViewColumn tvc in treeview_trasp_devol.Columns)
				treeview_trasp_devol.RemoveColumn(tvc);
				treeviewEnginetraspdevol = new TreeStore(typeof(bool),
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
			treeview_trasp_devol.Model = treeviewEnginetraspdevol;
			treeview_trasp_devol.RulesHint = true;
			//treeview_trasp_devol.Selection.Mode = SelectionMode.Multiple;

			toggle_ = new CellRendererToggle ();
			toggle_.Xalign = 0.0f;
			columns.Add (toggle_);
			toggle_.Toggled += selecciona_fila;
			TreeViewColumn column0 = new TreeViewColumn("Seleccion",toggle_,"active",Column_trapdev.seleccion);
			column0.Resizable = true;
			column0.SortColumnId = (int) Column_trapdev.seleccion;

			text_ = new CellRendererText ();
			text_.Xalign = 0.0f;
			text_.Editable = true;
			text_.Edited += NumberCellEdited_Autorizado;
			columns.Add (text_);
			TreeViewColumn column1 = new TreeViewColumn("Cantidad",text_,"text",Column_trapdev.cantidad);
			column1.Resizable = true;
			column1.SortColumnId = (int) Column_trapdev.cantidad;

			text_ = new CellRendererText ();
			text_.Xalign = 0.0f;
			text_.Editable = true;
			columns.Add (text_);
			TreeViewColumn column2 = new TreeViewColumn("ID Prod.",text_,"text",Column_trapdev.idproducto);
			column2.Resizable = true;
			column2.SortColumnId = (int) Column_trapdev.idproducto;

			text_ = new CellRendererText ();
			text_.Xalign = 0.0f;
			text_.Editable = true;
			columns.Add (text_);
			TreeViewColumn column3 = new TreeViewColumn("Desc. Producto",text_,"text",Column_trapdev.descripprod);
			column3.Resizable = true;
			column3.SortColumnId = (int) Column_trapdev.descripprod;

			text_ = new CellRendererText ();
			text_.Xalign = 0.0f;
			text_.Editable = true;
			columns.Add (text_);
			TreeViewColumn column4 = new TreeViewColumn("Stock",text_,"text",Column_trapdev.stock);
			column4.Resizable = true;
			column4.SortColumnId = (int) Column_trapdev.stock;

			treeview_trasp_devol.InsertColumn (column0, (int) Column_trapdev.seleccion);
			treeview_trasp_devol.InsertColumn (column1, (int) Column_trapdev.cantidad);
			treeview_trasp_devol.InsertColumn (column2, (int) Column_trapdev.idproducto);
			treeview_trasp_devol.InsertColumn (column3, (int) Column_trapdev.descripprod);
			treeview_trasp_devol.InsertColumn (column4, (int) Column_trapdev.stock);
		}

		enum Column_trapdev
		{
			seleccion,
			cantidad,
			idproducto,
			descripprod,
			stock
		}

		void crea_treeview_aceptadevtrasp()
		{
			Gtk.CellRendererText text_;
			Gtk.CellRendererToggle toggle_;
			foreach (TreeViewColumn tvc in treeview_autoriza_devotras.Columns)
				treeview_trasp_devol.RemoveColumn(tvc);
			treeviewEngineAcptraspdevol = new TreeStore(typeof(bool),
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
			treeview_autoriza_devotras.Model = treeviewEngineAcptraspdevol;
			treeview_autoriza_devotras.RulesHint = true;
			//treeview_trasp_devol.Selection.Mode = SelectionMode.Multiple;

			toggle_ = new CellRendererToggle ();
			toggle_.Xalign = 0.0f;
			columns.Add (toggle_);
			toggle_.Toggled += selecciona_fila;
			TreeViewColumn column0 = new TreeViewColumn("Seleccion",toggle_,"active",Column_trapdev.seleccion);
			column0.Resizable = true;
			column0.SortColumnId = (int) Column_trapdev.seleccion;

			text_ = new CellRendererText ();
			text_.Xalign = 0.0f;
			text_.Editable = true;
			text_.Edited += NumberCellEdited_Autorizado;
			columns.Add (text_);
			TreeViewColumn column1 = new TreeViewColumn("Cantidad",text_,"text",Column_trapdev.cantidad);
			column1.Resizable = true;
			column1.SortColumnId = (int) Column_trapdev.cantidad;

			text_ = new CellRendererText ();
			text_.Xalign = 0.0f;
			text_.Editable = true;
			columns.Add (text_);
			TreeViewColumn column2 = new TreeViewColumn("ID Prod.",text_,"text",Column_trapdev.idproducto);
			column2.Resizable = true;
			column2.SortColumnId = (int) Column_trapdev.idproducto;

			text_ = new CellRendererText ();
			text_.Xalign = 0.0f;
			text_.Editable = true;
			columns.Add (text_);
			TreeViewColumn column3 = new TreeViewColumn("Desc. Producto",text_,"text",Column_trapdev.descripprod);
			column3.Resizable = true;
			column3.SortColumnId = (int) Column_trapdev.descripprod;

			text_ = new CellRendererText ();
			text_.Xalign = 0.0f;
			text_.Editable = true;
			columns.Add (text_);
			TreeViewColumn column4 = new TreeViewColumn("Stock",text_,"text",Column_trapdev.stock);
			column4.Resizable = true;
			column4.SortColumnId = (int) Column_trapdev.stock;

			treeview_autoriza_devotras.InsertColumn (column0, (int) Column_trapdev.seleccion);
			treeview_autoriza_devotras.InsertColumn (column1, (int) Column_trapdev.cantidad);
			treeview_autoriza_devotras.InsertColumn (column2, (int) Column_trapdev.idproducto);
			treeview_autoriza_devotras.InsertColumn (column3, (int) Column_trapdev.descripprod);
			treeview_autoriza_devotras.InsertColumn (column4, (int) Column_trapdev.stock);
		}

		// Cuando selecciona la columna con la propiedad toogled
		void selecciona_fila(object sender, ToggledArgs args)
		{
			TreeIter iter;
			if (treeview_trasp_devol.Model.GetIter (out iter,new TreePath (args.Path))) {
				bool old = (bool) treeview_trasp_devol.Model.GetValue (iter,0);
				treeview_trasp_devol.Model.SetValue(iter,0,!old);
			}
		}

		void on_button_quitar_productos_clicked(object sender, EventArgs args)
		{
			TreeModel model;
			TreeIter iterSelected;
			if(treeview_trasp_devol.Selection.GetSelected(out model, out iterSelected)){
				MessageDialog msgBox = new MessageDialog (MyWin,DialogFlags.Modal,
					MessageType.Question,ButtonsType.YesNo,"¿ Esta quitar el producto "+treeview_trasp_devol.Model.GetValue (iterSelected,3));
				ResponseType miResultado = (ResponseType)msgBox.Run ();
				msgBox.Destroy();
				if (miResultado == ResponseType.Yes){
					treeviewEnginetraspdevol.Remove(ref iterSelected);
				}
			}else{
				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
					MessageType.Error, ButtonsType.Close,"NO hay nada para quitar...");
				msgBoxError.Run ();						msgBoxError.Destroy();
			}
		}

		// Valida entradas que solo sean numericas, se utiliza eb ventana de
		// carga de producto
		[GLib.ConnectBefore ()]   	  // Esto es indispensable para que funcione    
		void onKeyPressEvent(object o, Gtk.KeyPressEventArgs args)
		{
			//Console.WriteLine(Convert.ToChar(args.Event.KeyValue));
			string misDigitos = ".0123456789ﾰﾱﾲﾳﾴﾵﾶﾷﾸﾹﾮｔｒｓｑ（）";
			if (Array.IndexOf(misDigitos.ToCharArray(), Convert.ToChar(args.Event.Key)) == -1 && args.Event.Key.ToString()  != "BackSpace"){
				args.RetVal = true;
			}
		}
		
		void on_button_cargar_devoltra_clicked(object sender, EventArgs args)
		{
			NpgsqlConnection conexion; 
			conexion = new NpgsqlConnection (connectionString+nombrebd);
			// Verifica que la base de datos este conectada
			try{
				treeviewEnginetraspdevol.Clear();
				conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand ();               	
				comando.CommandText = "SELECT osiris_erp_movtraspdevo.id_producto AS idproducto " +
							"FROM osiris_erp_movtraspdevo " +
							"WHERE id_almacen = '"+idalmacen.ToString().Trim()+"';";
				Console.WriteLine(comando.CommandText);
				NpgsqlDataReader lector = comando.ExecuteReader ();
				while (lector.Read()){
					
				}
				
			}catch (NpgsqlException ex){
				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
					MessageType.Error,ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
				msgBoxError.Run ();				msgBoxError.Destroy();
			}
			conexion.Close ();
		}
		
		
		void NumberCellEdited_Autorizado (object o, EditedArgs args)
		{
			Gtk.TreeIter iter;
			bool esnumerico = false;
			int var_paso = 0;
			int largo_variable = args.NewText.ToString().Length;
			string toma_variable = args.NewText.ToString();

			treeviewEnginetraspdevol.GetIter (out iter, new Gtk.TreePath (args.Path));

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
				treeviewEnginetraspdevol.SetValue(iter,(int) Column_trapdev.cantidad,args.NewText);
			}
		}
				
		void on_cierraventanas_clicked(object sender, EventArgs args)
		{
			Widget win = (Widget) sender;
			win.Toplevel.Destroy();
		}
	}
}