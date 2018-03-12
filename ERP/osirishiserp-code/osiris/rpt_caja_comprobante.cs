///////////////////////////////////////////////////////////
// project created on 24/10/2006 at 10:20 a
// Sistema Hospitalario OSIRIS
// Monterrey - Mexico
//
// Autor    	: Juan Antonio Peña Gonzalez (Programacion) gjuanzz@gmail.com
// 				  Daniel Olivares Cuevas (Pre-Programacion, Colaboracion y Ajustes) arcangeldoc@openmailbox.org
//				  Traspaso a GTKPrint 23/09/2010
//				  Traspaso a iTextSharp Feb. 2016
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
// Programa		: Osiris
// Proposito	: 
// Objeto		:
/////////////////////////////////////////////////////////
using System;
using Gtk;
using Npgsql;
using Glade;
using Cairo;
using Pango;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;

namespace osiris
{
	public class caja_comprobante
	{
		private static int pangoScale = 1024;
		private PrintOperation print;
		private double fontSize = 8.0;
		int escala_en_linux_windows;		// Linux = 1  Windows = 8
		int comienzo_linea = 60;
		int comienzo_linea2 = 60;
		int separacion_linea = 10;
		int numpage = 1;
		int numero_comprobante;
		
		string connectionString;
        string nombrebd;
		float valoriva;
		string tipocomprobante = "";
		string tipo_paciente = "";
		string empresapac = "";
		string nombreempleado = "";
		string titulo_comprobante = "";
		string desctipocomprobate = "";
		
		PrintContext context;
		
		string sql_compcaja = "";
						//"AND to_char(osiris_erp_movcargos.fechahora_admision_registro,'dd') >= '"+DateTime.Now.ToString("dd")+"'  AND to_char(osiris_erp_movcargos.fechahora_admision_registro,'dd') <= '"+DateTime.Now.ToString("dd")+"' "+
						//"AND to_char(osiris_erp_movcargos.fechahora_admision_registro,'MM') >= '"+DateTime.Now.ToString("MM")+"' AND to_char(osiris_erp_movcargos.fechahora_admision_registro,'MM') <= '"+DateTime.Now.ToString("MM")+"' "+
						//"AND to_char(osiris_erp_movcargos.fechahora_admision_registro,'yyyy') >= '"+DateTime.Now.ToString("yyyy")+"' AND to_char(osiris_erp_movcargos.fechahora_admision_registro,'yyyy') <= '"+DateTime.Now.ToString("yyyy")+"' " ;
		//Declaracion de ventana de error
		string sql_foliodeservicio = "";
		string sql_numerocomprobante = "";
		string sql_orderquery = "";
		protected Gtk.Window MyWinError;
		
		class_conexion conexion_a_DB = new class_conexion();
		class_public classpublic = new class_public();
		
		public caja_comprobante(int numero_comprobante_, string tipo_comprobante_, int folioservicio_, string sql_consulta_, string nombreempleado_)
		{
			connectionString = conexion_a_DB._url_servidor+conexion_a_DB._port_DB+conexion_a_DB._usuario_DB+conexion_a_DB._passwrd_user_DB;
			nombrebd = conexion_a_DB._nombrebd;
			valoriva = float.Parse(classpublic.ivaparaaplicar);
			escala_en_linux_windows = classpublic.escala_linux_windows;
			tipocomprobante = tipo_comprobante_;
			numero_comprobante = numero_comprobante_;
			sql_compcaja = sql_consulta_;
			nombreempleado = nombreempleado_;
			
			if (tipocomprobante == "CAJA"){			
				sql_numerocomprobante = "AND osiris_erp_abonos.numero_recibo_caja = '"+numero_comprobante.ToString().Trim()+"' ";
				sql_foliodeservicio = "AND osiris_erp_cobros_deta.folio_de_servicio = '"+folioservicio_.ToString()+"' ";
				sql_orderquery = " ORDER BY osiris_erp_cobros_deta.id_tipo_admisiones ASC, osiris_productos.id_grupo_producto;";
				rpt_pago_abono(tipocomprobante);
			}
			if (tipocomprobante == "ABONO"){
				sql_numerocomprobante = "AND osiris_erp_abonos.numero_recibo_caja = '"+numero_comprobante.ToString().Trim()+"' ";
				sql_orderquery = "";
				sql_foliodeservicio = "AND osiris_erp_cobros_enca.folio_de_servicio = '"+folioservicio_.ToString()+"' ";
				rpt_pago_abono(tipocomprobante);
			}
			if (tipocomprobante == "SERVICIO"){			
				sql_numerocomprobante = "AND osiris_erp_comprobante_servicio.numero_comprobante_servicio = '"+numero_comprobante.ToString().Trim()+"' ";
				sql_foliodeservicio = "AND osiris_erp_cobros_deta.folio_de_servicio = '"+folioservicio_.ToString()+"' ";
				sql_orderquery = " ORDER BY osiris_erp_cobros_deta.id_tipo_admisiones ASC, osiris_productos.id_grupo_producto;";
				print = new PrintOperation ();
				print.JobName = "IMPRIME COMPROBANTE DE "+tipocomprobante;
				print.BeginPrint += new BeginPrintHandler (OnBeginPrint);
				print.DrawPage += new DrawPageHandler (OnDrawPage);
				print.EndPrint += new EndPrintHandler (OnEndPrint);
				print.Run (PrintOperationAction.PrintDialog, null);
			}
			if (tipocomprobante == "PAGARE"){
				sql_numerocomprobante = "AND osiris_erp_comprobante_pagare.numero_comprobante_pagare = '"+numero_comprobante.ToString().Trim()+"' ";
				sql_foliodeservicio = "AND osiris_erp_cobros_enca.folio_de_servicio = '"+folioservicio_.ToString()+"' ";
				sql_orderquery = " ";
				print = new PrintOperation ();
				print.JobName = "IMPRIME COMPROBANTE DE "+tipocomprobante;
				print.BeginPrint += new BeginPrintHandler (OnBeginPrint);
				print.DrawPage += new DrawPageHandler (OnDrawPage);
				print.EndPrint += new EndPrintHandler (OnEndPrint);
				print.Run (PrintOperationAction.PrintDialog, null);
			}
			if (tipocomprobante == "HONORARIO"){
				sql_numerocomprobante = "AND osiris_erp_abonos.numero_recibo_caja = '"+numero_comprobante.ToString().Trim()+"' ";
				sql_orderquery = "";
				sql_foliodeservicio = "AND osiris_erp_cobros_enca.folio_de_servicio = '"+folioservicio_.ToString()+"' ";
				print = new PrintOperation ();
				print.JobName = "IMPRIME COMPROBANTE DE "+tipocomprobante;
				print.BeginPrint += new BeginPrintHandler (OnBeginPrint);
				print.DrawPage += new DrawPageHandler (OnDrawPage);
				print.EndPrint += new EndPrintHandler (OnEndPrint);
				print.Run (PrintOperationAction.PrintDialog, null);
			}					
		}
		
		private void OnBeginPrint (object obj, Gtk.BeginPrintArgs args)
		{
			print.NPages = 3;  // crea cantidad de copias del reporte			
			
			if (tipocomprobante == "PAGARE"){
				print.NPages = 1;  // crea cantidad de copias del reporte
			}
			// para imprimir horizontalmente el reporte
			//print.PrintSettings.Orientation = PageOrientation.Landscape;
			//Console.WriteLine(print.PrintSettings.Orientation.ToString());
		}
		
		private void OnDrawPage (object obj, Gtk.DrawPageArgs args)
		{			
			context = args.Context;			
			ejecutar_consulta_reporte(context);			
		}
		
