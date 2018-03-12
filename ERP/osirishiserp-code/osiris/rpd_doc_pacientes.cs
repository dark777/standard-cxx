///////////////////////////////////////////////////////////
// project created on 24/10/2006 at 10:20 a
// Sistema Hospitalario OSIRIS
// Monterrey - Mexico
//
// Autor   Ing. Juan Antonio Peña Gzz.(Programation & Glade's window)
// ayuda:  Ing. Erick Eduardo Gonzalez Reyes (Programation & Glade's window)
//         Ing. R. Israel Peña Gonzalez	(Programation & Glade's window)
// mejoras Ing. Daniel Olivares Cuevas 22/08/2012 29/12/2014 arcangeldoc@openmailbox.org (Programation & Glade's window)
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

namespace osiris
{
	public class impr_doc_pacientes
	{
		// Boton general para salir de las ventanas
		// Todas la ventanas en glade este boton debe estra declarado identico
		[Widget] Gtk.Button button_salir;
		
		// Para todas las busquedas este es el nombre asignado
		// se declara una vez
		[Widget] Gtk.Entry entry_expresion;
		[Widget] Gtk.Button button_selecciona;
		[Widget] Gtk.Button button_buscar_busqueda;
		//busqueda diagnostico cie-10
		[Widget] Gtk.TreeView lista_de_producto;
		[Widget] Gtk.RadioButton radiobutton_nombre;
		[Widget] Gtk.RadioButton radiobutton_codigo;
		
		
		// Declarando ventana principal
		[Widget] Gtk.Window busqueda_folio;
		[Widget] Gtk.Entry entry_folio_servicio;
		[Widget] Gtk.Entry entry_fecha_admision;
		[Widget] Gtk.Entry entry_hora_registro;
		[Widget] Gtk.Entry entry_fechahora_alta;
		[Widget] Gtk.Entry entry_pid_paciente;
		[Widget] Gtk.Entry entry_nombre_paciente;
		[Widget] Gtk.Entry entry_edad_paciente = null;
		[Widget] Gtk.Entry entry_telefono_paciente;
		[Widget] Gtk.Entry entry_doctor;
		[Widget] Gtk.Entry entry_especialidad;
		[Widget] Gtk.Entry entry_tipo_paciente;
		[Widget] Gtk.Entry entry_aseguradora;
		[Widget] Gtk.Entry entry_poliza = null;
		[Widget] Gtk.Entry entry_observaciones = null;
		[Widget] Gtk.Entry entry_servicio_medico = null;
		[Widget] Gtk.Button button_buscar_paciente = null;
		[Widget] Gtk.Button button_selec_folio = null;



		[Widget] Gtk.RadioButton radiobutton_tipo_cirugia  = null;
		[Widget] Gtk.ComboBox combobox_tipo_cirugia = null;
		[Widget] Gtk.CheckButton checkbutton_camb_dats;
		[Widget] Gtk.ComboBox combobox_diagprimeravez = null;
		[Widget] Gtk.Entry entry_estatus = null;
		[Widget] Gtk.RadioButton radiobutton_med;
		[Widget] Gtk.RadioButton radiobutton_med_trat;
		[Widget] Gtk.RadioButton radiobutton_motingreso;
		[Widget] Gtk.RadioButton radiobutton_cirugia;
		[Widget] Gtk.RadioButton radiobutton_diag_cie10;
		[Widget] Gtk.RadioButton radiobutton_diag_final;
		[Widget] Gtk.RadioButton radiobutton_diag_primeravez;
		[Widget] Gtk.RadioButton radiobutton_observacion = null;
		[Widget] Gtk.RadioButton radiobutton_servmedico = null;
		[Widget] Gtk.RadioButton radiobutton_pq_medico = null;
		[Widget] Gtk.Entry entry_id_paquete_med = null;
		[Widget] Gtk.Entry entry_paquete_medico = null;
		[Widget] Gtk.Button button_busca_paquete = null;
		[Widget] Gtk.ComboBox combobox_reportes = null;
		[Widget] Gtk.Button button_imprimir_reportes = null;
				
		// declarando tab2 quirofano
		[Widget] Gtk.CheckButton checkbutton_nropaseqx = null;
		[Widget] Gtk.ComboBox combobox_nropaseqx = null;
		[Widget] Gtk.Entry entry_fechapaseqx = null;
		[Widget] Gtk.CheckButton checkbutton_cirujano1 = null;
		[Widget] Gtk.Entry entry_id_cirujano1 = null;
		[Widget] Gtk.Entry entry_nombre_cirujano1 = null;
		[Widget] Gtk.Button button_buscar_cirujano1 = null;
		[Widget] Gtk.CheckButton checkbutton_cirugiapaseqx = null;
		[Widget] Gtk.Entry entry_cirugia_paseqx = null;
		[Widget] Gtk.Button button_busca_cxpaseqx = null;
		[Widget] Gtk.CheckButton checkbutton_lente = null;
		[Widget] Gtk.Entry entry_id_producto = null;
		[Widget] Gtk.Entry entry_descripcion_producto = null;
		[Widget] Gtk.Button button_busca_producto = null;
		[Widget] Gtk.Entry entry_nroserie = null;
		[Widget] Gtk.CheckButton checkbutton_tipo_anestesia = null;
		[Widget] Gtk.ComboBox combobox_tipo_anestecia = null;
		[Widget] Gtk.CheckButton checkbutton_anestesiologo = null;
		[Widget] Gtk.Entry entry_id_anestesiologo = null;
		[Widget] Gtk.Entry entry_anestesiologo = null;
		[Widget] Gtk.Button button_buscar_anestesiologo = null;
		[Widget] Gtk.Button button_guardar_tabqx = null;		
		[Widget] Gtk.CheckButton checkbutton_observacion_qx = null;
		[Widget] Gtk.Entry entry_observacion_qx = null;		
		[Widget] Gtk.CheckButton checkbutton_cirujano2 = null;
		[Widget] Gtk.Entry entry_id_cirujano2 = null;
		[Widget] Gtk.Entry entry_nombre_cirujano2 = null;
		[Widget] Gtk.Button button_buscar_cirujano2 = null;
			
		// declarando tab 3 Vision
		[Widget] Gtk.ComboBox combobox_tecnicovision = null;
		[Widget] Gtk.ComboBox combobox_maquinavision = null;
		[Widget] Gtk.Entry entry_nro_estudiomaquina = null;
		[Widget] Gtk.Button button_guardar_tabvision = null;
		[Widget] Gtk.CheckButton checkbutton_tecnicovision = null;
		[Widget] Gtk.CheckButton checkbutton_maquinavision = null;
		
		// Declarando Tab Admision
		[Widget] Gtk.CheckButton checkbutton_edit_admision = null;
		[Widget] Gtk.CheckButton checkbutton_edit_aseguradora = null;
		[Widget] Gtk.Entry entry_edit_idaseguradora = null;
		[Widget] Gtk.Entry entry_nombre_aseguradora = null;
		[Widget] Gtk.Button button_busca_aseguradora = null;	
		[Widget] Gtk.CheckButton checkbutton_edit_instempre = null;
		[Widget] Gtk.Entry entry_edit_idinstempr = null;
		[Widget] Gtk.Entry entry_nombre_instempr = null;
		[Widget] Gtk.Button button_busca_instempr = null;
				
		[Widget] Gtk.CheckButton checkbutton_edit_tipopx = null;
		[Widget] Gtk.ComboBox combobox_tipo_paciente = null;
		[Widget] Gtk.CheckButton checkbutton_edit_tipoadmin = null;
		[Widget] Gtk.ComboBox combobox_tipo_admision = null;
		[Widget] Gtk.CheckButton checkbutton_fech_altamed = null;
		[Widget] Gtk.Entry entry_ano_altamed = null;
		[Widget] Gtk.Entry entry_mes_altamed = null;
		[Widget] Gtk.Entry entry_dia_altamed = null;
		[Widget] Gtk.Entry entry_hora_altamed = null;
		[Widget] Gtk.Entry entry_minuto_altamed = null;

		[Widget] Gtk.CheckButton checkbutton_regadm = null;
		[Widget] Gtk.Entry entry_ano_regadm = null;
		[Widget] Gtk.Entry entry_mes_regadm = null;
		[Widget] Gtk.Entry entry_dia_regadm = null;
		[Widget] Gtk.Entry entry_hora_regadm = null;
		[Widget] Gtk.Entry entry_minuto_regadm = null;

		[Widget] Gtk.Button button_graba_regentrada = null;

		[Widget] Gtk.Button button_graba_edirespo = null;
		
		//Declarando la barra de estado
		[Widget] Gtk.Statusbar statusbar_caja;
		
		/////// Ventana Busqueda de paciente\\\\\\\\
		//[Widget] Gtk.Window busca_paciente;
		[Widget] Gtk.TreeView lista_de_Pacientes;
		[Widget] Gtk.Button button_nuevo_paciente;
		[Widget] Gtk.RadioButton radiobutton_busca_apellido;
		[Widget] Gtk.RadioButton radiobutton_busca_nombre;
		[Widget] Gtk.RadioButton radiobutton_busca_expediente;
		
		/////// Ventana Busqueda de paciente\\\\\\\\
		[Widget] Gtk.Window busca_producto;
		//[Widget] Gtk.TreeView lista_de_producto;
		[Widget] Gtk.Label label33;
		[Widget] Gtk.Label label34;
		
		//Seleccion de documentos a imprimir
		[Widget] Gtk.Button button_busc_medic_diag;
		[Widget] Gtk.Button button_busc_medic_trat;
		[Widget] Gtk.Button button_busc_diag;
		[Widget] Gtk.Button button_busc_cirugia;
		[Widget] Gtk.Button button_guardar;
		[Widget] Gtk.Entry entry_medic_diag;
		[Widget] Gtk.Entry entry_med_trat;
		[Widget] Gtk.TextView textview_motivo_ingreso = null;
		[Widget] Gtk.Entry entry_docimp_cirugia;		
		
		// busqueda de Medicos
		[Widget] Gtk.Window buscador_medicos;
		[Widget] Gtk.Button button_buscar;
		[Widget] Gtk.TreeView lista_de_medicos;
		
		// Ventana de Busqueda de Cirugias
		[Widget] Gtk.Window busca_cirugias;
		[Widget] Gtk.TreeView lista_cirugia;
		[Widget] Gtk.Button button_llena_cirugias;
		
		[Widget] Gtk.ComboBox combobox_tipo_busqueda;
		
		//nuevos:
		//[Widget] Gtk.RadioButton radiobutton_diag_cie_10;
		//[Widget] Gtk.RadioButton radiobutton_diag_final;
		[Widget] Gtk.Button button_diag_final;
		[Widget] Gtk.Entry entry_diag_cie10;
		[Widget] Gtk.Entry entry_diag_final;
		
		[Widget] Gtk.Entry entry_expresion_busca;
								
		private TreeStore treeViewEngineMedicos;
		private TreeStore treeViewEngineBusca;
		
		//Declaracion de ventana de error
		protected Gtk.Window MyWin;
		protected Gtk.Window MyWinError;
		
		TextBuffer buffer = new TextBuffer(null);
		TextIter insertIter;
		
		// Declaracion de variables publicas		
		string paquete = "";
		string selcampo = "";
		string selmedico = "";
						
		int idcirugianumero = 1;
		int idcxpaseqx = 1;
		int idmednum = 0;
	    int folioservicio = 0;	    			// Toma el valor de numero de atencion de paciente
		int PidPaciente = 0;					// Toma la actualizacion del pid del paciente
		int id_tipopaciente;        			// Toma el valor del tipo de paciente
		int id_tipopacienteanterior;
		int id_tipoadmision = 10;				// Toma el valor del tipo de internamiento
		int id_tipoadmisionanterior = 10;
				
		int iddiagnosticocie10 = 1;				// Toma el valor del id de tabla de diagnosticos
		int iddiagnosticofinal = 1;				// toma el valor del id del diagnostico final
		int idaseguradora_paciente = 1;
		int idempresa_paciente = 1;
		bool cuenta_cerrada;
		string nropaseqxurg = "0";				//toma el valor del pase quirurgico o de urgencias
		int cuenta_check_edicion = 0;
		
		//busqueda de diagnostico
		private TreeStore treeViewEngineBusca2;	// Para la busqueda de Productos
    	private TreeStore treeViewEngineSelec;	// Lista de Productos seleccionados
    	private TreeStore treeViewEngineResumen;	// Lista de Productos seleccionados
				
		int idmedico = 1;
		int idmedicotratante = 1;
 		string nombmedico;
 		string tipobusqueda = "AND osiris_his_medicos.nombre1_medico LIKE '";
		string descrip_tipo_cirugia = "";
		string diag_primeravez = "";
		bool tipomedico = true;
		string LoginEmpleado;
		string NomEmpleado;
		string AppEmpleado;
		string ApmEmpleado;
		string NomEmpleados;	
		string connectionString;
		string nombrebd;
		string fechanacimientopx;
		string sexopacientepx;
		string tipoantestesia = "";
		string nombretecnicovision = "";
		string tipodereporte = "";
		string[] args_args = {""};
		int[] args_id_array = {0,1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20};
		string[] args_tipos_reporte = {"","PROTOCOLO DE ADMISION","PASE DE INGRESO","PASE SECCION 50","CONSENTIMIENTO INFORMADO","CONTRATO","ROTULOS HABITACION","HISTORIA CLINICA"};
		string[] args_tipos_cirugias = {"","CIRUGIA AMBULATORIA","CIRUGIA PROGRAMADA","SIN CIRUGIA"};
		string[] args_diag_primeravez = {"","SI","NO"};
		string[] args_tipobusqueda = {"","PRIMER NOMBRE","SEGUNDO NOMBRE","APELLIDO PATERNO","APELLIDO MATERNO","CEDULA MEDICA","ESPECIALIDAD"};
		string[] args_tipoanestesia = {"","INTRACAM","INTRACAM + SEDACION","RETROBULVAR","RETROBULVAR + SEDACION","TOPICA","NO TOPICA","EPIDURAL O RAQUEA","LOCAL + SEDACION","GENERAL","ENDOVENOSA","TOPICA + LOCAL","TOPICA + SEDACION"};
		string[] args_tecnicosvision = {"",
										"AZALIA DIAZ ORTIZ",
										"ANGELICA SARAHI ROSALES SAUCEDO",
										"DAMARIS LUCERO RICO TORRES",
										"ALEJANDRA ABIGAIL TORRES CELESTINO"};
		class_conexion conexion_a_DB = new class_conexion();
		class_public classpublic = new class_public();
		class_buscador classfind_data = new class_buscador();
		
