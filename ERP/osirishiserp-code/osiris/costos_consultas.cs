///////////////////////////////////////////////////////
// created on 17/10/2007 at 03:53 p
// Sistema Hospitalario OSIRIS
// Monterrey - Mexico
//
// Autor    	: Ing. Daniel Olivares (Programacion)
// Ajustes		: Ing. Daniel Olivares Oct. 2015 (Seleccionar por grupos)
//				 
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
// Proposito	: Consultas de Costos
// Objeto		: 
//////////////////////////////////////////////////////////	
using System;
using Npgsql;
using System.Data;
using Gtk;
using Glade;

namespace osiris
{
	public class costos_consultas
	{
		// Boton general para salir de las ventanas
		// Todas la ventanas en glade este boton debe estra declarado identico
		[Widget] Gtk.Button button_salir;
		//[Widget] Gtk.Button button_buscar;
		[Widget] Gtk.Button button_imprimir;
		[Widget] Gtk.Entry entry_expresion;
		
		// Declarando ventana del menu de costos
		[Widget] Gtk.Window menu_costos;
		[Widget] Gtk.Button button_movtotal_producto;		
		[Widget] Gtk.Button button_productos_aplicados;
		[Widget] Gtk.Button button_analisis_de_cargos;
		[Widget] Gtk.Button button_costeo_procedimiento;
		[Widget] Gtk.Button button_listas_precios;
		[Widget] Gtk.Button button_catalogo_productos;		
		[Widget] Gtk.Button button_farmacia;
		
		//Declarando Ventana de Reporte de Lista de Precios
		[Widget] Gtk.Window reporte_lista_de_precios;
		[Widget] Gtk.ComboBox combobox_tipo_paciente;
		//[Widget] Gtk.Entry entry_empresa_aseguradora;
		[Widget] Gtk.Button button_busca_empresas;
		[Widget] Gtk.Button button_buscar_emp;
		[Widget] Gtk.Entry entry_empresa_aseguradora;
		[Widget] Gtk.Button button_selecciona;
		[Widget] Gtk.CheckButton checkbutton_grupo;
		[Widget] Gtk.CheckButton checkbutton_grupo1;
		[Widget] Gtk.CheckButton checkbutton_grupo2;
		[Widget] Gtk.CheckButton checkbutton_tarjeta;
		[Widget] Gtk.CheckButton checkbutton_especiales;  
		[Widget] Gtk.RadioButton radiobutton_desglosado;
		[Widget] Gtk.RadioButton radiobutton_con_iva;
		[Widget] Gtk.RadioButton radiobutton_sin_iva;
		
		// declaracion de treeview
		[Widget] Gtk.TreeView lista_empresas;
		[Widget] Gtk.TreeView lista_grupo;
		[Widget] Gtk.TreeView lista_grupo1;
		[Widget] Gtk.TreeView lista_grupo2;		
		
		string connectionString;
		string nombrebd;
		string LoginEmpleado;
		string NomEmpleado;
		string AppEmpleado;
  		string ApmEmpleado;
    	
		string tipo_paciente = "";
		int id_tipopaciente = 0;  // toma el valor del tipo de paciente
		int id_empresa = 0;
		int id_aseguradora = 0;
		string idgrupoproducto;  
		bool accesocatalogoprod;
    	    
    	//Declaracion de ventana de error
		protected Gtk.Window MyWinError;
		protected Gtk.Window MyWin;
		private MessageDialog msgBox;
		
		//declaracion de treeview aseguradoras
		private TreeStore treeViewEngineaseguradoras;
		//private TreeStore treeViewEngineempresas;
		
		private ListStore treeViewEnginegrupos;
		private ListStore treeViewEnginegrupos1;
		private ListStore treeViewEnginegrupos2;
		//private ArrayList grupos;
		//private ArrayList arraycargosextras;
		
		class_conexion conexion_a_DB = new class_conexion();
		class_public classpublic = new class_public();
		
