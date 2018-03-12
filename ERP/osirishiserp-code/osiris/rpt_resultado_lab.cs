/////////////////////////////////////////////////////////////
// project created on 01/10/2016 at 10:20 a
// Sistema Hospitalario OSIRIS
// Monterrey - Mexico
//
// Autor    	: Daniel Olivares - arcangeldoc@openmailbox.org (Programacion Mono)
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
using Cairo;
using Pango;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;

namespace osiris
{
	public class imprime_resultadolab
	{
		string connectionString;
        string nombrebd;
				
		//Declaracion de ventana de error
		protected Gtk.Window MyWinError;
		protected Gtk.Window MyWin;
		//public System.Drawing.Image myimage;
		
		class_conexion conexion_a_DB = new class_conexion();
		class_public classpublic = new class_public();
	
		public imprime_resultadolab (int nrosolicitud_,string idproducto_,int departamento_)
		{
			connectionString = conexion_a_DB._url_servidor+conexion_a_DB._port_DB+conexion_a_DB._usuario_DB+conexion_a_DB._passwrd_user_DB;
			nombrebd = conexion_a_DB._nombrebd;

			string sql_query_producto = "";
			string toma_idproducto;
			string nombre_quimico = "";
			string cedula_quimica = "";
			if (idproducto_ != null && idproducto_ != "") {
				sql_query_producto = "AND osiris_his_resultados_laboratorio.id_producto = '" + idproducto_ + "' ";
			}
			iTextSharp.text.Font _NormalFont;
			iTextSharp.text.Font _BoldFont;
			_NormalFont = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 7, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);
			_BoldFont = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 7, iTextSharp.text.Font.BOLD, BaseColor.BLACK);

