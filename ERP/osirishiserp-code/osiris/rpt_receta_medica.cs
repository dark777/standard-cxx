//  
//  rpt_receta_medica.cs
//  
//  Author:
//       Daniel Olivares <arcangeldoc@openmailbox.org>
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
using Glade;
using Cairo;
using Pango;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;

namespace osiris
{
	public class rpt_receta_medica
	{
		string connectionString;
        string nombrebd;
		string empresa_o_aseguradora;
		
		class_conexion conexion_a_DB = new class_conexion();
		class_public classpublic = new class_public();
		
		protected Gtk.Window MyWinError;
		
		public rpt_receta_medica (string numeroreceta_,int folioservicio_,int PidPaciente_,string nombrepx_,string fechanacimientopx_,string edadpx_,string sexopx_)
		{
			connectionString = conexion_a_DB._url_servidor+conexion_a_DB._port_DB+conexion_a_DB._usuario_DB+conexion_a_DB._passwrd_user_DB;
			nombrebd = conexion_a_DB._nombrebd;
			
			iTextSharp.text.Font _NormalFont;
			iTextSharp.text.Font _BoldFont;
			_NormalFont = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 7, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);
			_BoldFont = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 7, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
	
			string pdf_name = System.IO.Path.GetTempPath() + Guid.NewGuid().ToString() + ".pdf";
							
			NpgsqlConnection conexion;
			conexion = new NpgsqlConnection (connectionString+nombrebd);
			// Verifica que la base de datos este conectada
			try{
				conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand ();	          	
				comando.CommandText = "SELECT descripcion_tipo_paciente,to_char(osiris_his_receta_medica.fechahora_creacion,'dd-MM-yyyy') AS fechareceta," +
										"folio_de_receta,folio_de_solicitud,descripcion_prescripcion,cantidad_recetada," +
										"osiris_his_medicos.nombre_medico,osiris_his_medicos.cedula_medico,"+							
										"osiris_erp_cobros_enca.id_aseguradora AS idaseguradora,descripcion_aseguradora," +
										"osiris_erp_cobros_enca.id_empresa AS idempresa,osiris_empresas.descripcion_empresa " +
										"FROM osiris_his_receta_medica,osiris_erp_cobros_enca,osiris_his_tipo_pacientes,osiris_empresas,osiris_aseguradoras,osiris_his_medicos " +
										"WHERE osiris_his_receta_medica.folio_de_servicio = osiris_erp_cobros_enca.folio_de_servicio " +
										"AND osiris_erp_cobros_enca.id_tipo_paciente = osiris_his_tipo_pacientes.id_tipo_paciente " +
										"AND osiris_erp_cobros_enca.id_empresa = osiris_empresas.id_empresa "+
			        					"AND osiris_erp_cobros_enca.id_aseguradora = osiris_aseguradoras.id_aseguradora "+
										"AND osiris_his_receta_medica.id_quiencreo = osiris_his_medicos.login_empleado " +
										"AND osiris_his_receta_medica.eliminado = 'false' " +
										"AND osiris_his_receta_medica.folio_de_receta = '"+numeroreceta_+"' "+
										"AND osiris_his_receta_medica.folio_de_servicio = '"+folioservicio_.ToString().Trim()+"';";
				//Console.WriteLine(comando.CommandText.ToString());
				NpgsqlDataReader lector = comando.ExecuteReader ();
				if (lector.Read()){
					// step 1: creation of a document-object
					Document documento = new Document(iTextSharp.text.PageSize.LETTER, 15, 15, 15, 15);
					//Document document = new Document(PageSize.LETTER.Rotate());
					try{
						PdfWriter writerpdf = PdfWriter.GetInstance(documento, new FileStream(pdf_name, FileMode.Create));
						
						documento.AddTitle("Receta Medica");
				       	documento.AddCreator("Sistema Hospitalario OSIRIS");
				       	documento.AddAuthor("Sistema Hospitalario OSIRIS");
				       	documento.AddSubject("OSIRSrpt");
						EventoTitulos_recetamedica ev = new EventoTitulos_recetamedica();
						ev.titulo1_rpt = "RECETA MEDICA";
						ev.nro_expediente_px = PidPaciente_.ToString().Trim();
						ev.nombres_apellidos_px = nombrepx_;
						ev.fecha_nacimiento_px = fechanacimientopx_;
						ev.edad_px = edadpx_;
						ev.sexo_px = sexopx_;
						ev.nombremedico_px = lector["nombre_medico"].ToString().Trim();
						ev.cedulamedico_px = lector["cedula_medico"].ToString().Trim();
						writerpdf.PageEvent = ev;
						documento.Open();				
						
						iTextSharp.text.pdf.PdfPCell cellcol1;
						iTextSharp.text.pdf.PdfPCell cellcol2;
						iTextSharp.text.pdf.PdfPCell cellcol3;
						iTextSharp.text.pdf.PdfPCell cellcol4;
						iTextSharp.text.pdf.PdfPCell cellcol5;
						iTextSharp.text.pdf.PdfPCell cellcol6;
						iTextSharp.text.pdf.PdfPCell cellcol7;
						iTextSharp.text.pdf.PdfPCell cellcol8;
											
						if(int.Parse(lector["idaseguradora"].ToString().Trim()) > 1){
							empresa_o_aseguradora = (string) lector["descripcion_aseguradora"];
						}else{
							empresa_o_aseguradora = (string) lector["descripcion_empresa"];						
						}
						
						PdfPTable tabsubtitulo2 = new PdfPTable(8);
						tabsubtitulo2.WidthPercentage = 100;
						float[] widths_tabsubtit2 = new float[] { 30f,30f,30f,30f,30f,30f,30f,30f };	// controlando el ancho de cada columna tienen que sumas 315 en total
						tabsubtitulo2.SetWidths(widths_tabsubtit2);
						tabsubtitulo2.HorizontalAlignment = 1;
						
						cellcol1 = new PdfPCell(new Phrase("FECHA RECETA" ,new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 8, iTextSharp.text.Font.BOLD, BaseColor.BLACK)));
						cellcol1.Border = PdfPCell.NO_BORDER;
						cellcol1.CellEvent = new RoundedBorder();
						cellcol1.HorizontalAlignment = 1;	
						
						cellcol2 = new PdfPCell(new Phrase((string) lector["fechareceta"] ,new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 8, iTextSharp.text.Font.BOLD, BaseColor.BLACK)));
						cellcol2.Border = PdfPCell.NO_BORDER;
						cellcol2.CellEvent = new RoundedBorder();
						cellcol2.HorizontalAlignment = 1;
						
						cellcol3 = new PdfPCell(new Phrase("N° RECETA" ,new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 8, iTextSharp.text.Font.BOLD, BaseColor.BLACK)));
						cellcol3.Border = PdfPCell.NO_BORDER;
						cellcol3.CellEvent = new RoundedBorder();
						cellcol3.HorizontalAlignment = 1;
						
