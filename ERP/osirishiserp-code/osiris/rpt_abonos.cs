//////////////////////////////////////////////////////////////////////
// created on 28/02/2008 at 09:47 a
// Sistema Hospitalario OSIRIS
// Monterrey - Mexico
//
// Autor    	: Tec. Homero Montoya Galvan (Programacion) homerokda@hotmail.com
// 				  Ing. Daniel Olivares C. (Modificaciones y Ajustes)
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
using Glade;
using Cairo;
using Pango;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;

namespace osiris
{
	public class reporte_de_abonos
	{
		// Ventana de Rango de Fecha
		[Widget] Gtk.Window rango_de_fecha;
		[Widget] Gtk.Entry entry_dia1;
		[Widget] Gtk.Entry entry_dia2;
		[Widget] Gtk.Entry entry_mes1;
		[Widget] Gtk.Entry entry_mes2;
		[Widget] Gtk.Entry entry_ano1;
		[Widget] Gtk.Entry entry_ano2;
		[Widget] Gtk.Entry entry_referencia_inicial;
		[Widget] Gtk.Entry entry_cliente;
		[Widget] Gtk.Label label_orden;
		[Widget] Gtk.Label label_nom_cliente;
		[Widget] Gtk.Label label142;
		[Widget] Gtk.ComboBox combobox_rango_fecha = null;
		[Widget] Gtk.CheckButton checkbutton_todos_rango_fecha = null;
		[Widget] Gtk.RadioButton radiobutton_cliente;
		[Widget] Gtk.RadioButton radiobutton_fecha;
		[Widget] Gtk.Button button_busca_cliente;
		[Widget] Gtk.Button button_salir;
		[Widget] Gtk.Button button_imprime_rangofecha;
		[Widget] Gtk.CheckButton checkbutton_impr_todo_proce;
		[Widget] Gtk.CheckButton checkbutton_todos_los_clientes;
		[Widget] Gtk.CheckButton checkbutton_export_to = null;
		[Widget] Gtk.CheckButton checkbutton_incluye_concep_ccj = null;
		
		private static int pangoScale = 1024;
		private PrintOperation print;
		private double fontSize = 8.0;
		int escala_en_linux_windows;		// Linux = 1  Windows = 8
		int comienzo_linea = 70;
		int separacion_linea = 10;
		int numpage = 1;
		Pango.FontDescription desc;
		
		string connectionString;
        string nombrebd;
		string tiporeporte = "ABONOS";
		string titulo = "REPORTE DE ABONOS";
		
		string query_fechas = " ";
		string rango1 = "";
		string rango2 = "";
		string tiporpt = "";
		string LoginEmpleado = "";
		string idtipocomprobante = "";
		int folioservicio;
		int PidPaciente;
		string sql_query = "";

		string[] args_args = {""};
		int[] args_id_array = {0,1,2,3,4,5,6,7,8,9,10,11,12,13,14};
		
		//Declaracion de ventana de error
		protected Gtk.Window MyWinError;
		
		class_conexion conexion_a_DB = new class_conexion();
		class_public classpublic = new class_public();
		
		public reporte_de_abonos(string _nombrebd_,string tiporpt_,string LoginEmpleado_,int folioservicio_,int pidpaciente_)
		{
			connectionString = conexion_a_DB._url_servidor+conexion_a_DB._port_DB+conexion_a_DB._usuario_DB+conexion_a_DB._passwrd_user_DB;
			nombrebd = conexion_a_DB._nombrebd;
			escala_en_linux_windows = classpublic.escala_linux_windows;
			tiporpt = tiporpt_;
			LoginEmpleado = LoginEmpleado_;
			folioservicio = folioservicio_;
			PidPaciente = pidpaciente_;
			Glade.XML gxml = new Glade.XML (null, "caja.glade", "rango_de_fecha", null);
			gxml.Autoconnect                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                 (this);
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
        	button_salir.Clicked += new EventHandler(on_cierraventanas_clicked);
        	button_imprime_rangofecha.Clicked += new EventHandler(imprime_reporte_abonos);
			checkbutton_todos_rango_fecha.Clicked += new EventHandler(on_checkbutton1_clicked);
        	label_orden.Hide();
			label_nom_cliente.Hide();
			label142.Hide();
			radiobutton_cliente.Hide();
			radiobutton_fecha.Hide();
			checkbutton_todos_los_clientes.Hide();
			entry_referencia_inicial.Hide();
			entry_cliente.Hide();
			button_busca_cliente.Hide();
			combobox_rango_fecha.Sensitive = false;
			checkbutton_todos_rango_fecha.Active = true;
			llenado_combobox(0,"",combobox_rango_fecha,"sql","SELECT * FROM osiris_erp_tipo_comprobante WHERE activo = 'true' ORDER BY id_tipo_comprobante;","descripcion_tipo_comprobante","id_tipo_comprobante",args_args,args_id_array,"");
			if(tiporpt == "abonospagos_x_atencion"){
				checkbutton_impr_todo_proce.Active = true;
				sql_query = "AND osiris_erp_abonos.folio_de_servicio = '" + folioservicio.ToString ().Trim () + "' ";
				tiporpt = "abonospagos_x_atencion_expedi";
			}
			if(tiporpt == "abonospagos_x_expediente"){
				checkbutton_impr_todo_proce.Active = true;
				sql_query = "AND osiris_erp_abonos.pid_paciente = '" + PidPaciente.ToString ().Trim () + "' ";
				tiporpt = "abonospagos_x_atencion_expedi";
			}
    	}
		