		public impr_doc_pacientes(string LoginEmp, string NomEmpleado_, string AppEmpleado_, string ApmEmpleado_, string nombrebd_,string folioserv_,int control) 
		{
			LoginEmpleado = LoginEmp;
			NomEmpleado = NomEmpleado_;
			AppEmpleado = AppEmpleado_;
			ApmEmpleado = ApmEmpleado_;
			connectionString = conexion_a_DB._url_servidor+conexion_a_DB._port_DB+conexion_a_DB._usuario_DB+conexion_a_DB._passwrd_user_DB;
			nombrebd = conexion_a_DB._nombrebd;
									
			Glade.XML gxml = new Glade.XML (null, "impr_documentos.glade", "busqueda_folio", null);
			gxml.Autoconnect (this);
	        
			// Muestra ventana de Glade
			busqueda_folio.Show();
			
			buffer = textview_motivo_ingreso.Buffer;
			classpublic.CreateTags(buffer);
			insertIter = buffer.StartIter;
						
			entry_folio_servicio.Text = folioserv_;
			entry_fecha_admision.IsEditable = false;
			entry_hora_registro.IsEditable = false;
			entry_fechahora_alta.IsEditable = false;
			entry_nombre_paciente.IsEditable = false;
			entry_pid_paciente.IsEditable = false;
			entry_telefono_paciente.IsEditable = false;
			entry_doctor.IsEditable = false;
			entry_especialidad.IsEditable = false;
			entry_tipo_paciente.IsEditable = false;
			entry_aseguradora.IsEditable = false;
			entry_poliza.IsEditable = false;
			entry_edad_paciente.IsEditable = false;
			textview_motivo_ingreso.Sensitive = false;
			entry_observaciones.Sensitive = false;
			entry_servicio_medico.Sensitive = false;
			entry_medic_diag.Sensitive = false;
			entry_diag_cie10.Sensitive = false;
		    entry_diag_final.Sensitive = false;
			entry_med_trat.Sensitive = false;
			entry_docimp_cirugia.Sensitive = false;
			button_busc_medic_diag.Sensitive = false;
		    button_busc_medic_trat.Sensitive = false;
		    button_busc_diag.Sensitive = false;
			button_diag_final.Sensitive = false;
		    button_busc_cirugia.Sensitive = false;
		    button_guardar.Sensitive = false;
			radiobutton_med.Sensitive= false;
			radiobutton_med_trat.Sensitive= false;
			radiobutton_motingreso.Sensitive= false;
			radiobutton_diag_cie10.Sensitive = false;
		    radiobutton_diag_final.Sensitive = false;
			radiobutton_cirugia.Sensitive= false;
			radiobutton_tipo_cirugia.Sensitive= false;
			radiobutton_observacion.Sensitive = false;
			radiobutton_servmedico.Sensitive = false;
			radiobutton_diag_primeravez.Sensitive = false;
			radiobutton_pq_medico.Sensitive = false;
			combobox_tipo_cirugia.Sensitive = false;
			combobox_diagprimeravez.Sensitive = false;
			button_guardar_tabqx.Sensitive = false;
			entry_id_paquete_med.Sensitive = false;
			entry_paquete_medico.Sensitive = false;
			button_busca_paquete.Sensitive = false;									
			if( control == 1){
				entry_folio_servicio.Sensitive = false;
				entry_folio_servicio.ModifyText(StateType.Normal, new Gdk.Color(255,0,0));
				llenado_de_datos_paciente( (string) entry_folio_servicio.Text );
			}				
			entry_folio_servicio.KeyPressEvent += onKeyPressEvent_enter_folio;
			button_buscar_paciente.Clicked += new EventHandler(on_button_buscar_paciente_clicked);
			button_selec_folio.Clicked += new EventHandler(on_selec_folio_clicked);

			/*
			button_imprimir_protocolo.Clicked += new EventHandler(on_button_imprimir_protocolo_clicked);
			button_pase_de_ingreso.Clicked += new EventHandler(on_button_pase_de_ingreso_clicked);
			button_cons_informado.Clicked += new EventHandler(on_button_cons_informado_clicked);
			button_contrato_prest.Clicked += new EventHandler(on_button_contrato_prest_clicked);
			button_historia_clinica.Clicked += new EventHandler(on_button_historia_clinica_clicked);
			*/

			button_imprimir_reportes.Clicked += new EventHandler(on_button_imprimir_reportes_clicked);

			button_busca_producto.Clicked += new EventHandler(on_button_busca_producto_clicked);
			// Sale de la ventana
			button_salir.Clicked += new EventHandler(on_cierraventanas_clicked);	    	
	    	checkbutton_camb_dats.Clicked  += new EventHandler(on_checkbutton_camb_dats_clicked);			
			button_busc_medic_diag.Clicked += new EventHandler(on_button_busca_medicos_clicked);			
			button_busc_medic_trat.Clicked += new EventHandler(on_button_busca_medicos_clicked);			
			button_busc_diag.Clicked += new EventHandler(on_button_busc_diag_clicked);			
			button_diag_final.Clicked += new EventHandler(on_button_busc_diag_final_clicked);			
			button_busc_cirugia.Clicked += new EventHandler(on_button_busc_cirugia_clicked);
			button_busca_paquete.Clicked += new EventHandler(on_button_busc_cirugia_clicked);
			button_guardar.Clicked += new EventHandler(on_button_guardar_clicked);			//radiobuttons pacientes:
			radiobutton_med.Clicked += new EventHandler(on_radio_clicked);			
			radiobutton_med_trat.Clicked += new EventHandler(on_radio_clicked);			
			radiobutton_motingreso.Clicked += new EventHandler(on_radio_clicked);			
			radiobutton_diag_cie10.Clicked += new EventHandler(on_radio_clicked);		
			radiobutton_diag_final.Clicked += new EventHandler(on_radio_clicked);			
			radiobutton_cirugia.Clicked += new EventHandler(on_radio_clicked);			
			radiobutton_tipo_cirugia.Clicked += new EventHandler(on_radio_clicked);			
			radiobutton_diag_primeravez.Clicked += new EventHandler(on_radio_clicked);
			radiobutton_observacion.Clicked += new EventHandler(on_radio_clicked);
			radiobutton_servmedico.Clicked += new EventHandler(on_radio_clicked);
			
			// tab quirofano
			checkbutton_nropaseqx.Clicked += new EventHandler(on_checkbutton_clicked);
			checkbutton_lente.Clicked += new EventHandler(on_checkbutton_clicked);
			checkbutton_tipo_anestesia.Clicked += new EventHandler(on_checkbutton_clicked);
			checkbutton_anestesiologo.Clicked += new EventHandler(on_checkbutton_clicked);
			checkbutton_observacion_qx.Clicked += new EventHandler(on_checkbutton_clicked);
			checkbutton_cirujano2.Clicked += new EventHandler(on_checkbutton_clicked);
			checkbutton_cirujano1.Clicked += new EventHandler(on_checkbutton_clicked);
			checkbutton_cirugiapaseqx.Clicked += new EventHandler(on_checkbutton_clicked);			
			button_buscar_anestesiologo.Clicked += new EventHandler(on_button_buscar_anestesiologo_clicked);
			button_buscar_cirujano1.Clicked += new EventHandler(on_button_buscar_cirujano1_clicked);
			button_buscar_cirujano2.Clicked += new EventHandler(on_button_buscar_cirujano2_clicked);
			button_busca_cxpaseqx.Clicked += new EventHandler(on_button_busca_cxpaseqx_clicked);
			button_guardar_tabqx.Clicked += new EventHandler(on_button_guardar_tab_clicked);
			
			// tab vision
			button_guardar_tabvision.Clicked += new EventHandler(on_button_guardar_tab_clicked);
			checkbutton_tecnicovision.Clicked += new EventHandler(on_checkbutton_clicked);
			checkbutton_maquinavision.Clicked += new EventHandler(on_checkbutton_clicked);
			
			// tab Admision
			checkbutton_edit_admision.Clicked += new EventHandler(on_checkbutton_clicked);
			checkbutton_edit_aseguradora.Clicked += new EventHandler(on_checkbutton_clicked);
			checkbutton_edit_instempre.Clicked += new EventHandler(on_checkbutton_clicked);
			checkbutton_edit_tipopx.Clicked += new EventHandler(on_checkbutton_clicked);
			checkbutton_edit_tipoadmin.Clicked += new EventHandler(on_checkbutton_clicked);
			checkbutton_fech_altamed.Clicked += new EventHandler(on_checkbutton_clicked);
			button_graba_regentrada.Clicked += new EventHandler(on_button_guardar_tab_clicked);
			button_busca_instempr.Clicked += new EventHandler(on_button_busca_instempr_clicked);
			button_busca_aseguradora.Clicked += new EventHandler(on_button_busca_aseguradora_clicked);
			checkbutton_regadm.Clicked += new EventHandler(on_checkbutton_clicked);
			
			button_graba_edirespo.Clicked += new EventHandler(on_button_guardar_tab_clicked);
			
			// desactivo tab Quirofano
			entry_id_producto.Sensitive = false;
			entry_descripcion_producto.Sensitive = false;
			button_busca_producto.Sensitive = false;
			entry_nroserie.Sensitive = false;			
			combobox_tipo_anestecia.Sensitive = false;			
			entry_id_anestesiologo.Sensitive = false;
			entry_anestesiologo.Sensitive = false;
			button_buscar_anestesiologo.Sensitive = false;
			entry_observacion_qx.Sensitive = false;
			entry_id_cirujano2.Sensitive = false;
			entry_nombre_cirujano2.Sensitive = false;
			button_buscar_cirujano2.Sensitive = false;
			entry_id_cirujano1.Sensitive = false;
			entry_nombre_cirujano1.Sensitive = false;
			button_buscar_cirujano1.Sensitive = false;
			entry_cirugia_paseqx.Sensitive = false;
			button_busca_cxpaseqx.Sensitive = false;
			
			// desactivo tab admision
			checkbutton_edit_admision.Sensitive = false;
			entry_edit_idaseguradora.Sensitive = false;
			entry_nombre_aseguradora.Sensitive = false;
			button_busca_aseguradora.Sensitive = false;
			entry_edit_idinstempr.Sensitive = false;
			entry_nombre_instempr.Sensitive = false;
			button_busca_instempr.Sensitive = false;
			combobox_tipo_paciente.Sensitive = false;
			combobox_tipo_admision.Sensitive = false;
			entry_ano_altamed.Sensitive = false;
			entry_mes_altamed.Sensitive = false;
			entry_dia_altamed.Sensitive = false;
			entry_hora_altamed.Sensitive = false;
			entry_minuto_altamed.Sensitive = false;
			entry_ano_regadm.Sensitive = false;
			entry_mes_regadm.Sensitive = false;
			entry_dia_regadm.Sensitive = false;
			entry_hora_regadm.Sensitive = false;
			entry_minuto_regadm.Sensitive = false;

			button_graba_regentrada.Sensitive = false;
			
			checkbutton_edit_aseguradora.Sensitive = false;
			checkbutton_edit_instempre.Sensitive = false;
			checkbutton_edit_tipopx.Sensitive = false;
			checkbutton_edit_tipoadmin.Sensitive = false;
			checkbutton_fech_altamed.Sensitive = false;
			checkbutton_regadm.Sensitive = false;
			
			// tab quirofano
			llenado_combobox(0,"",combobox_tipo_cirugia,"array","","","",args_tipos_cirugias,args_id_array,"");
			llenado_combobox(0,"",combobox_diagprimeravez,"array","","","",args_diag_primeravez,args_id_array,"");
			llenado_combobox(0,"",combobox_tipo_anestecia,"array","","","",args_tipoanestesia,args_id_array,"");
			llenado_combobox(0,"",combobox_tecnicovision,"array","","","",args_tecnicosvision,args_id_array,"");
			llenado_combobox(0,"",combobox_reportes,"array","","","",args_tipos_reporte,args_id_array,"");
			statusbar_caja.Pop(0);
			statusbar_caja.Push(1, "login: "+LoginEmpleado+"  |Usuario: "+NomEmpleado+" "+AppEmpleado+" "+ApmEmpleado);
			statusbar_caja.HasResizeGrip = false;
			/*
			char[] delimiterChars = {';'}; // delimitador de Cadenas
			//char[] delimiterChars = {' '}; // delimitador de Cadenas
			//string texto = (string) lector["prueba"]; // puede ser una campo de la base de datos tipo Text
			string texto = "1;daniel;olivares;cuevas;\n"+
			"2;genaro;cuevas;bazaldua;\n"+
			"3;gladys;perez;orellana;\n";
			string[] words = texto.Split(delimiterChars); // Separa las Cadenas
			string lineas_texto = "";			
			// Recorre la variable
			int contador = 1;
			foreach (string s in words){
				if (s.Length > 0){
					Console.WriteLine(s.ToString()+"    "+contador.ToString());
					lineas_texto += s;
					contador += 1;
				}
			}
			*/
		}
		
