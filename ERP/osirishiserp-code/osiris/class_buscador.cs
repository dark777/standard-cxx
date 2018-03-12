/// <summary>
// 
//
// Autor    	: Daniel Olivares Cuevas - arcangeldoc@openmailbox.org (Programacion Mono)
//		  		  Daniel Olivares Cuevas - arcangeldoc@openmailbox.org (Diseño de Pantallas Glade)
// 				  
// Licencia		: GPL
// S.O. 		: GNU/Linux
//
// proyect Facturador is free software; you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation; either version 2 of the License, or
// (at your option) any later version.
// 
// proyect is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with Foobar; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
//
// Coment
//
// This module is copy the Project Sistema Hospitalario OSIRIS
//
/// </summary>

using Gtk;
using Gdk;
using System;
using Glade;
using Npgsql;
using System.Data;
using System.Collections;

namespace osiris
{		
	public class class_buscador
	{		
		//Declarando ventanas de busqueda
		[Widget] Gtk.Window buscador = null;
		[Widget] Gtk.Entry entry_expresion = null;
		[Widget] Gtk.Entry entry_expresion2 = null;
		[Widget] Gtk.Button button_buscar_busqueda = null;
		[Widget] Gtk.Button button_selecciona = null;
		[Widget] Gtk.Button button_salir = null;
		[Widget] Gtk.ComboBox combobox_busqueda1 = null;
		[Widget] Gtk.ComboBox combobox_busqueda2 = null;
		[Widget] Gtk.Entry entry_cantidad_producto = null;
		[Widget] Gtk.Label labelcantidad = null;
		[Widget] Gtk.ComboBox combobox_orden_por = null;
		[Widget] Gtk.CheckButton checkbutton_busqueda2 = null;
		[Widget] Gtk.TreeView lista_de_busqueda = null;
		[Widget] Gtk.Button button_nuevo = null;
		
		TreeStore treeViewEngineBuscador;

		Gtk.TreeView treeviewobject = null;
		Gtk.ListStore treeViewEngine = null;
		
		ArrayList arraylistobject;
		
		// Busqueda de Clientes para los reportes
		Gtk.Entry entry_nombre_cliente = null;
		Gtk.Entry entry_id_cliente = null;
		
		// Busqueda de Estados y Regiones para los catalogos
		Gtk.Entry entry_id_estado = null;
		Gtk.Entry entry_estado = null;
		Gtk.ToggleButton togglebutton_editar_estado = null;
		
		Gtk.Entry entry_id_municipio = null; 
		Gtk.Entry entry_municipio = null;
		Gtk.Frame frame2 = null;		
		Gtk.ToggleButton togglebutton_editar_municipio = null;
		Gtk.Button button_guardar_municipio = null; 
		
		// Busqueda de Grupo de Grupo de Productos 0
		Gtk.Entry entry_id_grupo = null;
		Gtk.Entry entry_descripcion_grupo = null;
		Gtk.ToggleButton togglebutton_editar_grupo = null;
		Gtk.Button button_guardar_grupo = null;
		Gtk.CheckButton checkbutton_activar_grupo = null;
		Gtk.Entry entry_porcentage_utilidad = null;
		Gtk.Entry entry_id_centrocosto = null;
		Gtk.Entry entry_descripcion_centrocosto = null;
		Gtk.Button button_buscar_centrocosto = null;
		
		// Busqueda de Grupo1 o Familia1 de Productos
		Gtk.Entry entry_id_grupo1 = null;
		Gtk.Entry entry_descripcion_grupo1 = null;
		Gtk.ToggleButton togglebutton_editar_grupo1 = null;
		Gtk.Button button_guardar_grupo1 = null;
		Gtk.Button button_buscar_grupo1 = null;
		Gtk.CheckButton checkbutton_activar_grupo1 = null;
		
		// Busqueda de Grupo2 o Familia1 de Productos
		Gtk.Entry entry_id_grupo2 = null;
		Gtk.Entry entry_descripcion_grupo2 = null;
		Gtk.ToggleButton togglebutton_editar_grupo2 = null;
		Gtk.Button button_guardar_grupo2 = null;
		Gtk.Button button_buscar_grupo2 = null;
		Gtk.CheckButton checkbutton_activar_grupo2 = null;
		
		// Busqueda de Marca de Productos
		Gtk.Entry entry_idmarca_producto = null;
		Gtk.Entry entry_descripcion_marca_producto = null;
		
		// Busqueda de Proveedores
		Gtk.Entry entry_id_proveedor = null;
		Gtk.Entry entry_nombre_proveedor = null;
		Gtk.Entry entry_formapago = null;
		Gtk.Entry entry_direccion_proveedor = null;
		Gtk.Entry entry_tel_proveedor = null;
		Gtk.Entry entry_contacto_proveedor = null;
		Gtk.Entry entry_rfc_proveedor = null;
				
		// Busqueda de Especialidades Medicas
		Gtk.Entry entry_id_especialidad = null;
		Gtk.Entry entry_especialidad = null;
		
		// Busqueda de Empresas-enlase por tipo de pacientes
		Gtk.Entry entry_id_empaseg_cita = null;
		Gtk.Entry entry_nombre_empaseg_cita = null;
		
		Gtk.Entry entry_id_empaseg_qx = null;
		Gtk.Entry entry_nombre_empaseg_qx = null;
		
		Gtk.Entry entry_edit_idaseguradora = null;
		Gtk.Entry entry_nombre_aseguradora = null;
		
		Gtk.Entry entry_edit_idinstempr = null;
		Gtk.Entry entry_nombre_instempr = null;
		
		// Busqueda doctor cita de paciente
		Gtk.Entry entry_id_doctor_cita = null;
		Gtk.Entry entry_nombre_doctor_cita = null;
		
		// Busqueda doctor cita de paciente
		Gtk.Entry entry_id_doctor_consulta = null;
		Gtk.Entry entry_nombre_doctor_consulta = null;
		
		// Busqueda Especilidad en cita
		Gtk.Entry entry_id_especialidad_cita = null;
		Gtk.Entry entry_nombre_especialidad_cita = null;

		Gtk.Entry entry_id_medico_admision = null;
		Gtk.Entry entry_nombre_medico_admision = null;
		Gtk.Entry entry_especialidad_medico_admision = null;
		Gtk.Entry entry_cedula_medico_admision = null;
		Gtk.Entry entry_tel_medico_admision = null;

		Gtk.Entry entry_id_medico_ref = null;
		Gtk.Entry entry_medico_ref = null;
		Gtk.Entry entry_espmed_ref = null;

		// Busqueda espacialidad cita qx.
		Gtk.Entry entry_id_especialidad_qx = null;
		Gtk.Entry entry_nombre_especialidad_qx = null;
		
		// Busqueda Especilidad en cita
		Gtk.Entry entry_id_especialidad_consulta = null;
		Gtk.Entry entry_nombre_especialidad_consulta = null;
		
		//Busqueda de paciente_cita
		Gtk.Entry entry_pid_paciente_cita = null;
		Gtk.Entry entry_nombre_paciente_cita1 = null;
		Gtk.Entry entry_fecha_nac_cita = null;
		Gtk.Entry entry_edad_paciente_cita = null;
		
		//Busqueda de doctor en Cita de Quirofano
		Gtk.Entry entry_id_doctor_qx = null;
		Gtk.Entry entry_nombre_doctor_qx = null;
		
		// Busqueda de Cirugia y/o Paquete quirirugico
		Gtk.Entry entry_id_cirugia = null;
		Gtk.Entry entry_cirugia = null;
		Gtk.CheckButton checkbutton_paquete_sino = null;		
		
		// Busqueda de Pacientes con numero de Atencion o Evolucion
		Gtk.Entry entry_folio_servicio = null;
		Gtk.Entry entry_pid_paciente = null;
		Gtk.Entry entry_nombre_paciente = null;
		
		// Busqueda de almacen en iventario fisico
		Gtk.Entry entry_id_almacen = null;
		Gtk.Entry entry_almacen = null;
		
		// busqueda del producto con id y descripcion de producto
		Gtk.Entry entry_id_producto = null;
		Gtk.Entry entry_descripcion_producto = null;
		
		// Busqueda para el CIE10
		Gtk.Entry entry_id_cie10 = null;
		Gtk.Entry entry_descrip_cie10 = null;

		// busca el medico para visita medica de especialidad
		Gtk.Entry entry_id_medtratante = null;
		Gtk.Entry entry_nom_medtratante = null;
		Gtk.Entry entry_espe_medtratante = null;

		// Marca de Producto
		Gtk.Entry entry_idmarca = null;
		Gtk.Entry entry_marcaproducto = null;
		Gtk.CheckButton checkbutton_activar = null;
		Gtk.ToggleButton togglebutton_editar = null;

		// Busqueda de producto en parametros de laboratorio
		Gtk.Entry entry_idproducto = null;
		Gtk.Entry entry_descripcion_estudio = null;

		// ventana de registro Admision
		Gtk.Window registro = null;
		
		class_conexion conexion_a_DB = new class_conexion();
		class_public classpublic = new class_public();

		int[] args_id_array = {0,1,2,3,4,5,6,7,8};
		string sql_busqueda1;
		string sql_comodin1;
		string sql_busqueda2;
		string sql_comodin2;
		string type_find = "";
		string string_sql="";

		string connectionString;
		string nombrebd;

		string LoginEmpleado;
		string NomEmpleado;
		string AppEmpleado;
		string ApmEmpleado;
		int id_tipopaciente;
		int idempresa_paciente;
		int idaseguradora_paciente;
		float valoriva;
		bool aplica_precios_aseguradoras;	// Toma el valor de si se tiene creado la lista de precio en la tabla de Productos
		bool aplica_precios_empresas;		// Toma el valor de si se tiene creado la lista de precio en la tabla
		int idtipointernamiento; 
		int idsubalmacen;


		//declaracion de columnas y celdas de treeview de busqueda
		TreeViewColumn col_buscador0;	CellRendererText cellrt0;
		TreeViewColumn col_buscador1;	CellRendererText cellrt1;
		TreeViewColumn col_buscador2; 	CellRendererToggle cellrt2;
		TreeViewColumn col_buscador3;	CellRendererText cellrt3;
		TreeViewColumn col_buscador4;	CellRendererText cellrt4;
		TreeViewColumn col_buscador5;	CellRendererText cellrt5;
		TreeViewColumn col_buscador6;	CellRendererText cellrt6;
		TreeViewColumn col_buscador7;	CellRendererText cellrt7;
		TreeViewColumn col_buscador8;	CellRendererText cellrt8;
				
		//Declaracion de ventana de error y mensaje
		protected Gtk.Window MyWinError;
		
