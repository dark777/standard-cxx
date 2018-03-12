//////////////////////////////////////////////////////////////////////
// created on 31/08/2007 at 04:16 p
// Sistema Hospitalario OSIRIS
// Monterrey - Mexico
//
// Autor    	: Ing. Juan Antonio Peña Gonzalez (Programacion) gjuanzz@gmail.com
// 				  Ing. Daniel Olivares C. (Modificaciones y Ajustes)
//				  Ing. Jesus Buentello (Ajustes , Reportes)
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
using Gtk;
using Npgsql;
using Glade;
using Cairo;
using Pango;

namespace osiris
{
	public class reporte_porcedimientos_facturados
	{
		private static int pangoScale = 1024;
		private PrintOperation print;
		private double fontSize = 8.0;
		int escala_en_linux_windows;		// Linux = 1  Windows = 8
		int comienzo_linea = 162;
		int separacion_linea = 10;
		int numpage = 1;
		
		string connectionString;
        string nombrebd;
		string LoginEmpleado;
		string NomEmpleado;
		string AppEmpleado;
		string ApmEmpleado;
		string tiporeporte = "NO FACTURADOS";
		string titulo = "REPORTE DE PROCEDIMIENTOS NO FACTURADOS";
		
		string query_fechas = " ";
		string query_cliente = " ";
		string orden = " ";
		string rango1 = "";
		string rango2 = "";
		bool pagados = false;
		
		//Declaracion de ventana de error
		protected Gtk.Window MyWinError;
		
		class_conexion conexion_a_DB = new class_conexion();
		class_public classpublic = new class_public();
		
