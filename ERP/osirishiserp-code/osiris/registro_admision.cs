//////////////////////////////////////////////////////////
// project created on 24/10/2006 at 10:20 a
// Sistema Hospitalario OSIRIS
// Monterrey - Mexico
//
// Autor    	: Daniel Olivares - arcangeldoc@openmailbox.org (Programacion Mono)
//				  Daniel Olivares - arcangeldoc@openmailbox.org (Diseño de Pantallas Glade)
// 				  
// Licencia		: GLP
// S.O. 		: 
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
//
// Programa de Clase	: registro_admision.cs		
//	
// Proposito			: Registro, admsion de pacintes y busqueda de paciente 
//////////////////////////////////////////////////////////
using System;
using Npgsql;
using Gtk;
using Glade;
using Gdk;
using GLib;
using System.Collections;
using System.Data;
using System.Text;
using System.Drawing;
using System.IO;
using System.Windows;

namespace osiris
{	
	public class registro_paciente_busca
	{
		// Boton general para salir de las ventanas
		// Todas la ventanas en glade este boton debe estra declarado identico
		[Widget] Gtk.Button button_salir = null;
		
		// Para todas las busquedas este es el nombre asignado
		// se declara una vez
		[Widget] Gtk.Entry entry_expresion = null;
		[Widget] Gtk.Button button_selecciona = null;
		
		// Ventana Busqueda de Empresas \\\\\\
		[Widget] Gtk.Window busca_empresas = null;
		[Widget] Gtk.TreeView lista_empresas = null;
		[Widget] Gtk.Button button_busca_empresas = null;
		[Widget] Gtk.Button button_buscar_busqueda = null;
		
		private ListStore treeViewEngineBuscaEmpresa;
		
		////// Ventana de Registro y Admision \\\\\\\\
		[Widget] Gtk.Window registro = null;
		[Widget] Gtk.Image image_foto = null;
		[Widget] Gtk.Entry entry_estatus_pid = null;
		[Widget] Gtk.ComboBox combobox_estado_civil = null;
		[Widget] Gtk.ComboBox combobox_tipo_paciente = null;
		[Widget] Gtk.ComboBox combobox_aseguradora = null;
		[Widget] Gtk.ComboBox combobox_municipios = null;
		[Widget] Gtk.ComboBox combobox_estado = null;
		[Widget] Gtk.ComboBox combobox_tipo_busqueda = null;
		[Widget] Gtk.Button button_buscar_paciente = null;
		[Widget] Gtk.Button button_selec_exp = null;
		[Widget] Gtk.Button button_grabar = null;
		[Widget] Gtk.Button button_responsable = null;
		[Widget] Gtk.Button button_imprimir_protocolo = null;
		[Widget] Gtk.Button button_cancelar_pid = null;
		[Widget] Gtk.Button button_admision = null;
		[Widget] Gtk.Button button_contrata_paquete = null;		
		[Widget] Gtk.Entry entry_nombre_1 = null;
		[Widget] Gtk.Entry entry_nombre_2 = null;
		[Widget] Gtk.Entry entry_apellido_paterno = null;
		[Widget] Gtk.Entry entry_apellido_materno = null;
		[Widget] Gtk.Entry entry_dia_nacimiento = null;
		[Widget] Gtk.Entry entry_mes_nacimiento = null;
		[Widget] Gtk.Entry entry_ano_nacimiento = null;
		[Widget] Gtk.Entry entry_rfc = null;
		[Widget] Gtk.Entry entry_curp = null;
		[Widget] Gtk.Button button_edita_exp = null;
		[Widget] Gtk.Entry entry_ocupacion = null;		
		[Widget] Gtk.Entry entry_empresa = null;
		[Widget] Gtk.Button button_lista_empresas = null;
		[Widget] Gtk.RadioButton radiobutton_masculino = null;
		[Widget] Gtk.RadioButton radiobutton_femenino = null;
		[Widget] Gtk.Entry entry_email = null;
		[Widget] Gtk.Entry entry_calle = null;
		[Widget] Gtk.Entry entry_numero = null;
		[Widget] Gtk.Entry entry_colonia = null;
		[Widget] Gtk.Entry entry_CP = null;
		[Widget] Gtk.Entry entry_telcasa = null;
		[Widget] Gtk.Entry entry_teloficina = null;
		[Widget] Gtk.Entry entry_telcelular = null;
		[Widget] Gtk.ComboBox combobox_religion_paciente = null;
		[Widget] Gtk.Entry entry_alergia_paciente = null;
		[Widget] Gtk.Entry entry_observacion_ingreso = null;
		[Widget] Gtk.Entry entry_servicio_medico = null;
		[Widget] Gtk.Entry entry_lugar_nacimiento = null;
		[Widget] Gtk.TreeView treeview_servicios = null;
		[Widget] Gtk.Entry entry_pid_paciente = null;
		[Widget] Gtk.Entry entry_folio_paciente = null;
		[Widget] Gtk.Entry entry_folio_interno_dep = null;
		[Widget] Gtk.Statusbar statusbar_registro = null;
		[Widget] Gtk.Button button_separa_folio = null;
		[Widget] Gtk.Entry entry_entre_calles = null;
		[Widget] Gtk.Entry entry_nro_nomina = null;
		
		// Ventana de Internar al paciente en urgencias, hospital, etc.
		[Widget] Gtk.Window admision = null;
		[Widget] Gtk.Button button_graba_admision = null;
		[Widget] Gtk.Entry entry_pid_admision = null;
		[Widget] Gtk.Entry entry_paciente_admision = null;
		[Widget] Gtk.TextView textview_diag_admision = null;
		[Widget] Gtk.Entry entry_id_medico = null;
		[Widget] Gtk.Entry entry_nombre_medico = null;
		[Widget] Gtk.Entry entry_especialidad_medico = null;
		[Widget] Gtk.Entry entry_tel_medico = null;
		[Widget] Gtk.Entry entry_cedula_medico = null;
		[Widget] Gtk.Button button_busca_medpcontacto = null;
		[Widget] Gtk.ComboBox combobox_tipo_admision = null;
		[Widget] Gtk.ComboBox combobox_tipo_cirugia = null;
		[Widget] Gtk.CheckButton checkbutton_primera_vez = null;
		
		[Widget] Gtk.CheckButton checkbutton_referido = null;
		[Widget] Gtk.Entry entry_id_medico_ref = null;
		[Widget] Gtk.Entry entry_medico_ref = null;
		[Widget] Gtk.Entry entry_espmed_ref = null;
		
		[Widget] Gtk.CheckButton checkbutton_laboratorio = null;
		[Widget] Gtk.CheckButton checkbutton_imagenologia = null;
		[Widget] Gtk.CheckButton checkbutton_rehabilitacion = null;
		[Widget] Gtk.CheckButton checkbutton_checkup = null;
		[Widget] Gtk.CheckButton checkbutton_optica = null;
		[Widget] Gtk.CheckButton checkbutton_otros_servicios = null;
		[Widget] Gtk.Entry entry_observacion_otros_serv = null;
		[Widget] Gtk.CheckButton checkbutton_podologia = null;
		[Widget] Gtk.CheckButton checkbutton_centromedico = null;
		[Widget] Gtk.Button button_busca_ref = null;
				
		// Ventana Busqueda de cirugias
		[Widget] Gtk.Window busca_cirugias = null;
		[Widget] Gtk.TreeView lista_cirugia = null;
		[Widget] Gtk.Button button_llena_cirugias = null;
		
		// Ventana Busqueda de Medicos
		[Widget] Gtk.Window buscador_medicos = null;
		//[Widget] Gtk.TreeView lista_medicos;
		//[Widget] Gtk.Button button_llena_medicos;
		[Widget] Gtk.TreeView lista_de_medicos = null;
		private TreeStore treeViewEngineMedicos;
		
		// Ventana de datos del responsable de la cuenta
		[Widget] Gtk.Window datos_del_responsable = null;
		[Widget] Gtk.Button button_graba_responsable = null;
		[Widget] Gtk.Button button_paciente_responsable = null;
		[Widget] Gtk.Button button_misma_direccion = null;
		[Widget] Gtk.Button button_asignacion_habitacion = null;
		[Widget] Gtk.Entry entry_nombre_responsable = null;
		[Widget] Gtk.Entry entry_telefono_responsable = null;
		[Widget] Gtk.Entry entry_direcc_responsable = null;
		[Widget] Gtk.Entry entry_empresa_responsable = null;
		[Widget] Gtk.Entry entry_ocupacion_responsable = null;
		[Widget] Gtk.Entry entry_observacion_responsable = null;
		[Widget] Gtk.Entry entry_telef_empre_respo = null;
		[Widget] Gtk.Entry entry_direc_empre_respo = null;
				
		[Widget] Gtk.TreeView treeview_documentos = null;
		[Widget] Gtk.Entry entry_empresa_convenio = null;
		
		[Widget] Gtk.ComboBox combobox_parent_responsable = null;
		
		[Widget] Gtk.ComboBox combobox_paquete_check_up = null;	
		
		TextBuffer buffer = new TextBuffer(null);
		TextIter insertIter;
		
		// Cambio para VENEZUELA
		[Widget] Gtk.Label label37 = null;
		[Widget] Gtk.Label label43 = null;		
		
		// Declaracion de variables publicas
		int PidPaciente = 0;		 // Toma la actualizacion del pid del paciente
		string nomnaciente;		 // Toma el valor del nombre completo del paciente
		string fechanacpaciente;  // Toma la fecha de nacimiento
		string edadpaciente;		 // Toma la edad del paciente
		string fecharegadmision;  // Toma el valor de la fecha de admision
		string horaregadmision;   // Toma el valor de la hora de admision
		string tipopaciente = ""; // toma el valor del texto de tipo de paciente
		int id_tipopaciente = 0;  // toma el valor del tipo de paciente Privado = 200
		int id_tipopaciente_valid = 0;	// toma valor de tipo paciente para validarlo
		int folioservicio = 0;	 // Toma el valor de numero de atencion de paciente
		int id_tipodocumentopx = 1; // Toma el valor del tipo de documento que necesitan llenar para cada paciente
		int id_tipodocumentopx2 = 1; // para cuando ya tiene asignado un municipio o una empresa
		string tipointernamiento = "";//"Urgencias";  // Toma el valor del tipo de internamiento
		int idtipointernamiento = 0;       // Toma el valor del id de internamiento
		bool grabainternamiento = false; // me indica que debe grabar el internamiento del paciente
		bool grabarespocuenta = false;  // me indica si grabo los datos del responsable de la cuenta
		bool almacen_encabezado = true; // banadera de almacenamiento de cobro
		
		string estadocivil = "Casado";   // toma el valor del texto del estahdo civil
		string sexopaciente = "H"; 		// toma el valor del sexo del paciente
		string municipios = " ";	// toma el valor del municipio del paciente (direccion de su casa)
		string estado = " ";	// Toma el valor del estadoo de donde vive el paciente
		int idestado = 1;
		string nombre_aseguradora ="";
		int idaseguradora = 1;
		int idempresa_paciente = 1;
		int numeronomina_px;
		string departamento_px = "";
		string descripcion_empresa_paciente ="";
		bool boolaseguradora = false;
		int idcirugia = 1;   // Toma el valor id de la cirugia
		string decirugia = ""; // toma la descripcion de la
		bool editar = false;
		string busqueda = "";
		string tipobusqueda = "AND osiris_his_medicos.nombre1_medico LIKE '";

		string religionpaciente = "";
		int tipo_filtro = 0;
		
		// Variables publicas medico quien interna
		int idmedico = 1;
 		string nombmedico = "";
 		string especialidadmed = "";
 		string telmedico = "";
 		string cedmedico = "";
 		string motivodeingreso = "";
		string observacionotrosserv = "";
		bool check_laboratorio = false;
		bool check_rayosx_imagen = false;
		bool check_rehabilitacion = false;
		bool check_podologia = false;
		bool check_checkup = false;
		bool check_optica = false;
		bool check_otros_serv = false;
		bool check_centromedico = false;
		string nro_paquete_checkup = "1";
 		
		int idmedico_ref = 1;
	 	string nombmedico_ref = "";
		string especialidadmed_ref = "";
		int accesoserviciosdirecto = 1;
		
		// Variables publicas del responsable de cuenta
		string nombr_respo = "";
		string telef_respo = "";
		string direc_respo = "";
		string empre_respo = "";
		string ocupa_respo = "";
		string certif_respo = "";
		string direc_empre_respo = "";
		string telef_empre_respo = "";
		string parentezcoresponsable = "Sin Parentesco";
		string observacion_respo = "";
		
		// Variables publicas para grabar el encargado de la cuenta
		string _tipo_="";   // que tipo de entrada es nuevo o esta buscando
		string LoginEmpleado;
		string NomEmpleado = "";
		string AppEmpleado = "";
		string ApmEmpleado = "";		
		string connectionString;
		string nombrebd;
		bool esdeprimera_vez = true;
		
		string[] args_args = {""};
		int[] args_id_array = {0,1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20};
		string[] args_estado_civil = {"","CASADO(A)","SOLTERO(A)","SEPARADO(A)","VIUDO(A)","UNION LIBRE","DIVORCIADO(A)"};
		string[] args_tipos_cirugias = {"","CIRUGIA AMBULATORIA","CIRUGIA PROGRAMADA","SIN CIRUGIA"};
		string[] args_parentesco = {"SIN PARENTESCO","ESPOSO(A)","PAPA","MAMA","ABUELO(A)","HERMANO(A)","PRIMO(A)","TIO(A)","CUÑADO(A)","TUTOR","HIJO(A)","CAONCUÑO(A)"};
		string[] args_filtro_busqueda = {"","APELLIDO PATERNO","APELLIDO MATERNO","PRIMER NOMBRE","SEGUNDO NOMBRE"};
		Item documento_foo;
		
		protected Gtk.Window MyWin;
		protected Gtk.Window MyWinError;
		
		private TreeStore treeViewEngine;
		private ListStore store_aseguradora;
		private ListStore treeViewEngineDocumentos;
				
		private ArrayList arraydocumentos;
						
		class_conexion conexion_a_DB = new class_conexion();
		class_public classpublic = new class_public();
		class_buscador classfind_data = new class_buscador();
		
		public registro_paciente_busca(string _tipo, string LoginEmp, string NomEmpleado_, string AppEmpleado_, string ApmEmpleado_, string nombrebd_,string pidpaciente_) 
		{
			LoginEmpleado = LoginEmp;
			NomEmpleado = NomEmpleado_;
			AppEmpleado = AppEmpleado_;
			ApmEmpleado = ApmEmpleado_;
			connectionString = conexion_a_DB._url_servidor+conexion_a_DB._port_DB+conexion_a_DB._usuario_DB+conexion_a_DB._passwrd_user_DB;
			nombrebd = conexion_a_DB._nombrebd;
			_tipo_ = _tipo;

			arraydocumentos = new ArrayList();
			if(_tipo == "selecciona"){
				llena_datos_del_paciente("seleccion_no_admision",pidpaciente_);
			}
			if(_tipo == "busca1"){
				busca_pacientes();
			}
			if (_tipo == "nuevo") {
				nuevo_paciente();
			}
		}
		
