//  
//  proveedores.cs
//  
//  Author:
//       Daniel Olivares <arcangeldoc@openmailbox.org>
// 
//  Copyright (c) 2012 dolivares
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
using GtkSharp;

// libreria creada con el proyecto AODL 1.4 .ods
using AODL;
using AODL.Document.SpreadsheetDocuments;
using AODL.Document.Content;
using AODL.Document.Content.Tables;
using AODL.Document;
using AODL.Package;
using AODL.Document.Collections;

namespace osiris
{
	public class proveedores_remisiones
	{
		// Boton general para salir de las ventanas
		// Todas la ventanas en glade este boton debe estra declarado identico
		[Widget] Gtk.Button button_salir = null;
		
		// Declarando ventana del menu de costos
		[Widget] Gtk.Window proveedores_remi = null;
		[Widget] Gtk.TreeView treeview_lista_facprove = null;
		[Widget] Gtk.Button button_consulta_cont = null;
		[Widget] Gtk.Button button_busca_proveedores = null;
		[Widget] Gtk.Entry entry_dia_inicial = null;
		[Widget] Gtk.Entry entry_mes_inicial = null;
		[Widget] Gtk.Entry entry_ano_inicial = null;
		[Widget] Gtk.Entry entry_dia_final = null;
		[Widget] Gtk.Entry entry_mes_final = null;
		[Widget] Gtk.Entry entry_ano_final = null;
		[Widget] Gtk.Entry entry_id_proveedor = null;
		[Widget] Gtk.Entry entry_nombre_proveedor = null;
		[Widget] Gtk.Button button_reporte = null;
		[Widget] Gtk.RadioButton radiobutton_x_valid_conta = null;
		[Widget] Gtk.RadioButton radiobutton_valid_conta = null;
		[Widget] Gtk.Entry entry_subtotal_cont = null;
		[Widget] Gtk.Entry entry_iva_cont = null;
		[Widget] Gtk.Entry entry_total_cont = null;
		[Widget] Gtk.Entry entry_totalnro_facturas = null;
		[Widget] Gtk.CheckButton checkbutton_filtro_provee = null;
		[Widget] Gtk.Button button_export_sheet = null;
		
		string connectionString;
		string nombrebd;
		string LoginEmpleado;
    	string NomEmpleado;
    	string AppEmpleado;
    	string ApmEmpleado;
		
		string query_proveedores = "";
		string query_consulta = "";
		string query_rango_fechas = "AND to_char(osiris_erp_factura_compra_enca.fechahora_creacion,'yyyy') >= '"+DateTime.Now.ToString("yyyy")+"' "+
									"AND to_char(osiris_erp_factura_compra_enca.fechahora_creacion,'yyyy') <= '"+DateTime.Now.ToString("yyyy")+"' " +
									"AND to_char(osiris_erp_factura_compra_enca.fechahora_creacion,'MM') >= '"+DateTime.Now.ToString("MM")+"' "+
									"AND to_char(osiris_erp_factura_compra_enca.fechahora_creacion,'MM') <= '"+DateTime.Now.ToString("MM")+"' "+
									"AND to_char(osiris_erp_factura_compra_enca.fechahora_creacion,'dd') >= '"+DateTime.Now.ToString("dd")+"' " +
									"AND to_char(osiris_erp_factura_compra_enca.fechahora_creacion,'dd') <= '"+DateTime.Now.ToString("dd")+"' ";
				
		TreeStore treeViewEngineFacProv;
		
		TreeViewColumn col_00;	CellRendererToggle cellrt_00;
		TreeViewColumn col_01;	CellRendererText cellrt_01;
		TreeViewColumn col_02;	CellRendererText cellrt_02;
		TreeViewColumn col_03;	CellRendererText cellrt_03;
		TreeViewColumn col_04;	CellRendererText cellrt_04;
		TreeViewColumn col_05;	CellRendererText cellrt_05;
		TreeViewColumn col_06;	CellRendererText cellrt_06;
		TreeViewColumn col_07;	CellRendererText cellrt_07;
		TreeViewColumn col_08;	CellRendererText cellrt_08;
		TreeViewColumn col_09;	CellRendererText cellrt_09;
		TreeViewColumn col_10;	CellRendererText cellrt_10;
		
		//Declaracion de ventana de error
		protected Gtk.Window MyWinError;
		
		class_conexion conexion_a_DB = new class_conexion();
		class_public classpublic = new class_public();
		class_buscador classfind_data = new class_buscador();
		