		void ejecutar_consulta_reporte(PrintContext context)
		{
			bool apl_desc = false;
			bool apl_desc_siempre = true;
			int toma_tipoadmisiones = 0;
			int toma_grupoproducto = 0;
			int toma_tipocomprobante = 0;
			float toma_valor_total = 0;
			string fechahorapago = "";
			decimal subtotal = 0;
			decimal sumaiva = 0;
			decimal total = 0;
			decimal ivaprod = 0;
			decimal subtiva = 0;
			decimal subtnoiva = 0;
			
			comienzo_linea = 60;
			Cairo.Context cr = context.CairoContext;
			Pango.Layout layout = context.CreatePangoLayout ();
						
			Pango.FontDescription desc = Pango.FontDescription.FromString ("Sans");									
			//cr.Rotate(90);  //Imprimir Orizontalmente rota la hoja cambian las posiciones de las lineas y columna					
			fontSize = 8.0;
			desc.Size = (int)(fontSize * pangoScale);					layout.FontDescription = desc;
			
			NpgsqlConnection conexion; 
	        conexion = new NpgsqlConnection (connectionString+nombrebd);
	        // Verifica que la base de datos este conectada
	        try{
	 			conexion.Open ();
	        	NpgsqlCommand comando; 
	        	comando = conexion.CreateCommand (); 
	           	comando.CommandText = sql_compcaja+sql_numerocomprobante+sql_foliodeservicio+sql_orderquery;
	        	//Console.WriteLine(comando.CommandText);
				NpgsqlDataReader lector = comando.ExecuteReader();
				if (lector.Read()){
					switch (tipocomprobante){	
					case "CAJA":
						toma_valor_total = float.Parse((string) lector["montodelabono"]);
						toma_tipocomprobante = int.Parse(lector["id_tipo_comprobante"].ToString().Trim());
						desctipocomprobate = (string) lector["descripcion_tipo_comprobante"];
						titulo_comprobante = tipocomprobante+"_"+ (string) lector["descripcion_tipo_comprobante"];
						fechahorapago = "|"+(string) lector["fechcreacomp"].ToString().Trim()+"|"+(string) lector["horacreacomp"].ToString().Trim()+"|"+numero_comprobante.ToString().Trim()+"|";
						break;
					case "ABONO":
						toma_valor_total = float.Parse((string) lector["montodelabono"]);
						toma_tipocomprobante = int.Parse(lector["id_tipo_comprobante"].ToString().Trim());
						titulo_comprobante = "CAJA_"+ (string) lector["descripcion_tipo_comprobante"];
						desctipocomprobate = (string) lector["descripcion_tipo_comprobante"];
						fechahorapago = "|"+(string) lector["fechcreacomp"].ToString().Trim()+"|"+(string) lector["horacreacomp"].ToString().Trim()+"|"+numero_comprobante.ToString().Trim()+"|";
						break;
					case "PAGARE":
						toma_valor_total = float.Parse((string) lector["montodelabono"]);
						titulo_comprobante = tipocomprobante;
						break;
					case "SERVICIO":
						titulo_comprobante = tipocomprobante;
						break;
					case "HONORARIO":
						toma_valor_total = float.Parse((string) lector["montodelabono"]);
						titulo_comprobante = tipocomprobante+"_MEDICO";
						fechahorapago = "|"+(string) lector["fechcreacomp"].ToString().Trim()+"|"+(string) lector["horacreacomp"].ToString().Trim()+"|"+numero_comprobante.ToString().Trim()+"|";
						break;
					}
					
					tipo_paciente = classpublic.lee_registro_de_tabla("osiris_erp_movcargos,osiris_his_tipo_pacientes","descripcion_tipo_paciente","WHERE osiris_erp_movcargos.folio_de_servicio = '"+lector["foliodeservicio"].ToString().Trim()+"' AND osiris_erp_movcargos.id_tipo_paciente = osiris_his_tipo_pacientes.id_tipo_paciente ","descripcion_tipo_paciente","string");
					
					imprime_encabezado(cr,layout,numero_comprobante.ToString().Trim(),lector["foliodeservicio"].ToString().Trim(),lector["pidpaciente"].ToString().Trim(),
					               		lector["nombre_completo"].ToString().Trim(),lector["descripcion_empresa"].ToString().Trim(),lector["telefono_particular1_paciente"].ToString().Trim(),
					               		lector["fechcreacion"].ToString().Trim()+" "+lector["horacreacion"].ToString().Trim(),lector["concepto_comprobante"].ToString().Trim(),lector["observacionesvarias"].ToString().Trim(),
					                   	toma_valor_total,lector["nombre_medico_encabezado"].ToString().Trim(),fechahorapago,titulo_comprobante,lector["descripcion_aseguradora"].ToString().Trim());
					if(tipocomprobante == "CAJA" || tipocomprobante == "SERVICIO" ){
						toma_tipoadmisiones = (int) lector["idadmisiones"];
						toma_grupoproducto = (int) lector["id_grupo_producto"];
						comienzo_linea += separacion_linea;
						comienzo_linea += separacion_linea;
						fontSize = 8.0;			layout = null;			layout = context.CreatePangoLayout ();
						desc.Size = (int)(fontSize * pangoScale);		layout.FontDescription = desc;
						layout.FontDescription.Weight = Weight.Normal;		// Letra normal
						subtotal += decimal.Parse((string) lector["ppcantidad"].ToString().Trim());
						if((bool) lector["aplicar_iva"] == true) {
							ivaprod = decimal.Parse((string) lector["ivaproducto"]);
							subtiva += ivaprod;
						}else{
							ivaprod = 0;
							subtnoiva += ivaprod;							
						}
						sumaiva += ivaprod;
						total = subtotal + ivaprod;
						if ((int) lector["idadmisiones"] == 300 || (int) lector["idadmisiones"] == 400 || (int) lector["idadmisiones"] == 920 || (int) lector["idadmisiones"] == 950 || (int) lector["idadmisiones"] == 960 || (int) lector["idadmisiones"] == 970 || (int) lector["idadmisiones"] == 980){
							if((bool) classpublic.muestradeta_comprcaja == true){
								
								//cr.MoveTo(05*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);		layout.SetText((string) lector["descripcion_admisiones"].ToString().Trim());	Pango.CairoHelper.ShowLayout (cr, layout);
								//comienzo_linea += separacion_linea;
								//cr.MoveTo(15*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);		layout.SetText((string) lector["descripcion_grupo_producto"].ToString().Trim());	Pango.CairoHelper.ShowLayout (cr, layout);
								//comienzo_linea += separacion_linea;
								cr.MoveTo(12*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);		layout.SetText("Cant.");	Pango.CairoHelper.ShowLayout (cr, layout);
								cr.MoveTo(40*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);		layout.SetText("Unidad");	Pango.CairoHelper.ShowLayout (cr, layout);
								cr.MoveTo(120*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);		layout.SetText("Descripcion");	Pango.CairoHelper.ShowLayout (cr, layout);
								cr.MoveTo(420*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);		layout.SetText("Sub-Total");	Pango.CairoHelper.ShowLayout (cr, layout);
								cr.MoveTo(490*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);		layout.SetText("Total");	Pango.CairoHelper.ShowLayout (cr, layout);
								comienzo_linea += separacion_linea;
								cr.MoveTo(565*escala_en_linux_windows,(comienzo_linea)*escala_en_linux_windows);
								cr.LineTo(05,(comienzo_linea)*escala_en_linux_windows);
								cr.FillExtents();  //. FillPreserve(); 
								cr.SetSourceRGB (0, 0, 0);
								cr.LineWidth = 0.1;
								cr.Stroke();
								
								cr.MoveTo(10*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);		layout.SetText((string) lector["cantidadaplicada"].ToString().Trim());	Pango.CairoHelper.ShowLayout (cr, layout);
								cr.MoveTo(80*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);		layout.SetText((string) lector["descripcion_producto"].ToString().Trim());	Pango.CairoHelper.ShowLayout (cr, layout);
								cr.MoveTo(420*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);		layout.SetText(float.Parse((string) lector["preciounitario"].ToString().Trim()).ToString("C"));	Pango.CairoHelper.ShowLayout (cr, layout);
								cr.MoveTo(480*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);		layout.SetText(float.Parse((string) lector["ppcantidad"].ToString().Trim()).ToString("C"));	Pango.CairoHelper.ShowLayout (cr, layout);
							}else{
								cr.MoveTo(05*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);		layout.SetText((string) lector["descripcion_admisiones"].ToString().Trim());	Pango.CairoHelper.ShowLayout (cr, layout);
								comienzo_linea += separacion_linea;
								cr.MoveTo(15*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);		layout.SetText((string) lector["descripcion_grupo_producto"].ToString().Trim());	Pango.CairoHelper.ShowLayout (cr, layout);
								comienzo_linea += separacion_linea;
								cr.MoveTo(35*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);		layout.SetText((string) lector["idproducto"].ToString().Trim()+" "+(string) lector["descripcion_producto"].ToString().Trim());	Pango.CairoHelper.ShowLayout (cr, layout);								
							}
						}else{							
							cr.MoveTo(05*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);		layout.SetText((string) lector["descripcion_admisiones"].ToString().Trim());	Pango.CairoHelper.ShowLayout (cr, layout);
							comienzo_linea += separacion_linea;
							cr.MoveTo(15*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);		layout.SetText((string) lector["descripcion_grupo_producto"].ToString().Trim());	Pango.CairoHelper.ShowLayout (cr, layout);
						}					
						while (lector.Read()){
							subtotal += decimal.Parse((string) lector["ppcantidad"].ToString().Trim());
							if((bool) lector["aplicar_iva"] == true) {
								ivaprod = decimal.Parse((string) lector["ivaproducto"]);
								subtiva += ivaprod;
							}else{
								ivaprod = 0;
								subtnoiva += ivaprod;							
							}
							sumaiva += ivaprod;
							total = subtotal + ivaprod;
							if (toma_tipoadmisiones != (int) lector["idadmisiones"]){
								comienzo_linea += separacion_linea;
								cr.MoveTo(05*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);		layout.SetText((string) lector["descripcion_admisiones"].ToString().Trim());	Pango.CairoHelper.ShowLayout (cr, layout);
								comienzo_linea += separacion_linea;
								if ((int) lector["idadmisiones"] == 300 || (int) lector["idadmisiones"] == 400 || (int) lector["idadmisiones"] == 920 || (int) lector["idadmisiones"] == 950 || (int) lector["idadmisiones"] == 960 || (int) lector["idadmisiones"] == 970 || (int) lector["idadmisiones"] == 980){
									cr.MoveTo(15*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);		layout.SetText((string) lector["descripcion_grupo_producto"].ToString().Trim());	Pango.CairoHelper.ShowLayout (cr, layout);
									if((bool) classpublic.muestradeta_comprcaja == true){
										comienzo_linea += separacion_linea;
										cr.MoveTo(10*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);		layout.SetText((string) lector["cantidadaplicada"].ToString().Trim());	Pango.CairoHelper.ShowLayout (cr, layout);
										cr.MoveTo(80*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);		layout.SetText((string) lector["descripcion_producto"].ToString().Trim());	Pango.CairoHelper.ShowLayout (cr, layout);
										cr.MoveTo(420*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);		layout.SetText(float.Parse((string) lector["preciounitario"].ToString().Trim()).ToString("C"));	Pango.CairoHelper.ShowLayout (cr, layout);
										cr.MoveTo(480*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);		layout.SetText(float.Parse((string) lector["ppcantidad"].ToString().Trim()).ToString("C"));	Pango.CairoHelper.ShowLayout (cr, layout);
									}else{
										comienzo_linea += separacion_linea;
										cr.MoveTo(35*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);		layout.SetText((string) lector["idproducto"].ToString().Trim()+" "+(string) lector["descripcion_producto"].ToString().Trim());	Pango.CairoHelper.ShowLayout (cr, layout);
									}									
								}
								toma_tipoadmisiones = (int) lector["idadmisiones"];
								toma_grupoproducto = (int) lector["id_grupo_producto"];							
							}else{
								if ((int) lector["idadmisiones"] == 300 || (int) lector["idadmisiones"] == 400 || (int) lector["idadmisiones"] == 920 || (int) lector["idadmisiones"] == 950 || (int) lector["idadmisiones"] == 960 || (int) lector["idadmisiones"] == 970 || (int) lector["idadmisiones"] == 980){
									if(toma_grupoproducto != (int) lector["id_grupo_producto"]){
										comienzo_linea += separacion_linea;
										cr.MoveTo(15*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);		layout.SetText((string) lector["descripcion_grupo_producto"].ToString().Trim());	Pango.CairoHelper.ShowLayout (cr, layout);
										toma_grupoproducto = (int) lector["id_grupo_producto"];
										if((bool) classpublic.muestradeta_comprcaja == true){
											comienzo_linea += separacion_linea;
											cr.MoveTo(10*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);		layout.SetText((string) lector["cantidadaplicada"].ToString().Trim());	Pango.CairoHelper.ShowLayout (cr, layout);
											cr.MoveTo(80*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);		layout.SetText((string) lector["descripcion_producto"].ToString().Trim());	Pango.CairoHelper.ShowLayout (cr, layout);
											cr.MoveTo(420*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);		layout.SetText(float.Parse((string) lector["preciounitario"].ToString().Trim()).ToString("C"));	Pango.CairoHelper.ShowLayout (cr, layout);
											cr.MoveTo(480*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);		layout.SetText(float.Parse((string) lector["ppcantidad"].ToString().Trim()).ToString("C"));	Pango.CairoHelper.ShowLayout (cr, layout);
										}else{
											comienzo_linea += separacion_linea;										
											cr.MoveTo(35*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);		layout.SetText((string) lector["idproducto"].ToString().Trim()+" "+(string) lector["descripcion_producto"].ToString().Trim());	Pango.CairoHelper.ShowLayout (cr, layout);
										}
									}else{
										if((bool) classpublic.muestradeta_comprcaja == true){
											comienzo_linea += separacion_linea;
											cr.MoveTo(10*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);		layout.SetText((string) lector["cantidadaplicada"].ToString().Trim());	Pango.CairoHelper.ShowLayout (cr, layout);
											cr.MoveTo(80*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);		layout.SetText((string) lector["descripcion_producto"].ToString().Trim());	Pango.CairoHelper.ShowLayout (cr, layout);
											cr.MoveTo(420*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);		layout.SetText(float.Parse((string) lector["preciounitario"].ToString().Trim()).ToString("C"));	Pango.CairoHelper.ShowLayout (cr, layout);
											cr.MoveTo(480*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);		layout.SetText(float.Parse((string) lector["ppcantidad"].ToString().Trim()).ToString("C"));	Pango.CairoHelper.ShowLayout (cr, layout);
										}else{
											comienzo_linea += separacion_linea;										
											cr.MoveTo(35*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);		layout.SetText((string) lector["idproducto"].ToString().Trim()+" "+(string) lector["descripcion_producto"].ToString().Trim());	Pango.CairoHelper.ShowLayout (cr, layout);
										}
									}
								}
							}
						}
					}
					if(tipocomprobante == "PAGARE"){
						comienzo_linea += separacion_linea;
						comienzo_linea += separacion_linea;
						fontSize = 8.0;			layout = null;			layout = context.CreatePangoLayout ();
						desc.Size = (int)(fontSize * pangoScale);		layout.FontDescription = desc;
						layout.FontDescription.Weight = Weight.Bold;
						cr.MoveTo(05*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);		layout.SetText("PAGARÉ");	Pango.CairoHelper.ShowLayout (cr, layout);
						cr.MoveTo(200*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);		layout.SetText("N° 1/1");	Pango.CairoHelper.ShowLayout (cr, layout);
						cr.MoveTo(400*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);		layout.SetText("BUENO POR "+toma_valor_total.ToString("C"));	Pango.CairoHelper.ShowLayout (cr, layout);
						layout.FontDescription.Weight = Weight.Normal;
						comienzo_linea += separacion_linea;
						comienzo_linea += separacion_linea;
						cr.MoveTo(270*escala_en_linux_windows, comienzo_linea*escala_en_linux_windows);				layout.SetText("Monterrey, Nuevo León a los "+DateTime.Now.ToString("dd")+" días del mes de "+classpublic.nom_mes(DateTime.Now.ToString("MM"))+" del año "+DateTime.Now.ToString("yyyy"));	Pango.CairoHelper.ShowLayout (cr, layout);
						comienzo_linea += separacion_linea;
						fontSize = 6.0;			layout = null;			layout = context.CreatePangoLayout ();
						desc.Size = (int)(fontSize * pangoScale);		layout.FontDescription = desc;
						layout.FontDescription.Weight = Weight.Normal;
						cr.MoveTo(370*escala_en_linux_windows, comienzo_linea*escala_en_linux_windows);				layout.SetText("Lugar y fecha de expedición");	Pango.CairoHelper.ShowLayout (cr, layout);
						comienzo_linea += separacion_linea;
						fontSize = 8.0;			layout = null;			layout = context.CreatePangoLayout ();
						desc.Size = (int)(fontSize * pangoScale);		layout.FontDescription = desc;
						layout.FontDescription.Weight = Weight.Normal;
						cr.MoveTo(05*escala_en_linux_windows, comienzo_linea*escala_en_linux_windows);				layout.SetText("Debo(mos) y pagaré(mos) indicionalmente por este Pagaré a la orden de :");	Pango.CairoHelper.ShowLayout (cr, layout);
						comienzo_linea += separacion_linea;
						cr.MoveTo(05*escala_en_linux_windows, comienzo_linea*escala_en_linux_windows);				layout.SetText(classpublic.nombre_empresa);	Pango.CairoHelper.ShowLayout (cr, layout);
						comienzo_linea += separacion_linea;
						cr.MoveTo(05*escala_en_linux_windows, comienzo_linea*escala_en_linux_windows);				layout.SetText("en Monterrey, Nuevo León el "+lector["vencimiento_pagare"].ToString());	Pango.CairoHelper.ShowLayout (cr, layout);
						comienzo_linea += separacion_linea;
						cr.MoveTo(05*escala_en_linux_windows, comienzo_linea*escala_en_linux_windows);				layout.SetText("La cantidad de:");	Pango.CairoHelper.ShowLayout (cr, layout);
						comienzo_linea += separacion_linea;
						cr.MoveTo(05*escala_en_linux_windows, comienzo_linea*escala_en_linux_windows);		layout.SetText((string) class_public.ConvertirCadena(toma_valor_total.ToString(),"PESOS").ToUpper());	Pango.CairoHelper.ShowLayout (cr, layout);							
						comienzo_linea += separacion_linea;
						comienzo_linea += separacion_linea;
						cr.MoveTo(05*escala_en_linux_windows, comienzo_linea*escala_en_linux_windows);				layout.SetText("Valor Recibido a mi (nuestra) entera satisfacción. Este pagaré forma parte de una serie numerada del 1 al 1 y todos están sujetos a  la");	Pango.CairoHelper.ShowLayout (cr, layout);
						comienzo_linea += separacion_linea;
						cr.MoveTo(05*escala_en_linux_windows, comienzo_linea*escala_en_linux_windows);				layout.SetText("condición de que, al no pagarse cualquiera de ellos a su vencimiento, serán exigible todos los que le sigan en número, ademas de  los");	Pango.CairoHelper.ShowLayout (cr, layout);
						comienzo_linea += separacion_linea;
						cr.MoveTo(05*escala_en_linux_windows, comienzo_linea*escala_en_linux_windows);				layout.SetText("ya vencido, desde la fecha de vencimiento a este documento hasta el día de su  liquidacion, causara intereses  moratorios  al tipo  del");	Pango.CairoHelper.ShowLayout (cr, layout);
						comienzo_linea += separacion_linea;
						cr.MoveTo(05*escala_en_linux_windows, comienzo_linea*escala_en_linux_windows);				layout.SetText("_____% mensual, pagadero en esta ciudad juntamente con el principal.");	Pango.CairoHelper.ShowLayout (cr, layout);
						comienzo_linea += separacion_linea;
						comienzo_linea += separacion_linea;
						layout.FontDescription.Weight = Weight.Bold;
						cr.MoveTo(05*escala_en_linux_windows, comienzo_linea*escala_en_linux_windows);				layout.SetText("Datos del Deudor");	Pango.CairoHelper.ShowLayout (cr, layout);
						layout.FontDescription.Weight = Weight.Normal;
						comienzo_linea += separacion_linea;
						cr.MoveTo(05*escala_en_linux_windows, comienzo_linea*escala_en_linux_windows);				layout.SetText("Nombre: "+lector["nombre_completo"].ToString());	Pango.CairoHelper.ShowLayout (cr, layout);
						cr.MoveTo(400*escala_en_linux_windows, comienzo_linea*escala_en_linux_windows);				layout.SetText("Acepto(amos)");	Pango.CairoHelper.ShowLayout (cr, layout);
						comienzo_linea += separacion_linea;
						cr.MoveTo(05*escala_en_linux_windows, comienzo_linea*escala_en_linux_windows);				layout.SetText("Dirección: "+lector["direccion_paciente"].ToString().Trim()+" "+lector["numero_casa_paciente"].ToString().Trim()+lector["numero_departamento_paciente"].ToString().Trim()+" "+lector["colonia_paciente"].ToString().Trim()+" CP. "+lector["codigo_postal_paciente"].ToString().Trim());	Pango.CairoHelper.ShowLayout (cr, layout);
						comienzo_linea += separacion_linea;
						cr.MoveTo(05*escala_en_linux_windows, comienzo_linea*escala_en_linux_windows);				layout.SetText("Población: "+lector["municipio_paciente"].ToString().Trim()+", "+lector["estado_paciente"].ToString().Trim());	Pango.CairoHelper.ShowLayout (cr, layout);
						cr.MoveTo(400*escala_en_linux_windows, comienzo_linea*escala_en_linux_windows);				layout.SetText("Firma Deudor(s) ________________");	Pango.CairoHelper.ShowLayout (cr, layout);
						comienzo_linea += separacion_linea;
						comienzo_linea += separacion_linea;
						layout.FontDescription.Weight = Weight.Bold;
						cr.MoveTo(05*escala_en_linux_windows, comienzo_linea*escala_en_linux_windows);				layout.SetText("Datos y firma(s) del(os) Aval(es)");	Pango.CairoHelper.ShowLayout (cr, layout);
						layout.FontDescription.Weight = Weight.Normal;
						comienzo_linea += separacion_linea;
						cr.MoveTo(05*escala_en_linux_windows, comienzo_linea+5*escala_en_linux_windows);				layout.SetText("Nombre:_____________________________________________________ ");	Pango.CairoHelper.ShowLayout (cr, layout);
						comienzo_linea += separacion_linea;
						cr.MoveTo(05*escala_en_linux_windows, comienzo_linea+5*escala_en_linux_windows);				layout.SetText("Dirección:___________________________________________________");	Pango.CairoHelper.ShowLayout (cr, layout);
						comienzo_linea += separacion_linea;
						cr.MoveTo(05*escala_en_linux_windows, comienzo_linea+5*escala_en_linux_windows);				layout.SetText("Población:_____________________________________________ Telefono :______________");	Pango.CairoHelper.ShowLayout (cr, layout);
						cr.MoveTo(400*escala_en_linux_windows, comienzo_linea+5*escala_en_linux_windows);				layout.SetText("Firma(s) Aval __________________");	Pango.CairoHelper.ShowLayout (cr, layout);						
					}
					if (tipocomprobante == "CAJA"){
						fontSize = 7.0;			layout = null;			layout = context.CreatePangoLayout ();
						desc.Size = (int)(fontSize * pangoScale);		layout.FontDescription = desc;
						layout.FontDescription.Weight = Weight.Normal;
						if(toma_tipocomprobante != 6){
							// numeros en letras
							if(toma_valor_total < 0){
								toma_valor_total = toma_valor_total * -1;
							}
							cr.MoveTo(05*escala_en_linux_windows,comienzo_linea2+(separacion_linea*26)*escala_en_linux_windows);		layout.SetText((string) class_public.ConvertirCadena(toma_valor_total.ToString(),"PESOS").ToUpper());	Pango.CairoHelper.ShowLayout (cr, layout);
							fontSize = 8.0;		
							desc.Size = (int)(fontSize * pangoScale);					layout.FontDescription = desc;
							layout.FontDescription.Weight = Weight.Bold;		// Letra negrita
							if(toma_valor_total < 0){
								toma_valor_total = toma_valor_total * -1;
							}
							if((bool) classpublic.muestradeta_comprcaja == true){
								cr.MoveTo(400*escala_en_linux_windows,comienzo_linea2+(separacion_linea*24)*escala_en_linux_windows);		layout.SetText("SUB-TOTAL");					Pango.CairoHelper.ShowLayout (cr, layout);
								cr.MoveTo(480*escala_en_linux_windows,comienzo_linea2+(separacion_linea*24)*escala_en_linux_windows);		layout.SetText(subtotal.ToString("C"));	Pango.CairoHelper.ShowLayout (cr, layout);	
								cr.MoveTo(400*escala_en_linux_windows,comienzo_linea2+(separacion_linea*25)*escala_en_linux_windows);		layout.SetText(valoriva.ToString().Trim()+ "% IVA");					Pango.CairoHelper.ShowLayout (cr, layout);
								cr.MoveTo(480*escala_en_linux_windows,comienzo_linea2+(separacion_linea*25)*escala_en_linux_windows);		layout.SetText(sumaiva.ToString("C"));	Pango.CairoHelper.ShowLayout (cr, layout);	
								cr.MoveTo(400*escala_en_linux_windows,comienzo_linea2+(separacion_linea*26)*escala_en_linux_windows);		layout.SetText("T O T A L");					Pango.CairoHelper.ShowLayout (cr, layout);
								cr.MoveTo(480*escala_en_linux_windows,comienzo_linea2+(separacion_linea*26)*escala_en_linux_windows);		layout.SetText(total.ToString("C"));	Pango.CairoHelper.ShowLayout (cr, layout);
							}else{
								cr.MoveTo(400*escala_en_linux_windows,comienzo_linea2+(separacion_linea*26)*escala_en_linux_windows);		layout.SetText("T O T A L");					Pango.CairoHelper.ShowLayout (cr, layout);
								cr.MoveTo(480*escala_en_linux_windows,comienzo_linea2+(separacion_linea*26)*escala_en_linux_windows);		layout.SetText(toma_valor_total.ToString("C"));	Pango.CairoHelper.ShowLayout (cr, layout);
							}
						}else{
							cr.MoveTo(05*escala_en_linux_windows,comienzo_linea2+(separacion_linea*26)*escala_en_linux_windows);		layout.SetText("____ CONSULTA     ____ CONSULTA URG.   ____ PROCEDIMIENTO");	Pango.CairoHelper.ShowLayout (cr, layout);
						}
					}
					
					if (tipocomprobante == "ABONO"){
						fontSize = 7.0;			layout = null;			layout = context.CreatePangoLayout ();
						desc.Size = (int)(fontSize * pangoScale);		layout.FontDescription = desc;
						layout.FontDescription.Weight = Weight.Normal;
						if(toma_tipocomprobante != 6){
							// numeros en letras
							if(toma_valor_total < 0){
								toma_valor_total = toma_valor_total * -1;
							}
							cr.MoveTo(05*escala_en_linux_windows,comienzo_linea2+(separacion_linea*26)*escala_en_linux_windows);		layout.SetText((string) class_public.ConvertirCadena(toma_valor_total.ToString(),"PESOS").ToUpper());	Pango.CairoHelper.ShowLayout (cr, layout);
							fontSize = 8.0;		
							desc.Size = (int)(fontSize * pangoScale);					layout.FontDescription = desc;
							layout.FontDescription.Weight = Weight.Bold;		// Letra negrita
							if(toma_valor_total < 0){
								toma_valor_total = toma_valor_total * -1;
							}
							if((bool) classpublic.muestradeta_comprcaja == true){
								cr.MoveTo(400*escala_en_linux_windows,comienzo_linea2+(separacion_linea*24)*escala_en_linux_windows);		layout.SetText("SUB-TOTAL");					Pango.CairoHelper.ShowLayout (cr, layout);
								cr.MoveTo(480*escala_en_linux_windows,comienzo_linea2+(separacion_linea*24)*escala_en_linux_windows);		layout.SetText(subtotal.ToString("C"));	Pango.CairoHelper.ShowLayout (cr, layout);	
								cr.MoveTo(400*escala_en_linux_windows,comienzo_linea2+(separacion_linea*25)*escala_en_linux_windows);		layout.SetText(valoriva.ToString().Trim()+ "% IVA");					Pango.CairoHelper.ShowLayout (cr, layout);
								cr.MoveTo(480*escala_en_linux_windows,comienzo_linea2+(separacion_linea*25)*escala_en_linux_windows);		layout.SetText(sumaiva.ToString("C"));	Pango.CairoHelper.ShowLayout (cr, layout);	
								cr.MoveTo(400*escala_en_linux_windows,comienzo_linea2+(separacion_linea*26)*escala_en_linux_windows);		layout.SetText("T O T A L");					Pango.CairoHelper.ShowLayout (cr, layout);
								cr.MoveTo(480*escala_en_linux_windows,comienzo_linea2+(separacion_linea*26)*escala_en_linux_windows);		layout.SetText(total.ToString("C"));	Pango.CairoHelper.ShowLayout (cr, layout);
							}else{
								cr.MoveTo(400*escala_en_linux_windows,comienzo_linea2+(separacion_linea*26)*escala_en_linux_windows);		layout.SetText("T O T A L");					Pango.CairoHelper.ShowLayout (cr, layout);
								cr.MoveTo(480*escala_en_linux_windows,comienzo_linea2+(separacion_linea*26)*escala_en_linux_windows);		layout.SetText(toma_valor_total.ToString("C"));	Pango.CairoHelper.ShowLayout (cr, layout);
							}
						}else{
							cr.MoveTo(05*escala_en_linux_windows,comienzo_linea2+(separacion_linea*26)*escala_en_linux_windows);		layout.SetText("____ CONSULTA     ____ CONSULTA URG.   ____ PROCEDIMIENTO");	Pango.CairoHelper.ShowLayout (cr, layout);
						}
					}
				}								
			}catch (NpgsqlException ex){
				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
					MessageType.Warning, ButtonsType.Ok, "PostgresSQL error: {0}",ex.Message);
					msgBoxError.Run ();					msgBoxError.Destroy();
				Console.WriteLine ("PostgresSQL error: {0}",ex.Message);
				return; 
			}
			conexion.Close();
		}
		
