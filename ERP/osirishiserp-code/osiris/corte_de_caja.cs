//
//  corte_de_caja.cs
//
//  Author:
//       dolivares <>
//
//  Copyright (c) 2017 dolivares
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
using Cairo;
using Pango;
using System.Collections;
using System.Data;
using System.Collections.Generic;
using System.IO;

namespace osiris
{
	public class corte_de_caja
	{
		// Ventana de Corte de Caja
		[Widget] Gtk.Window cortedecaja = null;
		[Widget] Gtk.Entry entry_fecha_iniciocorte = null;
		[Widget] Gtk.Entry entry_fecha_terminocorte = null;
		[Widget] Gtk.ComboBox combobox_tipo_comprobantecorte = null;
		[Widget] Gtk.CheckButton checkbutton_todos_tipcomprcorte = null;
		[Widget] Gtk.ComboBox combobox_formapagocorte = null;
		[Widget] Gtk.CheckButton checkbutton_todos_formapago = null;
		[Widget] Gtk.Entry entry_total_cortecaja = null;
		[Widget] Gtk.Entry entry_comprgenerados = null;			

		[Widget] Gtk.Button button_export_movcomp = null;
		[Widget] Gtk.Button button_salir = null;
		[Widget] Gtk.Button button_generar_corte = null;
		[Widget] Gtk.TreeView treeview_cortecaja = null;
		Gtk.ListStore treeViewEngineCortecaja;

		[Widget] Gtk.TreeView treeview_atensinpasarcaja = null;
		Gtk.ListStore treeViewEngineSinpasarcaja;

		[Widget] Gtk.TreeView treeview_totformapago = null;
		Gtk.ListStore treeViewEngineTotFormapago;



		// tab mov corte
		[Widget] Gtk.Entry entry_fecha_iniciomov = null;
		[Widget] Gtk.Entry entry_fecha_terminomov = null;
		[Widget] Gtk.ComboBox combobox_tipo_comprobantemov = null;
		[Widget] Gtk.CheckButton checkbutton_todos_tipcomprmov = null;
		[Widget] Gtk.TreeView treeview_lista_comprcaja = null;
		[Widget] Gtk.Button button_consulta_movcaja = null;

		Gtk.ListStore treeViewEngineListcomprcaja = null;

		string connectionString;
		string nombrebd;
		string LoginEmpleado = "";
		string idtipocomprobantecorte = "";
		string idtipocomprobante = "";

		string[] args_args = {""};
		int[] args_id_array = {0,1,2,3,4,5,6,7,8,9,10,11,12,13,14};

		//Declaracion de ventana de error
		protected Gtk.Window MyWinError;

		class_conexion conexion_a_DB = new class_conexion();
		class_public classpublic = new class_public();

		public corte_de_caja (string _nombrebd_,string LoginEmpleado_)
		{

			connectionString = conexion_a_DB._url_servidor+conexion_a_DB._port_DB+conexion_a_DB._usuario_DB+conexion_a_DB._passwrd_user_DB;
			nombrebd = conexion_a_DB._nombrebd;
			LoginEmpleado = LoginEmpleado_;

			Glade.XML gxml = new Glade.XML (null, "caja.glade", "cortedecaja", null);
			gxml.Autoconnect                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                 (this);
			cortedecaja.Show();

			entry_fecha_iniciocorte.Text = DateTime.Now.ToString("yyyy-MM-dd");
			entry_fecha_terminocorte.Text = DateTime.Now.ToString("yyyy-MM-dd");

			button_generar_corte.Clicked += new EventHandler(on_button_generar_corte_clicked);


			// corte de movimientos
			entry_fecha_iniciomov.Text = DateTime.Now.ToString("yyyy-MM-dd");
			entry_fecha_terminomov.Text = DateTime.Now.ToString("yyyy-MM-dd");
			button_consulta_movcaja.Clicked += new EventHandler (on_button_consulta_movcaja_clicked);
			button_export_movcomp.Clicked += new EventHandler (on_button_export_movcomp_clicked);

			checkbutton_todos_tipcomprcorte.Clicked += new EventHandler(on_checkbutton_todos_clicked);
			checkbutton_todos_formapago.Clicked += new EventHandler(on_checkbutton_todos_clicked);
			checkbutton_todos_tipcomprmov.Clicked += new EventHandler(on_checkbutton_todos_clicked);

			button_salir.Clicked += new EventHandler(on_cierraventanas_clicked);

			checkbutton_todos_tipcomprcorte.Active = true;
			checkbutton_todos_tipcomprmov.Active = true;

			entry_total_cortecaja.ModifyBase(StateType.Normal, new Gdk.Color(54,180,221));
			entry_fecha_iniciocorte.ModifyBase(StateType.Normal, new Gdk.Color(254,253,152));
			entry_fecha_terminocorte.ModifyBase(StateType.Normal, new Gdk.Color(254,253,152));

			entry_fecha_iniciomov.ModifyBase(StateType.Normal, new Gdk.Color(254,253,152));
			entry_fecha_terminomov.ModifyBase(StateType.Normal, new Gdk.Color(254,253,152));

			crea_treeview_cortecompcaja ();
			crea_treeview_totformapoagos ();
			crea_treeview_movicompcaja ();

			llenado_combobox(0,"",combobox_tipo_comprobantecorte,"sql","SELECT * FROM osiris_erp_tipo_comprobante WHERE activo = 'true' ORDER BY id_tipo_comprobante;","descripcion_tipo_comprobante","id_tipo_comprobante",args_args,args_id_array,"");
			llenado_combobox(0,"",combobox_formapagocorte,"sql","SELECT * FROM osiris_erp_forma_de_pago WHERE activo_formapago = 'true' ORDER BY id_forma_de_pago;","descripcion_forma_de_pago","id_forma_de_pago",args_args,args_id_array,"");

			llenado_combobox(0,"",combobox_tipo_comprobantemov,"sql","SELECT * FROM osiris_erp_tipo_comprobante WHERE activo = 'true' ORDER BY id_tipo_comprobante;","descripcion_tipo_comprobante","id_tipo_comprobante",args_args,args_id_array,"");
		}

