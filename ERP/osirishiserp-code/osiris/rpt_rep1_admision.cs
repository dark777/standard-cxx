// created on 07/02/2008 at 09:34 a
// Sistema Hospitalario OSIRIS
// Monterrey - Mexico
//
// Autor    	: Ing. Daniel Olivares C. (Adecuaciones y mejoras) arcangeldoc@openmailbox.org 03/06/2010
//				  Traspaso a GTKprint y la creacion de la clase
//				  Traspaso a iTextSharp Ago. 2016 y se crea una clase para cada reporte
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
//////////////////////////////////////////////////////

using System;
using Gtk;
using Npgsql;
using Glade;
using Cairo;
using Pango;

namespace osiris
{
	public class rptAdmision
	{
		[Widget] Gtk.Window rango_rep_adm = null;
		// Boton general para salir de las ventanas
		// Todas la ventanas en glade este boton debe estra declarado identico
		[Widget] Gtk.Button button_salir = null;
		// Entradas y botones de la ventana
		[Widget] Gtk.Entry entry_dia_inicial = null;
		[Widget] Gtk.Entry entry_mes_inicial = null;
		[Widget] Gtk.Entry entry_ano_inicial = null;
		[Widget] Gtk.Entry entry_dia_final = null;
		[Widget] Gtk.Entry entry_mes_final = null;
		[Widget] Gtk.Entry entry_ano_final = null;
		[Widget] Gtk.Entry entry_id_empaseg_cita = null;
		[Widget] Gtk.Entry entry_nombre_empaseg_cita = null;
		[Widget] Gtk.Entry entry_id_doctor_consulta = null;
		[Widget] Gtk.Entry entry_nombre_doctor_consulta = null;
		// ComboBox
		[Widget] Gtk.ComboBox combobox_tipo_admision = null;
		[Widget] Gtk.ComboBox combobox_tipo_paciente = null;
		// CheckButtons
		[Widget] Gtk.CheckButton checkbutton_todas_fechas = null;
		[Widget] Gtk.CheckButton checkbutton_todos_admision = null;
		[Widget] Gtk.CheckButton checkbutton_todos_paciente = null;
		[Widget] Gtk.CheckButton checkbutton_todas_empresas = null;
		[Widget] Gtk.CheckButton checkbutton_todos_doctores = null;
		[Widget] Gtk.CheckButton checkbutton_primera_vez = null;
		[Widget] Gtk.CheckButton checkbutton_status = null;
		[Widget] Gtk.CheckButton checkbutton_all_rpt = null;
		[Widget] Gtk.CheckButton checkbutton_export_ods = null;
		[Widget] Gtk.CheckButton checkbutton_empraseg = null;

		[Widget] Gtk.RadioButton radiobutton_detallado = null;
		[Widget] Gtk.RadioButton radiobutton_total_grupo = null;
		[Widget] Gtk.RadioButton radiobutton_total = null;
		[Widget] Gtk.RadioButton radiobutton_masculino = null;
		[Widget] Gtk.RadioButton radiobutton_femenino = null;
		[Widget] Gtk.RadioButton radiobutton_ambos_sexos = null;
		[Widget] Gtk.RadioButton radiobutton_cancelados = null;
		[Widget] Gtk.RadioButton radiobutton_no_cancelados = null;
		[Widget] Gtk.RadioButton radiobutton_reporte_general = null;
		[Widget] Gtk.RadioButton radiobutton_folio_servicio = null;
		[Widget] Gtk.RadioButton radiobutton_pid_paciente = null;
		[Widget] Gtk.RadioButton radiobutton_nombres = null;
		[Widget] Gtk.RadioButton radiobutton_doctores = null;
		[Widget] Gtk.RadioButton radiobutton_tipo_admision = null;
				
		[Widget] Gtk.Button button_busca_empresa = null;
		[Widget] Gtk.Button button_busca_doctores = null;
		[Widget] Gtk.Button button_imprimir = null;
			
		protected Gtk.Window MyWinError;
		
		private static int pangoScale = 1024;
		private PrintOperation print;
		private double fontSize = 8.0;
		PrintContext context;
		
		int escala_en_linux_windows;		// Linux = 1  Windows = 8
		int comienzo_linea = 95;
		int separacion_linea = 9;
		int numerpage = 1;
		string connectionString;
        string nombrebd;
	    string tipointernamiento = "";
   		int idtipointernamiento = 10;
   	    string tipopaciente = ""; 
		int id_tipopaciente = 100;
								
    	string query_reporte = "SELECT "+
				"osiris_erp_movcargos.id_tipo_admisiones AS idtipoadmisiones,osiris_his_tipo_admisiones.descripcion_admisiones, "+
				"osiris_erp_movcargos.folio_de_servicio,osiris_erp_movcargos.folio_de_servicio_dep,osiris_erp_cobros_enca.cancelado, "+
				"to_char(fechahora_admision_registro,'yyyy-MM-dd') AS fech_reg_adm,alta_paciente,"+ 
				"fechahora_admision_registro,osiris_erp_movcargos.id_tipo_paciente,descripcion_tipo_paciente,"+
				"osiris_erp_movcargos.pid_paciente,nombre1_paciente,nombre2_paciente,apellido_paterno_paciente,apellido_materno_paciente, "+
				"osiris_his_paciente.nombre1_paciente || ' ' || osiris_his_paciente.nombre2_paciente || ' ' || osiris_his_paciente.apellido_paterno_paciente || ' ' || osiris_his_paciente.apellido_materno_paciente AS nombre_completo,"+
				"nombre1_medico,nombre2_medico,apellido_paterno_medico,apellido_materno_medico,grupo_sanguineo_paciente,to_char(fechahora_admision_registro,'HH24:mi') AS hora_reg_adm,"+ 
				"to_char(to_number(to_char(age('"+DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss")+"',osiris_his_paciente.fecha_nacimiento_paciente),'yyyy'),'9999'),'9999') AS edad,"+
				"to_char(to_number(to_char(age('"+DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss")+"',osiris_his_paciente.fecha_nacimiento_paciente),'MM'),'99'),'99') AS mesesedad,"+
				"direccion_paciente,numero_casa_paciente,codigo_postal_paciente,estado_civil_paciente,osiris_erp_cobros_enca.responsable_cuenta,"+
				"colonia_paciente,numero_departamento_paciente,ocupacion_paciente,sexo_paciente,"+
				"to_char(fecha_nacimiento_paciente,'dd-MM-yyyy') AS fech_nacimiento,"+
				"id_empleado_admision," +
				"osiris_erp_cobros_enca.id_aseguradora,descripcion_aseguradora,"+
				"osiris_erp_cobros_enca.id_empresa,descripcion_empresa,public.osiris_erp_cobros_enca.id_medico,osiris_erp_movcargos.descripcion_diagnostico_movcargos, osiris_erp_cobros_enca.motivo_cancelacion,"+
				"numero_certificado,numero_poliza,osiris_erp_cobros_enca.numero_factura,"+
				//"sub_total_15,sub_total_0,iva_al_15,osiris_erp_factura_enca.honorario_medico,"+
				"historial_facturados,total_procedimiento,tipo_cirugia,osiris_erp_movcargos.vista_primera_vez,"+
				"osiris_his_medicos.nombre_medico,osiris_his_tipo_cirugias.id_tipo_cirugia, descripcion_cirugia, empresa_labora_responsable,"+				
				"nombre_medico_encabezado,"+
				"id_medico_tratante,nombre_medico_tratante,osiris_erp_movcargos.descripcion_diagnostico_cie10 "+
				"FROM "+
				"osiris_erp_movcargos,osiris_his_paciente,osiris_his_tipo_pacientes,osiris_his_tipo_admisiones,osiris_erp_cobros_enca,osiris_aseguradoras,osiris_empresas, "+ 
				"osiris_his_tipo_cirugias,osiris_his_medicos "+   //,osiris_erp_factura_enca "+
				"WHERE  "+
				"osiris_erp_movcargos.pid_paciente = osiris_his_paciente.pid_paciente  "+
				"AND osiris_erp_cobros_enca.pid_paciente = osiris_his_paciente.pid_paciente  "+
				"AND osiris_erp_cobros_enca.folio_de_servicio = osiris_erp_movcargos.folio_de_servicio  "+
				"AND osiris_erp_movcargos.id_tipo_paciente = osiris_his_tipo_pacientes.id_tipo_paciente  "+
				"AND osiris_erp_movcargos.id_tipo_admisiones = osiris_his_tipo_admisiones.id_tipo_admisiones  "+
				"AND osiris_aseguradoras.id_aseguradora = osiris_erp_cobros_enca.id_aseguradora "+
				"AND osiris_empresas.id_empresa = osiris_erp_cobros_enca.id_empresa "+
				//"AND osiris_empresas.id_empresa = osiris_his_paciente.id_empresa "+  // enlase empresa con el paciente
				//"AND osiris_erp_factura_enca.numero_factura = osiris_erp_cobros_enca.numero_factura "+
				//"AND osiris_empresas.id_empresa = osiris_his_medicos.id_empresa "+
				//"AND osiris_empresas.id_empresa = 3 "+//se aactiva para cuando se quiera ver san nicolas
				"AND osiris_erp_movcargos.id_tipo_cirugia = osiris_his_tipo_cirugias.id_tipo_cirugia "+
				"AND osiris_erp_cobros_enca.id_medico = osiris_his_medicos.id_medico ";
    	string query_tipo_admision  = "AND osiris_erp_movcargos.id_tipo_admisiones = '0' ";
		string query_tipo_paciente = "AND osiris_erp_movcargos.id_tipo_paciente = '200' ";
    	string query_rango_fechas = "AND to_char(osiris_erp_cobros_enca.fechahora_creacion,'yyyy-MM-dd') >= '"+DateTime.Now.ToString("yyyy")+"-"+DateTime.Now.ToString("MM")+"-"+DateTime.Now.ToString("dd")+"' "+
										"AND to_char(osiris_erp_cobros_enca.fechahora_creacion,'yyyy-MM-dd') <= '"+DateTime.Now.ToString("yyyy")+"-"+DateTime.Now.ToString("MM")+"-"+DateTime.Now.ToString("dd")+"' "; 
    	/*query_rango_fechas = "AND to_number(to_char(osiris_erp_movcargos.fechahora_admision_registro,'yyyy'),9999) >= '"+DateTime.Now.ToString("yyyy")+"' AND to_number(to_char(osiris_erp_movcargos.fechahora_admision_registro,'yyyy'),9999) <= '"+DateTime.Now.ToString("yyyy")+"' "+  
    									"AND to_number(to_char(osiris_erp_movcargos.fechahora_admision_registro,'MM'),99) >= '"+DateTime.Now.ToString("MM")+"' AND to_number(to_char(osiris_erp_movcargos.fechahora_admision_registro,'MM'),99) <= '"+DateTime.Now.ToString("MM")+"' "+
    									"AND to_number(to_char(osiris_erp_movcargos.fechahora_admision_registro,'dd'),99) >= '"+DateTime.Now.ToString("dd")+"'  AND to_number(to_char(osiris_erp_movcargos.fechahora_admision_registro,'dd'),99) <= '"+DateTime.Now.ToString("dd")+"' " ;
		*/
		string query_sexo = " "; 
    	string query_empresa = " "; //"AND osiris_erp_cobros_enca.id_empresa = 3 "; 
    	string query_aseguradora = " ";
    	string query_tipo_reporte = " ";
    	string query_medico = " ";
		string query_orden = "ORDER BY osiris_erp_movcargos.folio_de_servicio;";
		string query_primeravez = " ";
		
