//  
//  rpt_mov_gestioncobrz.cs
//  
//  Author:
//       Daniel Olivares <arcangeldoc@openmailbox.org>
// 
//  Copyright (c) 2015 dolivares
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
using Glade;
using Cairo;
using Pango;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;

namespace osiris
{
	public class rpt_mov_gestioncobrz
	{
		int numpage = 1;
		
		string connectionString;
        string nombrebd;
		
		//Declaracion de ventana de error
		protected Gtk.Window MyWinError;		
		class_conexion conexion_a_DB = new class_conexion();
		class_public classpublic = new class_public();
		
		public rpt_mov_gestioncobrz (int foliodeatencion_,string nombrepaciente_)
		{
			connectionString = conexion_a_DB._url_servidor+conexion_a_DB._port_DB+conexion_a_DB._usuario_DB+conexion_a_DB._passwrd_user_DB;
			nombrebd = conexion_a_DB._nombrebd;
			
			NpgsqlConnection conexion;
			conexion = new NpgsqlConnection (connectionString+nombrebd);
	        // Verifica que la base de datos este conectada
			try{
				conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand ();
				comando.CommandText = "SELECT to_char(osiris_erp_gestcobrzmov.fechahora_creacion,'yyyy-MM-dd') AS fechacrea,folio_de_servicio," +
				 	"to_char(osiris_erp_gestcobrzmov.fechahora_creacion,'HH24:MI') AS horacrea,nota,telefono,osiris_erp_gestcobrzmov.id_secuencia AS idsecuencia " +
					"FROM osiris_erp_gestcobrzmov " +
					"WHERE folio_de_servicio = '"+foliodeatencion_+"' " +
					"AND eliminado = 'false' " +
					"ORDER BY osiris_erp_gestcobrzmov.fechahora_creacion DESC;";
				NpgsqlDataReader lector = comando.ExecuteReader ();
				if(lector.Read()){
					string pdf_name = System.IO.Path.GetTempPath() + Guid.NewGuid().ToString() + ".pdf";
					// step 1: creation of a document-object
					Document documento = new Document(iTextSharp.text.PageSize.LETTER, 15, 15, 15, 15);
					//Document document = new Document(PageSize.A4.Rotate());
						
					try{
						PdfWriter writerpdf = PdfWriter.GetInstance(documento, new FileStream(pdf_name, FileMode.Create));
						
						documento.AddTitle("Reporte Historial de Movimientos");
			           	documento.AddCreator("Sistema Hospitalario OSIRIS");
			           	documento.AddAuthor("Sistema Hospitalario OSIRIS");
			           	documento.AddSubject("OSIRSrpt");
						EventoTitulos ev = new EventoTitulos();
						ev.titulo1_rpt = "MOVIMIENTOS DE COBRANZA";
						writerpdf.PageEvent = ev;
						documento.Open();		
						
						iTextSharp.text.Font _standardFont = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 7, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);
						// Creamos una tabla para el contenido
				        PdfPTable tblreporte = new PdfPTable(5);
				        tblreporte.WidthPercentage = 100;
						float[] widths = new float[] { 25f, 30f, 30f, 40f, 150f };	// controlando el ancho de cada columna
						tblreporte.SetWidths(widths);
						tblreporte.HorizontalAlignment = 0;						
						// Configuramos el título de las columnas de la tabla
						PdfPCell cl01 = new PdfPCell();
						PdfPCell cl02 = new PdfPCell();
						PdfPCell cl03 = new PdfPCell();
						PdfPCell cl04 = new PdfPCell();
						PdfPCell cl05 = new PdfPCell();
						
						// Configuramos el título de las columnas de la tabla
			            cl01 = new PdfPCell(new Phrase((string) lector["folio_de_servicio"].ToString().Trim(), _standardFont));
			            //clnroatencion.BorderWidth = 1;			// Ancho del borde
						cl01.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
						cl02 = new PdfPCell(new Phrase(lector["fechacrea"].ToString().Trim(), _standardFont));
			            cl02.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
						cl02.HorizontalAlignment = 1;		// centro
			            cl03 = new PdfPCell(new Phrase(lector["horacrea"].ToString().Trim(), _standardFont));
			            cl03.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
						cl04 = new PdfPCell(new Phrase(lector["telefono"].ToString().Trim(), _standardFont));
			            cl04.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
						cl05 = new PdfPCell(new Phrase(lector["nota"].ToString().Trim(), _standardFont));
			            cl05.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
			            // Añadimos las celdas a la tabla
			            tblreporte.AddCell(cl01);
			            tblreporte.AddCell(cl02);
						tblreporte.AddCell(cl03);
						tblreporte.AddCell(cl04);
						tblreporte.AddCell(cl05);
						
						while(lector.Read()){
							// Configuramos el título de las columnas de la tabla
				            cl01 = new PdfPCell(new Phrase((string) lector["folio_de_servicio"].ToString().Trim(), _standardFont));
				            //clnroatencion.BorderWidth = 1;			// Ancho del borde
							cl01.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
							cl02 = new PdfPCell(new Phrase(lector["fechacrea"].ToString().Trim(), _standardFont));
				            cl02.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
							cl02.HorizontalAlignment = 1;		// centro
				            cl03 = new PdfPCell(new Phrase(lector["horacrea"].ToString().Trim(), _standardFont));
				            cl03.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
							cl04 = new PdfPCell(new Phrase(lector["telefono"].ToString().Trim(), _standardFont));
				            cl04.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
							cl05 = new PdfPCell(new Phrase(lector["nota"].ToString().Trim(), _standardFont));
				            cl05.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
				            // Añadimos las celdas a la tabla
				            tblreporte.AddCell(cl01);
				            tblreporte.AddCell(cl02);
							tblreporte.AddCell(cl03);
							tblreporte.AddCell(cl04);
							tblreporte.AddCell(cl05);							
						}
						documento.Add(tblreporte);						
						//System.Diagnostics.Process proc = new System.Diagnostics.Process();
						//proc.EnableRaisingEvents = true;
						//proc.StartInfo.UseShellExecute = false;
						//proc.StartInfo.CreateNoWindow = true;
						//proc.StartInfo.RedirectStandardOutput = true;
						//proc.StartInfo.WorkingDirectory = AppDomain.CurrentDomain.BaseDirectory;
						//proc.StartInfo.FileName = "evince";
						//proc.StartInfo.Arguments = pdf_name;
						//proc.StartInfo.Arguments = "/home/dolivares/Desktop/"+siglasemisorcfd + "-" + entry_numero_factura.Text.Trim() + "_remision.pdf";
						//if((string) classpublic.plataform_OS() == "Unix"){ 
						//		proc.StartInfo.FileName = classpublic.lector_de_pdf_linux;
						//}
						//if((string) classpublic.plataform_OS() == "Win32NT"){ 
						//	proc.StartInfo.FileName = classpublic.lector_de_pdf_linux;
						//}
						try{				
							//proc.Start();
							System.Diagnostics.Process.Start(pdf_name);
						}catch(Exception ex){
								
						}					
					}catch(Exception de){
						Console.Error.WriteLine(de.StackTrace);
					}
					// step 5: we close the document
					documento.Close();
				}
			}catch(NpgsqlException ex){
				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
				MessageType.Error,ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
				msgBoxError.Run ();				msgBoxError.Destroy();
			}
			conexion.Close();
		}
		