		void llenado_combobox(int tipodellenado,string descrip_defaul,object obj,string sql_or_array,string query_SQL,string name_field_desc,string name_field_id,string[] args_array,int[] args_id_array,string name_field_id2)
		{			
			Gtk.ComboBox combobox_llenado = (Gtk.ComboBox) obj;
			//Gtk.ComboBox combobox_pos_neg = obj as Gtk.ComboBox;
			//Console.WriteLine((string) combobox_llenado.GetType().ToString());
			combobox_llenado.Clear();
			CellRendererText cell = new CellRendererText();
			combobox_llenado.PackStart(cell, true);
			combobox_llenado.AddAttribute(cell,"text",0);	        
			ListStore store = new ListStore( typeof (string),typeof (int),typeof(bool));
			combobox_llenado.Model = store;			
			if ((int) tipodellenado == 1){
				store.AppendValues ((string) descrip_defaul,0);
			}			
			if(sql_or_array == "array"){			
				for (int colum_field = 0; colum_field < args_array.Length; colum_field++){
					store.AppendValues (args_array[colum_field],args_id_array[colum_field]);
				}
			}
			if(sql_or_array == "sql"){			
				NpgsqlConnection conexion; 
				conexion = new NpgsqlConnection (connectionString+nombrebd);
				// Verifica que la base de datos este conectada
				try{
					conexion.Open ();
					NpgsqlCommand comando; 
					comando = conexion.CreateCommand ();
					comando.CommandText = query_SQL;					
					NpgsqlDataReader lector = comando.ExecuteReader ();
					while (lector.Read()){
						store.AppendValues ((string) lector[name_field_desc ], (int) lector[name_field_id],false);
					}
				}catch (NpgsqlException ex){
					MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
						MessageType.Error,ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
					msgBoxError.Run ();				msgBoxError.Destroy();
				}
				conexion.Close ();
			}			
			TreeIter iter;
			if (store.GetIterFirst(out iter)){
				combobox_llenado.SetActiveIter (iter);
			}
			combobox_llenado.Changed += new EventHandler (onComboBoxChanged_llenado);			
		}

		void onComboBoxChanged_llenado (object sender, EventArgs args)
		{
			ComboBox onComboBoxChanged = sender as ComboBox;
			if (sender == null){	return; }
			TreeIter iter;
			if (onComboBoxChanged.GetActiveIter (out iter)){
				switch (onComboBoxChanged.Name.ToString()){	
				case "combobox_rango_fecha":
					idtipocomprobante = onComboBoxChanged.Model.GetValue(iter,1).ToString().Trim();
				break;
				}
			}
		}
		
		void on_checkbutton1_clicked(object sender, EventArgs args)
		{
			combobox_rango_fecha.Sensitive = !(bool) checkbutton_todos_rango_fecha.Active;
		}
		
		void imprime_reporte_abonos(object sender, EventArgs args)
		{
			genera_reporte_export ();
		}