		void imprime_encabezado(Cairo.Context cr,Pango.Layout layout,string numerocomprobante, string numerodeatencion, string numeroexpediente, 
		                        string nombrepaciente, string descripcion_empmuni, string telefono_paciente, string fechahoraregistro,
		                        string conceptocomprobante, string observacionescomprobante,float tomavalortotal,string doctor_admision,string fechahorapago,
		                        string titulo_comprobante_, string descripcion_asegu)
		{
			comienzo_linea = 60;
			Pango.FontDescription desc = Pango.FontDescription.FromString ("Sans");								
			//cr.Rotate(90);  //Imprimir Orizontalmente rota la hoja cambian las posiciones de las lineas y columna					
			fontSize = 8.0;
			desc.Size = (int)(fontSize * pangoScale);					layout.FontDescription = desc;
			layout.FontDescription.Weight = Weight.Bold;		// Letra negrita
			cr.MoveTo(05*escala_en_linux_windows,05*escala_en_linux_windows);			layout.SetText(classpublic.nombre_empresa);			Pango.CairoHelper.ShowLayout (cr, layout);
			//cr.MoveTo(05*escala_en_linux_windows,05*escala_en_linux_windows);			layout.SetText(desctipocomprobate);			Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(05*escala_en_linux_windows,15*escala_en_linux_windows);			layout.SetText(classpublic.direccion_empresa);		Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(05*escala_en_linux_windows,25*escala_en_linux_windows);			layout.SetText(classpublic.telefonofax_empresa);	Pango.CairoHelper.ShowLayout (cr, layout);
			fontSize = 6.0;
			desc.Size = (int)(fontSize * pangoScale);					layout.FontDescription = desc;
			cr.MoveTo(479*escala_en_linux_windows,05*escala_en_linux_windows);			layout.SetText("Fech.Rpt:"+(string) DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));		Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(479*escala_en_linux_windows,15*escala_en_linux_windows);			layout.SetText("N° Page :"+numpage.ToString().Trim());		Pango.CairoHelper.ShowLayout (cr, layout);

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
			if (tipocomprobante == "PAGARE"){
				cr.MoveTo(width/2,45*escala_en_linux_windows);	layout.SetText(titulo_comprobante_);	Pango.CairoHelper.ShowLayout (cr, layout);
			}else{
				cr.MoveTo(width/2,45*escala_en_linux_windows);	layout.SetText("COMPROBANTE_"+titulo_comprobante_);	Pango.CairoHelper.ShowLayout (cr, layout);
			}
			fontSize = 9.0;			layout = null;			layout = context.CreatePangoLayout ();
			desc.Size = (int)(fontSize * pangoScale);		layout.FontDescription = desc;
			layout.FontDescription.Weight = Weight.Bold;		// Letra negrita
			cr.MoveTo(479*escala_en_linux_windows, 25*escala_en_linux_windows);			layout.SetText("N° Folio "+numerocomprobante);				Pango.CairoHelper.ShowLayout (cr, layout);
			