		string tipo_de_salida = "";
		string tiporeporte = "";
		
		string idempresa = "1";
		string idaseguradora = "1";
		string idmedico = "1";

		string[] args_args = {""};
		int[] args_id_array = {0,1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20};

		class_conexion conexion_a_DB = new class_conexion();
		class_buscador classfind_data = new class_buscador();
		class_public classpublic = new class_public();
                               
		/// <summary>
		/// Genera reporte inteligente de consulta
		/// </summary>
		/// <param name="nombrebd_">
		/// A <see cref="System.String"/>
		/// </param>
		/// <param name="tipo_reporte impresora o archivo">
		/// A <see cref="System.String"/>
		/// </param>
		public rptAdmision (string nombrebd_,string tipo_de_salida_,string tiporeporte_)
		{
			connectionString = conexion_a_DB._url_servidor+conexion_a_DB._port_DB+conexion_a_DB._usuario_DB+conexion_a_DB._passwrd_user_DB;
			nombrebd = conexion_a_DB._nombrebd;
			escala_en_linux_windows = classpublic.escala_linux_windows;
			tipo_de_salida = tipo_de_salida_;
			tiporeporte = tiporeporte_;
			//crea la ventana de glade
			Glade.XML gxml = new Glade.XML (null, "reportes.glade", "rango_rep_adm", null);
			gxml.Autoconnect (this);
			//muestra la ventana de reportes
			rango_rep_adm.Show();
			entry_dia_inicial.Text = DateTime.Now.ToString("dd");
			entry_mes_inicial.Text = DateTime.Now.ToString("MM");
			entry_ano_inicial.Text = DateTime.Now.ToString("yyyy");
			
			entry_dia_final.Text = DateTime.Now.ToString("dd");
			entry_mes_final.Text = DateTime.Now.ToString("MM");
			entry_ano_final.Text = DateTime.Now.ToString("yyyy");
			// Imprime reporte
			if(tipo_de_salida == "impresora"){
				button_imprimir.Clicked += new EventHandler(on_button_imprimir_clicked);
			}
			if(tipo_de_salida == "archivo"){
				button_imprimir.Clicked += new EventHandler(on_button_imprimir_clicked);	
			}
			
			button_busca_doctores.Clicked += new EventHandler(on_button_busca_doctores_clicked);
			button_busca_empresa.Clicked += new EventHandler(on_button_busca_empresa_clicked);
			
			//Activo los valores por default de busqueda;
	    	radiobutton_ambos_sexos.Active = true;
	    	radiobutton_folio_servicio.Active = true;
	    	radiobutton_reporte_general.Active = true;
	    	
	    	//acciones al dar click en los botones
	    	checkbutton_todos_admision.Clicked += new EventHandler(on_checkbutton_todos_admision_clicked);
	    	checkbutton_todos_paciente.Clicked += new EventHandler(on_checkbutton_todos_paciente_clicked);
	    	checkbutton_todas_fechas.Clicked += new EventHandler(on_checkbutton_todas_fechas_clicked);
	    	checkbutton_todas_empresas.Clicked += new EventHandler(on_checkbutton_todas_empresas_clicked);
	    	checkbutton_todos_doctores.Clicked += new EventHandler(on_checkbutton_todos_doctores_clicked);
			checkbutton_primera_vez.Clicked += new EventHandler(on_checkbutton_filtros_clicked);
	    	checkbutton_status.Clicked += new EventHandler(on_checkbutton_filtros_clicked);
			checkbutton_export_ods.Clicked += new EventHandler(on_checkbutton_export_ods_clicked);
			checkbutton_all_rpt.Clicked += new EventHandler(on_checkbutton_all_rpt_clicked);
			radiobutton_total.Clicked += new EventHandler(on_radiobutton_tiporpt_clicked);
			radiobutton_detallado.Clicked += new EventHandler(on_radiobutton_tiporpt_clicked);
			radiobutton_total_grupo.Clicked += new EventHandler(on_radiobutton_tiporpt_clicked);
						
		    // Desactivando Combobox en la entrada para que el usuario lo pueda elegir o activar
		    combobox_tipo_admision.Sensitive = false;
		    combobox_tipo_paciente.Sensitive = false;
		    //Activando los check buttons paa que se inicialicen activos
		    checkbutton_todos_admision.Active = true;
		    checkbutton_todos_paciente.Active = true;
			checkbutton_empraseg.Sensitive = false;
		   		    
		    // Salir de la ventana
			button_salir.Clicked += new EventHandler(on_cierraventanas_clicked);

			llenado_combobox(1,"",combobox_tipo_admision,"sql","SELECT * FROM osiris_his_tipo_admisiones WHERE servicio_directo = 'false' "+
				"AND activo_admision = 'true' "+
				"ORDER BY id_tipo_admisiones;","descripcion_admisiones","id_tipo_admisiones",args_args,args_id_array,"acceso_servicios_directo");
			llenado_combobox(1,"",combobox_tipo_paciente,"sql","SELECT * FROM osiris_his_tipo_pacientes WHERE activo_tipo_paciente = 'true' ORDER BY descripcion_tipo_paciente;",
				"descripcion_tipo_paciente","id_tipo_paciente",args_args,args_id_array,"id_tipo_documento");
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
				case "combobox_tipo_admision":
					tipointernamiento = (string) combobox_tipo_admision.Model.GetValue(iter,0);
					idtipointernamiento = (int) combobox_tipo_admision.Model.GetValue(iter,1);
					query_tipo_admision = "AND osiris_erp_movcargos.id_tipo_admisiones = '"+idtipointernamiento.ToString()+"' ";
					break;
				case "combobox_tipo_paciente":
					tipopaciente = (string) combobox_tipo_paciente.Model.GetValue(iter,0);
					id_tipopaciente = (int) combobox_tipo_paciente.Model.GetValue(iter,1);
					query_tipo_paciente = "AND osiris_erp_movcargos.id_tipo_paciente = '"+id_tipopaciente.ToString()+"' ";
					break;
				}
			}
		}
			
		void on_button_busca_doctores_clicked(object sender, EventArgs args)
		{
			object[] parametros_objetos = {entry_id_doctor_consulta,entry_nombre_doctor_consulta};
			string[] parametros_sql = {"SELECT * FROM osiris_his_medicos WHERE medico_activo = 'true' "};
			string[] parametros_string = {};
			string[,] args_buscador1 = {{"NOMBRE O APELLIDOS","AND nombre_medico LIKE '%","%' "},
										{"ID MEDICO","AND id_medico = '","' "}};
			string[,] args_buscador2 = {{"ID MEDICO","AND id_medico = '","' "},
										{"NOMBRE O APELLIDOS","AND nombre_medico LIKE '%","%' "}};
			string[,] args_orderby = {{"",""}};
			classfind_data.buscandor(parametros_objetos,parametros_sql,parametros_string,"find_medico_consulta",0,args_buscador1,args_buscador2,args_orderby);
			idmedico = entry_id_doctor_consulta.Text.ToString().Trim();
		}
					
		void on_button_busca_empresa_clicked(object sender, EventArgs args)
		{
			//query_empresa = "AND hscmty_empresas.id_empresa = '"+idempresa.ToString()+"' ";
			// diferenciar el tipo de busqueda empresa o aseguradora
			//id_tipopaciente = 400 asegurados
			//id_tipopaciente = 102 empresas
			//id_tipopaciente = 500 municipio
			//id_tipopaciente = 100 DIF
			//id_tipopaciente = 600 Sindicato
			// Los parametros de del SQL siempre es primero cuando busca todo y la otra por expresion
			// la clase recibe tambien el orden del query
			// es importante definir que tipo de busqueda es para que los objetos caigan ahi mismo
			//Console.WriteLine(id_tipopaciente.ToString());
			if(checkbutton_todos_paciente.Active == false){
				if (id_tipopaciente != 400){				
					//Console.WriteLine("Empresas");
					object[] parametros_objetos = {entry_id_empaseg_cita,entry_nombre_empaseg_cita};
					string[] parametros_sql = {"SELECT * FROM osiris_empresas WHERE id_tipo_paciente = '"+id_tipopaciente.ToString().Trim()+"' "};
					string[] parametros_string = {};
					string[,] args_buscador1 = {{"EMPRESA","AND descripcion_empresa LIKE '%","%' "},
												{"ID EMPRESA","AND id_empresa = '","' "}};
					string[,] args_buscador2 = {{"ID EMPRESA","AND id_empresa = '","' "},
												{"EMPRESA","AND descripcion_empresa LIKE '%","%' "}};
					string[,] args_orderby = {{"",""}};
					classfind_data.buscandor(parametros_objetos,parametros_sql,parametros_string,"find_empresa_cita",0,args_buscador1,args_buscador2,args_orderby);
					idempresa = entry_id_empaseg_cita.Text.ToString().Trim();					
					idaseguradora = "1";		
				}else{
					//Console.WriteLine("Aseguradoras");
					// Buscando aseguradora
					object[] parametros_objetos = {entry_id_empaseg_cita,entry_nombre_empaseg_cita};
					string[] parametros_sql = {"SELECT * FROM osiris_aseguradoras WHERE activa = 'true' "};
					string[] parametros_string = {};
					string[,] args_buscador1 = {{"ASEGURADORA","AND descripcion_aseguradora LIKE '%","%' "},
												{"ID ASEGURADORA","AND id_aseguradora = '","' "}};
					string[,] args_buscador2 = {{"ID ASEGURADORA","AND id_aseguradora = '","' "},
												{"ASEGURADORA","AND descripcion_aseguradora LIKE '%","%' "}};
					string[,] args_orderby = {{"",""}};
					classfind_data.buscandor(parametros_objetos,parametros_sql,parametros_string,"find_aseguradoras_cita",0,args_buscador1,args_buscador2,args_orderby);
					idaseguradora = entry_id_empaseg_cita.Text.ToString().Trim();
					idempresa = "1";					
				}
			}
		}
		
		/////////////////Acciones del boton todas las admisiones
		void on_checkbutton_todos_admision_clicked(object sender, EventArgs args)
		{
			if (checkbutton_todos_admision.Active == true){
				combobox_tipo_admision.Sensitive = false;
				query_tipo_admision = " ";			
			}else{
				//query_tipo_admision = "AND osiris_erp_movcargos.id_tipo_admisiones = '"+idtipointernamiento.ToString()+"' ";
				combobox_tipo_admision.Sensitive = true;
			}
		}
		/////////////////Acciones del boton todos los tipos de pacientes
		void on_checkbutton_todos_paciente_clicked(object sender, EventArgs args)
		{
			if (checkbutton_todos_paciente.Active == true){
				query_tipo_paciente= " ";
				combobox_tipo_paciente.Sensitive = false;
			}else{
				//query_tipo_paciente = "AND osiris_erp_movcargos.id_tipo_paciente = '"+id_tipopaciente.ToString()+"' ";
				combobox_tipo_paciente.Sensitive = true;
			}
		}
		
		void on_checkbutton_todas_empresas_clicked(object sender, EventArgs args)
		{
			if (checkbutton_todas_empresas.Active == false){
				button_busca_empresa.Sensitive = true;
				query_empresa = " ";
				query_aseguradora = " ";
			}else{
				if (id_tipopaciente != 400){
					query_empresa = "AND osiris_empresas.id_empresa = '"+entry_id_empaseg_cita.Text.ToString()+"' ";
				}else{
					query_aseguradora = "AND osiris_aseguradoras.id_aseguradora = '"+entry_id_empaseg_cita.Text.ToString()+"' ";
				}
				button_busca_empresa.Sensitive = false;
			}
		}
		
		void on_checkbutton_todos_doctores_clicked(object sender, EventArgs args)
		{
			if (checkbutton_todos_doctores.Active == false){
				button_busca_doctores.Sensitive = true;
				query_medico = " ";			
			}else{
				query_medico = "AND osiris_his_medicos.id_medico = '"+entry_id_doctor_consulta.Text.ToString()+"' ";
				button_busca_doctores.Sensitive = false;
			}
		}
		
		/////////////////Acciones del boton todas las fechas
		void on_checkbutton_todas_fechas_clicked(object sender, EventArgs args)
		{
			bool active_checkbutton;			
			if (checkbutton_todas_fechas.Active == true){
				query_rango_fechas= " ";
				active_checkbutton = false;
			}else{
				query_rango_fechas = "AND to_char(osiris_erp_cobros_enca.fechahora_creacion,'yyyy-MM-dd') >= '"+(string) entry_ano_inicial.Text.ToString()+"-"+(string) entry_mes_inicial.Text.ToString()+"-"+(string) entry_dia_inicial.Text.ToString()+"' "+
									"AND to_char(osiris_erp_cobros_enca.fechahora_creacion,'yyyy-MM-dd') <= '"+(string) entry_ano_final.Text.ToString()+"-"+(string) entry_mes_final.Text.ToString()+"-"+(string) entry_dia_final.Text.ToString()+"' ";
				active_checkbutton = true;
			}				
			entry_dia_inicial.Sensitive = active_checkbutton;
			entry_mes_inicial.Sensitive = active_checkbutton;
			entry_ano_inicial.Sensitive = active_checkbutton;
			entry_dia_final.Sensitive = active_checkbutton;
			entry_mes_final.Sensitive = active_checkbutton;
			entry_ano_final.Sensitive = active_checkbutton;			
		}
		
		void on_checkbutton_filtros_clicked(object sender, EventArgs args)
		{
			Gtk.CheckButton checkbuttonfiltros = (object) sender as Gtk.CheckButton;
			//Console.WriteLine(checkbuttonfiltros.Name.ToString());
			switch ((string) checkbuttonfiltros.Name.ToString()){
				case "checkbutton_primera_vez":
					if((bool) checkbutton_primera_vez.Active == true){
						query_primeravez = "AND vista_primera_vez = 'true' ";
					}else{
						query_primeravez = " ";
					}					
				break;
				case "checkbutton_status":
				
				break;
				case "checkbutton_empraseg":
				
				break;
			}
		}

		void on_checkbutton_export_ods_clicked(object sender, EventArgs args)
		{
			if(checkbutton_export_ods.Active == true){
				tipo_de_salida = "archivo";
			}else{
				tipo_de_salida = "impresora";
			}
			radiobutton_detallado.Sensitive = (bool) !checkbutton_export_ods.Active;
			radiobutton_total_grupo.Sensitive = (bool) !checkbutton_export_ods.Active;
			radiobutton_total.Sensitive = (bool) !checkbutton_export_ods.Active;
			checkbutton_all_rpt.Sensitive = (bool) !checkbutton_export_ods.Active;
		}

		void on_checkbutton_all_rpt_clicked(object sender, EventArgs args)
		{
			radiobutton_detallado.Sensitive = (bool) !checkbutton_all_rpt.Active;
			radiobutton_total_grupo.Sensitive = (bool) !checkbutton_all_rpt.Active;
			radiobutton_total.Sensitive = (bool) !checkbutton_all_rpt.Active;
			checkbutton_export_ods.Sensitive = (bool) !checkbutton_all_rpt.Active;
		}

		void on_radiobutton_tiporpt_clicked(object sender, EventArgs args)
		{
			Gtk.RadioButton radiobutton = (Gtk.RadioButton) sender;
			//Gtk.RadioButton radiobutton = obj sender Gtk.RadioButton;
			
			if(radiobutton.Name.ToString() == "radiobutton_detallado"){
				radiobutton_folio_servicio.Active = true;
				radiobutton_no_cancelados.Active = true;
				radiobutton_reporte_general.Active = true;
			}
			if(radiobutton.Name.ToString() == "radiobutton_total"){
				radiobutton_tipo_admision.Active = true;
				radiobutton_no_cancelados.Active = true;
			}
			if(radiobutton.Name.ToString() == "radiobutton_total_grupo"){
				radiobutton_tipo_admision.Active = true;
				radiobutton_no_cancelados.Active = true;
			}
			if(radiobutton.Name.ToString() == "checkbutton_all_rpt"){
				radiobutton_folio_servicio.Active = true;
				radiobutton_reporte_general.Active = true;
			}
			if(radiobutton.Name.ToString() == "checkbutton_export_ods"){
				radiobutton_tipo_admision.Active = true;
				radiobutton_no_cancelados.Active = true;
				radiobutton_reporte_general.Active = true;
			}
			checkbutton_empraseg.Sensitive = radiobutton_total_grupo.Active;
		}

		// cierra ventanas emergentes
		void on_cierraventanas_clicked (object sender, EventArgs a)
		{
			Widget win = (Widget) sender;
			win.Toplevel.Destroy();
		}
		
		void tipo_de_reporte_a_mostrar(object sender, EventArgs args)
		{
			if(radiobutton_reporte_general.Active == true) { query_tipo_reporte = " "; }
			if(radiobutton_cancelados.Active == true) { query_tipo_reporte = " AND cancelado = 'true' "; }
			if(radiobutton_no_cancelados.Active == true) { query_tipo_reporte = " AND cancelado = 'false' "; }	
		}
		
		void tipo_de_sexo(object sender, EventArgs args)
		{
			if(radiobutton_ambos_sexos.Active == true){
				query_sexo = " "; }
			if(radiobutton_masculino.Active == true){
				query_sexo = "AND sexo_paciente = 'H'"; }
			if(radiobutton_femenino.Active == true){
				query_sexo = "AND sexo_paciente = 'M'"; }
		}
		
		void tipo_radiobutton(object sender, EventArgs args)
		{
			if(radiobutton_folio_servicio.Active == true){
				query_orden = "ORDER BY osiris_erp_cobros_enca.folio_de_servicio;";}
			if(radiobutton_pid_paciente.Active == true){
				query_orden = "ORDER BY osiris_erp_cobros_enca.pid_paciente;"; }
			if(radiobutton_nombres.Active == true){
				query_orden = "ORDER BY nombre1_paciente || ' ' || nombre2_paciente || ' ' || apellido_paterno_paciente || ' ' || apellido_materno_paciente;"; }
			if(radiobutton_doctores.Active == true){
				query_orden = "ORDER BY nombre1_medico || ' ' || nombre2_medico || ' ' || apellido_paterno_medico || ' ' || apellido_materno_medico;"; }
			if(radiobutton_tipo_admision.Active == true){
				query_orden = "ORDER BY osiris_his_tipo_admisiones.id_tipo_admisiones,osiris_erp_cobros_enca.folio_de_servicio;";}
			if(checkbutton_all_rpt.Active == false){
				if(radiobutton_total.Active == true){
					query_orden = "ORDER BY osiris_his_tipo_pacientes.id_tipo_paciente,osiris_erp_cobros_enca.folio_de_servicio;";
				}
				if(radiobutton_total_grupo.Active == true){
					query_orden = "ORDER BY osiris_his_tipo_pacientes.id_tipo_paciente,osiris_erp_cobros_enca.id_empresa;";
				}
			}
		}
		
		void on_button_imprimir_clicked(object sender, EventArgs a)
		{		
			tipo_de_reporte_a_mostrar(sender, a);
	    	tipo_de_sexo(sender, a);
			tipo_radiobutton(sender, a);
			string tipopaciente_rpt = "";
			string tipoadmision_rpt = "";
			string nombredoctor_rpt = "";


			if(tipo_de_salida == "impresora"){				
				if (checkbutton_todas_fechas.Active == true) {
					query_rango_fechas = " ";
					entry_dia_inicial.Sensitive = false;
					entry_mes_inicial.Sensitive = false;
					entry_ano_inicial.Sensitive = false;
					entry_dia_final.Sensitive = false;
					entry_mes_final.Sensitive = false;
					entry_ano_final.Sensitive = false;
				} else {	
					query_rango_fechas = "AND to_char(osiris_erp_cobros_enca.fechahora_creacion,'yyyy-MM-dd') >= '" + (string)entry_ano_inicial.Text.ToString () + "-" + (string)entry_mes_inicial.Text.ToString () + "-" + (string)entry_dia_inicial.Text.ToString () + "'  " +
					"AND to_char(osiris_erp_cobros_enca.fechahora_creacion,'yyyy-MM-dd') <= '" + (string)entry_ano_final.Text.ToString () + "-" + (string)entry_mes_final.Text.ToString () + "-" + (string)entry_dia_final.Text.ToString () + "' ";
				}
				if(radiobutton_detallado.Active == true){
					if (checkbutton_todos_paciente.Active == true) {
						tipopaciente_rpt = "";
					} else {
						tipopaciente_rpt = tipopaciente;
					}
					if (checkbutton_todos_admision.Active == true) {
						tipoadmision_rpt = "";
					} else {
						tipoadmision_rpt = tipointernamiento;
					}
					if (checkbutton_todos_doctores.Active == true) {
						nombredoctor_rpt = "";entry_nombre_doctor_consulta.Text = "";
					} else {
						nombredoctor_rpt = entry_nombre_doctor_consulta.Text;
					}
					string[] parametros = {query_reporte,entry_dia_inicial.Text+"-"+entry_mes_inicial.Text+"-"+entry_ano_inicial.Text,
						entry_dia_final.Text+"-"+entry_mes_final.Text+"-"+entry_ano_final.Text,tipopaciente_rpt,tipoadmision_rpt,nombredoctor_rpt};
					new osiris.rpt_admisiones( parametros, 
						query_tipo_admision + 
						query_tipo_paciente + 
						query_sexo + 
						query_empresa + 
						query_aseguradora + 
						query_tipo_reporte + 
						query_medico +
						query_rango_fechas + 
						query_primeravez,
						query_orden,(bool) checkbutton_status.Active);
				}
				if(radiobutton_total_grupo.Active == true){

				}
				if(radiobutton_total.Active == true){
					if(checkbutton_empraseg.Active == false){	
						genera_reporte_totaltipopx ();
					}else{

					}
				}
			}
			if(tipo_de_salida == "archivo"){				
				//if (id_tipopaciente != 400){
				//	query_empresa = "AND osiris_empresas.id_empresa = '"+entry_id_empaseg_cita.Text.ToString()+"' ";
				//}else{
				//	query_aseguradora = "AND osiris_aseguradores.id_aseguradora = '"+entry_id_empaseg_cita.Text.ToString()+"' ";
				//}
				if (checkbutton_todos_paciente.Active == true){
					query_tipo_paciente= " ";					
				}else{
					if(tiporeporte == "COMPROBANTES_SERVICIO"){
						query_tipo_paciente = "AND osiris_erp_comprobante_servicio.id_tipo_paciente = '"+id_tipopaciente.ToString()+"' ";					
					}
					if(tiporeporte == "PASES_QUIROFANO_URGENCIAS"){
						query_tipo_paciente = "AND osiris_erp_pases_qxurg.id_tipo_paciente = '"+id_tipopaciente.ToString()+"' ";
					}
					if(tiporeporte == "PAGARES"){
						query_tipo_paciente = "AND osiris_erp_comprobante_pagare.id_tipo_paciente = '"+id_tipopaciente.ToString()+"' ";
					}
				}				
				if (checkbutton_todas_fechas.Active == true){
					query_rango_fechas= " ";
					entry_dia_inicial.Sensitive = false;
					entry_mes_inicial.Sensitive = false;
					entry_ano_inicial.Sensitive = false;
					entry_dia_final.Sensitive = false;
					entry_mes_final.Sensitive = false;
					entry_ano_final.Sensitive = false;
				}else{
					if(tiporeporte == "COMPROBANTES_SERVICIO"){
						query_rango_fechas = "AND to_char(osiris_erp_cobros_enca.fechahora_creacion,'yyyy-MM-dd') >= '"+(string) entry_ano_inicial.Text.ToString()+"-"+(string) entry_mes_inicial.Text.ToString()+"-"+(string) entry_dia_inicial.Text.ToString()+"'  "+
									"AND to_char(osiris_erp_cobros_enca.fechahora_creacion,'yyyy-MM-dd') <= '"+(string) entry_ano_final.Text.ToString()+"-"+(string) entry_mes_final.Text.ToString()+"-"+(string) entry_dia_final.Text.ToString()+"' ";
					}
					if(tiporeporte == "PASES_QUIROFANO_URGENCIAS"){
						query_rango_fechas = "AND to_char(osiris_erp_pases_qxurg.fechahora_creacion,'yyyy-MM-dd') >= '"+(string) entry_ano_inicial.Text.ToString()+"-"+(string) entry_mes_inicial.Text.ToString()+"-"+(string) entry_dia_inicial.Text.ToString()+"'  "+
									"AND to_char(osiris_erp_pases_qxurg.fechahora_creacion,'yyyy-MM-dd') <= '"+(string) entry_ano_final.Text.ToString()+"-"+(string) entry_mes_final.Text.ToString()+"-"+(string) entry_dia_final.Text.ToString()+"' ";
					}
					if(tiporeporte == "PAGARES"){
						query_rango_fechas = "AND to_char(osiris_erp_comprobante_pagare.fechahora_creacion,'yyyy-MM-dd') >= '"+(string) entry_ano_inicial.Text.ToString()+"-"+(string) entry_mes_inicial.Text.ToString()+"-"+(string) entry_dia_inicial.Text.ToString()+"'  "+
									"AND to_char(osiris_erp_comprobante_pagare.fechahora_creacion,'yyyy-MM-dd') <= '"+(string) entry_ano_final.Text.ToString()+"-"+(string) entry_mes_final.Text.ToString()+"-"+(string) entry_dia_final.Text.ToString()+"' ";	
					}
					if(tiporeporte == "PAGARES_X_VENCER"){
						query_rango_fechas = "AND to_char(osiris_erp_comprobante_pagare.fecha_vencimiento_pagare,'yyyy-MM-dd') >= '"+(string) entry_ano_inicial.Text.ToString()+"-"+(string) entry_mes_inicial.Text.ToString()+"-"+(string) entry_dia_inicial.Text.ToString()+"'  "+
									"AND to_char(osiris_erp_comprobante_pagare.fecha_vencimiento_pagare,'yyyy-MM-dd') <= '"+(string) entry_ano_final.Text.ToString()+"-"+(string) entry_mes_final.Text.ToString()+"-"+(string) entry_dia_final.Text.ToString()+"' ";	
					}
					if(tiporeporte == "ADMISIONES"){
						query_rango_fechas = "AND to_char(osiris_erp_cobros_enca.fechahora_creacion,'yyyy-MM-dd') >= '"+(string) entry_ano_inicial.Text.ToString()+"-"+(string) entry_mes_inicial.Text.ToString()+"-"+(string) entry_dia_inicial.Text.ToString()+"'  "+
										"AND to_char(osiris_erp_cobros_enca.fechahora_creacion,'yyyy-MM-dd') <= '"+(string) entry_ano_final.Text.ToString()+"-"+(string) entry_mes_final.Text.ToString()+"-"+(string) entry_dia_final.Text.ToString()+"' ";
					}					
				}
				
				//"AND osiris_erp_comprobante_servicio.id_tipo_paciente = '102' " +
				//"AND to_char(osiris_erp_cobros_enca.fechahora_creacion,'MM') = '10' " +
				//"AND to_char(osiris_erp_cobros_enca.fechahora_creacion,'yyyy') = '2011' " +
				//"AND osiris_erp_comprobante_servicio.id_empresa = '31' " +
				//"ORDER BY osiris_erp_cobros_deta.folio_de_servicio ASC;";
				if(tiporeporte == "COMPROBANTES_SERVICIO"){				
					string query_sql = "SELECT osiris_erp_cobros_deta.folio_de_servicio AS foliodeservicio,osiris_erp_cobros_deta.pid_paciente AS pidpaciente,sexo_paciente,"+
						"osiris_his_tipo_admisiones.descripcion_admisiones,aplicar_iva, osiris_his_tipo_admisiones.id_tipo_admisiones AS idadmisiones,"+
							"osiris_grupo_producto.descripcion_grupo_producto, osiris_productos.id_grupo_producto,  to_char(osiris_erp_cobros_deta.porcentage_descuento,'999.99') AS porcdesc," +
							 "to_char(osiris_erp_cobros_enca.fechahora_creacion,'dd-mm-yyyy') AS fechcreacion,to_char(osiris_erp_cobros_enca.fechahora_creacion,'HH:mm') AS horacreacion," +
							 "to_char(osiris_erp_cobros_deta.id_producto,'999999999999') AS idproducto,descripcion_producto, to_char(osiris_erp_cobros_deta.cantidad_aplicada,'99999999.99') AS cantidadaplicada," +
							 "to_char(osiris_erp_cobros_deta.precio_producto,'9999999.99') AS preciounitario, ltrim(to_char(osiris_erp_cobros_deta.precio_producto,'9999999.99')) AS preciounitarioprod," +
							 "to_char(osiris_erp_cobros_deta.iva_producto,'999999.99') AS ivaproducto," +
							 "to_char(osiris_erp_cobros_deta.cantidad_aplicada * osiris_erp_cobros_deta.precio_producto,'99999999.99') AS ppcantidad," +
							 "to_char(osiris_productos.precio_producto_publico,'999999999.99999') AS preciopublico,osiris_erp_comprobante_servicio.numero_comprobante_servicio AS numerorecibo," +
							 "osiris_his_paciente.nombre1_paciente || ' ' || osiris_his_paciente.nombre2_paciente || ' ' || osiris_his_paciente.apellido_paterno_paciente || ' ' || osiris_his_paciente.apellido_materno_paciente AS nombre_paciente," +
							 "to_char(osiris_his_paciente.fecha_nacimiento_paciente, 'dd-MM-yyyy') AS fechanacpaciente,to_char(to_number(to_char(age('2011-01-20 05:25:11',osiris_his_paciente.fecha_nacimiento_paciente),'yyyy') ,'9999'),'9999') AS edadpaciente," +
							 "telefono_particular1_paciente,osiris_erp_comprobante_servicio.observaciones AS nro_oficio,osiris_erp_comprobante_servicio.observaciones2 AS nro_nomina,osiris_erp_comprobante_servicio.observaciones3 AS departamento," +
							 "osiris_erp_comprobante_servicio.concepto_del_comprobante AS concepto_comprobante,osiris_erp_cobros_enca.id_empresa,descripcion_empresa,osiris_erp_cobros_enca.nombre_medico_encabezado AS medico_tratante," +
							 "osiris_erp_cobros_enca.id_aseguradora,descripcion_aseguradora "+
							 "FROM osiris_erp_cobros_deta,osiris_his_tipo_admisiones,osiris_productos,osiris_grupo_producto,osiris_erp_comprobante_servicio,osiris_his_paciente,osiris_erp_cobros_enca,osiris_empresas,osiris_aseguradoras " +
							 "WHERE osiris_erp_cobros_deta.id_tipo_admisiones = osiris_his_tipo_admisiones.id_tipo_admisiones " +
							 "AND osiris_erp_cobros_deta.id_producto = osiris_productos.id_producto " +
							 "AND osiris_productos.id_grupo_producto = osiris_grupo_producto.id_grupo_producto " +
							 "AND osiris_erp_cobros_deta.pid_paciente = osiris_his_paciente.pid_paciente " +
							 "AND osiris_erp_cobros_enca.id_empresa = osiris_empresas.id_empresa " +
							 "AND osiris_aseguradoras.id_aseguradora = osiris_erp_cobros_enca.id_aseguradora "+
							 "AND osiris_erp_cobros_enca.folio_de_servicio = osiris_erp_cobros_deta.folio_de_servicio " +
							 "AND osiris_erp_comprobante_servicio.folio_de_servicio = osiris_erp_cobros_deta.folio_de_servicio " +
							 "AND osiris_erp_cobros_deta.eliminado = 'false' " +
							 "AND osiris_erp_cobros_enca.cancelado = 'false' " +
							
							query_tipo_paciente + 
							query_sexo + 
							query_empresa + 
							query_aseguradora +
							query_tipo_reporte +
							query_rango_fechas + 
							query_orden;
									
					string[] args_names_field = {"foliodeservicio","pidpaciente","descripcion_admisiones","descripcion_grupo_producto","fechcreacion","idproducto","descripcion_producto",
											"cantidadaplicada","preciounitario","numerorecibo","nombre_paciente","nro_oficio","nro_nomina","departamento","medico_tratante"};
					string[] args_type_field = {"float","float","string","string","string","float","string","float","float","float","string","string","string","string","string"};
					string[] args_field_text = {""};
					string[] args_more_title = {""};
					string[,] args_formulas = {{"",""}};
					string[,] args_width = {{"",""}};
					new osiris.class_traslate_spreadsheet(query_sql,args_names_field,args_type_field,false,args_field_text,"",false,args_more_title,args_formulas,args_width);					
				}
				if(tiporeporte == "PASES_QUIROFANO_URGENCIAS"){
					// "SELECT DISTINCT ON (osiris_erp_pases_qxurg.folio_de_servicio) osiris_erp_pases_qxurg.folio_de_servicio,
					string query_sql = "SELECT osiris_erp_pases_qxurg.folio_de_servicio, osiris_erp_pases_qxurg.id_secuencia AS nro_pase,osiris_erp_pases_qxurg.folio_de_servicio AS foliodeservicio,to_char(osiris_erp_cobros_enca.pid_paciente,'9999999999') AS expediente," +
						"nombre1_paciente || ' ' || nombre2_paciente || ' ' || apellido_paterno_paciente || ' ' || apellido_materno_paciente AS nombre_completo," +
						"to_char(osiris_his_paciente.fecha_nacimiento_paciente, 'dd-MM-yyyy') AS fechanacpaciente,to_char(to_number(to_char(age('"+DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss")+"',osiris_his_paciente.fecha_nacimiento_paciente),'yyyy') ,'9999'),'9999') AS edadpaciente," +
						"osiris_his_paciente.sexo_paciente,osiris_erp_cobros_enca.nombre_medico_tratante,osiris_erp_pases_qxurg.id_quien_creo,nombre1_empleado || ' ' || nombre2_empleado || ' ' || apellido_paterno_empleado || ' ' || apellido_materno_empleado AS nombresolicitante," +
						"to_char(osiris_erp_pases_qxurg.fechahora_creacion,'yyyy-MM-dd') AS fechapaseqx,osiris_erp_pases_qxurg.id_tipo_admisiones,descripcion_admisiones,osiris_erp_cobros_enca.id_empresa AS idempresa,osiris_empresas.descripcion_empresa," +
						"osiris_erp_cobros_enca.id_aseguradora,osiris_aseguradoras.descripcion_aseguradora,descripcion_diagnostico_movcargos AS motivo_ingreso,descripcion_tipo_paciente," +
						"subtotal15+subtotal0+subtotal_iva AS totalprocedimiento,total_costo_con_impuesto+total_costo_sin_impuesto+total_iva_costo AS totcostoprocedimiento," +
						"osiris_erp_movcargos.id_tipo_cirugia,osiris_his_tipo_cirugias.descripcion_cirugia AS descripcioncirugia,osiris_erp_cobros_enca.numero_factura,osiris_erp_cobros_enca.honorario_medico," +
						"osiris_erp_cobros_enca.id_medico_tratante,osiris_his_medicos.nombre_medico AS medicotratante,nombre_medico_encabezado AS dr_solicita,"+
						"osiris_erp_pases_qxurg.id_producto AS idproducto,descripcion_producto,numero_serie,osiris_erp_pases_qxurg.tipo_anestesia AS tipoanestesia,osiris_erp_pases_qxurg.id_cirujano AS idcirujano,nombre_cirujano," +
						"osiris_erp_pases_qxurg.id_anestesiologo AS idanestesiologo,nombre_anestesiologo,id_ayudante,nombre_ayudante,osiris_erp_pases_qxurg.observaciones AS observacionespaseqx," +
						"osiris_erp_cobros_enca.observaciones1,total_abonos+total_pago AS pagosabonos,monto_convenio AS montoconvenido,cerrado, "+
						"osiris_erp_pases_qxurg.eliminado,osiris_erp_pases_qxurg.motivo_eliminacion,osiris_erp_pases_qxurg.descripcion_cirugia AS cxpaseqx,"+
						"osiris_erp_pases_qxurg.id_tipo_admisiones2,descripcion_admisiones AS tipodepase "+
						"FROM osiris_erp_pases_qxurg,osiris_his_tipo_admisiones,osiris_erp_cobros_enca,osiris_his_paciente,osiris_empleado,osiris_empresas,osiris_aseguradoras,osiris_erp_movcargos,osiris_his_tipo_pacientes,osiris_his_tipo_cirugias,osiris_his_medicos "+
						"WHERE " +
						//"osiris_erp_pases_qxurg.id_tipo_admisiones = osiris_his_tipo_admisiones.id_tipo_admisiones " +
						" osiris_erp_pases_qxurg.pid_paciente = osiris_his_paciente.pid_paciente " +
						"AND osiris_erp_pases_qxurg.folio_de_servicio = osiris_erp_cobros_enca.folio_de_servicio " +
						"AND osiris_erp_pases_qxurg.id_quien_creo = osiris_empleado.login_empleado " +
						"AND osiris_erp_cobros_enca.id_empresa = osiris_empresas.id_empresa " +
						"AND osiris_erp_pases_qxurg.id_tipo_admisiones2 = osiris_his_tipo_admisiones.id_tipo_admisiones " +
						"AND osiris_erp_cobros_enca.id_aseguradora = osiris_aseguradoras.id_aseguradora "+
						"AND osiris_erp_movcargos.id_tipo_paciente = osiris_his_tipo_pacientes.id_tipo_paciente "+
						"AND osiris_erp_movcargos.folio_de_servicio = osiris_erp_pases_qxurg.folio_de_servicio " +
						"AND osiris_erp_movcargos.id_tipo_cirugia = osiris_his_tipo_cirugias.id_tipo_cirugia " +
						"AND osiris_erp_cobros_enca.id_medico_tratante = osiris_his_medicos.id_medico " +
						"AND osiris_erp_cobros_enca.cancelado = 'false' " +
						//"AND osiris_erp_pases_qxurg.eliminado = 'false' "+
						//"AND osiris_erp_movcargos.id_anestesiologo = osiris_his_medicos.id_medico "+ 
						query_rango_fechas +
						query_tipo_paciente +
						query_empresa +
						query_aseguradora +
							
						"ORDER BY osiris_erp_pases_qxurg.folio_de_servicio;";
					string[] args_names_field = {"fechapaseqx","nro_pase","tipodepase","foliodeservicio","totalprocedimiento","totcostoprocedimiento","pagosabonos","montoconvenido","honorario_medico","numero_factura","expediente","nombre_completo","motivo_ingreso","descripcion_tipo_paciente","descripcion_empresa","descripcion_aseguradora","dr_solicita","medicotratante","cerrado","idproducto","descripcion_producto","numero_serie","tipoanestesia","idcirujano","nombre_cirujano","idanestesiologo","nombre_anestesiologo","id_ayudante","nombre_ayudante",
						"cxpaseqx","observacionespaseqx","eliminado","motivo_eliminacion","observaciones1","id_tipo_admisiones2"};
					string[] args_type_field = {"string","float","string","float","float","float","float","float","float","string","string","string","string","string","string","string","string","string","string","string","string","string","string","string","string","float","string","float","string","string","string","string","string","string","float"};
					string[] args_field_text = {"id_producto","nombre_producto","nro_serie","tipo_anestesia","id_anestesiologo","nombre_anestesiologo","observaciones","id_cirujano2","nombre_cirujano2"};
					string[] args_more_title = {""};
					string[,] args_formulas = {{"",""}};
					string[,] args_width = {{"10","6cm"},{"11","7cm"},{"12","5cm"},{"13","7cm"},{"14","6cm"},{"15","6cm"},{"28","7cm"}};
					new osiris.class_traslate_spreadsheet(query_sql,args_names_field,args_type_field,true,args_field_text,"observaciones1",false,args_more_title,args_formulas,args_width);
					
				}
				if(tiporeporte == "PAGARES"){
					string query_sql = "SELECT DISTINCT ON (osiris_erp_comprobante_pagare.folio_de_servicio) osiris_erp_comprobante_pagare.folio_de_servicio AS foliodeservicio,numero_comprobante_pagare AS nro_pagare,"+
						"to_char(osiris_erp_comprobante_pagare.fecha_comprobante,'yyyy-MM-dd') AS fechapagare,to_char(osiris_erp_comprobante_pagare.fecha_vencimiento_pagare,'yyyy-MM-dd') AS vencimientopagare," +
						"to_char(osiris_erp_comprobante_pagare.pid_paciente,'9999999999') AS pidpaciente," +
						"to_char(osiris_erp_comprobante_pagare.monto_pagare,'99999999.99') AS montopagare, "+
						"nombre1_paciente || ' ' || nombre2_paciente || ' ' || apellido_paterno_paciente || ' ' || apellido_materno_paciente AS nombre_completo," +
						"telefono_particular1_paciente AS telefono1,celular1_paciente AS celular," +
						"direccion_paciente || ' ' || numero_casa_paciente || ' ' || colonia_paciente || ' ' || codigo_postal_paciente || ' ' || estado_paciente AS direccion_px," +
						 "descripcion_tipo_paciente AS tipopaciente,osiris_erp_cobros_enca.monto_convenio,(osiris_erp_cobros_enca.total_abonos+osiris_erp_cobros_enca.total_pago) AS tot_abonos," +
						 "osiris_erp_cobros_enca.monto_convenio AS montoconvenio," +
						 "osiris_erp_cobros_enca.total_pago,osiris_erp_cobros_enca.monto_convenio - (osiris_erp_cobros_enca.total_abonos+osiris_erp_cobros_enca.total_pago) AS saldo_deuda " +
						"FROM osiris_erp_comprobante_pagare,osiris_his_paciente,osiris_his_tipo_pacientes,osiris_erp_cobros_enca " +
						"WHERE osiris_erp_comprobante_pagare.pid_paciente = osiris_his_paciente.pid_paciente " +
						"AND osiris_erp_cobros_enca.folio_de_servicio = osiris_erp_comprobante_pagare.folio_de_servicio "+
						"AND osiris_erp_comprobante_pagare.id_tipo_paciente = osiris_his_tipo_pacientes.id_tipo_paciente " +
						"AND osiris_erp_comprobante_pagare.eliminado = 'false' "+
						query_rango_fechas+
						//query_tipo_paciente+
						" ORDER BY osiris_erp_comprobante_pagare.folio_de_servicio;";
					string[] args_names_field = {"fechapagare","nro_pagare","vencimientopagare","montopagare","tot_abonos","saldo_deuda","montoconvenio","foliodeservicio","pidpaciente","nombre_completo","telefono1","celular","direccion_px","tipopaciente"};
						//"pagosabonos","pidpaciente","nombre_completo","motivo_ingreso","descripcion_tipo_paciente","descripcion_cirugia","dr_solicita","medicotratante","cerrado"};
					
					string[] args_type_field = {"string","float","string","float","float","float","float","float","float","string","string","string","string","string"};
							//,"float","string","string","string","string","string","string","string"};
					
					string[] args_field_text = {""};
					string[] args_more_title = {""};
					string[,] args_formulas = {{"3","=SUM(D2:D"},{"4","=SUM(E2:E"},{"5","=SUM(F2:F"}};
					string[,] args_width = {{"9","7cm"},{"10","5cm"}};
					new osiris.class_traslate_spreadsheet(query_sql,args_names_field,args_type_field,false,args_field_text,"",false,args_more_title,args_formulas,args_width);					
				}
				if(tiporeporte == "PAGARES_X_VENCER"){
					string query_sql = "SELECT osiris_erp_comprobante_pagare.folio_de_servicio AS foliodeservicio,numero_comprobante_pagare AS nro_pagare,"+
						"to_char(osiris_erp_comprobante_pagare.fecha_comprobante,'yyyy-MM-dd') AS fechapagare,to_char(osiris_erp_comprobante_pagare.fecha_vencimiento_pagare,'yyyy-MM-dd') AS vencimientopagare," +
						"to_char(osiris_erp_comprobante_pagare.pid_paciente,'9999999999') AS pidpaciente," +
						"to_char(osiris_erp_comprobante_pagare.monto_pagare,'99999999.99') AS montopagare, "+
						"nombre1_paciente || ' ' || nombre2_paciente || ' ' || apellido_paterno_paciente || ' ' || apellido_materno_paciente AS nombre_completo," +
						"telefono_particular1_paciente AS telefono1,celular1_paciente AS celular," +
						"direccion_paciente || ' ' || numero_casa_paciente || ' ' || colonia_paciente || ' ' || codigo_postal_paciente || ' ' || estado_paciente AS direccion_px," +
						 "descripcion_tipo_paciente AS tipopaciente,osiris_erp_cobros_enca.monto_convenio,(osiris_erp_cobros_enca.total_abonos+osiris_erp_cobros_enca.total_pago) AS tot_abonos," +
						 "osiris_erp_cobros_enca.monto_convenio AS montoconvenio," +
						 "osiris_erp_cobros_enca.total_pago,osiris_erp_cobros_enca.monto_convenio - (osiris_erp_cobros_enca.total_abonos+osiris_erp_cobros_enca.total_pago) AS saldo_deuda " +
						"FROM osiris_erp_comprobante_pagare,osiris_his_paciente,osiris_his_tipo_pacientes,osiris_erp_cobros_enca " +
						"WHERE osiris_erp_comprobante_pagare.pid_paciente = osiris_his_paciente.pid_paciente " +
						"AND osiris_erp_cobros_enca.folio_de_servicio = osiris_erp_comprobante_pagare.folio_de_servicio "+
						"AND osiris_erp_comprobante_pagare.id_tipo_paciente = osiris_his_tipo_pacientes.id_tipo_paciente " +
						"AND osiris_erp_comprobante_pagare.eliminado = 'false' "+
						query_rango_fechas+
						//query_tipo_paciente+
						" ORDER BY osiris_erp_comprobante_pagare.fecha_vencimiento_pagare;";
					string[] args_names_field = {"fechapagare","nro_pagare","vencimientopagare","montopagare","tot_abonos","saldo_deuda","montoconvenio","foliodeservicio","pidpaciente","nombre_completo","telefono1","celular","direccion_px","tipopaciente"};
						//"pagosabonos","pidpaciente","nombre_completo","motivo_ingreso","descripcion_tipo_paciente","descripcion_cirugia","dr_solicita","medicotratante","cerrado"};
					
					string[] args_type_field = {"string","float","string","float","float","float","float","float","float","string","string","string","string","string"};
							//,"float","string","string","string","string","string","string","string"};
					
					string[] args_field_text = {""};
					string[] args_more_title = {""};
					string[,] args_formulas = {{"3","=SUM(D2:D"},{"4","=SUM(E2:E"},{"5","=SUM(F2:F"}};
					string[,] args_width = {{"9","7cm"},{"10","5cm"}};
					new osiris.class_traslate_spreadsheet(query_sql,args_names_field,args_type_field,false,args_field_text,"",false,args_more_title,args_formulas,args_width);					
				}
				if(tiporeporte == "MOVIMIENTOS_GESTION_COBRANZA"){
					
				}
				if(tiporeporte == "ADMISIONES"){
					string consulta_sql = query_reporte + query_tipo_admision + query_tipo_paciente + query_sexo + 
										query_empresa + query_aseguradora + query_tipo_reporte + query_medico +
										query_rango_fechas + 
										query_primeravez + 
										query_orden;			
					string[] args_names_field = {"fech_reg_adm","folio_de_servicio","pid_paciente","nombre_completo","edad","sexo_paciente","descripcion_diagnostico_movcargos","descripcion_admisiones","descripcion_tipo_paciente","descripcion_empresa","descripcion_aseguradora","nombre_medico_encabezado","nombre_medico_tratante","cancelado","motivo_cancelacion","vista_primera_vez"};
					string[] args_type_field = {"string","float","float","string","float","string","string","string","string","string","string","string","string","string","string","string"};
					string[] args_field_text = {""};
					string[] args_more_title = {""};
					string[,] args_formulas = {{"",""}};
					string[,] args_width = {{"3","9cm"},{"5","4cm"},{"6","5cm"}};
					new osiris.class_traslate_spreadsheet(consulta_sql,args_names_field,args_type_field,false,args_field_text,"",false,args_more_title,args_formulas,args_width);
				}
			}
		}
		
		void genera_reporte_totaltipopx()
		{			
			print = new PrintOperation ();
			print.JobName = "Reporte Admisiones";
			print.BeginPrint += new BeginPrintHandler (OnBeginPrint);
			print.DrawPage += new DrawPageHandler (OnDrawPage);
			print.EndPrint += new EndPrintHandler (OnEndPrint);
			print.Run (PrintOperationAction.PrintDialog, null);			
		}
		
		private void OnBeginPrint (object obj, Gtk.BeginPrintArgs args)
		{
			print.NPages = 1;  // crea cantidad de copias del reporte			
			if(radiobutton_total.Active == true){
				// para imprimir vertical el reporte
				print.PrintSettings.Orientation = PageOrientation.Portrait;
			}else{
				// para imprimir horizontalmente el reporte
				print.PrintSettings.Orientation = PageOrientation.Landscape;
			}
			//Console.WriteLine(print.PrintSettings.Orientation.ToString());
		}
		
		private void OnDrawPage (object obj, Gtk.DrawPageArgs args)
		{			
			context = args.Context;
			ejecutar_reporte_totaltipopx(context);
		}
		
		void ejecutar_reporte_totaltipopx(PrintContext context)
		{
			print.PrintSettings.Orientation = PageOrientation.Portrait;
			Cairo.Context cr = context.CairoContext;
			Pango.Layout layout = context.CreatePangoLayout ();
			Pango.FontDescription desc = Pango.FontDescription.FromString ("Sans");			
			fontSize = 8.0;											layout = context.CreatePangoLayout ();		
			desc.Size = (int)(fontSize * pangoScale);				layout.FontDescription = desc;
			comienzo_linea = 85;
			int comienzolinea2 = comienzo_linea;
			NpgsqlConnection conexion;
			conexion = new NpgsqlConnection (connectionString+nombrebd);
			// Verifica que la base de datos este conectada
			try{
				conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand (); 
				if (checkbutton_todas_fechas.Active == true){
					query_rango_fechas= " ";
					entry_dia_inicial.Sensitive = false;
					entry_mes_inicial.Sensitive = false;
					entry_ano_inicial.Sensitive = false;
					entry_dia_final.Sensitive = false;
					entry_mes_final.Sensitive = false;
					entry_ano_final.Sensitive = false;
				}else{	
					query_rango_fechas = "AND to_char(osiris_erp_cobros_enca.fechahora_creacion,'yyyy-MM-dd') >= '"+(string) entry_ano_inicial.Text.ToString()+"-"+(string) entry_mes_inicial.Text.ToString()+"-"+(string) entry_dia_inicial.Text.ToString()+"'  "+
						"AND to_char(osiris_erp_cobros_enca.fechahora_creacion,'yyyy-MM-dd') <= '"+(string) entry_ano_final.Text.ToString()+"-"+(string) entry_mes_final.Text.ToString()+"-"+(string) entry_dia_final.Text.ToString()+"' ";
				}
				comando.CommandText = query_reporte + query_tipo_admision + query_tipo_paciente + query_sexo + 
					query_empresa + query_aseguradora + query_tipo_reporte + query_medico +
					query_rango_fechas + 
					query_primeravez + 
					query_orden;
				//Console.WriteLine(comando.CommandText.ToString());
				NpgsqlDataReader lector = comando.ExecuteReader ();
				int idtipopaciente;
				string descriptipopaciente = "";
				string foliodeservicio = "";
				int contador_primeravez = 0;
				int contador_subsecuente = 0;
				int total_primeravez = 0;
				int total_subsecuente = 0;
				if(lector.Read()){
					imprime_encabezado_portrait(cr,layout);
					cr.MoveTo(90*escala_en_linux_windows, comienzo_linea*escala_en_linux_windows);			layout.SetText("Tipo Paciente");			Pango.CairoHelper.ShowLayout (cr, layout);
					cr.MoveTo(190*escala_en_linux_windows, comienzo_linea*escala_en_linux_windows);			layout.SetText("1ra. Vez");			Pango.CairoHelper.ShowLayout (cr, layout);
					cr.MoveTo(280*escala_en_linux_windows, comienzo_linea*escala_en_linux_windows);			layout.SetText("Subsecuente");			Pango.CairoHelper.ShowLayout (cr, layout);
					cr.MoveTo(360*escala_en_linux_windows, comienzo_linea*escala_en_linux_windows);			layout.SetText("TOTAL");			Pango.CairoHelper.ShowLayout (cr, layout);
					fontSize = 8.0;			layout = null;			layout = context.CreatePangoLayout ();
					desc.Size = (int)(fontSize * pangoScale);		layout.FontDescription = desc;
					layout.FontDescription.Weight = Weight.Normal;		// Letra normal
					comienzo_linea += separacion_linea+4;
					idtipopaciente = (int) lector["id_tipo_paciente"];
					descriptipopaciente = (string) lector["descripcion_tipo_paciente"];
					foliodeservicio = (string) lector["folio_de_servicio"].ToString();
					//totaltipos_px += 1;
					if((bool) lector["vista_primera_vez"]){
						contador_primeravez += 1;
						total_primeravez += 1;
					}else{
						contador_subsecuente += 1;
						total_subsecuente += 1;
					}
					while (lector.Read()){
						if(idtipopaciente != (int) lector["id_tipo_paciente"]){
							comienzo_linea += separacion_linea + 4;
							cr.MoveTo(400*escala_en_linux_windows, comienzo_linea-2*escala_en_linux_windows);
							cr.LineTo(05,comienzo_linea-2);		// Linea Horizontal 1
							cr.MoveTo(10*escala_en_linux_windows, comienzo_linea*escala_en_linux_windows);			layout.SetText(descriptipopaciente);			Pango.CairoHelper.ShowLayout (cr, layout);
							cr.MoveTo(190*escala_en_linux_windows, comienzo_linea*escala_en_linux_windows);			layout.SetText(contador_primeravez.ToString());			Pango.CairoHelper.ShowLayout (cr, layout);
							cr.MoveTo(270*escala_en_linux_windows, comienzo_linea*escala_en_linux_windows);			layout.SetText(contador_subsecuente.ToString());			Pango.CairoHelper.ShowLayout (cr, layout);
							cr.MoveTo(350*escala_en_linux_windows, comienzo_linea*escala_en_linux_windows);			layout.SetText(Convert.ToString(contador_primeravez+contador_subsecuente));			Pango.CairoHelper.ShowLayout (cr, layout);
							layout.FontDescription.Weight = Weight.Normal;
							idtipopaciente = (int) lector["id_tipo_paciente"];
							descriptipopaciente = (string) lector["descripcion_tipo_paciente"];
							foliodeservicio = (string) lector["folio_de_servicio"].ToString();
							contador_primeravez = 0;
							contador_subsecuente = 0;
							if((bool) lector["vista_primera_vez"]){
								contador_primeravez += 1;
								total_primeravez += 1;
							}else{
								contador_subsecuente += 1;
								total_subsecuente +=1;
							}
						}else{
							if (foliodeservicio != (string) lector["folio_de_servicio"].ToString()){
								if((bool) lector["vista_primera_vez"]){
									contador_primeravez += 1;
									total_primeravez += 1;
								}else{
									contador_subsecuente += 1;
									total_subsecuente += 1;
								}
							}else{
								foliodeservicio = (string) lector["folio_de_servicio"].ToString();
							}
						}
					}
					comienzo_linea += separacion_linea + 4;
					cr.MoveTo(400*escala_en_linux_windows, comienzo_linea-2*escala_en_linux_windows);
					cr.LineTo(05,comienzo_linea-2);		// Linea Horizontal 1
					cr.MoveTo(10*escala_en_linux_windows, comienzo_linea*escala_en_linux_windows);			layout.SetText(descriptipopaciente);			Pango.CairoHelper.ShowLayout (cr, layout);
					cr.MoveTo(190*escala_en_linux_windows, comienzo_linea*escala_en_linux_windows);			layout.SetText(contador_primeravez.ToString());			Pango.CairoHelper.ShowLayout (cr, layout);
					cr.MoveTo(270*escala_en_linux_windows, comienzo_linea*escala_en_linux_windows);			layout.SetText(contador_subsecuente.ToString());			Pango.CairoHelper.ShowLayout (cr, layout);
					cr.MoveTo(350*escala_en_linux_windows, comienzo_linea*escala_en_linux_windows);			layout.SetText(Convert.ToString(contador_primeravez+contador_subsecuente));			Pango.CairoHelper.ShowLayout (cr, layout);

					cr.MoveTo(090*escala_en_linux_windows, (comienzo_linea+separacion_linea+4)*escala_en_linux_windows);			layout.SetText("TOTALES");			Pango.CairoHelper.ShowLayout (cr, layout);
					cr.MoveTo(190*escala_en_linux_windows, (comienzo_linea+separacion_linea+4)*escala_en_linux_windows);			layout.SetText(total_primeravez.ToString());			Pango.CairoHelper.ShowLayout (cr, layout);
					cr.MoveTo(270*escala_en_linux_windows, (comienzo_linea+separacion_linea+4)*escala_en_linux_windows);			layout.SetText(total_subsecuente.ToString());			Pango.CairoHelper.ShowLayout (cr, layout);
					cr.MoveTo(350*escala_en_linux_windows, (comienzo_linea+separacion_linea+4)*escala_en_linux_windows);			layout.SetText(Convert.ToString(total_primeravez+total_subsecuente));	Pango.CairoHelper.ShowLayout (cr, layout);


					cr.MoveTo(400*escala_en_linux_windows, comienzolinea2*escala_en_linux_windows);
					cr.LineTo(05,comienzolinea2); // horizontal

					cr.MoveTo(400*escala_en_linux_windows, comienzo_linea+separacion_linea+4*escala_en_linux_windows);
					cr.LineTo(05,comienzo_linea+separacion_linea+4);  // horizontal


					cr.MoveTo(05*escala_en_linux_windows, comienzolinea2*escala_en_linux_windows);
					cr.LineTo(05,comienzo_linea+separacion_linea+4);		// vertical 1

					cr.MoveTo(160*escala_en_linux_windows, comienzolinea2*escala_en_linux_windows);
					cr.LineTo(160,comienzo_linea+separacion_linea+4);		// vertical 2

					cr.MoveTo(240*escala_en_linux_windows, comienzolinea2*escala_en_linux_windows);
					cr.LineTo(240,comienzo_linea+separacion_linea+4);		// vertical 3

					cr.MoveTo(320*escala_en_linux_windows, comienzolinea2*escala_en_linux_windows);
					cr.LineTo(320,comienzo_linea+separacion_linea+4);		// vertical 4

					cr.MoveTo(400*escala_en_linux_windows, comienzolinea2*escala_en_linux_windows);
					cr.LineTo(400,comienzo_linea+separacion_linea+4);		// vertical 5

					cr.FillExtents();  //. FillPreserve(); 
					cr.SetSourceRGB (0, 0, 0);
					cr.LineWidth = 0.3;
					cr.Stroke();
				}else{

				}
			}catch (NpgsqlException ex){
				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
					MessageType.Error, ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
				msgBoxError.Run ();
				msgBoxError.Destroy();
				return; 
			}
		}
		
		void ejecutar_reporte_totaltipopx_empraseg(PrintContext context)
		{
			
		}
				
		void imprime_encabezado_portrait(Cairo.Context cr,Pango.Layout layout)
		{
			Pango.FontDescription desc = Pango.FontDescription.FromString ("Sans");								
			//cr.Rotate(90);  //Imprimir Orizontalmente rota la hoja cambian las posiciones de las lineas y columna					
			fontSize = 8.0;
			desc.Size = (int)(fontSize * pangoScale);					layout.FontDescription = desc;
			layout.FontDescription.Weight = Weight.Bold;		// Letra negrita
			//titulo_comprobante
			cr.MoveTo(05*escala_en_linux_windows,05*escala_en_linux_windows);			layout.SetText(classpublic.nombre_empresa);			Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(05*escala_en_linux_windows,15*escala_en_linux_windows);			layout.SetText(classpublic.direccion_empresa);		Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(05*escala_en_linux_windows,25*escala_en_linux_windows);			layout.SetText(classpublic.telefonofax_empresa);	Pango.CairoHelper.ShowLayout (cr, layout);
			fontSize = 6.0;
			desc.Size = (int)(fontSize * pangoScale);					layout.FontDescription = desc;
			cr.MoveTo(479*escala_en_linux_windows,05*escala_en_linux_windows);			layout.SetText("Fech.Rpt:"+(string) DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));		Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(479*escala_en_linux_windows,15*escala_en_linux_windows);			layout.SetText("N° Page : 1");		Pango.CairoHelper.ShowLayout (cr, layout);

			cr.MoveTo(05*escala_en_linux_windows,35*escala_en_linux_windows);			layout.SetText("Sistema Hospitalario OSIRIS");		Pango.CairoHelper.ShowLayout (cr, layout);
			// Cambiando el tamaño de la fuente			
			fontSize = 10.0;		
			desc.Size = (int)(fontSize * pangoScale);					layout.FontDescription = desc;
			layout.FontDescription.Weight = Weight.Bold;		// Letra negrita
			layout.Alignment = Pango.Alignment.Center;

			double width = context.Width;
			layout.Width = (int) width;
			layout.Alignment = Pango.Alignment.Center;
			//layout.Wrap = Pango.WrapMode.Word;
			//layout.SingleParagraphMode = true;
			layout.Justify =  false;
			cr.MoveTo(width/2,45*escala_en_linux_windows);	layout.SetText("RESUMEN_TOTAL_REGISTRO_DE_PACIENTES");	Pango.CairoHelper.ShowLayout (cr, layout);
			fontSize = 9.0;			layout = null;			layout = context.CreatePangoLayout ();
			desc.Size = (int)(fontSize * pangoScale);		layout.FontDescription = desc;
			layout.FontDescription.Weight = Weight.Bold;		// Letra negrita
			cr.MoveTo(05*escala_en_linux_windows, 65*escala_en_linux_windows);			layout.SetText("Rango de Fecha :"+entry_ano_inicial.Text+"-"+entry_mes_inicial.Text+"-"+entry_dia_inicial.Text+"   Hasta "+entry_ano_final.Text+"-"+entry_mes_final.Text+"-"+entry_dia_final.Text);				Pango.CairoHelper.ShowLayout (cr, layout);

		}
						
		private void OnEndPrint (object obj, Gtk.EndPrintArgs args)
		{
			
		}
	}
}