		void genera_reporte_export()
		{			
			query_fechas = " ";
			float totalabonos_pagos = 0;
			float totalfacturado = 0;
			string query_tipocomprobante = "";
			rango1 = "";
			rango2 = "";
			if (checkbutton_impr_todo_proce.Active == false){
				rango1 = entry_dia1.Text+"-"+entry_mes1.Text+"-"+entry_ano1.Text;
				rango2 = entry_dia2.Text+"-"+entry_mes2.Text+"-"+entry_ano2.Text;
				query_fechas = "AND to_char(osiris_erp_abonos.fecha_abono,'yyyy-MM-dd') >= '"+entry_ano1.Text+"-"+entry_mes1.Text+"-"+entry_dia1.Text+"' "+//;//;//'"+rango1+"' "+
								"AND to_char(osiris_erp_abonos.fecha_abono,'yyyy-MM-dd') <= '"+entry_ano2.Text+"-"+entry_mes2.Text+"-"+entry_dia2.Text+"' ";			
			}
						
			if(tiporpt == "abonospagos_x_atencion_expedi"){
				NpgsqlConnection conexion;
				conexion = new NpgsqlConnection (connectionString+nombrebd);
	        	// Verifica que la base de datos este conectada
				try{
					conexion.Open ();
					NpgsqlCommand comando; 
					comando = conexion.CreateCommand ();
					comando.CommandText = "SELECT to_char(osiris_erp_abonos.folio_de_servicio,'9999999999') AS folio,"+
								"to_char(osiris_erp_abonos.id_abono,'9999999999') AS idabono,osiris_erp_abonos.observaciones,"+
								"to_char(numero_recibo_caja,'9999999999') AS recibocaja, id_quien_creo,"+
								"osiris_erp_abonos.monto_de_abono_procedimiento AS abono,"+
								"osiris_erp_abonos.concepto_del_abono AS concepto,osiris_erp_abonos.numero_factura,(subtotal_facturado+iva_facturado) AS totalfactura,"+
								"osiris_erp_abonos.eliminado,osiris_erp_abonos.id_quien_elimino,osiris_erp_abonos.fechahora_eliminado,"+
								"to_char(osiris_erp_abonos.fecha_abono,'yyyy-MM-dd') AS fechaabono,"+
								"osiris_erp_abonos.id_forma_de_pago,osiris_erp_abonos.id_tipo_comprobante,descripcion_tipo_comprobante,"+ 
								"osiris_erp_forma_de_pago.id_forma_de_pago,descripcion_forma_de_pago AS descripago,"+
								"osiris_his_paciente.pid_paciente AS pidpaciente, nombre1_paciente || ' ' || nombre2_paciente || ' ' || apellido_paterno_paciente || ' ' || apellido_materno_paciente AS nombre_completo "+
								"FROM osiris_erp_abonos,osiris_erp_forma_de_pago,osiris_erp_cobros_enca,osiris_his_paciente,osiris_erp_tipo_comprobante "+
								"WHERE osiris_erp_abonos.eliminado = false "+
								"AND osiris_erp_abonos.id_forma_de_pago = osiris_erp_forma_de_pago.id_forma_de_pago "+
								"AND osiris_erp_abonos.folio_de_servicio = osiris_erp_cobros_enca.folio_de_servicio "+
								"AND osiris_erp_cobros_enca.pid_paciente = osiris_his_paciente.pid_paciente " +
								"AND osiris_erp_abonos.id_tipo_comprobante = osiris_erp_tipo_comprobante.id_tipo_comprobante " +
								sql_query+
								" "+query_fechas+" "+
								"ORDER BY osiris_erp_abonos.folio_de_servicio;";															
					//Console.WriteLine(comando.CommandText);
					NpgsqlDataReader lector = comando.ExecuteReader ();
					
					if (lector.Read()){
						string pdf_name = System.IO.Path.GetTempPath() + Guid.NewGuid().ToString() + ".pdf";
						// step 1: creation of a document-object
						Document documento = new Document(iTextSharp.text.PageSize.LETTER, 15, 15, 15, 15);
						//Document document = new Document(PageSize.A4.Rotate());
						
						try{
							PdfWriter writerpdf = PdfWriter.GetInstance(documento, new FileStream(pdf_name, FileMode.Create));
							
							documento.AddTitle("Reporte de Abonos/Pagos por Paciente");
			            	documento.AddCreator("Sistema Hospitalario OSIRIS");
			            	documento.AddAuthor("Sistema Hospitalario OSIRIS");
			            	documento.AddSubject("OSIRSrpt");	
							EventoTitulos ev = new EventoTitulos();
							ev.titulo1_rpt = "REPORTE DE ABONOS";
							ev.titulo2_rpt = "NOMBRE PX. "+lector["nombre_completo"].ToString().Trim()+"   N° EXP. "+PidPaciente.ToString().Trim();
							writerpdf.PageEvent = ev;
							documento.Open();
														
							iTextSharp.text.Font _standardFont = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 7, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);
							iTextSharp.text.Font _bolddFont = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 7, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
							 
							// Creamos una tabla para el contenido
				            PdfPTable tblreporte = new PdfPTable(8);
				            tblreporte.WidthPercentage = 100;
							float[] widths = new float[] { 25f, 30f, 25f, 25f, 65f, 105f, 30f, 30f };	// controlando el ancho de cada columna
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
							PdfPCell cl08 = new PdfPCell();
							
							// Configuramos el título de las columnas de la tabla
							cl01 = new PdfPCell(new Phrase((string) lector["folio"].ToString().Trim(), _standardFont));
				            //cl01.BorderWidth = 0;
							cl01.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
							cl02 = new PdfPCell(new Phrase(string.Format("{0:C}",decimal.Parse(lector["abono"].ToString())), _standardFont));
				            //cl02.BorderWidth = 0;
							cl02.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
							cl02.HorizontalAlignment = 2;		// derecha
							cl02.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
							cl03 = new PdfPCell(new Phrase(lector["fechaabono"].ToString(), _standardFont));
				            //cl03.BorderWidth = 0;
							cl03.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
							cl04 = new PdfPCell(new Phrase(lector["recibocaja"].ToString().Trim(), _standardFont));
				            //cl04.BorderWidth = 0;
							cl04.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
							cl05 = new PdfPCell(new Phrase(lector["descripcion_tipo_comprobante"].ToString(), _standardFont));
				            //cl05.BorderWidth = 0;
							cl05.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
							cl06 = new PdfPCell(new Phrase(lector["observaciones"].ToString(), _standardFont));
							//cl05.BorderWidth = 0;
							cl06.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
							cl07 = new PdfPCell(new Phrase(lector["numero_factura"].ToString(), _standardFont));
							//cl05.BorderWidth = 0;
							cl07.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
							cl08 = new PdfPCell(new Phrase(string.Format("{0:C}",float.Parse(lector["totalfactura"].ToString())), _standardFont));
							cl08.HorizontalAlignment = 2;		// derecha
							cl08.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;

				            // Añadimos las celdas a la tabla
				            tblreporte.AddCell(cl01);
				            tblreporte.AddCell(cl02);
							tblreporte.AddCell(cl03);
							tblreporte.AddCell(cl04);
							tblreporte.AddCell(cl05);
							tblreporte.AddCell(cl06);
							tblreporte.AddCell(cl07);
							tblreporte.AddCell(cl08);
							totalabonos_pagos += float.Parse(lector["abono"].ToString());
							totalfacturado += float.Parse(lector["totalfactura"].ToString());
														
							while(lector.Read()){
								// Configuramos el título de las columnas de la tabla
								cl01 = new PdfPCell(new Phrase((string) lector["folio"].ToString().Trim(), _standardFont));
					            //cl01.BorderWidth = 0;
								cl01.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
								cl02 = new PdfPCell(new Phrase(string.Format("{0:C}",decimal.Parse(lector["abono"].ToString())), _standardFont));
					            //cl02.BorderWidth = 0;
								cl02.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
								cl02.HorizontalAlignment = 2;		// derecha
								cl02.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
								cl03 = new PdfPCell(new Phrase(lector["fechaabono"].ToString(), _standardFont));
					            //cl03.BorderWidth = 0;
								cl03.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
								cl04 = new PdfPCell(new Phrase(lector["recibocaja"].ToString().Trim(), _standardFont));
					            //cl04.BorderWidth = 0;
								cl04.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
								cl05 = new PdfPCell(new Phrase(lector["descripcion_tipo_comprobante"].ToString(), _standardFont));
					            //cl05.BorderWidth = 0;
								cl05.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
								cl06 = new PdfPCell(new Phrase(lector["observaciones"].ToString(), _standardFont));
								//cl05.BorderWidth = 0;
								cl06.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
								cl07 = new PdfPCell(new Phrase(lector["numero_factura"].ToString(), _standardFont));
								//cl05.BorderWidth = 0;
								cl07.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
								cl08 = new PdfPCell(new Phrase(string.Format("{0:C}",float.Parse(lector["totalfactura"].ToString())), _standardFont));
								cl08.HorizontalAlignment = 2;		// derecha
								cl08.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
								// Añadimos las celdas a la tabla
					            tblreporte.AddCell(cl01);
					            tblreporte.AddCell(cl02);
								tblreporte.AddCell(cl03);
								tblreporte.AddCell(cl04);
								tblreporte.AddCell(cl05);
								tblreporte.AddCell(cl06);
								tblreporte.AddCell(cl07);
								tblreporte.AddCell(cl08);
								totalabonos_pagos += float.Parse(lector["abono"].ToString());
								totalfacturado += float.Parse(lector["totalfactura"].ToString());
							}

							// Configuramos el título de las columnas de la tabla
							cl01 = new PdfPCell(new Phrase("TOTAL", _bolddFont));
							cl01.HorizontalAlignment = 2;		// derecha
							cl01.Border = iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
							cl02 = new PdfPCell(new Phrase(string.Format("{0:C}",totalabonos_pagos), _bolddFont));
							//cl02.BorderWidth = 0;
							cl02.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
							cl02.HorizontalAlignment = 2;		// derecha
							cl02.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
							cl03 = new PdfPCell(new Phrase("", _standardFont));
							//cl03.BorderWidth = 0;
							cl03.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER;
							cl04 = new PdfPCell(new Phrase("", _standardFont));
							//cl04.BorderWidth = 0;
							cl04.Border = iTextSharp.text.Rectangle.TOP_BORDER;
							cl05 = new PdfPCell(new Phrase("", _standardFont));
							//cl05.BorderWidth = 0;
							cl05.Border = iTextSharp.text.Rectangle.TOP_BORDER;
							cl06 = new PdfPCell(new Phrase("", _standardFont));
							//cl05.BorderWidth = 0;
							cl06.Border = iTextSharp.text.Rectangle.TOP_BORDER;
							cl07 = new PdfPCell(new Phrase("TOTAL", _bolddFont));
							cl07.HorizontalAlignment = 2;		// derecha
							cl07.Border = iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
							cl08 = new PdfPCell(new Phrase(string.Format("{0:C}",totalfacturado), _bolddFont));
							cl08.HorizontalAlignment = 2;		// derecha
							cl08.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
							// Añadimos las celdas a la tabla
							tblreporte.AddCell(cl01);
							tblreporte.AddCell(cl02);
							tblreporte.AddCell(cl03);
							tblreporte.AddCell(cl04);
							tblreporte.AddCell(cl05);
							tblreporte.AddCell(cl06);
							tblreporte.AddCell(cl07);
							tblreporte.AddCell(cl08);

							// Finalmente, añadimos la tabla al documento PDF
				            documento.Add(tblreporte);
				            					
							//documento.NewPage();
							//documento.Add(new Paragraph("Mi primer documento PDF"));
				            //documento.Add(Chunk.NEWLINE);																		
							
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
							//	proc.StartInfo.FileName = classpublic.lector_de_pdf_linux;
							//}
							//proc.StartInfo.Arguments = pdf_name;
							//proc.StartInfo.Arguments = "/home/dolivares/Desktop/"+siglasemisorcfd + "-" + entry_numero_factura.Text.Trim() + "_remision.pdf";
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
						Console.WriteLine("PostgresSQL error: {0}",ex.Message);
						MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
									MessageType.Error,ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
						msgBoxError.Run ();		msgBoxError.Destroy();
				}
				conexion.Close();					
			}			
			
			// exporta corte de pagares generados
			if(tiporpt == "corte_caja_pagares"){				
				if((string) classpublic.lee_registro_de_tabla("osiris_empleado","exportar_cortecaja_pagares","WHERE exportar_cortecaja_pagares = 'true' AND login_empleado = '"+LoginEmpleado+"' ","exportar_cortecaja_pagares","bool") == "True"){
					if(checkbutton_export_to.Active == true){
						string query_sql = "SELECT DISTINCT (osiris_erp_movcargos.folio_de_servicio),to_char(osiris_erp_abonos.fecha_abono,'yyyy-MM-dd') AS fechaabonopago,"+
										"osiris_erp_abonos.id_abono,monto_x_facturar,osiris_erp_abonos.numero_factura AS numerofactura,subtotal_facturado+iva_facturado AS totalfactura,"+
										"to_char(osiris_erp_abonos.folio_de_servicio,'9999999999') AS foliodeservicio,"+
										"osiris_his_paciente.pid_paciente AS pidpaciente,nombre1_paciente || ' ' || nombre2_paciente || ' ' || apellido_paterno_paciente || ' ' || apellido_materno_paciente AS nombrepaciente,"+
										"osiris_erp_abonos.monto_de_abono_procedimiento AS monto_comprobante,osiris_erp_abonos.concepto_del_abono,numero_recibo_caja AS numerorecibo,osiris_erp_cobros_enca.pagare,"+
										"osiris_erp_tipo_comprobante.descripcion_tipo_comprobante,osiris_erp_forma_de_pago.descripcion_forma_de_pago AS forma_de_pago," +
										"osiris_erp_cobros_enca.monto_convenio,(osiris_erp_cobros_enca.total_abonos+osiris_erp_cobros_enca.total_pago) AS tot_abonos," +
										"to_char(osiris_erp_comprobante_pagare.monto_pagare,'99999999.99') AS montopagare, "+
							 			"osiris_erp_cobros_enca.total_pago,osiris_erp_cobros_enca.monto_convenio - (osiris_erp_cobros_enca.total_abonos+osiris_erp_cobros_enca.total_pago) AS saldo_deuda,"+
										"osiris_erp_abonos.observaciones," +
										"osiris_erp_movcargos.id_tipo_paciente,descripcion_tipo_paciente,osiris_erp_tipo_comprobante.id_tipo_comprobante," +
										"osiris_erp_abonos.eliminado,to_char(osiris_erp_abonos.fechahora_eliminado,'yyyy-MM-dd') AS fechaeliminado,osiris_erp_abonos.motivo_eliminacion,numero_comprobante_pagare AS numero_pagare "+
										"FROM osiris_erp_cobros_enca,osiris_erp_abonos,osiris_erp_tipo_comprobante,osiris_his_paciente,osiris_erp_forma_de_pago,osiris_erp_movcargos,osiris_his_tipo_pacientes,osiris_erp_comprobante_pagare "+
										"WHERE osiris_erp_cobros_enca.cancelado = 'false' "+
										"AND osiris_erp_abonos.folio_de_servicio = osiris_erp_cobros_enca.folio_de_servicio "+
										"AND osiris_erp_movcargos.folio_de_servicio = osiris_erp_abonos.folio_de_servicio "+
										"AND osiris_erp_abonos.id_forma_de_pago = osiris_erp_forma_de_pago.id_forma_de_pago "+ 
										"AND osiris_erp_cobros_enca.pid_paciente = osiris_his_paciente.pid_paciente " +
										"AND osiris_erp_comprobante_pagare.eliminado = 'false' "+
										"AND osiris_erp_abonos.id_tipo_comprobante = osiris_erp_tipo_comprobante.id_tipo_comprobante " +
										"AND osiris_his_tipo_pacientes.id_tipo_paciente = osiris_erp_movcargos.id_tipo_paciente " +
										"AND osiris_erp_abonos.folio_de_servicio = osiris_erp_comprobante_pagare.folio_de_servicio "+
										query_fechas+
										" ORDER BY to_char(osiris_erp_abonos.fecha_abono,'yyyy-MM-dd'),osiris_erp_tipo_comprobante.id_tipo_comprobante,numero_recibo_caja;";
									
						string[] args_names_field = {"fechaabonopago","foliodeservicio","pidpaciente","nombrepaciente","numerorecibo","descripcion_tipo_comprobante","monto_comprobante","forma_de_pago","concepto_del_abono","observaciones","monto_convenio","montopagare","tot_abonos","saldo_deuda","descripcion_tipo_paciente","pagare","eliminado","fechaeliminado","motivo_eliminacion","monto_x_facturar","numerofactura","totalfactura","numero_pagare"};
						string[] args_type_field = {"string","float","float","string","float","string","float","string","string","string","float","float","float","float","string","string","string","string","string","float","string","float","float"};
						string[] args_field_text = {""};
						string[] args_more_title = {""};
						string[,] args_formulas = {{"6","=SUM(G2:G"}};
						string[,] args_width = {{"3","7cm"},{"5","5cm"},{"9","7cm"}};										
					
						new osiris.class_traslate_spreadsheet(query_sql,args_names_field,args_type_field,false,args_field_text,"",false,args_more_title,args_formulas,args_width);
					}else{
						
					}
				}else{
					MessageDialog msgBox = new MessageDialog (MyWinError,DialogFlags.Modal,
									MessageType.Info,ButtonsType.Ok,"No tiene Permiso para esta Opcion");
					msgBox.Run ();msgBox.Destroy();
				}									
			}

			// movimiento de la gestion de la cobranza
			if(tiporpt == "movimientos_gestion_cobranza"){
				query_fechas = " ";	 
				rango1 = "";
				rango2 = "";
				if (checkbutton_impr_todo_proce.Active == false){
					rango1 = entry_dia1.Text+"-"+entry_mes1.Text+"-"+entry_ano1.Text;
					rango2 = entry_dia2.Text+"-"+entry_mes2.Text+"-"+entry_ano2.Text;
					query_fechas = "AND to_char(osiris_erp_gestcobrzmov.fechahora_creacion,'yyyy-MM-dd') >= '"+entry_ano1.Text+"-"+entry_mes1.Text+"-"+entry_dia1.Text+"' "+
									"AND to_char(osiris_erp_gestcobrzmov.fechahora_creacion,'yyyy-MM-dd') <= '"+entry_ano2.Text+"-"+entry_mes2.Text+"-"+entry_dia2.Text+"' ";			
				}
				
				if(checkbutton_export_to.Active == true){
					string query_sql = "SELECT to_char(osiris_erp_gestcobrzmov.fechahora_creacion,'yyyy-MM-dd') AS fechacrea," +
			 							"to_char(osiris_erp_gestcobrzmov.fechahora_creacion,'HH24:MI') AS horacrea,nota,telefono," +
			 							"numero_comprobante_pagare AS numeropagare,osiris_erp_comprobante_pagare.folio_de_servicio AS folioservicio,osiris_erp_gestcobrzmov.pid_paciente AS expediente,"+
										"osiris_his_paciente.nombre1_paciente || ' ' || osiris_his_paciente.nombre2_paciente || ' ' || osiris_his_paciente.apellido_paterno_paciente || ' ' || osiris_his_paciente.apellido_materno_paciente AS nombre_completo "+
										"FROM osiris_erp_gestcobrzmov,osiris_his_paciente,osiris_erp_comprobante_pagare " +
										"WHERE osiris_erp_gestcobrzmov.pid_paciente = osiris_his_paciente.pid_paciente "+
										"AND osiris_erp_gestcobrzmov.folio_de_servicio = osiris_erp_comprobante_pagare.folio_de_servicio " +
										"AND osiris_erp_gestcobrzmov.eliminado = 'false' "+
										query_fechas+
										"ORDER BY osiris_erp_gestcobrzmov.fechahora_creacion;";
								
					string[] args_names_field = {"fechacrea","horacrea","telefono","nota","numeropagare","folioservicio","expediente","nombre_completo"};
					string[] args_type_field = {"string","string","string","string","float","float","float","string"};
					string[] args_field_text = {""};
					string[] args_more_title = {""};
					string[,] args_formulas = {{"",""}};
					string[,] args_width = {{"3","13cm"},{"7","11cm"}};
					
					new osiris.class_traslate_spreadsheet(query_sql,args_names_field,args_type_field,false,args_field_text,"",false,args_more_title,args_formulas,args_width);
				}else{
					
				}				
			}
			
			if(tiporpt == "pagares_liquidados"){
				query_fechas = " ";	 
				rango1 = "";
				rango2 = "";
				if (checkbutton_impr_todo_proce.Active == false){
					rango1 = entry_dia1.Text+"-"+entry_mes1.Text+"-"+entry_ano1.Text;
					rango2 = entry_dia2.Text+"-"+entry_mes2.Text+"-"+entry_ano2.Text;
					query_fechas = "AND to_char(osiris_erp_comprobante_pagare.fechahora_pago,'yyyy-MM-dd') >= '"+entry_ano1.Text+"-"+entry_mes1.Text+"-"+entry_dia1.Text+"' "+
									"AND to_char(osiris_erp_comprobante_pagare.fechahora_pago,'yyyy-MM-dd') <= '"+entry_ano2.Text+"-"+entry_mes2.Text+"-"+entry_dia2.Text+"' ";			
				}
				
				if(checkbutton_export_to.Active == true){
					string query_sql = "SELECT osiris_erp_comprobante_pagare.folio_de_servicio AS foliodeservicio,numero_comprobante_pagare AS nro_pagare,"+
						"to_char(osiris_erp_comprobante_pagare.fecha_comprobante,'yyyy-MM-dd') AS fechapagare,to_char(osiris_erp_comprobante_pagare.fecha_vencimiento_pagare,'yyyy-MM-dd') AS vencimientopagare," +
						"to_char(osiris_erp_comprobante_pagare.fechahora_pago,'yyyy-MM-dd') AS fechaliquidado,"+
						"to_char(osiris_erp_comprobante_pagare.pid_paciente,'9999999999') AS pidpaciente," +
						"to_char(osiris_erp_comprobante_pagare.monto_pagare,'99999999.99') AS montopagare, "+
						"nombre1_paciente || ' ' || nombre2_paciente || ' ' || apellido_paterno_paciente || ' ' || apellido_materno_paciente AS nombre_completo," +
						 "descripcion_tipo_paciente AS tipopaciente,osiris_erp_cobros_enca.monto_convenio,(osiris_erp_cobros_enca.total_abonos+osiris_erp_cobros_enca.total_pago) AS tot_abonos," +
						 "osiris_erp_cobros_enca.monto_convenio AS montoconvenio," +
						 "osiris_erp_cobros_enca.total_pago,osiris_erp_cobros_enca.monto_convenio - (osiris_erp_cobros_enca.total_abonos+osiris_erp_cobros_enca.total_pago) AS saldo_deuda " +
						"FROM osiris_erp_comprobante_pagare,osiris_his_paciente,osiris_his_tipo_pacientes,osiris_erp_cobros_enca " +
						"WHERE osiris_erp_comprobante_pagare.pid_paciente = osiris_his_paciente.pid_paciente " +
						"AND osiris_erp_cobros_enca.folio_de_servicio = osiris_erp_comprobante_pagare.folio_de_servicio "+
						"AND osiris_erp_comprobante_pagare.id_tipo_paciente = osiris_his_tipo_pacientes.id_tipo_paciente " +
						"AND osiris_erp_comprobante_pagare.eliminado = 'false' "+
						query_fechas+
						//query_tipo_paciente+
						" ORDER BY osiris_erp_comprobante_pagare.fechahora_pago;";
					string[] args_names_field = {"fechapagare","nro_pagare","vencimientopagare","montopagare","tot_abonos","saldo_deuda","montoconvenio","foliodeservicio","pidpaciente","nombre_completo","tipopaciente","fechaliquidado"};
						//"pagosabonos","pidpaciente","nombre_completo","motivo_ingreso","descripcion_tipo_paciente","descripcion_cirugia","dr_solicita","medicotratante","cerrado"};
					
					string[] args_type_field = {"string","float","string","float","float","float","float","float","float","string","string","string"};
							//,"float","string","string","string","string","string","string","string"};
					
					string[] args_field_text = {""};
					string[] args_more_title = {""};
					string[,] args_formulas = {{"3","=SUM(D2:D"},{"4","=SUM(E2:E"},{"5","=SUM(F2:F"}};
					string[,] args_width = {{"9","7cm"},{"10","5cm"}};
					new osiris.class_traslate_spreadsheet(query_sql,args_names_field,args_type_field,false,args_field_text,"",false,args_more_title,args_formulas,args_width);	
				}else{
					
				}
			}
		}	
					
		void onKeyPressEvent(object o, Gtk.KeyPressEventArgs args)
		{
			//Console.WriteLine(args.Event.Key);
			//Console.WriteLine(Convert.ToChar(args.Event.Key));
			string misDigitos = ".0123456789ﾰﾱﾲﾳﾴﾵﾶﾷﾸﾹﾮｔｒｓｑ（）";
			if (Array.IndexOf(misDigitos.ToCharArray(), Convert.ToChar(args.Event.Key)) == -1 && args.Event.Key.ToString()  != "BackSpace"){
				args.RetVal = true;
			}
		}
		
		private void OnEndPrint (object obj, Gtk.EndPrintArgs args)
		{
			
		}
		
		// cierra ventanas emergentes
		void on_cierraventanas_clicked (object sender, EventArgs args)
		{
			Widget win = (Widget) sender;
			win.Toplevel.Destroy();
		}		
		
		class EventoTitulos : PdfPageEventHelper
		{
			class_public classpublic = new class_public();
									
			#region Fields
			private string _titulo1_rpt;
			private string _titulo2_rpt;
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
			public string titulo2_rpt
        	{
            	get{
					return _titulo2_rpt;
				}
            	set{
					_titulo2_rpt = value;
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
				documento.Add (new Paragraph (" ", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 6)));
								
				Paragraph titulo1_reporte = new Paragraph(titulo1_rpt, new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 13, iTextSharp.text.Font.BOLD, BaseColor.BLACK));
                titulo1_reporte.Alignment = Element.ALIGN_CENTER;
                documento.Add(titulo1_reporte);
				documento.Add (new Paragraph (" ", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 6)));
								
				Paragraph titulo2_reporte = new Paragraph(titulo2_rpt, new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 9, iTextSharp.text.Font.BOLD, BaseColor.BLACK));
                titulo2_reporte.Alignment = Element.ALIGN_LEFT;
                documento.Add(titulo2_reporte);
				documento.Add (new Paragraph (" ", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 6)));
																
