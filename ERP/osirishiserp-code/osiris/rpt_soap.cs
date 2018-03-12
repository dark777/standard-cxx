//
//  rpt_soap.cs
//
//  Author:
//       Daniel Olivares <arcangeldoc@openmailbox.org>
//
//  Copyright (c) 2014 dolivares
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
using Npgsql;
using System.Data;
using Gtk;
using Glade;
using Cairo;
using Pango;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;

namespace osiris
{
	public class rpt_soap
	{
		// Declarando variable publicas
		string connectionString;
		string nombrebd;
				
		string LoginEmpleado;
		string NomEmpleado;
		string AppEmpleado;
		string ApmEmpleado;
		string folioservicio;	        		// Toma el valor de numero de atencion de paciente
		string PidPaciente;		   			// Toma la actualizacion del pid del paciente
		string idespecilidad;
		string nombrepx;
		string edadpx;
		string fechanacpx;
		string sexopx; 
		string habitacionpx;
		string fechacreaexpe;
		string horacreaexpe;
		string direccionpx;
		string telefonopx;
		string tipopaciente;
		string convenio;
		
		
		Gtk.TreeView treeview_registro_soap = null;
		Gtk.TreeStore treeViewEngine_registro_soap = null;
		
		class_public classpublic = new class_public();
		class_conexion conexion_a_DB = new class_conexion();
		
		//Declaracion de ventana de error
		protected Gtk.Window MyWinError;

		public rpt_soap (string LoginEmp, string NomEmpleado_, string AppEmpleado_, string ApmEmpleado_,string numeroatencion, string expclinico, string idespecilidad_, 
		                 object _treeview_registro_soap_, object _treeViewEngine_registro_soap_,string nombrepx_, string edadpx_, 
		                 string fechanacpx_, string sexopx_, string habitacionpx_,string fechacreaexpe_,string horacreaexpe_,string direccionpx_,
		                 string telefonopx_, string tipopaciente_,string convenio_)
		{
			connectionString = conexion_a_DB._url_servidor+conexion_a_DB._port_DB+conexion_a_DB._usuario_DB+conexion_a_DB._passwrd_user_DB;
			nombrebd = conexion_a_DB._nombrebd;
			
			LoginEmpleado = LoginEmp;
			NomEmpleado = NomEmpleado_;
			AppEmpleado = AppEmpleado_;
			ApmEmpleado = ApmEmpleado_;
			treeview_registro_soap = _treeview_registro_soap_ as Gtk.TreeView;
			treeViewEngine_registro_soap = _treeViewEngine_registro_soap_ as Gtk.TreeStore;
			folioservicio = numeroatencion;
			PidPaciente = expclinico;
			idespecilidad = idespecilidad_;
			nombrepx = nombrepx_;
			edadpx = edadpx_;
			fechanacpx = fechanacpx_;
			sexopx = sexopx_;			
			habitacionpx = habitacionpx_;
			fechacreaexpe = fechacreaexpe_;
			horacreaexpe = horacreaexpe_;
			direccionpx = direccionpx_;
			telefonopx = telefonopx_;
			tipopaciente = tipopaciente_;
			convenio = convenio_;
			
			leyendo_treeview_soap();			
		}
				
