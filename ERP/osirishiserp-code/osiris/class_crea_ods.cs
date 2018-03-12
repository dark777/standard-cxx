//  
//  class_crea_ods.cs
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
//using SmartXLS;		// libreria para crear archivo xls es de paga
using Npgsql;
using Gtk;
using Glade;

// libreria creada con el proyecto AODL 1.4 .ods
using AODL;
using AODL.Document.SpreadsheetDocuments;
using AODL.Document.Content;
using AODL.Document.Content.Tables;
using AODL.Document;
using AODL.Package;
using AODL.Document.Collections;
//using NUnit.Framework;

// libreria EPPlus crea hojas de calculo en formato .xlsx
using System.Collections;
using OfficeOpenXml;
using System.IO;
using System.Reflection;

namespace osiris
{
	public class class_traslate_spreadsheet
	{
		string connectionString;
		string nombrebd;		
		class_conexion conexion_a_DB = new class_conexion();
				
		//Declaracion de ventana de error
		protected Gtk.Window MyWinError;
		protected Gtk.Window MyWin;
		
		public class_traslate_spreadsheet (string query_sql,string[] args_names_field,string[] args_type_field,bool typetext,
		                                   string[] args_field_text,string name_field_text,bool more_title,string[] args_more_title,string[,] args_formulas,string[,] args_width)
		{
			//Console.WriteLine(name_field_text+" nombre del campo");
			int files_field = 0;
			string [] array_field_text = new string[args_field_text.Length];
			connectionString = conexion_a_DB._url_servidor+conexion_a_DB._port_DB+conexion_a_DB._usuario_DB+conexion_a_DB._passwrd_user_DB;
			nombrebd = conexion_a_DB._nombrebd;			
			//Create new spreadsheet open document (.ods) 
			AODL.Document.SpreadsheetDocuments.SpreadsheetDocument spreadsheetDocument = new AODL.Document.SpreadsheetDocuments.SpreadsheetDocument();
			spreadsheetDocument.New();			
			//Create a new table
			AODL.Document.Content.Tables.Table table = new AODL.Document.Content.Tables.Table(spreadsheetDocument, "hoja1", "tablefirst");
			NpgsqlConnection conexion; 
			conexion = new NpgsqlConnection (connectionString+nombrebd);
	        // Verifica que la base de datos este conectada
			try{
				conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand ();
				comando.CommandText = query_sql;
				//Console.WriteLine(comando.CommandText);
				comando.ExecuteNonQuery();    comando.Dispose();
				NpgsqlDataReader lector = comando.ExecuteReader ();
				// Creando los nombres de ancabezado de los campos				
				for (int colum_field = 0; colum_field < args_names_field.Length; colum_field++){
					AODL.Document.Content.Tables.Cell cell = table.CreateCell ();
					//cell.OfficeValueType ="float";
					AODL.Document.Content.Text.Paragraph paragraph = new AODL.Document.Content.Text.Paragraph(spreadsheetDocument);
					
					//AODL.Document.Content.Text.FormatedText formText;
					//formText.TextStyle.TextProperties.Bold();
															
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
								
				if(typetext == true){
					// Creando los nombres de ancabezado de los campos cuando son de tipo text (almacenado en una tabla tipo text)
					for(int colum_field2 = 0 ; colum_field2 < args_field_text.Length; colum_field2++){					
						AODL.Document.Content.Tables.Cell cell = table.CreateCell ();
						//cell.OfficeValueType ="float";
						AODL.Document.Content.Text.Paragraph paragraph = new AODL.Document.Content.Text.Paragraph(spreadsheetDocument);				
						string text = (string) args_field_text[ colum_field2 ].ToString().Trim();			
						paragraph.TextContent.Add(new AODL.Document.Content.Text.SimpleText(spreadsheetDocument,text));
						cell.Content.Add(paragraph);
						cell.OfficeValueType = "string";							
						cell.OfficeValue = text;
						table.InsertCellAt (files_field, colum_field2+args_names_field.Length, cell);					
					}
				}
				if(more_title == true){
					int title_field_text = 0;
					if(typetext == true){
						title_field_text = args_field_text.Length;
					}
					// Creando los nombres de ancabezado de los campos cuando son de tipo text (almacenado en una tabla tipo text)
					for(int colum_field3 = 0 ; colum_field3 < args_more_title.Length; colum_field3++){					
						AODL.Document.Content.Tables.Cell cell = table.CreateCell ();
						//cell.OfficeValueType ="float";
						AODL.Document.Content.Text.Paragraph paragraph = new AODL.Document.Content.Text.Paragraph(spreadsheetDocument);				
						string text = (string)args_more_title[ colum_field3 ].ToString().Trim();			
						paragraph.TextContent.Add(new AODL.Document.Content.Text.SimpleText(spreadsheetDocument,text));
						cell.Content.Add(paragraph);
						cell.OfficeValueType = "string";							
						cell.OfficeValue = text;
						table.InsertCellAt (files_field, colum_field3+args_names_field.Length+title_field_text, cell);					
					}
				}
				files_field++;
				string texto = "";
				while (lector.Read()){
					for (int colum_field = 0; colum_field < args_names_field.Length; colum_field++){					
						AODL.Document.Content.Tables.Cell cell = table.CreateCell ();
						//cell.OfficeValueType ="float";
						AODL.Document.Content.Text.Paragraph paragraph = new AODL.Document.Content.Text.Paragraph(spreadsheetDocument);				
						string text = lector[(string) args_names_field[ colum_field ]].ToString().Trim();			
						paragraph.TextContent.Add(new AODL.Document.Content.Text.SimpleText(spreadsheetDocument,text));
						cell.Content.Add(paragraph);
						cell.OfficeValueType = (string) args_type_field [colum_field];							
						cell.OfficeValue = text;
						table.InsertCellAt (files_field, colum_field, cell);					
					}
					if(typetext == true){
						texto = (string) lector[name_field_text]; // puede ser una campo de la base de datos tipo Text
						char[] delimiterChars = {'\n'}; // delimitador de Cadenas
						char[] delimiterChars1 = {';'}; // delimitador de Cadenas
						//string texto = "1;daniel; ;olivares;cuevas";
						//"2;genaro;cuevas;bazaldua\n"+
						//"3;gladys;perez;orellana\n";
						string[] words = texto.Split(delimiterChars); // Separa las Cadenas
						if(texto != ""){						
							// Recorre la variable
							foreach (string s in words){
								if (s.Length > 0){
									string texto1 = (string) s;
									string[] words1 = texto1.Split(delimiterChars1);
									//for (int i = 1; i <= 6; i++){
									int i=0;
									int i2 = 1;
									foreach (string s1 in words1){
										//Console.WriteLine(s1.ToString());
										if(i2 <= args_field_text.Length){
											array_field_text[i] = s1.ToString();
										}
										i++;
										i2++;
									}
								}
							}
							for( int i = 0; i < array_field_text.Length; i++ ){
								//Console.WriteLine(array_field_text[i]);
								AODL.Document.Content.Tables.Cell cell = table.CreateCell ();
								//cell.OfficeValueType ="float";
								AODL.Document.Content.Text.Paragraph paragraph = new AODL.Document.Content.Text.Paragraph(spreadsheetDocument);				
								string text = (string) array_field_text[i];			
								paragraph.TextContent.Add(new AODL.Document.Content.Text.SimpleText(spreadsheetDocument,text));
								cell.Content.Add(paragraph);
								cell.OfficeValueType = "string";							
								cell.OfficeValue = text;
								table.InsertCellAt (files_field, i+args_names_field.Length, cell);
							}
						}else{
							
						}
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
				conexion.Close();				
				
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

			}catch(NpgsqlException ex){
					MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
									MessageType.Error, 
									ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
					msgBoxError.Run ();		msgBoxError.Destroy();
			}
		}		
	}

	public class class_traslate_ods_array
	{
		protected Gtk.Window MyWinError;

		public class_traslate_ods_array(TreeView treeviewobject_,ListStore treeViewEngine_,string[,] args_name_type_active,bool typetext,
			string[] args_field_text,string name_field_text,bool more_title,string[] args_more_title,string[,] args_formulas,string[,] args_width,string titulo1,string titulo2)
		{
			TreeIter iter;
			int files_field = 0;
			if (treeViewEngine_.GetIterFirst (out iter)){
				//Create new spreadsheet open document (.ods) 
				AODL.Document.SpreadsheetDocuments.SpreadsheetDocument spreadsheetDocument = new AODL.Document.SpreadsheetDocuments.SpreadsheetDocument();
				spreadsheetDocument.New();			
				//Create a new table
				AODL.Document.Content.Tables.Table table = new AODL.Document.Content.Tables.Table(spreadsheetDocument, "hoja1", "tablefirst");
				// creando los titulos del reporte
				if (titulo1 != "") {
					string text;
					AODL.Document.Content.Tables.Cell cell;
					cell = table.CreateCell ();
					//cell.OfficeValueType ="float";
					AODL.Document.Content.Text.Paragraph paragraph;
					paragraph = new AODL.Document.Content.Text.Paragraph (spreadsheetDocument);
					text = titulo1;			
					paragraph.TextContent.Add (new AODL.Document.Content.Text.SimpleText (spreadsheetDocument, text));
					cell.Content.Add (paragraph);
					cell.OfficeValueType = "string";
					cell.OfficeValue = text;
					//cell.StyleName = cell.CellStyle.CellProperties.
					table.InsertCellAt (files_field, 0, cell);
					files_field++;
					if (titulo2 != "") {
						cell = table.CreateCell ();
						cell.OfficeValueType ="float";
						paragraph = new AODL.Document.Content.Text.Paragraph (spreadsheetDocument);
						text = "";
						paragraph.TextContent.Add (new AODL.Document.Content.Text.SimpleText (spreadsheetDocument, text));
						cell.Content.Add (paragraph);
						cell.OfficeValueType = "string";
						cell.OfficeValue = text;
						//cell.StyleName = cell.CellStyle.CellProperties.
						table.InsertCellAt (files_field, 0, cell);
						files_field++;

						cell = table.CreateCell ();
						//cell.OfficeValueType ="float";
						paragraph = new AODL.Document.Content.Text.Paragraph (spreadsheetDocument);
						text = titulo2;
						paragraph.TextContent.Add (new AODL.Document.Content.Text.SimpleText (spreadsheetDocument, text));
						cell.Content.Add (paragraph);
						cell.OfficeValueType = "string";
						cell.OfficeValue = text;
						//cell.StyleName = cell.CellStyle.CellProperties.
						table.InsertCellAt (files_field, 0, cell);
						files_field++;

						cell = table.CreateCell ();
						//cell.OfficeValueType ="float";
						paragraph = new AODL.Document.Content.Text.Paragraph (spreadsheetDocument);
						text = "";
						paragraph.TextContent.Add (new AODL.Document.Content.Text.SimpleText (spreadsheetDocument, text));
						cell.Content.Add (paragraph);
						cell.OfficeValueType = "string";
						cell.OfficeValue = text;
						//cell.StyleName = cell.CellStyle.CellProperties.
						table.InsertCellAt (files_field, 0, cell);
						files_field++;
					}
				}

				// Creando los nombres de ancabezado de los campos
				int columnas_validas = 0;
				for (int colum_field = 0; colum_field <= args_name_type_active.GetUpperBound (0); colum_field++){
					if ((string) args_name_type_active [colum_field, 2] == "active") {
						AODL.Document.Content.Tables.Cell cell = table.CreateCell ();
						//cell.OfficeValueType ="float";
						AODL.Document.Content.Text.Paragraph paragraph = new AODL.Document.Content.Text.Paragraph(spreadsheetDocument);
						string text = (string) args_name_type_active[ colum_field,0 ].ToString().Trim();			
						paragraph.TextContent.Add(new AODL.Document.Content.Text.SimpleText(spreadsheetDocument,text));
						cell.Content.Add(paragraph);
						cell.OfficeValueType = "string";
						cell.OfficeValue = text;
						//cell.StyleName = cell.CellStyle.CellProperties.
						table.InsertCellAt (files_field, columnas_validas, cell);
						//table.ColumnCollection[colum_field].ColumnStyle.StyleName = "bold";
						columnas_validas++;
					}
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


				if(more_title == true){
					int title_field_text = 0;
					if(typetext == true){
						title_field_text = args_field_text.Length;
					}
					// Creando los nombres de ancabezado de los campos cuando son de tipo text (almacenado en una tabla tipo text)
					for(int colum_field3 = 0 ; colum_field3 < args_more_title.Length; colum_field3++){					
						AODL.Document.Content.Tables.Cell cell = table.CreateCell ();
						//cell.OfficeValueType ="float";
						AODL.Document.Content.Text.Paragraph paragraph = new AODL.Document.Content.Text.Paragraph(spreadsheetDocument);				
						string text = (string)args_more_title[ colum_field3 ].ToString().Trim();			
						paragraph.TextContent.Add(new AODL.Document.Content.Text.SimpleText(spreadsheetDocument,text));
						cell.Content.Add(paragraph);
						cell.OfficeValueType = "string";							
						cell.OfficeValue = text;
						table.InsertCellAt (files_field, colum_field3+args_name_type_active.Length+title_field_text, cell);					
					}
				}

				// insertando valores que estan dentro del treeview
				files_field++;
				columnas_validas = 0;
				for (int colum_field = 0; colum_field <= args_name_type_active.GetUpperBound (0); colum_field++){
					if ((string) args_name_type_active [colum_field, 2] == "active") {
						AODL.Document.Content.Tables.Cell cell = table.CreateCell ();
						//cell.OfficeValueType ="float";
						AODL.Document.Content.Text.Paragraph paragraph = new AODL.Document.Content.Text.Paragraph (spreadsheetDocument);				
						string text = (string) treeviewobject_.Model.GetValue (iter, colum_field);
						paragraph.TextContent.Add (new AODL.Document.Content.Text.SimpleText (spreadsheetDocument, text));
						cell.Content.Add (paragraph);
						cell.OfficeValueType = (string) args_name_type_active [colum_field, 1];
						cell.OfficeValue = text;
						table.InsertCellAt (files_field, columnas_validas, cell);
						columnas_validas++;
					}
				}

				files_field++;
				columnas_validas = 0;
				while (treeViewEngine_.IterNext(ref iter)){
					for (int colum_field = 0; colum_field <= args_name_type_active.GetUpperBound (0); colum_field++){
						if ((string)args_name_type_active [colum_field, 2] == "active") {
							AODL.Document.Content.Tables.Cell cell = table.CreateCell ();
							//cell.OfficeValueType ="float";
							AODL.Document.Content.Text.Paragraph paragraph = new AODL.Document.Content.Text.Paragraph (spreadsheetDocument);				
							string text = (string)treeviewobject_.Model.GetValue (iter, colum_field);
							paragraph.TextContent.Add (new AODL.Document.Content.Text.SimpleText (spreadsheetDocument, text));
							cell.Content.Add (paragraph);
							cell.OfficeValueType = (string) args_name_type_active [colum_field,1];
							cell.OfficeValue = text;
							table.InsertCellAt (files_field, columnas_validas, cell);
							columnas_validas++;
						}
					}
					columnas_validas = 0;
					files_field++;
				}

				// activando las formulas
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
	}
	
	public class class_traslate_xlsx
	{
		// usando libreria EPPlus.dll
		// Declaracion de ventana de error
		protected Gtk.Window MyWinError;
		protected Gtk.Window MyWin;
		
		public class_traslate_xlsx()
		{
			string xlsx_name = System.IO.Path.GetTempPath() + Guid.NewGuid().ToString() + ".xlsx";
			FileInfo newFile = new FileInfo(xlsx_name);
			
			ExcelPackage hojacalaculo = new ExcelPackage();
			hojacalaculo.Workbook.Worksheets.Add("hoja de calculo");
			
			ExcelWorksheet ws = hojacalaculo.Workbook.Worksheets[1];
			ws.Name = "sheet1"; //Setting Sheet's name
			//ws.Cells.Style.Font.Size = 10; //Default font size for whole sheet
			//ws.Cells.Style.Font.Name = "Calibri"; //Default Font name for whole sheet
			
			// ancho de la fila
			//ws.Row(1).Height = 20;
			//ws.Row(2).Height = 18;
			
			// Definir el valor de una celda de ambas maneras
			//ws.Cells[1, 1].Value = "Hello World";
			ws.Cells["A1"].Value = "ColumnaA";
			ws.Cells["B1"].Value = "ColumnaB";
			
			ws.Cells["A2"].Value = 100;
			ws.Cells["B2"].Value = 200;
			
			ws.Cells["A3"].Value = 200;
			ws.Cells["B3"].Value = 300;
			
			ws.Cells["A4"].Formula = "=SUM(A2:A3)";
			ws.Cells["B4"].Formula = "=SUM(B2:B3)";
			hojacalaculo.SaveAs(newFile);
			
			try{
				// open the document automatic
				System.Diagnostics.Process.Start("export.xlsx");
			}catch(Exception ex){
				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
											MessageType.Error, 
											ButtonsType.Close,"Open error file: {0}",ex.Message);
				msgBoxError.Run ();		msgBoxError.Destroy();
			}
		}
	}
}