		public costos_consultas(string LoginEmp_, string NomEmpleado_, string AppEmpleado_, string ApmEmpleado_, string nombrebd_)
		{
			LoginEmpleado = LoginEmp_;
    		NomEmpleado = NomEmpleado_;
    		AppEmpleado = AppEmpleado_;
    		ApmEmpleado = ApmEmpleado_;
			connectionString = conexion_a_DB._url_servidor+conexion_a_DB._port_DB+conexion_a_DB._usuario_DB+conexion_a_DB._passwrd_user_DB;
			nombrebd = conexion_a_DB._nombrebd;
    		
			Glade.XML gxml = new Glade.XML (null, "costos.glade", "menu_costos", null);
			gxml.Autoconnect (this);
	        menu_costos.Show();
			
			button_movtotal_producto.Clicked += new EventHandler(on_button_movtotal_producto_clicked);
			button_productos_aplicados.Clicked += new EventHandler(on_button_productos_aplicados_clicked);
			button_listas_precios.Clicked += new EventHandler(on_button_listas_precios_clicked);
			button_costeo_procedimiento.Clicked += new EventHandler(on_button_costeo_procedimiento_clicked);
			button_catalogo_productos.Clicked += new EventHandler(on_button_catalogo_productos_clicked);
			button_farmacia.Clicked += new EventHandler(on_button_button_farmacia_clicked);
			button_analisis_de_cargos.Clicked += new EventHandler(on_button_analisis_de_cargos_clicked);
			button_salir.Clicked += new EventHandler(on_cierraventanas_clicked);			
		}
		
		void on_button_movtotal_producto_clicked (object sender, EventArgs args)
		{
			new osiris.movimiento_mensual(LoginEmpleado,NomEmpleado,AppEmpleado,ApmEmpleado,nombrebd);
		}
		
		void on_button_catalogo_productos_clicked (object sender, EventArgs args)
		{
			if((string) classpublic.lee_registro_de_tabla("osiris_empleado","acceso_catalogo_producto","WHERE acceso_catalogo_producto = 'true' AND login_empleado = '"+LoginEmpleado+"' ","acceso_catalogo_producto","bool") == "True"){
				new osiris.catalogo_productos_nuevos(LoginEmpleado,NomEmpleado,AppEmpleado,ApmEmpleado,nombrebd);
			}else{
				msgBox = new MessageDialog (MyWin,DialogFlags.Modal,
										MessageType.Error,ButtonsType.Ok,"No tiene Autorizacion para esta opcion, verifique...");
				ResponseType miResultado = (ResponseType) msgBox.Run ();				msgBox.Destroy();
			}
		}
		void on_button_analisis_de_cargos_clicked(object sender, EventArgs args)
		{
			new osiris.movimientos_productos(LoginEmpleado,NomEmpleado,AppEmpleado,ApmEmpleado,nombrebd,"cargos_x_fecha","");
		}
		
		void on_button_button_farmacia_clicked(object sender, EventArgs args)
		{
			new osiris.rpt_compras_farmacia(LoginEmpleado,NomEmpleado,AppEmpleado,ApmEmpleado,nombrebd);
		}
		
		void on_button_costeo_procedimiento_clicked(object sender, EventArgs args)
		{
			new osiris.costeo_productos(LoginEmpleado,NomEmpleado,AppEmpleado,ApmEmpleado,nombrebd);  // costeo.cs
		}
		
		void on_button_productos_aplicados_clicked(object sender, EventArgs args)
		{
			new osiris.movimientos_productos(LoginEmpleado,NomEmpleado,AppEmpleado,ApmEmpleado,nombrebd,"cargos_pacientes","");
		}
		