		void leyendo_treeview_soap()
		{
			Gtk.TreeIter iter2;
			if(treeViewEngine_registro_soap.GetIterFirst (out iter2)){
				string pdf_name = System.IO.Path.GetTempPath() + Guid.NewGuid().ToString() + ".pdf";
				// step 1: creation of a document-object
				Document documento = new Document(iTextSharp.text.PageSize.LETTER, 15, 15, 15, 15);
				//Document document = new Document(PageSize.A4.Rotate());
						
				try{
					PdfWriter writerpdf = PdfWriter.GetInstance(documento, new FileStream(pdf_name, FileMode.Create));
					
					documento.AddTitle("Reporte de Notas Medicas (SOAP)");
			       	documento.AddCreator("Sistema Hospitalario OSIRIS");
			       	documento.AddAuthor("Sistema Hospitalario OSIRIS");
			       	documento.AddSubject("OSIRSrpt");	
					EventoTitulos ev = new EventoTitulos();
					ev.titulo1_rpt = "NOTA DE EVOLUCION (SOAP)";
					ev.nro_expediente_px = PidPaciente;
					ev.nombres_apellidos_px = nombrepx;
					ev.fecha_nacimiento_px = fechanacpx;
					ev.edad_px = edadpx;
					ev.sexo_px = sexopx;
					ev.direccion_px = direccionpx;
					ev.telefono_px = telefonopx;
					ev.nroatencion_px = folioservicio;
					ev.fechaatencion_px = "";
					writerpdf.PageEvent = ev;
					documento.Open();
					
					bool signosvitales = true;
										
					if((bool) treeview_registro_soap.Model.GetValue(iter2,1) == true){
						imprime_signos_vitales(writerpdf,documento,(string) treeview_registro_soap.Model.GetValue(iter2,0));
						signosvitales = !(bool) treeview_registro_soap.Model.GetValue(iter2,1);
						seleccion_campo_table(writerpdf,documento,(string) treeview_registro_soap.Model.GetValue(iter2,10),
							                      (string) treeview_registro_soap.Model.GetValue(iter2,13),
							                      false,
							                      (int) treeview_registro_soap.Model.GetValue(iter2,3));
							
						// anexa informacion anexesa al SOAP como observaciones en la tambla his_informacion_medica
						// se enlasa con tabla osiris_his_explfis_titulos    campo anexar_info_tablesoap    boleano    true
						if((bool) treeview_registro_soap.Model.GetValue(iter2,12) == true){
							seleccion_campo_table(writerpdf,documento,"osiris_his_informacion_medica",
								                      (string) treeview_registro_soap.Model.GetValue(iter2,13),
								                      true,
								                      (int) treeview_registro_soap.Model.GetValue(iter2,3));
						}
															
					}
					while(treeViewEngine_registro_soap.IterNext(ref iter2)){
						if((bool) treeview_registro_soap.Model.GetValue(iter2,1) == true){
							if(signosvitales == true){
								imprime_signos_vitales(writerpdf,documento,(string) treeview_registro_soap.Model.GetValue(iter2,0));
								signosvitales = !(bool) treeview_registro_soap.Model.GetValue(iter2,1);
							}
							seleccion_campo_table(writerpdf,documento,(string) treeview_registro_soap.Model.GetValue(iter2,10),
								                      (string) treeview_registro_soap.Model.GetValue(iter2,13),
								                      false,
								                      (int) treeview_registro_soap.Model.GetValue(iter2,3));
								
								
							// anexa informacion anexesa al SOAP como observaciones en la tambla his_informacion_medica
							// se enlasa con tabla osiris_his_explfis_titulos    campo anexar_info_tablesoap    boleano    true   
							if((bool) treeview_registro_soap.Model.GetValue(iter2,12) == true){
								seleccion_campo_table(writerpdf,documento,"osiris_his_informacion_medica",
									                      (string) treeview_registro_soap.Model.GetValue(iter2,13),
									                      true,
									                      (int) treeview_registro_soap.Model.GetValue(iter2,3));
							}							
						}
					}
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
		
		class EventoTitulos : PdfPageEventHelper
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
			private string _direccion_px;
			private string _telefono_px;
			private string _nroatencion_px;
			private string _fechaatencion_px;
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
			public string nroatencion_px
        	{
            	get{
					return _nroatencion_px;
				}
            	set{
					_nroatencion_px = value;
				}
        	}		
			public string fechaatencion_px
        	{
            	get{
					return _fechaatencion_px;
				}
            	set{
					_fechaatencion_px = value;
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
				cb.EndText ();
				documento.Add (new Paragraph (""));
				documento.Add (Chunk.NEWLINE);
								
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
				cellcol1.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
				cellcol2 = new PdfPCell(new Phrase(curp_px,_NormalFont));
				cellcol2.Border = PdfPCell.NO_BORDER;
				cellcol2.HorizontalAlignment = 0;
				cellcol3 = new PdfPCell(new Phrase("Sexo",_BoldFont));
				cellcol3.Border = PdfPCell.NO_BORDER;
				cellcol4 = new PdfPCell(new Phrase(sexo_px,_NormalFont));
				cellcol4.Border = PdfPCell.NO_BORDER;
				cellcol4.HorizontalAlignment = 0;
				cellcol5 = new PdfPCell(new Phrase("Ocupacion",_BoldFont));
				cellcol5.Border = PdfPCell.NO_BORDER;
				cellcol5.HorizontalAlignment = 2;
				cellcol6 = new PdfPCell(new Phrase(ocupacion_px,_NormalFont));
				cellcol6.Border = iTextSharp.text.Rectangle.RIGHT_BORDER;
				cellcol6.HorizontalAlignment = 0;
				tabFot2.AddCell(cellcol1);
				tabFot2.AddCell(cellcol2);
				tabFot2.AddCell(cellcol3);
				tabFot2.AddCell(cellcol4);
				tabFot2.AddCell(cellcol5);
				tabFot2.AddCell(cellcol6);
				documento.Add(tabFot2);				
				
				PdfPTable tabFot3 = new PdfPTable(4);
				tabFot3.WidthPercentage = 100;
				float[] widths_tabfot3 = new float[] { 25f, 200f, 40f, 45f };	// controlando el ancho de cada columna tienen que sumas 315 en total
				tabFot3.SetWidths(widths_tabfot3);
				tabFot3.HorizontalAlignment = 0;
				cellcol1 = new PdfPCell(new Phrase("Direccion ",_BoldFont));
				cellcol1.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER;
				cellcol2 = new PdfPCell(new Phrase(direccion_px,_NormalFont));
				cellcol2.Border = PdfPCell.NO_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER;
				cellcol2.HorizontalAlignment = 0;
				cellcol3 = new PdfPCell(new Phrase("Telefono",_BoldFont));
				cellcol3.HorizontalAlignment = 2;		// derecha		
				cellcol3.Border = PdfPCell.NO_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER;
				cellcol4 = new PdfPCell(new Phrase(telefono_px,_NormalFont));
				cellcol4.Border = iTextSharp.text.Rectangle.RIGHT_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER;
				cellcol4.HorizontalAlignment = 0;
				tabFot3.AddCell(cellcol1);
				tabFot3.AddCell(cellcol2);
				tabFot3.AddCell(cellcol3);
				tabFot3.AddCell(cellcol4);
				documento.Add(tabFot3);
				
				documento.Add (new Paragraph (" ", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 6)));
				
				PdfPTable tabsubtitulo2 = new PdfPTable(1);
				tabsubtitulo2.WidthPercentage = 36.0f;
				float[] widths_tabsubtit2 = new float[] { 1f };	// controlando el ancho de cada columna tienen que sumas 315 en total
				tabsubtitulo2.SetWidths(widths_tabsubtit2);
				tabsubtitulo2.HorizontalAlignment = 1;
				
				PdfPCell cellsubtit_col2;
				cellsubtit_col2 = new PdfPCell(new Phrase("N° DE ATENCION   "+ nroatencion_px ,new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 9, iTextSharp.text.Font.BOLD, BaseColor.BLACK)));
				cellsubtit_col2.Border = PdfPCell.NO_BORDER;
				cellsubtit_col2.CellEvent = new RoundedBorder();
				cellsubtit_col2.HorizontalAlignment = 1;
				tabsubtitulo2.AddCell(cellsubtit_col2);
				documento.Add(tabsubtitulo2);
				
				documento.Add (new Paragraph (" ", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 6)));
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
		
		/// <summary>
		/// Seleccion_campo_table the specified name_table_data, idsecuncia and padreehijo.
		/// </summary>
		/// <param name='name_table_data'>
		/// Name_table_data.
		/// </param>
		/// <param name='idsecuncia'>
		/// Idsecuncia.
		/// </param>
		/// <param name='padreehijo'>
		/// Padreehijo.
		/// </param>
		void seleccion_campo_table(PdfWriter writerpdf, Document documento,string name_table_data, string idsecuncia,bool padreehijo, int numeroatencion)
		{	
			string query_notas_evolucion = "";
			int idparametro;
			if("osiris_his_informacion_medica" == name_table_data){
				query_notas_evolucion = "SELECT to_char(fecha_anotacion,'yyyy-MM-dd') AS fechaanotacion,descripcion_titulo,pid_paciente,folio_de_servicio," +
								"osiris_his_informacion_medica.id_secuencia,osiris_his_informacion_medica.id_titulo_explfis AS idtituloexplfis," +
								"s_subjetivo,o_objetivo,a_analisis,p_plan,o_objetivo2,descripcion_especialidad,osiris_his_informacion_medica.id_empleado_creacion," +
								"osiris_his_medicos.nombre_medico,osiris_his_medicos.cedula_medico "+
								"FROM "+name_table_data+",osiris_his_explfis_titulos,osiris_his_tipo_especialidad,osiris_his_medicos " +
								"WHERE osiris_his_informacion_medica.id_titulo_explfis = osiris_his_explfis_titulos.id_titulo_explfis " +
								"AND "+name_table_data+".eliminado = 'false' "+
								"AND "+name_table_data+".id_especialidad = osiris_his_tipo_especialidad.id_especialidad " +
								"AND "+name_table_data+".id_empleado_creacion = osiris_his_medicos.login_empleado " +
								"AND pid_paciente = '" +PidPaciente.Trim()+"' " +
								"AND osiris_his_informacion_medica.secuencia_interna= '"+idsecuncia+"';";			
			}
			if("osiris_his_explfis_mov" == name_table_data){
				query_notas_evolucion = "SELECT to_char(fecha_anotacion,'yyyy-MM-dd') AS fechaanotacion,secuencia_interna,descripcion_parametro,id_parametro,notas_derecha," +
					"notas_izquierda,folio_de_servicio,descripcion_titulo,osiris_his_explfis_mov.id_titulo_explfis AS idtituloexplfis " +
					"FROM "+name_table_data+",osiris_his_explfis_titulos " +
					"WHERE osiris_his_explfis_mov.id_titulo_explfis = osiris_his_explfis_titulos.id_titulo_explfis " +
					"AND pid_paciente = '" +PidPaciente.Trim()+"' " +
					"AND secuencia_interna = '"+idsecuncia+"';"; 
			}
						
			NpgsqlConnection conexion;
			conexion = new NpgsqlConnection (connectionString+nombrebd);
            // Verifica que la base de datos este conectada
			try{
				conexion.Open ();
				NpgsqlCommand comando;
				comando = conexion.CreateCommand ();
				comando.CommandText = query_notas_evolucion;
				Console.WriteLine(comando.CommandText);
				NpgsqlDataReader lector = comando.ExecuteReader ();
				if(lector.Read()){
					if("osiris_his_informacion_medica" == name_table_data){
						
						if(padreehijo == false){
							imprime_soap(writerpdf,documento,
							             lector["fechaanotacion"].ToString().Trim(),
							             lector["folio_de_servicio"].ToString().Trim(),
							             lector["s_subjetivo"].ToString().ToUpper(),
							             lector["o_objetivo"].ToString().ToUpper(),
							             lector["a_analisis"].ToString().ToUpper(),
							             lector["p_plan"].ToString().ToUpper(),
							             lector["descripcion_especialidad"].ToString().ToUpper(),
							             lector["nombre_medico"].ToString().ToUpper(),
							             lector["cedula_medico"].ToString().ToUpper(),
							             tipopaciente,
							             convenio);
						}
						if(padreehijo == true){
							// opcion para mostrar observaciones de la especilidad medica
						}
					}
					if("osiris_his_explfis_mov" == name_table_data){
						idparametro = (int) lector["id_parametro"];
						
						// OPTOMETRIA
						if((int) lector["id_parametro"] == 1){
												
						}
						// OPTOMETRIA
						if((int) lector["id_parametro"] == 2){
							
						}							
						// OFTALMOLOGIA
						if((int) lector["id_parametro"] == 3){
								
						}					
					}
					while(lector.Read()){
						if("osiris_his_informacion_medica" == name_table_data){
							if(padreehijo == false){
								//ejecutar_consulta_reporte(context);
							}
							if(padreehijo == true){
								// opcion para mostrar observaciones de la especilidad medica
							}
						}
						if("osiris_his_explfis_mov" == name_table_data){
							idparametro = (int) lector["id_parametro"];
							// OPTOMETRIA
							if((int) lector["id_parametro"] == 1){
								
							}
							// OPTOMETRIA
							if((int) lector["id_parametro"] == 2){
								
							}							
							// OFTALMOLOGIA
							if((int) lector["id_parametro"] == 3){
								imprime_exploracion_fisica();
							}
						}
						if("osiris_his_somatometria" == name_table_data){
						
						}
					}	
					
				}
			}catch (NpgsqlException ex){
				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
										MessageType.Error,ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
				msgBoxError.Run ();				msgBoxError.Destroy();
			}
			conexion.Close ();
		}		
		
		void imprime_signos_vitales(PdfWriter writerpdf, Document documento,string fechaanotacion)
		{
			// fuente para las tablas creadas
			iTextSharp.text.Font _NormalFont;
			iTextSharp.text.Font _BoldFont;
			_NormalFont = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 7, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);
			_BoldFont = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 7, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
			
			PdfPCell cellcol1;
			PdfPCell cellcol3;
			PdfPCell cellcol2;
			PdfPCell cellcol4;
			PdfPCell cellcol5;
			PdfPCell cellcol6;
			PdfPCell cellcol7;
			PdfPCell cellcol8;
			PdfPCell cellcol9;
			
			iTextSharp.text.Paragraph p = new Paragraph ("SIGNOS VITALES", _BoldFont);
			p.Alignment = Element.ALIGN_LEFT;										
			documento.Add (p);
			documento.Add (new Paragraph (" ", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 2)));
			
			PdfPTable tabFot1 = new PdfPTable(9);
			tabFot1.WidthPercentage = 100;
			float[] widths_tabfot1 = new float[] { 35f, 25f, 25f, 25f, 25f, 25f, 25f, 25f, 60f };	// controlando el ancho de cada columna tienen que sumas 315 en total
			tabFot1.SetWidths(widths_tabfot1);
			tabFot1.HorizontalAlignment = 1;
			cellcol1 = new PdfPCell(new Phrase("Fecha",_BoldFont));
			cellcol1.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
			cellcol1.HorizontalAlignment = 1;
			cellcol2 = new PdfPCell(new Phrase("Hora",_BoldFont));
			cellcol2.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
			cellcol2.HorizontalAlignment = 1;
			cellcol3 = new PdfPCell(new Phrase("TA.",_BoldFont));
			cellcol3.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
			cellcol3.HorizontalAlignment = 1;
			cellcol4 = new PdfPCell(new Phrase("FC.",_BoldFont));
			cellcol4.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
			cellcol4.HorizontalAlignment = 1;
			cellcol5 = new PdfPCell(new Phrase("FR.",_BoldFont));
			cellcol5.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
			cellcol5.HorizontalAlignment = 1;
			cellcol6 = new PdfPCell(new Phrase("Temp.",_BoldFont));
			cellcol6.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
			cellcol6.HorizontalAlignment = 1;
			cellcol7 = new PdfPCell(new Phrase("Peso",_BoldFont));
			cellcol7.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
			cellcol7.HorizontalAlignment = 1;
			cellcol8= new PdfPCell(new Phrase("Talla",_BoldFont));
			cellcol8.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
			cellcol8.HorizontalAlignment = 1;
			cellcol9 = new PdfPCell(new Phrase("Depto.",_BoldFont));
			cellcol9.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
			cellcol9.HorizontalAlignment = 1;
			tabFot1.AddCell(cellcol1);
			tabFot1.AddCell(cellcol2);
			tabFot1.AddCell(cellcol3);
			tabFot1.AddCell(cellcol4);
			tabFot1.AddCell(cellcol5);
			tabFot1.AddCell(cellcol6);
			tabFot1.AddCell(cellcol7);
			tabFot1.AddCell(cellcol8);
			tabFot1.AddCell(cellcol9);
			//documento.Add(tabFot1);

			NpgsqlConnection conexion; 
			conexion = new NpgsqlConnection (connectionString+nombrebd);
			// Verifica que la base de datos este conectada
			try{
				conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand ();               	
				comando.CommandText = "SELECT folio_de_servicio,pid_paciente,to_char(fecha_somatometria,'yyyy-MM-dd') as fechasomatometria,hora_somatometria,tension_arterial,"+
					"pulso,frecuencia_respiratoria,temperatura,peso,talla,osiris_his_somatometria.id_tipo_admisiones,osiris_his_tipo_admisiones.descripcion_admisiones AS descripcionadmisiones "+
					"FROM osiris_his_somatometria,osiris_his_tipo_admisiones "+
					"WHERE osiris_his_somatometria.id_tipo_admisiones = osiris_his_tipo_admisiones.id_tipo_admisiones "+
					"AND folio_de_servicio = '"+folioservicio+"';";
				Console.WriteLine(comando.CommandText);
				NpgsqlDataReader lector = comando.ExecuteReader ();
				if(lector.Read()){
					cellcol1 = new PdfPCell(new Phrase(lector["fechasomatometria"].ToString().Trim(),_NormalFont));
					cellcol1.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
					cellcol1.HorizontalAlignment = 1;
					cellcol2 = new PdfPCell(new Phrase(lector["hora_somatometria"].ToString().Trim(),_NormalFont));
					cellcol2.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
					cellcol2.HorizontalAlignment = 1;
					cellcol3 = new PdfPCell(new Phrase(lector["tension_arterial"].ToString().Trim(),_NormalFont));
					cellcol3.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
					cellcol3.HorizontalAlignment = 1;
					cellcol4 = new PdfPCell(new Phrase(lector["pulso"].ToString().Trim(),_NormalFont));
					cellcol4.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
					cellcol4.HorizontalAlignment = 1;
					cellcol5 = new PdfPCell(new Phrase(lector["frecuencia_respiratoria"].ToString().Trim(),_NormalFont));
					cellcol5.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
					cellcol5.HorizontalAlignment = 1;
					cellcol6 = new PdfPCell(new Phrase(lector["temperatura"].ToString().Trim(),_NormalFont));
					cellcol6.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
					cellcol6.HorizontalAlignment = 1;
					cellcol7 = new PdfPCell(new Phrase(lector["peso"].ToString().Trim(),_NormalFont));
					cellcol7.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
					cellcol7.HorizontalAlignment = 1;
					cellcol8= new PdfPCell(new Phrase(lector["talla"].ToString().Trim(),_NormalFont));
					cellcol8.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
					cellcol8.HorizontalAlignment = 1;
					cellcol9 = new PdfPCell(new Phrase(lector["descripcionadmisiones"].ToString().Trim(),_NormalFont));
					cellcol9.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
					cellcol9.HorizontalAlignment = 1;
					tabFot1.AddCell(cellcol1);
					tabFot1.AddCell(cellcol2);
					tabFot1.AddCell(cellcol3);
					tabFot1.AddCell(cellcol4);
					tabFot1.AddCell(cellcol5);
					tabFot1.AddCell(cellcol6);
					tabFot1.AddCell(cellcol7);
					tabFot1.AddCell(cellcol8);
					tabFot1.AddCell(cellcol9);		
					//documento.Add(tabFot1);
					while(lector.Read()){
						cellcol1 = new PdfPCell(new Phrase(lector["fechasomatometria"].ToString().Trim(),_NormalFont));
						cellcol1.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
						cellcol1.HorizontalAlignment = 1;
						cellcol2 = new PdfPCell(new Phrase(lector["hora_somatometria"].ToString().Trim(),_NormalFont));
						cellcol2.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
						cellcol2.HorizontalAlignment = 1;
						cellcol3 = new PdfPCell(new Phrase(lector["tension_arterial"].ToString().Trim(),_NormalFont));
						cellcol3.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
						cellcol3.HorizontalAlignment = 1;
						cellcol4 = new PdfPCell(new Phrase(lector["pulso"].ToString().Trim(),_NormalFont));
						cellcol4.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
						cellcol4.HorizontalAlignment = 1;
						cellcol5 = new PdfPCell(new Phrase(lector["frecuencia_respiratoria"].ToString().Trim(),_NormalFont));
						cellcol5.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
						cellcol5.HorizontalAlignment = 1;
						cellcol6 = new PdfPCell(new Phrase(lector["temperatura"].ToString().Trim(),_NormalFont));
						cellcol6.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
						cellcol6.HorizontalAlignment = 1;
						cellcol7 = new PdfPCell(new Phrase(lector["peso"].ToString().Trim(),_NormalFont));
						cellcol7.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
						cellcol7.HorizontalAlignment = 1;
						cellcol8= new PdfPCell(new Phrase(lector["talla"].ToString().Trim(),_NormalFont));
						cellcol8.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
						cellcol8.HorizontalAlignment = 1;
						cellcol9 = new PdfPCell(new Phrase(lector["descripcionadmisiones"].ToString().Trim(),_NormalFont));
						cellcol9.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
						cellcol9.HorizontalAlignment = 1;
						tabFot1.AddCell(cellcol1);
						tabFot1.AddCell(cellcol2);
						tabFot1.AddCell(cellcol3);
						tabFot1.AddCell(cellcol4);
						tabFot1.AddCell(cellcol5);
						tabFot1.AddCell(cellcol6);
						tabFot1.AddCell(cellcol7);
						tabFot1.AddCell(cellcol8);
						tabFot1.AddCell(cellcol9);		
						//documento.Add(tabFot1);
					}
				}else{
					cellcol1 = new PdfPCell(new Phrase(" ",_NormalFont));
					cellcol1.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
					cellcol1.HorizontalAlignment = 1;
					cellcol2 = new PdfPCell(new Phrase(" ",_NormalFont));
					cellcol2.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
					cellcol2.HorizontalAlignment = 1;
					cellcol3 = new PdfPCell(new Phrase(" ",_NormalFont));
					cellcol3.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
					cellcol3.HorizontalAlignment = 1;
					cellcol4 = new PdfPCell(new Phrase(" ",_NormalFont));
					cellcol4.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
					cellcol4.HorizontalAlignment = 1;
					cellcol5 = new PdfPCell(new Phrase(" ",_NormalFont));
					cellcol5.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
					cellcol5.HorizontalAlignment = 1;
					cellcol6 = new PdfPCell(new Phrase(" ",_NormalFont));
					cellcol6.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
					cellcol6.HorizontalAlignment = 1;
					cellcol7 = new PdfPCell(new Phrase(" ",_NormalFont));
					cellcol7.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
					cellcol7.HorizontalAlignment = 1;
					cellcol8= new PdfPCell(new Phrase(" ",_NormalFont));
					cellcol8.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
					cellcol8.HorizontalAlignment = 1;
					cellcol9 = new PdfPCell(new Phrase(" ",_NormalFont));
					cellcol9.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
					cellcol9.HorizontalAlignment = 1;
					tabFot1.AddCell(cellcol1);
					tabFot1.AddCell(cellcol2);
					tabFot1.AddCell(cellcol3);
					tabFot1.AddCell(cellcol4);
					tabFot1.AddCell(cellcol5);
					tabFot1.AddCell(cellcol6);
					tabFot1.AddCell(cellcol7);
					tabFot1.AddCell(cellcol8);
					tabFot1.AddCell(cellcol9);		
					//documento.Add(tabFot1);
				}
				documento.Add(tabFot1);
			}catch (NpgsqlException ex){
				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
					MessageType.Error,ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
				msgBoxError.Run ();				msgBoxError.Destroy();
			}
			conexion.Close ();
			documento.Add (new Paragraph (" ", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 6)));
		}
		