		public proveedores_remisiones (string LoginEmp_, string NomEmpleado_, string AppEmpleado_, string ApmEmpleado_, string nombrebd_)
		{
			LoginEmpleado = LoginEmp_;
    		NomEmpleado = NomEmpleado_;
    		AppEmpleado = AppEmpleado_;
    		ApmEmpleado = ApmEmpleado_;
    		connectionString = conexion_a_DB._url_servidor+conexion_a_DB._port_DB+conexion_a_DB._usuario_DB+conexion_a_DB._passwrd_user_DB;
			nombrebd = conexion_a_DB._nombrebd;
			
			Glade.XML gxml = new Glade.XML (null, "almacen_costos_compras.glade", "proveedores_remi", null);
			gxml.Autoconnect (this);
			
			entry_dia_inicial.Text = DateTime.Now.ToString("dd");
			entry_mes_inicial.Text = DateTime.Now.ToString("MM");
			entry_ano_inicial.Text = DateTime.Now.ToString("yyyy");			
			entry_dia_final.Text = DateTime.Now.ToString("dd");
			entry_mes_final.Text = DateTime.Now.ToString("MM");
			entry_ano_final.Text = DateTime.Now.ToString("yyyy");
			
			button_consulta_cont.Clicked += new EventHandler(on_button_consulta_cont_clicked);
			button_busca_proveedores.Clicked += new EventHandler(on_button_busca_proveedores_clicked);
			button_reporte.Clicked += new EventHandler(on_button_reporte_clicked);
			button_export_sheet.Clicked += new EventHandler(on_button_export_sheet_clicked);
						
			// Sale de la ventana
			button_salir.Clicked += new EventHandler(on_cierraventanas_clicked);
			
			crea_treeview_facproveedes();			
		}
		
		void on_button_consulta_cont_clicked(object sender, EventArgs args)
		{
			llenado_treeview_contabilidad();
		}
		
		void llenado_treeview_contabilidad()
		{
			treeViewEngineFacProv.Clear();
			string numeros_requis = "";
			float subtotal_factprov = 0;
			float ivatotal_factprov = 0;
			float totatota_factprov = 0;
			int totalnro_facturas = 0;
			if((bool) checkbutton_filtro_provee.Active == true && entry_id_proveedor.Text.Trim() != ""){
				query_proveedores = "AND osiris_erp_factura_compra_enca.id_proveedor = '"+entry_id_proveedor.Text.Trim()+"' ";
			}else{
				query_proveedores = "";
			}						
			query_rango_fechas ="AND to_char(osiris_erp_factura_compra_enca.fechahora_creacion,'yyyy-MM-dd') >= '"+(string) entry_ano_inicial.Text.ToString()+"-"+(string) entry_mes_inicial.Text.ToString()+"-"+(string) entry_dia_inicial.Text.ToString()+"' "+
									 "AND to_char(osiris_erp_factura_compra_enca.fechahora_creacion,'yyyy-MM-dd') <= '"+(string) entry_ano_final.Text.ToString()+"-"+(string) entry_mes_final.Text.ToString()+"-"+(string) entry_dia_final.Text.ToString()+"' ";
									
			query_consulta = "SELECT to_char(fechahora_creacion,'yyyy-MM-dd') AS fechacaptura,to_char(osiris_erp_factura_compra_enca.fecha_factura,'yyyy-MM-dd') AS fechafactura," +
							"to_char(osiris_erp_factura_compra_enca.fecha_entrada_almacen,'yyyy-MM-dd') AS fechaentrada," +
							"osiris_erp_factura_compra_enca.numero_factura_proveedor AS numerofactura," +
							"osiris_erp_factura_compra_enca.numero_orden_compra AS ordencompra," +
							"subtotal_factura,iva_factura,total_factura,osiris_erp_factura_compra_enca.id_emisor AS idreceptor," +
							"osiris_erp_factura_compra_enca.id_proveedor,descripcion_proveedor," +
							"osiris_erp_emisor.emisor AS receptorfactura,osiris_erp_factura_compra_enca.numero_orden_compra,uuid " +
							"FROM osiris_erp_factura_compra_enca,osiris_erp_proveedores,osiris_erp_emisor " +
							"WHERE osiris_erp_factura_compra_enca.id_proveedor = osiris_erp_proveedores.id_proveedor " +
							"AND osiris_erp_factura_compra_enca.id_emisor = osiris_erp_emisor.id_emisor " +
							query_proveedores+
							query_rango_fechas+
							"ORDER BY to_char(osiris_erp_factura_compra_enca.fecha_factura,'yyyy-MM-dd'),osiris_erp_factura_compra_enca.id_proveedor,numerofactura;";
			NpgsqlConnection conexion; 
			conexion = new NpgsqlConnection (connectionString+nombrebd);
			// Verifica que la base de datos este conectada
			try{
				conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand ();
				comando.CommandText = query_consulta;
				//Console.WriteLine(comando.CommandText);
				NpgsqlDataReader lector = comando.ExecuteReader ();
               	while(lector.Read()){
					totalnro_facturas += 1;
					if(lector["numero_orden_compra"].ToString() != ""){
						numeros_requis = (string) busca_nros_requisiciones(lector["numero_orden_compra"].ToString());
					}					
					treeViewEngineFacProv.AppendValues(false,
					                                   lector["fechacaptura"].ToString().Trim(),
					                                   lector["fechafactura"].ToString().Trim(),
					                                   lector["descripcion_proveedor"].ToString().Trim(),
					                                   lector["numerofactura"].ToString().Trim(),
					                                   float.Parse(lector["subtotal_factura"].ToString().Trim()).ToString("F"),
					                                   float.Parse(lector["iva_factura"].ToString().Trim()).ToString("F"),
					                                   float.Parse(lector["total_factura"].ToString().Trim()).ToString("F"),
					                                   lector["numero_orden_compra"].ToString(),
					                                   numeros_requis,
					                                   lector["uuid"].ToString().Trim());
					subtotal_factprov += float.Parse(lector["subtotal_factura"].ToString().Trim());
					ivatotal_factprov += float.Parse(lector["iva_factura"].ToString().Trim());
					totatota_factprov += float.Parse(lector["total_factura"].ToString().Trim());
				}
				entry_subtotal_cont.Text = subtotal_factprov.ToString("C");
				entry_iva_cont.Text = ivatotal_factprov.ToString("C");
				entry_total_cont.Text = totatota_factprov.ToString("C");
				entry_totalnro_facturas.Text = totalnro_facturas.ToString().Trim();
			}catch (NpgsqlException ex){
				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
										MessageType.Error,ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
				msgBoxError.Run ();				msgBoxError.Destroy();
			}
			conexion.Close ();			
		}
		
