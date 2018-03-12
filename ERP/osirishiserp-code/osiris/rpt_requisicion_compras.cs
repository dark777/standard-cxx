///////////////////////////////////////////////////////////
// project created on 24/10/2006 at 10:20 a
// Sistema Hospitalario OSIRIS
// Monterrey - Mexico
// created on 10/10/2008 at 10:21 a
// 				: Ing. R. Israel Peña Gonzalez
// Autor    	: Ing. Daniel Olivares C. cambio a GTKPrint con Pango y Cairo arcangeldoc@openmailbox.org
//				  Ing. Daniel Olivares C. Cambio a iTextSharp Nov. 2016
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
/////////////////////////////////////////////////////////
using System;
using Gtk;
using Npgsql;
using Cairo;
using Pango;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using System.Collections;


namespace osiris
{
	public class rpt_requisicion_compras
	{
		private static int pangoScale = 1024;
		private PrintOperation print;
		private double fontSize = 8.0;
		int escala_en_linux_windows;		// Linux = 1  Windows = 8
		int comienzo_linea = 162;
		int separacion_linea = 10;
		int numpage = 1;
		
		string connectionString;						
		string nombrebd;
		string LoginEmpleado;
    	string NomEmpleado;
    	string AppEmpleado;
    	string ApmEmpleado;
    	
    	string querylist = "";
    	
    	// Declarando el treeview
		Gtk.TreeView lista_requisicion_productos;
		Gtk.TreeStore treeViewEngineRequisicion;
    		
    	string titulo = "REQUISICION DE COMPRAS ";
    	string numero_requisicion;
		string status_requisicion;
		string fecha_solicitud;
		string fecha_requerida;
		string observaciones;
		string totalitems_productos;
		string solicitado_por;
		string motivo_de_requi;
		string nombre_proveedor1;
		string nombre_proveedor2;
		string nombre_proveedor3;
		
		string descripcion_tipo_requi;
    	string descripinternamiento;
		string descripinternamiento2;
		string nombrepaciente;
		string folioservicio;
		string pidpaciente;
		float subtotal_requisicion;
		float ivatotal_requisicion;
		float totaltotal_requisicion;
    				
		//Declaracion de ventana de error
		protected Gtk.Window MyWinError;
		protected Gtk.Window MyWin;
		
		class_conexion conexion_a_DB = new class_conexion();
		class_public classpublic = new class_public();
	
		public rpt_requisicion_compras(string nombrebd_,string numero_requisicion_,string status_requisicion_,string fecha_solicitud_,string fecha_requerida_,
									string observaciones_,string totalitems_productos_,
									object lista_requisicion_productos_,object treeViewEngineRequisicion_,string solicitado_por_,string motivo_de_requi_,
		                            string nombre_proveedor1_,string nombre_proveedor2_,string nombre_proveedor3_,
		                            string descripcion_tipo_requi_,
		                            string descripinternamiento_,string descripinternamiento2_,string nombrepaciente_,string folioservicio_,string pidpaciente_,
		                            float subtotal_requisicion_,float ivatotal_requisicion_,float totaltotal_requisicion_)
    	{
    	 	connectionString = conexion_a_DB._url_servidor+conexion_a_DB._port_DB+conexion_a_DB._usuario_DB+conexion_a_DB._passwrd_user_DB;
			nombrebd = conexion_a_DB._nombrebd;
			escala_en_linux_windows = classpublic.escala_linux_windows;
    	 	numero_requisicion = numero_requisicion_;
    		status_requisicion = status_requisicion_;
    		fecha_solicitud = fecha_solicitud_;
    		fecha_requerida = fecha_requerida_;
    		observaciones = observaciones_;
    		lista_requisicion_productos = lista_requisicion_productos_ as Gtk.TreeView;
    		treeViewEngineRequisicion = treeViewEngineRequisicion_ as Gtk.TreeStore;
			solicitado_por = solicitado_por_;
			motivo_de_requi = motivo_de_requi_;
			nombre_proveedor1 = nombre_proveedor1_;
			nombre_proveedor2 = nombre_proveedor2_;
			nombre_proveedor3 = nombre_proveedor3_;
			descripcion_tipo_requi = descripcion_tipo_requi_;
			descripinternamiento = descripinternamiento_;
			descripinternamiento2 = descripinternamiento2_;
			nombrepaciente = nombrepaciente_;
			folioservicio = folioservicio_;
			pidpaciente = pidpaciente_;

			subtotal_requisicion = subtotal_requisicion_;
			ivatotal_requisicion = ivatotal_requisicion_;
			totaltotal_requisicion = totaltotal_requisicion_;

			iTextSharp.text.Font _NormalFont;
			iTextSharp.text.Font _BoldFont;
			_NormalFont = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 7, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);
			_BoldFont = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 7, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
			int nro_de_partida = 1;