			fontSize = 8.0;			layout = null;			layout = context.CreatePangoLayout ();
			desc.Size = (int)(fontSize * pangoScale);		layout.FontDescription = desc;
			layout.FontDescription.Weight = Weight.Bold;		// Letra negrita
			cr.MoveTo(05*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);		layout.SetText("N° Atencion: "+numerodeatencion);	Pango.CairoHelper.ShowLayout (cr, layout);
			layout.FontDescription.Weight = Weight.Normal;		// Letra normal
			cr.MoveTo(120*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);		layout.SetText("N° Expe.: "+numeroexpediente);		Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(220*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);		layout.SetText("Nombre Paciente: "+nombrepaciente);	Pango.CairoHelper.ShowLayout (cr, layout);
			comienzo_linea += separacion_linea;
			cr.MoveTo(05*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);		layout.SetText("Telefono: "+telefono_paciente);	Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(180*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);		layout.SetText("Fecha Admision: "+fechahoraregistro);	Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(340*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);		layout.SetText("F/H Comprobante: "+fechahorapago);	Pango.CairoHelper.ShowLayout (cr, layout);
			comienzo_linea += separacion_linea;
			cr.MoveTo(05*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);		layout.SetText("Tipo Paciente: "+tipo_paciente);	Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(200*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);		layout.SetText("Convenio :"+descripcion_empmuni+" "+descripcion_asegu);	Pango.CairoHelper.ShowLayout (cr, layout);		
			comienzo_linea += separacion_linea;
			cr.MoveTo(05*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);		layout.SetText("Doctor: "+doctor_admision);	Pango.CairoHelper.ShowLayout (cr, layout);		
			