		string busca_nros_requisiciones(string numero_orden_compra_)
		{
			string numerosdereq = "";
			NpgsqlConnection conexion; 
			conexion = new NpgsqlConnection (connectionString+nombrebd);
			// Verifica que la base de datos este conectada
			try{
				conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand ();
				comando.CommandText = "SELECT id_requisicion,numero_orden_compra FROM osiris_erp_requisicion_deta " +
									"WHERE numero_orden_compra = '"+numero_orden_compra_.Trim()+"' GROUP BY id_requisicion,numero_orden_compra";
				//Console.WriteLine(comando.CommandText);
				NpgsqlDataReader lector = comando.ExecuteReader ();
               	
				if(lector.Read()){
					numerosdereq = lector["id_requisicion"].ToString();
				}	
				while(lector.Read()){
					numerosdereq += ","+lector["id_requisicion"].ToString();
				}
			}catch (NpgsqlException ex){
				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
										MessageType.Error,ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
				msgBoxError.Run ();				msgBoxError.Destroy();
			}
			conexion.Close ();
			return numerosdereq;
		}
				
		void crea_treeview_facproveedes()
		{
			treeViewEngineFacProv = new TreeStore(typeof(bool),
			                                      typeof (string),
			                                      typeof (string),
			                                      typeof (string),
			                                      typeof (string),
			                                      typeof (string),
			                                      typeof (string),
			                                      typeof (string),
			                                      typeof (string),
			                                      typeof (string),
			                                      typeof (string));
			
			treeview_lista_facprove.Model = treeViewEngineFacProv;
			
			treeview_lista_facprove.RulesHint = true;
			
			col_00 = new TreeViewColumn();
			cellrt_00 = new CellRendererToggle();
			col_00.Title = "Seleccion";
			col_00.PackStart(cellrt_00, true);
			col_00.AddAttribute (cellrt_00, "active", 0);
			cellrt_00.Activatable = true;
			cellrt_00.Toggled += selecciona_fila;
			
			col_01 = new TreeViewColumn();
			cellrt_01 = new CellRendererText();
			col_01.Title = "Fech.Ingreso";
			col_01.PackStart(cellrt_01, true);
			col_01.AddAttribute (cellrt_01, "text", 1);
			//col_cantidad.SortColumnId = (int) col_requisicion.col_cantidad;
			
			col_02 = new TreeViewColumn();
			cellrt_02 = new CellRendererText();
			col_02.Title = "Fech.Factura";
			col_02.PackStart(cellrt_02, true);
			col_02.AddAttribute (cellrt_02, "text", 2);
			//col_cantidad.SortColumnId = (int) col_requisicion.col_cantidad;
			
			col_03 = new TreeViewColumn();
			cellrt_03 = new CellRendererText();
			col_03.Title = "Proveedor";
			col_03.PackStart(cellrt_03, true);
			col_03.AddAttribute (cellrt_03, "text", 3);
			//col_cantidad.SortColumnId = (int) col_requisicion.col_cantidad;
			
			col_04 = new TreeViewColumn();
			cellrt_04 = new CellRendererText();
			col_04.Title = "Nro. Factura";
			col_04.PackStart(cellrt_04, true);
			col_04.AddAttribute (cellrt_04, "text", 4);
			//col_cantidad.SortColumnId = (int) col_requisicion.col_cantidad;
			
			col_05 = new TreeViewColumn();
			cellrt_05 = new CellRendererText();
			col_05.Title = "SUB-TOTAL";
			col_05.PackStart(cellrt_05, true);
			col_05.AddAttribute (cellrt_05, "text", 5);
			//col_cantidad.SortColumnId = (int) col_requisicion.col_cantidad;
			
			col_06 = new TreeViewColumn();
			cellrt_06 = new CellRendererText();
			col_06.Title = "IVA";
			col_06.PackStart(cellrt_06, true);
			col_06.AddAttribute (cellrt_06, "text", 6);
			//col_cantidad.SortColumnId = (int) col_requisicion.col_cantidad;
			
			col_07 = new TreeViewColumn();
			cellrt_07 = new CellRendererText();
			col_07.Title = "TOTAL";
			col_07.PackStart(cellrt_07, true);
			col_07.AddAttribute (cellrt_07, "text", 7);
			//col_cantidad.SortColumnId = (int) col_requisicion.col_cantidad;
			
			col_08 = new TreeViewColumn();
			cellrt_08 = new CellRendererText();
			col_08.Title = "Ords. Compra";
			col_08.PackStart(cellrt_08, true);
			col_08.AddAttribute (cellrt_08, "text", 8);
			
			col_09 = new TreeViewColumn();
			cellrt_09 = new CellRendererText();
			col_09.Title = "Nros. Requis.";
			col_09.PackStart(cellrt_09, true);
			col_09.AddAttribute (cellrt_09, "text", 9);
			//col_cantidad.SortColumnId = (int) col_requisicion.col_cantidad;
			
			col_10 = new TreeViewColumn();
			cellrt_10 = new CellRendererText();
			col_10.Title = "UUID";
			col_10.PackStart(cellrt_10, true);
			col_10.AddAttribute (cellrt_10, "text", 10);
			//col_cantidad.SortColumnId = (int) col_requisicion.col_cantidad;
			
			treeview_lista_facprove.AppendColumn(col_00);
			treeview_lista_facprove.AppendColumn(col_01);
			treeview_lista_facprove.AppendColumn(col_02);
			treeview_lista_facprove.AppendColumn(col_03);
			treeview_lista_facprove.AppendColumn(col_04);
			treeview_lista_facprove.AppendColumn(col_05);
			treeview_lista_facprove.AppendColumn(col_06);
			treeview_lista_facprove.AppendColumn(col_07);
			treeview_lista_facprove.AppendColumn(col_08);
			treeview_lista_facprove.AppendColumn(col_09);
			treeview_lista_facprove.AppendColumn(col_10);
		}
		