				// Creamos una tabla para el contenido
	            PdfPTable tblreporte = new PdfPTable(8);
	            tblreporte.WidthPercentage = 100;
				float[] widths = new float[] { 25f, 30f, 25f, 25f, 65f, 105f, 30f, 30f };	// controlando el ancho de cada columna
				tblreporte.SetWidths(widths);
				tblreporte.HorizontalAlignment = 0;
				iTextSharp.text.Font _standardFont = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 7, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
								
				// Configuramos el título de las columnas de la tabla
				PdfPCell cl01 = new PdfPCell(new Phrase("N° Aten.", _standardFont));
				//clnroatencion.BorderWidth = 1;			// Ancho del borde
				cl01.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
				cl01.HorizontalAlignment = 1;		// centro
				PdfPCell cl02 = new PdfPCell(new Phrase("$ Pagado", _standardFont));
				cl02.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
				cl02.HorizontalAlignment = 1;		// centro
				PdfPCell cl03 = new PdfPCell(new Phrase("Fecha", _standardFont));
				cl03.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
				cl03.HorizontalAlignment = 1;		// centro
				PdfPCell cl04 = new PdfPCell(new Phrase("Fol.Caja", _standardFont));
				cl04.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
				cl04.HorizontalAlignment = 1;		// centro
				PdfPCell cl05 = new PdfPCell(new Phrase("Tipo Comprobante", _standardFont));
				cl05.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
				cl05.HorizontalAlignment = 1;		// centro
				PdfPCell cl06 = new PdfPCell(new Phrase("Observaciones", _standardFont));
				cl06.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
				cl06.HorizontalAlignment = 1;		// centro
				PdfPCell cl07 = new PdfPCell(new Phrase("N° Fact.", _standardFont));
				cl07.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
				cl07.HorizontalAlignment = 1;		// centro
				PdfPCell cl08 = new PdfPCell(new Phrase("Tot.Fact.", _standardFont));
				cl08.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
				cl08.HorizontalAlignment = 1;		// centro
				// Añadimos las celdas a la tabla
				tblreporte.AddCell(cl01);
				tblreporte.AddCell(cl02);
				tblreporte.AddCell(cl03);
				tblreporte.AddCell(cl04);
				tblreporte.AddCell(cl05);
				tblreporte.AddCell(cl06);
				tblreporte.AddCell(cl07);
				tblreporte.AddCell(cl08);
				documento.Add(tblreporte);
		    }
		
		    public override void OnEndPage(PdfWriter writerpdf, Document documento)
		    {
				
		    }
		
		}
	}
}