		void imprime_soap(PdfWriter writerpdf, Document documento,string fechaanotacion,string foliodeservicio,string s_subjetivo,string o_objetivo,
		                  string a_analisis,string p_plan,string especialidadmed,string nombremedico,string cedulamedico,string tipopaciente_,string convenio_)
		{
			// fuente para las tablas creadas
			iTextSharp.text.Font _NormalFont;
			iTextSharp.text.Font _BoldFont;
			_NormalFont = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 7, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);
			_BoldFont = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 7, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
			
			PdfPCell cellcol1;
			PdfPCell cellcol3;
			PdfPCell cellcol2;
			PdfPCell cellcol4;
			
			iTextSharp.text.Paragraph p = new Paragraph ("S.O.A.P.", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 9, iTextSharp.text.Font.BOLD, BaseColor.BLACK));
			p.Alignment = Element.ALIGN_LEFT;										
			documento.Add (p);
			documento.Add (new Paragraph (" ", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 2)));
			
			PdfPTable tabFot1 = new PdfPTable(4);
			tabFot1.WidthPercentage = 100;
			float[] widths_tabfot1 = new float[] { 35f, 50f, 40f, 100f };	// controlando el ancho de cada columna tienen que sumas 315 en total
			tabFot1.SetWidths(widths_tabfot1);
			tabFot1.HorizontalAlignment = 0;
			cellcol1 = new PdfPCell(new Phrase("FECHA",_BoldFont));
			cellcol1.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
			cellcol1.HorizontalAlignment = 0;
			cellcol2 = new PdfPCell(new Phrase(fechaanotacion,_NormalFont));
			cellcol2.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER ;
			cellcol2.HorizontalAlignment = 0;
			cellcol3 = new PdfPCell(new Phrase("ESPECIALIDAD",_BoldFont));
			cellcol3.HorizontalAlignment = 2;		// derecha		
			cellcol3.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
			cellcol4 = new PdfPCell(new Phrase(especialidadmed,_NormalFont));
			cellcol4.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
			cellcol4.HorizontalAlignment = 0;
			tabFot1.AddCell(cellcol1);
			tabFot1.AddCell(cellcol2);
			tabFot1.AddCell(cellcol3);
			tabFot1.AddCell(cellcol4);
			documento.Add(tabFot1);			
						
			PdfPTable tabFot2 = new PdfPTable(4);
			tabFot2.WidthPercentage = 100;
			float[] widths_tabfot2 = new float[] { 35f, 50f, 40f, 100f };	// controlando el ancho de cada columna tienen que sumas 315 en total
			tabFot2.SetWidths(widths_tabfot2);
			tabFot2.HorizontalAlignment = 0;
			cellcol1 = new PdfPCell(new Phrase("TIPO DE PACIENTE",new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 8, iTextSharp.text.Font.BOLD, BaseColor.BLACK)));
			cellcol1.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
			cellcol1.HorizontalAlignment = 0;
			cellcol2 = new PdfPCell(new Phrase(tipopaciente_,new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 8, iTextSharp.text.Font.BOLD, BaseColor.BLACK)));
			cellcol2.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
			cellcol2.HorizontalAlignment = 0;
			cellcol3 = new PdfPCell(new Phrase("CONVENIO/EMPR.",new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 8, iTextSharp.text.Font.BOLD, BaseColor.BLACK)));
			cellcol3.HorizontalAlignment = 2;		// derecha		
			cellcol3.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
			cellcol4 = new PdfPCell(new Phrase(convenio_,new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 8, iTextSharp.text.Font.BOLD, BaseColor.BLACK)));
			cellcol4.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
			cellcol4.HorizontalAlignment = 0;
			cellcol4.BackgroundColor = BaseColor.YELLOW;
			tabFot2.AddCell(cellcol1);
			tabFot2.AddCell(cellcol2);
			tabFot2.AddCell(cellcol3);
			tabFot2.AddCell(cellcol4);
			documento.Add(tabFot2);			
						
			PdfPTable tabFot3 = new PdfPTable(2);
			tabFot3.WidthPercentage = 100;
			float[] widths_tabfot3 = new float[] { 48f, 260f };	// controlando el ancho de cada columna tienen que sumas 315 en total
			tabFot3.SetWidths(widths_tabfot3);
			tabFot3.HorizontalAlignment = 0;
			cellcol1 = new PdfPCell(new Phrase("Nomina",_BoldFont));
			cellcol1.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
			cellcol2 = new PdfPCell(new Phrase((string) llenado_movi_documentos(foliodeservicio),_NormalFont));
			cellcol2.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
			cellcol2.HorizontalAlignment = 0;
			tabFot3.AddCell(cellcol1);
			tabFot3.AddCell(cellcol2);
			documento.Add(tabFot3);
			
			documento.Add (new Paragraph (" ", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 2)));
			
			p = new Paragraph ("(S) Resumen del Interrogatorio (Subjetivos)", _BoldFont);
			p.Alignment = Element.ALIGN_LEFT;										
			documento.Add (p);
			documento.Add (new Paragraph (" ", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 2)));
			PdfPTable tabFot4 = new PdfPTable(1);
			tabFot4.WidthPercentage = 100;
			float[] widths_tabfot4 = new float[] { 315f };	// controlando el ancho de cada columna tienen que sumas 315 en total
			tabFot4.SetWidths(widths_tabfot4);
			tabFot4.HorizontalAlignment = 0;
			cellcol1 = new PdfPCell(new Phrase(s_subjetivo+System.Environment.NewLine+ " \n ",_NormalFont));
			cellcol1.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
			cellcol1.HorizontalAlignment = 0;
			tabFot4.AddCell(cellcol1);			
			documento.Add(tabFot4);
			documento.Add (new Paragraph (" ", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 2)));
			
			p = new Paragraph ("(O) Exploracion Fisica y/o Estudios Auxiliares (Objetivo)", _BoldFont);
			p.Alignment = Element.ALIGN_LEFT;										
			documento.Add (p);
			documento.Add (new Paragraph (" ", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 2)));
			PdfPTable tabFot5 = new PdfPTable(1);
			tabFot5.WidthPercentage = 100;
			float[] widths_tabfot5 = new float[] { 315f };	// controlando el ancho de cada columna tienen que sumas 315 en total
			tabFot5.SetWidths(widths_tabfot5);
			tabFot5.HorizontalAlignment = 0;
			cellcol1 = new PdfPCell(new Phrase(o_objetivo+System.Environment.NewLine+ " \n ",_NormalFont));
			cellcol1.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
			cellcol1.HorizontalAlignment = 0;
			tabFot5.AddCell(cellcol1);			
			documento.Add(tabFot5);
			documento.Add (new Paragraph (" ", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 2)));
			
			p = new Paragraph ("(A) Diagnóstico/Pronósticos (Analisis)", _BoldFont);
			p.Alignment = Element.ALIGN_LEFT;										
			documento.Add (p);
			documento.Add (new Paragraph (" ", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 2)));
			PdfPTable tabFot6 = new PdfPTable(1);
			tabFot6.WidthPercentage = 100;
			float[] widths_tabfot6 = new float[] { 315f };	// controlando el ancho de cada columna tienen que sumas 315 en total
			tabFot6.SetWidths(widths_tabfot4);
			tabFot6.HorizontalAlignment = 0;
			cellcol1 = new PdfPCell(new Phrase(a_analisis+System.Environment.NewLine+ " \n ",_NormalFont));
			cellcol1.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
			cellcol1.HorizontalAlignment = 0;
			tabFot6.AddCell(cellcol1);			
			documento.Add(tabFot6);
			documento.Add (new Paragraph (" ", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 2)));
			
			p = new Paragraph ("(P) Plan de manejo y tratamiento (Plan)", _BoldFont);
			p.Alignment = Element.ALIGN_LEFT;										
			documento.Add (p);
			documento.Add (new Paragraph (" ", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 2)));
			PdfPTable tabFot7 = new PdfPTable(1);
			tabFot7.WidthPercentage = 100;
			float[] widths_tabfot7 = new float[] { 315f };	// controlando el ancho de cada columna tienen que sumas 315 en total
			tabFot7.SetWidths(widths_tabfot7);
			tabFot7.HorizontalAlignment = 0;
			cellcol1 = new PdfPCell(new Phrase(p_plan+System.Environment.NewLine+ " \n ",_NormalFont));
			cellcol1.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
			cellcol1.HorizontalAlignment = 0;
			tabFot7.AddCell(cellcol1);			
			documento.Add(tabFot7);
			documento.Add (new Paragraph (" ", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 2)));
			documento.Add (new Paragraph ("MEDICO :"+nombremedico, new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 7)));
			documento.Add (new Paragraph ("CEDULA :"+cedulamedico, new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 7)));
			documento.Add (new Paragraph (" ", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 6)));
			documento.Add (new Paragraph ("Firma Doctor :______________________________________ ", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 7)));
			documento.Add (new Paragraph (" ", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 2)));
		}
		
		void imprime_exploracion_fisica()
		{
		
		}	
	}
}