		void on_button_generar_corte_clicked (object sender, EventArgs args)
		{
			/*SimpleTreeView tree = new SimpleTreeView(treeview_cortecaja,treeViewEngineCortecaja);
			tree.AddColumn("ID","string"); 
			tree.AddColumn("Fecha","string");
			tree.AddColumn("N° Atencion","string");
			tree.AddColumn("N° Comprobante","string");
			tree.AddColumn("Monto","string");
			tree.AddColumn("Nombre Paciente","string");
			tree.AddColumn("Tipo Comprobante","string");
			tree.AddColumn("Forma de Pago","string");
			tree.AddColumn("ID.Cajero","string");

			tree.AddData(
						secuencia.ToString().Trim(),
						lector["fechaabonopago"].ToString().Trim(),
						lector["foliodeservicio"].ToString().Trim(),
						lector["numerorecibo"].ToString().Trim(),
						float.Parse(lector["monto_comprobante"].ToString().Trim()).ToString("F"),
						lector["nombrepaciente"].ToString().Trim(),
						lector["descripcion_tipo_comprobante"].ToString().Trim(),
						lector["forma_de_pago"].ToString().Trim(),
						lector["id_quien_creo"].ToString().Trim());
			tree.Finish();*/
			crea_treeview_cortecompcaja ();
			crea_treeview_totformapoagos ();
			movimientos_comprobantes_caja ();
			atenciones_sin_pasar_a_caja ();
			llenado_totcomprfpago ();
		}

		void crea_treeview_cortecompcaja ()
		{
			object[] parametros = { treeview_cortecaja, treeViewEngineCortecaja};
			string[,] coltreeview = {
				{ "#", "text" },
				{ "Fecha", "text" },
				{ "N° Atencion", "text" },
				{ "N° Comprobante", "text" },
				{ "Monto", "text" },
				{ "Nombre Paciente", "text" },
				{ "Tipo Comprobante", "text" },
				{ "Forma de Pago", "text" },
				{ "Creado Por", "text" }
			};
			crea_colums_treeview (parametros, coltreeview,"treeview_cortecaja");
		}

		void crea_treeview_totformapoagos ()
		{
			object[] parametros = { treeview_totformapago, treeViewEngineTotFormapago};
			string[,] coltreeview = {
				{ "#", "text" },
				{ "Tipo Comprobante", "text" },
				{ "Forma de Pago", "text" },
				{ "TOTAL", "text" },
				{ "Fecha", "text" }
			};
			crea_colums_treeview (parametros, coltreeview,"treeview_totformapago");
		}