		public reporte_porcedimientos_facturados (bool pagados_,string rango1_,string rango2_,string query_fechas_,string nombrebd_,string LoginEmpleado_,string NomEmpleado_,
												string AppEmpleado_,string ApmEmpleado_,string tiporeporte_,string orden_,string query_cliente_,bool checkbutton_export_to_)
		{
			connectionString = conexion_a_DB._url_servidor+conexion_a_DB._port_DB+conexion_a_DB._usuario_DB+conexion_a_DB._passwrd_user_DB;
			nombrebd = conexion_a_DB._nombrebd;
			escala_en_linux_windows = classpublic.escala_linux_windows;
			LoginEmpleado = LoginEmpleado_;
			NomEmpleado = NomEmpleado_;
			AppEmpleado = AppEmpleado_;
			ApmEmpleado = ApmEmpleado_;
			query_fechas = query_fechas_;
			query_cliente = query_cliente_;
			tiporeporte = tiporeporte_;
			rango1 = rango1_;
			rango2 = rango2_;
			orden = orden_;
			pagados = pagados_;
			
 			if(pagados == true){
				titulo = "REPORTE DE FACTURAS NO PAGADAS";
			}

 			if(pagados == false){
				if(tiporeporte == "NO FACTURADOS"){
					titulo = "REPORTE DE PROCEDIMIENTOS NO FACTURADOS";
				}
				if(tiporeporte == "FACTURADOS"){
					titulo = "REPORTE DE PROCEDIMIENTOS FACTURADOS";
				}
			}
			
			if(checkbutton_export_to_ == false){
				print = new PrintOperation ();
				print.JobName = titulo;
				print.BeginPrint += new BeginPrintHandler (OnBeginPrint);
				print.DrawPage += new DrawPageHandler (OnDrawPage);
				print.EndPrint += new EndPrintHandler (OnEndPrint);
				print.Run (PrintOperationAction.PrintDialog, null);
			}else{
				if(tiporeporte == "FACTURADOS"){
					string query_sql = "SELECT DISTINCT(osiris_erp_movcargos.folio_de_servicio),serie,osiris_erp_factura_enca.numero_factura,descripcion_cliente,emisor,to_char(osiris_erp_factura_enca.fechahora_factura,'yyyy-MM-dd HH24:mi:ss') AS fechafactura," +
										"osiris_erp_factura_enca.cancelado,osiris_erp_factura_enca.fechahora_cancelacion,osiris_erp_factura_enca.motivo_cancelacion,subtotal_factura,total_impuesto_factura,total_factura,osiris_erp_abonos.pid_paciente AS nroexpe,osiris_erp_cobros_enca.fechahora_creacion AS fechaingreso," +
										"osiris_his_paciente.nombre1_paciente || ' ' || osiris_his_paciente.nombre2_paciente || ' ' || osiris_his_paciente.apellido_paterno_paciente || ' ' || osiris_his_paciente.apellido_materno_paciente AS nombrepaciente," +
										"osiris_erp_abonos.numero_recibo_caja AS nrorecibocaja,osiris_erp_abonos.folio_de_servicio AS nroatencion," +
										"osiris_erp_movcargos.descripcion_diagnostico_movcargos AS motivo_ingreso," +
										"osiris_erp_tipo_comprobante.id_tipo_comprobante,descripcion_tipo_comprobante,monto_de_abono_procedimiento AS monto_comprobante,to_char(osiris_erp_abonos.fechahora_registro,'yyyy-MM-dd HH24:mi:ss') AS fechahora_recibo " +
										"FROM osiris_erp_factura_enca,osiris_erp_emisor,osiris_erp_abonos,osiris_his_paciente,osiris_erp_cobros_enca,osiris_erp_tipo_comprobante,osiris_erp_movcargos " +  //osiris_his_tipo_pacientes " +
										"WHERE osiris_erp_factura_enca.id_emisor = osiris_erp_emisor.id_emisor " +
										"AND osiris_erp_factura_enca.serie || osiris_erp_factura_enca.numero_factura = osiris_erp_abonos.numero_factura " +
										"AND osiris_erp_factura_enca.id_emisor = osiris_erp_abonos.id_emisor " +
										"AND osiris_his_paciente.pid_paciente = osiris_erp_abonos.pid_paciente " +
										"AND osiris_erp_abonos.folio_de_servicio = osiris_erp_cobros_enca.folio_de_servicio " +
										"AND osiris_erp_abonos.folio_de_servicio = osiris_erp_movcargos.folio_de_servicio "+
										"AND osiris_erp_tipo_comprobante.id_tipo_comprobante = osiris_erp_abonos.id_tipo_comprobante " +
										//"AND osiris_erp_cobros_enca.id_tipo_paciente = osiris_his_tipo_pacientes.id_tipo_paciente "+
										query_fechas_+
										"ORDER BY osiris_erp_movcargos.folio_de_servicio;";
									
					string[] args_names_field = {"emisor","serie","nroatencion","numero_factura","nombrepaciente","motivo_ingreso","fechafactura","total_factura","descripcion_cliente","subtotal_factura","total_impuesto_factura","nroexpe","nrorecibocaja","descripcion_tipo_comprobante","monto_comprobante","fechahora_recibo","fechaingreso"};
					string[] args_type_field = {"string","string","float","string","string","string","string","float","string","float","float","float","float","string","float","string","string"};
					string[] args_field_text = {""};
					string[] args_more_title = {""};
					string[,] args_formulas = {{"",""}};
					string[,] args_width = {{"0","8.89cm"},{"1","1.016cm"},{"4","10.16cm"},{"5","10.16cm"},{"8","10.16cm"}};
									
					new osiris.class_traslate_spreadsheet(query_sql,args_names_field,args_type_field,false,args_field_text,"",false,args_more_title,args_formulas,args_width);				
				}
				if(tiporeporte == "FACTURAS"){
					string query_sql = "SELECT serie,osiris_erp_factura_enca.numero_factura,descripcion_cliente,emisor,to_char(osiris_erp_factura_enca.fechahora_factura,'yyyy-MM-dd HH24:mi:ss') AS fechafactura," +
										"osiris_erp_factura_enca.cancelado,fechahora_cancelacion,motivo_cancelacion,subtotal_factura,total_impuesto_factura,total_factura," +
										"cancelado,fechahora_cancelacion,motivo_cancelacion " +
										"FROM osiris_erp_factura_enca,osiris_erp_emisor " +
										"WHERE osiris_erp_factura_enca.id_emisor = osiris_erp_emisor.id_emisor " +
										query_fechas_+
										"ORDER BY osiris_erp_factura_enca.id_emisor,serie,osiris_erp_factura_enca.numero_factura;";
									
					string[] args_names_field = {"emisor","serie","numero_factura","fechafactura","descripcion_cliente","subtotal_factura","total_impuesto_factura","total_factura","cancelado","motivo_cancelacion"};
					string[] args_type_field = {"string","string","float","string","string","float","float","float","string","string"};
					string[] args_field_text = {""};
					string[] args_more_title = {""};
					string[,] args_formulas = {{"",""}};
					string[,] args_width = {{"0","8.89cm"},{"1","1.016cm"},{"4","10.16cm"},{"9","10.16cm"}};
					new osiris.class_traslate_spreadsheet(query_sql,args_names_field,args_type_field,false,args_field_text,"",false,args_more_title,args_formulas,args_width);
				}
			}
		}
		
				
		private void OnBeginPrint (object obj, Gtk.BeginPrintArgs args)
		{
			print.NPages = 1;  // crea cantidad de copias del reporte			
			// para imprimir horizontalmente el reporte
			print.PrintSettings.Orientation = PageOrientation.Landscape;
			//Console.WriteLine(print.PrintSettings.Orientation.ToString());
		}
		
