// created on 07/02/2008 at 09:34 a
// Sistema Hospitalario OSIRIS
// Monterrey - Mexico
//
// Autor    : Ing. Daniel Olivares C. (Adecuaciones y mejoras) arcangeldoc@openmailbox.org 03/06/2010
// 			  Traspaso a formato .PDF OCT. 2015 iTextSharp
// 
// Licencia : GLP
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
// Programa  :
// Proposito :
// Objeto    :
//////////////////////////////////////////////////////

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
	public class rpt_no_ingreso_caja
	{
		//declarando la ventana de rango de fechas
		[Widget] Gtk.Window rango_de_fecha;
		[Widget] Gtk.Entry entry_dia1;
		[Widget] Gtk.Entry entry_mes1;
		[Widget] Gtk.Entry entry_ano1;
		[Widget] Gtk.Entry entry_dia2;
		[Widget] Gtk.Entry entry_mes2;
		[Widget] Gtk.Entry entry_ano2;
		[Widget] Gtk.CheckButton  checkbutton_impr_todo_proce;
		[Widget] Gtk.CheckButton checkbutton_todos_los_clientes;
		[Widget] Gtk.Entry entry_referencia_inicial;
		[Widget] Gtk.Button button_imprime_rangofecha;
		[Widget] Gtk.Button button_salir;
		
		protected Gtk.Window MyWinError;
		
		private static int pangoScale = 1024;
		private PrintOperation print;
		private double fontSize = 8.0;
		int escala_en_linux_windows; // Linux = 1  Windows = 8
		int comienzo_linea = 70;
		int separacion_linea = 10;
		int numpage = 1;
		
		PrintContext context;
		Pango.FontDescription desc = Pango.FontDescription.FromString ("Sans");
		
		string connectionString;
		string nombrebd;
		
		string tipo_salida;
				   
		class_conexion conexion_a_DB = new class_conexion();
		class_buscador classfind_data = new class_buscador();
		class_public classpublic = new class_public();
		                              
		/// <summary>
		/// Genera reporte inteligente de consulta
		/// </summary>
		/// <param name="nombrebd_">
		/// A <see cref="System.String"/>
		/// </param>
		/// <param name="tipo_reporte impresora o archivo">
		/// A <see cref="System.String"/>
		/// </param>
		public rpt_no_ingreso_caja (string tipo_salida_)
		{
			tipo_salida = tipo_salida_;
			connectionString = conexion_a_DB._url_servidor+conexion_a_DB._port_DB+conexion_a_DB._usuario_DB+conexion_a_DB._passwrd_user_DB;
			nombrebd = conexion_a_DB._nombrebd;
			escala_en_linux_windows = classpublic.escala_linux_windows;
			//tipo_de_salida = tipo_de_salida_;
			//crea la ventana de glade
			Glade.XML  gxml = new Glade.XML  (null, "caja.glade", "rango_de_fecha", null);
			gxml.Autoconnect  (this);
			rango_de_fecha.Show();			
			
			checkbutton_impr_todo_proce.Label = "Imprime TODO";
			entry_referencia_inicial.IsEditable = false;
			entry_referencia_inicial.Text = DateTime.Now.ToString("dd-MM-yyyy");			
			
			entry_dia1.KeyPressEvent += onKeyPressEvent;
			entry_mes1.KeyPressEvent += onKeyPressEvent;
			entry_ano1.KeyPressEvent += onKeyPressEvent;
			entry_dia2.KeyPressEvent += onKeyPressEvent;
			entry_mes2.KeyPressEvent += onKeyPressEvent;
			entry_ano2.KeyPressEvent += onKeyPressEvent;
			entry_dia1.Text =DateTime.Now.ToString("dd");
			entry_mes1.Text =DateTime.Now.ToString("MM");
			entry_ano1.Text =DateTime.Now.ToString("yyyy");
			entry_dia2.Text =DateTime.Now.ToString("dd");
			entry_mes2.Text =DateTime.Now.ToString("MM");
			entry_ano2.Text =DateTime.Now.ToString("yyyy");
			//facturados = "CERRADOS";
			
			button_imprime_rangofecha.Clicked += new EventHandler(on_button_rpt_print_report_noingresa_caja);
			button_salir.Clicked += new EventHandler(on_cierraventanas_clicked);
			//llenado_combobox();
		}
		
		
		// cierra ventanas emergentes
		void on_cierraventanas_clicked(object sender, EventArgs a)
		{
			Widget win = (Widget) sender;
			win.Toplevel.Destroy();
		}
		
		void on_button_rpt_print_report_noingresa_caja(object sender, EventArgs a)
		{
			if(tipo_salida != "pdf"){
				print = new PrintOperation ();
				print.JobName = "Reporte de paciente ingresado que no pasaron por caja";
				print.BeginPrint += new BeginPrintHandler (OnBeginPrint);
				print.DrawPage += new DrawPageHandler (OnDrawPage);
				print.EndPrint += new EndPrintHandler (OnEndPrint);
				print.Run (PrintOperationAction.PrintDialog, null);
			}else{
				crea_rpt_pdf();
			}
		}
		
		void crea_rpt_pdf()
		{
			int folioservicio = 0;
			string empresa_aseguradora = "";
			string tipopaciente = "";
			NpgsqlConnection conexion;
			conexion = new NpgsqlConnection (connectionString+nombrebd);
			// Verifica que la base de datos este conectada
			try{
				conexion.Open ();
				NpgsqlCommand comando;
				comando = conexion.CreateCommand ();
					            
				string query_fechas = " AND to_char(osiris_erp_cobros_enca.fechahora_creacion,'yyyy-MM-dd') >= '"+entry_ano1.Text+"-"+entry_mes1.Text+"-"+entry_dia1.Text+"' "+
										" AND to_char(osiris_erp_cobros_enca.fechahora_creacion,'yyyy-MM-dd') <= '"+entry_ano2.Text+"-"+entry_mes2.Text+"-"+entry_dia2.Text+"' ";								
				comando.CommandText = "SELECT osiris_erp_cobros_enca.folio_de_servicio,osiris_erp_cobros_enca.pid_paciente AS pidpaciente, " +
											"osiris_his_paciente.nombre1_paciente || ' ' || osiris_his_paciente.nombre2_paciente || ' ' || osiris_his_paciente.apellido_paterno_paciente || ' ' || osiris_his_paciente.apellido_materno_paciente AS nombre_completo," +
											"to_char(osiris_erp_cobros_enca.fechahora_creacion,'yyyy-MM-dd HH24:MI') AS fechahoracreacion,osiris_erp_cobros_enca.id_empleado_admision," +
											"osiris_erp_cobros_enca.id_empresa,descripcion_empresa," +
				        					"osiris_erp_cobros_enca.id_aseguradora,descripcion_aseguradora " +
											"FROM osiris_erp_cobros_enca,osiris_his_paciente,osiris_aseguradoras,osiris_empresas " +
											"WHERE osiris_erp_cobros_enca.pid_paciente = osiris_his_paciente.pid_paciente " +
											"AND cancelado = 'false' "+
											"AND osiris_erp_cobros_enca.id_empresa = osiris_empresas.id_empresa "+
				        					"AND osiris_erp_cobros_enca.id_aseguradora = osiris_aseguradoras.id_aseguradora "+
											query_fechas+
											" ORDER BY osiris_erp_cobros_enca.folio_de_servicio;";
					//Console.WriteLine(comando.CommandText.ToString());
				NpgsqlDataReader lector = comando.ExecuteReader ();
				if(lector.Read()){
					folioservicio = (int) lector["folio_de_servicio"];
					string pdf_name = System.IO.Path.GetTempPath() + Guid.NewGuid().ToString() + ".pdf";
					// step 1: creation of a document-object
					Document documento = new Document(iTextSharp.text.PageSize.LETTER, 15, 15, 15, 15);
					//Document document = new Document(PageSize.A4.Rotate());
	
					try{
						PdfWriter writerpdf = PdfWriter.GetInstance(documento, new FileStream(pdf_name, FileMode.Create));
						documento.AddTitle("Ingresos de PX. NO Reportados");
						documento.AddCreator("Sistema Hospitalario OSIRIS");
						documento.AddAuthor("Sistema Hospitalario OSIRIS");
						documento.AddSubject("OSIRSrpt");
						EventoTitulos ev = new EventoTitulos();
						ev.titulo_rpt = "ATENCIONES SIN PASAR A CAJA";
						writerpdf.PageEvent = ev;
						documento.Open();
						
						iTextSharp.text.Font _standardFont = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 7, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);
						// Creamos una tabla para el contenido
				        // Creamos una tabla para el contenido
		        		PdfPTable tblreporte = new PdfPTable(7);
		        		tblreporte.WidthPercentage = 100;
						float[] widths = new float[] { 32f, 20f, 20f, 82f, 50f, 50f, 26f };	// controlando el ancho de cada columna
						tblreporte.SetWidths(widths);
						tblreporte.HorizontalAlignment = 0;						
						// Configuramos el título de las columnas de la tabla
						PdfPCell cl01 = new PdfPCell();
						PdfPCell cl02 = new PdfPCell();
						PdfPCell cl03 = new PdfPCell();
						PdfPCell cl04 = new PdfPCell();
						PdfPCell cl05 = new PdfPCell();
						PdfPCell cl06 = new PdfPCell();
						PdfPCell cl07 = new PdfPCell();
						
						if((int) lector ["id_aseguradora"] > 1){
							empresa_aseguradora = lector["descripcion_aseguradora"].ToString().Trim();
						}else{
							empresa_aseguradora = lector["descripcion_empresa"].ToString().Trim();						
						}
											
						//Console.WriteLine(folioservicio.ToString());
						if((string) classpublic.lee_registro_de_tabla("osiris_erp_comprobante_servicio","folio_de_servicio","WHERE folio_de_servicio = '"+folioservicio.ToString().Trim()+"'","folio_de_servicio","int") == ""){
							//Console.WriteLine(folioservicio.ToString().Trim()+" NO Tiene Comprobante de Servicio ");
							if((string) classpublic.lee_registro_de_tabla("osiris_erp_abonos","folio_de_servicio","WHERE folio_de_servicio = '"+folioservicio.ToString().Trim()+"' AND honorario_medico = 'false' ","folio_de_servicio","int") == ""){
								//Console.WriteLine(folioservicio.ToString().Trim()+" NO Tiene Comprobante de PAGO o ABONO ");
								//if((string) classpublic.lee_registro_de_tabla("osiris_erp_comprobante_pagare","folio_de_servicio","WHERE folio_de_servicio = '"+folioservicio.ToString().Trim()+"' ","folio_de_servicio","int") == ""){
									//Console.WriteLine(folioservicio.ToString().Trim()+" NO Tiene Comprobante de PAGARE ");
									//if((string) classpublic.lee_registro_de_tabla("osiris_erp_pases_qxurg","folio_de_servicio","WHERE folio_de_servicio = '"+folioservicio.ToString().Trim()+"' ","folio_de_servicio","int") == ""){
										//Console.WriteLine(folioservicio.ToString().Trim()+" NO Tiene PASE QX/URGENCIA ");
										//cr.MoveTo(05*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);		layout.SetText("FECHA :"++"N° Atencion :"++"    Expediente: "++"  Paciente: "++"  "+);	Pango.CairoHelper.ShowLayout (cr, layout);
										// Configuramos el título de las columnas de la tabla										
										tipopaciente = classpublic.lee_registro_de_tabla("osiris_erp_movcargos,osiris_his_tipo_pacientes","descripcion_tipo_paciente","WHERE osiris_erp_movcargos.folio_de_servicio = '"+folioservicio.ToString().Trim()+"' AND osiris_erp_movcargos.id_tipo_paciente = osiris_his_tipo_pacientes.id_tipo_paciente ","descripcion_tipo_paciente","string");
										cl01 = new PdfPCell(new Phrase((string) lector["fechahoracreacion"].ToString(), _standardFont));
										//clnroatencion.BorderWidth = 1;			// Ancho del borde
										cl01.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
										cl02 = new PdfPCell(new Phrase(folioservicio.ToString().Trim(), _standardFont));
										cl02.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
										//cl02.HorizontalAlignment = 1;		// centro
										cl03 = new PdfPCell(new Phrase((string) lector["pidpaciente"].ToString().Trim(), _standardFont));
										cl03.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
										cl04 = new PdfPCell(new Phrase((string) lector["nombre_completo"].ToString(), _standardFont));
										cl04.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
										cl05 = new PdfPCell(new Phrase(tipopaciente, _standardFont));
										cl05.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
										cl06 = new PdfPCell(new Phrase(empresa_aseguradora, _standardFont));
										cl06.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
										cl07 = new PdfPCell(new Phrase(lector["id_empleado_admision"].ToString().Trim(), _standardFont));
										cl07.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
										// Añadimos las celdas a la tabla
										tblreporte.AddCell(cl01);
										tblreporte.AddCell(cl02);
										tblreporte.AddCell(cl03);
										tblreporte.AddCell(cl04);
										tblreporte.AddCell(cl05);
										tblreporte.AddCell(cl06);
										tblreporte.AddCell(cl07);
									//}
								//}
							}					
						}
												
						while(lector.Read()){
							folioservicio = (int) lector["folio_de_servicio"];
							if((int) lector ["id_aseguradora"] > 1){
								empresa_aseguradora = lector["descripcion_aseguradora"].ToString().Trim();
							}else{
								empresa_aseguradora = lector["descripcion_empresa"].ToString().Trim();						
							}
							//Console.WriteLine(folioservicio.ToString());
							if((string) classpublic.lee_registro_de_tabla("osiris_erp_comprobante_servicio","folio_de_servicio","WHERE folio_de_servicio = '"+folioservicio.ToString().Trim()+"'","folio_de_servicio","int") == ""){
								//Console.WriteLine(folioservicio.ToString().Trim()+" NO Tiene Comprobante de Servicio ");
								if((string) classpublic.lee_registro_de_tabla("osiris_erp_abonos","folio_de_servicio","WHERE folio_de_servicio = '"+folioservicio.ToString().Trim()+"' AND honorario_medico = 'false' ","folio_de_servicio","int") == ""){
									//Console.WriteLine(folioservicio.ToString().Trim()+" NO Tiene Comprobante de PAGO o ABONO ");
									//if((string) classpublic.lee_registro_de_tabla("osiris_erp_comprobante_pagare","folio_de_servicio","WHERE folio_de_servicio = '"+folioservicio.ToString().Trim()+"' ","folio_de_servicio","int") == ""){
										//Console.WriteLine(folioservicio.ToString().Trim()+" NO Tiene Comprobante de PAGARE ");
										//if((string) classpublic.lee_registro_de_tabla("osiris_erp_pases_qxurg","folio_de_servicio","WHERE folio_de_servicio = '"+folioservicio.ToString().Trim()+"' ","folio_de_servicio","int") == ""){
											//Console.WriteLine(folioservicio.ToString().Trim()+" NO Tiene PASE QX/URGENCIA ");
											//cr.MoveTo(05*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);		layout.SetText("FECHA :"++"N° Atencion :"++"    Expediente: "++"  Paciente: "++"  "+);	Pango.CairoHelper.ShowLayout (cr, layout);
											// Configuramos el título de las columnas de la tabla
											tipopaciente = classpublic.lee_registro_de_tabla("osiris_erp_movcargos,osiris_his_tipo_pacientes","descripcion_tipo_paciente","WHERE osiris_erp_movcargos.folio_de_servicio = '"+folioservicio.ToString().Trim()+"' AND osiris_erp_movcargos.id_tipo_paciente = osiris_his_tipo_pacientes.id_tipo_paciente ","descripcion_tipo_paciente","string");
											cl01 = new PdfPCell(new Phrase((string) lector["fechahoracreacion"].ToString(), _standardFont));
											//clnroatencion.BorderWidth = 1;			// Ancho del borde
											cl01.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
											cl02 = new PdfPCell(new Phrase(folioservicio.ToString().Trim(), _standardFont));
											cl02.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
											//cl02.HorizontalAlignment = 1;		// centro
											cl03 = new PdfPCell(new Phrase((string) lector["pidpaciente"].ToString().Trim(), _standardFont));
											cl03.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
											cl04 = new PdfPCell(new Phrase((string) lector["nombre_completo"].ToString(), _standardFont));
											cl04.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
											cl05 = new PdfPCell(new Phrase(tipopaciente, _standardFont));
											cl05.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
											cl06 = new PdfPCell(new Phrase(empresa_aseguradora, _standardFont));
											cl06.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
											cl07 = new PdfPCell(new Phrase(lector["id_empleado_admision"].ToString().Trim(), _standardFont));
											cl07.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
											// Añadimos las celdas a la tabla
											tblreporte.AddCell(cl01);
											tblreporte.AddCell(cl02);
											tblreporte.AddCell(cl03);
											tblreporte.AddCell(cl04);
											tblreporte.AddCell(cl05);
											tblreporte.AddCell(cl06);
											tblreporte.AddCell(cl07);
										//}
									//}
								}					
							}	
						}
						documento.Add(tblreporte);
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
						try{				
							System.Diagnostics.Process.Start(pdf_name);	
						}catch(Exception ex){
							Console.WriteLine("Error al leer el PDF: {0}",ex.Message);
							MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
									MessageType.Error,ButtonsType.Close,"Debe Instalar Acroba Reader 9 error: {0}",ex.Message);
							msgBoxError.Run ();		msgBoxError.Destroy();
						}
					}catch(Exception de){
							Console.Error.WriteLine(de.StackTrace);
					}
					// step 5: we close the document
					documento.Close();
				}
			}catch (NpgsqlException ex){
				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
					MessageType.Warning, ButtonsType.Ok, "PostgresSQL error: {0}",ex.Message);
				msgBoxError.Run ();				msgBoxError.Destroy();
				Console.WriteLine ("PostgresSQL error: {0}",ex.Message);
			}				
		}		
		
		public class EventoTitulos : PdfPageEventHelper
		{
			class_public classpublic = new class_public();
						
			#region Fields
			private string _titulo_rpt;
			#endregion
			
        	#region Properties
			public string titulo_rpt
        	{
            	get{
					return _titulo_rpt;
				}
            	set{
					_titulo_rpt = value;
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
				cb.SetTextMatrix (130, 750);			cb.ShowText (classpublic.nombre_empresa);
				cb.SetTextMatrix (130, 740);			cb.ShowText (classpublic.direccion_empresa);
				cb.SetTextMatrix (130, 730);			cb.ShowText (classpublic.telefonofax_empresa);
				cb.SetFontAndSize (BaseFont.CreateFont (BaseFont.HELVETICA_BOLD, BaseFont.CP1252, BaseFont.NOT_EMBEDDED), 6);
				cb.SetTextMatrix (130, 720);			cb.ShowText ("Sistema Hospitalario OSIRIS");
				cb.SetTextMatrix (500, 750);		cb.ShowText ("Fech.Rpt:" + (string)DateTime.Now.ToString ("yyyy-MM-dd HH:mm:ss"));
				cb.SetTextMatrix (500, 740);		cb.ShowText ("N° Page :"+writerpdf.PageNumber.ToString("D8"));
				cb.EndText ();
				documento.Add (new Paragraph (""));
				documento.Add (Chunk.NEWLINE);
				documento.Add (new Paragraph (""));
				documento.Add (Chunk.NEWLINE);
				
				Paragraph titulo_reporte = new Paragraph(titulo_rpt, new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 13, iTextSharp.text.Font.BOLD, BaseColor.BLACK));
                titulo_reporte.Alignment = Element.ALIGN_CENTER;
                documento.Add(titulo_reporte);
				documento.Add (new Paragraph ("", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 6)));
				documento.Add (Chunk.NEWLINE);
								
				// Creamos una tabla para el contenido
		        PdfPTable tblreporte = new PdfPTable(7);
		        tblreporte.WidthPercentage = 100;
				float[] widths = new float[] { 32f, 20f, 20f, 82f, 50f, 50f, 26f };	// controlando el ancho de cada columna
				tblreporte.SetWidths(widths);
				tblreporte.HorizontalAlignment = 0;
				iTextSharp.text.Font _standardFont = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 7, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
								
				// Configuramos el título de las columnas de la tabla
				PdfPCell cl01 = new PdfPCell(new Phrase("Fecha", _standardFont));
				//clnroatencion.BorderWidth = 1;			// Ancho del borde
				cl01.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
				cl01.HorizontalAlignment = 1;		// centro
				cl01.BackgroundColor = BaseColor.YELLOW;
				PdfPCell cl02 = new PdfPCell(new Phrase("N° Aten.", _standardFont));
				cl02.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
				cl02.HorizontalAlignment = 1;		// centro
				cl02.BackgroundColor = BaseColor.YELLOW;
				PdfPCell cl03 = new PdfPCell(new Phrase("N° Exp.", _standardFont));
				cl03.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
				cl03.HorizontalAlignment = 1;		// centro
				cl03.BackgroundColor = BaseColor.YELLOW;
				PdfPCell cl04 = new PdfPCell(new Phrase("Nombre Paciente", _standardFont));
				cl04.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
				cl04.HorizontalAlignment = 1;		// centro
				cl04.BackgroundColor = BaseColor.YELLOW;
				PdfPCell cl05 = new PdfPCell(new Phrase("Tipo PX.", _standardFont));
				cl05.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
				cl05.HorizontalAlignment = 1;		// centro
				cl05.BackgroundColor = BaseColor.YELLOW;
				PdfPCell cl06 = new PdfPCell(new Phrase("Convenio", _standardFont));
				cl06.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
				cl06.HorizontalAlignment = 1;		// centro
				cl06.BackgroundColor = BaseColor.YELLOW;
				PdfPCell cl07 = new PdfPCell(new Phrase("Ingr. Por", _standardFont));
				cl07.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
				cl07.HorizontalAlignment = 1;		// centro
				cl07.BackgroundColor = BaseColor.YELLOW;
				// Añadimos las celdas a la tabla
				tblreporte.AddCell(cl01);
				tblreporte.AddCell(cl02);
				tblreporte.AddCell(cl03);
				tblreporte.AddCell(cl04);
				tblreporte.AddCell(cl05);
				tblreporte.AddCell(cl06);
				tblreporte.AddCell(cl07);
				documento.Add(tblreporte);
		    }
		
		    public override void OnEndPage(PdfWriter writerpdf, Document documento)
		    {
			
		    }
		
		}
		
		private void OnBeginPrint (object obj, Gtk.BeginPrintArgs args)
		{
			print.NPages = 1;  // crea cantidad de copias del reporte
			// para imprimir horizontalmente el reporte
			//print.PrintSettings.Orientation = PageOrientation.Landscape;
			//Console.WriteLine(print.PrintSettings.Orientation.ToString());
		}
		
		private void OnDrawPage (object obj, Gtk.DrawPageArgs args)
		{
			context = args.Context;
			//imprime_encabezado(cr,layou);
			ejecutar_consulta_reporte(context);
		}
		
		void ejecutar_consulta_reporte(PrintContext context)
		{
			//string mesage_1 = "NO Tiene Comprobante de Servicio";
			//string mesage_2 = "NO Tiene Comprobante de PAGO o ABONO";
			//string mesage_3 = "NO Tiene Comprobante de PAGARE";
			//string mesage_4 = "NO Tiene PASE QX/URGENCIA";
			Cairo.Context cr = context.CairoContext;
			Pango.Layout layout = context.CreatePangoLayout ();
			
			
			comienzo_linea = 85;
			NpgsqlConnection conexion;
			conexion = new NpgsqlConnection (connectionString+nombrebd);
			           
			// Verifica que la base de datos este conectada
			try{
				imprime_encabezado(cr,layout);
				fontSize = 8.0; layout = context.CreatePangoLayout ();
				desc.Size = (int)(fontSize * pangoScale); layout.FontDescription = desc;
				
				conexion.Open ();
				NpgsqlCommand comando;
				comando = conexion.CreateCommand ();
				            
				int folioservicio = 0;
				int contador = 0;
				string query_fechas = " AND to_char(osiris_erp_cobros_enca.fechahora_creacion,'yyyy-MM-dd') >= '"+entry_ano1.Text+"-"+entry_mes1.Text+"-"+entry_dia1.Text+"' "+
									" AND to_char(osiris_erp_cobros_enca.fechahora_creacion,'yyyy-MM-dd') <= '"+entry_ano2.Text+"-"+entry_mes2.Text+"-"+entry_dia2.Text+"' ";								
				comando.CommandText = "SELECT osiris_erp_cobros_enca.folio_de_servicio,osiris_erp_cobros_enca.pid_paciente AS pidpaciente, " +
										"osiris_his_paciente.nombre1_paciente || ' ' || osiris_his_paciente.nombre2_paciente || ' ' || osiris_his_paciente.apellido_paterno_paciente || ' ' || osiris_his_paciente.apellido_materno_paciente AS nombre_completo," +
										"osiris_erp_cobros_enca.fechahora_creacion,osiris_erp_cobros_enca.id_empleado_admision "+
										"FROM osiris_erp_cobros_enca,osiris_his_paciente " +
										"WHERE osiris_erp_cobros_enca.pid_paciente = osiris_his_paciente.pid_paciente " +
										"AND cancelado = 'false' "+
										query_fechas+
										" ORDER BY osiris_erp_cobros_enca.folio_de_servicio;";
				//Console.WriteLine(comando.CommandText.ToString());
				NpgsqlDataReader lector = comando.ExecuteReader ();
				while(lector.Read()){
					folioservicio = (int) lector["folio_de_servicio"];
					//Console.WriteLine(folioservicio.ToString());
					if((string) classpublic.lee_registro_de_tabla("osiris_erp_comprobante_servicio","folio_de_servicio","WHERE folio_de_servicio = '"+folioservicio.ToString().Trim()+"'","folio_de_servicio","int") == ""){
						//Console.WriteLine(folioservicio.ToString().Trim()+" NO Tiene Comprobante de Servicio ");
						if((string) classpublic.lee_registro_de_tabla("osiris_erp_abonos","folio_de_servicio","WHERE folio_de_servicio = '"+folioservicio.ToString().Trim()+"' AND honorario_medico = 'false' ","folio_de_servicio","int") == ""){
							//Console.WriteLine(folioservicio.ToString().Trim()+" NO Tiene Comprobante de PAGO o ABONO ");
							if((string) classpublic.lee_registro_de_tabla("osiris_erp_comprobante_pagare","folio_de_servicio","WHERE folio_de_servicio = '"+folioservicio.ToString().Trim()+"' ","folio_de_servicio","int") == ""){
								//Console.WriteLine(folioservicio.ToString().Trim()+" NO Tiene Comprobante de PAGARE ");
								if((string) classpublic.lee_registro_de_tabla("osiris_erp_pases_qxurg","folio_de_servicio","WHERE folio_de_servicio = '"+folioservicio.ToString().Trim()+"' ","folio_de_servicio","int") == ""){
									//Console.WriteLine(folioservicio.ToString().Trim()+" NO Tiene PASE QX/URGENCIA ");
									cr.MoveTo(05*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);		layout.SetText("FECHA :"+(string) lector["fechahora_creacion"].ToString()+"N° Atencion :"+folioservicio.ToString()+"    Expediente: "+(string) lector["pidpaciente"].ToString()+"  Paciente: "+(string) lector["nombre_completo"].ToString()+"  "+lector["id_empleado_admision"].ToString().Trim());	Pango.CairoHelper.ShowLayout (cr, layout);
									comienzo_linea += separacion_linea;
									contador ++;
									salto_de_pagina(cr,layout);
									fontSize = 8.0; layout = context.CreatePangoLayout ();
									desc.Size = (int)(fontSize * pangoScale); layout.FontDescription = desc;
								}
							}
						}					
					}
				}
				comienzo_linea += separacion_linea;
				salto_de_pagina(cr,layout);
				cr.MoveTo(05*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);		layout.SetText("TOTAL "+ contador.ToString());	Pango.CairoHelper.ShowLayout (cr, layout);

			}catch (NpgsqlException ex){
				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
										MessageType.Error,ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
				msgBoxError.Run ();				msgBoxError.Destroy();
			}
			conexion.Close ();
		}
		
		private void OnEndPrint (object obj, Gtk.EndPrintArgs args)
		{
			
		}
		
		void imprime_encabezado(Cairo.Context cr,Pango.Layout layout)
		{
			//cr.Rotate(90);  //Imprimir Orizontalmente rota la hoja cambian las posiciones de las lineas y columna					
			fontSize = 8.0;
			desc.Size = (int)(fontSize * pangoScale);					layout.FontDescription = desc;
			layout.FontDescription.Weight = Weight.Bold;		// Letra negrita
			cr.MoveTo(05*escala_en_linux_windows,05*escala_en_linux_windows);			layout.SetText(classpublic.nombre_empresa);			Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(05*escala_en_linux_windows,15*escala_en_linux_windows);			layout.SetText(classpublic.direccion_empresa);		Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(05*escala_en_linux_windows,25*escala_en_linux_windows);			layout.SetText(classpublic.telefonofax_empresa);	Pango.CairoHelper.ShowLayout (cr, layout);
			fontSize = 6.0;
			desc.Size = (int)(fontSize * pangoScale);					layout.FontDescription = desc;
			cr.MoveTo(650*escala_en_linux_windows,05*escala_en_linux_windows);			layout.SetText("Fech.Rpt:"+(string) DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));		Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(650*escala_en_linux_windows,15*escala_en_linux_windows);			layout.SetText("N° Page :"+numpage.ToString().Trim());		Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(05*escala_en_linux_windows,35*escala_en_linux_windows);			layout.SetText("Sistema Hospitalario OSIRIS");		Pango.CairoHelper.ShowLayout (cr, layout);
			
			fontSize = 11.0;
			desc.Size = (int)(fontSize * pangoScale);					layout.FontDescription = desc;
			layout.FontDescription.Weight = Weight.Bold;		// Letra negrita
			layout.Alignment = Pango.Alignment.Center;
			
			double width = context.Width;
			layout.Width = (int) width;
			layout.Alignment = Pango.Alignment.Center;
			//layout.Wrap = Pango.WrapMode.Word;
			//layout.SingleParagraphMode = true;
			layout.Justify =  false;
			cr.MoveTo(width/2,45*escala_en_linux_windows);	layout.SetText("PACIENTES_NO_INGRESADOS_A_CAJA");	Pango.CairoHelper.ShowLayout (cr, layout);
			//cr.MoveTo(225*escala_en_linux_windows, 35*escala_en_linux_windows);			layout.SetText(titulo_rpt);				Pango.CairoHelper.ShowLayout (cr, layout);
			fontSize = 8.0;			layout = null;			layout = context.CreatePangoLayout ();
			desc.Size = (int)(fontSize * pangoScale);		layout.FontDescription = desc;
			layout.FontDescription.Weight = Weight.Normal;		// Letra negrita
		}
		
		void salto_de_pagina(Cairo.Context cr,Pango.Layout layout)			
		{
			if(comienzo_linea >660){
				cr.ShowPage();
				desc = Pango.FontDescription.FromString ("Sans");								
				fontSize = 8.0;		desc.Size = (int)(fontSize * pangoScale);					layout.FontDescription = desc;
				comienzo_linea = 70;
				numpage += 1;
				imprime_encabezado(cr,layout);
			}
		}
		
		// Valida entradas que solo sean numericas, se utiliza eb ventana de
		//de rangos de fechas
		[GLib.ConnectBefore ()]     // Esto es indispensable para que funcione   
		public void onKeyPressEvent(object o, Gtk.KeyPressEventArgs args)
		{
			//Console.WriteLine(args.Event.Key);
			//Console.WriteLine(Convert.ToChar(args.Event.Key));
			string misDigitos = ".0123456789ﾰﾱﾲﾳﾴﾵﾶﾷﾸﾹﾮｔｒｓｑ（）";
			if (Array.IndexOf(misDigitos.ToCharArray(), Convert.ToChar(args.Event.Key)) == -1 && args.Event.Key != Gdk.Key.BackSpace){
				args.RetVal = true;
			}
		}
	}
}