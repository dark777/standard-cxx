///////////////////////////////////////////////////////////
// project created on 24/10/2006 at 10:20 a
// Sistema Hospitalario OSIRIS
// Monterrey - Mexico
//
// Autor    	: Juan Antonio Peña Gonzalez (Programacion) gjuanzz@gmail.com
// 				  Daniel Olivares Cuevas (Pre-Programacion, Colaboracion y Ajustes) arcangeldoc@openmailbox.org
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
	
	public class rpt_honorarios_medicos
	{
		int numpage = 1;
		
		string connectionString;
		string nombrebd;
		string LoginEmpleado;
		string NomEmpleado;
		string AppEmpleado;
		string ApmEmpleado;
		
		string query_sql = "SELECT osiris_erp_honorarios_medicos.folio_de_servicio AS folioservicio,osiris_erp_cobros_enca.pid_paciente AS expediente," +
			"to_char(osiris_erp_honorarios_medicos.fechahora_abono,'yyyy-MM-dd') AS fecharegistro,"+
			"to_char(osiris_erp_honorarios_medicos.fecha_visita,'yyyy-MM-dd') AS fechavisita,id_tipo_honorario," +
			"to_char(osiris_erp_cobros_enca.fechahora_creacion,'yyyy-MM-dd') AS fechaingreso,"+
			"osiris_his_paciente.nombre1_paciente || ' ' || osiris_his_paciente.nombre2_paciente || ' ' || osiris_his_paciente.apellido_paterno_paciente || ' ' || osiris_his_paciente.apellido_materno_paciente AS nombre_paciente," +
			"descripcion_diagnostico_movcargos AS procedimiento_admision,monto_del_abono AS valorhonorario," +
			"osiris_erp_honorarios_medicos.pagado,to_char(osiris_erp_honorarios_medicos.fechahora_pagado,'yyyy-MM-dd') AS fechapago," +
			"osiris_his_medicos.nombre_medico,osiris_his_medicos.id_especialidad,osiris_his_tipo_especialidad.descripcion_especialidad,motivo_visita," +
			"id_tipo_honorario,osiris_erp_honorarios_medicos.id_tipo_paciente,descripcion_tipo_paciente AS tipopaciente "+
			"FROM osiris_erp_honorarios_medicos,osiris_erp_cobros_enca,osiris_his_paciente,osiris_his_medicos,osiris_his_tipo_especialidad,osiris_his_tipo_pacientes " +
			"WHERE osiris_erp_honorarios_medicos.folio_de_servicio = osiris_erp_cobros_enca.folio_de_servicio " +
			"AND osiris_his_paciente.pid_paciente = osiris_erp_cobros_enca.pid_paciente " +
			"AND osiris_erp_honorarios_medicos.id_medico = osiris_his_medicos.id_medico " +
			"AND osiris_his_medicos.id_especialidad = osiris_his_tipo_especialidad.id_especialidad " +
			"AND osiris_erp_honorarios_medicos.id_tipo_paciente = osiris_his_tipo_pacientes.id_tipo_paciente " ;
						
		//Declaracion de ventana de error 
		protected Gtk.Window MyWinError;
		
		class_conexion conexion_a_DB = new class_conexion();
		class_public classpublic = new class_public();
		
		public rpt_honorarios_medicos(string query_fechas_,string _nombrebd_,string LoginEmpleado_,string NomEmpleado_,
			string AppEmpleado_,string ApmEmpleado_,string tiporeporte_,string orderby_,string query_medico_,string query_nroatencion_)
		{
			connectionString = conexion_a_DB._url_servidor+conexion_a_DB._port_DB+conexion_a_DB._usuario_DB+conexion_a_DB._passwrd_user_DB;
			nombrebd = conexion_a_DB._nombrebd;
			LoginEmpleado = LoginEmpleado_;
			NomEmpleado = NomEmpleado_;
			AppEmpleado = AppEmpleado_;
			ApmEmpleado = ApmEmpleado_;
			query_sql = query_sql + query_medico_ + query_nroatencion_ + query_fechas_ + orderby_;

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
			PdfPCell cl11 = new PdfPCell();

			// fuente para las tablas creadas
			iTextSharp.text.Font _NormalFont = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 7, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);
			iTextSharp.text.Font _BoldFont = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 7, iTextSharp.text.Font.BOLD, BaseColor.BLACK);

			string pdf_name = System.IO.Path.GetTempPath() + Guid.NewGuid().ToString() + ".pdf";

			NpgsqlConnection conexion; 
			conexion = new NpgsqlConnection (connectionString+nombrebd);
			// Verifica que la base de datos este conectada
			try{
				conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand (); 
				comando.CommandText = query_sql;
				Console.WriteLine(comando.CommandText);
				NpgsqlDataReader lector = comando.ExecuteReader();
				if (lector.Read()){
					// step 1: creation of a document-object
					//Document documento = new Document(iTextSharp.text.PageSize.LETTER, 15, 15, 15, 15);
					Document documento = new Document(PageSize.LETTER.Rotate());
					try{
						PdfWriter writerpdf = PdfWriter.GetInstance(documento, new FileStream(pdf_name, FileMode.Create));
						EventoTitulos_honormedicos ev = new EventoTitulos_honormedicos();
						documento.AddTitle("Reporte de Honorarios Medicos");
						documento.AddCreator("Sistema Hospitalario OSIRIS");
						documento.AddAuthor("Sistema Hospitalario OSIRIS");
						documento.AddSubject("OSIRSrpt");
						writerpdf.PageEvent = ev;
						ev.titulo1_rpt = "SOLICITUDES DE VISITAS MEDICAS";
						documento.Open();

						PdfPTable tblConceptos = new PdfPTable(11);
						tblConceptos.WidthPercentage = 100;
						float[] widths1 = new float[] { 25f, 25f, 25f, 25f, 30f, 80f, 80f, 60f, 60, 30f, 30 };	// controlando el ancho de cada columna tienen que sumas 315 en total
						tblConceptos.SetWidths(widths1);
						tblConceptos.HorizontalAlignment = 0;


						// Configuramos el título de las columnas de la tabla
						cl01 = new PdfPCell(new Phrase(lector["fechavisita"].ToString().Trim()+" "+lector["hora_reg_adm"].ToString().Trim(), _NormalFont));
						//clnroatencion.BorderWidth = 1;			// Ancho del borde
						cl01.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER;
						cl01.HorizontalAlignment = 0;
						cl02 = new PdfPCell(new Phrase(lector["fecharegistro"].ToString().Trim(), _BoldFont));
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
						cl05 = new PdfPCell(new Phrase("", _NormalFont));
						cl05.Border = iTextSharp.text.Rectangle.TOP_BORDER;
						cl05.HorizontalAlignment = 0;
						cl06 = new PdfPCell(new Phrase("", _BoldFont));
						cl06.Border = iTextSharp.text.Rectangle.TOP_BORDER;
						cl06.HorizontalAlignment = 0;
						cl07 = new PdfPCell(new Phrase("", _NormalFont));
						cl07.Border = iTextSharp.text.Rectangle.TOP_BORDER;
						cl07.HorizontalAlignment = 0;
						cl08 = new PdfPCell(new Phrase("", _NormalFont));
						cl08.Border = iTextSharp.text.Rectangle.TOP_BORDER;
						cl08.HorizontalAlignment = 1;
						cl09= new PdfPCell(new Phrase("", _NormalFont));
						cl09.Border = iTextSharp.text.Rectangle.TOP_BORDER;
						cl09.HorizontalAlignment = 1;
						cl10 = new PdfPCell(new Phrase("", _NormalFont));
						cl10.Border = iTextSharp.text.Rectangle.TOP_BORDER;
						cl10.HorizontalAlignment = 1;
						cl11 = new PdfPCell(new Phrase("", _NormalFont));
						cl11.Border = iTextSharp.text.Rectangle.RIGHT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER;
						cl11.HorizontalAlignment = 0;

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
						tblConceptos.AddCell(cl11);
						documento.Add(tblConceptos);
						while(lector.Read()){
							
						}
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
				msgBoxError.Run ();					msgBoxError.Destroy();
				Console.WriteLine ("PostgresSQL error: {0}",ex.Message);
			}
			conexion.Close();
					
		}

		private class EventoTitulos_honormedicos : PdfPageEventHelper
		{
			class_public classpublic = new class_public();
			PdfContentByte cb;
			iTextSharp.text.pdf.PdfTemplate template;

			#region Fields
			private string _titulo1_rpt;

			private string _fechavisitamed;
			private string _fecharegistro;
			private string _fechaadmision_px;
			private string _nroexpiente_px;
			private string _nroatencion_px;
			private string _nombresapellidos_px;
			private string _motivoingreso_px;
			private string _medico_tratante_px;
			private string _especia_medtrat_px;
			private string _montotabulado;
			private string _montoafacturar;
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
			public string fechavisitamed
			{
				get{
					return _fechavisitamed;
				}
				set{
					_fechavisitamed = value;
				}
			}
			public string fecharegistro
			{
				get{
					return _fecharegistro;
				}
				set{
					_fecharegistro = value;
				}
			}
			public string fechaadmision_px
			{
				get{
					return _fechaadmision_px;
				}
				set{
					_fechaadmision_px = value;
				}
			}
			public string nroexpiente_px
			{
				get{
					return _nroexpiente_px;
				}
				set{
					_nroexpiente_px = value;
				}
			}
			public string nroatencion_px
			{
				get{
					return _nroatencion_px;
				}
				set{
					_nroatencion_px = value;
				}
			}
			public string nombresapellidos_px
			{
				get{
					return _nombresapellidos_px;
				}
				set{
					_nombresapellidos_px = value;
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
				PdfPCell cl11 = new PdfPCell();

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
				cellsubtit_col1 = new PdfPCell(new Phrase("REPORTE DE HONORARIOS REGISTRADOS",new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 9, iTextSharp.text.Font.BOLD, BaseColor.BLACK)));
				cellsubtit_col1.Border = PdfPCell.NO_BORDER;
				cellsubtit_col1.CellEvent = new RoundedBorder();
				cellsubtit_col1.HorizontalAlignment = 1;
				tabsubtitulo.AddCell(cellsubtit_col1);
				documento.Add(tabsubtitulo);

				documento.Add (new Paragraph (" ", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 5)));

				PdfPTable tblConceptos = new PdfPTable(11);
				tblConceptos.WidthPercentage = 100;
				float[] widths1 = new float[] { 25f, 25f, 25f, 25f, 30f, 80f, 80f, 60f, 60, 30f, 30 };	// controlando el ancho de cada columna tienen que sumas 315 en total
				tblConceptos.SetWidths(widths1);
				tblConceptos.HorizontalAlignment = 0;

				// Configuramos el título de las columnas de la tabla
				cl01 = new PdfPCell(new Phrase("Fech.Visita", _BoldFont));
				//clnroatencion.BorderWidth = 1;			// Ancho del borde
				cl01.Border = iTextSharp.text.Rectangle.NO_BORDER;
				cl01.HorizontalAlignment = 0;
				cl02 = new PdfPCell(new Phrase("Fech.Reg.", _BoldFont));
				cl02.Border = iTextSharp.text.Rectangle.NO_BORDER;
				cl02.HorizontalAlignment = 0;
				cl03 = new PdfPCell(new Phrase("Fech.Adm.", _BoldFont));
				cl03.Border = iTextSharp.text.Rectangle.NO_BORDER;
				cl03.HorizontalAlignment = 0;
				cl04 = new PdfPCell(new Phrase("N° Exp.", _BoldFont));
				cl04.Border = iTextSharp.text.Rectangle.NO_BORDER;
				cl04.HorizontalAlignment = 0;
				cl05 = new PdfPCell(new Phrase("N° Aten.", _BoldFont));
				cl05.Border = iTextSharp.text.Rectangle.NO_BORDER;
				cl05.HorizontalAlignment = 0;
				cl06 = new PdfPCell(new Phrase("Nombre Paciente", _BoldFont));
				cl06.Border = iTextSharp.text.Rectangle.NO_BORDER;
				cl06.HorizontalAlignment = 0;
				cl07 = new PdfPCell(new Phrase("Motivo de Impreso", _BoldFont));
				cl07.Border = iTextSharp.text.Rectangle.NO_BORDER;
				cl07.HorizontalAlignment = 0;
				cl08 = new PdfPCell(new Phrase("Nombre Doctor", _BoldFont));
				cl08.Border = iTextSharp.text.Rectangle.NO_BORDER;
				cl08.HorizontalAlignment = 0;
				cl09 = new PdfPCell(new Phrase("Especialidad", _BoldFont));
				cl09.Border = iTextSharp.text.Rectangle.NO_BORDER;
				cl09.HorizontalAlignment = 0;
				cl10 = new PdfPCell(new Phrase("Tabulado", _BoldFont));
				cl10.Border = iTextSharp.text.Rectangle.NO_BORDER;
				cl10.HorizontalAlignment = 0;
				cl11 = new PdfPCell(new Phrase("Facturado", _BoldFont));
				cl11.Border = iTextSharp.text.Rectangle.NO_BORDER;
				cl11.HorizontalAlignment = 0;

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
				tblConceptos.AddCell(cl11);
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
				cb.SetTextMatrix(140, 560);
				cb.SetTextMatrix (670, 550);		cb.ShowText (text);
				cb.EndText ();
				cb.AddTemplate(template, documento.LeftMargin + len, pageSize.GetBottom(documento.BottomMargin));
			}

			public override void OnCloseDocument(PdfWriter writerpdf, Document documento)
			{
				base.OnCloseDocument(writerpdf, documento);
				iTextSharp.text.pdf.BaseFont bf = BaseFont.CreateFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
				// we tell the ContentByte we're ready to draw text
				template.BeginText ();
				template.SetFontAndSize (bf, 6);
				template.SetTextMatrix(140, 560);
				//cb.SetTextMatrix (670, 570);		cb.ShowText ("" + (writerpdf.PageNumber - 1));
				template.SetTextMatrix (670, 570);		cb.ShowText ("" + (writerpdf.PageNumber - 1));
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