		void movimientos_comprobantes_caja ()
		{
			int secuencia = 0;
			float totalabonos = 0;

			string query_tipocomprobante = "";
			if (checkbutton_todos_tipcomprcorte.Active == true) {
				query_tipocomprobante = "";
			} else {
				query_tipocomprobante = "AND osiris_erp_abonos.id_tipo_comprobante = '" + idtipocomprobantecorte + "' ";
			}
			string query_fechas = "AND to_char(osiris_erp_abonos.fecha_abono,'yyyy-MM-dd') >= '"+entry_fecha_iniciocorte.Text+"' "+
				"AND to_char(osiris_erp_abonos.fecha_abono,'yyyy-MM-dd') <= '"+entry_fecha_terminocorte.Text+"' ";
			NpgsqlConnection conexion;
			conexion = new NpgsqlConnection (connectionString+nombrebd);
			// Verifica que la base de datos este conectada
			try{
				conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand ();
				comando.CommandText ="SELECT DISTINCT (osiris_erp_movcargos.folio_de_servicio),to_char(osiris_erp_abonos.fecha_abono,'yyyy-MM-dd') AS fechaabonopago,"+
					"osiris_erp_abonos.id_abono,monto_x_facturar,osiris_erp_abonos.numero_factura AS numerofactura,subtotal_facturado+iva_facturado AS totalfactura,"+
					"to_char(osiris_erp_abonos.folio_de_servicio,'9999999999') AS foliodeservicio,osiris_erp_cobros_enca.fechahora_creacion,id_quien_creo," +
					//"osiris_erp_movcargos.id_tipo_admisiones,descripcion_admisiones,"+
					"osiris_his_paciente.pid_paciente AS pidpaciente,nombre1_paciente || ' ' || nombre2_paciente || ' ' || apellido_paterno_paciente || ' ' || apellido_materno_paciente AS nombrepaciente,"+
					"osiris_erp_abonos.monto_de_abono_procedimiento AS monto_comprobante,osiris_erp_abonos.concepto_del_abono,numero_recibo_caja AS numerorecibo,pagare,"+
					"osiris_erp_tipo_comprobante.descripcion_tipo_comprobante,osiris_erp_forma_de_pago.descripcion_forma_de_pago AS forma_de_pago,osiris_erp_abonos.monto_convenio,osiris_erp_abonos.observaciones," +
					"osiris_erp_movcargos.id_tipo_paciente,descripcion_tipo_paciente,osiris_erp_tipo_comprobante.id_tipo_comprobante," +
					"osiris_erp_cobros_enca.id_empresa AS idempresa,osiris_empresas.descripcion_empresa AS nombre_empresa,"+
					"osiris_erp_cobros_enca.id_aseguradora,descripcion_aseguradora AS nombre_aseguradora,"+
					"eliminado,to_char(osiris_erp_abonos.fechahora_eliminado,'yyyy-MM-dd') AS fechaeliminado,motivo_eliminacion,osiris_erp_abonos.id_quien_creo AS idquiencreo "+
					"FROM osiris_erp_cobros_enca,osiris_erp_abonos,osiris_erp_tipo_comprobante, osiris_his_paciente, osiris_erp_forma_de_pago,osiris_erp_movcargos,osiris_his_tipo_pacientes,osiris_empresas,osiris_aseguradoras "+
					"WHERE osiris_erp_abonos.folio_de_servicio = osiris_erp_cobros_enca.folio_de_servicio "+
					"AND osiris_erp_movcargos.folio_de_servicio = osiris_erp_abonos.folio_de_servicio "+
					"AND osiris_erp_abonos.id_forma_de_pago = osiris_erp_forma_de_pago.id_forma_de_pago "+ 
					"AND osiris_erp_cobros_enca.pid_paciente = osiris_his_paciente.pid_paciente "+
					"AND osiris_erp_abonos.id_tipo_comprobante = osiris_erp_tipo_comprobante.id_tipo_comprobante " +

					"AND osiris_his_tipo_pacientes.id_tipo_paciente = osiris_erp_cobros_enca.id_tipo_paciente " +
					//"AND osiris_erp_movcargos.id_tipo_admisiones = osiris_his_tipo_admisiones.id_tipo_admisiones " +
					"AND osiris_erp_cobros_enca.id_empresa = osiris_empresas.id_empresa "+
					"AND osiris_erp_cobros_enca.id_aseguradora = osiris_aseguradoras.id_aseguradora "+
					query_tipocomprobante+
					query_fechas+
					" ORDER BY to_char(osiris_erp_abonos.fecha_abono,'yyyy-MM-dd'),osiris_erp_tipo_comprobante.id_tipo_comprobante,numero_recibo_caja;";

				//Console.WriteLine(comando.CommandText);
				NpgsqlDataReader lector = comando.ExecuteReader ();
				while (lector.Read()){
					secuencia ++;
					totalabonos += float.Parse(lector["monto_comprobante"].ToString().Trim());
					treeViewEngineCortecaja.AppendValues(
						secuencia.ToString().Trim(),
						lector["fechaabonopago"].ToString().Trim(),
						lector["foliodeservicio"].ToString().Trim(),
						lector["numerorecibo"].ToString().Trim(),
						float.Parse(lector["monto_comprobante"].ToString().Trim()).ToString("F"),
						lector["nombrepaciente"].ToString().Trim(),
						lector["descripcion_tipo_comprobante"].ToString().Trim(),
						lector["forma_de_pago"].ToString().Trim(),
						lector["id_quien_creo"].ToString().Trim());					
				}
				entry_total_cortecaja.Text = totalabonos.ToString("F");
				entry_comprgenerados.Text = secuencia.ToString();
			}catch(NpgsqlException ex){
				Console.WriteLine("PostgresSQL error: {0}",ex.Message);
				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
					MessageType.Error,ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
				msgBoxError.Run ();		msgBoxError.Destroy();
			}
			conexion.Close();
		}

		void atenciones_sin_pasar_a_caja ()
		{
			
		}