		void on_button_reporte_clicked(object sender, EventArgs args)
		{
			if(radiobutton_valid_conta.Active == true){
				new osiris.rpt_facturas_proveedores("facprov_validadas",query_consulta);
			}
			if(radiobutton_x_valid_conta.Active == true){
				new osiris.rpt_facturas_proveedores("facprov_por_validadas",query_consulta);
			}
		}
		
		void on_button_export_sheet_clicked(object sender, EventArgs args)
		{
			string[] args_names_field = {"fecha_ingreso","fecha_factura","proveedor","numero_factura","sub_total","iva","total","orden_compra","requisiciones","UUID"};
			string[] args_type_field = {"string","string","string","string","float","float","float","string","string","string"};
			string[,] args_formulas = {{"",""}};
			string[,] args_width = {{"2","8cm"},{"9","7cm"}};
			int files_field = 0;
			TreeIter iter;
 			if (treeViewEngineFacProv.GetIterFirst (out iter)){
				//Create new spreadsheet open document (.ods) 
				AODL.Document.SpreadsheetDocuments.SpreadsheetDocument spreadsheetDocument = new AODL.Document.SpreadsheetDocuments.SpreadsheetDocument();
				spreadsheetDocument.New();			
				//Create a new table
				AODL.Document.Content.Tables.Table table = new AODL.Document.Content.Tables.Table(spreadsheetDocument, "hoja1", "tablefirst");
				for (int colum_field = 0; colum_field < args_names_field.Length; colum_field++){
					AODL.Document.Content.Tables.Cell cell = table.CreateCell ();
					//cell.OfficeValueType ="float";
					AODL.Document.Content.Text.Paragraph paragraph = new AODL.Document.Content.Text.Paragraph(spreadsheetDocument);
					string text = (string) args_names_field[ colum_field ].ToString().Trim();			
					paragraph.TextContent.Add(new AODL.Document.Content.Text.SimpleText(spreadsheetDocument,text));
					cell.Content.Add(paragraph);
					cell.OfficeValueType = "string";
					cell.OfficeValue = text;
					//cell.StyleName = cell.CellStyle.CellProperties.
					table.InsertCellAt (files_field, colum_field, cell);
					//table.ColumnCollection[colum_field].ColumnStyle.StyleName = "bold";
				}
				// establece el ancho de la columna
				int cell_style1;
				string cell_style2;				
				if(args_width[0,0] != ""){
					for (int i = 0; i < args_width.Length / 2; i++){
					    cell_style1 = int.Parse(args_width[i,0].ToString());
						cell_style2 = args_width[i,1].ToString();
						table.ColumnCollection[cell_style1].ColumnStyle.ColumnProperties.Width = cell_style2;
						//table.ColumnCollection[int.Parse(args_width[0,0].ToString())].ColumnStyle.ColumnProperties.Width = args_width[0,1].ToString();
					    //Console.WriteLine("{0}, {1}", cell_style1, cell_style2);
					}
				}				
				files_field++;
				for (int colum_field = 0; colum_field < args_names_field.Length; colum_field++){					
					AODL.Document.Content.Tables.Cell cell = table.CreateCell ();
					//cell.OfficeValueType ="float";
					AODL.Document.Content.Text.Paragraph paragraph = new AODL.Document.Content.Text.Paragraph(spreadsheetDocument);				
					string text = (string) treeview_lista_facprove.Model.GetValue (iter,colum_field+1);
					paragraph.TextContent.Add(new AODL.Document.Content.Text.SimpleText(spreadsheetDocument,text));
					cell.Content.Add(paragraph);
					cell.OfficeValueType = (string) args_type_field [colum_field];
					cell.OfficeValue = text;
					table.InsertCellAt (files_field, colum_field, cell);					
				}
				files_field++;
				while (treeViewEngineFacProv.IterNext(ref iter)){
					for (int colum_field = 0; colum_field < args_names_field.Length; colum_field++){					
						AODL.Document.Content.Tables.Cell cell = table.CreateCell ();
						//cell.OfficeValueType ="float";
						AODL.Document.Content.Text.Paragraph paragraph = new AODL.Document.Content.Text.Paragraph(spreadsheetDocument);				
						string text = (string) treeview_lista_facprove.Model.GetValue (iter,colum_field+1);
						paragraph.TextContent.Add(new AODL.Document.Content.Text.SimpleText(spreadsheetDocument,text));
						cell.Content.Add(paragraph);
						cell.OfficeValueType = (string) args_type_field [colum_field];
						cell.OfficeValue = text;
						table.InsertCellAt (files_field, colum_field, cell);					
					}
					files_field++;
				}
				if(args_formulas[0,0] != ""){					
					for (int i = 0; i < args_formulas.Length / 2; i++){
						AODL.Document.Content.Tables.Cell cell1 = table.CreateCell ();
						AODL.Document.Content.Text.Paragraph paragraph1 = new AODL.Document.Content.Text.Paragraph(spreadsheetDocument);				
						cell1.Content.Add(paragraph1);
						cell1.OfficeValueType = "float";
						//   ---cell1.OfficeValue = text;
						cell1.Formula = args_formulas[i,1].ToString()+files_field+")";
						table.InsertCellAt (files_field, int.Parse(args_formulas[i,0]), cell1);
						//Console.WriteLine("{0}, {1}", args_formulas[i,0], args_formulas[i,1]);
					}
				}
				
				//Insert table into the spreadsheet document
				spreadsheetDocument.TableCollection.Add(table);
				string ods_name = System.IO.Path.GetTempPath() + Guid.NewGuid().ToString() + ".ods";
				spreadsheetDocument.SaveTo(ods_name);				
				
				/*
				System.Diagnostics.Process proc = new System.Diagnostics.Process();
				proc.EnableRaisingEvents = true;
				proc.StartInfo.UseShellExecute = false;
				proc.StartInfo.CreateNoWindow = true;
				proc.StartInfo.RedirectStandardOutput = true;
				proc.StartInfo.WorkingDirectory = System.AppDomain.CurrentDomain.BaseDirectory;

				//System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory,"OSIRISLogo.jpg")
				proc.StartInfo.FileName = "libreoffice --calc";
				proc.StartInfo.Arguments = System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory,"export.ods");
				proc.Start();
				*/

				try{
					// open the document automatic
					System.Diagnostics.Process.Start(ods_name);
				}catch(Exception ex){
					MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
											MessageType.Error, 
											ButtonsType.Close,"Open error file: {0}",ex.Message);
					msgBoxError.Run ();		msgBoxError.Destroy();
				}
			}
		}
		