		void on_button_listas_precios_clicked (object sender, EventArgs args)
		{
			Glade.XML gxml = new Glade.XML (null, "costos.glade", "reporte_lista_de_precios", null);
			gxml.Autoconnect (this);
			reporte_lista_de_precios.Show();
			
			llenado_tipo_paciente();
			crea_treeview_grupo();
			crea_treeview_grupo1();
			crea_treeview_grupo2();
			
			checkbutton_grupo.Clicked += new EventHandler(on_checkbutton_llenando_todos_grupo);
			checkbutton_grupo1.Clicked += new EventHandler(on_checkbutton_llenando_todos_grupo1);			
			checkbutton_grupo2.Clicked += new EventHandler(on_checkbutton_llenando_todos_grupo2);
			checkbutton_tarjeta.Clicked += new EventHandler(on_checkbutton_tarjeta_clicked);
			checkbutton_especiales.Clicked += new EventHandler(on_checkbutton_especiales_clicked);
			button_buscar_emp.Clicked += new EventHandler(on_button_buscar_emp_clicked);
			button_imprimir.Clicked += new EventHandler(on_button_imprimir_clicked);
			button_salir.Clicked += new EventHandler(on_cierraventanas_clicked); // esta sub-clase esta la final de la classe
		}
		
		void on_checkbutton_tarjeta_clicked(object sender, EventArgs args)
		{
		
			if(checkbutton_tarjeta.Active == true){
				combobox_tipo_paciente.Sensitive = false;
				button_buscar_emp.Sensitive = false;
				entry_empresa_aseguradora.Text = "";			
				checkbutton_especiales.Sensitive = false;
				combobox_tipo_paciente.Clear();
				lista_grupo.Sensitive = false;
				lista_grupo1.Sensitive = false;
				lista_grupo2.Sensitive = false;
				checkbutton_grupo.Sensitive = false;
				checkbutton_grupo1.Sensitive = false;
				checkbutton_grupo2.Sensitive = false;
			}else{
				combobox_tipo_paciente.Sensitive = true;
				button_buscar_emp.Sensitive = true;
				checkbutton_especiales.Sensitive = true;
				lista_grupo.Sensitive = true;
				lista_grupo1.Sensitive = true;
				lista_grupo2.Sensitive = true;
				checkbutton_grupo.Sensitive = true;
				checkbutton_grupo1.Sensitive = true;
				checkbutton_grupo2.Sensitive = true;
				
				llenado_tipo_paciente();
			}
			llena_grupos();
			
		}
		
		void on_checkbutton_especiales_clicked(object sender, EventArgs args)
		{
			if(checkbutton_especiales.Active == true){							
				checkbutton_tarjeta.Sensitive = false;
				lista_grupo.Sensitive = false;
				lista_grupo1.Sensitive = false;
				lista_grupo2.Sensitive = false;
				checkbutton_grupo.Sensitive = false;
				checkbutton_grupo1.Sensitive = false;
				checkbutton_grupo2.Sensitive = false;
			}else{
				checkbutton_tarjeta.Sensitive = true;
				lista_grupo.Sensitive = true;
				lista_grupo1.Sensitive = true;
				lista_grupo2.Sensitive = true;
				checkbutton_grupo.Sensitive = true;
				checkbutton_grupo1.Sensitive = true;
				checkbutton_grupo2.Sensitive = true;
			}

		}
			
		/////////////////////// BOTON BUSCAR aseguradora//////////////////////////////////////////////////////////
		void on_button_buscar_emp_clicked(object sender, EventArgs args)
		{
			Glade.XML gxml = new Glade.XML (null, "costos.glade", "busca_empresas", null);
			gxml.Autoconnect (this);
	      	
	      	crea_treeview_aseguradoras();
			button_busca_empresas.Clicked += new EventHandler(on_llena_lista);
			button_salir.Clicked +=  new EventHandler(on_cierraventanas_clicked);
			button_selecciona.Clicked +=  new EventHandler(on_selecciona_emp);
		}
		