		void llenado_totcomprfpago()
		{
			int secuencia = 0;
			string query_fechas = "AND to_char(osiris_erp_abonos.fecha_abono,'yyyy-MM-dd') >= '"+entry_fecha_iniciocorte.Text+"' "+
				"AND to_char(osiris_erp_abonos.fecha_abono,'yyyy-MM-dd') <= '"+entry_fecha_terminocorte.Text+"' ";
			NpgsqlConnection conexion;
			conexion = new NpgsqlConnection (connectionString+nombrebd);
			// Verifica que la base de datos este conectada
			try{
				conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand ();
				comando.CommandText ="SELECT descripcion_tipo_comprobante,SUM(monto_de_abono_procedimiento) AS motototal,COUNT(osiris_erp_abonos.id_forma_de_pago) AS idformadepago," +
					"osiris_erp_abonos.id_forma_de_pago,descripcion_forma_de_pago,to_char(osiris_erp_abonos.fecha_abono,'yyyy-MM-dd') AS fechaabono " +
					"FROM osiris_erp_abonos,osiris_erp_forma_de_pago,osiris_erp_tipo_comprobante " +
					"WHERE osiris_erp_abonos.id_forma_de_pago = osiris_erp_forma_de_pago.id_forma_de_pago " +
					"AND osiris_erp_abonos.id_tipo_comprobante = osiris_erp_tipo_comprobante.id_tipo_comprobante " +
					query_fechas+
					"GROUP BY osiris_erp_tipo_comprobante.descripcion_tipo_comprobante,osiris_erp_abonos.id_forma_de_pago,descripcion_forma_de_pago,to_char(osiris_erp_abonos.fecha_abono,'yyyy-MM-dd') " +
					"ORDER BY to_char(osiris_erp_abonos.fecha_abono,'yyyy-MM-dd'),osiris_erp_tipo_comprobante.descripcion_tipo_comprobante,osiris_erp_abonos.id_forma_de_pago";

				//Console.WriteLine(comando.CommandText);
				NpgsqlDataReader lector = comando.ExecuteReader ();
				while (lector.Read()){
					secuencia ++;
					treeViewEngineTotFormapago.AppendValues(
						secuencia.ToString().Trim(),
						lector["descripcion_tipo_comprobante"].ToString().Trim(),
						lector["descripcion_forma_de_pago"].ToString().Trim(),
						float.Parse(lector["motototal"].ToString().Trim()).ToString("F"),
						lector["fechaabono"].ToString().Trim()
						);
				}
			}catch(NpgsqlException ex){
				Console.WriteLine("PostgresSQL error: {0}",ex.Message);
				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
					MessageType.Error,ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
				msgBoxError.Run ();		msgBoxError.Destroy();
			}
			conexion.Close();
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
				case "combobox_tipo_comprobantecorte":
					idtipocomprobantecorte = onComboBoxChanged.Model.GetValue(iter,1).ToString().Trim();
					break;
				case "combobox_tipo_comprobantemov":
					idtipocomprobante = onComboBoxChanged.Model.GetValue(iter,1).ToString().Trim();
					break;
				}
			
			}
		}

		void on_button_consulta_movcaja_clicked(object sender, EventArgs args)
		{
			crea_treeview_movicompcaja ();
			llenado_movicompcaja ();
		}

		void crea_treeview_movicompcaja (){
			object[] parametros = { treeview_lista_comprcaja, treeViewEngineListcomprcaja};
			string[,] coltreeview = {
				{ "#", "text" },
				{ "Fecha", "text" },
				{ "N° Recibo", "text" },
				{ "Nombre Paciente", "text" },
				{ "Folio Serv.", "text" },
				{ "N° Exp.", "text" },
				{ "Tipo Comprobante", "text" },
				{ "Monto Compr.", "text" },
				{ "Forma de Pago", "text" },
				{ "Concepto del Abono", "text" },
				{ "Observaciones", "text" },
				{ "Tipo de Paciente", "text" },
				{ "Nombre Empresa", "text" },
				{ "Nombre Aseguradora", "text" },
				{ "Pagare", "text" },
				{ "Eliminado", "text" },
				{ "Fecha Eliminado", "text" },
				{ "Motivo Eliminacion", "text" },
				{ "Monto X Fact.", "text" },
				{ "N° Factura", "text" },
				{ "Monto Factura", "text" },
				{ "Creado Por", "text" },
				{ "Fech.Creacion", "text" },
			};
			crea_colums_treeview (parametros, coltreeview,"treeview_lista_comprcaja");
		}

		void llenado_movicompcaja ()
		{
			int secuencia = 0;
			string query_tipocomprobante = "";
			if (checkbutton_todos_tipcomprmov.Active == true) {
				query_tipocomprobante = "";
			} else {
				query_tipocomprobante = "AND osiris_erp_abonos.id_tipo_comprobante = '" + idtipocomprobante + "' ";
			}
			string query_fechas = "AND to_char(osiris_erp_abonos.fecha_abono,'yyyy-MM-dd') >= '"+entry_fecha_iniciomov.Text+"' "+
				"AND to_char(osiris_erp_abonos.fecha_abono,'yyyy-MM-dd') <= '"+entry_fecha_terminomov.Text+"' ";
			NpgsqlConnection conexion;
			conexion = new NpgsqlConnection (connectionString+nombrebd);
			// Verifica que la base de datos este conectada
			try{
				conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand ();
				comando.CommandText ="SELECT DISTINCT (osiris_erp_movcargos.folio_de_servicio),to_char(osiris_erp_abonos.fecha_abono,'yyyy-MM-dd') AS fechaabonopago,"+
					"osiris_erp_abonos.id_abono,monto_x_facturar,osiris_erp_abonos.numero_factura AS numerofactura,subtotal_facturado+iva_facturado AS totalfactura,"+
					"to_char(osiris_erp_abonos.folio_de_servicio,'9999999999') AS foliodeservicio,osiris_erp_cobros_enca.fechahora_creacion," +
					//"osiris_erp_movcargos.id_tipo_admisiones,descripcion_admisiones,"+
					"osiris_his_paciente.pid_paciente AS pidpaciente,nombre1_paciente || ' ' || nombre2_paciente || ' ' || apellido_paterno_paciente || ' ' || apellido_materno_paciente AS nombrepaciente,"+
					"osiris_erp_abonos.monto_de_abono_procedimiento AS monto_comprobante,osiris_erp_abonos.concepto_del_abono,numero_recibo_caja AS numerorecibo,pagare,"+
					"osiris_erp_tipo_comprobante.descripcion_tipo_comprobante,osiris_erp_forma_de_pago.descripcion_forma_de_pago AS forma_de_pago,osiris_erp_abonos.monto_convenio,osiris_erp_abonos.observaciones," +
					"osiris_erp_movcargos.id_tipo_paciente,descripcion_tipo_paciente,osiris_erp_tipo_comprobante.id_tipo_comprobante," +
					"osiris_erp_cobros_enca.id_empresa AS idempresa,osiris_empresas.descripcion_empresa AS nombre_empresa,"+
					"osiris_erp_cobros_enca.id_aseguradora,descripcion_aseguradora AS nombre_aseguradora,"+
					"eliminado,to_char(osiris_erp_abonos.fechahora_eliminado,'yyyy-MM-dd') AS fechaeliminado,motivo_eliminacion,osiris_erp_abonos.id_quien_creo AS idquiencreo "+
					"FROM osiris_erp_cobros_enca,osiris_erp_abonos,osiris_erp_tipo_comprobante, osiris_his_paciente, osiris_erp_forma_de_pago,osiris_erp_movcargos,osiris_his_tipo_pacientes,osiris_empresas,osiris_aseguradoras "+
					"WHERE osiris_erp_abonos.folio_de_servicio = osiris_erp_cobros_enca.folio_de_servicio "+
					"AND osiris_erp_movcargos.folio_de_servicio = osiris_erp_abonos.folio_de_servicio "+
					"AND osiris_erp_abonos.id_forma_de_pago = osiris_erp_forma_de_pago.id_forma_de_pago "+ 
					"AND osiris_erp_cobros_enca.pid_paciente = osiris_his_paciente.pid_paciente "+
					"AND osiris_erp_abonos.id_tipo_comprobante = osiris_erp_tipo_comprobante.id_tipo_comprobante " +
					"AND osiris_his_tipo_pacientes.id_tipo_paciente = osiris_erp_cobros_enca.id_tipo_paciente " +
					//"AND osiris_erp_movcargos.id_tipo_admisiones = osiris_his_tipo_admisiones.id_tipo_admisiones " +
					"AND osiris_erp_cobros_enca.id_empresa = osiris_empresas.id_empresa "+
					"AND osiris_erp_cobros_enca.id_aseguradora = osiris_aseguradoras.id_aseguradora "+
					query_tipocomprobante+
					query_fechas+
					" ORDER BY to_char(osiris_erp_abonos.fecha_abono,'yyyy-MM-dd'),osiris_erp_tipo_comprobante.id_tipo_comprobante,numero_recibo_caja;";
				
				//Console.WriteLine(comando.CommandText);
				NpgsqlDataReader lector = comando.ExecuteReader ();
				while (lector.Read()){
					secuencia ++;
					treeViewEngineListcomprcaja.AppendValues(
						secuencia.ToString().Trim(),
						lector["fechaabonopago"].ToString().Trim(),
						lector["numerorecibo"].ToString().Trim(),
						lector["nombrepaciente"].ToString().Trim(),
						lector["foliodeservicio"].ToString().Trim(),
						lector["pidpaciente"].ToString().Trim(),
						lector["descripcion_tipo_comprobante"].ToString().Trim(),
						lector["monto_comprobante"].ToString().Trim(),
						lector["forma_de_pago"].ToString().Trim());					
				}
			}catch(NpgsqlException ex){
				Console.WriteLine("PostgresSQL error: {0}",ex.Message);
				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
					MessageType.Error,ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
				msgBoxError.Run ();		msgBoxError.Destroy();
			}
			conexion.Close();
		}

		void crea_colums_treeview(object[] args,string [,] args_colums,string nombre_treeview_)
		{
			System.Type typebool = typeof(bool);
			System.Type typestring = typeof(string);
			Gtk.CellRendererText text;
			Gtk.CellRendererToggle toggle;
			Gtk.TreeView treeviewobject = null;
			Gtk.ListStore treeViewEngine = null;
			ArrayList columns = new ArrayList ();

			treeviewobject = (object) args[0] as Gtk.TreeView;
			treeViewEngine = (object) args[1] as Gtk.ListStore;

			var columns_treeview = new List<TreeViewColumn>();

			foreach (TreeViewColumn tvc in treeviewobject.Columns)
				treeviewobject.RemoveColumn(tvc);

			Type[] t = new Type[args_colums.GetUpperBound (0)+1];
			for (int colum_field = 0; colum_field <= args_colums.GetUpperBound (0); colum_field++) {
				if ((string)args_colums [colum_field, 1] == "text") {
					t [colum_field] = typestring;
				}
				if ((string)args_colums [colum_field, 1] == "toogle") {
					t [colum_field] = typebool;
				}
			}
			treeViewEngine = new Gtk.ListStore(t);
			//treeViewEngine = liststore_;
			treeviewobject.Model = treeViewEngine;
			treeviewobject.RulesHint = true;
			//treeviewobject.Selection.Mode = SelectionMode.Multiple;
			if (args_colums.GetUpperBound (0) >= 0) {
				for (int colum_field = 0; colum_field <= args_colums.GetUpperBound (0); colum_field++) {
					if ((string)args_colums [colum_field, 0] != "") {
						if ((string)args_colums [colum_field, 1] == "text") {
							// column for holiday names
							text = new CellRendererText ();
							text.Xalign = 0.0f;
							columns.Add (text);
							columns_treeview.Add (new TreeViewColumn ((string)args_colums [colum_field, 0], text, "text", colum_field));
							columns_treeview [colum_field].Resizable = true;
							treeviewobject.InsertColumn (columns_treeview [colum_field], colum_field);
							if (colum_field == 3) {
								text.CellBackgroundGdk = new Gdk.Color (135, 193, 243);
							}
						}
						if ((string)args_colums [colum_field, 1] == "toogle") {
							// column for holiday names
							toggle = new CellRendererToggle ();
							toggle.Xalign = 0.0f;
							columns.Add (toggle);
							columns_treeview.Add (new TreeViewColumn ((string)args_colums [colum_field, 0], toggle, "active", colum_field));
							columns_treeview [colum_field].Resizable = true;
							treeviewobject.InsertColumn (columns_treeview [colum_field], colum_field);
							//toggle.Toggled += new ToggledHandler (selecciona_fila);
						}
					}
				}
				if (nombre_treeview_ == "treeview_lista_comprcaja"){
					//treeviewobject.RowActivated += on_button_dbl_clicked;
					//treeview_lista_comprcaja.GetPathAtPos
					treeview_lista_comprcaja = treeviewobject;
					treeViewEngineListcomprcaja = treeViewEngine;
				}
				if (nombre_treeview_ == "treeview_cortecaja") {
					treeview_cortecaja = treeviewobject;
					treeViewEngineCortecaja = treeViewEngine;
				}
				if (nombre_treeview_ == "treeview_totformapago") {
					treeview_totformapago = treeviewobject;
					treeViewEngineTotFormapago = treeViewEngine;
				}
			}
		}

		void on_button_dbl_clicked(object sender, DataColumn args)
		{
			//Gtk.TreeIter iter;
			//
			Gtk.TreePath path;

			Console.WriteLine (args.ToString ());
			Console.WriteLine (sender.ToString ());

			System.Console.WriteLine ("There's been a button press!");

			//GetPathAtPos (System.Convert.ToInt16 (e.X), System.Convert.ToInt16 (e.Y), out path);

			/*
			// given this click event, get its window path
			path = new Gtk.TreePath ();
			GetPathAtPos (System.Convert.ToInt16 (e.X), System.Convert.ToInt16 (e.Y), out path);
			if (treeview_lista_comprcaja.Model.GetIter (out iter, new TreePath (args.Path))) {
				
			}
			// given the path, get some data out of the model for this item
			if (this.Model.GetIter (out iter, path)) {
				FruitTreePiece piece = (FruitTreePiece) this.Model.GetValue (iter, 0);
				string name = piece.name;
				System.Console.WriteLine ("Selected row during click: " + name);

				FruitTreePiece.makeMyTree(this.target);
			}*/
		}

		void on_checkbutton_todos_clicked(object sender, EventArgs args)
		{
			Gtk.CheckButton objcheck = (object) sender as Gtk.CheckButton;
			switch (objcheck.Name.ToString()) {
				case "checkbutton_todos_tipcomprcorte":
					combobox_tipo_comprobantecorte.Sensitive = !(bool)objcheck.Active;	
				break;
				case "checkbutton_todos_formapago":
					combobox_formapagocorte.Sensitive = !(bool)objcheck.Active;	
				break;
				case "checkbutton_todos_tipcomprmov":
					combobox_tipo_comprobantemov.Sensitive = !(bool)objcheck.Active;
				break;

			}			
		}

		void on_button_export_movcomp_clicked(object sender, EventArgs args)
		{
			string query_tipocomprobante = "";
			if((string) classpublic.lee_registro_de_tabla("osiris_empleado","exportar_cortecaja","WHERE exportar_cortecaja = 'true' AND login_empleado = '"+LoginEmpleado+"' ","exportar_cortecaja","bool") == "True"){
				if (checkbutton_todos_tipcomprmov.Active == true) {
					query_tipocomprobante = "";
				} else {
					query_tipocomprobante = "AND osiris_erp_abonos.id_tipo_comprobante = '" + idtipocomprobante + "' ";
				}
				string query_fechas = "AND to_char(osiris_erp_abonos.fecha_abono,'yyyy-MM-dd') >= '"+entry_fecha_iniciomov.Text+"' "+
					"AND to_char(osiris_erp_abonos.fecha_abono,'yyyy-MM-dd') <= '"+entry_fecha_terminomov.Text+"' ";

				string query_sql = "SELECT DISTINCT (osiris_erp_movcargos.folio_de_servicio),to_char(osiris_erp_abonos.fecha_abono,'yyyy-MM-dd') AS fechaabonopago,"+
					"osiris_erp_abonos.id_abono,monto_x_facturar,osiris_erp_abonos.numero_factura AS numerofactura,subtotal_facturado+iva_facturado AS totalfactura,"+
					"to_char(osiris_erp_abonos.folio_de_servicio,'9999999999') AS foliodeservicio,osiris_erp_cobros_enca.fechahora_creacion," +
					//"osiris_erp_movcargos.id_tipo_admisiones,descripcion_admisiones,"+
					"osiris_his_paciente.pid_paciente AS pidpaciente,nombre1_paciente || ' ' || nombre2_paciente || ' ' || apellido_paterno_paciente || ' ' || apellido_materno_paciente AS nombrepaciente,"+
					"osiris_erp_abonos.monto_de_abono_procedimiento AS monto_comprobante,osiris_erp_abonos.concepto_del_abono,numero_recibo_caja AS numerorecibo,pagare,"+
					"osiris_erp_tipo_comprobante.descripcion_tipo_comprobante,osiris_erp_forma_de_pago.descripcion_forma_de_pago AS forma_de_pago,osiris_erp_abonos.monto_convenio,osiris_erp_abonos.observaciones," +
					"osiris_erp_movcargos.id_tipo_paciente,descripcion_tipo_paciente,osiris_erp_tipo_comprobante.id_tipo_comprobante," +
					"osiris_erp_cobros_enca.id_empresa AS idempresa,osiris_empresas.descripcion_empresa AS nombre_empresa,"+
					"osiris_erp_cobros_enca.id_aseguradora,descripcion_aseguradora AS nombre_aseguradora,"+
					"eliminado,to_char(osiris_erp_abonos.fechahora_eliminado,'yyyy-MM-dd') AS fechaeliminado,motivo_eliminacion,osiris_erp_abonos.id_quien_creo AS idquiencreo "+
					"FROM osiris_erp_cobros_enca,osiris_erp_abonos,osiris_erp_tipo_comprobante, osiris_his_paciente, osiris_erp_forma_de_pago,osiris_erp_movcargos,osiris_his_tipo_pacientes,osiris_empresas,osiris_aseguradoras "+
					"WHERE osiris_erp_abonos.folio_de_servicio = osiris_erp_cobros_enca.folio_de_servicio "+
					"AND osiris_erp_movcargos.folio_de_servicio = osiris_erp_abonos.folio_de_servicio "+
					"AND osiris_erp_abonos.id_forma_de_pago = osiris_erp_forma_de_pago.id_forma_de_pago "+ 
					"AND osiris_erp_cobros_enca.pid_paciente = osiris_his_paciente.pid_paciente "+
					"AND osiris_erp_abonos.id_tipo_comprobante = osiris_erp_tipo_comprobante.id_tipo_comprobante " +
					"AND osiris_his_tipo_pacientes.id_tipo_paciente = osiris_erp_cobros_enca.id_tipo_paciente " +
					//"AND osiris_erp_movcargos.id_tipo_admisiones = osiris_his_tipo_admisiones.id_tipo_admisiones " +
					"AND osiris_erp_cobros_enca.id_empresa = osiris_empresas.id_empresa "+
					"AND osiris_erp_cobros_enca.id_aseguradora = osiris_aseguradoras.id_aseguradora "+
					query_tipocomprobante+
					query_fechas+
					" ORDER BY to_char(osiris_erp_abonos.fecha_abono,'yyyy-MM-dd'),osiris_erp_tipo_comprobante.id_tipo_comprobante,numero_recibo_caja;";

				string[] args_names_field = {"fechaabonopago","foliodeservicio","pidpaciente","nombrepaciente","numerorecibo","descripcion_tipo_comprobante","monto_comprobante","forma_de_pago","concepto_del_abono","observaciones","monto_convenio","descripcion_tipo_paciente","nombre_empresa","nombre_aseguradora","pagare","eliminado","fechaeliminado","motivo_eliminacion","monto_x_facturar","numerofactura","totalfactura","idquiencreo","fechahora_creacion"};
				string[] args_type_field = {"string","float","float","string","float","string","float","string","string","string","float","string","string","string","string","string","string","string","float","string","float","string","string"};
				string[] args_field_text = {""};
				string[] args_more_title = {""};
				string[,] args_formulas = {{"6","=SUM(G2:G"},{"18","=SUM(S2:S"}};
				string[,] args_width = {{"3","7cm"},{"5","5cm"},{"7","5cm"},{"8","7cm"},{"9","8cm"},{"11","5cm"},{"12","7cm"},{"13","7cm"}};

				new osiris.class_traslate_spreadsheet(query_sql,args_names_field,args_type_field,false,args_field_text,"",false,args_more_title,args_formulas,args_width);

			}else{
				MessageDialog msgBox = new MessageDialog (MyWinError,DialogFlags.Modal,
					MessageType.Info,ButtonsType.Ok,"No tiene Permiso para esta Opcion");
				msgBox.Run ();msgBox.Destroy();
			}
		}

		// cierra ventanas emergentes
		void on_cierraventanas_clicked (object sender, EventArgs args)
		{
			Widget win = (Widget) sender;
			win.Toplevel.Destroy();
		}	
	}

	// MCD Marengo 2012
	// This class assumes that you have a GTK tree view on a form - just
	// instantiate this class with that control and this does all the
	// hard stuff. Note all columns are treated as text.
	// To use:
	// SimpleTreeView tree=new SimpleTreeView(controlOnForm);
	// tree.AddColumn("Col1"); tree.AddColumn("Col2"); // adding columns
	// tree.AddData(string, ...); // add data, one string per column
	// tree.Finish(); // finalise the process with this
	public class SimpleTreeView{
		List<Gtk.TreeViewColumn> _cols;
		List<Gtk.CellRendererText> _cells;
		Gtk.TreeView _tree;
		Gtk.ListStore _list = null;
		bool _colsAdded=false;

		public SimpleTreeView(Gtk.TreeView tree,Gtk.ListStore list_store){
			_tree = tree as Gtk.TreeView;
			_list = list_store as Gtk.ListStore;
			_cols = new List<Gtk.TreeViewColumn>();
			_cells = new List<Gtk.CellRendererText>();
		}

		public void AddColumn(string colName,string colType){
			if(_colsAdded == true){
				// can't add columns after you've started adding data.
				throw new Exception("Cannot add columns after data has been added");
			}
			_cols.Add(new Gtk.TreeViewColumn()); 
			_cols[_cols.Count - 1].Title = colName;
		}

		public void AddData(params string[] fieldData){
			// it is assumed when this is called that the user has finished adding columns.
			_colsAdded = true;
			if(_cols.Count != fieldData.Length){
				throw new Exception("Mismatch on number of columns defined and items of data passed");
			}

			if(_list == null){
				Type[] t = new Type[_cols.Count];
				for(int n=0; n<_cols.Count; ++n){
					t[n] = typeof(string);
				}
				_list = new Gtk.ListStore(t);
			}
			_list.AppendValues(fieldData);
		}

		public void Finish(){
			for(int n=0; n<_cols.Count; ++n){
				_cells.Add(new Gtk.CellRendererText());
				_cols[n].PackStart(_cells[_cells.Count-1], true);
			}
			for(int n=0; n<_cols.Count; ++n){
				_cols[n].AddAttribute(_cells[n], "text", n);
			}
			for(int n=0; n<_cols.Count; ++n){
				_tree.AppendColumn(_cols[n]);
			}
			_tree.Model=_list;
			_tree.RulesHint = true;

			Console.WriteLine (_tree.Name.ToString ());
		}

		public List<string> GetSelected(){
			// returns the cols of the current line
			Gtk.TreeSelection selection =_tree.Selection;
			Gtk.TreeModel model;
			Gtk.TreeIter iter;
			List<string> rc=new List<string>();
			if(selection.GetSelected(out model, out iter)){
				for(int n=0; n<_cols.Count; n++){
					rc.Add(model.GetValue(iter,n).ToString());
				}
			}
			return rc;
		}
	}
}