						cellcol4 = new PdfPCell(new Phrase(lector["folio_de_receta"].ToString().Trim() ,new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 8, iTextSharp.text.Font.BOLD, BaseColor.BLACK)));
						cellcol4.Border = PdfPCell.NO_BORDER;
						cellcol4.CellEvent = new RoundedBorder();
						cellcol4.HorizontalAlignment = 1;
						
						cellcol5 = new PdfPCell(new Phrase("N° ATENCION" ,new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 8, iTextSharp.text.Font.BOLD, BaseColor.BLACK)));
						cellcol5.Border = PdfPCell.NO_BORDER;
						cellcol5.CellEvent = new RoundedBorder();
						cellcol5.HorizontalAlignment = 1;
						
						cellcol6= new PdfPCell(new Phrase(folioservicio_.ToString().Trim() ,new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 8, iTextSharp.text.Font.BOLD, BaseColor.BLACK)));
						cellcol6.Border = PdfPCell.NO_BORDER;
						cellcol6.CellEvent = new RoundedBorder();
						cellcol6.HorizontalAlignment = 1;
						
						cellcol7 = new PdfPCell(new Phrase("N° SOLICITUD" ,new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 8, iTextSharp.text.Font.BOLD, BaseColor.BLACK)));
						cellcol7.Border = PdfPCell.NO_BORDER;
						cellcol7.CellEvent = new RoundedBorder();
						cellcol7.HorizontalAlignment = 1;
						
						cellcol8 = new PdfPCell(new Phrase(lector["folio_de_solicitud"].ToString().Trim(),new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 8, iTextSharp.text.Font.BOLD, BaseColor.BLACK)));
						cellcol8.Border = PdfPCell.NO_BORDER;
						cellcol8.CellEvent = new RoundedBorder();
						cellcol8.HorizontalAlignment = 1;
										
						tabsubtitulo2.AddCell(cellcol1);
						tabsubtitulo2.AddCell(cellcol2);
						tabsubtitulo2.AddCell(cellcol3);
						tabsubtitulo2.AddCell(cellcol4);
						tabsubtitulo2.AddCell(cellcol5);
						tabsubtitulo2.AddCell(cellcol6);
						tabsubtitulo2.AddCell(cellcol7);
						tabsubtitulo2.AddCell(cellcol8);
										
						documento.Add(tabsubtitulo2);
						documento.Add (new Paragraph (" ", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 2)));
						
						PdfPTable tabFot2 = new PdfPTable(4);
						tabFot2.WidthPercentage = 100;
						float[] widths_tabfot2 = new float[] { 35f, 50f, 40f, 100f };	// controlando el ancho de cada columna tienen que sumas 315 en total
						tabFot2.SetWidths(widths_tabfot2);
						tabFot2.HorizontalAlignment = 0;
						cellcol1 = new PdfPCell(new Phrase("TIPO DE PACIENTE",_BoldFont));
						cellcol1.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
						cellcol1.HorizontalAlignment = 0;
						cellcol2 = new PdfPCell(new Phrase(lector["descripcion_tipo_paciente"].ToString().Trim(),_NormalFont));
						cellcol2.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
						cellcol2.HorizontalAlignment = 0;
						cellcol3 = new PdfPCell(new Phrase("CONVENIO/EMPR.",_BoldFont));
						cellcol3.HorizontalAlignment = 2;		// derecha		
						cellcol3.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
						cellcol4 = new PdfPCell(new Phrase(empresa_o_aseguradora,_NormalFont));
						cellcol4.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
						cellcol4.HorizontalAlignment = 0;
						tabFot2.AddCell(cellcol1);
						tabFot2.AddCell(cellcol2);
						tabFot2.AddCell(cellcol3);
						tabFot2.AddCell(cellcol4);
						documento.Add(tabFot2);
										
						iTextSharp.text.Paragraph p = new Paragraph ("PRESCRIPCION DE MEDICAMENTOS", _BoldFont);
						p.Alignment = Element.ALIGN_LEFT;										
						documento.Add (p);
						documento.Add (new Paragraph (" ", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 2)));
						
						PdfPTable tabFot1 = new PdfPTable(3);
						tabFot1.WidthPercentage = 100;
						float[] widths_tabfot1 = new float[] { 270f, 35f, 35f };	// controlando el ancho de cada columna tienen que sumas 315 en total
						tabFot1.SetWidths(widths_tabfot1);
						tabFot1.HorizontalAlignment = 1;
						
						cellcol1 = new PdfPCell(new Phrase("Dosis, Metodo de Empleo",_BoldFont));
						cellcol1.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
						cellcol1.HorizontalAlignment = 1;
						cellcol1.BackgroundColor = BaseColor.YELLOW;
						cellcol2 = new PdfPCell(new Phrase("Recetado",_BoldFont));
						cellcol2.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
						cellcol2.HorizontalAlignment = 1;
						cellcol2.BackgroundColor = BaseColor.YELLOW;
						cellcol3 = new PdfPCell(new Phrase("Surtido",_BoldFont));
						cellcol3.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
						cellcol3.HorizontalAlignment = 1;
						cellcol3.BackgroundColor = BaseColor.YELLOW;
						tabFot1.AddCell(cellcol1);
						tabFot1.AddCell(cellcol2);
						tabFot1.AddCell(cellcol3);
						
						cellcol1 = new PdfPCell(new Phrase(lector["descripcion_prescripcion"].ToString().Trim(),_NormalFont));
						cellcol1.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
						cellcol1.HorizontalAlignment = 0;
						
						cellcol2 = new PdfPCell(new Phrase(lector["cantidad_recetada"].ToString().Trim(),_NormalFont));
						cellcol2.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
						cellcol2.HorizontalAlignment = 1;
						
						cellcol3 = new PdfPCell(new Phrase("",_NormalFont));
						cellcol3.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
						cellcol3.HorizontalAlignment = 1;
						
						tabFot1.AddCell(cellcol1);
						tabFot1.AddCell(cellcol2);
						tabFot1.AddCell(cellcol3);
						while (lector.Read()){
							cellcol1 = new PdfPCell(new Phrase(lector["descripcion_prescripcion"].ToString().Trim(),_NormalFont));
							cellcol1.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
							cellcol1.HorizontalAlignment = 0;
						
							cellcol2 = new PdfPCell(new Phrase(lector["cantidad_recetada"].ToString().Trim(),_NormalFont));
							cellcol2.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
							cellcol2.HorizontalAlignment = 1;
						
							cellcol3 = new PdfPCell(new Phrase("",_NormalFont));
							cellcol3.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
							cellcol3.HorizontalAlignment = 1;
							
							tabFot1.AddCell(cellcol1);
							tabFot1.AddCell(cellcol2);
							tabFot1.AddCell(cellcol3);							
						}
						documento.Add(tabFot1);
					}catch(Exception de){
						Console.Error.WriteLine(de.StackTrace);
					}
					// step 5: we close the document
					documento.Close();
				}
			}catch (NpgsqlException ex){
				Console.WriteLine ("PostgresSQL error: {0}",ex.Message);
			   	MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
								MessageType.Info,ButtonsType.Close, "PostgresSQL error: {0} ",ex.Message);
				msgBoxError.Run ();					msgBoxError.Destroy();
			}
			conexion.Close ();
			try{				
				//proc.Start();
				System.Diagnostics.Process.Start(pdf_name);	
			}catch(Exception ex){
								
			}
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
		
		class EventoTitulos_recetamedica : PdfPageEventHelper
		{
			class_public classpublic = new class_public();
									
			#region Fields
			private string _titulo1_rpt;
			private string _nro_expediente_px;
			private string _nombres_apellidos_px;
			private string _fecha_nacimiento_px;
			private string _edad_px;
			private string _curp_px;
			private string _sexo_px;
			private string _ocupacion_px;
			private string _nombremedico_px;
			private string _cedulamedico_px;
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
			public string curp_px
        	{
            	get{
					return _curp_px;
				}
            	set{
					_curp_px = value;
				}
        	}
			public string sexo_px
        	{
            	get{
					return _sexo_px;
				}
            	set{
					_sexo_px = value;
				}
        	}
			public string ocupacion_px
        	{
            	get{
					return _ocupacion_px;
				}
            	set{
					_ocupacion_px = value;
				}
        	}		
			public string nombremedico_px
        	{
            	get{
					return _nombremedico_px;
				}
            	set{
					_nombremedico_px = value;
				}
        	}
			public string cedulamedico_px
        	{
            	get{
					return _cedulamedico_px;
				}
            	set{
					_cedulamedico_px = value;
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
				cb.SetTextMatrix (130, 750);		cb.ShowText (classpublic.nombre_empresa);
				cb.SetTextMatrix (130, 740);		cb.ShowText (classpublic.direccion_empresa);
				cb.SetTextMatrix (130, 730);		cb.ShowText (classpublic.telefonofax_empresa);
				cb.SetFontAndSize (BaseFont.CreateFont (BaseFont.HELVETICA_BOLD, BaseFont.CP1252, BaseFont.NOT_EMBEDDED), 6);
				cb.SetTextMatrix (130, 720);		cb.ShowText ("Sistema Hospitalario OSIRIS");
				cb.SetTextMatrix (500, 750);		cb.ShowText ("Fech.Rpt:" + (string)DateTime.Now.ToString ("yyyy-MM-dd HH:mm:ss"));
				cb.SetTextMatrix (500, 740);		cb.ShowText ("N° Page :"+writerpdf.PageNumber.ToString("D8"));
				
				cb.SetFontAndSize (BaseFont.CreateFont (BaseFont.HELVETICA_BOLD, BaseFont.CP1252, BaseFont.NOT_EMBEDDED), 7);
				cb.SetTextMatrix (15,440);	cb.ShowText ("MEDICO: "+nombremedico_px);
				cb.SetTextMatrix (300,440);	cb.ShowText ("CEDULA: "+cedulamedico_px);
				cb.SetTextMatrix (450,440);	cb.ShowText ("FIRMA:____________________________ ");
				
				cb.EndText ();
				documento.Add (new Paragraph (" ", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 8)));
				//documento.Add (new Paragraph (""));
				//documento.Add (Chunk.NEWLINE);
								
				Paragraph titulo1_reporte = new Paragraph(titulo1_rpt, new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 13, iTextSharp.text.Font.BOLD, BaseColor.BLACK));
                titulo1_reporte.Alignment = Element.ALIGN_CENTER;
                documento.Add(titulo1_reporte);
				documento.Add (new Paragraph (" ", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 6)));
								
				PdfPTable tabsubtitulo = new PdfPTable(1);
				tabsubtitulo.WidthPercentage = 36.0f;
				float[] widths_tabsubtit = new float[] { 1f };	// controlando el ancho de cada columna tienen que sumas 315 en total
				tabsubtitulo.SetWidths(widths_tabsubtit);
				tabsubtitulo.HorizontalAlignment = 1;
								
				PdfPCell cellsubtit_col1;
				cellsubtit_col1 = new PdfPCell(new Phrase("INFORMACION DEL PACIENTE",new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 9, iTextSharp.text.Font.BOLD, BaseColor.BLACK)));
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
				
				PdfPTable tabFot1 = new PdfPTable(8);
				tabFot1.WidthPercentage = 100;
				float[] widths_tabfot1 = new float[] { 25f, 25f, 30f, 90f, 30f, 35f, 30f, 50f };	// controlando el ancho de cada columna tienen que sumas 315 en total
				tabFot1.SetWidths(widths_tabfot1);
				tabFot1.HorizontalAlignment = 0;
				cellcol1 = new PdfPCell(new Phrase("N° EXP.",_BoldFont));
				cellcol1.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER;
				cellcol2 = new PdfPCell(new Phrase(nro_expediente_px,_NormalFont));
				cellcol2.Border = iTextSharp.text.Rectangle.TOP_BORDER;
				cellcol2.HorizontalAlignment = 0;
				cellcol3 = new PdfPCell(new Phrase("Nombre PX.",_BoldFont));
				cellcol3.Border = iTextSharp.text.Rectangle.TOP_BORDER;
				cellcol4 = new PdfPCell(new Phrase(nombres_apellidos_px,_NormalFont));
				cellcol4.Border = iTextSharp.text.Rectangle.TOP_BORDER;
				cellcol4.HorizontalAlignment = 0;
				cellcol5 = new PdfPCell(new Phrase("Fech.Nac.",_BoldFont));
				cellcol5.Border = iTextSharp.text.Rectangle.TOP_BORDER;
				cellcol5.HorizontalAlignment = 2;
				cellcol6 = new PdfPCell(new Phrase(fecha_nacimiento_px,_NormalFont));
				cellcol6.Border = iTextSharp.text.Rectangle.TOP_BORDER;
				cellcol6.HorizontalAlignment = 0;
				cellcol7 = new PdfPCell(new Phrase("Edad",_BoldFont));
				cellcol7.Border = iTextSharp.text.Rectangle.TOP_BORDER;
				cellcol7.HorizontalAlignment = 2;
				cellcol8 = new PdfPCell(new Phrase(edad_px,_NormalFont));
				cellcol8.Border = iTextSharp.text.Rectangle.RIGHT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER;
				cellcol8.HorizontalAlignment = 0;
				tabFot1.AddCell(cellcol1);
				tabFot1.AddCell(cellcol2);
				tabFot1.AddCell(cellcol3);
				tabFot1.AddCell(cellcol4);
				tabFot1.AddCell(cellcol5);
				tabFot1.AddCell(cellcol6);
				tabFot1.AddCell(cellcol7);
				tabFot1.AddCell(cellcol8);
				documento.Add(tabFot1);
				
				PdfPTable tabFot2 = new PdfPTable(6);
				tabFot2.WidthPercentage = 100;
				float[] widths_tabfot2 = new float[] { 30f, 90f, 30f, 90f, 30f, 90f };	// controlando el ancho de cada columna tienen que sumas 315 en total
				tabFot2.SetWidths(widths_tabfot2);
				tabFot2.HorizontalAlignment = 0;
				cellcol1 = new PdfPCell(new Phrase("CURP.",_BoldFont));
				cellcol1.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER;
				cellcol2 = new PdfPCell(new Phrase(curp_px,_NormalFont));
				cellcol2.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
				cellcol2.HorizontalAlignment = 0;
				cellcol3 = new PdfPCell(new Phrase("Sexo",_BoldFont));
				cellcol3.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
				cellcol4 = new PdfPCell(new Phrase(sexo_px,_NormalFont));
				cellcol4.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
				cellcol4.HorizontalAlignment = 0;
				cellcol5 = new PdfPCell(new Phrase("Ocupacion",_BoldFont));
				cellcol5.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
				cellcol5.HorizontalAlignment = 2;
				cellcol6 = new PdfPCell(new Phrase(ocupacion_px,_NormalFont));
				cellcol6.Border = iTextSharp.text.Rectangle.RIGHT_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER;
				cellcol6.HorizontalAlignment = 0;
				tabFot2.AddCell(cellcol1);
				tabFot2.AddCell(cellcol2);
				tabFot2.AddCell(cellcol3);
				tabFot2.AddCell(cellcol4);
				tabFot2.AddCell(cellcol5);
				tabFot2.AddCell(cellcol6);
				documento.Add(tabFot2);
				
				// codificacion del codigo de barra numero de receta/numero de atencion/codigo del DR.
				iTextSharp.text.Image imagen2 = CreateBarcodeImage("1-17720-101");
				//imagen2.SetAbsolutePosition(15, 710);
				//imagen2.SetAbsolutePosition.
				imagen2.Alignment = Element.ALIGN_LEFT;
				percentage = 100 / imagen2.Width;
				imagen2.ScalePercent(percentage * 130);
				documento.Add(imagen2);
				//cb.AddImage(imagen2);
				
				documento.Add (new Paragraph (" ", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 2)));
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