		void crea_treeview_aseguradoras()
		{
			{
				treeViewEngineaseguradoras = new TreeStore(typeof(int),
														typeof(string));
				
				lista_empresas.RulesHint = true;
				lista_empresas.Model = treeViewEngineaseguradoras;
				lista_empresas.RowActivated += on_selecciona_emp;
								
				TreeViewColumn col_id_aseg = new TreeViewColumn();
				CellRendererText cellr0 = new CellRendererText();
				col_id_aseg.Title = "No. Identificacion";
				col_id_aseg.PackStart(cellr0, true);
				col_id_aseg.AddAttribute (cellr0, "text", 0);
				col_id_aseg.SortColumnId = (int) Col_aseg.col_id_aseg;
				
				TreeViewColumn col_ase = new TreeViewColumn();
				CellRendererText cellrt1 = new CellRendererText();
				col_ase.Title = "descripcion";
				col_ase.PackStart(cellrt1, true);
				col_ase.AddAttribute (cellrt1, "text", 1); 
				col_ase.SortColumnId = (int) Col_aseg.col_ase;
							           
				lista_empresas.AppendColumn(col_id_aseg);
				lista_empresas.AppendColumn(col_ase);
			}
		}
		
		enum Col_aseg
		{
			col_id_aseg,
			col_ase
		}
		
		// llena lista de Empresas o Aseguradoras
		void on_llena_lista(object sender, EventArgs args)
		{
			if (id_tipopaciente == 400){
				llenando_lista_de_aseguradoras();
			}else{
				llenando_lista_de_empresas();
			}
		}

		void llenando_lista_de_aseguradoras()
		{
			treeViewEngineaseguradoras.Clear(); // Limpia el treeview cuando realiza una nueva busqueda
			
			NpgsqlConnection conexion; 
			conexion = new NpgsqlConnection (connectionString+nombrebd);
            // Verifica que la base de datos este conectada
			try{
				conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand ();
				if ((string) entry_expresion.Text.ToUpper() == "*" || (string) entry_expresion.Text.ToUpper() == "")
				{
					comando.CommandText = "SELECT id_aseguradora,descripcion_aseguradora "+
								"FROM osiris_aseguradoras "+
								"WHERE lista_de_precio = true "+
								"ORDER BY id_aseguradora;";
				}else{
					comando.CommandText = "SELECT id_aseguradora,descripcion_aseguradora "+
								"FROM osiris_aseguradoras "+
								"WHERE descripcion_aseguradora LIKE '%"+(string) entry_expresion.Text.ToUpper()+"%' "+
								"lista_de_precio = true "+
								"ORDER BY descripcion_aseguradora;";
				}
				NpgsqlDataReader lector = comando.ExecuteReader ();
				//bool verifica_activacion;
				while (lector.Read()){	
					treeViewEngineaseguradoras.AppendValues ((int) lector["id_aseguradora"],//0
													(string) lector["descripcion_aseguradora"]);//1
				}
			}catch (NpgsqlException ex){
	   			MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
									MessageType.Error,ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
				msgBoxError.Run ();			msgBoxError.Destroy();
			}
			conexion.Close ();
		}
		
		void llenando_lista_de_empresas()
		{
			
			treeViewEngineaseguradoras.Clear();
			
			NpgsqlConnection conexion; 
			conexion = new NpgsqlConnection (connectionString+nombrebd);
            // Verifica que la base de datos este conectada
			try{
				conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand ();
				if ((string) entry_expresion.Text.ToUpper() == "*" || (string) entry_expresion.Text.ToUpper() == "")
				{
					comando.CommandText = "SELECT id_empresa,descripcion_empresa "+
								"FROM osiris_empresas "+
								"WHERE lista_de_precio = true "+								
								"ORDER BY id_empresa;";
				}else{
					comando.CommandText = "SELECT  id_empresa,descripcion_empresa "+
								"FROM osiris_empresas "+
								"WHERE descripcion_empresa LIKE '%"+(string) entry_expresion.Text.ToUpper()+"%' "+
								"lista_de_precio = true "+
								"ORDER BY descripcion_empresa;";
				}
				NpgsqlDataReader lector = comando.ExecuteReader ();
				//bool verifica_activacion;
				while (lector.Read())
				{	
					treeViewEngineaseguradoras.AppendValues ((int) lector["id_empresa"],//0
													(string) lector["descripcion_empresa"]);//1
				}
			}catch (NpgsqlException ex){
	   			MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
									MessageType.Error,ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
				msgBoxError.Run ();			msgBoxError.Destroy();
			}
			conexion.Close ();
		}
		
