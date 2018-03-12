//  
//  cambia_fecha_cargo.cs
//  
//  Author:
//       Daniel Olivares <arcangeldoc@openmailbox.org>
// 
//  Copyright (c) 2015 dolivares
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
using Glade;
using Gdk;
using System.Data;
using GLib;
using System.Collections;

namespace osiris
{
	public class cambia_fecha_cargo
	{
		
		// Ventana de Rango de Fecha
		[Widget] Gtk.Window cambia_fecha_cargos = null;
		[Widget] Gtk.Entry entry_folio_servicio = null;
		[Widget] Gtk.Entry entry_pid_paciente = null;
		[Widget] Gtk.Entry entry_nombre_paciente = null;
		[Widget] Gtk.Entry entry_dia_selec = null;
		[Widget] Gtk.Entry entry_mes_selec = null;
		[Widget] Gtk.Entry entry_ano_selec = null;
		[Widget] Gtk.Entry entry_dia_nuevo = null;
		[Widget] Gtk.Entry entry_mes_nuevo = null;
		[Widget] Gtk.Entry entry_ano_nuevo = null;
		
		[Widget] Gtk.Button button_guardar = null;
		[Widget] Gtk.TreeView treeview_lista_fechas = null;
		[Widget] Gtk.Button button_salir = null;
		[Widget] Gtk.Statusbar statusbar_cambio_fecha = null;
		
		private string LoginEmpleado;
		private string NomEmpleado = "";
		private string AppEmpleado = "";
		private string ApmEmpleado = "";
		private string connectionString = "";
		private string nombrebd = "";
		
		private int PidPaciente;
		private int folioservicio;
		private string nombre_paciente;
		
		private TreeStore treeViewEngineListaFechas = null;		// Pacientes
		
		//Declaracion de ventana de error
		protected Gtk.Window MyWinError;
		
		private ArrayList columns = new ArrayList ();
		private Gtk.TreeIter iter;
		
		private class_conexion conexion_a_DB = new class_conexion();
		private class_public classpublic = new class_public();
		
		public cambia_fecha_cargo (string LoginEmpleado_, string NomEmpleado_, string AppEmpleado_, string ApmEmpleado_, string nombrebd_,int PidPaciente_ ,int folioservicio_,string nombre_paciente_)
		{
			LoginEmpleado = LoginEmpleado_;
			NomEmpleado = NomEmpleado_;
			AppEmpleado = AppEmpleado_;
			ApmEmpleado = ApmEmpleado_;
			PidPaciente = PidPaciente_;
			folioservicio = folioservicio_;
			nombre_paciente = nombre_paciente_;
			connectionString = conexion_a_DB._url_servidor+conexion_a_DB._port_DB+conexion_a_DB._usuario_DB+conexion_a_DB._passwrd_user_DB;
			nombrebd = conexion_a_DB._nombrebd;
			
			Glade.XML gxml = new Glade.XML (null, "caja.glade", "cambia_fecha_cargos", null);
			gxml.Autoconnect (this);
	        // Muestra ventana de Glade
			cambia_fecha_cargos.Show();
			
			button_guardar.Clicked += new EventHandler(on_button_guardar_clicked);
			button_salir.Clicked += new EventHandler(on_cierraventanas_clicked);
			
			entry_folio_servicio.Text = folioservicio.ToString().Trim();
			entry_pid_paciente.Text = PidPaciente.ToString();
			entry_nombre_paciente.Text = nombre_paciente;
			
			entry_folio_servicio.ModifyBase(StateType.Normal, new Gdk.Color(166,220,255));	// Color Celeste
			entry_pid_paciente.ModifyBase(StateType.Normal, new Gdk.Color(166,220,255));	// Color Celeste
			entry_nombre_paciente.ModifyBase(StateType.Normal, new Gdk.Color(166,220,255));	// Color Celeste
			
			statusbar_cambio_fecha.Pop(0);
			statusbar_cambio_fecha.Push(1, "login: "+LoginEmpleado+"  |Usuario: "+NomEmpleado+" "+AppEmpleado+" "+ApmEmpleado);
			statusbar_cambio_fecha.HasResizeGrip = false;
			
			crea_treeview_fechas();
			llenado_treeview_fechas();
		}
		