			NpgsqlConnection conexion; 		
			conexion = new NpgsqlConnection (connectionString+nombrebd);
			try{
				conexion.Open ();
				NpgsqlCommand comando;			
				comando = conexion.CreateCommand ();
				comando.CommandText = "SELECT parametro,valor_referencia,resultado,unidades,"+
					"osiris_his_solicitudes_labrx.id_producto,osiris_his_solicitudes_labrx.pid_paciente,"+
					"folio_laboratorio,osiris_his_solicitudes_labrx.validado,to_char(osiris_his_solicitudes_labrx.fechahora_validacion,'yyyy-MM-dd HH24:mi') AS fechavalidacion,"+ 
					"observaciones_de_examen,osiris_his_solicitudes_labrx.fechahora_solicitud,osiris_his_solicitudes_labrx.folio_de_servicio," +
					"descripcion_producto,osiris_his_tipo_pacientes.descripcion_tipo_paciente,"+
					"osiris_his_paciente.nombre1_paciente || ' ' || osiris_his_paciente.nombre2_paciente || ' ' || osiris_his_paciente.apellido_paterno_paciente || ' ' || osiris_his_paciente.apellido_materno_paciente AS nombrepaciente," +
					"to_char(fecha_nacimiento_paciente,'yyyy-MM-dd') AS fech_nacimiento,sexo_paciente,"+
					"to_char(to_number(to_char(age('"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"',osiris_his_paciente.fecha_nacimiento_paciente),'yyyy') ,'9999'),'9999') AS edad,"+
					"osiris_erp_cobros_enca.id_aseguradora,descripcion_aseguradora,"+
					"osiris_erp_cobros_enca.id_empresa,descripcion_empresa,descripcion_admisiones,"+
					"nombre1_empleado || ' ' || nombre2_empleado || ' ' || apellido_paterno_empleado || ' ' || apellido_materno_empleado AS nombre_completo,cedula_profesional "+
					"FROM osiris_his_solicitudes_labrx,osiris_his_resultados_laboratorio,osiris_his_paciente,osiris_productos,osiris_erp_cobros_enca,osiris_his_tipo_pacientes,osiris_aseguradoras,osiris_empresas,osiris_his_tipo_admisiones,osiris_empleado,osiris_empleado_detalle "+
					"WHERE osiris_his_solicitudes_labrx.pid_paciente = osiris_his_paciente.pid_paciente " +
					"AND osiris_his_solicitudes_labrx.id_producto = osiris_productos.id_producto " +
					"AND osiris_his_solicitudes_labrx.id_tipo_admisiones = osiris_his_tipo_admisiones.id_tipo_admisiones "+
					"AND osiris_his_solicitudes_labrx.folio_de_servicio = osiris_erp_cobros_enca.folio_de_servicio " +

					"AND osiris_his_resultados_laboratorio.folio_laboratorio = osiris_his_solicitudes_labrx.folio_de_solicitud " +

					"AND osiris_erp_cobros_enca.id_tipo_paciente = osiris_his_tipo_pacientes.id_tipo_paciente "+
					"AND osiris_erp_cobros_enca.id_aseguradora = osiris_aseguradoras.id_aseguradora "+
					"AND osiris_erp_cobros_enca.id_empresa = osiris_empresas.id_empresa "+
					"AND osiris_his_resultados_laboratorio.id_quimico = osiris_empleado.id_empleado "+
					"AND osiris_empleado.id_empleado = osiris_empleado_detalle.id_empleado "+
					"AND osiris_his_solicitudes_labrx.validado = 'true' " +
					"AND osiris_his_solicitudes_labrx.id_tipo_admisiones2 = '"+departamento_.ToString().Trim()+"' "+
					"AND osiris_his_resultados_laboratorio.folio_laboratorio = '"+nrosolicitud_.ToString()+"' " +
					sql_query_producto +
					"ORDER BY osiris_his_solicitudes_labrx.id_producto,osiris_his_resultados_laboratorio.id_secuencia;"; 
				Console.WriteLine(comando.CommandText);
				NpgsqlDataReader lector = comando.ExecuteReader ();
				if (lector.Read()){
					toma_idproducto = lector["id_producto"].ToString().Trim();
					nombre_quimico = lector["nombre_completo"].ToString().Trim();
					cedula_quimica = lector["cedula_profesional"].ToString().Trim();
					// step 1: creation of a document-object
					Document documento = new Document(iTextSharp.text.PageSize.LETTER, 15, 15, 15, 15);
					//Document document = new Document(PageSize.A4.Rotate());
					string pdf_name = System.IO.Path.GetTempPath() + Guid.NewGuid().ToString() + ".pdf";
					try{
						PdfWriter writerpdf = PdfWriter.GetInstance(documento, new FileStream(pdf_name, FileMode.Create));	
						documento.AddTitle("Reporte Resultado de Laboratorio");
						documento.AddCreator("Sistema Hospitalario OSIRIS");
						documento.AddAuthor("Sistema Hospitalario OSIRIS");
						documento.AddSubject("OSIRSrpt");
						EventoTitulos ev = new EventoTitulos();
						ev.titulo1_rpt = "RESULTADOS DE LABORATORIO";
						ev.numerosolicitud = nrosolicitud_.ToString().Trim();
						ev.fechasolicitud = lector["fechahora_solicitud"].ToString().Trim();
						ev.fechavalidacion = "";
						ev.numero_atencion_px = lector["folio_de_servicio"].ToString().Trim();
						ev.nro_expediente_px =  lector["pid_paciente"].ToString().Trim();
						ev.nombres_apellidos_px = lector["nombrepaciente"].ToString().Trim();
						ev.fechanac_px = lector["fech_nacimiento"].ToString().Trim();
						ev.edad_px = lector["edad"].ToString().Trim();
						ev.nom_solitante_estudio = lector["descripcion_admisiones"].ToString().Trim();
						if (lector["sexo_paciente"].ToString().Trim() == "H"){
							ev.sexo_px = "HOMBRE";						
						}else{
							ev.sexo_px = "MUJER";
						}
						ev.tipode_px = lector["descripcion_tipo_paciente"].ToString().Trim();
						if((int) lector ["id_aseguradora"] > 1){
							ev.instempresa_px = lector["descripcion_aseguradora"].ToString().Trim();
						}else{
							ev.instempresa_px = lector["descripcion_empresa"].ToString().Trim();
						}
						writerpdf.PageEvent = ev;
						documento.Open();

						PdfPCell cellcol1;
						PdfPCell cellcol2;
						PdfPCell cellcol3;
						PdfPCell cellcol4;


						Paragraph nombre_estudio = new Paragraph(lector["descripcion_producto"].ToString().Trim(), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 9, iTextSharp.text.Font.BOLD, BaseColor.BLACK));
						nombre_estudio.Alignment = Element.ALIGN_CENTER;
						documento.Add(nombre_estudio);

						documento.Add (new Paragraph (" ", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 6)));

						PdfPTable tblConceptos = new PdfPTable(4);
						tblConceptos.WidthPercentage = 100;
						float[] widthsconceptos = new float[] { 100f, 90f, 50f, 100f };	// controlando el ancho de cada columna tienen que sumas 315 en total
						tblConceptos.SetWidths(widthsconceptos);
						tblConceptos.HorizontalAlignment = 0;

						// Configuramos el título de las columnas de la tabla
						cellcol1 = new PdfPCell(new Phrase("PARAMETROS", _BoldFont));
						//clnroatencion.BorderWidth = 1;			// Ancho del borde
						cellcol1.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
						cellcol1.HorizontalAlignment = 1;		// centro
						cellcol1.BackgroundColor = BaseColor.YELLOW;

						cellcol2 = new PdfPCell(new Phrase("RESULTADO", _BoldFont));
						cellcol2.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
						cellcol2.HorizontalAlignment = 1;		// centro
						cellcol2.BackgroundColor = BaseColor.YELLOW;

						cellcol3 = new PdfPCell(new Phrase("UNIDADES", _BoldFont));
						cellcol3.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
						cellcol3.HorizontalAlignment = 1;		// centro
						cellcol3.BackgroundColor = BaseColor.YELLOW;

						cellcol4 = new PdfPCell(new Phrase("V.R.", _BoldFont));
						cellcol4.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
						cellcol4.HorizontalAlignment = 1;		// centro
						cellcol4.BackgroundColor = BaseColor.YELLOW;

						// Añadimos las celdas a la tabla
						tblConceptos.AddCell(cellcol1);
						tblConceptos.AddCell(cellcol2);
						tblConceptos.AddCell(cellcol3);
						tblConceptos.AddCell(cellcol4);

						// Configuramos el título de las columnas de la tabla
						cellcol1 = new PdfPCell(new Phrase(lector["parametro"].ToString(), _NormalFont));
						//clnroatencion.BorderWidth = 1;			// Ancho del borde
						cellcol1.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
						cellcol1.HorizontalAlignment = 0;		// centro
						cellcol2 = new PdfPCell(new Phrase(lector["resultado"].ToString(), _NormalFont));
						cellcol2.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
						cellcol2.HorizontalAlignment = 0;		// centro
						cellcol3 = new PdfPCell(new Phrase(lector["unidades"].ToString(), _NormalFont));
						cellcol3.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
						cellcol3.HorizontalAlignment = 1;		// centro
						cellcol4 = new PdfPCell(new Phrase(lector["valor_referencia"].ToString(), _NormalFont));
						cellcol4.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
						cellcol4.HorizontalAlignment = 1;		// centro

						// Añadimos las celdas a la tabla
						tblConceptos.AddCell(cellcol1);
						tblConceptos.AddCell(cellcol2);
						tblConceptos.AddCell(cellcol3);
						tblConceptos.AddCell(cellcol4);

						while(lector.Read()){
							// Configuramos el título de las columnas de la tabla
							cellcol1 = new PdfPCell(new Phrase(lector["parametro"].ToString(), _NormalFont));
							//clnroatencion.BorderWidth = 1;			// Ancho del borde
							cellcol1.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
							cellcol1.HorizontalAlignment = 0;		// centro
							cellcol2 = new PdfPCell(new Phrase(lector["resultado"].ToString(), _NormalFont));
							cellcol2.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
							cellcol2.HorizontalAlignment = 0;		// centro
							cellcol3 = new PdfPCell(new Phrase(lector["unidades"].ToString(), _NormalFont));
							cellcol3.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
							cellcol3.HorizontalAlignment = 1;		// centro
							cellcol4 = new PdfPCell(new Phrase(lector["valor_referencia"].ToString(), _NormalFont));
							cellcol4.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
							cellcol4.HorizontalAlignment = 1;		// centro

							// Añadimos las celdas a la tabla
							tblConceptos.AddCell(cellcol1);
							tblConceptos.AddCell(cellcol2);
							tblConceptos.AddCell(cellcol3);
							tblConceptos.AddCell(cellcol4);
						}

						iTextSharp.text.Image firma_quimico = iTextSharp.text.Image.GetInstance(System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory,"OSIRISLogo2.png"));