		void busca_pacientes()
		{
			object[] parametros_objetos = {entry_pid_paciente};
			string[] parametros_sql = {"SELECT pid_paciente,nombre1_paciente,nombre2_paciente, apellido_paterno_paciente,id_quienlocreo_paciente,"+
				"apellido_materno_paciente, to_char(fecha_nacimiento_paciente,'yyyy-MM-dd') AS fech_nacimiento,sexo_paciente,"+
				"to_char(to_number(to_char(age('"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"',osiris_his_paciente.fecha_nacimiento_paciente),'yyyy') ,'9999'),'9999') AS edad,"+
				"to_char(to_number(to_char(age('"+DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss")+"',osiris_his_paciente.fecha_nacimiento_paciente),'MM'),'99'),'99') AS mesesedad,"+
				"to_char(fechahora_registro_paciente,'dd-MM-yyyy HH:mi:ss') AS fech_creacion,activo FROM osiris_his_paciente WHERE activo = 'true' "};
			string[] parametros_string = {LoginEmpleado,NomEmpleado,AppEmpleado,ApmEmpleado,nombrebd};
			string[,] args_buscador1 = {
									{"APELLIDO PATERNO","AND apellido_paterno_paciente LIKE '%","%' "},
									{"APELLIDO MATERNO","AND apellido_materno_paciente LIKE '%","%' "},
									{"PRIMER NOMBRE","AND nombre1_paciente LIKE '%","%' "},
									{"SEGUNDO NOMBRE","AND nombre2_paciente LIKE '%","%' "},
									{"NRO. EXPEDIENTE","AND osiris_his_paciente.pid_paciente = '","' "},
									{"NRO. NOMINA","AND osiris_his_paciente.nomina_paciente = '","' "}
									};
			string[,] args_buscador2 = {
									{"APELLIDO MATERNO","AND apellido_materno_paciente LIKE '%","%' "},
									{"APELLIDO PATERNO","AND apellido_paterno_paciente LIKE '%","%' "},
									{"PRIMER NOMBRE","AND nombre1_paciente LIKE '%","%' "},
									{"SEGUNDO NOMBRE","AND nombre2_paciente LIKE '%","%' "},
									{"NRO. EXPEDIENTE","AND osiris_his_paciente.pid_paciente = '","' "},
									{"NRO. NOMINA","AND osiris_his_paciente.nomina_paciente = '","' "}};
			string[,] args_orderby = {{"",""}};
			classfind_data.buscandor(parametros_objetos,parametros_sql,parametros_string,"find_paciente_admision1",1,args_buscador1,args_buscador2,args_orderby);
		}
				
		void on_button_cancelar_pid_clicked(object sender, EventArgs args)
		{
			MessageDialog msgBox = new MessageDialog (MyWin,DialogFlags.Modal,MessageType.Question,ButtonsType.YesNo,"¿ Esta seguro que desea Cancelar este PID ?");

			ResponseType miResultado = (ResponseType)msgBox.Run ();
			msgBox.Destroy(); 
			if (miResultado == ResponseType.Yes){			
				NpgsqlConnection conexion = new NpgsqlConnection(connectionString+nombrebd);
				try{
					conexion.Open();
					NpgsqlCommand comando = conexion.CreateCommand ();
					comando.CommandText = "UPDATE osiris_his_paciente SET "+
										"activo = 'false' WHERE pid_paciente = '"+this.PidPaciente.ToString()+"' ;";
					//Console.WriteLine(comando.CommandText);
					comando.ExecuteNonQuery();	        comando.Dispose();
					conexion.Close();
				}catch(NpgsqlException ex){
					Console.WriteLine("PostgresSQL error: {0}",ex.Message);
					MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
									MessageType.Error,ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
					msgBoxError.Run ();		msgBoxError.Destroy();
				}
			}
		}
		
		void on_button_asignacion_habitacion_clicked(object sender, EventArgs args)
		{
		   new osiris.asignacion_de_habitacion(LoginEmpleado,NomEmpleado,AppEmpleado,ApmEmpleado,nombrebd,0);
		}
		
		void nuevo_paciente()
		{
			_tipo_ = "nuevo";
			esdeprimera_vez = true;
			PidPaciente = 0;
			//registro_paciente_busca("nuevo",LoginEmpleado,NomEmpleado,AppEmpleado,ApmEmpleado,nombrebd);
				
			Glade.XML gxml = new Glade.XML (null, "registro_admision.glade", "registro", null);
        	gxml.Autoconnect (this);
        
	        // Muestra ventana de Glade
			registro.Show();

			entry_nombre_1.GrabFocus();
				        	
			llena_Ventana_de_datos(PidPaciente.ToString().Trim());
        	button_grabar.Clicked += new EventHandler(on_graba_informacion_clicked);
			entry_pid_paciente.ModifyBase(StateType.Normal, new Gdk.Color(255,243,169)); // Color Amarillo
			entry_folio_paciente.ModifyBase(StateType.Normal, new Gdk.Color(255,243,169)); // Color Amarillo
			entry_folio_interno_dep.ModifyBase(StateType.Normal, new Gdk.Color(255,243,169)); // Color Amarillo
			entry_estatus_pid.ModifyBase (StateType.Normal, new Gdk.Color (153, 204, 255)); // Color azul claro

			entry_nombre_1.ModifyBase (StateType.Normal, new Gdk.Color (255, 156, 146));
			entry_nombre_2.ModifyBase (StateType.Normal, new Gdk.Color (255, 156, 146));
			entry_apellido_paterno.ModifyBase (StateType.Normal, new Gdk.Color (255, 156, 146));
			entry_apellido_materno.ModifyBase (StateType.Normal, new Gdk.Color (255, 156, 146));
			entry_dia_nacimiento.ModifyBase (StateType.Normal, new Gdk.Color (255, 156, 146));
			entry_mes_nacimiento.ModifyBase (StateType.Normal, new Gdk.Color (255, 156, 146));
			entry_ano_nacimiento.ModifyBase (StateType.Normal, new Gdk.Color (255, 156, 146));
			entry_rfc.ModifyBase (StateType.Normal, new Gdk.Color (255, 156, 146));
			entry_curp.ModifyBase (StateType.Normal, new Gdk.Color (255, 156, 146));
			entry_lugar_nacimiento.ModifyBase (StateType.Normal, new Gdk.Color (255, 156, 146));
			entry_alergia_paciente.ModifyBase (StateType.Normal, new Gdk.Color (255, 156, 146));
			entry_calle.ModifyBase (StateType.Normal, new Gdk.Color (255, 156, 146));
			entry_numero.ModifyBase (StateType.Normal, new Gdk.Color (255, 156, 146));
			entry_colonia.ModifyBase (StateType.Normal, new Gdk.Color (255, 156, 146));
			entry_CP.ModifyBase (StateType.Normal, new Gdk.Color (255, 156, 146));
			entry_telcasa.ModifyBase (StateType.Normal, new Gdk.Color (255, 156, 146));

			entry_nombre_1.KeyPressEvent += onKeyPressEvent_cambiacolor;
			entry_nombre_2.KeyPressEvent += onKeyPressEvent_cambiacolor;
			entry_apellido_paterno.KeyPressEvent += onKeyPressEvent_cambiacolor;
			entry_apellido_materno.KeyPressEvent += onKeyPressEvent_cambiacolor;
			entry_dia_nacimiento.KeyPressEvent += onKeyPressEvent_cambiacolor;
			entry_mes_nacimiento.KeyPressEvent += onKeyPressEvent_cambiacolor;
			entry_ano_nacimiento.KeyPressEvent += onKeyPressEvent_cambiacolor;
			entry_rfc.KeyPressEvent += onKeyPressEvent_cambiacolor;
			entry_curp.KeyPressEvent += onKeyPressEvent_cambiacolor;
			entry_lugar_nacimiento.KeyPressEvent += onKeyPressEvent_cambiacolor;
			entry_alergia_paciente.KeyPressEvent += onKeyPressEvent_cambiacolor;
			entry_calle.KeyPressEvent += onKeyPressEvent_cambiacolor;
			entry_numero.KeyPressEvent += onKeyPressEvent_cambiacolor;
			entry_colonia.KeyPressEvent += onKeyPressEvent_cambiacolor;
			entry_CP.KeyPressEvent += onKeyPressEvent_cambiacolor;
			entry_telcasa.KeyPressEvent += onKeyPressEvent_cambiacolor;
											        	
			// Activa boton de responsable
			button_responsable.Clicked += new EventHandler(on_button_validaadmision_clicked);
			button_responsable.Sensitive = false;
			combobox_tipo_paciente.Sensitive = false;
			entry_observacion_ingreso.Sensitive = false;
			entry_servicio_medico.Sensitive = false;
			//Activa boton para imprimir el protocolo
			button_imprimir_protocolo.Clicked += new EventHandler(on_button_imprimir_protocolo_clicked);
			button_imprimir_protocolo.Sensitive = false;
			button_edita_exp.Clicked += new EventHandler(on_button_edita_exp_clicked);
			button_buscar_paciente.Clicked += new EventHandler(on_button_buscar_paciente_clicked);
        	// desactiva Boton de Internamiento de Paciente tiene que grabar primero
			button_admision.Sensitive = false;
        	//Desactiva campos de PID y de FOLIO para que no se escriba en ellos
        	entry_pid_paciente.IsEditable = false;
        	entry_folio_paciente.IsEditable = false;
        	entry_folio_interno_dep.IsEditable = false;
			//Lista a las empresas con convenio
        	button_lista_empresas.Clicked += new EventHandler(on_button_lista_empresas_clicked);
        	// Contratacion de paquetes
			button_contrata_paquete.Clicked += new EventHandler(on_button_contrata_paquete_clicked);
			button_contrata_paquete.Sensitive = false;
        		        	
        	// lenado de los ComboBox
			llenado_combobox(0,"",combobox_estado,"sql","SELECT * FROM osiris_estados ORDER BY descripcion_estado;","descripcion_estado","id_estado",args_args,args_id_array,"");
			llenado_combobox(0,"",combobox_estado_civil,"array","","","",args_estado_civil,args_id_array,"");
        	llenado_combobox(0,"",combobox_religion_paciente,"sql","SELECT * FROM osiris_tipos_religiones WHERE activo = 'true' ORDER BY id_tipo_religion;","descripcion_religion","id_tipo_religion",args_args,args_id_array,"");
			// Sexo Paciente
			radiobutton_masculino.Clicked += new EventHandler(on_cambioHM_clicked);
			radiobutton_femenino.Clicked += new EventHandler(on_cambioHM_clicked);
					
			// Cambio para VENEZUELA
			//label37.Text = "RIF";			// RFC
			//label43.Text = "C.I.";		// CURP
		}

		// Valida entradas que solo sean numericas, se utiliza en ventana
		// Principal cuando selecciona el folio de productos
		// Ademas controla la tecla ENTRER para ver el procedimiento
		[GLib.ConnectBefore ()]   	  // Esto es indispensable para que funcione    
		public void onKeyPressEvent_cambiacolor(object obj, Gtk.KeyPressEventArgs args)
		{
			args.RetVal = false;
			Gtk.Entry entry_obj = (object) obj as Gtk.Entry;
			//Console.WriteLine (entry_obj.Text+ "    ->"+entry_obj.Text.Length +"    ->"+args.Event.Key.ToString());
			if (args.Event.Key.ToString () == "BackSpace" && entry_obj.Text.Length <= 1) {
				entry_obj.ModifyBase (StateType.Normal, new Gdk.Color (255, 156, 146));
			} else {
				entry_obj.ModifyBase (StateType.Normal, new Gdk.Color (122, 238, 147));
			}
		}
		
		void on_button_edita_exp_clicked(object sender, EventArgs a)
		{
			new osiris.cambia_paciente(LoginEmpleado,NomEmpleado,AppEmpleado,ApmEmpleado,nombrebd,int.Parse(entry_pid_paciente.Text));  // cambia_pacientes.cs
		}