		public void buscandor(object[] args, string[] args_sql_, string[] args_varible_, string type_find_,int typeseek_,string [,] args_buscador_1,string [,] args_buscador_2,string [,] args_orderby_)
		{
			connectionString = conexion_a_DB._url_servidor+conexion_a_DB._port_DB+conexion_a_DB._usuario_DB+conexion_a_DB._passwrd_user_DB;
			nombrebd = conexion_a_DB._nombrebd;
			valoriva = float.Parse(classpublic.ivaparaaplicar);
			Glade.XML gxml = new Glade.XML (null, "osiris.glade", "buscador", null);
			gxml.Autoconnect(this);
			buscador.Title = "Buscador "+type_find_;
	        //Muestra ventana de Glade
			buscador.Show();
			entry_cantidad_producto.Hide();
			labelcantidad.Hide();
			button_nuevo.Hide ();
			crea_treeview_busqueda(type_find_);
												
			//Console.WriteLine("nº de argumentos: {0}", args.Length);
			//for (int i = 0; i < args.Length; i++)
        	//Console.WriteLine("args[{0}] = {1} {2}", i, args[i],@args[i]);  
			switch (type_find_){	
				case "find_client":
					entry_id_cliente = (object) args[0] as Gtk.Entry;
					entry_nombre_cliente = (object) args[1] as Gtk.Entry;				
				break;		
				case "find_estado_region":
					entry_id_estado = (object) args[0] as Gtk.Entry;
					entry_estado = (object) args[1] as Gtk.Entry;
					togglebutton_editar_estado = (object) args[2] as Gtk.ToggleButton;
					frame2 = (object) args[3] as Gtk.Frame;
					togglebutton_editar_municipio = (object) args[4] as Gtk.ToggleButton;
					button_guardar_municipio = (object) args[5] as Gtk.Button;
					entry_municipio = (object) args[6] as Gtk.Entry;
					entry_id_municipio = (object) args[7] as Gtk.Entry;
				break;			
				case "find_municipio":
					entry_id_municipio = (object) args[0] as Gtk.Entry;
					entry_municipio = (object) args[1] as Gtk.Entry;
					togglebutton_editar_municipio = (object) args[2] as Gtk.ToggleButton;
					button_guardar_municipio = (object) args[3] as Gtk.Button;
				break;				
				case "find_grupo_producto":
					entry_id_grupo = (object) args[0] as Gtk.Entry;
					entry_descripcion_grupo = (object) args[1] as Gtk.Entry;
					togglebutton_editar_grupo = (object) args[2] as Gtk.ToggleButton;
					button_guardar_grupo = (object) args[3] as Gtk.Button;
					checkbutton_activar_grupo  = (object) args[4] as Gtk.CheckButton;
					entry_porcentage_utilidad = (object) args[5] as Gtk.Entry;
					entry_id_centrocosto = (object) args[6] as Gtk.Entry;
					entry_descripcion_centrocosto = (object) args[7] as Gtk.Entry;
					button_buscar_centrocosto  = (object) args[8] as Gtk.Button;
				break;				
				case "find_grupo1_producto":
					entry_id_grupo1 = (object) args[0] as Gtk.Entry;
					entry_descripcion_grupo1 = (object) args[1] as Gtk.Entry;
					togglebutton_editar_grupo1 = (object) args[2] as Gtk.ToggleButton;
					button_guardar_grupo1 = (object) args[3] as Gtk.Button;
					checkbutton_activar_grupo1  = (object) args[4] as Gtk.CheckButton;				
				break;				
				case "find_grupo2_producto":
					entry_id_grupo2 = (object) args[0] as Gtk.Entry;
					entry_descripcion_grupo2 = (object) args[1] as Gtk.Entry;
					togglebutton_editar_grupo2 = (object) args[2] as Gtk.ToggleButton;
					button_guardar_grupo2 = (object) args[3] as Gtk.Button;
					checkbutton_activar_grupo2  = (object) args[4] as Gtk.CheckButton;				
				break;				
				case "find_centrodecosto":
					entry_id_centrocosto = (object) args[0] as Gtk.Entry;
					entry_descripcion_centrocosto = (object) args[1] as Gtk.Entry;
				break;							
				case "find_marca_producto":
					entry_idmarca_producto = (object) args[0] as Gtk.Entry;
					entry_descripcion_marca_producto = (object) args[1] as Gtk.Entry;
				break;				
				case "find_proveedores":
					entry_id_proveedor = (object) args[0] as Gtk.Entry;
					entry_nombre_proveedor = (object) args[1] as Gtk.Entry;
					entry_formapago = (object) args[2] as Gtk.Entry;
				break;
				case "find_proveedores_id_nombre":
					entry_id_proveedor = (object) args[0] as Gtk.Entry;
					entry_nombre_proveedor = (object) args[1] as Gtk.Entry;
				break;
				case "find_proveedores_catalogo_producto":
					entry_id_proveedor = (object) args[0] as Gtk.Entry;
					entry_nombre_proveedor = (object) args[1] as Gtk.Entry;
					treeviewobject = (object) args[2] as Gtk.TreeView;
					treeViewEngine = (object) args[3] as Gtk.ListStore;
				break;
				case "find_proveedores_newoc":
					entry_id_proveedor = (object) args[0] as Gtk.Entry;
					entry_nombre_proveedor = (object) args[1] as Gtk.Entry;
					entry_formapago = (object) args[2] as Gtk.Entry;
					entry_direccion_proveedor  = (object) args[3] as Gtk.Entry;
					entry_tel_proveedor = (object) args[4] as Gtk.Entry;
					entry_rfc_proveedor = (object) args[5] as Gtk.Entry;
				break;
				case "find_proveedores_OC":
					entry_id_proveedor = (object) args[0] as Gtk.Entry;
					entry_nombre_proveedor = (object) args[1] as Gtk.Entry;
					entry_direccion_proveedor  = (object) args[2] as Gtk.Entry;
					entry_tel_proveedor  = (object) args[3] as Gtk.Entry;
					entry_contacto_proveedor  = (object) args[4] as Gtk.Entry;
					entry_formapago  = (object) args[5] as Gtk.Entry;
				break;
				case "find_especialidad_medica":
					entry_id_especialidad = (object) args[0] as Gtk.Entry;
					entry_especialidad = (object) args[1] as Gtk.Entry;
				break;				
				case "find_empresa_cita":
					entry_id_empaseg_cita = (object) args[0] as Gtk.Entry;
					entry_nombre_empaseg_cita = (object) args[1] as Gtk.Entry;
				break;				
				case "find_aseguradoras_cita":
					entry_id_empaseg_cita = (object) args[0] as Gtk.Entry;
					entry_nombre_empaseg_cita = (object) args[1] as Gtk.Entry;
				break;
				case "find_empresa_citaqx":
					entry_id_empaseg_qx = (object) args[0] as Gtk.Entry;
					entry_nombre_empaseg_qx = (object) args[1] as Gtk.Entry;
				break;
				case "find_aseguradora_edit":
					entry_edit_idaseguradora = (object) args[0] as Gtk.Entry;
					entry_nombre_aseguradora = (object) args[1] as Gtk.Entry;
					entry_edit_idinstempr = (object) args[2] as Gtk.Entry;
					entry_nombre_instempr = (object) args[3] as Gtk.Entry;
				break;
				case "find_instempres_edit":
					entry_edit_idinstempr = (object) args[0] as Gtk.Entry;
					entry_nombre_instempr = (object) args[1] as Gtk.Entry;
					entry_edit_idaseguradora = (object) args[2] as Gtk.Entry;
					entry_nombre_aseguradora = (object) args[3] as Gtk.Entry;
				break;
				case "find_aseguradoras_citaqx":
					entry_id_empaseg_qx = (object) args[0] as Gtk.Entry;
					entry_nombre_empaseg_qx = (object) args[1] as Gtk.Entry;
				break;				
				case "find_medico_cita":
					entry_id_doctor_cita = (object) args[0] as Gtk.Entry;
					entry_nombre_doctor_cita = (object) args[1] as Gtk.Entry;
				break;
				case "find_medico_consulta":
					entry_id_doctor_consulta = (object) args[0] as Gtk.Entry;
					entry_nombre_doctor_consulta = (object) args[1] as Gtk.Entry;
				break;
				case "find_especialidad_cita":
					entry_id_especialidad_cita = (object) args[0] as Gtk.Entry;
					entry_nombre_especialidad_cita = (object) args[1] as Gtk.Entry;
				break;
				case "find_especialidad_citaqx":
					entry_id_especialidad_qx = (object) args[0] as Gtk.Entry;
					entry_nombre_especialidad_qx = (object) args[1] as Gtk.Entry;
				break;
				case "find_especialidad_consulta":
					entry_id_especialidad_consulta = (object) args[0] as Gtk.Entry;
					entry_nombre_especialidad_consulta = (object) args[1] as Gtk.Entry;
				break;
				case "find_paciente_cita":
					entry_pid_paciente_cita = (object) args[0] as Gtk.Entry;
					entry_nombre_paciente_cita1 = (object) args[1] as Gtk.Entry;
					entry_fecha_nac_cita = (object) args[2] as Gtk.Entry;
					col_buscador1.Title = "Primer Nombre";
					col_buscador3.Title = "Segundo Nombre";
					col_buscador4.Title = "Apellido Paterno";
					col_buscador5.Title = "Apellido Materno";
					lista_de_busqueda.AppendColumn(col_buscador3);
					lista_de_busqueda.AppendColumn(col_buscador4);
					lista_de_busqueda.AppendColumn(col_buscador5);
				break;
				case "find_paciente_gestcobr":
					entry_pid_paciente = (object) args[0] as Gtk.Entry;
					entry_nombre_paciente = (object) args[1] as Gtk.Entry;
					col_buscador1.Title = "Primer Nombre";
					col_buscador3.Title = "Segundo Nombre";
					col_buscador4.Title = "Apellido Paterno";
					col_buscador5.Title = "Apellido Materno";
					col_buscador6.Title = "N° Atencion";
					col_buscador7.Title = "N° Pagare";
					lista_de_busqueda.AppendColumn(col_buscador6);
					lista_de_busqueda.AppendColumn(col_buscador7);
					lista_de_busqueda.AppendColumn(col_buscador3);
					lista_de_busqueda.AppendColumn(col_buscador4);
					lista_de_busqueda.AppendColumn(col_buscador5);
				break;
				case "find_cirugia_paquetes":
					entry_id_cirugia = (object) args[0] as Gtk.Entry;
					entry_cirugia = (object) args[1] as Gtk.Entry;
					registro = (object)args [2] as Gtk.Window;
				break;
				case "find_cirugia_presupuesto":
					entry_id_cirugia = (object) args[0] as Gtk.Entry;
					entry_cirugia = (object) args[1] as Gtk.Entry;
				break;
				case "find_paciente":
					entry_folio_servicio = (object)args [0] as Gtk.Entry;
					entry_pid_paciente = (object)args [1] as Gtk.Entry;
					entry_nombre_paciente = (object)args [2] as Gtk.Entry;
					col_buscador0.Title = "N° Atencion";
					col_buscador1.Title = "N° Expediente";
					col_buscador3.Title = "Primer Nombre";
					col_buscador4.Title = "Segundo Nombre";
					col_buscador5.Title = "Apellido Paterno";
					col_buscador6.Title = "Apellido Materno";
					lista_de_busqueda.AppendColumn(col_buscador3);
					lista_de_busqueda.AppendColumn(col_buscador4);
					lista_de_busqueda.AppendColumn(col_buscador5);
					lista_de_busqueda.AppendColumn(col_buscador6);
				break;
				case "find_paciente1":
					entry_pid_paciente = (object) args[0] as Gtk.Entry;
					entry_nombre_paciente = (object) args[1] as Gtk.Entry;
					col_buscador1.Title = "Primer Nombre";
					col_buscador3.Title = "Segundo Nombre";
					col_buscador4.Title = "Apellido Paterno";
					col_buscador5.Title = "Apellido Materno";
					lista_de_busqueda.AppendColumn(col_buscador3);
					lista_de_busqueda.AppendColumn(col_buscador4);
					lista_de_busqueda.AppendColumn(col_buscador5);
				break;
				case "find_paciente_admision1":
					//entry_pid_paciente = (object)args [0] as Gtk.Entry;
					col_buscador0.Title = "Nro. Exp.";
					col_buscador1.Title = "Primer Nombre";
					col_buscador3.Title = "Segundo Nombre";
					col_buscador4.Title = "Apellido Paterno";
					col_buscador5.Title = "Apellido Materno";
					col_buscador6.Title = "Fech.Nac.";
					col_buscador7.Title = "Edad";
					col_buscador8.Title = "Nomina";
					lista_de_busqueda.AppendColumn(col_buscador3);
					lista_de_busqueda.AppendColumn(col_buscador4);
					lista_de_busqueda.AppendColumn(col_buscador5);
					lista_de_busqueda.AppendColumn(col_buscador6);
					lista_de_busqueda.AppendColumn(col_buscador7);
					LoginEmpleado = (string) args_varible_[0];
					NomEmpleado = (string) args_varible_[1];
					AppEmpleado = (string) args_varible_[2];
					ApmEmpleado = (string) args_varible_[3];
					nombrebd = (string) args_varible_[4];
					button_nuevo.Show ();
					button_nuevo.Sensitive = false;
				break;
				case "find_paciente_admision2":
					registro = (object)args [0] as Gtk.Window;
					col_buscador0.Title = "Nro. Exp.";
					col_buscador1.Title = "Primer Nombre";
					col_buscador3.Title = "Segundo Nombre";
					col_buscador4.Title = "Apellido Paterno";
					col_buscador5.Title = "Apellido Materno";
					col_buscador6.Title = "Fech.Nac.";
					col_buscador7.Title = "Edad";
					col_buscador8.Title = "Nomina";
					lista_de_busqueda.AppendColumn(col_buscador3);
					lista_de_busqueda.AppendColumn(col_buscador4);
					lista_de_busqueda.AppendColumn(col_buscador5);
					lista_de_busqueda.AppendColumn(col_buscador6);
					lista_de_busqueda.AppendColumn(col_buscador7);
					LoginEmpleado = (string) args_varible_[0];
					NomEmpleado = (string) args_varible_[1];
					AppEmpleado = (string) args_varible_[2];
					ApmEmpleado = (string) args_varible_[3];
					nombrebd = (string) args_varible_[4];
				break;
				case "find_empresa_catalogo":
					registro = (object) args [0] as Gtk.Window;
				break;
				case "find_almacen_inventario":
					entry_id_almacen = (object) args[0] as Gtk.Entry;
					entry_almacen = (object) args[1] as Gtk.Entry;
				break;
				case "find_cirugia_paquetes_soliprod":
					entry_id_cirugia = (object) args[0] as Gtk.Entry;
					entry_cirugia = (object) args[1] as Gtk.Entry;
					treeviewobject = (object) args[2] as Gtk.TreeView;
					treeViewEngine = (object) args[3] as Gtk.ListStore;
				break;
				case "find_cirugia_cargos_modmedicos":
					arraylistobject = (object) args[0] as ArrayList;
					treeviewobject = (object) args[1] as Gtk.TreeView;
					treeViewEngine = (object) args[2] as Gtk.ListStore;
					LoginEmpleado = (string) args_varible_[0];
					id_tipopaciente = int.Parse((string) args_varible_[1]);
					idempresa_paciente = int.Parse((string) args_varible_[2]);
					idaseguradora_paciente = int.Parse((string) args_varible_[3]);
					aplica_precios_aseguradoras = (bool) Convert.ToBoolean((string) args_varible_[4]);
					aplica_precios_empresas = (bool) Convert.ToBoolean((string) args_varible_[5]);
					idtipointernamiento = int.Parse((string) args_varible_[6]);
					idsubalmacen = int.Parse((string) args_varible_[7]);
				break;
				case "find_producto_doc_medicos":
					entry_id_producto = (object) args[0] as Gtk.Entry;
					entry_descripcion_producto = (object) args[1] as Gtk.Entry;
				break;
				case "find_cie10":
					entry_id_cie10 = (object) args[0] as Gtk.Entry;
					entry_descrip_cie10 = (object) args[1] as Gtk.Entry;
					treeviewobject = (object) args[2] as Gtk.TreeView;
					treeViewEngine = (object) args[3] as Gtk.ListStore;
				break;
				case "find_medico_citaqx":
					entry_id_doctor_qx = (object) args[0] as Gtk.Entry;
					entry_nombre_doctor_qx = (object) args[1] as Gtk.Entry;
				break;
				case "find_medico_visitamedica":
					entry_id_medtratante = (object) args[0] as Gtk.Entry;
					entry_nom_medtratante = (object) args[1] as Gtk.Entry;
					entry_espe_medtratante = (object) args[2] as Gtk.Entry;
					treeviewobject = (object) args[3] as Gtk.TreeView;
					treeViewEngine = (object) args[4] as Gtk.ListStore;
					entry_folio_servicio = (object) args[5] as Gtk.Entry;
				break;
				case "find_marcaproducto":
					entry_idmarca = (object) args[0] as Gtk.Entry;
					entry_marcaproducto = (object) args[1] as Gtk.Entry;
				break;
				case "find_catalogo_marca":
					entry_idmarca = (object) args[0] as Gtk.Entry;
					entry_marcaproducto = (object) args[1] as Gtk.Entry;
					togglebutton_editar = (object) args[2] as Gtk.ToggleButton;
					checkbutton_activar = (object) args[4] as Gtk.CheckButton;
				break;
				case "find_medico_admision":
					entry_id_medico_admision = (object) args[0] as Gtk.Entry;
					entry_nombre_medico_admision = (object) args[1] as Gtk.Entry;
					entry_especialidad_medico_admision = (object) args[2] as Gtk.Entry;
					entry_cedula_medico_admision = (object) args[3] as Gtk.Entry;
					entry_tel_medico_admision = (object) args[4] as Gtk.Entry;
					lista_de_busqueda.AppendColumn(col_buscador3);
				break;
				case "find_medico_admision_referido":
					entry_id_medico_ref = (object) args[0] as Gtk.Entry;
					entry_medico_ref = (object) args[1] as Gtk.Entry;
					entry_espmed_ref = (object) args[2] as Gtk.Entry;
					lista_de_busqueda.AppendColumn(col_buscador3);
				break;
				case "find_laboratorio_param":
					entry_idproducto = (object) args[0] as Gtk.Entry;
					entry_descripcion_estudio = (object) args[1] as Gtk.Entry;
					registro = (object) args [2] as Gtk.Window;
				break;
			}
			string_sql = (string) args_sql_[0];
			type_find = type_find_;
			sql_busqueda1 = args_buscador_1 [0, 1];
			sql_comodin1 = args_buscador_1 [0, 2];
			sql_busqueda2 = args_buscador_2 [0, 1];
			sql_comodin2 = args_buscador_2 [0, 2];

			entry_expresion2.Sensitive = checkbutton_busqueda2.Active;
			combobox_busqueda2.Sensitive = checkbutton_busqueda2.Active;
			button_buscar_busqueda.Clicked += new EventHandler(on_llena_lista);
			entry_expresion.KeyPressEvent += onKeyPressEvent_enter;
			entry_expresion2.KeyPressEvent += onKeyPressEvent_enter;
	  		button_selecciona.Clicked += new EventHandler(on_selecciona_busqueda);
			checkbutton_busqueda2.Clicked += new EventHandler (on_checkbutton_busqueda2_clicked);
			button_nuevo.Clicked += new EventHandler (on_button_nuevo_clicked);
			button_salir.Clicked +=  new EventHandler(on_cierraventanas_clicked);
			llenado_combobox(0,"",combobox_busqueda1,"array","","","",args_buscador_1,args_id_array,"");
			llenado_combobox(0,"",combobox_busqueda2,"array","","","",args_buscador_2,args_id_array,"");
		}

