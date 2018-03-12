///////////////////////////////////////////////////////////
// project created on 24/10/2006 at 10:20 a
// Sistema Hospitalario OSIRIS
// Monterrey - Mexico
// created on 22/09/2010 at 17:20 p
// 				
// Autor    	: Ing. Daniel Olivares C. GTKPrint con Pango y Cairo arcangeldoc@openmailbox.org
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
/// //////////////////////////////////////////////////////
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
	public class rpt_solicitud_labrx
	{
		private static int pangoScale = 1024;
		private PrintOperation print;
		private double fontSize = 8.0;
		int escala_en_linux_windows;		// Linux = 1  Windows = 8
		int comienzo_linea = 05;
		int separacion_linea = 10;
		int numpage = 1;
		int linea_detalle1;
		int linea_detalle2;
		string departament;
		string agrupacion_lab_rx;
		string query_general = "SELECT osiris_his_solicitudes_labrx.area_quien_solicita,osiris_his_solicitudes_labrx.folio_de_solicitud," +
		                       "osiris_his_solicitudes_labrx.fechahora_solicitud,osiris_his_solicitudes_labrx.folio_de_servicio AS foliodeservicio,osiris_his_solicitudes_labrx.pid_paciente AS pidpaciente," +
		                       "osiris_his_solicitudes_labrx.id_quien_solicito,osiris_his_solicitudes_labrx.id_proveedor,osiris_his_solicitudes_labrx.id_producto,osiris_his_solicitudes_labrx.cantidad_solicitada," +
		                       "observaciones_solicitud,turno," +
		                       "nombre1_paciente || ' ' || nombre2_paciente || ' ' || apellido_paterno_paciente || ' ' || apellido_materno_paciente AS nombre_completo," +
		                       "to_char(osiris_his_paciente.fecha_nacimiento_paciente, 'dd-MM-yyyy') AS fechanacpaciente, " +
		                       "to_char(to_number(to_char(age('" + DateTime.Now.ToString ("yyyy-MM-dd hh:mm:ss") + "',osiris_his_paciente.fecha_nacimiento_paciente),'yyyy') ,'9999'),'9999') AS edadpaciente," +
		                       "to_char(to_number(to_char(age('" + DateTime.Now.ToString ("yyyy-MM-dd hh:mm:ss") + "',osiris_his_paciente.fecha_nacimiento_paciente),'MM'),'99'),'99') AS mesesedad," +
		                       "osiris_his_paciente.sexo_paciente,osiris_erp_cobros_enca.nombre_medico_tratante," +
		                       "osiris_erp_cobros_enca.id_habitacion,osiris_his_habitaciones.descripcion_cuarto,osiris_his_habitaciones.numero_cuarto,osiris_empleado.login_empleado," +
		                       "nombre1_empleado || ' ' || nombre2_empleado || ' ' || apellido_paterno_empleado || ' ' || apellido_materno_empleado AS nombresolicitante,osiris_erp_proveedores.descripcion_proveedor," +
								"osiris_his_tipo_pacientes.descripcion_tipo_paciente,"+
								"osiris_erp_cobros_enca.id_aseguradora,descripcion_aseguradora,"+
								"osiris_erp_cobros_enca.id_empresa,descripcion_empresa,"+
								"osiris_productos.descripcion_producto,descripcion_especialidad " +
								"FROM osiris_his_solicitudes_labrx,osiris_his_paciente,osiris_erp_cobros_enca,osiris_his_habitaciones,osiris_empleado,osiris_erp_proveedores,osiris_productos,osiris_his_medicos,osiris_his_tipo_especialidad,osiris_empresas,osiris_aseguradoras,osiris_his_tipo_pacientes " +
		                       "WHERE osiris_his_solicitudes_labrx.pid_paciente = osiris_his_paciente.pid_paciente " +
		                       "AND osiris_his_solicitudes_labrx.folio_de_servicio = osiris_erp_cobros_enca.folio_de_servicio " +
		                       "AND osiris_erp_cobros_enca.id_habitacion = osiris_his_habitaciones.id_habitacion " +
		                       "AND osiris_his_solicitudes_labrx.id_quien_solicito = osiris_empleado.login_empleado " +
		                       "AND osiris_his_solicitudes_labrx.id_proveedor = osiris_erp_proveedores.id_proveedor " +
		                       "AND osiris_his_solicitudes_labrx.id_producto = osiris_productos.id_producto " +
								"AND osiris_erp_cobros_enca.id_medico_tratante = osiris_his_medicos.id_medico " +
								"AND osiris_his_tipo_especialidad.id_especialidad = osiris_his_medicos.id_especialidad " +
								"AND osiris_erp_cobros_enca.id_empresa = osiris_empresas.id_empresa "+
								"AND osiris_erp_cobros_enca.id_aseguradora = osiris_aseguradoras.id_aseguradora "+
								"AND osiris_erp_cobros_enca.id_tipo_paciente = osiris_his_tipo_pacientes.id_tipo_paciente "+
		                       "AND osiris_his_solicitudes_labrx.eliminado = 'false' ";
		string diagnostico_movcargo = "";
		string nombrecirugia_movcargo = "";
		
		string connectionString;
        string nombrebd;
		
		class_public classpublic = new class_public();
		class_conexion conexion_a_DB = new class_conexion();
		
		protected Gtk.Window MyWinError;
		
		public rpt_solicitud_labrx (string departament_,int id_tipoadmisiones_,string agrupacion_lab_rx_,string numerosolicitud)
		{
			escala_en_linux_windows = classpublic.escala_linux_windows;
			connectionString = conexion_a_DB._url_servidor+conexion_a_DB._port_DB+conexion_a_DB._usuario_DB+conexion_a_DB._passwrd_user_DB;
			nombrebd = conexion_a_DB._nombrebd;
			departament = departament_;
			agrupacion_lab_rx = agrupacion_lab_rx_;
			query_general = query_general+"AND osiris_his_solicitudes_labrx.id_tipo_admisiones2 = '"+id_tipoadmisiones_.ToString().Trim()+"' "+"AND folio_de_solicitud = '"+numerosolicitud+"' ";
			string sexopaciente = "";
			int folioservicio = 0;

			iTextSharp.text.Font _NormalFont;
			iTextSharp.text.Font _BoldFont;
			_NormalFont = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 7, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);
			_BoldFont = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 7, iTextSharp.text.Font.BOLD, BaseColor.BLACK);


			NpgsqlConnection conexion; 
			conexion = new NpgsqlConnection (connectionString+nombrebd);
			// Verifica que la base de dato s este conectada
			try{
				conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand ();
				comando.CommandText = query_general;
				//Console.WriteLine(comando.CommandText);
				NpgsqlDataReader lector = comando.ExecuteReader ();
				if (lector.Read()){
					folioservicio = int.Parse(lector["foliodeservicio"].ToString());
					if (lector["sexo_paciente"].ToString().Trim() == "H"){
						sexopaciente = "MASCULINO";
					}else{
						sexopaciente = "FEMENINO";
					}
					// step 1: creation of a document-object
					Document documento = new Document(iTextSharp.text.PageSize.LETTER, 15, 15, 15, 15);
					//Document document = new Document(PageSize.A4.Rotate());
					string pdf_name = System.IO.Path.GetTempPath() + Guid.NewGuid().ToString() + ".pdf";
					try{
						PdfWriter writerpdf = PdfWriter.GetInstance(documento, new FileStream(pdf_name, FileMode.Create));	
						documento.AddTitle("Reporte de Abonos/Pagos por Paciente");
						documento.AddCreator("Sistema Hospitalario OSIRIS");
						documento.AddAuthor("Sistema Hospitalario OSIRIS");
						documento.AddSubject("OSIRSrpt");
						EventoTitulos ev = new EventoTitulos();
						ev.titulo1_rpt = departament+"-"+lector["turno"].ToString().Trim();
						ev.departamento_sol = lector["area_quien_solicita"].ToString().Trim();
						ev.numerosolicitud = lector["folio_de_solicitud"].ToString().Trim();
						ev.fechaenvio = lector["fechahora_solicitud"].ToString().Trim();
						ev.numero_atencion_px = lector["foliodeservicio"].ToString().Trim();
						ev.nro_expediente_px = lector["pidpaciente"].ToString().Trim();
						ev.nombres_apellidos_px = lector["nombre_completo"].ToString().Trim();
						ev.fechanac_px = lector["fechanacpaciente"].ToString().Trim();
						ev.edad_px = lector["edadpaciente"].ToString().Trim()+"/"+lector["mesesedad"].ToString().Trim();
						ev.sexo_px = sexopaciente;
						ev.tipode_px = lector["descripcion_tipo_paciente"].ToString().Trim();
						if((int) lector ["id_aseguradora"] > 1){
							ev.instempresa_px = lector["descripcion_aseguradora"].ToString().Trim();
						}else{
							ev.instempresa_px = lector["descripcion_empresa"].ToString().Trim();
						}
						ev.motivoingreso_px = (string) classpublic.lee_registro_de_tabla("osiris_erp_movcargos","folio_de_servicio","WHERE folio_de_servicio = '"+folioservicio.ToString().Trim()+"' ","descripcion_diagnostico_movcargos","string");
						ev.medico_tratante_px = lector["nombre_medico_tratante"].ToString().Trim();
						ev.especia_medtrat_px = lector["descripcion_especialidad"].ToString().Trim();
						ev.observacion_solicitud = lector["observaciones_solicitud"].ToString().Trim();
						ev.gabinete_proveedor = lector["descripcion_proveedor"].ToString().Trim();
						ev.habitacion_px = lector["descripcion_cuarto"].ToString().Trim()+" "+lector["numero_cuarto"].ToString().Trim();
						ev.procedimiento_qx = (string) classpublic.lee_registro_de_tabla("osiris_erp_movcargos","folio_de_servicio","WHERE folio_de_servicio = '"+folioservicio.ToString().Trim()+"' ","nombre_de_cirugia","string"); 
						ev.nom_solitante_estudio = lector["nombresolicitante"].ToString();
						ev.id_solitante_estudio = lector["id_quien_solicito"].ToString().Trim();
						writerpdf.PageEvent = ev;
						documento.Open();

						PdfPCell cellcol1;
						PdfPCell cellcol3;
						PdfPCell cellcol2;
						PdfPCell cellcol4;

						PdfPTable tblConceptos = new PdfPTable(4);
						tblConceptos.WidthPercentage = 100;
						float[] widthsconceptos = new float[] { 15f, 35f, 140f, 30f };	// controlando el ancho de cada columna tienen que sumas 315 en total
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
						cellcol4 = new PdfPCell(new Phrase("TOMA MUESTRA", _BoldFont));
						cellcol4.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
						cellcol4.HorizontalAlignment = 1;		// centro
						cellcol4.BackgroundColor = BaseColor.YELLOW;

						// Añadimos las celdas a la tabla
						tblConceptos.AddCell(cellcol1);
						tblConceptos.AddCell(cellcol2);
						tblConceptos.AddCell(cellcol3);
						tblConceptos.AddCell(cellcol4);

						// Configuramos el título de las columnas de la tabla
						cellcol1 = new PdfPCell(new Phrase(lector["cantidad_solicitada"].ToString().Trim(),_NormalFont));
						//clnroatencion.BorderWidth = 1;			// Ancho del borde
						cellcol1.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
						cellcol1.HorizontalAlignment = 1;		// centro
						cellcol2 = new PdfPCell(new Phrase(lector["id_producto"].ToString().Trim(),_NormalFont));
						cellcol2.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
						cellcol2.HorizontalAlignment = 1;		// centro
						cellcol3 = new PdfPCell(new Phrase(lector["descripcion_producto"].ToString().Trim(), _NormalFont));
						cellcol3.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
						cellcol3.HorizontalAlignment = 0;		// centro
						cellcol4 = new PdfPCell(new Phrase("", _NormalFont));
						cellcol4.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
						cellcol4.HorizontalAlignment = 1;		// centro
						// Añadimos las celdas a la tabla
						tblConceptos.AddCell(cellcol1);
						tblConceptos.AddCell(cellcol2);
						tblConceptos.AddCell(cellcol3);
						tblConceptos.AddCell(cellcol4);

						while (lector.Read()){
							// Configuramos el título de las columnas de la tabla
							cellcol1 = new PdfPCell(new Phrase(lector["cantidad_solicitada"].ToString().Trim(),_NormalFont));
							//clnroatencion.BorderWidth = 1;			// Ancho del borde
							cellcol1.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
							cellcol1.HorizontalAlignment = 1;		// centro
							cellcol2 = new PdfPCell(new Phrase(lector["id_producto"].ToString().Trim(),_NormalFont));
							cellcol2.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
							cellcol2.HorizontalAlignment = 1;		// centro
							cellcol3 = new PdfPCell(new Phrase(lector["descripcion_producto"].ToString().Trim(), _NormalFont));
							cellcol3.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
							cellcol3.HorizontalAlignment = 0;		// centro
							cellcol4 = new PdfPCell(new Phrase("", _NormalFont));
							cellcol4.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
							cellcol4.HorizontalAlignment = 1;		// centro	
							// Añadimos las celdas a la tabla
							tblConceptos.AddCell(cellcol1);
							tblConceptos.AddCell(cellcol2);
							tblConceptos.AddCell(cellcol3);
							tblConceptos.AddCell(cellcol4);
						}
						documento.Add(tblConceptos);
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
					MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
						MessageType.Error,ButtonsType.Close,"El numero de solicitud seleccionado NO EXISTE...");
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
				
		private class EventoTitulos : PdfPageEventHelper
		{
			class_public classpublic = new class_public();

			#region Fields
			private string _titulo1_rpt;
			private string _departamento_sol;
			private string _numerosolicitud;
			private string _fechaenvio;
			private string _numero_atencion_px;
			private string _nro_expediente_px;
			private string _nombres_apellidos_px;
			private string _fechanac_px;
			private string _edad_px;
			private string _sexo_px;
			private string _tipode_px;
			private string _instempresa_px;
			private string _motivoingreso_px;
			private string _medico_tratante_px;
			private string _especia_medtrat_px;
			private string _procedimiento_qx;
			private string _gabinete_proveedor;
			private string _habitacion_px;
			private string _observacion_solicitud;
			private string _nom_solitante_estudio;
			private string _id_solitante_estudio;
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
			public string departamento_sol
			{
				get{
					return _departamento_sol;
				}
				set{
					_departamento_sol = value;
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
			public string fechaenvio
			{
				get{
					return _fechaenvio;
				}
				set{
					_fechaenvio = value;
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
			public string procedimiento_qx
			{
				get{
					return _procedimiento_qx;
				}
				set{
					_procedimiento_qx = value;
				}
			}
			public string gabinete_proveedor
			{
				get{
					return _gabinete_proveedor;
				}
				set{
					_gabinete_proveedor = value;
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
			public string observacion_solicitud
			{
				get{
					return _observacion_solicitud;
				}
				set{
					_observacion_solicitud = value;
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
			public string id_solitante_estudio
			{
				get{
					return _id_solitante_estudio;
				}
				set{
					_id_solitante_estudio = value;
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
				cb.SetTextMatrix (130, 750);		cb.ShowText (classpublic.nombre_empresa);
				cb.SetTextMatrix (130, 740);		cb.ShowText (classpublic.direccion_empresa);
				cb.SetTextMatrix (130, 730);		cb.ShowText (classpublic.telefonofax_empresa);
				cb.SetFontAndSize (bf, 6);
				cb.SetTextMatrix (130, 720);		cb.ShowText ("Sistema Hospitalario OSIRIS");
				cb.SetTextMatrix (500, 750);		cb.ShowText ("Fech.Rpt:" + (string)DateTime.Now.ToString ("yyyy-MM-dd HH:mm:ss"));
				cb.SetTextMatrix (500, 740);		cb.ShowText ("N° Page :"+writerpdf.PageNumber.ToString("D8"));
				cb.SetFontAndSize (bf, 5);
				for (int f = 18; f <= 423; f += 135) {
					cb.SetTextMatrix (f, 440);					cb.ShowText (nombres_apellidos_px);
					cb.SetTextMatrix (f, 431);					cb.ShowText (edad_px);
					cb.SetTextMatrix (f, 422);					cb.ShowText (fechaenvio);
				}
				cb.EndText ();

				cb.MoveTo(0, documento.PageSize.Height/2);
				cb.SetLineWidth(0.05f);
				cb.LineTo(documento.PageSize.Width, documento.PageSize.Height / 2);
				cb.Stroke();

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

				documento.Add (new Paragraph (" ", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 5)));

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
				float[] widths_tabfot1 = new float[] { 40f, 40f, 40f, 40f, 40f, 40f };	// controlando el ancho de cada columna tienen que sumas 315 en total
				tabFot1.SetWidths(widths_tabfot1);
				tabFot1.HorizontalAlignment = 0;
				cellcol1 = new PdfPCell(new Phrase("Area quien Solicito",_BoldFont));
				cellcol1.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER ;
				cellcol2 = new PdfPCell(new Phrase(departamento_sol,_NormalFont));
				cellcol2.Border = iTextSharp.text.Rectangle.TOP_BORDER;
				cellcol3 = new PdfPCell(new Phrase("N° de Solicitud",_BoldFont));
				cellcol3.HorizontalAlignment = 2;	
				cellcol3.Border = iTextSharp.text.Rectangle.TOP_BORDER;
				cellcol4 = new PdfPCell(new Phrase(numerosolicitud,_NormalFont));
				cellcol4.Border = iTextSharp.text.Rectangle.TOP_BORDER;
				cellcol5 = new PdfPCell(new Phrase("Fecha Envio",_BoldFont));
				cellcol5.Border = iTextSharp.text.Rectangle.TOP_BORDER;
				cellcol5.HorizontalAlignment = 2;
				cellcol6 = new PdfPCell(new Phrase(fechaenvio,_NormalFont));
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
				float[] widths_tabfot2 = new float[] { 25f, 20f, 20f, 20f, 30f, 80f, 30f, 30f, 20f, 30f, 20f, 30f };	// controlando el ancho de cada columna tienen que sumas 315 en total
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
				float[] widths_tabfot3 = new float[] { 40f, 80f, 40f, 100f };	// controlando el ancho de cada columna tienen que sumas 315 en total
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

				PdfPTable tabFot4 = new PdfPTable(2);
				tabFot4.WidthPercentage = 100;
				float[] widths_tabfot4 = new float[] { 40f, 260f };	// controlando el ancho de cada columna tienen que sumas 315 en total
				tabFot4.SetWidths(widths_tabfot4);
				tabFot4.HorizontalAlignment = 0;
				cellcol1 = new PdfPCell(new Phrase("Motivo de Ingreso",_BoldFont));
				cellcol1.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
				cellcol2 = new PdfPCell(new Phrase(motivoingreso_px,_NormalFont));
				cellcol2.Border = iTextSharp.text.Rectangle.RIGHT_BORDER;
				cellcol2.HorizontalAlignment = 0;
				tabFot4.AddCell(cellcol1);
				tabFot4.AddCell(cellcol2);
				documento.Add(tabFot4);

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

				PdfPTable tabFot6 = new PdfPTable(2);
				tabFot6.WidthPercentage = 100;
				float[] widths_tabfot6 = new float[] { 50f, 250f };	// controlando el ancho de cada columna tienen que sumas 315 en total
				tabFot6.SetWidths(widths_tabfot6);
				tabFot6.HorizontalAlignment = 0;
				cellcol1 = new PdfPCell(new Phrase("Observacion Solicitud",_BoldFont));
				cellcol1.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
				cellcol2 = new PdfPCell(new Phrase(observacion_solicitud,_NormalFont));
				cellcol2.Border = iTextSharp.text.Rectangle.RIGHT_BORDER;
				cellcol2.HorizontalAlignment = 0;
				tabFot6.AddCell(cellcol1);
				tabFot6.AddCell(cellcol2);
				documento.Add(tabFot6);

				PdfPTable tabFot7 = new PdfPTable(4);
				tabFot7.WidthPercentage = 100;
				float[] widths_tabfot7 = new float[] { 40f, 120f, 40f, 100f };	// controlando el ancho de cada columna tienen que sumas 315 en total
				tabFot7.SetWidths(widths_tabfot7);
				tabFot7.HorizontalAlignment = 0;
				cellcol1 = new PdfPCell(new Phrase("Gabinete",_BoldFont));
				cellcol1.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
				cellcol2 = new PdfPCell(new Phrase(gabinete_proveedor,_NormalFont));
				cellcol2.Border = PdfPCell.NO_BORDER;
				cellcol2.HorizontalAlignment = 0;
				cellcol3 = new PdfPCell(new Phrase("N° Habitacion",_BoldFont));
				cellcol3.HorizontalAlignment = 2;		// derecha		
				cellcol3.Border = PdfPCell.NO_BORDER;
				cellcol4 = new PdfPCell(new Phrase(habitacion_px,_NormalFont));
				cellcol4.Border = iTextSharp.text.Rectangle.RIGHT_BORDER;
				cellcol4.HorizontalAlignment = 0;
				tabFot7.AddCell(cellcol1);
				tabFot7.AddCell(cellcol2);
				tabFot7.AddCell(cellcol3);
				tabFot7.AddCell(cellcol4);
				documento.Add(tabFot7);

				PdfPTable tabFot8 = new PdfPTable(2);
				tabFot8.WidthPercentage = 100;
				float[] widths_tabfot8 = new float[] { 40f, 260f };	// controlando el ancho de cada columna tienen que sumas 315 en total
				tabFot8.SetWidths(widths_tabfot8);
				tabFot8.HorizontalAlignment = 0;
				cellcol1 = new PdfPCell(new Phrase("Procedimiento QX.",_BoldFont));
				cellcol1.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
				cellcol2 = new PdfPCell(new Phrase(procedimiento_qx,_NormalFont));
				cellcol2.Border = iTextSharp.text.Rectangle.RIGHT_BORDER;
				cellcol2.HorizontalAlignment = 0;
				tabFot8.AddCell(cellcol1);
				tabFot8.AddCell(cellcol2);
				documento.Add(tabFot8);

				PdfPTable tabFot9 = new PdfPTable(4);
				tabFot9.WidthPercentage = 100;
				float[] widths_tabfot9 = new float[] { 40f, 120f, 40f, 100f };	// controlando el ancho de cada columna tienen que sumas 315 en total
				tabFot9.SetWidths(widths_tabfot9);
				tabFot9.HorizontalAlignment = 0;
				cellcol1 = new PdfPCell(new Phrase("Solicitante",_BoldFont));
				cellcol1.Border = iTextSharp.text.Rectangle.LEFT_BORDER  | iTextSharp.text.Rectangle.BOTTOM_BORDER;
				cellcol2 = new PdfPCell(new Phrase(nom_solitante_estudio,_NormalFont));
				cellcol2.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
				cellcol2.HorizontalAlignment = 0;
				cellcol3 = new PdfPCell(new Phrase("ID Usuario",_BoldFont));
				cellcol3.HorizontalAlignment = 2;		// derecha		
				cellcol3.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
				cellcol4 = new PdfPCell(new Phrase(id_solitante_estudio,_NormalFont));
				cellcol4.Border = iTextSharp.text.Rectangle.RIGHT_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER;
				cellcol4.HorizontalAlignment = 0;
				tabFot9.AddCell(cellcol1);
				tabFot9.AddCell(cellcol2);
				tabFot9.AddCell(cellcol3);
				tabFot9.AddCell(cellcol4);
				documento.Add(tabFot9);

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
