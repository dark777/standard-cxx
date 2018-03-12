///////////////////////////////////////////////////////////
// project created on 23/10/2008 at 10:20 a
// Sistema Hospitalario OSIRIS
// Monterrey - Mexico
//
// 				: Israel Peña Gonzalez
// Autor    	: Ing. Daniel Olivares C. cambio a GTKPrint con Pango y Cairo arcangeldoc@openmailbox.org 18/11/2010
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

namespace osiris
{
	public class rpt_orden_compras
	{
		string connectionString;						
		string nombrebd;
		string LoginEmpleado;
    	string NomEmpleado;
    	string AppEmpleado;
    	string ApmEmpleado;
		string query_general = "SELECT osiris_erp_ordenes_compras_enca.numero_orden_compra,osiris_erp_ordenes_compras_enca.id_proveedor,osiris_erp_ordenes_compras_enca.descripcion_proveedor," +
		                       "osiris_erp_ordenes_compras_enca.direccion_proveedor,to_char(id_requisicion,'9999999999') AS idrequisicion,osiris_erp_requisicion_deta.porcentage_iva," +
		                       "osiris_erp_ordenes_compras_enca.rfc_proveedor,osiris_erp_ordenes_compras_enca.telefonos_proveedor,to_char(osiris_erp_ordenes_compras_enca.fecha_de_entrega,'yyyy-MM-dd') AS fechadeentrega," +
		                       "osiris_erp_ordenes_compras_enca.lugar_de_entrega,osiris_erp_ordenes_compras_enca.condiciones_de_pago,osiris_erp_ordenes_compras_enca.dep_solicitante," +
		                       "osiris_erp_ordenes_compras_enca.observaciones,to_char(osiris_erp_ordenes_compras_enca.fecha_deorden_compra,'yyyy-MM-dd') AS fechaordencompra," +
		                       "to_char(cantidad_comprada,'999999999.99') AS cantidadcomprada,to_char(osiris_erp_requisicion_deta.cantidad_de_embalaje_compra,'999999.99') AS cantidadembalaje," +
		                       "osiris_productos.id_producto,osiris_productos.descripcion_producto,osiris_catalogo_productos_proveedores.descripcion_producto AS descripcionproducto,osiris_productos.aplicar_iva," +
		                       "to_char(precio_costo_prov_selec,'999999999.99') AS preciodelproveedor," +
		                       "osiris_erp_requisicion_deta.tipo_unidad_producto AS tipounidadproducto,tipo_orden_compra," +
		                       "rfc,emisor,calle,noexterior,nointerior,colonia,municipio,estado,codigopostal " +
		                       "FROM osiris_erp_ordenes_compras_enca,osiris_erp_proveedores,osiris_erp_requisicion_deta,osiris_productos,osiris_catalogo_productos_proveedores,osiris_erp_emisor " +
		                       "WHERE osiris_erp_ordenes_compras_enca.id_proveedor = osiris_erp_proveedores.id_proveedor " +
		                       "AND osiris_erp_ordenes_compras_enca.numero_orden_compra = osiris_erp_requisicion_deta.numero_orden_compra " +
		                       "AND osiris_erp_requisicion_deta.id_producto = osiris_productos.id_producto " +
		                       "AND osiris_catalogo_productos_proveedores.id_producto = osiris_erp_requisicion_deta.id_producto " +
		                       "AND osiris_erp_ordenes_compras_enca.id_proveedor = osiris_catalogo_productos_proveedores.id_proveedor " +
		                       "AND osiris_erp_ordenes_compras_enca.id_emisor =  osiris_erp_requisicion_deta.id_emisor " +
		                       "AND osiris_erp_ordenes_compras_enca.id_emisor = osiris_erp_emisor.id_emisor " +
		                       "AND osiris_erp_requisicion_deta.eliminado = 'false' " +
		                       "AND osiris_catalogo_productos_proveedores.eliminado = 'false' ";
		string orden_por = " ORDER BY id_orden_compra,osiris_erp_requisicion_deta.id_secuencia;";
		float valoriva;
				
		//Declaracion de ventana de error
		protected Gtk.Window MyWinError;
		protected Gtk.Window MyWin;
		
		class_conexion conexion_a_DB = new class_conexion();
		class_public classpublic = new class_public();
		
		public rpt_orden_compras(string query_ordenescompra,string query_fechas)
		{
			connectionString = conexion_a_DB._url_servidor+conexion_a_DB._port_DB+conexion_a_DB._usuario_DB+conexion_a_DB._passwrd_user_DB;
			nombrebd = conexion_a_DB._nombrebd;
			valoriva = float.Parse(classpublic.ivaparaaplicar);
			query_general = query_general +					
							query_ordenescompra+" " +
							query_fechas+
							orden_por;

			iTextSharp.text.Font _NormalFont;
			iTextSharp.text.Font _BoldFont;
			_NormalFont = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 7, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);
			_BoldFont = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 7, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
			int nro_de_partida = 1;
			float precios_total = 0;
			float iva_total = 0;
			float total_total = 0;
			float calculo_iva = 0;
			float precio_mas_iva = 0;


