///////////////////////////////////////////////////////////
// project created on 24/10/2006 at 10:20 a
// Sistema Hospitalario OSIRIS
// Monterrey - Mexico
//
// Autor    	: Juan Antonio Peña Gonzalez (Programacion) gjuanzz@gmail.com
// 				  Daniel Olivares Cuevas (Pre-Programacion, Colaboracion y Ajustes) arcangeldoc@openmailbox.org
//				  Daniel Olivares Cuevas (cambio de biblioteca de impresion a GTKPrinter+Pango+Cairo)
//				  Daniel Olivares Cuevas (Cambio a libreria iTextSharp Nov. 2015)
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
// Proposito	: Impresion del procedimiento de cobranza 
// Objeto		: 
using System;
using Gtk;
using Npgsql;
using Cairo;
using Pango;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;

namespace osiris
{
	public class rpt_proc_cobranza
	{
		string connectionString;
		string nombrebd;
		
		private static int pangoScale = 1024;
		private PrintOperation print;
		private double fontSize = 8.0;
		int numpage = 1;		
		int comienzo_linea = 0;
		int separacion_linea = 10;  		
		int escala_en_linux_windows;		// Linux = 1  Windows = 8
		        
		int PidPaciente = 0;
		int folioservicio = 0;
		string fecha_admision;
		string fechahora_alta;
		string nombre_paciente;
		string telefono_paciente;
		string doctor;
		string cirugia;
		string fecha_nacimiento;
		string edadpac;
		int id_tipopaciente = 0;
		string tipo_paciente;
		string aseguradora;
		string dir_pac;
		string empresapac;
		string salahabitacion;
		string especialidad_doctor;
		string dignostico_paciente;
		bool apl_desc_siempre = true;
		bool apl_desc;
		int contador = 1;
		//querys
		string query_rango_fechas = " ";
		string query_nro_atencion = "";
		string query_todo = "SELECT "+
			"osiris_erp_cobros_deta.folio_de_servicio,osiris_erp_cobros_deta.pid_paciente, "+ 
			"osiris_his_tipo_admisiones.descripcion_admisiones,osiris_productos.aplicar_iva, "+
			"osiris_his_tipo_admisiones.id_tipo_admisiones AS idadmisiones,"+
			"osiris_grupo_producto.descripcion_grupo_producto,osiris_grupo_producto.cuenta_mayor_ingreso,"+
			"osiris_productos.id_grupo_producto,"+
			"to_char(osiris_erp_cobros_deta.porcentage_descuento,'999.99') AS porcdesc, "+
			"to_char(osiris_erp_cobros_deta.fechahora_creacion,'dd-mm-yyyy') AS fechcreacion,  "+
			"to_char(osiris_erp_cobros_deta.fechahora_creacion,'HH:mm') AS horacreacion,  "+
			"to_char(osiris_erp_cobros_deta.id_producto,'999999999999') AS idproducto,osiris_productos.descripcion_producto, "+
			"to_char(osiris_erp_cobros_deta.cantidad_aplicada,'9999.99') AS cantidadaplicada, "+
			"to_char(osiris_erp_cobros_deta.precio_producto,'999999.99') AS preciounitario, "+
			"ltrim(to_char(osiris_erp_cobros_deta.precio_producto,'9999999.99')) AS preciounitarioprod, "+
			"to_char(osiris_erp_cobros_deta.iva_producto,'999999.99') AS ivaproducto, "+
			"to_char(osiris_erp_cobros_deta.cantidad_aplicada * osiris_erp_cobros_deta.precio_producto,'99999999.99') AS ppcantidad,"+
			//"to_char(osiris_erp_cobros_deta.precio_por_cantidad,'999999.99') AS ppcantidad, "+					
			"to_char(osiris_productos.precio_producto_publico,'999999999.99999') AS preciopublico, "+
			"to_char(osiris_erp_cobros_enca.total_abonos,'999999999.999') AS totalabono, "+ 
			"to_char(osiris_erp_cobros_enca.honorario_medico,'999999999.999') AS honorario, "+
			"to_char(osiris_erp_cobros_enca.total_pago,'999999999.999') AS totalpago,aplica_iva_engrupos," +
			"osiris_erp_cobros_enca.id_paquete_quirurgico AS idpaquete_qx,descripcion_cirugia,osiris_his_tipo_cirugias.precio_de_venta AS preciodeventa_pq "+
			"FROM "+
			"osiris_erp_cobros_deta,osiris_erp_cobros_enca,osiris_his_tipo_admisiones,osiris_productos,osiris_grupo_producto,osiris_his_tipo_cirugias "+
			"WHERE "+
			"osiris_erp_cobros_deta.id_tipo_admisiones = osiris_his_tipo_admisiones.id_tipo_admisiones "+
			"AND osiris_erp_cobros_deta.id_producto = osiris_productos.id_producto "+ 
			"AND osiris_productos.id_grupo_producto = osiris_grupo_producto.id_grupo_producto "+
			"AND osiris_erp_cobros_enca.folio_de_servicio = osiris_erp_cobros_deta.folio_de_servicio "+
			"AND osiris_erp_cobros_deta.eliminado = 'false' " +
			"AND osiris_erp_cobros_enca.id_paquete_quirurgico = osiris_his_tipo_cirugias.id_tipo_cirugia ";
		string order_by_query = "";
		
		int idadmision_ = 0;
		int idgrupoproducto = 0;
		string descrip_producto = "";
		string cuentamayoringreso = "";
		string datos = "";
		string fcreacion = "";
		decimal porcentajedes =  0;
		decimal descsiniva = 0;
		decimal ivadedesc = 0;
		decimal descuento = 0;
		decimal ivaprod = 0;
		decimal subtotal = 0;
		decimal subtotal_impuesto = 0;
		decimal total_subtotal_0 = 0;
		decimal sumadesc = 0;
		decimal total_iva = 0;
		decimal total = 0;
		decimal totaladm = 0;
		decimal totaldesc = 0;
		decimal subtotal_depto_producto = 0;
		decimal ivatotal_depto_producto = 0;
		decimal total_depto_producto = 0;
		decimal total_grupo_producto = 0;
		decimal subtotal_grupo_producto = 0;
		decimal ivatotal_grupo_producto = 0;
		decimal subtotaldelmov = 0;
		decimal deducible = 0;
		decimal coaseguro = 0;
		//public int contdesc = 0;
		//agrega abonos y pagos honorarios
		decimal totabono = 0;
		decimal totpago = 0;
		decimal honorarios = 0;
		decimal PorcentIVA;
		string name_reporte = "";
		string tipodereporte = "";
		bool aplica_iva_servmedico;
		
		
		//Declaracion de ventana de error
		protected Gtk.Window MyWinError;
		
		class_conexion conexion_a_DB = new class_conexion();
		class_public classpublic = new class_public();
		
		public rpt_proc_cobranza (int PidPaciente_,int folioservicio_,string nombrebd_ ,string entry_fecha_admision_,string entry_fechahora_alta_,
						string entry_numero_factura_,string entry_nombre_paciente_,string entry_telefono_paciente_,string entry_doctor_,
						string entry_tipo_paciente_,string entry_aseguradora_,string edadpac_,string fecha_nacimiento_,string dir_pac_,
						string cirugia_,string empresapac_,int idtipopaciente_,string query, string salahabitacion_,string especialidad_doctor_,
						string dignostico_paciente_,string tipodereporte_,string tipo_salida_,int idpaquetequirurgico_,bool aplica_iva_servmedico_)
		{
			PidPaciente = PidPaciente_;//
			folioservicio = folioservicio_;//
			
			connectionString = conexion_a_DB._url_servidor+conexion_a_DB._port_DB+conexion_a_DB._usuario_DB+conexion_a_DB._passwrd_user_DB;
			nombrebd = conexion_a_DB._nombrebd;
			escala_en_linux_windows = classpublic.escala_linux_windows;
			PorcentIVA = decimal.Parse(classpublic.ivaparaaplicar);
			
			//nombrebd = _nombrebd_;//
			fecha_admision = entry_fecha_admision_;
			fechahora_alta = entry_fechahora_alta_;
			nombre_paciente = entry_nombre_paciente_;
			telefono_paciente = entry_telefono_paciente_;
			doctor = entry_doctor_;
			cirugia = cirugia_;
			id_tipopaciente = idtipopaciente_;
			tipo_paciente = entry_tipo_paciente_;
			empresapac = empresapac_;
			aseguradora = entry_aseguradora_;
			edadpac = edadpac_;
			fecha_nacimiento = fecha_nacimiento_;
			dir_pac = dir_pac_;
			query_rango_fechas = query;
			salahabitacion = salahabitacion_;
			especialidad_doctor = especialidad_doctor_;
			dignostico_paciente = dignostico_paciente_;
			tipodereporte = tipodereporte_;
			aplica_iva_servmedico = aplica_iva_servmedico_;
			query_nro_atencion = "AND osiris_erp_cobros_deta.folio_de_servicio = '" + folioservicio.ToString () + "' ";
			if (tipodereporte_ == "procedimiento"){
				// imprime el detalle divido por fechas y deparatamentos
				name_reporte = "PROCEDIMIENTO DE COBRANZA";
			}
			if (tipodereporte_ == "resumen_factura"){
				// Imprime solo los totales del procedimiento
				name_reporte = "RESUMEN DE COBRANZA";
			}
			crea_rpt_pdf(tipodereporte_);
		}
		
