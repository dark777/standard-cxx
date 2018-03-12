//
//  rpt_control_visita_medica.cs
//
//  Author:
//       dolivares <>
//
//  Copyright (c) 2017 dolivares
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
	public class rpt_control_visita_medica
	{
		string connectionString;
		string nombrebd;

		class_conexion conexion_a_DB = new class_conexion();
		class_public classpublic = new class_public();

		protected Gtk.Window MyWinError;

		public rpt_control_visita_medica (string nrovisitamedica_,int nrocopias_)
		{
			connectionString = conexion_a_DB._url_servidor+conexion_a_DB._port_DB+conexion_a_DB._usuario_DB+conexion_a_DB._passwrd_user_DB;
			nombrebd = conexion_a_DB._nombrebd;

			iTextSharp.text.Font _NormalFont;
			iTextSharp.text.Font _BoldFont;
			_NormalFont = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 7, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);
			_BoldFont = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 7, iTextSharp.text.Font.BOLD, BaseColor.BLACK);

			NpgsqlConnection conexion;
			conexion = new NpgsqlConnection (connectionString+nombrebd);
			// Verifica que la base de datos este conectada
			try{
				conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand ();	          	
				comando.CommandText = "SELECT * " +
					"FROM osiris_erp_honorarios_medicos " +
					"WHERE id_abono = '"+nrovisitamedica_+"';";

				Console.WriteLine(comando.CommandText);
				NpgsqlDataReader lector = comando.ExecuteReader ();
				if (lector.Read()){
					Document documento = new Document(iTextSharp.text.PageSize.LETTER, 15, 15, 15, 15);
					//Document document = new Document(PageSize.A4.Rotate());
					string pdf_name = System.IO.Path.GetTempPath() + Guid.NewGuid().ToString() + ".pdf";
					try{
						PdfWriter writerpdf = PdfWriter.GetInstance(documento, new FileStream(pdf_name, FileMode.Create));	
						documento.AddTitle("Reporte Control de Visita Medica");
						documento.AddCreator("Sistema Hospitalario OSIRIS");
						documento.AddAuthor("Sistema Hospitalario OSIRIS");
						documento.AddSubject("OSIRSrpt");

						EventoTitulos ev = new EventoTitulos();
						ev.titulo1_rpt = "CONTROL DE VISITA MEDICA";



						writerpdf.PageEvent = ev;
						documento.Open();




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
					}catch(Exception de){
						Console.Error.WriteLine(de.StackTrace);
					}
				}
			}catch (NpgsqlException ex){
				Console.WriteLine ("PostgresSQL error: {0}",ex.Message);
				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
					MessageType.Info,ButtonsType.Close, "PostgresSQL error: {0} ",ex.Message);
				msgBoxError.Run ();					msgBoxError.Destroy();
			}
			conexion.Close ();
		}

		private class EventoTitulos : PdfPageEventHelper
		{
			class_public classpublic = new class_public();

			#region Fields
			private string _titulo1_rpt;
		
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
				iTextSharp.text.Image firma_quimico = iTextSharp.text.Image.GetInstance(System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory,"OSIRISLogo2.png"));

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
				cb.SetTextMatrix (130, 750);		cb.ShowText (classpublic.nombre_empresa);
				cb.SetTextMatrix (130, 740);		cb.ShowText (classpublic.direccion_empresa);
				cb.SetTextMatrix (130, 730);		cb.ShowText (classpublic.telefonofax_empresa);
				cb.SetFontAndSize (bf, 6);
				cb.SetTextMatrix (130, 720);		cb.ShowText ("Sistema Hospitalario OSIRIS");
				cb.SetTextMatrix (500, 750);		cb.ShowText ("Fech.Rpt:" + (string)DateTime.Now.ToString ("yyyy-MM-dd HH:mm:ss"));
				cb.SetTextMatrix (500, 740);		cb.ShowText ("N° Page :"+writerpdf.PageNumber.ToString("D8"));
				cb.SetFontAndSize (bf, 5);
				cb.EndText ();

				documento.Add (new Paragraph (" ", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 6)));

				Paragraph titulo1_reporte = new Paragraph(titulo1_rpt, new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 10, iTextSharp.text.Font.BOLD, BaseColor.BLACK));
				titulo1_reporte.Alignment = Element.ALIGN_CENTER;
				documento.Add(titulo1_reporte);
				documento.Add (new Paragraph (" ", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 6)));

				// fuente para las tablas creadas
				iTextSharp.text.Font _NormalFont;
				iTextSharp.text.Font _BoldFont;
				_NormalFont = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 7, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);
				_BoldFont = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 7, iTextSharp.text.Font.BOLD, BaseColor.BLACK);

				//Paragraph titulo2_reporte = new Paragraph("INFORMACION DE INGRESO DEL PACIENTE", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 9, iTextSharp.text.Font.BOLD, BaseColor.BLACK));
				//titulo2_reporte.Alignment = Element.ALIGN_CENTER;
				//documento.Add(titulo2_reporte);

				PdfPCell cellcol1;
				PdfPCell cellcol3;
				PdfPCell cellcol2;
				PdfPCell cellcol4;
				PdfPCell cellcol5;
				PdfPCell cellcol6;
				PdfPCell cellcol7;
				PdfPCell cellcol8;
				PdfPCell cellcol9;
				PdfPCell cellcol10;
				PdfPCell cellcol11;
				PdfPCell cellcol12;
				
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