		void on_selecciona_emp(object sender, EventArgs args)
		{
			TreeModel model;
			TreeIter iterSelected;
			if (lista_empresas.Selection.GetSelected(out model, out iterSelected)){			
 				if (id_tipopaciente == 400){
 					this.id_aseguradora = (int) model.GetValue(iterSelected, 0);	
 					this.id_aseguradora = 0;	
 				}else{	
 					this.id_empresa = 0; 	
 					this.id_empresa = (int) model.GetValue(iterSelected, 0);
 			}
 				Widget win = (Widget) sender;
				win.Toplevel.Destroy();
				entry_empresa_aseguradora.Text = (string) model.GetValue(iterSelected, 1);
				llena_grupos();
			}
		}
		
		/// CREANDO TREEVIEW DE GRUPOS
		
		void crea_treeview_grupo()
		{
			treeViewEnginegrupos = new ListStore(typeof(bool), 
													typeof(string),
													typeof(string),
													typeof(string),
													typeof(string),
													typeof(string),
													typeof(string));
												
			lista_grupo.Model = treeViewEnginegrupos;
			
			lista_grupo.RulesHint = true;
				
			TreeViewColumn col_seleccion = new TreeViewColumn();
			CellRendererToggle cellr0 = new CellRendererToggle();
			col_seleccion.Title = "Seleccion"; // titulo de la cabecera de la columna, si está visible
			col_seleccion.PackStart(cellr0, true);
			//col_seleccion.SetCellDataFunc(cellr0, new TreeCellDataFunc (BoolCellDataFunc));  // funcion de columna
			col_seleccion.AddAttribute (cellr0, "active", 0);
			cellr0.Activatable = true;
			cellr0.Toggled += selecciona_fila_grupo; 
		
			TreeViewColumn col_producto = new TreeViewColumn();
			CellRendererText cellr1 = new CellRendererText();
			col_producto.Title = "descripcion"; // titulo de la cabecera de la columna, si está visible
			col_producto.PackStart(cellr1, true);
			col_producto.AddAttribute (cellr1, "text", 1);
			//cellr1.Editable = true;   // Permite edita este campo
			//cellr1.Edited += new EditedHandler (NumberCellEdited);
			cellr1.Foreground = "darkblue";
			
			lista_grupo.AppendColumn(col_seleccion);
			lista_grupo.AppendColumn(col_producto);
		}
	
		void llena_grupos()
		{
			llenando_lista_grupo();
			llenando_lista_grupo1();
			llenando_lista_grupo2();
		}

		void llenando_lista_grupo()
		{
			treeViewEnginegrupos.Clear(); // Limpia el treeview cuando realiza una nueva busqueda
			
			NpgsqlConnection conexion; 
			conexion = new NpgsqlConnection (connectionString+nombrebd);
            // Verifica que la base de datos este conectada
			try{	
				conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand ();
				comando.CommandText = "SELECT to_char(id_grupo_producto,'999999999') AS idgrupoproducto,descripcion_grupo_producto, "+
								"agrupacion,agrupacion2,agrupacion3,agrupacion4 "+
								"FROM osiris_grupo_producto "+
								"WHERE id_grupo_producto > 0 "+
								"ORDER BY id_grupo_producto;";
				
				NpgsqlDataReader lector = comando.ExecuteReader ();
				//bool verifica_activacion;
				while (lector.Read()){	
					treeViewEnginegrupos.AppendValues (false,
													(string) lector["descripcion_grupo_producto"],//0
													(string) lector["idgrupoproducto"],//1
													(string) lector["agrupacion"],//2
													(string) lector["agrupacion2"],//4
													(string) lector["agrupacion3"],//5
													(string) lector["agrupacion4"]);//6
				}
			}catch (NpgsqlException ex){
	   			MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
									MessageType.Error,ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
				msgBoxError.Run ();			msgBoxError.Destroy();
			}
			conexion.Close ();
		}
		
