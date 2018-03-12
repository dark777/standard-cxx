////////////////////////////////////////////////////////////
// project created on 20/02/2012 at 10:20 a
// Sistema Hospitalario OSIRIS
// Monterrey - Mexico
//
// Autor    	: Ing. Daniel Olivares (Programacion)
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
using Npgsql;
using Gtk;
using Glade;

namespace osiris
{
	public class consulta_medica
	{
		// Boton general para salir de las ventanas
		// Todas la ventanas en glade este boton debe estra declarado identico
		[Widget] Gtk.Button button_salir;
		
		// Declarando ventana principal de Hospitalizacion
		[Widget] Gtk.Window menu_hospitalizacion = null;
		[Widget] Gtk.Button button_cargos_pacientes = null;
		[Widget] Gtk.Button button_soli_material = null;
		[Widget] Gtk.Button button_autorizacion_medicamento = null;
		[Widget] Gtk.Button button_inv_subalmacen = null;
		[Widget] Gtk.Button button_asignacion_habitacion = null;
		[Widget] Gtk.Button button_traspaso_subalmacenes = null;
		[Widget] Gtk.Button button_reportes = null;
		[Widget] Gtk.Label label35 = null;
		
		
		// Ventana de Rango de Fecha
		[Widget] Gtk.Window rango_de_fecha;
		[Widget] Gtk.Entry entry_dia1;
		[Widget] Gtk.Entry entry_dia2;
		[Widget] Gtk.Entry entry_mes1;
		[Widget] Gtk.Entry entry_mes2;
		[Widget] Gtk.Entry entry_ano1;
		[Widget] Gtk.Entry entry_ano2;
		[Widget] Gtk.Entry entry_referencia_inicial;
		[Widget] Gtk.Entry entry_cliente;
		[Widget] Gtk.Label label_orden;
		[Widget] Gtk.Label label_nom_cliente;
		[Widget] Gtk.Label label142;
		[Widget] Gtk.RadioButton radiobutton_cliente;
		[Widget] Gtk.RadioButton radiobutton_fecha;
		[Widget] Gtk.Button button_busca_cliente;
		[Widget] Gtk.Button button_imprime_rangofecha;
		[Widget] Gtk.CheckButton checkbutton_impr_todo_proce;
		[Widget] Gtk.CheckButton checkbutton_todos_los_clientes;
		[Widget] Gtk.CheckButton checkbutton_export_to = null;
		
		string query_fechas = " ";
		string rango1 = "";
		string rango2 = "";
				
		string LoginEmpleado;
		string NomEmpleado;
		string AppEmpleado;
		string ApmEmpleado;
		string nombrebd;
		class_conexion conexion_a_DB = new class_conexion();
		
		public consulta_medica(string LoginEmp_, string NomEmpleado_, string AppEmpleado_, string ApmEmpleado_, string nombrebd_)
		{
			LoginEmpleado = LoginEmp_;
			NomEmpleado = NomEmpleado_;
			AppEmpleado = AppEmpleado_;
			ApmEmpleado = ApmEmpleado_;
			nombrebd = conexion_a_DB._nombrebd;			
			Glade.XML gxml = new Glade.XML (null, "hospitalizacion.glade", "menu_hospitalizacion", null);
			gxml.Autoconnect (this);
			////// Muestra ventana de Glade
			menu_hospitalizacion.Show();
			menu_hospitalizacion.Title = " Menu de Consulta Medica";
			label35.Text = "\nMenu de Consulta Medica\n";
			////// Sale de la ventana
			button_salir.Clicked += new EventHandler(on_cierraventanas_clicked);
			button_cargos_pacientes.Clicked += new EventHandler(on_button_cargos_pacientes_clicked);
			button_soli_material.Clicked += new EventHandler(on_button_soli_material_clicked);
			button_autorizacion_medicamento.Clicked += new EventHandler(on_button_autorizacion_medicamento_clicked);
			button_inv_subalmacen.Clicked += new EventHandler(on_button_inv_subalmacen_clicked);
			button_asignacion_habitacion.Clicked += new EventHandler(on_button_asignacion_habitacion_clicked);
			button_traspaso_subalmacenes.Clicked += new EventHandler(on_button_traspaso_subalmacenes_clicked);
			button_reportes.Clicked += new EventHandler(on_button_reportes_clicked);
		}
		