		void crea_rpt_pdf(string tipodereporte_)
		{
			
			string convenio_empr_aseg = "";
			
			if(tipo_paciente == "ASEGURADO"){				
				convenio_empr_aseg = aseguradora;
			}else{
				convenio_empr_aseg = empresapac;
			}
			
			if (tipodereporte_ == "procedimiento"){
				// imprime el detalle divido por fechas y deparatamentos
				NpgsqlConnection conexion; 
				conexion = new NpgsqlConnection (connectionString+nombrebd);
				// Verifica que la base de datos este conectada Querys
				try{
					conexion.Open ();
					NpgsqlCommand comando; 
					comando = conexion.CreateCommand (); 
					comando.CommandText = query_todo + query_nro_atencion + query_rango_fechas + order_by_query;
					Console.WriteLine(comando.CommandText);		
					NpgsqlDataReader lector = comando.ExecuteReader ();					
					if (lector.Read()){
						// step 1: creation of a document-object
						Document documento = new Document(iTextSharp.text.PageSize.LETTER, 15, 15, 15, 15);
						//Document document = new Document(PageSize.A4.Rotate());
						string movdocumentos = llenado_movi_documentos(folioservicio.ToString());
						string ticket_de_caja = llenado_ticket_caja(folioservicio.ToString().Trim());
						string doc_x_facturar = llenado_doc_x_facturar(folioservicio.ToString().Trim());
						string pdf_name = System.IO.Path.GetTempPath() + Guid.NewGuid().ToString() + ".pdf";
						totpago = decimal.Parse((string) lector["totalabono"]);
						totabono = decimal.Parse((string) lector["totalpago"]);
						honorarios = decimal.Parse((string) lector["honorario"]);						
						try{
							PdfWriter writerpdf = PdfWriter.GetInstance(documento, new FileStream(pdf_name, FileMode.Create));	
							documento.AddTitle("Reporte de Abonos/Pagos por Paciente");
				            documento.AddCreator("Sistema Hospitalario OSIRIS");
				            documento.AddAuthor("Sistema Hospitalario OSIRIS");
				            documento.AddSubject("OSIRSrpt");	
							EventoTitulos ev = new EventoTitulos();
							ev.titulo1_rpt = "HOJA DE CARGOS";
							ev.numero_atencion_px = folioservicio.ToString();
							ev.fecha_ingresso_px = fecha_admision;
							ev.fecha_egresso_px = fechahora_alta;
							ev.nro_expediente_px = PidPaciente.ToString().Trim();
							ev.nombres_apellidos_px = classpublic.extract_spaces(nombre_paciente.ToString());
							ev.fecha_nacimiento_px = fecha_nacimiento;
							ev.edad_px = edadpac;
							ev.direccion_px = dir_pac;
							ev.telefono_px = telefono_paciente;
							ev.tipo_paciente_px = tipo_paciente;
							ev.convenio_px = convenio_empr_aseg;
							ev.medico_tratante_px = doctor;
							ev.especia_medtrat_px = especialidad_doctor;
							ev.habitacion_px = salahabitacion;
							ev.doc_convenio_px = movdocumentos;
							ev.motivoingreso_px = dignostico_paciente;
							ev.nro_ticket_px = ticket_de_caja;
							ev.doc_x_facturar_px = doc_x_facturar;
							writerpdf.PageEvent = ev;

							documento.Open();
							imprime_totales_grupo_prod(documento,writerpdf,int.Parse(lector["idpaquete_qx"].ToString()),lector["descripcion_cirugia"].ToString().Trim(),decimal.Parse(lector["preciodeventa_pq"].ToString()));
							if(int.Parse(lector["idpaquete_qx"].ToString()) > 1){
								imprime_extras_pq(documento,writerpdf,int.Parse(lector["idpaquete_qx"].ToString()),lector["descripcion_cirugia"].ToString().Trim(),decimal.Parse(lector["preciodeventa_pq"].ToString()));
								imprime_conceptos_totales(documento,writerpdf,int.Parse(lector["idpaquete_qx"].ToString()));
							}else{
								imprime_conceptos_group_fecha_depto(documento,writerpdf);	
							}
														
							//System.Diagnostics.Process proc = new System.Diagnostics.Process();
							//proc.EnableRaisingEvents = true;
							//proc.StartInfo.UseShellExecute = false;
							//proc.StartInfo.CreateNoWindow = true;
							//proc.StartInfo.RedirectStandardOutput = true;
							//proc.StartInfo.WorkingDirectory = AppDomain.CurrentDomain.BaseDirectory;
							//if((string) classpublic.plataform_OS() == "Unix"){ 
							//	proc.StartInfo.FileName = classpublic.lector_de_pdf_linux;
							//}
							//if((string) classpublic.plataform_OS() == "Win32NT"){ 
							//	proc.StartInfo.FileName = classpublic.lectos_de_pdf_win;
							//}
							//proc.StartInfo.Arguments = pdf_name;
						}catch(Exception de){
							Console.Error.WriteLine(de.StackTrace);
						}
						// step 5: we close the document
						documento.Close();
						try{				
							//proc.Start();
							System.Diagnostics.Process.Start(pdf_name);
						}catch(Exception ex){
							Console.WriteLine("Error al leer el PDF: {0}",ex.Message);
							MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
								MessageType.Error,ButtonsType.Close,"Debe Instalar un Lector de archivos tipo PDF error: {0}",ex.Message);
							msgBoxError.Run ();		msgBoxError.Destroy();
						}
					}
				}catch (NpgsqlException ex){
					MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
						MessageType.Warning, ButtonsType.Ok, "PostgresSQL error: {0}",ex.Message);
					msgBoxError.Run ();				msgBoxError.Destroy();
					Console.WriteLine ("PostgresSQL error: {0}",ex.Message);
				}
				conexion.Close();
			}
			if (tipodereporte_ == "resumen_factura"){
				// imprime el detalle divido por fechas y deparatamentos
				NpgsqlConnection conexion; 
				conexion = new NpgsqlConnection (connectionString+nombrebd);
				// Verifica que la base de datos este conectada Querys
				try{
					conexion.Open ();
					NpgsqlCommand comando; 
					comando = conexion.CreateCommand (); 
					comando.CommandText = query_todo + query_nro_atencion + query_rango_fechas + order_by_query;
					//Console.WriteLine(comando.CommandText);		
					NpgsqlDataReader lector = comando.ExecuteReader ();					
					if (lector.Read()){
						string movdocumentos = llenado_movi_documentos(folioservicio.ToString());
						string ticket_de_caja = llenado_ticket_caja(folioservicio.ToString().Trim());
						string doc_x_facturar = llenado_doc_x_facturar(folioservicio.ToString().Trim());
						string pdf_name = System.IO.Path.GetTempPath() + Guid.NewGuid().ToString() + ".pdf";
						totpago = decimal.Parse((string) lector["totalabono"]);
						totabono = decimal.Parse((string) lector["totalpago"]);
						honorarios = decimal.Parse((string) lector["honorario"]);						
						// step 1: creation of a document-object
						Document documento = new Document(iTextSharp.text.PageSize.LETTER, 15, 15, 15, 15);
						//Document document = new Document(PageSize.A4.Rotate());	
						try{
							PdfWriter writerpdf = PdfWriter.GetInstance(documento, new FileStream(pdf_name, FileMode.Create));
	
							documento.AddTitle("Reporte de Abonos/Pagos por Paciente");
				            documento.AddCreator("Sistema Hospitalario OSIRIS");
				            documento.AddAuthor("Sistema Hospitalario OSIRIS");
				            documento.AddSubject("OSIRSrpt");	
							EventoTitulos ev = new EventoTitulos();
							ev.titulo1_rpt = "TOTALES PARA FACTURACION";
							ev.numero_atencion_px = folioservicio.ToString();
							ev.fecha_ingresso_px = fecha_admision;
							ev.fecha_egresso_px = fechahora_alta;
							ev.nro_expediente_px = PidPaciente.ToString().Trim();
							ev.nombres_apellidos_px = classpublic.extract_spaces(nombre_paciente.ToString());
							ev.fecha_nacimiento_px = fecha_nacimiento;
							ev.edad_px = edadpac;
							ev.direccion_px = dir_pac;
							ev.telefono_px = telefono_paciente;
							ev.tipo_paciente_px = tipo_paciente;
							ev.convenio_px = convenio_empr_aseg;
							ev.medico_tratante_px = doctor;
							ev.especia_medtrat_px = especialidad_doctor;
							ev.habitacion_px = salahabitacion;
							ev.doc_convenio_px = movdocumentos;
							ev.motivoingreso_px = dignostico_paciente;
							ev.nro_ticket_px = ticket_de_caja;
							ev.doc_x_facturar_px = doc_x_facturar;
							writerpdf.PageEvent = ev;
							documento.Open();
							
							imprime_totales_grupo_prod(documento,writerpdf,int.Parse(lector["idpaquete_qx"].ToString()),lector["descripcion_cirugia"].ToString().Trim(),decimal.Parse(lector["preciodeventa_pq"].ToString()));
							
							//System.Diagnostics.Process proc = new System.Diagnostics.Process();
							//proc.EnableRaisingEvents = true;
							//proc.StartInfo.UseShellExecute = false;
							//proc.StartInfo.CreateNoWindow = true;
							//proc.StartInfo.RedirectStandardOutput = true;
							//proc.StartInfo.WorkingDirectory = AppDomain.CurrentDomain.BaseDirectory;
							//if((string) classpublic.plataform_OS() == "Unix"){ 
							//	proc.StartInfo.FileName = classpublic.lector_de_pdf_linux;
							//}
							//if((string) classpublic.plataform_OS() == "Win32NT"){ 
							//	proc.StartInfo.FileName = classpublic.lectos_de_pdf_win;
							//}
							//proc.StartInfo.Arguments = pdf_name;
						}catch(Exception de){
							Console.Error.WriteLine(de.StackTrace);
						}
						// step 5: we close the document
						documento.Close();
						try{				
							//proc.Start();
							System.Diagnostics.Process.Start(pdf_name);
						}catch(Exception ex){
							Console.WriteLine("Error al leer el PDF: {0}",ex.Message);
							MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
									MessageType.Error,ButtonsType.Close,"Debe Instalar un Lector de archivos tipo PDF error: {0}",ex.Message);
							msgBoxError.Run ();		msgBoxError.Destroy();
						}
					}else{
						Console.WriteLine("NO encontre nada para Imprimir");
						MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
									MessageType.Error,ButtonsType.Close,"NO encontre nada para Imprimir");
						msgBoxError.Run ();		msgBoxError.Destroy();
					}
				}catch (NpgsqlException ex){
					MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
						MessageType.Warning, ButtonsType.Ok, "PostgresSQL error: {0}",ex.Message);
					msgBoxError.Run ();				msgBoxError.Destroy();
					Console.WriteLine ("PostgresSQL error: {0}",ex.Message);
				}
				conexion.Close();
			}
		}
		
		void imprime_totales_grupo_prod(Document documento,PdfWriter writerpdf,int nro_paquete_qx,string nombre_paquete_qx,decimal valor_paquete_qx)
		{
			decimal totaldelmov = ((subtotaldelmov - deducible - coaseguro - totaldesc - totabono - totpago) + honorarios) + valor_paquete_qx;
			order_by_query = "ORDER BY osiris_productos.id_grupo_producto,osiris_erp_cobros_deta.id_secuencia; ";
			subtotal_grupo_producto = 0;
			ivatotal_grupo_producto = 0;
			total_grupo_producto = 0;
			decimal iva_subtotal_al_0 = 0;
			decimal subtotaldelmov_pq = 0;
			decimal total_iva_pq = 0;
			// imprime el detalle divido por fechas y deparatamentos
			NpgsqlConnection conexion; 
			conexion = new NpgsqlConnection (connectionString+nombrebd);
			// Verifica que la base de datos este conectada Querys
			try{
				conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand (); 
				comando.CommandText = query_todo + query_nro_atencion + query_rango_fechas + order_by_query;
				//Console.WriteLine(comando.CommandText);		
				NpgsqlDataReader lector = comando.ExecuteReader ();					
				if (lector.Read()){
					// fuente para las tablas creadas
					iTextSharp.text.Font _NormalFont;
					iTextSharp.text.Font _BoldFont;
					_NormalFont = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 7, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);
					_BoldFont = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 7, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
					// Creamos una tabla para el contenido
		            PdfPTable tblreporte = new PdfPTable(5);
		            tblreporte.WidthPercentage = 100;
					float[] widths = new float[] { 35f, 160f, 40f, 40f, 40f };	// controlando el ancho de cada columna tienen que sumas 315 en total
					tblreporte.SetWidths(widths);
					tblreporte.HorizontalAlignment = 0;
												
					// Configuramos el título de las columnas de la tabla
					PdfPCell cl01 = new PdfPCell();
					PdfPCell cl02 = new PdfPCell();
					PdfPCell cl03 = new PdfPCell();
					PdfPCell cl04 = new PdfPCell();
					PdfPCell cl05 = new PdfPCell();
					
					iTextSharp.text.Paragraph p = new Paragraph ("TOTALES GRUPO PRODUCTOS/PQ.", _NormalFont);
					p.Alignment = Element.ALIGN_CENTER;										
					documento.Add (p);
					documento.Add (new Paragraph (" ", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 2)));
					
					// Creamos una tabla para el contenido
					// Configuramos el título de las columnas de la tabla
					cl01 = new PdfPCell(new Phrase("CODIGO", _BoldFont));
					//clnroatencion.BorderWidth = 1;			// Ancho del borde
					cl01.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
					cl01.HorizontalAlignment = 1;		// centro
					cl01.BackgroundColor = BaseColor.YELLOW;
					cl02 = new PdfPCell(new Phrase("GRUPO", _BoldFont));
					cl02.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
					cl02.HorizontalAlignment = 1;		// centro
					cl02.BackgroundColor = BaseColor.YELLOW;
					cl03 = new PdfPCell(new Phrase("SUB-TOTAL", _BoldFont));
					cl03.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
					cl03.HorizontalAlignment = 1;		// centro
					cl03.BackgroundColor = BaseColor.YELLOW;
					cl04 = new PdfPCell(new Phrase("IVA.", _BoldFont));
					cl04.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
					cl04.HorizontalAlignment = 1;		// centro
					cl04.BackgroundColor = BaseColor.YELLOW;
					cl05 = new PdfPCell(new Phrase("TOTAL", _BoldFont));
					cl05.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
					cl05.HorizontalAlignment = 1;		// centro
					cl05.BackgroundColor = BaseColor.YELLOW;
					// Añadimos las celdas a la tabla
					tblreporte.AddCell(cl01);
					tblreporte.AddCell(cl02);
					tblreporte.AddCell(cl03);
					tblreporte.AddCell(cl04);
					tblreporte.AddCell(cl05);
										
					idgrupoproducto = (int) lector["id_grupo_producto"];
					descrip_producto = (string) lector["descripcion_grupo_producto"];
					cuentamayoringreso = lector["cuenta_mayor_ingreso"].ToString().Trim();
					
					subtotal = decimal.Parse((string) lector["cantidadaplicada"].ToString()) * decimal.Parse((string) lector["preciounitario"].ToString());
					
					if((bool) aplica_iva_servmedico == false){
						if((bool) lector["aplicar_iva"]== true) {
							ivaprod = (subtotal*PorcentIVA)/100;
							subtotal_impuesto += subtotal;
						}else{
							total_subtotal_0 += subtotal;
							ivaprod = 0;
						}
					}else{
						if((bool) lector["aplica_iva_engrupos"] == true){
							ivaprod = (subtotal*PorcentIVA)/100;
							subtotal_impuesto += subtotal;
						}else{
							ivaprod = 0;
							subtotal_impuesto += subtotal;
						}
					}						
					total_iva += ivaprod;
					subtotaldelmov += subtotal;
					
					subtotal_grupo_producto += subtotal;
					ivatotal_grupo_producto += ivaprod;
										
					while (lector.Read()){
						if(idgrupoproducto != (int) lector["id_grupo_producto"]){
							if(nro_paquete_qx > 1){
								subtotal_grupo_producto = 0;
								ivatotal_grupo_producto = 0;	
							}
							// Configuramos el título de las columnas de la tabla
							//clnroatencion.BorderWidth = 1;			// Ancho del borde
							cl01 = new PdfPCell(new Phrase(cuentamayoringreso, _NormalFont));
							cl01.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
							cl01.HorizontalAlignment = 0;		// centro
							cl02 = new PdfPCell(new Phrase(descrip_producto, _NormalFont));
							cl02.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
							cl02.HorizontalAlignment = 0;		// centro
							cl03 = new PdfPCell(new Phrase(subtotal_grupo_producto.ToString("C"), _NormalFont));
							cl03.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
							cl03.HorizontalAlignment = 2;		// centro
							cl04 = new PdfPCell(new Phrase(ivatotal_grupo_producto.ToString("C"), _NormalFont));
							cl04.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
							cl04.HorizontalAlignment = 2;		// centro
							cl05 = new PdfPCell(new Phrase(string.Format("{0:C}",subtotal_grupo_producto+ivatotal_grupo_producto), _NormalFont));
							cl05.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
							cl05.HorizontalAlignment = 2;		// centro
							// Añadimos las celdas a la tabla
							tblreporte.AddCell(cl01);
							tblreporte.AddCell(cl02);
							tblreporte.AddCell(cl03);
							tblreporte.AddCell(cl04);
							tblreporte.AddCell(cl05);
							
							subtotal_grupo_producto = 0;
							ivatotal_grupo_producto = 0;
							idgrupoproducto = (int) lector["id_grupo_producto"];
							descrip_producto = (string) lector["descripcion_grupo_producto"];
							cuentamayoringreso = lector["cuenta_mayor_ingreso"].ToString().Trim();
						}
						subtotal = decimal.Parse((string) lector["cantidadaplicada"].ToString()) * decimal.Parse((string) lector["preciounitario"].ToString());
						if((bool) aplica_iva_servmedico == false){
							if((bool) lector["aplicar_iva"]== true) {
								ivaprod = (subtotal*PorcentIVA)/100;
								subtotal_impuesto += subtotal;
							}else{
								total_subtotal_0 += subtotal;
								ivaprod = 0;
							}
						}else{
							if((bool) lector["aplica_iva_engrupos"] == true){
								ivaprod = (subtotal*PorcentIVA)/100;
								subtotal_impuesto += subtotal;
							}else{
								ivaprod = 0;
								subtotal_impuesto += subtotal;
							}
						}
						subtotaldelmov += subtotal;
						total_iva += ivaprod;
						
						subtotal_grupo_producto += subtotal;
						ivatotal_grupo_producto += ivaprod;
						
					}
					if(nro_paquete_qx > 1){
						subtotal_grupo_producto = 0;
						ivatotal_grupo_producto = 0;
						subtotal_impuesto = 0;
						total_iva = 0;
						total_subtotal_0 = 0;
					}
									
					// Configuramos el título de las columnas de la tabla
					cl01 = new PdfPCell(new Phrase(cuentamayoringreso, _NormalFont));
					//clnroatencion.BorderWidth = 1;			// Ancho del borde
					cl01.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
					cl01.HorizontalAlignment = 0;		// centro
					cl02 = new PdfPCell(new Phrase(descrip_producto, _NormalFont));
					cl02.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
					cl02.HorizontalAlignment = 0;		// centro
					cl03 = new PdfPCell(new Phrase(subtotal_grupo_producto.ToString("C"), _NormalFont));
					cl03.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
					cl03.HorizontalAlignment = 2;		// centro
					cl04 = new PdfPCell(new Phrase(ivatotal_grupo_producto.ToString("C"), _NormalFont));
					cl04.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
					cl04.HorizontalAlignment = 2;		// centro
					cl05 = new PdfPCell(new Phrase(string.Format("{0:C}",subtotal_grupo_producto+ivatotal_grupo_producto), _NormalFont));
					cl05.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
					cl05.HorizontalAlignment = 2;		// centro
					// Añadimos las celdas a la tabla
					tblreporte.AddCell(cl01);
					tblreporte.AddCell(cl02);
					tblreporte.AddCell(cl03);
					tblreporte.AddCell(cl04);
					tblreporte.AddCell(cl05);
					
					if(nro_paquete_qx > 1){
						subtotaldelmov_pq = valor_paquete_qx / ((PorcentIVA/100) + 1);
						total_iva_pq = valor_paquete_qx - (valor_paquete_qx / ((PorcentIVA/100) +1));
						//preciodeventa
						// Configuramos el título de las columnas de la tabla
						//clnroatencion.BorderWidth = 1;			// Ancho del borde
						cl01 = new PdfPCell(new Phrase("", _NormalFont));
						cl01.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
						cl01.HorizontalAlignment = 0;		// centro
						cl02 = new PdfPCell(new Phrase("PAQUETE "+nro_paquete_qx.ToString().Trim()+" "+nombre_paquete_qx, _NormalFont));
						cl02.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
						cl02.HorizontalAlignment = 0;		// centro
						cl03 = new PdfPCell(new Phrase(string.Format("{0:C}",subtotaldelmov_pq), _NormalFont));
						cl03.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
						cl03.HorizontalAlignment = 2;		// centro
						cl04 = new PdfPCell(new Phrase(string.Format("{0:C}",total_iva_pq), _NormalFont));
						cl04.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
						cl04.HorizontalAlignment = 2;		// centro
						cl05 = new PdfPCell(new Phrase(valor_paquete_qx.ToString("C"), _NormalFont));
						cl05.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
						cl05.HorizontalAlignment = 2;		// centro
						// Añadimos las celdas a la tabla
						tblreporte.AddCell(cl01);
						tblreporte.AddCell(cl02);
						tblreporte.AddCell(cl03);
						tblreporte.AddCell(cl04);
						tblreporte.AddCell(cl05);
					}
					// Configuramos el título de las columnas de la tabla
					cl01 = new PdfPCell(new Phrase("", _NormalFont));
					//clnroatencion.BorderWidth = 1;			// Ancho del borde
					cl01.Border = iTextSharp.text.Rectangle.TOP_BORDER;
					cl01.HorizontalAlignment = 0;		// izq
					cl02 = new PdfPCell(new Phrase("SubTotal c/iva", _NormalFont));
					cl02.Border = iTextSharp.text.Rectangle.TOP_BORDER;
					cl02.HorizontalAlignment = 2;		// der
					cl03 = new PdfPCell(new Phrase(subtotal_impuesto.ToString("C"), _NormalFont));
					cl03.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
					cl03.HorizontalAlignment = 2;		// der
					cl04 = new PdfPCell(new Phrase(total_iva.ToString("C"), _NormalFont));
					cl04.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
					cl04.HorizontalAlignment = 2;		// der
					cl05 = new PdfPCell(new Phrase(string.Format("{0:C}",subtotal_impuesto+total_iva), _NormalFont));
					cl05.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
					cl05.HorizontalAlignment = 2;		// der
					// Añadimos las celdas a la tabla
					tblreporte.AddCell(cl01);
					tblreporte.AddCell(cl02);
					tblreporte.AddCell(cl03);
					tblreporte.AddCell(cl04);
					tblreporte.AddCell(cl05);
					
					// Configuramos el título de las columnas de la tabla
					cl01 = new PdfPCell(new Phrase("", _NormalFont));
					//clnroatencion.BorderWidth = 1;			// Ancho del borde
					cl01.Border = iTextSharp.text.Rectangle.NO_BORDER;
					cl01.HorizontalAlignment = 0;		// izq
					cl02 = new PdfPCell(new Phrase("SubTotal sin/iva", _NormalFont));
					cl02.Border = iTextSharp.text.Rectangle.NO_BORDER;
					cl02.HorizontalAlignment = 2;		// der
					cl03 = new PdfPCell(new Phrase(total_subtotal_0.ToString("C"), _NormalFont));
					cl03.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
					cl03.HorizontalAlignment = 2;		// der
					cl04 = new PdfPCell(new Phrase(iva_subtotal_al_0.ToString("C"), _NormalFont));
					cl04.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
					cl04.HorizontalAlignment = 2;		// der
					cl05 = new PdfPCell(new Phrase(string.Format("{0:C}",total_subtotal_0), _NormalFont));
					cl05.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
					cl05.HorizontalAlignment = 2;		// der
					// Añadimos las celdas a la tabla
					tblreporte.AddCell(cl01);
					tblreporte.AddCell(cl02);
					tblreporte.AddCell(cl03);
					tblreporte.AddCell(cl04);
					tblreporte.AddCell(cl05);
					
					if(nro_paquete_qx > 1){
						subtotaldelmov = subtotaldelmov_pq;
						total_iva = total_iva_pq;
					}
					
					// Configuramos el título de las columnas de la tabla
					cl01 = new PdfPCell(new Phrase("", _NormalFont));
					//clnroatencion.BorderWidth = 1;			// Ancho del borde
					cl01.Border = iTextSharp.text.Rectangle.NO_BORDER;
					cl01.HorizontalAlignment = 0;		// izq
					cl02 = new PdfPCell(new Phrase("SUB-TOTALES", _BoldFont));
					cl02.Border = iTextSharp.text.Rectangle.NO_BORDER;
					cl02.HorizontalAlignment = 2;		// der
					cl03 = new PdfPCell(new Phrase(subtotaldelmov.ToString("C"), _BoldFont));
					cl03.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
					cl03.HorizontalAlignment = 2;		// der
					cl04 = new PdfPCell(new Phrase(total_iva.ToString("C"), _BoldFont));
					cl04.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
					cl04.HorizontalAlignment = 2;		// der
					cl05 = new PdfPCell(new Phrase(string.Format("{0:C}",subtotaldelmov+total_iva), _BoldFont));
					cl05.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
					cl05.HorizontalAlignment = 2;		// der
					// Añadimos las celdas a la tabla
					tblreporte.AddCell(cl01);
					tblreporte.AddCell(cl02);
					tblreporte.AddCell(cl03);
					tblreporte.AddCell(cl04);
					tblreporte.AddCell(cl05);					
					
					// Configuramos el título de las columnas de la tabla
					cl01 = new PdfPCell(new Phrase("", _NormalFont));
					//clnroatencion.BorderWidth = 1;			// Ancho del borde
					cl01.Border = iTextSharp.text.Rectangle.NO_BORDER;
					cl01.HorizontalAlignment = 0;		// izq
					cl02 = new PdfPCell(new Phrase("MENOS DEDUCIBLE", _NormalFont));
					cl02.Border = iTextSharp.text.Rectangle.NO_BORDER;
					cl02.HorizontalAlignment = 2;		// der
					cl03 = new PdfPCell(new Phrase("0.00", _NormalFont));
					cl03.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
					cl03.HorizontalAlignment = 2;		// der
					cl04 = new PdfPCell(new Phrase("0.00", _NormalFont));
					cl04.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
					cl04.HorizontalAlignment = 2;		// der
					cl05 = new PdfPCell(new Phrase("0.00", _NormalFont));
					cl05.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
					cl05.HorizontalAlignment = 2;		// der
					// Añadimos las celdas a la tabla
					tblreporte.AddCell(cl01);
					tblreporte.AddCell(cl02);
					tblreporte.AddCell(cl03);
					tblreporte.AddCell(cl04);
					tblreporte.AddCell(cl05);
					
					// Configuramos el título de las columnas de la tabla
					cl01 = new PdfPCell(new Phrase("", _NormalFont));
					//clnroatencion.BorderWidth = 1;			// Ancho del borde
					cl01.Border = iTextSharp.text.Rectangle.NO_BORDER;
					cl01.HorizontalAlignment = 0;
					cl02 = new PdfPCell(new Phrase("MENOS COASEGURO", _NormalFont));
					cl02.Border = iTextSharp.text.Rectangle.NO_BORDER;
					cl02.HorizontalAlignment = 2;
					cl03 = new PdfPCell(new Phrase("0.00", _NormalFont));
					cl03.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
					cl03.HorizontalAlignment = 2;
					cl04 = new PdfPCell(new Phrase("0.00", _NormalFont));
					cl04.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
					cl04.HorizontalAlignment = 2;
					cl05 = new PdfPCell(new Phrase("0.00", _NormalFont));
					cl05.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
					cl05.HorizontalAlignment = 2;
					// Añadimos las celdas a la tabla
					tblreporte.AddCell(cl01);
					tblreporte.AddCell(cl02);
					tblreporte.AddCell(cl03);
					tblreporte.AddCell(cl04);
					tblreporte.AddCell(cl05);
					
					// Configuramos el título de las columnas de la tabla
					cl01 = new PdfPCell(new Phrase("", _NormalFont));
					//clnroatencion.BorderWidth = 1;			// Ancho del borde
					cl01.Border = iTextSharp.text.Rectangle.NO_BORDER;
					cl01.HorizontalAlignment = 0;
					cl02 = new PdfPCell(new Phrase("MENOS DESCUENTO", _NormalFont));
					cl02.Border = iTextSharp.text.Rectangle.NO_BORDER;
					cl02.HorizontalAlignment = 2;
					cl03 = new PdfPCell(new Phrase("0.00", _NormalFont));
					cl03.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
					cl03.HorizontalAlignment = 2;
					cl04 = new PdfPCell(new Phrase("0.00", _NormalFont));
					cl04.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
					cl04.HorizontalAlignment = 2;
					cl05 = new PdfPCell(new Phrase("0.00", _NormalFont));
					cl05.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
					cl05.HorizontalAlignment = 2;
					// Añadimos las celdas a la tabla
					tblreporte.AddCell(cl01);
					tblreporte.AddCell(cl02);
					tblreporte.AddCell(cl03);
					tblreporte.AddCell(cl04);
					tblreporte.AddCell(cl05);
					
					// Configuramos el título de las columnas de la tabla
					cl01 = new PdfPCell(new Phrase("", _NormalFont));
					//clnroatencion.BorderWidth = 1;			// Ancho del borde
					cl01.Border = iTextSharp.text.Rectangle.NO_BORDER;
					cl01.HorizontalAlignment = 0;		// izq
					cl02 = new PdfPCell(new Phrase("TOTAL PAGOS", _NormalFont));
					cl02.Border = iTextSharp.text.Rectangle.NO_BORDER;
					cl02.HorizontalAlignment = 2;		// der
					cl03 = new PdfPCell(new Phrase("0.00", _NormalFont));
					cl03.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
					cl03.HorizontalAlignment = 2;		// der
					cl04 = new PdfPCell(new Phrase("0.00", _NormalFont));
					cl04.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
					cl04.HorizontalAlignment = 2;		// der
					cl05 = new PdfPCell(new Phrase(totpago.ToString("C"), _NormalFont));
					cl05.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
					cl05.HorizontalAlignment = 2;		// der
					// Añadimos las celdas a la tabla
					tblreporte.AddCell(cl01);
					tblreporte.AddCell(cl02);
					tblreporte.AddCell(cl03);
					tblreporte.AddCell(cl04);
					tblreporte.AddCell(cl05);
					
					// Configuramos el título de las columnas de la tabla
					cl01 = new PdfPCell(new Phrase("", _NormalFont));
					//clnroatencion.BorderWidth = 1;			// Ancho del borde
					cl01.Border = iTextSharp.text.Rectangle.NO_BORDER;
					cl01.HorizontalAlignment = 0;		// izq
					cl02 = new PdfPCell(new Phrase("TOTAL ABONOS", _NormalFont));
					cl02.Border = iTextSharp.text.Rectangle.NO_BORDER;
					cl02.HorizontalAlignment = 2;		// der
					cl03 = new PdfPCell(new Phrase("0.00", _NormalFont));
					cl03.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
					cl03.HorizontalAlignment = 2;		// der
					cl04 = new PdfPCell(new Phrase("0.00", _NormalFont));
					cl04.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
					cl04.HorizontalAlignment = 2;		// der
					cl05 = new PdfPCell(new Phrase(totabono.ToString("C"), _NormalFont));
					cl05.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
					cl05.HorizontalAlignment = 2;		// der
					// Añadimos las celdas a la tabla
					tblreporte.AddCell(cl01);
					tblreporte.AddCell(cl02);
					tblreporte.AddCell(cl03);
					tblreporte.AddCell(cl04);
					tblreporte.AddCell(cl05);
					
					// Configuramos el título de las columnas de la tabla
					cl01 = new PdfPCell(new Phrase("", _NormalFont));
					//clnroatencion.BorderWidth = 1;			// Ancho del borde
					cl01.Border = iTextSharp.text.Rectangle.NO_BORDER;
					cl01.HorizontalAlignment = 0;		// izq
					cl02 = new PdfPCell(new Phrase("HONORARIO MEDICO", _NormalFont));
					cl02.Border = iTextSharp.text.Rectangle.NO_BORDER;
					cl02.HorizontalAlignment = 2;		// der
					cl03 = new PdfPCell(new Phrase("0.00", _NormalFont));
					cl03.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
					cl03.HorizontalAlignment = 2;		// der
					cl04 = new PdfPCell(new Phrase("0.00", _NormalFont));
					cl04.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
					cl04.HorizontalAlignment = 2;		// der
					cl05 = new PdfPCell(new Phrase("0.00", _NormalFont));
					cl05.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
					cl05.HorizontalAlignment = 2;		// der
					// Añadimos las celdas a la tabla
					tblreporte.AddCell(cl01);
					tblreporte.AddCell(cl02);
					tblreporte.AddCell(cl03);
					tblreporte.AddCell(cl04);
					tblreporte.AddCell(cl05);
					
					// Configuramos el título de las columnas de la tabla
					cl01 = new PdfPCell(new Phrase("", _NormalFont));
					//clnroatencion.BorderWidth = 1;			// Ancho del borde
					cl01.Border = iTextSharp.text.Rectangle.NO_BORDER;
					cl01.HorizontalAlignment = 0;		// izq
					cl02 = new PdfPCell(new Phrase("SALDO", _NormalFont));
					cl02.Border = iTextSharp.text.Rectangle.NO_BORDER;
					cl02.HorizontalAlignment = 2;		// der
					cl03 = new PdfPCell(new Phrase(subtotaldelmov.ToString("C"), _NormalFont));
					cl03.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
					cl03.HorizontalAlignment = 2;		// der
					cl04 = new PdfPCell(new Phrase(total_iva.ToString("C"), _NormalFont));
					cl04.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
					cl04.HorizontalAlignment = 2;		// der
					cl05 = new PdfPCell(new Phrase(string.Format("{0:C}",subtotaldelmov+total_iva), _NormalFont));
					cl05.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
					cl05.HorizontalAlignment = 2;		// der
					// Añadimos las celdas a la tabla
					tblreporte.AddCell(cl01);
					tblreporte.AddCell(cl02);
					tblreporte.AddCell(cl03);
					tblreporte.AddCell(cl04);
					tblreporte.AddCell(cl05);
					
					// Finalmente, añadimos la tabla al documento PDF
					documento.Add(tblreporte);
					documento.Add (new Paragraph (" ", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 5)));
					documento.Add (new Paragraph (" ", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 5)));
				}
			}catch (NpgsqlException ex){
				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
						MessageType.Warning, ButtonsType.Ok, "PostgresSQL error: {0}",ex.Message);
				msgBoxError.Run ();				msgBoxError.Destroy();
				Console.WriteLine ("PostgresSQL error: {0}",ex.Message);
			}
			conexion.Close();
		}
		
		void imprime_conceptos_totales(Document documento,PdfWriter writerpdf,int idpaqueteqx_)
		{
			iTextSharp.text.Font _NormalFont;
			iTextSharp.text.Font _BoldFont;
			_NormalFont = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 7, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);
			_BoldFont = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 7, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
			order_by_query = "ORDER BY descripcion_producto; ";
						
			// Los totales generales
			total_subtotal_0 = 0;
			subtotaldelmov = 0;
			total_iva = 0;
			total = 0;
			
			decimal preciounitario_prod = 0;
			
			// imprime el detalle divido por fechas y deparatamentos
			NpgsqlConnection conexion; 
			conexion = new NpgsqlConnection (connectionString+nombrebd);
			// Verifica que la base de datos este conectada Querys
			try{
				conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand (); 
				comando.CommandText = "SELECT folio_de_servicio,to_char(osiris_erp_cobros_deta.id_producto,'999999999999') AS idproducto,osiris_productos.descripcion_producto,"+
									"SUM(cantidad_aplicada) AS cantidadaplicada," +
									"osiris_productos.aplicar_iva,"+
									"osiris_erp_cobros_deta.precio_producto AS preciounitario "+
									"FROM osiris_erp_cobros_deta,osiris_productos "+
									"WHERE osiris_erp_cobros_deta.id_producto = osiris_productos.id_producto "+
									"AND osiris_erp_cobros_deta.eliminado = 'false' "+
									query_nro_atencion+
									"GROUP BY osiris_erp_cobros_deta.id_producto,osiris_productos.descripcion_producto,folio_de_servicio,preciounitario,aplicar_iva "+
									"ORDER BY osiris_productos.descripcion_producto;";
				//Console.WriteLine(comando.CommandText);		
				NpgsqlDataReader lector = comando.ExecuteReader ();					
				if (lector.Read()){
					PdfPTable tblConceptos = new PdfPTable(7);
		            tblConceptos.WidthPercentage = 100;
					float[] widths2 = new float[] { 35f, 15f, 140f, 30f, 30f, 30f, 30f };	// controlando el ancho de cada columna tienen que sumas 315 en total
					tblConceptos.SetWidths(widths2);
					tblConceptos.HorizontalAlignment = 0;
					
					PdfPCell cl01 = new PdfPCell();
					PdfPCell cl02 = new PdfPCell();
					PdfPCell cl03 = new PdfPCell();
					PdfPCell cl04 = new PdfPCell();
					PdfPCell cl05 = new PdfPCell();
					PdfPCell cl06 = new PdfPCell();
					PdfPCell cl07 = new PdfPCell();
					
					iTextSharp.text.Paragraph p = new Paragraph ("TOTALES DE CONCEPTOS CARGADOS", _NormalFont);
					p.Alignment = Element.ALIGN_CENTER;										
					documento.Add (p);
					documento.Add (new Paragraph (" ", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 2)));
					//(string) lector["descripcion_admisiones"],fcreacion
					
					subtotal = decimal.Parse((string) lector["cantidadaplicada"].ToString()) * decimal.Parse((string) lector["preciounitario"].ToString());
					preciounitario_prod = decimal.Parse((string) lector["preciounitario"].ToString());
					ivaprod = 0;
					if(idpaqueteqx_ < 1){						
						if((bool) lector["aplicar_iva"]== true) {
							ivaprod = (subtotal*PorcentIVA)/100;
							subtotal_impuesto += subtotal;
						}else{
							total_subtotal_0 += subtotal;
							ivaprod = 0;
						}
					}else{
						subtotal = 0;
						ivaprod = 0;
						preciounitario_prod = 0;
					}																
					subtotaldelmov += subtotal;
					total_iva += ivaprod;
					total += subtotaldelmov + total_iva;
						
					
					// Configuramos el título de las columnas de la tabla
					cl01 = new PdfPCell(new Phrase("CODIGO", _BoldFont));
					//clnroatencion.BorderWidth = 1;			// Ancho del borde
					cl01.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
					cl01.HorizontalAlignment = 1;
					cl01.BackgroundColor = BaseColor.YELLOW;
					cl02 = new PdfPCell(new Phrase("CANT.", _BoldFont));
					cl02.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
					cl02.HorizontalAlignment = 1;
					cl02.BackgroundColor = BaseColor.YELLOW;
					cl03 = new PdfPCell(new Phrase("DESCRIPCION PRODUCTO", _BoldFont));
					cl03.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
					cl03.HorizontalAlignment = 0;
					cl03.BackgroundColor = BaseColor.YELLOW;
					cl04 = new PdfPCell(new Phrase("PRECIO", _BoldFont));
					cl04.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
					cl04.HorizontalAlignment = 1;
					cl04.BackgroundColor = BaseColor.YELLOW;
					cl05 = new PdfPCell(new Phrase("SUB-TOTAL", _BoldFont));
					cl05.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
					cl05.HorizontalAlignment = 1;
					cl05.BackgroundColor = BaseColor.YELLOW;
					cl06 = new PdfPCell(new Phrase("IVA", _BoldFont));
					cl06.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
					cl06.HorizontalAlignment = 1;
					cl06.BackgroundColor = BaseColor.YELLOW;
					cl07 = new PdfPCell(new Phrase("TOTAL", _BoldFont));
					cl07.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
					cl07.HorizontalAlignment = 1;
					cl07.BackgroundColor = BaseColor.YELLOW;
					// Añadimos las celdas a la tabla
					tblConceptos.AddCell(cl01);
					tblConceptos.AddCell(cl02);
					tblConceptos.AddCell(cl03);
					tblConceptos.AddCell(cl04);
					tblConceptos.AddCell(cl05);
					tblConceptos.AddCell(cl06);
					tblConceptos.AddCell(cl07);
					// Configuramos el título de las columnas de la tabla
					cl01 = new PdfPCell(new Phrase(lector["idproducto"].ToString().Trim(),_NormalFont));
					//clnroatencion.BorderWidth = 1;			// Ancho del borde
					cl01.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
					cl01.HorizontalAlignment = 0;
					cl02 = new PdfPCell(new Phrase(decimal.Parse(lector["cantidadaplicada"].ToString()).ToString("F"), _NormalFont));
					cl02.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
					cl02.HorizontalAlignment = 2;
					cl03 = new PdfPCell(new Phrase(lector["descripcion_producto"].ToString().Trim(), _NormalFont));
					cl03.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
					cl03.HorizontalAlignment = 0;
					cl04 = new PdfPCell(new Phrase(preciounitario_prod.ToString("F"), _NormalFont));
					cl04.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
					cl04.HorizontalAlignment = 2;
					cl05 = new PdfPCell(new Phrase(subtotal.ToString("F"), _NormalFont));
					cl05.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
					cl05.HorizontalAlignment = 2;
					cl06 = new PdfPCell(new Phrase(ivaprod.ToString("F"), _NormalFont));
					cl06.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
					cl06.HorizontalAlignment = 2;
					cl07 = new PdfPCell(new Phrase(string.Format("{0:C}",subtotal+ivaprod), _NormalFont));
					cl07.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
					cl07.HorizontalAlignment = 2;
					
					// Añadimos las celdas a la tabla
					tblConceptos.AddCell(cl01);
					tblConceptos.AddCell(cl02);
					tblConceptos.AddCell(cl03);
					tblConceptos.AddCell(cl04);
					tblConceptos.AddCell(cl05);
					tblConceptos.AddCell(cl06);
					tblConceptos.AddCell(cl07);
					
					while (lector.Read()){
						subtotal = decimal.Parse((string) lector["cantidadaplicada"].ToString()) * decimal.Parse((string) lector["preciounitario"].ToString());
						preciounitario_prod = decimal.Parse((string) lector["preciounitario"].ToString());
						ivaprod = 0;
						if(idpaqueteqx_ < 1){
							if((bool) lector["aplicar_iva"]== true) {
								ivaprod = (subtotal*PorcentIVA)/100;
								subtotal_impuesto += subtotal;
							}else{
								total_subtotal_0 += subtotal;
								ivaprod = 0;
							}
						}else{
							subtotal = 0;
							ivaprod = 0;
							preciounitario_prod = 0;
						}																	
						subtotaldelmov += subtotal;
						total_iva += ivaprod;
						total += subtotaldelmov + total_iva;
						
						// Configuramos el título de las columnas de la tabla
						cl01 = new PdfPCell(new Phrase(lector["idproducto"].ToString().Trim(),_NormalFont));
						//clnroatencion.BorderWidth = 1;			// Ancho del borde
						cl01.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
						cl01.HorizontalAlignment = 0;
						cl02 = new PdfPCell(new Phrase(decimal.Parse(lector["cantidadaplicada"].ToString()).ToString("F"), _NormalFont));
						cl02.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
						cl02.HorizontalAlignment = 2;
						cl03 = new PdfPCell(new Phrase(lector["descripcion_producto"].ToString().Trim(), _NormalFont));
						cl03.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
						cl03.HorizontalAlignment = 0;
						cl04 = new PdfPCell(new Phrase(preciounitario_prod.ToString("F"), _NormalFont));
						cl04.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
						cl04.HorizontalAlignment = 2;
						cl05 = new PdfPCell(new Phrase(subtotal.ToString("F"), _NormalFont));
						cl05.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
						cl05.HorizontalAlignment = 2;
						cl06 = new PdfPCell(new Phrase(total_iva.ToString("F"), _NormalFont));
						cl06.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
						cl06.HorizontalAlignment = 2;
						cl07 = new PdfPCell(new Phrase(string.Format("{0:C}",subtotal+ivaprod), _NormalFont));
						cl07.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
						cl07.HorizontalAlignment = 2;
						// Añadimos las celdas a la tabla
						tblConceptos.AddCell(cl01);
						tblConceptos.AddCell(cl02);
						tblConceptos.AddCell(cl03);
						tblConceptos.AddCell(cl04);
						tblConceptos.AddCell(cl05);
						tblConceptos.AddCell(cl06);
						tblConceptos.AddCell(cl07);
					}
					// Configuramos el título de las columnas de la tabla
					cl01 = new PdfPCell(new Phrase("",_NormalFont));
					//clnroatencion.BorderWidth = 1;			// Ancho del borde
					cl01.Border = iTextSharp.text.Rectangle.TOP_BORDER;
					cl01.HorizontalAlignment = 0;
					cl02 = new PdfPCell(new Phrase("", _NormalFont));
					cl02.Border = iTextSharp.text.Rectangle.TOP_BORDER;
					cl02.HorizontalAlignment = 2;
					cl03 = new PdfPCell(new Phrase("", _NormalFont));
					cl03.Border = iTextSharp.text.Rectangle.TOP_BORDER;
					cl03.HorizontalAlignment = 0;
					cl04 = new PdfPCell(new Phrase("TOTAL", _NormalFont));
					cl04.Border = iTextSharp.text.Rectangle.TOP_BORDER;
					cl04.HorizontalAlignment = 2;
					cl05 = new PdfPCell(new Phrase(subtotaldelmov.ToString("F"), _NormalFont));
					cl05.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
					cl05.HorizontalAlignment = 2;
					cl06 = new PdfPCell(new Phrase(total_iva.ToString("F"), _NormalFont));
					cl06.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
					cl06.HorizontalAlignment = 2;
					cl07 = new PdfPCell(new Phrase(string.Format("{0:C}",subtotaldelmov+total_iva), _NormalFont));
					cl07.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
					cl07.HorizontalAlignment = 2;
					// Añadimos las celdas a la tabla
					tblConceptos.AddCell(cl01);
					tblConceptos.AddCell(cl02);
					tblConceptos.AddCell(cl03);
					tblConceptos.AddCell(cl04);
					tblConceptos.AddCell(cl05);
					tblConceptos.AddCell(cl06);
					tblConceptos.AddCell(cl07);
					
					// Finalmente, añadimos las tabla al documento PDF
		            documento.Add(tblConceptos);
				}
			}catch (NpgsqlException ex){
				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
						MessageType.Warning, ButtonsType.Ok, "PostgresSQL error: {0}",ex.Message);
				msgBoxError.Run ();				msgBoxError.Destroy();
				Console.WriteLine ("PostgresSQL error: {0}",ex.Message);
			}
			conexion.Close();
		}
		
		void imprime_conceptos_group_fecha_depto(Document documento,PdfWriter writerpdf)
		{
			iTextSharp.text.Font _NormalFont;
			iTextSharp.text.Font _BoldFont;
			_NormalFont = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 7, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);
			_BoldFont = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 7, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
			order_by_query = "ORDER BY to_char(osiris_erp_cobros_deta.fechahora_creacion,'yyyy-MM-dd') ASC, osiris_erp_cobros_deta.id_tipo_admisiones ASC, osiris_productos.id_grupo_producto,osiris_erp_cobros_deta.id_secuencia;";
			// totales del grupo de producto
			subtotal_grupo_producto = 0;
			ivatotal_grupo_producto = 0;
			total_grupo_producto = 0;
			
			// totales del departamento
			subtotal_depto_producto = 0;
			ivatotal_depto_producto = 0;
			total_depto_producto = 0;
			
			// Los totales generales
			total_subtotal_0 = 0;
			subtotaldelmov = 0;
			total_iva = 0;
			total = 0;
			
			// imprime el detalle divido por fechas y deparatamentos
			NpgsqlConnection conexion; 
			conexion = new NpgsqlConnection (connectionString+nombrebd);
			// Verifica que la base de datos este conectada Querys
			try{
				conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand (); 
				comando.CommandText = query_todo + query_nro_atencion + query_rango_fechas + order_by_query;
				//Console.WriteLine(comando.CommandText);		
				NpgsqlDataReader lector = comando.ExecuteReader ();					
				if (lector.Read()){
					PdfPTable tblConceptos = new PdfPTable(7);
		            tblConceptos.WidthPercentage = 100;
					float[] widths2 = new float[] { 35f, 15f, 140f, 30f, 30f, 30f, 30f };	// controlando el ancho de cada columna tienen que sumas 315 en total
					tblConceptos.SetWidths(widths2);
					tblConceptos.HorizontalAlignment = 0;
					
					PdfPCell cl01 = new PdfPCell();
					PdfPCell cl02 = new PdfPCell();
					PdfPCell cl03 = new PdfPCell();
					PdfPCell cl04 = new PdfPCell();
					PdfPCell cl05 = new PdfPCell();
					PdfPCell cl06 = new PdfPCell();
					PdfPCell cl07 = new PdfPCell();
										
					documento.Add (new Paragraph ("CONCEPTOS CARGADOS", _NormalFont));
					documento.Add (new Paragraph (" ", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 2)));
					//(string) lector["descripcion_admisiones"],fcreacion
					
					subtotal = decimal.Parse((string) lector["cantidadaplicada"].ToString()) * decimal.Parse((string) lector["preciounitario"].ToString());
					ivaprod = 0;
					
					if((bool) aplica_iva_servmedico == false){
						if((bool) lector["aplicar_iva"]== true) {
							ivaprod = (subtotal*PorcentIVA)/100;
							subtotal_impuesto += subtotal;
						}else{
							total_subtotal_0 += subtotal;
							ivaprod = 0;
						}
					}else{
						if((bool) lector["aplica_iva_engrupos"] == true){
							ivaprod = (subtotal*PorcentIVA)/100;
							subtotal_impuesto += subtotal;
						}else{
							ivaprod = 0;
							subtotal_impuesto += subtotal;
						}
					}
					subtotal_grupo_producto += subtotal;
					ivatotal_grupo_producto += ivaprod;
					
					subtotaldelmov += subtotal;
					total_iva += ivaprod;
					total += subtotaldelmov + total_iva;
					
					idadmision_ = (int) lector["idadmisiones"];
					idgrupoproducto = (int) lector["id_grupo_producto"];
					fcreacion = (string) lector["fechcreacion"];
					
					// Configuramos el título de las columnas de la tabla
					cl01 = new PdfPCell(new Phrase("CODIGO", _BoldFont));
					//clnroatencion.BorderWidth = 1;			// Ancho del borde
					cl01.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
					cl01.HorizontalAlignment = 1;		// centro
					cl01.BackgroundColor = BaseColor.YELLOW;
					cl02 = new PdfPCell(new Phrase("CANT.", _BoldFont));
					cl02.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
					cl02.HorizontalAlignment = 1;		// centro
					cl02.BackgroundColor = BaseColor.YELLOW;
					cl03 = new PdfPCell(new Phrase("DESCRIPCION PRODUCTO", _BoldFont));
					cl03.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
					cl03.HorizontalAlignment = 0;		// centro
					cl03.BackgroundColor = BaseColor.YELLOW;
					cl04 = new PdfPCell(new Phrase("PRECIO", _BoldFont));
					cl04.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
					cl04.HorizontalAlignment = 1;		// centro
					cl04.BackgroundColor = BaseColor.YELLOW;
					cl05 = new PdfPCell(new Phrase("SUB-TOTAL", _BoldFont));
					cl05.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
					cl05.HorizontalAlignment = 1;		// centro
					cl05.BackgroundColor = BaseColor.YELLOW;
					cl06 = new PdfPCell(new Phrase("IVA", _BoldFont));
					cl06.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
					cl06.HorizontalAlignment = 1;		// centro
					cl06.BackgroundColor = BaseColor.YELLOW;
					cl07 = new PdfPCell(new Phrase("TOTAL", _BoldFont));
					cl07.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
					cl07.HorizontalAlignment = 1;		// centro
					cl07.BackgroundColor = BaseColor.YELLOW;
					// Añadimos las celdas a la tabla
					tblConceptos.AddCell(cl01);
					tblConceptos.AddCell(cl02);
					tblConceptos.AddCell(cl03);
					tblConceptos.AddCell(cl04);
					tblConceptos.AddCell(cl05);
					tblConceptos.AddCell(cl06);
					tblConceptos.AddCell(cl07);
					
					// Configuramos el título de las columnas de la tabla
					cl01 = new PdfPCell(new Phrase("", _BoldFont));
					//clnroatencion.BorderWidth = 1;			// Ancho del borde
					cl01.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
					cl01.HorizontalAlignment = 1;
					cl02 = new PdfPCell(new Phrase("", _BoldFont));
					cl02.Border = iTextSharp.text.Rectangle.NO_BORDER;
					cl02.HorizontalAlignment = 1;
					cl03 = new PdfPCell(new Phrase((string) lector["descripcion_admisiones"]+" "+fcreacion, _BoldFont));
					cl03.Border = iTextSharp.text.Rectangle.NO_BORDER;
					cl03.HorizontalAlignment = 1;
					cl04 = new PdfPCell(new Phrase("", _BoldFont));
					cl04.Border = iTextSharp.text.Rectangle.NO_BORDER;
					cl04.HorizontalAlignment = 1;
					cl05 = new PdfPCell(new Phrase("", _BoldFont));
					cl05.Border = iTextSharp.text.Rectangle.NO_BORDER;
					cl05.HorizontalAlignment = 1;
					cl06 = new PdfPCell(new Phrase("", _BoldFont));
					cl06.Border = iTextSharp.text.Rectangle.NO_BORDER;
					cl06.HorizontalAlignment = 1;
					cl07 = new PdfPCell(new Phrase("", _BoldFont));
					cl07.Border = iTextSharp.text.Rectangle.RIGHT_BORDER;
					cl07.HorizontalAlignment = 1;					
					// Añadimos las celdas a la tabla
					tblConceptos.AddCell(cl01);
					tblConceptos.AddCell(cl02);
					tblConceptos.AddCell(cl03);
					tblConceptos.AddCell(cl04);
					tblConceptos.AddCell(cl05);
					tblConceptos.AddCell(cl06);
					tblConceptos.AddCell(cl07);
					
					// Configuramos el título de las columnas de la tabla
					cl01 = new PdfPCell(new Phrase("", _BoldFont));
					//clnroatencion.BorderWidth = 1;			// Ancho del borde
					cl01.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
					cl01.HorizontalAlignment = 1;
					cl02 = new PdfPCell(new Phrase("", _BoldFont));
					cl02.Border = iTextSharp.text.Rectangle.NO_BORDER;
					cl02.HorizontalAlignment = 1;
					cl03 = new PdfPCell(new Phrase((string) lector["descripcion_grupo_producto"], _BoldFont));
					cl03.Border = iTextSharp.text.Rectangle.NO_BORDER;
					cl03.HorizontalAlignment = 0;
					cl04 = new PdfPCell(new Phrase("", _BoldFont));
					cl04.Border = iTextSharp.text.Rectangle.NO_BORDER;
					cl04.HorizontalAlignment = 1;
					cl05 = new PdfPCell(new Phrase("", _BoldFont));
					cl05.Border = iTextSharp.text.Rectangle.NO_BORDER;
					cl05.HorizontalAlignment = 1;
					cl06 = new PdfPCell(new Phrase("", _BoldFont));
					cl06.Border = iTextSharp.text.Rectangle.NO_BORDER;
					cl06.HorizontalAlignment = 1;
					cl07 = new PdfPCell(new Phrase("", _BoldFont));
					cl07.Border = iTextSharp.text.Rectangle.RIGHT_BORDER;
					cl07.HorizontalAlignment = 1;					
					// Añadimos las celdas a la tabla
					tblConceptos.AddCell(cl01);
					tblConceptos.AddCell(cl02);
					tblConceptos.AddCell(cl03);
					tblConceptos.AddCell(cl04);
					tblConceptos.AddCell(cl05);
					tblConceptos.AddCell(cl06);
					tblConceptos.AddCell(cl07);				
					
					// Configuramos el título de las columnas de la tabla
					cl01 = new PdfPCell(new Phrase(lector["idproducto"].ToString().Trim(),_NormalFont));
					//clnroatencion.BorderWidth = 1;			// Ancho del borde
					cl01.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
					cl01.HorizontalAlignment = 0;		// centro
					cl02 = new PdfPCell(new Phrase(decimal.Parse(lector["cantidadaplicada"].ToString()).ToString("F"), _NormalFont));
					cl02.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
					cl02.HorizontalAlignment = 2;		// centro
					cl03 = new PdfPCell(new Phrase(lector["descripcion_producto"].ToString().Trim(), _NormalFont));
					cl03.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
					cl03.HorizontalAlignment = 0;		// centro
					cl04 = new PdfPCell(new Phrase(decimal.Parse((string) lector["preciounitario"].ToString()).ToString("F"), _NormalFont));
					cl04.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
					cl04.HorizontalAlignment = 2;		// centro
					cl05 = new PdfPCell(new Phrase(subtotal.ToString("F"), _NormalFont));
					cl05.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
					cl05.HorizontalAlignment = 2;		// centro
					cl06 = new PdfPCell(new Phrase(ivaprod.ToString("F"), _NormalFont));
					cl06.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
					cl06.HorizontalAlignment = 2;		// centro
					cl07 = new PdfPCell(new Phrase(string.Format("{0:C}",subtotal+ivaprod), _NormalFont));
					cl07.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
					cl07.HorizontalAlignment = 2;		// centro					
					// Añadimos las celdas a la tabla
					tblConceptos.AddCell(cl01);
					tblConceptos.AddCell(cl02);
					tblConceptos.AddCell(cl03);
					tblConceptos.AddCell(cl04);
					tblConceptos.AddCell(cl05);
					tblConceptos.AddCell(cl06);
					tblConceptos.AddCell(cl07);
					
					while (lector.Read()){
						subtotal = decimal.Parse((string) lector["cantidadaplicada"].ToString()) * decimal.Parse((string) lector["preciounitario"].ToString());
						ivaprod = 0;
						if((bool) aplica_iva_servmedico == false){
							if((bool) lector["aplicar_iva"]== true) {
								ivaprod = (subtotal*PorcentIVA)/100;
								subtotal_impuesto += subtotal;
							}else{
								total_subtotal_0 += subtotal;
								ivaprod = 0;
							}
						}else{
							if((bool) lector["aplica_iva_engrupos"] == true){
								ivaprod = (subtotal*PorcentIVA)/100;
								subtotal_impuesto += subtotal;
							}else{
								ivaprod = 0;
								subtotal_impuesto += subtotal;
							}
						}
						subtotal_grupo_producto += subtotal;
						ivatotal_grupo_producto += ivaprod;
						
						subtotaldelmov += subtotal;
						total_iva += ivaprod;
						total += subtotaldelmov + total_iva;
						
						if(idadmision_ == (int) lector["idadmisiones"] && fcreacion == (string) lector["fechcreacion"]) {
							if (idgrupoproducto != (int) lector["id_grupo_producto"]){
								// Configuramos el título de las columnas de la tabla
								cl01 = new PdfPCell(new Phrase("", _BoldFont));
								//clnroatencion.BorderWidth = 1;			// Ancho del borde
								cl01.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
								cl01.HorizontalAlignment = 1;
								cl02 = new PdfPCell(new Phrase("", _BoldFont));
								cl02.Border = iTextSharp.text.Rectangle.NO_BORDER;
								cl02.HorizontalAlignment = 1;
								cl03 = new PdfPCell(new Phrase((string) lector["descripcion_grupo_producto"], _BoldFont));
								cl03.Border = iTextSharp.text.Rectangle.NO_BORDER;
								cl03.HorizontalAlignment = 0;
								cl04 = new PdfPCell(new Phrase("", _BoldFont));
								cl04.Border = iTextSharp.text.Rectangle.NO_BORDER;
								cl04.HorizontalAlignment = 1;
								cl05 = new PdfPCell(new Phrase("", _BoldFont));
								cl05.Border = iTextSharp.text.Rectangle.NO_BORDER;
								cl05.HorizontalAlignment = 1;
								cl06 = new PdfPCell(new Phrase("", _BoldFont));
								cl06.Border = iTextSharp.text.Rectangle.NO_BORDER;
								cl06.HorizontalAlignment = 1;
								cl07 = new PdfPCell(new Phrase("", _BoldFont));
								cl07.Border = iTextSharp.text.Rectangle.RIGHT_BORDER;
								cl07.HorizontalAlignment = 1;								
								// Añadimos las celdas a la tabla
								tblConceptos.AddCell(cl01);
								tblConceptos.AddCell(cl02);
								tblConceptos.AddCell(cl03);
								tblConceptos.AddCell(cl04);
								tblConceptos.AddCell(cl05);
								tblConceptos.AddCell(cl06);
								tblConceptos.AddCell(cl07);															
								idgrupoproducto = (int) lector["id_grupo_producto"];								
							}else{
																								
								total_grupo_producto = 0;
								subtotal_grupo_producto = 0;
								ivatotal_grupo_producto = 0;
								total_grupo_producto += total;
								subtotal_grupo_producto += subtotal;
								ivatotal_grupo_producto += ivaprod;
							}
						}else{
							if(fcreacion != (string) lector["fechcreacion"]){
								fcreacion = (string) lector["fechcreacion"];								
							}
							// Configuramos el título de las columnas de la tabla
							cl01 = new PdfPCell(new Phrase("", _BoldFont));
							//clnroatencion.BorderWidth = 1;			// Ancho del borde
							cl01.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
							cl01.HorizontalAlignment = 1;
							cl02 = new PdfPCell(new Phrase("", _BoldFont));
							cl02.Border = iTextSharp.text.Rectangle.NO_BORDER;
							cl02.HorizontalAlignment = 1;
							cl03 = new PdfPCell(new Phrase((string) lector["descripcion_admisiones"]+" "+fcreacion, _BoldFont));
							cl03.Border = iTextSharp.text.Rectangle.NO_BORDER;
							cl03.HorizontalAlignment = 1;
							cl04 = new PdfPCell(new Phrase("", _BoldFont));
							cl04.Border = iTextSharp.text.Rectangle.NO_BORDER;
							cl04.HorizontalAlignment = 1;
							cl05 = new PdfPCell(new Phrase("", _BoldFont));
							cl05.Border = iTextSharp.text.Rectangle.NO_BORDER;
							cl05.HorizontalAlignment = 1;
							cl06 = new PdfPCell(new Phrase("", _BoldFont));
							cl06.Border = iTextSharp.text.Rectangle.NO_BORDER;
							cl06.HorizontalAlignment = 1;
							cl07 = new PdfPCell(new Phrase("", _BoldFont));
							cl07.Border = iTextSharp.text.Rectangle.RIGHT_BORDER;
							cl07.HorizontalAlignment = 1;							
							// Añadimos las celdas a la tabla
							tblConceptos.AddCell(cl01);
							tblConceptos.AddCell(cl02);
							tblConceptos.AddCell(cl03);
							tblConceptos.AddCell(cl04);
							tblConceptos.AddCell(cl05);
							tblConceptos.AddCell(cl06);
							tblConceptos.AddCell(cl07);							
							
							if (idgrupoproducto != (int) lector["id_grupo_producto"]){								
								// Configuramos el título de las columnas de la tabla
								cl01 = new PdfPCell(new Phrase("", _BoldFont));
								//clnroatencion.BorderWidth = 1;			// Ancho del borde
								cl01.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
								cl01.HorizontalAlignment = 1;
								cl02 = new PdfPCell(new Phrase("", _BoldFont));
								cl02.Border = iTextSharp.text.Rectangle.NO_BORDER;
								cl02.HorizontalAlignment = 1;
								cl03 = new PdfPCell(new Phrase((string) lector["descripcion_grupo_producto"], _BoldFont));
								cl03.Border = iTextSharp.text.Rectangle.NO_BORDER;
								cl03.HorizontalAlignment = 0;
								cl04 = new PdfPCell(new Phrase("", _BoldFont));
								cl04.Border = iTextSharp.text.Rectangle.NO_BORDER;
								cl04.HorizontalAlignment = 1;
								cl05 = new PdfPCell(new Phrase("", _BoldFont));
								cl05.Border = iTextSharp.text.Rectangle.NO_BORDER;
								cl05.HorizontalAlignment = 1;
								cl06 = new PdfPCell(new Phrase("", _BoldFont));
								cl06.Border = iTextSharp.text.Rectangle.NO_BORDER;
								cl06.HorizontalAlignment = 1;
								cl07 = new PdfPCell(new Phrase("", _BoldFont));
								cl07.Border = iTextSharp.text.Rectangle.RIGHT_BORDER;
								cl07.HorizontalAlignment = 1;								
								// Añadimos las celdas a la tabla
								tblConceptos.AddCell(cl01);
								tblConceptos.AddCell(cl02);
								tblConceptos.AddCell(cl03);
								tblConceptos.AddCell(cl04);
								tblConceptos.AddCell(cl05);
								tblConceptos.AddCell(cl06);
								tblConceptos.AddCell(cl07);
								
								idgrupoproducto = (int) lector["id_grupo_producto"];
							}
							idadmision_ = (int) lector["idadmisiones"];
						}						
						// Configuramos el título de las columnas de la tabla
						cl01 = new PdfPCell(new Phrase(lector["idproducto"].ToString().Trim(),_NormalFont));
						//clnroatencion.BorderWidth = 1;			// Ancho del borde
						cl01.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
						cl01.HorizontalAlignment = 0;		// centro
						cl02 = new PdfPCell(new Phrase(decimal.Parse(lector["cantidadaplicada"].ToString()).ToString("F"), _NormalFont));
						cl02.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
						cl02.HorizontalAlignment = 2;		// centro
						cl03 = new PdfPCell(new Phrase(lector["descripcion_producto"].ToString().Trim(), _NormalFont));
						cl03.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
						cl03.HorizontalAlignment = 0;		// centro
						cl04 = new PdfPCell(new Phrase(decimal.Parse((string) lector["preciounitario"].ToString()).ToString("F"), _NormalFont));
						cl04.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
						cl04.HorizontalAlignment = 2;		// centro
						cl05 = new PdfPCell(new Phrase(subtotal.ToString("F"), _NormalFont));
						cl05.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
						cl05.HorizontalAlignment = 2;		// centro
						cl06 = new PdfPCell(new Phrase(ivaprod.ToString("F"), _NormalFont));
						cl06.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
						cl06.HorizontalAlignment = 2;		// centro
						cl07 = new PdfPCell(new Phrase(string.Format("{0:C}",subtotal+ivaprod), _NormalFont));
						cl07.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
						cl07.HorizontalAlignment = 2;		// centro
						// Añadimos las celdas a la tabla
						tblConceptos.AddCell(cl01);
						tblConceptos.AddCell(cl02);
						tblConceptos.AddCell(cl03);
						tblConceptos.AddCell(cl04);
						tblConceptos.AddCell(cl05);
						tblConceptos.AddCell(cl06);
						tblConceptos.AddCell(cl07);
					}
					// Finalmente, añadimos las tabla al documento PDF
		            documento.Add(tblConceptos);
				}
			}catch (NpgsqlException ex){
				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
						MessageType.Warning, ButtonsType.Ok, "PostgresSQL error: {0}",ex.Message);
				msgBoxError.Run ();				msgBoxError.Destroy();
				Console.WriteLine ("PostgresSQL error: {0}",ex.Message);
			}
			conexion.Close();
		}
		
		void imprime_extras_pq(Document documento,PdfWriter writerpdf,int nro_paquete_qx_,string nombre_paquete_qx_,decimal valor_paquete_qx_)
		{
			// Los totales generales
			total_subtotal_0 = 0;
			subtotaldelmov = 0;
			total_iva = 0;
			total = 0;
			
			iTextSharp.text.Font _NormalFont;
			iTextSharp.text.Font _BoldFont;
			_NormalFont = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 7, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);
			_BoldFont = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 7, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
			
			PdfPTable tblConceptos = new PdfPTable(7);
            tblConceptos.WidthPercentage = 100;
			float[] widths2 = new float[] { 35f, 15f, 140f, 30f, 30f, 30f, 30f };	// controlando el ancho de cada columna tienen que sumas 315 en total
			tblConceptos.SetWidths(widths2);
			tblConceptos.HorizontalAlignment = 0;
			
			PdfPCell cl01;
			PdfPCell cl02;
			PdfPCell cl03;
			PdfPCell cl04;
			PdfPCell cl05;
			PdfPCell cl06;
			PdfPCell cl07;
					
			iTextSharp.text.Paragraph p = new Paragraph ("CONCEPTOS EXTRAS CARGADOS", _NormalFont);
			p.Alignment = Element.ALIGN_CENTER;										
			documento.Add (p);
			documento.Add (new Paragraph (" ", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 2)));
			//(string) lector["descripcion_admisiones"],fcreacion
			
			// Configuramos el título de las columnas de la tabla
			cl01 = new PdfPCell(new Phrase("CODIGO", _BoldFont));
			//clnroatencion.BorderWidth = 1;			// Ancho del borde
			cl01.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
			cl01.HorizontalAlignment = 1;		// centro
			cl01.BackgroundColor = BaseColor.YELLOW;
			cl02 = new PdfPCell(new Phrase("CANT.", _BoldFont));
			cl02.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
			cl02.HorizontalAlignment = 1;		// centro
			cl02.BackgroundColor = BaseColor.YELLOW;
			cl03 = new PdfPCell(new Phrase("DESCRIPCION PRODUCTO", _BoldFont));
			cl03.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
			cl03.HorizontalAlignment = 0;		// centro
			cl03.BackgroundColor = BaseColor.YELLOW;
			cl04 = new PdfPCell(new Phrase("PRECIO", _BoldFont));
			cl04.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
			cl04.HorizontalAlignment = 1;		// centro
			cl04.BackgroundColor = BaseColor.YELLOW;
			cl05 = new PdfPCell(new Phrase("SUB-TOTAL", _BoldFont));
			cl05.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
			cl05.HorizontalAlignment = 1;		// centro
			cl05.BackgroundColor = BaseColor.YELLOW;
			cl06 = new PdfPCell(new Phrase("IVA", _BoldFont));
			cl06.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
			cl06.HorizontalAlignment = 1;		// centro
			cl06.BackgroundColor = BaseColor.YELLOW;
			cl07 = new PdfPCell(new Phrase("TOTAL", _BoldFont));
			cl07.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
			cl07.HorizontalAlignment = 1;		// centro
			cl07.BackgroundColor = BaseColor.YELLOW;
			// Añadimos las celdas a la tabla
			tblConceptos.AddCell(cl01);
			tblConceptos.AddCell(cl02);
			tblConceptos.AddCell(cl03);
			tblConceptos.AddCell(cl04);
			tblConceptos.AddCell(cl05);
			tblConceptos.AddCell(cl06);
			tblConceptos.AddCell(cl07);
			
			string num_presu_paquete_folio = "id_tipo_cirugia = '"+nro_paquete_qx_+"' ";
			string nombre_tabla = "osiris_his_cirugias_deta";
			string folio_a_comparar = "id_tipo_cirugia";
			string precio_del_producto = ", to_char(osiris_productos.precio_producto_publico,'99999999,99') AS precioproducto ";
			string grupo_producto = "osiris_productos.precio_producto_publico";
			decimal calculadiferencia = 0;
			string idproducto_tabla = "";
			string toma_descripcion = "";
			decimal toma_totcantaplica = 0;
			decimal toma_precio_unitario = 0;
			subtotal = 0;
			ivaprod = 0;
			
			
			NpgsqlConnection conexion; 
			conexion = new NpgsqlConnection (connectionString+nombrebd);
			try{
				conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand ();
				comando.CommandText = comando.CommandText = "SELECT osiris_erp_cobros_deta.folio_de_servicio,to_char(osiris_erp_cobros_deta.id_producto,'999999999999') AS idproducto,osiris_productos.descripcion_producto,"+
									"to_char(SUM(cantidad_aplicada),'999999999,99') AS cantidadaplicada,SUM(cantidad_aplicada*precio_producto) AS totalprecioporcantidad,"+
									"SUM(porcentage_utilidad*cantidad_aplicada) AS porcentageutilidad,"+
									"osiris_erp_cobros_deta.precio_producto AS preciounitario,precio_costo_unitario,aplicar_iva "+
									"FROM osiris_erp_cobros_deta,osiris_productos "+
									"WHERE osiris_erp_cobros_deta.id_producto = osiris_productos.id_producto "+
									"AND osiris_erp_cobros_deta.eliminado = 'false' "+
									query_nro_atencion+
									"GROUP BY osiris_erp_cobros_deta.id_producto,osiris_productos.descripcion_producto,osiris_erp_cobros_deta.folio_de_servicio,osiris_erp_cobros_deta.precio_producto,precio_costo_unitario,aplicar_iva "+
									"ORDER BY osiris_productos.descripcion_producto;";
				//Console.WriteLine(comando.CommandText);
				NpgsqlDataReader lector = comando.ExecuteReader ();
				while(lector.Read()){					
					idproducto_tabla = (string) lector["idproducto"];
					toma_totcantaplica = decimal.Parse(lector["cantidadaplicada"].ToString().Trim());
					toma_descripcion = (string) lector["descripcion_producto"];
					toma_precio_unitario = decimal.Parse(lector["preciounitario"].ToString().Trim());
										
					NpgsqlConnection conexion1; 
					conexion1 = new NpgsqlConnection (connectionString+nombrebd);
					try{
						conexion1.Open ();
						NpgsqlCommand comando1; 
						comando1 = conexion1.CreateCommand ();
						comando1.CommandText = "SELECT "+folio_a_comparar+",to_char("+nombre_tabla+".id_producto,'999999999999') AS idproducto,osiris_productos.descripcion_producto,"+
											"to_char(SUM(cantidad_aplicada),'999999999,99') AS cantidadaplicada,"+
								            "aplicar_iva "+
								             precio_del_producto+" "+
											"FROM osiris_productos,"+nombre_tabla+" "+
											"WHERE "+num_presu_paquete_folio+" "+
											"AND "+nombre_tabla+".id_producto = osiris_productos.id_producto "+
											"AND "+nombre_tabla+".eliminado = 'false' "+
											"AND "+nombre_tabla+".id_producto = '"+idproducto_tabla+"' "+
											"GROUP BY "+nombre_tabla+".id_producto,"+grupo_producto+",aplicar_iva,osiris_productos.descripcion_producto,"+folio_a_comparar+";";
						//Console.WriteLine(comando1.CommandText);
						//Console.WriteLine(nombre_tabla);
						NpgsqlDataReader lector1 = comando1.ExecuteReader();						
						if(lector1.Read()){							
							calculadiferencia = toma_totcantaplica - decimal.Parse((string) lector1["cantidadaplicada"]);
							if(calculadiferencia > 0){
								subtotal = calculadiferencia * toma_precio_unitario;
								ivaprod = 0;
								if(nro_paquete_qx_ > 1){
									if((bool) lector1["aplicar_iva"] == true) {
										ivaprod = ( subtotal * PorcentIVA ) / 100;
										subtotal_impuesto += ivaprod;
									}else{
										total_subtotal_0 += subtotal;
										ivaprod = 0;
									}
								}else{
									subtotal = 0;
									ivaprod = 0;
									toma_precio_unitario = 0;
								}
								subtotaldelmov += subtotal;
								total_iva += ivaprod;
								total = subtotal + ivaprod;
								
								cl01 = new PdfPCell(new Phrase(lector["idproducto"].ToString().Trim(),_NormalFont));
								//clnroatencion.BorderWidth = 1;			// Ancho del borde
								cl01.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
								cl01.HorizontalAlignment = 0;		// centro
								cl02 = new PdfPCell(new Phrase(calculadiferencia.ToString("F"), _NormalFont));
								cl02.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
								cl02.HorizontalAlignment = 2;		// centro
								cl03 = new PdfPCell(new Phrase(lector["descripcion_producto"].ToString().Trim(), _NormalFont));
								cl03.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
								cl03.HorizontalAlignment = 0;		// centro
								cl04 = new PdfPCell(new Phrase(toma_precio_unitario.ToString("C"), _NormalFont));
								cl04.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
								cl04.HorizontalAlignment = 2;		// centro
								cl05 = new PdfPCell(new Phrase(subtotal.ToString("C"), _NormalFont));
								cl05.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
								cl05.HorizontalAlignment = 2;		// centro
								cl06 = new PdfPCell(new Phrase(ivaprod.ToString("C"), _NormalFont));
								cl06.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
								cl06.HorizontalAlignment = 2;		// centro
								cl07 = new PdfPCell(new Phrase(string.Format("{0:C}",subtotal+ivaprod), _NormalFont));
								cl07.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
								cl07.HorizontalAlignment = 2;		// centro
								// Añadimos las celdas a la tabla
								tblConceptos.AddCell(cl01);
								tblConceptos.AddCell(cl02);
								tblConceptos.AddCell(cl03);
								tblConceptos.AddCell(cl04);
								tblConceptos.AddCell(cl05);
								tblConceptos.AddCell(cl06);
								tblConceptos.AddCell(cl07);								
							}
						}else{
							subtotal = decimal.Parse((string) lector["cantidadaplicada"].ToString()) * decimal.Parse((string) lector["preciounitario"].ToString());
							toma_precio_unitario = decimal.Parse((string) lector["preciounitario"].ToString());
							ivaprod = 0;
							if(nro_paquete_qx_ > 1){
								if((bool) lector["aplicar_iva"]== true) {
									ivaprod = (subtotal * PorcentIVA) / 100;
									subtotal_impuesto += subtotal;
								}else{
									total_subtotal_0 += subtotal;
									ivaprod = 0;
								}
							}else{
								subtotal = 0;
								ivaprod = 0;
								toma_precio_unitario = 0;
							}																
							subtotaldelmov += subtotal;
							total_iva += ivaprod;
							total = subtotaldelmov + total_iva;
							
							// (Extra) No esta en el Paquete							
							cl01 = new PdfPCell(new Phrase(lector["idproducto"].ToString().Trim(),_NormalFont));
							//clnroatencion.BorderWidth = 1;			// Ancho del borde
							cl01.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
							cl01.HorizontalAlignment = 0;		// centro
							cl02 = new PdfPCell(new Phrase(toma_totcantaplica.ToString("F"), _NormalFont));
							cl02.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
							cl02.HorizontalAlignment = 2;		// centro
							cl03 = new PdfPCell(new Phrase(lector["descripcion_producto"].ToString().Trim(), _NormalFont));
							cl03.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
							cl03.HorizontalAlignment = 0;		// centro
							cl04 = new PdfPCell(new Phrase(toma_precio_unitario.ToString("C"), _NormalFont));
							cl04.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
							cl04.HorizontalAlignment = 2;		// centro
							cl05 = new PdfPCell(new Phrase(subtotal.ToString("C"), _NormalFont));
							cl05.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
							cl05.HorizontalAlignment = 2;		// centro
							cl06 = new PdfPCell(new Phrase(ivaprod.ToString("C"), _NormalFont));
							cl06.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
							cl06.HorizontalAlignment = 2;		// centro
							cl07 = new PdfPCell(new Phrase(string.Format("{0:C}",subtotal+ivaprod), _NormalFont));
							cl07.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
							cl07.HorizontalAlignment = 2;		// centro
							// Añadimos las celdas a la tabla
							tblConceptos.AddCell(cl01);
							tblConceptos.AddCell(cl02);
							tblConceptos.AddCell(cl03);
							tblConceptos.AddCell(cl04);
							tblConceptos.AddCell(cl05);
							tblConceptos.AddCell(cl06);
							tblConceptos.AddCell(cl07);
						}
					}catch (NpgsqlException ex){
				   		Console.WriteLine ("PostgresSQL error: {0}",ex.Message);
				   		MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
						MessageType.Error,ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
						msgBoxError.Run ();			msgBoxError.Destroy();
					}
					conexion1.Close ();				
				}
				// Configuramos el título de las columnas de la tabla
				cl01 = new PdfPCell(new Phrase("",_NormalFont));
				//clnroatencion.BorderWidth = 1;			// Ancho del borde
				cl01.Border = iTextSharp.text.Rectangle.NO_BORDER;
				cl01.HorizontalAlignment = 0;
				cl02 = new PdfPCell(new Phrase("", _NormalFont));
				cl02.Border = iTextSharp.text.Rectangle.NO_BORDER;
				cl02.HorizontalAlignment = 2;
				cl03 = new PdfPCell(new Phrase("", _NormalFont));
				cl03.Border = iTextSharp.text.Rectangle.NO_BORDER;
				cl03.HorizontalAlignment = 0;
				cl04 = new PdfPCell(new Phrase("Extras c/iva", _NormalFont));
				cl04.Border = iTextSharp.text.Rectangle.NO_BORDER;
				cl04.HorizontalAlignment = 2;
				cl05 = new PdfPCell(new Phrase(subtotal_impuesto.ToString("C"), _NormalFont));
				cl05.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
				cl05.HorizontalAlignment = 2;
				cl06 = new PdfPCell(new Phrase(total_iva.ToString("C"), _NormalFont));
				cl06.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
				cl06.HorizontalAlignment = 2;
				cl07 = new PdfPCell(new Phrase(string.Format("{0:C}",subtotal_impuesto+total_iva), _NormalFont));
				cl07.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
				cl07.HorizontalAlignment = 2;
				// Añadimos las celdas a la tabla
				tblConceptos.AddCell(cl01);
				tblConceptos.AddCell(cl02);
				tblConceptos.AddCell(cl03);
				tblConceptos.AddCell(cl04);
				tblConceptos.AddCell(cl05);
				tblConceptos.AddCell(cl06);
				tblConceptos.AddCell(cl07);
				
				// Configuramos el título de las columnas de la tabla
				cl01 = new PdfPCell(new Phrase("",_NormalFont));
				//clnroatencion.BorderWidth = 1;			// Ancho del borde
				cl01.Border = iTextSharp.text.Rectangle.NO_BORDER;
				cl01.HorizontalAlignment = 0;
				cl02 = new PdfPCell(new Phrase("", _NormalFont));
				cl02.Border = iTextSharp.text.Rectangle.NO_BORDER;
				cl02.HorizontalAlignment = 2;
				cl03 = new PdfPCell(new Phrase("", _NormalFont));
				cl03.Border = iTextSharp.text.Rectangle.NO_BORDER;
				cl03.HorizontalAlignment = 0;
				cl04 = new PdfPCell(new Phrase("Extras sin/iva", _NormalFont));
				cl04.Border = iTextSharp.text.Rectangle.NO_BORDER;
				cl04.HorizontalAlignment = 2;
				cl05 = new PdfPCell(new Phrase(total_subtotal_0.ToString("C"), _NormalFont));
				cl05.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
				cl05.HorizontalAlignment = 2;
				cl06 = new PdfPCell(new Phrase("0.00", _NormalFont));
				cl06.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
				cl06.HorizontalAlignment = 2;
				cl07 = new PdfPCell(new Phrase(total_subtotal_0.ToString("C"), _NormalFont));
				cl07.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
				cl07.HorizontalAlignment = 2;
				// Añadimos las celdas a la tabla
				tblConceptos.AddCell(cl01);
				tblConceptos.AddCell(cl02);
				tblConceptos.AddCell(cl03);
				tblConceptos.AddCell(cl04);
				tblConceptos.AddCell(cl05);
				tblConceptos.AddCell(cl06);
				tblConceptos.AddCell(cl07);
				
				// Configuramos el título de las columnas de la tabla
				cl01 = new PdfPCell(new Phrase("",_NormalFont));
				//clnroatencion.BorderWidth = 1;			// Ancho del borde
				cl01.Border = iTextSharp.text.Rectangle.NO_BORDER;
				cl01.HorizontalAlignment = 0;
				cl02 = new PdfPCell(new Phrase("", _NormalFont));
				cl02.Border = iTextSharp.text.Rectangle.NO_BORDER;
				cl02.HorizontalAlignment = 2;
				cl03 = new PdfPCell(new Phrase("", _NormalFont));
				cl03.Border = iTextSharp.text.Rectangle.NO_BORDER;
				cl03.HorizontalAlignment = 0;
				cl04 = new PdfPCell(new Phrase("TOT.EXTRAS", _NormalFont));
				cl04.Border = iTextSharp.text.Rectangle.NO_BORDER;
				cl04.HorizontalAlignment = 2;
				cl05 = new PdfPCell(new Phrase(subtotaldelmov.ToString("C"), _NormalFont));
				cl05.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
				cl05.HorizontalAlignment = 2;
				cl06 = new PdfPCell(new Phrase(total_iva.ToString("C"), _NormalFont));
				cl06.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
				cl06.HorizontalAlignment = 2;
				cl07 = new PdfPCell(new Phrase(string.Format("{0:C}",subtotaldelmov+total_iva), _NormalFont));
				cl07.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
				cl07.HorizontalAlignment = 2;
				// Añadimos las celdas a la tabla
				tblConceptos.AddCell(cl01);
				tblConceptos.AddCell(cl02);
				tblConceptos.AddCell(cl03);
				tblConceptos.AddCell(cl04);
				tblConceptos.AddCell(cl05);
				tblConceptos.AddCell(cl06);
				tblConceptos.AddCell(cl07);

				documento.Add(tblConceptos);

				documento.Add (new Paragraph (" ", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 5)));
				documento.Add (new Paragraph (" ", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 5)));
			}catch (NpgsqlException ex){
		   		Console.WriteLine ("PostgresSQL error: {0}",ex.Message);
		   		MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
				MessageType.Error,ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
				msgBoxError.Run ();			msgBoxError.Destroy();
			}
			conexion.Close ();
		}

		string llenado_ticket_caja(string folioservicio_)
		{
			string cadena_mov_doc = "";
			NpgsqlConnection conexion1; 
			conexion1 = new NpgsqlConnection (connectionString+nombrebd);
	        try{
				conexion1.Open ();
				NpgsqlCommand comando1; 
				comando1 = conexion1.CreateCommand ();
				comando1.CommandText = "SELECT * FROM osiris_erp_abonos,osiris_erp_tipo_comprobante " +
					"WHERE osiris_erp_abonos.id_tipo_comprobante = osiris_erp_tipo_comprobante.id_tipo_comprobante " +
					"AND eliminado = 'false' " +
					"AND osiris_erp_tipo_comprobante.docu_por_facturar = 'false' " +
					"AND folio_de_servicio = '"+folioservicio_+"' " +
					"ORDER BY numero_recibo_caja;";
				NpgsqlDataReader lector1 = comando1.ExecuteReader ();
				while (lector1.Read()){
					cadena_mov_doc += lector1["numero_recibo_caja"].ToString().Trim()+"/";	
				}
				return cadena_mov_doc;
			}catch(NpgsqlException ex){
				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
									MessageType.Error,ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
				msgBoxError.Run ();					msgBoxError.Destroy();
	   		}
			conexion1.Close ();
			return cadena_mov_doc;
		}
		
		string llenado_doc_x_facturar(string folioservicio_)
		{
			string cadena_mov_doc = "";
			NpgsqlConnection conexion1; 
			conexion1 = new NpgsqlConnection (connectionString+nombrebd);
	        try{
				conexion1.Open ();
				NpgsqlCommand comando1; 
				comando1 = conexion1.CreateCommand ();
				comando1.CommandText = comando1.CommandText = "SELECT * FROM osiris_erp_abonos,osiris_erp_tipo_comprobante " +
					"WHERE osiris_erp_abonos.id_tipo_comprobante = osiris_erp_tipo_comprobante.id_tipo_comprobante " +
					"AND eliminado = 'false' " +
					"AND osiris_erp_tipo_comprobante.docu_por_facturar = 'true' " +
					"AND folio_de_servicio = '"+folioservicio_+"' " +
					"ORDER BY numero_recibo_caja;";
				NpgsqlDataReader lector1 = comando1.ExecuteReader ();
				while (lector1.Read()){
					cadena_mov_doc += lector1["numero_recibo_caja"].ToString().Trim()+"/";	
				}
				return cadena_mov_doc;
			}catch(NpgsqlException ex){
				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
									MessageType.Error,ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
				msgBoxError.Run ();					msgBoxError.Destroy();
	   		}
			conexion1.Close ();
			return cadena_mov_doc;
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
							
		private class EventoTitulos : PdfPageEventHelper
		{
			class_public classpublic = new class_public();
									
			#region Fields
			private string _titulo1_rpt;
			private string _numero_atencion_px;
			private string _fecha_ingresso_px;
			private string _fecha_egresso_px;
			private string _nro_expediente_px;
			private string _nombres_apellidos_px;
			private string _fecha_nacimiento_px;
			private string _edad_px;
			private string _direccion_px;
			private string _telefono_px;
			private string _tipo_paciente_px;
			private string _convenio_px;
			private string _medico_tratante_px;
			private string _especia_medtrat_px;
			private string _habitacion_px;
			private string _doc_convenio_px;
			private string _motivoingreso_px;
			private string _nro_ticket_px;
			private string _doc_x_facturar_px;
			private string _observacion_ingreso;
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
			public string numero_atencion_px
        	{
            	get{
					return _numero_atencion_px;
				}
            	set{
					_numero_atencion_px = value;
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
			public string fecha_egresso_px
        	{
            	get{
					return _fecha_egresso_px;
				}
            	set{
					_fecha_egresso_px = value;
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
			public string fecha_nacimiento_px
        	{
            	get{
					return _fecha_nacimiento_px;
				}
            	set{
					_fecha_nacimiento_px = value;
				}
        	}			
			public string edad_px
        	{
            	get{
					return _edad_px;
				}
            	set{
					_edad_px = value;
				}
        	}			
			public string direccion_px
        	{
            	get{
					return _direccion_px;
				}
            	set{
					_direccion_px = value;
				}
        	}
			public string telefono_px
        	{
            	get{
					return _telefono_px;
				}
            	set{
					_telefono_px = value;
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
			public string medico_tratante_px
        	{
            	get{
					return _medico_tratante_px;
				}
            	set{
					_medico_tratante_px = value;
				}
        	}
			public string especia_medtrat_px
        	{
            	get{
					return _especia_medtrat_px;
				}
            	set{
					_especia_medtrat_px = value;
				}
        	}
			public string habitacion_px
        	{
            	get{
					return _habitacion_px;
				}
            	set{
					_habitacion_px = value;
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
			public string motivoingreso_px
        	{
            	get{
					return _motivoingreso_px;
				}
            	set{
					_motivoingreso_px = value;
				}
        	}
			public string nro_ticket_px
        	{
            	get{
					return _nro_ticket_px;
				}
            	set{
					_nro_ticket_px = value;
				}
        	}
			public string doc_x_facturar_px
        	{
            	get{
					return _doc_x_facturar_px;
				}
            	set{
					_doc_x_facturar_px = value;
				}
        	}
			public string observacion_ingreso
			{
				get{
					return _observacion_ingreso;
				}
				set{
					_observacion_ingreso = value;
				}
			}
			#endregion
			
		    public override void OnOpenDocument(PdfWriter writerpdf, Document documento)
		    {
					
		    }
		
		    public override void OnStartPage(PdfWriter writerpdf, Document documento)
		    {		
		        iTextSharp.text.pdf.BaseFont bf = BaseFont.CreateFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
				iTextSharp.text.Rectangle pageSize = documento.PageSize;
				
				// Creamos la imagen y le ajustamos el tamaño
				//iTextSharp.text.Image imagen = iTextSharp.text.Image.GetInstance("/opt/osiris/bin/OSIRISLogo2.png");
				iTextSharp.text.Image imagen = iTextSharp.text.Image.GetInstance(System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory,"OSIRISLogo2.png"));
				
				imagen.BorderWidth = 0;
				imagen.Alignment = Element.ALIGN_LEFT;
				float percentage = 0.0f;
				percentage = 150 / imagen.Width;
				imagen.ScalePercent(percentage * 65);
				
				//Insertamos la imagen en el documento
				documento.Add(imagen);
				
				
				PdfContentByte cb = writerpdf.DirectContent;
				// we tell the ContentByte we're ready to draw text
				cb.BeginText ();
				cb.SetFontAndSize (bf, 9);
				cb.SetTextMatrix (130, 750);			cb.ShowText (classpublic.nombre_empresa);
				cb.SetTextMatrix (130, 740);			cb.ShowText (classpublic.direccion_empresa);
				cb.SetTextMatrix (130, 730);			cb.ShowText (classpublic.telefonofax_empresa);
				cb.SetTextMatrix (500,725);			cb.ShowText ("N° DE ATENCION");
				cb.SetColorFill(iTextSharp.text.BaseColor.RED);
                cb.SetFontAndSize(bf, 11);
				cb.SetTextMatrix (510,710);			cb.ShowText (numero_atencion_px);
				cb.SetColorFill(iTextSharp.text.BaseColor.BLACK);
				cb.SetFontAndSize (bf, 6);
				cb.SetTextMatrix (130, 720);			cb.ShowText ("Sistema Hospitalario OSIRIS");
				cb.SetTextMatrix (500, 750);		cb.ShowText ("Fech.Rpt:" + (string)DateTime.Now.ToString ("yyyy-MM-dd HH:mm:ss"));
				cb.SetTextMatrix (500, 740);		cb.ShowText ("N° Page :"+writerpdf.PageNumber.ToString("D8"));
				cb.EndText ();
				
				documento.Add (new Paragraph (" ", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 8)));
												
				Paragraph titulo1_reporte = new Paragraph(titulo1_rpt, new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 12, iTextSharp.text.Font.BOLD, BaseColor.BLACK));
                titulo1_reporte.Alignment = Element.ALIGN_CENTER;
                documento.Add(titulo1_reporte);
				documento.Add (new Paragraph (" ", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 8)));
								
				// fuente para las tablas creadas
				iTextSharp.text.Font _NormalFont;
				iTextSharp.text.Font _BoldFont;
				_NormalFont = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 7, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);
				_BoldFont = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 7, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
				
				//Paragraph titulo2_reporte = new Paragraph("INFORMACION DE INGRESO DEL PACIENTE", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 9, iTextSharp.text.Font.BOLD, BaseColor.BLACK));
                //titulo2_reporte.Alignment = Element.ALIGN_CENTER;
                //documento.Add(titulo2_reporte);
				
				PdfPTable tabsubtitulo = new PdfPTable(1);
				tabsubtitulo.WidthPercentage = 36.0f;
				float[] widths_tabsubtit = new float[] { 1f };	// controlando el ancho de cada columna tienen que sumas 315 en total
				tabsubtitulo.SetWidths(widths_tabsubtit);
				tabsubtitulo.HorizontalAlignment = 1;
								
				PdfPCell cellsubtit_col1;
				cellsubtit_col1 = new PdfPCell(new Phrase("INFORMACION DE INGRESO DEL PACIENTE",new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 9, iTextSharp.text.Font.BOLD, BaseColor.BLACK)));
				cellsubtit_col1.Border = PdfPCell.NO_BORDER;
				cellsubtit_col1.CellEvent = new RoundedBorder();
				cellsubtit_col1.HorizontalAlignment = 1;
				tabsubtitulo.AddCell(cellsubtit_col1);
				documento.Add(tabsubtitulo);
				
				documento.Add (new Paragraph (" ", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 6)));
				
				PdfPCell cellcol1;
				PdfPCell cellcol3;
				PdfPCell cellcol2;
				PdfPCell cellcol4;
				PdfPCell cellcol5;
				PdfPCell cellcol6;
				PdfPCell cellcol7;
				PdfPCell cellcol8;
				
				/*************LINEA 1*******************/
				PdfPTable tabFot1 = new PdfPTable(4);
				tabFot1.WidthPercentage = 100;
				float[] widths_tabfot1 = new float[] { 25f, 137.5f, 117.5f, 40f };	// controlando el ancho de cada columna tienen que sumas 315 en total
				tabFot1.SetWidths(widths_tabfot1);
				tabFot1.HorizontalAlignment = 0;
				cellcol1 = new PdfPCell(new Phrase("INGRESO",_BoldFont));
				cellcol1.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER ;
				cellcol2 = new PdfPCell(new Phrase(fecha_ingresso_px,_NormalFont));
				cellcol2.Border = iTextSharp.text.Rectangle.TOP_BORDER ;
				cellcol3 = new PdfPCell(new Phrase("EGRESO",_BoldFont));
				cellcol3.HorizontalAlignment = 2;		// derecha		
				cellcol3.Border = iTextSharp.text.Rectangle.TOP_BORDER ;
				cellcol4 = new PdfPCell(new Phrase(fecha_egresso_px,_NormalFont));
				cellcol4.Border = iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
				tabFot1.AddCell(cellcol1);
				tabFot1.AddCell(cellcol2);
				tabFot1.AddCell(cellcol3);
				tabFot1.AddCell(cellcol4);				
				documento.Add(tabFot1);
				
				/**************LINEA 2******************/
				PdfPTable tabFot2 = new PdfPTable(8);
				tabFot2.WidthPercentage = 100;
				float[] widths_tabfot2 = new float[] { 25f, 25f, 30f, 90f, 30f, 35f, 30f, 50f };	// controlando el ancho de cada columna tienen que sumas 315 en total
				tabFot2.SetWidths(widths_tabfot2);
				tabFot2.HorizontalAlignment = 0;
				cellcol1 = new PdfPCell(new Phrase("N° EXP.",_BoldFont));
				cellcol1.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
				cellcol2 = new PdfPCell(new Phrase(nro_expediente_px,_NormalFont));
				cellcol2.Border = PdfPCell.NO_BORDER;
				cellcol2.HorizontalAlignment = 0;
				cellcol3 = new PdfPCell(new Phrase("Nombre PX.",_BoldFont));
				cellcol3.Border = PdfPCell.NO_BORDER;
				cellcol4 = new PdfPCell(new Phrase(nombres_apellidos_px,_NormalFont));
				cellcol4.Border = PdfPCell.NO_BORDER;
				cellcol4.HorizontalAlignment = 0;
				cellcol5 = new PdfPCell(new Phrase("Fech.Nac.",_BoldFont));
				cellcol5.Border = PdfPCell.NO_BORDER;
				cellcol5.HorizontalAlignment = 2;
				cellcol6 = new PdfPCell(new Phrase(fecha_nacimiento_px,_NormalFont));
				cellcol6.Border = PdfPCell.NO_BORDER;
				cellcol6.HorizontalAlignment = 0;
				cellcol7 = new PdfPCell(new Phrase("Edad",_BoldFont));
				cellcol7.Border = PdfPCell.NO_BORDER;
				cellcol7.HorizontalAlignment = 2;
				cellcol8 = new PdfPCell(new Phrase(edad_px,_NormalFont));
				cellcol8.Border = iTextSharp.text.Rectangle.RIGHT_BORDER;
				cellcol8.HorizontalAlignment = 0;
				tabFot2.AddCell(cellcol1);
				tabFot2.AddCell(cellcol2);
				tabFot2.AddCell(cellcol3);
				tabFot2.AddCell(cellcol4);
				tabFot2.AddCell(cellcol5);
				tabFot2.AddCell(cellcol6);
				tabFot2.AddCell(cellcol7);
				tabFot2.AddCell(cellcol8);
				documento.Add(tabFot2);
				
				/**************LINEA 3******************/
				PdfPTable tabFot3 = new PdfPTable(4);
				tabFot3.WidthPercentage = 100;
				float[] widths_tabfot3 = new float[] { 25f, 200f, 40f, 45f };	// controlando el ancho de cada columna tienen que sumas 315 en total
				tabFot3.SetWidths(widths_tabfot3);
				tabFot3.HorizontalAlignment = 0;
				cellcol1 = new PdfPCell(new Phrase("Direccion ",_BoldFont));
				cellcol1.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
				cellcol2 = new PdfPCell(new Phrase(direccion_px,_NormalFont));
				cellcol2.Border = PdfPCell.NO_BORDER;
				cellcol2.HorizontalAlignment = 0;
				cellcol3 = new PdfPCell(new Phrase("Telefono",_BoldFont));
				cellcol3.HorizontalAlignment = 2;		// derecha		
				cellcol3.Border = PdfPCell.NO_BORDER;
				cellcol4 = new PdfPCell(new Phrase(telefono_px,_NormalFont));
				cellcol4.Border = iTextSharp.text.Rectangle.RIGHT_BORDER;
				cellcol4.HorizontalAlignment = 0;
				tabFot3.AddCell(cellcol1);
				tabFot3.AddCell(cellcol2);
				tabFot3.AddCell(cellcol3);
				tabFot3.AddCell(cellcol4);
				documento.Add(tabFot3);
				
				/**************LINEA 4******************/
				PdfPTable tabFot4 = new PdfPTable(4);
				tabFot4.WidthPercentage = 100;
				float[] widths_tabfot4 = new float[] { 30f, 50f, 50f, 100f };	// controlando el ancho de cada columna tienen que sumas 315 en total
				tabFot4.SetWidths(widths_tabfot4);
				tabFot4.HorizontalAlignment = 0;
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
				tabFot4.AddCell(cellcol1);
				tabFot4.AddCell(cellcol2);
				tabFot4.AddCell(cellcol3);
				tabFot4.AddCell(cellcol4);
				documento.Add(tabFot4);
				
				/**************LINEA 5******************/
				PdfPTable tabFot5 = new PdfPTable(4);
				tabFot5.WidthPercentage = 100;
				float[] widths_tabfot5 = new float[] { 40f, 120f, 40f, 100f };	// controlando el ancho de cada columna tienen que sumas 315 en total
				tabFot5.SetWidths(widths_tabfot5);
				tabFot5.HorizontalAlignment = 0;
				cellcol1 = new PdfPCell(new Phrase("Medico Tratante",_BoldFont));
				cellcol1.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
				cellcol2 = new PdfPCell(new Phrase(medico_tratante_px,_NormalFont));
				cellcol2.Border = PdfPCell.NO_BORDER;
				cellcol2.HorizontalAlignment = 0;
				cellcol3 = new PdfPCell(new Phrase("Especialidad",_BoldFont));
				cellcol3.HorizontalAlignment = 2;		// derecha		
				cellcol3.Border = PdfPCell.NO_BORDER;
				cellcol4 = new PdfPCell(new Phrase(especia_medtrat_px,_NormalFont));
				cellcol4.Border = iTextSharp.text.Rectangle.RIGHT_BORDER;
				cellcol4.HorizontalAlignment = 0;
				tabFot5.AddCell(cellcol1);
				tabFot5.AddCell(cellcol2);
				tabFot5.AddCell(cellcol3);
				tabFot5.AddCell(cellcol4);
				documento.Add(tabFot5);
				
				/**************LINEA 6******************/
				PdfPTable tabFot6 = new PdfPTable(4);
				tabFot6.WidthPercentage = 100;
				float[] widths_tabfot6 = new float[] { 40f, 120f, 40f, 100f };	// controlando el ancho de cada columna tienen que sumas 315 en total
				tabFot6.SetWidths(widths_tabfot6);
				tabFot6.HorizontalAlignment = 0;
				cellcol1 = new PdfPCell(new Phrase("N° Habitacion",_BoldFont));
				cellcol1.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
				cellcol2 = new PdfPCell(new Phrase(habitacion_px,_NormalFont));
				cellcol2.Border = PdfPCell.NO_BORDER;
				cellcol2.HorizontalAlignment = 0;
				cellcol3 = new PdfPCell(new Phrase("N° Nomina",_BoldFont));
				cellcol3.HorizontalAlignment = 2;		// derecha		
				cellcol3.Border = PdfPCell.NO_BORDER;
				cellcol4 = new PdfPCell(new Phrase(doc_convenio_px,_NormalFont));
				cellcol4.Border = iTextSharp.text.Rectangle.RIGHT_BORDER;
				cellcol4.HorizontalAlignment = 0;
				tabFot6.AddCell(cellcol1);
				tabFot6.AddCell(cellcol2);
				tabFot6.AddCell(cellcol3);
				tabFot6.AddCell(cellcol4);
				documento.Add(tabFot6);
				
				/**************LINEA 7******************/
				PdfPTable tabFot7 = new PdfPTable(2);
				tabFot7.WidthPercentage = 100;
				float[] widths_tabfot7 = new float[] { 40f, 260f };	// controlando el ancho de cada columna tienen que sumas 315 en total
				tabFot7.SetWidths(widths_tabfot7);
				tabFot7.HorizontalAlignment = 0;
				cellcol1 = new PdfPCell(new Phrase("Motivo de Ingreso",_BoldFont));
				cellcol1.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
				cellcol2 = new PdfPCell(new Phrase(motivoingreso_px,_NormalFont));
				cellcol2.Border = iTextSharp.text.Rectangle.RIGHT_BORDER;
				cellcol2.HorizontalAlignment = 0;
				tabFot7.AddCell(cellcol1);
				tabFot7.AddCell(cellcol2);
				documento.Add(tabFot7);
				
				/**************LINEA 8******************/
				PdfPTable tabFot8 = new PdfPTable(4);
				tabFot8.WidthPercentage = 100;
				float[] widths_tabfot8 = new float[] { 40f, 120f, 40f, 100f };	// controlando el ancho de cada columna tienen que sumas 315 en total
				tabFot8.SetWidths(widths_tabfot8);
				tabFot8.HorizontalAlignment = 0;
				cellcol1 = new PdfPCell(new Phrase("N° Ticket",_BoldFont));
				cellcol1.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
				cellcol2 = new PdfPCell(new Phrase(nro_ticket_px,_NormalFont));
				cellcol2.Border = iTextSharp.text.Rectangle.NO_BORDER;
				cellcol2.HorizontalAlignment = 0;
				cellcol3 = new PdfPCell(new Phrase("Doc. X Fact.",_BoldFont));
				cellcol3.HorizontalAlignment = 2;		// derecha		
				cellcol3.Border = iTextSharp.text.Rectangle.NO_BORDER;
				cellcol4 = new PdfPCell(new Phrase(doc_x_facturar_px,_NormalFont));
				cellcol4.Border = iTextSharp.text.Rectangle.RIGHT_BORDER;
				cellcol4.HorizontalAlignment = 0;
				tabFot8.AddCell(cellcol1);
				tabFot8.AddCell(cellcol2);
				tabFot8.AddCell(cellcol3);
				tabFot8.AddCell(cellcol4);
				documento.Add(tabFot8);
				
				PdfPTable tabFot9 = new PdfPTable(2);
				tabFot9.WidthPercentage = 100;
				float[] widths_tabfot9 = new float[] { 40f, 260f };	// controlando el ancho de cada columna tienen que sumas 315 en total
				tabFot9.SetWidths(widths_tabfot9);
				tabFot9.HorizontalAlignment = 0;
				cellcol1 = new PdfPCell(new Phrase("Obser. de Ingreso",_BoldFont));
				cellcol1.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER;
				cellcol2 = new PdfPCell(new Phrase(observacion_ingreso,_NormalFont));
				cellcol2.Border = iTextSharp.text.Rectangle.RIGHT_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER;
				cellcol2.HorizontalAlignment = 0;
				tabFot9.AddCell(cellcol1);
				tabFot9.AddCell(cellcol2);
				documento.Add(tabFot9);
				
				documento.Add (new Paragraph (" ", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 5)));
				documento.Add (new Paragraph (" ", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 5)));
		    }
		
		    public override void OnEndPage(PdfWriter writerpdf, Document documento)
		    {
				
		    }
		}
		
		class RoundedBorder : IPdfPCellEvent {
			public void CellLayout(PdfPCell cell, iTextSharp.text.Rectangle rect, PdfContentByte[] canvas) {
				PdfContentByte cb = canvas[PdfPTable.BACKGROUNDCANVAS];
				cb.RoundRectangle(
					rect.Left,
          			rect.Bottom,
          			rect.Width,
          			rect.Height,				
					3);
				cb.SetLineWidth(0.7f);
				cb.Stroke();
			}
		}
	}
}