		void on_button_nuevo_clicked (object sender, EventArgs args)
		{
			Widget win = (Widget) sender;
			win.Toplevel.Destroy();
			new osiris.registro_paciente_busca("nuevo",LoginEmpleado,NomEmpleado,AppEmpleado,ApmEmpleado,nombrebd,"1");
		}
						
		void llenado_combobox(int tipodellenado,string descrip_defaul,object obj,string sql_or_array,string query_SQL,string name_field_desc,string name_field_id,string[,] args_array,int[] args_id_array,string name_field_id2)
		{			
			Gtk.ComboBox combobox_llenado = (Gtk.ComboBox) obj;
			//Gtk.ComboBox combobox_pos_neg = obj as Gtk.ComboBox;
			combobox_llenado.Clear();
			CellRendererText cell = new CellRendererText();
			combobox_llenado.PackStart(cell, true);
			combobox_llenado.AddAttribute(cell,"text",0);	        
			ListStore store = new ListStore(typeof (string), typeof (int), typeof (int), typeof (string), typeof (string));
			combobox_llenado.Model = store;			
			if ((int) tipodellenado == 1){
				store.AppendValues ((string) descrip_defaul,0,1,"");
			}			
			if (sql_or_array == "array") {
				if (args_array.Length > 0){
					if (args_array.GetUpperBound (0) > 0) {
						for (int colum_field = 0; colum_field <= args_array.GetUpperBound (0); colum_field++) {
							store.AppendValues (args_array [colum_field, 0], args_id_array [colum_field], 1, args_array [colum_field, 1], args_array [colum_field, 2]);
						}
					}
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
					//Console.WriteLine(comando.CommandText);
					NpgsqlDataReader lector = comando.ExecuteReader ();
	               	while (lector.Read()){
						if(name_field_id2 == ""){
							store.AppendValues ((string) lector[ name_field_desc ], (int) lector[ name_field_id],1);
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
				case "combobox_busqueda1":
					sql_busqueda1 = (string) onComboBoxChanged.Model.GetValue (iter, 3);
					sql_comodin1 = (string) onComboBoxChanged.Model.GetValue (iter, 4);
					break;
				case "combobox_busqueda2":
					sql_busqueda2 = (string) onComboBoxChanged.Model.GetValue (iter, 3);
					sql_comodin2 = (string) onComboBoxChanged.Model.GetValue (iter, 4);
					break;
				}
			}
		}

		void on_checkbutton_busqueda2_clicked(object sender, EventArgs args)
		{
			entry_expresion2.Sensitive = checkbutton_busqueda2.Active;
			combobox_busqueda2.Sensitive = checkbutton_busqueda2.Active;
		}
		
		void crea_treeview_busqueda(string type_find_)
		{
			treeViewEngineBuscador = new TreeStore(typeof(string),
													typeof(string),
													typeof(bool),
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
													typeof(int),
													typeof(string),
													typeof(bool),
			                                        typeof(string),
			                                        typeof(string),
			                                        typeof(string));
												
			lista_de_busqueda.Model = treeViewEngineBuscador;			
			lista_de_busqueda.RulesHint = true;
			
			lista_de_busqueda.RowActivated += on_selecciona_busqueda;  // Doble click selecciono cliente*/
			col_buscador0 = new TreeViewColumn();
			cellrt0 = new CellRendererText();
			col_buscador0.Title = "ID"; // titulo de la cabecera de la columna, si está visible
			col_buscador0.PackStart(cellrt0, true);
			col_buscador0.AddAttribute (cellrt0, "text", 0);
			//col_idcliente.SetCellDataFunc(cellr0, new Gtk.TreeCellDataFunc(cambia_colores_cliente));
			col_buscador0.SortColumnId = (int) col_treview.col_buscador0;
			col_buscador0.Resizable = true;
			
			col_buscador1 = new TreeViewColumn();
			cellrt1 = new CellRendererText();
			col_buscador1.Title = "Descripcion";
			col_buscador1.PackStart(cellrt1, true);
			col_buscador1.AddAttribute (cellrt1, "text", 1);
			//col_descripcion.SetCellDataFunc(cellrt1, new Gtk.TreeCellDataFunc(cambia_colores_cliente));
			col_buscador1.SortColumnId = (int) col_treview.col_buscador1;
			col_buscador1.Resizable = true;
			
			col_buscador2 = new TreeViewColumn();
			cellrt2 = new CellRendererToggle();
			col_buscador2.Title = "Activo";
			col_buscador2.PackStart(cellrt2, true);
			col_buscador2.AddAttribute (cellrt2, "active", 2);
			//col_descripcion.SetCellDataFunc(cellrt1, new Gtk.TreeCellDataFunc(cambia_colores_cliente));
			col_buscador2.SortColumnId = (int) col_treview.col_buscador2;
			col_buscador2.Resizable = true;
			
			col_buscador3 = new TreeViewColumn();
			cellrt3 = new CellRendererText();
			col_buscador3.Title = "";
			col_buscador3.PackStart(cellrt3, true);
			col_buscador3.AddAttribute (cellrt3, "text", 3);
			//col_descripcion.SetCellDataFunc(cellrt1, new Gtk.TreeCellDataFunc(cambia_colores_cliente));
			col_buscador3.SortColumnId = (int) col_treview.col_buscador3;
			col_buscador3.Resizable = true;
			
			col_buscador4 = new TreeViewColumn();
			cellrt4 = new CellRendererText();
			col_buscador4.Title = "";
			col_buscador4.PackStart(cellrt4, true);
			col_buscador4.AddAttribute (cellrt4, "text", 4);
			//col_descripcion.SetCellDataFunc(cellrt1, new Gtk.TreeCellDataFunc(cambia_colores_cliente));
			col_buscador4.SortColumnId = (int) col_treview.col_buscador4;
			col_buscador4.Resizable = true;
			
			col_buscador5 = new TreeViewColumn();
			cellrt5 = new CellRendererText();
			col_buscador5.Title = "";
			col_buscador5.PackStart(cellrt5, true);
			col_buscador5.AddAttribute (cellrt5, "text", 5);
			//col_descripcion.SetCellDataFunc(cellrt1, new Gtk.TreeCellDataFunc(cambia_colores_cliente));
			col_buscador5.SortColumnId = (int) col_treview.col_buscador5;
			col_buscador5.Resizable = true;
			
			col_buscador6 = new TreeViewColumn();
			cellrt6 = new CellRendererText();
			col_buscador6.Title = "";
			col_buscador6.PackStart(cellrt6, true);
			col_buscador6.AddAttribute (cellrt6, "text", 6);
			//col_descripcion.SetCellDataFunc(cellrt1, new Gtk.TreeCellDataFunc(cambia_colores_cliente));
			col_buscador6.SortColumnId = (int) col_treview.col_buscador6;
			col_buscador6.Resizable = true;
			
			col_buscador7 = new TreeViewColumn();
			cellrt7 = new CellRendererText();
			col_buscador7.Title = "";
			col_buscador7.PackStart(cellrt7, true);
			col_buscador7.AddAttribute (cellrt7, "text", 7);
			//col_descripcion.SetCellDataFunc(cellrt1, new Gtk.TreeCellDataFunc(cambia_colores_cliente));
			col_buscador7.SortColumnId = (int) col_treview.col_buscador7;
			col_buscador7.Resizable = true;

			col_buscador8 = new TreeViewColumn();
			cellrt8 = new CellRendererText();
			col_buscador8.Title = "";
			col_buscador8.PackStart(cellrt7, true);
			col_buscador8.AddAttribute (cellrt7, "text", 7);
			//col_descripcion.SetCellDataFunc(cellrt1, new Gtk.TreeCellDataFunc(cambia_colores_cliente));
			col_buscador8.SortColumnId = (int) col_treview.col_buscador8;
			col_buscador8.Resizable = true;
				            
			lista_de_busqueda.AppendColumn(col_buscador0);
			lista_de_busqueda.AppendColumn(col_buscador1);
			//lista_de_busqueda.AppendColumn(col_buscador3);
			switch (type_find_) {	
			case "find_cirugia_paquetes":
				col_buscador3.Title = "Cliente";
				lista_de_busqueda.AppendColumn(col_buscador3);
				break;	
			}
		}
		
		enum col_treview
		{
			col_buscador0,col_buscador1,col_buscador2,col_buscador3,col_buscador4,col_buscador5,col_buscador6,col_buscador7,col_buscador8
		}
		
		void cambia_colores_fila(Gtk.TreeViewColumn column, Gtk.CellRenderer cell, Gtk.TreeModel model, Gtk.TreeIter iter)
		{
			if ((bool) this.lista_de_busqueda.Model.GetValue (iter,2) == false){
					(cell as Gtk.CellRendererText).Foreground = "red";
				}else{		
					(cell as Gtk.CellRendererText).Foreground = "black";
				}
			}
		
		void on_llena_lista(object sender, EventArgs args)
		{
			llenando_lista_de_lista();
		}
		
		void llenando_lista_de_lista()
		{
			if((string) entry_expresion.Text.Trim() !=""){
				string connectionString = conexion_a_DB._url_servidor+
										conexion_a_DB._port_DB+
										conexion_a_DB._usuario_DB+
										conexion_a_DB._passwrd_user_DB;
				string nombrebd = conexion_a_DB._nombrebd;
									
				treeViewEngineBuscador.Clear(); // Limpia el treeview cuando realiza una nueva busqueda
				NpgsqlConnection conexion; 
				conexion = new NpgsqlConnection (connectionString+nombrebd);
            	// Verifica que la base de datos este conectada
				try{
					conexion.Open ();
					NpgsqlCommand comando; 
					comando = conexion.CreateCommand ();
					if ((string) entry_expresion.Text.ToUpper() == "*"){
						comando.CommandText = string_sql;
					}else{
						if((bool) checkbutton_busqueda2.Active == true){
							comando.CommandText = string_sql + sql_busqueda1 + (string) entry_expresion.Text.ToUpper()+sql_comodin1+
																sql_busqueda2 + (string) entry_expresion2.Text.ToUpper()+sql_comodin2+";";
						}else{
							comando.CommandText = string_sql + sql_busqueda1 + (string) entry_expresion.Text.ToUpper()+sql_comodin1+";";
						}
					}
					//Console.WriteLine(comando.CommandText);
					NpgsqlDataReader lector = comando.ExecuteReader ();				
					while (lector.Read()){
						switch (type_find){	
							case "find_client":
								treeViewEngineBuscador.AppendValues ((string) lector["id_cliente"].ToString(),
													(string) lector["descripcion_cliente"]);
							break;			
							case "find_estado_region":
								treeViewEngineBuscador.AppendValues ((string) lector["id_estado"].ToString(),
													(string) lector["descripcion_estado"]);
							break;			
							case "find_municipio":
								treeViewEngineBuscador.AppendValues( (string) lector["id_municipio"].ToString(),
														(string) lector["descripcion_municipio"]);
							break;							
							case "find_grupo_producto":
								treeViewEngineBuscador.AppendValues( (string) lector["id_grupo_producto"].ToString(),
														(string) lector["descripcion_grupo_producto"],
							                            (bool) lector["activo_gp"],
							                            (string) Convert.ToString(lector["porcentage_utilidad_grupo"]),
							                            (string) Convert.ToString(lector["idcentrodecosto"]),
							                            (string) lector["descripcion_centro_de_costo"]);
							
								col_buscador0.SetCellDataFunc(cellrt0, new Gtk.TreeCellDataFunc(cambia_colores_fila));
								col_buscador1.SetCellDataFunc(cellrt1, new Gtk.TreeCellDataFunc(cambia_colores_fila));
							break;							
							case "find_grupo1_producto":
								treeViewEngineBuscador.AppendValues( (string) lector["id_grupo1_producto"].ToString(),
														(string) lector["descripcion_grupo1_producto"],
							                            (bool) lector["activo"]);
							
								col_buscador0.SetCellDataFunc(cellrt0, new Gtk.TreeCellDataFunc(cambia_colores_fila));
								col_buscador1.SetCellDataFunc(cellrt1, new Gtk.TreeCellDataFunc(cambia_colores_fila));
							break;							
							case "find_grupo2_producto":
								treeViewEngineBuscador.AppendValues( (string) lector["id_grupo2_producto"].ToString(),
														(string) lector["descripcion_grupo2_producto"],
							                            (bool) lector["activo"]);
							
								col_buscador0.SetCellDataFunc(cellrt0, new Gtk.TreeCellDataFunc(cambia_colores_fila));
								col_buscador1.SetCellDataFunc(cellrt1, new Gtk.TreeCellDataFunc(cambia_colores_fila));
							break;							
							case "find_centrodecosto":
								treeViewEngineBuscador.AppendValues( (string) lector["id_centro_de_costos"].ToString(),
														(string) lector["descripcion_centro_de_costo"]);
							
							break;							
							case "find_marca_producto":
								treeViewEngineBuscador.AppendValues ((string) lector["id_marca_producto"].ToString(),
													(string) lector["descripcion"]);
							break;							
							case "find_proveedores":
								treeViewEngineBuscador.AppendValues ((string) lector["id_proveedor"].ToString(),
													(string) lector["descripcion_proveedor"],
							                        (bool) lector["proveedor_activo"],
													(string) lector["direccion_proveedor"],
													(string) lector["colonia_proveedor"],
													(string) lector["municipio_proveedor"],
													(string) lector["estado_proveedor"],
													(string) lector["telefono1_proveedor"],
													(string) lector["contacto1_proveedor"],
													(string) lector["rfc_proveedor"],
													(string) lector["pagina_web_proveedor"],
													(string) lector["descripcion_forma_de_pago"]);
													//(string) lector["fax_proveedor"], //12
							                        //(int) lector["id_forma_de_pago"]);//13
							break;
							case "find_empresa_catalogo":
								treeViewEngineBuscador.AppendValues ((string) lector["id_empresa"].ToString(),
																	(string)lector["descripcion_empresa"]);
							break;
							case "find_proveedores_id_nombre":
								treeViewEngineBuscador.AppendValues ((string) lector["id_proveedor"].ToString(),
												(string) lector["descripcion_proveedor"],
								(bool) lector["proveedor_activo"]);								
							break;
							case "find_proveedores_newoc":
								treeViewEngineBuscador.AppendValues ((string) lector["id_proveedor"].ToString(),
													(string) lector["descripcion_proveedor"],
							                        (bool) lector["proveedor_activo"],
													(string) lector["direccion_proveedor"],
													(string) lector["colonia_proveedor"],
													(string) lector["municipio_proveedor"],
													(string) lector["estado_proveedor"],
													(string) lector["telefono1_proveedor"],
													(string) lector["contacto1_proveedor"],
													(string) lector["rfc_proveedor"],
													(string) lector["pagina_web_proveedor"],
													(string) lector["descripcion_forma_de_pago"]);
													//(string) lector["fax_proveedor"], //12
							                        //(int) lector["id_forma_de_pago"]);//13
							break;
							case "find_proveedores_catalogo_producto":
								treeViewEngineBuscador.AppendValues ((string) lector["id_proveedor"].ToString(),//0
													(string) lector["descripcion_proveedor"],//1
							                        (bool) lector["proveedor_activo"], //, // 2
													(string) lector["direccion_proveedor"],//3
													(string) lector["colonia_proveedor"],//4
													(string) lector["municipio_proveedor"],//5
													(string) lector["estado_proveedor"],//6
													(string) lector["telefono1_proveedor"],//7
													(string) lector["contacto1_proveedor"],//8
													(string) lector["rfc_proveedor"],//9
													(string) lector["pagina_web_proveedor"],//10
													(string) lector["descripcion_forma_de_pago"]);//11
													//(string) lector["fax_proveedor"], //12
							                        //(int) lector["id_forma_de_pago"]);//13
							break;							
							case "find_especialidad_medica":
								treeViewEngineBuscador.AppendValues ((string) lector["id_especialidad"].ToString(),
													(string) lector["descripcion_especialidad"]);
								
							break;
							case "find_empresa_cita":
								treeViewEngineBuscador.AppendValues ((string) lector["id_empresa"].ToString(),
													(string)lector["descripcion_empresa"]);
							break;
							case "find_aseguradoras_cita":
								treeViewEngineBuscador.AppendValues ((string) lector["id_aseguradora"].ToString(),
													(string)lector["descripcion_aseguradora"]);
							break;
							case "find_empresa_citaqx":
								treeViewEngineBuscador.AppendValues ((string) lector["id_empresa"].ToString(),
													(string)lector["descripcion_empresa"]);
							break;
							case "find_aseguradoras_citaqx":
								treeViewEngineBuscador.AppendValues ((string) lector["id_aseguradora"].ToString(),
													(string)lector["descripcion_aseguradora"]);
							break;
							case "find_aseguradora_edit":
								treeViewEngineBuscador.AppendValues ((string) lector["id_aseguradora"].ToString(),
													(string)lector["descripcion_aseguradora"]);
							break;							
							case "find_instempres_edit":
								treeViewEngineBuscador.AppendValues ((string) lector["id_empresa"].ToString(),
													(string)lector["descripcion_empresa"]);
							break;
							case "find_medico_cita":
								treeViewEngineBuscador.AppendValues ((string) lector["id_medico"].ToString(),
													(string)lector["nombre_medico"]);
							break;
							case "find_especialidad_citaqx":
								treeViewEngineBuscador.AppendValues ((string) lector["id_especialidad"].ToString(),
													(string)lector["descripcion_especialidad"]);
							break;
							case "find_medico_consulta":
								treeViewEngineBuscador.AppendValues ((string) lector["id_medico"].ToString(),
													(string)lector["nombre_medico"]);
							break;
							case "find_especialidad_cita":
								treeViewEngineBuscador.AppendValues ((string) lector["id_especialidad"].ToString(),
													(string)lector["descripcion_especialidad"]);
							break;
							case "find_especialidad_consulta":
								treeViewEngineBuscador.AppendValues ((string) lector["id_especialidad"].ToString(),
													(string)lector["descripcion_especialidad"]);
							break;
							case "find_paciente_cita":
								treeViewEngineBuscador.AppendValues ((string) lector["pid_paciente"].ToString(),
													(string) lector["nombre1_paciente"].ToString().Trim(),
							                        (bool) lector["activo"],
							                        (string) lector["nombre2_paciente"].ToString().Trim(),
							                        (string) lector["apellido_paterno_paciente"].ToString().Trim(),
							                        (string) lector["apellido_materno_paciente"].ToString().Trim(),
							                        (string) lector["fech_nacimiento"],
							              			(string) lector["edad"],
							                        (string) lector["fech_nacimiento"]);
							break;
							case "find_paciente_gestcobr":
								treeViewEngineBuscador.AppendValues ((string) lector["pid_paciente"].ToString(),
													(string) lector["nombre1_paciente"].ToString().Trim(),
							                        (bool) lector["activo"],
							                        (string) lector["nombre2_paciente"].ToString().Trim(),
							                        (string) lector["apellido_paterno_paciente"].ToString().Trim(),
							                        (string) lector["apellido_materno_paciente"].ToString().Trim(),
							                        (string) lector["fech_nacimiento"],
							              			(string) lector["edad"],
							                        (string) lector["fech_nacimiento"]);
							break;
							case "find_cirugia_paquetes":
								treeViewEngineBuscador.AppendValues ((string) lector["id_tipo_cirugia"].ToString(),
													(string) lector["descripcion_cirugia"],
													(bool) lector["tiene_paquete"],
													lector["cliente"].ToString().Trim(),
													lector["id_empresa"].ToString().Trim(),
													lector["id_aseguradora"].ToString().Trim());
							break;
							case "find_cirugia_presupuesto":
								treeViewEngineBuscador.AppendValues ((string) lector["id_tipo_cirugia"].ToString(),
																	(string) lector["descripcion_cirugia"],
																	(bool) lector["tiene_paquete"]);
							break;
							case "find_cirugia_paquetes_soliprod":
								treeViewEngineBuscador.AppendValues ((string) lector["id_tipo_cirugia"].ToString(),
							                                   	(string) lector["descripcion_cirugia"],
							                                     (bool) lector["tiene_paquete"]);
							break;
							case "find_cirugia_cargos_modmedicos":
									treeViewEngineBuscador.AppendValues ((string) lector["id_tipo_cirugia"].ToString(),
							                                   	(string) lector["descripcion_cirugia"],
							                                     (bool) lector["tiene_paquete"]);
							break;							
							case "find_paciente":
								treeViewEngineBuscador.AppendValues ((string) lector["folio_de_servicio"].ToString(),
													(string) lector["pidpaciente"].ToString().Trim(),
							                        (bool) lector["activo"],
							                        (string) lector["nombre1_paciente"].ToString().Trim(),
							                        (string) lector["nombre2_paciente"].ToString().Trim(),
							                        (string) lector["apellido_paterno_paciente"].ToString().Trim(),
							                        (string) lector["apellido_materno_paciente"].ToString().Trim(),
							                        (string) lector["fech_nacimiento"],
							              			(string) lector["edad"]);
							break;
							case "find_paciente1":
								treeViewEngineBuscador.AppendValues ((string) lector["pid_paciente"].ToString(),
													(string) lector["nombre1_paciente"].ToString().Trim(),
							                        (bool) lector["activo"],
							                        (string) lector["nombre2_paciente"].ToString().Trim(),
							                        (string) lector["apellido_paterno_paciente"].ToString().Trim(),
							                        (string) lector["apellido_materno_paciente"].ToString().Trim(),
							                        (string) lector["fech_nacimiento"],
							              			(string) lector["edad"]);
							break;
							case "find_paciente_admision1":
								treeViewEngineBuscador.AppendValues ((string) lector["pid_paciente"].ToString(),
													(string) lector["nombre1_paciente"].ToString().Trim(),
													(bool) lector["activo"],
													(string) lector["nombre2_paciente"].ToString().Trim(),
													(string) lector["apellido_paterno_paciente"].ToString().Trim(),
													(string) lector["apellido_materno_paciente"].ToString().Trim(),
													(string) lector["fech_nacimiento"],
													(string) lector["edad"],
													"");
							break;
							case "find_paciente_admision2":
								treeViewEngineBuscador.AppendValues ((string) lector["pid_paciente"].ToString(),
													(string) lector["nombre1_paciente"].ToString().Trim(),
													(bool) lector["activo"],
													(string) lector["nombre2_paciente"].ToString().Trim(),
													(string) lector["apellido_paterno_paciente"].ToString().Trim(),
													(string) lector["apellido_materno_paciente"].ToString().Trim(),
													(string) lector["fech_nacimiento"],
													(string) lector["edad"],
													"");							
							break;
							case "find_proveedores_OC":
								treeViewEngineBuscador.AppendValues ((string) lector["id_proveedor"].ToString(),
													(string) lector["descripcion_proveedor"],
							                        (bool) lector["proveedor_activo"],
													(string) lector["direccion_proveedor"],
													(string) lector["colonia_proveedor"],
													(string) lector["municipio_proveedor"],
													(string) lector["estado_proveedor"],
													(string) lector["telefono1_proveedor"],
													(string) lector["contacto1_proveedor"],
													(string) lector["rfc_proveedor"],
													(string) lector["pagina_web_proveedor"],
													(string) lector["descripcion_forma_de_pago"]);
							break;
							case "find_almacen_inventario":
								treeViewEngineBuscador.AppendValues((string) lector["id_almacen"].ToString(),
							                                  (string) lector["descripcion_almacen"]);
							break;
							case "find_producto_doc_medicos":
								treeViewEngineBuscador.AppendValues((string) lector["codProducto"].ToString(),
							                                  (string) lector["descripcion_producto"]);
							break;
							case "find_medico_citaqx":
								treeViewEngineBuscador.AppendValues ((string) lector["id_medico"].ToString(),
													(string)lector["nombre_medico"]);
							break;
							case "find_cie10":
								treeViewEngineBuscador.AppendValues((string) lector["id_diagnostico"].ToString(),
																(string) lector["descripcion_diagnostico"],
																false,
																(string) lector["id_cie_10"].ToString());							
							break;
							case "find_medico_visitamedica":
								treeViewEngineBuscador.AppendValues((string) lector["id_medico"].ToString(),
																	(string) lector["nombre_medico"],
																	true,
																lector["descripcion_especialidad"].ToString().Trim());
							break;
							case "find_marcaproducto":
								treeViewEngineBuscador.AppendValues(lector["id_marca_producto"].ToString(),
																(string) lector["descripcion_marca"]);
							break;
							case "find_catalogo_marca":
								treeViewEngineBuscador.AppendValues ((string) lector["id_marca_producto"].ToString(),
											(string) lector["descripcion_marca"],
											(bool) lector["activa"]);
							break;
							case "find_medico_admision":
								treeViewEngineBuscador.AppendValues((string) lector["id_medico"].ToString(),
												(string) lector["nombre_medico"],
												true,
												lector["descripcion_especialidad"].ToString().Trim(),
												lector["cedula_medico"].ToString().Trim(),
												lector["celular1_medico"].ToString().Trim());
							break;
							case "find_medico_admision_referido":
								treeViewEngineBuscador.AppendValues((string) lector["id_medico"].ToString(),
														(string) lector["nombre_medico"],
														true,
														lector["descripcion_especialidad"].ToString().Trim());								
							break;
							case "find_laboratorio_param":
								treeViewEngineBuscador.AppendValues((string) lector["codProducto"].ToString(),
													(string) lector["descripcion_producto"],
													true,
													lector["descripcion_grupo_producto"].ToString().Trim());
							break;
						}
					}
					if(type_find == "find_paciente_admision1"){
						button_nuevo.Sensitive = true;
					}
				}catch (NpgsqlException ex){
	   				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
									MessageType.Error,ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
					msgBoxError.Run ();			msgBoxError.Destroy();
				}
				conexion.Close ();
			}
		}
		
		void on_selecciona_busqueda(object sender, EventArgs args)
		{
			TreeModel model;
			TreeIter iterSelected;			
			if (lista_de_busqueda.Selection.GetSelected(out model, out iterSelected)){										
				string tomaid = (string) model.GetValue(iterSelected, 0);												
				switch (type_find){	
					case "find_client":
						entry_id_cliente.Text = tomaid.ToString();
						entry_nombre_cliente.Text = (string) model.GetValue(iterSelected, 1);
					break;
					
					case "find_estado_region":
						entry_id_estado.Text = tomaid.ToString();
						entry_estado.Text = (string) model.GetValue(iterSelected, 1);
						frame2.Sensitive = true;
						togglebutton_editar_estado.Sensitive = true;
						togglebutton_editar_estado.Active = false;
						togglebutton_editar_municipio.Sensitive = false;
						togglebutton_editar_municipio.Active = false;
						button_guardar_municipio.Sensitive = false;
						entry_municipio.Sensitive = false;
						entry_municipio.Text = "";
						entry_id_municipio.Text = "";
					break;
					
					case "find_municipio":
						entry_id_municipio.Text = tomaid.ToString();
						entry_municipio.Text = (string) model.GetValue(iterSelected, 1);
						togglebutton_editar_municipio.Sensitive = true;
						togglebutton_editar_municipio.Active = false;
						button_guardar_municipio.Sensitive = false;
					break;	
					
					case "find_grupo_producto":
						entry_id_grupo.Text = tomaid.ToString();
						entry_descripcion_grupo.Text = (string) model.GetValue(iterSelected, 1);
						checkbutton_activar_grupo.Active = (bool) model.GetValue(iterSelected, 2);
						entry_porcentage_utilidad.Text = (string) model.GetValue(iterSelected, 3);
						entry_id_centrocosto.Text = (string) model.GetValue(iterSelected, 4);
						entry_descripcion_centrocosto.Text = (string) model.GetValue(iterSelected, 5);
						button_guardar_grupo.Sensitive = false;
						togglebutton_editar_grupo.Sensitive = true;
						togglebutton_editar_grupo.Active = false;
						checkbutton_activar_grupo.Sensitive = false;
						entry_porcentage_utilidad.Sensitive = false;
						entry_id_centrocosto.Sensitive = false;
						entry_descripcion_centrocosto.Sensitive = false;
						button_buscar_centrocosto.Sensitive = false;
					break;
					case "find_grupo1_producto":
						entry_id_grupo1.Text = tomaid.ToString();
						entry_descripcion_grupo1.Text = (string) model.GetValue(iterSelected, 1);
						checkbutton_activar_grupo1.Active = (bool) model.GetValue(iterSelected, 2);						
						button_guardar_grupo1.Sensitive = false;
						togglebutton_editar_grupo1.Sensitive = true;
						togglebutton_editar_grupo1.Active = false;
						checkbutton_activar_grupo1.Sensitive = false;
					break;
					case "find_grupo2_producto":
						entry_id_grupo2.Text = tomaid.ToString();
						entry_descripcion_grupo2.Text = (string) model.GetValue(iterSelected, 1);
						checkbutton_activar_grupo2.Active = (bool) model.GetValue(iterSelected, 2);						
						button_guardar_grupo2.Sensitive = false;
						togglebutton_editar_grupo2.Sensitive = true;
						togglebutton_editar_grupo2.Active = false;
						checkbutton_activar_grupo2.Sensitive = false;
					break;					
					case "find_centrodecosto":
						entry_id_centrocosto.Text = tomaid.ToString();
						entry_descripcion_centrocosto.Text = (string) model.GetValue(iterSelected, 1);
					break;
					case "find_marca_producto":
						entry_idmarca_producto.Text = tomaid.ToString();
						entry_descripcion_marca_producto.Text = (string) model.GetValue(iterSelected, 1);
					break;
					case "find_especialidad_medica":
						entry_id_especialidad.Text = tomaid.ToString();
						entry_especialidad.Text = (string) model.GetValue(iterSelected, 1);
					break;
					case "find_empresa_cita":
						entry_id_empaseg_cita.Text = tomaid.ToString();
						entry_nombre_empaseg_cita.Text = (string) model.GetValue(iterSelected, 1);
					break;
					case "find_empresa_citaqx":
						entry_id_empaseg_qx.Text = tomaid.ToString();
						entry_nombre_empaseg_qx.Text = (string) model.GetValue(iterSelected, 1);
					break;
					case "find_aseguradoras_citaqx":
						entry_id_empaseg_qx.Text = tomaid.ToString();
						entry_nombre_empaseg_qx.Text = (string) model.GetValue(iterSelected, 1);
					break;
					case "find_aseguradoras_cita":
						entry_id_empaseg_cita.Text = tomaid.ToString();
						entry_nombre_empaseg_cita.Text = (string) model.GetValue(iterSelected, 1);
					break;
					case "find_aseguradora_edit":
						entry_edit_idaseguradora.Text = tomaid.ToString();
						entry_nombre_aseguradora.Text = (string) model.GetValue(iterSelected, 1);
						entry_edit_idinstempr.Text = "1";
						entry_nombre_instempr.Text = "";						
					break;
					case "find_instempres_edit":
						entry_edit_idinstempr.Text = tomaid.ToString();
						entry_nombre_instempr.Text = (string) model.GetValue(iterSelected, 1);
						entry_edit_idaseguradora.Text = "1";
						entry_nombre_aseguradora.Text = "";
					break;
					case "find_medico_cita":
						entry_id_doctor_cita.Text = tomaid.ToString();
						entry_nombre_doctor_cita.Text = (string) model.GetValue(iterSelected, 1);
					break;
					case "find_medico_consulta":
						entry_id_doctor_consulta.Text = tomaid.ToString();
						entry_nombre_doctor_consulta.Text = (string) model.GetValue(iterSelected, 1);
					break;
					case "find_especialidad_cita":
						entry_id_especialidad_cita.Text = tomaid.ToString();
						entry_nombre_especialidad_cita.Text = (string) model.GetValue(iterSelected, 1);
					break;
					case "find_especialidad_citaqx":
						entry_id_especialidad_qx.Text = tomaid.ToString();
						entry_nombre_especialidad_qx.Text = (string) model.GetValue(iterSelected, 1);
					break;
					case "find_especialidad_consulta":
						entry_id_especialidad_consulta.Text = tomaid.ToString();
						entry_nombre_especialidad_consulta.Text = (string) model.GetValue(iterSelected, 1);
					break;
					case "find_paciente_cita":
						entry_pid_paciente_cita.Text = tomaid.ToString();
						entry_nombre_paciente_cita1.Text = (string) model.GetValue(iterSelected, 1)+" "+
															(string) model.GetValue(iterSelected, 3)+" "+
															(string) model.GetValue(iterSelected, 4)+" "+
															(string) model.GetValue(iterSelected, 5);
					entry_fecha_nac_cita.Text = (string) model.GetValue(iterSelected, 8);
					break;
					case "find_paciente_gestcobr":
						entry_pid_paciente.Text = tomaid.ToString();
						entry_nombre_paciente.Text = (string) model.GetValue(iterSelected, 1)+" "+
															(string) model.GetValue(iterSelected, 3)+" "+
															(string) model.GetValue(iterSelected, 4)+" "+
															(string) model.GetValue(iterSelected, 5);
					break;
					case "find_cirugia_paquetes":
						entry_id_cirugia.Text = tomaid.ToString ();
						entry_cirugia.Text = (string)model.GetValue (iterSelected, 1);
						registro.Destroy ();
						new osiris.paquetes_cirugias(LoginEmpleado,NomEmpleado,AppEmpleado,ApmEmpleado,nombrebd,tomaid);
					break;
					case "find_cirugia_presupuesto":
						entry_id_cirugia.Text = tomaid.ToString();
						entry_cirugia.Text = (string) model.GetValue(iterSelected, 1);
					break;
					case "find_cirugia_paquetes_soliprod":
						entry_id_cirugia.Text = tomaid.ToString();
						entry_cirugia.Text = (string) model.GetValue(iterSelected, 1);
						carga_valores_treeview(int.Parse(tomaid),treeviewobject,treeViewEngine);
					break;
					case "find_cirugia_cargos_modmedicos":
						carga_valores_treeview_pq_modcargos(int.Parse(tomaid),treeviewobject,treeViewEngine);
					break;
					case "find_paciente":
						entry_folio_servicio.Text = tomaid.ToString();
						entry_pid_paciente.Text = (string) model.GetValue(iterSelected, 1);
						entry_nombre_paciente.Text = (string) model.GetValue(iterSelected, 3)+" "+
															(string) model.GetValue(iterSelected, 4)+" "+
															(string) model.GetValue(iterSelected, 5)+" "+
															(string) model.GetValue(iterSelected, 6);
					break;
					case "find_paciente1":
						entry_pid_paciente.Text = tomaid.ToString();
						entry_nombre_paciente.Text = (string) model.GetValue(iterSelected, 1)+" "+
															(string) model.GetValue(iterSelected, 3)+" "+
															(string) model.GetValue(iterSelected, 4)+" "+
															(string) model.GetValue(iterSelected, 5);
						//entry_edad_paciente_cita.Text = (string) model.GetValue(iterSelected, 4);
					break;
					case "find_paciente_admision1":
						//entry_pid_paciente.Text = tomaid.ToString();
						new osiris.registro_paciente_busca("selecciona",LoginEmpleado,NomEmpleado,AppEmpleado,ApmEmpleado,nombrebd,tomaid);
					break;
					case "find_paciente_admision2":
						//entry_pid_paciente.Text = tomaid.ToString();
						registro.Destroy();
						new osiris.registro_paciente_busca("selecciona",LoginEmpleado,NomEmpleado,AppEmpleado,ApmEmpleado,nombrebd,tomaid);
					break;
					case "find_empresa_catalogo":
						registro.Destroy ();
					new osiris.catalogo_empresas(LoginEmpleado,NomEmpleado,AppEmpleado,ApmEmpleado,nombrebd,int.Parse(tomaid));
					break;
					case "find_proveedores":
						entry_id_proveedor.Text = tomaid.ToString();
						entry_nombre_proveedor.Text = (string) model.GetValue(iterSelected, 1);
						entry_formapago.Text = (string) model.GetValue(iterSelected, 11);
					break;
					case "find_proveedores_id_nombre":
						entry_id_proveedor.Text = tomaid.ToString();
						entry_nombre_proveedor.Text = (string) model.GetValue(iterSelected, 1);
					break;
					case "find_proveedores_catalogo_producto":
						entry_id_proveedor.Text = tomaid.ToString();
						entry_nombre_proveedor.Text = (string) model.GetValue(iterSelected, 1);
						carga_catalogo_prodprove(int.Parse(tomaid),treeviewobject,treeViewEngine);				
					break;
					case "find_proveedores_newoc":						
						entry_id_proveedor.Text = tomaid.ToString();
						entry_nombre_proveedor.Text = (string) model.GetValue(iterSelected, 1);
						entry_formapago.Text = (string) model.GetValue(iterSelected, 11);
						entry_direccion_proveedor.Text = (string) model.GetValue(iterSelected, 3)+" "+(string) model.GetValue(iterSelected, 4)+" "+
														(string) model.GetValue(iterSelected, 5)+ " " +(string) model.GetValue(iterSelected, 6);
						entry_tel_proveedor.Text = (string) model.GetValue(iterSelected, 7);
						entry_rfc_proveedor.Text = (string) model.GetValue(iterSelected, 9);
					break;
					case "find_proveedores_OC":						
						entry_id_proveedor.Text = tomaid.ToString();
						entry_nombre_proveedor.Text = (string) model.GetValue(iterSelected, 1);
						entry_formapago.Text = (string) model.GetValue(iterSelected, 11);
						entry_direccion_proveedor.Text = (string) model.GetValue(iterSelected, 3)+" "+(string) model.GetValue(iterSelected, 4)+" "+
														(string) model.GetValue(iterSelected, 5)+ " " +(string) model.GetValue(iterSelected, 6);
						entry_tel_proveedor.Text = (string) model.GetValue(iterSelected, 7);
						entry_contacto_proveedor.Text =  ""; //(string) model.GetValue(iterSelected, 1);
					break;
					case "find_almacen_inventario":
						entry_id_almacen.Text = tomaid.ToString();
						entry_almacen.Text = (string) model.GetValue(iterSelected, 1);
					break;
					case "find_producto_doc_medicos":
						entry_id_producto.Text = tomaid.ToString();
						entry_descripcion_producto.Text = (string) model.GetValue(iterSelected, 1);
					break;
					case "find_cie10":
						entry_id_cie10.Text = tomaid.ToString();
						entry_descrip_cie10.Text = (string) model.GetValue(iterSelected, 1);
						treeViewEngine.AppendValues(false,tomaid.ToString(),(string) model.GetValue(iterSelected, 3),(string) model.GetValue(iterSelected, 1));
					break;
					case "find_medico_citaqx":
						entry_id_doctor_qx.Text = tomaid.ToString();
						entry_nombre_doctor_qx.Text = (string) model.GetValue(iterSelected, 1);
					break;
					case "find_medico_visitamedica":
						entry_id_medtratante.Text = tomaid.ToString();
						entry_nom_medtratante.Text = (string) model.GetValue(iterSelected, 1);
						entry_espe_medtratante.Text = (string) model.GetValue(iterSelected, 3);
						string acceso_a_productos = (string) classpublic.lee_registro_de_tabla("osiris_his_medicos","id_productos_consulta"," WHERE id_medico = '"+tomaid.ToString()+"' ","id_productos_consulta","string");

						
						
						carga_valores_treeview_prodvismedica(acceso_a_productos,treeviewobject,treeViewEngine);




					break;
					case "find_marcaproducto":
						entry_idmarca.Text = tomaid.ToString();
						entry_marcaproducto.Text = (string) model.GetValue(iterSelected, 1);
					break;
					case "find_catalogo_marca":
						entry_idmarca.Text = model.GetValue(iterSelected, 0).ToString().Trim();
						entry_marcaproducto.Text =model.GetValue (iterSelected, 1).ToString ().Trim ();
						checkbutton_activar.Active = (bool) model.GetValue(iterSelected, 2);
						togglebutton_editar.Sensitive = true;
					break;
					case "find_medico_admision":
						entry_id_medico_admision.Text = model.GetValue(iterSelected, 0).ToString().Trim();
						entry_nombre_medico_admision.Text = model.GetValue(iterSelected, 1).ToString().Trim();
						entry_especialidad_medico_admision.Text = model.GetValue(iterSelected, 3).ToString().Trim();
						entry_cedula_medico_admision.Text = model.GetValue(iterSelected, 4).ToString().Trim();
						entry_tel_medico_admision.Text = model.GetValue(iterSelected, 5).ToString().Trim();
					break;
					case "find_medico_admision_referido":
						entry_id_medico_ref.Text = model.GetValue(iterSelected, 0).ToString().Trim();
						entry_medico_ref.Text = model.GetValue(iterSelected, 1).ToString().Trim();
						entry_espmed_ref.Text = model.GetValue(iterSelected, 3).ToString().Trim();
					break;
					case "find_laboratorio_param":
						entry_idproducto.Text = model.GetValue(iterSelected, 0).ToString().Trim();
						entry_descripcion_estudio.Text = model.GetValue(iterSelected, 1).ToString().Trim();
						registro.Destroy ();
						new osiris.laboratorio_parametros(LoginEmpleado,NomEmpleado,AppEmpleado,ApmEmpleado,nombrebd,tomaid);
					break;
				}				
			}
			Widget win = (Widget) sender;
			win.Toplevel.Destroy();			
		}
					
		[GLib.ConnectBefore ()]   	  // Esto es indispensable para que funcione    
		void onKeyPressEvent_enter(object sender, Gtk.KeyPressEventArgs args)
		{
			if (args.Event.Key.ToString() == "Return" || args.Event.Key.ToString() == "KP_Enter"){
				args.RetVal = true;				
				llenando_lista_de_lista();
			}
		}
		
		// cierra ventanas emergentes
		void on_cierraventanas_clicked (object sender, EventArgs args)
		{
			Widget win = (Widget) sender;
			win.Toplevel.Destroy();
		}
			
		void carga_valores_treeview(int idcode_find,object treeview_,object listotree_store_)
		{
			string acceso_a_grupos = classpublic.lee_registro_de_tabla("osiris_almacenes","id_almacen"," WHERE osiris_almacenes.id_almacen = '5' ","acceso_grupo_producto","int");
			
			string query_sql_llenado_treeview = "SELECT descripcion_producto,osiris_his_tipo_admisiones.descripcion_admisiones, "+
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
							"AND id_grupo_producto IN("+acceso_a_grupos+") "+
							"AND osiris_his_cirugias_deta.eliminado = false "+ 
							"AND osiris_his_cirugias_deta.id_tipo_admisiones = osiris_his_tipo_admisiones.id_tipo_admisiones "+
							"AND osiris_his_cirugias_deta.id_tipo_cirugia = '"+idcode_find.ToString().Trim()+"' "+
						    "AND osiris_his_cirugias_deta.eliminado = 'false' "+
							"ORDER BY osiris_productos.descripcion_producto,to_char(osiris_his_cirugias_deta.fechahora_creacion,'yyyy-mm-dd HH:mm:ss');";
			//Console.WriteLine(query_sql_llenado_treeview);
			NpgsqlConnection conexion; 
			conexion = new NpgsqlConnection (connectionString+nombrebd);
			            
			// Verifica que la base de datos este conectada
			try{
				conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand ();
				comando.CommandText = query_sql_llenado_treeview;
				NpgsqlDataReader lector = comando.ExecuteReader ();
				while (lector.Read()){
					treeViewEngine.AppendValues((string) lector["descripcion_producto"],
														(string) lector["idproducto"],
														(string) lector["cantidadaplicada"],
														(string) DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
														"",
														"",
														(string) lector["costoproductounitario"],
														(string) lector["preciopublico"],
														false,
														false,
														"");
					
				}
			}catch (NpgsqlException ex){
	   			Console.WriteLine ("PostgresSQL error: {0}",ex.Message);
   				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
									MessageType.Info,ButtonsType.Close, "PostgresSQL error: {0} ",ex.Message);
				msgBoxError.Run ();		msgBoxError.Destroy();
			}
			conexion.Close ();				
		}
					
		void carga_valores_treeview_pq_modcargos(int idcode_find,object treeview_,object listotree_store_)
		{
			string precio_a_tomar = "";    // en esta variable dejo el precio que va tomar para los direfentes clientes
			float calculodeiva = 0;
			float ppcantidad = 0;
			float suma_total = 0;
			float preciocondesc = 0;
			float tomaprecio = 0;
			float tomadescue = 0;
			float valor_descuento = 0;
			treeViewEngine.Clear();
			
			string acceso_a_grupos = classpublic.lee_registro_de_tabla("osiris_almacenes","id_almacen"," WHERE osiris_almacenes.id_almacen = '5' ","acceso_grupo_prodcargos","int");
			
			//// para las diferentes listas de precios \\\\\\\\\\\\\			
			if (id_tipopaciente == 500 || id_tipopaciente == 102) {  // Municipio y Empresas			
				// verifica si ese cliente tiene una lista de precio asignada
				if (this.aplica_precios_empresas == true || aplica_precios_aseguradoras == true){     
					precio_a_tomar = "precio_producto_"+id_tipopaciente.ToString().Trim()+idempresa_paciente.ToString().Trim();
					//precio_a_tomar = "precio_producto_publico1";
				}else{
					precio_a_tomar = "precio_producto_publico";
				}
			}else{				
				if (id_tipopaciente == 400 ) { // Aseguradora
					// verifica si ese cliente tiene una lista de precio asignada
					if (this.aplica_precios_empresas == true || aplica_precios_aseguradoras == true){    
						precio_a_tomar = "precio_producto_"+id_tipopaciente.ToString().Trim()+this.idaseguradora_paciente.ToString().Trim();
						//precio_a_tomar = "precio_producto_publico1";
					}else{
						precio_a_tomar = "precio_producto_publico";
					}
				}else{
					precio_a_tomar = "precio_producto_publico";
				}
			}
			
			string query_sql_llenado_treeview = "SELECT descripcion_producto,osiris_his_tipo_admisiones.descripcion_admisiones, "+
							"id_empleado,osiris_his_cirugias_deta.eliminado,osiris_productos.aplicar_iva,osiris_his_cirugias_deta.id_tipo_admisiones AS idtipoadmisiones,"+
							"to_char(osiris_his_cirugias_deta.id_producto,'999999999999') AS idproducto, "+
							"to_char(osiris_his_cirugias_deta.cantidad_aplicada,'99999.99') AS cantidadaplicada, "+
							"to_char(osiris_productos.precio_producto_publico,'99999999.99') AS preciopublico,"+
							"to_char(osiris_productos.costo_por_unidad,'999999999.99') AS costoproductounitario, "+
							"to_char(osiris_productos.porcentage_ganancia,'99999.99') AS porcentageutilidad, "+
							"to_char(osiris_productos.costo_producto,'999999999.99') AS costoproducto, "+
							"to_char(osiris_productos.porcentage_descuento,'999.99') AS porcentagesdesc, " +
							"osiris_productos.aplica_descuento,"+
							"to_char(osiris_his_cirugias_deta.fechahora_creacion,'dd-MM-yyyy HH:mi:ss') AS fechcreacion ,"+
							"to_char(osiris_his_cirugias_deta.id_secuencia,'9999999999') AS secuencia,"+
							"to_char("+precio_a_tomar+",'99999999.99') AS preciopublico_cliente "+
							// "to_char(stock,'999999999.99') AS stock_subalmacen "+
							"FROM "+
							"osiris_his_cirugias_deta,osiris_productos,osiris_his_tipo_cirugias,osiris_his_tipo_admisiones "+
							"WHERE "+
							"osiris_his_cirugias_deta.id_producto = osiris_productos.id_producto "+
							"AND osiris_his_cirugias_deta.id_tipo_cirugia = osiris_his_tipo_cirugias.id_tipo_cirugia "+
							"AND id_grupo_producto IN("+acceso_a_grupos+") "+
							"AND osiris_his_cirugias_deta.eliminado = false "+ 
							"AND osiris_his_cirugias_deta.id_tipo_admisiones = osiris_his_tipo_admisiones.id_tipo_admisiones "+
							"AND osiris_his_cirugias_deta.id_tipo_cirugia = '"+idcode_find.ToString().Trim()+"' "+
							"AND osiris_his_cirugias_deta.id_tipo_admisiones = '"+idtipointernamiento.ToString().Trim()+"' "+
						    "AND osiris_his_cirugias_deta.eliminado = 'false' "+
							"ORDER BY osiris_productos.descripcion_producto,to_char(osiris_his_cirugias_deta.fechahora_creacion,'yyyy-mm-dd HH:mm:ss');";
			NpgsqlConnection conexion; 
			conexion = new NpgsqlConnection (connectionString+nombrebd);
			            
			// Verifica que la base de datos este conectada
			try{
				conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand ();
				comando.CommandText = query_sql_llenado_treeview;
				//Console.WriteLine(comando.CommandText);
				NpgsqlDataReader lector = comando.ExecuteReader ();
				while (lector.Read()){					
					if (float.Parse((string) lector["preciopublico_cliente"].ToString().Trim()) > 0){
						tomaprecio = float.Parse((string) lector["preciopublico_cliente"]);
					}else{
						tomaprecio = float.Parse((string) lector["preciopublico"]);
					}
					tomadescue = float.Parse((string) lector["porcentagesdesc"],System.Globalization.NumberStyles.Float,new System.Globalization.CultureInfo("es-MX"));
					preciocondesc = tomaprecio;
					if ((bool) lector["aplicar_iva"]){
						calculodeiva = ((tomaprecio * valoriva)/100)*float.Parse((string) lector["cantidadaplicada"]);
					}
					if ((bool) lector["aplica_descuento"]){
						preciocondesc = (tomaprecio-((tomaprecio * tomadescue)/100)) * float.Parse((string) lector["cantidadaplicada"]);
					}
					ppcantidad = tomaprecio * float.Parse((string) lector["cantidadaplicada"]);
					suma_total = ppcantidad + calculodeiva;
					valor_descuento = ppcantidad - preciocondesc;
					osiris.cargos_modulos_medicos.Item foo = new osiris.cargos_modulos_medicos.Item(false,
					                float.Parse( (string) lector["cantidadaplicada"]),
					                (string) lector["idproducto"],
					                (string) lector["descripcion_producto"],
					                LoginEmpleado,
									(string) DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
					                (string) lector["descripcion_admisiones"],
					                tomaprecio.ToString("F"),
					                ppcantidad.ToString(),
									calculodeiva.ToString("F").PadLeft(10),
					                suma_total.ToString("F").PadLeft(10),
					                (string) lector["porcentagesdesc"],
					                valor_descuento.ToString("F").PadLeft(10),
					                preciocondesc.ToString("F").PadLeft(10),
					                int.Parse((string) lector["idtipoadmisiones"].ToString()),
					                (string) lector["costoproductounitario"],
					                (string) lector["porcentageutilidad"],
					                (string) lector["costoproducto"]);
					arraylistobject.Add(foo);
					treeViewEngine.AppendValues(false,
					                            float.Parse((string) lector["cantidadaplicada"]),
					            				(string) lector["idproducto"],
					                			(string) lector["descripcion_producto"],
					                            LoginEmpleado,
					                            (string) DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
					                            (string) lector["descripcion_admisiones"],
					                            tomaprecio.ToString("F"),
					                            ppcantidad.ToString(),
												calculodeiva.ToString("F").PadLeft(10),
					                            suma_total.ToString("F").PadLeft(10),
					                            (string) lector["porcentagesdesc"],
					                			valor_descuento.ToString("F").PadLeft(10),
					                            preciocondesc.ToString("F").PadLeft(10),
					                             int.Parse((string) lector["idtipoadmisiones"].ToString()),
					                            (string) lector["costoproductounitario"],
					                            (string) lector["porcentageutilidad"],
					                            (string) lector["costoproducto"]);
				}
			}catch (NpgsqlException ex){
	   			Console.WriteLine ("PostgresSQL error: {0}",ex.Message);
   				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
									MessageType.Info,ButtonsType.Close, "PostgresSQL error: {0} ",ex.Message);
				msgBoxError.Run ();		msgBoxError.Destroy();
			}
			conexion.Close ();				
		}