		void on_button_cargos_pacientes_clicked(object sender, EventArgs args)
		{
			//new osiris.cargos_hospitalizacion(LoginEmpleado,NomEmpleado,AppEmpleado,ApmEmpleado,nombrebd);
			new osiris.cargos_modulos_medicos(LoginEmpleado,NomEmpleado,AppEmpleado,ApmEmpleado,950,"CONSULTA MEDICA",17,"");
		}
		
		void on_button_soli_material_clicked(object sender, EventArgs args)
		{
			new osiris.solicitud_material(LoginEmpleado,NomEmpleado,AppEmpleado,ApmEmpleado,nombrebd,17,0,0,"");
		}
		
		void on_button_autorizacion_medicamento_clicked(object sender, EventArgs args)
		{
			 new osiris.orden_compra_urgencias(LoginEmpleado,NomEmpleado,AppEmpleado,ApmEmpleado,nombrebd,17,"CONSULTA MEDICA",0,"");
		}
		
		void on_button_inv_subalmacen_clicked(object sender, EventArgs args)
		{
			new osiris.inventario_sub_almacen(LoginEmpleado,NomEmpleado,AppEmpleado,ApmEmpleado,nombrebd,17,"CONSULTA MEDICA",1,false);
		}
		
		void on_button_traspaso_subalmacenes_clicked(object sender, EventArgs args)
		{
			new osiris.inventario_sub_almacen(LoginEmpleado,NomEmpleado,AppEmpleado,ApmEmpleado,nombrebd,17,"CONSULTA MEDICA",2,false);
		}
		
		void on_button_asignacion_habitacion_clicked(object sender, EventArgs args)
		{
		   new osiris.asignacion_de_habitacion(LoginEmpleado,NomEmpleado,AppEmpleado,ApmEmpleado,nombrebd, 0);
		}
		
		void on_button_reportes_clicked(object sender, EventArgs args)
		{
			Glade.XML gxml = new Glade.XML (null, "caja.glade", "rango_de_fecha", null);
			gxml.Autoconnect                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                 (this);
			rango_de_fecha.Show();
			
			checkbutton_impr_todo_proce.Label = "Imprime TODO";
			entry_referencia_inicial.IsEditable = false;
			entry_referencia_inicial.Text = DateTime.Now.ToString("dd-MM-yyyy");
			entry_dia1.KeyPressEvent += onKeyPressEvent;
			entry_mes1.KeyPressEvent += onKeyPressEvent;
			entry_ano1.KeyPressEvent += onKeyPressEvent;
			entry_dia2.KeyPressEvent += onKeyPressEvent;
			entry_mes2.KeyPressEvent += onKeyPressEvent;
			entry_ano2.KeyPressEvent += onKeyPressEvent;
			entry_dia1.Text =DateTime.Now.ToString("dd");
			entry_mes1.Text =DateTime.Now.ToString("MM");
			entry_ano1.Text =DateTime.Now.ToString("yyyy");
			entry_dia2.Text =DateTime.Now.ToString("dd");
			entry_mes2.Text =DateTime.Now.ToString("MM");
			entry_ano2.Text =DateTime.Now.ToString("yyyy");
			label_orden.Hide();
			label_nom_cliente.Hide();
			label142.Hide();
			radiobutton_cliente.Hide();
			radiobutton_fecha.Hide();
			checkbutton_todos_los_clientes.Hide();
			entry_referencia_inicial.Hide();
			entry_cliente.Hide();
			button_busca_cliente.Hide();
			
			button_salir.Clicked += new EventHandler(on_cierraventanas_clicked);
			button_imprime_rangofecha.Clicked += new EventHandler(imprime_reporte_abonos);
		}
		