						firma_quimico.BorderWidth = 0;
						firma_quimico.Alignment = Element.ALIGN_LEFT;
						float percentage = 0.0f;
						percentage = 150 / firma_quimico.Width;
						firma_quimico.ScalePercent(percentage * 65);
						firma_quimico.Alignment = Element.ALIGN_CENTER;

						documento.Add(tblConceptos);
						documento.Add (new Paragraph (" ", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 7)));
						documento.Add (new Paragraph (" ", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 7)));

						//Insertamos la imagen en el documento
						//documento.Add(firma_quimico);

						Paragraph firmareporte;
						firmareporte = new Paragraph("_________________________________", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 7, iTextSharp.text.Font.NORMAL, BaseColor.BLACK));
						firmareporte.Alignment = Element.ALIGN_CENTER;
						documento.Add(firmareporte);
						firmareporte = new Paragraph("Firma Quimico(a)", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 7, iTextSharp.text.Font.NORMAL, BaseColor.BLACK));
						firmareporte.Alignment = Element.ALIGN_CENTER;
						documento.Add(firmareporte);
						firmareporte = new Paragraph("NOMBRE: "+nombre_quimico, new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 7, iTextSharp.text.Font.NORMAL, BaseColor.BLACK));
						firmareporte.Alignment = Element.ALIGN_CENTER;
						documento.Add(firmareporte);
						firmareporte = new Paragraph("CEDULA: "+cedula_quimica, new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 7, iTextSharp.text.Font.NORMAL, BaseColor.BLACK));
						firmareporte.Alignment = Element.ALIGN_CENTER;
						documento.Add(firmareporte);
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
				//Console.WriteLine ("PostgresSQL error: {0}",ex.Message);
				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,MessageType.Info,ButtonsType.Close, "PostgresSQL error: {0}",ex.Message);
				msgBoxError.Run ();	msgBoxError.Destroy();
			}
			conexion.Close ();
		}

		private class EventoTitulos : PdfPageEventHelper
		{
			class_public classpublic = new class_public();

			#region Fields
			private string _titulo1_rpt;
			private string _numerosolicitud;
			private string _fechasolicitud;
			private string _fechavalidacion;
			private string _numero_atencion_px;
			private string _nro_expediente_px;
			private string _nombres_apellidos_px;
			private string _fechanac_px;
			private string _edad_px;
			private string _sexo_px;
			private string _tipode_px;
			private string _instempresa_px;
			private string _medico_tratante_px;
			private string _especia_medtrat_px;
			private string _nom_solitante_estudio;
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
			public string numerosolicitud
			{
				get{
					return _numerosolicitud;
				}
				set{
					_numerosolicitud = value;
				}
			}
			public string fechasolicitud
			{
				get{
					return _fechasolicitud;
				}
				set{
					_fechasolicitud = value;
				}
			}
			public string fechavalidacion
			{
				get{
					return _fechavalidacion;
				}
				set{
					_fechavalidacion = value;
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
			public string fechanac_px
			{
				get{
					return _fechanac_px;
				}
				set{
					_fechanac_px = value;
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
			public string sexo_px
			{
				get{
					return _sexo_px;
				}
				set{
					_sexo_px = value;
				}
			}
			public string tipode_px
			{
				get{
					return _tipode_px;
				}
				set{
					_tipode_px = value;
				}
			}
			public string instempresa_px
			{
				get{
					return _instempresa_px;
				}
				set{
					_instempresa_px = value;
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
			public string nom_solitante_estudio
			{
				get{
					return _nom_solitante_estudio;
				}
				set{
					_nom_solitante_estudio = value;
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

				PdfPTable tabFot1 = new PdfPTable(6);
				tabFot1.WidthPercentage = 100;
				float[] widths_tabfot1 = new float[] { 40f, 40f, 40f, 40f, 40f, 40f };
				tabFot1.SetWidths(widths_tabfot1);
				tabFot1.HorizontalAlignment = 0;
				cellcol1 = new PdfPCell(new Phrase("N° de Solicitud",_BoldFont));
				cellcol1.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER ;
				cellcol2 = new PdfPCell(new Phrase(numerosolicitud,_NormalFont));
				cellcol2.Border = iTextSharp.text.Rectangle.TOP_BORDER;
				cellcol3 = new PdfPCell(new Phrase("Fech. Solicitud",_BoldFont));
				cellcol3.HorizontalAlignment = 2;	
				cellcol3.Border = iTextSharp.text.Rectangle.TOP_BORDER;
				cellcol4 = new PdfPCell(new Phrase(fechasolicitud,_NormalFont));
				cellcol4.Border = iTextSharp.text.Rectangle.TOP_BORDER;
				cellcol5 = new PdfPCell(new Phrase("Fecha Validacion",_BoldFont));
				cellcol5.Border = iTextSharp.text.Rectangle.TOP_BORDER;
				cellcol5.HorizontalAlignment = 2;
				cellcol6 = new PdfPCell(new Phrase(fechavalidacion,_NormalFont));
				cellcol6.Border = iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
				cellcol6.HorizontalAlignment = 0;
				tabFot1.AddCell(cellcol1);
				tabFot1.AddCell(cellcol2);
				tabFot1.AddCell(cellcol3);
				tabFot1.AddCell(cellcol4);
				tabFot1.AddCell(cellcol5);
				tabFot1.AddCell(cellcol6);
				documento.Add(tabFot1);

				PdfPTable tabFot2 = new PdfPTable(12);
				tabFot2.WidthPercentage = 100;
				float[] widths_tabfot2 = new float[] { 25f, 20f, 20f, 20f, 30f, 80f, 30f, 30f, 20f, 30f, 20f, 30f };
				tabFot2.SetWidths(widths_tabfot2);
				tabFot2.HorizontalAlignment = 0;
				cellcol1 = new PdfPCell(new Phrase("N° Aten.",_BoldFont));
				cellcol1.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
				cellcol2 = new PdfPCell(new Phrase(numero_atencion_px,_NormalFont));
				cellcol2.Border = iTextSharp.text.Rectangle.NO_BORDER;
				cellcol3 = new PdfPCell(new Phrase("N° Exp.",_BoldFont));
				cellcol3.HorizontalAlignment = 2;	
				cellcol3.Border = iTextSharp.text.Rectangle.NO_BORDER;
				cellcol4 = new PdfPCell(new Phrase(nro_expediente_px,_NormalFont));
				cellcol4.Border = iTextSharp.text.Rectangle.NO_BORDER;
				cellcol5 = new PdfPCell(new Phrase("Nombre PX.",_BoldFont));
				cellcol5.Border = iTextSharp.text.Rectangle.NO_BORDER;
				cellcol5.HorizontalAlignment = 2;
				cellcol6 = new PdfPCell(new Phrase(nombres_apellidos_px,_NormalFont));
				cellcol6.Border = iTextSharp.text.Rectangle.NO_BORDER;
				cellcol6.HorizontalAlignment = 0;
				cellcol7 = new PdfPCell(new Phrase("Fec. Nac.",_BoldFont));
				cellcol7.Border = iTextSharp.text.Rectangle.NO_BORDER;
				cellcol7.HorizontalAlignment = 2;
				cellcol8 = new PdfPCell(new Phrase(fechanac_px,_NormalFont));
				cellcol8.Border = iTextSharp.text.Rectangle.NO_BORDER;
				cellcol8.HorizontalAlignment = 0;
				cellcol9 = new PdfPCell(new Phrase("Edad",_BoldFont));
				cellcol9.Border = iTextSharp.text.Rectangle.NO_BORDER;
				cellcol9.HorizontalAlignment = 2;
				cellcol10 = new PdfPCell(new Phrase(edad_px,_NormalFont));
				cellcol10.Border = iTextSharp.text.Rectangle.NO_BORDER;
				cellcol10.HorizontalAlignment = 0;
				cellcol11 = new PdfPCell(new Phrase("Sexo",_BoldFont));
				cellcol11.Border = iTextSharp.text.Rectangle.NO_BORDER;
				cellcol11.HorizontalAlignment = 2;
				cellcol12 = new PdfPCell(new Phrase(sexo_px,_NormalFont));
				cellcol12.Border = iTextSharp.text.Rectangle.RIGHT_BORDER;
				cellcol12.HorizontalAlignment = 0;

				tabFot2.AddCell(cellcol1);
				tabFot2.AddCell(cellcol2);
				tabFot2.AddCell(cellcol3);
				tabFot2.AddCell(cellcol4);
				tabFot2.AddCell(cellcol5);
				tabFot2.AddCell(cellcol6);
				tabFot2.AddCell(cellcol7);
				tabFot2.AddCell(cellcol8);
				tabFot2.AddCell(cellcol9);
				tabFot2.AddCell(cellcol10);
				tabFot2.AddCell(cellcol11);
				tabFot2.AddCell(cellcol12);
				documento.Add(tabFot2);

				PdfPTable tabFot3 = new PdfPTable(4);
				tabFot3.WidthPercentage = 100;
				float[] widths_tabfot3 = new float[] { 40f, 80f, 40f, 100f };
				tabFot3.SetWidths(widths_tabfot3);
				tabFot3.HorizontalAlignment = 0;
				cellcol1 = new PdfPCell(new Phrase("Tipo Paciente",_BoldFont));
				cellcol1.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
				cellcol2 = new PdfPCell(new Phrase(tipode_px,_NormalFont));
				cellcol2.Border = iTextSharp.text.Rectangle.NO_BORDER;
				cellcol2.HorizontalAlignment = 0;
				cellcol3 = new PdfPCell(new Phrase("Institucion/Empresa",_BoldFont));
				cellcol3.HorizontalAlignment = 2;	
				cellcol3.Border = iTextSharp.text.Rectangle.NO_BORDER;
				cellcol4 = new PdfPCell(new Phrase(instempresa_px,_NormalFont));
				cellcol4.Border = iTextSharp.text.Rectangle.RIGHT_BORDER;
				tabFot3.AddCell(cellcol1);
				tabFot3.AddCell(cellcol2);
				tabFot3.AddCell(cellcol3);
				tabFot3.AddCell(cellcol4);
				documento.Add(tabFot3);

				PdfPTable tabFot4 = new PdfPTable(4);
				tabFot4.WidthPercentage = 100;
				float[] widths_tabfot4 = new float[] { 40f, 120f, 40f, 100f };
				tabFot4.SetWidths(widths_tabfot4);
				tabFot4.HorizontalAlignment = 0;
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
				tabFot4.AddCell(cellcol1);
				tabFot4.AddCell(cellcol2);
				tabFot4.AddCell(cellcol3);
				tabFot4.AddCell(cellcol4);
				documento.Add(tabFot4);

				PdfPTable tabFot5 = new PdfPTable(2);
				tabFot5.WidthPercentage = 100;
				float[] widths_tabfot5 = new float[] { 40f, 250f };
				tabFot5.SetWidths(widths_tabfot5);
				tabFot5.HorizontalAlignment = 0;
				cellcol1 = new PdfPCell(new Phrase("Depto. Solicitante",_BoldFont));
				cellcol1.Border = iTextSharp.text.Rectangle.LEFT_BORDER  | iTextSharp.text.Rectangle.BOTTOM_BORDER;
				cellcol2 = new PdfPCell(new Phrase(nom_solitante_estudio,_NormalFont));
				cellcol2.Border = iTextSharp.text.Rectangle.RIGHT_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER;
				cellcol2.HorizontalAlignment = 0;
				tabFot5.AddCell(cellcol1);
				tabFot5.AddCell(cellcol2);
				documento.Add(tabFot5);

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