		void carga_catalogo_prodprove(int idcode_find,object treeview_,object listotree_store_)
		{
			treeViewEngine.Clear();
			NpgsqlConnection conexion; 
			conexion = new NpgsqlConnection (connectionString+nombrebd);
            // Verifica que la base de datos este conectada
			try{
				conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand ();
               	comando.CommandText = "SELECT osiris_catalogo_productos_proveedores.descripcion_producto,"+
					                "to_char(osiris_catalogo_productos_proveedores.precio_costo, '999999999.99') AS preciocosto,"+
					                "to_char(osiris_catalogo_productos_proveedores.precio_costo_unitario, '999999999.99') AS preciocostouni,"+
						            "to_char(osiris_catalogo_productos_proveedores.id_producto, '999999999999999') AS idproducto,"+
               						"osiris_catalogo_productos_proveedores.codigo_producto_proveedor,osiris_catalogo_productos_proveedores.descripcion_producto_osiris,"+
               						"to_char(osiris_catalogo_productos_proveedores.id_secuencia,'9999999999') AS idsecuencia "+
               						//"to_char(osiris_productos.costo_por_unidad,'999999999.99') AS costoproductounitario,"+
               						//"to_char(osiris_productos.costo_producto,'999999999.99') AS costoproducto "+
               						"FROM osiris_catalogo_productos_proveedores "+
               						"WHERE osiris_catalogo_productos_proveedores.id_proveedor = '"+idcode_find.ToString().Trim()+"' " + 
               						"AND osiris_catalogo_productos_proveedores.id_producto = 0 " + 
						            "AND osiris_catalogo_productos_proveedores.eliminado = 'false';";   
				//Console.WriteLine(comando.CommandText);				
				NpgsqlDataReader lector = comando.ExecuteReader ();
				while(lector.Read()){					
					treeViewEngine.AppendValues(false,
							                 (string) lector["descripcion_producto"].ToString().Trim(),
					                         (string) lector["preciocosto"].ToString().Trim(),
					                         (string) lector["preciocostouni"].ToString().Trim(),
					                         (string) lector["codigo_producto_proveedor"].ToString().Trim(),
					                         "",
					                         "",
					                         // (string) lector["costoproducto"],
					                                     //(string) lector["costoproductounitario"],
					                         (string) lector["idproducto"].ToString().Trim(),
					                         (string) lector["descripcion_producto_osiris"].ToString().Trim(),
					                         (string) lector["idsecuencia"].ToString().Trim());
													
				}	
			}catch (NpgsqlException ex){
				Console.WriteLine ("PostgresSQL error: {0}",ex.Message);
				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
				                                               MessageType.Info,ButtonsType.Close, "PostgresSQL error: {0} ",ex.Message);
				msgBoxError.Run ();					msgBoxError.Destroy();
			}
			