			NpgsqlConnection conexion;
			conexion = new NpgsqlConnection (connectionString+nombrebd );
			// Verifica que la base de datos este conectada
			try{
				conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand ();		
				// Buscando numero de requisicion
				comando.CommandText = query_general;
				NpgsqlDataReader lector = comando.ExecuteReader ();
				if ((bool) lector.Read()){
					// step 1: creation of a document-object
					//Document documento = new Document(iTextSharp.text.PageSize.LETTER, 15, 15, 15, 15);
					Document documento = new Document(PageSize.LETTER.Rotate());
					string pdf_name = System.IO.Path.GetTempPath() + Guid.NewGuid().ToString() + ".pdf";
					try{
						PdfWriter writerpdf = PdfWriter.GetInstance(documento, new FileStream(pdf_name, FileMode.Create));	
						documento.AddTitle("Orden de Compras");
						documento.AddCreator("Sistema Hospitalario OSIRIS");
						documento.AddAuthor("Sistema Hospitalario OSIRIS");
						documento.AddSubject("OSIRSrpt");

						EventoTitulos ev = new EventoTitulos();
						ev.titulo1_rpt = "ORDEN DE COMPRAS";
						ev.nombreproveedor = lector["descripcion_proveedor"].ToString().Trim();
						ev.rfcproveedor = lector["rfc_proveedor"].ToString().Trim();
						ev.direccproveedor = lector["direccion_proveedor"].ToString().Trim();
						ev.telproveedor = lector["telefonos_proveedor"].ToString().Trim();
						ev.fechaordencompra = lector["fechaordencompra"].ToString().Trim();
						ev.numeroordencompra = lector["numero_orden_compra"].ToString().Trim();
						ev.lugardeentrega = lector["lugar_de_entrega"].ToString().Trim();
						ev.fechaentrega = lector["fechadeentrega"].ToString().Trim();
						ev.tipoordencompra = lector["tipo_orden_compra"].ToString().Trim();
						ev.observaciones_oc = lector["observaciones"].ToString().Trim();
						ev.facturarreceptor_a = classpublic.extract_spaces((string) lector["rfc"].ToString().Trim()+"-"+
							(string) lector["emisor"].ToString().Trim()+" "+
							(string) lector["calle"].ToString().Trim() +
							(string) lector["noexterior"].ToString().Trim()+" "+
							(string) lector["nointerior"].ToString().Trim()+", COL."+
							(string) lector["colonia"].ToString().Trim()+","+
							(string) lector["municipio"].ToString().Trim()+","+
							(string) lector["estado"].ToString().Trim()+","+
							(string) lector["codigopostal"].ToString().Trim());


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

						PdfPTable tblConceptos = new PdfPTable(10);
						tblConceptos.WidthPercentage = 100;
						float[] widthsconceptos = new float[] { 10f, 17f, 25f, 25f, 150f, 25f, 25f, 25f, 25f, 25f };	// controlando el ancho de cada columna tienen que sumas 315 en total
						tblConceptos.SetWidths(widthsconceptos);
						tblConceptos.HorizontalAlignment = 0;

						// Configuramos el título de las columnas de la tabla
						cl01 = new PdfPCell(new Phrase("#", _BoldFont));
						cl01.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER;
						cl01.HorizontalAlignment = 1;
						cl01.BackgroundColor = BaseColor.YELLOW;

						cl02 = new PdfPCell(new Phrase("CANT.", _BoldFont));
						cl02.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER;
						cl02.HorizontalAlignment = 1;
						cl02.BackgroundColor = BaseColor.YELLOW;

						cl03 = new PdfPCell(new Phrase("U.MED.", _BoldFont));
						cl03.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER;
						cl03.HorizontalAlignment = 1;
						cl03.BackgroundColor = BaseColor.YELLOW;

						cl04 = new PdfPCell(new Phrase("PAQ.", _BoldFont));
						cl04.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER;
						cl04.HorizontalAlignment = 1;
						cl04.BackgroundColor = BaseColor.YELLOW;

						cl05 = new PdfPCell(new Phrase("DESCRIPCION PRODUCTO", _BoldFont));
						cl05.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER;
						cl05.HorizontalAlignment = 1;
						cl05.BackgroundColor = BaseColor.YELLOW;

						cl06 = new PdfPCell(new Phrase("REQU.", _BoldFont));
						cl06.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER;
						cl06.HorizontalAlignment = 1;		// centro
						cl06.BackgroundColor = BaseColor.YELLOW;

						cl07 = new PdfPCell(new Phrase("PRECIO", _BoldFont));
						cl07.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER;
						cl07.HorizontalAlignment = 1;		// centro
						cl07.BackgroundColor = BaseColor.YELLOW;

						cl08 = new PdfPCell(new Phrase("SUB-TOTAL", _BoldFont));
						cl08.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER;
						cl08.HorizontalAlignment = 1;		// centro
						cl08.BackgroundColor = BaseColor.YELLOW;

						cl09 = new PdfPCell(new Phrase("IVA", _BoldFont));
						cl09.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER;
						cl09.HorizontalAlignment = 1;		// centro
						cl09.BackgroundColor = BaseColor.YELLOW;

						cl10 = new PdfPCell(new Phrase("TOTAL", _BoldFont));
						cl10.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
						cl10.HorizontalAlignment = 1;		// centro
						cl10.BackgroundColor = BaseColor.YELLOW;

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

						precios_total = 0;
						iva_total = 0;
						total_total = 0;

						if((bool) lector["aplicar_iva"] == true){
							calculo_iva = ((float.Parse((string) lector["preciodelproveedor"].ToString().Trim()) * float.Parse((string) lector["cantidadcomprada"].ToString().Trim())) * float.Parse((string) lector["porcentage_iva"].ToString().Trim()))/100;
							precio_mas_iva = (float.Parse((string) lector["preciodelproveedor"].ToString().Trim()) * float.Parse((string) lector["cantidadcomprada"].ToString().Trim())) + calculo_iva;
						}else{
							calculo_iva = 0;
							precio_mas_iva = (float.Parse((string) lector["preciodelproveedor"].ToString().Trim()) * float.Parse((string) lector["cantidadcomprada"].ToString().Trim())) + calculo_iva;
						}
						precios_total = float.Parse((string) lector["preciodelproveedor"].ToString().Trim()) * float.Parse((string) lector["cantidadcomprada"].ToString().Trim());
						iva_total += calculo_iva;
						total_total += precio_mas_iva;

						// Configuramos el título de las columnas de la tabla
						cl01 = new PdfPCell(new Phrase(nro_de_partida.ToString().Trim(), _BoldFont));
						cl01.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER;
						cl01.HorizontalAlignment = 2;

						cl02 = new PdfPCell(new Phrase(float.Parse(lector["cantidadcomprada"].ToString().Trim()).ToString("F"), _NormalFont));
						cl02.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER;
						cl02.HorizontalAlignment = 2;

						cl03 = new PdfPCell(new Phrase(lector["tipounidadproducto"].ToString().Trim(), _NormalFont));
						cl03.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER;
						cl03.HorizontalAlignment = 0;

						cl04 = new PdfPCell(new Phrase(float.Parse(lector["cantidadembalaje"].ToString().Trim()).ToString("F"), _NormalFont));
						cl04.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER;
						cl04.HorizontalAlignment = 2;

						cl05 = new PdfPCell(new Phrase(lector["descripcionproducto"].ToString().Trim(), _NormalFont));
						cl05.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER;
						cl05.HorizontalAlignment = 0;

						cl06 = new PdfPCell(new Phrase(lector["idrequisicion"].ToString().Trim(), _NormalFont));
						cl06.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER;
						cl06.HorizontalAlignment = 2;		// centro

						cl07 = new PdfPCell(new Phrase(String.Format("{0:N}",float.Parse((string) lector["preciodelproveedor"].ToString().Trim())), _NormalFont));
						cl07.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER;
						cl07.HorizontalAlignment = 2;		// centro

						cl08 = new PdfPCell(new Phrase(String.Format("{0:N}",float.Parse((string) lector["preciodelproveedor"].ToString().Trim()) * float.Parse((string) lector["cantidadcomprada"].ToString().Trim())), _NormalFont));
						cl08.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER;
						cl08.HorizontalAlignment = 2;		// centro

						cl09 = new PdfPCell(new Phrase(String.Format("{0:N}",calculo_iva), _NormalFont));
						cl09.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER;
						cl09.HorizontalAlignment = 2;		// centro

						cl10 = new PdfPCell(new Phrase(String.Format("{0:N}",precio_mas_iva), _NormalFont));
						cl10.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
						cl10.HorizontalAlignment = 2;		// centro

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

						while ((bool) lector.Read()){
							nro_de_partida ++;
							if((bool) lector["aplicar_iva"] == true){
								calculo_iva = ((float.Parse((string) lector["preciodelproveedor"].ToString().Trim()) * float.Parse((string) lector["cantidadcomprada"].ToString().Trim())) * float.Parse((string) lector["porcentage_iva"].ToString().Trim()))/100;
								precio_mas_iva = (float.Parse((string) lector["preciodelproveedor"].ToString().Trim()) * float.Parse((string) lector["cantidadcomprada"].ToString().Trim())) + calculo_iva;
							}else{
								calculo_iva = 0;
								precio_mas_iva = (float.Parse((string) lector["preciodelproveedor"].ToString().Trim()) * float.Parse((string) lector["cantidadcomprada"].ToString().Trim())) + calculo_iva;
							}
							precios_total += float.Parse((string) lector["preciodelproveedor"].ToString().Trim()) * float.Parse((string) lector["cantidadcomprada"].ToString().Trim());
							iva_total += calculo_iva;
							total_total += precio_mas_iva;
							// Configuramos el título de las columnas de la tabla
							cl01 = new PdfPCell(new Phrase(nro_de_partida.ToString().Trim(), _BoldFont));
							cl01.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER;
							cl01.HorizontalAlignment = 2;

							cl02 = new PdfPCell(new Phrase(float.Parse(lector["cantidadcomprada"].ToString().Trim()).ToString("F"), _NormalFont));
							cl02.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER;
							cl02.HorizontalAlignment = 2;

							cl03 = new PdfPCell(new Phrase(lector["tipounidadproducto"].ToString().Trim(), _NormalFont));
							cl03.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER;
							cl03.HorizontalAlignment = 0;

							cl04 = new PdfPCell(new Phrase(float.Parse(lector["cantidadembalaje"].ToString().Trim()).ToString("F"), _NormalFont));
							cl04.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER;
							cl04.HorizontalAlignment = 2;

							cl05 = new PdfPCell(new Phrase(lector["descripcionproducto"].ToString().Trim(), _NormalFont));
							cl05.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER;
							cl05.HorizontalAlignment = 0;

							cl06 = new PdfPCell(new Phrase(lector["idrequisicion"].ToString().Trim(), _NormalFont));
							cl06.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER;
							cl06.HorizontalAlignment = 2;		// centro

							cl07 = new PdfPCell(new Phrase(String.Format("{0:N}",float.Parse((string) lector["preciodelproveedor"].ToString().Trim())), _NormalFont));
							cl07.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER;
							cl07.HorizontalAlignment = 2;		// centro

							cl08 = new PdfPCell(new Phrase(String.Format("{0:N}",float.Parse((string) lector["preciodelproveedor"].ToString().Trim()) * float.Parse((string) lector["cantidadcomprada"].ToString().Trim())), _NormalFont));
							cl08.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER;
							cl08.HorizontalAlignment = 2;		// centro

							cl09 = new PdfPCell(new Phrase(String.Format("{0:N}",calculo_iva), _NormalFont));
							cl09.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER;
							cl09.HorizontalAlignment = 2;		// centro

							cl10 = new PdfPCell(new Phrase(String.Format("{0:N}",precio_mas_iva), _NormalFont));
							cl10.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
							cl10.HorizontalAlignment = 2;		// centro

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
						}
						// Configuramos el título de las columnas de la tabla
						cl01 = new PdfPCell(new Phrase(" ", _BoldFont));
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
						cl04.HorizontalAlignment = 0;

						cl05 = new PdfPCell(new Phrase(" ", _NormalFont));
						cl05.Border = iTextSharp.text.Rectangle.TOP_BORDER;
						cl05.HorizontalAlignment = 0;

						cl06 = new PdfPCell(new Phrase(" ", _NormalFont));
						cl06.Border = iTextSharp.text.Rectangle.TOP_BORDER;
						cl06.HorizontalAlignment = 2;		// centro

						cl07 = new PdfPCell(new Phrase("T O T A L", _BoldFont));
						cl07.Border = iTextSharp.text.Rectangle.TOP_BORDER;
						cl07.HorizontalAlignment = 2;		// centro

						cl08 = new PdfPCell(new Phrase(String.Format("{0:N}",precios_total), _NormalFont));
						cl08.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER;
						cl08.HorizontalAlignment = 2;		// centro

						cl09 = new PdfPCell(new Phrase(String.Format("{0:N}",iva_total), _NormalFont));
						cl09.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER;
						cl09.HorizontalAlignment = 2;		// centro

						cl10 = new PdfPCell(new Phrase(String.Format("{0:N}",total_total), _NormalFont));
						cl10.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER;
						cl10.HorizontalAlignment = 2;		// centro

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
						documento.Add(tblConceptos);

						documento.Add (new Paragraph (" ", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 5)));
						documento.Add (new Paragraph (" ", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 5)));
						documento.Add (new Paragraph ("                  ____________________________                     ____________________________", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 7)));
						documento.Add (new Paragraph ("                    Nombre y Firma Solicitante OC.                          Nombre y Firma Autorización OC.", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 7)));

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
						MessageType.Info,ButtonsType.Ok,"La ORDEN DE COMPRA NO esta disponible o se encuentra CANCELADA, verifique...");
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
			private string _nombreproveedor;
			private string _rfcproveedor;
			private string _direccproveedor;
			private string _telproveedor;
			private string _fechaordencompra;
			private string _numeroordencompra;
			private string _lugardeentrega;
			private string _fechaentrega;
			private string _tipoordencompra;
			private string _condiciondepago;
			private string _observaciones_oc;
			private string _facturarreceptor_a;
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
			public string nombreproveedor
			{
				get{
					return _nombreproveedor;
				}
				set{
					_nombreproveedor = value;
				}
			}
			public string rfcproveedor
			{
				get{
					return _rfcproveedor;
				}
				set{
					_rfcproveedor = value;
				}
			}
			public string direccproveedor
			{
				get{
					return _direccproveedor;
				}
				set{
					_direccproveedor = value;
				}
			}
			public string telproveedor
			{
				get{
					return _telproveedor;
				}
				set{
					_telproveedor = value;
				}
			}
			public string fechaordencompra
			{
				get{
					return _fechaordencompra;
				}
				set{
					_fechaordencompra = value;
				}
			}
			public string numeroordencompra
			{
				get{
					return _numeroordencompra;
				}
				set{
					_numeroordencompra = value;
				}
			}
			public string lugardeentrega
			{
				get{
					return _lugardeentrega;
				}
				set{
					_lugardeentrega = value;
				}
			}
			public string fechaentrega
			{
				get{
					return _fechaentrega;
				}
				set{
					_fechaentrega = value;
				}
			}
			public string tipoordencompra
			{
				get{
					return _tipoordencompra;
				}
				set{
					_tipoordencompra = value;
				}
			}
			public string condiciondepago
			{
				get{
					return _condiciondepago;
				}
				set{
					_condiciondepago = value;
				}
			}
			public string observaciones_oc
			{
				get{
					return _observaciones_oc;
				}
				set{
					_observaciones_oc = value;
				}
			}
			public string facturarreceptor_a
			{
				get{
					return _facturarreceptor_a;
				}
				set{
					_facturarreceptor_a = value;
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
				PdfPCell cl07 = new PdfPCell();
				PdfPCell cl08 = new PdfPCell();

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
				documento.Add (new Paragraph (" ", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 5)));

				// fuente para las tablas creadas
				iTextSharp.text.Font _NormalFont = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 7, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);
				iTextSharp.text.Font _BoldFont = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 7, iTextSharp.text.Font.BOLD, BaseColor.BLACK);

				PdfPTable tblordecompra1 = new PdfPTable(6);
				tblordecompra1.WidthPercentage = 100;
				float[] widths1 = new float[] { 41f, 180f, 30f, 40f, 30f, 30f };	// controlando el ancho de cada columna tienen que sumas 315 en total
				tblordecompra1.SetWidths(widths1);
				tblordecompra1.HorizontalAlignment = 0;

				// Configuramos el título de las columnas de la tabla
				cl01 = new PdfPCell(new Phrase("PROVEEDOR", _BoldFont));
				cl01.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER;
				//clnroatencion.BorderWidth = 1;			// Ancho del borde
				cl01.HorizontalAlignment = 1;
				cl02 = new PdfPCell(new Phrase(nombreproveedor, _NormalFont));
				cl02.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER;
				cl02.HorizontalAlignment = 0;
				cl03 = new PdfPCell(new Phrase("RFC", _BoldFont));
				cl03.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER;
				cl03.HorizontalAlignment = 1;
				cl04 = new PdfPCell(new Phrase(rfcproveedor, _NormalFont));
				cl04.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER;
				cl04.HorizontalAlignment = 0;
				cl05 = new PdfPCell(new Phrase("FECHA OC.", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 8, iTextSharp.text.Font.BOLD, BaseColor.BLACK)));
				cl05.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER;
				cl05.HorizontalAlignment = 1;
				cl06 = new PdfPCell(new Phrase("N° O.COMPRA", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 8, iTextSharp.text.Font.BOLD, BaseColor.BLACK)));
				cl06.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER;
				cl06.HorizontalAlignment = 1;

				// Añadimos las celdas a la tabla
				tblordecompra1.AddCell(cl01);
				tblordecompra1.AddCell(cl02);
				tblordecompra1.AddCell(cl03);
				tblordecompra1.AddCell(cl04);
				tblordecompra1.AddCell(cl05);
				tblordecompra1.AddCell(cl06);
				documento.Add(tblordecompra1);

				PdfPTable tblordecompra2 = new PdfPTable(6);
				tblordecompra2.WidthPercentage = 100;
				float[] widths2 = new float[] { 41f, 180f, 30f, 40f, 30f, 30f };	// controlando el ancho de cada columna tienen que sumas 315 en total
				tblordecompra2.SetWidths(widths2);
				tblordecompra2.HorizontalAlignment = 0;

				// Configuramos el título de las columnas de la tabla
				cl01 = new PdfPCell(new Phrase("DIRECCION", _BoldFont));
				cl01.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER;
				//clnroatencion.BorderWidth = 1;			// Ancho del borde
				cl01.HorizontalAlignment = 1;
				cl02 = new PdfPCell(new Phrase(direccproveedor, _NormalFont));
				cl02.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER;
				cl02.HorizontalAlignment = 0;
				cl03 = new PdfPCell(new Phrase("TELEFONO", _BoldFont));
				cl03.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER;
				cl03.HorizontalAlignment = 1;
				cl04 = new PdfPCell(new Phrase(telproveedor, _NormalFont));
				cl04.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER;
				cl04.HorizontalAlignment = 0;
				cl05 = new PdfPCell(new Phrase(fechaordencompra, new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 9, iTextSharp.text.Font.BOLD, BaseColor.BLACK)));
				cl05.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER;
				cl05.HorizontalAlignment = 1;
				cl06 = new PdfPCell(new Phrase(numeroordencompra, new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 9, iTextSharp.text.Font.BOLD, BaseColor.BLACK)));
				cl06.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER;
				cl06.HorizontalAlignment = 1;
				cl06.BackgroundColor = BaseColor.YELLOW;

				// Añadimos las celdas a la tabla
				tblordecompra2.AddCell(cl01);
				tblordecompra2.AddCell(cl02);
				tblordecompra2.AddCell(cl03);
				tblordecompra2.AddCell(cl04);
				tblordecompra2.AddCell(cl05);
				tblordecompra2.AddCell(cl06);
				documento.Add(tblordecompra2);

				PdfPTable tblordecompra3 = new PdfPTable(8);
				tblordecompra3.WidthPercentage = 100;
				float[] widths3 = new float[] { 40f, 60f, 40f, 40f, 40f, 30f, 40f, 50f };	// controlando el ancho de cada columna tienen que sumas 315 en total
				tblordecompra3.SetWidths(widths3);
				tblordecompra3.HorizontalAlignment = 0;

				// Configuramos el título de las columnas de la tabla
				cl01 = new PdfPCell(new Phrase("Lugar de Entrega", _BoldFont));
				cl01.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER;
				//clnroatencion.BorderWidth = 1;			// Ancho del borde
				cl01.HorizontalAlignment = 1;
				cl02 = new PdfPCell(new Phrase(lugardeentrega, _NormalFont));
				cl02.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER;
				cl02.HorizontalAlignment = 0;
				cl03 = new PdfPCell(new Phrase("Fecha de Entrega", _BoldFont));
				cl03.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER;
				cl03.HorizontalAlignment = 1;
				cl04 = new PdfPCell(new Phrase(fechaentrega, _NormalFont));
				cl04.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER;
				cl04.HorizontalAlignment = 0;
				cl05 = new PdfPCell(new Phrase("Tipo Orden Compra", _BoldFont));
				cl05.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER;
				cl05.HorizontalAlignment = 1;
				cl06 = new PdfPCell(new Phrase(tipoordencompra, _NormalFont));
				cl06.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER;
				cl06.HorizontalAlignment = 0;
				cl07 = new PdfPCell(new Phrase("Cond. Pago", _BoldFont));
				cl07.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER;
				cl07.HorizontalAlignment = 1;
				cl08 = new PdfPCell(new Phrase(condiciondepago, _NormalFont));
				cl08.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER;
				cl08.HorizontalAlignment = 0;

				// Añadimos las celdas a la tabla
				tblordecompra3.AddCell(cl01);
				tblordecompra3.AddCell(cl02);
				tblordecompra3.AddCell(cl03);
				tblordecompra3.AddCell(cl04);
				tblordecompra3.AddCell(cl05);
				tblordecompra3.AddCell(cl06);
				tblordecompra3.AddCell(cl07);
				tblordecompra3.AddCell(cl08);
				documento.Add(tblordecompra3);

				PdfPTable tblordecompra4 = new PdfPTable(2);
				tblordecompra4.WidthPercentage = 100;
				float[] widths4 = new float[] { 40f, 300f };	// controlando el ancho de cada columna tienen que sumas 315 en total
				tblordecompra4.SetWidths(widths4);
				tblordecompra4.HorizontalAlignment = 0;

				// Configuramos el título de las columnas de la tabla
				cl01 = new PdfPCell(new Phrase("Observaciones", _BoldFont));
				cl01.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER;
				//clnroatencion.BorderWidth = 1;			// Ancho del borde
				cl01.HorizontalAlignment = 1;
				cl02 = new PdfPCell(new Phrase(observaciones_oc, _NormalFont));
				cl02.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER;
				cl02.HorizontalAlignment = 0;

				// Añadimos las celdas a la tabla
				tblordecompra4.AddCell(cl01);
				tblordecompra4.AddCell(cl02);
				documento.Add(tblordecompra4);

				PdfPTable tblordecompra5 = new PdfPTable(2);
				tblordecompra5.WidthPercentage = 100;
				float[] widths5 = new float[] { 40f, 300f };	// controlando el ancho de cada columna tienen que sumas 315 en total
				tblordecompra5.SetWidths(widths5);
				tblordecompra5.HorizontalAlignment = 0;

				// Configuramos el título de las columnas de la tabla
				cl01 = new PdfPCell(new Phrase("Facturar A", _BoldFont));
				cl01.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.TOP_BORDER;
				//clnroatencion.BorderWidth = 1;			// Ancho del borde
				cl01.HorizontalAlignment = 1;
				cl02 = new PdfPCell(new Phrase(facturarreceptor_a, _NormalFont));
				cl02.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.TOP_BORDER;
				cl02.HorizontalAlignment = 0;

				// Añadimos las celdas a la tabla
				tblordecompra5.AddCell(cl01);
				tblordecompra5.AddCell(cl02);
				documento.Add(tblordecompra5);

				documento.Add (new Paragraph (" ", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 5)));

				documento.Add (new Paragraph ("CALIDAD: El comprador tendra el derecho de inspeccionar antes de aceptar la mercancia.", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 7)));
				documento.Add (new Paragraph ("PRECIO: El Proveedor facturará a precios y terminos de la Orden de Compra.", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 7)));
				documento.Add (new Paragraph ("ENTREGA: Si no se entega la mercancía dentro del plazo, el Comprador podrá cancelar el pedido o rehusarse a aceptar la mercancia.", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 7)));

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

		/*
		void ejecutar_consulta_reporte(PrintContext context)
		{
			int contador = 1;
			int contador_salto_pagina = 1;
			int numero_ordencompra = 0;
			string facturar_a = "";
			float precios_total = 0;
			float iva_total = 0;
			float total_total = 0;
			float calculo_iva = 0;
			float precio_mas_iva = 0;
			
			Cairo.Context cr = context.CairoContext;
			Pango.Layout layout = context.CreatePangoLayout ();
			Pango.FontDescription desc = Pango.FontDescription.FromString ("Sans");									
			// cr.Rotate(90)  Imprimir Orizontalmente rota la hoja cambian las posiciones de las lineas y columna					
			fontSize = 8.0;			layout = null;			layout = context.CreatePangoLayout ();
			desc.Size = (int)(fontSize * pangoScale);		layout.FontDescription = desc;
			
			
			NpgsqlConnection conexion;
			conexion = new NpgsqlConnection (connectionString+nombrebd);
        	// Verifica que la base de datos este conectada
			try{				
				conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand ();
				comando.CommandText = query_general;
				Console.WriteLine(comando.CommandText);
				NpgsqlDataReader lector = comando.ExecuteReader ();				
				if (lector.Read()){
					numero_ordencompra = (int) lector["numero_orden_compra"];
					facturar_a = classpublic.extract_spaces((string) lector["rfc"].ToString().Trim()+"-"+
								(string) lector["emisor"].ToString().Trim()+" "+
								(string) lector["calle"].ToString().Trim() +
								(string) lector["noexterior"].ToString().Trim()+" "+
								(string) lector["nointerior"].ToString().Trim()+", COL."+
								(string) lector["colonia"].ToString().Trim()+","+
								(string) lector["municipio"].ToString().Trim()+","+
								(string) lector["estado"].ToString().Trim()+","+
								(string) lector["codigopostal"].ToString().Trim());
					//Console.WriteLine((string) lector["descripcion_proveedor"].ToString().Trim());
					//Console.WriteLine((string) lector["direccion_proveedor"].ToString().Trim());
					//Console.WriteLine((string) lector["rfc_proveedor"].ToString().Trim());
					//Console.WriteLine((string) lector["telefonos_proveedor"].ToString().Trim());
					//Console.WriteLine((string) lector["lugar_de_entrega"].ToString().Trim());
					//Console.WriteLine((string) lector["dep_solicitante"].ToString().Trim());
					//Console.WriteLine((string) lector["observaciones"].ToString().Trim());
					//Console.WriteLine((string) lector["fecha_deorden_compra"].ToString().Trim());
					//Console.WriteLine((string) lector["numero_orden_compra"].ToString().Trim());
					//Console.WriteLine((string) lector["fecha_de_entrega"].ToString().Trim());
					//Console.WriteLine((string) lector["condiciones_de_pago"].ToString().Trim());
					imprime_encabezado(cr,layout,
						(string) lector["descripcion_proveedor"].ToString().Trim(),
						(string) lector["direccion_proveedor"].ToString().Trim(),
						(string) lector["rfc_proveedor"].ToString().Trim(),
						(string) lector["telefonos_proveedor"].ToString().Trim(),
						(string) lector["lugar_de_entrega"].ToString().Trim(),
					    (string) lector["condiciones_de_pago"].ToString().Trim(),
						(string) lector["dep_solicitante"].ToString().Trim(),
						(string) lector["observaciones"].ToString().Trim(),
						(string) lector["fechaordencompra"].ToString().Trim(),
						(string) lector["numero_orden_compra"].ToString().Trim(),
						(string) lector["fechadeentrega"].ToString().Trim(),
					    facturar_a,
					    (string) lector["tipo_orden_compra"].ToString().Trim());
					if((bool) lector["aplicar_iva"] == true){
						calculo_iva = ((float.Parse((string) lector["preciodelproveedor"].ToString().Trim()) * float.Parse((string) lector["cantidadcomprada"].ToString().Trim())) * float.Parse((string) lector["porcentage_iva"].ToString().Trim()))/100;
						precio_mas_iva = (float.Parse((string) lector["preciodelproveedor"].ToString().Trim()) * float.Parse((string) lector["cantidadcomprada"].ToString().Trim())) + calculo_iva;
					}else{
						calculo_iva = 0;
						precio_mas_iva = (float.Parse((string) lector["preciodelproveedor"].ToString().Trim()) * float.Parse((string) lector["cantidadcomprada"].ToString().Trim())) + calculo_iva;
					}
					precios_total = float.Parse((string) lector["preciodelproveedor"].ToString().Trim()) * float.Parse((string) lector["cantidadcomprada"].ToString().Trim());
					iva_total += calculo_iva;
					total_total += precio_mas_iva;					
					
					cr.MoveTo(07*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);			layout.SetText(contador.ToString().Trim());					Pango.CairoHelper.ShowLayout (cr, layout);
					cr.MoveTo(27*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);			layout.SetText((string) lector["cantidadcomprada"].ToString().Trim());					Pango.CairoHelper.ShowLayout (cr, layout);
					cr.MoveTo(60*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);			layout.SetText((string) lector["tipounidadproducto"].ToString().Trim());				Pango.CairoHelper.ShowLayout (cr, layout);
					cr.MoveTo(102*escala_en_linux_windows, comienzo_linea*escala_en_linux_windows);			layout.SetText((string) lector["cantidadembalaje"].ToString().Trim());					Pango.CairoHelper.ShowLayout (cr, layout);
					cr.MoveTo(140*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);			layout.SetText((string) lector["descripcionproducto"].ToString().Trim());					Pango.CairoHelper.ShowLayout (cr, layout);
					cr.MoveTo(485*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);			layout.SetText((string) lector["idrequisicion"].ToString().Trim());				Pango.CairoHelper.ShowLayout (cr, layout);
					
					desc = Pango.FontDescription.FromString ("Courier New");									
					// cr.Rotate(90)  Imprimir Orizontalmente rota la hoja cambian las posiciones de las lineas y columna					
					fontSize = 7.0;			layout = null;			layout = context.CreatePangoLayout ();
					desc.Size = (int)(fontSize * pangoScale);		layout.FontDescription = desc;
					// Precio del Producto
					cr.MoveTo(535*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);			layout.SetText(String.Format("${0,10:F}",float.Parse((string) lector["preciodelproveedor"].ToString().Trim())    ));					Pango.CairoHelper.ShowLayout (cr, layout);
					cr.MoveTo(590*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);			layout.SetText(String.Format("${0,10:F}",float.Parse((string) lector["preciodelproveedor"].ToString().Trim()) * float.Parse((string) lector["cantidadcomprada"].ToString().Trim())  ));					Pango.CairoHelper.ShowLayout (cr, layout);
					cr.MoveTo(645*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);			layout.SetText(String.Format("${0,10:F}",calculo_iva));				Pango.CairoHelper.ShowLayout (cr, layout);
					cr.MoveTo(700*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);			layout.SetText(String.Format("${0,10:F}",precio_mas_iva));			Pango.CairoHelper.ShowLayout (cr, layout);

					desc = Pango.FontDescription.FromString ("Sans");									
					// cr.Rotate(90)  Imprimir Orizontalmente rota la hoja cambian las posiciones de las lineas y columna					
					fontSize = 7.0;			layout = null;			layout = context.CreatePangoLayout ();
					desc.Size = (int)(fontSize * pangoScale);		layout.FontDescription = desc;
					
					comienzo_linea += separacion_linea;
					//Console.WriteLine((string) lector["descripcionproducto"].ToString().Trim());
					while(lector.Read()){
						//Console.WriteLine((string) lector["descripcionproducto"].ToString().Trim());
						if(numero_ordencompra != (int) lector["numero_orden_compra"]){
							numero_ordencompra = (int) lector["numero_orden_compra"];
							desc = Pango.FontDescription.FromString ("Courier New");									
							// cr.Rotate(90)  Imprimir Orizontalmente rota la hoja cambian las posiciones de las lineas y columna					
							fontSize = 7.0;			layout = null;			layout = context.CreatePangoLayout ();
							desc.Size = (int)(fontSize * pangoScale);		layout.FontDescription = desc;
							cr.MoveTo(590*escala_en_linux_windows,435*escala_en_linux_windows);			layout.SetText(String.Format("${0,10:F}",precios_total));		Pango.CairoHelper.ShowLayout (cr, layout);
							cr.MoveTo(645*escala_en_linux_windows,435*escala_en_linux_windows);			layout.SetText(String.Format("${0,10:F}",iva_total));			Pango.CairoHelper.ShowLayout (cr, layout);
							cr.MoveTo(700*escala_en_linux_windows,435*escala_en_linux_windows);			layout.SetText(String.Format("${0,10:F}",total_total));			Pango.CairoHelper.ShowLayout (cr, layout);
							desc = Pango.FontDescription.FromString ("Sans");									
							// cr.Rotate(90)  Imprimir Orizontalmente rota la hoja cambian las posiciones de las lineas y columna					
							fontSize = 7.0;			layout = null;			layout = context.CreatePangoLayout ();
							desc.Size = (int)(fontSize * pangoScale);		layout.FontDescription = desc;
														
							precios_total = 0;
							iva_total = 0;
							total_total = 0;
							
							if((bool) lector["aplicar_iva"] == true){
								calculo_iva = ((float.Parse((string) lector["preciodelproveedor"].ToString().Trim()) * float.Parse((string) lector["cantidadcomprada"].ToString().Trim())) * float.Parse((string) lector["porcentage_iva"].ToString().Trim()))/100;
								precio_mas_iva = (float.Parse((string) lector["preciodelproveedor"].ToString().Trim()) * float.Parse((string) lector["cantidadcomprada"].ToString().Trim())) + calculo_iva;
							}else{
								calculo_iva = 0;
								precio_mas_iva = (float.Parse((string) lector["preciodelproveedor"].ToString().Trim()) * float.Parse((string) lector["cantidadcomprada"].ToString().Trim())) + calculo_iva;
							}
							precios_total = float.Parse((string) lector["preciodelproveedor"].ToString().Trim()) * float.Parse((string) lector["cantidadcomprada"].ToString().Trim());
							iva_total += calculo_iva;
							total_total += precio_mas_iva;
							
							facturar_a = classpublic.extract_spaces((string) lector["rfc"].ToString().Trim()+"-"+
								(string) lector["emisor"].ToString().Trim()+" "+
								(string) lector["calle"].ToString().Trim() +
								(string) lector["noexterior"].ToString().Trim()+" "+
								(string) lector["nointerior"].ToString().Trim()+" "+
								(string) lector["colonia"].ToString().Trim()+","+
								(string) lector["municipio"].ToString().Trim()+","+
								(string) lector["estado"].ToString().Trim()+","+
								(string) lector["codigopostal"].ToString().Trim());
							comienzo_linea = 162;
							contador = 1;
							contador_salto_pagina = 1;
							
							cr.ShowPage();
							imprime_encabezado(cr,layout,
									(string) lector["descripcion_proveedor"].ToString().Trim(),
									(string) lector["direccion_proveedor"].ToString().Trim(),
									(string) lector["rfc_proveedor"].ToString().Trim(),
									(string) lector["telefonos_proveedor"].ToString().Trim(),
									(string) lector["lugar_de_entrega"].ToString().Trim(),
								    (string) lector["condiciones_de_pago"].ToString().Trim(),
									(string) lector["dep_solicitante"].ToString().Trim(),
									(string) lector["observaciones"].ToString().Trim(),
									(string) lector["fechaordencompra"].ToString().Trim(),
									(string) lector["numero_orden_compra"].ToString().Trim(),
									(string) lector["fechadeentrega"].ToString().Trim(),
									facturar_a,
							        (string) lector["tipo_orden_compra"].ToString().Trim());
									cr.MoveTo(07*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);			layout.SetText(contador.ToString().Trim());					Pango.CairoHelper.ShowLayout (cr, layout);
									cr.MoveTo(27*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);			layout.SetText((string) lector["cantidadcomprada"].ToString().Trim());					Pango.CairoHelper.ShowLayout (cr, layout);
									cr.MoveTo(60*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);			layout.SetText((string) lector["tipounidadproducto"].ToString().Trim());				Pango.CairoHelper.ShowLayout (cr, layout);
									cr.MoveTo(102*escala_en_linux_windows, comienzo_linea*escala_en_linux_windows);			layout.SetText((string) lector["cantidadembalaje"].ToString().Trim());					Pango.CairoHelper.ShowLayout (cr, layout);
									cr.MoveTo(140*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);			layout.SetText((string) lector["descripcionproducto"].ToString().Trim());					Pango.CairoHelper.ShowLayout (cr, layout);
									cr.MoveTo(485*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);			layout.SetText((string) lector["idrequisicion"].ToString().Trim());				Pango.CairoHelper.ShowLayout (cr, layout);
									desc = Pango.FontDescription.FromString ("Courier New");									
									// cr.Rotate(90)  Imprimir Orizontalmente rota la hoja cambian las posiciones de las lineas y columna					
									fontSize = 7.0;			layout = null;			layout = context.CreatePangoLayout ();
									desc.Size = (int)(fontSize * pangoScale);		layout.FontDescription = desc;
									// Precio del Producto
									cr.MoveTo(535*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);			layout.SetText(String.Format("${0,10:F}",float.Parse((string) lector["preciodelproveedor"].ToString().Trim())    ));					Pango.CairoHelper.ShowLayout (cr, layout);
									cr.MoveTo(590*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);			layout.SetText(String.Format("${0,10:F}",float.Parse((string) lector["preciodelproveedor"].ToString().Trim()) * float.Parse((string) lector["cantidadcomprada"].ToString().Trim())  ));					Pango.CairoHelper.ShowLayout (cr, layout);
									cr.MoveTo(645*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);			layout.SetText(String.Format("${0,10:F}",calculo_iva));				Pango.CairoHelper.ShowLayout (cr, layout);
									cr.MoveTo(700*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);			layout.SetText(String.Format("${0,10:F}",precio_mas_iva));			Pango.CairoHelper.ShowLayout (cr, layout);

									desc = Pango.FontDescription.FromString ("Sans");									
									// cr.Rotate(90)  Imprimir Orizontalmente rota la hoja cambian las posiciones de las lineas y columna					
									fontSize = 7.0;			layout = null;			layout = context.CreatePangoLayout ();
									desc.Size = (int)(fontSize * pangoScale);		layout.FontDescription = desc;
														
									comienzo_linea += separacion_linea;
						}else{
							contador += 1;
							contador_salto_pagina += 1;
							if(contador_salto_pagina == 28){
								comienzo_linea = 162;
								contador_salto_pagina = 1;
								cr.ShowPage();
								imprime_encabezado(cr,layout,
									(string) lector["descripcion_proveedor"].ToString().Trim(),
									(string) lector["direccion_proveedor"].ToString().Trim(),
									(string) lector["rfc_proveedor"].ToString().Trim(),
									(string) lector["telefonos_proveedor"].ToString().Trim(),
									(string) lector["lugar_de_entrega"].ToString().Trim(),
									(string) lector["condiciones_de_pago"].ToString().Trim(),
									(string) lector["dep_solicitante"].ToString().Trim(),
									(string) lector["observaciones"].ToString().Trim(),
									(string) lector["fechaordencompra"].ToString().Trim(),
									(string) lector["numero_orden_compra"].ToString().Trim(),
									(string) lector["fechadeentrega"].ToString().Trim(),
									facturar_a,
									(string) lector["tipo_orden_compra"].ToString().Trim());
							}
							if((bool) lector["aplicar_iva"] == true){
								calculo_iva = ((float.Parse((string) lector["preciodelproveedor"].ToString().Trim()) * float.Parse((string) lector["cantidadcomprada"].ToString().Trim())) * float.Parse((string) lector["porcentage_iva"].ToString().Trim()))/100;
								precio_mas_iva = (float.Parse((string) lector["preciodelproveedor"].ToString().Trim()) * float.Parse((string) lector["cantidadcomprada"].ToString().Trim())) + calculo_iva;
							}else{
								calculo_iva = 0;
								precio_mas_iva = (float.Parse((string) lector["preciodelproveedor"].ToString().Trim()) * float.Parse((string) lector["cantidadcomprada"].ToString().Trim())) + calculo_iva;
							}
							precios_total += float.Parse((string) lector["preciodelproveedor"].ToString().Trim()) * float.Parse((string) lector["cantidadcomprada"].ToString().Trim());
							iva_total += calculo_iva;
							total_total += precio_mas_iva;
							cr.MoveTo(07*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);			layout.SetText(contador.ToString().Trim());					Pango.CairoHelper.ShowLayout (cr, layout);
							cr.MoveTo(27*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);			layout.SetText((string) lector["cantidadcomprada"].ToString().Trim());					Pango.CairoHelper.ShowLayout (cr, layout);
							cr.MoveTo(60*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);			layout.SetText((string) lector["tipounidadproducto"].ToString().Trim());				Pango.CairoHelper.ShowLayout (cr, layout);
							cr.MoveTo(102*escala_en_linux_windows, comienzo_linea*escala_en_linux_windows);			layout.SetText((string) lector["cantidadembalaje"].ToString().Trim());					Pango.CairoHelper.ShowLayout (cr, layout);
							cr.MoveTo(140*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);			layout.SetText((string) lector["descripcionproducto"].ToString().Trim());				Pango.CairoHelper.ShowLayout (cr, layout);
							cr.MoveTo(485*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);			layout.SetText((string) lector["idrequisicion"].ToString().Trim());				Pango.CairoHelper.ShowLayout (cr, layout);
							desc = Pango.FontDescription.FromString ("Courier New");									
							// cr.Rotate(90)  Imprimir Orizontalmente rota la hoja cambian las posiciones de las lineas y columna					
							fontSize = 7.0;			layout = null;			layout = context.CreatePangoLayout ();
							desc.Size = (int)(fontSize * pangoScale);		layout.FontDescription = desc;
							// Precio del Producto
							cr.MoveTo(535*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);			layout.SetText(String.Format("${0,10:F}",float.Parse((string) lector["preciodelproveedor"].ToString().Trim())    ));					Pango.CairoHelper.ShowLayout (cr, layout);
							cr.MoveTo(590*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);			layout.SetText(String.Format("${0,10:F}",float.Parse((string) lector["preciodelproveedor"].ToString().Trim()) * float.Parse((string) lector["cantidadcomprada"].ToString().Trim())));					Pango.CairoHelper.ShowLayout (cr, layout);
							cr.MoveTo(645*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);			layout.SetText(String.Format("${0,10:F}",calculo_iva));				Pango.CairoHelper.ShowLayout (cr, layout);
							cr.MoveTo(700*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);			layout.SetText(String.Format("${0,10:F}",precio_mas_iva));			Pango.CairoHelper.ShowLayout (cr, layout);

							desc = Pango.FontDescription.FromString ("Sans");									
							// cr.Rotate(90)  Imprimir Orizontalmente rota la hoja cambian las posiciones de las lineas y columna					
							fontSize = 7.0;			layout = null;			layout = context.CreatePangoLayout ();
							desc.Size = (int)(fontSize * pangoScale);		layout.FontDescription = desc;																				
							comienzo_linea += separacion_linea;

						}
					}
					desc = Pango.FontDescription.FromString ("Courier New");									
					// cr.Rotate(90)  Imprimir Orizontalmente rota la hoja cambian las posiciones de las lineas y columna					
					fontSize = 7.0;			layout = null;			layout = context.CreatePangoLayout ();
					desc.Size = (int)(fontSize * pangoScale);		layout.FontDescription = desc;
					cr.MoveTo(590*escala_en_linux_windows,435*escala_en_linux_windows);			layout.SetText(String.Format("${0,10:F}",precios_total));		Pango.CairoHelper.ShowLayout (cr, layout);
					cr.MoveTo(645*escala_en_linux_windows,435*escala_en_linux_windows);			layout.SetText(String.Format("${0,10:F}",iva_total));			Pango.CairoHelper.ShowLayout (cr, layout);
					cr.MoveTo(700*escala_en_linux_windows,435*escala_en_linux_windows);			layout.SetText(String.Format("${0,10:F}",total_total));			Pango.CairoHelper.ShowLayout (cr, layout);
					desc = Pango.FontDescription.FromString ("Sans");									
					// cr.Rotate(90)  Imprimir Orizontalmente rota la hoja cambian las posiciones de las lineas y columna					
					fontSize = 7.0;			layout = null;			layout = context.CreatePangoLayout ();
					desc.Size = (int)(fontSize * pangoScale);		layout.FontDescription = desc;
				}
			}catch(NpgsqlException ex){
			
				Console.WriteLine("PostgresSQL error: {0}",ex.Message);
				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
								MessageType.Error,ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
				msgBoxError.Run ();		msgBoxError.Destroy();
			}
		}
					
		void imprime_encabezado(Cairo.Context cr,Pango.Layout layout,
			                    string descripcion_proveedor_,
					           	string direccion_proveedor_,
						        string rfc_proveedor_,
					        	string telefonos_proveedor_,
				         		string lugar_de_entrega_,
				        	    string condiciones_de_pago_,
					            string dep_solicitante_,
					        	string observaciones_,
					        	string fecha_deorden_compra_,
					        	string numero_orden_compra_,
					        	string fecha_de_entrega_,
		                        string facturar_a_,string tipoordencompra)
			
		{
			Pango.FontDescription desc = Pango.FontDescription.FromString ("Sans");								
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
			// Cambiando el tamaño de la fuente			
			fontSize = 11.0;
			desc.Size = (int)(fontSize * pangoScale);					layout.FontDescription = desc;
			layout.FontDescription.Weight = Weight.Bold;
			cr.MoveTo(290*escala_en_linux_windows, 25*escala_en_linux_windows);			layout.SetText("ORDEN_DE_COMPRAS");				Pango.CairoHelper.ShowLayout (cr, layout);
			
			fontSize = 9.0;
			desc.Size = (int)(fontSize * pangoScale);					layout.FontDescription = desc;
			cr.MoveTo(655*escala_en_linux_windows, 62*escala_en_linux_windows);		layout.SetText("N° O.COMPRA");					Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(660*escala_en_linux_windows, 72*escala_en_linux_windows);		layout.SetText(numero_orden_compra_);				Pango.CairoHelper.ShowLayout (cr, layout);
						
			fontSize = 7.0;
			desc.Size = (int)(fontSize * pangoScale);					layout.FontDescription = desc;
			layout.FontDescription.Weight = Weight.Bold;
			
			cr.MoveTo(07*escala_en_linux_windows,62*escala_en_linux_windows);			layout.SetText("PROVEEDOR");	Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(07*escala_en_linux_windows,75*escala_en_linux_windows);			layout.SetText("DIRECCION");	Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(07*escala_en_linux_windows,88*escala_en_linux_windows);			layout.SetText("R.F.C.");	Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(250*escala_en_linux_windows,88*escala_en_linux_windows);			layout.SetText("TELEFONOS");	Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(380*escala_en_linux_windows,88*escala_en_linux_windows);			layout.SetText("em@il");	Pango.CairoHelper.ShowLayout (cr, layout);
					
			cr.MoveTo(555*escala_en_linux_windows,60*escala_en_linux_windows);			layout.SetText("Fecha Orden Compra");	Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(07*escala_en_linux_windows,100*escala_en_linux_windows);			layout.SetText("Lugar de Entrega");	Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(410*escala_en_linux_windows,100*escala_en_linux_windows);			layout.SetText("Fecha de Entrega ");	Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(07*escala_en_linux_windows,130*escala_en_linux_windows);			layout.SetText("Facturar A:");	Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(595*escala_en_linux_windows,100*escala_en_linux_windows);			layout.SetText("Tipo Orden de Compra");	Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(07*escala_en_linux_windows,110*escala_en_linux_windows);			layout.SetText("Observaciones");	Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(615*escala_en_linux_windows,120*escala_en_linux_windows);			layout.SetText("Condicion de Pago");	Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(540*escala_en_linux_windows,435*escala_en_linux_windows);			layout.SetText("TOTALES");	Pango.CairoHelper.ShowLayout (cr, layout);
			fontSize = 7.0;
			desc.Size = (int)(fontSize * pangoScale);					layout.FontDescription = desc;
			layout.FontDescription.Weight = Weight.Normal;
			cr.MoveTo(610*escala_en_linux_windows,110*escala_en_linux_windows);			layout.SetText(tipoordencompra);	Pango.CairoHelper.ShowLayout (cr, layout);
			
			cr.MoveTo(65*escala_en_linux_windows,62*escala_en_linux_windows);			layout.SetText(descripcion_proveedor_);	Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(65*escala_en_linux_windows,75*escala_en_linux_windows);			layout.SetText(direccion_proveedor_);	Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(65*escala_en_linux_windows,88*escala_en_linux_windows);			layout.SetText(rfc_proveedor_);	Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(300*escala_en_linux_windows,88*escala_en_linux_windows);			layout.SetText(telefonos_proveedor_);	Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(495*escala_en_linux_windows,100*escala_en_linux_windows);			layout.SetText(fecha_de_entrega_);	Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(565*escala_en_linux_windows,75*escala_en_linux_windows);			layout.SetText(fecha_deorden_compra_);	Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(80*escala_en_linux_windows,100*escala_en_linux_windows);			layout.SetText(lugar_de_entrega_);	Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(07*escala_en_linux_windows,120*escala_en_linux_windows);			layout.SetText(observaciones_);	Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(60*escala_en_linux_windows,130*escala_en_linux_windows);			layout.SetText(facturar_a_);	Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(555*escala_en_linux_windows,130*escala_en_linux_windows);			layout.SetText(condiciones_de_pago_);	Pango.CairoHelper.ShowLayout (cr, layout);
			
			cr.MoveTo(07*escala_en_linux_windows, 142*escala_en_linux_windows);			layout.SetText("N°");							Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(07*escala_en_linux_windows, 152*escala_en_linux_windows);			layout.SetText("Part.");						Pango.CairoHelper.ShowLayout (cr, layout);
			//cr.MoveTo(07*escala_en_linux_windows, 162*escala_en_linux_windows);			layout.SetText("100");					Pango.CairoHelper.ShowLayout (cr, layout);
			
			cr.MoveTo(27*escala_en_linux_windows, 142*escala_en_linux_windows);			layout.SetText("Cantid.");					Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(27*escala_en_linux_windows, 152*escala_en_linux_windows);			layout.SetText(" Soli.");					Pango.CairoHelper.ShowLayout (cr, layout);
			//cr.MoveTo(27*escala_en_linux_windows, 162*escala_en_linux_windows);			layout.SetText("1000.00");					Pango.CairoHelper.ShowLayout (cr, layout);
			
			cr.MoveTo(60*escala_en_linux_windows, 142*escala_en_linux_windows);			layout.SetText("Unidad de");					Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(60*escala_en_linux_windows, 152*escala_en_linux_windows);			layout.SetText("Medida");					Pango.CairoHelper.ShowLayout (cr, layout);
			//cr.MoveTo(60*escala_en_linux_windows, 162*escala_en_linux_windows);			layout.SetText("PIEZA");					Pango.CairoHelper.ShowLayout (cr, layout);
			
			cr.MoveTo(102*escala_en_linux_windows, 142*escala_en_linux_windows);			layout.SetText("Empaque");					Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(102*escala_en_linux_windows, 152*escala_en_linux_windows);			layout.SetText("Produc.");					Pango.CairoHelper.ShowLayout (cr, layout);
			//cr.MoveTo(102*escala_en_linux_windows, 162*escala_en_linux_windows);			layout.SetText("1000.00");					Pango.CairoHelper.ShowLayout (cr, layout);
			
			cr.MoveTo(140*escala_en_linux_windows, 142*escala_en_linux_windows);			layout.SetText("Descripcion del");					Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(140*escala_en_linux_windows, 152*escala_en_linux_windows);			layout.SetText("Producto");					Pango.CairoHelper.ShowLayout (cr, layout);
			//cr.MoveTo(140*escala_en_linux_windows, 162*escala_en_linux_windows);			layout.SetText("BOLSA RECOLECTORA DE ORINA UROTEK DE 2 LTS.");					Pango.CairoHelper.ShowLayout (cr, layout);
			
			cr.MoveTo(490*escala_en_linux_windows, 152*escala_en_linux_windows);			layout.SetText("# REQ.");					Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(545*escala_en_linux_windows, 152*escala_en_linux_windows);			layout.SetText("PRECIO");					Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(595*escala_en_linux_windows, 152*escala_en_linux_windows);			layout.SetText("SUB-TOTAL");					Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(660*escala_en_linux_windows, 152*escala_en_linux_windows);			layout.SetText("IVA");					Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(710*escala_en_linux_windows, 152*escala_en_linux_windows);			layout.SetText("TOTAL");					Pango.CairoHelper.ShowLayout (cr, layout);
			//cr.MoveTo(600*escala_en_linux_windows, 152*escala_en_linux_windows);			layout.SetText("1000.00");					Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(05*escala_en_linux_windows, 435*escala_en_linux_windows);			layout.SetText("CALIDAD: El comprador tendra el derecho de inspeccionar antes de aceptar la mercancia.");					Pango.CairoHelper.ShowLayout (cr, layout);	
			cr.MoveTo(05*escala_en_linux_windows, 445*escala_en_linux_windows);			layout.SetText("PRECIO: El Proveedor facturará a precios y terminos de la Orden de Compra.");					Pango.CairoHelper.ShowLayout (cr, layout);	
			cr.MoveTo(05*escala_en_linux_windows, 455*escala_en_linux_windows);			layout.SetText("ENTREGA: Si no se entega la mercancía dentro del plazo, el Comprador podrá cancelar el pedido o rehusarse a aceptar la mercancia.");					Pango.CairoHelper.ShowLayout (cr, layout);	
			
			cr.MoveTo(55*escala_en_linux_windows, 545*escala_en_linux_windows);			layout.SetText("Firma Solicitante");		Pango.CairoHelper.ShowLayout (cr, layout);	
			cr.MoveTo(350*escala_en_linux_windows, 545*escala_en_linux_windows);		layout.SetText("Firma Autorización");		Pango.CairoHelper.ShowLayout (cr, layout);	
			
			cr.MoveTo(05*escala_en_linux_windows, 60*escala_en_linux_windows);
			cr.LineTo(05,430);		// vertical 1
			
			cr.MoveTo(750*escala_en_linux_windows, 60*escala_en_linux_windows);
			cr.LineTo(750,450);		// vertical 2
			
			cr.MoveTo(550*escala_en_linux_windows, 60*escala_en_linux_windows);
			cr.LineTo(550,140);		// vertical 3
			
			cr.MoveTo(650*escala_en_linux_windows, 60*escala_en_linux_windows);
			cr.LineTo(650,100);		// vertical 4
			
			cr.MoveTo(25*escala_en_linux_windows, 140*escala_en_linux_windows);
			cr.LineTo(25,430);		// vertical 5
			
			cr.MoveTo(57*escala_en_linux_windows, 140*escala_en_linux_windows);
			cr.LineTo(57,430);		// vertical 6
			
			cr.MoveTo(100*escala_en_linux_windows, 140*escala_en_linux_windows);
			cr.LineTo(100,430);		// vertical 7
			
			cr.MoveTo(138*escala_en_linux_windows, 140*escala_en_linux_windows);
			cr.LineTo(138,430);		// vertical 8
			
			cr.MoveTo(480*escala_en_linux_windows, 140*escala_en_linux_windows);
			cr.LineTo(480,430);		// vertical 10
			
			cr.MoveTo(530*escala_en_linux_windows, 140*escala_en_linux_windows);
			cr.LineTo(530,430);		// vertical 10			
			
			cr.MoveTo(585*escala_en_linux_windows, 140*escala_en_linux_windows);
			cr.LineTo(585,450);		// vertical 11
			
			cr.MoveTo(640*escala_en_linux_windows, 140*escala_en_linux_windows);
			cr.LineTo(640,450);		// vertical 12
			
			cr.MoveTo(695*escala_en_linux_windows, 140*escala_en_linux_windows);
			cr.LineTo(695,450);
			
			// Linea recuadro de inicio
			cr.MoveTo(750*escala_en_linux_windows, 60*escala_en_linux_windows);
			cr.LineTo(05,60);		// Linea Horizontal 1
			// Linea divide el lugar de entrega observaciones
			cr.MoveTo(750*escala_en_linux_windows, 100*escala_en_linux_windows);
			cr.LineTo(05,100);		
			// divide tipo de OC. y condiciones de pago
			cr.MoveTo(750*escala_en_linux_windows, 120*escala_en_linux_windows);
			cr.LineTo(550,120);
			// linea inicio de titulos de los conceptos
			cr.MoveTo(750*escala_en_linux_windows, 140*escala_en_linux_windows);
			cr.LineTo(05,140);
			// linea inicio de conceptos
			cr.MoveTo(750*escala_en_linux_windows, 160*escala_en_linux_windows);
			cr.LineTo(05,160);
			// linea final de los conceptos de la orden de compra
			cr.MoveTo(05*escala_en_linux_windows, 430*escala_en_linux_windows);
			cr.LineTo(750,430);
			// linea final de los totales
			cr.MoveTo(750*escala_en_linux_windows, 450*escala_en_linux_windows);
			cr.LineTo(585,450);			
			// Lineas de la Firmas
			cr.MoveTo(35*escala_en_linux_windows, 545*escala_en_linux_windows);	
			cr.LineTo(145,545);			
			cr.MoveTo(320*escala_en_linux_windows, 545*escala_en_linux_windows);	
			cr.LineTo(440,545);
						
			cr.FillExtents();  //. FillPreserve(); 
			cr.SetSourceRGB (0, 0, 0);
			cr.LineWidth = 0.3;
			cr.Stroke();
		}
		*/
	}	
}