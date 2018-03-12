//
//  rpt_citas_agenda.cs
//
//  Author:
//       Ing. Mauro Villanueva
//
//  Copyright (c) 2016
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
using System.Data;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;

namespace osiris
{
	public class rpt_citas_agenda
	{
		// Declarando variable publicas
		string connectionString;
		string nombrebd;

		class_conexion conexion_a_DB = new class_conexion();

		protected Gtk.Window MyWinError;

		public rpt_citas_agenda(int foliodecita_)
		{
			connectionString = conexion_a_DB._url_servidor+conexion_a_DB._port_DB+conexion_a_DB._usuario_DB+conexion_a_DB._passwrd_user_DB;
			nombrebd = conexion_a_DB._nombrebd;			
			string sexopaciente = "";
			string empresa_o_aseguradora = "";
			string titulo_de_pase = "";
			int comienzo_linea = 1;
			string query_slq = "SELECT to_char(fecha_programacion,'dd-MM-yyyy') AS fechaprogramacion,hora_programacion,id_numero_citaqx,osiris_his_calendario_citaqx.id_secuencia AS idsecuencia," +
				"osiris_his_calendario_citaqx.pid_paciente AS pidpaciente,osiris_his_paciente.nomina_paciente," +
				"osiris_his_calendario_citaqx.nombre_paciente," +
				"nombre1_paciente || ' ' || nombre2_paciente || ' ' || apellido_paterno_paciente || ' ' || apellido_materno_paciente AS nombre_completo," +
				"osiris_his_paciente.celular1_paciente,osiris_his_paciente.telefono_particular1_paciente AS telefonoparticular1_paciente," +
				"osiris_his_calendario_citaqx.celular1_paciente AS celular1paciente_cita,osiris_his_calendario_citaqx.telefono_paciente AS telefonopaciente_cita," +
				"osiris_his_paciente.email_paciente,osiris_his_calendario_citaqx.email_paciente AS emailpaciente_cita,osiris_his_calendario_citaqx.id_tipo_paciente," +
				"descripcion_tipo_paciente,osiris_his_calendario_citaqx.id_tipo_admisiones,descripcion_admisiones,osiris_his_medicos.id_medico,osiris_his_medicos.nombre_medico AS nombremedico," +
				"osiris_his_tipo_especialidad.id_especialidad,osiris_his_tipo_especialidad.descripcion_especialidad AS descripcionespecialidad,motivo_consulta," +
				"osiris_his_calendario_citaqx.observaciones AS observaciones_citaqx,referido_por,osiris_his_calendario_citaqx.cancelado AS cancelacitaqx," +
				"id_quiencreo_cita,osiris_his_calendario_citaqx.fechahora_creacion AS fechahoracreacion,motivo_cancelacion_citaqx," +
				"osiris_empresas.id_empresa AS idempresa,descripcion_empresa,osiris_aseguradoras.id_aseguradora AS idaseguradora," +
				"descripcion_aseguradora,osiris_his_calendario_citaqx.id_quiencreo_cita,osiris_his_paciente.departamento_paciente," +
				"to_char(to_number(to_char(age('"+DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss")+"',osiris_his_paciente.fecha_nacimiento_paciente),'yyyy') ,'9999'),'9999') AS edadpaciente,"+
				"osiris_his_calendario_citaqx.id_numero_citaqx AS folio_citaqx " +
				"FROM osiris_his_calendario_citaqx,osiris_his_paciente,osiris_his_tipo_pacientes,osiris_his_tipo_admisiones,osiris_his_medicos,osiris_his_tipo_especialidad,osiris_empresas,osiris_aseguradoras " +
				"WHERE osiris_his_calendario_citaqx.pid_paciente = osiris_his_paciente.pid_paciente " +
				"AND osiris_his_tipo_pacientes.id_tipo_paciente = osiris_his_calendario_citaqx.id_tipo_paciente " +
				"AND osiris_his_tipo_admisiones.id_tipo_admisiones = osiris_his_calendario_citaqx.id_tipo_admisiones " +
				"AND osiris_his_medicos.id_medico = osiris_his_calendario_citaqx.id_medico " +
				"AND osiris_his_tipo_especialidad.id_especialidad = osiris_his_calendario_citaqx.id_especialidad " +
				"AND osiris_empresas.id_empresa = osiris_his_calendario_citaqx.id_empresa " +
				"AND osiris_aseguradoras.id_aseguradora = osiris_his_calendario_citaqx.id_aseguradora " +
				"AND osiris_his_calendario_citaqx.cancelado = 'false' " +
				"AND id_numero_citaqx = '" + foliodecita_.ToString() + "' " +
				"ORDER BY to_char(fecha_programacion,'yyyy-MM-dd'),hora_programacion ASC;";
			titulo_de_pase = "CITA CON ESPECIALISTA";
			NpgsqlConnection conexion; 
			conexion = new NpgsqlConnection (connectionString+nombrebd);
			// Verifica que la base de datos este conectada
			try{
				conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand (); 
				comando.CommandText = query_slq;
				Console.WriteLine(comando.CommandText);
				NpgsqlDataReader lector = comando.ExecuteReader();
				if (lector.Read()){
					//buscar_en_movcargos(lector["foliodeservicio"].ToString().Trim());
					//if (lector["sexo_paciente"].ToString().Trim() == "H"){
					//	sexopaciente = "MASCULINO";
					//}else{
					//	sexopaciente = "FEMENINO";
					//}
					if ((int)lector["idaseguradora"] > 1)
					{
						empresa_o_aseguradora = (string) lector["descripcion_aseguradora"];
					}else{
						empresa_o_aseguradora = (string) lector["descripcion_empresa"];						
					}

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
						documento.AddTitle("Comprobante de CITA");
						documento.AddCreator("Sistema Hospitalario OSIRIS");
						documento.AddAuthor("Sistema Hospitalario OSIRIS");
						documento.AddSubject("OSIRSrpt");
						EventoTitulos_cita ev = new EventoTitulos_cita();
						ev.titulo1_rpt = "COMPROBANTE DE CITA";
						ev.tipodecita_px = lector["descripcion_admisiones"].ToString().Trim();
						ev.fechaprogramacion_px = lector["fechaprogramacion"].ToString().Trim();
						ev.horaprogramacion_px = lector["hora_programacion"].ToString().Trim();
						ev.nro_expediente_px = lector["pidpaciente"].ToString().Trim() + " / " + lector["nomina_paciente"].ToString().Trim();//
						ev.nombres_apellidos_px = lector["nombre_completo"].ToString().Trim();
						ev.folio_cita = lector["folio_citaqx"].ToString().Trim();
						ev.edad_px = lector["edadpaciente"].ToString().Trim();
						//ev.sexo_px = sexopaciente;
						ev.motivocita_px = lector["motivo_consulta"].ToString().Trim();
						ev.nombremedico_px = lector["nombremedico"].ToString().Trim();
						ev.especialidad_medico = lector["descripcionespecialidad"].ToString().Trim();
						ev.tipo_paciente_px = lector["descripcion_tipo_paciente"].ToString().Trim().ToUpper();
						ev.convenio_px = empresa_o_aseguradora;
						ev.idusuario = lector["id_quiencreo_cita"].ToString().Trim();
						//ev.nombredeusuario = lector["nombresolicitante"].ToString().Trim();
						ev.departamento_px = lector["departamento_paciente"].ToString().Trim();

						writerpdf.PageEvent = ev;
						documento.Open();
						//if(tipo_pase=="pase_de_ingreso"){
						//	layout.FontDescription.Weight = Weight.Bold;		// Letra negrita
						//	cr.MoveTo(05*escala_en_linux_windows, comienzo_linea*escala_en_linux_windows);		layout.SetText("Admitido a : "+areaquiensolicita);	Pango.CairoHelper.ShowLayout (cr, layout);
						//	layout.FontDescription.Weight = Weight.Normal;		// Letra normal
						//}

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

		class EventoTitulos_cita : PdfPageEventHelper
		{
			class_public classpublic = new class_public();

			#region Fields
			private string _titulo1_rpt;
			private string _tipodecita_px;
			private string _fechaprogramacion_px;
			private string _horaprogramacion_px;
			private string _nro_expediente_px;
			private string _nombres_apellidos_px;
			private string _folio_cita;
			private string _edad_px; 
			private string _sexo_px;
			private string _motivocita_px;
			private string _nombremedico_px;
			private string _especialidad_medico;
			private string _tipo_paciente_px;
			private string _convenio_px;
			private string _idusuario;
			private string _nombredeusuario;
			private string _departamento_px;
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
			public string tipodecita_px
			{
				get{
					return _tipodecita_px;
				}
				set{
					_tipodecita_px = value;
				}
			}
			public string fechaprogramacion_px
			{
				get{
					return _fechaprogramacion_px;
				}
				set{
					_fechaprogramacion_px = value;
				}
			}
			public string horaprogramacion_px
			{
				get{
					return _horaprogramacion_px;
				}
				set{
					_horaprogramacion_px = value;
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
			public string folio_cita
			{
				get{
					return _folio_cita;
				}
				set{
					_folio_cita = value;
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
			public string motivocita_px
			{
				get{
					return _motivocita_px;
				}
				set{
					_motivocita_px = value;
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
			public string especialidad_medico
			{
				get{
					return _especialidad_medico;
				}
				set{
					_especialidad_medico = value;
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
			public string idusuario
			{
				get{
					return _idusuario;
				}
				set{
					_idusuario = value;
				}
			}
			public string nombredeusuario
			{
				get{
					return _nombredeusuario;
				}
				set{
					_nombredeusuario = value;
				}
			}
			public string departamento_px
			{
				get
				{
					return _departamento_px;
				}
				set
				{
					_departamento_px = value;
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
				iTextSharp.text.Font _NormalFont2;
				iTextSharp.text.Font _BoldFont2;
				_NormalFont2 = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 9, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);
				_BoldFont2 = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 9, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
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

				PdfPCell cellcol1;
				PdfPCell cellcol3;
				PdfPCell cellcol2;
				PdfPCell cellcol4;
				PdfPCell cellcol5;
				PdfPCell cellcol6;


				PdfPTable tabFot1 = new PdfPTable(4);
				tabFot1.WidthPercentage = 100;
				float[] widths_tabfot1 = new float[] { 30f, 50f, 50f, 100f };	// controlando el ancho de cada columna tienen que sumas 315 en total
				tabFot1.SetWidths(widths_tabfot1);
				tabFot1.HorizontalAlignment = 0;
				cellcol1 = new PdfPCell(new Phrase("Fecha de la Cita",_BoldFont2));
				cellcol1.Border = iTextSharp.text.Rectangle.LEFT_BORDER  | iTextSharp.text.Rectangle.TOP_BORDER;
				cellcol2 = new PdfPCell(new Phrase(fechaprogramacion_px, _NormalFont2));
				cellcol2.Border = iTextSharp.text.Rectangle.TOP_BORDER;
				cellcol2.HorizontalAlignment = 0;
				cellcol3 = new PdfPCell(new Phrase("Hora de la cita",_BoldFont2));
				cellcol3.HorizontalAlignment = 2;		// derecha		
				cellcol3.Border = iTextSharp.text.Rectangle.TOP_BORDER;
				cellcol4 = new PdfPCell(new Phrase(horaprogramacion_px, _NormalFont2));
				cellcol4.Border = iTextSharp.text.Rectangle.RIGHT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER;
				cellcol4.HorizontalAlignment = 0;

				tabFot1.AddCell(cellcol1);
				tabFot1.AddCell(cellcol2);                
				tabFot1.AddCell(cellcol3);
				tabFot1.AddCell(cellcol4);			

				documento.Add(tabFot1);

				PdfPTable tabFot11 = new PdfPTable(1);
				tabFot11.WidthPercentage = 100;
				float[] widths_tabfot11 = new float[] { 150f };	// controlando el ancho de cada columna tienen que sumas 315 en total
				tabFot11.SetWidths(widths_tabfot11);
				tabFot11.HorizontalAlignment = 0;
				cellcol1 = new PdfPCell(new Phrase(" ", _BoldFont));

				cellcol1.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
				cellcol1.HorizontalAlignment = 0;
				tabFot11.AddCell(cellcol1);

				documento.Add(tabFot11);

				PdfPTable tabFot2 = new PdfPTable(6);
				tabFot2.WidthPercentage = 100;
				float[] widths_tabfot2 = new float[] { 40f, 90f, 50f, 50f, 50f, 170f };	// controlando el ancho de cada columna tienen que sumas 315 en total
				tabFot2.SetWidths(widths_tabfot2);
				tabFot2.HorizontalAlignment = 0;
				cellcol1 = new PdfPCell(new Phrase("N° de cita:",_BoldFont));
				cellcol1.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
				cellcol2 = new PdfPCell(new Phrase(folio_cita, _BoldFont));
				cellcol2.Border = iTextSharp.text.Rectangle.NO_BORDER;;
				cellcol2.HorizontalAlignment = 0;

				cellcol3 = new PdfPCell(new Phrase("#Exp. / #Nomina:", _BoldFont));
				cellcol3.Border = iTextSharp.text.Rectangle.NO_BORDER; ;
				cellcol4 = new PdfPCell(new Phrase(nro_expediente_px, _NormalFont));
				cellcol4.Border = iTextSharp.text.Rectangle.NO_BORDER; ;
				cellcol4.HorizontalAlignment = 0;

				cellcol5 = new PdfPCell(new Phrase("Nombre Paciente:",_BoldFont));
				cellcol5.Border = iTextSharp.text.Rectangle.NO_BORDER;;
				cellcol5.HorizontalAlignment = 2;
				cellcol6 = new PdfPCell(new Phrase(nombres_apellidos_px, _NormalFont));
				cellcol6.Border = iTextSharp.text.Rectangle.RIGHT_BORDER;
				cellcol6.HorizontalAlignment = 0;
				tabFot2.AddCell(cellcol1);
				tabFot2.AddCell(cellcol2);
				tabFot2.AddCell(cellcol3);
				tabFot2.AddCell(cellcol4);
				tabFot2.AddCell(cellcol5);
				tabFot2.AddCell(cellcol6);
				documento.Add(tabFot2);

				PdfPTable tabFot3 = new PdfPTable(6);
				tabFot3.WidthPercentage = 100;
				float[] widths_tabfot3 = new float[] { 50f, 90f, 30f, 30, 60f, 90f };	// controlando el ancho de cada columna tienen que sumas 315 en total
				tabFot3.SetWidths(widths_tabfot3);
				tabFot3.HorizontalAlignment = 0;
				cellcol1 = new PdfPCell(new Phrase("Departamento: ",_BoldFont));
				cellcol1.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
				cellcol2 = new PdfPCell(new Phrase(departamento_px, _NormalFont));
				cellcol2.Border = iTextSharp.text.Rectangle.NO_BORDER;
				cellcol2.HorizontalAlignment = 0;
				cellcol3 = new PdfPCell(new Phrase("Edad: ",_BoldFont));
				cellcol3.Border = iTextSharp.text.Rectangle.NO_BORDER;
				cellcol4 = new PdfPCell(new Phrase(edad_px,_NormalFont));
				cellcol4.Border = iTextSharp.text.Rectangle.NO_BORDER;
				cellcol4.HorizontalAlignment = 0;
				cellcol5 = new PdfPCell(new Phrase("Tipo de paciente: ",_BoldFont));
				cellcol5.Border = iTextSharp.text.Rectangle.NO_BORDER;
				cellcol5.HorizontalAlignment = 2;
				cellcol6 = new PdfPCell(new Phrase(tipo_paciente_px, _NormalFont));
				cellcol6.Border = iTextSharp.text.Rectangle.RIGHT_BORDER;
				cellcol6.HorizontalAlignment = 0;

				tabFot3.AddCell(cellcol1);
				tabFot3.AddCell(cellcol2);
				tabFot3.AddCell(cellcol3);
				tabFot3.AddCell(cellcol4);
				tabFot3.AddCell(cellcol5);
				tabFot3.AddCell(cellcol6);
				documento.Add(tabFot3);

				documento.Add(tabFot11);

				PdfPTable tabFot4 = new PdfPTable(2);
				tabFot4.WidthPercentage = 100;
				float[] widths_tabfot4 = new float[] { 30f, 210f };	// controlando el ancho de cada columna tienen que sumas 315 en total
				tabFot4.SetWidths(widths_tabfot4);
				tabFot4.HorizontalAlignment = 0;
				cellcol1 = new PdfPCell(new Phrase("Motivo de Consulta: ",_BoldFont));
				cellcol1.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
				cellcol2 = new PdfPCell(new Phrase(motivocita_px, _NormalFont));
				cellcol2.Border = iTextSharp.text.Rectangle.RIGHT_BORDER;
				cellcol2.HorizontalAlignment = 0;
				tabFot4.AddCell(cellcol1);
				tabFot4.AddCell(cellcol2);
				documento.Add(tabFot4);

				PdfPTable tabFot5 = new PdfPTable(4);
				tabFot5.WidthPercentage = 100;
				float[] widths_tabfot5 = new float[] { 60f, 140f, 140f, 60f };	// controlando el ancho de cada columna tienen que sumas 315 en total
				tabFot5.SetWidths(widths_tabfot5);
				tabFot5.HorizontalAlignment = 0;
				cellcol1 = new PdfPCell(new Phrase("Tiene cita con el Dr. : ", _BoldFont));
				cellcol1.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
				cellcol2 = new PdfPCell(new Phrase(nombremedico_px,_NormalFont));
				cellcol2.Border = iTextSharp.text.Rectangle.NO_BORDER;
				cellcol2.HorizontalAlignment = 0;
				cellcol3 = new PdfPCell(new Phrase("Tipo de cita: ",_BoldFont));
				cellcol3.HorizontalAlignment = 2;		// derecha		
				cellcol3.Border = iTextSharp.text.Rectangle.NO_BORDER;
				cellcol4 = new PdfPCell(new Phrase(tipodecita_px, _NormalFont));
				cellcol4.Border = iTextSharp.text.Rectangle.RIGHT_BORDER;
				cellcol4.HorizontalAlignment = 0;								
				tabFot5.AddCell(cellcol1);
				tabFot5.AddCell(cellcol2);
				tabFot5.AddCell(cellcol3);
				tabFot5.AddCell(cellcol4);				
				documento.Add(tabFot5);

				PdfPTable tabFot6 = new PdfPTable(2);
				tabFot6.WidthPercentage = 100;
				float[] widths_tabfot6 = new float[] { 10f, 60f };	// controlando el ancho de cada columna tienen que sumas 315 en total
				tabFot6.SetWidths(widths_tabfot6);
				tabFot6.HorizontalAlignment = 0;
				cellcol1 = new PdfPCell(new Phrase("Especialidad: ",_BoldFont));
				cellcol1.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
				cellcol2 = new PdfPCell(new Phrase(especialidad_medico, _BoldFont));
				cellcol2.Border = iTextSharp.text.Rectangle.RIGHT_BORDER;
				cellcol2.HorizontalAlignment = 0;									
				tabFot6.AddCell(cellcol1);
				tabFot6.AddCell(cellcol2);							
				documento.Add(tabFot6);

				documento.Add(tabFot11);

				PdfPTable tabFot7 = new PdfPTable(4);
				tabFot7.WidthPercentage = 100;
				float[] widths_tabfot7 = new float[] { 40f, 70f, 50f, 150f };	// controlando el ancho de cada columna tienen que sumas 315 en total
				tabFot7.SetWidths(widths_tabfot7);
				tabFot7.HorizontalAlignment = 0;
				cellcol1 = new PdfPCell(new Phrase("Usuario",_BoldFont));
				cellcol1.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER;
				cellcol2 = new PdfPCell(new Phrase(idusuario,_NormalFont));
				cellcol2.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
				cellcol2.HorizontalAlignment = 0;
				cellcol3 = new PdfPCell(new Phrase("Nombre Usuario",_NormalFont));
				cellcol3.HorizontalAlignment = 2;		// derecha		
				cellcol3.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
				cellcol4 = new PdfPCell(new Phrase(nombredeusuario,_BoldFont));
				cellcol4.Border = iTextSharp.text.Rectangle.RIGHT_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER;
				cellcol4.HorizontalAlignment = 0;								
				tabFot7.AddCell(cellcol1);
				tabFot7.AddCell(cellcol2);
				tabFot7.AddCell(cellcol3);
				tabFot7.AddCell(cellcol4);				
				documento.Add(tabFot7);

				PdfPTable tabFot8 = new PdfPTable(1);
				tabFot8.WidthPercentage = 100;
				float[] widths_tabfot8 = new float[] { 150f };	// controlando el ancho de cada columna tienen que sumas 315 en total
				tabFot8.SetWidths(widths_tabfot8);
				tabFot8.HorizontalAlignment = 0;
				cellcol1 = new PdfPCell(new Phrase("  ", _BoldFont));
				cellcol1.Border = iTextSharp.text.Rectangle.NO_BORDER;
				cellcol1.HorizontalAlignment = 0;
				tabFot8.AddCell(cellcol1);                
				documento.Add(tabFot8);
				documento.Add(tabFot8);

				PdfPTable tabFot9 = new PdfPTable(1);
				tabFot9.WidthPercentage = 100;
				float[] widths_tabfot9 = new float[] { 150f };	// controlando el ancho de cada columna tienen que sumas 315 en total
				tabFot9.SetWidths(widths_tabfot9);
				tabFot9.HorizontalAlignment = 0;
				cellcol1 = new PdfPCell(new Phrase(" ", _BoldFont));
				cellcol1.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
				cellcol1.HorizontalAlignment = 0;
				tabFot9.AddCell(cellcol1);
				documento.Add(tabFot9);

				PdfPTable tabFot10 = new PdfPTable(1);
				tabFot10.WidthPercentage = 100;
				float[] widths_tabfot10 = new float[] { 150f };	// controlando el ancho de cada columna tienen que sumas 315 en total
				tabFot10.SetWidths(widths_tabfot10);
				tabFot10.HorizontalAlignment = 0;
				cellcol1 = new PdfPCell(new Phrase("Para cualquier información y/o cancelacion de su cita favor de comunicarse al " + classpublic.telefonofax_empresa.ToString().Trim(), _BoldFont2));                
				cellcol1.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
				cellcol1.HorizontalAlignment = 1;
				tabFot10.AddCell(cellcol1);
				documento.Add(tabFot10);

				documento.Add(tabFot11);

				PdfPTable tabFot16 = new PdfPTable(1);
				tabFot16.WidthPercentage = 100;
				float[] widths_tabfot16 = new float[] { 150f };	// controlando el ancho de cada columna tienen que sumas 315 en total
				tabFot16.SetWidths(widths_tabfot16);
				tabFot16.HorizontalAlignment = 0;
				cellcol1 = new PdfPCell(new Phrase(" ", _BoldFont));
				cellcol1.Border =
					cellcol1.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER;
				cellcol1.HorizontalAlignment = 0;
				tabFot16.AddCell(cellcol1);
				documento.Add(tabFot16);

				documento.Add (new Paragraph (" ", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 5)));
			}
		}
	}
}