// created on 20/06/2010
///////////////////////////////////////////////////////////
// Sistema Hospitalario OSIRIS
// Monterrey - Mexico
//
// Autor    	: Ing. Daniel Olivares C. (Programacion Base y Ajustes)
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
/////////////////////////////////////////////////////////
using System;
using Npgsql;
using Gtk;
using Glade;
using Gdk;
using GLib;
using System.Collections;
using System.Text;
using System.Text.RegularExpressions;

namespace osiris
{
	public class notas_medicas
	{
		// Boton general para salir de las ventanas
		// Todas la ventanas en glade este boton debe estra declarado identico
		[Widget] Gtk.Button button_salir = null;
		
		[Widget] Gtk.Window notas_medicas_enfermeria = null;
		[Widget] Gtk.TextView textview1 = null;
		[Widget] Gtk.TextView textview2 = null;
		
		[Widget] Gtk.Entry entry_pid_paciente = null;
		[Widget] Gtk.Entry entry_nombre_paciente = null;
		[Widget] Gtk.Entry entry_edad_paciente = null;		
		[Widget] Gtk.Entry entry_numerotencion = null;
		[Widget] Gtk.Entry entry_fecha_nacimiento = null;
		[Widget] Gtk.Entry entry_sexo_paciente = null;
		[Widget] Gtk.Entry entry_id_doctor = null;
		[Widget] Gtk.Entry entry_doctor = null;
		
		// notas medicas 
		[Widget] Gtk.Button button_guardar = null;
		[Widget] Gtk.Button button_imprimir_notas = null;
		[Widget] Gtk.Entry entry_fechanotas = null;
		[Widget] Gtk.TreeView treeview_listanotas = null;
		[Widget] Gtk.ComboBox combobox_hora_nota = null;
		[Widget] Gtk.ComboBox combobox_minutos_nota = null;
		[Widget] Gtk.CheckButton checkbutton_selectall = null;
		[Widget] Gtk.Button button_interconsulta = null;		
		
		[Widget] Gtk.Notebook notebook1 = null;

		TextBuffer buffer = new TextBuffer(null);
		TextIter insertIter;
		
		TextBuffer bufferhisclinica = new TextBuffer(null);
		TextIter insertIterhc;

		TextBuffer bufferanalisis = new TextBuffer(null);
		TextIter insertIteranalisis;
				
		// Somatometria
		[Widget] Gtk.Entry entry_presion_arterial = null;
		[Widget] Gtk.SpinButton spinbutton_pulso = null;
		[Widget] Gtk.SpinButton spinbutton_frecrespiratoria = null;
		[Widget] Gtk.SpinButton spinbutton_temperatura = null;
		[Widget] Gtk.SpinButton spinbutton_sat_oxigeno = null;
		[Widget] Gtk.SpinButton spinbutton_peso = null;
		[Widget] Gtk.SpinButton spinbutton_talla = null;
		[Widget] Gtk.Entry entry_diuresis = null;
		[Widget] Gtk.Entry entry_evacuacion = null;
		[Widget] Gtk.ComboBox combobox_hora_svitales = null;
		[Widget] Gtk.ComboBox combobox_minutos_svitales = null;
		[Widget] Gtk.TreeView treeview_lista_svitales = null;
		[Widget] Gtk.Button button_guardar_somato = null;
		
		// Consulta Medica y Especialidades
		[Widget] Gtk.TreeView treeview_registro_soap = null;
		Gtk.TreeStore treeViewEngine_registro_soap = null;
				
		[Widget] Gtk.Button button_imprime_hc = null;
		[Widget] Gtk.TextView textview_subjetivo = null;
		[Widget] Gtk.TreeView treeview_objetivo1 = null;
		Gtk.TreeStore treeViewEngine_Obj1 = null;
		[Widget] Gtk.TreeView treeview_objetivo2 = null;
		Gtk.TreeStore treeViewEngine_Obj2 = null;
		[Widget] Gtk.TreeView treeview_objetivo3 = null;
		Gtk.TreeStore treeViewEngine_Obj3 = null;
		[Widget] Gtk.TextView textview_objetivo = null;
		[Widget] Gtk.TextView textview_objetivo2 = null;
		[Widget] Gtk.TextView textview_analisis = null;
		[Widget] Gtk.Entry entry_id_cie10 = null;
		[Widget] Gtk.Entry entry_descrip_cie10 = null;
		[Widget] Gtk.TreeView treeview_codigoscie10 = null;
		Gtk.ListStore treeViewEngine_codigoscie10 = null;
		[Widget] Gtk.TextView textview_pronostico = null;
		[Widget] Gtk.Button button_buscar_cie10 = null;
		[Widget] Gtk.TextView textview_plan = null;
		[Widget] Gtk.Notebook notebook_explfisica = null;
		[Widget] Gtk.CheckButton checkbutton_allexplor = null;
		[Widget] Gtk.Button button_guarda_soap = null;
		[Widget] Gtk.Button button_guarda_optometria = null;
		[Widget] Gtk.Button button_guarda_oftalmologia = null;
		[Widget] Gtk.TextView textview_hisclinica = null;
		[Widget] Gtk.Button button_hc_seleccionada = null;
		[Widget] Gtk.Button button_hc_limpiar = null;
		[Widget] Gtk.Button button_agrega_cie10 = null;
		[Widget] Gtk.Button button_imprime_soap = null;
		[Widget] Gtk.Button button_elimina_regsoap = null;
		[Widget] Gtk.Button button_receta_medica = null;
				
		[Widget] Gtk.RadioButton radiobutton_pron_ligevo = null;
		[Widget] Gtk.RadioButton radiobutton_pron_bueno = null;
		[Widget] Gtk.RadioButton radiobutton_pron_malo = null;
		[Widget] Gtk.RadioButton radiobutton_pron_reser = null;

		[Widget] Gtk.ScrolledWindow scrolledwindow37 = null;
				
		ArrayList columns = new ArrayList ();
		ArrayList arrayRecetaMedica = new ArrayList ();

		Gtk.TreeIter iter;		
			
		string connectionString;
		string nombrebd;
		string LoginEmpleado;
		string NomEmpleado;
		string AppEmpleado;
		string ApmEmpleado;
		string name_field;
		string pidpaciente;
		string folioservicio;
		string nombredoctor;
		string diagnosticoadmision;
		int idespecialidad;
		string nombreespecialidad;
		int idtipodeadmision;
		int idtitulosoap;
		int idtipointernamiento;
		string descripinternamiento;
		string direccionpx;
		string telefonopx;
		int idsubalmacen;
		string hora_nota = "";
		string minutos_nota = "";
		string hora_somatometria = "";
		string minutos_somatometria = "";
		string fecha_crea_expediente = "";
		string hora_crea_expediente = "";
		string tipopaciente = "";
		string convenio = "";
		int idempresa_paciente = 0;
		int idaseguradora_paciente = 0;
		
		string sql_general = "SELECT notas_de_enfermeria,notas_de_evolucion,indicaciones_medicas,nombre1_paciente,nombre2_paciente,apellido_paterno_paciente,apellido_materno_paciente,"+
							"to_char(to_number(to_char(age('"+DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss")+"',osiris_his_paciente.fecha_nacimiento_paciente),'yyyy') ,'9999'),'9999') AS edad," +
							"to_char(osiris_his_paciente.fecha_nacimiento_paciente,'dd-mm-yyyy') AS fechanacimiento_pac,"+
							"to_char(fecha_anotacion,'dd-MM-yyyy') AS fechaanotacion,hora_anotacion AS horaanotacion,osiris_his_informacion_medica.id_secuencia,"+
							"alegias_paciente,osiris_his_paciente.pid_paciente," +
							"to_char(osiris_his_paciente.fechahora_registro_paciente,'dd-mm-yyyy') AS fechacreaexpe," +
							"to_char(osiris_his_paciente.fechahora_registro_paciente,'HH24:mi') AS horacreaexpe,"+
							"to_char(osiris_erp_cobros_enca.fecha_alta_paciente,'dd-mm-yyyy HH:mi') AS fechadeegreso,osiris_his_paciente.sexo_paciente,"+
							"osiris_erp_cobros_enca.id_habitacion,osiris_his_habitaciones.descripcion_cuarto,osiris_his_habitaciones.numero_cuarto,"+
							"nombre1_empleado || ' ' || nombre2_empleado || ' ' || apellido_paterno_empleado || ' ' || apellido_materno_empleado AS nombreempleado "+
							"FROM osiris_his_informacion_medica,osiris_his_paciente,osiris_empleado,osiris_erp_cobros_enca,osiris_his_habitaciones "+
									"WHERE osiris_his_informacion_medica.pid_paciente = osiris_his_paciente.pid_paciente "+
									"AND osiris_his_informacion_medica.folio_de_servicio = osiris_erp_cobros_enca.folio_de_servicio "+
									"AND id_empleado_creacion = login_empleado "+
									"AND osiris_his_informacion_medica.eliminado = 'false' "+
									"AND osiris_erp_cobros_enca.id_habitacion = osiris_his_habitaciones.id_habitacion ";
		string sql_pidpaciente;
		string sql_folioservicio;
		string sql_filtronotasblanco;
		string ultimonumero_receta;
		
		private Gtk.TreeStore treeViewEngineListaNotas = null;
		private Gtk.TreeStore treeViewEnginesvitales = null;
				
		TreeViewColumn col_00;		CellRendererToggle cellrt00;
		TreeViewColumn col_01;		CellRendererText cellrt01;
		TreeViewColumn col_02;		CellRendererText cellrt02;
		TreeViewColumn col_03;		CellRendererText cellrt03;
		TreeViewColumn col_04;		CellRendererText cellrt04;
		TreeViewColumn col_05;		CellRendererText cellrt05;
						
		class_conexion conexion_a_DB = new class_conexion();
		class_public classpublic = new class_public();
		class_buscador classfind_data = new class_buscador();
		
		//Declaracion de ventana de error
		protected Gtk.Window MyWinError;
		protected Gtk.Window MyWin;
		
		public notas_medicas (string LoginEmp, string NomEmpleado_, string AppEmpleado_, string ApmEmpleado_,
		                      string title_window, string name_field_,string pidpaciente_,string folioservicio_,string iddoctor_,string nombredoctor_,string nombrepaciente_,
		                      bool altapaciente_,string edadpaciente_, string diagnosticoadmision_, int idespecialidad_,string nombreespecialidad_,int idtipodeadmision_,
							  int idtitulosoap_,int idtipointernamiento_,string descripinternamiento_,string fechanac_,string sexopaciente_,string direccionpx_,
		                      string telefonopx_,int idsubalmacen_,string tipopaciente_, string convenio_)
		{
			connectionString = conexion_a_DB._url_servidor+conexion_a_DB._port_DB+conexion_a_DB._usuario_DB+conexion_a_DB._passwrd_user_DB;
			nombrebd = conexion_a_DB._nombrebd;
			LoginEmpleado = LoginEmp;
			NomEmpleado = NomEmpleado_;
			AppEmpleado = AppEmpleado_;
			ApmEmpleado = ApmEmpleado_;
			name_field = name_field_;
			pidpaciente = pidpaciente_;
			folioservicio = folioservicio_;
			nombredoctor = nombredoctor_;
			diagnosticoadmision = diagnosticoadmision_;
			idespecialidad = idespecialidad_;
			nombreespecialidad = nombreespecialidad_;
			idtipodeadmision = idtipodeadmision_;
			idtitulosoap = idtitulosoap_;
			idtipointernamiento = idtipointernamiento_;
			descripinternamiento = descripinternamiento_;
			direccionpx = direccionpx_;
			telefonopx = telefonopx_;
			idsubalmacen = idsubalmacen_;
			tipopaciente = tipopaciente_;
			convenio = convenio_;
			idempresa_paciente = int.Parse ((string)classpublic.lee_registro_de_tabla ("osiris_erp_cobros_enca", "id_empresa", "WHERE folio_de_servicio = '" + folioservicio_ + "' ", "id_empresa", "string"));
			idaseguradora_paciente = int.Parse ((string)classpublic.lee_registro_de_tabla ("osiris_erp_cobros_enca", "id_aseguradora", "WHERE folio_de_servicio = '" + folioservicio_ + "' ", "id_aseguradora", "string"));
								
			sql_pidpaciente = " AND osiris_his_informacion_medica.pid_paciente = '"+pidpaciente+"' ";
			sql_folioservicio = " AND osiris_his_informacion_medica.folio_de_servicio = '"+folioservicio+"' ";
			sql_filtronotasblanco = " AND "+name_field+" <> '' ";
						
			Glade.XML gxml = new Glade.XML (null, "hospitalizacion.glade", "notas_medicas_enfermeria", null);
			gxml.Autoconnect (this);
			notas_medicas_enfermeria.Show();
			//notas_medicas_enfermeria.Maximize();
			notas_medicas_enfermeria.SetPosition(WindowPosition.Center);	// centra la ventana en la pantalla
			notas_medicas_enfermeria.Title = title_window;
			button_salir.Clicked += new EventHandler(on_cierraventanas_clicked);
			button_guardar.Clicked += new EventHandler(on_button_guardar_clicked);
			button_imprimir_notas.Clicked += new EventHandler(on_button_imprimir_notas_clicked);
			checkbutton_selectall.Clicked += new EventHandler(on_checkbutton_selectall_clicked);
			entry_fechanotas.Text = (string) DateTime.Now.ToString("yyyy-MM-dd");
			entry_pid_paciente.Text = (string) pidpaciente;
			entry_nombre_paciente.Text = (string) nombrepaciente_;
			entry_numerotencion.Text = (string) folioservicio;
			entry_id_doctor.Text = (string) iddoctor_;
			entry_doctor.Text = (string) nombredoctor_;
			entry_edad_paciente.Text = (string) edadpaciente_;
			entry_numerotencion.IsEditable = false;
			entry_fecha_nacimiento.Text = fechanac_;
			if(sexopaciente_ == "F"){
				entry_sexo_paciente.Text = "FEMENINO";
			}
			if(sexopaciente_ == "M"){
				entry_sexo_paciente.Text = "FEMENINO";
			}
			if(sexopaciente_ == "H"){
				entry_sexo_paciente.Text = "MASCULINO";
			}
			
			// action somatometria
			button_guardar_somato.Clicked += new EventHandler(on_button_guardar_somato_clicked);
			// soap 
			button_guarda_soap.Clicked += new EventHandler(on_button_guarda_soap_clicked);
			// Optometria
			button_guarda_optometria.Clicked += new EventHandler(on_button_guarda_soap_clicked);
			// Oftalmologia
			button_guarda_oftalmologia.Clicked += new EventHandler(on_button_guarda_soap_clicked);
			button_hc_seleccionada.Clicked += new EventHandler(on_button_hc_seleccionada_clicked);
			button_hc_limpiar.Clicked += new EventHandler(on_button_hc_limpiar_clicked);
			button_imprime_soap.Clicked += new EventHandler(on_button_imprime_soap_clicked);
			button_elimina_regsoap.Clicked += new EventHandler(on_button_elimina_regsoap_clicked);
			button_interconsulta.Clicked += new EventHandler(on_button_interconsulta_clicked);
			entry_numerotencion.ModifyBase(StateType.Normal, new Gdk.Color(140,255,170)); // Color Verde Claro
			entry_id_doctor.ModifyBase(StateType.Normal, new Gdk.Color(140,255,170)); // Color Verde Claro
			entry_doctor.ModifyBase(StateType.Normal, new Gdk.Color(140,255,170)); // Color Verde Claro
			entry_pid_paciente.ModifyBase(StateType.Normal, new Gdk.Color(140,255,170)); // Color Verde Claro
			entry_nombre_paciente.ModifyBase(StateType.Normal, new Gdk.Color(140,255,170)); // Color Verde Claro
			entry_edad_paciente.ModifyBase(StateType.Normal, new Gdk.Color(140,255,170)); // Color Verde Claro
			entry_fecha_nacimiento.ModifyBase(StateType.Normal, new Gdk.Color(140,255,170)); // Color Verde Claro
			entry_sexo_paciente.ModifyBase(StateType.Normal, new Gdk.Color(140,255,170)); // Color Verde Claro
				
			bufferhisclinica = textview_hisclinica.Buffer;
			insertIterhc = bufferhisclinica.StartIter;
			classpublic.CreateTags(bufferhisclinica);
			bufferhisclinica.Clear();

			bufferanalisis = textview_analisis.Buffer;
			insertIteranalisis = bufferanalisis.StartIter;
			classpublic.CreateTags(bufferanalisis);
			bufferanalisis.Clear();
			
			crea_treeview_notas();
			crea_treeview_svitales();
			crea_treeview_objetivo1();
			crea_treeview_objetivo2();
			crea_treeview_objetivo3();
			crea_treeview_soap();
			crea_treeview_codigoscie10();

			// Cambiando el color del fondo para distinguir la ventana
			switch (name_field){	
				case "notas_de_evolucion":
					textview1.ModifyBase(StateType.Normal, new Gdk.Color(255,243,169)); // Color Amarillo
					llenando_informacion();
				break;
				case "notas_de_enfermeria":
					textview1.ModifyBase(StateType.Normal, new Gdk.Color(255,179,235)); // Color Rosa
					llenando_informacion();
				break;
				case "indicaciones_medicas":
					textview1.ModifyBase(StateType.Normal, new Gdk.Color(152,255,255)); // Color Celeste
					llenando_informacion();
				break;
				case "consulta_medica":
					// exploracion fisica de espacialidades
					button_guardar.Sensitive = false;
					textview2.Sensitive = false;
					entry_fechanotas.Sensitive = false;
					combobox_hora_nota.Sensitive = false;
					combobox_minutos_nota.Sensitive = false;
					button_guardar_somato.Sensitive = true;
					button_buscar_cie10.Clicked += new EventHandler(on_button_buscar_cie10_clicked);
					checkbutton_allexplor.Clicked += new EventHandler(on_checkbutton_selectall_clicked);
					button_agrega_cie10.Clicked += new EventHandler(on_button_agrega_cie10_clicked);
					button_receta_medica.Clicked += new EventHandler(on_button_receta_medica_clicked);
					notebook1.CurrentPage = 4;
					//notebook_explfisica.CurrentPage = 2;
				break;				
			}
			
			llena_horas_notas();
			llenado_treeview_objetivo1();
			llenado_treeview_objetivo2();	
			llenado_treeview_objetivo3();
			llenado_treeview_soap();
			llenado_template_cie10();
			llenado_signos_vitales ();
			if (altapaciente_ == false){
				button_guardar.Sensitive = false;
				textview2.Sensitive = false;
				entry_fechanotas.Sensitive = false;
				combobox_hora_nota.Sensitive = false;
				combobox_minutos_nota.Sensitive = false;
				//button_guardar_somato.Sensitive = false;
			}			
			/*
			// Creando un tab nuevo
			string titulo_tab = "Tab";
			string titulo_frame = "Titulo Frame";
			Gtk.Frame frame = new Gtk.Frame();
			frame.Label = titulo_frame;
			frame.Show();
			Gtk.Label label_notebook = new Gtk.Label();
			label_notebook.Text = titulo_tab;
			label_notebook.Show();
			notebook_explfisica.InsertPage(frame,label_notebook,3);
			notebook_explfisica.CurrentPage = 2;
			*/
		}