		void on_checkbutton_checkup_clicked(object sender, EventArgs a)
		{
			//Console.WriteLine (idempresa_paciente);
			//Console.WriteLine (idaseguradora);
			if (checkbutton_checkup.Active == true){				
				if(idempresa_paciente != 1 ){
					combobox_paquete_check_up.Sensitive = true;
					button_admision.Sensitive = false;
					llenado_combobox(1,"",combobox_paquete_check_up,"sql","SELECT * FROM osiris_his_tipo_cirugias WHERE paquete_checkup = 'true' AND id_empresa = '"+idempresa_paciente.ToString().Trim()+"';","descripcion_cirugia","id_tipo_cirugia",args_args,args_id_array,"");
				}
				if(idaseguradora != 1){
					combobox_paquete_check_up.Sensitive = true;
					button_admision.Sensitive = false;
					llenado_combobox(1,"",combobox_paquete_check_up,"sql","SELECT * FROM osiris_his_tipo_cirugias WHERE paquete_checkup = 'true' AND id_aseguradora = '"+idaseguradora.ToString().Trim() +"';","descripcion_cirugia","id_tipo_cirugia",args_args,args_id_array,"");				
				}
			}else{
				combobox_paquete_check_up.Sensitive = false;
				button_admision.Sensitive = true;
			}
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
				store.AppendValues ((string) descrip_defaul,0,1);
			}			
			if(sql_or_array == "array"){			
				for (int colum_field = 0; colum_field < args_array.Length; colum_field++){
					store.AppendValues (args_array[colum_field],args_id_array[colum_field],1);
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
					Console.WriteLine(comando.CommandText);
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
				case "combobox_estado":
					estado = (string) combobox_estado.Model.GetValue(iter,0);
					idestado = (int) combobox_estado.Model.GetValue(iter,1);
					llenado_combobox(0,"",combobox_municipios,"sql","SELECT * FROM osiris_municipios WHERE id_estado = '"+idestado.ToString()+"' "+
               						"ORDER BY descripcion_municipio;","descripcion_municipio","id_municipio",args_args,args_id_array,"");
					break;
				case "combobox_municipios":
					municipios = (string) combobox_municipios.Model.GetValue(iter,0);
					//idmunicipio = (int) combobox_municipios.Model.GetValue(iter,1);					
					break;
				case "combobox_estado_civil":
					estadocivil = (string) combobox_estado_civil.Model.GetValue(iter,0);
					break;
				case "combobox_tipo_paciente":
					tipopaciente = (string) combobox_tipo_paciente.Model.GetValue(iter,0);
					id_tipopaciente = (int) combobox_tipo_paciente.Model.GetValue(iter,1);
					id_tipodocumentopx = (int) combobox_tipo_paciente.Model.GetValue(iter,2);
					switch (id_tipopaciente){	
						case 400:
							// Asegurados
							boolaseguradora = true;
							combobox_aseguradora.Sensitive = true;
							llenado_combobox(0,"",combobox_aseguradora,"sql","SELECT * FROM osiris_aseguradoras WHERE activa = 'true' ORDER BY descripcion_aseguradora;","descripcion_aseguradora","id_aseguradora",args_args,args_id_array,"id_tipo_documento");
							idempresa_paciente = 1;
							entry_empresa.Text = "";
							entry_empresa.Sensitive = true;
							break;
						case 100:
							// DIF
							entry_empresa.Sensitive = false;
							boolaseguradora = false;
							combobox_aseguradora.Sensitive = false;
							idaseguradora = 1;
							llenado_combobox(0,"",combobox_aseguradora,"sql","SELECT * FROM osiris_aseguradoras WHERE activa = 'true' ORDER BY descripcion_aseguradora;","descripcion_aseguradora","id_aseguradora",args_args,args_id_array,"id_tipo_documento");
							break;
						case 102:
							// institucion o empresas
							entry_empresa.Sensitive = false;
							boolaseguradora = false;
							combobox_aseguradora.Sensitive = false;
							idaseguradora = 1;
							llenado_combobox(0,"",combobox_aseguradora,"sql","SELECT * FROM osiris_aseguradoras WHERE activa = 'true' ORDER BY descripcion_aseguradora;","descripcion_aseguradora","id_aseguradora",args_args,args_id_array,"id_tipo_documento");
							break;
						case 104:
							// Seguro Popular
							entry_empresa.Sensitive = false;
							boolaseguradora = false;
							combobox_aseguradora.Sensitive = false;
							idaseguradora = 1;
							llenado_combobox(0,"",combobox_aseguradora,"sql","SELECT * FROM osiris_aseguradoras WHERE activa = 'true' ORDER BY descripcion_aseguradora;","descripcion_aseguradora","id_aseguradora",args_args,args_id_array,"id_tipo_documento");
							break;
						case 106:
							// Tarjeta Medica
							entry_empresa.Sensitive = false;
							boolaseguradora = false;
							combobox_aseguradora.Sensitive = false;
							idaseguradora = 1;
							llenado_combobox(0,"",combobox_aseguradora,"sql","SELECT * FROM osiris_aseguradoras WHERE activa = 'true' ORDER BY descripcion_aseguradora;","descripcion_aseguradora","id_aseguradora",args_args,args_id_array,"id_tipo_documento");
						break;
						case 500:
							// municipios						
							entry_empresa.Sensitive = false;
							boolaseguradora = false;
							combobox_aseguradora.Sensitive = false;
							idaseguradora = 1;
							id_tipodocumentopx = id_tipodocumentopx2;
							llenado_combobox(0,"",combobox_aseguradora,"sql","SELECT * FROM osiris_aseguradoras WHERE activa = 'true' ORDER BY descripcion_aseguradora;","descripcion_aseguradora","id_aseguradora",args_args,args_id_array,"id_tipo_documento");
							break;
						case 700:
							// SECCION 50
							entry_empresa.Sensitive = false;
							boolaseguradora = false;
							combobox_aseguradora.Sensitive = false;
							idaseguradora = 1;
							llenado_combobox(0,"",combobox_aseguradora,"sql","SELECT * FROM osiris_aseguradoras WHERE activa = 'true' ORDER BY descripcion_aseguradora;","descripcion_aseguradora","id_aseguradora",args_args,args_id_array,"id_tipo_documento");
							break;
						default:
							idaseguradora = 1;
							entry_empresa.Text = "";
							entry_empresa.Sensitive = true;
							combobox_aseguradora.Sensitive = false;
							llenado_combobox(0,"",combobox_aseguradora,"sql","SELECT * FROM osiris_aseguradoras WHERE activa = 'true' ORDER BY descripcion_aseguradora;","descripcion_aseguradora","id_aseguradora",args_args,args_id_array,"id_tipo_documento");
							break;
					}
					break;
				case "combobox_aseguradora":
					nombre_aseguradora = (string) combobox_aseguradora.Model.GetValue(iter,0);
					idaseguradora = (int) combobox_aseguradora.Model.GetValue(iter,1);
					id_tipodocumentopx = (int) combobox_aseguradora.Model.GetValue(iter,2);
					break;
				case "combobox_tipo_admision":
					tipointernamiento = (string) combobox_tipo_admision.Model.GetValue(iter,0);
					idtipointernamiento = (int) combobox_tipo_admision.Model.GetValue(iter,1);
					accesoserviciosdirecto = (int) combobox_tipo_admision.Model.GetValue(iter,2);
					if((int) combobox_tipo_admision.Model.GetValue(iter,2) == 1){
						checkbutton_laboratorio.Sensitive = true;
						checkbutton_imagenologia.Sensitive = true;
						checkbutton_rehabilitacion.Sensitive = true;
						checkbutton_podologia.Sensitive = true;
						checkbutton_checkup.Sensitive = true;
						checkbutton_optica.Sensitive = true;
						checkbutton_otros_servicios.Sensitive = true;
						checkbutton_centromedico.Sensitive = true;
					}else{
						checkbutton_laboratorio.Sensitive = false;
						checkbutton_imagenologia.Sensitive = false;
						checkbutton_rehabilitacion.Sensitive = false;
						checkbutton_podologia.Sensitive = false;
						checkbutton_checkup.Sensitive = false;
						checkbutton_optica.Sensitive = false;
						checkbutton_otros_servicios.Sensitive = false;
						checkbutton_centromedico.Sensitive = false;
						
						checkbutton_laboratorio.Active = false;
						checkbutton_imagenologia.Active = false;
						checkbutton_rehabilitacion.Active = false;
						checkbutton_podologia.Active = false;
						checkbutton_checkup.Active = false;
						checkbutton_optica.Active = false;
						checkbutton_otros_servicios.Active = false;
						checkbutton_centromedico.Active = false;
						entry_observacion_otros_serv.Text = "";
					}
					break;
				case "combobox_paquete_check_up":
					nro_paquete_checkup = combobox_paquete_check_up.Model.GetValue(iter,1).ToString().Trim();
					break;
				case "combobox_religion_paciente":
					religionpaciente = (string) combobox_religion_paciente.Model.GetValue(iter,0);
					break;
				case "combobox_tipo_cirugia":
					decirugia = (string) combobox_tipo_cirugia.Model.GetValue(iter,0);
					break;
				case "combobox_parent_responsable":
					parentezcoresponsable = (string) combobox_parent_responsable.Model.GetValue(iter,0);
					break;
				}
			}
		}
				