			if (tipocomprobante == "HONORARIO"){				
				fontSize = 7.0;		
				desc.Size = (int)(fontSize * pangoScale);					layout.FontDescription = desc;
				layout.FontDescription.Weight = Weight.Normal;
				// numeros en letras
				if(tomavalortotal < 0){
					tomavalortotal = tomavalortotal * -1;
				}
				cr.MoveTo(05*escala_en_linux_windows,comienzo_linea2+(separacion_linea*26)*escala_en_linux_windows);		layout.SetText((string) class_public.ConvertirCadena(tomavalortotal.ToString(),"PESOS").ToUpper());	Pango.CairoHelper.ShowLayout (cr, layout);		
				fontSize = 8.0;		
				desc.Size = (int)(fontSize * pangoScale);					layout.FontDescription = desc;
				layout.FontDescription.Weight = Weight.Bold;		// Letra negrita
				if(tomavalortotal < 0){
					tomavalortotal = tomavalortotal * -1;
				}
				//if((bool) classpublic.muestradeta_comprcaja == true){
				//	cr.MoveTo(400*escala_en_linux_windows,comienzo_linea+(separacion_linea*20)*escala_en_linux_windows);		layout.SetText("SUB-TOTAL");						Pango.CairoHelper.ShowLayout (cr, layout);
				//	cr.MoveTo(480*escala_en_linux_windows,comienzo_linea+(separacion_linea*20)*escala_en_linux_windows);		layout.SetText(":"+tomavalortotal.ToString("C"));	Pango.CairoHelper.ShowLayout (cr, layout);	
				//	cr.MoveTo(400*escala_en_linux_windows,comienzo_linea+(separacion_linea*21)*escala_en_linux_windows);		layout.SetText(valoriva.ToString().Trim()+ "% IVA");	Pango.CairoHelper.ShowLayout (cr, layout);
				//	cr.MoveTo(480*escala_en_linux_windows,comienzo_linea+(separacion_linea*21)*escala_en_linux_windows);		layout.SetText(":"+tomavalortotal.ToString("C"));	Pango.CairoHelper.ShowLayout (cr, layout);	
				//}
				cr.MoveTo(400*escala_en_linux_windows,comienzo_linea2+(separacion_linea*26)*escala_en_linux_windows);		layout.SetText("T O T A L");	Pango.CairoHelper.ShowLayout (cr, layout);
				cr.MoveTo(480*escala_en_linux_windows,comienzo_linea2+(separacion_linea*26)*escala_en_linux_windows);		layout.SetText(tomavalortotal.ToString("C"));	Pango.CairoHelper.ShowLayout (cr, layout);
			}

			/*
			if (tipocomprobante == "PAGARE"){				
				fontSize = 7.0;		
				desc.Size = (int)(fontSize * pangoScale);					layout.FontDescription = desc;
				layout.FontDescription.Weight = Weight.Normal;
				// numeros en letras
				cr.MoveTo(05*escala_en_linux_windows,comienzo_linea+(separacion_linea*22)*escala_en_linux_windows);		layout.SetText((string) class_public.ConvertirCadena(tomavalortotal.ToString(),"PESOS").ToUpper());	Pango.CairoHelper.ShowLayout (cr, layout);		
				fontSize = 8.0;		
				desc.Size = (int)(fontSize * pangoScale);					layout.FontDescription = desc;
				layout.FontDescription.Weight = Weight.Bold;		// Letra negrita
				cr.MoveTo(300*escala_en_linux_windows,comienzo_linea+(separacion_linea*22)*escala_en_linux_windows);		layout.SetText("VALOR TOTAL DEL PAGARE: "+tomavalortotal.ToString("C"));	Pango.CairoHelper.ShowLayout (cr, layout);		
			}*/
			
