//
//  rpt_admisiones.cs
//
//  Author:
//       dolivares <>
//
//  Copyright (c) 2016 dolivares
//
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.
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
	public class rpt_admisiones
	{
		string connectionString;
		string nombrebd;

		string query_reporte = "SELECT DISTINCT ON (osiris_erp_movcargos.folio_de_servicio) osiris_erp_movcargos.folio_de_servicio,"+
			"osiris_erp_movcargos.id_tipo_admisiones AS idtipoadmisiones,osiris_his_tipo_admisiones.descripcion_admisiones, "+
			"osiris_erp_movcargos.folio_de_servicio_dep,osiris_erp_cobros_enca.cancelado, "+
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
			"id_empleado_admision,cerrado," +
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
			"WHERE "+
			"osiris_erp_movcargos.pid_paciente = osiris_his_paciente.pid_paciente "+
			"AND osiris_erp_cobros_enca.pid_paciente = osiris_his_paciente.pid_paciente "+
			"AND osiris_erp_cobros_enca.folio_de_servicio = osiris_erp_movcargos.folio_de_servicio "+
			"AND osiris_erp_movcargos.id_tipo_paciente = osiris_his_tipo_pacientes.id_tipo_paciente "+
			"AND osiris_erp_movcargos.id_tipo_admisiones = osiris_his_tipo_admisiones.id_tipo_admisiones "+
			"AND osiris_aseguradoras.id_aseguradora = osiris_erp_cobros_enca.id_aseguradora "+
			"AND osiris_empresas.id_empresa = osiris_erp_cobros_enca.id_empresa "+
			//"AND osiris_empresas.id_empresa = osiris_his_paciente.id_empresa "+  // enlase empresa con el paciente
			//"AND osiris_erp_factura_enca.numero_factura = osiris_erp_cobros_enca.numero_factura "+
			//"AND osiris_empresas.id_empresa = osiris_his_medicos.id_empresa "+
			//"AND osiris_empresas.id_empresa = 3 "+//se aactiva para cuando se quiera ver san nicolas
			"AND osiris_erp_movcargos.id_tipo_cirugia = osiris_his_tipo_cirugias.id_tipo_cirugia "+
			"AND osiris_erp_cobros_enca.id_medico = osiris_his_medicos.id_medico ";

		//Declaracion de ventana de error
		protected Gtk.Window MyWinError;

		class_conexion conexion_a_DB = new class_conexion();
		class_buscador classfind_data = new class_buscador();
		class_public classpublic = new class_public();

		public rpt_admisiones (string[] parametros,string query_filtros_,string query_orden_,bool incluir_status)
		{
			connectionString = conexion_a_DB._url_servidor+conexion_a_DB._port_DB+conexion_a_DB._usuario_DB+conexion_a_DB._passwrd_user_DB;
			nombrebd = conexion_a_DB._nombrebd;
			int contador_primeravez = 0;
			int total_de_admisiones = 0;
			string edad;
			string descri_empresa_aseguradora = "";
			string primeravez = "";
			string movdocumentos = "";
			//string query_orden = "ORDER BY osiris_erp_movcargos.folio_de_servicio;";
			//"AM=Alta Medica   Ps=Pase Serv. Med.   CS=Compr.Serv.   RC=Registro Caja   Ce=Cerrado   Fa=Facturado"
			string altamedica_px = "";
			string paseservicio_px = "";
			string comprservicio_px = "";
			string regitroccaja_px = "";
			string cerrado_atencion = "";
			string facturado_atencion = "";

			// fuente para las tablas creadas
			iTextSharp.text.Font _NormalFont;
			iTextSharp.text.Font _BoldFont;
			_NormalFont = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 7, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);
			_BoldFont = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 7, iTextSharp.text.Font.BOLD, BaseColor.BLACK);

			// step 1: creation of a document-object
			//Document documento = new Document(iTextSharp.text.PageSize.LETTER, 15, 15, 15, 15);

			Document documento = new Document(PageSize.LETTER.Rotate());
			string pdf_name = System.IO.Path.GetTempPath() + Guid.NewGuid().ToString() + ".pdf";
			try{
				PdfWriter writerpdf = PdfWriter.GetInstance(documento, new FileStream(pdf_name, FileMode.Create));	
				documento.AddTitle("Reporte de Admisiones");
				documento.AddCreator("Sistema Hospitalario OSIRIS");
				documento.AddAuthor("Sistema Hospitalario OSIRIS");
				documento.AddSubject("OSIRSrpt");	
				EventoTitulos ev = new EventoTitulos();
				ev.titulo1_rpt = "REPORTE DE ADMISIONES";
				ev.fechainicio = (string) parametros[1];
				ev.fechatermino = (string) parametros[2];
				ev.tipodepaciente = (string) parametros[3];
				ev.tiposadmision = (string) parametros[4];
				ev.nombredoctor =  (string) parametros[5];
				writerpdf.PageEvent = ev;
				documento.Open();

				PdfPCell cl01 = new PdfPCell();
				PdfPCell cl02 = new PdfPCell();
				PdfPCell cl03 = new PdfPCell();
				PdfPCell cl04 = new PdfPCell();
				PdfPCell cl05 = new PdfPCell();
				PdfPCell cl06 = new PdfPCell();
				PdfPCell cl07 = new PdfPCell();
				PdfPCell cl08 = new PdfPCell();
				PdfPCell cl09 = new PdfPCell();

				PdfPTable tblConceptos = new PdfPTable(9);
				tblConceptos.WidthPercentage = 100;
				float[] widths1 = new float[] { 30f, 20f, 20f, 100f, 20f, 50f, 80f, 10f ,30f };	// controlando el ancho de cada columna tienen que sumas 315 en total
				tblConceptos.SetWidths(widths1);
				tblConceptos.HorizontalAlignment = 0;

				NpgsqlConnection conexion; 
				conexion = new NpgsqlConnection (connectionString+nombrebd);
				// Verifica que la base de datos este conectada Querys
				try{
					conexion.Open ();
					NpgsqlCommand comando; 
					comando = conexion.CreateCommand (); 
					comando.CommandText = query_reporte + query_filtros_;
					Console.WriteLine(comando.CommandText);		
					NpgsqlDataReader lector = comando.ExecuteReader ();					
					while(lector.Read()){
						total_de_admisiones ++;
						tblConceptos = new PdfPTable(9);
						tblConceptos.WidthPercentage = 100;
						widths1 = new float[] { 30f, 20f, 20f, 100f, 20f, 50f, 80f, 10f ,30f };	// controlando el ancho de cada columna tienen que sumas 315 en total
						tblConceptos.SetWidths(widths1);
						tblConceptos.HorizontalAlignment = 0;
						if(int.Parse(lector["edad"].ToString().Trim()) > 0){
							edad = lector["edad"].ToString().Trim()+" Años"; 
						}else{
							edad = lector["mesesedad"].ToString().Trim()+" Meses";
						}
						if((int) lector ["id_aseguradora"] > 1){
							descri_empresa_aseguradora =  (string) lector["descripcion_aseguradora"];
						}else{
							descri_empresa_aseguradora =  (string) lector["descripcion_empresa"];						
						}
						if((bool) lector["cancelado"]){

						}
						if((bool) lector["vista_primera_vez"]){
							primeravez = "SI";
							contador_primeravez += 1;
						}else{
							primeravez = "";
						}
						// Configuramos el título de las columnas de la tabla
						cl01 = new PdfPCell(new Phrase(lector["fech_reg_adm"].ToString().Trim()+" "+lector["hora_reg_adm"].ToString().Trim(), _NormalFont));
						//clnroatencion.BorderWidth = 1;			// Ancho del borde
						cl01.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER;
						cl01.HorizontalAlignment = 0;
						cl02 = new PdfPCell(new Phrase(lector["folio_de_servicio"].ToString().Trim(), _BoldFont));
						cl02.Border = iTextSharp.text.Rectangle.TOP_BORDER;
						cl02.HorizontalAlignment = 0;
						cl03 = new PdfPCell(new Phrase(lector["pid_paciente"].ToString().Trim(), _NormalFont));
						cl03.Border = iTextSharp.text.Rectangle.TOP_BORDER;
						cl03.HorizontalAlignment = 0;
						cl04 = new PdfPCell(new Phrase(lector["nombre1_paciente"].ToString().Trim()+" "+ 
														lector["nombre2_paciente"].ToString().Trim()+" "+
														lector["apellido_paterno_paciente"].ToString().Trim()+" "+
														lector["apellido_materno_paciente"].ToString().Trim(), _NormalFont));
						cl04.Border = iTextSharp.text.Rectangle.TOP_BORDER;
						cl04.HorizontalAlignment = 0;
						cl05 = new PdfPCell(new Phrase(edad, _NormalFont));
						cl05.Border = iTextSharp.text.Rectangle.TOP_BORDER;
						cl05.HorizontalAlignment = 0;
						cl06 = new PdfPCell(new Phrase(lector["descripcion_tipo_paciente"].ToString().Trim(), _BoldFont));
						cl06.Border = iTextSharp.text.Rectangle.TOP_BORDER;
						cl06.HorizontalAlignment = 0;
						cl07 = new PdfPCell(new Phrase(descri_empresa_aseguradora, _NormalFont));
						cl07.Border = iTextSharp.text.Rectangle.TOP_BORDER;
						cl07.HorizontalAlignment = 0;
						cl08 = new PdfPCell(new Phrase(primeravez, _NormalFont));
						cl08.Border = iTextSharp.text.Rectangle.TOP_BORDER;
						cl08.HorizontalAlignment = 1;
						cl09 = new PdfPCell(new Phrase(lector["id_empleado_admision"].ToString().Trim(), _NormalFont));
						cl09.Border = iTextSharp.text.Rectangle.RIGHT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER;
						cl09.HorizontalAlignment = 0;

						// Añadimos las celdas a la tabla
						tblConceptos.AddCell(cl01);
						tblConceptos.AddCell(cl02);
						tblConceptos.AddCell(cl03);
						tblConceptos.AddCell(cl04);
						tblConceptos.AddCell(cl05);
						tblConceptos.AddCell(cl06);
						tblConceptos.AddCell(cl07);
						tblConceptos.AddCell(cl08);
						tblConceptos.AddCell(cl09);
						documento.Add(tblConceptos);

						tblConceptos = new PdfPTable(9);
						tblConceptos.WidthPercentage = 100;
						widths1 = new float[] { 70f, 100f, 70f, 10f, 10f, 10f, 10f, 10f, 10f };	// controlando el ancho de cada columna tienen que sumas 315 en total
						tblConceptos.SetWidths(widths1);
						tblConceptos.HorizontalAlignment = 0;

						movdocumentos = (string) llenado_admitido_a(lector["folio_de_servicio"].ToString().Trim());
						if((bool) incluir_status == true){
							// AM  Alta Medica
							if((bool) lector["alta_paciente"]){
								altamedica_px = "SI";
							}else{
								altamedica_px = "";
							}
							// Ps Pase Servicio Medico
							if((string) classpublic.lee_registro_de_tabla("osiris_erp_pases_qxurg","folio_de_servicio","WHERE folio_de_servicio = '"+lector["folio_de_servicio"].ToString().Trim()+"' ","folio_de_servicio","string") != ""){
								paseservicio_px = "SI";
							}else{
								paseservicio_px = "";
							}
							// CS Comprobante de Servicios
							if((string) classpublic.lee_registro_de_tabla("osiris_erp_comprobante_servicio","folio_de_servicio","WHERE folio_de_servicio = '"+lector["folio_de_servicio"].ToString().Trim()+"'","folio_de_servicio","string") != ""){
								comprservicio_px = "SI";
							}else{
								comprservicio_px = "";
							}
							// RG Registro en Caja  (pago/abono/documento por facturar)
							if((string) classpublic.lee_registro_de_tabla("osiris_erp_abonos","folio_de_servicio","WHERE folio_de_servicio = '"+lector["folio_de_servicio"].ToString().Trim()+"'","folio_de_servicio","string") != ""){
								regitroccaja_px = "SI";
							}else{
								regitroccaja_px = "";
							}
							// CR Cerrado Numero de atencion
							if((bool) lector["cerrado"] == true){
								cerrado_atencion = "SI";
							}else{
								cerrado_atencion = "";
							}
						}else{
							altamedica_px = "";
							paseservicio_px = "";
							comprservicio_px = "";
							regitroccaja_px = "";
							cerrado_atencion = "";
							facturado_atencion = "";
						}
						// Configuramos el título de las columnas de la tabla
						cl01 = new PdfPCell(new Phrase(lector["nombre_medico_encabezado"].ToString().Trim(), _NormalFont));
						//clnroatencion.BorderWidth = 1;			// Ancho del borde
						cl01.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER;
						cl01.HorizontalAlignment = 0;
						cl02 = new PdfPCell(new Phrase(lector["descripcion_diagnostico_movcargos"].ToString().Trim(), _NormalFont));
						//clnroatencion.BorderWidth = 1;			// Ancho del borde
						cl02.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
						cl02.HorizontalAlignment = 0;
						cl03 = new PdfPCell(new Phrase(movdocumentos, _NormalFont));
						//clnroatencion.BorderWidth = 1;			// Ancho del borde
						cl03.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
						cl03.HorizontalAlignment = 0;
						cl04 = new PdfPCell(new Phrase(altamedica_px, _NormalFont));
						//clnroatencion.BorderWidth = 1;			// Ancho del borde
						cl04.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
						cl04.HorizontalAlignment = 0;
						cl05 = new PdfPCell(new Phrase(paseservicio_px, _NormalFont));
						//clnroatencion.BorderWidth = 1;			// Ancho del borde
						cl05.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
						cl05.HorizontalAlignment = 0;
						cl06 = new PdfPCell(new Phrase(comprservicio_px, _NormalFont));
						//clnroatencion.BorderWidth = 1;			// Ancho del borde
						cl06.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
						cl06.HorizontalAlignment = 0;
						cl07 = new PdfPCell(new Phrase(regitroccaja_px, _NormalFont));
						//clnroatencion.BorderWidth = 1;			// Ancho del borde
						cl07.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
						cl07.HorizontalAlignment = 0;
						cl08 = new PdfPCell(new Phrase(cerrado_atencion, _NormalFont));
						//clnroatencion.BorderWidth = 1;			// Ancho del borde
						cl08.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
						cl08.HorizontalAlignment = 0;
						cl09 = new PdfPCell(new Phrase(facturado_atencion, _NormalFont));
						//clnroatencion.BorderWidth = 1;			// Ancho del borde
						cl09.Border = iTextSharp.text.Rectangle.RIGHT_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER;
						cl09.HorizontalAlignment = 0;

						// Añadimos las celdas a la tabla
						tblConceptos.AddCell(cl01);
						tblConceptos.AddCell(cl02);
						tblConceptos.AddCell(cl03);
						tblConceptos.AddCell(cl04);
						tblConceptos.AddCell(cl05);
						tblConceptos.AddCell(cl06);
						tblConceptos.AddCell(cl07);
						tblConceptos.AddCell(cl08);
						tblConceptos.AddCell(cl09);
						documento.Add(tblConceptos);

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
					}
					documento.Add (new Paragraph (" ", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 5)));

					tblConceptos = new PdfPTable(5);
					tblConceptos.WidthPercentage = 100;
					widths1 = new float[] { 40f, 30f, 40f, 30f, 150f };	// controlando el ancho de cada columna tienen que sumas 315 en total
					tblConceptos.SetWidths(widths1);
					tblConceptos.HorizontalAlignment = 0;

					// Configuramos el título de las columnas de la tabla
					cl01 = new PdfPCell(new Phrase("TOT. Admisiones", _BoldFont));
					//clnroatencion.BorderWidth = 1;			// Ancho del borde
					cl01.Border = iTextSharp.text.Rectangle.NO_BORDER;
					cl01.HorizontalAlignment = 0;
					cl02 = new PdfPCell(new Phrase(total_de_admisiones.ToString().Trim(), _BoldFont));
					//clnroatencion.BorderWidth = 1;			// Ancho del borde
					cl02.Border = iTextSharp.text.Rectangle.NO_BORDER;
					cl02.HorizontalAlignment = 0;
					cl03 = new PdfPCell(new Phrase("TOT. Primera Vez" , _BoldFont));
					//clnroatencion.BorderWidth = 1;			// Ancho del borde
					cl03.Border = iTextSharp.text.Rectangle.NO_BORDER;
					cl03.HorizontalAlignment = 0;
					cl04 = new PdfPCell(new Phrase(contador_primeravez.ToString().Trim(), _BoldFont));
					//clnroatencion.BorderWidth = 1;			// Ancho del borde
					cl04.Border = iTextSharp.text.Rectangle.NO_BORDER;
					cl04.HorizontalAlignment = 0;
					cl05 = new PdfPCell(new Phrase("AM=Alta Medica  PS=Pase Serv. Med.  CS=Compr.Serv. RC=Registro Caja  CR=Cerrado FA=Facturado", _BoldFont));
					//clnroatencion.BorderWidth = 1;			// Ancho del borde
					cl05.Border = iTextSharp.text.Rectangle.NO_BORDER;
					cl05.HorizontalAlignment = 0;
					tblConceptos.AddCell(cl01);
					tblConceptos.AddCell(cl02);
					tblConceptos.AddCell(cl03);
					tblConceptos.AddCell(cl04);
					tblConceptos.AddCell(cl05);
					documento.Add(tblConceptos);
				}catch (NpgsqlException ex){
					MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
						MessageType.Warning, ButtonsType.Ok, "PostgresSQL error: {0}",ex.Message);
					msgBoxError.Run ();				msgBoxError.Destroy();
					Console.WriteLine ("PostgresSQL error: {0}",ex.Message);
				}
				conexion.Close();
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

		string llenado_admitido_a(string folioservicio_)
		{
			string cadena_admitido_a = "";
			NpgsqlConnection conexion1; 
			conexion1 = new NpgsqlConnection (connectionString+nombrebd);
			try{
				conexion1.Open ();
				NpgsqlCommand comando1; 
				comando1 = conexion1.CreateCommand ();
				comando1.CommandText = "SELECT osiris_erp_movcargos.folio_de_servicio,osiris_erp_movcargos.id_tipo_admisiones AS idtipoadmisiones,osiris_his_tipo_admisiones.descripcion_admisiones "+
					"FROM osiris_erp_movcargos,osiris_his_tipo_admisiones " +
					"WHERE osiris_erp_movcargos.id_tipo_admisiones = osiris_his_tipo_admisiones.id_tipo_admisiones  "+
					"AND folio_de_servicio = '"+folioservicio_+"';";
				NpgsqlDataReader lector1 = comando1.ExecuteReader ();
				if (lector1.Read()){
					cadena_admitido_a += lector1["descripcion_admisiones"].ToString().Trim();
					while (lector1.Read()){
						cadena_admitido_a += "/"+lector1["descripcion_admisiones"].ToString().Trim();
					}
				}
				return cadena_admitido_a;
			}catch(NpgsqlException ex){
				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
					MessageType.Error,ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
				msgBoxError.Run ();					msgBoxError.Destroy();
			}
			conexion1.Close ();
			return cadena_admitido_a;
		}

		private class EventoTitulos : PdfPageEventHelper
		{
			class_public classpublic = new class_public();
			PdfContentByte cb;
			iTextSharp.text.pdf.PdfTemplate template;

			#region Fields
			private string _titulo1_rpt;
			private string _fechainicio;
			private string _fechatermino;
			private string _tiposadmision;
			private string _tipodepaciente;
			private string _nombredoctor;
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
			public string fechainicio
			{
				get{
					return _fechainicio;
				}
				set{
					_fechainicio = value;
				}
			}
			public string fechatermino
			{
				get{
					return _fechatermino;
				}
				set{
					_fechatermino = value;
				}
			}
			public string tiposadmision
			{
				get{
					return _tiposadmision;
				}
				set{
					_tiposadmision = value;
				}
			}
			public string tipodepaciente
			{
				get{
					return _tipodepaciente;
				}
				set{
					_tipodepaciente = value;
				}
			}
			public string nombredoctor
			{
				get{
					return _nombredoctor;
				}
				set{
					_nombredoctor = value;
				}
			}
			#endregion

			public override void OnOpenDocument(PdfWriter writerpdf, Document documento)
			{
				try{
					cb = writerpdf.DirectContent;
					template = cb.CreateTemplate(50, 50);
				}catch (DocumentException de){

            	}catch (System.IO.IOException ioe){

            	}
			}

			public override void OnStartPage(PdfWriter writerpdf, Document documento)
			{		
				//Will hold our current x,y coordinates;
				float curY;
				float curX;

				PdfPCell cl01 = new PdfPCell();
				PdfPCell cl02 = new PdfPCell();
				PdfPCell cl03 = new PdfPCell();
				PdfPCell cl04 = new PdfPCell();
				PdfPCell cl05 = new PdfPCell();
				PdfPCell cl06 = new PdfPCell();
				PdfPCell cl07 = new PdfPCell();
				PdfPCell cl08 = new PdfPCell();
				PdfPCell cl09 = new PdfPCell();
				PdfPCell cl10 = new PdfPCell();

				curY = writerpdf.GetVerticalPosition(true);

				//The current X is just the left margin
				curX = documento.LeftMargin;

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

				cb = writerpdf.DirectContent;
				// we tell the ContentByte we're ready to draw text
				cb.BeginText ();
				cb.SetFontAndSize (bf, 9);
				cb.SetTextMatrix(140, 560);
				//cb.SetTextMatrix (130, 750);		
				cb.ShowText (classpublic.nombre_empresa);
				cb.SetTextMatrix (140, 550);		cb.ShowText (classpublic.direccion_empresa);
				cb.SetTextMatrix (140, 540);		cb.ShowText (classpublic.telefonofax_empresa);
				cb.SetFontAndSize (bf, 6);
				cb.SetTextMatrix (140, 530);		cb.ShowText ("Sistema Hospitalario OSIRIS");
				cb.SetTextMatrix (670, 560);		cb.ShowText ("Fech.Rpt:" + (string)DateTime.Now.ToString ("yyyy-MM-dd HH:mm:ss"));
				//cb.SetTextMatrix (670, 550);		cb.ShowText ("N° Page :"+writerpdf.PageNumber.ToString("D8"));
				cb.EndText ();

				documento.Add (new Paragraph (" ", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 8)));

				Paragraph titulo1_reporte = new Paragraph(titulo1_rpt, new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 12, iTextSharp.text.Font.BOLD, BaseColor.BLACK));
				titulo1_reporte.Alignment = Element.ALIGN_CENTER;
				documento.Add(titulo1_reporte);
				documento.Add (new Paragraph (" ", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 8)));

				// fuente para las tablas creadas
				iTextSharp.text.Font _NormalFont = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 7, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);
				iTextSharp.text.Font _BoldFont = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 7, iTextSharp.text.Font.BOLD, BaseColor.BLACK);

				//Paragraph titulo2_reporte = new Paragraph("INFORMACION DE INGRESO DEL PACIENTE", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 9, iTextSharp.text.Font.BOLD, BaseColor.BLACK));
				//titulo2_reporte.Alignment = Element.ALIGN_CENTER;
				//documento.Add(titulo2_reporte);

				PdfPTable tabsubtitulo = new PdfPTable(1);
				tabsubtitulo.WidthPercentage = 36.0f;
				float[] widths_tabsubtit = new float[] { 1f };	// controlando el ancho de cada columna tienen que sumas 315 en total
				tabsubtitulo.SetWidths(widths_tabsubtit);
				tabsubtitulo.HorizontalAlignment = 1;

				PdfPCell cellsubtit_col1;
				cellsubtit_col1 = new PdfPCell(new Phrase("FILTROS DEL REPORTE",new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 9, iTextSharp.text.Font.BOLD, BaseColor.BLACK)));
				cellsubtit_col1.Border = PdfPCell.NO_BORDER;
				cellsubtit_col1.CellEvent = new RoundedBorder();
				cellsubtit_col1.HorizontalAlignment = 1;
				tabsubtitulo.AddCell(cellsubtit_col1);
				documento.Add(tabsubtitulo);

				documento.Add (new Paragraph (" ", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 5)));

				PdfPTable tblConceptos = new PdfPTable(10);
				tblConceptos.WidthPercentage = 100;
				float[] widths1 = new float[] { 30f, 20f, 30f, 40f, 30f, 30f, 30f, 30f, 30, 90f };	// controlando el ancho de cada columna tienen que sumas 315 en total
				tblConceptos.SetWidths(widths1);
				tblConceptos.HorizontalAlignment = 0;

				// Configuramos el título de las columnas de la tabla
				cl01 = new PdfPCell(new Phrase("Fecha Iincio", _BoldFont));
				//clnroatencion.BorderWidth = 1;			// Ancho del borde
				cl01.Border = iTextSharp.text.Rectangle.NO_BORDER;
				cl01.HorizontalAlignment = 0;
				cl02 = new PdfPCell(new Phrase(fechainicio, _BoldFont));
				cl02.Border = iTextSharp.text.Rectangle.NO_BORDER;
				cl02.HorizontalAlignment = 0;
				cl03 = new PdfPCell(new Phrase("Fecha Final", _BoldFont));
				cl03.Border = iTextSharp.text.Rectangle.NO_BORDER;
				cl03.HorizontalAlignment = 0;
				cl04 = new PdfPCell(new Phrase(fechatermino, _BoldFont));
				cl04.Border = iTextSharp.text.Rectangle.NO_BORDER;
				cl04.HorizontalAlignment = 0;
				cl05 = new PdfPCell(new Phrase("Tipo PX.", _BoldFont));
				cl05.Border = iTextSharp.text.Rectangle.NO_BORDER;
				cl05.HorizontalAlignment = 0;
				cl06 = new PdfPCell(new Phrase(tipodepaciente, _BoldFont));
				cl06.Border = iTextSharp.text.Rectangle.NO_BORDER;
				cl06.HorizontalAlignment = 0;
				cl07 = new PdfPCell(new Phrase("Tipo Admision", _BoldFont));
				cl07.Border = iTextSharp.text.Rectangle.NO_BORDER;
				cl07.HorizontalAlignment = 0;
				cl08 = new PdfPCell(new Phrase(tiposadmision, _BoldFont));
				cl08.Border = iTextSharp.text.Rectangle.NO_BORDER;
				cl08.HorizontalAlignment = 0;
				cl09 = new PdfPCell(new Phrase("Doctor", _BoldFont));
				cl09.Border = iTextSharp.text.Rectangle.NO_BORDER;
				cl09.HorizontalAlignment = 0;
				cl10 = new PdfPCell(new Phrase(tiposadmision, _BoldFont));
				cl10.Border = iTextSharp.text.Rectangle.NO_BORDER;
				cl10.HorizontalAlignment = 0;

				// Añadimos las celdas a la tabla
				tblConceptos.AddCell(cl01);
				tblConceptos.AddCell(cl02);
				tblConceptos.AddCell(cl03);
				tblConceptos.AddCell(cl04);
				tblConceptos.AddCell(cl05);
				tblConceptos.AddCell(cl06);
				tblConceptos.AddCell(cl07);
				tblConceptos.AddCell(cl08);
				tblConceptos.AddCell(cl09);
				tblConceptos.AddCell(cl10);
				documento.Add(tblConceptos);

				documento.Add (new Paragraph (" ", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 5)));


				tblConceptos = new PdfPTable(9);
				tblConceptos.WidthPercentage = 100;
				widths1 = new float[] { 30f, 20f, 20f, 100f, 20f, 50f, 80f, 10f ,30f };	// controlando el ancho de cada columna tienen que sumas 315 en total
				tblConceptos.SetWidths(widths1);
				tblConceptos.HorizontalAlignment = 0;

				// Configuramos el título de las columnas de la tabla
				cl01 = new PdfPCell(new Phrase("Fecha-Hora", _BoldFont));
				//clnroatencion.BorderWidth = 1;			// Ancho del borde
				cl01.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER;
				cl01.HorizontalAlignment = 1;
				cl01.BackgroundColor = BaseColor.YELLOW;
				cl02 = new PdfPCell(new Phrase("Atencion", _BoldFont));
				cl02.Border = iTextSharp.text.Rectangle.TOP_BORDER;
				cl02.HorizontalAlignment = 1;
				cl02.BackgroundColor = BaseColor.YELLOW;
				cl03 = new PdfPCell(new Phrase("Expe.", _BoldFont));
				cl03.Border = iTextSharp.text.Rectangle.TOP_BORDER;
				cl03.HorizontalAlignment = 0;
				cl03.BackgroundColor = BaseColor.YELLOW;
				cl04 = new PdfPCell(new Phrase("Nombre Paciente", _BoldFont));
				cl04.Border = iTextSharp.text.Rectangle.TOP_BORDER;
				cl04.HorizontalAlignment = 1;
				cl04.BackgroundColor = BaseColor.YELLOW;
				cl05 = new PdfPCell(new Phrase("Edad", _BoldFont));
				cl05.Border = iTextSharp.text.Rectangle.TOP_BORDER;
				cl05.HorizontalAlignment = 1;
				cl05.BackgroundColor = BaseColor.YELLOW;
				cl06 = new PdfPCell(new Phrase("Tipo Paciente", _BoldFont));
				cl06.Border = iTextSharp.text.Rectangle.TOP_BORDER;
				cl06.HorizontalAlignment = 1;
				cl06.BackgroundColor = BaseColor.YELLOW;
				cl07 = new PdfPCell(new Phrase("Institucion/Empresa", _BoldFont));
				cl07.Border = iTextSharp.text.Rectangle.TOP_BORDER;
				cl07.HorizontalAlignment = 1;
				cl07.BackgroundColor = BaseColor.YELLOW;
				cl08 = new PdfPCell(new Phrase("1ra Vez", _BoldFont));
				cl08.Border = iTextSharp.text.Rectangle.TOP_BORDER;
				cl08.HorizontalAlignment = 1;
				cl08.BackgroundColor = BaseColor.YELLOW;
				cl09 = new PdfPCell(new Phrase("Ingr. Por", _BoldFont));
				cl09.Border = iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
				cl09.HorizontalAlignment = 1;
				cl09.BackgroundColor = BaseColor.YELLOW;
				// Añadimos las celdas a la tabla
				tblConceptos.AddCell(cl01);
				tblConceptos.AddCell(cl02);
				tblConceptos.AddCell(cl03);
				tblConceptos.AddCell(cl04);
				tblConceptos.AddCell(cl05);
				tblConceptos.AddCell(cl06);
				tblConceptos.AddCell(cl07);
				tblConceptos.AddCell(cl08);
				tblConceptos.AddCell(cl09);
				documento.Add(tblConceptos);

				tblConceptos = new PdfPTable(9);
				tblConceptos.WidthPercentage = 100;
				widths1 = new float[] { 70f, 100f, 70f, 10f, 10f, 10f, 10f, 10f, 10f };	// controlando el ancho de cada columna tienen que sumas 315 en total
				tblConceptos.SetWidths(widths1);
				tblConceptos.HorizontalAlignment = 0;

				// Configuramos el título de las columnas de la tabla
				cl01 = new PdfPCell(new Phrase("Medico 1er. Contacto", _BoldFont));
				cl01.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER;
				cl01.HorizontalAlignment = 1;
				cl01.BackgroundColor = BaseColor.YELLOW;
				cl02 = new PdfPCell(new Phrase("Motivo de Ingreso", _BoldFont));
				cl02.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
				cl02.HorizontalAlignment = 1;
				cl02.BackgroundColor = BaseColor.YELLOW;
				cl03 = new PdfPCell(new Phrase("Admitido a:", _BoldFont));
				cl03.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
				cl03.HorizontalAlignment = 0;
				cl03.BackgroundColor = BaseColor.YELLOW;
				cl04 = new PdfPCell(new Phrase("AM", _BoldFont));
				cl04.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
				cl04.HorizontalAlignment = 0;
				cl04.BackgroundColor = BaseColor.YELLOW;
				cl05 = new PdfPCell(new Phrase("PS", _BoldFont));
				cl05.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
				cl05.HorizontalAlignment = 0;
				cl05.BackgroundColor = BaseColor.YELLOW;
				cl06 = new PdfPCell(new Phrase("CS", _BoldFont));
				cl06.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
				cl06.HorizontalAlignment = 0;
				cl06.BackgroundColor = BaseColor.YELLOW;
				cl07 = new PdfPCell(new Phrase("RC", _BoldFont));
				cl07.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
				cl07.HorizontalAlignment = 0;
				cl07.BackgroundColor = BaseColor.YELLOW;
				cl08 = new PdfPCell(new Phrase("CR", _BoldFont));
				cl08.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
				cl08.HorizontalAlignment = 0;
				cl08.BackgroundColor = BaseColor.YELLOW;
				cl09 = new PdfPCell(new Phrase("FA", _BoldFont));
				cl09.Border = iTextSharp.text.Rectangle.RIGHT_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER;
				cl09.HorizontalAlignment = 0;
				cl09.BackgroundColor = BaseColor.YELLOW;
				// Añadimos las celdas a la tabla
				tblConceptos.AddCell(cl01);
				tblConceptos.AddCell(cl02);
				tblConceptos.AddCell(cl03);
				tblConceptos.AddCell(cl04);
				tblConceptos.AddCell(cl05);
				tblConceptos.AddCell(cl06);
				tblConceptos.AddCell(cl07);
				tblConceptos.AddCell(cl08);
				tblConceptos.AddCell(cl09);
				documento.Add(tblConceptos);
			}

			public override void OnEndPage(PdfWriter writerpdf, Document documento)
			{
				base.OnEndPage(writerpdf, documento);
				int pageN = writerpdf.PageNumber;
				iTextSharp.text.pdf.BaseFont bf = BaseFont.CreateFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
				iTextSharp.text.Rectangle pageSize = documento.PageSize;
				String text = "N° Page : "+writerpdf.PageNumber.ToString().Trim()+" of ";
				//float len = bf.GetWidthPoint(text, 6);
				float len = bf.GetWidthPoint(text, 6);
				cb = writerpdf.DirectContent;
				// we tell the ContentByte we're ready to draw text
				cb.BeginText ();
				cb.SetFontAndSize (bf, 6);
				cb.SetTextMatrix (670, 550);		cb.ShowText (text);
				cb.EndText ();
				cb.AddTemplate(template, 670 + len, 550);
			}

			public override void OnCloseDocument(PdfWriter writerpdf, Document documento)
			{
				base.OnCloseDocument(writerpdf, documento);
				iTextSharp.text.pdf.BaseFont bf = BaseFont.CreateFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
				// we tell the ContentByte we're ready to draw text
				template.BeginText ();
				template.SetFontAndSize (bf, 6);
				//cb.SetTextMatrix (670, 570);		cb.ShowText ("" + (writerpdf.PageNumber - 1));
				template.SetTextMatrix (670, 570);		template.ShowText ("" + (writerpdf.PageNumber - 1));
				template.EndText ();
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