		void on_button_busca_producto_clicked(object sender, EventArgs args)
		{
			//string acceso_a_grupos = classpublic.lee_registro_de_tabla("osiris_almacenes","id_almacen"," WHERE osiris_almacenes.id_almacen = '"+this.idalmacen.ToString().Trim()+"' ","acceso_grupo_producto","int");
			// Los parametros de del SQL siempre es primero cuando busca todo y la otra por expresion
			// la clase recibe tambien el orden del query
			// es importante definir que tipo de busqueda es para que los objetos caigan ahi mismo
			int idalmacen = 5;   // quirofano			
			object[] parametros_objetos = {entry_id_producto,entry_descripcion_producto};
			string[] parametros_sql = {"SELECT to_char(osiris_productos.id_producto,'999999999999') AS codProducto,"+
						"osiris_productos.descripcion_producto,to_char(precio_producto_publico,'99999999.99') AS preciopublico,"+
						"aplicar_iva,to_char(osiris_erp_cobros_deta.porcentage_descuento,'999.99') AS porcentagesdesc,aplica_descuento,"+
						"descripcion_grupo_producto,descripcion_grupo1_producto,descripcion_grupo2_producto,to_char(costo_por_unidad,'999999999.99') AS costoproductounitario, "+
						"to_char(porcentage_ganancia,'99999.99') AS porcentageutilidad,to_char(costo_producto,'999999999.99') AS costoproducto "+
						"FROM osiris_erp_cobros_deta,osiris_productos,osiris_catalogo_almacenes,osiris_grupo_producto,osiris_grupo1_producto,osiris_grupo2_producto "+
						"WHERE osiris_productos.id_grupo_producto = osiris_grupo_producto.id_grupo_producto "+
						"AND osiris_productos.id_producto = osiris_catalogo_almacenes.id_producto " +
						"AND osiris_erp_cobros_deta.id_producto = osiris_catalogo_almacenes.id_producto "+
						"AND osiris_catalogo_almacenes.id_almacen = '"+idalmacen.ToString().Trim()+"' " +
						"AND folio_de_servicio = '"+entry_folio_servicio.Text+"' "+
						"AND osiris_catalogo_almacenes.eliminado = 'false' "+	
						"AND osiris_productos.id_grupo1_producto = osiris_grupo1_producto.id_grupo1_producto "+
						"AND osiris_productos.id_grupo2_producto = osiris_grupo2_producto.id_grupo2_producto "+
						"AND osiris_productos.cobro_activo = 'true' "};
			string[] parametros_string = {};
			string[,] args_buscador1 = {{"DESCRIPCION","AND osiris_productos.descripcion_producto LIKE '%","%' "},
										{"ID PRODUCTO","AND osiris_productos.id_producto ='","' "}};
			string[,] args_buscador2 = {{"ID PRODUCTO","AND osiris_productos.id_producto ='","' "},
										{"DESCRIPCION","AND osiris_productos.descripcion_producto LIKE '%","%' "}};
			string[,] args_orderby = {{"",""}};
			classfind_data.buscandor(parametros_objetos,parametros_sql,parametros_string,"find_producto_doc_medicos",0,args_buscador1,args_buscador2,args_orderby);
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
					case "combobox_diagprimeravez":
						diag_primeravez = (string) onComboBoxChanged.Model.GetValue(iter,0);
					break;
					case "combobox_tipo_cirugia":
						descrip_tipo_cirugia = (string) onComboBoxChanged.Model.GetValue(iter,0);
					break;
					case "combobox_tipo_busqueda":
						if((int) onComboBoxChanged.Model.GetValue(iter,1) == 1)  { tipobusqueda = "AND osiris_his_medicos.nombre1_medico LIKE '";}
						if((int) onComboBoxChanged.Model.GetValue(iter,1) == 2)  { tipobusqueda = "AND osiris_his_medicos.nombre2_medico LIKE '";}
						if((int) onComboBoxChanged.Model.GetValue(iter,1) == 3)  { tipobusqueda = "AND osiris_his_medicos.apellido_paterno_medico LIKE '";}
						if((int) onComboBoxChanged.Model.GetValue(iter,1) == 4)  { tipobusqueda = "AND osiris_his_medicos.apellido_materno_medico LIKE '";}
						if((int) onComboBoxChanged.Model.GetValue(iter,1) == 5)  { tipobusqueda = "AND osiris_his_medicos.cedula_medico LIKE '";}
						if((int) onComboBoxChanged.Model.GetValue(iter,1) == 6)  { tipobusqueda = "AND osiris_his_tipo_especialidad.descripcion_especialidad LIKE '";}
						llenando_lista_de_medicos();	
					break;
					case "combobox_tipo_anestecia":
						tipoantestesia = (string) onComboBoxChanged.Model.GetValue(iter,0);
					break;
					case "combobox_tecnicovision":
						nombretecnicovision = (string) onComboBoxChanged.Model.GetValue(iter,0);
					break;
					case "combobox_nropaseqx":
						nropaseqxurg = (string) onComboBoxChanged.Model.GetValue(iter,0);
						llenado_informacion_paseqxurg(nropaseqxurg.Trim(),folioservicio.ToString().Trim());
					break;
					case "combobox_tipo_admision":
						id_tipoadmision = (int) onComboBoxChanged.Model.GetValue(iter,1);
					break;
					case "combobox_tipo_paciente":
						id_tipopaciente = (int) onComboBoxChanged.Model.GetValue(iter,1);
						switch ((int) onComboBoxChanged.Model.GetValue(iter,1)){							
							case 400:
								// Asegurados
								checkbutton_edit_aseguradora.Active = true;
								checkbutton_edit_instempre.Active = false;
							break;
							case 100:
								// DIF
								checkbutton_edit_aseguradora.Active = false;
								checkbutton_edit_instempre.Active = true;
							break;
							case 102:
								// institucion o empresas
								checkbutton_edit_aseguradora.Active = false;
								checkbutton_edit_instempre.Active = true;
							break;
							case 104:
								// Seguro Popular/Secretaria de Salud
								checkbutton_edit_aseguradora.Active = false;
								checkbutton_edit_instempre.Active = true;
							break;
							case 106:
								// Tarjeta Medica
								checkbutton_edit_aseguradora.Active = false;
								checkbutton_edit_instempre.Active = true;
							break;
							case 500:
								// municipios						
								checkbutton_edit_aseguradora.Active = false;
								checkbutton_edit_instempre.Active = true;
							break;
							case 700:
								// SECCION 50
								checkbutton_edit_aseguradora.Active = false;
								checkbutton_edit_instempre.Active = true;
							break;
						}
					break;
					case "combobox_reportes":
						tipodereporte = (string) onComboBoxChanged.Model.GetValue(iter,0);
					break;
				}
			}
		}
		
		void on_checkbutton_clicked(object sender, EventArgs args)
		{
			CheckButton onCheckBoxChanged = sender as CheckButton;
			if(sender == null){return;}			
			switch (onCheckBoxChanged.Name.ToString()){
				case "checkbutton_nropaseqx":
					combobox_nropaseqx.Sensitive = onCheckBoxChanged.Active;
					activa_botton_graba_tabqx();
				break;
				case "checkbutton_lente":
					entry_id_producto.Sensitive = onCheckBoxChanged.Active;
					entry_descripcion_producto.Sensitive = onCheckBoxChanged.Active;
					button_busca_producto.Sensitive = onCheckBoxChanged.Active;
					entry_nroserie.Sensitive = onCheckBoxChanged.Active;
					activa_botton_graba_tabqx();
				break;
				case "checkbutton_tipo_anestesia":
					combobox_tipo_anestecia.Sensitive = onCheckBoxChanged.Active;
					activa_botton_graba_tabqx();
				break;
				case "checkbutton_anestesiologo":
					entry_id_anestesiologo.Sensitive = onCheckBoxChanged.Active;
					entry_anestesiologo.Sensitive = onCheckBoxChanged.Active;
					button_buscar_anestesiologo.Sensitive = onCheckBoxChanged.Active;
					activa_botton_graba_tabqx();
				break;
				case "checkbutton_tecnicovision":
					combobox_tecnicovision.Sensitive = onCheckBoxChanged.Active;
					// vision
					if((bool) checkbutton_tecnicovision.Active == true || (bool) checkbutton_maquinavision.Active == true){
						button_guardar_tabvision.Sensitive = true;
					}else{
						button_guardar_tabvision.Sensitive = false;
					}
				break;
				case "checkbutton_maquinavision":
					combobox_maquinavision.Sensitive = onCheckBoxChanged.Active;
					entry_nro_estudiomaquina.Sensitive = onCheckBoxChanged.Active;
					// vision
					if((bool) checkbutton_tecnicovision.Active == true || (bool) checkbutton_maquinavision.Active == true){
						button_guardar_tabvision.Sensitive = true;
					}else{
						button_guardar_tabvision.Sensitive = false;
					}
				break;
				case "checkbutton_observacion_qx":
					entry_observacion_qx.Sensitive = onCheckBoxChanged.Active;
					activa_botton_graba_tabqx();
				break;
				case "checkbutton_cirujano1":
					entry_id_cirujano1.Sensitive = onCheckBoxChanged.Active;
					entry_nombre_cirujano1.Sensitive = onCheckBoxChanged.Active;
					button_buscar_cirujano1.Sensitive = onCheckBoxChanged.Active;
					activa_botton_graba_tabqx();
				break;
				case "checkbutton_cirujano2":
					entry_id_cirujano2 .Sensitive = onCheckBoxChanged.Active;
					entry_nombre_cirujano2.Sensitive = onCheckBoxChanged.Active;
					button_buscar_cirujano2.Sensitive = onCheckBoxChanged.Active;
					activa_botton_graba_tabqx();
				break;
				case "checkbutton_cirugiapaseqx":
					entry_cirugia_paseqx.Sensitive = onCheckBoxChanged.Active;
					button_busca_cxpaseqx.Sensitive = onCheckBoxChanged.Active;
					activa_botton_graba_tabqx();
				break;
				case "checkbutton_edit_aseguradora":
					entry_edit_idaseguradora.Sensitive = onCheckBoxChanged.Active;
					entry_nombre_aseguradora.Sensitive = onCheckBoxChanged.Active;
					button_busca_aseguradora.Sensitive = onCheckBoxChanged.Active;
					activa_botton_grabar_edit(onCheckBoxChanged.Active);
				break;
				case "checkbutton_edit_instempre":
					entry_edit_idinstempr.Sensitive = onCheckBoxChanged.Active;
					entry_nombre_instempr.Sensitive = onCheckBoxChanged.Active;
					button_busca_instempr.Sensitive = onCheckBoxChanged.Active;
					activa_botton_grabar_edit(onCheckBoxChanged.Active);
				break;
				case "checkbutton_edit_tipopx":
					combobox_tipo_paciente.Sensitive = onCheckBoxChanged.Active;
					activa_botton_grabar_edit(onCheckBoxChanged.Active);
				break;
				case "checkbutton_edit_tipoadmin":
					combobox_tipo_admision.Sensitive = onCheckBoxChanged.Active;
					activa_botton_grabar_edit(onCheckBoxChanged.Active);
				break;
				case "checkbutton_fech_altamed":
					entry_ano_altamed.Sensitive = onCheckBoxChanged.Active;
					entry_mes_altamed.Sensitive = onCheckBoxChanged.Active;
					entry_dia_altamed.Sensitive = onCheckBoxChanged.Active;
					entry_hora_altamed.Sensitive = onCheckBoxChanged.Active;
					entry_minuto_altamed.Sensitive = onCheckBoxChanged.Active;
					activa_botton_grabar_edit(onCheckBoxChanged.Active);
				break;
				case "checkbutton_regadm":
					entry_ano_regadm.Sensitive = onCheckBoxChanged.Active;
					entry_mes_regadm.Sensitive = onCheckBoxChanged.Active;
					entry_dia_regadm.Sensitive = onCheckBoxChanged.Active;
					entry_hora_regadm.Sensitive = onCheckBoxChanged.Active;
					entry_minuto_regadm.Sensitive = onCheckBoxChanged.Active;
					activa_botton_grabar_edit(onCheckBoxChanged.Active);
				break;
				case "checkbutton_edit_admision":
						if((bool) checkbutton_edit_admision.Active == true){
							if((string) classpublic.lee_registro_de_tabla("osiris_empleado","acceso_edita_admision","WHERE acceso_edita_admision = 'true' AND login_empleado = '"+LoginEmpleado+"' ","acceso_edita_admision","bool") == "True"){
								checkbutton_edit_aseguradora.Sensitive = onCheckBoxChanged.Active;
								checkbutton_edit_instempre.Sensitive = onCheckBoxChanged.Active;
								checkbutton_edit_tipopx.Sensitive = onCheckBoxChanged.Active;
								checkbutton_edit_tipoadmin.Sensitive = onCheckBoxChanged.Active;
								checkbutton_fech_altamed.Sensitive = onCheckBoxChanged.Active;
								checkbutton_regadm.Sensitive = onCheckBoxChanged.Active;
						}else{
							MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,									
												MessageType.Error,ButtonsType.Close,"NO tiene permiso para poder EDITAR esta informacion");
												msgBoxError.Run ();							msgBoxError.Destroy();
							checkbutton_edit_admision.Active = false;
						}
						}else{
							checkbutton_edit_aseguradora.Sensitive = onCheckBoxChanged.Active;
							checkbutton_edit_instempre.Sensitive = onCheckBoxChanged.Active;
							checkbutton_edit_tipopx.Sensitive = onCheckBoxChanged.Active;
							checkbutton_edit_tipoadmin.Sensitive = onCheckBoxChanged.Active;
							checkbutton_fech_altamed.Sensitive = onCheckBoxChanged.Active;
							checkbutton_edit_aseguradora.Active = onCheckBoxChanged.Active;
							checkbutton_edit_instempre.Active = onCheckBoxChanged.Active;
							checkbutton_edit_tipopx.Active = onCheckBoxChanged.Active;
							checkbutton_edit_tipoadmin.Active = onCheckBoxChanged.Active;
							checkbutton_regadm.Active = onCheckBoxChanged.Active;
						}					
				break;
			}			
		}
		
		void activa_botton_graba_tabqx()
		{
			if((bool)checkbutton_lente.Active == true 
				   || (bool) checkbutton_nropaseqx.Active == true 
				   || (bool) checkbutton_tipo_anestesia.Active == true 
				   || (bool) checkbutton_anestesiologo.Active == true 
				   || (bool) checkbutton_observacion_qx.Active == true
				   || (bool) checkbutton_cirujano1.Active == true
				   || (bool) checkbutton_cirujano2.Active == true){
				button_guardar_tabqx.Sensitive = true;
			}else{
				button_guardar_tabqx.Sensitive = false;
			}
		}
		
		void activa_botton_grabar_edit(bool activa_los_checkbutton)
		{
			if((bool) checkbutton_edit_aseguradora.Active == true
			   		|| (bool) checkbutton_edit_instempre.Active == true
			   		|| (bool) checkbutton_edit_tipopx.Active == true
			   		|| (bool) checkbutton_edit_tipoadmin.Active == true
			   		|| (bool) checkbutton_fech_altamed.Active == true
					|| (bool) checkbutton_regadm.Active == true){
				button_graba_regentrada.Sensitive = true;
			}else{
				button_graba_regentrada.Sensitive = false;
			}
			if((bool) activa_los_checkbutton){
				cuenta_check_edicion += 1;
			}else{
				cuenta_check_edicion -= 1;
			}
		}
		
		void on_button_busca_instempr_clicked(object sender, EventArgs args)
		{
			// diferenciar el tipo de busqueda empresa o aseguradora
			//id_tipopaciente = 400 asegurados
			//id_tipopaciente = 102 empresas
			//id_tipopaciente = 500 municipio
			//id_tipopaciente = 100 DIF
			//id_tipopaciente = 600 Sindicato
			// Los parametros de del SQL siempre es primero cuando busca todo y la otra por expresion la clase recibe tambien el orden del query
			// es importante definir que tipo de busqueda es para que los objetos caigan ahi mismo
			if (id_tipopaciente != 400){
				object[] parametros_objetos = {entry_edit_idinstempr,entry_nombre_instempr,entry_edit_idaseguradora,entry_nombre_aseguradora};
				string[] parametros_sql = {"SELECT * FROM osiris_empresas WHERE id_tipo_paciente = '"+id_tipopaciente.ToString().Trim()+"' "};
				string[] parametros_string = {};
				string[,] args_buscador1 = {{"EMPRESA","AND descripcion_empresa LIKE '%","%' "},
											{"ID EMPRESA","AND id_empresa = '","' "}};
				string[,] args_buscador2 = {{"ID EMPRESA","AND id_empresa = '","' "},
											{"EMPRESA","AND descripcion_empresa LIKE '%","%' "}};
				string[,] args_orderby = {{"",""}};
				classfind_data.buscandor(parametros_objetos,parametros_sql,parametros_string,"find_instempres_edit",0,args_buscador1,args_buscador2,args_orderby);
				//idempresa = entry_id_empaseg_cita.Text.ToString().Trim();
				//idaseguradora = "1";		
			}
		}
		
		void on_button_busca_aseguradora_clicked(object sender, EventArgs args)
		{
			// diferenciar el tipo de busqueda empresa o aseguradora
			//id_tipopaciente = 400 asegurados
			//id_tipopaciente = 102 empresas
			//id_tipopaciente = 500 municipio
			//id_tipopaciente = 100 DIF
			//id_tipopaciente = 600 Sindicato
			// Los parametros de del SQL siempre es primero cuando busca todo y la otra por expresion la clase recibe tambien el orden del query
			// es importante definir que tipo de busqueda es para que los objetos caigan ahi mismo
			if (id_tipopaciente == 400){
				object[] parametros_objetos = {entry_edit_idaseguradora,entry_nombre_aseguradora,entry_edit_idinstempr,entry_nombre_instempr};
				string[] parametros_sql = {"SELECT * FROM osiris_aseguradoras WHERE activa = 'true' "};
				string[] parametros_string = {};
				string[,] args_buscador1 = {{"ASEGURADORA","AND descripcion_aseguradora LIKE '%","%' "},
											{"ID ASEGURADORA","AND id_aseguradora = '","' "}};
				string[,] args_buscador2 = {{"ID ASEGURADORA","AND id_aseguradora = '","' "},
											{"ASEGURADORA","AND descripcion_aseguradora LIKE '%","%' "}};
				string[,] args_orderby = {{"",""}};
				classfind_data.buscandor(parametros_objetos,parametros_sql,parametros_string,"find_aseguradora_edit",0,args_buscador1,args_buscador2,args_orderby);
				//idaseguradora = entry_id_empaseg_cita.Text.ToString().Trim();
				//idempresa = "1";	
			}
		}
				
		void on_combobox_tipo_anestecia_clicked(object sender, EventArgs args)
		{
				
		}
		
		void on_radio_clicked(object sender, EventArgs args)
		{
			verifica_radiobutton();	
		}
		
		void verifica_radiobutton()
		{
			if (checkbutton_camb_dats.Active == true){
				if (radiobutton_med.Active == true ){
				    button_busc_medic_diag.Sensitive = true;
					button_busc_medic_trat.Sensitive =false;
				    button_busc_diag.Sensitive = false;
					textview_motivo_ingreso.Sensitive = false;
					button_diag_final.Sensitive = false;
				    button_busc_cirugia.Sensitive = false;
					combobox_tipo_cirugia.Sensitive= false;
					combobox_diagprimeravez.Sensitive = false;
					entry_observaciones.Sensitive = false;
					entry_servicio_medico.Sensitive = false;
					entry_id_paquete_med.Sensitive = false;
					entry_paquete_medico.Sensitive = false;
					button_busca_paquete.Sensitive = false;
				}
				if (radiobutton_med_trat.Active == true){
					button_busc_medic_trat.Sensitive =true;
					button_busc_medic_diag.Sensitive = false;
					button_busc_diag.Sensitive = false;
					textview_motivo_ingreso.Sensitive = false;
					button_diag_final.Sensitive = false;
					button_busc_cirugia.Sensitive = false;
					combobox_tipo_cirugia.Sensitive= false;
					combobox_diagprimeravez.Sensitive = false;
					entry_observaciones.Sensitive = false;
					entry_servicio_medico.Sensitive = false;
					entry_id_paquete_med.Sensitive = false;
					entry_paquete_medico.Sensitive = false;
					button_busca_paquete.Sensitive = false;
				}
				if (radiobutton_motingreso.Active == true){
					textview_motivo_ingreso.Sensitive = true;
					button_busc_diag.Sensitive = false;
					button_diag_final.Sensitive = false;
					button_busc_cirugia.Sensitive = false;
					button_busc_medic_diag.Sensitive = false;
					button_busc_medic_trat.Sensitive =false;
					combobox_tipo_cirugia.Sensitive= false;
					combobox_diagprimeravez.Sensitive = false;
					entry_observaciones.Sensitive = false;
					entry_servicio_medico.Sensitive = false;
					entry_id_paquete_med.Sensitive = false;
					entry_paquete_medico.Sensitive = false;
					button_busca_paquete.Sensitive = false;
				}					    
				if (radiobutton_cirugia.Active == true){
					button_busc_cirugia.Sensitive = true;
					button_busc_medic_trat.Sensitive =false;
					button_busc_medic_diag.Sensitive = false;
					button_busc_diag.Sensitive = false;
					textview_motivo_ingreso.Sensitive = false;
					button_diag_final.Sensitive = false;
					combobox_tipo_cirugia.Sensitive= false;
					combobox_diagprimeravez.Sensitive = false;
					entry_observaciones.Sensitive = false;
					entry_servicio_medico.Sensitive = false;
					entry_id_paquete_med.Sensitive = false;
					entry_paquete_medico.Sensitive = false;
					button_busca_paquete.Sensitive = false;
				}
				if (radiobutton_diag_cie10.Active == true){
					button_busc_diag.Sensitive = true;
					button_diag_final.Sensitive = true;
					button_busc_medic_trat.Sensitive =false;
					button_busc_medic_diag.Sensitive = false;
					textview_motivo_ingreso.Sensitive = false;
					button_diag_final.Sensitive = false;
					button_busc_cirugia.Sensitive = false;
					combobox_tipo_cirugia.Sensitive= false;
					combobox_diagprimeravez.Sensitive = false;
					entry_observaciones.Sensitive = false;
					entry_servicio_medico.Sensitive = false;
					entry_id_paquete_med.Sensitive = false;
					entry_paquete_medico.Sensitive = false;
					button_busca_paquete.Sensitive = false;
				}
	            if (radiobutton_diag_final.Active == true){
					button_busc_medic_trat.Sensitive = false;
					button_diag_final.Sensitive = true;
					button_busc_medic_diag.Sensitive = false;
					button_busc_diag.Sensitive = false;
					textview_motivo_ingreso.Sensitive = false;
					button_busc_cirugia.Sensitive = false;
					button_busc_diag.Sensitive = false;
					combobox_tipo_cirugia.Sensitive= false;
					combobox_diagprimeravez.Sensitive = false;
					entry_observaciones.Sensitive = false;
					entry_servicio_medico.Sensitive = false;
					entry_id_paquete_med.Sensitive = false;
					entry_paquete_medico.Sensitive = false;
					button_busca_paquete.Sensitive = false;
				}
				if(radiobutton_tipo_cirugia.Active == true){
					button_busc_medic_trat.Sensitive = false;
					button_diag_final.Sensitive = false;
					button_busc_medic_diag.Sensitive = false;
					button_busc_diag.Sensitive = false;
					textview_motivo_ingreso.Sensitive = false;
					button_busc_cirugia.Sensitive = false;
					combobox_tipo_cirugia.Sensitive= true;
					combobox_diagprimeravez.Sensitive = false;
					entry_observaciones.Sensitive = false;
					entry_servicio_medico.Sensitive = false;
					entry_id_paquete_med.Sensitive = false;
					entry_paquete_medico.Sensitive = false;
					button_busca_paquete.Sensitive = false;
				}
				if(radiobutton_diag_primeravez.Active == true){
					combobox_diagprimeravez.Sensitive = true;
					button_busc_cirugia.Sensitive = false;
					button_busc_medic_trat.Sensitive =false;
					button_busc_medic_diag.Sensitive = false;
					textview_motivo_ingreso.Sensitive = false;
					button_busc_diag.Sensitive = false;
					button_diag_final.Sensitive = false;
					combobox_tipo_cirugia.Sensitive= false;
					entry_observaciones.Sensitive = false;
					entry_servicio_medico.Sensitive = false;
					entry_id_paquete_med.Sensitive = false;
					entry_paquete_medico.Sensitive = false;
					button_busca_paquete.Sensitive = false;
				}
				if(radiobutton_observacion.Active == true){
					combobox_diagprimeravez.Sensitive = false;
					button_busc_cirugia.Sensitive = false;
					button_busc_medic_trat.Sensitive =false;
					button_busc_medic_diag.Sensitive = false;
					textview_motivo_ingreso.Sensitive = false;
					button_busc_diag.Sensitive = false;
					button_diag_final.Sensitive = false;
					combobox_tipo_cirugia.Sensitive= false;
					entry_observaciones.Sensitive = true;
					entry_servicio_medico.Sensitive = false;
					entry_id_paquete_med.Sensitive = false;
					entry_paquete_medico.Sensitive = false;
					button_busca_paquete.Sensitive = false;
				}
				if(radiobutton_servmedico.Active == true){
					combobox_diagprimeravez.Sensitive = false;
					button_busc_cirugia.Sensitive = false;
					button_busc_medic_trat.Sensitive =false;
					button_busc_medic_diag.Sensitive = false;
					textview_motivo_ingreso.Sensitive = false;
					button_busc_diag.Sensitive = false;
					button_diag_final.Sensitive = false;
					combobox_tipo_cirugia.Sensitive= false;
					entry_observaciones.Sensitive = false;
					entry_servicio_medico.Sensitive = true;
					entry_id_paquete_med.Sensitive = false;
					entry_paquete_medico.Sensitive = false;
					button_busca_paquete.Sensitive = false;
				}
				if(radiobutton_pq_medico.Active == true){
					combobox_diagprimeravez.Sensitive = false;
					button_busc_cirugia.Sensitive = false;
					button_busc_medic_trat.Sensitive =false;
					button_busc_medic_diag.Sensitive = false;
					textview_motivo_ingreso.Sensitive = false;
					button_busc_diag.Sensitive = false;
					button_diag_final.Sensitive = false;
					combobox_tipo_cirugia.Sensitive= false;
					entry_observaciones.Sensitive = false;
					entry_servicio_medico.Sensitive = false;
					entry_id_paquete_med.Sensitive = true;
					entry_paquete_medico.Sensitive = true;
					button_busca_paquete.Sensitive = true;					
				}
			}
		}
		
		void on_checkbutton_camb_dats_clicked(object sender, EventArgs args)
		{   
		  	this.button_guardar.Sensitive = true;
		  	if (checkbutton_camb_dats.Active == true ){
				radiobutton_med.Sensitive= true;
				radiobutton_med_trat.Sensitive= true;
				radiobutton_motingreso.Sensitive = true;
				radiobutton_diag_cie10.Sensitive= true;
		        radiobutton_diag_final.Sensitive= true;
				radiobutton_cirugia.Sensitive= true;
				radiobutton_tipo_cirugia.Sensitive= true;
				radiobutton_observacion.Sensitive= true;
				radiobutton_servmedico.Sensitive = true;
				radiobutton_diag_primeravez.Sensitive = true;
				radiobutton_pq_medico.Sensitive = true;
				button_busc_medic_diag.Sensitive = true;
				button_guardar.Sensitive = true;
				combobox_tipo_cirugia.Sensitive = true;
				combobox_diagprimeravez.Sensitive = true;
						
		      }else{	       
		      	radiobutton_med.Sensitive= false;
				radiobutton_med_trat.Sensitive= false;
				radiobutton_motingreso.Sensitive= false;
				radiobutton_diag_cie10.Sensitive = false;
		        radiobutton_diag_final.Sensitive = false;
				radiobutton_cirugia.Sensitive= false;
				radiobutton_tipo_cirugia.Sensitive= false;
				radiobutton_observacion.Sensitive= false;
				radiobutton_servmedico.Sensitive = false;
				radiobutton_diag_primeravez.Sensitive = false;
				radiobutton_pq_medico.Sensitive = false;
				button_busc_medic_diag.Sensitive = false;
				button_busc_medic_trat.Sensitive = false;
				button_busc_diag.Sensitive = false;
				button_diag_final.Sensitive = false;
				button_busc_cirugia.Sensitive = false;
				button_guardar.Sensitive = false;
				combobox_tipo_cirugia.Sensitive = false;
				combobox_diagprimeravez.Sensitive = false;
			}
			verifica_radiobutton();
		}
		
		// Ventana de Busqueda de Medico
		void on_button_busca_medicos_clicked (object sender, EventArgs args)
		{
			Glade.XML gxml = new Glade.XML (null, "catalogos.glade", "buscador_medicos", null);
			gxml.Autoconnect                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                 (this);
			buscador_medicos.Show();
			crea_treeview_busqueda("medicos","");
			llenado_combobox(0,"",combobox_tipo_busqueda,"array","","","",args_tipobusqueda,args_id_array,"");
			entry_expresion.KeyPressEvent += onKeyPressEvent_enter;
			button_buscar_busqueda.Clicked += new EventHandler(on_button_llena_medicos_clicked);
			button_selecciona.Clicked += new EventHandler(on_selecciona_medico_clicked);
	        button_salir.Clicked +=  new EventHandler(on_cierraventanas_clicked);
		}
		
		void on_selecciona_medico_clicked (object sender, EventArgs args)
		{
			TreeModel model;
			TreeIter iterSelected;
			if (lista_de_medicos.Selection.GetSelected(out model, out iterSelected)){
 				idmedico = (int) model.GetValue(iterSelected, 0);
 				idmedicotratante = (int) model.GetValue(iterSelected, 0);
 				nombmedico = (string) model.GetValue(iterSelected, 1)+" "+(string) model.GetValue(iterSelected, 2)+" "+
							(string) model.GetValue(iterSelected, 3)+" "+(string) model.GetValue(iterSelected,4);
				if (radiobutton_med.Active == true ){
					entry_medic_diag.Text = nombmedico;
				}
				if (radiobutton_med_trat.Active == true){
					entry_med_trat.Text = nombmedico;
				}
				Widget win = (Widget) sender;
				win.Toplevel.Destroy();
			}
		}
		
		void on_button_buscar_anestesiologo_clicked(object sender, EventArgs args)
		{
			Glade.XML gxml = new Glade.XML (null, "catalogos.glade", "buscador_medicos", null);
			gxml.Autoconnect                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                 (this);
			buscador_medicos.Show();
			crea_treeview_busqueda("medicos","anestesiologo");
			llenado_combobox(0,"",combobox_tipo_busqueda,"array","","","",args_tipobusqueda,args_id_array,"");
			entry_expresion.KeyPressEvent += onKeyPressEvent_enter;
			button_buscar_busqueda.Clicked += new EventHandler(on_button_llena_medicos_clicked);
			button_selecciona.Clicked += new EventHandler(on_selecciona_anestesiologo_clicked);
	        button_salir.Clicked +=  new EventHandler(on_cierraventanas_clicked);
		}
		
		void on_selecciona_anestesiologo_clicked(object sender, EventArgs args)
		{
			TreeModel model;
			TreeIter iterSelected;
			if (lista_de_medicos.Selection.GetSelected(out model, out iterSelected)){
				entry_id_anestesiologo.Text = model.GetValue(iterSelected, 0).ToString().Trim();
				entry_anestesiologo.Text = (string) model.GetValue(iterSelected, 1)+" "+(string) model.GetValue(iterSelected, 2)+" "+
							(string) model.GetValue(iterSelected, 3)+" "+(string) model.GetValue(iterSelected,4);
			}
			Widget win = (Widget) sender;
			win.Toplevel.Destroy();
		}
		
		void on_button_buscar_cirujano1_clicked(object sender, EventArgs args)
		{
			Glade.XML gxml = new Glade.XML (null, "catalogos.glade", "buscador_medicos", null);
			gxml.Autoconnect                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                 (this);
			buscador_medicos.Show();
			crea_treeview_busqueda("medicos","cirujano1");
			llenado_combobox(0,"",combobox_tipo_busqueda,"array","","","",args_tipobusqueda,args_id_array,"");
			entry_expresion.KeyPressEvent += onKeyPressEvent_enter;
			button_buscar_busqueda.Clicked += new EventHandler(on_button_llena_medicos_clicked);
			button_selecciona.Clicked += new EventHandler(on_selecciona_cirujano1_clicked);
	        button_salir.Clicked +=  new EventHandler(on_cierraventanas_clicked);
		}
		
		void on_selecciona_cirujano1_clicked(object sender, EventArgs args)
		{
			TreeModel model;
			TreeIter iterSelected;
			if (lista_de_medicos.Selection.GetSelected(out model, out iterSelected)){
				entry_id_cirujano1.Text = model.GetValue(iterSelected, 0).ToString().Trim();
				entry_nombre_cirujano1.Text = (string) model.GetValue(iterSelected, 1)+" "+(string) model.GetValue(iterSelected, 2)+" "+
							(string) model.GetValue(iterSelected, 3)+" "+(string) model.GetValue(iterSelected,4);
			}
			Widget win = (Widget) sender;
			win.Toplevel.Destroy();
		}
		
		void on_button_buscar_cirujano2_clicked(object sender, EventArgs args)
		{
			Glade.XML gxml = new Glade.XML (null, "catalogos.glade", "buscador_medicos", null);
			gxml.Autoconnect                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                 (this);
			buscador_medicos.Show();
			crea_treeview_busqueda("medicos","cirujano2");
			llenado_combobox(0,"",combobox_tipo_busqueda,"array","","","",args_tipobusqueda,args_id_array,"");
			entry_expresion.KeyPressEvent += onKeyPressEvent_enter;
			button_buscar_busqueda.Clicked += new EventHandler(on_button_llena_medicos_clicked);
			button_selecciona.Clicked += new EventHandler(on_selecciona_cirujano2_clicked);
	        button_salir.Clicked +=  new EventHandler(on_cierraventanas_clicked);
		}
		
		void on_selecciona_cirujano2_clicked(object sender, EventArgs args)
		{
			TreeModel model;
			TreeIter iterSelected;
			if (lista_de_medicos.Selection.GetSelected(out model, out iterSelected)){
				entry_id_cirujano2.Text = model.GetValue(iterSelected, 0).ToString().Trim();
				entry_nombre_cirujano2.Text = (string) model.GetValue(iterSelected, 1)+" "+(string) model.GetValue(iterSelected, 2)+" "+
							(string) model.GetValue(iterSelected, 3)+" "+(string) model.GetValue(iterSelected,4);
			}
			Widget win = (Widget) sender;
			win.Toplevel.Destroy();
		}
		
		void on_button_llena_medicos_clicked (object sender, EventArgs args)
		{
			llenando_lista_de_medicos();
		}
        
        void llenando_lista_de_medicos() 
		{
			TreeIter iter;
			if (combobox_tipo_busqueda.GetActiveIter(out iter)){
				if((int) combobox_tipo_busqueda.Model.GetValue(iter,1) > 0) {
					treeViewEngineMedicos.Clear(); // Limpia el treeview cuando realiza una nueva busqueda
					NpgsqlConnection conexion; 
					conexion = new NpgsqlConnection (connectionString+nombrebd);
		            // Verifica que la base de datos este conectada
					try{
						conexion.Open ();
						NpgsqlCommand comando; 
						comando = conexion.CreateCommand ();
						if ((string) entry_expresion.Text.ToUpper().Trim() == ""){
							comando.CommandText = "SELECT id_medico, "+
										"to_char(id_empresa,'999999') AS idempresa, "+
										"to_char(osiris_his_tipo_especialidad.id_especialidad,'999999') AS idespecialidad, "+
										"nombre_medico,descripcion_empresa,descripcion_especialidad,centro_medico, "+
										"nombre1_medico,nombre2_medico,apellido_paterno_medico,apellido_materno_medico, "+
										"telefono1_medico,cedula_medico,telefono2_medico,celular1_medico,celular2_medico, "+
										"nextel_medico,beeper_medico,cedula_medico,direccion_medico,direccion_consultorio_medico, "+
										"to_char(fecha_ingreso_medico,'dd-mm-yyyy') AS fecha_ingreso, "+
										"to_char(fecha_revision_medico,'dd-mm-yyyy') AS fecha_revision, "+
										"titulo_profesional_medico,cedula_profecional_medico,diploma_especialidad_medico, "+
										"diploma_subespecialidad_medico,copia_identificacion_oficial_medico,copia_cedula_rfc_medico, "+
										"diploma_cursos_adiestramiento_medico,certificacion_recertificacion_consejo_subespecialidad_medico, "+
										"copia_comprobante_domicilio_medico,diploma_seminarios_medico,diploma_cursos_medico, "+
										"diplomas_extranjeros_medico,constancia_congresos_medico,cedula_especialidad_medico, "+
										"medico_activo,autorizado "+
										"FROM osiris_his_medicos,osiris_his_tipo_especialidad,osiris_empresas "+
										"WHERE osiris_his_medicos.id_especialidad = osiris_his_tipo_especialidad.id_especialidad "+
										"AND osiris_his_medicos.id_empresa_medico = osiris_empresas.id_empresa "+
										"AND medico_activo = 'true' "+
										"ORDER BY id_medico;";
						}else{
							comando.CommandText = "SELECT id_medico, "+
										"to_char(id_empresa,'999999') AS idempresa, "+
										"to_char(osiris_his_tipo_especialidad.id_especialidad,'999999') AS idespecialidad, "+
										"nombre_medico,descripcion_empresa,descripcion_especialidad,centro_medico, "+
										"nombre1_medico,nombre2_medico,apellido_paterno_medico,apellido_materno_medico, "+
										"telefono1_medico,cedula_medico,telefono2_medico,celular1_medico,celular2_medico, "+
										"nextel_medico,beeper_medico,cedula_medico,direccion_medico,direccion_consultorio_medico, "+
										"to_char(fecha_ingreso_medico,'dd-mm-yyyy') AS fecha_ingreso, "+
										"to_char(fecha_revision_medico,'dd-mm-yyyy') AS fecha_revision, "+
										"titulo_profesional_medico,cedula_profecional_medico,diploma_especialidad_medico, "+
										"diploma_subespecialidad_medico,copia_identificacion_oficial_medico,copia_cedula_rfc_medico, "+
										"diploma_cursos_adiestramiento_medico,certificacion_recertificacion_consejo_subespecialidad_medico, "+
										"copia_comprobante_domicilio_medico,diploma_seminarios_medico,diploma_cursos_medico, "+
										"diplomas_extranjeros_medico,constancia_congresos_medico,cedula_especialidad_medico, "+
										"medico_activo,autorizado "+
										"FROM osiris_his_medicos,osiris_his_tipo_especialidad,osiris_empresas "+
										"WHERE osiris_his_medicos.id_especialidad = osiris_his_tipo_especialidad.id_especialidad "+
										"AND osiris_his_medicos.id_empresa_medico = osiris_empresas.id_empresa "+
										"AND medico_activo = 'true' "+
								  		tipobusqueda+(string) entry_expresion.Text.Trim().ToUpper()+"%' "+
										"ORDER BY id_medico;";
						}
						NpgsqlDataReader lector = comando.ExecuteReader ();
						//Console.WriteLine(comando.CommandText);
						while (lector.Read()){
							treeViewEngineMedicos.AppendValues ((int) lector["id_medico"],//0
										(string) lector["nombre1_medico"],//1
										(string) lector["nombre2_medico"],//2
										(string) lector["apellido_paterno_medico"],//3
										(string) lector["apellido_materno_medico"],//4
										(string) lector["descripcion_especialidad"],//5
										(string) lector["cedula_medico"],//6
										(string) lector["telefono1_medico"],//7
										(string) lector["telefono2_medico"],//8
										(string) lector["celular1_medico"],//9
										(string) lector["celular2_medico"],//10
										(string) lector["nextel_medico"],//11
										(string) lector["beeper_medico"],//12
										(string) lector["descripcion_empresa"],//13
										(string) lector["idespecialidad"],//14
										(string) lector["idempresa"],//15
										(string) lector["fecha_ingreso"],//16
										(string) lector["fecha_revision"],//17
										(string) lector["direccion_medico"],//18
										(string) lector["direccion_consultorio_medico"],//19
										(bool) lector["titulo_profesional_medico"],//20
										(bool) lector["cedula_profecional_medico"],//21
										(bool) lector["diploma_especialidad_medico"], //22
										(bool) lector["diploma_subespecialidad_medico"],//23
										(bool) lector["copia_identificacion_oficial_medico"],//24
										(bool) lector["copia_cedula_rfc_medico"], //25
										(bool) lector["diploma_cursos_adiestramiento_medico"],//26
										(bool) lector["certificacion_recertificacion_consejo_subespecialidad_medico"],//27
										(bool) lector["copia_comprobante_domicilio_medico"],//28
										(bool) lector["diploma_seminarios_medico"],//29
										(bool) lector["diploma_cursos_medico"],//30
										(bool) lector["diplomas_extranjeros_medico"],//31
										(bool) lector["constancia_congresos_medico"],//32
										(bool) lector["cedula_especialidad_medico"],//33
										(bool) lector["medico_activo"],//34
										(bool) lector["centro_medico"],//35
										(bool) lector["autorizado"]//36
										);
						}
					}catch (NpgsqlException ex){
			   			Console.WriteLine ("PostgresSQL error: {0}",ex.Message);
			   			MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
						MessageType.Error,ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
						msgBoxError.Run ();			msgBoxError.Destroy();
					}
					conexion.Close ();
				}else{	
					MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
					MessageType.Info,ButtonsType.Close, " selecione un tipo de busqueda ");
					msgBoxError.Run ();			msgBoxError.Destroy();
				}
			}
		}
		
		// Activa en enter en la busqueda de los productos
		[GLib.ConnectBefore ()]   	  // Esto es indispensable para que funcione    
		public void onKeyPressEvent_enter(object o, Gtk.KeyPressEventArgs args)
		{
			//Console.WriteLine(args.Event.Key);
			if (args.Event.Key.ToString() == "Return" || args.Event.Key.ToString() == "KP_Enter"){
				args.RetVal = true;
				llenando_lista_de_medicos();
				//if(busqueda == "cirugia") {llenando_lista_de_cirugias();}				
			}
		}
		///////////////Busqueda de diagnostico CIE-10//////////////////////////////	      
		void on_button_busc_diag_clicked (object sender, EventArgs args)
		{
			Glade.XML gxml = new Glade.XML (null, "costos.glade", "busca_producto", null);
			gxml.Autoconnect (this);
			crea_treeview_busqueda_diag();
			button_buscar_busqueda.Clicked += new EventHandler(on_llena_lista_diag_clicked);
			entry_expresion.KeyPressEvent += onKeyPressEvent_enterbucar_busqueda;
			button_selecciona.Clicked += new EventHandler(on_selecciona_diag_clicked);
			label33.LabelProp = "Busqueda de Diagnostico";
			label34.LabelProp = "Diagnostico a Buscar";
		
								
			button_salir.Clicked += new EventHandler(on_cierraventanas_clicked); // esta sub-clase esta la final de la classe*/
		}
		
		void on_llena_lista_diag_clicked (object sender, EventArgs args)
		{
			if(this.entry_expresion.Text.Trim() != ""){
				llenando_lista_de_productos();
			}else{
				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
							MessageType.Info,ButtonsType.Close, " Favor de escribir el Diagnostico a Buscar ");
							msgBoxError.Run ();			msgBoxError.Destroy();
			}
		}
		
		void llenando_lista_de_productos()			
		{   
			treeViewEngineBusca2.Clear(); // Limpia el treeview cuando realiza una nueva busqueda
			NpgsqlConnection conexion; 
			conexion = new NpgsqlConnection (connectionString+nombrebd);
			//Verifica que la base de datos este conectada
			string query_tipo_busqueda = "";			
			if(radiobutton_nombre.Active == true){
				query_tipo_busqueda = "AND osiris_his_tipo_diagnosticos.descripcion_diagnostico LIKE '%"+entry_expresion.Text.ToUpper().Trim()+"%' ORDER BY descripcion_diagnostico; ";
			}			
			if(radiobutton_codigo.Active == true){
				query_tipo_busqueda = "AND osiris_his_tipo_diagnosticos.id_diagnostico LIKE '"+entry_expresion.Text.Trim()+"%'  ORDER BY id_diagnostico; ";
			}	           
			try{
				conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand ();
				comando.CommandText = "SELECT to_char(osiris_his_tipo_diagnosticos.id_diagnostico,'999999999999') AS iddiagnostico,"+
					                  "osiris_his_tipo_diagnosticos.descripcion_diagnostico,"+
                                      "osiris_his_tipo_diagnosticos.id_cie_10,"+
                                      "to_char(osiris_his_tipo_diagnosticos.id_cie_10_grupo,'999999999999') AS idcie10grupo,"+
                                      "osiris_his_tipo_diagnosticos.sub_grupo "+
                                      "FROM osiris_his_tipo_diagnosticos "+
                                      "WHERE osiris_his_tipo_diagnosticos.sub_grupo = 'false' "+
                                       query_tipo_busqueda;
				//Console.WriteLine(comando.CommandText);
				NpgsqlDataReader lector = comando.ExecuteReader ();
				while (lector.Read()){
				        treeViewEngineBusca2.AppendValues (
											(string) lector["iddiagnostico"],
											(string) lector["descripcion_diagnostico"],
				                            (string) lector["id_cie_10"],
											(string) lector["idcie10grupo"]);
											//(string) lector["sub_grupo"]);
											
				}
			}catch (NpgsqlException ex){
		   		MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
											MessageType.Error, 
									ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
				msgBoxError.Run ();		msgBoxError.Destroy();
			}
			conexion.Close();                                      
       }
		
	 	[GLib.ConnectBefore ()] 
		public void onKeyPressEvent_enterbucar_busqueda(object o, Gtk.KeyPressEventArgs args)
		{
			if (args.Event.Key.ToString() == "Return" || args.Event.Key.ToString() == "KP_Enter"){
				args.RetVal = true;
				llenando_lista_de_productos();
				//Console.WriteLine ("key press");
								
			}
		}
		
		void crea_treeview_busqueda_diag()
		{
			treeViewEngineBusca2 = new TreeStore(typeof(string),
				                                         typeof(string),
														 typeof(string),
														 //typeof(string),
														 typeof(string));
				                                     
            lista_de_producto.Model = treeViewEngineBusca2;
			
			lista_de_producto.RulesHint = true;				
			lista_de_producto.RowActivated += on_selecciona_diag_clicked;  // Doble click selecciono paciente				
			TreeViewColumn col_iddiagnostico = new TreeViewColumn();
			CellRendererText cellr0 = new CellRendererText();
			col_iddiagnostico.Title = "ID Diagnostico"; // titulo de la cabecera de la columna, si está visible
			col_iddiagnostico.PackStart(cellr0, true);
			col_iddiagnostico.AddAttribute (cellr0, "text", 0);    // la siguiente columna será 1 en vez de 1
			col_iddiagnostico.SortColumnId = (int) Column_prod.col_iddiagnostico;
		
			TreeViewColumn col_desc_diagnostico = new TreeViewColumn();
			CellRendererText cellr1 = new CellRendererText();
			col_desc_diagnostico.Title = "Descripcion Diagnostico"; // titulo de la cabecera de la columna, si está visible
			col_desc_diagnostico.PackStart(cellr1, true);
			col_desc_diagnostico.AddAttribute (cellr1, "text", 1);    // la siguiente columna será 1 en vez de 1
			col_desc_diagnostico.SortColumnId = (int) Column_prod.col_desc_diagnostico;
			//cellr0.Editable = true;   // Permite edita este campo
       
			TreeViewColumn col_id_cie_10 = new TreeViewColumn();
			CellRendererText cellrt2 = new CellRendererText();
			col_id_cie_10.Title = "ID CIE-10";
			col_id_cie_10.PackStart(cellrt2, true);
			col_id_cie_10.AddAttribute (cellrt2, "text", 2); // la siguiente columna será 1 en vez de 2
			col_id_cie_10.SortColumnId = (int) Column_prod.col_id_cie_10;
       
			TreeViewColumn col_id_cie_10_grupo = new TreeViewColumn();
			CellRendererText cellrt3 = new CellRendererText();
			col_id_cie_10_grupo.Title = "ID CIE-10 Grupo";
			col_id_cie_10_grupo.PackStart(cellrt3, true);
			col_id_cie_10_grupo.AddAttribute (cellrt3, "text", 3); // la siguiente columna será 2 en vez de 3
			col_id_cie_10_grupo.SortColumnId = (int) Column_prod.col_id_cie_10_grupo;
       
			/*TreeViewColumn col_sub_grupo = new TreeViewColumn();
			CellRendererText cellrt4 = new CellRendererText();
			col_sub_grupo.Title = "Sub Grupo";
			col_sub_grupo.PackStart(cellrt4, true);
			col_sub_grupo.AddAttribute (cellrt4, "text", 4); // la siguiente columna será 3 en vez de 4
			col_sub_grupo.SortColumnId = (int) Column_prod.col_sub_grupo;*/
		
			lista_de_producto.AppendColumn(col_iddiagnostico);
			lista_de_producto.AppendColumn(col_desc_diagnostico);
			lista_de_producto.AppendColumn(col_id_cie_10);
			lista_de_producto.AppendColumn(col_id_cie_10_grupo);
			//lista_de_producto.AppendColumn(col_sub_grupo);
			
		}
		//lista de diagnostico cie-10
		enum Column_prod
		{
			col_iddiagnostico,
			col_desc_diagnostico,
			col_id_cie_10,
			col_id_cie_10_grupo
			//col_sub_grupo
		}
				
		void on_selecciona_diag_clicked (object sender, EventArgs args)
		{
			TreeModel model;
			TreeIter iterSelected;
			if (lista_de_producto.Selection.GetSelected(out model, out iterSelected)){
				if(radiobutton_diag_cie10.Active == true){
					entry_diag_cie10.Text = (string) model.GetValue(iterSelected, 1);
		 			iddiagnosticocie10 = int.Parse((string) model.GetValue(iterSelected, 0));
				}				
				if(radiobutton_diag_final.Active == true){
					entry_diag_final.Text = (string) model.GetValue(iterSelected, 1);
					iddiagnosticofinal = int.Parse((string) model.GetValue(iterSelected, 0));
				}
				if(radiobutton_pq_medico.Active == true){
					entry_paquete_medico.Text = (string) model.GetValue(iterSelected, 1);
				}
			}else{
 				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
							MessageType.Error, 
							ButtonsType.Close, "NO existen diagnosticos para seleccionar");
				msgBoxError.Run ();
				msgBoxError.Destroy();
			}
			Widget win =(Widget) sender;
			win.Toplevel.Destroy();
		}
		
		///////////////Busqueda de diagnostico Final//////////////////////////////
		void on_button_busc_diag_final_clicked (object sender, EventArgs args)
		{
			Glade.XML gxml = new Glade.XML (null, "costos.glade", "busca_producto", null);
			gxml.Autoconnect (this);
			crea_treeview_busqueda_diag();
			button_buscar_busqueda.Clicked += new EventHandler(on_llena_lista_diag_clicked);
			entry_expresion.KeyPressEvent += onKeyPressEvent_enterbucar_busqueda;
			button_selecciona.Clicked += new EventHandler(on_selecciona_diag_clicked);
			button_salir.Clicked += new EventHandler(on_cierraventanas_clicked); // esta sub-clase esta la final de la classe*/
		}
		
		void on_button_busc_cirugia_clicked (object sender, EventArgs args)
		{
			Glade.XML gxml = new Glade.XML (null, "registro_admision.glade", "busca_cirugias", null);
			gxml.Autoconnect (this);
	       	busca_cirugias.Show();
	       	crea_treeview_busqueda("cirugia","");
	       	button_salir.Clicked += new EventHandler(on_cierraventanas_clicked); // esta sub-clase esta en
			button_llena_cirugias.Clicked += new EventHandler(on_button_llena_cirugias_clicked);
		    button_selecciona.Clicked += new EventHandler(on_selecciona_cirugia_clicked);
		    entry_expresion.KeyPressEvent += onKeyPressEvent_enterbucar_cirugia;
		    button_busc_medic_diag.Sensitive = false;
		    button_busc_medic_trat.Sensitive = false;
		}
		
		void on_selecciona_cirugia_clicked (object sender, EventArgs args)
		{
			TreeModel model;
			TreeIter iterSelected;
			if (lista_cirugia.Selection.GetSelected(out model, out iterSelected)){
				//Console.WriteLine((string) model.GetValue(iterSelected, 1));
				if(radiobutton_cirugia.Active == true){
					entry_docimp_cirugia.Text = (string) model.GetValue(iterSelected, 1);
					idcirugianumero = (int) model.GetValue(iterSelected, 0);
					button_busc_medic_diag.Sensitive = true;
			    	button_busc_medic_trat.Sensitive = true;
				}
				if(radiobutton_pq_medico.Active == true){
					entry_id_paquete_med.Text = Convert.ToString((int) model.GetValue(iterSelected, 0)).Trim();
					entry_paquete_medico.Text = (string) model.GetValue(iterSelected, 1);
				}
				busca_cirugias.Destroy();
			}
		}
		
		void on_button_busca_cxpaseqx_clicked (object sender, EventArgs args)
		{
			Glade.XML gxml = new Glade.XML (null, "registro_admision.glade", "busca_cirugias", null);
			gxml.Autoconnect (this);
	       	busca_cirugias.Show();
	       	crea_treeview_busqueda("cirugia","paseqx");
	       	button_salir.Clicked += new EventHandler(on_cierraventanas_clicked);
			button_llena_cirugias.Clicked += new EventHandler(on_button_llena_cirugias_clicked);
		    button_selecciona.Clicked += new EventHandler(on_selecciona_cxpaseqx_clicked);
		    entry_expresion.KeyPressEvent += onKeyPressEvent_enterbucar_cxpaseqx;		    
		}
		
		void on_selecciona_cxpaseqx_clicked (object sender, EventArgs args)
		{
			TreeModel model;
			TreeIter iterSelected;
			if (lista_cirugia.Selection.GetSelected(out model, out iterSelected)) {
				//Console.WriteLine((string) model.GetValue(iterSelected, 1));
				entry_cirugia_paseqx.Text = (string) model.GetValue(iterSelected, 1);
				idcxpaseqx = (int) model.GetValue(iterSelected, 0);
				busca_cirugias.Destroy();
			}
		}
		
		
		void on_button_guardar_clicked (object sender, EventArgs args)
		{
			MessageDialog msgBox = new MessageDialog (MyWin,DialogFlags.Modal,
									MessageType.Question,ButtonsType.YesNo,"¿ Esta seguro de actualizar la informacion... ");
			ResponseType miResultado = (ResponseType)msgBox.Run ();
			msgBox.Destroy();
		 	if (miResultado == ResponseType.Yes){
				NpgsqlConnection conexion; 
	            conexion = new NpgsqlConnection (connectionString+nombrebd);
				// Verifica que la base de datos este conectada
				try{
					conexion.Open ();
					NpgsqlCommand comando; 
					comando = conexion.CreateCommand ();
					//idmedico
					string strsql = "";
					if (radiobutton_med.Active == true ){
						strsql = "UPDATE osiris_erp_cobros_enca SET id_medico = '" + idmedico+"', "+
												"nombre_medico_encabezado ='" +entry_medic_diag.Text.Trim()+"' "+
												"WHERE folio_de_servicio =  '"+entry_folio_servicio.Text+"';";
					}
					if (radiobutton_med_trat.Active == true){
						strsql = "UPDATE osiris_erp_cobros_enca SET id_medico_tratante = '" + idmedico+"', "+
												"nombre_medico_tratante ='" +entry_med_trat.Text.Trim()+"' "+
												"WHERE folio_de_servicio =  '"+entry_folio_servicio.Text+"';";
					}
					if (radiobutton_motingreso.Active == true){
						strsql = "UPDATE osiris_erp_movcargos SET "+
												"descripcion_diagnostico_movcargos ='" +textview_motivo_ingreso.Buffer.Text.ToString().ToUpper()+"' "+
												"WHERE folio_de_servicio =  '"+entry_folio_servicio.Text+"';";
						string nuevo_motivoingreso = textview_motivo_ingreso.Buffer.Text.ToString().ToUpper();
						buffer.Clear();
						buffer = textview_motivo_ingreso.Buffer;
						insertIter = buffer.StartIter;
						buffer.Insert (ref insertIter,nuevo_motivoingreso);
						textview_motivo_ingreso.Sensitive = false;
						
					}
					if (radiobutton_diag_cie10.Active == true){
						strsql = "UPDATE osiris_erp_movcargos SET "+
									"id_cie_10 = '"+iddiagnosticocie10.ToString().Trim()+"', "+
									"descripcion_diagnostico_cie10 = '"+this.entry_diag_cie10.Text+"' "+
									"WHERE folio_de_servicio =  '"+this.entry_folio_servicio.Text+"';";
					}
			        if (radiobutton_diag_final.Active == true){
			        	strsql = "UPDATE osiris_erp_movcargos SET "+
									"id_diagnostico_final = '"+this.iddiagnosticofinal.ToString().Trim()+"', "+
									"descripcion_diagnostico_final = '"+this.entry_diag_final.Text+"' "+
									"WHERE folio_de_servicio =  '"+this.entry_folio_servicio.Text+"';";		        	
					}
					if (radiobutton_cirugia.Active == true){
					            	strsql = "UPDATE osiris_erp_movcargos SET id_tipo_cirugia = '" + idcirugianumero+"', "+
												"nombre_de_cirugia ='" +this.entry_docimp_cirugia.Text.Trim()+"' "+
												"WHERE folio_de_servicio =  '"+this.entry_folio_servicio.Text+"';";
					}
					if (radiobutton_tipo_cirugia.Active == true){
						strsql = "UPDATE osiris_erp_movcargos SET tipo_cirugia ='"+descrip_tipo_cirugia+"' "+
												"WHERE folio_de_servicio =  '"+this.entry_folio_servicio.Text+"';";
					}
					if (radiobutton_diag_primeravez.Active == true){					
						strsql = "UPDATE osiris_erp_movcargos SET diagnostico_primeravez ='"+diag_primeravez+"' "+
												"WHERE folio_de_servicio =  '"+this.entry_folio_servicio.Text+"';";
					}
					if (radiobutton_observacion.Active == true){					
						strsql = "UPDATE osiris_erp_cobros_enca SET observacion_ingreso ='" + entry_observaciones.Text.Trim().ToUpper()+"' "+
												"WHERE folio_de_servicio =  '"+entry_folio_servicio.Text+"';";
						entry_observaciones.Sensitive = false;
						entry_observaciones.Text = entry_observaciones.Text.Trim().ToUpper();
					}
					if(radiobutton_servmedico.Active == true){					
						strsql = "UPDATE osiris_erp_cobros_enca SET otro_servicio_medico ='" + entry_servicio_medico.Text.Trim().ToUpper()+"' "+
												"WHERE folio_de_servicio =  '"+entry_folio_servicio.Text+"';";
						entry_servicio_medico.Sensitive = false;
						entry_servicio_medico.Text = entry_servicio_medico.Text.Trim().ToUpper();
					}
					if(radiobutton_pq_medico.Active == true){					
						strsql = "UPDATE osiris_erp_cobros_enca SET id_paquete_quirurgico ='" + entry_id_paquete_med.Text.Trim()+"' "+
									"WHERE folio_de_servicio =  '"+entry_folio_servicio.Text+"';";
					}					

					Console.WriteLine(strsql);
					comando.CommandText = strsql;
					comando.ExecuteNonQuery();
					comando.Dispose();
					checkbutton_camb_dats.Active = false;
					button_guardar.Sensitive = false;
					button_busc_medic_diag.Sensitive = false;
					button_busc_medic_trat.Sensitive = false;
					button_busc_diag.Sensitive = false;
					button_diag_final.Sensitive = false;
					button_busc_cirugia.Sensitive = false;
					button_busca_paquete.Sensitive = false;
					entry_id_paquete_med.Sensitive = false;
					entry_paquete_medico.Sensitive = false;	
					MessageDialog msgBox1 = new MessageDialog (MyWin,DialogFlags.Modal,MessageType.Info,
				         	                 ButtonsType.Ok,"El registro se guardo satisfactoriamente...");										
					msgBox1.Run ();	msgBox1.Destroy();								
				  }catch(NpgsqlException ex){
							   	MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
													MessageType.Error, 
												ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
					msgBoxError.Run ();		msgBoxError.Destroy();
				}
				conexion.Close();
			}
		}
		
		void on_button_guardar_tab_clicked(object sender, EventArgs a)
		{
			Gtk.Button button_guardar_sender = sender as Gtk.Button;
			string strsql = "";
			string update_producto = "";
			string update_tipoanestesia = "";
			string update_anestesiologo = "";
			string update_tecnicovision = "";
			string update_observacion_qx = "";
			string update_cirujano1 = "";
			string update_cirujano2 = "";
			string update_nropaseqx = "";
			string updatecxpaseqx = "";			
			if(button_guardar_sender.Name == "button_guardar_tabqx"){
				if((string) classpublic.lee_registro_de_tabla("osiris_erp_pases_qxurg","id_secuencia","WHERE id_secuencia = '"+nropaseqxurg.Trim()+"' AND folio_de_servicio = '"+folioservicio.ToString().Trim()+"' ","id_secuencia","string") != ""){
						
						MessageDialog msgBox = new MessageDialog (MyWin,DialogFlags.Modal,
									MessageType.Question,ButtonsType.YesNo,"¿ Esta seguro de actualizar la informacion... ");
					ResponseType miResultado = (ResponseType)msgBox.Run ();
					msgBox.Destroy();
		 			if (miResultado == ResponseType.Yes){
						if((bool) button_guardar_tabqx.Sensitive == true){	
							update_producto = entry_id_producto.Text.ToString().Trim()+" ;"+entry_descripcion_producto.Text.ToString()+" ;"+entry_nroserie.Text.ToString().Trim().ToUpper();
							update_tipoanestesia = " ;"+tipoantestesia;
							update_anestesiologo = " ;"+entry_id_anestesiologo.Text.Trim()+" ;"+entry_anestesiologo.Text.ToString().Trim().ToUpper();
							update_observacion_qx = " ;"+entry_observacion_qx.Text.ToUpper();
							update_cirujano1 = " ;"+entry_id_cirujano1.Text+" ;"+entry_nombre_cirujano1.Text.ToUpper();
							update_cirujano2 = " ;"+entry_id_cirujano2.Text+" ;"+entry_nombre_cirujano2.Text.ToUpper();
							updatecxpaseqx = ";"+idcxpaseqx.ToString().Trim()+" ;"+entry_cirugia_paseqx.Text.ToUpper();
							NpgsqlConnection conexion; 
			            	conexion = new NpgsqlConnection (connectionString+nombrebd);
							// Verifica que la base de datos este conectada
							try{
								conexion.Open ();
								NpgsqlCommand comando; 
								comando = conexion.CreateCommand ();
								strsql = "UPDATE osiris_erp_cobros_enca SET observaciones1 = observaciones1 || '"+update_producto+update_tipoanestesia+update_anestesiologo+update_observacion_qx+update_cirujano2+update_cirujano1+";"+nropaseqxurg.Trim()+updatecxpaseqx+
								";"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+";"+LoginEmpleado+"\n'"+
										"WHERE folio_de_servicio =  '"+folioservicio.ToString().Trim()+"';";
								//Console.WriteLine(strsql);
								comando.CommandText = strsql;
								comando.ExecuteNonQuery();
								comando.Dispose();
								
								strsql = "UPDATE osiris_erp_movcargos SET id_producto = '"+entry_id_producto.Text.Trim()+"',"+
											"producto_observacion1 = '"+entry_nroserie.Text+"'," +
											"tipo_anestesia = '"+tipoantestesia.ToString().Trim()+"'," +
											"id_anestesiologo = '"+entry_id_anestesiologo.Text.Trim()+"' "+
											"WHERE folio_de_servicio = '"+folioservicio.ToString().Trim()+"';";
								//Console.WriteLine(strsql);
								comando.CommandText = strsql;
								comando.ExecuteNonQuery();
								comando.Dispose();
								
								strsql = "UPDATE osiris_erp_pases_qxurg SET observaciones = '"+entry_observacion_qx.Text.ToUpper()+"'," +
								 									"id_producto = '"+entry_id_producto.Text.ToString().Trim()+"'," +
								 									"descripcion_producto = '"+entry_descripcion_producto.Text.ToString()+"'," +
								 									"numero_serie = '"+entry_nroserie.Text.ToString().Trim().ToUpper()+"'," +
								 									"tipo_anestesia = '" +tipoantestesia+"'," +
								 									"id_anestesiologo = '"+entry_id_anestesiologo.Text.Trim()+"'," +
								 									"nombre_anestesiologo = '"+entry_anestesiologo.Text.ToString().Trim().ToUpper()+"'," +
								 									"id_ayudante = '" +entry_id_cirujano2.Text.ToString().Trim()+"'," +
								 									"nombre_ayudante = '"+entry_nombre_cirujano2.Text.ToUpper()+"'," +
								 									"id_cirujano = '"+entry_id_cirujano1.Text+"'," +
								 									"nombre_cirujano = '"+entry_nombre_cirujano1.Text.ToUpper()+"'," +
								 									"id_cirugia = '" +idcxpaseqx.ToString().Trim()+"'," +
								 									"descripcion_cirugia = '" + entry_cirugia_paseqx.Text.ToUpper()+"'," +
								 									"fechahora_actualizapase = '"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"'," +
								 									"id_quien_actualizapase = '"+LoginEmpleado+"' "+							 									
								 									"WHERE id_secuencia = '"+nropaseqxurg.Trim()+"' " +
								 									"AND folio_de_servicio = '"+folioservicio.ToString().Trim()+"';";
								//Console.WriteLine(strsql);
								comando.CommandText = strsql;
								comando.ExecuteNonQuery();
								comando.Dispose();
							
								MessageDialog msgBox1 = new MessageDialog (MyWin,DialogFlags.Modal,MessageType.Info,
						         	                 ButtonsType.Ok,"El registro se guardo satisfactoriamente...");										
								msgBox1.Run ();	msgBox1.Destroy();
							}catch(NpgsqlException ex){
								MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
															MessageType.Error, 
														ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
								msgBoxError.Run ();		msgBoxError.Destroy();
							}
							conexion.Close();
						}
					}
				}else{
					MessageDialog msgBox1 = new MessageDialog (MyWin,DialogFlags.Modal,MessageType.Info,
						         	                 ButtonsType.Ok,"Debe Seleccionar un pase QX./URG. para poder actualizar la informacion...");										
					msgBox1.Run ();	msgBox1.Destroy();
				}
			}
				
			if(button_guardar_sender.Name == "button_guardar_tabvision"){
				MessageDialog msgBox = new MessageDialog (MyWin,DialogFlags.Modal,
									MessageType.Question,ButtonsType.YesNo,"¿ Esta seguro de actualizar la informacion... ");
				ResponseType miResultado = (ResponseType)msgBox.Run ();
				msgBox.Destroy();
		 		if (miResultado == ResponseType.Yes){				
					if((bool) button_guardar_tabvision.Sensitive == true){
						update_tecnicovision = nombretecnicovision;
						strsql = "UPDATE osiris_erp_cobros_enca SET observaciones2 = observaciones2 || '"+update_tecnicovision+";"+DateTime.Now.ToString("yyyy-MM-dd HH:mm tt")+";"+LoginEmpleado+"\n' "+
									"WHERE folio_de_servicio =  '"+folioservicio.ToString().Trim()+"';";
						//Console.WriteLine(strsql);
						
						NpgsqlConnection conexion; 
			           	conexion = new NpgsqlConnection (connectionString+nombrebd);
						// Verifica que la base de datos este conectada
						try{
							conexion.Open ();
							NpgsqlCommand comando; 
							comando = conexion.CreateCommand ();
							comando.CommandText = strsql;
							comando.ExecuteNonQuery();
							comando.Dispose();
						}catch(NpgsqlException ex){
							MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
														MessageType.Error, 
													ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
							msgBoxError.Run ();		msgBoxError.Destroy();
						}
						conexion.Close();
					}
				}
			}
			if(button_guardar_sender.Name == "button_graba_regentrada"){
				// logs_modificaciones 
				
				NpgsqlConnection conexion; 
			    conexion = new NpgsqlConnection (connectionString+nombrebd);
				// Verifica que la base de datos este conectada
				try{
					conexion.Open ();
					NpgsqlCommand comando; 
					comando = conexion.CreateCommand ();
					if((bool) checkbutton_edit_aseguradora.Active == true){
						strsql = "UPDATE osiris_erp_cobros_enca SET "+"id_aseguradora = '"+entry_edit_idaseguradora.Text.Trim()+"'," +
						 		"logs_cambios = logs_cambios||'id_aseguradora;"+idaseguradora_paciente.ToString().Trim()+";"+entry_edit_idaseguradora.Text.Trim()+";"+LoginEmpleado+"\n' "+
								"WHERE folio_de_servicio = '"+folioservicio.ToString().Trim()+"';";
						comando.CommandText = strsql;
						//Console.WriteLine(strsql);
						comando.ExecuteNonQuery();
						comando.Dispose();
						idaseguradora_paciente = int.Parse(entry_edit_idaseguradora.Text.Trim());
						entry_aseguradora.Text = entry_nombre_aseguradora.Text;
					}
			   		if((bool) checkbutton_edit_instempre.Active == true){
						strsql = "UPDATE osiris_erp_cobros_enca SET id_empresa = '"+entry_edit_idinstempr.Text.Trim()+"'," +
						 		"logs_cambios = logs_cambios||'id_empresa;"+idempresa_paciente.ToString().Trim()+";"+entry_edit_idinstempr.Text.Trim()+";"+LoginEmpleado+"\n' "+
								"WHERE folio_de_servicio = '"+folioservicio.ToString().Trim()+"';";
						comando.CommandText = strsql;
						//Console.WriteLine(strsql);
						comando.ExecuteNonQuery();
						comando.Dispose();
						idempresa_paciente = int.Parse(entry_edit_idinstempr.Text.Trim());
					}
			   		if((bool) checkbutton_edit_tipopx.Active == true){
						strsql = "UPDATE osiris_erp_movcargos SET id_tipo_paciente = '"+id_tipopaciente.ToString().Trim()+"'," +
						 		"logs_modificaciones = logs_modificaciones ||'id_tipo_paciente;"+id_tipopaciente.ToString().Trim()+";"+id_tipopacienteanterior.ToString().Trim()+";"+LoginEmpleado+"\n' "+
								"WHERE folio_de_servicio = '"+folioservicio.ToString().Trim()+"';";
						comando.CommandText = strsql;
						//Console.WriteLine(strsql);
						comando.ExecuteNonQuery();
						comando.Dispose();

						strsql = "UPDATE osiris_erp_cobros_enca SET id_tipo_paciente = '"+id_tipopaciente.ToString().Trim()+"'," +
							"logs_cambios = logs_cambios ||'id_tipo_paciente;"+id_tipopaciente.ToString().Trim()+";"+id_tipopacienteanterior.ToString().Trim()+";"+LoginEmpleado+"\n' "+
							"WHERE folio_de_servicio = '"+folioservicio.ToString().Trim()+"';";
						comando.CommandText = strsql;
						//Console.WriteLine(strsql);
						comando.ExecuteNonQuery();
						comando.Dispose();

						strsql = "UPDATE osiris_erp_abonos SET id_tipo_paciente = '"+id_tipopaciente.ToString().Trim()+"' " +
								"WHERE folio_de_servicio = '"+folioservicio.ToString().Trim()+"';";
						comando.CommandText = strsql;
						//Console.WriteLine(strsql);
						comando.ExecuteNonQuery();
						comando.Dispose();

					}
			   		if((bool) checkbutton_edit_tipoadmin.Active == true){
						strsql = "UPDATE  osiris_erp_movcargos SET id_tipo_admisiones = '"+id_tipoadmision.ToString().Trim()+"'," +
						 		"logs_modificaciones = logs_modificaciones||'id_tipo_admisiones;"+id_tipoadmision.ToString().Trim()+";"+id_tipoadmisionanterior.ToString().Trim()+";"+LoginEmpleado+"\n' "+
								"WHERE folio_de_servicio = '"+folioservicio.ToString().Trim()+"';";
						comando.CommandText = strsql;
						//Console.WriteLine(strsql);
						comando.ExecuteNonQuery();
						comando.Dispose();
					}
			   		if((bool) checkbutton_fech_altamed.Active == true){
						strsql = "UPDATE osiris_erp_cobros_enca SET fecha_alta_paciente = '"+entry_ano_altamed.Text+"-"+entry_mes_altamed.Text+"-"+entry_dia_altamed.Text+" "+entry_hora_altamed.Text+":"+entry_minuto_altamed.Text+":00'," +
						 		"logs_cambios = logs_cambios||'fecha_alta_paciente;"+entry_fechahora_alta.Text+";"+entry_ano_altamed.Text+"-"+entry_mes_altamed.Text+"-"+entry_dia_altamed.Text+" "+entry_hora_altamed.Text+":"+entry_minuto_altamed.Text+":00"+";"+LoginEmpleado+"\n' "+
								"WHERE folio_de_servicio = '"+folioservicio.ToString().Trim()+"';";
						comando.CommandText = strsql;
						//Console.WriteLine(strsql);
						comando.ExecuteNonQuery();
						comando.Dispose();
						checkbutton_fech_altamed.Active = false;
						entry_fechahora_alta.Text = entry_ano_altamed.Text+"-"+entry_mes_altamed.Text+"-"+entry_dia_altamed.Text+" "+entry_hora_altamed.Text+":"+entry_minuto_altamed.Text+":00'";
					}
					if((bool) checkbutton_regadm.Active == true){
						strsql = "UPDATE osiris_erp_cobros_enca SET fechahora_creacion = '"+entry_ano_regadm.Text+"-"+entry_mes_regadm.Text+"-"+entry_dia_regadm.Text+" "+entry_hora_regadm.Text+":"+entry_minuto_regadm.Text+":00'," +
							"logs_cambios = logs_cambios||'fechahora_creacion;"+entry_fecha_admision.Text+";"+entry_ano_regadm.Text+"-"+entry_mes_regadm.Text+"-"+entry_dia_regadm.Text+" "+entry_hora_regadm.Text+":"+entry_minuto_regadm.Text+":00"+";"+LoginEmpleado+"\n' "+
							"WHERE folio_de_servicio = '"+folioservicio.ToString().Trim()+"';";
						comando.CommandText = strsql;
						Console.WriteLine(strsql);
						comando.ExecuteNonQuery();
						comando.Dispose();
						checkbutton_regadm.Active = false;
						entry_fecha_admision.Text = entry_dia_regadm.Text+"-"+entry_mes_regadm.Text+"-"+entry_ano_regadm.Text;
						entry_hora_registro.Text = entry_hora_regadm.Text+":"+entry_minuto_regadm.Text;	
					}
					/*
					id_tipopaciente = (int) lector["idtipopaciente"];
					id_tipoadmision = (int) lector["idtipoadmision"];
            		folioservicio = int.Parse((string) foliodeservicio_);
					PidPaciente = int.Parse(entry_pid_paciente.Text);						
					idmedico = (int) lector["id_medico"];
					idcirugianumero = (int) lector["id_tipo_cirugia"];
					idmedicotratante =  (int) lector["id_medico_tratante"]; 						
					idaseguradora_paciente = (int) lector["idaseguradora"];
					idempresa_paciente = (int) lector["idempresa"];
					*/
				}catch(NpgsqlException ex){
					MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
												MessageType.Error, 
											ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
					msgBoxError.Run ();		msgBoxError.Destroy();
				}
				conexion.Close();				
			}
			if(button_guardar_sender.Name == "button_graba_edirespo"){
				
			}			
		}
		
		void on_button_llena_cirugias_clicked(object sender, EventArgs a)
		{
			llena_lista_cirugias();			
		}
		
		[GLib.ConnectBefore ()] 
		void onKeyPressEvent_enterbucar_cirugia(object o, Gtk.KeyPressEventArgs args)
		{
			if (args.Event.Key.ToString() == "Return" || args.Event.Key.ToString() == "KP_Enter"){
				args.RetVal = true;
				llena_lista_cirugias();
			}	
		}
		
		[GLib.ConnectBefore ()]
		void onKeyPressEvent_enterbucar_cxpaseqx(object o, Gtk.KeyPressEventArgs args)
		{
			if (args.Event.Key.ToString() == "Return" || args.Event.Key.ToString() == "KP_Enter"){
				args.RetVal = true;
				llena_lista_cirugias();
			}	
		}
		
		void llena_lista_cirugias()			
		{
			treeViewEngineBusca.Clear();						
			NpgsqlConnection conexion; 
			conexion = new NpgsqlConnection (connectionString+nombrebd );            
			// Verifica que la base de datos este conectada
			try{
				conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand ();
				entry_expresion.Text = this.entry_expresion.Text.Trim();			
				if ((string) entry_expresion.Text.Trim() == ""){
					comando.CommandText = "SELECT id_tipo_cirugia, descripcion_cirugia, tiene_paquete "+
									 	  "FROM osiris_his_tipo_cirugias " +
									   		"WHERE (id_tipo_cirugia != '0');";
				}else{
				    comando.CommandText = "SELECT id_tipo_cirugia, descripcion_cirugia, tiene_paquete "+
									 	  "FROM osiris_his_tipo_cirugias " +
									 	  "WHERE (descripcion_cirugia LIKE '%"+entry_expresion.Text.ToUpper()+"%') "+
									  		"ORDER BY id_tipo_cirugia;";
				}
				NpgsqlDataReader lector = comando.ExecuteReader();
				//Console.WriteLine(comando.CommandText);
				while (lector.Read()){
	                paquete = Convert.ToString((bool) lector["tiene_paquete"]);					
					if (paquete == "True"){
						treeViewEngineBusca.AppendValues ((int) lector["id_tipo_cirugia"], 
											(string) lector["descripcion_cirugia"],"Si");
					}else{
						treeViewEngineBusca.AppendValues ((int) lector["id_tipo_cirugia"], 
											(string) lector["descripcion_cirugia"],"");
					}
				}
			}catch (NpgsqlException ex){
	   			MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
										MessageType.Error, ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
				msgBoxError.Run ();	msgBoxError.Destroy();
            }
            conexion.Close ();
		}

		void on_button_imprimir_reportes_clicked (object sender, EventArgs args)
		{
			//"PROTOCOLO DE ADMISION","PASE DE INGRESO","CONSENTIMIENTO INFORMADO","CONTRATO","ROTULOS HABITACION","HISTORIA CLINICA"
			switch (tipodereporte) {
			case "PROTOCOLO DE ADMISION":
				if ((string) this.entry_folio_servicio.Text == "" || (string) entry_pid_paciente.Text == "" ){	
					MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
						MessageType.Error, 
						ButtonsType.Close, "Debe de llenar el campo de Folio con uno \n"+
						"existente para que el protocolo se muestre \n"+"o no a pulsado el boton ''Seleccionar''");
					msgBoxError.Run ();					msgBoxError.Destroy();
				}else{
					//rpt_prot_admision.cs
					new protocolo_admision(PidPaciente,int.Parse(entry_folio_servicio.Text),nombrebd,entry_doctor.Text.ToUpper().Trim()); // rpt_prot_admision.cs
				} 
				break;
			case "PASE DE INGRESO":
				if ((string) this.entry_folio_servicio.Text == "" || (string) entry_pid_paciente.Text == "" ){	
					MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
						MessageType.Error, 
						ButtonsType.Close, "Debe de llenar el campo de Folio con uno \n"+
						"existente para que el contrato se muestre \n"+"o no a pulsado el boton ''Seleccionar''");
					msgBoxError.Run ();					msgBoxError.Destroy();
				}else{
					new osiris.pases_a_quirofano(PidPaciente,folioservicio,id_tipoadmision,LoginEmpleado,id_tipopaciente,idempresa_paciente,idaseguradora_paciente,false,"pase_de_ingreso",false,false,cuenta_cerrada);
				}
				break;
			case "CONSENTIMIENTO INFORMADO":
				if ((string) this.entry_folio_servicio.Text == "" || (string) entry_pid_paciente.Text == "" ){	
					MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
						MessageType.Error, 
						ButtonsType.Close, "Debe de llenar el campo de Folio con uno \n"+
						"existente para que el consentimento se muestre \n"+"o no a pulsado el boton ''Seleccionar''");
					msgBoxError.Run ();					msgBoxError.Destroy();
				}else{
					//rpt_cons_informado.cs
					new conse_info(PidPaciente,int.Parse(entry_folio_servicio.Text),nombrebd,
						entry_nombre_paciente.Text.Trim(),
						entry_medic_diag.Text.Trim(),
						entry_med_trat.Text.ToUpper().Trim(),
						"");   // rpt_cons_informado.cs
				}
				break;
			case "CONTRATO":
				if ((string) this.entry_folio_servicio.Text == "" || (string) entry_pid_paciente.Text == "" ){	
					MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
						MessageType.Error, 
						ButtonsType.Close, "Debe de llenar el campo de Folio con uno \n"+
						"existente para que el contrato se muestre \n"+"o no a pulsado el boton ''Seleccionar''");
					msgBoxError.Run ();					msgBoxError.Destroy();
				}else{
					//new con_prest(PidPaciente,int.Parse(entry_folio_servicio.Text),nombrebd,entry_doctor.Text);   // rpt_cons_informado.cs
				}
				break;
			case "ROTULOS HABITACION":

				break;
			case "HISTORIA CLINICA":
				if ((string) this.entry_folio_servicio.Text == "" || (string) entry_pid_paciente.Text == "" ){	
					MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
						MessageType.Error, 
						ButtonsType.Close, "Debe de llenar el campo de Folio con uno \n"+
						"existente para que el contrato se muestre \n"+"o no a pulsado el boton ''Seleccionar''");
					msgBoxError.Run ();					msgBoxError.Destroy();
				}else{
					new osiris.historia_clinica(entry_nombre_paciente.Text,entry_pid_paciente.Text,entry_edad_paciente.Text,
						LoginEmpleado,NomEmpleado,AppEmpleado,ApmEmpleado,nombrebd,entry_fecha_admision.Text,"entry_fecha_nacimiento.Text");
				}
				break;
			case "PASE SECCION 50":
				if ((string)this.entry_folio_servicio.Text == "" || (string)entry_pid_paciente.Text == "") {	
					MessageDialog msgBoxError = new MessageDialog (MyWinError, DialogFlags.DestroyWithParent,
						                            MessageType.Error, 
						                            ButtonsType.Close, "Debe de llenar el campo de Folio con uno \n" +
					                            "existente para que el contrato se muestre \n" + "o no a pulsado el boton ''Seleccionar''");
					msgBoxError.Run ();					msgBoxError.Destroy ();
				} else {
					new osiris.pases_a_quirofano(PidPaciente,folioservicio,id_tipoadmision,LoginEmpleado,id_tipopaciente,idempresa_paciente,idaseguradora_paciente,false,"pase_seccion_50",false,false,cuenta_cerrada);
				}
				break;
			}
		}

		void on_selec_folio_clicked (object sender, EventArgs args)
		{
			llenado_de_datos_paciente( (string) entry_folio_servicio.Text );
		}
		
		void llenado_de_datos_paciente(string foliodeservicio_)
		{
			// limpiando entry tab_qx
			buffer.Clear();
			buffer = textview_motivo_ingreso.Buffer;
			insertIter = buffer.StartIter;
			
			entry_id_producto.Text = "";
			entry_descripcion_producto.Text = "";
			entry_nroserie.Text = "";			
			entry_id_anestesiologo.Text = "0";
			entry_anestesiologo.Text = "";
			entry_observacion_qx.Text = "";
			entry_id_cirujano2.Text = "0";
			entry_nombre_cirujano2.Text = "";
			entry_id_cirujano1.Text = "0";
			entry_nombre_cirujano1.Text = "";
			entry_estatus.Text = "";
			checkbutton_lente.Sensitive = true;
			checkbutton_tipo_anestesia.Sensitive = true;
			checkbutton_anestesiologo.Sensitive = true;
			checkbutton_observacion_qx.Sensitive = true;
			checkbutton_cirujano2.Sensitive = true;
			checkbutton_edit_admision.Sensitive = true;
			llenado_combobox(0,"",combobox_tipo_cirugia,"array","","","",args_tipos_cirugias,args_id_array,"");
			llenado_combobox(0,"",combobox_diagprimeravez,"array","","","",args_diag_primeravez,args_id_array,"");
			llenado_combobox(0,"",combobox_tipo_anestecia,"array","","","",args_tipoanestesia,args_id_array,"");
			llenado_combobox(0,"",combobox_tecnicovision,"array","","","",args_tecnicosvision,args_id_array,"");
						
			NpgsqlConnection conexion; 
			conexion = new NpgsqlConnection (connectionString+nombrebd);
            
			// Verifica que la base de datos este conectada
			try{
				conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand ();
               	
				// asigna el numero de folio de ingreso de paciente (FOLIO)
				comando.CommandText = "SELECT osiris_erp_cobros_enca.folio_de_servicio AS foliodeatencion,to_char(osiris_erp_cobros_enca.pid_paciente,'9999999999') AS pidpaciente,"+
            				"nombre1_paciente,nombre2_paciente,apellido_paterno_paciente,apellido_materno_paciente,"+
            				"telefono_particular1_paciente,numero_poliza,folio_de_servicio_dep,"+
            				"nombre_de_cirugia,cerrado,"+
							"to_char(to_number(to_char(age('"+DateTime.Now.ToString("yyyy-MM-dd")+"',osiris_his_paciente.fecha_nacimiento_paciente),'yyyy') ,'9999'),'9999') AS edad_paciente, "+
            				"to_char(fecha_nacimiento_paciente,'yyyy-MM-dd') AS fech_nacimiento,sexo_paciente,"+
							"descripcion_diagnostico_movcargos,osiris_erp_movcargos.id_tipo_cirugia,"+
            				"nombre_medico_encabezado,observacion_ingreso,otro_servicio_medico,"+
            				"id_medico_tratante,nombre_medico_tratante,"+
            				"to_char(osiris_erp_cobros_enca.fechahora_creacion,'dd-MM-yyyy') AS fecha_registro,"+
            				"to_char(osiris_erp_cobros_enca.fechahora_creacion,'HH24:MI') AS hora_registro," +
							"to_char(osiris_erp_cobros_enca.fechahora_creacion, 'yyyy') AS ano_fecha_registro,"+ 
							"to_char(osiris_erp_cobros_enca.fechahora_creacion, 'MM') AS mes_fecha_registro,"+
							"to_char(osiris_erp_cobros_enca.fechahora_creacion, 'dd') AS dia_fecha_registro, "+
							"to_char(osiris_erp_cobros_enca.fechahora_creacion, 'HH24') AS horas_registro,"+
							"to_char(osiris_erp_cobros_enca.fechahora_creacion, 'MI') AS minuto_registro,"+
							"alta_paciente," +
            				"to_char(osiris_erp_cobros_enca.fecha_alta_paciente, 'yyyy-MM-dd HH24:mi:ss') AS fechaaltapaciente,"+
            				"to_char(osiris_erp_cobros_enca.fecha_alta_paciente, 'yyyy') AS ano_fecha_egre,"+ 
							"to_char(osiris_erp_cobros_enca.fecha_alta_paciente, 'MM') AS mes_fecha_egre,"+
							"to_char(osiris_erp_cobros_enca.fecha_alta_paciente, 'dd') AS dia_fecha_egre, "+
							"to_char(osiris_erp_cobros_enca.fecha_alta_paciente, 'HH24') AS hora_egre,"+
							"to_char(osiris_erp_cobros_enca.fecha_alta_paciente, 'MI') AS minuto_egre,"+
            				"osiris_erp_movcargos.id_tipo_paciente AS idtipopaciente,descripcion_tipo_paciente,"+
            				"osiris_erp_movcargos.id_tipo_admisiones AS idtipoadmision,descripcion_admisiones,"+
							"osiris_erp_cobros_enca.id_empresa AS idempresa,osiris_empresas.descripcion_empresa,"+
            				"osiris_erp_cobros_enca.id_aseguradora AS idaseguradora,descripcion_aseguradora, "+
            				"descripcion_diagnostico_movcargos,nombre_medico_encabezado, "+
            				"osiris_erp_cobros_enca.id_medico,nombre_medico,cancelado,tipo_cirugia,diagnostico_primeravez,"+
            				"osiris_erp_movcargos.descripcion_diagnostico_cie10,osiris_erp_movcargos.descripcion_diagnostico_final," +
            				"observaciones1,observaciones2,osiris_erp_cobros_enca.id_paquete_quirurgico AS idpaqueteqx,osiris_his_tipo_cirugias.descripcion_cirugia "+            				
            				//"osiris_his_tipo_diagnosticos.descripcion_diagnostico "+
            				"FROM osiris_erp_cobros_enca,osiris_his_paciente,osiris_erp_movcargos,osiris_his_tipo_admisiones,osiris_his_tipo_pacientes, "+
            				"osiris_aseguradoras,osiris_his_medicos,osiris_empresas,osiris_his_tipo_cirugias "+ 
            				//"osiris_his_tipo_diagnosticos "+
            				"WHERE osiris_erp_cobros_enca.pid_paciente = osiris_his_paciente.pid_paciente "+
            				"AND osiris_erp_movcargos.folio_de_servicio = osiris_erp_cobros_enca.folio_de_servicio "+
            				"AND osiris_erp_cobros_enca.id_medico = osiris_his_medicos.id_medico "+
							"AND osiris_erp_cobros_enca.id_empresa = osiris_empresas.id_empresa "+
            				"AND osiris_erp_cobros_enca.id_aseguradora = osiris_aseguradoras.id_aseguradora "+ 
            				"AND osiris_erp_movcargos.id_tipo_admisiones = osiris_his_tipo_admisiones.id_tipo_admisiones "+
            				"AND osiris_erp_movcargos.id_tipo_paciente = osiris_his_tipo_pacientes.id_tipo_paciente "+
							"AND osiris_erp_cobros_enca.id_paquete_quirurgico = osiris_his_tipo_cirugias.id_tipo_cirugia "+
            				//"AND osiris_erp_movcargos.id_cie_10 = osiris_his_tipo_diagnosticos.id_cie_10 "+
            				"AND osiris_erp_cobros_enca.folio_de_servicio = '"+foliodeservicio_+"';";
							
				NpgsqlDataReader lector = comando.ExecuteReader ();            	
				if(lector.Read()){
					if((bool) lector["cancelado"]){
						MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
													MessageType.Error, ButtonsType.Close, "ESTE FOLIO HA SIDO CANCELADO\n");
						msgBoxError.Run ();
						msgBoxError.Destroy();
						entry_estatus.Text = "CANCELADO";
						checkbutton_lente.Sensitive = false;
						checkbutton_tipo_anestesia.Sensitive = false;
						checkbutton_anestesiologo.Sensitive = false;
						checkbutton_observacion_qx.Sensitive = false;
						checkbutton_cirujano2.Sensitive = false;
						checkbutton_edit_admision.Sensitive = false;
					}else{
						if((bool) lector["alta_paciente"]){
							entry_estatus.Text = "ALTA MEDICA";
							checkbutton_edit_admision.Sensitive = false;
						}
						if((bool) lector["cerrado"]){
							entry_estatus.Text = "CERRADO";
							checkbutton_lente.Sensitive = false;
							checkbutton_tipo_anestesia.Sensitive = false;
							checkbutton_anestesiologo.Sensitive = false;
							checkbutton_observacion_qx.Sensitive = false;
							checkbutton_cirujano2.Sensitive = false;
							checkbutton_edit_admision.Sensitive = false;
						}
						
						cuenta_cerrada = (bool) lector["cerrado"];
						entry_fecha_admision.Text = (string) lector["fecha_registro"];
						entry_hora_registro.Text = (string) lector["hora_registro"];
						entry_fechahora_alta.Text = (string) lector["fechaaltapaciente"];
						entry_nombre_paciente.Text = (string) lector["nombre1_paciente"].ToString().Trim()+" "
													+lector["nombre2_paciente"].ToString().Trim()+" "+lector["apellido_paterno_paciente"].ToString().Trim()+" "
													+lector["apellido_materno_paciente"].ToString().Trim();
						entry_pid_paciente.Text = (string) lector["pidpaciente"];
						entry_telefono_paciente.Text = (string) lector["telefono_particular1_paciente"];
						if((int) lector["id_medico"] > 1){
							entry_doctor.Text = lector["nombre_medico"].ToString().Trim();
						}else{
							entry_doctor.Text = lector["nombre_medico_encabezado"].ToString().Trim();
						}
						if((int) lector ["idaseguradora"] > 1){
							entry_aseguradora.Text = (string) lector["descripcion_aseguradora"];
						}else{
							entry_aseguradora.Text = (string) lector["descripcion_empresa"];
						}
						entry_observaciones.Text = (string) lector["observacion_ingreso"];
						entry_servicio_medico.Text = (string) lector["otro_servicio_medico"];
						entry_tipo_paciente.Text = (string) lector["descripcion_tipo_paciente"];
						entry_edit_idaseguradora.Text = lector["descripcion_aseguradora"].ToString();
						entry_nombre_aseguradora.Text = lector["descripcion_aseguradora"].ToString();
						entry_edit_idinstempr.Text = lector["idempresa"].ToString();
						entry_nombre_instempr.Text = lector["descripcion_empresa"].ToString();
						entry_edad_paciente.Text = (string) lector["edad_paciente"];						
						entry_poliza.Text =  (string) lector["numero_poliza"];
						id_tipopaciente = (int) lector["idtipopaciente"];
						id_tipopacienteanterior = (int) lector["idtipopaciente"];
						id_tipoadmision = (int) lector["idtipoadmision"];
						id_tipoadmisionanterior = (int) lector["idtipoadmision"];
            			folioservicio = int.Parse((string) foliodeservicio_);
						PidPaciente = int.Parse(entry_pid_paciente.Text);						
						idmedico = (int) lector["id_medico"];
						idcirugianumero = (int) lector["id_tipo_cirugia"];
						idmedicotratante =  (int) lector["id_medico_tratante"]; 						
						idaseguradora_paciente = (int) lector["idaseguradora"];
						idempresa_paciente = (int) lector["idempresa"];						
						entry_edit_idaseguradora.Text = idaseguradora_paciente.ToString().Trim();
						entry_nombre_aseguradora.Text = (string) lector["descripcion_aseguradora"];
						entry_ano_altamed.Text = (string) lector["ano_fecha_egre"];
						entry_mes_altamed.Text = (string) lector["mes_fecha_egre"];
						entry_dia_altamed.Text = (string) lector["dia_fecha_egre"];
						entry_hora_altamed.Text = (string) lector["hora_egre"];
						entry_minuto_altamed.Text = (string) lector["minuto_egre"];
						entry_ano_regadm.Text = (string) lector["ano_fecha_registro"];
						entry_mes_regadm.Text = (string) lector["mes_fecha_registro"];
						entry_dia_regadm.Text = (string) lector["dia_fecha_registro"];
						entry_hora_regadm.Text = (string) lector["horas_registro"];
						entry_minuto_regadm.Text = (string) lector["minuto_registro"];
						entry_medic_diag.Text = (string) lector["nombre_medico"];
						entry_docimp_cirugia.Text = (string) lector["nombre_de_cirugia"];
						entry_med_trat.Text = (string) lector["nombre_medico_tratante"];
						buffer.Insert (ref insertIter,(string) lector["descripcion_diagnostico_movcargos"]);
						entry_diag_cie10.Text = (string) lector["descripcion_diagnostico_cie10"];
						entry_diag_final.Text = (string) lector["descripcion_diagnostico_final"];
						entry_id_paquete_med.Text = lector["idpaqueteqx"].ToString().Trim();
						entry_paquete_medico.Text = lector["descripcion_cirugia"].ToString().Trim();
						llenado_combobox(1,lector["tipo_cirugia"].ToString().Trim(),combobox_tipo_cirugia,"array","","","",args_tipos_cirugias,args_id_array,"");
						llenado_combobox(1,lector["diagnostico_primeravez"].ToString().Trim(),combobox_diagprimeravez,"array","","","",args_diag_primeravez,args_id_array,"");
						llenado_combobox(1,"0",combobox_nropaseqx,"sql","SELECT to_char(id_secuencia,'99999999999') AS idsecuencia,id_secuencia,folio_de_servicio FROM osiris_erp_pases_qxurg WHERE folio_de_servicio = '"+foliodeservicio_+"' AND eliminado = 'false' ORDER BY id_secuencia;","idsecuencia","id_secuencia",args_args,args_id_array,"id_secuencia");
						llenado_combobox(1,lector["descripcion_tipo_paciente"].ToString().Trim(),combobox_tipo_paciente,"sql","SELECT * FROM osiris_his_tipo_pacientes WHERE activo_tipo_paciente = 'true' ORDER BY descripcion_tipo_paciente;","descripcion_tipo_paciente","id_tipo_paciente",args_args,args_id_array,"id_tipo_documento");
						llenado_combobox(1,lector["descripcion_admisiones"].ToString().Trim(),combobox_tipo_admision,"sql","SELECT * FROM osiris_his_tipo_admisiones WHERE servicio_directo = 'false' AND activo_admision = 'true' ORDER BY id_tipo_admisiones;","descripcion_admisiones","id_tipo_admisiones",args_args,args_id_array,"acceso_servicios_directo");
						char[] delimiterChars = {'\n'}; // delimitador de Cadenas
						char[] delimiterChars1 = {';'}; // delimitador de Cadenas
						string texto = (string) lector["observaciones1"]; // puede ser una campo de la base de datos tipo Text
						//string texto = "1;daniel; ;olivares;cuevas";
						//"2;genaro;cuevas;bazaldua\n"+
						//"3;gladys;perez;orellana\n";
						string[] words = texto.Split(delimiterChars); // Separa las Cadenas
						// Recorre la variable
						foreach (string s in words){
							if (s.Length > 0){
								string texto1 = (string) s;
								string[] words1 = texto1.Split(delimiterChars1);
								//for (int i = 1; i <= 6; i++){
								int i=1;
								foreach (string s1 in words1){
									//Console.WriteLine(s1.ToString());
									switch (i){
										case 1:
											if(words1[i].Trim() == ""){
												entry_id_producto.Text = "0";
											}else{
												entry_id_producto.Text = s1;
											}
										break;
										case 2:
											entry_descripcion_producto.Text = s1;
										break;
										case 3:
											entry_nroserie.Text = s1;
										break;
										case 4:
											tipoantestesia = s1;
											llenado_combobox(1,s1,combobox_tipo_anestecia,"array","","","",args_tipoanestesia,args_id_array,"");
										break;
										case 5:
											if(s.Trim() == ""){
												entry_id_anestesiologo.Text = "0";
											}else{
												entry_id_anestesiologo.Text = s1;
											}
										break;
										case 6:
											entry_anestesiologo.Text = s1;
										break;
										case 7:
											entry_observacion_qx.Text = s1;
										break;
										case 8:
											if(s.Trim() == ""){
												entry_id_cirujano2.Text = "0";
											}else{
												entry_id_cirujano2.Text = s1;
											}
										break;
										case 9:
											entry_nombre_cirujano2.Text = s1;
										break;
									}
									i++;
								}
							}
						}	
					}
					checkbutton_camb_dats.Sensitive = true;
					//this.button_guardar.Sensitive = true;
				}else{
					MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
													MessageType.Error, ButtonsType.Close, "NO existe el numero de atencion... verifique.");
					msgBoxError.Run ();						msgBoxError.Destroy();					
				}
			}catch (NpgsqlException ex){
	   			Console.WriteLine ("PostgresSQL error: {0}",ex.Message);
	       	}
       		conexion.Close ();
		}
		
		void llenado_informacion_paseqxurg(string nropaseqxurg_,string folioservicio_)
		{
			NpgsqlConnection conexion; 
			conexion = new NpgsqlConnection (connectionString+nombrebd);
            
			// Verifica que la base de datos este conectada
			try{
				conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand ();
               	
				// asigna el numero de folio de ingreso de paciente (FOLIO)
				comando.CommandText = "SELECT id_secuencia,pid_paciente,folio_de_servicio,id_producto,descripcion_producto,id_cirugia,descripcion_cirugia,to_char(fechahora_creacion,'yyyy-MM-dd') AS fechapaseqx," +
					"numero_serie,tipo_anestesia,id_anestesiologo,nombre_anestesiologo,id_ayudante,nombre_ayudante,id_cirujano,nombre_cirujano,observaciones " +
					"FROM osiris_erp_pases_qxurg " +
					"WHERE id_secuencia ='"+ nropaseqxurg_ +"' " +
					"AND folio_de_servicio ='"+folioservicio_+"' " +
					"AND eliminado = 'false' " +
					"ORDER BY id_secuencia DESC;";
				NpgsqlDataReader lector = comando.ExecuteReader ();            	
				if(lector.Read()){					
					entry_id_producto.Text = lector["id_producto"].ToString().Trim();
					entry_descripcion_producto.Text = lector["descripcion_producto"].ToString().Trim();
					entry_nroserie.Text = lector["numero_serie"].ToString().Trim();
					tipoantestesia = lector["tipo_anestesia"].ToString().Trim();
					llenado_combobox(1,tipoantestesia,combobox_tipo_anestecia,"array","","","",args_tipoanestesia,args_id_array,"");
					entry_id_anestesiologo.Text = lector["id_anestesiologo"].ToString().Trim();
					entry_anestesiologo.Text = lector["nombre_anestesiologo"].ToString().Trim();
					entry_observacion_qx.Text = lector["observaciones"].ToString().Trim();
					entry_id_cirujano2.Text = lector["id_ayudante"].ToString().Trim();
					entry_nombre_cirujano2.Text = lector["nombre_ayudante"].ToString().Trim();
					entry_id_cirujano1.Text = lector["id_cirujano"].ToString().Trim();
					entry_nombre_cirujano1.Text = lector["nombre_cirujano"].ToString().Trim();
					entry_cirugia_paseqx.Text = lector["descripcion_cirugia"].ToString().Trim();
					idcxpaseqx = int.Parse(lector["id_cirugia"].ToString().Trim());
					entry_fechapaseqx.Text = lector["fechapaseqx"].ToString().Trim();
				}
				
			}catch (NpgsqlException ex){
	   			Console.WriteLine ("PostgresSQL error: {0}",ex.Message);
	       	}
       		conexion.Close ();
		}
		
		// busco un paciente pantalla de ingreso de nuevo paciente
		void on_button_buscar_paciente_clicked(object sender, EventArgs args)
	    	{
			Glade.XML gxml = new Glade.XML (null, "impr_documentos.glade", "busca_paciente", null);
			gxml.Autoconnect (this);
			crea_treeview_busqueda("paciente","");
			this.entry_expresion.KeyPressEvent += onKeyPressEvent_expresion;
			button_buscar_busqueda.Clicked += new EventHandler(on_buscar_paciente_clicked);
			button_selecciona.Clicked += new EventHandler(on_selecciona_paciente_clicked);
			button_nuevo_paciente.Sensitive = false;
			button_salir.Clicked += new EventHandler(on_cierraventanas_clicked); // esta sub-clase esta en hscmty.cs
		}
	    
		void crea_treeview_busqueda(string tipo_busqueda,string subbusqueda)
		{
			if ((string) tipo_busqueda == "paciente"){
				treeViewEngineBusca = new TreeStore(typeof(int),typeof(int),typeof(string),typeof(string),typeof(string),typeof(string),typeof(string),
												typeof(string),typeof(string),typeof(string) );
				lista_de_Pacientes.Model = treeViewEngineBusca;
			
				lista_de_Pacientes.RulesHint = true;
			
				lista_de_Pacientes.RowActivated += on_selecciona_paciente_clicked;  // Doble click selecciono paciente*/

				TreeViewColumn col_foliodeatencion = new TreeViewColumn();
				CellRendererText cellr0 = new CellRendererText();
				col_foliodeatencion.Title = "Folio de Antencion"; // titulo de la cabecera de la columna, si está visible
				col_foliodeatencion.PackStart(cellr0, true);
				col_foliodeatencion.AddAttribute (cellr0, "text", 0);    // la siguiente columna será 1 en vez de 1
				col_foliodeatencion.SortColumnId = (int) Column.col_foliodeatencion;
			
				TreeViewColumn col_PidPaciente = new TreeViewColumn();
				CellRendererText cellr1 = new CellRendererText();
				col_PidPaciente.Title = "PID Paciente"; // titulo de la cabecera de la columna, si está visible
				col_PidPaciente.PackStart(cellr1, true);
				col_PidPaciente.AddAttribute (cellr1, "text", 1);    // la siguiente columna será 1 en vez de 1
				col_PidPaciente.SortColumnId = (int) Column.col_PidPaciente;
				//cellr0.Editable = true;   // Permite edita este campo
            
				TreeViewColumn col_Nombre1_Paciente = new TreeViewColumn();
				CellRendererText cellrt2 = new CellRendererText();
				col_Nombre1_Paciente.Title = "Nombre 1";
				col_Nombre1_Paciente.PackStart(cellrt2, true);
				col_Nombre1_Paciente.AddAttribute (cellrt2, "text", 2); // la siguiente columna será 1 en vez de 2
				col_Nombre1_Paciente.SortColumnId = (int) Column.col_Nombre1_Paciente;
            
				TreeViewColumn col_Nombre2_Paciente = new TreeViewColumn();
				CellRendererText cellrt3 = new CellRendererText();
				col_Nombre2_Paciente.Title = "Nombre 2";
				col_Nombre2_Paciente.PackStart(cellrt3, true);
				col_Nombre2_Paciente.AddAttribute (cellrt3, "text", 3); // la siguiente columna será 2 en vez de 3
				col_Nombre2_Paciente.SortColumnId = (int) Column.col_Nombre2_Paciente;
            
				TreeViewColumn col_app_Paciente = new TreeViewColumn();
				CellRendererText cellrt4 = new CellRendererText();
				col_app_Paciente.Title = "Apellido Paterno";
				col_app_Paciente.PackStart(cellrt4, true);
				col_app_Paciente.AddAttribute (cellrt4, "text", 4); // la siguiente columna será 3 en vez de 4
				col_app_Paciente.SortColumnId = (int) Column.col_app_Paciente;
            
				TreeViewColumn col_apm_Paciente = new TreeViewColumn();
				CellRendererText cellrt5 = new CellRendererText();
				col_apm_Paciente.Title = "Apellido Materno";
				col_apm_Paciente.PackStart(cellrt5, true);
				col_apm_Paciente.AddAttribute (cellrt5, "text", 5); // la siguiente columna será 5 en vez de 6
				col_apm_Paciente.SortColumnId = (int) Column.col_apm_Paciente;
      
				TreeViewColumn col_fechanacimiento_Paciente = new TreeViewColumn();
				CellRendererText cellrt6 = new CellRendererText();
				col_fechanacimiento_Paciente.Title = "Fecha Nacimiento";
				col_fechanacimiento_Paciente.PackStart(cellrt6, true);
				col_fechanacimiento_Paciente.AddAttribute (cellrt6, "text", 6);     // la siguiente columna será 6 en vez de 7
				col_fechanacimiento_Paciente.SortColumnId = (int) Column.col_fechanacimiento_Paciente;
            
				TreeViewColumn col_edad_Paciente = new TreeViewColumn();
				CellRendererText cellrt7 = new CellRendererText();
				col_edad_Paciente.Title = "Edad";
				col_edad_Paciente.PackStart(cellrt7, true);
				col_edad_Paciente.AddAttribute (cellrt7, "text", 7); // la siguiente columna será 7 en vez de 8
				col_edad_Paciente.SortColumnId = (int) Column.col_edad_Paciente;
            
				TreeViewColumn col_sexo_Paciente = new TreeViewColumn();
				CellRendererText cellrt8 = new CellRendererText();
				col_sexo_Paciente.Title = "Sexo";
				col_sexo_Paciente.PackStart(cellrt8, true);
				col_sexo_Paciente.AddAttribute (cellrt8, "text", 8); // la siguiente columna será 8 en vez de 9
				col_sexo_Paciente.SortColumnId = (int) Column.col_sexo_Paciente;
                        
				TreeViewColumn col_creacion_Paciente = new TreeViewColumn();
				CellRendererText cellrt9 = new CellRendererText();
				col_creacion_Paciente.Title = "Fecha creacion";
				col_creacion_Paciente.PackStart(cellrt9, true);
				col_creacion_Paciente.AddAttribute (cellrt9, "text", 9); // la siguiente columna será 8 en vez de 9
				col_creacion_Paciente.SortColumnId = (int) Column.col_creacion_Paciente;

				lista_de_Pacientes.AppendColumn(col_foliodeatencion);
				lista_de_Pacientes.AppendColumn(col_PidPaciente);
				lista_de_Pacientes.AppendColumn(col_Nombre1_Paciente);
				lista_de_Pacientes.AppendColumn(col_Nombre2_Paciente);
				lista_de_Pacientes.AppendColumn(col_app_Paciente);
				lista_de_Pacientes.AppendColumn(col_apm_Paciente);
				lista_de_Pacientes.AppendColumn(col_fechanacimiento_Paciente);
				lista_de_Pacientes.AppendColumn(col_edad_Paciente);
				lista_de_Pacientes.AppendColumn(col_sexo_Paciente);
				lista_de_Pacientes.AppendColumn(col_creacion_Paciente);
			}
			if((string) tipo_busqueda == "medicos"){
				treeViewEngineMedicos = new TreeStore(typeof(int),typeof(string),typeof(string),typeof(string),typeof(string),typeof(string),
													typeof(string),typeof(string),typeof(string),typeof(string),typeof(string),typeof(string),
													typeof(string),typeof(string),typeof(string),typeof(string),typeof(string),typeof(string),
													typeof(string),typeof(string),typeof(bool),typeof(bool),typeof(bool),typeof(bool),
													typeof(bool),typeof(bool),typeof(bool),typeof(bool),typeof(bool),typeof(bool),
													typeof(bool),typeof(bool),typeof(bool),typeof(bool),typeof(bool),typeof(bool),typeof(bool));
				lista_de_medicos.Model = treeViewEngineMedicos;
				lista_de_medicos.RulesHint = true;
			
				if(subbusqueda == "anestesiologo"){
					lista_de_medicos.RowActivated += on_selecciona_anestesiologo_clicked;
				}
				if(subbusqueda == "cirujano1"){
					lista_de_medicos.RowActivated += on_selecciona_cirujano1_clicked;
				}
				if(subbusqueda == "cirujano2"){
					lista_de_medicos.RowActivated += on_selecciona_cirujano2_clicked;
				}
				if(subbusqueda == ""){
					lista_de_medicos.RowActivated += on_selecciona_medico_clicked;
				}
								
				TreeViewColumn col_idmedico = new TreeViewColumn();
				CellRendererText cellr0 = new CellRendererText();
				col_idmedico.Title = "ID Medico"; // titulo de la cabecera de la columna, si está visible
				col_idmedico.PackStart(cellr0, true);
				col_idmedico.AddAttribute (cellr0, "text", 0);
				col_idmedico.SortColumnId = (int) Coldatos_medicos.col_idmedico;    
	            
				TreeViewColumn col_nomb1medico = new TreeViewColumn();
				CellRendererText cellrt1 = new CellRendererText();
				col_nomb1medico.Title = "1º Nombre";
				col_nomb1medico.PackStart(cellrt1, true);
				col_nomb1medico.AddAttribute (cellrt1, "text", 1);
				col_nomb1medico.SortColumnId = (int) Coldatos_medicos.col_nomb1medico; 
	            
	            TreeViewColumn col_nomb2medico = new TreeViewColumn();
				CellRendererText cellrt2 = new CellRendererText();
				col_nomb2medico.Title = "2º Nombre";
				col_nomb2medico.PackStart(cellrt2, true);
				col_nomb2medico.AddAttribute (cellrt2, "text", 2);
				col_nomb2medico.SortColumnId = (int) Coldatos_medicos.col_nomb2medico; 
				
				TreeViewColumn col_appmedico = new TreeViewColumn();
				CellRendererText cellrt3 = new CellRendererText();
				col_appmedico.Title = "Apellido Paterno";
				col_appmedico.PackStart(cellrt3, true);
				col_appmedico.AddAttribute (cellrt3, "text", 3);
				col_appmedico.SortColumnId = (int) Coldatos_medicos.col_appmedico;
				
				TreeViewColumn col_apmmedico = new TreeViewColumn();
				CellRendererText cellrt4 = new CellRendererText();
				col_apmmedico.Title = "Apellido Materno";
				col_apmmedico.PackStart(cellrt4, true);
				col_apmmedico.AddAttribute (cellrt4, "text", 4);
				col_apmmedico.SortColumnId = (int) Coldatos_medicos.col_apmmedico;
	            
				TreeViewColumn col_espemedico = new TreeViewColumn();
				CellRendererText cellrt5 = new CellRendererText();
				col_espemedico.Title = "Especialidad";
				col_espemedico.PackStart(cellrt5, true);
				col_espemedico.AddAttribute (cellrt5, "text", 5);
				col_espemedico.SortColumnId = (int) Coldatos_medicos.col_espemedico;
	            
				TreeViewColumn col_telmedico = new TreeViewColumn();
				CellRendererText cellrt6 = new CellRendererText();
				col_telmedico.Title = "Cedula Medica";
				col_telmedico.PackStart(cellrt6, true);
				col_telmedico.AddAttribute (cellrt6, "text", 6); 
				col_telmedico.SortColumnId = (int) Coldatos_medicos.col_telmedico;
	            
				TreeViewColumn col_cedulamedico = new TreeViewColumn();
				CellRendererText cellrt7 = new CellRendererText();
				col_cedulamedico.Title = "Telefono Casa";
				col_cedulamedico.PackStart(cellrt7, true);
				col_cedulamedico.AddAttribute (cellrt7, "text", 7); 
				col_cedulamedico.SortColumnId = (int) Coldatos_medicos.col_cedulamedico;
				
				TreeViewColumn col_telOfmedico = new TreeViewColumn();
				CellRendererText cellrt8 = new CellRendererText();
				col_telOfmedico.Title = "Telefono Oficina";
				col_telOfmedico.PackStart(cellrt8, true);
				col_telOfmedico.AddAttribute (cellrt8, "text", 8);
				col_telOfmedico.SortColumnId = (int) Coldatos_medicos.col_telOfmedico; 
				
				TreeViewColumn col_celmedico = new TreeViewColumn();
				CellRendererText cellrt9 = new CellRendererText();
				col_celmedico.Title = "Celular 1";
				col_celmedico.PackStart(cellrt9, true);
				col_celmedico.AddAttribute (cellrt9, "text", 9); 
				col_celmedico.SortColumnId = (int) Coldatos_medicos.col_celmedico;
				
				TreeViewColumn col_celmedico2 = new TreeViewColumn();
				CellRendererText cellrt10 = new CellRendererText();
				col_celmedico2.Title = "Celular 2";
				col_celmedico2.PackStart(cellrt10, true);
				col_celmedico2.AddAttribute (cellrt10, "text", 10);
				col_celmedico2.SortColumnId = (int) Coldatos_medicos.col_celmedico2;
										
				TreeViewColumn col_nextelmedico = new TreeViewColumn();
				CellRendererText cellrt11 = new CellRendererText();
				col_nextelmedico.Title = "Nextel";
				col_nextelmedico.PackStart(cellrt11, true);
				col_nextelmedico.AddAttribute (cellrt11, "text", 11);
				col_nextelmedico.SortColumnId = (int) Coldatos_medicos.col_nextelmedico;
				
				TreeViewColumn col_beepermedico = new TreeViewColumn();
				CellRendererText cellrt12 = new CellRendererText();
				col_beepermedico.Title = "Beeper";
				col_beepermedico.PackStart(cellrt12, true);
				col_beepermedico.AddAttribute (cellrt12, "text", 12);
				col_beepermedico.SortColumnId = (int) Coldatos_medicos.col_beepermedico;
				
				TreeViewColumn col_empresamedico = new TreeViewColumn();
				CellRendererText cellrt13 = new CellRendererText();
				col_empresamedico.Title = "Empresa";
				col_empresamedico.PackStart(cellrt13, true);
				col_empresamedico.AddAttribute (cellrt13, "text", 13);
				col_empresamedico.SortColumnId = (int) Coldatos_medicos.col_empresamedico;
				                        
				lista_de_medicos.AppendColumn(col_idmedico);
				lista_de_medicos.AppendColumn(col_nomb1medico);
				lista_de_medicos.AppendColumn(col_nomb2medico);
				lista_de_medicos.AppendColumn(col_appmedico);
				lista_de_medicos.AppendColumn(col_apmmedico);
				lista_de_medicos.AppendColumn(col_espemedico);
				lista_de_medicos.AppendColumn(col_cedulamedico);
				lista_de_medicos.AppendColumn(col_telmedico);
				lista_de_medicos.AppendColumn(col_telOfmedico);
				lista_de_medicos.AppendColumn(col_celmedico);
				lista_de_medicos.AppendColumn(col_celmedico2);
				lista_de_medicos.AppendColumn(col_nextelmedico);
				lista_de_medicos.AppendColumn(col_beepermedico);
				lista_de_medicos.AppendColumn(col_empresamedico);
			}
			if((string) tipo_busqueda == "cirugia"){
				treeViewEngineBusca = new TreeStore(typeof(int),typeof(string),typeof(string));
				lista_cirugia.Model = treeViewEngineBusca;
				
				lista_cirugia.RulesHint = true;
				
				//on_selecciona_cxpaseqx_clicked
				
				if(subbusqueda == ""){
					lista_cirugia.RowActivated += on_selecciona_cirugia_clicked;  // Doble click selecciono paciente*/
				}
				if(subbusqueda == "paseqx"){
					lista_cirugia.RowActivated += on_selecciona_cxpaseqx_clicked;
				}
				
				TreeViewColumn col_idcirugia = new TreeViewColumn();
				CellRendererText cellr0 = new CellRendererText();
				col_idcirugia.Title = "ID Cirugia"; // titulo de la cabecera de la columna, si está visible
				col_idcirugia.PackStart(cellr0, true);
				col_idcirugia.AddAttribute (cellr0, "text", 0);    // la siguiente columna será 1 en vez de 1
				col_idcirugia.SortColumnId = (int) Column3.col_idcirugia;
				//cellr0.Editable = true;   // Permite edita este campo
	            
				TreeViewColumn col_descripcion_cirugia = new TreeViewColumn();
				CellRendererText cellrt1 = new CellRendererText();
				col_descripcion_cirugia.Title = "Descripcion de cirugia";
				col_descripcion_cirugia.PackStart(cellrt1, true);
				col_descripcion_cirugia.AddAttribute (cellrt1, "text", 1); // la siguiente columna será 1 en vez de 2
				col_descripcion_cirugia.SortColumnId = (int) Column3.col_descripcion_cirugia;
	            
				TreeViewColumn col_tiene_paquete = new TreeViewColumn();
				CellRendererText cellrt2 = new CellRendererText();
				col_tiene_paquete.Title = "Tiene paquete";
				col_tiene_paquete.PackStart(cellrt2, true);
				col_tiene_paquete.AddAttribute (cellrt2, "text", 2); // la siguiente columna será 2 en vez de 3
				col_tiene_paquete.SortColumnId = (int) Column3.col_tiene_paquete;
	            
			    lista_cirugia.AppendColumn(col_idcirugia);
				lista_cirugia.AppendColumn(col_descripcion_cirugia);
				lista_cirugia.AppendColumn(col_tiene_paquete);
			}
		}
			
		enum Column
		{
			col_foliodeatencion,
			col_PidPaciente,
			col_Nombre1_Paciente,
			col_Nombre2_Paciente,
			col_app_Paciente,
			col_apm_Paciente,
			col_fechanacimiento_Paciente,
			col_edad_Paciente,
			col_sexo_Paciente,
			col_creacion_Paciente,
			
		}
		
		enum Coldatos_medicos
		{
			col_idmedico,
			col_nomb1medico,
			col_nomb2medico,
			col_appmedico,
			col_apmmedico,
			col_espemedico,
			col_cedulamedico,
			col_telmedico,
			col_telOfmedico,
			col_celmedico,
			col_celmedico2,
			col_nextelmedico,
			col_beepermedico,
			col_empresamedico
		}
		
		enum Column3
		{
			col_idcirugia,
			col_descripcion_cirugia,
			col_tiene_paquete,
		}
				
		// activa busqueda con boton busqueda de paciente
		// y llena la lista con los pacientes		
		void on_buscar_paciente_clicked (object sender, EventArgs args)
		{
			buscando_paciente();
		}
		void buscando_paciente()
		{
			treeViewEngineBusca.Clear(); // Limpia el treeview cuando realiza una nueva busqueda			
			NpgsqlConnection conexion; 
			conexion = new NpgsqlConnection (connectionString+nombrebd);            
			// Verifica que la base de datos este conectada
			try{
				conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand ();               	               	
				if (radiobutton_busca_apellido.Active == true){
					comando.CommandText = "SELECT osiris_erp_cobros_enca.folio_de_servicio,osiris_his_paciente.pid_paciente, nombre1_paciente,nombre2_paciente, apellido_paterno_paciente,"+
										"apellido_materno_paciente, to_char(fecha_nacimiento_paciente,'yyyy-MM-dd') AS fech_nacimiento,"+
										"to_char(to_number(to_char(age('"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"',fecha_nacimiento_paciente),'yyyy') ,'9999'),'9999') AS edad, sexo_paciente,"+
										"to_char(fechahora_registro_paciente,'dd-MM-yyyy HH:mi:ss') AS fech_creacion "+
										"FROM osiris_his_paciente,osiris_erp_cobros_enca "+
										"WHERE osiris_his_paciente.pid_paciente = osiris_erp_cobros_enca.pid_paciente "+ 
										"AND osiris_erp_cobros_enca.pagado = 'false' "+
										"AND osiris_erp_cobros_enca.cancelado = 'false' "+
										"AND apellido_paterno_paciente LIKE '"+entry_expresion.Text.ToUpper()+"%' ORDER BY pid_paciente;";
				}
				if (radiobutton_busca_nombre.Active == true){
					comando.CommandText = "SELECT osiris_erp_cobros_enca.folio_de_servicio,osiris_erp_cobros_enca.pid_paciente, nombre1_paciente,nombre2_paciente, apellido_paterno_paciente,"+
										"apellido_materno_paciente, to_char(fecha_nacimiento_paciente,'yyyy-MM-dd') AS fech_nacimiento,"+
										"to_char(to_number(to_char(age('"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"',fecha_nacimiento_paciente),'yyyy') ,'9999'),'9999') AS edad, sexo_paciente,"+
										"to_char(fechahora_registro_paciente,'dd-MM-yyyy HH:mi:ss') AS fech_creacion "+
										"FROM osiris_his_paciente,osiris_erp_cobros_enca "+
										"WHERE osiris_his_paciente.pid_paciente = osiris_erp_cobros_enca.pid_paciente "+ 
										"AND osiris_erp_cobros_enca.pagado = 'false' "+ 
										"AND osiris_erp_cobros_enca.cancelado = 'false' "+
										"AND nombre1_paciente LIKE '"+entry_expresion.Text.ToUpper()+"%' ORDER BY osiris_erp_cobros_enca.pid_paciente;";
				}
				if (radiobutton_busca_expediente.Active == true)
				{
					comando.CommandText = "SELECT osiris_erp_cobros_enca.folio_de_servicio,osiris_erp_cobros_enca.pid_paciente, nombre1_paciente,nombre2_paciente, apellido_paterno_paciente,"+
										"apellido_materno_paciente, to_char(fecha_nacimiento_paciente,'yyyy-MM-dd') AS fech_nacimiento,"+
										"to_char(to_number(to_char(age('"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"',fecha_nacimiento_paciente),'yyyy') ,'9999'),'9999') AS edad, sexo_paciente,"+
										"to_char(fechahora_registro_paciente,'dd-MM-yyyy HH:mi:ss') AS fech_creacion "+
										"FROM osiris_his_paciente,osiris_erp_cobros_enca "+
										"WHERE osiris_his_paciente.pid_paciente = osiris_erp_cobros_enca.pid_paciente "+ 
										"AND osiris_erp_cobros_enca.pagado = 'false' "+
										"AND osiris_erp_cobros_enca.cancelado = 'false' "+
										"AND osiris_erp_cobros_enca.pid_paciente = '"+entry_expresion.Text+"' ORDER BY osiris_erp_cobros_enca.pid_paciente;";					
				}
				if ((string) entry_expresion.Text.ToString() == "*"){
					comando.CommandText = "SELECT osiris_erp_cobros_enca.folio_de_servicio,osiris_erp_cobros_enca.pid_paciente, nombre1_paciente,nombre2_paciente, apellido_paterno_paciente,"+
										"apellido_materno_paciente, to_char(fecha_nacimiento_paciente,'yyyy-MM-dd') AS fech_nacimiento,"+
										"to_char(to_number(to_char(age('"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"',fecha_nacimiento_paciente),'yyyy') ,'9999'),'9999') AS edad, sexo_paciente,"+
										"to_char(fechahora_registro_paciente,'dd-MM-yyyy HH:mi:ss') AS fech_creacion "+
										"FROM osiris_his_paciente,osiris_erp_cobros_enca "+
										"WHERE osiris_his_paciente.pid_paciente = osiris_erp_cobros_enca.pid_paciente "+ 
										"AND osiris_erp_cobros_enca.pagado = 'false' "+
										"AND osiris_erp_cobros_enca.cancelado = 'false' "+
										"ORDER BY osiris_erp_cobros_enca.pid_paciente;";
				}
				NpgsqlDataReader lector = comando.ExecuteReader ();
				while (lector.Read()){
					treeViewEngineBusca.AppendValues ((int) lector["folio_de_servicio"],//TreeIter iter =
										(int) lector["pid_paciente"],
										(string) lector["nombre1_paciente"],(string) lector["nombre2_paciente"],
										(string) lector["apellido_paterno_paciente"], (string) lector["apellido_materno_paciente"],
										(string) lector["fech_nacimiento"], (string) lector["edad"],
										(string) lector["sexo_paciente"],
										(string) lector["fech_creacion"]);
				}
				
				
			}catch (NpgsqlException ex){
	   			Console.WriteLine ("PostgresSQL error: {0}",ex.Message);
			}
			conexion.Close ();
		}
					
		void on_selecciona_paciente_clicked(object sender, EventArgs args)
		{
			TreeModel model;
			TreeIter iterSelected;

 			if (lista_de_Pacientes.Selection.GetSelected(out model, out iterSelected)) 
 			{
 				 folioservicio = (int) model.GetValue(iterSelected, 0);
 				 entry_folio_servicio.Text = folioservicio.ToString();
 				 llenado_de_datos_paciente(folioservicio.ToString());
 			}
 			// cierra la ventana despues que almaceno la informacion en variables
			Widget win = (Widget) sender;
			win.Toplevel.Destroy();
 		}
 		
		// Valida entradas que solo sean numericas, se utiliza eb ventana de
		// carga de producto
		[GLib.ConnectBefore ()]   	  // Esto es indispensable para que funcione    
		public void onKeyPressEvent_enter_folio(object o, Gtk.KeyPressEventArgs args)
		{
			//Console.WriteLine(args.Event.Key);
			if (args.Event.Key.ToString() == "Return" || args.Event.Key.ToString() == "KP_Enter"){
				args.RetVal = true;
				llenado_de_datos_paciente( (string) entry_folio_servicio.Text );				
			}
			string misDigitos = ".0123456789ﾰﾱﾲﾳﾴﾵﾶﾷﾸﾹﾮｔｒｓｑ（）";
			if (Array.IndexOf(misDigitos.ToCharArray(), Convert.ToChar(args.Event.Key)) == -1 && args.Event.Key.ToString()  != "BackSpace")
			{
				args.RetVal = true;
			}
		}
 		
 		[GLib.ConnectBefore ()]   	  // Esto es indispensable para que funcione    
		public void onKeyPressEvent_expresion(object o, Gtk.KeyPressEventArgs args)
		{
			//Console.WriteLine(args.Event.Key);
			if (args.Event.Key.ToString() == "Return" || args.Event.Key.ToString() == "KP_Enter"){
				args.RetVal = true;
				this.buscando_paciente();				
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