			conexion.Close ();				
			NpgsqlConnection conexion1; 
			conexion1 = new NpgsqlConnection (connectionString+nombrebd);
			try{
				conexion1.Open ();
				NpgsqlCommand comando1; 
				comando1 = conexion1.CreateCommand ();
               	comando1.CommandText = "SELECT osiris_catalogo_productos_proveedores.descripcion_producto,"+
					                "to_char(osiris_catalogo_productos_proveedores.precio_costo, '999999999.99') AS preciocosto,"+
					                "to_char(osiris_catalogo_productos_proveedores.precio_costo_unitario, '999999999.99') AS preciocostouni,"+
						            "to_char(osiris_catalogo_productos_proveedores.id_producto, '999999999999999') AS idproducto,"+
               						"osiris_catalogo_productos_proveedores.codigo_producto_proveedor,osiris_catalogo_productos_proveedores.descripcion_producto_osiris,"+
               						"to_char(osiris_productos.costo_por_unidad,'999999999.99') AS costoproductounitario,"+
               						"to_char(osiris_productos.costo_producto,'999999999.99') AS costoproducto,"+
               						"to_char(osiris_catalogo_productos_proveedores.id_secuencia,'9999999999') AS idsecuencia "+
               						"FROM osiris_catalogo_productos_proveedores, osiris_productos "+
               						"WHERE osiris_catalogo_productos_proveedores.id_proveedor = '"+idcode_find.ToString().Trim()+"' " + 
               						"AND osiris_catalogo_productos_proveedores.id_producto = osiris_productos.id_producto " + 
						            "AND osiris_catalogo_productos_proveedores.eliminado = 'false';";   
				//Console.WriteLine(comando1.CommandText);				
				NpgsqlDataReader lector1 = comando1.ExecuteReader ();
				while(lector1.Read()){
					treeViewEngine.AppendValues(false,
					                           (string) lector1["descripcion_producto"].ToString().Trim(),
					                           (string) lector1["preciocosto"].ToString().Trim(),
					                           (string) lector1["preciocostouni"].ToString().Trim(),
					                           (string) lector1["codigo_producto_proveedor"].ToString().Trim(),
					                           (string) lector1["costoproducto"].ToString().Trim(),
					                           (string) lector1["costoproductounitario"].ToString().Trim(),
					                           (string) lector1["idproducto"].ToString().Trim(),
					                           (string) lector1["descripcion_producto_osiris"].ToString().Trim(),
					                           (string) lector1["idsecuencia"].ToString().Trim());
													
				}										
			}catch (NpgsqlException ex){
				Console.WriteLine ("PostgresSQL error: {0}",ex.Message);
				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
				                                               MessageType.Info,ButtonsType.Close, "PostgresSQL error: {0} ",ex.Message);
				msgBoxError.Run ();					msgBoxError.Destroy();
			}
			conexion1.Close ();
		}

		void carga_valores_treeview_prodvismedica(string idcode_find,object treeview_,object listotree_store_)
		{
			float tomaprecio;
			float calculodeiva;
			float preciomasiva;
			float tomadescue;
			float preciocondesc;

			string precio_a_tomar = "";
			id_tipopaciente = int.Parse((string) classpublic.lee_registro_de_tabla ("osiris_erp_cobros_enca", "id_tipo_paciente", "WHERE folio_de_servicio = '"+entry_folio_servicio.Text.Trim()+"' ", "id_tipo_paciente", "string"));
			string aplica_precios_tipopx = (string) classpublic.lee_registro_de_tabla ("osiris_erp_cobros_enca,osiris_his_tipo_pacientes", "lista_de_precio", "WHERE osiris_erp_cobros_enca.id_tipo_paciente = osiris_his_tipo_pacientes.id_tipo_paciente AND folio_de_servicio = '"+entry_folio_servicio.Text.Trim()+"' ", "lista_de_precio", "bool");
			string aplica_precios_sub_tipopx = (string) classpublic.lee_registro_de_tabla ("osiris_erp_cobros_enca,osiris_his_tipo_pacientes", "sub_lista_precios", "WHERE osiris_erp_cobros_enca.id_tipo_paciente = osiris_his_tipo_pacientes.id_tipo_paciente AND folio_de_servicio = '"+entry_folio_servicio.Text.Trim()+"' ", "sub_lista_precios", "bool");
			int idempresa_paciente = int.Parse((string) classpublic.lee_registro_de_tabla ("osiris_erp_cobros_enca", "id_empresa", "WHERE folio_de_servicio = '"+entry_folio_servicio.Text.Trim()+"' ", "id_empresa", "string"));
			int idaseguradora_paciente  = int.Parse((string) classpublic.lee_registro_de_tabla ("osiris_erp_cobros_enca", "id_aseguradora", "WHERE folio_de_servicio = '"+entry_folio_servicio.Text.Trim()+"' ", "id_aseguradora", "string"));

			//// para las diferentes listas de precios \\\\\\\\\\\\\
			if(aplica_precios_tipopx == "True"){
				precio_a_tomar = "precio_producto_"+id_tipopaciente.ToString().Trim();
				if(aplica_precios_sub_tipopx == "True"){
					if(idempresa_paciente != 1){						
						if((string) classpublic.lee_registro_de_tabla("osiris_empresas","lista_de_precio","WHERE lista_de_precio = 'true' AND id_empresa = '"+idempresa_paciente.ToString().Trim()+"' ","lista_de_precio","bool") == "True"){
							precio_a_tomar = precio_a_tomar + idempresa_paciente.ToString().Trim();
						}else{
							precio_a_tomar = "precio_producto_"+id_tipopaciente.ToString().Trim();
						}
					}
					if(idaseguradora_paciente != 1){
						if((string) classpublic.lee_registro_de_tabla("osiris_aseguradoras","lista_de_precio","WHERE lista_de_precio = 'true' AND id_aseguradora = '"+idaseguradora_paciente.ToString().Trim()+"' ","lista_de_precio","bool") == "True"){
							precio_a_tomar = precio_a_tomar + idaseguradora_paciente.ToString().Trim();
						}else{
							precio_a_tomar = "precio_producto_"+id_tipopaciente.ToString().Trim();
						}
					}
				}
			}else{
				precio_a_tomar = "precio_producto_publico";
			}


			//foreach (TreeViewColumn tvc in treeviewobject.Columns)
			//	treeviewobject.RemoveColumn(tvc);
			NpgsqlConnection conexion; 
			conexion = new NpgsqlConnection (connectionString + nombrebd);
			// Verifica que la base de datos este conectada
			try {
				conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand ();
				comando.CommandText = "SELECT to_char(osiris_productos.id_producto,'999999999999') AS codProducto, "+
					"osiris_productos.descripcion_producto,osiris_productos.nombre_articulo,osiris_productos.nombre_generico_articulo, "+
					"to_char(precio_producto_publico,'99999999.99') AS preciopublico,"+
					"to_char(precio_producto_publico1,'99999999.99') AS preciopublico1,"+
					"to_char("+precio_a_tomar+",'99999999.99') AS preciopublico_cliente,"+
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
					"AND osiris_productos.id_producto IN ("+idcode_find+")";
				Console.WriteLine(comando.CommandText);				
				NpgsqlDataReader lector = comando.ExecuteReader ();
				while (lector.Read ()) {
					calculodeiva = 0;
					preciomasiva = 0;					
					///////////////////////////////////////////////////////////
					// ---- nuevo para las multiples listas de precio					
					if (float.Parse((string) lector["preciopublico_cliente"]) > 0){
						tomaprecio = float.Parse((string) lector["preciopublico_cliente"]);
					}else{
						tomaprecio = float.Parse((string) lector["preciopublico"]);
					}									
					tomadescue = float.Parse((string) lector["porcentagesdesc"],System.Globalization.NumberStyles.Float,new System.Globalization.CultureInfo("es-MX"));
					preciocondesc = tomaprecio;
					if ((bool) lector["aplicar_iva"]){
						calculodeiva = (tomaprecio * valoriva)/100;
					}
					if ((bool) lector["aplica_descuento"]){
						preciocondesc = tomaprecio-((tomaprecio*tomadescue)/100);
					}
					preciomasiva = tomaprecio + calculodeiva; 

					treeViewEngine.AppendValues (
						false,
						"1.00",
						lector ["codProducto"].ToString ().Trim (),
						lector ["descripcion_producto"].ToString ().Trim (),
						tomaprecio.ToString("F").PadLeft(10),
						calculodeiva.ToString("F").PadLeft(10),
						preciomasiva.ToString("F").PadLeft(10),
						lector["porcentagesdesc"].ToString().Trim(),
						preciocondesc.ToString("F").PadLeft(10),
						lector["costoproductounitario"].ToString().Trim(),
						lector["porcentageutilidad"].ToString().Trim(),
						lector["costoproducto"].ToString().Trim());
				}										
			} catch (NpgsqlException ex) {
				Console.WriteLine ("PostgresSQL error: {0}", ex.Message);
				MessageDialog msgBoxError = new MessageDialog (MyWinError, DialogFlags.DestroyWithParent,
					                            MessageType.Info, ButtonsType.Close, "PostgresSQL error: {0} ", ex.Message);
				msgBoxError.Run (); 			msgBoxError.Destroy ();
			}
			conexion.Close ();
		}
	}
}