		private void OnDrawPage (object obj, Gtk.DrawPageArgs args)
		{			
			PrintContext context = args.Context;
			ejecutar_consulta_reporte(context);
		}
		
		void ejecutar_consulta_reporte(PrintContext context)
		{	
			Cairo.Context cr = context.CairoContext;
			Pango.Layout layout = context.CreatePangoLayout ();
			Pango.FontDescription desc = Pango.FontDescription.FromString ("Sans");									
			// cr.Rotate(90)  Imprimir Orizontalmente rota la hoja cambian las posiciones de las lineas y columna					
			fontSize = 8.0;			layout = null;			layout = context.CreatePangoLayout ();
			desc.Size = (int)(fontSize * pangoScale);		layout.FontDescription = desc;
		}
		
		void imprime_rpt_facturados()
		{
			
		}
		
		private void OnEndPrint (object obj, Gtk.EndPrintArgs args)
		{
			
		}
	}    

	/// <summary>
	/// Reporte de Facturas Pagadas
	/// </summary>
	public class reporte_facturas_pagadas
	{		
		private static int pangoScale = 1024;
		private PrintOperation print;
		private double fontSize = 8.0;
		int escala_en_linux_windows;		// Linux = 1  Windows = 8
		int comienzo_linea = 162;
		int separacion_linea = 10;
		int numpage = 1;
		
		string connectionString;
		string nombrebd;
		string LoginEmpleado;
		string NomEmpleado;
		string AppEmpleado;
		string ApmEmpleado;
		string tiporeporte;
		string titulo;
		
		int columna = 0;
		int fila = -70;
		int contador = 1;
		
		string query_fechas = " ";
		string query_cliente = " ";
		string orden = " ";
		string rango1 = "";
		string rango2 = "";
		string facturas_ = "";
		
		//Declaracion de ventana de error
		protected Gtk.Window MyWinError;
		
		class_conexion conexion_a_DB = new class_conexion();
		class_public classpublic = new class_public();
		
		public reporte_facturas_pagadas(string rango1_,string rango2_,string query_fechas_,string nombrebd_,string LoginEmpleado_,string NomEmpleado_,
												string AppEmpleado_,string ApmEmpleado_,string tiporeporte_,string orden_,string query_cliente_,string _facturas_)
		{
			connectionString = conexion_a_DB._url_servidor+conexion_a_DB._port_DB+conexion_a_DB._usuario_DB+conexion_a_DB._passwrd_user_DB;
			nombrebd = conexion_a_DB._nombrebd;
			escala_en_linux_windows = classpublic.escala_linux_windows;
			LoginEmpleado = LoginEmpleado_;
			NomEmpleado = NomEmpleado_;
			AppEmpleado = AppEmpleado_;
			ApmEmpleado = ApmEmpleado_;
			query_fechas = query_fechas_;
			query_cliente = query_cliente_;
			tiporeporte = tiporeporte_;
			rango1 = rango1_;
			rango2 = rango2_;
			orden = orden_;
			facturas_ = _facturas_;
			
			if(tiporeporte == "FACTURADOS") { titulo = "REPORTE FACTURAS PAGADAS"; }
			print = new PrintOperation ();
			print.JobName = titulo;
			print.BeginPrint += new BeginPrintHandler (OnBeginPrint);
			print.DrawPage += new DrawPageHandler (OnDrawPage);
			print.EndPrint += new EndPrintHandler (OnEndPrint);
			print.Run (PrintOperationAction.PrintDialog, null);	
			
		}
		
		private void OnBeginPrint (object obj, Gtk.BeginPrintArgs args)
		{
			print.NPages = 1;  // crea cantidad de copias del reporte			
			// para imprimir horizontalmente el reporte
			print.PrintSettings.Orientation = PageOrientation.Landscape;
			//Console.WriteLine(print.PrintSettings.Orientation.ToString());
		}
		
		private void OnDrawPage (object obj, Gtk.DrawPageArgs args)
		{			
			PrintContext context = args.Context;
			ejecutar_consulta_reporte(context);
		}
		
		void ejecutar_consulta_reporte(PrintContext context)
		{
			Cairo.Context cr = context.CairoContext;
			Pango.Layout layout = context.CreatePangoLayout ();
			Pango.FontDescription desc = Pango.FontDescription.FromString ("Sans");									
			// cr.Rotate(90)  Imprimir Orizontalmente rota la hoja cambian las posiciones de las lineas y columna					
			fontSize = 8.0;			layout = null;			layout = context.CreatePangoLayout ();
			desc.Size = (int)(fontSize * pangoScale);		layout.FontDescription = desc;

		}
		
		private void OnEndPrint (object obj, Gtk.EndPrintArgs args)
		{
		}
 	}   
}