		 //DECLARACION TREEVIEW GRUPO 1 y 2
		void crea_treeview_grupo1()
		{
			treeViewEnginegrupos1 = new ListStore(typeof(bool), 
													typeof(string),
													typeof(string));
												
			lista_grupo1.Model = treeViewEnginegrupos1;
			lista_grupo1.RulesHint = true;
				
			TreeViewColumn col_seleccion = new TreeViewColumn();
			CellRendererToggle cellr0 = new CellRendererToggle();
			col_seleccion.Title = "Seleccion"; // titulo de la cabecera de la columna, si está visible
			col_seleccion.PackStart(cellr0, true);
			//col_seleccion.SetCellDataFunc(cellr0, new TreeCellDataFunc (BoolCellDataFunc));  // funcion de columna
			col_seleccion.AddAttribute (cellr0, "active", 0);
			cellr0.Activatable = true;
			cellr0.Toggled += selecciona_fila_grupo1; 
		
			TreeViewColumn col_producto = new TreeViewColumn();
			CellRendererText cellr1 = new CellRendererText();
			col_producto.Title = "descripcion"; // titulo de la cabecera de la columna, si está visible
			col_producto.PackStart(cellr1, true);
			col_producto.AddAttribute (cellr1, "text", 1);
			//cellr1.Editable = true;   // Permite edita este campo
			//cellr1.Edited += new EditedHandler (NumberCellEdited);
			cellr1.Foreground = "darkblue";
			
			lista_grupo1.AppendColumn(col_seleccion);
			lista_grupo1.AppendColumn(col_producto);
		}
		
		void llenando_lista_grupo1()
		{
			treeViewEnginegrupos1.Clear(); // Limpia el treeview cuando realiza una nueva busqueda
			
			NpgsqlConnection conexion; 
			conexion = new NpgsqlConnection (connectionString+nombrebd);
            // Verifica que la base de datos este conectada
			try{
				conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand ();

				comando.CommandText = "SELECT to_char(id_grupo1_producto,'999999999') AS idgrupo1producto,descripcion_grupo1_producto "+
								"FROM osiris_grupo1_producto "+
								"WHERE id_grupo1_producto > 0 "+
								"ORDER BY id_grupo1_producto;";
				
				NpgsqlDataReader lector = comando.ExecuteReader ();
				//bool verifica_activacion;
				while (lector.Read()){	
					treeViewEnginegrupos1.AppendValues (false,
													(string) lector["descripcion_grupo1_producto"],//0
													(string) lector["idgrupo1producto"]);//1
													
				}
			}catch (NpgsqlException ex){
	   			MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
									MessageType.Error,ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
				msgBoxError.Run ();			msgBoxError.Destroy();
			}
			conexion.Close ();
		}
		
		void selecciona_fila_grupo(object sender, ToggledArgs args)
		{
			TreeIter iter;
			TreePath path = new TreePath (args.Path);
			if (lista_grupo.Model.GetIter (out iter, path)) {
				bool old = (bool) lista_grupo.Model.GetValue (iter,0);
				lista_grupo.Model.SetValue(iter,0,!old);
			}
		}
		
		void selecciona_fila_grupo1(object sender, ToggledArgs args)
		{
			TreeIter iter;
			TreePath path = new TreePath (args.Path);
			if (lista_grupo1.Model.GetIter (out iter, path)) {
				bool old = (bool) lista_grupo1.Model.GetValue (iter,0);
				lista_grupo1.Model.SetValue(iter,0,!old);
			}
		}
	    void selecciona_fila_grupo2(object sender, ToggledArgs args)
			{
			TreeIter iter;
			TreePath path = new TreePath (args.Path);
			if (lista_grupo2.Model.GetIter (out iter, path)) {
				bool old = (bool) lista_grupo2.Model.GetValue (iter,0);
				lista_grupo2.Model.SetValue(iter,0,!old);
			}
		}
		