		void on_button_busca_proveedores_clicked(object sender, EventArgs args)
		{
			// Los parametros de del SQL siempre es primero cuando busca todo y la otra por expresion
			// la clase recibe tambien el orden del query es importante definir que tipo de busqueda es para que los objetos caigan ahi mismo
			object[] parametros_objetos = {entry_id_proveedor,entry_nombre_proveedor};
			string[] parametros_sql = {"SELECT descripcion_proveedor,direccion_proveedor,rfc_proveedor,curp_proveedor, "+
								"colonia_proveedor,municipio_proveedor,estado_proveedor,telefono1_proveedor, "+ 
								"telefono2_proveedor,celular_proveedor,rfc_proveedor, proveedor_activo, "+
								"id_proveedor,contacto1_proveedor,mail_proveedor,pagina_web_proveedor,"+
								"osiris_erp_proveedores.id_forma_de_pago,descripcion_forma_de_pago,fax_proveedor "+
								"FROM osiris_erp_proveedores, osiris_erp_forma_de_pago "+
								"WHERE osiris_erp_proveedores.id_forma_de_pago = osiris_erp_forma_de_pago.id_forma_de_pago "};
			string [] parametros_string = {};
			string[,] args_buscador1 = {{"PROVEEDOR","AND descripcion_proveedor LIKE '%","%'" },
										{"ID PROVEEDOR","AND id_proveedor = '","' "},
										{"RFC","AND rfc_proveedor = '","' "}};
			string[,] args_buscador2 = {{"ID PROVEEDOR","AND id_proveedor = '","' "},
										{"PROVEEDOR","AND descripcion_proveedor LIKE '%","%'" },
										{"RFC","AND rfc_proveedor = '","' "}};
			string[,] args_orderby = {{"",""}};
			classfind_data.buscandor(parametros_objetos,parametros_sql,parametros_string,"find_proveedores_catalogo_producto",0,args_buscador1,args_buscador2,args_orderby);
		}
		
		// Cuando seleccion campos para la autorizacion de compras  
		void selecciona_fila(object sender, ToggledArgs args)
		{
			TreeIter iter;
			TreePath path = new TreePath (args.Path);
			if (treeview_lista_facprove.Model.GetIter (out iter, path)){					
				bool old = (bool) treeview_lista_facprove.Model.GetValue(iter,0);
				if(!old == true){
					//entry_subtotal_cont.Text = Convert.ToString(float.Parse((string) entry_subtotal_cont.Text) + float.Parse((string) treeview_lista_facprove.Model.GetValue(iter,5)));
				}else{
					//entry_subtotal_cont.Text = Convert.ToString(float.Parse((string) entry_subtotal_cont.Text) - float.Parse((string) treeview_lista_facprove.Model.GetValue(iter,5))); 
				}
				//Console.WriteLine(selecciono_productos.ToString());
				treeview_lista_facprove.Model.SetValue(iter,0,!old);
			}				
		}
		
		// cierra ventanas emergentes
		void on_cierraventanas_clicked (object sender, EventArgs args)
		{
			Widget win = (Widget) sender;
			win.Toplevel.Destroy();
		}
	}
}