		public struct struct_recetamedica
		{
			#region Fields
			private string _col00_receta;
			private string _col01_receta;
			private string _col02_receta;
			private string _col03_receta;
			private string _col04_receta;
			#endregion

			#region Properties
			public string col00_receta{
				get { return _col00_receta; }
				set { _col00_receta = value; }
			}
			public string col01_receta{
				get { return _col01_receta; }
				set { _col01_receta = value; }
			}
			public string col02_receta{
				get { return _col02_receta; }
				set { _col02_receta = value; }
			}
			public string col03_receta{
				get { return _col03_receta; }
				set { _col03_receta = value; }
			}
			public string col04_receta {
				get { return _col04_receta; }
				set { _col04_receta = value; }
			}
			#endregion

			public struct_recetamedica (string col00_receta_,string col01_receta_,string col02_receta_,string col03_receta_,string col04_receta_)
			{
				_col00_receta = col00_receta_;
				_col01_receta = col01_receta_;
				_col02_receta = col02_receta_;
				_col03_receta = col03_receta_;
				_col04_receta = col04_receta_;
			}
		}
		
		void on_button_receta_medica_clicked(object sender, EventArgs args)
		{
			new osiris.receta_medica(LoginEmpleado,NomEmpleado,AppEmpleado,ApmEmpleado,folioservicio,entry_id_doctor.Text,entry_doctor.Text,
			                         pidpaciente,entry_nombre_paciente.Text,entry_edad_paciente.Text,entry_fecha_nacimiento.Text,
				entry_sexo_paciente.Text,idsubalmacen,textview_plan,arrayRecetaMedica);
		}
		
		void on_button_imprimir_notas_clicked(object sender, EventArgs args)
		{
			string numeros_seleccionado = "";
			string almacenes_seleccionados = ""; 
			string variable_paso_03 = "";
			int variable_paso_02_1 = 0;
			string query_in_num = "";
			
			//poder elegir una fila del treeview
			TreeIter iter;
			if (treeViewEngineListaNotas.GetIterFirst (out iter)){			
 				if ((bool) treeview_listanotas.Model.GetValue (iter,0) == true){
					numeros_seleccionado = (string) treeview_listanotas.Model.GetValue (iter,1);
 					variable_paso_02_1 += 1;		
 				}
 				while (treeViewEngineListaNotas.IterNext(ref iter)){
 					if ((bool) treeview_listanotas.Model.GetValue (iter,0) == true){
						if (variable_paso_02_1 == 0){ 				    	
 							numeros_seleccionado = (string) treeview_listanotas.Model.GetValue (iter,1);
 							variable_paso_02_1 += 1;
 						}else{
 							variable_paso_03 = (string) treeview_listanotas.Model.GetValue (iter,1);
 							numeros_seleccionado = numeros_seleccionado.Trim() + "','" + variable_paso_03.Trim();
 						}
 					}
 				}
 			}
			if (variable_paso_02_1 > 0){
	 			query_in_num = " AND id_secuencia IN('"+numeros_seleccionado+"') ";
			}
			if ( treeViewEngineListaNotas.GetIterFirst (out iter)){
				if (variable_paso_02_1 > 0){
					//Console.WriteLine(query_in_num);
					new osiris.rpt_notas_medicas(folioservicio,name_field,sql_general+sql_pidpaciente+sql_folioservicio+sql_filtronotasblanco+query_in_num+" ORDER BY to_char(fecha_anotacion,'yyyy-MM-dd'),hora_anotacion DESC;", diagnosticoadmision);
				}
			}
		}
		
		void llenando_informacion()
		{
			buffer = textview1.Buffer;
			classpublic.CreateTags(buffer);
			insertIter = buffer.StartIter;
			buffer.Clear();
			// elimina las columnas del treeview
			//foreach (TreeViewColumn tvc in treeview_listanotas.Columns)
			//				treeview_listanotas.RemoveColumn(tvc);
			treeViewEngineListaNotas.Clear();
			
			NpgsqlConnection conexion; 
			conexion = new NpgsqlConnection (connectionString+nombrebd);            
			// Verifica que la base de datos este conectada
			try{
				conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand ();
               	
				// asigna el numero de folio de ingreso de paciente (FOLIO)
				comando.CommandText = sql_general+sql_pidpaciente+sql_folioservicio+sql_filtronotasblanco+" ORDER BY to_char(fecha_anotacion,'yyyy-MM-dd'),hora_anotacion DESC;";
				//Console.WriteLine(comando.CommandText);					
				NpgsqlDataReader lector = comando.ExecuteReader ();
				
				if(lector.Read()){
					fecha_crea_expediente = lector["fechacreaexpe"].ToString().Trim();
					hora_crea_expediente = lector["horacreaexpe"].ToString().Trim();
					entry_pid_paciente.Text = pidpaciente;
					entry_nombre_paciente.Text = (string) lector["nombre1_paciente"].ToString().Trim()+" "+
											(string) lector["nombre2_paciente"].ToString().Trim()+" "+
											(string) lector["apellido_paterno_paciente"].ToString().Trim()+" "+
											(string) lector["apellido_materno_paciente"].ToString().Trim();
					entry_edad_paciente.Text = (string) lector["edad"].ToString();
					entry_numerotencion.Text = folioservicio.Trim();
					entry_doctor.Text = nombredoctor;
					if((string) lector[name_field].ToString() != ""){
						buffer.InsertWithTagsByName (ref insertIter, "Fecha de Nota: "+(string) lector["fechaanotacion"].ToString().Trim()+"    Nº de NOTA :"+(string) lector["id_secuencia"].ToString().Trim()+"\n", "bold");
						buffer.InsertWithTagsByName (ref insertIter, "Hora de Nota : "+(string) lector["horaanotacion"].ToString().Trim()+" \n\n", "bold");
						buffer.Insert (ref insertIter, (string) lector[name_field].ToString().ToUpper()+"\n\n\n");
						treeViewEngineListaNotas.AppendValues(false,
						                                      (string) lector["id_secuencia"].ToString().Trim(),
						                                      (string) lector["fechaanotacion"].ToString().Trim(),
						                                      (string) lector["horaanotacion"].ToString().Trim(),
						                                      (string) lector["nombreempleado"].ToString().Trim());
						
					}
					while(lector.Read()){
						if((string) lector[name_field].ToString() != ""){
							buffer.InsertWithTagsByName (ref insertIter, "Fecha de Nota: "+(string) lector["fechaanotacion"].ToString().Trim()+"    Nº de NOTA :"+(string) lector["id_secuencia"].ToString().Trim()+"\n", "bold");
							buffer.InsertWithTagsByName (ref insertIter, "Hora de Nota : "+(string) lector["horaanotacion"].ToString().Trim()+" \n\n", "bold");
							buffer.Insert (ref insertIter, (string) lector[name_field].ToString().ToUpper()+"\n\n\n");
							treeViewEngineListaNotas.AppendValues(false,
							                                      (string) lector["id_secuencia"].ToString().Trim(),
							                                      (string) lector["fechaanotacion"].ToString().Trim(),
							                                      (string) lector["horaanotacion"].ToString().Trim(),
							                                      (string) lector["nombreempleado"].ToString().Trim());
						}
					}
				}
			}catch (NpgsqlException ex){
				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.Modal,
						MessageType.Error, 
						ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
				msgBoxError.Run ();				msgBoxError.Destroy();
	   			
	       	}
       		conexion.Close ();
			//buffer.InsertWithTagsByName (ref insertIter, "\nThis line has center justification.\n", "center")
		}
		
		void crea_treeview_notas()
		{
			treeViewEngineListaNotas = new TreeStore( typeof(bool),
													typeof(string),
													typeof(string),
			                                        typeof(string),
													typeof(string),
													typeof(string),
													typeof(string),
													typeof(string),
													typeof(string));
				
			treeview_listanotas.Model = treeViewEngineListaNotas;
			treeview_listanotas.RulesHint = true;
			
			col_00 = new TreeViewColumn();
			cellrt00 = new CellRendererToggle();
			col_00.Title = "Selecciona"; // titulo de la cabecera de la columna, si está visible
			col_00.PackStart(cellrt00, true);
			col_00.AddAttribute (cellrt00, "active", 0);
			cellrt00.Activatable = true;
			cellrt00.Toggled += selecciona_fila;
			//col_00.SortColumnId = (int) Column_notas.col_00;
			
			col_01 = new TreeViewColumn();
			cellrt01 = new CellRendererText();
			col_01.Title = "N° Nota"; // titulo de la cabecera de la columna, si está visible
			col_01.PackStart(cellrt01, true);
			col_01.AddAttribute (cellrt01, "text", 1);
			//col_01.SortColumnId = (int) Column_notas.col_01;
			
			col_02 = new TreeViewColumn();
			cellrt02 = new CellRendererText();
			col_02.Title = "Fecha Nota"; // titulo de la cabecera de la columna, si está visible
			col_02.PackStart(cellrt02, true);
			col_02.AddAttribute (cellrt02, "text", 2);
			//col_03.SortColumnId = (int) Column_notas.col_01;
			
			col_03 = new TreeViewColumn();
			cellrt03 = new CellRendererText();
			col_03.Title = "Hora Nota"; // titulo de la cabecera de la columna, si está visible
			col_03.PackStart(cellrt03, true);
			col_03.AddAttribute (cellrt03, "text", 3);
			//col_03.SortColumnId = (int) Column_notas.col_02;
			
			col_04 = new TreeViewColumn();
			cellrt04 = new CellRendererText();
			col_04.Title = "Quien Realizo"; // titulo de la cabecera de la columna, si está visible
			col_04.PackStart(cellrt04, true);
			col_04.AddAttribute (cellrt04, "text", 4);
			//col_03.SortColumnId = (int) Column_notas.col_03;
			
			treeview_listanotas.AppendColumn(col_00);
			treeview_listanotas.AppendColumn(col_01);
			treeview_listanotas.AppendColumn(col_02);
			treeview_listanotas.AppendColumn(col_03);
			treeview_listanotas.AppendColumn(col_04);
		}
		
		void crea_treeview_svitales()
		{
			object [] parametros = {treeview_lista_svitales,treeViewEnginesvitales};
			string[,] coltreeview = {{"Fecha","text"},
									{"Hora","text"},
									{"Tomado en","text"},
									{"Tension Arte.","text"},
									{"Pulso","text"},
									{"Fr.Resp.","text"},
									{"Temp.","text"},
									{"Peso","text"},
									{"Talla","text"},
									{"Sat. Oxigeno","text"},
									{"Diuresis","text"},
									{"Evacuacion","text"}};
			crea_colums_treeview_svitales(parametros,coltreeview);
			//new osiris.crea_colums_treeview(parametros,coltreeview);
		}

		void llenado_signos_vitales ()
		{
			treeViewEnginesvitales.Clear();
			NpgsqlConnection conexion; 
			conexion = new NpgsqlConnection (connectionString+nombrebd);
			// Verifica que la base de datos este conectada
			try{
				conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand ();               	
				comando.CommandText = "SELECT *,to_char(fecha_somatometria,'yyyy-MM-dd') as fechasomatometria,hora_somatometria "+
										"FROM osiris_his_somatometria "+
										"WHERE pid_paciente = '"+pidpaciente+"';";
				//Console.WriteLine(comando.CommandText);
				NpgsqlDataReader lector = comando.ExecuteReader ();
				while(lector.Read()){
					treeViewEnginesvitales.AppendValues(lector["fechasomatometria"].ToString().Trim(),
								lector["hora_somatometria"].ToString().Trim(),
									"",
								lector["tension_arterial"].ToString().Trim(),
								lector["pulso"].ToString().Trim(),
								lector["frecuencia_respiratoria"].ToString().Trim(),
								lector["temperatura"].ToString().Trim(),
								lector["peso"].ToString().Trim(),
								lector["talla"].ToString().Trim(),
								lector["saturacion_oxigeno"].ToString().Trim(),
								lector["diuresis"].ToString().Trim(),
								lector["evacuacion"].ToString().Trim()
					);
				}
			}catch (NpgsqlException ex){
				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
					MessageType.Error,ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
				msgBoxError.Run ();				msgBoxError.Destroy();
			}
			conexion.Close ();
		}

		void crea_colums_treeview_svitales(object[] args,string [,] args_colums)
		{
			Gtk.CellRendererText text;
			Gtk.CellRendererToggle toggle;
			ArrayList columns = new ArrayList ();

			Gtk.TreeView treeviewobject = (object) args[0] as Gtk.TreeView;
			Gtk.TreeStore treeviewEngine = (object) args[1] as Gtk.TreeStore;

			treeViewEnginesvitales = new TreeStore(typeof (string),	typeof (string),
													typeof (string),typeof (string),
													typeof (string),typeof (string),
													typeof (string),typeof (string),
													typeof (string),typeof (string),
													typeof (string),typeof (string),
													typeof (bool),typeof (bool));

			treeview_lista_svitales.Model = treeViewEnginesvitales;
			treeview_lista_svitales.RulesHint = true;
			treeview_lista_svitales.Selection.Mode = SelectionMode.Multiple;
			if(args_colums.GetUpperBound(0) >= 0){
				for (int colum_field = 0; colum_field <= args_colums.GetUpperBound (0); colum_field++) {
					if((string) args_colums [colum_field, 1] == "text"){
						// column for holiday names
						text = new CellRendererText ();
						text.Xalign = 0.0f;
						columns.Add (text);
						TreeViewColumn column0 = new TreeViewColumn((string) args_colums [colum_field, 0], text,"text",colum_field);
						column0.Resizable = true;
						//column0.SortColumnId = colum_field;
						treeviewobject.InsertColumn (column0, colum_field);					
					}
					if((string) args_colums [colum_field, 1] == "toogle"){

					}
				}
			}
		}
				
		void selecciona_fila(object sender, ToggledArgs args)
		{
			TreeIter iter;
			if (treeview_listanotas.Model.GetIter (out iter,new TreePath (args.Path))) {
				bool old = (bool) treeview_listanotas.Model.GetValue (iter,0);
				treeview_listanotas.Model.SetValue(iter,0,!old);
			}	
		}