		void crea_treeview_fechas()
		{
			Gtk.CellRendererText text = null;
			foreach (TreeViewColumn tvc in treeview_lista_fechas.Columns)
							treeview_lista_fechas.RemoveColumn(tvc);
		 	treeViewEngineListaFechas = new TreeStore(typeof(string),
													typeof(string),
													typeof(string),
			                                        typeof(string),
			                                        typeof(bool),
			                                        typeof(bool));
			treeview_lista_fechas.Model = treeViewEngineListaFechas;
			treeview_lista_fechas.RulesHint = true;
			treeview_lista_fechas.MoveCursor += on_lista_fechas;
			treeview_lista_fechas.RowActivated += on_lista_fechas;		// activa el doble click
									
			// column for holiday names
			text = new CellRendererText ();
			text.Xalign = 0.0f;
			columns.Add (text);
			TreeViewColumn column0 = new TreeViewColumn("Dia",text,"text",Column.columna00);
			column0.Resizable = true;
			column0.SortColumnId = (int) Column.columna00;
			treeview_lista_fechas.InsertColumn (column0, (int) Column.columna00);
			
			text = new CellRendererText ();
			text.Xalign = 0.0f;
			columns.Add (text);
			TreeViewColumn column1 = new TreeViewColumn("Mes",text,"text",Column.columna01);
			column1.Resizable = true;
			column1.SortColumnId = (int) Column.columna01;
			treeview_lista_fechas.InsertColumn (column1, (int) Column.columna01);
			
			text = new CellRendererText ();
			text.Xalign = 0.0f;
			columns.Add (text);
			TreeViewColumn column2 = new TreeViewColumn("Año",text,"text",Column.columna02);
			column2.Resizable = true;
			column2.SortColumnId = (int) Column.columna02;
			treeview_lista_fechas.InsertColumn (column2, (int) Column.columna02);
			
			text = new CellRendererText ();
			text.Xalign = 0.0f;
			columns.Add (text);
			TreeViewColumn column3 = new TreeViewColumn("Total Registros",text,"text",Column.columna03);
			column3.Resizable = true;
			column3.SortColumnId = (int) Column.columna03;
			treeview_lista_fechas.InsertColumn (column3, (int) Column.columna03);			
		}
		
		enum Column
		{
			columna00,
			columna01,
			columna02,
			columna03,
			Visible,
			World,			
		}
		
		void on_lista_fechas(object sender, EventArgs args)
		{
			TreeModel model;
			TreeIter iterSelected;
			if (treeview_lista_fechas.Selection.GetSelected(out model, out iterSelected)){
				entry_dia_selec.Text = (string) model.GetValue(iterSelected, 0);
				entry_mes_selec.Text = (string) model.GetValue(iterSelected, 1);
				entry_ano_selec.Text = (string) model.GetValue(iterSelected, 2);
			}
		}
		
		void llenado_treeview_fechas()
		{
			treeViewEngineListaFechas.Clear();
			NpgsqlConnection conexion;
			conexion = new NpgsqlConnection (connectionString+nombrebd );
			// Verifica que la base de datos este conectada
			try{
				conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand ();
				comando.CommandText = "SELECT COUNT(to_char(fechahora_creacion,'yyyy-MM-dd')) AS total_cargos,to_char(fechahora_creacion,'yyyy-MM-dd') AS fecha_cargo,"+
										"to_char(fechahora_creacion,'dd') AS dia_fecha_cargo,to_char(fechahora_creacion,'MM') AS mes_fecha_cargo,to_char(fechahora_creacion,'yyyy') AS ano_fecha_cargo " +
										" FROM osiris_erp_cobros_deta " +
										"WHERE eliminado = 'false' " +
										"AND folio_de_servicio = '"+folioservicio.ToString().Trim()+"' "+
					"GROUP BY to_char(fechahora_creacion,'yyyy-MM-dd'),to_char(fechahora_creacion,'dd'),to_char(fechahora_creacion,'MM'),to_char(fechahora_creacion,'yyyy') " +
					"ORDER BY to_char(fechahora_creacion,'yyyy-MM-dd');";
				//Console.WriteLine(comando.CommandText);
				NpgsqlDataReader lector = comando.ExecuteReader ();
				while((bool) lector.Read()){
					treeViewEngineListaFechas.AppendValues ((string) lector["dia_fecha_cargo"].ToString().Trim(),
							    	(string) lector["mes_fecha_cargo"].ToString().Trim(),
				                	(string) lector["ano_fecha_cargo"].ToString().Trim(),
								    (string) lector["total_cargos"].ToString().Trim(),
							    	true,
							    	true);
					
				}
			}catch (NpgsqlException ex){
				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
							MessageType.Error, ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
				msgBoxError.Run ();						msgBoxError.Destroy();
			}
			conexion.Close();			
		}
		