		void crea_treeview_grupo2()
		{
			treeViewEnginegrupos2 = new ListStore(typeof(bool), 
													typeof(string),
													typeof(string));
												
			lista_grupo2.Model = treeViewEnginegrupos2;
			lista_grupo2.RulesHint = true;
				
			TreeViewColumn col_seleccion = new TreeViewColumn();
			CellRendererToggle cellr0 = new CellRendererToggle();
			col_seleccion.Title = "Seleccion"; // titulo de la cabecera de la columna, si está visible
			col_seleccion.PackStart(cellr0, true);
			//col_seleccion.SetCellDataFunc(cellr0, new TreeCellDataFunc (BoolCellDataFunc));  // funcion de columna
			col_seleccion.AddAttribute (cellr0, "active", 0);
			cellr0.Activatable = true;
			cellr0.Toggled += selecciona_fila_grupo2; 
		
			TreeViewColumn col_producto = new TreeViewColumn();
			CellRendererText cellr1 = new CellRendererText();
			col_producto.Title = "descripcion"; // titulo de la cabecera de la columna, si está visible
			col_producto.PackStart(cellr1, true);
			col_producto.AddAttribute (cellr1, "text", 1);
			//cellr1.Editable = true;   // Permite edita este campo
			//cellr1.Edited += new EditedHandler (NumberCellEdited);
			cellr1.Foreground = "darkblue";
			
			lista_grupo2.AppendColumn(col_seleccion);
			lista_grupo2.AppendColumn(col_producto);
		}
		
		void llenando_lista_grupo2()
		{
			treeViewEnginegrupos2.Clear(); // Limpia el treeview cuando realiza una nueva busqueda
			
			NpgsqlConnection conexion; 
			conexion = new NpgsqlConnection (connectionString+nombrebd);
            // Verifica que la base de datos este conectada
			try{
				conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand ();

				comando.CommandText = "SELECT to_char(id_grupo2_producto,'999999999') AS idgrupo2producto,descripcion_grupo2_producto "+
									"FROM osiris_grupo2_producto "+
									"WHERE id_grupo2_producto > 0 "+
									"ORDER BY id_grupo2_producto;";
				
				NpgsqlDataReader lector = comando.ExecuteReader ();
				//bool verifica_activacion;
				while (lector.Read())
				{	
					treeViewEnginegrupos2.AppendValues (false,
													(string) lector["descripcion_grupo2_producto"],//0
													(string) lector["idgrupo2producto"]);//1
													
				}
			}catch (NpgsqlException ex){
	   			MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
									MessageType.Error,ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
				msgBoxError.Run ();			msgBoxError.Destroy();
			}
			conexion.Close ();
		}		
		
		void on_checkbutton_llenando_todos_grupo(object sender, EventArgs args)
		{
			if ((bool)checkbutton_grupo.Active == true){
				TreeIter iter2;
				if (this.treeViewEnginegrupos.GetIterFirst (out iter2)){
					lista_grupo.Model.SetValue(iter2,0,true);
					while (this.treeViewEnginegrupos.IterNext(ref iter2)){
						lista_grupo.Model.SetValue(iter2,0,true);
					}
				}
			}else{
				TreeIter iter2;
				if (this.treeViewEnginegrupos.GetIterFirst (out iter2)){
					lista_grupo.Model.SetValue(iter2,0,false);
					while (this.treeViewEnginegrupos.IterNext(ref iter2)){
						lista_grupo.Model.SetValue(iter2,0,false);
					}
				}
			}
		}

		void on_checkbutton_llenando_todos_grupo1(object sender, EventArgs args)
		{
			if ((bool)checkbutton_grupo1.Active == true){
				TreeIter iter2;
				if (this.treeViewEnginegrupos1.GetIterFirst (out iter2)){
					lista_grupo1.Model.SetValue(iter2,0,true);
					while (this.treeViewEnginegrupos1.IterNext(ref iter2)){
						lista_grupo1.Model.SetValue(iter2,0,true);
					}
				}
			}else{
				TreeIter iter2;
				if (this.treeViewEnginegrupos1.GetIterFirst (out iter2)){
					lista_grupo1.Model.SetValue(iter2,0,false);
					while (this.treeViewEnginegrupos1.IterNext(ref iter2)){
						lista_grupo1.Model.SetValue(iter2,0,false);
					}
				}
			}
		}