		void on_button_guardar_somato_clicked(object sender, EventArgs args)
		{
			if(hora_somatometria != "" && minutos_somatometria != ""){
				MessageDialog msgBox = new MessageDialog (MyWin,DialogFlags.Modal,
					MessageType.Question,ButtonsType.YesNo,"¿ Esta seguro de Almacenar SIGNOS VITALES?");
				ResponseType miResultado = (ResponseType)
					msgBox.Run ();				msgBox.Destroy();
				if (miResultado == ResponseType.Yes){
					int foliointernoespmed = int.Parse(classpublic.lee_ultimonumero_registrado("osiris_his_informacion_medica","secuencia_interna","WHERE pid_paciente = '" +entry_pid_paciente.Text.ToString().Trim()+"'"));
					int idtituloexplofisica = 0;
					NpgsqlConnection conexion; 
					conexion = new NpgsqlConnection (connectionString+nombrebd);
					try{
						conexion.Open ();
						NpgsqlCommand comando; 
						comando = conexion.CreateCommand();

						comando.CommandText = "INSERT INTO osiris_his_somatometria(" +
							"pid_paciente," +
							"folio_de_servicio,"+
							"fechahora_creacion," +
							"fecha_somatometria,"+
							"id_empleado_creacion," +
							"hora_somatometria,"+
							"tension_arterial," +
							"pulso," +
							"frecuencia_respiratoria," +
							"temperatura," +
							"saturacion_oxigeno,"+
							"diuresis," +
							"evacuacion," +
							"id_tipo_admisiones,"+
							"peso,"+
							"talla"+
							") VALUES ('"+
							(string) entry_pid_paciente.Text.ToString().Trim()+"','"+
							(string) entry_numerotencion.Text.ToString().Trim()+"','"+
							DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"','"+
							DateTime.Now.ToString("yyyy-MM-dd")+"','"+
							LoginEmpleado+"','"+
							hora_somatometria+":"+minutos_somatometria+"','"+
							entry_presion_arterial.Text.Trim()+"','"+
							spinbutton_pulso.Text.Trim()+"','"+
							spinbutton_frecrespiratoria.Text.Trim()+"','"+
							spinbutton_temperatura.Text.Trim()+"','"+
							spinbutton_sat_oxigeno.Text.Trim()+"','"+
							entry_diuresis.Text.Trim().ToUpper()+"','"+
							entry_evacuacion.Text.Trim().ToUpper()+"','"+
							idtipointernamiento.ToString().Trim()+"','"+
							spinbutton_peso.Text.Trim()+"','"+
							spinbutton_talla.Text.Trim()+
							"');";
						//Console.WriteLine(comando.CommandText);
						comando.ExecuteNonQuery();
						comando.Dispose();

						guarda_soap(2,foliointernoespmed,5);
						MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,MessageType.Info,
							ButtonsType.Close, "Los datos se guardaron con EXITO");
						msgBoxError.Run ();			msgBoxError.Destroy();						
					}catch (NpgsqlException ex){
						MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.Modal,
							MessageType.Error, 
							ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
						msgBoxError.Run ();					msgBoxError.Destroy();	   			
					}
					conexion.Close ();
				}
			}else{
				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.Modal,
					MessageType.Error,ButtonsType.Close,"La Somatometria no tiene hora o minutos, verifique...");
				msgBoxError.Run ();						msgBoxError.Destroy();
			}
		}
		
		void on_button_guardar_clicked(object sender, EventArgs args)
		{
			if(textview2.Buffer.Text.ToString()!=""){				
				if(hora_nota != "" && minutos_nota != ""){
					MessageDialog msgBox = new MessageDialog (MyWin,DialogFlags.Modal,
												MessageType.Question,ButtonsType.YesNo,"¿ Esta seguro de Almacenar esta NOTA ?");
					ResponseType miResultado = (ResponseType)
					msgBox.Run ();				msgBox.Destroy();
		 			if (miResultado == ResponseType.Yes){
						int foliointernoespmed = int.Parse(classpublic.lee_ultimonumero_registrado("osiris_his_informacion_medica","secuencia_interna","WHERE pid_paciente = '" +entry_pid_paciente.Text.ToString().Trim()+"'"));
						NpgsqlConnection conexion; 
						conexion = new NpgsqlConnection (connectionString+nombrebd);
		            	try{
							conexion.Open ();
							NpgsqlCommand comando; 
							comando = conexion.CreateCommand();
							comando.CommandText = "INSERT INTO osiris_his_informacion_medica(" +
								"pid_paciente," +
								"folio_de_servicio,"+
								"fechahora_creacion," +
								"id_empleado_creacion," +
								"id_medico,"+
								name_field+"," +
								"fecha_anotacion," +
								"hora_anotacion," +
								"secuencia_interna"+
							") VALUES ('"+
								(string) entry_pid_paciente.Text.ToString().Trim()+"','"+
								(string) entry_numerotencion.Text.ToString().Trim()+"','"+
								DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"','"+
								LoginEmpleado+"','"+
								(string) entry_id_doctor.Text.ToString().Trim()+"','"+
								textview2.Buffer.Text.ToString().ToUpper()+"','"+
								DateTime.Now.ToString("yyyy-MM-dd")+"','"+
								hora_nota+":"+minutos_nota+"','"+
								foliointernoespmed.ToString().Trim()+"')";
							//Console.WriteLine(comando.CommandText);
							comando.ExecuteNonQuery();
							comando.Dispose();
							MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,MessageType.Info,
					 							ButtonsType.Close, "Los datos se guardaron con EXITO");
							msgBoxError.Run ();			msgBoxError.Destroy();
							textview2.Buffer.Clear();
							llenando_informacion();
						}catch (NpgsqlException ex){
							MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.Modal,
								MessageType.Error, 
								ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
							msgBoxError.Run ();					msgBoxError.Destroy();	   			
			       		}
		       			conexion.Close ();      							
					}			
					//Console.WriteLine(textview2.Buffer.Text.ToString());
				}else{
					MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.Modal,
										MessageType.Error,ButtonsType.Close,"La nota no tiene hora o minutos, verifique...");
					msgBoxError.Run ();						msgBoxError.Destroy();	
				}
			}else{
				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.Modal,
										MessageType.Error,ButtonsType.Close,"La nota no contiene informacion, verifique...");
				msgBoxError.Run ();						msgBoxError.Destroy();
			}
		}
		
		void llena_horas_notas()
		{
			combobox_hora_nota.Clear();
			CellRendererText cell2 = new CellRendererText();
			combobox_hora_nota.PackStart(cell2, true);
			combobox_hora_nota.AddAttribute(cell2,"text",0);	        
			ListStore store2 = new ListStore( typeof (string), typeof (int));
			combobox_hora_nota.Model = store2;
			for(int i = 1; i < (int)classpublic.horario_24_horas+1 ; i++){				
				store2.AppendValues ((string)i.ToString("00").Trim());
			}
			combobox_hora_nota.Changed += new EventHandler (onComboBoxChanged_hora_minutos_cita);			
			
			combobox_minutos_nota.Clear();
			CellRendererText cell3 = new CellRendererText();
			combobox_minutos_nota.PackStart(cell3, true);
			combobox_minutos_nota.AddAttribute(cell3,"text",0);	        
			ListStore store3 = new ListStore( typeof (string), typeof (int));
			combobox_minutos_nota.Model = store3;			
			for(int i = (int) 0; i < 60 ; i=i+(int) classpublic.intervalo_minutos){				
				store3.AppendValues ((string)i.ToString("00").Trim());
			}
			combobox_minutos_nota.Changed += new EventHandler (onComboBoxChanged_hora_minutos_cita);
			
			combobox_hora_svitales.Clear();
			CellRendererText cell4 = new CellRendererText();
			combobox_hora_svitales.PackStart(cell4, true);
			combobox_hora_svitales.AddAttribute(cell4,"text",0);	        
			ListStore store4 = new ListStore( typeof (string), typeof (int));
			combobox_hora_svitales.Model = store4;
			for(int i = 1; i < (int)classpublic.horario_24_horas+1 ; i++){				
				store4.AppendValues ((string)i.ToString("00").Trim());
			}
			combobox_hora_svitales.Changed += new EventHandler (onComboBoxChanged_hora_minutos_cita);
			
			combobox_minutos_svitales.Clear();
			CellRendererText cell5 = new CellRendererText();
			combobox_minutos_svitales.PackStart(cell5, true);
			combobox_minutos_svitales.AddAttribute(cell5,"text",0);	        
			ListStore store5 = new ListStore( typeof (string), typeof (int));
			combobox_minutos_svitales.Model = store5;			
			for(int i = (int) 0; i < 60 ; i=i+(int) classpublic.intervalo_minutos){				
				store5.AppendValues ((string)i.ToString("00").Trim());
			}
			combobox_minutos_svitales.Changed += new EventHandler (onComboBoxChanged_hora_minutos_cita);
		}
		
		void onComboBoxChanged_hora_minutos_cita(object sender, EventArgs args)
		{
			//Gtk.ComboBox hora_minutos_cita = (Gtk.ComboBox) sender;
			Gtk.ComboBox hora_minutos = sender as Gtk.ComboBox;			
			if (sender == null){
				return;
			}
			TreeIter iter;
			if (hora_minutos.GetActiveIter (out iter)){
				if(hora_minutos.Name.ToString() == "combobox_hora_nota"){				
					hora_nota = (string) hora_minutos.Model.GetValue(iter,0);
				}			
				if(hora_minutos.Name.ToString() == "combobox_minutos_nota"){
					minutos_nota = (string) hora_minutos.Model.GetValue(iter,0);
				}
				if(hora_minutos.Name.ToString() == "combobox_hora_svitales"){
					hora_somatometria = (string) hora_minutos.Model.GetValue(iter,0);
				}			
				if(hora_minutos.Name.ToString() == "combobox_minutos_svitales"){
					minutos_somatometria = (string) hora_minutos.Model.GetValue(iter,0);
				}
			}
		}

		void on_button_interconsulta_clicked(object sender, EventArgs args)
		{
			new osiris.interconsula_visimed(LoginEmpleado,NomEmpleado,AppEmpleado,ApmEmpleado,
			                                folioservicio,pidpaciente.ToString().Trim(),entry_nombre_paciente.Text,entry_edad_paciente.Text,entry_fecha_nacimiento.Text,
				entry_sexo_paciente.Text,this.idsubalmacen,this.entry_id_doctor.Text.Trim(),nombreespecialidad,nombreespecialidad,idtipointernamiento,true);
		}
		
		//Seleccionar todos los del treeview, un check_button 
		void on_checkbutton_selectall_clicked(object sender, EventArgs args)
		{
			Gtk.CheckButton checksender = sender as Gtk.CheckButton;
			TreeIter iter2;
			if(checksender.Name == "checkbutton_selectall"){			
				if ((bool) checksender.Active == true){
					if (treeViewEngineListaNotas.GetIterFirst (out iter2)){
						treeview_listanotas.Model.SetValue(iter2,0,true);
						while (treeViewEngineListaNotas.IterNext(ref iter2)){
							treeview_listanotas.Model.SetValue(iter2,0,true);
						}
					}
				}else{
					if (treeViewEngineListaNotas.GetIterFirst (out iter2)){
						treeview_listanotas.Model.SetValue(iter2,0,false);
						while (treeViewEngineListaNotas.IterNext(ref iter2)){
							treeview_listanotas.Model.SetValue(iter2,0,false);
						}
					}
				}
			}
			if(checksender.Name == "checkbutton_allexplor"){
				if ((bool) checksender.Active == true){
					if (treeViewEngine_Obj3.GetIterFirst (out iter2)){
						treeview_objetivo3.Model.SetValue(iter2,0,true);
						treeview_objetivo3.Model.SetValue(iter2,2,(string) treeview_objetivo3.Model.GetValue(iter2,4));
						treeview_objetivo3.Model.SetValue(iter2,3,(string) treeview_objetivo3.Model.GetValue(iter2,4));						
						while(treeViewEngine_Obj3.IterNext(ref iter2)){
							treeview_objetivo3.Model.SetValue(iter2,0,true);
							treeview_objetivo3.Model.SetValue(iter2,2,(string) treeview_objetivo3.Model.GetValue(iter2,4));
							treeview_objetivo3.Model.SetValue(iter2,3,(string) treeview_objetivo3.Model.GetValue(iter2,4));
						}
					}
				}else{
					if (treeViewEngine_Obj3.GetIterFirst (out iter2)){
						treeview_objetivo3.Model.SetValue(iter2,0,false);
						treeview_objetivo3.Model.SetValue(iter2,2,"");
						treeview_objetivo3.Model.SetValue(iter2,3,"");
						while(treeViewEngine_Obj3.IterNext(ref iter2)){
							treeview_objetivo3.Model.SetValue(iter2,0,false);
							treeview_objetivo3.Model.SetValue(iter2,2,"");
							treeview_objetivo3.Model.SetValue(iter2,3,"");
						}
					}
				}
			}
		}
				
		void on_button_guarda_soap_clicked(object sender, EventArgs args)
		{
			Gtk.Button buttonsender = sender as Gtk.Button;
			//Console.Write(buttonsender.Name);
			ultimonumero_receta = "1";
			switch (buttonsender.Name){	
				case "button_guarda_soap":
					int foliointernoespmed = int.Parse(classpublic.lee_ultimonumero_registrado("osiris_his_informacion_medica","secuencia_interna","WHERE pid_paciente = '" +entry_pid_paciente.Text.ToString().Trim()+"'"));
					guarda_soap(1,foliointernoespmed,idtitulosoap);
				break;
				case "button_guarda_optometria":
					guarda_especialidad_medica2();
				break;
				case "button_guarda_oftalmologia":
					guarda_especialidad_medica1();
				break;							
			}			
		}
		
		void almacena_solitud_y_receta(int foliointernoespmed_)
		{
			struct_recetamedica concepto_receta;
			string ultimasolicitud = "0";
			bool autoriza_solicitud = false;
			string[,] parametros;
			object[] paraobj;
			for (int i = 0; i < arrayRecetaMedica.Count; ++i) {
				if (i == 0) {
					ultimonumero_receta = classpublic.lee_ultimonumero_registrado ("osiris_his_receta_medica", "folio_de_receta", "");
					if (idaseguradora_paciente > 1) {
						if ((string)classpublic.lee_registro_de_tabla ("osiris_aseguradoras", "solicitud_receta_medica", "WHERE solicitud_receta_medica = 'true' AND id_aseguradora = '" + idaseguradora_paciente.ToString ().Trim () + "' ", "solicitud_receta_medica", "bool") == "True") {
							ultimasolicitud = classpublic.lee_ultimonumero_registrado ("osiris_his_solicitudes_deta", "folio_de_solicitud", "WHERE id_almacen = '" + idsubalmacen.ToString ().Trim () + "' ");
							autoriza_solicitud = true;
						}
					} else {
						if (idempresa_paciente > 1) {
							if ((string)classpublic.lee_registro_de_tabla ("osiris_empresas", "solicitud_receta_medica", "WHERE solicitud_receta_medica = 'true' AND id_empresa = '" + idempresa_paciente.ToString ().Trim () + "' ", "solicitud_receta_medica", "bool") == "True") {
								ultimasolicitud = classpublic.lee_ultimonumero_registrado ("osiris_his_solicitudes_deta", "folio_de_solicitud", "WHERE id_almacen = '" + idsubalmacen.ToString ().Trim () + "' ");
								autoriza_solicitud = true;
							}
						}
					}
				}
				concepto_receta = (struct_recetamedica) arrayRecetaMedica [i];
				//Console.WriteLine (concepto_receta.col00_receta + " " + concepto_receta.col01_receta + " " + concepto_receta.col02_receta);
				if ((bool) autoriza_solicitud) {
					parametros = new [,] {
						{ "folio_de_solicitud,", "'" + ultimasolicitud + "'," },
						{ "id_producto,", "'" + concepto_receta.col01_receta + "'," },
						{ "precio_producto_publico,", "'" + concepto_receta.col03_receta + "'," },
						{ "costo_por_unidad,", "'" + concepto_receta.col04_receta + "'," },
						{ "cantidad_solicitada,", "'" + concepto_receta.col02_receta + "'," },
						{ "fechahora_solicitud,", "'" + DateTime.Now.ToString ("yyyy-MM-dd HH:mm:ss") + "'," },
						{ "id_quien_solicito,", "'" + LoginEmpleado + "'," },
						{ "id_almacen,", "'" + idsubalmacen.ToString () + "'," },
						{ "folio_de_servicio,", "'" + folioservicio + "'," },
						{ "pid_paciente,", "'" + pidpaciente + "'," },
						{ "solicitud_stock,", "'false'," },
						{ "pre_solicitud,", "'false'," },
						{ "nombre_paciente,", "''," },
						{ "procedimiento_qx,", "''," },
						{ "diagnostico_qx,", "''," },
						{ "observaciones_solicitud,", "'SOLICITUD CREADA DESDE RECETA MEDICA'," },
						{ "tipo_solicitud,", "'ORDINARIA'," },
						{ "status,", "'true'," },
						{ "fecha_envio_almacen,", "'" + DateTime.Now.ToString ("yyyy-MM-dd HH:mm:ss") + "'," },
						{ "id_empleado", "'"+LoginEmpleado+"'" }
					};
					paraobj = new [] { entry_numerotencion };
					new osiris.insert_registro ("osiris_his_solicitudes_deta", parametros, paraobj);
				}
				parametros = new [,] {
						{"folio_de_servicio,","'"+folioservicio+"',"},
						{"pid_paciente,","'"+pidpaciente+"',"},
						{"fechahora_creacion,","'"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"',"},
						{"id_medico,","'"+entry_id_doctor.Text.ToString()+"',"},
						{"id_quiencreo,","'"+LoginEmpleado+"',"},
						{"id_producto,","'"+concepto_receta.col01_receta+"',"},
						{"descripcion_prescripcion,","'"+concepto_receta.col00_receta+"',"},
						{"cantidad_recetada,","'"+concepto_receta.col02_receta+"',"},
						{"folio_de_receta,","'"+ultimonumero_receta+"',"},
						{"folio_de_solicitud,","'"+ultimasolicitud+"',"},
						{"secuencia_interna_soap","'"+foliointernoespmed_.ToString().Trim()+"'"}
					};
				paraobj = new [] { entry_numerotencion };
				new osiris.insert_registro ("osiris_his_receta_medica", parametros, paraobj);				
			}
		}
		
		/// <summary>
		/// Guarda_soap the specified tipoguardado and foliointernoespmed.
		/// </summary>
		/// <param name='tipoguardado'>
		/// Tipoguardado.
		/// </param>
		/// <param name='foliointernoespmed'>
		/// Foliointernoespmed.
		/// </param>
		void guarda_soap(int tipoguardado,int foliointernoespmed,int idtituloexpfis)
		{			
			string pronostico_soap = "";
			Gtk.MessageDialog msgBoxError;
			// Guarda el SOAP principal
			Gtk.TreeIter iter2;
			if(tipoguardado == 1){
				if(textview_subjetivo.Buffer.Text.ToString().Trim() != "" && textview_objetivo.Buffer.Text.ToString().Trim() != "" && 
					textview_analisis.Buffer.Text.ToString().Trim() != "" && textview_plan.Buffer.Text.ToString().Trim() != ""){
					MessageDialog msgBox = new MessageDialog (MyWin,DialogFlags.Modal,
												MessageType.Question,ButtonsType.YesNo,"¿ Esta seguro de Almacenar SOAP ?");
					ResponseType miResultado = (ResponseType)
					msgBox.Run ();				msgBox.Destroy();
		 			if (miResultado == ResponseType.Yes){						
						if(radiobutton_pron_ligevo.Active == true){
							pronostico_soap = "\n\nPRONOSTICO: LIGADO A EVOLUCION";	
						}
						if(radiobutton_pron_bueno.Active == true){
							pronostico_soap = "\n\nPRONOSTICO: BUENO";
						}
						if(radiobutton_pron_malo.Active == true){
							pronostico_soap = "\n\nPRONOSTICO: MALO";
						}
						if(radiobutton_pron_reser.Active == true){
							pronostico_soap = "\n\nPRONOSTICO: RESERVADO";
						}						
						NpgsqlConnection conexion; 
						conexion = new NpgsqlConnection (connectionString+nombrebd);
		            	try{
							conexion.Open ();
							NpgsqlCommand comando; 
							comando = conexion.CreateCommand();
							comando.CommandText = "INSERT INTO osiris_his_informacion_medica("+
												"pid_paciente," +
												"folio_de_servicio,"+
												"fechahora_creacion," +
												"id_empleado_creacion," +
												"id_medico," +
												"fecha_anotacion," +
												"hora_anotacion," +
												"id_especialidad," +
												"id_tipo_admisiones," +
												"id_titulo_explfis," +
												"soap," +
												"id_cie_10," +
												"s_subjetivo," +
												"o_objetivo," +
												"a_analisis," +
												"p_plan," +
												"secuencia_interna) "+
											"VALUES ('"+
												entry_pid_paciente.Text.ToString().Trim()+"','"+
												entry_numerotencion.Text.ToString().Trim()+"','"+
												DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"','"+
												LoginEmpleado+"','"+
												entry_id_doctor.Text.ToString().Trim()+"','"+
												DateTime.Now.ToString("yyyy-MM-dd")+"','"+
												DateTime.Now.ToString("HH:mm")+"','"+
												idespecialidad.ToString().Trim()+"','"+
												idtipointernamiento.ToString().Trim()+"','"+
												idtituloexpfis.ToString().Trim()+"','"+
												"true".Trim()+"','"+
												entry_id_cie10.Text.ToString().Trim()+"','"+
									
												(string) classpublic.RemoveAccentsWithRegEx(textview_subjetivo.Buffer.Text.ToString().ToUpper())+"','"+
												(string) classpublic.RemoveAccentsWithRegEx(textview_objetivo.Buffer.Text.ToString().ToUpper())+"','"+
												(string) classpublic.RemoveAccentsWithRegEx(textview_analisis.Buffer.Text.ToString().ToUpper())+pronostico_soap+"','"+
												(string) classpublic.RemoveAccentsWithRegEx(textview_plan.Buffer.Text.ToString().ToUpper())+"','"+
									
												foliointernoespmed.ToString().Trim()+
												"');";
							//Console.WriteLine(comando.CommandText);
							comando.ExecuteNonQuery();
							comando.Dispose();
							button_guarda_soap.Sensitive = false;
							llenado_treeview_soap();
							
							// Almacenando los registros de CIE-10
							NpgsqlConnection conexion1; 
							conexion1 = new NpgsqlConnection (connectionString+nombrebd);
							try{
								conexion1.Open ();
								NpgsqlCommand comando1; 
								comando1 = conexion1.CreateCommand();
								if(treeViewEngine_codigoscie10.GetIterFirst (out iter2)){
									if((bool) treeview_codigoscie10.Model.GetValue(iter2,0) == true){
										comando1.CommandText = "INSERT INTO osiris_his_movdiag_cie10(" +
											"folio_de_servicio," +
											"pid_paciente," +
											"id_diagnostico," +
											"id_cie_10," +
											"fechahora_creacion," +
											"id_tipo_admisiones," +
											"id_quien_creo," +
											"secuencia_interna) " +
											"VALUES ('"+
											entry_numerotencion.Text.ToString().Trim()+"','"+
											entry_pid_paciente.Text.ToString().Trim()+"','"+
											treeview_codigoscie10.Model.GetValue(iter2,1).ToString().Trim()+"','"+
											treeview_codigoscie10.Model.GetValue(iter2,2).ToString().Trim()+"','"+
											DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"','"+
											idtipointernamiento+"','"+
											LoginEmpleado+"','"+
											foliointernoespmed.ToString().Trim()+"')";
										//Console.WriteLine(comando1.CommandText);
										comando1.ExecuteNonQuery();
										comando1.Dispose();
									}
									while(treeViewEngine_codigoscie10.IterNext(ref iter2)){
										if((bool) treeview_codigoscie10.Model.GetValue(iter2,0) == true){
											comando1.CommandText = "INSERT INTO osiris_his_movdiag_cie10(" +
													"folio_de_servicio," +
													"pid_paciente," +
													"id_diagnostico," +
													"id_cie_10," +
													"fechahora_creacion," +
													"id_tipo_admisiones," +
													"id_quien_creo," +
													"secuencia_interna) " +
													"VALUES ('"+
													entry_numerotencion.Text.ToString().Trim()+"','"+
													entry_pid_paciente.Text.ToString().Trim()+"','"+
													treeview_codigoscie10.Model.GetValue(iter2,1).ToString().Trim()+"','"+
													treeview_codigoscie10.Model.GetValue(iter2,2).ToString().Trim()+"','"+
													DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"','"+
													idtipointernamiento+"','"+
													LoginEmpleado+"','"+
													foliointernoespmed.ToString().Trim()+"')";
											//Console.WriteLine(comando1.CommandText);
											comando1.ExecuteNonQuery();
											comando1.Dispose();
										}
									}
								}
							}catch (NpgsqlException ex){
								msgBoxError = new MessageDialog (MyWinError,DialogFlags.Modal,
									MessageType.Error, 
									ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
								msgBoxError.Run ();					msgBoxError.Destroy();	   			
							}
							conexion1.Close ();														
						}catch (NpgsqlException ex){
							msgBoxError = new MessageDialog (MyWinError,DialogFlags.Modal,
								MessageType.Error, 
								ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
							msgBoxError.Run ();					msgBoxError.Destroy();	   			
			       		}
		       			conexion.Close ();
						msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,MessageType.Info,
												ButtonsType.Close, "Los datos SOAP se guardaron con EXITO");
						msgBoxError.Run ();			msgBoxError.Destroy();
						
						almacena_solitud_y_receta(foliointernoespmed);
						new osiris.rpt_receta_medica(ultimonumero_receta,int.Parse(entry_numerotencion.Text),int.Parse(entry_pid_paciente.Text),entry_nombre_paciente.Text,
				                             entry_fecha_nacimiento.Text,entry_edad_paciente.Text,entry_sexo_paciente.Text);
					}
				}else{
					msgBoxError = new MessageDialog (MyWinError,DialogFlags.Modal,
										MessageType.Error,ButtonsType.Close,"S.O.A.P. no tiene la informacion completa, verifique...");
					msgBoxError.Run ();						msgBoxError.Destroy();
				}
			}

			// aqui guarda la parte del SOAP referente a la especialidad Medica cuando existen algunas notas
			if(tipoguardado == 2){
				NpgsqlConnection conexion; 
				conexion = new NpgsqlConnection (connectionString+nombrebd);
            	try{
					conexion.Open ();
					NpgsqlCommand comando; 
					comando = conexion.CreateCommand();
					
					comando.CommandText = "INSERT INTO osiris_his_informacion_medica("+
										"pid_paciente," +
										"folio_de_servicio,"+
										"fechahora_creacion," +
										"id_empleado_creacion," +
										"id_medico," +
										"fecha_anotacion," +
										"hora_anotacion," +
										"id_especialidad," +
										"id_tipo_admisiones," +
										"id_titulo_explfis," +
										"soap," +
										"objetivo_2," +
										"o_objetivo2," +
										"secuencia_interna) " +
									"VALUES ('"+
										entry_pid_paciente.Text.ToString().Trim()+"','"+
										entry_numerotencion.Text.ToString().Trim()+"','"+
										DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"','"+
										LoginEmpleado+"','"+
										entry_id_doctor.Text.ToString().Trim()+"','"+
										DateTime.Now.ToString("yyyy-MM-dd")+"','"+
										DateTime.Now.ToString("HH:mm")+"','"+
										idespecialidad.ToString().Trim()+"','"+
										idtipodeadmision.ToString().Trim()+"','"+
										idtituloexpfis.ToString().Trim()+"','"+
										"false"+"','"+
										"true"+"','"+
										textview_objetivo2.Buffer.Text.ToString().ToUpper()+"','"+
										foliointernoespmed.ToString().Trim()+"');";
					//Console.WriteLine(comando.CommandText);
					comando.ExecuteNonQuery();
					comando.Dispose();
					llenado_treeview_soap();
					llenado_signos_vitales();
				}catch (NpgsqlException ex){
					msgBoxError = new MessageDialog (MyWinError,DialogFlags.Modal,MessageType.Error, 
												ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
					msgBoxError.Run ();					msgBoxError.Destroy();	   			
	       		}
       			conexion.Close ();
			}
		}
				
		void guarda_especialidad_medica1()
		{
			int idtituloexplofisica = 0;
			MessageDialog msgBox = new MessageDialog (MyWin,DialogFlags.Modal,
											MessageType.Question,ButtonsType.YesNo,"¿ Esta seguro Esta Especilidad Medica ?");
			ResponseType miResultado = (ResponseType)
			msgBox.Run ();				msgBox.Destroy();
	 		if (miResultado == ResponseType.Yes){
				TreeIter iter;
				int foliointernoespmed = int.Parse(classpublic.lee_ultimonumero_registrado("osiris_his_informacion_medica","secuencia_interna","WHERE pid_paciente = '" +entry_pid_paciente.Text.ToString().Trim()+"'"));
				NpgsqlConnection conexion1; 
				conexion1 = new NpgsqlConnection (connectionString+nombrebd);
	    		try{
					conexion1.Open ();
					NpgsqlCommand comando1; 
					comando1 = conexion1.CreateCommand();
					// Leyendo treeview Oftalmologia
					if(treeViewEngine_Obj3.GetIterFirst (out iter)){
						idtituloexplofisica = (int) treeview_objetivo3.Model.GetValue(iter,5);
						//Console.WriteLine((string) treeview_objetivo3.Model.GetValue (iter,1));
						comando1.CommandText = "INSERT INTO osiris_his_explfis_mov("+
													"pid_paciente," +
													"folio_de_servicio,"+
													"fechahora_creacion," +
													"fecha_anotacion," +
													"id_empleado_creacion," +
													"id_medico," +
													"id_titulo_explfis," +
													"id_parametro," +
													"id_secuencia_parametro," +
													"descripcion_parametro," +
													"notas_derecha," +
													"notas_izquierda," +
													"id_tipo_admision," +
													"id_especialidad," +
													"secuencia_interna" +
												") VALUES ('"+
													(string) entry_pid_paciente.Text.ToString().Trim()+"','"+
													(string) entry_numerotencion.Text.ToString().Trim()+"','"+
													DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"','"+
													DateTime.Now.ToString("yyyy-MM-dd")+"','"+
													LoginEmpleado+"','"+
													(string) entry_id_doctor.Text.ToString().Trim()+"','" +										
													treeview_objetivo3.Model.GetValue(iter,5).ToString().Trim()+"','" +
													treeview_objetivo3.Model.GetValue(iter,6).ToString().Trim()+"','" +
													treeview_objetivo3.Model.GetValue(iter,7).ToString().Trim()+"','" +
													treeview_objetivo3.Model.GetValue(iter,1).ToString().Trim()+"','" +
													treeview_objetivo3.Model.GetValue(iter,2).ToString().Trim()+"','" +
													treeview_objetivo3.Model.GetValue(iter,3).ToString().Trim()+"','" +
													idtipodeadmision.ToString().Trim()+"','" +
													idespecialidad.ToString().Trim()+"','" +
													foliointernoespmed.ToString().Trim()+"'"+
														");";
						//Console.WriteLine(comando1.CommandText);
						comando1.ExecuteNonQuery();
						comando1.Dispose();
						while(treeViewEngine_Obj3.IterNext(ref iter)){
							//Console.WriteLine((string) treeview_objetivo3.Model.GetValue (iter,1));
							comando1.CommandText = "INSERT INTO osiris_his_explfis_mov("+
													"pid_paciente," +
													"folio_de_servicio,"+
													"fechahora_creacion," +
													"fecha_anotacion," +
													"id_empleado_creacion," +
													"id_medico," +
													"id_titulo_explfis," +
													"id_parametro," +
													"id_secuencia_parametro," +
													"descripcion_parametro," +
													"notas_derecha," +
													"notas_izquierda," +
													"id_tipo_admision," +
													"id_especialidad," +
													"secuencia_interna" +
												") VALUES ('"+
													(string) entry_pid_paciente.Text.ToString().Trim()+"','"+
													(string) entry_numerotencion.Text.ToString().Trim()+"','"+
													DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"','"+
													DateTime.Now.ToString("yyyy-MM-dd")+"','"+
													LoginEmpleado+"','"+
													(string) entry_id_doctor.Text.ToString().Trim()+"','" +
													treeview_objetivo3.Model.GetValue(iter,5).ToString().Trim()+"','" +
													treeview_objetivo3.Model.GetValue(iter,6).ToString().Trim()+"','" +
													treeview_objetivo3.Model.GetValue(iter,7).ToString().Trim()+"','" +
													treeview_objetivo3.Model.GetValue(iter,1).ToString().Trim()+"','" +
													treeview_objetivo3.Model.GetValue(iter,2).ToString().Trim()+"','" +
													treeview_objetivo3.Model.GetValue(iter,3).ToString().Trim()+"','" +
													idtipodeadmision.ToString().Trim()+"','" +
													idespecialidad.ToString().Trim()+"','" +
													foliointernoespmed.ToString().Trim()+"'"+
														");";
							//Console.WriteLine(comando1.CommandText);
							comando1.ExecuteNonQuery();
							comando1.Dispose();							
						}					
					}
					button_guarda_oftalmologia.Sensitive = false;
					guarda_soap(2,foliointernoespmed,idtituloexplofisica);
					llenado_treeview_soap();
					MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,MessageType.Info,
						 							ButtonsType.Close, "Los datos de Oftalmologia se guardaron con EXITO");
					msgBoxError.Run ();			msgBoxError.Destroy();
				}catch (NpgsqlException ex){
						MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.Modal,
											MessageType.Error, 
											ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
						msgBoxError.Run ();					msgBoxError.Destroy();	   			
				}
		       	conexion1.Close ();
			}
		}
		
		void guarda_especialidad_medica2()
		{
			MessageDialog msgBox = new MessageDialog (MyWin,DialogFlags.Modal,
											MessageType.Question,ButtonsType.YesNo,"¿ Esta seguro esta Optometria ?");
			ResponseType miResultado = (ResponseType)
			msgBox.Run ();				msgBox.Destroy();
	 		if (miResultado == ResponseType.Yes){			
				int foliointernoespmed = int.Parse(classpublic.lee_ultimonumero_registrado("osiris_his_informacion_medica","secuencia_interna","WHERE pid_paciente = '" +entry_pid_paciente.Text.ToString().Trim()+"'"));
				TreeIter iter;
				NpgsqlConnection conexion1; 
				conexion1 = new NpgsqlConnection (connectionString+nombrebd);
	    		try{
					conexion1.Open ();
					NpgsqlCommand comando1; 
					comando1 = conexion1.CreateCommand();					
					// Leyendo treeview Optometria
					if(treeViewEngine_Obj1.GetIterFirst (out iter)){
						//Console.WriteLine((string) treeview_objetivo1.Model.GetValue (iter,1));
						comando1.CommandText = "INSERT INTO osiris_his_explfis_mov("+
										"pid_paciente," +
										"folio_de_servicio,"+
										"fechahora_creacion," +
										"fecha_anotacion," +
										"id_empleado_creacion," +
										"id_medico," +
										"id_titulo_explfis," +
										"id_parametro," +
										"id_secuencia_parametro," +
										"descripcion_parametro," +
										"notas_derecha," +
										"notas_izquierda," +
										"id_tipo_admision," +
										"id_especialidad," +
										"secuencia_interna" +
									") VALUES ('"+
										(string) entry_pid_paciente.Text.ToString().Trim()+"','"+
										(string) entry_numerotencion.Text.ToString().Trim()+"','"+
										DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"','"+
										DateTime.Now.ToString("yyyy-MM-dd")+"','"+
										LoginEmpleado+"','"+
										(string) entry_id_doctor.Text.ToString().Trim()+"','" +
										treeview_objetivo1.Model.GetValue(iter,4).ToString().Trim()+"','" +
										treeview_objetivo1.Model.GetValue(iter,5).ToString().Trim()+"','" +
										treeview_objetivo1.Model.GetValue(iter,6).ToString().Trim()+"','" +
										treeview_objetivo1.Model.GetValue(iter,1).ToString().Trim()+"','" +
										treeview_objetivo1.Model.GetValue(iter,2).ToString().Trim()+"','" +
										treeview_objetivo1.Model.GetValue(iter,3).ToString().Trim()+"','" +
										idtipodeadmision.ToString().Trim()+"','" +
										idespecialidad.ToString().Trim()+"','" +
										foliointernoespmed.ToString().Trim()+"'"+
											");";
						//Console.WriteLine(comando1.CommandText);
						comando1.ExecuteNonQuery();
						comando1.Dispose();						
						while(treeViewEngine_Obj1.IterNext(ref iter)){
							//Console.WriteLine((string) treeview_objetivo1.Model.GetValue (iter,1));
							comando1.CommandText = "INSERT INTO osiris_his_explfis_mov("+
										"pid_paciente," +
										"folio_de_servicio,"+
										"fechahora_creacion," +
										"fecha_anotacion," +
										"id_empleado_creacion," +
										"id_medico," +
										"id_titulo_explfis," +
										"id_parametro," +
										"id_secuencia_parametro," +
										"descripcion_parametro," +
										"notas_derecha," +
										"notas_izquierda," +
										"id_tipo_admision," +
										"id_especialidad," +
										"secuencia_interna" +
									") VALUES ('"+
										(string) entry_pid_paciente.Text.ToString().Trim()+"','"+
										(string) entry_numerotencion.Text.ToString().Trim()+"','"+
										DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"','"+
										DateTime.Now.ToString("yyyy-MM-dd")+"','"+
										LoginEmpleado+"','"+
										(string) entry_id_doctor.Text.ToString().Trim()+"','" +
										treeview_objetivo1.Model.GetValue(iter,4).ToString().Trim()+"','" +
										treeview_objetivo1.Model.GetValue(iter,5).ToString().Trim()+"','" +
										treeview_objetivo1.Model.GetValue(iter,6).ToString().Trim()+"','" +
										treeview_objetivo1.Model.GetValue(iter,1).ToString().Trim()+"','" +
										treeview_objetivo1.Model.GetValue(iter,2).ToString().Trim()+"','" +
										treeview_objetivo1.Model.GetValue(iter,3).ToString().Trim()+"','" +
										idtipodeadmision.ToString().Trim()+"','" +
										idespecialidad.ToString().Trim()+"','" +
										foliointernoespmed.ToString().Trim()+"'"+
											");";
							//Console.WriteLine(comando1.CommandText);
							comando1.ExecuteNonQuery();
							comando1.Dispose();
						}							
					}
				
					if(treeViewEngine_Obj2.GetIterFirst (out iter)){
						//Console.WriteLine((string) treeview_objetivo2.Model.GetValue (iter,1));
						comando1.CommandText = "INSERT INTO osiris_his_explfis_mov("+
													"pid_paciente," +
													"folio_de_servicio,"+
													"fechahora_creacion," +
													"fecha_anotacion," +
													"id_empleado_creacion," +
													"id_medico," +
													"id_titulo_explfis," +
													"id_parametro," +
													"id_secuencia_parametro," +
													"descripcion_parametro," +
													"notas_derecha," +
													"notas_izquierda," +
													"id_tipo_admision," +
													"id_especialidad," +
													"secuencia_interna" +
												") VALUES ('"+
													(string) entry_pid_paciente.Text.ToString().Trim()+"','"+
													(string) entry_numerotencion.Text.ToString().Trim()+"','"+
													DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"','"+
													DateTime.Now.ToString("yyyy-MM-dd")+"','"+
													LoginEmpleado+"','"+
													(string) entry_id_doctor.Text.ToString().Trim()+"','" +
													treeview_objetivo2.Model.GetValue(iter,14).ToString().Trim()+"','" +
													treeview_objetivo2.Model.GetValue(iter,15).ToString().Trim()+"','" +
													treeview_objetivo2.Model.GetValue(iter,16).ToString().Trim()+"','" +
													treeview_objetivo2.Model.GetValue(iter,1).ToString().Trim()+"','" +											
													treeview_objetivo2.Model.GetValue(iter,2).ToString().Trim()+" |" +
													treeview_objetivo2.Model.GetValue(iter,3).ToString().Trim()+" |" +
													treeview_objetivo2.Model.GetValue(iter,4).ToString().Trim()+" |" +
													treeview_objetivo2.Model.GetValue(iter,5).ToString().Trim()+" |" +
													treeview_objetivo2.Model.GetValue(iter,6).ToString().Trim()+" | " +
													treeview_objetivo2.Model.GetValue(iter,7).ToString().Trim()+"','" +
													treeview_objetivo2.Model.GetValue(iter,8).ToString().Trim()+" |" +
													treeview_objetivo2.Model.GetValue(iter,9).ToString().Trim()+" |" +
													treeview_objetivo2.Model.GetValue(iter,10).ToString().Trim()+" |" +
													treeview_objetivo2.Model.GetValue(iter,11).ToString().Trim()+" |" +
													treeview_objetivo2.Model.GetValue(iter,12).ToString().Trim()+" | " +
													treeview_objetivo2.Model.GetValue(iter,13).ToString().Trim()+"','" +									
													idtipodeadmision.ToString().Trim()+"','" +
													idespecialidad.ToString().Trim()+"','" +
													foliointernoespmed.ToString().Trim()+"'"+
														");";
										//Console.WriteLine(comando1.CommandText);
										comando1.ExecuteNonQuery();
										comando1.Dispose();
						while(treeViewEngine_Obj2.IterNext(ref iter)){
							comando1.CommandText = "INSERT INTO osiris_his_explfis_mov("+
													"pid_paciente," +
													"folio_de_servicio,"+
													"fechahora_creacion," +
													"fecha_anotacion," +
													"id_empleado_creacion," +
													"id_medico," +
													"id_titulo_explfis," +
													"id_parametro," +
													"id_secuencia_parametro," +
													"descripcion_parametro," +
													"notas_derecha," +
													"notas_izquierda," +
													"id_tipo_admision," +
													"id_especialidad," +
													"secuencia_interna" +
												") VALUES ('"+
													(string) entry_pid_paciente.Text.ToString().Trim()+"','"+
													(string) entry_numerotencion.Text.ToString().Trim()+"','"+
													DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"','"+
													DateTime.Now.ToString("yyyy-MM-dd")+"','"+
													LoginEmpleado+"','"+
													(string) entry_id_doctor.Text.ToString().Trim()+"','" +
													treeview_objetivo2.Model.GetValue(iter,14).ToString().Trim()+"','" +
													treeview_objetivo2.Model.GetValue(iter,15).ToString().Trim()+"','" +
													treeview_objetivo2.Model.GetValue(iter,16).ToString().Trim()+"','" +
													treeview_objetivo2.Model.GetValue(iter,1).ToString().Trim()+"','" +											
													treeview_objetivo2.Model.GetValue(iter,2).ToString().Trim()+" |" +
													treeview_objetivo2.Model.GetValue(iter,3).ToString().Trim()+" |" +
													treeview_objetivo2.Model.GetValue(iter,4).ToString().Trim()+" |" +
													treeview_objetivo2.Model.GetValue(iter,5).ToString().Trim()+" |" +
													treeview_objetivo2.Model.GetValue(iter,6).ToString().Trim()+" | " +
													treeview_objetivo2.Model.GetValue(iter,7).ToString().Trim()+"','" +
													treeview_objetivo2.Model.GetValue(iter,8).ToString().Trim()+" |" +
													treeview_objetivo2.Model.GetValue(iter,9).ToString().Trim()+" |" +
													treeview_objetivo2.Model.GetValue(iter,10).ToString().Trim()+" |" +
													treeview_objetivo2.Model.GetValue(iter,11).ToString().Trim()+" |" +
													treeview_objetivo2.Model.GetValue(iter,12).ToString().Trim()+" | " +
													treeview_objetivo2.Model.GetValue(iter,13).ToString().Trim()+"','" +									
													idtipodeadmision.ToString().Trim()+"','" +
													idespecialidad.ToString().Trim()+"','" +
													foliointernoespmed.ToString().Trim()+"'"+
														");";
							//Console.WriteLine(comando1.CommandText);
							comando1.ExecuteNonQuery();
							comando1.Dispose();									
						}
						button_guarda_optometria.Sensitive = false;
						guarda_soap(2,foliointernoespmed,3);
						llenado_treeview_soap();
						MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,MessageType.Info,
						 							ButtonsType.Close, "Los datos de Optometria se guardaron con EXITO");
						msgBoxError.Run ();			msgBoxError.Destroy();
					}
				}catch (NpgsqlException ex){
						MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.Modal,
											MessageType.Error, 
											ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
						msgBoxError.Run ();					msgBoxError.Destroy();	   			
				}
		       	conexion1.Close ();
			}
		}
		
		void crea_treeview_objetivo1()
		{
			Gtk.CellRendererToggle toogle;
			Gtk.CellRendererText text;
			foreach (TreeViewColumn tvc in treeview_objetivo1.Columns)
							treeview_objetivo1.RemoveColumn(tvc);
			treeViewEngine_Obj1 = new TreeStore(typeof(bool),
												typeof(string),
												typeof(string),
			                                    typeof(string),
			                                    typeof(int),typeof(int),typeof(int));
			treeview_objetivo1.Model = treeViewEngine_Obj1;
			treeview_objetivo1.RulesHint = true;
			treeview_objetivo1.Selection.Mode = SelectionMode.Multiple;
			
			// column for holiday names
			toogle = new CellRendererToggle();
			toogle.Xalign = 0.0f;
			columns.Add (toogle);
			TreeViewColumn column0 = new TreeViewColumn("Selec.",toogle,"active",Column.col00);
			column0.Resizable = true;
			column0.SortColumnId = (int) Column.col00;
			treeview_objetivo1.InsertColumn (column0, (int) Column.col00);
			
			text = new CellRendererText();
			text.Xalign = 0.0f;
			columns.Add (text);
			TreeViewColumn column1 = new TreeViewColumn("PARAMETROS",text,"text",Column.col01);
			column1.Resizable = true;
			column1.SortColumnId = (int) Column.col01;
			treeview_objetivo1.InsertColumn (column1, (int) Column.col01);
			
			text = new CellRendererText();
			text.Xalign = 0.0f;
			text.Editable = true;
			text.Edited += new EditedHandler(NumberCellEdited_obj1_od);
			columns.Add (text);
			TreeViewColumn column2 = new TreeViewColumn("OJO DERECHO",text,"text",Column.col02);
			column2.Resizable = true;
			column2.SortColumnId = (int) Column.col02;
			//text.CellBackground = "red";
			treeview_objetivo1.InsertColumn (column2, (int) Column.col02);
			
			text = new CellRendererText();
			text.Xalign = 0.0f;
			text.Editable = true;
			text.Edited += new EditedHandler(NumberCellEdited_obj1_oi);
			columns.Add (text);
			TreeViewColumn column3 = new TreeViewColumn("OJO IZQUIERDO",text,"text",Column.col03);
			column3.Resizable = true;
			column3.SortColumnId = (int) Column.col03;
			//text.CellBackground = "Yellow";
			treeview_objetivo1.InsertColumn (column3, (int) Column.col03);
		}
		
		enum Column
		{
			col00,col01,col02,
			col03,col04,col05,
			col06,col07,col08,
			col09,col10,col11,
			col12,col13
		}
		
		void NumberCellEdited_obj1_od(object sender, EditedArgs args)
		{
			//Gtk.CellRendererText obj_sender = (object) sender as CellRendererText;
			Gtk.TreeIter iter;
			TreePath path = new TreePath (args.Path);
			if (treeview_objetivo1.Model.GetIter (out iter, path)){
				treeview_objetivo1.Model.SetValue(iter,2,args.NewText );		
			}
		}
		
		void NumberCellEdited_obj1_oi(object sender, EditedArgs args)
		{
			//Gtk.CellRendererText obj_sender = (object) sender as CellRendererText;
			Gtk.TreeIter iter;
			TreePath path = new TreePath (args.Path);
			if (treeview_objetivo1.Model.GetIter (out iter, path)){
				treeview_objetivo1.Model.SetValue(iter,3,args.NewText );		
			}
		}
		
		void llenado_treeview_objetivo1()
		{
			treeViewEngine_Obj1.Clear();
			NpgsqlConnection conexion; 
			conexion = new NpgsqlConnection (connectionString+nombrebd);
            // Verifica que la base de datos este conectada
			try{
				conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand ();               	
				comando.CommandText = "SELECT descripcion_parametro,osiris_his_explfis_parametros.id_titulo_explfis,id_parametro,id_secuencia_parametro,descripcion_titulo " +
					"FROM osiris_his_explfis_parametros,osiris_his_explfis_titulos " +
					"WHERE osiris_his_explfis_parametros.id_titulo_explfis = osiris_his_explfis_titulos.id_titulo_explfis " +
					"AND id_tipo_admision = '940' " +
					"AND id_parametro = '1' " +
					"AND activo = 'true' " +
					"ORDER BY osiris_his_explfis_parametros.id_titulo_explfis,id_parametro,id_secuencia_parametro";
				//Console.WriteLine(comando.CommandText);
				NpgsqlDataReader lector = comando.ExecuteReader ();
				while(lector.Read()){
					iter = treeViewEngine_Obj1.AppendValues (false,
								    lector["descripcion_parametro"],
				                    "",
					                "",
					                int.Parse(lector["id_titulo_explfis"].ToString()),
					                int.Parse(lector["id_parametro"].ToString()),
					                int.Parse(lector["id_secuencia_parametro"].ToString()));
					
				}
			}catch (NpgsqlException ex){
				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
										MessageType.Error,ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
				msgBoxError.Run ();				msgBoxError.Destroy();
			}
			conexion.Close ();
		}
		
		void crea_treeview_objetivo2()
		{
			Gtk.CellRendererToggle toogle;
			Gtk.CellRendererText text;
			foreach (TreeViewColumn tvc in treeview_objetivo2.Columns)
							treeview_objetivo2.RemoveColumn(tvc);
			treeViewEngine_Obj2 = new TreeStore(typeof(bool),
												typeof(string),
												typeof(string),typeof(string),typeof(string),typeof(string),typeof(string),typeof(string),
			                                    typeof(string),typeof(string),typeof(string),typeof(string),typeof(string),typeof(string),
			                                    typeof(int),typeof(int),typeof(int));
			treeview_objetivo2.Model = treeViewEngine_Obj2;
			treeview_objetivo2.RulesHint = true;
			treeview_objetivo2.Selection.Mode = SelectionMode.Multiple;
			
			// column for holiday names
			toogle = new CellRendererToggle();
			toogle.Xalign = 0.0f;
			columns.Add (toogle);
			TreeViewColumn column0 = new TreeViewColumn("Selec.",toogle,"active",Column.col00);
			column0.Resizable = true;
			column0.SortColumnId = (int) Column.col00;
			treeview_objetivo2.InsertColumn (column0, (int) Column.col00);
			
			text = new CellRendererText();
			text.Xalign = 0.0f;
			columns.Add (text);
			TreeViewColumn column1 = new TreeViewColumn("PARAMETROS",text,"text",Column.col01);
			column1.Resizable = true;
			column1.SortColumnId = (int) Column.col01;
			treeview_objetivo2.InsertColumn (column1, (int) Column.col01);
			
			text = new CellRendererText();
			text.Xalign = 0.0f;
			text.Editable = true;
			text.Edited += new EditedHandler(NumberCellEdited_obj2_esf_od);
			columns.Add (text);
			TreeViewColumn column2 = new TreeViewColumn("Esfera-OD",text,"text",2);
			column2.Resizable = true;
			column2.SortColumnId = (int) Column.col02;
			column2.AddAttribute (text, "text", (int) Column.col02);
			treeview_objetivo2.InsertColumn (column2, (int) Column.col02);
			
			text = new CellRendererText();
			text.Xalign = 0.0f;
			text.Editable = true;
			text.Edited += new EditedHandler(NumberCellEdited_obj2_cil_od);
			columns.Add (text);
			TreeViewColumn column3 = new TreeViewColumn("Cilindro-OD",text,"text",Column.col03);
			column3.Resizable = true;
			column3.SortColumnId = (int) Column.col03;
			column3.AddAttribute (text, "text", (int) Column.col03);
			treeview_objetivo2.InsertColumn (column3, (int) Column.col03);
			
			text = new CellRendererText();
			text.Xalign = 0.0f;
			text.Editable = true;
			text.Edited += new EditedHandler(NumberCellEdited_obj2_eje_od);
			columns.Add (text);
			TreeViewColumn column4 = new TreeViewColumn("Eje-OD",text,"text",Column.col04);
			column4.Resizable = true;
			column4.SortColumnId = (int) Column.col04;
			column4.AddAttribute (text, "text", (int) Column.col04);
			treeview_objetivo2.InsertColumn (column4, (int) Column.col04);
			
			text = new CellRendererText();
			text.Xalign = 0.0f;
			text.Editable = true;
			text.Edited += new EditedHandler(NumberCellEdited_obj2_av1_od);
			columns.Add (text);
			TreeViewColumn column5 = new TreeViewColumn("AV1-OD",text,"text",Column.col05);
			column5.Resizable = true;
			column5.SortColumnId = (int) Column.col05;
			column5.AddAttribute (text, "text", (int) Column.col05);
			treeview_objetivo2.InsertColumn (column5, (int) Column.col05);
			
			text = new CellRendererText();
			text.Xalign = 0.0f;
			text.Editable = true;
			text.Edited += new EditedHandler(NumberCellEdited_obj2_add_od);
			columns.Add (text);
			TreeViewColumn column6 = new TreeViewColumn("ADD-OD",text,"text",Column.col06);
			column6.Resizable = true;
			column6.SortColumnId = (int) Column.col06;
			column6.AddAttribute (text, "text", (int) Column.col06);
			treeview_objetivo2.InsertColumn (column6, (int) Column.col06);
			
			text = new CellRendererText();
			text.Xalign = 0.0f;
			text.Editable = true;
			text.Edited += new EditedHandler(NumberCellEdited_obj2_av2_od);
			columns.Add (text);
			TreeViewColumn column7 = new TreeViewColumn("AV2-OD",text,"text",Column.col07);
			column7.Resizable = true;
			column7.SortColumnId = (int) Column.col07;
			treeview_objetivo2.InsertColumn (column7, (int) Column.col07);
			
			//+++++++++ Ojo izquierdo  +++++++++++++++++++++++
			text = new CellRendererText();
			text.Xalign = 0.0f;
			text.Editable = true;
			text.Edited += new EditedHandler(NumberCellEdited_obj2_esf_oi);
			columns.Add (text);
			TreeViewColumn column8 = new TreeViewColumn("Esfera-OI",text,"text",Column.col08);
			column8.Resizable = true;
			column8.SortColumnId = (int) Column.col08;
			treeview_objetivo2.InsertColumn (column8, (int) Column.col08);
			
			text = new CellRendererText();
			text.Xalign = 0.0f;
			text.Editable = true;
			text.Edited += new EditedHandler(NumberCellEdited_obj2_cil_oi);
			columns.Add (text);
			TreeViewColumn column9 = new TreeViewColumn("Cilindro-OI",text,"text",Column.col09);
			column9.Resizable = true;
			column9.SortColumnId = (int) Column.col09;
			treeview_objetivo2.InsertColumn (column9, (int) Column.col09);
			
			text = new CellRendererText();
			text.Xalign = 0.0f;
			text.Editable = true;
			text.Edited += new EditedHandler(NumberCellEdited_obj2_eje_oi);
			columns.Add (text);
			TreeViewColumn column10 = new TreeViewColumn("Eje-OI",text,"text",Column.col10);
			column10.Resizable = true;
			column10.SortColumnId = (int) Column.col10;
			treeview_objetivo2.InsertColumn (column10, (int) Column.col10);
			
			text = new CellRendererText();
			text.Xalign = 0.0f;
			text.Editable = true;
			text.Edited += new EditedHandler(NumberCellEdited_obj2_av1_oi);
			columns.Add (text);
			TreeViewColumn column11 = new TreeViewColumn("AV1-OI",text,"text",Column.col11);
			column11.Resizable = true;
			column11.SortColumnId = (int) Column.col11;
			treeview_objetivo2.InsertColumn (column11, (int) Column.col11);
			
			text = new CellRendererText();
			text.Xalign = 0.0f;
			text.Editable = true;
			text.Edited += new EditedHandler(NumberCellEdited_obj2_add_oi);
			columns.Add (text);
			TreeViewColumn column12 = new TreeViewColumn("ADD-OI",text,"text",Column.col12);
			column12.Resizable = true;
			column12.SortColumnId = (int) Column.col12;
			treeview_objetivo2.InsertColumn (column12, (int) Column.col12);
			
			text = new CellRendererText();
			text.Xalign = 0.0f;
			text.Editable = true;
			text.Edited += new EditedHandler(NumberCellEdited_obj2_av2_oi);
			columns.Add (text);
			TreeViewColumn column13 = new TreeViewColumn("AV2-OI",text,"text",Column.col13);
			column13.Resizable = true;
			column13.SortColumnId = (int) Column.col13;
			treeview_objetivo2.InsertColumn (column13, (int) Column.col13);			
		}
		
		void NumberCellEdited_obj2_esf_od(object sender, EditedArgs args)
		{
			//Gtk.CellRendererText obj_sender = (object) sender as CellRendererText;
			Gtk.TreeIter iter;
			TreePath path = new TreePath (args.Path);
			if (treeview_objetivo2.Model.GetIter (out iter, path)){
				treeview_objetivo2.Model.SetValue(iter,2,args.NewText );
			}
		}
		
		void NumberCellEdited_obj2_cil_od(object sender, EditedArgs args)
		{
			//Gtk.CellRendererText obj_sender = (object) sender as CellRendererText;
			Gtk.TreeIter iter;
			TreePath path = new TreePath (args.Path);
			if (treeview_objetivo2.Model.GetIter (out iter, path)){
				treeview_objetivo2.Model.SetValue(iter,3,args.NewText );
			}
		}
				
		void NumberCellEdited_obj2_eje_od(object sender, EditedArgs args)
		{
			//Gtk.CellRendererText obj_sender = (object) sender as CellRendererText;
			Gtk.TreeIter iter;
			TreePath path = new TreePath (args.Path);
			if (treeview_objetivo2.Model.GetIter (out iter, path)){
				treeview_objetivo2.Model.SetValue(iter,4,args.NewText );
			}
		}
		
		void NumberCellEdited_obj2_av1_od(object sender, EditedArgs args)
		{
			//Gtk.CellRendererText obj_sender = (object) sender as CellRendererText;
			Gtk.TreeIter iter;
			TreePath path = new TreePath (args.Path);
			if (treeview_objetivo2.Model.GetIter (out iter, path)){
				treeview_objetivo2.Model.SetValue(iter,5,args.NewText );
			}
		}
		
		void NumberCellEdited_obj2_add_od(object sender, EditedArgs args)
		{
			//Gtk.CellRendererText obj_sender = (object) sender as CellRendererText;
			Gtk.TreeIter iter;
			TreePath path = new TreePath (args.Path);
			if (treeview_objetivo2.Model.GetIter (out iter, path)){
				treeview_objetivo2.Model.SetValue(iter,6,args.NewText );
			}
		}
		
		void NumberCellEdited_obj2_av2_od(object sender, EditedArgs args)
		{
			//Gtk.CellRendererText obj_sender = (object) sender as CellRendererText;
			Gtk.TreeIter iter;
			TreePath path = new TreePath (args.Path);
			if (treeview_objetivo2.Model.GetIter (out iter, path)){
				treeview_objetivo2.Model.SetValue(iter,7,args.NewText );
			}
		}
		
		// ojo izquierdo
		void NumberCellEdited_obj2_esf_oi(object sender, EditedArgs args)
		{
			//Gtk.CellRendererText obj_sender = (object) sender as CellRendererText;
			Gtk.TreeIter iter;
			TreePath path = new TreePath (args.Path);
			if (treeview_objetivo2.Model.GetIter (out iter, path)){
				treeview_objetivo2.Model.SetValue(iter,8,args.NewText );
			}
		}
		
		void NumberCellEdited_obj2_cil_oi(object sender, EditedArgs args)
		{
			//Gtk.CellRendererText obj_sender = (object) sender as CellRendererText;
			Gtk.TreeIter iter;
			TreePath path = new TreePath (args.Path);
			if (treeview_objetivo2.Model.GetIter (out iter, path)){
				treeview_objetivo2.Model.SetValue(iter,9,args.NewText );
			}
		}
				
		void NumberCellEdited_obj2_eje_oi(object sender, EditedArgs args)
		{
			//Gtk.CellRendererText obj_sender = (object) sender as CellRendererText;
			Gtk.TreeIter iter;
			TreePath path = new TreePath (args.Path);
			if (treeview_objetivo2.Model.GetIter (out iter, path)){
				treeview_objetivo2.Model.SetValue(iter,10,args.NewText );
			}
		}
		
		void NumberCellEdited_obj2_av1_oi(object sender, EditedArgs args)
		{
			//Gtk.CellRendererText obj_sender = (object) sender as CellRendererText;
			Gtk.TreeIter iter;
			TreePath path = new TreePath (args.Path);
			if (treeview_objetivo2.Model.GetIter (out iter, path)){
				treeview_objetivo2.Model.SetValue(iter,11,args.NewText );
			}
		}
		
		void NumberCellEdited_obj2_add_oi(object sender, EditedArgs args)
		{
			//Gtk.CellRendererText obj_sender = (object) sender as CellRendererText;
			Gtk.TreeIter iter;
			TreePath path = new TreePath (args.Path);
			if (treeview_objetivo2.Model.GetIter (out iter, path)){
				treeview_objetivo2.Model.SetValue(iter,12,args.NewText );
			}
		}
		
		void NumberCellEdited_obj2_av2_oi(object sender, EditedArgs args)
		{
			//Gtk.CellRendererText obj_sender = (object) sender as CellRendererText;
			Gtk.TreeIter iter;
			TreePath path = new TreePath (args.Path);
			if (treeview_objetivo2.Model.GetIter (out iter, path)){
				treeview_objetivo2.Model.SetValue(iter,13,args.NewText );
			}
		}
		
		//---------------------------------------------------
		void llenado_treeview_objetivo2()
		{
			treeViewEngine_Obj2.Clear();			
			NpgsqlConnection conexion; 
			conexion = new NpgsqlConnection (connectionString+nombrebd);
            // Verifica que la base de datos este conectada
			try{
				conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand ();               	
				comando.CommandText = "SELECT descripcion_parametro,osiris_his_explfis_parametros.id_titulo_explfis,id_parametro,id_secuencia_parametro,descripcion_titulo " +
					"FROM osiris_his_explfis_parametros,osiris_his_explfis_titulos " +
					"WHERE osiris_his_explfis_parametros.id_titulo_explfis = osiris_his_explfis_titulos.id_titulo_explfis " +
					"AND id_tipo_admision = '940' " +
					"AND id_parametro = '2' " +
					"AND activo = 'true' " +
					"ORDER BY osiris_his_explfis_parametros.id_titulo_explfis,id_parametro,id_secuencia_parametro";
				//Console.WriteLine(comando.CommandText);
				NpgsqlDataReader lector = comando.ExecuteReader ();
				while(lector.Read()){
					iter = treeViewEngine_Obj2.AppendValues (false,
								    lector["descripcion_parametro"],
				                    "","","","","","",
					                "","","","","","",
					                int.Parse(lector["id_titulo_explfis"].ToString()),
					                int.Parse(lector["id_parametro"].ToString()),
					                int.Parse(lector["id_secuencia_parametro"].ToString()));
					
				}
			}catch (NpgsqlException ex){
				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
										MessageType.Error,ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
				msgBoxError.Run ();				msgBoxError.Destroy();
			}
			conexion.Close ();
		}
		
		void crea_treeview_objetivo3()
		{
			Gtk.CellRendererToggle toogle;
			Gtk.CellRendererText text;
			foreach (TreeViewColumn tvc in treeview_objetivo3.Columns)
							treeview_objetivo3.RemoveColumn(tvc);
			treeViewEngine_Obj3 = new TreeStore(typeof(bool),
												typeof(string),
												typeof(string),
			                                    typeof(string),
			                                    typeof(string),
			                                    typeof(int),
			                                    typeof(int),
			                                    typeof(int));
			treeview_objetivo3.Model = treeViewEngine_Obj3;
			treeview_objetivo3.RulesHint = true;
			treeview_objetivo3.Selection.Mode = SelectionMode.Multiple;
			
			// column for holiday names
			toogle = new CellRendererToggle();
			toogle.Xalign = 0.0f;
			columns.Add (toogle);
			toogle.Toggled += new ToggledHandler (ItemToggled);
			TreeViewColumn column0 = new TreeViewColumn("Selec.",toogle,"active",Column.col00);
			column0.Resizable = true;
			column0.SortColumnId = (int) Column.col00;
			treeview_objetivo3.InsertColumn (column0, (int) Column.col00);
			
			
			/*
			toggle = new CellRendererToggle ();
			toggle.Xalign = 0.0f;
			columns.Add (toggle);
			toggle.Toggled += new ToggledHandler (ItemToggled);
			TreeViewColumn column0 = new TreeViewColumn ("Seleccion", toggle,
							     "active", (int) Column.col00,
							     "visible", (int) Column.Visible,
							     "activatable", (int) Column.World);
			column0.Sizing = TreeViewColumnSizing.Fixed;
			column0.FixedWidth = 65;
			column0.Clickable = true;
			column0.Resizable = true;
			column0.SortColumnId = (int) Column.col00;
			treeview_objetivo3.InsertColumn (column0, (int) Column.col00);
			*/
			
			text = new CellRendererText();
			text.Xalign = 0.0f;
			columns.Add (text);
			TreeViewColumn column1 = new TreeViewColumn("PARAMETROS",text,"text",Column.col01);
			column1.Resizable = true;
			column1.SortColumnId = (int) Column.col01;
			treeview_objetivo3.InsertColumn (column1, (int) Column.col01);
			
			text = new CellRendererText();
			text.Xalign = 0.0f;
			text.Editable = true;
			text.Xalign = 0.0f;
			text.Edited += new EditedHandler(NumberCellEdited_obj3_od);
			TreeViewColumn column2 = new TreeViewColumn("OJO DERECHO",text,"text",Column.col02);
			column2.Resizable = true;
			column2.SortColumnId = (int) Column.col02;
			//text.CellBackground = "red";
			treeview_objetivo3.InsertColumn (column2, (int) Column.col02);
			
			text = new CellRendererText();
			text.Xalign = 0.0f;
			text.Editable = true;
			text.Edited += new EditedHandler(NumberCellEdited_obj3_oi);
			columns.Add (text);
			TreeViewColumn column3 = new TreeViewColumn("OJO IZQUIERDO",text,"text",Column.col03);
			column3.Resizable = true;
			column3.SortColumnId = (int) Column.col03;
			//text.CellBackground = "yellow";
			treeview_objetivo3.InsertColumn (column3, (int) Column.col03);
		}
		
		void NumberCellEdited_obj3_od(object sender, EditedArgs args)
		{
			//Gtk.CellRendererText obj_sender = (object) sender as CellRendererText;
			Gtk.TreeIter iter;
			TreePath path = new TreePath (args.Path);
			if (treeview_objetivo3.Model.GetIter (out iter, path)){
				treeview_objetivo3.Model.SetValue(iter,2,args.NewText );		
			}
		}
		
		void NumberCellEdited_obj3_oi(object sender, EditedArgs args)
		{
			//Gtk.CellRendererText obj_sender = (object) sender as CellRendererText;
			Gtk.TreeIter iter;
			TreePath path = new TreePath (args.Path);
			if (treeview_objetivo3.Model.GetIter (out iter, path)){
				treeview_objetivo3.Model.SetValue(iter,3,args.NewText );		
			}
		}
		
		void ItemToggled (object sender, ToggledArgs args)
		{
			Gtk.TreeIter iter; 			
			TreePath path = new TreePath (args.Path);
			if (treeview_objetivo3.Model.GetIter (out iter, path)){					
				bool old = (bool) treeview_objetivo3.Model.GetValue(iter,0);
				treeview_objetivo3.Model.SetValue(iter,0,!old);
				if((bool) treeview_objetivo3.Model.GetValue(iter,0) == true){
					treeview_objetivo3.Model.SetValue(iter,2,(string) treeview_objetivo3.Model.GetValue(iter,4));
					treeview_objetivo3.Model.SetValue(iter,3,(string) treeview_objetivo3.Model.GetValue(iter,4));
				}else{
					treeview_objetivo3.Model.SetValue(iter,2,"");
					treeview_objetivo3.Model.SetValue(iter,3,"");
				}
			}						
		}
		
		void llenado_treeview_objetivo3()
		{
			treeViewEngine_Obj3.Clear();
			NpgsqlConnection conexion; 
			conexion = new NpgsqlConnection (connectionString+nombrebd);
            // Verifica que la base de datos este conectada
			try{
				conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand ();               	
				comando.CommandText = "SELECT descripcion_parametro,osiris_his_explfis_parametros.id_titulo_explfis,id_parametro,id_secuencia_parametro,descripcion_titulo,valor_default " +
					"FROM osiris_his_explfis_parametros,osiris_his_explfis_titulos " +
					"WHERE osiris_his_explfis_parametros.id_titulo_explfis = osiris_his_explfis_titulos.id_titulo_explfis " +
					"AND id_tipo_admision = '940' " +
					"AND id_parametro = '3' " +
					"AND activo = 'true' " +
					"ORDER BY osiris_his_explfis_parametros.id_titulo_explfis,id_parametro,id_secuencia_parametro";
				//Console.WriteLine(comando.CommandText);
				NpgsqlDataReader lector = comando.ExecuteReader ();
				while(lector.Read()){
					iter = treeViewEngine_Obj3.AppendValues (false,
								    lector["descripcion_parametro"],
				                    "",
					                "",
					                lector["valor_default"],
					                int.Parse(lector["id_titulo_explfis"].ToString()),
					                int.Parse(lector["id_parametro"].ToString()),
					                int.Parse(lector["id_secuencia_parametro"].ToString()));
					
				}
			}catch (NpgsqlException ex){
				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
										MessageType.Error,ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
				msgBoxError.Run ();				msgBoxError.Destroy();
			}
			conexion.Close ();
		}
		
		void crea_treeview_soap()
		{
			//treeview_registro_soap
			//treeViewEngine_registro_soap
			
			Gtk.CellRendererText text;
			Gtk.CellRendererToggle toggle;			
			// Erase all columns
			foreach (TreeViewColumn tvc in treeview_registro_soap.Columns)
			treeview_registro_soap.RemoveColumn(tvc);
			treeview_registro_soap.Selection.Mode = SelectionMode.Multiple;
			treeViewEngine_registro_soap = new TreeStore(typeof(string),
			                                             typeof(bool),
			                                             typeof(string),
			                                             typeof(int),
			                                             typeof(string),
			                                             typeof(string),
				                                         typeof(bool),
			                                             typeof(bool),
			                                             typeof(string),
			                                             typeof(int),
			                                             typeof(string),
			                                             typeof(string),
			                                             typeof(bool),
			                                             typeof(string),
			                                             typeof(string));
			treeview_registro_soap.Model = treeViewEngine_registro_soap;
			treeview_registro_soap.RulesHint = true;
			treeview_registro_soap.Selection.Mode = SelectionMode.Multiple;
			treeview_registro_soap.RowActivated += on_llena_soap_espec_clicked;
				
			// column for holiday names
			text = new CellRendererText ();
			text.Xalign = 0.0f;
			columns.Add (text);
			TreeViewColumn column0 = new TreeViewColumn("fecha", text,"text", Column_soap.fechasoap);
			column0.Resizable = true;
			column0.SortColumnId = (int) Column_soap.fechasoap;
														
			toggle = new CellRendererToggle ();
			toggle.Xalign = 0.0f;
			columns.Add (toggle);
			toggle.Toggled += new ToggledHandler (ItemToggled_soap);
			TreeViewColumn column1 = new TreeViewColumn ("Seleccion", toggle,"active", (int) Column_soap.seleccion,"visible", (int) Column_soap.Visible,"activatable", (int) Column_soap.World);
			column1.Sizing = TreeViewColumnSizing.Fixed;
			column1.FixedWidth = 65;
			column1.Clickable = true;
			column1.Resizable = true;
			column1.SortColumnId = (int) Column_soap.seleccion;
							
			text = new CellRendererText ();
			text.Xalign = 0.0f;
			columns.Add (text);
			TreeViewColumn column2 = new TreeViewColumn("Titulo", text,"text", Column_soap.tituloesp);
			column2.Resizable = true;
			column2.SortColumnId = (int) Column_soap.tituloesp;
						
			text = new CellRendererText ();
			text.Xalign = 0.0f;
			columns.Add (text);
			TreeViewColumn column3 = new TreeViewColumn("N° Atencion", text,"text", Column_soap.nro_atencion);
			column3.Resizable = true;
			column3.SortColumnId = (int) Column_soap.nro_atencion;
						
			text = new CellRendererText ();
			text.Xalign = 0.0f;
			columns.Add (text);
			TreeViewColumn column4 = new TreeViewColumn("Motivo de Ingreso", text,"text", Column_soap.motivoingreso);
			column4.Resizable = true;
			column4.SortColumnId = (int) Column_soap.motivoingreso;
			
			text = new CellRendererText ();
			text.Xalign = 0.0f;
			columns.Add (text);
			TreeViewColumn column5 = new TreeViewColumn("Departamento", text,"text", Column_soap.departamento);
			column5.Resizable = true;
			column5.SortColumnId = (int) Column_soap.departamento;
			
			treeview_registro_soap.InsertColumn (column0, (int) Column_soap.fechasoap);
			treeview_registro_soap.InsertColumn (column1, (int) Column_soap.seleccion);
			treeview_registro_soap.InsertColumn (column2, (int) Column_soap.tituloesp);
			treeview_registro_soap.InsertColumn (column3, (int) Column_soap.nro_atencion);
			treeview_registro_soap.InsertColumn (column4, (int) Column_soap.motivoingreso);
			treeview_registro_soap.InsertColumn (column5, (int) Column_soap.departamento);
		}
				
		enum Column_soap
		{
			fechasoap,
			seleccion,
			tituloesp,
			nro_atencion,
			motivoingreso,
			departamento,
			Visible,
			World,			
		}
		
		void on_button_elimina_regsoap_clicked(object sender, EventArgs args)
		{
			TreeIter iter;
 			TreeModel model;
			TreeSelection tSelect = treeview_registro_soap.Selection;
			Gtk.TreePath[] tPaths = tSelect.GetSelectedRows(out model);
			MessageDialog msgBox = new MessageDialog (MyWin,DialogFlags.Modal,MessageType.Question,ButtonsType.YesNo,"¿ Esta seguro de eliminar el registro ?");
			ResponseType miResultado = (ResponseType) msgBox.Run ();				msgBox.Destroy();
			
			if(miResultado == ResponseType.Yes){
				NpgsqlConnection conexion; 
				conexion = new NpgsqlConnection (connectionString+nombrebd);
    	        // Verifica que la base de datos este conectada
				try{				
					conexion.Open ();
					NpgsqlCommand comando; 
					comando = conexion.CreateCommand();
					foreach(TreePath tPath in tPaths){
						if(treeViewEngine_registro_soap.GetIter(out iter,tPath)){
							comando.CommandText = "UPDATE osiris_his_informacion_medica "+
											"SET eliminado = 'true',"+
											"fechahora_eliminado = '"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"',"+
											"id_quien_elimino = '"+LoginEmpleado+"' " +
											"WHERE id_secuencia =  '"+(string) treeViewEngine_registro_soap.GetValue(iter,7)+"';";											
							//Console.WriteLine(comando.CommandText);
							comando.ExecuteNonQuery();
							comando.Dispose();
							
							if((string) treeViewEngine_registro_soap.GetValue(iter,12) != "0"){
								// Elimina registro de movimiento de especialidades medicas
								
							}
						}
            		}
					llenado_treeview_soap();
					creando_vista_textview();					
				}catch (NpgsqlException ex){
					MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
															MessageType.Error, 
															ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
					msgBoxError.Run ();				conexion.Close();		
				}
				conexion.Close();	
			}
		}

		void crea_treeview_codigoscie10()
		{
			treeViewEngine_codigoscie10 = new ListStore(typeof(bool),
														typeof(string),
														typeof(string),
														typeof(string));
			treeview_codigoscie10.Model = treeViewEngine_codigoscie10;
			treeview_codigoscie10.RulesHint = true;

			TreeViewColumn column0 = new TreeViewColumn();
			CellRendererToggle cellrt00 = new CellRendererToggle();
			column0.Title = "Selec.";
			column0.PackStart(cellrt00, true);
			column0.AddAttribute (cellrt00, "active", 0);
			cellrt00.Activatable = true;
			cellrt00.Toggled += selecciona_fila_cie10;

			TreeViewColumn column1 = new TreeViewColumn();
			CellRendererText cellrt01 = new CellRendererText();
			column1.Title = "Codigo";
			column1.PackStart(cellrt01, true);
			column1.AddAttribute (cellrt01, "text", 1);

			TreeViewColumn column2 = new TreeViewColumn();
			CellRendererText cellrt02 = new CellRendererText();
			column2.Title = "ID CIE-10";
			column2.PackStart(cellrt02, true);
			column2.AddAttribute (cellrt02, "text", 2);

			TreeViewColumn column3 = new TreeViewColumn();
			CellRendererText cellrt03 = new CellRendererText();
			column3.Title = "Diagnostico CIE-10";
			column3.PackStart(cellrt03, true);
			column3.AddAttribute (cellrt03, "text", 3);

			treeview_codigoscie10.AppendColumn(column0);
			treeview_codigoscie10.AppendColumn(column1);
			treeview_codigoscie10.AppendColumn(column2);
			treeview_codigoscie10.AppendColumn(column3);
		}

		enum Column_cie10
		{
			seleccion,codigo_osiris,idcie10,diagcie10
		}

		// Cuando selecciona la columna con la propiedad toogled
		void selecciona_fila_cie10(object sender, ToggledArgs args)
		{
			TreeIter iter;
			if (treeview_codigoscie10.Model.GetIter (out iter,new TreePath (args.Path))) {
				bool old = (bool) treeview_codigoscie10.Model.GetValue (iter,0);
				treeview_codigoscie10.Model.SetValue(iter,0,!old);
			}
		}
		
		private void ItemToggled_soap (object sender, ToggledArgs args)
		{
			Gtk.TreeIter iter1;
			TreePath path = new TreePath (args.Path);
			if (treeview_registro_soap.Model.GetIter (out iter1, path)){					
				bool old = (bool) treeview_registro_soap.Model.GetValue(iter1,1);
				treeview_registro_soap.Model.SetValue(iter1,1,!old);
				creando_vista_textview();
			}						
		}

		void llenado_template_cie10()
		{
			treeViewEngine_codigoscie10.Clear();
			NpgsqlConnection conexion; 
			conexion = new NpgsqlConnection (connectionString+nombrebd);
			// Verifica que la base de datos este conectada
			try{
				conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand ();               	
				comando.CommandText = "SELECT osiris_his_templates_cie10.id_diagnostico AS iddiagnostico,osiris_his_templates_cie10.id_especialidad," +
					"id_orden_interno,activo,osiris_his_templates_cie10.id_cie_10 AS idcie10,osiris_his_tipo_diagnosticos.descripcion_diagnostico AS descripciondiagnostico " +
					"FROM osiris_his_templates_cie10,osiris_his_tipo_diagnosticos " +
					"WHERE osiris_his_templates_cie10.id_diagnostico = osiris_his_tipo_diagnosticos.id_diagnostico " +
					"AND osiris_his_templates_cie10.id_especialidad = '" +idespecialidad.ToString().Trim()+"' " +
					"AND activo = 'true' "+
					"ORDER BY osiris_his_tipo_diagnosticos.descripcion_diagnostico;";
				//Console.WriteLine(comando.CommandText);
				NpgsqlDataReader lector = comando.ExecuteReader ();
				while(lector.Read()){
					treeViewEngine_codigoscie10.AppendValues(false,
						lector["iddiagnostico"].ToString().Trim(),
						lector["idcie10"].ToString().Trim(),
						lector["descripciondiagnostico"].ToString().Trim());
				}
			}catch (NpgsqlException ex){
				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
					MessageType.Error,ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
				msgBoxError.Run ();				msgBoxError.Destroy();
			}
			conexion.Close ();
		}
		
		void on_button_hc_seleccionada_clicked (object sender, EventArgs args)
		{
			creando_vista_textview();
		}
		
		void creando_vista_textview()
		{
			string secuencia_a_elegir = "";
			bufferhisclinica = textview_hisclinica.Buffer;
			insertIterhc = bufferhisclinica.StartIter;
			
			bufferhisclinica.Clear();
			bufferhisclinica = textview_hisclinica.Buffer;
			
			insertIterhc = bufferhisclinica.StartIter;
			Gtk.TreeIter iter2;
			if(treeViewEngine_registro_soap.GetIterFirst (out iter2)){
				if((bool) treeview_registro_soap.Model.GetValue(iter2,1) == true){
					seleccion_campo_table((string) treeview_registro_soap.Model.GetValue(iter2,10),
						                      (string) treeview_registro_soap.Model.GetValue(iter2,13),
						                      false,
						                      (int) treeview_registro_soap.Model.GetValue(iter2,3));
						
					// anexa informacion anexesa al SOAP como observaciones en la tambla his_informacion_medica
					// se enlasa con tabla osiris_his_explfis_titulos    campo anexar_info_tablesoap    boleano    true
					if((bool) treeview_registro_soap.Model.GetValue(iter2,12) == true){
							seleccion_campo_table("osiris_his_informacion_medica",
							                      (string) treeview_registro_soap.Model.GetValue(iter2,13),
							                      true,
							                      (int) treeview_registro_soap.Model.GetValue(iter2,3));
					}
														
				}
				while(treeViewEngine_registro_soap.IterNext(ref iter2)){
					if((bool) treeview_registro_soap.Model.GetValue(iter2,1) == true){
						seleccion_campo_table((string) treeview_registro_soap.Model.GetValue(iter2,10),
							                      (string) treeview_registro_soap.Model.GetValue(iter2,13),
							                      false,
							                      (int) treeview_registro_soap.Model.GetValue(iter2,3));
							
							
						// anexa informacion anexesa al SOAP como observaciones en la tambla his_informacion_medica
						// se enlasa con tabla osiris_his_explfis_titulos    campo anexar_info_tablesoap    boleano    true   
						if((bool) treeview_registro_soap.Model.GetValue(iter2,12) == true){
							seleccion_campo_table("osiris_his_informacion_medica",
								                      (string) treeview_registro_soap.Model.GetValue(iter2,13),
								                      true,
								                      (int) treeview_registro_soap.Model.GetValue(iter2,3));
						}							
					}
				}
			}			
		}
		
		/// <summary>
		/// Seleccion_campo_table the specified name_table_data, idsecuncia and padreehijo.
		/// </summary>
		/// <param name='name_table_data'>
		/// Name_table_data.
		/// </param>
		/// <param name='idsecuncia'>
		/// Idsecuncia.
		/// </param>
		/// <param name='padreehijo'>
		/// Padreehijo.
		/// </param>
		void seleccion_campo_table(string name_table_data, string idsecuncia,bool padreehijo, int numeroatencion)
		{	
			string query_notas_evolucion = "";
			int idparametro;
			if("osiris_his_informacion_medica" == name_table_data){
				query_notas_evolucion = "SELECT to_char(fecha_anotacion,'yyyy-MM-dd') AS fechaanotacion,descripcion_titulo,pid_paciente,folio_de_servicio," +
								"osiris_his_informacion_medica.id_secuencia,osiris_his_informacion_medica.id_titulo_explfis AS idtituloexplfis," +
								"s_subjetivo,o_objetivo,a_analisis,p_plan,o_objetivo2,descripcion_especialidad "+
								"FROM "+name_table_data+",osiris_his_explfis_titulos,osiris_his_tipo_especialidad " +
								"WHERE osiris_his_informacion_medica.id_titulo_explfis = osiris_his_explfis_titulos.id_titulo_explfis " +
								"AND "+name_table_data+".id_especialidad = osiris_his_tipo_especialidad.id_especialidad " +
								"AND "+name_table_data+".eliminado = 'false' "+
								"AND pid_paciente = '" +entry_pid_paciente.Text.Trim()+"' " +
								"AND osiris_his_informacion_medica.secuencia_interna= '"+idsecuncia+"';";				
			}
			if("osiris_his_explfis_mov" == name_table_data){
				query_notas_evolucion = "SELECT to_char(fecha_anotacion,'yyyy-MM-dd') AS fechaanotacion,secuencia_interna,descripcion_parametro,id_parametro,notas_derecha," +
					"notas_izquierda,folio_de_servicio,descripcion_titulo,osiris_his_explfis_mov.id_titulo_explfis AS idtituloexplfis " +
					"FROM "+name_table_data+",osiris_his_explfis_titulos " +
					"WHERE osiris_his_explfis_mov.id_titulo_explfis = osiris_his_explfis_titulos.id_titulo_explfis " +
					"AND pid_paciente = '" +entry_pid_paciente.Text.Trim()+"' " +
					"AND secuencia_interna = '"+idsecuncia+"';"; 
			}
			
			NpgsqlConnection conexion;
			conexion = new NpgsqlConnection (connectionString+nombrebd);
            // Verifica que la base de datos este conectada
			try{
				conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand ();               	
				comando.CommandText = query_notas_evolucion;
				//Console.WriteLine(comando.CommandText);
				NpgsqlDataReader lector = comando.ExecuteReader ();
				if(lector.Read()){
					if("osiris_his_informacion_medica" == name_table_data){
						if(padreehijo == false){
							bufferhisclinica.InsertWithTagsByName (ref insertIterhc,"Fecha Nota de Evolucion "+lector["fechaanotacion"].ToString().Trim()+"  N° Atencion "+lector["folio_de_servicio"].ToString().Trim()+" \n","courier new bold");
							bufferhisclinica.InsertWithTagsByName (ref insertIterhc,"          S.O.A.P.\n ","courier new bold");
							bufferhisclinica.Insert(ref insertIterhc, "\n");
							bufferhisclinica.InsertWithTagsByName (ref insertIterhc,"SUBJETIVO/Resumen del Interrogatorio\n","courier new bold");
							bufferhisclinica.InsertWithTagsByName (ref insertIterhc, (string) lector["s_subjetivo"].ToString().ToUpper()+"\n","courier new");
							bufferhisclinica.Insert (ref insertIterhc, "\n");
							
							bufferhisclinica.InsertWithTagsByName (ref insertIterhc,"OBJETIVO/Exploracion Física\n","courier new bold");
							bufferhisclinica.InsertWithTagsByName (ref insertIterhc, (string) lector["o_objetivo"].ToString().ToUpper()+"\n","courier new");
							bufferhisclinica.Insert (ref insertIterhc, "\n");
							
							bufferhisclinica.InsertWithTagsByName (ref insertIterhc,"ANALISIS/Diagnostico\n","courier new bold");
							bufferhisclinica.InsertWithTagsByName (ref insertIterhc, (string) lector["a_analisis"].ToString().ToUpper()+"\n","courier new");
							bufferhisclinica.Insert (ref insertIterhc, "\n");
							
							bufferhisclinica.InsertWithTagsByName (ref insertIterhc,"PLAN/Plan de Manejo\n","courier new bold");
							bufferhisclinica.InsertWithTagsByName (ref insertIterhc, (string) lector["p_plan"].ToString().ToUpper()+"\n","courier new");
							bufferhisclinica.Insert (ref insertIterhc, "\n");
						}
						if(padreehijo == true){
							bufferhisclinica.InsertWithTagsByName (ref insertIterhc,"Notas de la especialidad\n","courier new bold");
							bufferhisclinica.InsertWithTagsByName (ref insertIterhc, (string) lector["o_objetivo2"].ToString().ToUpper()+"\n","courier new");
							bufferhisclinica.Insert (ref insertIterhc, "\n");
						}
					}
					if("osiris_his_explfis_mov" == name_table_data){
						idparametro = (int) lector["id_parametro"];
						bufferhisclinica.InsertWithTagsByName (ref insertIterhc,"Fecha Nota de Evolucion "+lector["fechaanotacion"].ToString().Trim()+"  N° Atencion "+lector["folio_de_servicio"].ToString().Trim()+" \n","courier new bold");
						bufferhisclinica.InsertWithTagsByName (ref insertIterhc,"           "+(string) lector["descripcion_titulo"],"courier new bold");
						bufferhisclinica.Insert(ref insertIterhc, "\n");
						// OPTOMETRIA
						if((int) lector["id_parametro"] == 1){
							bufferhisclinica.InsertWithTagsByName (ref insertIterhc, "PARAMETROS".ToString().ToUpper().PadRight(30)+"|"+
								                                       "OJO DERECHO".ToString().ToUpper().PadRight(25)+"|"+
								                                       "OJO IZQUIERDO".ToString().ToUpper().PadRight(25)+"|"+
								                                       "\n","courier new bold");							
							bufferhisclinica.InsertWithTagsByName (ref insertIterhc, (string) lector["descripcion_parametro"].ToString().ToUpper().PadRight(30)+"|"+
								                                       (string) lector["notas_derecha"].ToString().ToUpper().PadRight(25)+"|"+
								                                       (string) lector["notas_izquierda"].ToString().ToUpper().PadRight(25)+"|"+
								                                       "\n","courier new");						
						}
						// OPTOMETRIA
						if((int) lector["id_parametro"] == 2){
							bufferhisclinica.InsertWithTagsByName (ref insertIterhc, (string) lector["descripcion_parametro"].ToString().ToUpper().PadRight(30)+"|"+
								                                       (string) lector["notas_derecha"].ToString().ToUpper()+"|"+
								                                       (string) lector["notas_izquierda"].ToString().ToUpper()+"|"+
								                                       "\n","courier new");
						}							
						// OFTALMOLOGIA
						if((int) lector["id_parametro"] == 3){
								bufferhisclinica.InsertWithTagsByName (ref insertIterhc, "PARAMETROS".ToString().ToUpper().PadRight(30)+"|"+
									                                       "OJO DERECHO".ToString().ToUpper().PadRight(25)+"|"+
									                                       "OJO IZQUIERDO".ToString().ToUpper().PadRight(25)+"|"+
									                                       "\n","courier new bold");
								bufferhisclinica.InsertWithTagsByName (ref insertIterhc, (string) lector["descripcion_parametro"].ToString().ToUpper().PadRight(30)+"|"+
									                                       (string) lector["notas_derecha"].ToString().ToUpper().PadRight(25)+"|"+
									                                       (string) lector["notas_izquierda"].ToString().ToUpper().PadRight(25)+"|"+
									                                       "\n","courier new");
						}					
					}
					while(lector.Read()){
						if("osiris_his_informacion_medica" == name_table_data){
							if(padreehijo == false){
								bufferhisclinica.InsertWithTagsByName (ref insertIterhc,"Fecha Nota de Evolucion "+lector["fechaanotacion"].ToString().Trim()+"  N° Atencion "+lector["folio_de_servicio"].ToString().Trim()+" \n","courier new bold");
								bufferhisclinica.InsertWithTagsByName (ref insertIterhc,"          S.O.A.P.\n ","courier new bold");
								bufferhisclinica.Insert(ref insertIterhc, "\n");
								bufferhisclinica.InsertWithTagsByName (ref insertIterhc,"SUBJETIVO/Resumen del Interrogatorio\n","courier new bold");
								bufferhisclinica.InsertWithTagsByName (ref insertIterhc, (string) lector["s_subjetivo"].ToString().ToUpper()+"\n","courier new");
								bufferhisclinica.Insert (ref insertIterhc, "\n");
								
								bufferhisclinica.InsertWithTagsByName (ref insertIterhc,"OBJETIVO/Exploracion Física\n","courier new bold");
								bufferhisclinica.InsertWithTagsByName (ref insertIterhc, (string) lector["o_objetivo"].ToString().ToUpper()+"\n","courier new");
								bufferhisclinica.Insert (ref insertIterhc, "\n");
								
								bufferhisclinica.InsertWithTagsByName (ref insertIterhc,"ANALISIS/Diagnostico\n","courier new bold");
								bufferhisclinica.InsertWithTagsByName (ref insertIterhc, (string) lector["a_analisis"].ToString().ToUpper()+"\n","courier new");
								bufferhisclinica.Insert (ref insertIterhc, "\n");
								
								bufferhisclinica.InsertWithTagsByName (ref insertIterhc,"PLAN/Plan de Manejo\n","courier new bold");
								bufferhisclinica.InsertWithTagsByName (ref insertIterhc, (string) lector["p_plan"].ToString().ToUpper()+"\n","courier new");
							}
							if(padreehijo == true){
								bufferhisclinica.InsertWithTagsByName (ref insertIterhc,"Notas de la especialidad\n","courier new bold");
								bufferhisclinica.InsertWithTagsByName (ref insertIterhc, (string) lector["o_objetivo2"].ToString().ToUpper()+"\n","courier new");
								bufferhisclinica.Insert (ref insertIterhc, "\n");
							}
						}
						if("osiris_his_explfis_mov" == name_table_data){
							idparametro = (int) lector["id_parametro"];
							// OPTOMETRIA
							if((int) lector["id_parametro"] == 1){
								bufferhisclinica.InsertWithTagsByName (ref insertIterhc, (string) lector["descripcion_parametro"].ToString().ToUpper().PadRight(30)+"|"+
								                                       (string) lector["notas_derecha"].ToString().ToUpper().PadRight(25)+"|"+
								                                       (string) lector["notas_izquierda"].ToString().ToUpper().PadRight(25)+"|"+
								                                       "\n","courier new");
							}
							// OPTOMETRIA
							if((int) lector["id_parametro"] == 2){
								bufferhisclinica.InsertWithTagsByName (ref insertIterhc, (string) lector["descripcion_parametro"].ToString().ToUpper().PadRight(30)+"|"+
								                                       (string) lector["notas_derecha"].ToString().ToUpper()+"#"+
								                                       (string) lector["notas_izquierda"].ToString().ToUpper()+"|"+
								                                       "\n","courier new");
							}							
							// OFTALMOLOGIA
							if((int) lector["id_parametro"] == 3){
								bufferhisclinica.InsertWithTagsByName (ref insertIterhc, (string) lector["descripcion_parametro"].ToString().ToUpper().PadRight(30)+"|"+
								                                       (string) lector["notas_derecha"].ToString().ToUpper().PadRight(25)+"|"+
								                                       (string) lector["notas_izquierda"].ToString().ToUpper().PadRight(25)+"|"+
								                                       "\n","courier new");
							}
						}
					}	
					bufferhisclinica.Insert (ref insertIterhc, "\n");
				}
			}catch (NpgsqlException ex){
				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
										MessageType.Error,ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
				msgBoxError.Run ();				msgBoxError.Destroy();
			}
			conexion.Close ();
		}
		
		void llenado_treeview_soap()
		{
			treeViewEngine_registro_soap.Clear();
			string motivodeingreso = (string) classpublic.lee_registro_de_tabla("osiris_erp_movcargos","descripcion_diagnostico_movcargos","WHERE folio_de_servicio = '"+entry_numerotencion.Text.Trim()+"' ","descripcion_diagnostico_movcargos","string");
			string tomafechaexplfisica;
			int tomanumeroatencion;
			string nametable_selection = "";
			NpgsqlConnection conexion;
			conexion = new NpgsqlConnection (connectionString+nombrebd);
            // Verifica que la base de datos este conectada
			try{
				conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand ();               	
				comando.CommandText = "SELECT to_char(fecha_anotacion,'yyyy-MM-dd') AS fechaanotacion,descripcion_titulo,osiris_his_informacion_medica.pid_paciente,folio_de_servicio,secuencia_interna," +
					"osiris_his_informacion_medica.id_secuencia,osiris_his_informacion_medica.id_titulo_explfis AS idtituloexplfis,anexar_info_tablesoap," +
					"osiris_his_informacion_medica.id_tipo_admisiones,descripcion_admisiones,osiris_his_explfis_titulos.name_table_contents," +
					"to_char(osiris_his_paciente.fechahora_registro_paciente,'dd-mm-yyyy') AS fechacreaexpe," +
					"to_char(osiris_his_paciente.fechahora_registro_paciente,'HH24:mi') AS horacreaexpe,"+
					"descripcion_especialidad " +
					"FROM osiris_his_informacion_medica,osiris_his_explfis_titulos,osiris_his_tipo_admisiones,osiris_his_tipo_especialidad,osiris_his_paciente " +
					"WHERE osiris_his_informacion_medica.id_titulo_explfis = osiris_his_explfis_titulos.id_titulo_explfis " +
					"AND osiris_his_informacion_medica.id_tipo_admisiones = osiris_his_tipo_admisiones.id_tipo_admisiones " +
					"AND osiris_his_informacion_medica.id_especialidad = osiris_his_tipo_especialidad.id_especialidad " +
					"AND osiris_his_informacion_medica.pid_paciente = '" +entry_pid_paciente.Text.Trim()+"' " +
					"AND osiris_his_informacion_medica.pid_paciente = osiris_his_paciente.pid_paciente " +
					//"AND soap = 'true' " +
					"AND eliminado = 'false' "+
					"AND descripcion_titulo != '' " +
					"ORDER BY to_char(fecha_anotacion,'yyyy-MM-dd') DESC,osiris_his_informacion_medica.id_titulo_explfis ;";
				//Console.WriteLine(comando.CommandText);
				NpgsqlDataReader lector = comando.ExecuteReader ();
				if(lector.Read()){					
					tomafechaexplfisica = lector["fechaanotacion"].ToString().Trim();
					tomanumeroatencion = (int) lector["folio_de_servicio"];
					
					fecha_crea_expediente = lector["fechacreaexpe"].ToString().Trim();
					hora_crea_expediente = lector["horacreaexpe"].ToString().Trim();
					
					treeViewEngine_registro_soap.AppendValues(
									lector["fechaanotacion"].ToString().Trim(),
					                false,
				                	lector["descripcion_titulo"].ToString().Trim(),
					                tomanumeroatencion,
					                diagnosticoadmision,
									lector["descripcion_admisiones"].ToString().Trim(),
					                true,
							    	true,
					                lector["id_secuencia"].ToString().Trim(),
					                (int) lector["idtituloexplfis"],
					                lector["name_table_contents"].ToString().Trim(),
					        		"secuencia_interna",
					                (bool) lector["anexar_info_tablesoap"],
									lector["secuencia_interna"].ToString().Trim(),
									lector["descripcion_especialidad"].ToString().Trim());
					//llenado_exploracion_especialidad(tomafechaexplfisica);
					while(lector.Read()){
						//llenado_exploracion_especialidad(tomafechaexplfisica);
						if(lector["fechaanotacion"].ToString().Trim() != tomafechaexplfisica){
							tomafechaexplfisica = lector["fechaanotacion"].ToString().Trim();
							tomanumeroatencion = (int) lector["folio_de_servicio"];
							treeViewEngine_registro_soap.AppendValues (
											lector["fechaanotacion"].ToString().Trim(),
						   					false,
							            	lector["descripcion_titulo"].ToString().Trim(),
			                				tomanumeroatencion,
							                diagnosticoadmision,
											lector["descripcion_admisiones"].ToString().Trim(),
			                				true,
						    				true,
							                lector["id_secuencia"].ToString().Trim(),
					                		(int) lector["idtituloexplfis"],
							                lector["name_table_contents"].ToString().Trim(),
					        				"id_secuencia",
							                (bool) lector["anexar_info_tablesoap"],
											lector["secuencia_interna"].ToString().Trim(),
											lector["descripcion_especialidad"].ToString().Trim());
						}else{
							treeViewEngine_registro_soap.AppendValues (
											"",
						   					false,
							            	lector["descripcion_titulo"].ToString().Trim(),
			                				tomanumeroatencion,
							                "",
											lector["descripcion_admisiones"].ToString().Trim(),
			                				true,
						    				true,
							                lector["id_secuencia"].ToString().Trim(),
					                		(int) lector["idtituloexplfis"],
							                lector["name_table_contents"].ToString().Trim(),
					        				"id_secuencia",
							                (bool) lector["anexar_info_tablesoap"],
											lector["secuencia_interna"].ToString().Trim(),
											lector["descripcion_especialidad"].ToString().Trim());
						}
					}
				}				
			}catch (NpgsqlException ex){
				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
										MessageType.Error,ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
				msgBoxError.Run ();				msgBoxError.Destroy();
			}
			conexion.Close ();
		}
		
		void llenado_exploracion_especialidad(string tomafechaexplfisica)
		{			
			NpgsqlConnection conexion1;
			conexion1 = new NpgsqlConnection (connectionString+nombrebd);
            // Verifica que la base de datos este conectada
			try{
				conexion1.Open ();
				NpgsqlCommand comando1; 
				comando1 = conexion1.CreateCommand ();               	
				comando1.CommandText = "SELECT DISTINCT osiris_his_explfis_mov.id_titulo_explfis,descripcion_titulo,secuencia_interna," +
					"pid_paciente,folio_de_servicio,anexar_info_tablesoap " +
					"FROM osiris_his_explfis_mov,osiris_his_explfis_titulos " +
					"WHERE osiris_his_explfis_mov.id_titulo_explfis = osiris_his_explfis_titulos.id_titulo_explfis " +
					"AND pid_paciente = '" +entry_pid_paciente.Text.Trim()+"' " +
					"AND eliminado = 'false' "+
					"AND descripcion_titulo != '' " +
					"AND to_char(fecha_anotacion,'yyyy-MM-dd') = '"+tomafechaexplfisica+"';";
				//Console.WriteLine(comando1.CommandText);
				NpgsqlDataReader lector1 = comando1.ExecuteReader ();
				while(lector1.Read()){
					treeViewEngine_registro_soap.AppendValues (
							"",
					   		false,
					        lector1["descripcion_titulo"].ToString().Trim(),
					    	(int) lector1["folio_de_servicio"],
					        null,
		                	true,
					    	true,
					        lector1["id_secuencia"].ToString().Trim(),
					        (int) lector1["id_titulo_explfis"],
					        "osiris_his_explfis_mov",
					        "secuencia_interna",
					        (bool) lector1["anexar_info_tablesoap"],
							lector1["secuencia_interna"].ToString().Trim());	
				}				
			}catch (NpgsqlException ex){
				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
									MessageType.Error,ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
				msgBoxError.Run ();				msgBoxError.Destroy();
			}
			conexion1.Close();
		}
				
		void on_llena_soap_espec_clicked(object sender, EventArgs args)
		{
			//Console.WriteLine("hice doble clic");
		}
		
		void on_button_hc_limpiar_clicked(object sender, EventArgs args)
		{
			bufferhisclinica = textview_hisclinica.Buffer;
			insertIterhc = bufferhisclinica.StartIter;
			bufferhisclinica.Clear();
			bufferhisclinica = textview_hisclinica.Buffer;
			insertIterhc = bufferhisclinica.StartIter;
		}
				
		void on_button_buscar_cie10_clicked(object sender, EventArgs args)
		{
			object[] parametros_objetos = {entry_id_cie10,entry_descrip_cie10,treeview_codigoscie10,treeViewEngine_codigoscie10};
			string[] parametros_sql = {"SELECT osiris_his_tipo_diagnosticos.id_diagnostico,"+
					                  "osiris_his_tipo_diagnosticos.descripcion_diagnostico,"+
                                      "osiris_his_tipo_diagnosticos.id_cie_10,"+
                                      "osiris_his_tipo_diagnosticos.id_cie_10_grupo,"+
                                      "osiris_his_tipo_diagnosticos.sub_grupo "+
                                      "FROM osiris_his_tipo_diagnosticos "+
                                      "WHERE osiris_his_tipo_diagnosticos.sub_grupo = 'false' ",				
										"SELECT osiris_his_tipo_diagnosticos.id_diagnostico,"+
					                  "osiris_his_tipo_diagnosticos.descripcion_diagnostico,"+
                                      "osiris_his_tipo_diagnosticos.id_cie_10,"+
                                      "osiris_his_tipo_diagnosticos.id_cie_10_grupo,"+
                                      "osiris_his_tipo_diagnosticos.sub_grupo "+
                                      "FROM osiris_his_tipo_diagnosticos "+
                                      "WHERE osiris_his_tipo_diagnosticos.sub_grupo = 'false' "};
			string[] parametros_string = {};
			string[,] args_buscador1 = {{"DIAGNOSTICO","AND descripcion_diagnostico LIKE '%","%' "},
										{"ID DIAGNOSTICO","AND id_cie_10 = '","' "}};
			string[,] args_buscador2 = {{"DIAGNOSTICO","AND descripcion_diagnostico LIKE '%","%' "},
										{"ID DIAGNOSTICO","AND id_cie_10 = '","' "}};
			string[,] args_orderby = {{"",""}};
			classfind_data.buscandor(parametros_objetos,parametros_sql,parametros_string,"find_cie10",0,args_buscador1,args_buscador2,args_orderby);
		}		

		void on_button_agrega_cie10_clicked(object sender, EventArgs args)
		{
			bufferanalisis = textview_analisis.Buffer;
			insertIteranalisis = bufferanalisis.StartIter;
			bufferanalisis.Clear();
			bufferanalisis = textview_analisis.Buffer;
			insertIteranalisis = bufferanalisis.StartIter;

			Gtk.TreeIter iter2;
			if(treeViewEngine_codigoscie10.GetIterFirst (out iter2)){
				if((bool) treeview_codigoscie10.Model.GetValue(iter2,0) == true){
					bufferanalisis.Insert(ref insertIteranalisis, (string) treeview_codigoscie10.Model.GetValue(iter2,2)+" "+
						(string) treeview_codigoscie10.Model.GetValue(iter2,3)+"\n");
				}
				while(treeViewEngine_codigoscie10.IterNext(ref iter2)){
					if((bool) treeview_codigoscie10.Model.GetValue(iter2,0) == true){
						bufferanalisis.Insert(ref insertIteranalisis, (string) treeview_codigoscie10.Model.GetValue(iter2,2)+" "+
							(string) treeview_codigoscie10.Model.GetValue(iter2,3)+"\n");
					}
				}
			}
		}

		void on_button_imprime_soap_clicked(object sender, EventArgs args)
		{
			new osiris.rpt_soap(LoginEmpleado,NomEmpleado,AppEmpleado,ApmEmpleado,folioservicio.ToString().Trim(),pidpaciente.ToString().Trim(),idespecialidad.ToString(),
			                    treeview_registro_soap,treeViewEngine_registro_soap,entry_nombre_paciente.Text,
			                    entry_edad_paciente.Text,entry_fecha_nacimiento.Text,entry_sexo_paciente.Text,"",fecha_crea_expediente,
			                    hora_crea_expediente,direccionpx,telefonopx,tipopaciente,convenio);
		}

		// cierra ventanas emergentes
		void on_cierraventanas_clicked(object sender, EventArgs args)
		{
			Widget win = (Widget) sender;
			win.Toplevel.Destroy();
		}
	}	
}