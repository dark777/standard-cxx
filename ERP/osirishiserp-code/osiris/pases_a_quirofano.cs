// created on 07/11/2011
//////////////////////////////////////////////////////////
// created on 21/06/2007 at 01:40 p
// Sistema Hospitalario OSIRIS
// Monterrey - Mexico
//
// Autor    	: Ing. Daniel Olivares (Programacion)
//				 
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
	public class pases_a_quirofano
	{
		private static int pangoScale = 1024;
		private PrintOperation print;
		private double fontSize = 8.0;
		int escala_en_linux_windows;		// Linux = 1  Windows = 8
		int comienzo_linea = 05;
		int separacion_linea = 10;
		int numpage = 1;
		PrintContext context;
		
		// Declarando variable publicas
		string connectionString;
		string nombrebd;
		
		// variable publicas
		int pidpaciente;
		int folioservicio;
		int nrodecita;
		int idcentro_costo;
		string LoginEmpleado;
		int idtipopaciente;
		int idempresa_paciente;
		int idaseguradora_paciente;		
		string diagnostico_movcargo = "";
		string nombrecirugia_movcargo = "";
		string descripciontipopaciente = "";
		string tipo_pase = "";
		string query_slq = "";
		string nro_de_pase_ingreso = "";
		
		string tipointernamiento = "";
		int idtipointernamiento = 0;
		
		// Boton general para salir de las ventanas
		// Todas la ventanas en glade este boton debe estra declarado identico
		[Widget] Gtk.Button button_salir;
		
		//[Widget] Gtk.Entry entry_fecha_inicio;
		[Widget] Gtk.Window pases_servicio_medico = null;
		[Widget] Gtk.Entry entry_dia_inicio = null;
		[Widget] Gtk.Entry entry_mes_inicio = null;
		[Widget] Gtk.Entry entry_ano_inicio = null;
		[Widget] Gtk.Button button_grabar_pase = null;
		[Widget] Gtk.Button button_cancela_pase = null;
		[Widget] Gtk.Button button_imprimir_pase = null;
		[Widget] Gtk.ComboBox combobox_tipo_admision = null;
				
		[Widget] Gtk.TreeView lista_pasesservmedico = null;
		
		TreeStore treeViewEnginePases;
		
		//Ventana de cancelacion de PASES
		[Widget] Gtk.Window cancelador_folios = null;
		[Widget] Gtk.Button button_cancelar = null;
		[Widget] Gtk.Entry entry_folio = null;
		[Widget] Gtk.Entry entry_motivo = null;
		[Widget] Gtk.Label label247 = null;
		//Declaracion de ventana de error
		protected Gtk.Window MyWinError;
		protected Gtk.Window MyWin;
		
		string[] args_args = {""};
		int[] args_id_array = {0,1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20};
		
		class_conexion conexion_a_DB = new class_conexion();
		class_public classpublic = new class_public();

		public pases_a_quirofano (int pidpaciente_,int folioservicio_,int idcentro_costo_,string LoginEmpleado_,
		                          int idtipopaciente_,int idempresa_paciente_,int idaseguradora_paciente_,
									bool altamedicapaciente,string tipo_pase_,bool elimina_pase,bool imprimir_pase,bool foliocerrado)
		{
			escala_en_linux_windows = classpublic.escala_linux_windows;
			connectionString = conexion_a_DB._url_servidor+conexion_a_DB._port_DB+conexion_a_DB._usuario_DB+conexion_a_DB._passwrd_user_DB;
			nombrebd = conexion_a_DB._nombrebd;
			pidpaciente = pidpaciente_;
			folioservicio = folioservicio_;
			nrodecita = folioservicio_;
			idcentro_costo = idcentro_costo_;
			LoginEmpleado = LoginEmpleado_;
			idtipopaciente = idtipopaciente_;
			idempresa_paciente = idempresa_paciente_;
			idaseguradora_paciente = idaseguradora_paciente_;
			tipo_pase = tipo_pase_;
			
			if(tipo_pase == "pase_qx_urg"){
				Glade.XML gxml = new Glade.XML (null, "caja.glade", "pases_servicio_medico", null);
				gxml.Autoconnect (this);
				pases_servicio_medico.Show();
				entry_dia_inicio.Text = DateTime.Now.ToString("dd");
				entry_mes_inicio.Text = DateTime.Now.ToString("MM");
				entry_ano_inicio.Text = DateTime.Now.ToString("yyyy");			
				entry_dia_inicio.IsEditable = false;
				entry_mes_inicio.IsEditable = false;
				entry_ano_inicio.IsEditable = false;
					
				button_salir.Clicked += new EventHandler(on_cierraventanas_clicked);
				button_cancela_pase.Clicked += new EventHandler(on_cancela_pase_clicked);
	           	button_imprimir_pase.Clicked += new EventHandler(on_printing_pase_qx_clcked);
	          	button_grabar_pase.Clicked += new EventHandler(on_create_pases_qxurg_clicked);
	          	llenado_combobox(1,"",combobox_tipo_admision,"sql","SELECT * FROM osiris_his_tipo_admisiones " +
	          				"WHERE pase_servicio_medico = 'true' ORDER BY descripcion_admisiones;",
			                 "descripcion_admisiones","id_tipo_admisiones",args_args,args_id_array,"");
				if(foliocerrado){
					button_grabar_pase.Sensitive = false;
				}
				crea_treeview_pases();
				llenado_treeview_pases();				
			}
			if(tipo_pase == "pase_de_ingreso"){
				printing_pase();
			}
			if(tipo_pase == "cita_a_paciente"){
				printing_pasecita ();
			}
			if(tipo_pase == "pase_consulta_medica"){
				printing_pase_consulta();
			}
			if (tipo_pase == "pase_seccion_50") {
				printing_pase_seccion50();
			}
		}
		
		void llenado_combobox(int tipodellenado,string descrip_defaul,object obj,string sql_or_array,string query_SQL,string name_field_desc,string name_field_id,string[] args_array,int[] args_id_array,string name_field_id2)
		{			
			Gtk.ComboBox combobox_llenado = (Gtk.ComboBox) obj;
			//Gtk.ComboBox combobox_pos_neg = obj as Gtk.ComboBox;
			combobox_llenado.Clear();
			CellRendererText cell = new CellRendererText();
			combobox_llenado.PackStart(cell, true);
			combobox_llenado.AddAttribute(cell,"text",0);	        
			ListStore store = new ListStore( typeof (string),typeof (int),typeof (int));
			combobox_llenado.Model = store;			
			if ((int) tipodellenado == 1){
				store.AppendValues ((string) descrip_defaul,0,1);
			}			
			if(sql_or_array == "array"){			
				for (int colum_field = 0; colum_field < args_array.Length; colum_field++){
					store.AppendValues (args_array[colum_field],args_id_array[colum_field],1);
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
					//Console.WriteLine(comando.CommandText);
					NpgsqlDataReader lector = comando.ExecuteReader ();
	               	while (lector.Read()){
						if(name_field_id2 == ""){
							store.AppendValues ((string) lector[ name_field_desc ], (int) lector[ name_field_id],1);
						}else{
							store.AppendValues ((string) lector[ name_field_desc ], (int) lector[ name_field_id],(int) lector[ name_field_id2]);
						}
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
				case "combobox_tipo_admision":
					tipointernamiento = (string) onComboBoxChanged.Model.GetValue(iter,0);
					idtipointernamiento = (int) onComboBoxChanged.Model.GetValue(iter,1);
					//accesoserviciosdirecto = (int) combobox_tipo_admision.Model.GetValue(iter,2);
					break;
				}
			}
		}
		
		void on_cancela_pase_clicked(object sender, EventArgs args)
		{
			TreeModel model;
			TreeIter iterSelected;
			if (lista_pasesservmedico.Selection.GetSelected(out model, out iterSelected)){
				nro_de_pase_ingreso = (string) lista_pasesservmedico.Model.GetValue (iterSelected,0);				
				if((string) classpublic.lee_registro_de_tabla("osiris_empleado","acceso_eliminarpase_qxurg","WHERE acceso_eliminarpase_qxurg = 'true' AND login_empleado = '"+LoginEmpleado+"' ","acceso_eliminarpase_qxurg","bool") == "True"){
					Glade.XML gxml = new Glade.XML (null, "registro_admision.glade", "cancelador_folios", null);
					gxml.Autoconnect (this);
					cancelador_folios.Title = "ELIMINAR PASE";
					label247.Text = "N° de Pase ";
					cancelador_folios.Show();
					entry_folio.Text = nro_de_pase_ingreso;
					entry_folio.IsEditable = false;
					button_cancelar.Clicked += new EventHandler(on_button_cancelar_clicked);
					button_salir.Clicked += new EventHandler(on_cierraventanas_clicked);
				}else{
					//checkbutton_todos_envios.Active = false;
					MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
								MessageType.Error, ButtonsType.Close,"No esta autorizado para CANCELAR pases a Quirofano/Urgencias, Verifique...");
					msgBoxError.Run ();						msgBoxError.Destroy();
				}
			}			
		}
		
		void crea_treeview_pases()
		{
			treeViewEnginePases = new TreeStore(typeof(string),
												 typeof(string),
												 typeof(string),
												 typeof(string),
												 typeof(string),
												 typeof(string),
												 typeof(string),
												 typeof(string),
												 typeof(string),
												 typeof(string),
												 typeof(string),
												 typeof(string),
												 typeof(string),
												 typeof(string));
						
				lista_pasesservmedico.Model = treeViewEnginePases;
				lista_pasesservmedico.RulesHint = true;
				//lista_pasesservmedico.RowActivated += on_selecciona_producto_clicked;  // Doble click selecciono paciente*/
					
				TreeViewColumn col_idproducto = new TreeViewColumn();
				CellRendererText cellr0 = new CellRendererText();
				col_idproducto.Title = "N° Pase";
				col_idproducto.PackStart(cellr0, true);
				col_idproducto.AddAttribute (cellr0, "text", 0);
				col_idproducto.SortColumnId = (int) Column_prod.col_idproducto;
			
				TreeViewColumn col_fechahora = new TreeViewColumn();
				CellRendererText cellr1 = new CellRendererText();
				col_fechahora.Title = "Fech. Hora";
				col_fechahora.PackStart(cellr1, true);
				col_fechahora.AddAttribute (cellr1, "text", 1);
				col_fechahora.SortColumnId = (int) Column_prod.col_fechahora;
				
				TreeViewColumn col_idquiencreo = new TreeViewColumn();
				CellRendererText cellr2 = new CellRendererText();
				col_idquiencreo.Title = "Quien creo";
				col_idquiencreo.PackStart(cellr2, true);
				col_idquiencreo.AddAttribute (cellr2, "text", 2);
				col_idquiencreo.SortColumnId = (int) Column_prod.col_idquiencreo;
			
				TreeViewColumn col_tipodepase = new TreeViewColumn();
				CellRendererText cellr3 = new CellRendererText();
				col_tipodepase.Title = "Departamento";
				col_tipodepase.PackStart(cellr3, true);
				col_tipodepase.AddAttribute (cellr3, "text", 3);
				col_tipodepase.SortColumnId = (int) Column_prod.col_tipodepase;
			
				lista_pasesservmedico.AppendColumn(col_idproducto);
				lista_pasesservmedico.AppendColumn(col_fechahora);
				lista_pasesservmedico.AppendColumn(col_idquiencreo);
				lista_pasesservmedico.AppendColumn(col_tipodepase);
		}
		
		//  lista_de_productos:
		enum Column_prod
		{
			col_idproducto,
			col_fechahora,
			col_idquiencreo,
			col_tipodepase
		}
		
		void llenado_treeview_pases()
		{
			treeViewEnginePases.Clear();
			NpgsqlConnection conexion;
			conexion = new NpgsqlConnection (connectionString+nombrebd);
			try{
				conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand();
				comando.CommandText = "SELECT osiris_erp_pases_qxurg.id_secuencia,osiris_erp_pases_qxurg.folio_de_servicio AS foliodeservicio," +
							"to_char(osiris_erp_cobros_enca.pid_paciente,'9999999999') AS pidpaciente,"+
							"nombre1_paciente || ' ' || nombre2_paciente || ' ' || apellido_paterno_paciente || ' ' || apellido_materno_paciente AS nombre_completo,"+
							"to_char(osiris_his_paciente.fecha_nacimiento_paciente, 'dd-MM-yyyy') AS fechanacpaciente,"+
							"to_char(to_number(to_char(age('"+DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss")+"',osiris_his_paciente.fecha_nacimiento_paciente),'yyyy') ,'9999'),'9999') AS edadpaciente,"+
							"osiris_his_paciente.sexo_paciente,"+
							"osiris_erp_cobros_enca.nombre_medico_tratante,"+
							"osiris_erp_pases_qxurg.id_quien_creo,nombre1_empleado || ' ' || nombre2_empleado || ' ' || apellido_paterno_empleado || ' ' || apellido_materno_empleado AS nombresolicitante,"+
							"to_char(osiris_erp_pases_qxurg.fechahora_creacion,'yyyy-MM-dd HH24:mi:ss') AS fechahoracrea," +
							"osiris_erp_pases_qxurg.id_tipo_admisiones,descripcion_admisiones," +
							"osiris_erp_cobros_enca.id_empresa AS idempresa,osiris_empresas.descripcion_empresa,"+
							"osiris_erp_cobros_enca.id_aseguradora,osiris_aseguradoras.descripcion_aseguradora,"+
							 "id_quien_creo " +
						 	"FROM osiris_erp_pases_qxurg,osiris_his_tipo_admisiones,osiris_erp_cobros_enca,osiris_his_paciente,osiris_empleado,osiris_empresas,osiris_aseguradoras "+
							"WHERE osiris_erp_pases_qxurg.id_tipo_admisiones2 = osiris_his_tipo_admisiones.id_tipo_admisiones " +
							"AND osiris_erp_pases_qxurg.pid_paciente = osiris_his_paciente.pid_paciente "+
							"AND osiris_erp_pases_qxurg.folio_de_servicio = osiris_erp_cobros_enca.folio_de_servicio "+
							"AND osiris_erp_pases_qxurg.id_quien_creo = osiris_empleado.login_empleado "+
							"AND osiris_erp_cobros_enca.id_empresa = osiris_empresas.id_empresa "+
							"AND osiris_erp_cobros_enca.id_aseguradora = osiris_aseguradoras.id_aseguradora " +
							"AND osiris_erp_pases_qxurg.eliminado = 'false' "+
							"AND osiris_erp_pases_qxurg.folio_de_servicio = '"+ folioservicio.ToString().Trim() +"';";		
				//Console.WriteLine(comando.CommandText);
				NpgsqlDataReader lector = comando.ExecuteReader ();
				while(lector.Read()){
					treeViewEnginePases.AppendValues(lector["id_secuencia"].ToString().Trim(),
					                                 lector["fechahoracrea"].ToString().Trim(),
					                                 lector["id_quien_creo"],
					                                 lector["descripcion_admisiones"]);
				}
			}catch (NpgsqlException ex){
				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
							MessageType.Error, ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
				msgBoxError.Run ();		msgBoxError.Destroy();				
			}
			conexion.Close();			
		}

		void on_button_cancelar_clicked(object sender, EventArgs args)
		{
			MessageDialog msgBox = new MessageDialog (MyWin,DialogFlags.Modal,MessageType.Question,
				ButtonsType.YesNo,"¿ Esta seguro(a) de ELIMINAR este PASE ?");
			ResponseType miResultado = (ResponseType)msgBox.Run ();
			msgBox.Destroy();
			if(miResultado == ResponseType.Yes){
				string[,] parametros = {{ "eliminado = ", "'true'," },
					{ "fechahora_eliminado = ", "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'," },
					{ "motivo_eliminacion =", "'" + entry_motivo.Text.Trim().ToUpper() + "'," },
					{ "id_quien_elimino = ", "'" + LoginEmpleado + "' " },
					{ "WHERE id_secuencia = '", entry_folio.Text.Trim()+ "';" }
				};
				object[] paraobj = { entry_folio };
				new osiris.update_registro ("osiris_erp_pases_qxurg", parametros, paraobj);
			}
		}
		
		void on_create_pases_qxurg_clicked (object sender, EventArgs args)
		{
			if(idtipointernamiento > 0){
				if((string) classpublic.lee_registro_de_tabla("osiris_empleado","acceso_crearpase_qxurg","WHERE acceso_crearpase_qxurg = 'true' AND login_empleado = '"+LoginEmpleado+"' ","acceso_crearpase_qxurg","bool") == "True"){
						MessageDialog msgBox = new MessageDialog (MyWin,DialogFlags.Modal,
												MessageType.Question,ButtonsType.YesNo,"Esta seguro de crear un pase para un SERVICIO MEDICO");
						ResponseType miResultado = (ResponseType)msgBox.Run ();
						msgBox.Destroy();
					 	if (miResultado == ResponseType.Yes){
							string[,] parametros = {
								{ "pid_paciente,", "'" + pidpaciente.ToString().Trim() + "'," },
								{ "folio_de_servicio,", "'" + folioservicio.ToString().Trim() + "'," },
								{ "fechahora_creacion,", "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'," },
								{ "id_tipo_admisiones,", "'" + idcentro_costo.ToString().Trim() + "'," },
								{ "id_quien_creo,", "'" + LoginEmpleado + "'," },
								{ "observaciones,", "'" + "" + "'," },
								{ "id_tipo_paciente,", "'" + idtipopaciente.ToString().Trim() + "'," },
								{ "id_empresa,", "'" + idempresa_paciente.ToString().Trim() + "'," },
								{ "id_aseguradora,", "'" + idaseguradora_paciente.ToString().Trim() + "'," },
								{ "id_tipo_admisiones2", "'" + idtipointernamiento.ToString().Trim() + "' " }
							};
							object[] paraobj = { entry_folio };
							new osiris.insert_registro ("osiris_erp_pases_qxurg", parametros, paraobj);
							llenado_treeview_pases();
					 	}
				}else{
					MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
								MessageType.Error, ButtonsType.Close,"No esta autorizado para GENERAR pases a Quirofano/Urgencias, Verifique...");
					msgBoxError.Run ();						msgBoxError.Destroy();
				}
			}else{
				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
								MessageType.Error, ButtonsType.Close,"Debe Seleccionar que TIPO DE PASE para poder grabar, Verifique...");
				msgBoxError.Run ();						msgBoxError.Destroy();
			}
		}
		
		void on_printing_pase_qx_clcked(object sender, EventArgs args)
		{
			TreeModel model;
			TreeIter iterSelected;
			if((string) classpublic.lee_registro_de_tabla("osiris_empleado","acceso_imprimirpase_qxurg","WHERE acceso_imprimirpase_qxurg = 'true' AND login_empleado = '"+LoginEmpleado+"' ","acceso_imprimirpase_qxurg","bool") == "True"){
	 			if (lista_pasesservmedico.Selection.GetSelected(out model, out iterSelected)){
					nro_de_pase_ingreso = (string) lista_pasesservmedico.Model.GetValue (iterSelected,0);

					printing_paseqxurg();
				}
			}else{
				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
							MessageType.Error, ButtonsType.Close,"No esta autorizado para IMPRIMIR pases a Quirofano/Urgencias, Verifique...");
				msgBoxError.Run ();						msgBoxError.Destroy();
			}
		}
		
		void printing_pase_consulta()
		{
			
		}
		
		void printing_pase()
		{
			new osiris.rpt_pases_de_ingreso(folioservicio);	
		}

		void printing_pasecita()
		{
			new osiris.rpt_citas_agenda (folioservicio);
		}

		void printing_pase_seccion50()
		{
			new osiris.rpt_pases_de_seccion50 (folioservicio);
		}

		void printing_paseqxurg()
		{
			print = new PrintOperation ();			
			print.JobName = "Pase a Quirofano o Urgencias";
			print.BeginPrint += new BeginPrintHandler (OnBeginPrint);
			print.DrawPage += new DrawPageHandler (OnDrawPage);
			print.EndPrint += new EndPrintHandler (OnEndPrint);
			print.Run(PrintOperationAction.PrintDialog, null);
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
			ejecutar_consulta_reporte(context);			
		}
		
		void ejecutar_consulta_reporte(PrintContext context)
		{
			string sexopaciente = "";
			string empresa_o_aseguradora = "";
			string titulo_de_pase = "";
						
			Cairo.Context cr = context.CairoContext;
			Pango.Layout layout = context.CreatePangoLayout ();
						
			Pango.FontDescription desc = Pango.FontDescription.FromString ("Sans");									
			//cr.Rotate(90);  //Imprimir Orizontalmente rota la hoja cambian las posiciones de las lineas y columna					
			fontSize = 8.0;
			desc.Size = (int)(fontSize * pangoScale);					layout.FontDescription = desc;
			
			if(tipo_pase=="pase_qx_urg"){
				query_slq = "SELECT osiris_erp_pases_qxurg.id_secuencia,osiris_erp_pases_qxurg.folio_de_servicio AS foliodeservicio," +
							"to_char(osiris_erp_cobros_enca.pid_paciente,'9999999999') AS pidpaciente,"+
							"nombre1_paciente || ' ' || nombre2_paciente || ' ' || apellido_paterno_paciente || ' ' || apellido_materno_paciente AS nombre_completo,"+
							"to_char(osiris_his_paciente.fecha_nacimiento_paciente, 'dd-MM-yyyy') AS fechanacpaciente,"+
							"to_char(to_number(to_char(age('"+DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss")+"',osiris_his_paciente.fecha_nacimiento_paciente),'yyyy') ,'9999'),'9999') AS edadpaciente,"+
							"osiris_his_paciente.sexo_paciente,"+
							"osiris_erp_cobros_enca.nombre_medico_tratante,"+
							"osiris_erp_pases_qxurg.id_quien_creo,nombre1_empleado || ' ' || nombre2_empleado || ' ' || apellido_paterno_empleado || ' ' || apellido_materno_empleado AS nombresolicitante,"+
							"to_char(osiris_erp_pases_qxurg.fechahora_creacion,'yyyy-MM-dd HH24:mi:ss') AS fechahoracrea," +
							"osiris_erp_pases_qxurg.id_tipo_admisiones,descripcion_admisiones," +
							"osiris_erp_cobros_enca.id_empresa AS idempresa,osiris_empresas.descripcion_empresa,"+
							"osiris_erp_cobros_enca.id_aseguradora,osiris_aseguradoras.descripcion_aseguradora,"+
							 "id_quien_creo " +
						 	"FROM osiris_erp_pases_qxurg,osiris_his_tipo_admisiones,osiris_erp_cobros_enca,osiris_his_paciente,osiris_empleado,osiris_empresas,osiris_aseguradoras "+
							"WHERE osiris_erp_pases_qxurg.id_tipo_admisiones2 = osiris_his_tipo_admisiones.id_tipo_admisiones " +
							"AND osiris_erp_pases_qxurg.pid_paciente = osiris_his_paciente.pid_paciente "+
							"AND osiris_erp_pases_qxurg.folio_de_servicio = osiris_erp_cobros_enca.folio_de_servicio "+
							"AND osiris_erp_pases_qxurg.id_quien_creo = osiris_empleado.login_empleado "+
							"AND osiris_erp_cobros_enca.id_empresa = osiris_empresas.id_empresa "+
							"AND osiris_erp_pases_qxurg.eliminado = 'false' "+
							"AND osiris_erp_pases_qxurg.id_secuencia = '"+nro_de_pase_ingreso+"' "+
							"AND osiris_erp_cobros_enca.id_aseguradora = osiris_aseguradoras.id_aseguradora "+
							"AND osiris_erp_pases_qxurg.folio_de_servicio = '"+ folioservicio.ToString().Trim() +"';";	 ; //    query_sql + "AND osiris_erp_pases_qxurg.folio_de_servicio = '"+ folioservicio.ToString().Trim() +"';";
				titulo_de_pase = "PASE_SERVICIO_MEDICO_";
			}
			if(tipo_pase == "pase_qx_urg"){
				NpgsqlConnection conexion; 
		        conexion = new NpgsqlConnection (connectionString+nombrebd);
		        // Verifica que la base de datos este conectada
		        try{
		 			conexion.Open ();
		        	NpgsqlCommand comando; 
		        	comando = conexion.CreateCommand (); 
		           	comando.CommandText = query_slq;
		        	//Console.WriteLine(comando.CommandText);
					NpgsqlDataReader lector = comando.ExecuteReader();
					if (lector.Read()){
						if (lector["sexo_paciente"].ToString().Trim() == "H"){
							sexopaciente = "MASCULINO";
						}else{
							sexopaciente = "FEMENINO";
						}					
						if((int) lector ["id_aseguradora"] > 1){
							empresa_o_aseguradora = (string) lector["descripcion_aseguradora"];
						}else{
							empresa_o_aseguradora = (string) lector["descripcion_empresa"];						
						}
						buscar_en_movcargos(lector["foliodeservicio"].ToString().Trim());
						imprime_encabezado(cr,
						                   layout,
						                   lector["descripcion_admisiones"].ToString().Trim(),
						                   lector["id_secuencia"].ToString().Trim(),
						                   lector["fechahoracrea"].ToString().Trim(),
						               		lector["foliodeservicio"].ToString().Trim(),
						                   lector["pidpaciente"].ToString().Trim(),
						                   lector["nombre_completo"].ToString().Trim(),
						               		lector["fechanacpaciente"].ToString().Trim(),
						                   lector["edadpaciente"].ToString().Trim(),
						                   sexopaciente,
						               		diagnostico_movcargo,
						                   nombrecirugia_movcargo,
						                   lector["nombre_medico_tratante"].ToString().Trim(),
						                   "",
						               		lector["id_quien_creo"].ToString().Trim(),
						                   lector["nombresolicitante"].ToString().Trim(),
						              	 	descripciontipopaciente,
						                 empresa_o_aseguradora,titulo_de_pase);
					}				
				}catch (NpgsqlException ex){
					MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
						MessageType.Warning, ButtonsType.Ok, "PostgresSQL error: {0}",ex.Message);
						msgBoxError.Run ();
						msgBoxError.Destroy();
					Console.WriteLine ("PostgresSQL error: {0}",ex.Message);
					return; 
				}
				conexion.Close();
			}
		}
		
		void imprime_encabezado(Cairo.Context cr,Pango.Layout layout,string areaquiensolicita,string numerosolicitud,string fechasolicitud, 
		                    string numerodeatencion, string numeroexpediente, string nombrepaciente, string fechanacimiento, string edadpaciente, 
		                    string sexodelpaciente, string descripciondiagnostico, string nombredecirugia, string medicotratante, string numerohabitacion,
		                    string quiensolicito, string nomsolicitante, string tipo_paciente,string empresa_aseguradora,string titulo_de_pase)
		{
			//Gtk.Image image5 = new Gtk.Image();
            //image5.Name = "image5";
			//image5.Pixbuf = new Gdk.Pixbuf(System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "osiris.jpg"));
			//image5.Pixbuf = new Gdk.Pixbuf("/opt/osiris/bin/OSIRISLogo.jpg");   // en Linux
			//---image5.Pixbuf.ScaleSimple(128, 128, Gdk.InterpType.Bilinear);
			//---Gdk.CairoHelper.SetSourcePixbuf(cr,image5.Pixbuf,1,-30);
			//---Gdk.CairoHelper.SetSourcePixbuf(cr,image5.Pixbuf.ScaleSimple(145, 50, Gdk.InterpType.Bilinear),1,1);
			//Gdk.CairoHelper.SetSourcePixbuf(cr,image5.Pixbuf.ScaleSimple(180, 64, Gdk.InterpType.Hyper),1,1);
			//cr.Fill();
			//cr.Paint();
			//cr.Restore();
			int comienzo_linea_interno = 0;
			comienzo_linea = 60;
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
			cr.MoveTo(479*escala_en_linux_windows,05*escala_en_linux_windows);			layout.SetText("Fech.Rpt:"+(string) DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));		Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(479*escala_en_linux_windows,15*escala_en_linux_windows);			layout.SetText("N° Page :"+numpage.ToString().Trim());		Pango.CairoHelper.ShowLayout (cr, layout);

			cr.MoveTo(05*escala_en_linux_windows,35*escala_en_linux_windows);			layout.SetText("Sistema Hospitalario OSIRIS");		Pango.CairoHelper.ShowLayout (cr, layout);
			// Cambiando el tamaño de la fuente			
			fontSize = 12.0;		
			desc.Size = (int)(fontSize * pangoScale);					layout.FontDescription = desc;
			layout.FontDescription.Weight = Weight.Bold;		// Letra negrita
			layout.Alignment = Pango.Alignment.Center;
			
			double width = context.Width;
			layout.Width = (int) width;
			layout.Alignment = Pango.Alignment.Center;
			//layout.Wrap = Pango.WrapMode.Word;
			//layout.SingleParagraphMode = true;
			layout.Justify =  false;
			if(tipo_pase == "pase_qx_urg"){
				cr.MoveTo(width/2,45*escala_en_linux_windows);	layout.SetText(titulo_de_pase+areaquiensolicita);	Pango.CairoHelper.ShowLayout (cr, layout);
			}
			
			fontSize = 9.0;			layout = null;			layout = context.CreatePangoLayout ();
			desc.Size = (int)(fontSize * pangoScale);		layout.FontDescription = desc;
			layout.FontDescription.Weight = Weight.Bold;		// Letra negrita						
			desc = Pango.FontDescription.FromString ("Sans");									 
			//cr.Rotate(90);  //Imprimir Orizontalmente rota la hoja cambian las posiciones de las lineas y columna					
			fontSize = 8.0;
			desc.Size = (int)(fontSize * pangoScale);					layout.FontDescription = desc;
			cr.MoveTo(370*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);		layout.SetText("Fecha Creacion: "+fechasolicitud);						Pango.CairoHelper.ShowLayout (cr, layout);
			comienzo_linea += separacion_linea;
			layout.FontDescription.Weight = Weight.Bold;		// Letra negrita
			if(tipo_pase == "pase_qx_urg"){
				cr.MoveTo(05*escala_en_linux_windows,comienzo_linea-separacion_linea*escala_en_linux_windows);		layout.SetText("N° de Pase: "+numerosolicitud);			Pango.CairoHelper.ShowLayout (cr, layout);
			}
			cr.MoveTo(05*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);		layout.SetText("N° Atencion: "+numerodeatencion);	Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(120*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);		layout.SetText("N° Expe.: "+numeroexpediente);		Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(220*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);		layout.SetText("Nombre Paciente: "+nombrepaciente);	Pango.CairoHelper.ShowLayout (cr, layout);
			comienzo_linea += separacion_linea;
			cr.MoveTo(05*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);		layout.SetText("Fecha Nacimiento: "+fechanacimiento);	Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(250*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);		layout.SetText("Edad: "+edadpaciente+" Años");				Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(400*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);		layout.SetText("Sexo: "+sexodelpaciente);			Pango.CairoHelper.ShowLayout (cr, layout);
			layout.FontDescription.Weight = Weight.Normal;
			comienzo_linea += separacion_linea;
			cr.MoveTo(05*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);		layout.SetText("Procedimiento: "+nombredecirugia);	Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(300*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);		layout.SetText("Motivo de Ingreso: "+descripciondiagnostico);	Pango.CairoHelper.ShowLayout (cr, layout);
			comienzo_linea += separacion_linea;
			cr.MoveTo(05*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);		layout.SetText("Medico Tratante: "+medicotratante);	Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(400*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);		layout.SetText("Habitacion: "+numerohabitacion);		Pango.CairoHelper.ShowLayout (cr, layout);
			comienzo_linea += separacion_linea;
			cr.MoveTo(05*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);		layout.SetText("Usuario: "+quiensolicito);							Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(200*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);		layout.SetText("Nom. Usuario: "+nomsolicitante);					Pango.CairoHelper.ShowLayout (cr, layout);
			comienzo_linea += separacion_linea;
			layout.FontDescription.Weight = Weight.Bold;		// Letra negrita
			cr.MoveTo(05*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);		layout.SetText("Tipo de Paciente : "+tipo_paciente);			Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(240*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);		layout.SetText("Convenio : "+empresa_aseguradora);							Pango.CairoHelper.ShowLayout (cr, layout);
			//cr.MoveTo(410*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);		layout.SetText("Ref. : "+empresa_aseguradora);							Pango.CairoHelper.ShowLayout (cr, layout);
			layout.FontDescription.Weight = Weight.Normal;		// Letra normal
			comienzo_linea += separacion_linea;
			comienzo_linea_interno = comienzo_linea;
			if(tipo_pase == "pase_qx_urg"){
				cr.MoveTo(10*escala_en_linux_windows,(comienzo_linea+(separacion_linea*2))*escala_en_linux_windows);		layout.SetText("Cirugia :___________________________________________");							Pango.CairoHelper.ShowLayout (cr, layout);						
				cr.MoveTo(10*escala_en_linux_windows,(comienzo_linea+(separacion_linea*4))*escala_en_linux_windows);		layout.SetText("Cirujano :____________________________________________");							Pango.CairoHelper.ShowLayout (cr, layout);						
				cr.MoveTo(10*escala_en_linux_windows,(comienzo_linea+(separacion_linea*6))*escala_en_linux_windows);		layout.SetText("Ayudante :____________________________________________");							Pango.CairoHelper.ShowLayout (cr, layout);						
				cr.MoveTo(10*escala_en_linux_windows,(comienzo_linea+(separacion_linea*8))*escala_en_linux_windows);		layout.SetText("Anestesiologo :______________________________________");							Pango.CairoHelper.ShowLayout (cr, layout);						
				cr.MoveTo(10*escala_en_linux_windows,(comienzo_linea+(separacion_linea*10))*escala_en_linux_windows);		layout.SetText("Tipo de Anestesia :______________________________________");							Pango.CairoHelper.ShowLayout (cr, layout);						
				cr.MoveTo(10*escala_en_linux_windows,(comienzo_linea+(separacion_linea*12))*escala_en_linux_windows);		layout.SetText("Nom. Proveedor 1 :___________________________________________");							Pango.CairoHelper.ShowLayout (cr, layout);						
				cr.MoveTo(10*escala_en_linux_windows,(comienzo_linea+(separacion_linea*14))*escala_en_linux_windows);		layout.SetText("Nom. Proveedor 2 :___________________________________________");							Pango.CairoHelper.ShowLayout (cr, layout);						
				cr.MoveTo(10*escala_en_linux_windows,(comienzo_linea+(separacion_linea*16))*escala_en_linux_windows);		layout.SetText("Materiales y/o Equipos :");							Pango.CairoHelper.ShowLayout (cr, layout);						
		
				cr.MoveTo(430*escala_en_linux_windows,(comienzo_linea+(separacion_linea*2))*escala_en_linux_windows);		layout.SetText("Sello de Dep. Medico");							Pango.CairoHelper.ShowLayout (cr, layout);						
				cr.MoveTo(150*escala_en_linux_windows,(comienzo_linea+(separacion_linea*21))*escala_en_linux_windows);		layout.SetText("Sello y Firma Cajero");							Pango.CairoHelper.ShowLayout (cr, layout);
			}
			fontSize = 6.5;
			desc.Size = (int)(fontSize * pangoScale);					layout.FontDescription = desc;
			layout.FontDescription.Weight = Weight.Bold;		// Letra negrita
			cr.MoveTo(405*escala_en_linux_windows,(comienzo_linea_interno+(separacion_linea*19))*escala_en_linux_windows);		layout.SetText(nombrepaciente);							Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(405*escala_en_linux_windows,(comienzo_linea_interno+(separacion_linea*20))*escala_en_linux_windows);		layout.SetText("Edad: "+edadpaciente+" Años");				Pango.CairoHelper.ShowLayout (cr, layout);
			if(medicotratante != ""){
				cr.MoveTo(405*escala_en_linux_windows,(comienzo_linea_interno+(separacion_linea*21))*escala_en_linux_windows);		layout.SetText("Dr. "+medicotratante);				Pango.CairoHelper.ShowLayout (cr, layout);
			}
			cr.MoveTo(405*escala_en_linux_windows,(comienzo_linea_interno+(separacion_linea*22))*escala_en_linux_windows);		layout.SetText("Fecha: "+(string) DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));				Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(005*escala_en_linux_windows,(comienzo_linea_interno+(separacion_linea*22))*escala_en_linux_windows);		layout.SetText("Nombre y Firma Admisión ");				Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(140*escala_en_linux_windows,(comienzo_linea_interno+(separacion_linea*22))*escala_en_linux_windows);		layout.SetText("Nombre y Firma Paciente ");				Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(270*escala_en_linux_windows,(comienzo_linea_interno+(separacion_linea*22))*escala_en_linux_windows);		layout.SetText("Nombre y Firma Autorización ");				Pango.CairoHelper.ShowLayout (cr, layout);

			layout.FontDescription.Weight = Weight.Normal;		// Letra normal
			fontSize = 8.0;
			desc.Size = (int)(fontSize * pangoScale);					layout.FontDescription = desc;
			//Console.WriteLine(comienzo_linea.ToString());
			cr.Rectangle (05*escala_en_linux_windows, comienzo_linea_interno*escala_en_linux_windows, 565*escala_en_linux_windows, (separacion_linea*18)*escala_en_linux_windows);
			
			if(tipo_pase == "pase_qx_urg"){
				// Linea Vertical
				cr.MoveTo(400*escala_en_linux_windows, comienzo_linea_interno*escala_en_linux_windows);
				cr.LineTo(400*escala_en_linux_windows,330);
			}
						
			cr.FillExtents();  //. FillPreserve(); 
			cr.SetSourceRGB (0, 0, 0);
			cr.LineWidth = 0.3;
			cr.Stroke();			
		}
		
		void buscar_en_movcargos(string foliodeservicio)
		{
			NpgsqlConnection conexion; 
	        conexion = new NpgsqlConnection (connectionString+nombrebd);
	        // Verifica que la base de datos este conectada
	        try{
	 			conexion.Open ();
	        	NpgsqlCommand comando; 
	        	comando = conexion.CreateCommand (); 
	           	comando.CommandText = "SELECT DISTINCT (osiris_erp_movcargos.folio_de_servicio),id_tipo_admisiones,osiris_erp_movcargos.id_tipo_paciente," +
	           		"pid_paciente,descripcion_tipo_paciente,descripcion_diagnostico_movcargos,nombre_de_cirugia "+
					"FROM osiris_erp_movcargos,osiris_his_tipo_pacientes "+
					"WHERE osiris_erp_movcargos.id_tipo_paciente = osiris_his_tipo_pacientes.id_tipo_paciente "+
						"AND osiris_erp_movcargos.folio_de_servicio = '"+foliodeservicio+"';";
	        	//Console.WriteLine(comando.CommandText);
				NpgsqlDataReader lector = comando.ExecuteReader();
				if (lector.Read()){
					diagnostico_movcargo = lector["descripcion_diagnostico_movcargos"].ToString().Trim();
					nombrecirugia_movcargo = lector["nombre_de_cirugia"].ToString().Trim();
					descripciontipopaciente = lector["descripcion_tipo_paciente"].ToString().Trim().ToUpper();
				}
			}catch (NpgsqlException ex){
				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
					MessageType.Warning, ButtonsType.Ok, "PostgresSQL error: {0}",ex.Message);
					msgBoxError.Run ();
					msgBoxError.Destroy();
				Console.WriteLine ("PostgresSQL error: {0}",ex.Message);
			}
			conexion.Close();
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
	}

	public class rpt_pases_de_ingreso
	{
		// Declarando variable publicas
		string connectionString;
		string nombrebd;
		
		class_conexion conexion_a_DB = new class_conexion();
		class_public classpublic = new class_public();

		protected Gtk.Window MyWinError;

		public rpt_pases_de_ingreso(int folioservicio_)
		{
			connectionString = conexion_a_DB._url_servidor+conexion_a_DB._port_DB+conexion_a_DB._usuario_DB+conexion_a_DB._passwrd_user_DB;
			nombrebd = conexion_a_DB._nombrebd;			
			string sexopaciente = "";
			string empresa_o_aseguradora = "";
			string titulo_de_pase = "";
			int comienzo_linea = 1;
			string query_slq = "SELECT osiris_erp_cobros_enca.folio_de_servicio AS id_secuencia,osiris_erp_cobros_enca.folio_de_servicio AS foliodeservicio," +
					"to_char(osiris_erp_cobros_enca.pid_paciente,'9999999999') AS pidpaciente,"+
					"nombre1_paciente || ' ' || nombre2_paciente || ' ' || apellido_paterno_paciente || ' ' || apellido_materno_paciente AS nombre_completo,"+
					"to_char(osiris_his_paciente.fecha_nacimiento_paciente, 'dd-MM-yyyy') AS fechanacpaciente,"+
					"to_char(to_number(to_char(age('"+DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss")+"',osiris_his_paciente.fecha_nacimiento_paciente),'yyyy') ,'9999'),'9999') AS edadpaciente,"+
					"osiris_his_paciente.sexo_paciente,"+
					"direccion_paciente,numero_casa_paciente,numero_departamento_paciente,colonia_paciente,codigo_postal_paciente,"+
					"osiris_erp_cobros_enca.nombre_medico_tratante,osiris_erp_cobros_enca.nombre_medico_encabezado,"+
					"osiris_erp_cobros_enca.id_empleado_admision,nombre1_empleado || ' ' || nombre2_empleado || ' ' || apellido_paterno_empleado || ' ' || apellido_materno_empleado AS nombresolicitante,"+
					"to_char(osiris_erp_cobros_enca.fechahora_creacion,'yyyy-MM-dd HH24:mi:ss') AS fechahoracrea," +
					"osiris_erp_movcargos.id_tipo_admisiones,descripcion_admisiones," +
					"osiris_erp_cobros_enca.id_empresa AS idempresa,osiris_empresas.descripcion_empresa,"+
					"osiris_erp_cobros_enca.id_aseguradora,osiris_aseguradoras.descripcion_aseguradora,"+
					"id_empleado_admision AS id_quien_creo,descripcion_diagnostico_movcargos,nombre_de_cirugia,descripcion_tipo_paciente " +
					"FROM osiris_erp_movcargos,osiris_his_tipo_admisiones,osiris_erp_cobros_enca,osiris_his_paciente,osiris_empleado,osiris_empresas,osiris_aseguradoras,osiris_his_tipo_pacientes "+
					"WHERE " +
					"osiris_erp_movcargos.id_tipo_admisiones = osiris_his_tipo_admisiones.id_tipo_admisiones " +
					"AND osiris_erp_cobros_enca.pid_paciente = osiris_his_paciente.pid_paciente "+
					"AND osiris_erp_cobros_enca.id_tipo_paciente = osiris_his_tipo_pacientes.id_tipo_paciente "+
					"AND osiris_erp_movcargos.folio_de_servicio = osiris_erp_cobros_enca.folio_de_servicio "+
					"AND osiris_erp_cobros_enca.id_empleado_admision = osiris_empleado.login_empleado "+
					"AND osiris_erp_cobros_enca.id_empresa = osiris_empresas.id_empresa "+
					"AND osiris_erp_cobros_enca.id_aseguradora = osiris_aseguradoras.id_aseguradora "+
					"AND osiris_erp_cobros_enca.folio_de_servicio = '"+ folioservicio_.ToString().Trim() +"';";
			titulo_de_pase = "PASE_DE_INGRESO";
			NpgsqlConnection conexion; 
			conexion = new NpgsqlConnection (connectionString+nombrebd);
			// Verifica que la base de datos este conectada
			try{
				conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand (); 
				comando.CommandText = query_slq;
				//Console.WriteLine(comando.CommandText);
				NpgsqlDataReader lector = comando.ExecuteReader();
				if (lector.Read()){
					//buscar_en_movcargos(lector["foliodeservicio"].ToString().Trim());
					if (lector["sexo_paciente"].ToString().Trim() == "H"){
						sexopaciente = "MASCULINO";
					}else{
						sexopaciente = "FEMENINO";
					}					
					if((int) lector ["id_aseguradora"] > 1){
						empresa_o_aseguradora = (string) lector["descripcion_aseguradora"];
					}else{
						empresa_o_aseguradora = (string) lector["descripcion_empresa"];						
					}

					iTextSharp.text.Font _NormalFont;
					iTextSharp.text.Font _NormalFont8;
					iTextSharp.text.Font _BoldFont;
					_NormalFont = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 7, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);
					_NormalFont8 = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 8, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);
					_BoldFont = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 7, iTextSharp.text.Font.BOLD, BaseColor.BLACK);

					string pdf_name = System.IO.Path.GetTempPath() + Guid.NewGuid().ToString() + ".pdf";

					// step 1: creation of a document-object
					Document documento = new Document(iTextSharp.text.PageSize.LETTER, 15, 15, 15, 15);
					//Document document = new Document(PageSize.LETTER.Rotate());
					try{
						PdfWriter writerpdf = PdfWriter.GetInstance(documento, new FileStream(pdf_name, FileMode.Create));
						documento.AddTitle("Comprobante de Pago en Caja");
						documento.AddCreator("Sistema Hospitalario OSIRIS");
						documento.AddAuthor("Sistema Hospitalario OSIRIS");
						documento.AddSubject("OSIRSrpt");
						EventoTitulos_abonopago ev = new EventoTitulos_abonopago();
						ev.titulo1_rpt = "PASE DE INGRESO";
						ev.tipo_admision_px = lector["descripcion_admisiones"].ToString().Trim();						
						ev.fecha_ingresso_px =  lector["fechahoracrea"].ToString().Trim();
						ev.numero_atencion_px = lector["foliodeservicio"].ToString().Trim();
						ev.nro_expediente_px = lector["pidpaciente"].ToString().Trim();
						ev.nombres_apellidos_px = lector["nombre_completo"].ToString().Trim();
						ev.fecha_nacimiento_px = lector["fechanacpaciente"].ToString().Trim();
						ev.edad_px = lector["edadpaciente"].ToString().Trim();
						ev.sexo_px = sexopaciente;
						ev.direccion_px = lector["direccion_paciente"].ToString().Trim()+" "+lector["numero_casa_paciente"].ToString().Trim()+lector["numero_departamento_paciente"].ToString().Trim()+" "+lector["colonia_paciente"].ToString().Trim()+" CP. "+lector["codigo_postal_paciente"].ToString().Trim();
						ev.motivodeingreso = lector["descripcion_diagnostico_movcargos"].ToString().Trim();
						ev.nombremedico_px = lector["nombre_medico_encabezado"].ToString().Trim();
						ev.tipo_paciente_px = lector["descripcion_tipo_paciente"].ToString().Trim().ToUpper();
						ev.convenio_px = empresa_o_aseguradora;
						ev.idusuario = lector["id_quien_creo"].ToString().Trim();
						ev.nombredeusuario = lector["nombresolicitante"].ToString().Trim();

						writerpdf.PageEvent = ev;
						documento.Open();

						PdfPCell cellcol1;
						PdfPCell cellcol2;

						PdfPTable tabFot1 = new PdfPTable(2);
						tabFot1.WidthPercentage = 100;
						float[] widths_tabfot1 = new float[] { 45f, 190f };	// controlando el ancho de cada columna tienen que sumas 315 en total
						tabFot1.SetWidths(widths_tabfot1);
						tabFot1.HorizontalAlignment = 0;
						cellcol1 = new PdfPCell(new Phrase("Documento",_BoldFont));
						cellcol1.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
						cellcol1.BackgroundColor = BaseColor.YELLOW;
						cellcol2 = new PdfPCell(new Phrase("Observacion",_BoldFont));
						cellcol2.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
						cellcol2.BackgroundColor = BaseColor.YELLOW;
						cellcol2.HorizontalAlignment = 0;
						tabFot1.AddCell(cellcol1);
						tabFot1.AddCell(cellcol2);

						NpgsqlConnection conexion1; 
						conexion1 = new NpgsqlConnection (connectionString+nombrebd);
						// Verifica que la base de datos este conectada
						try{							
							conexion1.Open ();
							NpgsqlCommand comando1; 
							comando1 = conexion1.CreateCommand (); 
							comando1.CommandText = "SELECT * FROM osiris_erp_movimiento_documentos WHERE folio_de_servicio ='"+folioservicio_.ToString().Trim()+"'; ";
							//Console.WriteLine(comando1.CommandText);
							NpgsqlDataReader lector1 = comando1.ExecuteReader();					
							while(lector1.Read()){
								cellcol1 = new PdfPCell(new Phrase(lector1 ["descripcion_documento"].ToString(),_NormalFont));
								cellcol1.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
								cellcol2 = new PdfPCell(new Phrase(lector1 ["informacion_capturada"].ToString(),_NormalFont));
								cellcol2.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
								cellcol2.HorizontalAlignment = 0;
								tabFot1.AddCell(cellcol1);
								tabFot1.AddCell(cellcol2);									
							}
							documento.Add(tabFot1);
						}catch (NpgsqlException ex){
							MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
									MessageType.Warning, ButtonsType.Ok, "PostgresSQL error: {0}",ex.Message);
							msgBoxError.Run ();					msgBoxError.Destroy();
							Console.WriteLine ("PostgresSQL error: {0}",ex.Message);
						}
						conexion1.Close();

						if (classpublic.nombre_empresa != "MUNICIPIO DE ESCOBEDO") {
							string autorizacion_txt = "Autorizo que se me practique en "+classpublic.nombre_empresa2+" cuantas curaciones, procedimientos," +
								"diagnósticos, tratamientos médicos y operaciones quirúrgicas requiera, derivados de mi actual estado de salud.\n" +
								"Acepto y autorizo expresamente al personal de la salud responsable de mi caso, quien apegandose a los provilegios clínicos" +
								"(autorizados para ejercer su especialidad dentro de la Clinica/Hospital), otorgados por "+classpublic.nombre_empresa2+" " +
								"practique u ordene cuanto examen, reconocimiento, curacion, procedimiento, diagnostico, tratamiento medico o intervecion"+
								"quirúrgica sea necesaria. Así mismo acepto, para que se solicite interconsulta o colaboracion del otro(s) médicos(s) que tengan " +
								"privilegios en "+classpublic.nombre_empresa2+" necesario(s) para la atencion a mi padecimiento o de cualquier consecuencia del mismo;" +
								"aceptando desde ahora, todos y cada uno de los riesgos implicitos a cuanto examen, reconocimiento, curacion, procedimiento," +
								"diagnostico, tratamiento medico o intervecion quírurgica, a lo(s) que acepto ser sometido(a).\n \n \n \n" +
								"___________________________________ \n" +
								"Nombre y firma del paciente o Tutor" ;
							documento.Add (new Paragraph (" ", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 5)));
							PdfPTable tabFot7 = new PdfPTable (1);
							tabFot7.WidthPercentage = 100;
							float[] widths_tabfot7 = new float[] { 150f };	// controlando el ancho de cada columna tienen que sumas 315 en total
							tabFot7.SetWidths (widths_tabfot7);
							tabFot7.HorizontalAlignment = 3;
							cellcol1 = new PdfPCell (new Phrase (autorizacion_txt,_NormalFont8));
							cellcol1.Border = iTextSharp.text.Rectangle.NO_BORDER;
							cellcol1.HorizontalAlignment = 3;
							tabFot7.AddCell (cellcol1);                
							documento.Add (tabFot7);
						}

						//if (classpublic.nombre_empresa == "MUNICIPIO DE ESCOBEDO" || classpublic.nombre_empresa == "HUMAN HOSPITAL") {
							documento.Add (new Paragraph (" ", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 5)));
							PdfPTable tabFot8 = new PdfPTable (1);
							tabFot8.WidthPercentage = 100;
							float[] widths_tabfot8 = new float[] { 150f };	// controlando el ancho de cada columna tienen que sumas 315 en total
							tabFot8.SetWidths (widths_tabfot8);
							tabFot8.HorizontalAlignment = 0;
							cellcol1 = new PdfPCell (new Phrase ("  ", _BoldFont));
							cellcol1.Border = iTextSharp.text.Rectangle.NO_BORDER;
							cellcol1.HorizontalAlignment = 0;
							tabFot8.AddCell (cellcol1);                
							documento.Add (tabFot8);

							PdfPTable tabFot9 = new PdfPTable (1);
							tabFot9.WidthPercentage = 100;
							float[] widths_tabfot9 = new float[] { 150f };	// controlando el ancho de cada columna tienen que sumas 315 en total
							tabFot9.SetWidths (widths_tabfot9);
							tabFot9.HorizontalAlignment = 0;
							cellcol1 = new PdfPCell (new Phrase (" ", _BoldFont));

							cellcol1.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
							cellcol1.HorizontalAlignment = 0;
							tabFot9.AddCell (cellcol1);
							documento.Add (tabFot9);

							PdfPTable tabFot10 = new PdfPTable (1);
							tabFot10.WidthPercentage = 100;
							float[] widths_tabfot10 = new float[] { 150f };	// controlando el ancho de cada columna tienen que sumas 315 en total
							tabFot10.SetWidths (widths_tabfot10);
							tabFot10.HorizontalAlignment = 0;
							cellcol1 = new PdfPCell (new Phrase (" Presion Arterial:_____________________________ mm de Hg                   Frecuencia Cardiaca: _______________________latidos/min.", _BoldFont));
							cellcol1.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
							cellcol1.HorizontalAlignment = 0;
							tabFot10.AddCell (cellcol1);
							documento.Add (tabFot10);

							PdfPTable tabFot11 = new PdfPTable (1);
							tabFot11.WidthPercentage = 100;
							float[] widths_tabfot11 = new float[] { 150f };	// controlando el ancho de cada columna tienen que sumas 315 en total
							tabFot11.SetWidths (widths_tabfot11);
							tabFot11.HorizontalAlignment = 0;
							cellcol1 = new PdfPCell (new Phrase (" ", _BoldFont));
							cellcol1.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
							cellcol1.HorizontalAlignment = 0;
							tabFot11.AddCell (cellcol1);
							documento.Add (tabFot11);

							PdfPTable tabFot12 = new PdfPTable (1);
							tabFot12.WidthPercentage = 100;
							float[] widths_tabfot12 = new float[] { 150f };	// controlando el ancho de cada columna tienen que sumas 315 en total
							tabFot12.SetWidths (widths_tabfot12);
							tabFot12.HorizontalAlignment = 0;
							cellcol1 = new PdfPCell (new Phrase (" Frecuencia Respiratoria:_________________________X min                     Temperatura_____________°C", _BoldFont));
							cellcol1.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
							cellcol1.HorizontalAlignment = 0;
							tabFot12.AddCell (cellcol1);
							documento.Add (tabFot12);

							PdfPTable tabFot13 = new PdfPTable (1);
							tabFot13.WidthPercentage = 100;
							float[] widths_tabfot13 = new float[] { 150f };	// controlando el ancho de cada columna tienen que sumas 315 en total
							tabFot13.SetWidths (widths_tabfot13);
							tabFot13.HorizontalAlignment = 0;
							cellcol1 = new PdfPCell (new Phrase (" ", _BoldFont));
							cellcol1.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
							cellcol1.HorizontalAlignment = 0;
							tabFot13.AddCell (cellcol1);
							documento.Add (tabFot13);

							PdfPTable tabFot14 = new PdfPTable (1);
							tabFot14.WidthPercentage = 100;
							float[] widths_tabfot14 = new float[] { 150f };	// controlando el ancho de cada columna tienen que sumas 315 en total
							tabFot14.SetWidths (widths_tabfot14);
							tabFot14.HorizontalAlignment = 0;
							cellcol1 = new PdfPCell (new Phrase (" Talla:___________________cm.                                                                      Peso:_______________Kg.", _BoldFont));
							cellcol1.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
							cellcol1.HorizontalAlignment = 0;
							tabFot14.AddCell (cellcol1);
							documento.Add (tabFot14);

							PdfPTable tabFot15 = new PdfPTable (1);
							tabFot15.WidthPercentage = 100;
							float[] widths_tabfot15 = new float[] { 150f };	// controlando el ancho de cada columna tienen que sumas 315 en total
							tabFot15.SetWidths (widths_tabfot15);
							tabFot15.HorizontalAlignment = 0;
							cellcol1 = new PdfPCell (new Phrase (" ", _BoldFont));
							cellcol1.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
							cellcol1.HorizontalAlignment = 0;
							tabFot15.AddCell (cellcol1);
							documento.Add (tabFot15);

							PdfPTable tabFot16 = new PdfPTable (1);
							tabFot16.WidthPercentage = 100;
							float[] widths_tabfot16 = new float[] { 150f };	// controlando el ancho de cada columna tienen que sumas 315 en total
							tabFot16.SetWidths (widths_tabfot16);
							tabFot16.HorizontalAlignment = 0;
							cellcol1 = new PdfPCell (new Phrase (" ", _BoldFont));
							cellcol1.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER;
							cellcol1.HorizontalAlignment = 0;
							tabFot16.AddCell (cellcol1);
							documento.Add (tabFot16);
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
		class EventoTitulos_abonopago : PdfPageEventHelper
		{
			class_public classpublic = new class_public();

			#region Fields
			private string _titulo1_rpt;
			private string _tipo_admision_px;
			private string _fecha_ingresso_px;
			private string _numero_atencion_px;
			private string _nro_expediente_px;
			private string _nombres_apellidos_px;
			private string _fecha_nacimiento_px;
			private string _edad_px;
			private string _sexo_px;
			private string _direccion_px;
			private string _motivodeingreso;
			private string _nombremedico_px;
			private string _numerodehabitacion;
			private string _tipo_paciente_px;
			private string _convenio_px;
			private string _idusuario;
			private string _nombredeusuario;
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
			public string tipo_admision_px
			{
				get{
					return _tipo_admision_px;
				}
				set{
					_tipo_admision_px = value;
				}
			}
			public string fecha_ingresso_px
			{
				get{
					return _fecha_ingresso_px;
				}
				set{
					_fecha_ingresso_px = value;
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
			public string sexo_px
        	{
            	get{
					return _sexo_px;
				}
            	set{
					_sexo_px = value;
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
			public string motivodeingreso
        	{
            	get{
					return _motivodeingreso;
				}
            	set{
					_motivodeingreso = value;
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
			public string numerodehabitacion
        	{
            	get{
					return _numerodehabitacion;
				}
            	set{
					_numerodehabitacion = value;
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

				/*
				cb.MoveTo(0, documento.PageSize.Height/2);
				cb.SetLineWidth(0.05f);
				cb.LineTo(documento.PageSize.Width, documento.PageSize.Height / 2);
				cb.Stroke();*/

				Paragraph titulo1_reporte = new Paragraph(titulo1_rpt, new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 10, iTextSharp.text.Font.BOLD, BaseColor.BLACK));
				titulo1_reporte.Alignment = Element.ALIGN_CENTER;
				documento.Add(titulo1_reporte);
				documento.Add (new Paragraph (" ", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 5)));

				PdfPCell cellcol1;
				PdfPCell cellcol2;
				PdfPCell cellcol3;
				PdfPCell cellcol4;
				PdfPCell cellcol5;
				PdfPCell cellcol6;

				PdfPTable tabFot1 = new PdfPTable(4);
				tabFot1.WidthPercentage = 100;
				float[] widths_tabfot1 = new float[] { 30f, 50f, 200f, 50f };	// controlando el ancho de cada columna tienen que sumas 315 en total
				tabFot1.SetWidths(widths_tabfot1);
				tabFot1.HorizontalAlignment = 0;
				cellcol1 = new PdfPCell(new Phrase("Admitido a",_BoldFont));
				cellcol1.Border = iTextSharp.text.Rectangle.LEFT_BORDER  | iTextSharp.text.Rectangle.TOP_BORDER;
				cellcol2 = new PdfPCell(new Phrase(tipo_admision_px,_NormalFont));
				cellcol2.Border = iTextSharp.text.Rectangle.TOP_BORDER;
				cellcol2.HorizontalAlignment = 0;
				cellcol3 = new PdfPCell(new Phrase("Registro",_BoldFont));
				cellcol3.HorizontalAlignment = 2;		// derecha		
				cellcol3.Border = iTextSharp.text.Rectangle.TOP_BORDER;
				cellcol4 = new PdfPCell(new Phrase(fecha_ingresso_px,_NormalFont));
				cellcol4.Border = iTextSharp.text.Rectangle.RIGHT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER;
				cellcol4.HorizontalAlignment = 0;								
				tabFot1.AddCell(cellcol1);
				tabFot1.AddCell(cellcol2);
				tabFot1.AddCell(cellcol3);
				tabFot1.AddCell(cellcol4);				
				documento.Add(tabFot1);

				PdfPTable tabFot2 = new PdfPTable(6);
				tabFot2.WidthPercentage = 100;
				float[] widths_tabfot2 = new float[] { 40f, 90f, 50f, 50f, 50f, 170f };	// controlando el ancho de cada columna tienen que sumas 315 en total
				tabFot2.SetWidths(widths_tabfot2);
				tabFot2.HorizontalAlignment = 0;
				cellcol1 = new PdfPCell(new Phrase("N° Atencion",_BoldFont));
				cellcol1.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
				cellcol2 = new PdfPCell(new Phrase(numero_atencion_px,_NormalFont));
				cellcol2.Border = iTextSharp.text.Rectangle.NO_BORDER;;
				cellcol2.HorizontalAlignment = 0;
				cellcol3 = new PdfPCell(new Phrase("N° Expediente",_BoldFont));
				cellcol3.Border = iTextSharp.text.Rectangle.NO_BORDER;;
				cellcol4 = new PdfPCell(new Phrase(nro_expediente_px,_NormalFont));
				cellcol4.Border = iTextSharp.text.Rectangle.NO_BORDER;;
				cellcol4.HorizontalAlignment = 0;
				cellcol5 = new PdfPCell(new Phrase("Nombre Paciente",_BoldFont));
				cellcol5.Border = iTextSharp.text.Rectangle.NO_BORDER;;
				cellcol5.HorizontalAlignment = 2;
				cellcol6 = new PdfPCell(new Phrase(nombres_apellidos_px,_NormalFont));
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
				float[] widths_tabfot3 = new float[] { 50f, 90f, 30f, 90f, 30f, 90f };	// controlando el ancho de cada columna tienen que sumas 315 en total
				tabFot3.SetWidths(widths_tabfot3);
				tabFot3.HorizontalAlignment = 0;
				cellcol1 = new PdfPCell(new Phrase("Fecha Nacimiento",_BoldFont));
				cellcol1.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
				cellcol2 = new PdfPCell(new Phrase(fecha_nacimiento_px,_NormalFont));
				cellcol2.Border = iTextSharp.text.Rectangle.NO_BORDER;
				cellcol2.HorizontalAlignment = 0;
				cellcol3 = new PdfPCell(new Phrase("Edad",_BoldFont));
				cellcol3.Border = iTextSharp.text.Rectangle.NO_BORDER;
				cellcol4 = new PdfPCell(new Phrase(edad_px,_NormalFont));
				cellcol4.Border = iTextSharp.text.Rectangle.NO_BORDER;
				cellcol4.HorizontalAlignment = 0;
				cellcol5 = new PdfPCell(new Phrase("Sexo",_BoldFont));
				cellcol5.Border = iTextSharp.text.Rectangle.NO_BORDER;
				cellcol5.HorizontalAlignment = 2;
				cellcol6 = new PdfPCell(new Phrase(sexo_px,_NormalFont));
				cellcol6.Border = iTextSharp.text.Rectangle.RIGHT_BORDER;
				cellcol6.HorizontalAlignment = 0;
				tabFot3.AddCell(cellcol1);
				tabFot3.AddCell(cellcol2);
				tabFot3.AddCell(cellcol3);
				tabFot3.AddCell(cellcol4);
				tabFot3.AddCell(cellcol5);
				tabFot3.AddCell(cellcol6);
				documento.Add(tabFot3);

				PdfPTable tabFot4 = new PdfPTable(2);
				tabFot4.WidthPercentage = 100;
				float[] widths_tabfot4 = new float[] { 30f, 210f };	// controlando el ancho de cada columna tienen que sumas 315 en total
				tabFot4.SetWidths(widths_tabfot4);
				tabFot4.HorizontalAlignment = 0;
				cellcol1 = new PdfPCell(new Phrase("Direccion",_BoldFont));
				cellcol1.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
				cellcol2 = new PdfPCell(new Phrase(direccion_px,_NormalFont));
				cellcol2.Border = iTextSharp.text.Rectangle.RIGHT_BORDER;
				cellcol2.HorizontalAlignment = 0;
				tabFot4.AddCell(cellcol1);
				tabFot4.AddCell(cellcol2);
				documento.Add(tabFot4);

				PdfPTable tabFot5 = new PdfPTable(2);
				tabFot5.WidthPercentage = 100;
				float[] widths_tabfot5 = new float[] { 30f, 210f };	// controlando el ancho de cada columna tienen que sumas 315 en total
				tabFot5.SetWidths(widths_tabfot5);
				tabFot5.HorizontalAlignment = 0;
				cellcol1 = new PdfPCell(new Phrase("Motivo de Ingreso",_BoldFont));
				cellcol1.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
				cellcol2 = new PdfPCell(new Phrase(motivodeingreso,_NormalFont));
				cellcol2.Border = iTextSharp.text.Rectangle.RIGHT_BORDER;
				cellcol2.HorizontalAlignment = 0;
				tabFot5.AddCell(cellcol1);
				tabFot5.AddCell(cellcol2);
				documento.Add(tabFot5);

				PdfPTable tabFot6 = new PdfPTable(4);
				tabFot6.WidthPercentage = 100;
				float[] widths_tabfot6 = new float[] { 50f, 150f, 140f, 60f };	// controlando el ancho de cada columna tienen que sumas 315 en total
				tabFot6.SetWidths(widths_tabfot6);
				tabFot6.HorizontalAlignment = 0;
				cellcol1 = new PdfPCell(new Phrase("Medico 1° Contacto",_BoldFont));
				cellcol1.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
				cellcol2 = new PdfPCell(new Phrase(nombremedico_px,_NormalFont));
				cellcol2.Border = iTextSharp.text.Rectangle.NO_BORDER;
				cellcol2.HorizontalAlignment = 0;
				cellcol3 = new PdfPCell(new Phrase("Habitacion",_BoldFont));
				cellcol3.HorizontalAlignment = 2;		// derecha		
				cellcol3.Border = iTextSharp.text.Rectangle.NO_BORDER;
				cellcol4 = new PdfPCell(new Phrase(numerodehabitacion,_NormalFont));
				cellcol4.Border = iTextSharp.text.Rectangle.RIGHT_BORDER;
				cellcol4.HorizontalAlignment = 0;								
				tabFot6.AddCell(cellcol1);
				tabFot6.AddCell(cellcol2);
				tabFot6.AddCell(cellcol3);
				tabFot6.AddCell(cellcol4);				
				documento.Add(tabFot6);

				PdfPTable tabFot7 = new PdfPTable(4);
				tabFot7.WidthPercentage = 100;
				float[] widths_tabfot7 = new float[] { 40f, 70f, 50f, 150f };	// controlando el ancho de cada columna tienen que sumas 315 en total
				tabFot7.SetWidths(widths_tabfot7);
				tabFot7.HorizontalAlignment = 0;
				cellcol1 = new PdfPCell(new Phrase("Tipo de Paciente",_BoldFont));
				cellcol1.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
				cellcol2 = new PdfPCell(new Phrase(tipo_paciente_px,_BoldFont));
				cellcol2.Border = iTextSharp.text.Rectangle.NO_BORDER;
				cellcol2.HorizontalAlignment = 0;
				cellcol3 = new PdfPCell(new Phrase("Convenio",_BoldFont));
				cellcol3.HorizontalAlignment = 2;		// derecha		
				cellcol3.Border = iTextSharp.text.Rectangle.NO_BORDER;
				cellcol4 = new PdfPCell(new Phrase(convenio_px,_BoldFont));
				cellcol4.Border = iTextSharp.text.Rectangle.RIGHT_BORDER;
				cellcol4.HorizontalAlignment = 0;								
				tabFot7.AddCell(cellcol1);
				tabFot7.AddCell(cellcol2);
				tabFot7.AddCell(cellcol3);
				tabFot7.AddCell(cellcol4);				
				documento.Add(tabFot7);

				PdfPTable tabFot8 = new PdfPTable(4);
				tabFot8.WidthPercentage = 100;
				float[] widths_tabfot8 = new float[] { 40f, 70f, 50f, 150f };	// controlando el ancho de cada columna tienen que sumas 315 en total
				tabFot8.SetWidths(widths_tabfot8);
				tabFot8.HorizontalAlignment = 0;
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
				tabFot8.AddCell(cellcol1);
				tabFot8.AddCell(cellcol2);
				tabFot8.AddCell(cellcol3);
				tabFot8.AddCell(cellcol4);				
				documento.Add(tabFot8);

				documento.Add (new Paragraph (" ", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 5)));
			}
		}
	}

	public class rpt_pases_de_seccion50
	{
		// Declarando variable publicas
		string connectionString;
		string nombrebd;

		class_conexion conexion_a_DB = new class_conexion();
		class_public classpublic = new class_public();

		protected Gtk.Window MyWinError;

		public rpt_pases_de_seccion50(int folioservicio_)
		{
			connectionString = conexion_a_DB._url_servidor+conexion_a_DB._port_DB+conexion_a_DB._usuario_DB+conexion_a_DB._passwrd_user_DB;
			nombrebd = conexion_a_DB._nombrebd;			
			string sexopaciente = "";
			string empresa_o_aseguradora = "";
			int comienzo_linea = 1;
			string query_slq = "SELECT osiris_erp_cobros_enca.folio_de_servicio AS id_secuencia,osiris_erp_cobros_enca.folio_de_servicio AS foliodeservicio," +
				"to_char(osiris_erp_cobros_enca.pid_paciente,'9999999999') AS pidpaciente,"+
				"nombre1_paciente || ' ' || nombre2_paciente || ' ' || apellido_paterno_paciente || ' ' || apellido_materno_paciente AS nombre_completo,"+
				"to_char(osiris_his_paciente.fecha_nacimiento_paciente, 'dd-MM-yyyy') AS fechanacpaciente,"+
				"to_char(to_number(to_char(age('"+DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss")+"',osiris_his_paciente.fecha_nacimiento_paciente),'yyyy') ,'9999'),'9999') AS edadpaciente,"+
				"osiris_his_paciente.sexo_paciente,"+
				"direccion_paciente,numero_casa_paciente,numero_departamento_paciente,colonia_paciente,codigo_postal_paciente,"+
				"osiris_erp_cobros_enca.nombre_medico_tratante,osiris_erp_cobros_enca.nombre_medico_encabezado,"+
				"osiris_erp_cobros_enca.id_empleado_admision,nombre1_empleado || ' ' || nombre2_empleado || ' ' || apellido_paterno_empleado || ' ' || apellido_materno_empleado AS nombresolicitante,"+
				"to_char(osiris_erp_cobros_enca.fechahora_creacion,'yyyy-MM-dd HH24:mi:ss') AS fechahoracrea," +
				"osiris_erp_movcargos.id_tipo_admisiones,descripcion_admisiones," +
				"osiris_erp_cobros_enca.id_empresa AS idempresa,osiris_empresas.descripcion_empresa,"+
				"osiris_erp_cobros_enca.id_aseguradora,osiris_aseguradoras.descripcion_aseguradora,"+
				"id_empleado_admision AS id_quien_creo,descripcion_diagnostico_movcargos,nombre_de_cirugia,descripcion_tipo_paciente " +
				"FROM osiris_erp_movcargos,osiris_his_tipo_admisiones,osiris_erp_cobros_enca,osiris_his_paciente,osiris_empleado,osiris_empresas,osiris_aseguradoras,osiris_his_tipo_pacientes "+
				"WHERE " +
				"osiris_erp_movcargos.id_tipo_admisiones = osiris_his_tipo_admisiones.id_tipo_admisiones " +
				"AND osiris_erp_cobros_enca.pid_paciente = osiris_his_paciente.pid_paciente "+
				"AND osiris_erp_cobros_enca.id_tipo_paciente = osiris_his_tipo_pacientes.id_tipo_paciente "+
				"AND osiris_erp_movcargos.folio_de_servicio = osiris_erp_cobros_enca.folio_de_servicio "+
				"AND osiris_erp_cobros_enca.id_empleado_admision = osiris_empleado.login_empleado "+
				"AND osiris_erp_cobros_enca.id_empresa = osiris_empresas.id_empresa "+
				"AND osiris_erp_cobros_enca.id_aseguradora = osiris_aseguradoras.id_aseguradora "+
				"AND osiris_erp_cobros_enca.folio_de_servicio = '"+ folioservicio_.ToString().Trim() +"';";
			
			NpgsqlConnection conexion; 
			conexion = new NpgsqlConnection (connectionString+nombrebd);
			// Verifica que la base de datos este conectada
			try{
				conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand (); 
				comando.CommandText = query_slq;
				//Console.WriteLine(comando.CommandText);
				NpgsqlDataReader lector = comando.ExecuteReader();
				if (lector.Read()){
					//buscar_en_movcargos(lector["foliodeservicio"].ToString().Trim());
					if (lector["sexo_paciente"].ToString().Trim() == "H"){
						sexopaciente = "MASCULINO";
					}else{
						sexopaciente = "FEMENINO";
					}					
					if((int) lector ["id_aseguradora"] > 1){
						empresa_o_aseguradora = (string) lector["descripcion_aseguradora"];
					}else{
						empresa_o_aseguradora = (string) lector["descripcion_empresa"];						
					}

					iTextSharp.text.Font _NormalFont;
					iTextSharp.text.Font _NormalFont8;
					iTextSharp.text.Font _BoldFont;
					_NormalFont = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 7, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);
					_NormalFont8 = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 8, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);
					_BoldFont = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 7, iTextSharp.text.Font.BOLD, BaseColor.BLACK);

					string pdf_name = System.IO.Path.GetTempPath() + Guid.NewGuid().ToString() + ".pdf";

					// step 1: creation of a document-object
					Document documento = new Document(iTextSharp.text.PageSize.LETTER, 15, 15, 15, 15);
					//Document document = new Document(PageSize.LETTER.Rotate());
					try{
						PdfWriter writerpdf = PdfWriter.GetInstance(documento, new FileStream(pdf_name, FileMode.Create));
						documento.AddTitle("Comprobante de Pago en Caja");
						documento.AddCreator("Sistema Hospitalario OSIRIS");
						documento.AddAuthor("Sistema Hospitalario OSIRIS");
						documento.AddSubject("OSIRSrpt");
						EventoTitulos_abonopago ev = new EventoTitulos_abonopago();
						ev.titulo1_rpt = "SOLICITUD DE ESTUDIOS SUBROGADOS";
						ev.tipo_admision_px = lector["descripcion_admisiones"].ToString().Trim();						
						ev.fecha_ingresso_px =  lector["fechahoracrea"].ToString().Trim();
						ev.numero_atencion_px = lector["foliodeservicio"].ToString().Trim();
						ev.nro_expediente_px = lector["pidpaciente"].ToString().Trim();
						ev.nombres_apellidos_px = lector["nombre_completo"].ToString().Trim();
						ev.fecha_nacimiento_px = lector["fechanacpaciente"].ToString().Trim();
						ev.edad_px = lector["edadpaciente"].ToString().Trim();
						ev.sexo_px = sexopaciente;
						ev.tipobeneficiario = "";
						ev.motivodeingreso = lector["descripcion_diagnostico_movcargos"].ToString().Trim();
						writerpdf.PageEvent = ev;
						documento.Open();

						PdfPCell cellcol1;
						PdfPCell cellcol2;
						PdfPCell cellcol3;
						PdfPCell cellcol4;

						PdfPTable tabFot1 = new PdfPTable(2);
						tabFot1.WidthPercentage = 100;
						float[] widths_tabfot1 = new float[] { 45f, 190f };	// controlando el ancho de cada columna tienen que sumas 315 en total
						tabFot1.SetWidths(widths_tabfot1);
						tabFot1.HorizontalAlignment = 0;
						cellcol1 = new PdfPCell(new Phrase("Documento",_BoldFont));
						cellcol1.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
						cellcol1.BackgroundColor = BaseColor.YELLOW;
						cellcol2 = new PdfPCell(new Phrase("Descripcion",_BoldFont));
						cellcol2.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
						cellcol2.BackgroundColor = BaseColor.YELLOW;
						cellcol2.HorizontalAlignment = 0;
						tabFot1.AddCell(cellcol1);
						tabFot1.AddCell(cellcol2);

						NpgsqlConnection conexion1; 
						conexion1 = new NpgsqlConnection (connectionString+nombrebd);
						// Verifica que la base de datos este conectada
						try{							
							conexion1.Open ();
							NpgsqlCommand comando1; 
							comando1 = conexion1.CreateCommand (); 
							comando1.CommandText = "SELECT * FROM osiris_erp_movimiento_documentos WHERE folio_de_servicio ='"+folioservicio_.ToString().Trim()+"'; ";
							//Console.WriteLine(comando1.CommandText);
							NpgsqlDataReader lector1 = comando1.ExecuteReader();					
							while(lector1.Read()){
								cellcol1 = new PdfPCell(new Phrase(lector1 ["descripcion_documento"].ToString(),_NormalFont));
								cellcol1.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
								cellcol2 = new PdfPCell(new Phrase(lector1 ["informacion_capturada"].ToString(),_NormalFont));
								cellcol2.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
								cellcol2.HorizontalAlignment = 0;
								tabFot1.AddCell(cellcol1);
								tabFot1.AddCell(cellcol2);									
							}
							documento.Add(tabFot1);
						}catch (NpgsqlException ex){
							MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
								MessageType.Warning, ButtonsType.Ok, "PostgresSQL error: {0}",ex.Message);
							msgBoxError.Run ();					msgBoxError.Destroy();
							Console.WriteLine ("PostgresSQL error: {0}",ex.Message);
						}
						conexion1.Close();

						documento.Add (new Paragraph (" ", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 5)));
						documento.Add (new Paragraph (" ", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 5)));
						documento.Add (new Paragraph (" ", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 5)));
						documento.Add (new Paragraph (" ", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 5)));

						PdfPTable tabFot2 = new PdfPTable(4);
						tabFot2.WidthPercentage = 100;
						float[] widths_tabfot2 = new float[] { 50f, 180f, 30f, 50f };	// controlando el ancho de cada columna tienen que sumas 315 en total
						tabFot2.SetWidths(widths_tabfot2);
						tabFot2.HorizontalAlignment = 0;

						cellcol1 = new PdfPCell(new Phrase("Nombre del Medico",_BoldFont));
						cellcol1.Border = iTextSharp.text.Rectangle.NO_BORDER;
						cellcol2 = new PdfPCell(new Phrase(lector["nombre_medico_encabezado"].ToString().Trim(),_NormalFont));
						cellcol2.Border = iTextSharp.text.Rectangle.NO_BORDER;
						cellcol2.HorizontalAlignment = 0;
						cellcol3 = new PdfPCell(new Phrase("Firma",_BoldFont));
						cellcol3.HorizontalAlignment = 2;		// derecha		
						cellcol3.Border = iTextSharp.text.Rectangle.NO_BORDER;
						cellcol4 = new PdfPCell(new Phrase("_____________________",_NormalFont));
						cellcol4.Border = iTextSharp.text.Rectangle.NO_BORDER;
						cellcol4.HorizontalAlignment = 0;								
						tabFot2.AddCell(cellcol1);
						tabFot2.AddCell(cellcol2);
						tabFot2.AddCell(cellcol3);
						tabFot2.AddCell(cellcol4);				
						documento.Add(tabFot2);
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

		class EventoTitulos_abonopago : PdfPageEventHelper
		{
			#region Fields
			private string _titulo1_rpt;
			private string _tipo_admision_px;
			private string _fecha_ingresso_px;
			private string _numero_atencion_px;
			private string _nro_expediente_px;
			private string _nombres_apellidos_px;
			private string _fecha_nacimiento_px;
			private string _edad_px;
			private string _sexo_px;
			private string _tipobeneficiario;
			private string _motivodeingreso;
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
			public string tipo_admision_px
			{
				get{
					return _tipo_admision_px;
				}
				set{
					_tipo_admision_px = value;
				}
			}
			public string fecha_ingresso_px
			{
				get{
					return _fecha_ingresso_px;
				}
				set{
					_fecha_ingresso_px = value;
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
			public string sexo_px
			{
				get{
					return _sexo_px;
				}
				set{
					_sexo_px = value;
				}
			}

			public string tipobeneficiario
			{
				get{
					return _tipobeneficiario;
				}
				set{
					_tipobeneficiario = value;
				}
			}

			public string motivodeingreso
			{
				get{
					return _motivodeingreso;
				}
				set{
					_motivodeingreso = value;
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
				iTextSharp.text.pdf.BaseFont bf = BaseFont.CreateFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
				iTextSharp.text.Rectangle pageSize = documento.PageSize;
				PdfContentByte cb = writerpdf.DirectContent;
				float percentage = 0.0f;

				// Creamos la imagen y le ajustamos el tamaño
				//iTextSharp.text.Image imagen = iTextSharp.text.Image.GetInstance("/opt/osiris/bin/OSIRISLogo2.png");
				iTextSharp.text.Image imagen = iTextSharp.text.Image.GetInstance(System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory,"OSIRISLogo3.png"));
				imagen.BorderWidth = 0;
				imagen.Alignment = Element.ALIGN_LEFT;
				percentage = 150 / imagen.Width;
				imagen.ScalePercent(percentage * 65);
				//Insertamos la imagen en el documento
				documento.Add(imagen);

				// we tell the ContentByte we're ready to draw text
				cb.BeginText ();
				cb.SetFontAndSize (BaseFont.CreateFont (BaseFont.HELVETICA_BOLD, BaseFont.CP1252, BaseFont.NOT_EMBEDDED), 9);
				cb.SetTextMatrix (130,760);		cb.ShowText ("SINDICATO NACIONAL DE TRABAJADORES DE LA EDUCACION");
				cb.SetTextMatrix (130,750);		cb.ShowText ("SECCION 50 S.N.T.E.");
				cb.SetTextMatrix (130,740);		cb.ShowText ("CENTRO DE ESPECIALIDADES MEDICAS");
				cb.SetFontAndSize (BaseFont.CreateFont (BaseFont.HELVETICA_BOLD, BaseFont.CP1252, BaseFont.NOT_EMBEDDED), 6);
				cb.SetTextMatrix (130,730);		cb.ShowText ("Sistema Hospitalario OSIRIS");
				cb.SetTextMatrix (500,760);		cb.ShowText ("Fech.Rpt:" + (string)DateTime.Now.ToString ("yyyy-MM-dd HH:mm:ss"));
				cb.SetTextMatrix (500,750);		cb.ShowText ("N° Page :"+writerpdf.PageNumber.ToString("D8"));
				cb.EndText ();

				/*
				cb.MoveTo(0, documento.PageSize.Height/2);
				cb.SetLineWidth(0.05f);
				cb.LineTo(documento.PageSize.Width, documento.PageSize.Height / 2);
				cb.Stroke();*/

				Paragraph titulo1_reporte = new Paragraph(titulo1_rpt, new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 10, iTextSharp.text.Font.BOLD, BaseColor.BLACK));
				titulo1_reporte.Alignment = Element.ALIGN_CENTER;
				documento.Add(titulo1_reporte);
				documento.Add (new Paragraph (" ", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 5)));

				PdfPCell cellcol1;
				PdfPCell cellcol2;
				PdfPCell cellcol3;
				PdfPCell cellcol4;
				PdfPCell cellcol5;
				PdfPCell cellcol6;

				PdfPTable tabFot1 = new PdfPTable(4);
				tabFot1.WidthPercentage = 100;
				float[] widths_tabfot1 = new float[] { 30f, 50f, 200f, 50f };	// controlando el ancho de cada columna tienen que sumas 315 en total
				tabFot1.SetWidths(widths_tabfot1);
				tabFot1.HorizontalAlignment = 0;
				cellcol1 = new PdfPCell(new Phrase("Admitido a",_BoldFont));
				cellcol1.Border = iTextSharp.text.Rectangle.LEFT_BORDER  | iTextSharp.text.Rectangle.TOP_BORDER;
				cellcol2 = new PdfPCell(new Phrase(tipo_admision_px,_NormalFont));
				cellcol2.Border = iTextSharp.text.Rectangle.TOP_BORDER;
				cellcol2.HorizontalAlignment = 0;
				cellcol3 = new PdfPCell(new Phrase("Registro",_BoldFont));
				cellcol3.HorizontalAlignment = 2;		// derecha		
				cellcol3.Border = iTextSharp.text.Rectangle.TOP_BORDER;
				cellcol4 = new PdfPCell(new Phrase(fecha_ingresso_px,_NormalFont));
				cellcol4.Border = iTextSharp.text.Rectangle.RIGHT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER;
				cellcol4.HorizontalAlignment = 0;								
				tabFot1.AddCell(cellcol1);
				tabFot1.AddCell(cellcol2);
				tabFot1.AddCell(cellcol3);
				tabFot1.AddCell(cellcol4);				
				documento.Add(tabFot1);

				PdfPTable tabFot2 = new PdfPTable(6);
				tabFot2.WidthPercentage = 100;
				float[] widths_tabfot2 = new float[] { 40f, 90f, 50f, 50f, 50f, 170f };	// controlando el ancho de cada columna tienen que sumas 315 en total
				tabFot2.SetWidths(widths_tabfot2);
				tabFot2.HorizontalAlignment = 0;
				cellcol1 = new PdfPCell(new Phrase("N° Atencion",_BoldFont));
				cellcol1.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
				cellcol2 = new PdfPCell(new Phrase(numero_atencion_px,_NormalFont));
				cellcol2.Border = iTextSharp.text.Rectangle.NO_BORDER;;
				cellcol2.HorizontalAlignment = 0;
				cellcol3 = new PdfPCell(new Phrase("N° Expediente",_BoldFont));
				cellcol3.Border = iTextSharp.text.Rectangle.NO_BORDER;;
				cellcol4 = new PdfPCell(new Phrase(nro_expediente_px,_NormalFont));
				cellcol4.Border = iTextSharp.text.Rectangle.NO_BORDER;;
				cellcol4.HorizontalAlignment = 0;
				cellcol5 = new PdfPCell(new Phrase("Nombre Paciente",_BoldFont));
				cellcol5.Border = iTextSharp.text.Rectangle.NO_BORDER;;
				cellcol5.HorizontalAlignment = 2;
				cellcol6 = new PdfPCell(new Phrase(nombres_apellidos_px,_NormalFont));
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
				float[] widths_tabfot3 = new float[] { 50f, 90f, 30f, 90f, 30f, 90f };	// controlando el ancho de cada columna tienen que sumas 315 en total
				tabFot3.SetWidths(widths_tabfot3);
				tabFot3.HorizontalAlignment = 0;
				cellcol1 = new PdfPCell(new Phrase("Fecha Nacimiento",_BoldFont));
				cellcol1.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
				cellcol2 = new PdfPCell(new Phrase(fecha_nacimiento_px,_NormalFont));
				cellcol2.Border = iTextSharp.text.Rectangle.NO_BORDER;
				cellcol2.HorizontalAlignment = 0;
				cellcol3 = new PdfPCell(new Phrase("Edad",_BoldFont));
				cellcol3.Border = iTextSharp.text.Rectangle.NO_BORDER;
				cellcol4 = new PdfPCell(new Phrase(edad_px,_NormalFont));
				cellcol4.Border = iTextSharp.text.Rectangle.NO_BORDER;
				cellcol4.HorizontalAlignment = 0;
				cellcol5 = new PdfPCell(new Phrase("Sexo",_BoldFont));
				cellcol5.Border = iTextSharp.text.Rectangle.NO_BORDER;
				cellcol5.HorizontalAlignment = 2;
				cellcol6 = new PdfPCell(new Phrase(sexo_px,_NormalFont));
				cellcol6.Border = iTextSharp.text.Rectangle.RIGHT_BORDER;
				cellcol6.HorizontalAlignment = 0;
				tabFot3.AddCell(cellcol1);
				tabFot3.AddCell(cellcol2);
				tabFot3.AddCell(cellcol3);
				tabFot3.AddCell(cellcol4);
				tabFot3.AddCell(cellcol5);
				tabFot3.AddCell(cellcol6);
				documento.Add(tabFot3);

				PdfPTable tabFot4 = new PdfPTable(2);
				tabFot4.WidthPercentage = 100;
				float[] widths_tabfot4 = new float[] { 30f, 210f };	// controlando el ancho de cada columna tienen que sumas 315 en total
				tabFot4.SetWidths(widths_tabfot4);
				tabFot4.HorizontalAlignment = 0;
				cellcol1 = new PdfPCell(new Phrase("Tipo de Beneficiario",_BoldFont));
				cellcol1.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
				cellcol2 = new PdfPCell(new Phrase(tipobeneficiario,_NormalFont));
				cellcol2.Border = iTextSharp.text.Rectangle.RIGHT_BORDER;
				cellcol2.HorizontalAlignment = 0;
				tabFot4.AddCell(cellcol1);
				tabFot4.AddCell(cellcol2);
				documento.Add(tabFot4);

				PdfPTable tabFot5 = new PdfPTable(2);
				tabFot5.WidthPercentage = 100;
				float[] widths_tabfot5 = new float[] { 30f, 210f };	// controlando el ancho de cada columna tienen que sumas 315 en total
				tabFot5.SetWidths(widths_tabfot5);
				tabFot5.HorizontalAlignment = 0;
				cellcol1 = new PdfPCell(new Phrase("Motivo de Ingreso",_BoldFont));
				cellcol1.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER;
				cellcol2 = new PdfPCell(new Phrase(motivodeingreso,_NormalFont));
				cellcol2.Border = iTextSharp.text.Rectangle.RIGHT_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER;
				cellcol2.HorizontalAlignment = 0;
				tabFot5.AddCell(cellcol1);
				tabFot5.AddCell(cellcol2);
				documento.Add(tabFot5);

				documento.Add (new Paragraph (" ", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 5)));
			}
		}
	}
}