		void on_checkbutton_llenando_todos_grupo2(object sender, EventArgs args)
		{
			if ((bool)checkbutton_grupo2.Active == true){
				TreeIter iter2;
				if (this.treeViewEnginegrupos2.GetIterFirst (out iter2)){
					lista_grupo2.Model.SetValue(iter2,0,true);
					while (this.treeViewEnginegrupos2.IterNext(ref iter2)){
						lista_grupo2.Model.SetValue(iter2,0,true);
					}
				}
			}else{
				TreeIter iter2;
				if (this.treeViewEnginegrupos2.GetIterFirst (out iter2)){
					lista_grupo2.Model.SetValue(iter2,0,false);
					while (this.treeViewEnginegrupos2.IterNext(ref iter2)){
						lista_grupo2.Model.SetValue(iter2,0,false);
					}
				}
			}
		}
		///////////////////////////// COMBOBOX TIPO PACIENTE /////////////////////////////////////////////
		void llenado_tipo_paciente()
		{
			combobox_tipo_paciente.Clear();
			CellRendererText cell3 = new CellRendererText();
			combobox_tipo_paciente.PackStart(cell3, true);
			combobox_tipo_paciente.AddAttribute(cell3,"text",0);
	        
			ListStore store3 = new ListStore( typeof (string), typeof (int));
			combobox_tipo_paciente.Model = store3;
			
			store3.AppendValues ((string) "",(int) 0);
			
	        NpgsqlConnection conexion; 
			conexion = new NpgsqlConnection (connectionString+nombrebd);
            // Verifica que la base de datos este conectada
            //Console.WriteLine("si busca");
			try{
				conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand ();
               	comando.CommandText = "SELECT descripcion_tipo_paciente, id_tipo_paciente FROM osiris_his_tipo_pacientes "+
               						"WHERE lista_de_precio = true "+
               						"ORDER BY descripcion_tipo_paciente;";
				
				NpgsqlDataReader lector = comando.ExecuteReader ();
               	while (lector.Read())
				{
					store3.AppendValues ((string) lector["descripcion_tipo_paciente"],(int) lector["id_tipo_paciente"]);
					
				}
			}catch (NpgsqlException ex){
				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
										MessageType.Error,ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
				msgBoxError.Run ();				msgBoxError.Destroy();
			}
			conexion.Close ();
	        
			TreeIter iter3;
			if (store3.GetIterFirst(out iter3))	{ combobox_tipo_paciente.SetActiveIter (iter3); 
			}
			combobox_tipo_paciente.Changed += new EventHandler (onComboBoxChanged_tipo_paciente);
		}
		void onComboBoxChanged_tipo_paciente (object sender, EventArgs args)
		{
			ComboBox combobox_tipo_paciente = sender as ComboBox;
			if (sender == null) {	return;	}
			TreeIter iter;
			if (combobox_tipo_paciente.GetActiveIter (out iter)){
				tipo_paciente = (string) combobox_tipo_paciente.Model.GetValue(iter,0);
				id_tipopaciente = (int) combobox_tipo_paciente.Model.GetValue(iter,1);				
			}
		}
		
		void on_button_imprimir_clicked(object sender, EventArgs args)
		{
			/*
			new osiris.lista_de_precios(this.nombrebd, this.treeViewEnginegrupos, this.treeViewEnginegrupos1, this.treeViewEnginegrupos2,
										lista_grupo, lista_grupo1, lista_grupo2,
										this.checkbutton_especiales.Active,this.checkbutton_tarjeta.Active,
										id_tipopaciente,id_empresa,id_aseguradora,radiobutton_desglosado.Active,radiobutton_con_iva.Active,
										radiobutton_sin_iva.Active,entry_empresa_aseguradora);
			*/
		}
		
		// cierra ventanas emergentes
		void on_cierraventanas_clicked (object sender, EventArgs args)
		{
			Widget win = (Widget) sender;
			win.Toplevel.Destroy();
		}
	}
}