			if (tipocomprobante != "PAGARE"){
				fontSize = 8.0;		
				desc.Size = (int)(fontSize * pangoScale);					layout.FontDescription = desc;
				layout.FontDescription.Weight = Weight.Normal;		// Letra normal
				
				cr.MoveTo(05*escala_en_linux_windows,comienzo_linea2+(separacion_linea*28)*escala_en_linux_windows);		layout.SetText("Concepto    : "+conceptocomprobante);	Pango.CairoHelper.ShowLayout (cr, layout);		
				cr.MoveTo(05*escala_en_linux_windows,comienzo_linea2+(separacion_linea*29)*escala_en_linux_windows);		layout.SetText("Observacion : "+observacionescomprobante);	Pango.CairoHelper.ShowLayout (cr, layout);		
				cr.MoveTo(05*escala_en_linux_windows,comienzo_linea2+(separacion_linea*30)*escala_en_linux_windows);		layout.SetText("Impreso por : "+nombreempleado);	Pango.CairoHelper.ShowLayout (cr, layout);		
	
				fontSize = 8.0;			layout = null;			layout = context.CreatePangoLayout ();
				desc.Size = (int)(fontSize * pangoScale);		layout.FontDescription = desc;
				layout.FontDescription.Weight = Weight.Normal;		// Letra normal
			}
		}
				
		private void OnEndPrint (object obj, Gtk.EndPrintArgs args)
		{
			
		}		
		
		void rpt_pago_abono(string tipocomprobante)
		{
			//if (tipocomprobante == "CAJA"){
			//if (tipocomprobante == "ABONO"){
			float toma_valor_total = 0;
			int toma_tipocomprobante = 0;
			string fechahorapago = "";
			decimal subtotal = 0;
			decimal sumaiva = 0;
			decimal ivaprod = 0;
			decimal subtiva = 0;
			decimal subtnoiva = 0;
			string convenio_empr_aseg = "";
			string tipopaciente = "";
			int cuenta_lineas = 1;
							
			NpgsqlConnection conexion; 
	        conexion = new NpgsqlConnection (connectionString+nombrebd);
	        // Verifica que la base de datos este conectada
	        try{
	 			conexion.Open ();
	        	NpgsqlCommand comando; 
	        	comando = conexion.CreateCommand (); 
	           	comando.CommandText = sql_compcaja+sql_numerocomprobante+sql_foliodeservicio+sql_orderquery;
	        	//Console.WriteLine(comando.CommandText);
				NpgsqlDataReader lector = comando.ExecuteReader();
				if (lector.Read()){
					string movdocumentos = llenado_movi_documentos(lector["foliodeservicio"].ToString().Trim());
					tipopaciente = classpublic.lee_registro_de_tabla("osiris_erp_movcargos,osiris_his_tipo_pacientes","descripcion_tipo_paciente","WHERE osiris_erp_movcargos.folio_de_servicio = '"+lector["foliodeservicio"].ToString().Trim()+"' AND osiris_erp_movcargos.id_tipo_paciente = osiris_his_tipo_pacientes.id_tipo_paciente ","descripcion_tipo_paciente","string");
					switch (tipocomprobante){	
					case "CAJA":
						titulo_comprobante = tipocomprobante+"_"+ (string) lector["descripcion_tipo_comprobante"];						
						break;
					case "ABONO":
						titulo_comprobante = "CAJA_"+(string) lector["descripcion_tipo_comprobante"];						
						break;
					}
					
					toma_valor_total = float.Parse((string) lector["montodelabono"]);
					toma_tipocomprobante = int.Parse(lector["id_tipo_comprobante"].ToString().Trim());
					desctipocomprobate = (string) lector["descripcion_tipo_comprobante"];
					fechahorapago = (string) lector["fechcreacomp"].ToString().Trim()+"&"+(string) lector["horacreacomp"].ToString().Trim()+"&"+numero_comprobante.ToString().Trim()+"&"+tipocomprobante;
					
					iTextSharp.text.Font _NormalFont;
					iTextSharp.text.Font _BoldFont;
					_NormalFont = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 7, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);
					_BoldFont = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 7, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
			
					string pdf_name = System.IO.Path.GetTempPath() + Guid.NewGuid().ToString() + ".pdf";
					
					// step 1: creation of a document-object
					Document documento = new Document(iTextSharp.text.PageSize.LETTER, 15, 15, 15, 15);
					//Document document = new Document(PageSize.LETTER.Rotate());
					try{
						PdfWriter writerpdf = PdfWriter.GetInstance(documento, new FileStream(pdf_name, FileMode.Create));
						
						documento.AddTitle("Comprobante de Pago en Caja");
				       	documento.AddCreator("Sistema Hospitalario OSIRIS");
				       	documento.AddAuthor("Sistema Hospitalario OSIRIS");
				       	documento.AddSubject("OSIRSrpt");	
						EventoTitulos_abonopago ev = new EventoTitulos_abonopago();
						ev.titulo1_rpt = "COMPROBANTE_"+titulo_comprobante;
						ev.numerorecibo_caja = numero_comprobante.ToString().Trim();
						ev.numero_atencion_px = lector["foliodeservicio"].ToString().Trim();
						ev.nro_expediente_px = lector["pidpaciente"].ToString().Trim();
					    ev.nombres_apellidos_px = lector["nombre_completo"].ToString().Trim();
						ev.fecha_ingresso_px = lector["fechcreacion"].ToString().Trim()+" "+lector["horacreacion"].ToString().Trim();
						  
						ev.codigobarra_registro = "";
						
						ev.fechahora_registropago = fechahorapago;
						ev.tipo_paciente_px = tipopaciente;
						if(tipopaciente == "ASEGURADO"){				
							convenio_empr_aseg = lector["descripcion_aseguradora"].ToString().Trim();
						}else{
							convenio_empr_aseg = lector["descripcion_empresa"].ToString().Trim();
						}
						ev.convenio_px = convenio_empr_aseg;
						ev.medico_guardia_px = lector["nombre_medico_encabezado"].ToString().Trim();
						ev.medico_tratante_px = lector["nombre_medico_tratante"].ToString().Trim();
						ev.concepto_depago = lector["concepto_comprobante"].ToString().Trim();
						ev.doc_convenio_px = movdocumentos;
						ev.observacion_depago = lector["observacionesvarias"].ToString().Trim()+"/"+lector["observaciones2"].ToString().Trim()+"/"+lector["observaciones3"].ToString().Trim();
						ev.impreso_por = nombreempleado;
						ev.creado_por = (string) classpublic.extract_spaces(lector["quien_creo_comprobante"].ToString().Trim());
						ev.tipocomprobante_cj = toma_tipocomprobante;
						
						writerpdf.PageEvent = ev;
						documento.Open();
						
						PdfPCell cellcol1;
						PdfPCell cellcol3;
						PdfPCell cellcol2;
						PdfPCell cellcol4;
						PdfPCell cellcol5;
						PdfPCell cellcol6;
						PdfPCell cellcol7;
						
						PdfPTable tblConceptos = new PdfPTable(7);
			            tblConceptos.WidthPercentage = 100;
						float[] widthsconceptos = new float[] { 15f, 35f, 140f, 30f, 30f, 30f, 30f };	// controlando el ancho de cada columna tienen que sumas 315 en total
						tblConceptos.SetWidths(widthsconceptos);
						tblConceptos.HorizontalAlignment = 0;
						
						// Configuramos el título de las columnas de la tabla
						cellcol1 = new PdfPCell(new Phrase("CANT.", _BoldFont));
						//clnroatencion.BorderWidth = 1;			// Ancho del borde
						cellcol1.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
						cellcol1.HorizontalAlignment = 1;		// centro
						cellcol1.BackgroundColor = BaseColor.YELLOW;
						cellcol2 = new PdfPCell(new Phrase("CODIGO", _BoldFont));
						cellcol2.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
						cellcol2.HorizontalAlignment = 1;		// centro
						cellcol2.BackgroundColor = BaseColor.YELLOW;
						cellcol3 = new PdfPCell(new Phrase("DESCRIPCION PRODUCTO", _BoldFont));
						cellcol3.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
						cellcol3.HorizontalAlignment = 0;		// centro
						cellcol3.BackgroundColor = BaseColor.YELLOW;
						cellcol4 = new PdfPCell(new Phrase("PRECIO", _BoldFont));
						cellcol4.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
						cellcol4.HorizontalAlignment = 1;		// centro
						cellcol4.BackgroundColor = BaseColor.YELLOW;
						cellcol5 = new PdfPCell(new Phrase("SUB-TOTAL", _BoldFont));
						cellcol5.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
						cellcol5.HorizontalAlignment = 1;		// centro
						cellcol5.BackgroundColor = BaseColor.YELLOW;
						cellcol6 = new PdfPCell(new Phrase("IVA", _BoldFont));
						cellcol6.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
						cellcol6.HorizontalAlignment = 1;		// centro
						cellcol6.BackgroundColor = BaseColor.YELLOW;
						cellcol7 = new PdfPCell(new Phrase("TOTAL", _BoldFont));
						cellcol7.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
						cellcol7.HorizontalAlignment = 1;		// centro
						cellcol7.BackgroundColor = BaseColor.YELLOW;
						// Añadimos las celdas a la tabla
						tblConceptos.AddCell(cellcol1);
						tblConceptos.AddCell(cellcol2);
						tblConceptos.AddCell(cellcol3);
						tblConceptos.AddCell(cellcol4);
						tblConceptos.AddCell(cellcol5);
						tblConceptos.AddCell(cellcol6);
						tblConceptos.AddCell(cellcol7);
						
						if((bool) classpublic.muestradeta_comprcaja == true){
							if(tipocomprobante == "CAJA"){
								
							}							
						}else{
							if(tipocomprobante == "CAJA"){
								subtotal += decimal.Parse((string) lector["ppcantidad"].ToString().Trim());
								if((bool) lector["aplicar_iva"] == true) {
									ivaprod = decimal.Parse((string) lector["ivaproducto"]);
									subtiva += ivaprod;
								}else{
									ivaprod = 0;
									subtnoiva += ivaprod;							
								}
								sumaiva += ivaprod;
																
								// Configuramos el título de las columnas de la tabla
								cellcol1 = new PdfPCell(new Phrase(lector["cantidadaplicada"].ToString().Trim(),_NormalFont));
								//clnroatencion.BorderWidth = 1;			// Ancho del borde
								cellcol1.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
								cellcol1.HorizontalAlignment = 1;		// centro
								cellcol2 = new PdfPCell(new Phrase(lector["idproducto"].ToString().Trim(),_NormalFont));
								cellcol2.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
								cellcol2.HorizontalAlignment = 1;		// centro
								cellcol3 = new PdfPCell(new Phrase(lector["descripcion_producto"].ToString().Trim(), _NormalFont));
								cellcol3.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
								cellcol3.HorizontalAlignment = 0;		// centro
								cellcol4 = new PdfPCell(new Phrase("", _NormalFont));
								cellcol4.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
								cellcol4.HorizontalAlignment = 1;		// centro
								cellcol5 = new PdfPCell(new Phrase("", _NormalFont));
								cellcol5.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
								cellcol5.HorizontalAlignment = 1;		// centro
								cellcol6 = new PdfPCell(new Phrase("", _NormalFont));
								cellcol6.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
								cellcol6.HorizontalAlignment = 1;		// centro
								cellcol7 = new PdfPCell(new Phrase("", _NormalFont));
								cellcol7.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
								cellcol7.HorizontalAlignment = 1;		// centro
								
								// Añadimos las celdas a la tabla
								tblConceptos.AddCell(cellcol1);
								tblConceptos.AddCell(cellcol2);
								tblConceptos.AddCell(cellcol3);
								tblConceptos.AddCell(cellcol4);
								tblConceptos.AddCell(cellcol5);
								tblConceptos.AddCell(cellcol6);
								tblConceptos.AddCell(cellcol7);
							}
						}												
						while (lector.Read()){
							cuenta_lineas += 1;
							if((bool) classpublic.muestradeta_comprcaja == true){
								
							}else{
								// Configuramos el título de las columnas de la tabla
								cellcol1 = new PdfPCell(new Phrase(lector["cantidadaplicada"].ToString().Trim(),_NormalFont));
								//clnroatencion.BorderWidth = 1;			// Ancho del borde
								cellcol1.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
								cellcol1.HorizontalAlignment = 1;		// centro
								cellcol2 = new PdfPCell(new Phrase(lector["idproducto"].ToString().Trim(),_NormalFont));
								cellcol2.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
								cellcol2.HorizontalAlignment = 1;		// centro
								cellcol3 = new PdfPCell(new Phrase(lector["descripcion_producto"].ToString().Trim(), _NormalFont));
								cellcol3.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
								cellcol3.HorizontalAlignment = 0;		// centro
								cellcol4 = new PdfPCell(new Phrase("", _NormalFont));
								cellcol4.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
								cellcol4.HorizontalAlignment = 1;		// centro
								cellcol5 = new PdfPCell(new Phrase("", _NormalFont));
								cellcol5.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
								cellcol5.HorizontalAlignment = 1;		// centro
								cellcol6 = new PdfPCell(new Phrase("", _NormalFont));
								cellcol6.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
								cellcol6.HorizontalAlignment = 1;		// centro
								cellcol7 = new PdfPCell(new Phrase("", _NormalFont));
								cellcol7.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
								cellcol7.HorizontalAlignment = 1;		// centro
								
								// Añadimos las celdas a la tabla
								tblConceptos.AddCell(cellcol1);
								tblConceptos.AddCell(cellcol2);
								tblConceptos.AddCell(cellcol3);
								tblConceptos.AddCell(cellcol4);
								tblConceptos.AddCell(cellcol5);
								tblConceptos.AddCell(cellcol6);
								tblConceptos.AddCell(cellcol7);
							}
							if(cuenta_lineas >= 11){
								documento.Add(tblConceptos);
								documento.NewPage();								
								
								tblConceptos = new PdfPTable(7);
			            		tblConceptos.WidthPercentage = 100;
								widthsconceptos = new float[] { 15f, 35f, 140f, 30f, 30f, 30f, 30f };	// controlando el ancho de cada columna tienen que sumas 315 en total
								tblConceptos.SetWidths(widthsconceptos);
								tblConceptos.HorizontalAlignment = 0;								
								// Configuramos el título de las columnas de la tabla
								cellcol1 = new PdfPCell(new Phrase("CANT.", _BoldFont));
								//clnroatencion.BorderWidth = 1;			// Ancho del borde
								cellcol1.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
								cellcol1.HorizontalAlignment = 1;		// centro
								cellcol1.BackgroundColor = BaseColor.YELLOW;
								cellcol2 = new PdfPCell(new Phrase("CODIGO", _BoldFont));
								cellcol2.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
								cellcol2.HorizontalAlignment = 1;		// centro
								cellcol2.BackgroundColor = BaseColor.YELLOW;
								cellcol3 = new PdfPCell(new Phrase("DESCRIPCION PRODUCTO", _BoldFont));
								cellcol3.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
								cellcol3.HorizontalAlignment = 0;		// centro
								cellcol3.BackgroundColor = BaseColor.YELLOW;
								cellcol4 = new PdfPCell(new Phrase("PRECIO", _BoldFont));
								cellcol4.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
								cellcol4.HorizontalAlignment = 1;		// centro
								cellcol4.BackgroundColor = BaseColor.YELLOW;
								cellcol5 = new PdfPCell(new Phrase("SUB-TOTAL", _BoldFont));
								cellcol5.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
								cellcol5.HorizontalAlignment = 1;		// centro
								cellcol5.BackgroundColor = BaseColor.YELLOW;
								cellcol6 = new PdfPCell(new Phrase("IVA", _BoldFont));
								cellcol6.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
								cellcol6.HorizontalAlignment = 1;		// centro
								cellcol6.BackgroundColor = BaseColor.YELLOW;
								cellcol7 = new PdfPCell(new Phrase("TOTAL", _BoldFont));
								cellcol7.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
								cellcol7.HorizontalAlignment = 1;		// centro
								cellcol7.BackgroundColor = BaseColor.YELLOW;
								// Añadimos las celdas a la tabla
								tblConceptos.AddCell(cellcol1);
								tblConceptos.AddCell(cellcol2);
								tblConceptos.AddCell(cellcol3);
								tblConceptos.AddCell(cellcol4);
								tblConceptos.AddCell(cellcol5);
								tblConceptos.AddCell(cellcol6);
								tblConceptos.AddCell(cellcol7);
								cuenta_lineas = 0;
							}
						}
						// Configuramos el título de las columnas de la tabla
						cellcol1 = new PdfPCell(new Phrase("", _BoldFont));
						//clnroatencion.BorderWidth = 1;			// Ancho del borde
						cellcol1.Border = iTextSharp.text.Rectangle.TOP_BORDER;
						cellcol1.HorizontalAlignment = 1;		// centro
						cellcol2 = new PdfPCell(new Phrase("", _BoldFont));
						cellcol2.Border = iTextSharp.text.Rectangle.TOP_BORDER;
						cellcol2.HorizontalAlignment = 1;		// centro
						cellcol3 = new PdfPCell(new Phrase("", _BoldFont));
						cellcol3.Border = iTextSharp.text.Rectangle.TOP_BORDER;
						cellcol3.HorizontalAlignment = 0;		// centro
						cellcol4 = new PdfPCell(new Phrase("TOTAL", _BoldFont));
						cellcol4.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
						cellcol4.HorizontalAlignment = 1;		// centro
						cellcol5 = new PdfPCell(new Phrase("", _BoldFont));
						cellcol5.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
						cellcol5.HorizontalAlignment = 1;		// centro
						cellcol6 = new PdfPCell(new Phrase("", _BoldFont));
						cellcol6.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
						cellcol6.HorizontalAlignment = 1;		// centro
						cellcol7 = new PdfPCell(new Phrase(toma_valor_total.ToString("C"), _BoldFont));
						cellcol7.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
						cellcol7.HorizontalAlignment = 1;		// centro
						// Añadimos las celdas a la tabla
						tblConceptos.AddCell(cellcol1);
						tblConceptos.AddCell(cellcol2);
						tblConceptos.AddCell(cellcol3);
						tblConceptos.AddCell(cellcol4);
						tblConceptos.AddCell(cellcol5);
						tblConceptos.AddCell(cellcol6);
						tblConceptos.AddCell(cellcol7);
						
						documento.Add(tblConceptos);
						
						documento.Add (new Paragraph (" ", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 6)));
						iTextSharp.text.Paragraph p = new Paragraph ("Importe en Letras :"+(string) class_public.ConvertirCadena(toma_valor_total.ToString(),"PESOS").ToUpper(), _BoldFont);
						p.Alignment = Element.ALIGN_LEFT;										
						documento.Add (p);						
					}catch(Exception de){
						Console.Error.WriteLine(de.StackTrace);
					}
					// step 5: we close the document
					documento.Close();					
					try{				
						//proc.Start();
						System.Diagnostics.Process.Start(pdf_name);	
					}catch(Exception ex){
						Console.Error.WriteLine(ex.Message);			
					}
				}
			}catch (NpgsqlException ex){
				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
				MessageType.Warning, ButtonsType.Ok, "PostgresSQL error: {0}",ex.Message);
				msgBoxError.Run ();					msgBoxError.Destroy();
				Console.WriteLine ("PostgresSQL error: {0}",ex.Message);
			}
			conexion.Close();			
		}
		
		
		private static iTextSharp.text.Image CreateBarcodeImage(string barcodeText)
        {
			Barcode128 code128 = new Barcode128();
			//code128.BarHeight = 5;
			code128.Code = barcodeText;
			System.Drawing.Bitmap bm = new System.Drawing.Bitmap(code128.CreateDrawingImage(System.Drawing.Color.Black, System.Drawing.Color.White));
			MemoryStream ms = new MemoryStream();
			bm.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
			iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance(ms.ToArray());
			code128.Extended = false;
			return img;
        }
		
		string llenado_movi_documentos(string folioservicio_)
		{
			string cadena_mov_doc = "";
			NpgsqlConnection conexion1; 
			conexion1 = new NpgsqlConnection (connectionString+nombrebd);
	        try{
				conexion1.Open ();
				NpgsqlCommand comando1; 
				comando1 = conexion1.CreateCommand ();
				comando1.CommandText = "SELECT * FROM osiris_erp_movimiento_documentos " +
					"WHERE folio_de_servicio = '"+folioservicio_+"';";
				NpgsqlDataReader lector1 = comando1.ExecuteReader ();
				while (lector1.Read()){
					cadena_mov_doc += lector1["informacion_capturada"].ToString().Trim()+"/";	
				}
				return cadena_mov_doc;
			}catch(NpgsqlException ex){
				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
									MessageType.Error,ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
				msgBoxError.Run ();					msgBoxError.Destroy();
	   		}
			conexion1.Close ();
			return "";
		}
		
		class EventoTitulos_abonopago : PdfPageEventHelper
		{
			class_public classpublic = new class_public();
									
			#region Fields
			private string _titulo1_rpt;
			private string _numerorecibo_caja;
			private string _numero_atencion_px;
			private string _nro_expediente_px;
			private string _nombres_apellidos_px;
			private string _fecha_ingresso_px;
			private string _fechahora_registropago;
			private string _codigobarra_registro;
			private string _tipo_paciente_px;
			private string _convenio_px;
			private string _medico_guardia_px;
			private string _medico_tratante_px;
			private string _concepto_depago;
			private string _doc_convenio_px;
			private string _observacion_depago;
			private string _impreso_por;
			private string _creado_por;
			private int _tipocomprobante_cj;
			#endregion
			
			#region Properties
			public string titulo1_rpt
        	{
            	get{
					return _titulo1_rpt;
				}
            	set{
					_titulo1_rpt = value;
				}
        	}
			public string numerorecibo_caja
        	{
            	get{
					return _numerorecibo_caja;
				}
            	set{
					_numerorecibo_caja = value;
				}
        	}
			public string numero_atencion_px
        	{
            	get{
					return _numero_atencion_px;
				}
            	set{
					_numero_atencion_px = value;
				}
        	}
			public string nro_expediente_px
        	{
            	get{
					return _nro_expediente_px;
				}
            	set{
					_nro_expediente_px = value;
				}
        	}
			public string nombres_apellidos_px
        	{
            	get{
					return _nombres_apellidos_px;
				}
            	set{
					_nombres_apellidos_px = value;
				}
        	}
			public string fecha_ingresso_px
        	{
            	get{
					return _fecha_ingresso_px;
				}
            	set{
					_fecha_ingresso_px = value;
				}
        	}
			public string fechahora_registropago
        	{
            	get{
					return _fechahora_registropago;
				}
            	set{
					_fechahora_registropago = value;
				}
        	}
			public string codigobarra_registro
        	{
            	get{
					return _codigobarra_registro;
				}
            	set{
					_codigobarra_registro = value;
				}
        	}
			public string tipo_paciente_px
        	{
            	get{
					return _tipo_paciente_px;
				}
            	set{
					_tipo_paciente_px = value;
				}
        	}
			public string convenio_px
        	{
            	get{
					return _convenio_px;
				}
            	set{
					_convenio_px = value;
				}
        	}
			public string medico_guardia_px
        	{
            	get{
					return _medico_guardia_px;
				}
            	set{
					_medico_guardia_px = value;
				}
        	}
			public string medico_tratante_px
        	{
            	get{
					return _medico_tratante_px;
				}
            	set{
					_medico_tratante_px = value;
				}
        	}
			public string concepto_depago
        	{
            	get{
					return _concepto_depago;
				}
            	set{
					_concepto_depago = value;
				}
        	}
			public string doc_convenio_px
        	{
            	get{
					return _doc_convenio_px;
				}
            	set{
					_doc_convenio_px = value;
				}
        	}
			public string observacion_depago
			{
				get{
					return _observacion_depago;
				}
				set{
					_observacion_depago = value;
				}
			}
			public string impreso_por
			{
				get{
					return _impreso_por;
				}
				set{
					_impreso_por = value;
				}
			}
			public string creado_por
			{
				get{
					return _creado_por;
				}
				set{
					_creado_por = value;
				}
			}
			public int tipocomprobante_cj
			{
				get{
					return _tipocomprobante_cj;
				}
				set{
					_tipocomprobante_cj = value;
				}
			}					
			#endregion
			
			public override void OnOpenDocument(PdfWriter writerpdf, Document documento)
		    {
					
		    }
		
		    public override void OnStartPage(PdfWriter writerpdf, Document documento)
		    {		
		        // fuente para las tablas creadas
				iTextSharp.text.Font _NormalFont;
				iTextSharp.text.Font _BoldFont;
				_NormalFont = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 7, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);
				_BoldFont = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 7, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
				 iTextSharp.text.pdf.BaseFont bf = BaseFont.CreateFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
				iTextSharp.text.Rectangle pageSize = documento.PageSize;
				PdfContentByte cb = writerpdf.DirectContent;
				float percentage = 0.0f;
				
				// Creamos la imagen y le ajustamos el tamaño
				//iTextSharp.text.Image imagen = iTextSharp.text.Image.GetInstance("/opt/osiris/bin/OSIRISLogo2.png");
				iTextSharp.text.Image imagen = iTextSharp.text.Image.GetInstance(System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory,"OSIRISLogo2.png"));
				imagen.BorderWidth = 0;
				imagen.Alignment = Element.ALIGN_LEFT;
				percentage = 150 / imagen.Width;
				imagen.ScalePercent(percentage * 65);
				//Insertamos la imagen en el documento
				documento.Add(imagen);
				
				// we tell the ContentByte we're ready to draw text
				cb.BeginText ();
				cb.SetFontAndSize (BaseFont.CreateFont (BaseFont.HELVETICA_BOLD, BaseFont.CP1252, BaseFont.NOT_EMBEDDED), 9);
				cb.SetTextMatrix (130,760);		cb.ShowText (classpublic.nombre_empresa);
				cb.SetTextMatrix (130,750);		cb.ShowText (classpublic.direccion_empresa);
				cb.SetTextMatrix (130,740);		cb.ShowText (classpublic.telefonofax_empresa);
				cb.SetTextMatrix (500,735);		cb.ShowText ("N° FOLIO");
				if(tipocomprobante_cj == 6){
					cb.SetFontAndSize (BaseFont.CreateFont (BaseFont.HELVETICA_BOLD, BaseFont.CP1252, BaseFont.NOT_EMBEDDED), 6);
					cb.SetTextMatrix (15,410);	cb.ShowText ("____ CONSULTA     ____ CONSULTA URG.   ____ PROCEDIMIENTO");					
				}
				cb.SetColorFill(iTextSharp.text.BaseColor.RED);
                cb.SetFontAndSize(bf, 11);
				cb.SetTextMatrix (510,720);		cb.ShowText (numerorecibo_caja);
				cb.SetColorFill(iTextSharp.text.BaseColor.BLACK);
				cb.SetFontAndSize (BaseFont.CreateFont (BaseFont.HELVETICA_BOLD, BaseFont.CP1252, BaseFont.NOT_EMBEDDED), 6);
				cb.SetTextMatrix (130,730);		cb.ShowText ("Sistema Hospitalario OSIRIS");
				cb.SetTextMatrix (500,760);		cb.ShowText ("Fech.Rpt:" + (string)DateTime.Now.ToString ("yyyy-MM-dd HH:mm:ss"));
				cb.SetTextMatrix (500,750);		cb.ShowText ("N° Page :"+writerpdf.PageNumber.ToString("D8"));
				cb.EndText ();
				
				cb.MoveTo(0, documento.PageSize.Height/2);
				cb.SetLineWidth(0.05f);
				cb.LineTo(documento.PageSize.Width, documento.PageSize.Height / 2);
				cb.Stroke();				
				
				Paragraph titulo1_reporte = new Paragraph(titulo1_rpt, new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 10, iTextSharp.text.Font.BOLD, BaseColor.BLACK));
                titulo1_reporte.Alignment = Element.ALIGN_CENTER;
                documento.Add(titulo1_reporte);
				documento.Add (new Paragraph (" ", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 6)));				
				//documento.Add (Chunk.NEWLINE);
				
				PdfPCell cellcol1;
				PdfPCell cellcol3;
				PdfPCell cellcol2;
				PdfPCell cellcol4;
				PdfPCell cellcol5;
				PdfPCell cellcol6;
				
				PdfPTable tabFot1 = new PdfPTable(6);
				tabFot1.WidthPercentage = 100;
				float[] widths_tabfot1 = new float[] { 30f, 20f, 30f, 20f, 30f, 190f };	// controlando el ancho de cada columna tienen que sumas 315 en total
				tabFot1.SetWidths(widths_tabfot1);
				tabFot1.HorizontalAlignment = 0;
				cellcol1 = new PdfPCell(new Phrase("N° Atencion",_BoldFont));
				cellcol1.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER ;
				cellcol2 = new PdfPCell(new Phrase(numero_atencion_px,_NormalFont));
				cellcol2.Border = iTextSharp.text.Rectangle.TOP_BORDER;
				cellcol3 = new PdfPCell(new Phrase("N° Exp.",_BoldFont));
				cellcol3.HorizontalAlignment = 2;	
				cellcol3.Border = iTextSharp.text.Rectangle.TOP_BORDER;
				cellcol4 = new PdfPCell(new Phrase(nro_expediente_px,_NormalFont));
				cellcol4.Border = iTextSharp.text.Rectangle.TOP_BORDER;
				cellcol5 = new PdfPCell(new Phrase("Nombre PX.",_BoldFont));
				cellcol5.Border = iTextSharp.text.Rectangle.TOP_BORDER;
				cellcol5.HorizontalAlignment = 2;
				cellcol6 = new PdfPCell(new Phrase(nombres_apellidos_px,_NormalFont));
				cellcol6.Border = iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
				cellcol6.HorizontalAlignment = 0;
				tabFot1.AddCell(cellcol1);
				tabFot1.AddCell(cellcol2);
				tabFot1.AddCell(cellcol3);
				tabFot1.AddCell(cellcol4);
				tabFot1.AddCell(cellcol5);
				tabFot1.AddCell(cellcol6);
				documento.Add(tabFot1);
				
				PdfPTable tabFot2 = new PdfPTable(4);
				tabFot2.WidthPercentage = 100;
				float[] widths_tabfot2 = new float[] { 40f, 50f, 50f, 200f };	// controlando el ancho de cada columna tienen que sumas 315 en total
				tabFot2.SetWidths(widths_tabfot2);
				tabFot2.HorizontalAlignment = 0;
				cellcol1 = new PdfPCell(new Phrase("Fecha Ingreso",_BoldFont));
				cellcol1.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
				cellcol2 = new PdfPCell(new Phrase(fecha_ingresso_px,_NormalFont));
				cellcol2.Border = PdfPCell.NO_BORDER;
				cellcol2.HorizontalAlignment = 0;
				cellcol3 = new PdfPCell(new Phrase("Registro/Pago",_BoldFont));
				cellcol3.HorizontalAlignment = 2;		// derecha		
				cellcol3.Border = PdfPCell.NO_BORDER;
				cellcol4 = new PdfPCell(new Phrase(fechahora_registropago,_NormalFont));
				cellcol4.Border = iTextSharp.text.Rectangle.RIGHT_BORDER;
				cellcol4.HorizontalAlignment = 0;								
				tabFot2.AddCell(cellcol1);
				tabFot2.AddCell(cellcol2);
				tabFot2.AddCell(cellcol3);
				tabFot2.AddCell(cellcol4);				
				documento.Add(tabFot2);
				
				PdfPTable tabFot3 = new PdfPTable(4);
				tabFot3.WidthPercentage = 100;
				float[] widths_tabfot3 = new float[] { 30f, 50f, 50f, 100f };	// controlando el ancho de cada columna tienen que sumas 315 en total
				tabFot3.SetWidths(widths_tabfot3);
				tabFot3.HorizontalAlignment = 0;
				cellcol1 = new PdfPCell(new Phrase("TIPO DE PACIENTE",_BoldFont));
				cellcol1.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
				cellcol2 = new PdfPCell(new Phrase(tipo_paciente_px,_BoldFont));
				cellcol2.Border = PdfPCell.NO_BORDER;
				cellcol2.HorizontalAlignment = 0;
				cellcol3 = new PdfPCell(new Phrase("CONVENIO",_BoldFont));
				cellcol3.HorizontalAlignment = 2;		// derecha		
				cellcol3.Border = PdfPCell.NO_BORDER;
				cellcol4 = new PdfPCell(new Phrase(convenio_px,_BoldFont));
				cellcol4.Border = iTextSharp.text.Rectangle.RIGHT_BORDER;
				cellcol4.HorizontalAlignment = 0;
				tabFot3.AddCell(cellcol1);
				tabFot3.AddCell(cellcol2);
				tabFot3.AddCell(cellcol3);
				tabFot3.AddCell(cellcol4);
				documento.Add(tabFot3);
				
				PdfPTable tabFot4 = new PdfPTable(4);
				tabFot4.WidthPercentage = 100;
				float[] widths_tabfot4 = new float[] { 40f, 120f, 40f, 120f };	// controlando el ancho de cada columna tienen que sumas 315 en total
				tabFot4.SetWidths(widths_tabfot4);
				tabFot4.HorizontalAlignment = 0;
				cellcol1 = new PdfPCell(new Phrase("Medico Guardia",_BoldFont));
				cellcol1.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
				cellcol2 = new PdfPCell(new Phrase(medico_guardia_px,_NormalFont));
				cellcol2.Border = iTextSharp.text.Rectangle.NO_BORDER;
				cellcol2.HorizontalAlignment = 0;
				cellcol3 = new PdfPCell(new Phrase("Medico Tratante",_BoldFont));
				cellcol3.HorizontalAlignment = 2;		// derecha		
				cellcol3.Border = iTextSharp.text.Rectangle.NO_BORDER;
				cellcol4 = new PdfPCell(new Phrase(medico_tratante_px,_NormalFont));
				cellcol4.Border = iTextSharp.text.Rectangle.RIGHT_BORDER;
				cellcol4.HorizontalAlignment = 0;
				tabFot4.AddCell(cellcol1);
				tabFot4.AddCell(cellcol2);
				tabFot4.AddCell(cellcol3);
				tabFot4.AddCell(cellcol4);
				documento.Add(tabFot4);
				
				PdfPTable tabFot5 = new PdfPTable(4);
				tabFot5.WidthPercentage = 100;
				float[] widths_tabfot5 = new float[] { 40f, 100f, 30f, 140 };	// controlando el ancho de cada columna tienen que sumas 315 en total
				tabFot5.SetWidths(widths_tabfot5);
				tabFot5.HorizontalAlignment = 0;
				cellcol1 = new PdfPCell(new Phrase("Concepto Pago",_BoldFont));
				cellcol1.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
				cellcol2 = new PdfPCell(new Phrase(concepto_depago,_NormalFont));
				cellcol2.Border = iTextSharp.text.Rectangle.NO_BORDER;
				cellcol2.HorizontalAlignment = 0;
				cellcol3 = new PdfPCell(new Phrase("Nomina",_BoldFont));
				cellcol3.HorizontalAlignment = 2;		// derecha		
				cellcol3.Border = iTextSharp.text.Rectangle.NO_BORDER;
				cellcol4 = new PdfPCell(new Phrase(doc_convenio_px,_NormalFont));
				cellcol4.Border = iTextSharp.text.Rectangle.RIGHT_BORDER;
				cellcol4.HorizontalAlignment = 0;
				tabFot5.AddCell(cellcol1);
				tabFot5.AddCell(cellcol2);
				tabFot5.AddCell(cellcol3);
				tabFot5.AddCell(cellcol4);
				
				documento.Add(tabFot5);
				
				PdfPTable tabFot6 = new PdfPTable(2);
				tabFot6.WidthPercentage = 100;
				float[] widths_tabfot6 = new float[] { 37f, 260f };	// controlando el ancho de cada columna tienen que sumas 315 en total
				tabFot6.SetWidths(widths_tabfot6);
				tabFot6.HorizontalAlignment = 0;
				cellcol1 = new PdfPCell(new Phrase("Observacion",_BoldFont));
				cellcol1.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
				cellcol2 = new PdfPCell(new Phrase(observacion_depago,_NormalFont));
				cellcol2.Border = iTextSharp.text.Rectangle.RIGHT_BORDER;
				cellcol2.HorizontalAlignment = 0;
				tabFot6.AddCell(cellcol1);
				tabFot6.AddCell(cellcol2);
				documento.Add(tabFot6);
				
				PdfPTable tabFot7 = new PdfPTable(4);
				tabFot7.WidthPercentage = 100;
				float[] widths_tabfot7 = new float[] { 40f, 120f, 40f, 120f };	// controlando el ancho de cada columna tienen que sumas 315 en total
				tabFot7.SetWidths(widths_tabfot7);
				tabFot7.HorizontalAlignment = 0;
				cellcol1 = new PdfPCell(new Phrase("Impreso Por",_BoldFont));
				cellcol1.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER;
				cellcol2 = new PdfPCell(new Phrase(impreso_por,_NormalFont));
				cellcol2.Border = iTextSharp.text.Rectangle.NO_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER;
				cellcol2.HorizontalAlignment = 0;
				cellcol3 = new PdfPCell(new Phrase("Creado Por",_BoldFont));
				cellcol3.HorizontalAlignment = 2;		// derecha		
				cellcol3.Border = iTextSharp.text.Rectangle.NO_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER;
				cellcol4 = new PdfPCell(new Phrase(creado_por,_NormalFont));
				cellcol4.Border = iTextSharp.text.Rectangle.RIGHT_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER;
				cellcol4.HorizontalAlignment = 0;
				tabFot7.AddCell(cellcol1);
				tabFot7.AddCell(cellcol2);
				tabFot7.AddCell(cellcol3);
				tabFot7.AddCell(cellcol4);
				documento.Add(tabFot7);
								
				iTextSharp.text.Image imagen2 = CreateBarcodeImage(fechahora_registropago);
				//imagen2.SetAbsolutePosition(15, 710);
				//imagen2.SetAbsolutePosition.
				imagen2.Alignment = Element.ALIGN_LEFT;
				percentage = 125 / imagen2.Width;
				imagen2.ScalePercent(percentage * 140);
				documento.Add(imagen2);
				//cb.AddImage(imagen2);
							
				documento.Add (new Paragraph (" ", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 5)));
			}				
		}
	}
}


/*

	Document document = new Document();
    PdfWriter writer = PdfWriter.getInstance(document, new FileOutputStream(dest));
    document.open();
    PdfPTable table = new PdfPTable(4);
    table.setWidthPercentage(100);
    for (int i = 0; i < 12; i++) {
        table.addCell(createBarcode(writer, String.format("%08d", i)));
    }
    document.add(table);
    document.close();


public static PdfPCell createBarcode(PdfWriter writer, String code) throws DocumentException, IOException {
    BarcodeEAN barcode = new BarcodeEAN();
    barcode.setCodeType(Barcode.EAN8);
    barcode.setCode(code);
    PdfPCell cell = new PdfPCell(barcode.createImageWithBarcode(writer.getDirectContent(), BaseColor.BLACK, BaseColor.GRAY), true);
    cell.setPadding(10);
    return cell;
}
*/