		void on_button_guardar_clicked(object sender, EventArgs args)
		{
			MessageDialog msgBox;
			if(entry_dia_selec.Text != "" && entry_mes_selec.Text != "" && entry_ano_selec.Text != ""){
				if(entry_dia_nuevo.Text != "" && entry_mes_nuevo.Text != "" && entry_ano_nuevo.Text != ""){
					msgBox = new MessageDialog (MyWinError,DialogFlags.Modal,MessageType.Question,
												ButtonsType.YesNo,"¿ Esta seguro(a) de Cambiar la fecha de Cargo Seleccionada ?");
					ResponseType miResultado = (ResponseType)msgBox.Run ();
					msgBox.Destroy();
					if (miResultado == ResponseType.Yes){
						Npgsql.NpgsqlConnection conexion;
						conexion = new NpgsqlConnection(connectionString+nombrebd);
						try{
							conexion.Open();
							NpgsqlCommand comando;
							comando = conexion.CreateCommand();
							comando.CommandText = "UPDATE osiris_erp_cobros_deta SET "+
													"fechahora_creacion = '"+entry_ano_nuevo.Text.Trim()+"-"+entry_mes_nuevo.Text.Trim()+"-"+entry_dia_nuevo.Text.Trim()+" 00:00:00',"+
													"historia_cambios_fecha = '"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+";"+"fecha "+entry_ano_selec.Text.Trim()+"-"+entry_mes_selec.Text.Trim()+"-"+entry_dia_selec.Text.Trim()+";fecha_nueva "+entry_ano_nuevo.Text.Trim()+"-"+entry_mes_nuevo.Text.Trim()+"-"+entry_dia_nuevo.Text.Trim()+"\n' "+
													"WHERE eliminado = 'false' " +
													"AND osiris_erp_cobros_deta.folio_de_servicio = '"+folioservicio.ToString().Trim()+"' " +
													"AND to_char(osiris_erp_cobros_deta.fechahora_creacion,'yyyy-MM-dd') = '"+entry_ano_selec.Text.Trim()+"-"+entry_mes_selec.Text.Trim()+"-"+entry_dia_selec.Text.Trim()+"';";
							//Console.WriteLine(comando.CommandText);
							comando.ExecuteNonQuery();                  comando.Dispose();
							msgBox = new MessageDialog (MyWinError,DialogFlags.Modal,MessageType.Info,
																	ButtonsType.Ok,"El cambio de la FECHA se realizo con exito!!");
							msgBox.Run();				msgBox.Destroy();
							entry_ano_selec.Text = "";
							entry_mes_selec.Text = "";
							entry_dia_selec.Text = "";
							entry_ano_nuevo.Text = "";
							entry_mes_nuevo.Text = "";
							entry_dia_nuevo.Text = "";
							llenado_treeview_fechas();
						}catch(Npgsql.NpgsqlException ex){
							//Console.WriteLine ("PostgresSQL error: {0}",ex.Message);
							msgBox = new MessageDialog (MyWinError,DialogFlags.Modal,MessageType.Error,
													ButtonsType.Ok,"PostgresSQL error: {0}",ex.Message);
							msgBox.Run();				msgBox.Destroy();
						}
					}
				}else{
					msgBox = new MessageDialog (MyWinError,DialogFlags.Modal,MessageType.Error,
											ButtonsType.Ok,"Digite/Capture FECHA que actualizara, verifique...");
				msgBox.Run();				msgBox.Destroy();
				}
			}else{
				msgBox = new MessageDialog (MyWinError,DialogFlags.Modal,MessageType.Error,
											ButtonsType.Ok,"Seleccione una FECHA, verifique...");
				msgBox.Run();				msgBox.Destroy();
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