		public class EventoTitulos : PdfPageEventHelper
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
		        iTextSharp.text.Rectangle pageSize = documento.PageSize;
				PdfContentByte cb = writerpdf.DirectContent;
				
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
				// we tell the ContentByte we're ready to draw text
				cb.BeginText ();
				cb.SetFontAndSize (BaseFont.CreateFont (BaseFont.HELVETICA_BOLD, BaseFont.CP1252, BaseFont.NOT_EMBEDDED), 9);
				cb.SetTextMatrix (130, 750); 			cb.ShowText (classpublic.nombre_empresa);
				cb.SetTextMatrix (130, 740);				cb.ShowText (classpublic.direccion_empresa);
				cb.SetTextMatrix (130, 730);				cb.ShowText (classpublic.telefonofax_empresa);
				cb.SetFontAndSize (BaseFont.CreateFont (BaseFont.HELVETICA_BOLD, BaseFont.CP1252, BaseFont.NOT_EMBEDDED), 6);
				cb.SetTextMatrix (130, 720); 			cb.ShowText ("Sistema Hospitalario OSIRIS");
				cb.SetTextMatrix (500, 750); 			cb.ShowText ("Fech.Rpt:" + (string)DateTime.Now.ToString ("yyyy-MM-dd HH:mm:ss"));
				cb.SetTextMatrix (500, 740);			cb.ShowText ("N° Page :"+writerpdf.PageNumber.ToString("D8"));
				cb.EndText ();
				documento.Add (new Paragraph (""));
				documento.Add (Chunk.NEWLINE);
				documento.Add (new Paragraph (""));
				documento.Add (Chunk.NEWLINE);
				
				Paragraph titulo1_reporte = new Paragraph(titulo1_rpt, new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 13, iTextSharp.text.Font.BOLD, BaseColor.BLACK));
                titulo1_reporte.Alignment = Element.ALIGN_CENTER;
                documento.Add(titulo1_reporte);
				documento.Add (new Paragraph (" ", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 6)));
								
				// Creamos una tabla para el contenido
	            PdfPTable tblreporte = new PdfPTable(5);
	            tblreporte.WidthPercentage = 100;
				float[] widths = new float[] { 25f, 30f, 30f, 40f, 150f };			// controlando el ancho de cada columna
				tblreporte.SetWidths(widths);
				tblreporte.HorizontalAlignment = 0;
				iTextSharp.text.Font _standardFont = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 7, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
				// Configuramos el título de las columnas de la tabla
	            PdfPCell cl01 = new PdfPCell(new Phrase("N° Atencion", _standardFont));
	            //clnroatencion.BorderWidth = 1;			// Ancho del borde
				cl01.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
				cl01.HorizontalAlignment = 1;		// centro
				PdfPCell cl02 = new PdfPCell(new Phrase("Fecha", _standardFont));
	            cl02.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
				cl02.HorizontalAlignment = 1;		// centro
	            PdfPCell cl03 = new PdfPCell(new Phrase("Hora", _standardFont));
	            cl03.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
				cl03.HorizontalAlignment = 1;		// centro
				PdfPCell cl04 = new PdfPCell(new Phrase("Telefono", _standardFont));
	            cl04.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
				cl04.HorizontalAlignment = 1;		// centro
				PdfPCell cl05 = new PdfPCell(new Phrase("Observaciones", _standardFont));
	            cl05.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
	            cl05.HorizontalAlignment = 1;		// centro
				// Añadimos las celdas a la tabla
	            tblreporte.AddCell(cl01);
	            tblreporte.AddCell(cl02);
				tblreporte.AddCell(cl03);
				tblreporte.AddCell(cl04);
				tblreporte.AddCell(cl05);				
				documento.Add(tblreporte);
		    }
		
		    public override void OnEndPage(PdfWriter writerpdf, Document documento)
		    {
		
		    }		
		}
	}
}