		// Funcion para grabar informacion del paciente, cuando es nuevo Paciente
		void on_graba_informacion_clicked (object sender, EventArgs a)
		{
			// Validando Informacion vacia, minimo Nombre1-App-Apm-Fecha Nacimiento
			//bool validainfocaptura = ;
			if((bool) verifica_datos() == false){	
				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
											MessageType.Error, 
											ButtonsType.Close, "Complete Informacion de Nombres, Apellidos, Fecha y RFC, \n"+
											"Direccion del Paciente o Responsable de Cuenta o \n"+
											"Nombre o Codigo de Empresa, Aseguradora ");
				msgBoxError.Run ();			msgBoxError.Destroy();
			
			}else{
				MessageDialog msgBox = new MessageDialog (MyWin,DialogFlags.Modal,MessageType.Question,ButtonsType.YesNo,"¿ Desea grabar esta infomacion ?");

				ResponseType miResultado = (ResponseType)msgBox.Run ();
				msgBox.Destroy();
 
				if (miResultado == ResponseType.Yes){					
					if (_tipo_ == "nuevo"){						
						//bool verifica_pac = ;          		        		        		
						if ((bool) verifica_paciente() == true){
							// Alamacena los datos del paciente cuando es nuevo
							// Ademas valida los datos								
							if ((bool) grabar_informacion_paciente()){
	        					MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
												MessageType.Info, 
												ButtonsType.Close, "Nombre de Paciente : "+entry_nombre_1.Text.Trim()+" "+entry_apellido_paterno.Text.Trim()+"\n"+	
												entry_apellido_materno.Text.Trim()+"\n Pid del Paciente : "+PidPaciente.ToString());
								msgBoxError.Run ();
								msgBoxError.Destroy();								
								activa_los_entry(false);
	               				button_admision.Sensitive = true;  // Activando Boton de Internamiento de Paciente
								button_responsable.Sensitive = true;
	               				entry_pid_paciente.Text = PidPaciente.ToString();
								combobox_tipo_paciente.Sensitive = true;
								entry_observacion_ingreso.Sensitive = true;
								entry_servicio_medico.Sensitive = true;
								_tipo_ = "busca1";									
							}else{
								MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
									MessageType.Info, 
									ButtonsType.Close, "Verifique la fecha de nacimiento o la informacion");
								msgBoxError.Run ();								msgBoxError.Destroy();
							}	
						}
					}										
					// Almaceno en movcargos y despues en encabezado
					bool almaceno_movcargos = true;					
					if (_tipo_ == "busca1"){						
						if ((bool) check_laboratorio == true ||
							(bool) check_rayosx_imagen == true ||
							(bool) check_rehabilitacion == true ||
							(bool) check_checkup == true ||
							(bool) check_otros_serv == true ||
						    (bool) check_optica == true ||
						    (bool) check_podologia == true ||
						    (bool)check_centromedico == true || 
						    tipointernamiento != ""){
							// asignando folio de servicio
							//this.folioservicio = ultimo_numero_atencion();
							folioservicio = int.Parse(classpublic.lee_ultimonumero_registrado("osiris_erp_movcargos","folio_de_servicio",""));
							button_imprimir_protocolo.Sensitive = true;
							entry_folio_paciente.Text = folioservicio.ToString().Trim();
							
							// Almacenando en mov_cargos
							if ((bool)check_centromedico == true){
								almaceno_movcargos = graba_admision("CENTRO MEDICO ",16,folioservicio,false);
								checkbutton_referido.Active = false;
								checkbutton_referido.Sensitive = false;
								entry_folio_paciente.Text = folioservicio.ToString();}
							if ((bool) check_laboratorio == true){
								almaceno_movcargos = graba_admision("LABORATORIO ",400,folioservicio,false);
								checkbutton_laboratorio.Active = false;
								checkbutton_laboratorio.Sensitive = false;
								entry_folio_paciente.Text = folioservicio.ToString();}
							if ((bool) check_rayosx_imagen == true){
								almaceno_movcargos = graba_admision("IMAGENOLOGIA-RX ",300,folioservicio,false);
								checkbutton_imagenologia.Active = false;
								checkbutton_imagenologia.Sensitive = false;
								entry_folio_paciente.Text = folioservicio.ToString();}
							if ((bool) check_rehabilitacion == true){
								almaceno_movcargos = graba_admision("REHABILITACION ",200,folioservicio,false);
								checkbutton_rehabilitacion.Active = false;
								checkbutton_rehabilitacion.Sensitive = false;
								entry_folio_paciente.Text = folioservicio.ToString();}							
							if ((bool) check_checkup == true){
								almaceno_movcargos = graba_admision("CHECK-UP ",200,folioservicio,true);
								checkbutton_checkup.Active = false;
								checkbutton_checkup.Sensitive = false;
								entry_folio_paciente.Text = folioservicio.ToString();}							
							if ((bool) check_otros_serv == true){
								almaceno_movcargos = graba_admision("OTROS SERVICIOS ",920,folioservicio,false);
								checkbutton_otros_servicios.Active = false;
								checkbutton_otros_servicios.Sensitive = false;
								entry_folio_paciente.Text = folioservicio.ToString();}
							if ((bool) check_optica == true){
								almaceno_movcargos = graba_admision("OPTICA ",970,folioservicio,false);
								checkbutton_optica.Active = false;
								checkbutton_optica.Sensitive = false;
								entry_folio_paciente.Text = folioservicio.ToString();}
							if ((bool) check_podologia == true){
								almaceno_movcargos = graba_admision("PODOLOGIA ",980,folioservicio,false);
								checkbutton_podologia.Active = false;
								checkbutton_podologia.Sensitive = false;
								entry_folio_paciente.Text = folioservicio.ToString();
								
							}
							if(tipointernamiento != ""){
								almaceno_movcargos = graba_admision(tipointernamiento,idtipointernamiento,folioservicio,false);
							}
							
							if (almaceno_movcargos == true){
								bool almaceno_encabezado = almacena_encabezado_de_cobro(folioservicio);
								button_grabar.Sensitive = false;
								llena_servicios_realizados(PidPaciente.ToString().Trim());
								button_asignacion_habitacion.Clicked += new EventHandler(on_button_asignacion_habitacion_clicked);
							
							}else{
								MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
									MessageType.Error,ButtonsType.Close,"OSIRIS no grabo nada... Verifique la informacion debe estar completa");
								msgBoxError.Run ();				msgBoxError.Destroy();
							}
						}else{
							MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
									MessageType.Error,ButtonsType.Close, 
									"No eligio ningun un tipo de ADMISION,"+
									" solo existe el numero de expediente del PACIENTE, OSIRS no ha podido crear el numero de Atencion");
							msgBoxError.Run ();				msgBoxError.Destroy();
						}	
					}
				}
			}
		}
		
	    // Imprime protocolo de admision
		void on_button_imprimir_protocolo_clicked (object sender, EventArgs args)
		{
			new osiris.impr_doc_pacientes(LoginEmpleado,NomEmpleado,AppEmpleado,ApmEmpleado,nombrebd,entry_folio_paciente.Text,1);//,nombmedico);
		}
	    
		// busco un paciente pantalla de ingreso de nuevo paciente
		void on_button_buscar_paciente_clicked(object sender, EventArgs a)
		{
			//busca_pacientes();
			object[] parametros_objetos = {registro};
			string[] parametros_sql = {"SELECT pid_paciente,nombre1_paciente,nombre2_paciente, apellido_paterno_paciente,id_quienlocreo_paciente,"+
				"apellido_materno_paciente, to_char(fecha_nacimiento_paciente,'yyyy-MM-dd') AS fech_nacimiento,sexo_paciente,"+
				"to_char(to_number(to_char(age('"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"',osiris_his_paciente.fecha_nacimiento_paciente),'yyyy') ,'9999'),'9999') AS edad,"+
				"to_char(to_number(to_char(age('"+DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss")+"',osiris_his_paciente.fecha_nacimiento_paciente),'MM'),'99'),'99') AS mesesedad,"+
				"to_char(fechahora_registro_paciente,'dd-MM-yyyy HH:mi:ss') AS fech_creacion,activo FROM osiris_his_paciente WHERE activo = 'true' "};
			string[] parametros_string = {LoginEmpleado,NomEmpleado,AppEmpleado,ApmEmpleado,nombrebd};
			string[,] args_buscador1 = {{"APELLIDO PATERNO","AND apellido_paterno_paciente LIKE '%","%' "},
				{"APELLIDO MATERNO","AND apellido_materno_paciente LIKE '%","%' "},
				{"PRIMER NOMBRE","AND nombre1_paciente LIKE '%","%' "},
				{"NRO. EXPEDIENTE","AND osiris_his_paciente.pid_paciente = '","' "},
				{"NRO. NOMINA","AND osiris_his_paciente.ciente = '","' "}};
			string[,] args_buscador2 = {{"APELLIDO MATERNO","AND apellido_materno_paciente LIKE '%","%' "},
				{"APELLIDO PATERNO","AND apellido_paterno_paciente LIKE '%","%' "},
				{"PRIMER NOMBRE","AND nombre1_paciente LIKE '%","%' "},
				{"NRO. EXPEDIENTE","AND osiris_his_paciente.pid_paciente = '","' "},
				{"NRO. NOMINA","AND osiris_his_paciente.nomina_paciente = '","' "}};
			string[,] args_orderby = {{"",""}};
			classfind_data.buscandor(parametros_objetos,parametros_sql,parametros_string,"find_paciente_admision2",1,args_buscador1,args_buscador2,args_orderby);
		}
	    
		// cambia el estatus del sexo del paciente
		void on_cambioHM_clicked (object sender, EventArgs args)
		{
			Gtk.RadioButton radiobutton_paciente_cita = (Gtk.RadioButton) sender;
			if(radiobutton_paciente_cita.Name.ToString() == "radiobutton_masculino"){
				if (radiobutton_masculino.Active == true){
					sexopaciente = "H";
				}else{
					sexopaciente = "M";}
			}
			if(radiobutton_paciente_cita.Name.ToString() == "radiobutton_femenino"){
				if (radiobutton_femenino.Active == true){
					sexopaciente = "M";
				}else{
					sexopaciente = "H";
				}
			}
		}
			    		
		// Admite a paciente Urgencias y Hospital Internamiento de paciente
		void admision_para_internamiento()
		{
			Glade.XML gxml = new Glade.XML (null, "registro_admision.glade", "admision", null);
			gxml.Autoconnect (this);
			admision.Show();
			
			buffer = textview_diag_admision.Buffer;
			buffer.Clear();
			classpublic.CreateTags(buffer);
			insertIter = buffer.StartIter;
			
			entry_id_medico.Text = idmedico.ToString();
			entry_nombre_medico.Text = nombmedico;
			entry_especialidad_medico.Text = especialidadmed;
			entry_tel_medico.Text = telmedico;
			entry_cedula_medico.Text = cedmedico;
			entry_observacion_otros_serv.Text = observacionotrosserv;
			buffer.Insert (ref insertIter,motivodeingreso);
			entry_id_medico_ref.Text = idmedico_ref.ToString();
			entry_medico_ref.Text = nombmedico_ref;
			entry_espmed_ref.Text = especialidadmed_ref;
			
			entry_id_medico_ref.IsEditable = false;
			entry_medico_ref.IsEditable = false;
			button_busca_ref.Sensitive = false;
			
			checkbutton_laboratorio.Active = check_laboratorio;
			checkbutton_imagenologia.Active = check_rayosx_imagen;
			checkbutton_rehabilitacion.Active = check_rehabilitacion;
			checkbutton_podologia.Active = check_podologia;
			checkbutton_optica.Active = check_optica;
			checkbutton_checkup.Active = check_checkup;
			checkbutton_otros_servicios.Active = check_otros_serv;

			entry_pid_admision.ModifyBase(StateType.Normal, new Gdk.Color(255,243,169)); 		// Color Amarillo
			entry_paciente_admision.ModifyBase(StateType.Normal, new Gdk.Color(255,243,169)); 	// Color Amarillo
			textview_diag_admision.ModifyBase(StateType.Normal, new Gdk.Color(185,238,243)); 		// Color Celeste

			entry_pid_admision.Text = PidPaciente.ToString();
			entry_paciente_admision.Text = entry_nombre_1.Text.ToString()+" "+entry_nombre_2.Text.ToString()+" "+entry_apellido_paterno.Text.ToString()+" "+entry_apellido_materno.Text.ToString();
        				
			button_salir.Clicked += new EventHandler(on_cierraventanas_clicked);
			button_graba_admision.Clicked += new EventHandler(on_graba_admision_clicked);

			button_busca_medpcontacto.Clicked += new EventHandler(on_button_busca_medicos_clicked);
			button_busca_ref.Clicked += new EventHandler(on_button_busca_ref_clicked);
						
			checkbutton_primera_vez.Active = esdeprimera_vez;
			checkbutton_primera_vez.Sensitive = false;
			
			checkbutton_otros_servicios.Clicked += new EventHandler(on_checkbutton_otros_servicios_clicked);
			checkbutton_referido.Clicked += new EventHandler(on_checkbutton_referido_clicked);
			checkbutton_checkup.Clicked += new EventHandler(on_checkbutton_checkup_clicked);
			
			if(grabainternamiento == true){
				llenado_combobox(1,decirugia,combobox_tipo_cirugia,"array","","","",args_tipos_cirugias,args_id_array,"");
				llenado_combobox(1,tipointernamiento,combobox_tipo_admision,"sql","SELECT * FROM osiris_his_tipo_admisiones WHERE servicio_directo = 'false' "+
	           							"AND activo_admision = 'true' "+
	               						"ORDER BY id_tipo_admisiones;","descripcion_admisiones","id_tipo_admisiones",args_args,args_id_array,"acceso_servicios_directo");
				
			}else{
				llenado_combobox(1,"",combobox_tipo_admision,"sql","SELECT * FROM osiris_his_tipo_admisiones WHERE servicio_directo = 'false' "+
	           							"AND activo_admision = 'true' "+
	               						"ORDER BY id_tipo_admisiones;","descripcion_admisiones","id_tipo_admisiones",args_args,args_id_array,"acceso_servicios_directo");
				llenado_combobox(0,"",combobox_tipo_cirugia,"array","","","",args_tipos_cirugias,args_id_array,"");
			}
		}
		
		void on_checkbutton_otros_servicios_clicked(object sender, EventArgs args)
		{
			entry_observacion_otros_serv.Sensitive = (bool) checkbutton_otros_servicios.Active;			                                          
		}
		
		void on_checkbutton_referido_clicked(object sender, EventArgs args)
		{
			button_busca_ref.Sensitive = (bool) checkbutton_referido.Active;			                                          
		}
			
		void on_graba_admision_clicked(object sender, EventArgs args)
		{
			if(accesoserviciosdirecto == 1){
				if(textview_diag_admision.Buffer.Text.ToString().Trim() == ""){
					MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
					MessageType.Error, 
					ButtonsType.Close, "Debe capturar el Motivo de Ingreso");
					msgBoxError.Run ();
					msgBoxError.Destroy();
				}else{
					grabainternamiento = true;
					motivodeingreso = textview_diag_admision.Buffer.Text.ToString().Trim().ToUpper();
					nombmedico = (string) entry_nombre_medico.Text.ToUpper();
					idmedico = int.Parse(entry_id_medico.Text);
	 				especialidadmed = (string) entry_especialidad_medico.Text;
					cedmedico = (string) entry_cedula_medico.Text;
					telmedico = entry_tel_medico.Text;
					observacionotrosserv = entry_observacion_otros_serv.Text.ToUpper().Trim();
					check_laboratorio = checkbutton_laboratorio.Active;
					check_rayosx_imagen = checkbutton_imagenologia.Active;
					check_rehabilitacion = checkbutton_rehabilitacion.Active;
					check_podologia = checkbutton_podologia.Active;
					check_optica = checkbutton_optica.Active;
					check_otros_serv = checkbutton_otros_servicios.Active;
					check_checkup = checkbutton_checkup.Active;
					check_centromedico = checkbutton_centromedico.Active;
									
					// cierra la ventana despues que almaceno la informacion en variables
					Widget win = (Widget) sender;
					win.Toplevel.Destroy();
				}
			}else{
				if((string) entry_nombre_medico.Text == "" || textview_diag_admision.Buffer.Text.ToString().Trim() == "" || idtipointernamiento == 0 || decirugia == ""){
					MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
					MessageType.Error, 
					ButtonsType.Close, "Debe poner una Admision, ni de dejar el Nombre del Medico o el Motivo de Ingreso vacios, ni la cirugia vacia");
					msgBoxError.Run ();
					msgBoxError.Destroy();
				}else{
					grabainternamiento = true;
					motivodeingreso = textview_diag_admision.Buffer.Text.ToString().Trim().ToUpper();					

					nombmedico = (string) entry_nombre_medico.Text.ToUpper();
					idmedico = int.Parse(entry_id_medico.Text);
	 				especialidadmed = (string) entry_especialidad_medico.Text;
					cedmedico = (string) entry_cedula_medico.Text;
					telmedico = entry_tel_medico.Text;

					observacionotrosserv = entry_observacion_otros_serv.Text.ToUpper().Trim();
					check_laboratorio = checkbutton_laboratorio.Active;
					check_rayosx_imagen = checkbutton_imagenologia.Active;
					check_rehabilitacion = checkbutton_rehabilitacion.Active;
					check_podologia = checkbutton_podologia.Active;
					check_optica = checkbutton_optica.Active;
					check_otros_serv = checkbutton_otros_servicios.Active;
					check_checkup = checkbutton_checkup.Active;
					check_centromedico = checkbutton_centromedico.Active;
									
					// cierra la ventana despues que almaceno la informacion en variables
					Widget win = (Widget) sender;
					win.Toplevel.Destroy();
				}				
			}
		}
		
		// Ventana de Busqueda de Medico
		void on_button_busca_medicos_clicked (object sender, EventArgs args)
		{
			object[] parametros_objetos = {entry_id_medico,entry_nombre_medico,entry_especialidad_medico,entry_cedula_medico,entry_tel_medico};
			string[] parametros_sql = {"SELECT * FROM osiris_his_medicos JOIN osiris_his_tipo_especialidad USING (id_especialidad) WHERE medico_activo = 'true' "};
			string[] parametros_string = {};
			string[,] args_buscador1 = {{"NOMBRES O APELLIDOS","AND nombre_medico LIKE '%","%' "},
										{"ID MEDICO","AND id_medico = '","' "}};
			string[,] args_buscador2 = {{"ID MEDICO","AND id_medico = '","' "},
										{"NOMBRE O APELLIDOS","AND nombre_medico LIKE '%","%' "}};
			string[,] args_orderby = {{"",""}};
			classfind_data.buscandor(parametros_objetos,parametros_sql,parametros_string,"find_medico_admision",0,args_buscador1,args_buscador2,args_orderby);
		}

		// Ventana de Busqueda de Medico
		void on_button_busca_ref_clicked (object sender, EventArgs args)
		{
			object[] parametros_objetos = {entry_id_medico_ref,entry_medico_ref,entry_espmed_ref};
			string[] parametros_sql = {"SELECT * FROM osiris_his_medicos JOIN osiris_his_tipo_especialidad USING (id_especialidad) WHERE medico_activo = 'true' "};
			string[] parametros_string = {};
			string[,] args_buscador1 = {{"NOMBRES O APELLIDOS","AND nombre_medico LIKE '%","%' "},
				{"ID MEDICO","AND id_medico = '","' "}};
			string[,] args_buscador2 = {{"ID MEDICO","AND id_medico = '","' "},
				{"NOMBRE O APELLIDOS","AND nombre_medico LIKE '%","%' "}};
			string[,] args_orderby = {{"",""}};
			classfind_data.buscandor(parametros_objetos,parametros_sql,parametros_string,"find_medico_admision_referido",0,args_buscador1,args_buscador2,args_orderby);
		}
		
		void on_button_contrata_paquete_clicked (object sender, EventArgs args)
		{
			Glade.XML gxml = new Glade.XML (null, "registro_admision.glade", "admision", null);
			gxml.Autoconnect (this);
	        admision.Show();
			
			entry_pid_admision.Text = PidPaciente.ToString();
			entry_paciente_admision.Text = entry_nombre_1.Text.ToString().Trim()+" "+entry_nombre_2.Text.ToString().Trim()+" "+entry_apellido_paterno.Text.ToString().Trim()+" "+entry_apellido_materno.Text.ToString().Trim();
        	entry_pid_admision.Sensitive = false;
			entry_paciente_admision.Sensitive = false;
			
			button_salir.Clicked += new EventHandler(on_cierraventanas_clicked);  			
		}
	    // Crea la lista de empresas que tienen convenio con el hospital
	    void on_button_lista_empresas_clicked(object sender, EventArgs args)
	    {
	    	busqueda = "empresas";
			Glade.XML gxml = new Glade.XML (null, "registro_admision.glade", "busca_empresas", null);
			gxml.Autoconnect (this);
			// Muestra ventana de Glade
			busca_empresas.Show();
			// Activa la salida de la ventana
			button_salir.Clicked += new EventHandler(on_cierraventanas_clicked);
			// Activa la seleccion de Medico
			button_selecciona.Clicked += new EventHandler(on_selecciona_empresa_clicked);
			// Llena treeview 
			button_busca_empresas.Clicked += new EventHandler(on_button_llena_empresas_clicked);
			entry_expresion.KeyPressEvent += onKeyPressEvent_enter;
			treeViewEngineBuscaEmpresa = new ListStore( typeof(int), typeof(string), typeof (int));
			lista_empresas.Model = treeViewEngineBuscaEmpresa;
			lista_empresas.RulesHint = true;
			lista_empresas.RowActivated += on_selecciona_empresa_clicked;  // Doble click selecciono empresa*/
			TreeViewColumn col_idempresa = new TreeViewColumn();
			CellRendererText cellr0 = new CellRendererText();
			col_idempresa.Title = "ID Empresa"; // titulo de la cabecera de la columna, si está visible
			col_idempresa.PackStart(cellr0, true);
			col_idempresa.AddAttribute (cellr0, "text", 0);    // la siguiente columna será 1
            
			TreeViewColumn col_nombrempresa = new TreeViewColumn();
			CellRendererText cellrt1 = new CellRendererText();
			col_nombrempresa.Title = "Nombre Empresa";
			col_nombrempresa.PackStart(cellrt1, true);
			col_nombrempresa.AddAttribute (cellrt1, "text", 1); // la siguiente columna será 2
			
			lista_empresas.AppendColumn(col_idempresa);
			lista_empresas.AppendColumn(col_nombrempresa);
		}
		
		void on_button_llena_empresas_clicked(object sender, EventArgs args)
		{
			llenado_lista_empresas();			
		}
		
		void llenado_lista_empresas()
		{
			treeViewEngineBuscaEmpresa.Clear(); // Limpia el treeview cuando realiza una nueva busqueda			
			NpgsqlConnection conexion; 
			conexion = new NpgsqlConnection (connectionString+nombrebd );            
			// Verifica que la base de datos este conectada
			try{
				conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand ();
				
				if ((string) entry_expresion.Text.ToUpper() == "*"){
					comando.CommandText = "SELECT * FROM osiris_empresas WHERE id_tipo_paciente = '"+id_tipopaciente+"' "+
								"ORDER BY descripcion_empresa DESC;";
				}else{
					comando.CommandText = "SELECT * FROM osiris_empresas WHERE id_tipo_paciente = '"+id_tipopaciente+"' "+
								"AND descripcion_empresa LIKE '%"+(string) entry_expresion.Text.ToUpper()+"%' "+
								"ORDER BY descripcion_empresa DESC;";
				}
				NpgsqlDataReader lector = comando.ExecuteReader ();
				while (lector.Read()){
					treeViewEngineBuscaEmpresa.AppendValues ((int) lector["id_empresa"],//TreeIter iter = 
											(string)lector["descripcion_empresa"],
					                        (int) lector["id_tipo_documento"]);
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
		
		void on_selecciona_empresa_clicked (object sender, EventArgs args)
		{
			TreeModel model;
			TreeIter iterSelected;
			if (lista_empresas.Selection.GetSelected(out model, out iterSelected)) {
				idempresa_paciente = (int) model.GetValue(iterSelected, 0);
				entry_empresa.Text = (string) model.GetValue(iterSelected, 1); 
				id_tipodocumentopx = (int) model.GetValue(iterSelected, 2);
				Widget win = (Widget) sender;
				win.Toplevel.Destroy();
			}
		}
	    
		// Procedimiento para el llenado de los datos del paciente
		public void llena_inf_de_paciente(string pidpaciente_)
		{
			NpgsqlConnection conexion; 
			conexion = new NpgsqlConnection (connectionString+nombrebd );            
			// Verifica que la base de datos este conectada
			try{
				conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand ();               	
				comando.CommandText = "SELECT pid_paciente, nombre1_paciente,nombre2_paciente, apellido_paterno_paciente,"+
							"apellido_materno_paciente, to_char(fecha_nacimiento_paciente,'dd') AS dia_nacimiento,"+
							"to_char(fecha_nacimiento_paciente,'MM') AS mes_nacimiento,paciente_bloqueado_cc,"+
							"to_char(fecha_nacimiento_paciente,'yyyy') AS ano_nacimiento,sexo_paciente, ocupacion_paciente,"+
							"rfc_paciente, curp_paciente, estado_civil_paciente, direccion_paciente, numero_casa_paciente,"+
							"colonia_paciente, codigo_postal_paciente,telefono_particular1_paciente,"+
							"telefono_trabajo1_paciente,celular1_paciente,municipio_paciente,estado_paciente,entre_calles_paciente,"+
							"osiris_his_paciente.id_empresa AS idempresapaciente,osiris_empresas.descripcion_empresa,"+
							"religion_paciente,alegias_paciente,lugar_nacimiento_paciente,id_tipo_documento,nomina_paciente,departamento_paciente "+
							"FROM osiris_his_paciente, osiris_empresas "+
							"WHERE osiris_his_paciente.id_empresa=osiris_empresas.id_empresa "+
							"AND pid_paciente = '"+pidpaciente_.ToString()+"'"+
							"AND activo = 'true' "+
							" ORDER BY pid_paciente;";
				Console.WriteLine(comando.CommandText);
				NpgsqlDataReader lector = comando.ExecuteReader ();
				if ((bool) lector.Read()){
					entry_nombre_1.Text = (string) lector["nombre1_paciente"];
					entry_nombre_2.Text = (string) lector["nombre2_paciente"];
					entry_apellido_paterno.Text = (string) lector["apellido_paterno_paciente"];
					entry_apellido_materno.Text = (string) lector["apellido_materno_paciente"];
					entry_dia_nacimiento.Text = (string) lector["dia_nacimiento"];
					entry_mes_nacimiento.Text = (string) lector["mes_nacimiento"];
					entry_ano_nacimiento.Text = (string) lector["ano_nacimiento"];
					entry_rfc.Text = (string) lector["rfc_paciente"];
					entry_curp.Text = (string) lector["curp_paciente"];
					entry_ocupacion.Text = (string) lector["ocupacion_paciente"];
					descripcion_empresa_paciente = (string) lector["descripcion_empresa"];
					idempresa_paciente = (int) lector["idempresapaciente"];
					id_tipodocumentopx2 = (int) lector["id_tipo_documento"];
					entry_empresa.Text = (string) descripcion_empresa_paciente.ToString().Trim();
					sexopaciente = (string) lector["sexo_paciente"];
					if (sexopaciente == "H"){
						radiobutton_masculino.Active = true;						
					}else{
						radiobutton_femenino.Active = true;
					}
					religionpaciente = lector["religion_paciente"].ToString().Trim();
					entry_calle.Text = lector["direccion_paciente"].ToString().Trim();
					entry_numero.Text = lector["numero_casa_paciente"].ToString().Trim();
					entry_colonia.Text = lector["colonia_paciente"].ToString().Trim();
					entry_CP.Text = lector["codigo_postal_paciente"].ToString().Trim();
					entry_telcasa.Text = lector["telefono_particular1_paciente"].ToString().Trim();
					entry_teloficina.Text = lector["telefono_trabajo1_paciente"].ToString().Trim();
					entry_telcelular.Text = lector["celular1_paciente"].ToString().Trim();
					entry_nro_nomina.Text = lector["nomina_paciente"].ToString().Trim();

					numeronomina_px = int.Parse(lector["nomina_paciente"].ToString().Trim());
					departamento_px = lector["departamento_paciente"].ToString().Trim();

					//Console.WriteLine(numeronomina_px);
					
					//entry_religion_paciente.Text = (string) lector["religion_paciente"];
					//llenado_combobox(1,(string) lector["religion_paciente"],"sql","SELECT * FROM osiris_tipos_religiones WHERE activo = 'true' ORDER BY id_tipo_religion;","descripcion_religion","id_tipo_religion",args_args,args_id_array);
					llenado_combobox(1,lector["religion_paciente"].ToString(),combobox_religion_paciente,"sql","SELECT * FROM osiris_tipos_religiones WHERE activo = 'true' ORDER BY id_tipo_religion;","descripcion_religion","id_tipo_religion",args_args,args_id_array,"");
					entry_alergia_paciente.Text = (string) lector["alegias_paciente"];
					entry_lugar_nacimiento.Text = (string) lector["lugar_nacimiento_paciente"];
					estadocivil = (string) lector["estado_civil_paciente"];
					entry_entre_calles.Text = (string) lector["entre_calles_paciente"];
				
					llenado_combobox(1,estadocivil,combobox_estado_civil,"array","","","",args_estado_civil,args_id_array,"");
					// Llenado de Municipios
					municipios = (string) lector["municipio_paciente"];
					llenado_combobox(1,municipios,combobox_municipios,"sql","SELECT * FROM osiris_municipios WHERE id_estado = '"+idestado.ToString()+"' "+
               						"ORDER BY descripcion_municipio;","descripcion_municipio","id_municipio",args_args,args_id_array,"");
					// Llenado de Estados
					estado = (string) lector["estado_paciente"];
					llenado_combobox(1,estado,combobox_estado,"sql","SELECT * FROM osiris_estados ORDER BY descripcion_estado;","descripcion_estado","id_estado",args_args,args_id_array,"");
					if((bool) lector["paciente_bloqueado_cc"]){
						MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
									MessageType.Error,ButtonsType.Close,"Este Expediente se encuentra BLOQUEADO, favor de Comunicarse a Credito y Cobranza");
								msgBoxError.Run ();				msgBoxError.Destroy();
						button_grabar.Sensitive = false;						
					}
					
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
	    			    
		// cierra ventanas emergentes
		void on_cierraventanas_clicked (object sender, EventArgs a)
		{
			Widget win = (Widget) sender;
			win.Toplevel.Destroy();
		}
	    
		// Valida entradas que solo sean numericas
		[GLib.ConnectBefore ()]   	  // Esto es indispensable para que funcione    
		public void onKeyPressEvent(object o, Gtk.KeyPressEventArgs args)
		{
			if (args.Event.Key == Gdk.Key.Return){
				//Console.WriteLine("Presione Enter");
			}
			string misDigitos = ".0123456789ﾰﾱﾲﾳﾴﾵﾶﾷﾸﾹﾮｔｒｓｑ（）";
			if (Array.IndexOf(misDigitos.ToCharArray(), Convert.ToChar(args.Event.Key)) == -1 && args.Event.Key.ToString()  != "BackSpace"){
				args.RetVal = true;
			}
		}
		
    	// Funcion para verificar que los datos sean llenados correctamente
		public bool verifica_datos()
		{
			if(_tipo_ == "nuevo"){
				if ((string) entry_nombre_1.Text.Trim() == "" || (string) entry_apellido_paterno.Text.Trim() == "" ||	
					(string) entry_apellido_materno.Text.Trim() == "" || (string) entry_dia_nacimiento.Text.Trim() == "" ||
					(string) entry_mes_nacimiento.Text.Trim() == "" || (string) entry_ano_nacimiento.Text.Trim() == "" ||
					(string) entry_rfc.Text.Trim() == "" ||
					estadocivil == "" || entry_lugar_nacimiento.Text.Trim() == "" ||
					religionpaciente == "" || entry_alergia_paciente.Text.Trim() == "" ||
					entry_ocupacion.Text.ToString().Trim() == ""){
					return false;
				}else{
					return true;
				}
			}else{
				if(classpublic.valid_dato_responsable == false){
					grabarespocuenta = true;
				}
				if ((string) entry_nombre_1.Text.Trim() == "" || (string) entry_apellido_paterno.Text.Trim() == "" ||	
					(string) entry_apellido_materno.Text.Trim() == "" || (string) entry_dia_nacimiento.Text.Trim() == "" ||
					(string) entry_mes_nacimiento.Text.Trim() == "" || (string) entry_ano_nacimiento.Text.Trim() == "" ||
					(string) entry_rfc.Text.Trim() == "" ||
					estadocivil == "" || entry_lugar_nacimiento.Text.Trim() == "" ||
					entry_observacion_ingreso.Text.ToString().Trim() == "" || 
				    entry_servicio_medico.Text.ToString().Trim() == "" ||
					id_tipopaciente == 0 || (string) entry_empresa.Text.ToString().Trim() == "" ||
					religionpaciente == "" || entry_alergia_paciente.Text.Trim() == "" ||
					id_tipopaciente == 0 || grabarespocuenta == false ||
					entry_ocupacion.Text.ToString().Trim() == ""){
					return false;				
				}else{
					// Aseguradora
					if (id_tipopaciente == 400){	
						if(idaseguradora == 1){
							return false;
						}else{
							return true;
						}
					}else{			
						return true;
					}
					// municipios
					if (id_tipopaciente == 500)	{
						if(idempresa_paciente == 1){
							return false;
						}else{
							return true;
						}
					}
					// secretaria de salud seguro popular
					if (id_tipopaciente == 104)	{
						if(idempresa_paciente == 1){
							return false;
						}else{
							return true;
						}
					}
				}
			}
		}
    	
		// Funcion para verificar que el paciente exista
		public bool verifica_paciente()
		{
	   		NpgsqlConnection conexion; 
			conexion = new NpgsqlConnection (connectionString+nombrebd );            
			// Verifica que la base de datos este conectada
			try{
			// Transformando al Mayuscula datos
				entry_nombre_1.Text = (string) entry_nombre_1.Text.ToUpper();
				entry_nombre_2.Text = (string) entry_nombre_2.Text.ToUpper();
				entry_apellido_paterno.Text = (string) entry_apellido_paterno.Text.ToUpper();
				entry_apellido_materno.Text = (string) entry_apellido_materno.Text.ToUpper();
				entry_rfc.Text = (string) entry_rfc.Text.ToUpper();
				entry_curp.Text = (string) entry_curp.Text.ToUpper();
        		string rfcpaciente =  "";
        		conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand ();             
				comando.CommandText = "SELECT rfc_paciente,activo "+
							"FROM osiris_his_paciente "+ 
							"WHERE activo = true and rfc_paciente = '"+entry_rfc.Text.ToUpper()+"';";
            	NpgsqlDataReader lector = comando.ExecuteReader ();
               	// Verificando la consulta si esta vacia
				if ((bool) lector.Read()){
					if ((bool) lector["activo"] == true){
						// Asignacion de Variables para verificar RFC
						rfcpaciente   =  (string) lector["rfc_paciente"];
						// Verificando si el paciente ya esta registrado
						if ((string) entry_rfc.Text.ToUpper() == rfcpaciente ){
							MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
											MessageType.Error, 
												ButtonsType.Close, "Este paciente ya existe en nuestros registros");
							msgBoxError.Run ();
							msgBoxError.Destroy();
						}
						lector.Close ();
						conexion.Close ();
						return false;
					}else{
						lector.Close ();
						conexion.Close ();
						return true;	
					}
				}else{
					conexion.Close ();
					return true;	
				}				
	   		}catch (NpgsqlException ex){
				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
										MessageType.Error, 
										ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
				msgBoxError.Run ();
				msgBoxError.Destroy();
				conexion.Close ();
				return false; 
			}			
		}
		
		//---------------------------------------------------------
		public string crea_rfc()
		{
			return "RFC";
		}
		
	 	// Graba informacion datos el paciente asignandole un numero de expediente
		// ademas verifica que que si marco alguna admision grabe los datos correspondiente
		// para que los pueda ver caja u urgencias, hospitalizacion, terapias
		public bool grabar_informacion_paciente()
		{
			//bool agregar_registro = false;			
			NpgsqlConnection conexion;
			conexion = new NpgsqlConnection (connectionString+nombrebd );
			// Verifica que la base de datos este conectada
			try{
				conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand ();
				PidPaciente = int.Parse((string) classpublic.lee_ultimonumero_registrado("osiris_his_paciente","pid_paciente",""));				
				//comando.CommandText = "SELECT pid_paciente FROM osiris_his_paciente ORDER BY pid_paciente DESC LIMIT 1;";
                //NpgsqlDataReader lector = comando.ExecuteReader ();
				//if ((bool) lector.Read()){
				//	PidPaciente = (int) lector["pid_paciente"] + 1;
				//	lector.Close ();
				//}else{
				//	PidPaciente = 1;
				//}
				// Agregando el nuevo registro de paciente
				comando.CommandText = "INSERT INTO osiris_his_paciente (fechahora_registro_paciente,"+
             						"nombre1_paciente,nombre2_paciente,"+
                					"apellido_paterno_paciente,apellido_materno_paciente,"+
                					"fecha_nacimiento_paciente,rfc_paciente,curp_paciente,"+
                					"direccion_paciente,numero_casa_paciente,colonia_paciente,"+
                					"codigo_postal_paciente,telefono_particular1_paciente,telefono_trabajo1_paciente,"+
                					"celular1_paciente,email_paciente,estado_civil_paciente,"+
                					"sexo_paciente,municipio_paciente,estado_paciente,ocupacion_paciente,"+
                					"id_quienlocreo_paciente,pid_paciente,id_empresa,activo,"+
									"religion_paciente," +
									"alegias_paciente," +
									"lugar_nacimiento_paciente," +
									"entre_calles_paciente" +
									") VALUES ('"+
                					DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"','"+
                					entry_nombre_1.Text.ToUpper().Trim()+"','"+
                					entry_nombre_2.Text.ToUpper().Trim()+"','"+
                					entry_apellido_paterno.Text.ToUpper().Trim()+"','"+
                					entry_apellido_materno.Text.ToUpper().Trim()+"','"+
                					entry_ano_nacimiento.Text+"-"+entry_mes_nacimiento.Text+"-"+entry_dia_nacimiento.Text+"','"+
                					entry_rfc.Text.ToUpper().Trim()+"','"+
                					entry_curp.Text.ToUpper().Trim()+"','"+
                					entry_calle.Text.ToUpper().Trim()+"','"+
                					entry_numero.Text+"','"+
                					entry_colonia.Text.ToUpper().Trim()+"','"+
                					entry_CP.Text+"','"+
                					entry_telcasa.Text+"','"+
                					entry_teloficina.Text+"','"+
                					entry_telcelular.Text+"','"+
                					entry_email.Text+"','"+
                					estadocivil+"','"+
                					sexopaciente+"','"+
                					municipios+"','"+
                					estado+"','"+
                					entry_ocupacion.Text.ToUpper().Trim()+"','"+
                					LoginEmpleado+"','"+
                					PidPaciente+"','"+
                					idempresa_paciente+"','"+
                					"true"+"','"+
									religionpaciente+"','"+
									entry_alergia_paciente.Text.ToUpper()+"','"+
									entry_lugar_nacimiento.Text.ToUpper()+"','"+
									entry_entre_calles.Text.ToUpper()+"');";
                	//Console.WriteLine("Grabo informacion del Paciente "+PidPaciente.ToString());
					comando.ExecuteNonQuery();					comando.Dispose();
					return true;
			}catch (NpgsqlException ex){
				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
							MessageType.Error, ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
				msgBoxError.Run ();		msgBoxError.Destroy();
				return false;
			}
		}
		
		// Actualizando la tabla de movimiento de servicios
		// para que caja pueda lee la informacion
		// Se dan de alta valores en movimientos por departamentos 
		bool graba_admision( string tiposervicio , int idtipoadmision, int folioservicio_,bool automatic_cargos)
		{
			bool validation_save = true;
			//if(tipointernamiento != ""){
				NpgsqlConnection conexion; 
				conexion = new NpgsqlConnection (connectionString+nombrebd );			           
				// Verifica que la base de datos este conectada						
				try{
					int foliointernodep = int.Parse(classpublic.lee_ultimonumero_registrado("osiris_erp_movcargos","folio_de_servicio_dep","WHERE id_tipo_admisiones = '"+idtipoadmision.ToString()+"'"));               			
					entry_folio_interno_dep.Text = entry_folio_interno_dep.Text+tiposervicio+foliointernodep.ToString()+" | ";
					conexion.Open ();
					NpgsqlCommand comando; 
					comando = conexion.CreateCommand ();
					comando.CommandText = "INSERT INTO osiris_erp_movcargos (id_tipo_admisiones," +
										"id_empleado,"+
									"fechahora_admision_registro," +
									"folio_de_servicio," +
									"folio_de_servicio_dep," +
									"pid_paciente," +
									"id_tipo_paciente,"+
									"id_tipo_cirugia," +
									"nombre_de_cirugia," +
									"tipo_cirugia," +
									"vista_primera_vez," +
									"descripcion_diagnostico_movcargos," +
									"otros_servicios" +
									") VALUES ('"+
									idtipoadmision+"', '"+
									LoginEmpleado+"', '"+
									DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"','"+
									folioservicio_.ToString().Trim()+"','"+
									foliointernodep+"','"+
									PidPaciente+"','"+
									id_tipopaciente+"','"+
									idcirugia+"','"+
									"','"+
									decirugia.ToUpper().Trim()+"','"+
									esdeprimera_vez.ToString()+"','"+
									(string) classpublic.RemoveAccentsWithRegEx(motivodeingreso.ToUpper().Trim())+"','"+
									observacionotrosserv.ToString()+
									"');";
						//Console.WriteLine(comando.CommandText);	
						comando.ExecuteNonQuery();				comando.Dispose();
					validation_save = true;
				}catch (NpgsqlException ex){
					MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
											MessageType.Error, ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
					msgBoxError.Run ();			msgBoxError.Destroy();
					validation_save = false;
				}
				conexion.Close ();
			//}else{
			//	validation_save = false;
			//}
			return validation_save;
		}
		
		bool almacena_encabezado_de_cobro(int folioservicio_)
		{
			//folioservicio = int.Parse(classpublic.lee_ultimonumero_registrado("osiris_erp_movcargos","folio_de_servicio",""));
			bool grabacion_sino = false;
			NpgsqlConnection conexion; 
			conexion = new NpgsqlConnection (connectionString+nombrebd );
			try{
				conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand ();
				// Este proceso se utiliza para cerrar las numeros de atencion que se usan en el centro medico 
				bool cerrar_folio;
				bool facturacion_folio;
				cerrar_folio = false;
				facturacion_folio = false;
				if((bool) checkbutton_referido.Active == false){
					idmedico_ref = 1;	
				}
				// Creando el registro de de encabezado para que lo pueda buscar
				// y realizar los cargos correspondiens
				comando.CommandText = "INSERT INTO osiris_erp_cobros_enca (" +
							"folio_de_servicio," +
							"pid_paciente," +
							"fechahora_creacion,"+
							"id_empleado_admision,"+
							"responsable_cuenta,"+
							"telefono1_responsable_cuenta,"+
							"direccion_responsable_cuenta,"+
							"empresa_labora_responsable,"+
							"ocupacion_responsable,"+
							"id_aseguradora,"+
							"paciente_asegurado,"+
							//"numero_poliza,"+
							"numero_certificado,"+
							"direccion_emp_responsable,"+
							"telefono_emp_responsable,"+
							"parentezco," +
							"id_medico," +
							"id_empresa," +
							"nombre_medico_encabezado,"+
							"cerrado," +
							"facturacion,"+
							"observacion_ingreso," +
							"otro_servicio_medico,"+
							"nombre_empresa_encabezado," +
							"id_referido," +
							"id_paquete_quirurgico," +
							"id_tipo_paciente" +
							") VALUES ('"+
							folioservicio_.ToString().Trim()+"','"+
							PidPaciente+"','"+
							DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"','"+
							LoginEmpleado+"','"+
							nombr_respo.ToUpper().Trim()+"','"+
							telef_respo+"','"+
							direc_respo.ToUpper().Trim()+"','"+
							empre_respo.ToUpper().Trim()+"','"+
							ocupa_respo.ToUpper().Trim()+"','"+
							idaseguradora+"','"+
							boolaseguradora+"','"+
							//poliz_respo.Trim()+"','"+
							certif_respo.Trim()+"','"+
							direc_empre_respo.ToUpper().Trim()+"','"+
							telef_empre_respo.ToUpper().Trim()+"','"+
							parentezcoresponsable+"','"+
							idmedico+"','"+
							idempresa_paciente+"','"+
							nombmedico.ToUpper().Trim()+"','"+
							cerrar_folio+"','"+
							facturacion_folio+"','"+
							entry_observacion_ingreso.Text.ToUpper()+"','"+
							entry_servicio_medico.Text.ToUpper()+"','"+
							entry_empresa.Text.ToString().Trim().ToUpper()+"','"+
							idmedico_ref.ToString().Trim()+"','"+
							nro_paquete_checkup+"','"+
							id_tipopaciente+"');";
				//Console.WriteLine("Graba Encabezado");
				comando.ExecuteNonQuery();			comando.Dispose();
				for (int i = 0; i < arraydocumentos.Count; i++) {
      				Item documento_foo = (Item) arraydocumentos[i];
					//treeViewEngineDocumentos.AppendValues(documento_foo.col00_,documento_foo.col01_);
					comando.CommandText = "INSERT INTO osiris_erp_movimiento_documentos (" +
									"pid_paciente," +
									"folio_de_servicio," +
									"descripcion_documento," +
									"informacion_capturada," +
									"id_tipo_documento) VALUES ('"+
									PidPaciente.ToString().Trim()+"','"+
									folioservicio_.ToString().Trim()+"','"+
									documento_foo.col00_+"','"+
									documento_foo.col01_+"','"+
									id_tipodocumentopx.ToString().Trim()+"');";
					comando.ExecuteNonQuery();			comando.Dispose();
    			}
				grabacion_sino = true;
			}catch (NpgsqlException ex){
	   			MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
										MessageType.Error, ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
				msgBoxError.Run ();				msgBoxError.Destroy();
				grabacion_sino = false;
			}
			conexion.Close ();
			return grabacion_sino;
		}
						
		void llena_datos_del_paciente(string opcion_,string pidpaciente_)
		{
			if(opcion_ == "seleccion_no_admision"){
				Glade.XML gxml = new Glade.XML (null, "registro_admision.glade", "registro", null);
				gxml.Autoconnect (this);		        		        
				// Muestra ventana de Glade
				registro.Show();				
				// Cambio para VENEZUELA
				//label37.Text = "RIF";			// RFC
				//label43.Text = "C.I.";		// CURP
				entry_pid_paciente.ModifyBase(StateType.Normal, new Gdk.Color(255,243,169)); // Color Amarillo
				entry_pid_paciente.ModifyFont(Pango.FontDescription.FromString ("Arial 10"));
				entry_folio_paciente.ModifyBase(StateType.Normal, new Gdk.Color(255,243,169)); // Color Amarillo
				entry_folio_interno_dep.ModifyBase(StateType.Normal, new Gdk.Color(255,243,169)); // Color Amarillo
				entry_estatus_pid.ModifyBase (StateType.Normal, new Gdk.Color (153, 204, 255)); // Color azul claro
				llena_Ventana_de_datos(pidpaciente_.ToString().Trim());
				llena_inf_de_paciente(pidpaciente_.ToString().Trim());
				activa_los_entry(false);				
				button_grabar.Clicked += new EventHandler(on_graba_informacion_clicked);
				button_responsable.Clicked += new EventHandler(on_button_validaadmision_clicked);
				button_buscar_paciente.Clicked += new EventHandler(on_button_buscar_paciente_clicked);
				button_imprimir_protocolo.Clicked += new EventHandler(on_button_imprimir_protocolo_clicked);
				button_asignacion_habitacion.Clicked += new EventHandler(on_button_asignacion_habitacion_clicked);
				button_contrata_paquete.Clicked += new EventHandler(on_button_contrata_paquete_clicked);
				button_lista_empresas.Clicked += new EventHandler(on_button_lista_empresas_clicked);
				button_separa_folio.Clicked += new EventHandler(on_button_separa_folio_clicked);
				button_edita_exp.Clicked += new EventHandler(on_button_edita_exp_clicked);
				button_admision.Sensitive = true;
				button_imprimir_protocolo.Sensitive = false;				
				button_contrata_paquete.Sensitive = false;				
				entry_pid_paciente.IsEditable = false;
				entry_folio_paciente.IsEditable = false;
				entry_folio_interno_dep.IsEditable = false;
				PidPaciente = int.Parse(pidpaciente_);
				entry_observacion_ingreso.Sensitive = true;
				entry_servicio_medico.Sensitive = true;
				// Cambio para VENEZUELA
				//label37.Text = "RIF";			// RFC
				//label43.Text = "C.I.";		// CURP	
				_tipo_ = "busca1";

				if (System.IO.File.Exists ("/home/dolivares/Desktop/danielito.jpg")) {					
					Gdk.Pixbuf fotografia = new Gdk.Pixbuf ("/home/dolivares/Desktop/danielito.jpg");
					if (fotografia != null) {
						image_foto.Pixbuf = fotografia;
					}
				}
			}
		}

		public static byte[] Image2Bytes( System.Drawing.Image pImagen)
		{
			byte[] mImage = null;
			try
			{
				if (pImagen != null){
					using (System.IO.MemoryStream ms = new System.IO.MemoryStream()){
						pImagen.Save(ms, pImagen.RawFormat);
						mImage = ms.GetBuffer();
						ms.Close();
					}
				}else{
					mImage = null;
				}
			}catch (Exception ex){
				throw (ex);
			}
			return mImage;
		}

		public static System.Drawing.Image Bytes2Image(byte[] bytes)
		{
			if (bytes == null) return null;
			using (MemoryStream ms = new MemoryStream(bytes)){
				Bitmap bm = null;
				try{
					bm = new Bitmap(ms);
				}catch (Exception ex){
					throw (ex);
				}
				return bm;
			}
		}

		private static void guardarFotosPostgres(string vServidor, string vBaseDatos,string vUsuario, string vPassword, string vPathFoto)
		{
			//String para cadena de conexion
			StringBuilder sCadena = new StringBuilder("");
			//objeto conexion
			NpgsqlConnection Con;

			//construccion de la cadena para conectarse a postgres
			sCadena.Append("Host=;");
			sCadena.Append("Database=;");
			sCadena.Append("User ID=;");
			sCadena.Append("Password=;");
			sCadena.Append("Port=5432;");
			sCadena.Replace("", vServidor);
			sCadena.Replace("", vBaseDatos);
			sCadena.Replace("", vUsuario);
			sCadena.Replace("", vPassword);

			Con = new NpgsqlConnection(Convert.ToString(sCadena));

			using (NpgsqlCommand Comando = new NpgsqlCommand())
			{
				try{
					//abrir la conexion
					Con.Open();
					//convierto la imagen en binario (el tipo de dato en postgres debe ser blob)
					byte[] Blob = Image2Bytes(System.Drawing.Image.FromFile(String.Format("{0}\\nuestraimagen.jpg", vPathFoto)));
					//asigno valores a los atributos del command que invocara un Stored Procedure
					Comando.CommandType = CommandType.StoredProcedure;
					Comando.Connection = Con;
					Comando.CommandText = "public.SPGuardaImagen";
					Comando.Parameters.Add("pfichero", Blob);
					//ejecucion de la funcion sin retorno de valores
					Comando.ExecuteNonQuery();
				}catch (Exception ex){
					throw (ex);
				}finally{
					Con.Close();
					Con.Dispose();
				}
			}
		}
				
		void on_button_separa_folio_clicked(object sender, EventArgs a)
		{
			int folioservicio = 0;
			TreeModel model;
			TreeIter iterSelected;
			if (this.treeview_servicios.Selection.GetSelected(out model, out iterSelected)){
				if((string) classpublic.lee_registro_de_tabla("osiris_empleado","acceso_reservar_folio","WHERE acceso_reservar_folio = 'true' AND login_empleado = '"+LoginEmpleado+"' ","acceso_reservar_folio","bool") == "True"){
					folioservicio = int.Parse((string) model.GetValue(iterSelected, 4));
					new osiris.reservacion_de_paquetes(LoginEmpleado,NomEmpleado,AppEmpleado,ApmEmpleado,nombrebd,folioservicio,true);
				}else{
					MessageDialog msgBox = new MessageDialog (MyWinError,DialogFlags.Modal,
									MessageType.Info,ButtonsType.Ok,"No tiene Permiso para esta Opcion");
					msgBox.Run ();msgBox.Destroy();
				}
			}	
		}

		// Cuando el paciente no es nuevo viene a este 
		void llena_Ventana_de_datos(string pidpaciente_)
		{
			// Cierra Ventana
			button_salir.Clicked += new EventHandler(on_cierraventanas_clicked);
			button_cancelar_pid.Sensitive = false;
			
			button_admision.Clicked += new EventHandler(on_button_validaadmision_clicked);		
			// desactiva botton de intermaniento de paciente
			button_admision.Sensitive = true;
			//Entrada de Fecha de Nacimiento valida solo numeros
			entry_dia_nacimiento.KeyPressEvent += onKeyPressEvent;
			entry_mes_nacimiento.KeyPressEvent += onKeyPressEvent;
			entry_ano_nacimiento.KeyPressEvent += onKeyPressEvent;
			llenado_combobox(1,"",combobox_tipo_paciente,"sql","SELECT * FROM osiris_his_tipo_pacientes WHERE activo_tipo_paciente = 'true' ORDER BY descripcion_tipo_paciente;",
			                 "descripcion_tipo_paciente","id_tipo_paciente",args_args,args_id_array,"id_tipo_documento");
			entry_empresa.Sensitive = false;			        		        
			// Creacion de Liststore
			treeViewEngine = new TreeStore(typeof (string),typeof (string),
							typeof (string), typeof (string), 
							typeof (string),typeof (string),typeof (string),
							typeof (string), typeof (string),typeof (string),
							typeof (string),typeof (bool),typeof (string));
	        							   
			treeview_servicios.Model = treeViewEngine;
			//treeViewEngine.SetSortColumnId (0, Gtk.SortType.Ascending);
			                            
			TreeViewColumn col_fecha = new TreeViewColumn();
			CellRendererText cellrt1 = new CellRendererText();  // aplica a todas la columnas
			col_fecha.Title = "Fecha-Hora"; // titulo de la cabecera de la columna, si está visible
			col_fecha.PackStart(cellrt1, true);
			col_fecha.AddAttribute (cellrt1, "text", 0);
			col_fecha.SortColumnId = (int) Column_serv.col_fecha;
             
			TreeViewColumn col_servicio = new TreeViewColumn();
			col_servicio.Title = "Servicio";
			col_servicio.PackStart(cellrt1, true);
			col_servicio.AddAttribute (cellrt1, "text", 1);
			col_servicio.SortColumnId = (int) Column_serv.col_servicio ;
      
			TreeViewColumn col_desc_servicio = new TreeViewColumn();
			col_desc_servicio.Title = "Motivo de Ingreso";
			col_desc_servicio.PackStart(cellrt1, true);
			col_desc_servicio.AddAttribute (cellrt1, "text", 2);
			col_desc_servicio.SortColumnId = (int) Column_serv.col_desc_servicio;
            
			TreeViewColumn col_valor = new TreeViewColumn();
			col_valor.Title = "Tipo de Cirugia";
			col_valor.PackStart(cellrt1, true);
			col_valor.AddAttribute (cellrt1, "text", 3);
			col_valor.SortColumnId = (int) Column_serv.col_valor;
            
			TreeViewColumn col_folio_ingreso = new TreeViewColumn();
			col_folio_ingreso.Title = "Folio Ingreso";
			col_folio_ingreso.PackStart(cellrt1, true);
			col_folio_ingreso.AddAttribute (cellrt1, "text", 4);
			col_folio_ingreso.SortColumnId = (int) Column_serv.col_folio_ingreso;
			
			TreeViewColumn col_num_factura = new TreeViewColumn();
			col_num_factura.Title = "N. Factura";
			col_num_factura.PackStart(cellrt1, true);
			col_num_factura.AddAttribute (cellrt1, "text", 5);
			col_num_factura.SortColumnId = (int) Column_serv.col_num_factura;			
            
			TreeViewColumn col_folio_ingreso_dep = new TreeViewColumn();
			col_folio_ingreso_dep.Title = "Folio Departamento";
			col_folio_ingreso_dep.PackStart(cellrt1, true);
			col_folio_ingreso_dep.AddAttribute (cellrt1, "text", 6);
			col_folio_ingreso_dep.SortColumnId = (int) Column_serv.col_folio_ingreso_dep;
			
			TreeViewColumn col_tipo_paciente = new TreeViewColumn();
			col_tipo_paciente.Title = "Tipo de Paciente";
			col_tipo_paciente.PackStart(cellrt1, true);
			col_tipo_paciente.AddAttribute (cellrt1, "text", 7);
			col_tipo_paciente.SortColumnId = (int) Column_serv.col_tipo_paciente;
			
			TreeViewColumn col_empresaasegu = new TreeViewColumn();
			col_empresaasegu.Title = "Empresa/Aseguradora";
			col_empresaasegu.PackStart(cellrt1, true);
			col_empresaasegu.AddAttribute (cellrt1, "text", 8);
			col_empresaasegu.SortColumnId = (int) Column_serv.col_empresaasegu;
						 
			TreeViewColumn col_admitio = new TreeViewColumn();
			col_admitio.Title = "Admitio";
			col_admitio.PackStart(cellrt1, true);
			col_admitio.AddAttribute (cellrt1, "text", 9);
			col_admitio.SortColumnId = (int) Column_serv.col_admitio;
			
			TreeViewColumn col_observaciones = new TreeViewColumn();
			col_observaciones.Title = "Observaciones";
			col_observaciones.PackStart(cellrt1, true);
			col_observaciones.AddAttribute (cellrt1, "text", 10);
			col_observaciones.SortColumnId = (int) Column_serv.col_observaciones;
			
			TreeViewColumn col_separacion = new TreeViewColumn();
			CellRendererToggle cellrtogg = new  CellRendererToggle();
			col_separacion.Title = "Separacion PQ.";
			col_separacion.PackStart(cellrtogg, true);
			col_separacion.AddAttribute (cellrtogg, "active", 11);
			col_separacion.SortColumnId = (int) Column_serv.col_separacion;
			
			TreeViewColumn col_servmedico = new TreeViewColumn();
			col_servmedico.Title = "Serv. Med.";
			col_servmedico.PackStart(cellrt1, true);
			col_servmedico.AddAttribute (cellrt1, "text", 12);
			col_servmedico.SortColumnId = (int) Column_serv.col_servmedico;
                        
			treeview_servicios.AppendColumn(col_fecha);
			treeview_servicios.AppendColumn(col_servicio);
			treeview_servicios.AppendColumn(col_desc_servicio);
			treeview_servicios.AppendColumn(col_observaciones);
			treeview_servicios.AppendColumn(col_servmedico);
			treeview_servicios.AppendColumn(col_valor);
			treeview_servicios.AppendColumn(col_folio_ingreso);
			treeview_servicios.AppendColumn(col_num_factura);
			treeview_servicios.AppendColumn(col_folio_ingreso_dep);
			treeview_servicios.AppendColumn(col_tipo_paciente);
			treeview_servicios.AppendColumn(col_empresaasegu);
			treeview_servicios.AppendColumn(col_admitio);
			treeview_servicios.AppendColumn(col_separacion);
			
			//Llena treview de servicio realizados
			// _tipo_ es una variable publica esta al inicio del programa
			if (_tipo_=="busca1"){
  				llena_servicios_realizados(pidpaciente_);
				//Console.WriteLine("llenando informacion");
			}
			if(_tipo_ == "selecciona"){
				llena_servicios_realizados(pidpaciente_);
			}
			entry_pid_paciente.Text = pidpaciente_;
			// Actulizando statusbar
			statusbar_registro.Pop(0);
			statusbar_registro.Push(1, "login: "+LoginEmpleado+"| Usuario: "+NomEmpleado+" "+AppEmpleado+" "+ApmEmpleado);
			statusbar_registro.HasResizeGrip = false;		
		}
		
		enum Column_serv
		{
			col_fecha,
			col_servicio,
			col_desc_servicio,
			col_observaciones,
			col_servmedico,
			col_valor,
			col_folio_ingreso,
			col_num_factura,
			col_folio_ingreso_dep,
			col_tipo_paciente,
			col_empresaasegu,
			col_admitio,
			col_separacion
		}
		
		void llena_servicios_realizados(string pidpaciente_)
		{
			treeViewEngine.Clear();
			NpgsqlConnection conexion; 
			conexion = new NpgsqlConnection (connectionString+nombrebd);
			//this.PidPaciente
			try{
				conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand ();
				comando.CommandText = "SELECT to_char(osiris_erp_cobros_enca.fechahora_creacion,'yyyy-MM-dd HH24:MI:SS') AS fechahoraadm, "+
									"osiris_his_tipo_admisiones.descripcion_admisiones,osiris_erp_cobros_enca.cancelado, "+
									"osiris_erp_movcargos.descripcion_diagnostico_movcargos, osiris_erp_movcargos.id_tipo_cirugia, "+
									//"osiris_his_tipo_cirugias.descripcion_cirugia, "+
									"to_char(osiris_erp_cobros_enca.folio_de_servicio,'9999999') AS folioserv, "+
									"to_char(osiris_erp_movcargos.folio_de_servicio_dep,'9999999') AS folioservdep, "+
									"osiris_his_tipo_pacientes.descripcion_tipo_paciente,osiris_erp_cobros_enca.id_empleado_admision,"+
									"osiris_erp_cobros_enca.id_aseguradora,descripcion_aseguradora,"+
									"osiris_erp_cobros_enca.id_empresa,descripcion_empresa,"+
									"osiris_erp_cobros_enca.reservacion,observacion_ingreso,otro_servicio_medico,"+
									"osiris_erp_cobros_enca.fecha_reservacion,"+
									"osiris_his_tipo_cirugias.descripcion_cirugia,"+
									"osiris_erp_cobros_enca.numero_factura AS numerofactura "+
									"FROM "+
									"osiris_erp_cobros_enca,osiris_erp_movcargos,osiris_his_tipo_pacientes,osiris_his_tipo_cirugias,osiris_his_tipo_admisiones,osiris_aseguradoras,osiris_empresas "+
									"WHERE "+
									" osiris_erp_cobros_enca.folio_de_servicio = osiris_erp_movcargos.folio_de_servicio "+
									"AND osiris_erp_movcargos.id_tipo_cirugia = osiris_his_tipo_cirugias.id_tipo_cirugia "+
									"AND osiris_erp_movcargos.id_tipo_paciente = osiris_his_tipo_pacientes.id_tipo_paciente "+
									"AND osiris_erp_movcargos.id_tipo_admisiones = osiris_his_tipo_admisiones.id_tipo_admisiones "+
									"AND osiris_erp_cobros_enca.pid_paciente = '"+pidpaciente_.ToString() +"' "+
									"AND osiris_erp_cobros_enca.id_aseguradora = osiris_aseguradoras.id_aseguradora "+
									"AND osiris_erp_cobros_enca.id_empresa = osiris_empresas.id_empresa "+  
									"ORDER BY osiris_erp_cobros_enca.folio_de_servicio DESC;";
				//Console.WriteLine("Query: "+comando.CommandText);
				NpgsqlDataReader lector = comando.ExecuteReader ();
				string diagnostico_cirugia = "";
				string aseguradora_empresa = "";
				string foliosseparados = "";
				bool folioreservado = false;
				int contador_servicios = 0;
				while (lector.Read()){					
					diagnostico_cirugia = (string) lector["descripcion_diagnostico_movcargos"];									
					if((int) lector ["id_aseguradora"] > 1){
						aseguradora_empresa = (string) lector["descripcion_aseguradora"];
					}else{
						aseguradora_empresa = (string) lector["descripcion_empresa"];
					}					
					if (!(bool) lector["cancelado"]){ 
						treeViewEngine.AppendValues ((string) lector["fechahoraadm"],
															(string) lector["descripcion_admisiones"],
															diagnostico_cirugia,
															(string) lector["descripcion_cirugia"],
															(string) lector["folioserv"],
															(string) lector["numerofactura"],
															(string) lector["folioservdep"],
															(string) lector["descripcion_tipo_paciente"],
															aseguradora_empresa,
															(string) lector["id_empleado_admision"],
															(string) lector["observacion_ingreso"],
						                             		(bool) lector["reservacion"],
						                             		(string) lector["otro_servicio_medico"]);
					}
					if (folioreservado == false && (bool) lector["reservacion"] == true){
						folioreservado = true;
						foliosseparados += (string) lector["folioserv"].ToString().Trim()+ " - ";
					}
					contador_servicios++;
				}
				if(contador_servicios != 0){
					esdeprimera_vez = false;
				}else{
					esdeprimera_vez = true;
				}
				if (folioreservado == true){
					MessageDialog msgBox = new MessageDialog (MyWin,DialogFlags.Modal,
								MessageType.Info,ButtonsType.Ok,"El paciente tiene separado un folio para un paquete quirurgico N Folio: "+foliosseparados);
					msgBox.Run ();			msgBox.Destroy();
				
				}
			}catch (NpgsqlException ex){
	   				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
										MessageType.Error, ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
					msgBoxError.Run ();			msgBoxError.Destroy();
			}
			conexion.Close ();
		}
		
		void activa_los_entry(bool valor)
		{
			entry_nombre_1.Sensitive = valor;
			entry_nombre_2.Sensitive = valor;
			entry_apellido_paterno.Sensitive = valor;
			entry_apellido_materno.Sensitive = valor;
			entry_dia_nacimiento.Sensitive = valor;
			entry_mes_nacimiento.Sensitive = valor;
			entry_ano_nacimiento.Sensitive = valor;
			entry_rfc.Sensitive = valor;
			entry_curp.Sensitive = valor;
			entry_ocupacion.Sensitive = valor;		
			entry_empresa.Sensitive = valor;
			entry_email.Sensitive = valor;
			entry_calle.Sensitive = valor;
			entry_numero.Sensitive = valor;
			entry_colonia.Sensitive = valor;
			entry_CP.Sensitive = valor;
			entry_telcasa.Sensitive = valor;
			entry_teloficina.Sensitive = valor;
			entry_telcelular.Sensitive = valor;
			combobox_estado_civil.Sensitive = valor;
			combobox_religion_paciente.Sensitive = valor;
			entry_alergia_paciente.Sensitive = valor;
			entry_observacion_ingreso.Sensitive = valor;
			entry_lugar_nacimiento.Sensitive = valor;
			combobox_aseguradora.Sensitive = valor;
			combobox_municipios.Sensitive = valor;
			combobox_estado.Sensitive = valor;
			entry_entre_calles.Sensitive = valor;
		}
		
		void on_button_validaadmision_clicked (object sender, EventArgs a) 
		{
			Gtk.Button onButton = sender as Gtk.Button;
			validando_admision(onButton.Name);
		}
		
		void validando_admision(string name_button_validation)
		{
			bool validando_tipo_paciente = false;
			string nombre_error = "Error";
			if(tipopaciente != ""){
				validando_tipo_paciente = true;
			}else{
				nombre_error = "Elija el tipo de Paciente... Verifique...";
				validando_tipo_paciente = false;
			}
			if(id_tipopaciente == 100){
				if(idempresa_paciente == 1){
					nombre_error = "La admision es "+tipopaciente+" elija un Convenio/Campaña... Verifique...";
					validando_tipo_paciente = false;
				}
			}
			if(id_tipopaciente == 102){
				if(idempresa_paciente == 1){
					nombre_error = "La admision es "+tipopaciente+" elija una Empresa... Verifique...";
					validando_tipo_paciente = false;
				}
			}
			if(id_tipopaciente == 106){
				if(idempresa_paciente == 1){
					nombre_error = "La admision es "+tipopaciente+" elija un Tipo de Tarjeta... Verifique...";
					validando_tipo_paciente = false;
				}
			}
			if(id_tipopaciente == 400){ 
			   if(idaseguradora == 1){
					nombre_error = "La admision es "+tipopaciente+" elija una Aseguradora... Verifique...";
					validando_tipo_paciente = false;
				}
			}
			if(id_tipopaciente == 500){ 
			   if(idempresa_paciente == 1){
					nombre_error = "La admision es "+tipopaciente+" elija un Municipio... Verifique...";
					validando_tipo_paciente = false;
				}
			}
			if(id_tipopaciente == 700){
				if(idempresa_paciente == 1){
					nombre_error = "La admision es "+tipopaciente+" elija una Convenio/Campaña... Verifique...";
					validando_tipo_paciente = false;
				}
			}
			if(validando_tipo_paciente == true){
				if(name_button_validation == "button_responsable"){
					responsablepaciente();
				}
				if(name_button_validation == "button_admision"){
					admision_para_internamiento();
				}
			}else{
				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
							MessageType.Info,ButtonsType.Close,nombre_error);
				msgBoxError.Run ();			msgBoxError.Destroy();
			}
		}
		
		void responsablepaciente()
		{
			Glade.XML gxml = new Glade.XML (null, "registro_admision.glade", "datos_del_responsable", null);
			gxml.Autoconnect (this);	                	
			datos_del_responsable.Show();
			entry_empresa_convenio.ModifyBase(StateType.Normal, new Gdk.Color(85,240,92)); // Color Amarillo.ModifyBase(StateType.Normal, new Gdk.Color(255,243,169)); // Color Amarillo
			button_salir.Clicked += new EventHandler(on_cierraventanas_clicked);					
			button_paciente_responsable.Clicked += new EventHandler(on_button_paciente_responsable_clicked);			
			button_misma_direccion.Clicked += new EventHandler(on_button_misma_direccion_clicked);			
			button_graba_responsable.Clicked += new EventHandler(on_button_graba_responsable_clicked);
			if (grabarespocuenta == true){
				if(id_tipopaciente != id_tipopaciente_valid){ 
					id_tipopaciente_valid = id_tipopaciente;
					grabarespocuenta = false;
				}else{
					for (int i = 0; i < arraydocumentos.Count; i++) {
      					Item documento_foo = (Item) arraydocumentos[i];
						if (documento_foo.col01_ == ""){
							grabarespocuenta = false;
						}
    				}
				}
				entry_nombre_responsable.Text = nombr_respo;
				entry_telefono_responsable.Text = telef_respo;
				entry_direcc_responsable.Text = direc_respo;
				entry_ocupacion_responsable.Text = ocupa_respo;
				entry_empresa_responsable.Text = empre_respo;
				entry_observacion_responsable.Text = observacion_respo;
				entry_telef_empre_respo.Text = telef_empre_respo;
				entry_direc_empre_respo.Text = direc_empre_respo;
				crea_treeview_documentos(false);
				llenado_tipos_documentos(false);
				llenado_combobox(1,parentezcoresponsable,combobox_parent_responsable,"array","","","",args_parentesco,args_id_array,"");
			}else{
				grabarespocuenta = false;
				crea_treeview_documentos(true);
				llenado_tipos_documentos(true);
				llenado_combobox(0,"",combobox_parent_responsable,"array","","","",args_parentesco,args_id_array,"");
			}
		}
		
		void on_button_graba_responsable_clicked (object sender, EventArgs a)
		{
			if(entry_nombre_responsable.Text.Trim() != "" && (bool) verifica_los_documentos() == true ){
				nombr_respo = entry_nombre_responsable.Text.ToString().ToUpper();
				telef_respo = entry_telefono_responsable.Text.ToString().ToUpper();
				direc_respo = entry_direcc_responsable.Text.ToString().ToUpper();
				ocupa_respo = entry_ocupacion_responsable.Text.ToString().ToUpper();
				empre_respo = entry_empresa_responsable.Text.ToString().ToUpper();
				observacion_respo = entry_observacion_responsable.Text.ToUpper();
				telef_empre_respo = entry_telef_empre_respo.Text.ToUpper();
				direc_empre_respo = entry_direc_empre_respo.Text.ToUpper();
				grabarespocuenta = true;
				id_tipopaciente_valid = id_tipopaciente;
				Widget win = (Widget) sender;
				win.Toplevel.Destroy();
			}else{
				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
							MessageType.Info,ButtonsType.Close,"No debe dejar los campos de nombre ");
				msgBoxError.Run ();			msgBoxError.Destroy();
			}
		}
		
		void crea_treeview_documentos(bool llenado_treeview)
		{
			treeViewEngineDocumentos = new ListStore(typeof (string),typeof (string),typeof (string));	        							   
			treeview_documentos.Model = treeViewEngineDocumentos;			
			TreeViewColumn col00 = new TreeViewColumn();
			CellRendererText cellrt00 = new CellRendererText();
			col00.Title = "Documento";
			col00.PackStart(cellrt00, true);
			col00.AddAttribute (cellrt00, "text", 0);
			col00.SortColumnId = (int) colum_documentos.col00;
			
			TreeViewColumn col01 = new TreeViewColumn();
			CellRendererText cellrt01 = new CellRendererText();
			col01.Title = "Captura";
			col01.PackStart(cellrt01, true);
			col01.AddAttribute (cellrt01, "text", 1);
			cellrt01.Editable = true;
			cellrt01.Edited += NumberCellEdited;
			col01.SortColumnId = (int) colum_documentos.col01;
			
			treeview_documentos.AppendColumn(col00);
			treeview_documentos.AppendColumn(col01);
		}
		
		enum colum_documentos
		{
			col00,col01
		}
		
		/// <summary>
		/// Llenado_tipos_documentos the specified read ArrayList.
		/// </summary>
		/// <param name='llenado_treeview'>
		/// Llenado_treeview.
		/// </param>
		void llenado_tipos_documentos(bool llenado_treeview)
		{
			if(llenado_treeview == true){
				NpgsqlConnection conexion; 
				conexion = new NpgsqlConnection (connectionString+nombrebd);
				try{
					conexion.Open ();
					NpgsqlCommand comando; 
					comando = conexion.CreateCommand ();
					comando.CommandText = "SELECT * FROM osiris_erp_documentos_convenio WHERE id_tipo_documento = '" +id_tipodocumentopx.ToString().Trim()+"' ORDER BY id_secuencia;";
					//Console.WriteLine(comando.CommandText);
					NpgsqlDataReader lector = comando.ExecuteReader ();
					while (lector.Read()){
						//numero_nomina	departamento
						if((bool) lector["numero_nomina"] == true && (bool) lector["departamento"] == false){
							documento_foo = new Item (lector["descripcion_documento"].ToString().Trim(),numeronomina_px.ToString().Trim(),numeronomina_px.ToString().Trim());
							arraydocumentos.Add(documento_foo);
							treeViewEngineDocumentos.AppendValues(lector["descripcion_documento"].ToString().Trim(),numeronomina_px.ToString().Trim());
						}
						if((bool) lector["departamento"] == true && (bool) lector["numero_nomina"] == false){
							documento_foo = new Item (lector["descripcion_documento"].ToString().Trim(),departamento_px,departamento_px);
							arraydocumentos.Add(documento_foo);
							treeViewEngineDocumentos.AppendValues(lector["descripcion_documento"].ToString().Trim(),departamento_px);
						}
						if((bool) lector["numero_nomina"] == false && (bool) lector["departamento"] == false){
							documento_foo = new Item (lector["descripcion_documento"].ToString().Trim(),"","");
							arraydocumentos.Add(documento_foo);
							treeViewEngineDocumentos.AppendValues(lector["descripcion_documento"].ToString().Trim(),"");
						}
					}
				}catch (NpgsqlException ex){
		   			MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
											MessageType.Error, ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
					msgBoxError.Run ();			msgBoxError.Destroy();
				}
				conexion.Close ();
			}else{
				for (int i = 0; i < arraydocumentos.Count; i++) {
      				Item documento_foo = (Item) arraydocumentos[i];
					treeViewEngineDocumentos.AppendValues(documento_foo.col00_,documento_foo.col01_);
    			}		
			}
			entry_empresa_convenio.Text = tipopaciente+"/"+nombre_aseguradora+entry_empresa.Text;
		}
		
		struct Item
 		{
 			public string col00_{
				get { return col_item_00; }
				set { col_item_00 = value; }
			}
			public string col01_{
				get { return col_item_01; }
				set { col_item_01 = value; }
			}
			public string col02_{
				get { return col_item_02; }
				set { col_item_02 = value; }
			}
			private string col_item_00;
			private string col_item_01;
			private string col_item_02;
			
			public Item (string col_item_00,string col_item_01,string col_item_02)
			{
				this.col_item_00 = col_item_00;
				this.col_item_01 = col_item_01;
				this.col_item_02 = col_item_02;
			}
		}
		
		void NumberCellEdited (object sender, EditedArgs args)
		{
			if (sender.GetType().Name == "CellRendererText"){
				
			}
			TreePath path = new TreePath (args.Path);
 			TreeIter iter;
			int i = path.Indices[0];
			try{
				string cambio_datos = args.NewText.ToUpper();
				Item documento_foo_tmp;
				documento_foo_tmp = (Item) arraydocumentos[i];
				arraydocumentos[i] = new Item (documento_foo_tmp.col00_,cambio_datos,"");
				treeViewEngineDocumentos.GetIter (out iter, path);
				treeViewEngineDocumentos.SetValue(iter,(int) colum_documentos.col01,cambio_datos);
			}catch (Exception e) {
				Console.WriteLine(e.Message);
				return;
			}
		}
		
		/*
		void NumberCellEdited_Autorizado (object o, EditedArgs args)
		{
			Gtk.TreeIter iter;
			bool esnumerico = false;
			int var_paso = 0;
			int largo_variable = args.NewText.ToString().Length;
			string toma_variable = args.NewText.ToString();			
			treeViewEngineDocumentos.GetIter (out iter, new Gtk.TreePath (args.Path));			
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
				//treeViewEngineDocumentos.SetValue(iter,(int) Col_traspaso.col_autorizado,args.NewText);
				//bool old = (bool) filter.Model.GetValue (iter,0);
				//filter.Model.SetValue(iter,0,!old);
			}
 		}*/
		
		void on_button_paciente_responsable_clicked (object sender, EventArgs a)
		{
			entry_nombre_responsable.Text = (string) entry_nombre_1.Text+" "+(string) entry_nombre_2.Text+" "+
							(string) entry_apellido_paterno.Text+" "+(string) entry_apellido_materno.Text;
			
			entry_direcc_responsable.Text = (string) entry_calle.Text+" "+entry_numero.Text+" Col. "+entry_colonia.Text+" CP. "+
							(string) entry_CP.Text+", "+(string) municipios+", "+(string) estado;
			//entry_empresa_responsable.Text = (string) entry_empresa.Text;
			entry_ocupacion_responsable.Text = (string) entry_ocupacion.Text;
			
		}
		
		void on_button_misma_direccion_clicked (object sender, EventArgs a)
		{
			entry_direcc_responsable.Text = (string) entry_calle.Text+" "+entry_numero.Text+" Col. "+entry_colonia.Text+" CP. "+
							(string) entry_CP.Text+", "+(string) municipios+", "+(string) estado;
			entry_telefono_responsable.Text = (string) entry_telcasa.Text;
			
		}
		
		bool verifica_los_documentos()
		{			
			return true;
		}
			
		// Activa en enter en la busqueda de los productos
		[GLib.ConnectBefore ()]   	  // Esto es indispensable para que funcione    
		public void onKeyPressEvent_enter(object o, Gtk.KeyPressEventArgs args)
		{
			if (args.Event.Key.ToString() == "Return" || args.Event.Key.ToString() == "KP_Enter"){
				if(busqueda == "empresas") {
					llenado_lista_empresas();
				}				
			}
		}
	}
				
	class documentos_pacientes
	{
		public string valor_string;
		public DateTime valor_fecha;
		public int valor_numerico;
		public bool valor_boleano;
		public documentos_pacientes (string valor_string, DateTime valor_fecha,int valor_numerico,bool valor_boleano)
		{
			valor_string = valor_string;
			valor_fecha = valor_fecha;
			valor_numerico = valor_numerico;
			valor_boleano = valor_boleano;
       }
   }
}