		void imprime_reporte_abonos(object sender, EventArgs args)
		{
			query_fechas = " ";
			rango1 = "";
			rango2 = "";
			if(checkbutton_impr_todo_proce.Active == false){
				rango1 = entry_dia1.Text+"-"+entry_mes1.Text+"-"+entry_ano1.Text;
				rango2 = entry_dia2.Text+"-"+entry_mes2.Text+"-"+entry_ano2.Text;
				query_fechas = "AND to_char(osiris_his_informacion_medica.fechahora_creacion,'yyyy-MM-dd') >= '"+entry_ano1.Text+"-"+entry_mes1.Text+"-"+entry_dia1.Text+"' "+//;//;//'"+rango1+"' "+
				               "AND to_char(osiris_his_informacion_medica.fechahora_creacion,'yyyy-MM-dd') <= '"+entry_ano2.Text+"-"+entry_mes2.Text+"-"+entry_dia2.Text+"' ";			
			}
			string query_sql = "SELECT DISTINCT ON (osiris_his_informacion_medica.folio_de_servicio) osiris_his_informacion_medica.folio_de_servicio," +
			                   "to_char(osiris_his_informacion_medica.fechahora_creacion,'yyyy-MM-dd') AS fechasoap,osiris_his_informacion_medica.pid_paciente AS pidpaciente," +
			                   "osiris_his_informacion_medica.a_analisis AS diagnostico,osiris_his_informacion_medica.p_plan AS tratamiento,"+
			                   "osiris_his_paciente.nombre1_paciente || ' ' || osiris_his_paciente.nombre2_paciente || ' ' || " +
			                   "osiris_his_paciente.apellido_paterno_paciente || ' ' || osiris_his_paciente.apellido_materno_paciente AS nombrepaciente,"+
			                   "osiris_his_informacion_medica.fechahora_creacion,descripcion_titulo,id_empleado_creacion AS nombremedico," +
			                   "osiris_erp_movcargos.vista_primera_vez AS primera_vez,osiris_erp_movcargos.descripcion_diagnostico_movcargos AS mot_ingreso," +
			                   "to_char(osiris_erp_movcargos.fechahora_admision_registro,'yyyy-MM-dd') AS fechaadmision," +
			                   "descripcion_tipo_paciente AS tipopaciente,osiris_erp_movcargos.id_tipo_paciente "+
			                   "FROM osiris_his_informacion_medica,osiris_his_paciente,osiris_his_explfis_titulos,osiris_erp_movcargos,osiris_his_tipo_pacientes "+
			                   "WHERE osiris_his_informacion_medica.pid_paciente = osiris_his_paciente.pid_paciente " +
			                   "AND osiris_his_informacion_medica.folio_de_servicio = osiris_erp_movcargos.folio_de_servicio "+
			                   //"AND to_char(osiris_his_informacion_medica.fechahora_creacion,'yyyy-MM-dd') IN('2013-12-16') "+
			                   "AND osiris_his_informacion_medica.id_titulo_explfis = osiris_his_explfis_titulos.id_titulo_explfis " +
			                   "AND osiris_erp_movcargos.id_tipo_paciente = osiris_his_tipo_pacientes.id_tipo_paciente " +
			                   "AND osiris_his_informacion_medica.id_titulo_explfis = '2' "+
			                   query_fechas+
			                   "ORDER BY osiris_his_informacion_medica.folio_de_servicio;";
			string[] args_names_field = {"fechasoap","fechaadmision","folio_de_servicio","pidpaciente","nombrepaciente","primera_vez","mot_ingreso","diagnostico","tratamiento","nombremedico","tipopaciente","descripcion_titulo"};
			string[] args_type_field = {"string","string","float","float","string","string","string","string","string","string","string","string"};
			string[] args_field_text = {""};
			string[] args_more_title = {""};
			string[,] args_formulas = {{"",""}};
			string[,] args_width = {{"",""}};
			if(checkbutton_export_to.Active == true){
				new osiris.class_traslate_spreadsheet(query_sql,args_names_field,args_type_field,false,args_field_text,"",false,args_more_title,args_formulas,args_width);
			}else{
				
			}
		}
		
		void onKeyPressEvent(object o, Gtk.KeyPressEventArgs args)
		{
			//Console.WriteLine(args.Event.Key);
			//Console.WriteLine(Convert.ToChar(args.Event.Key));
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