			NpgsqlConnection conexion;
			conexion = new NpgsqlConnection (connectionString+nombrebd );
			// Verifica que la base de datos este conectada
			try{
				conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand ();		
				// Buscando numero de requisicion
				comando.CommandText = "SELECT osiris_erp_requisicion_enca.id_requisicion,osiris_erp_requisicion_enca.fechahora_creacion_requisicion,"+
					"osiris_erp_requisicion_enca.id_tipo_admisiones,osiris_his_tipo_admisiones.descripcion_admisiones,"+
					"descripcion_admisiones_cargada,id_tipo_admisiones_cargada,"+
					"to_char(fechahora_envio_compras,'yyyy-MM-dd') AS fechahoraenviocompras,"+
					"to_char(fechahora_creacion_requisicion,'yyyy-MM-dd') AS fechacrearequisicion,"+
					"to_char(fecha_requerida,'yyyy') AS ano_fecha_requerida,"+
					"to_char(fecha_requerida,'MM') AS mes_fecha_requerida,"+
					"to_char(fecha_requerida,'dd') AS dia_fecha_requerida,"+
					"requisicion_ordinaria,requisicion_urgente,osiris_his_tipo_admisiones.mail_centro_costo,"+
					"enviada_a_compras,fechahora_envio_compras,osiris_erp_requisicion_enca.observaciones,motivo_requisicion,"+
					"cancelado,nombre1_empleado,nombre2_empleado,apellido_paterno_empleado,apellido_materno_empleado,"+
					"fechahora_autorizacion_comprar,autorizacion_para_comprar,items_autorizados_paracomprar,total_items_comprados," +
					"osiris_erp_requisicion_enca.folio_de_servicio AS foliodeatencion,"+
					"to_char(osiris_erp_requisicion_enca.pid_paciente,'9999999999') AS pidpaciente,"+
					"nombre1_paciente,nombre2_paciente,apellido_paterno_paciente,apellido_materno_paciente,"+
					"osiris_erp_requisicion_enca.id_tipo_requisicion_compra AS idtiporequicompra,osiris_erp_tipo_requisiciones_compra.descripcion_tipo_requisicion,"+
					"to_char(cantidad_solicitada,'999999.99') AS cantidadsolicitada,osiris_productos.tipo_unidad_producto," +
					"to_char(osiris_productos.cantidad_de_embalaje,'99999.99') AS cantidadembalaje,osiris_erp_requisicion_deta.id_producto AS idproducto,osiris_productos.nombre_articulo AS nombrearticulo," +
					"to_char(osiris_erp_requisicion_deta.costo_producto,'999999999.99') AS costoproducto," +
					"to_char(osiris_erp_requisicion_deta.costo_producto * osiris_erp_requisicion_deta.cantidad_solicitada,'999999999.99') AS importeconcepto "+
					"FROM osiris_erp_requisicion_enca,osiris_his_tipo_admisiones,osiris_empleado,osiris_erp_tipo_requisiciones_compra,osiris_his_paciente,osiris_erp_requisicion_deta,osiris_productos "+
					"WHERE osiris_erp_requisicion_enca.id_tipo_admisiones = osiris_his_tipo_admisiones.id_tipo_admisiones "+
					"AND osiris_erp_requisicion_enca.id_quien_requiso = osiris_empleado.login_empleado " +
					"AND osiris_erp_requisicion_enca.pid_paciente = osiris_his_paciente.pid_paciente "+
					"AND osiris_erp_requisicion_deta.id_producto = osiris_productos.id_producto "+
					"AND osiris_erp_requisicion_enca.id_requisicion = osiris_erp_requisicion_deta.id_requisicion "+
					"AND osiris_erp_requisicion_enca.id_tipo_requisicion_compra = osiris_erp_tipo_requisiciones_compra.id_tipo_requisicion_compra "+
					"AND osiris_erp_requisicion_enca.cancelado = 'false' " +
					"AND osiris_erp_requisicion_deta.eliminado = 'false' "+
					"AND osiris_erp_requisicion_enca.id_requisicion = '"+numero_requisicion_+"';";

				//Console.WriteLine(comando.CommandText);
				NpgsqlDataReader lector = comando.ExecuteReader ();
				if ((bool) lector.Read()){
					// step 1: creation of a document-object
					//Document documento = new Document(iTextSharp.text.PageSize.LETTER, 15, 15, 15, 15);
					Document documento = new Document(PageSize.LETTER.Rotate());
					string pdf_name = System.IO.Path.GetTempPath() + Guid.NewGuid().ToString() + ".pdf";
					try{
						PdfWriter writerpdf = PdfWriter.GetInstance(documento, new FileStream(pdf_name, FileMode.Create));	
						documento.AddTitle("Requisicion de Compras");
						documento.AddCreator("Sistema Hospitalario OSIRIS");
						documento.AddAuthor("Sistema Hospitalario OSIRIS");
						documento.AddSubject("OSIRSrpt");

						EventoTitulos ev = new EventoTitulos();
						ev.titulo1_rpt = "REQUISICION DE COMPRAS";
						ev.solicitante = solicitado_por_;
						ev.departamento_sol = descripinternamiento_;
						ev.departamento_cargo = descripinternamiento2_;
						ev.fecharequisicion = fecha_solicitud_;
						ev.numeroquisicion = numero_requisicion_;
						ev.motivo_requisicion = motivo_de_requi_;
						ev.fecharequerida = fecha_requerida_;
						ev.tiporequisicion = descripcion_tipo_requi_;
						ev.observacionrequi = observaciones_;
						ev.statusrequi = status_requisicion_;
						ev.nombres_apellidos_px = nombrepaciente_;
						ev.numero_atencion_px = folioservicio_;
						ev.nro_expediente_px = pidpaciente_;
						ev.proveedor_a = " ";
						ev.proveedor_b = " ";
						ev.proveedor_c = " ";

						writerpdf.PageEvent = ev;
						documento.Open();


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

						PdfPTable tblConceptos = new PdfPTable(11);
						tblConceptos.WidthPercentage = 100;
						float[] widthsconceptos = new float[] { 10f, 15f, 25f, 15f, 30f, 150f, 25f, 25f, 25f, 25f, 25f };	// controlando el ancho de cada columna tienen que sumas 315 en total
						tblConceptos.SetWidths(widthsconceptos);
						tblConceptos.HorizontalAlignment = 0;

						// Configuramos el título de las columnas de la tabla
						cl01 = new PdfPCell(new Phrase("#", _BoldFont));
						cl01.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER;
						cl01.HorizontalAlignment = 1;
						cl01.BackgroundColor = BaseColor.YELLOW;

						cl02 = new PdfPCell(new Phrase("C.SOL.", _BoldFont));
						cl02.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER;
						cl02.HorizontalAlignment = 1;
						cl02.BackgroundColor = BaseColor.YELLOW;

						cl03 = new PdfPCell(new Phrase("U.MED.", _BoldFont));
						cl03.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER;
						cl03.HorizontalAlignment = 0;
						cl03.BackgroundColor = BaseColor.YELLOW;

						cl04 = new PdfPCell(new Phrase("PAQ.", _BoldFont));
						cl04.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER;
						cl04.HorizontalAlignment = 1;
						cl04.BackgroundColor = BaseColor.YELLOW;

						cl05 = new PdfPCell(new Phrase("SKU", _BoldFont));
						cl05.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER;
						cl05.HorizontalAlignment = 1;
						cl05.BackgroundColor = BaseColor.YELLOW;

						cl06 = new PdfPCell(new Phrase("DESCRIPCION PRODUCTO", _BoldFont));
						cl06.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER;
						cl06.HorizontalAlignment = 1;
						cl06.BackgroundColor = BaseColor.YELLOW;

						cl07 = new PdfPCell(new Phrase("P.PROD.", _BoldFont));
						cl07.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER;
						cl07.HorizontalAlignment = 1;		// centro
						cl07.BackgroundColor = BaseColor.YELLOW;

						cl08 = new PdfPCell(new Phrase("IMPORTE", _BoldFont));
						cl08.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER;
						cl08.HorizontalAlignment = 1;		// centro
						cl08.BackgroundColor = BaseColor.YELLOW;

						cl09 = new PdfPCell(new Phrase("PROVE. A", _BoldFont));
						cl09.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER;
						cl09.HorizontalAlignment = 1;		// centro
						cl09.BackgroundColor = BaseColor.YELLOW;

						cl10 = new PdfPCell(new Phrase("PROVE. B", _BoldFont));
						cl10.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER;
						cl10.HorizontalAlignment = 1;		// centro
						cl10.BackgroundColor = BaseColor.YELLOW;

						cl11 = new PdfPCell(new Phrase("PROVE. C", _BoldFont));
						cl11.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
						cl11.HorizontalAlignment = 1;		// centro
						cl11.BackgroundColor = BaseColor.YELLOW;

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

						// Configuramos el título de las columnas de la tabla
						cl01 = new PdfPCell(new Phrase(nro_de_partida.ToString().Trim(), _NormalFont));
						cl01.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER;
						cl01.HorizontalAlignment = 2;

						cl02 = new PdfPCell(new Phrase(lector["cantidadsolicitada"].ToString().Trim(), _NormalFont));
						cl02.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER;
						cl02.HorizontalAlignment = 2;

						cl03 = new PdfPCell(new Phrase(lector["tipo_unidad_producto"].ToString().Trim(), _NormalFont));
						cl03.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER;
						cl03.HorizontalAlignment = 0;

						cl04 = new PdfPCell(new Phrase(lector["cantidadembalaje"].ToString().Trim(), _NormalFont));
						cl04.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER;
						cl04.HorizontalAlignment = 2;

						cl05 = new PdfPCell(new Phrase(lector["idproducto"].ToString().Trim(), _NormalFont));
						cl05.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER;
						cl05.HorizontalAlignment = 0;		// centro

						cl06 = new PdfPCell(new Phrase(lector["nombrearticulo"].ToString().Trim(), _NormalFont));
						cl06.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER;
						cl06.HorizontalAlignment = 0;

						cl07 = new PdfPCell(new Phrase(String.Format("{0:N}",float.Parse(lector["costoproducto"].ToString().Trim())), _NormalFont));
						cl07.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER;
						cl07.HorizontalAlignment = 2;

						cl08 = new PdfPCell(new Phrase(String.Format("{0:N}",float.Parse(lector["importeconcepto"].ToString().Trim())), _NormalFont));
						cl08.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER;
						cl08.HorizontalAlignment = 2;

						cl09 = new PdfPCell(new Phrase(" ", _NormalFont));
						cl09.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER;
						cl09.HorizontalAlignment = 1;

						cl10 = new PdfPCell(new Phrase(" ", _NormalFont));
						cl10.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER;
						cl10.HorizontalAlignment = 1;

						cl11 = new PdfPCell(new Phrase(" ", _NormalFont));
						cl11.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
						cl11.HorizontalAlignment = 1;

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
						while ((bool) lector.Read()){
							nro_de_partida ++;
							// Configuramos el título de las columnas de la tabla
							cl01 = new PdfPCell(new Phrase(nro_de_partida.ToString().Trim(), _NormalFont));
							cl01.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER;
							cl01.HorizontalAlignment = 2;

							cl02 = new PdfPCell(new Phrase(lector["cantidadsolicitada"].ToString().Trim(), _NormalFont));
							cl02.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER;
							cl02.HorizontalAlignment = 2;

							cl03 = new PdfPCell(new Phrase(lector["tipo_unidad_producto"].ToString().Trim(), _NormalFont));
							cl03.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER;
							cl03.HorizontalAlignment = 0;

							cl04 = new PdfPCell(new Phrase(lector["cantidadembalaje"].ToString().Trim(), _NormalFont));
							cl04.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER;
							cl04.HorizontalAlignment = 2;

							cl05 = new PdfPCell(new Phrase(lector["idproducto"].ToString().Trim(), _NormalFont));
							cl05.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER;
							cl05.HorizontalAlignment = 0;		// centro

							cl06 = new PdfPCell(new Phrase(lector["nombrearticulo"].ToString().Trim(), _NormalFont));
							cl06.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER;
							cl06.HorizontalAlignment = 0;

							cl07 = new PdfPCell(new Phrase(String.Format("{0:N}",float.Parse(lector["costoproducto"].ToString().Trim())), _NormalFont));
							cl07.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER;
							cl07.HorizontalAlignment = 2;

							cl08 = new PdfPCell(new Phrase(String.Format("{0:N}",float.Parse(lector["importeconcepto"].ToString().Trim())), _NormalFont));
							cl08.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER;
							cl08.HorizontalAlignment = 2;

							cl09 = new PdfPCell(new Phrase(" ", _NormalFont));
							cl09.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER;
							cl09.HorizontalAlignment = 1;

							cl10 = new PdfPCell(new Phrase(" ", _NormalFont));
							cl10.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER;
							cl10.HorizontalAlignment = 1;

							cl11 = new PdfPCell(new Phrase(" ", _NormalFont));
							cl11.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
							cl11.HorizontalAlignment = 1;

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
						}

						// Configuramos el título de las columnas de la tabla
						cl01 = new PdfPCell(new Phrase(" ", _NormalFont));
						cl01.Border = iTextSharp.text.Rectangle.TOP_BORDER;
						cl01.HorizontalAlignment = 2;

						cl02 = new PdfPCell(new Phrase(" ", _NormalFont));
						cl02.Border = iTextSharp.text.Rectangle.TOP_BORDER;
						cl02.HorizontalAlignment = 2;

						cl03 = new PdfPCell(new Phrase(" ", _NormalFont));
						cl03.Border = iTextSharp.text.Rectangle.TOP_BORDER;
						cl03.HorizontalAlignment = 0;

						cl04 = new PdfPCell(new Phrase(" ", _NormalFont));
						cl04.Border = iTextSharp.text.Rectangle.TOP_BORDER;
						cl04.HorizontalAlignment = 2;

						cl05 = new PdfPCell(new Phrase(" ", _NormalFont));
						cl05.Border = iTextSharp.text.Rectangle.TOP_BORDER;
						cl05.HorizontalAlignment = 0;		// centro

						cl06 = new PdfPCell(new Phrase(" ", _NormalFont));
						cl06.Border = iTextSharp.text.Rectangle.TOP_BORDER;
						cl06.HorizontalAlignment = 2;

						cl07 = new PdfPCell(new Phrase("SUBTOTAL", _BoldFont));
						cl07.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER;
						cl07.HorizontalAlignment = 2;

						cl08 = new PdfPCell(new Phrase(String.Format("{0:N}",subtotal_requisicion), _NormalFont));
						cl08.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
						cl08.HorizontalAlignment = 2;

						cl09 = new PdfPCell(new Phrase(" ", _NormalFont));
						cl09.Border = iTextSharp.text.Rectangle.TOP_BORDER;
						cl09.HorizontalAlignment = 1;

						cl10 = new PdfPCell(new Phrase(" ", _NormalFont));
						cl10.Border = iTextSharp.text.Rectangle.TOP_BORDER;
						cl10.HorizontalAlignment = 1;

						cl11 = new PdfPCell(new Phrase(" ", _NormalFont));
						cl11.Border = iTextSharp.text.Rectangle.TOP_BORDER;
						cl11.HorizontalAlignment = 1;

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

						// Configuramos el título de las columnas de la tabla
						cl01 = new PdfPCell(new Phrase(" ", _NormalFont));
						cl01.Border = iTextSharp.text.Rectangle.NO_BORDER;
						cl01.HorizontalAlignment = 2;

						cl02 = new PdfPCell(new Phrase(" ", _NormalFont));
						cl02.Border = iTextSharp.text.Rectangle.NO_BORDER;
						cl02.HorizontalAlignment = 2;

						cl03 = new PdfPCell(new Phrase(" ", _NormalFont));
						cl03.Border = iTextSharp.text.Rectangle.NO_BORDER;
						cl03.HorizontalAlignment = 0;

						cl04 = new PdfPCell(new Phrase(" ", _NormalFont));
						cl04.Border = iTextSharp.text.Rectangle.NO_BORDER;
						cl04.HorizontalAlignment = 2;

						cl05 = new PdfPCell(new Phrase(" ", _NormalFont));
						cl05.Border = iTextSharp.text.Rectangle.NO_BORDER;
						cl05.HorizontalAlignment = 0;		// centro

						cl06 = new PdfPCell(new Phrase(" ", _NormalFont));
						cl06.Border = iTextSharp.text.Rectangle.NO_BORDER;
						cl06.HorizontalAlignment = 2;

						cl07 = new PdfPCell(new Phrase("IVA", _BoldFont));
						cl07.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER;
						cl07.HorizontalAlignment = 2;

						cl08 = new PdfPCell(new Phrase(String.Format("{0:N}",ivatotal_requisicion), _NormalFont));
						cl08.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
						cl08.HorizontalAlignment = 2;

						cl09 = new PdfPCell(new Phrase(" ", _NormalFont));
						cl09.Border = iTextSharp.text.Rectangle.NO_BORDER;
						cl09.HorizontalAlignment = 1;

						cl10 = new PdfPCell(new Phrase(" ", _NormalFont));
						cl10.Border = iTextSharp.text.Rectangle.NO_BORDER;
						cl10.HorizontalAlignment = 1;

						cl11 = new PdfPCell(new Phrase(" ", _NormalFont));
						cl11.Border = iTextSharp.text.Rectangle.NO_BORDER;
						cl11.HorizontalAlignment = 1;

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

						// Configuramos el título de las columnas de la tabla
						cl01 = new PdfPCell(new Phrase(" ", _NormalFont));
						cl01.Border = iTextSharp.text.Rectangle.NO_BORDER;
						cl01.HorizontalAlignment = 2;

						cl02 = new PdfPCell(new Phrase(" ", _NormalFont));
						cl02.Border = iTextSharp.text.Rectangle.NO_BORDER;
						cl02.HorizontalAlignment = 2;

						cl03 = new PdfPCell(new Phrase(" ", _NormalFont));
						cl03.Border = iTextSharp.text.Rectangle.NO_BORDER;
						cl03.HorizontalAlignment = 0;

						cl04 = new PdfPCell(new Phrase(" ", _NormalFont));
						cl04.Border = iTextSharp.text.Rectangle.NO_BORDER;
						cl04.HorizontalAlignment = 2;

						cl05 = new PdfPCell(new Phrase(" ", _NormalFont));
						cl05.Border = iTextSharp.text.Rectangle.NO_BORDER;
						cl05.HorizontalAlignment = 0;		// centro

						cl06 = new PdfPCell(new Phrase(" ", _NormalFont));
						cl06.Border = iTextSharp.text.Rectangle.NO_BORDER;
						cl06.HorizontalAlignment = 2;

						cl07 = new PdfPCell(new Phrase("T O T A L", _BoldFont));
						cl07.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER;
						cl07.HorizontalAlignment = 2;

						cl08 = new PdfPCell(new Phrase(String.Format("{0:N}",totaltotal_requisicion), _NormalFont));
						cl08.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
						cl08.HorizontalAlignment = 2;

						cl09 = new PdfPCell(new Phrase(" ", _NormalFont));
						cl09.Border = iTextSharp.text.Rectangle.NO_BORDER;
						cl09.HorizontalAlignment = 1;

						cl10 = new PdfPCell(new Phrase(" ", _NormalFont));
						cl10.Border = iTextSharp.text.Rectangle.NO_BORDER;
						cl10.HorizontalAlignment = 1;

						cl11 = new PdfPCell(new Phrase(" ", _NormalFont));
						cl11.Border = iTextSharp.text.Rectangle.NO_BORDER;
						cl11.HorizontalAlignment = 1;

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
						documento.Add (new Paragraph (" ", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 5)));
						documento.Add (new Paragraph ("                  ____________________________                     ____________________________", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 7)));
						documento.Add (new Paragraph ("                      Nombre y Firma Solicitante                                 Nombre y Firma Autorización", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 7)));

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
				}else{
					MessageDialog msgBox = new MessageDialog (MyWin,DialogFlags.Modal,
						MessageType.Info,ButtonsType.Ok,"La requisicion NO esta disponible o se encuentra CANCELADA, verifique...");
					msgBox.Run ();		msgBox.Destroy();
				}			
			}catch (NpgsqlException ex){
				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
					MessageType.Error, 
					ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
				msgBoxError.Run ();						msgBoxError.Destroy();
			}
			conexion.Close();
		}

		private class EventoTitulos : PdfPageEventHelper
		{
			class_public classpublic = new class_public();
			PdfContentByte cb;
			iTextSharp.text.pdf.PdfTemplate template;

			#region Fields
			private string _titulo1_rpt;
			private string _solicitante;
			private string _departamento_sol;
			private string _departamento_cargo;
			private string _fecharequisicion;
			private string _numeroquisicion;
			private string _motivo_requisicion;
			private string _fecharequerida;
			private string _tiporequisicion;
			private string _observacionrequi;
			private string _statusrequi;
			private string _nombres_apellidos_px;
			private string _numero_atencion_px;
			private string _nro_expediente_px;
			private string _proveedor_a;
			private string _proveedor_b;
			private string _proveedor_c;
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
			public string solicitante
			{
				get{
					return _solicitante;
				}
				set{
					_solicitante = value;
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
			public string departamento_cargo
			{
				get{
					return _departamento_cargo;
				}
				set{
					_departamento_cargo = value;
				}
			}
			public string fecharequisicion
			{
				get{
					return _fecharequisicion;
				}
				set{
					_fecharequisicion = value;
				}
			}
			public string numeroquisicion
			{
				get{
					return _numeroquisicion;
				}
				set{
					_numeroquisicion = value;
				}
			}
			public string motivo_requisicion
			{
				get{
					return _motivo_requisicion;
				}
				set{
					_motivo_requisicion = value;
				}
			}
			public string fecharequerida
			{
				get{
					return _fecharequerida;
				}
				set{
					_fecharequerida = value;
				}
			}
			public string tiporequisicion
			{
				get{
					return _tiporequisicion;
				}
				set{
					_tiporequisicion = value;
				}
			}
			public string observacionrequi
			{
				get{
					return _observacionrequi;
				}
				set{
					_observacionrequi = value;
				}
			}
			public string statusrequi
			{
				get{
					return _statusrequi;
				}
				set{
					_statusrequi = value;
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
			public string proveedor_a
			{
				get{
					return _proveedor_a;
				}
				set{
					_proveedor_a = value;
				}
			}
			public string proveedor_b
			{
				get{
					return _proveedor_b;
				}
				set{
					_proveedor_b = value;
				}
			}
			public string proveedor_c
			{
				get{
					return _proveedor_c;
				}
				set{
					_proveedor_c = value;
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

				documento.Add (new Paragraph (" ", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 5)));

				Paragraph titulo1_reporte = new Paragraph(titulo1_rpt, new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 12, iTextSharp.text.Font.BOLD, BaseColor.BLACK));
				titulo1_reporte.Alignment = Element.ALIGN_CENTER;
				documento.Add(titulo1_reporte);
				documento.Add (new Paragraph (" ", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 8)));

				// fuente para las tablas creadas
				iTextSharp.text.Font _NormalFont = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 7, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);
				iTextSharp.text.Font _BoldFont = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 7, iTextSharp.text.Font.BOLD, BaseColor.BLACK);

				PdfPTable tblrequisicion1 = new PdfPTable(5);
				tblrequisicion1.WidthPercentage = 100;
				float[] widths1 = new float[] { 60f, 60f, 60f, 40f, 30f };	// controlando el ancho de cada columna tienen que sumas 315 en total
				tblrequisicion1.SetWidths(widths1);
				tblrequisicion1.HorizontalAlignment = 0;

				// Configuramos el título de las columnas de la tabla
				cl01 = new PdfPCell(new Phrase("SOLICITANTE", _BoldFont));
				cl01.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER;
				//clnroatencion.BorderWidth = 1;			// Ancho del borde
				cl01.HorizontalAlignment = 1;
				cl02 = new PdfPCell(new Phrase("DEPARTAMENTO", _BoldFont));
				cl02.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER;
				cl02.HorizontalAlignment = 1;
				cl03 = new PdfPCell(new Phrase("CUENTA DE CARGO", _BoldFont));
				cl03.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER;
				cl03.HorizontalAlignment = 1;
				cl04 = new PdfPCell(new Phrase("Fecha Requisicion", _BoldFont));
				cl04.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER;
				cl04.HorizontalAlignment = 1;
				cl05 = new PdfPCell(new Phrase("N° REQUISICION", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 8, iTextSharp.text.Font.BOLD, BaseColor.BLACK)));
				cl05.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER;
				cl05.HorizontalAlignment = 1;
				
				// Añadimos las celdas a la tabla
				tblrequisicion1.AddCell(cl01);
				tblrequisicion1.AddCell(cl02);
				tblrequisicion1.AddCell(cl03);
				tblrequisicion1.AddCell(cl04);
				tblrequisicion1.AddCell(cl05);

				// Configuramos el título de las columnas de la tabla
				cl01 = new PdfPCell(new Phrase(solicitante , _NormalFont));
				//clnroatencion.BorderWidth = 1;			// Ancho del borde
				cl01.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER;
				cl01.HorizontalAlignment = 1;
				cl02 = new PdfPCell(new Phrase(departamento_sol, _NormalFont));
				cl02.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER;
				cl02.HorizontalAlignment = 1;
				cl03 = new PdfPCell(new Phrase(departamento_cargo, _NormalFont));
				cl03.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER;
				cl03.HorizontalAlignment = 1;
				cl04 = new PdfPCell(new Phrase(fecharequisicion, _NormalFont));
				cl04.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER;
				cl04.HorizontalAlignment = 1;
				cl05 = new PdfPCell(new Phrase(numeroquisicion, new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 8, iTextSharp.text.Font.BOLD, BaseColor.BLACK)));
				cl05.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER;
				cl05.HorizontalAlignment = 1;

				// Añadimos las celdas a la tabla
				tblrequisicion1.AddCell(cl01);
				tblrequisicion1.AddCell(cl02);
				tblrequisicion1.AddCell(cl03);
				tblrequisicion1.AddCell(cl04);
				tblrequisicion1.AddCell(cl05);

				documento.Add(tblrequisicion1);


				PdfPTable tblrequisicion2 = new PdfPTable(6);
				tblrequisicion2.WidthPercentage = 100;
				float[] widths2 = new float[] { 50f,220f,35f,35f,40,35f};	// controlando el ancho de cada columna tienen que sumas 315 en total
				tblrequisicion2.SetWidths(widths2);
				tblrequisicion2.HorizontalAlignment = 0;

				// Configuramos el título de las columnas de la tabla
				cl01 = new PdfPCell(new Phrase("Motivo Requisicion", _BoldFont));
				cl01.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER;
				//clnroatencion.BorderWidth = 1;			// Ancho del borde
				cl01.HorizontalAlignment = 1;
				cl02 = new PdfPCell(new Phrase(motivo_requisicion, _NormalFont));
				cl02.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER;
				cl02.HorizontalAlignment = 0;

				cl03 = new PdfPCell(new Phrase("Fecha Requerida", _BoldFont));
				cl03.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER;
				cl03.HorizontalAlignment = 1;
				cl04 = new PdfPCell(new Phrase(fecharequerida, _NormalFont));
				cl04.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER;
				cl04.HorizontalAlignment = 0;

				cl05 = new PdfPCell(new Phrase("Tipo REQUISICION", _BoldFont));
				cl05.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER;
				cl05.HorizontalAlignment = 1;

				cl06 = new PdfPCell(new Phrase(tiporequisicion, _NormalFont));
				cl06.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER;
				cl06.HorizontalAlignment = 0;

				// Añadimos las celdas a la tabla
				tblrequisicion2.AddCell(cl01);
				tblrequisicion2.AddCell(cl02);
				tblrequisicion2.AddCell(cl03);
				tblrequisicion2.AddCell(cl04);
				tblrequisicion2.AddCell(cl05);
				tblrequisicion2.AddCell(cl06);

				documento.Add(tblrequisicion2);

				PdfPTable tblrequisicion3 = new PdfPTable(4);
				tblrequisicion3.WidthPercentage = 100;
				float[] widths3 = new float[] { 52f,200f,40f,140f};	// controlando el ancho de cada columna tienen que sumas 315 en total
				tblrequisicion3.SetWidths(widths3);
				tblrequisicion3.HorizontalAlignment = 0;

				// Configuramos el título de las columnas de la tabla
				cl01 = new PdfPCell(new Phrase("Observacion", _BoldFont));
				cl01.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER;
				//clnroatencion.BorderWidth = 1;			// Ancho del borde
				cl01.HorizontalAlignment = 1;
				cl02 = new PdfPCell(new Phrase(observacionrequi, _NormalFont));
				cl02.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER;
				cl02.HorizontalAlignment = 0;

				cl03 = new PdfPCell(new Phrase("Status Req.", _BoldFont));
				cl03.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER;
				cl03.HorizontalAlignment = 1;
				cl04 = new PdfPCell(new Phrase(statusrequi, _NormalFont));
				cl04.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER;
				cl04.HorizontalAlignment = 0;
				// Añadimos las celdas a la tabla
				tblrequisicion3.AddCell(cl01);
				tblrequisicion3.AddCell(cl02);
				tblrequisicion3.AddCell(cl03);
				tblrequisicion3.AddCell(cl04);
				documento.Add(tblrequisicion3);

				PdfPTable tblrequisicion4 = new PdfPTable(6);
				tblrequisicion4.WidthPercentage = 100;
				float[] widths4 = new float[] { 50f,220f,35f,35f,40,35f};	// controlando el ancho de cada columna tienen que sumas 315 en total
				tblrequisicion4.SetWidths(widths4);
				tblrequisicion4.HorizontalAlignment = 0;

				// Configuramos el título de las columnas de la tabla
				cl01 = new PdfPCell(new Phrase("Nombre Paciente", _BoldFont));
				cl01.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER;
				//clnroatencion.BorderWidth = 1;			// Ancho del borde
				cl01.HorizontalAlignment = 1;
				cl02 = new PdfPCell(new Phrase(nombres_apellidos_px, _NormalFont));
				cl02.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER;
				cl02.HorizontalAlignment = 0;

				cl03 = new PdfPCell(new Phrase("N° Atencion", _BoldFont));
				cl03.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER;
				cl03.HorizontalAlignment = 1;
				cl04 = new PdfPCell(new Phrase(numero_atencion_px, _NormalFont));
				cl04.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER;
				cl04.HorizontalAlignment = 0;

				cl05 = new PdfPCell(new Phrase("N° Expediente", _BoldFont));
				cl05.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER;
				cl05.HorizontalAlignment = 1;

				cl06 = new PdfPCell(new Phrase(nro_expediente_px, _NormalFont));
				cl06.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER;
				cl06.HorizontalAlignment = 0;

				// Añadimos las celdas a la tabla
				tblrequisicion4.AddCell(cl01);
				tblrequisicion4.AddCell(cl02);
				tblrequisicion4.AddCell(cl03);
				tblrequisicion4.AddCell(cl04);
				tblrequisicion4.AddCell(cl05);
				tblrequisicion4.AddCell(cl06);
				documento.Add(tblrequisicion4);

				documento.Add (new Paragraph (" ", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 5)));

				PdfPTable tblrequisicion5 = new PdfPTable(3);
				tblrequisicion5.WidthPercentage = 100;
				float[] widths5 = new float[] { 105f,105f,105f};	// controlando el ancho de cada columna tienen que sumas 315 en total
				tblrequisicion5.SetWidths(widths5);
				tblrequisicion5.HorizontalAlignment = 0;

				// Configuramos el título de las columnas de la tabla
				cl01 = new PdfPCell(new Phrase("PROVEEDOR A", _BoldFont));
				cl01.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER;
				//clnroatencion.BorderWidth = 1;			// Ancho del borde
				cl01.HorizontalAlignment = 1;

				cl02 = new PdfPCell(new Phrase("PROVEEDOR B", _BoldFont));
				cl02.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER;
				cl02.HorizontalAlignment = 1;

				cl03 = new PdfPCell(new Phrase("PROVEEDOR C", _BoldFont));
				cl03.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
				cl03.HorizontalAlignment = 1;

				// Añadimos las celdas a la tabla
				tblrequisicion5.AddCell(cl01);
				tblrequisicion5.AddCell(cl02);
				tblrequisicion5.AddCell(cl03);

				// Configuramos el título de las columnas de la tabla
				cl01 = new PdfPCell(new Phrase(proveedor_a, _NormalFont));
				cl01.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER;
				//clnroatencion.BorderWidth = 1;			// Ancho del borde
				cl01.HorizontalAlignment = 0;

				cl02 = new PdfPCell(new Phrase(proveedor_b, _NormalFont));
				cl02.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER;
				cl02.HorizontalAlignment = 0;

				cl03 = new PdfPCell(new Phrase(proveedor_c, _NormalFont));
				cl03.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER;
				cl03.HorizontalAlignment = 0;

				// Añadimos las celdas a la tabla
				tblrequisicion5.AddCell(cl01);
				tblrequisicion5.AddCell(cl02);
				tblrequisicion5.AddCell(cl03);

				documento.Add(tblrequisicion5);


				documento.Add (new Paragraph (" ", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 5)));

			}

			public override void OnEndPage(PdfWriter writerpdf, Document documento)
			{
				base.OnEndPage(writerpdf, documento);
				int pageN = writerpdf.PageNumber;
				iTextSharp.text.pdf.BaseFont bf = BaseFont.CreateFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
				iTextSharp.text.Rectangle pageSize = documento.PageSize;
				String text = "N° Page : "+writerpdf.PageNumber.ToString().Trim()+" of ";
				float len = bf.GetWidthPoint(text, 6);
				cb = writerpdf.DirectContent;
				// we tell the ContentByte we're ready to draw text
				cb.BeginText ();
				cb.SetFontAndSize (bf, 6);
				cb.SetTextMatrix (670, 550);
				cb.ShowText (text);
				cb.EndText ();
				cb.AddTemplate (template, documento.LeftMargin, pageSize.GetBottom(-18)); //documento.LeftMargin + len, pageSize.GetBottom(documento.BottomMargin));
			}

			public override void OnCloseDocument(PdfWriter writerpdf, Document documento)
			{
				base.OnCloseDocument(writerpdf, documento);
				iTextSharp.text.pdf.BaseFont bf = BaseFont.CreateFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);

				// we tell the ContentByte we're ready to draw text
				template.BeginText ();
				template.SetFontAndSize (bf, 6);
				template.SetTextMatrix (670, 590);
				template.ShowText (" "+(writerpdf.PageNumber - 1).ToString());
				template.EndText ();
				Console.WriteLine (writerpdf.PageNumber.ToString ());
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