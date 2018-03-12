// created on 15/02/2008 at 10:47 a
//////////////////////////////////////////////////////////////////////
// created on 21/01/2008 at 08:28 p
// Sistema Hospitalario OSIRIS
// Monterrey - Mexico
//
// Autor    	: Ing. Daniel Olivares C. (Modificaciones y Ajustes)
//				  Tec. Homero Montoya Galvan (Programaion)
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
using System.Xml;
using Gdk;

namespace osiris
{
	public class abonos
	{
		//Declarando ventana de cambios de datos de paciente
		[Widget] Gtk.Window abonar_procedimientos  = null;
		[Widget] Gtk.Entry entry_folio_servicio = null;
		[Widget] Gtk.Entry entry_pid_paciente = null;
		[Widget] Gtk.Entry entry_nombre_paciente = null;
		[Widget] Gtk.Entry entry_monto_abono = null;
		[Widget] Gtk.Entry entry_monto_convenio = null;
		[Widget] Gtk.Entry entry_recibo_caja = null;
		[Widget] Gtk.Entry entry_presupuesto = null;
		[Widget] Gtk.Entry entry_paquete = null;
		[Widget] Gtk.Entry entry_dia = null;
		[Widget] Gtk.Entry entry_mes = null;
		[Widget] Gtk.Entry entry_ano = null;
		[Widget] Gtk.Entry entry_concepto_abono = null;
		[Widget] Gtk.Entry entry_observaciones2 = null;
		[Widget] Gtk.Entry entry_observaciones3 = null;
		[Widget] Gtk.Entry entry_total_abonos = null;
		[Widget] Gtk.Entry entry_total_convenio = null;
		[Widget] Gtk.Entry entry_saldo_deuda = null;
		[Widget] Gtk.Button button_cancela_abonopago = null;
		[Widget] Gtk.Button button_cancelar_compserv = null;
		[Widget] Gtk.CheckButton checkbutton_nuevo_abono = null;
		[Widget] Gtk.Button button_guardar = null;
		[Widget] Gtk.Button button_imprimir = null;
		[Widget] Gtk.Button button_salir = null;
		[Widget] Gtk.Button button_resumen = null;
		[Widget] Gtk.Button button_abonos_expediente = null;
		[Widget] Gtk.TreeView lista_abonos = null;
		[Widget] Gtk.TreeView treeview_lista_comprserv  = null;
		[Widget] Gtk.Entry entry_valor_x_facturar = null;
		
		// Pestaña de pagares
		[Widget] Gtk.Button button_pagar_pagare = null;
		[Widget] Gtk.Button button_cancelar_pagare = null;
		[Widget] Gtk.TreeView treeview_lista_pagare = null;
		
		// Pestaña Comprobantes de Servicio
		[Widget] Gtk.Statusbar statusbar_abonos = null;
		[Widget] Gtk.ComboBox combobox_formapago = null;
		[Widget] Gtk.ComboBox combobox_tipocomprobante = null;
		[Widget] Gtk.Button button_imprimir_comp_serv = null;
		[Widget] Gtk.Button button_imprimir_pagare = null;
		[Widget] Gtk.Button button_fact_comprobante = null;
		
		//Ventana de cancelacion de folios
		[Widget] Gtk.Window cancelador_folios = null;
		[Widget] Gtk.Button button_cancelar = null;
		[Widget] Gtk.Entry entry_folio = null;
		[Widget] Gtk.Entry entry_motivo = null;
		
		// Ventana factura de comprobantes
		[Widget] Gtk.Window factura_comprobante_caja = null;
		[Widget] Gtk.Entry entry_nro_recibocaja = null;
		[Widget] Gtk.Entry entry_serie_factura = null;
		[Widget] Gtk.Entry entry_folio_factura = null;
		[Widget] Gtk.Entry entry_fech_recibocaja = null;
		[Widget] Gtk.Entry entry_monto_recibocaja = null;
		[Widget] Gtk.Entry entry_nro_factura = null;
		[Widget] Gtk.Entry entry_ano_fechafactura = null;
		[Widget] Gtk.Entry entry_mes_fechafactura = null;
		[Widget] Gtk.Entry entry_dia_fechafactura = null;
		[Widget] Gtk.Entry entry_horafactura = null;
		[Widget] Gtk.Entry entry_rfc_receptor = null;
		[Widget] Gtk.Entry entry_id_receptor = null;
		[Widget] Gtk.Entry entry_nombre_receptor = null;
		[Widget] Gtk.Entry entry_rfc_emisor = null;
		[Widget] Gtk.Entry entry_id_emisor = null;
		[Widget] Gtk.Entry entry_nombre_emisor = null;
		[Widget] Gtk.Entry entry_folio_fiscal = null;
		[Widget] Gtk.FileChooserButton filechooserbutton1 = null;
		[Widget] Gtk.Button button_cargar_xml = null;
		[Widget] Gtk.Entry entry_subtotal_fact = null;
		[Widget] Gtk.Entry entry_iva_fact = null;
		[Widget] Gtk.Entry entry_total_fact = null;
		[Widget] Gtk.Button button_facturar = null;
				
		int PidPaciente;
		int folioservicio;
		string fecha_admision;
		string fechahora_alta;
		string nombre_paciente;
		string telefono_paciente;
		string doctor;
		string cirugia;
		string fecha_nacimiento;
		string edadpac;
		string tipo_paciente;
		int id_tipopaciente;
		string aseguradora;
		string dir_pac;
		string empresapac;
		bool apl_desc_siempre = true;
		bool apl_desc;
		string nombrecajero;		
		string LoginEmpleado;
		int idformadepago = 1;
		string monto;
		string fecha;
		string concepto;
		string idcreo;
		string recibo;
		string presupuesto;
		string paquete;
		string descripcion;
		string nombrebd;		
		string connectionString;
		string idtipocomprobante = "1";
		bool pagarfactura = false;
		string montoconvenio;
		bool tienepagare;
		
		string calle_receptor = "";
		string noexterior_receptor = "";
		string nointerior_receptor = "";
		string colonia_receptor = "";
		string localidad_receptor = "";
		string referencia_receptor = "";
		string municipio_receptor = "";
		string estado_receptor = "";
		string pais_receptor = "";
		string codpostal_receptor = "";
		
		string nro_de_certificado_emisor = "";
		string certificado_emisor = "";
		string sello_emisor = "";
		string forma_de_pago_emisor = "";
		string metodo_de_pago_emisor = "";
		string tipo_de_comprobante_emisor = "";
		string lugar_expedicion_emisor = "";
		string tipo_cambio = "";
		string moneda = "";
		string sello_sat = "";
		string sello_cfd = "";
		string no_certificado_sat = "";
		string fecha_timbrado = "";
		string version_sat = "";
		
		XmlDocument xml;
		
		//Declaracion de ventana de error
		protected Gtk.Window MyWinError;
		protected Gtk.Window MyWin;
		
		TreeStore treeViewEngineabonos;
		TreeStore treeViewEnginecomprserv;
		TreeStore treeViewEnginepagare;
		//Declarando las celdas
		CellRendererText cellr0;		CellRendererText cellrt1;
		CellRendererText cellrt2;		CellRendererText cellrt3;
		CellRendererText cellrt4;		CellRendererText cellrt5;
		CellRendererText cellrt6;		CellRendererText cellrt7;
		CellRendererText cellrt8;
		
		class_conexion conexion_a_DB = new class_conexion();
		class_public classpublic = new class_public();
		/// <summary>
		/// Initializes a new instance of the <see cref="osiris.abonos"/> class.
		/// </summary>
		/// <param name='PidPaciente_'>
		/// Pid paciente_.
		/// </param>
		/// <param name='folioservicio_'>
		/// Folioservicio_.
		/// </param>
		/// <param name='nombrebd_'>
		/// Nombrebd_.
		/// </param>
		/// <param name='entry_fecha_admision_'>
		/// Entry_fecha_admision_.
		/// </param>
		/// <param name='entry_fechahora_alta_'>
		/// Entry_fechahora_alta_.
		/// </param>
		/// <param name='entry_numero_factura_'>
		/// Entry_numero_factura_.
		/// </param>
		/// <param name='entry_nombre_paciente_'>
		/// Entry_nombre_paciente_.
		/// </param>
		/// <param name='entry_telefono_paciente_'>
		/// Entry_telefono_paciente_.
		/// </param>
		/// <param name='entry_doctor_'>
		/// Entry_doctor_.
		/// </param>
		/// <param name='entry_tipo_paciente_'>
		/// Entry_tipo_paciente_.
		/// </param>
		/// <param name='entry_aseguradora_'>
		/// Entry_aseguradora_.
		/// </param>
		/// <param name='edadpac_'>
		/// Edadpac_.
		/// </param>
		/// <param name='fecha_nacimiento_'>
		/// Fecha_nacimiento_.
		/// </param>
		/// <param name='dir_pac_'>
		/// Dir_pac_.
		/// </param>
		/// <param name='cirugia_'>
		/// Cirugia_.
		/// </param>
		/// <param name='empresapac_'>
		/// Empresapac_.
		/// </param>
		/// <param name='idtipopaciente_'>
		/// Idtipopaciente_.
		/// </param>
		/// <param name='nombrecajero_'>
		/// Nombrecajero_.
		/// </param>
		/// <param name='LoginEmpleado_'>
		/// Login empleado_.
		/// </param>
		/// <param name='agregarmasabonos'>
		/// Agregarmasabonos.
		/// </param>
		/// <param name='montoconvenio_'>
		/// Montoconvenio_.
		/// </param>
		/// <param name='cuenta_cerrada_'>
		/// Cuenta_cerrada_.
		/// </param>
		public abonos (	int PidPaciente_ ,int folioservicio_,string nombrebd_ ,string entry_fecha_admision_,
						string entry_fechahora_alta_,string entry_numero_factura_,string entry_nombre_paciente_,
						string entry_telefono_paciente_,string entry_doctor_,string entry_tipo_paciente_,
						string entry_aseguradora_,string edadpac_,string fecha_nacimiento_,string dir_pac_,
						string cirugia_,string empresapac_,int idtipopaciente_,string nombrecajero_,string LoginEmpleado_,
		                bool agregarmasabonos,string montoconvenio_,bool cuenta_cerrada_,bool tienepagare_)
		{
			//nombrebd = _nombrebd_; 			
			PidPaciente = PidPaciente_;
			folioservicio = folioservicio_;			
			fecha_admision = entry_fecha_admision_;
			fechahora_alta = entry_fechahora_alta_;
			nombre_paciente = entry_nombre_paciente_;
			telefono_paciente = entry_telefono_paciente_;
			doctor = entry_doctor_;
			cirugia = cirugia_;
			tipo_paciente = entry_tipo_paciente_;
			id_tipopaciente = idtipopaciente_;
			aseguradora = entry_aseguradora_;
			edadpac = edadpac_;
			fecha_nacimiento = fecha_nacimiento_;
			dir_pac = dir_pac_;
			empresapac = empresapac_;
			nombrecajero = nombrecajero_;
			LoginEmpleado = LoginEmpleado_;
			connectionString = conexion_a_DB._url_servidor+conexion_a_DB._port_DB+conexion_a_DB._usuario_DB+conexion_a_DB._passwrd_user_DB;
			nombrebd = conexion_a_DB._nombrebd;
			montoconvenio = montoconvenio_;
			tienepagare = tienepagare_;
			
			Glade.XML gxml = new Glade.XML (null, "caja.glade", "abonar_procedimientos", null);
			gxml.Autoconnect                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                 (this);
	        abonar_procedimientos.Show();
			crea_treeview_abonos();
			crea_treeview_comprserv();
			crea_treeview_pagare();
			llenando_lista_de_abonos();
			llenando_lista_comprobante();
			llenando_lista_pagare();
			llenado_tipo_comprobante();
			
			//llenando_lista_de_pagares();
			
			button_guardar.Clicked += new EventHandler(on_button_guardar_clicked);
			button_imprimir.Clicked += new EventHandler(on_button_imprimir_clicked);
			button_imprimir_comp_serv.Clicked += new EventHandler(on_button_imprimir_comp_serv_clicked);
			button_imprimir_pagare.Clicked += new EventHandler(on_button_imprimir_pagare_clicked);
			button_resumen.Clicked += new EventHandler(on_button_resumen_clicked);
			button_abonos_expediente.Clicked += new EventHandler(on_button_abonos_expediente_clicked);
			button_salir.Clicked += new EventHandler(on_cierraventanas_clicked);
			button_cancela_abonopago.Clicked += new EventHandler(on_button_cancela_abonopago_clicked);
			button_cancelar_compserv.Clicked += new EventHandler(on_button_cancelar_compserv_clicked);
			
			button_cancelar_pagare.Clicked += new EventHandler(on_button_cancelar_pagare_clicked);
			button_pagar_pagare.Clicked += new EventHandler(on_button_pagar_pagare_clicked);
				
			button_fact_comprobante.Clicked += new EventHandler(on_button_fact_comprobante_clicked);
			checkbutton_nuevo_abono.Sensitive = !cuenta_cerrada_;
			
			checkbutton_nuevo_abono.Clicked += new EventHandler(on_checkbutton_nuevo_abono_clicked);
			entry_monto_abono.Sensitive = false;
			entry_recibo_caja.Sensitive = false;
			entry_presupuesto.Sensitive = false;
			entry_paquete.Sensitive = false;
			entry_dia.Sensitive = false;
			entry_dia.Text = DateTime.Now.ToString("dd");
			entry_mes.Sensitive = false;
			entry_mes.Text = DateTime.Now.ToString("MM");
			entry_ano.Sensitive = false;
			entry_ano.Text = DateTime.Now.ToString("yyyy");
			entry_concepto_abono.Sensitive = false;
			button_guardar.Sensitive = false;
			combobox_formapago.Sensitive = false;
			combobox_tipocomprobante.Sensitive = false;
			entry_recibo_caja.IsEditable = false;
			entry_valor_x_facturar.Sensitive = false;
			
			entry_folio_servicio.Text = folioservicio.ToString().Trim();
			entry_pid_paciente.Text = PidPaciente.ToString();
			entry_nombre_paciente.Text = nombre_paciente;
				
			entry_monto_convenio.Text = montoconvenio;
			entry_total_convenio.Text = montoconvenio;
			entry_saldo_deuda.Text = (float.Parse(entry_total_convenio.Text)-float.Parse(entry_total_abonos.Text)).ToString("F");
			
			entry_saldo_deuda.ModifyBase(StateType.Normal, new Gdk.Color(254,253,152));		// Color Amarillo
			entry_recibo_caja.ModifyBase(StateType.Normal, new Gdk.Color(27,255,37));		// Color Verde
			entry_folio_servicio.ModifyBase(StateType.Normal, new Gdk.Color(166,220,255));	// Color Celeste
			entry_pid_paciente.ModifyBase(StateType.Normal, new Gdk.Color(166,220,255));	// Color Celeste
			entry_nombre_paciente.ModifyBase(StateType.Normal, new Gdk.Color(166,220,255));	// Color Celeste
			
			entry_presupuesto.Text = "0";
			entry_paquete.Text = "0";
			entry_valor_x_facturar.Text = "0";
			
			statusbar_abonos.Pop(0);
			statusbar_abonos.Push(1, "login: "+LoginEmpleado+"  |Usuario: "+nombrecajero);
			statusbar_abonos.HasResizeGrip = false;
		}
		
		void llenado_tipo_comprobante()
		{
			
			CellRendererText cell3 = new CellRendererText();
			combobox_tipocomprobante.PackStart(cell3, true);
			combobox_tipocomprobante.AddAttribute(cell3,"text",0);
        
			ListStore store5 = new ListStore( typeof (string), typeof (int));
			combobox_tipocomprobante.Model = store5;
			
			NpgsqlConnection conexion; 
			conexion = new NpgsqlConnection (connectionString+nombrebd);
            // Verifica que la base de datos este conectada
			try{
				conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand ();
               	comando.CommandText = "SELECT * FROM osiris_erp_tipo_comprobante "+
									"WHERE activo = 'true' " +
									"AND acceso_caja_abonos = 'true' "+
               						"ORDER BY id_tipo_comprobante;";
				
				NpgsqlDataReader lector = comando.ExecuteReader ();
               	while (lector.Read()){
					store5.AppendValues ((string) lector["descripcion_tipo_comprobante"],
									 	(int) lector["id_tipo_comprobante"] );
				}
			}catch (NpgsqlException ex){
				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
										MessageType.Error,ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
				msgBoxError.Run ();				msgBoxError.Destroy();
			}
			conexion.Close ();			
			combobox_tipocomprobante.Changed += new EventHandler(onComboBoxChanged_tipocomprobante);
		}
		
		void onComboBoxChanged_tipocomprobante(object sender, EventArgs args)
		{
			ComboBox combobox_tipocomprobante = sender as ComboBox;
			if (sender == null)	{	return;	}
			TreeIter iter;			
			if (combobox_tipocomprobante.GetActiveIter (out iter)){
				idtipocomprobante = combobox_tipocomprobante.Model.GetValue(iter,1).ToString().Trim();
				entry_recibo_caja.Text = (string) classpublic.lee_ultimonumero_registrado("osiris_erp_abonos","numero_recibo_caja"," WHERE id_tipo_comprobante = '"+combobox_tipocomprobante.Model.GetValue(iter,1).ToString().Trim()+"' ");
				if(int.Parse(idtipocomprobante) == 6){
					entry_valor_x_facturar.Sensitive = true;
					entry_monto_abono.Sensitive = false;
					entry_monto_abono.Text = "0";					
				}else{
					entry_valor_x_facturar.Sensitive = false;
					entry_valor_x_facturar.Text = "0";
					entry_monto_abono.Sensitive = true;
					//entry_monto_abono.Text = "0";
				}
			}
		}
		
		void crea_treeview_abonos()
		{
			treeViewEngineabonos = new TreeStore(typeof(string),
												typeof(string),
												typeof(string),
												typeof(string),
												typeof(string),
												typeof(string),
												typeof(string),
			                                    typeof(string),
												typeof(string),
			                                    typeof(bool),
			                                    typeof(bool),
			                                    typeof(string),
												typeof(string),
			                                    typeof(string),
			                                    typeof(string),
			                                    typeof(string),
			                                    typeof(string),
			                                     typeof(string),
			                                     typeof(int),
			                                     typeof(bool));
			
			lista_abonos.Model = treeViewEngineabonos;						
			lista_abonos.RulesHint = true;							
			lista_abonos.RowActivated += on_button_imprimir_clicked;
			
			TreeViewColumn col_abono = new TreeViewColumn();
			cellr0 = new CellRendererText();
			col_abono.Title = "Abonos Ralizados";
			col_abono.PackStart(cellr0, true);
			col_abono.AddAttribute (cellr0, "text", 0);
			col_abono.SortColumnId = (int) Col_proveedores.col_abono;
			
			TreeViewColumn col_fecha_abono = new TreeViewColumn();
			CellRendererText cellrt1 = new CellRendererText();
			col_fecha_abono.Title = "Fecha del Abono";
			col_fecha_abono.PackStart(cellrt1, true);
			col_fecha_abono.AddAttribute (cellrt1, "text", 1);
			col_fecha_abono.SortColumnId = (int) Col_proveedores.col_fecha_abono;
			
			TreeViewColumn col_concepto = new TreeViewColumn();
			CellRendererText cellrt2 = new CellRendererText();
			col_concepto.Title = "Concepto";
			col_concepto.PackStart(cellrt2, true);
			col_concepto.AddAttribute (cellrt2, "text", 2);
			col_concepto.SortColumnId = (int) Col_proveedores.col_concepto;
			
			TreeViewColumn col_id_creo = new TreeViewColumn();
			CellRendererText cellrt3 = new CellRendererText();
			col_id_creo.Title = "Id Quien Creo";
			col_id_creo.PackStart(cellrt3, true);
			col_id_creo.AddAttribute (cellrt3, "text", 3);
			col_id_creo.SortColumnId = (int) Col_proveedores.col_id_creo;
			
			TreeViewColumn col_recibo = new TreeViewColumn();
			CellRendererText cellrt4 = new CellRendererText();
			col_recibo.Title = "No. Recibo Caja";
			col_recibo.PackStart(cellrt4, true);
			col_recibo.AddAttribute (cellrt4, "text", 4);
			col_recibo.SortColumnId = (int) Col_proveedores.col_recibo;
			
			TreeViewColumn col_tipocomprobante = new TreeViewColumn();
			CellRendererText cellrt5 = new CellRendererText();
			col_tipocomprobante.Title = "Tipo Comprante";
			col_tipocomprobante.PackStart(cellrt5, true);
			col_tipocomprobante.AddAttribute (cellrt5, "text", 5);
			col_tipocomprobante.SortColumnId = (int) Col_proveedores.col_tipocomprobante;
			
			TreeViewColumn col_presu = new TreeViewColumn();
			CellRendererText cellrt6 = new CellRendererText();
			col_presu.Title = "Id Presupuesto";
			col_presu.PackStart(cellrt6, true);
			col_presu.AddAttribute (cellrt6, "text", 6);
			col_presu.SortColumnId = (int) Col_proveedores.col_presu;
			
			TreeViewColumn col_paq = new TreeViewColumn();
			CellRendererText cellrt7 = new CellRendererText();
			col_paq.Title = "Id Paquete";
			col_paq.PackStart(cellrt7, true);
			col_paq.AddAttribute (cellrt7, "text", 7);
			col_paq.SortColumnId = (int) Col_proveedores.col_paq;
			
			TreeViewColumn col_forma_pago = new TreeViewColumn();
			CellRendererText cellrt8 = new CellRendererText();
			col_forma_pago.Title = "Forma de Pago";
			col_forma_pago.PackStart(cellrt8, true);
			col_forma_pago.AddAttribute (cellrt8, "text", 8);
			col_forma_pago.SortColumnId = (int) Col_proveedores.col_forma_pago;
			
			TreeViewColumn col_valor_convenido = new TreeViewColumn();
			CellRendererText cellrt11 = new CellRendererText();
			col_valor_convenido.Title = "$ Convenio QX.";
			col_valor_convenido.PackStart(cellrt11, true);
			col_valor_convenido.AddAttribute (cellrt11, "text", 11);
						
			TreeViewColumn col_observaciones = new TreeViewColumn();
			CellRendererText cellrt12 = new CellRendererText();
			col_observaciones.Title = "Observaciones";
			col_observaciones.PackStart(cellrt12, true);
			col_observaciones.AddAttribute (cellrt12, "text", 12);
			
			TreeViewColumn col_generado = new TreeViewColumn();
			CellRendererText cellrt13 = new CellRendererText();
			col_generado.Title = "Creado en";
			col_generado.PackStart(cellrt13, true);
			col_generado.AddAttribute (cellrt13, "text", 13);
			
			TreeViewColumn col_montoxfact = new TreeViewColumn();
			CellRendererText cellrt14 = new CellRendererText();
			col_montoxfact.Title = "Pend. X Factura";
			col_montoxfact.PackStart(cellrt14, true);
			col_montoxfact.AddAttribute (cellrt14, "text", 14);
			
			TreeViewColumn col_montodefact = new TreeViewColumn();
			CellRendererText cellrt15 = new CellRendererText();
			col_montodefact.Title = "Monto Factura";
			col_montodefact.PackStart(cellrt15, true);
			col_montodefact.AddAttribute (cellrt15, "text", 15);
			
			TreeViewColumn col_nrofactura = new TreeViewColumn();
			CellRendererText cellrt16 = new CellRendererText();
			col_nrofactura.Title = "Nro. Fact.";
			col_nrofactura.PackStart(cellrt16, true);
			col_nrofactura.AddAttribute (cellrt16, "text", 16);
			
			TreeViewColumn col_fechafactura = new TreeViewColumn();
			CellRendererText cellrt17 = new CellRendererText();
			col_fechafactura.Title = "Fecha Fact.";
			col_fechafactura.PackStart(cellrt17, true);
			col_fechafactura.AddAttribute (cellrt17, "text", 17);
						
			lista_abonos.AppendColumn(col_abono);
			lista_abonos.AppendColumn(col_fecha_abono);
			lista_abonos.AppendColumn(col_valor_convenido);
			lista_abonos.AppendColumn(col_concepto);
			lista_abonos.AppendColumn(col_observaciones);
			lista_abonos.AppendColumn(col_id_creo);
			lista_abonos.AppendColumn(col_recibo);
			lista_abonos.AppendColumn(col_tipocomprobante);
			lista_abonos.AppendColumn(col_presu);
			lista_abonos.AppendColumn(col_paq);
			lista_abonos.AppendColumn(col_forma_pago);
			lista_abonos.AppendColumn(col_generado);
			lista_abonos.AppendColumn(col_montoxfact);
			lista_abonos.AppendColumn(col_montodefact);
			lista_abonos.AppendColumn(col_nrofactura);
			lista_abonos.AppendColumn(col_fechafactura);
		}
		
		void crea_treeview_comprserv()
		{
			treeViewEnginecomprserv = new TreeStore(typeof(string),
												typeof(string),
												typeof(string),
												typeof(string),
												typeof(string),
												typeof(string),
												typeof(string),
												typeof(string),
			                                    typeof(string));
			
			treeview_lista_comprserv.Model = treeViewEnginecomprserv;
			
			treeview_lista_comprserv.RulesHint = true;
			
			TreeViewColumn col_abono = new TreeViewColumn();
			cellr0 = new CellRendererText();
			col_abono.Title = "N° Comp.Serv.";
			col_abono.PackStart(cellr0, true);
			col_abono.AddAttribute (cellr0, "text", 0);
			col_abono.SortColumnId = (int) Col_proveedores.col_abono;
			
			TreeViewColumn col_fecha_abono = new TreeViewColumn();
			CellRendererText cellrt1 = new CellRendererText();
			col_fecha_abono.Title = "Fecha Compr.";
			col_fecha_abono.PackStart(cellrt1, true);
			col_fecha_abono.AddAttribute (cellrt1, "text", 1);
			col_fecha_abono.SortColumnId = (int) Col_proveedores.col_fecha_abono;
			
			TreeViewColumn col_concepto = new TreeViewColumn();
			CellRendererText cellrt2 = new CellRendererText();
			col_concepto.Title = "Concepto";
			col_concepto.PackStart(cellrt2, true);
			col_concepto.AddAttribute (cellrt2, "text", 2);
			col_concepto.SortColumnId = (int) Col_proveedores.col_concepto;
			
			TreeViewColumn col_id_creo = new TreeViewColumn();
			CellRendererText cellrt3 = new CellRendererText();
			col_id_creo.Title = "Id Quien Creo";
			col_id_creo.PackStart(cellrt3, true);
			col_id_creo.AddAttribute (cellrt3, "text", 3);
			col_id_creo.SortColumnId = (int) Col_proveedores.col_id_creo;
			
			TreeViewColumn col_recibo = new TreeViewColumn();
			CellRendererText cellrt4 = new CellRendererText();
			col_recibo.Title = "No. Recibo";
			col_recibo.PackStart(cellrt4, true);
			col_recibo.AddAttribute (cellrt4, "text", 4);
			col_recibo.SortColumnId = (int) Col_proveedores.col_recibo;
			
			TreeViewColumn col_presu = new TreeViewColumn();
			CellRendererText cellrt5 = new CellRendererText();
			col_presu.Title = "Id Presupuesto";
			col_presu.PackStart(cellrt5, true);
			col_presu.AddAttribute (cellrt5, "text", 5);
			col_presu.SortColumnId = (int) Col_proveedores.col_presu;
			
			TreeViewColumn col_paq = new TreeViewColumn();
			CellRendererText cellrt6 = new CellRendererText();
			col_paq.Title = "Id Paquete";
			col_paq.PackStart(cellrt6, true);
			col_paq.AddAttribute (cellrt6, "text", 6);
			col_paq.SortColumnId = (int) Col_proveedores.col_paq;
			
			TreeViewColumn col_forma_pago = new TreeViewColumn();
			CellRendererText cellrt7 = new CellRendererText();
			col_forma_pago.Title = "Observaciones";
			col_forma_pago.PackStart(cellrt7, true);
			col_forma_pago.AddAttribute (cellrt7, "text", 7);
			col_forma_pago.SortColumnId = (int) Col_proveedores.col_forma_pago;			
			
			treeview_lista_comprserv.AppendColumn(col_abono);
			treeview_lista_comprserv.AppendColumn(col_fecha_abono);
			treeview_lista_comprserv.AppendColumn(col_concepto);
			treeview_lista_comprserv.AppendColumn(col_id_creo);
			treeview_lista_comprserv.AppendColumn(col_recibo);
			treeview_lista_comprserv.AppendColumn(col_presu);
			treeview_lista_comprserv.AppendColumn(col_paq);
			treeview_lista_comprserv.AppendColumn(col_forma_pago);
		}
		
		void crea_treeview_pagare()
		{
			treeViewEnginepagare = new TreeStore(typeof(string),
												typeof(string),
												typeof(string),
												typeof(string),
												typeof(string),
												typeof(string),
												typeof(string),
												typeof(string),
			                                    typeof(string),
			                                    typeof(bool));
			
			treeview_lista_pagare.Model = treeViewEnginepagare;
			
			treeview_lista_pagare.RulesHint = true;
			
			TreeViewColumn col_abono = new TreeViewColumn();
			cellr0 = new CellRendererText();
			col_abono.Title = "$ Pagare"; // titulo de la cabecera de la columna, si está visible
			col_abono.PackStart(cellr0, true);
			col_abono.AddAttribute (cellr0, "text", 0);    // la siguiente columna será 1
			col_abono.SortColumnId = (int) Col_proveedores.col_abono;
			
			TreeViewColumn col_fecha_abono = new TreeViewColumn();
			CellRendererText cellrt1 = new CellRendererText();
			col_fecha_abono.Title = "Fech.Pagare";
			col_fecha_abono.PackStart(cellrt1, true);
			col_fecha_abono.AddAttribute (cellrt1, "text", 1); // la siguiente columna será 2
			col_fecha_abono.SortColumnId = (int) Col_proveedores.col_fecha_abono;
			
			TreeViewColumn col_concepto = new TreeViewColumn();
			CellRendererText cellrt2 = new CellRendererText();
			col_concepto.Title = "Fech.Vencimiento";
			col_concepto.PackStart(cellrt2, true);
			col_concepto.AddAttribute (cellrt2, "text", 2); // la siguiente columna será 3
			col_concepto.SortColumnId = (int) Col_proveedores.col_concepto;
			
			TreeViewColumn col_id_creo = new TreeViewColumn();
			CellRendererText cellrt3 = new CellRendererText();
			col_id_creo.Title = "Id Quien Creo";
			col_id_creo.PackStart(cellrt3, true);
			col_id_creo.AddAttribute (cellrt3, "text", 3);
			col_id_creo.SortColumnId = (int) Col_proveedores.col_id_creo;
			
			TreeViewColumn col_recibo = new TreeViewColumn();
			CellRendererText cellrt4 = new CellRendererText();
			col_recibo.Title = "No.Rec.Pagare";
			col_recibo.PackStart(cellrt4, true);
			col_recibo.AddAttribute (cellrt4, "text", 4);
			col_recibo.SortColumnId = (int) Col_proveedores.col_recibo;
			
			TreeViewColumn col_presu = new TreeViewColumn();
			CellRendererText cellrt5 = new CellRendererText();
			col_presu.Title = "Observacion 1";
			col_presu.PackStart(cellrt5, true);
			col_presu.AddAttribute (cellrt5, "text", 5);
			col_presu.SortColumnId = (int) Col_proveedores.col_presu;
			
			TreeViewColumn col_paq = new TreeViewColumn();
			CellRendererText cellrt6 = new CellRendererText();
			col_paq.Title = "Observacion 2";
			col_paq.PackStart(cellrt6, true);
			col_paq.AddAttribute (cellrt6, "text", 6);
			col_paq.SortColumnId = (int) Col_proveedores.col_paq;
			
			TreeViewColumn col_forma_pago = new TreeViewColumn();
			CellRendererText cellrt7 = new CellRendererText();
			col_forma_pago.Title = "Observacion 3";
			col_forma_pago.PackStart(cellrt7, true);
			col_forma_pago.AddAttribute (cellrt7, "text", 7);
			col_forma_pago.SortColumnId = (int) Col_proveedores.col_forma_pago;
			
			TreeViewColumn col_fecha_pago = new TreeViewColumn();
			CellRendererText cellrt8 = new CellRendererText();
			col_fecha_pago.Title = "Fecha Pago";
			col_fecha_pago.PackStart(cellrt8, true);
			col_fecha_pago.AddAttribute (cellrt8, "text", 8);
			col_fecha_pago.SortColumnId = (int) Col_proveedores.col_fecha_pago;
			
			TreeViewColumn col_pagado = new TreeViewColumn();
			CellRendererToggle cel_pagado = new CellRendererToggle();
			col_pagado.Title = "Pagado";
			col_pagado.PackStart(cel_pagado, true);
			col_pagado.AddAttribute (cel_pagado, "active", 9);
			//cel_pagado.Activatable = true;
			
			
			treeview_lista_pagare.AppendColumn(col_abono);
			treeview_lista_pagare.AppendColumn(col_fecha_abono);
			treeview_lista_pagare.AppendColumn(col_concepto);
			treeview_lista_pagare.AppendColumn(col_id_creo);
			treeview_lista_pagare.AppendColumn(col_recibo);
			treeview_lista_pagare.AppendColumn(col_presu);
			treeview_lista_pagare.AppendColumn(col_paq);
			treeview_lista_pagare.AppendColumn(col_forma_pago);
			treeview_lista_pagare.AppendColumn(col_fecha_pago);
			treeview_lista_pagare.AppendColumn(col_pagado);
		}
		
		enum Col_proveedores
		{
			col_abono,
			col_fecha_abono,
			col_concepto,
			col_id_creo,
			col_recibo,
			col_tipocomprobante,
			col_presu,
			col_paq,
			col_forma_pago,
			col_fecha_pago
		}
		
		void llenando_lista_de_abonos()
		{
			decimal total = 0;
			entry_total_abonos.Text = total.ToString().Trim();
			treeViewEngineabonos.Clear(); // Limpia el treeview cuando realiza una nueva busqueda
			
			NpgsqlConnection conexion; 
			conexion = new NpgsqlConnection (connectionString+nombrebd);
	        // Verifica que la base de datos este conectada
			try{
				conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand ();
					comando.CommandText = "SELECT id_abono,to_char(osiris_erp_abonos.id_abono,'9999999999') AS idabono,"+
								"folio_de_servicio,monto_de_abono_procedimiento,monto_de_abono_factura,numero_recibo_caja,osiris_erp_tipo_comprobante.pagar_factura,"+
								"to_char(numero_recibo_caja,'9999999999') AS recibocaja,numero_factura,osiris_erp_abonos.tipo_comprobante AS generadoen,id_quien_creo,observaciones,"+
								"monto_de_abono_procedimiento,to_char(osiris_erp_abonos.monto_de_abono_procedimiento,'9999999999.99') AS monto_abono_proc,"+
								"concepto_del_abono,fechahora_registro,to_char(osiris_erp_abonos.fechahora_registro,'yyyy-MM-dd HH:mi:ss') AS fecha_registro,"+
								"to_char(osiris_erp_abonos.fecha_abono,'dd-MM-yyyy') AS fechaabono,id_presupuesto,"+
								"to_char(osiris_erp_abonos.monto_x_facturar,'9999999999.99') AS montoxfacturar,(subtotal_facturado+iva_facturado) AS montofacturado," +
								"to_char(osiris_erp_abonos.fechahora_factura,'dd-MM-yyyy') AS fechafactura,"+
								"to_char(id_presupuesto,'9999999999') AS presupuesto,id_paquete,pago,abono,monto_convenio,osiris_erp_abonos.id_forma_de_pago,"+ 
								"to_char(id_paquete,'9999999999') AS paquete,osiris_erp_forma_de_pago.id_forma_de_pago," +
								"descripcion_forma_de_pago AS descripago,osiris_erp_abonos.id_tipo_comprobante,descripcion_tipo_comprobante "+
								"FROM osiris_erp_abonos,osiris_erp_forma_de_pago,osiris_erp_tipo_comprobante "+
								"WHERE osiris_erp_abonos.folio_de_servicio = '"+this.folioservicio.ToString()+"' "+
								"AND osiris_erp_abonos.id_forma_de_pago = osiris_erp_forma_de_pago.id_forma_de_pago "+
								"AND osiris_erp_abonos.eliminado = 'false' "+
								"AND osiris_erp_abonos.id_tipo_comprobante = osiris_erp_tipo_comprobante.id_tipo_comprobante "+
								"ORDER BY osiris_erp_abonos.folio_de_servicio;";
				//Console.WriteLine(comando.CommandText);
				NpgsqlDataReader lector = comando.ExecuteReader ();
				while (lector.Read()){
					treeViewEngineabonos.AppendValues ((string) lector["monto_abono_proc"],
													(string) lector["fechaabono"],
													(string) lector["concepto_del_abono"],
													(string) lector["id_quien_creo"],
													(string) lector["recibocaja"],
					                                (string) lector["descripcion_tipo_comprobante"],
													(string) lector["presupuesto"],
													(string) lector["paquete"],
													(string) lector["descripago"],
					                                (bool) lector["pago"],
					                                (bool) lector["abono"],
					                                float.Parse(lector["monto_convenio"].ToString()).ToString("F"),
													lector["observaciones"].ToString().Trim(),
													lector["generadoen"].ToString().Trim(),
					                                lector["montoxfacturar"].ToString().Trim(),
					                                lector["montofacturado"].ToString().Trim(),
					                                lector["numero_factura"].ToString().Trim(),
					                                lector["fechafactura"].ToString().Trim(),
					                                lector["id_tipo_comprobante"],
					                                lector["pagar_factura"]   
					                                   );
					total += decimal.Parse((string) lector["monto_abono_proc"]);
					entry_total_abonos.Text = total.ToString("F");
				}
			}catch (NpgsqlException ex){
   				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
								MessageType.Error,ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
				msgBoxError.Run ();			msgBoxError.Destroy();
			}
			conexion.Close ();
		}
		
		void llenando_lista_comprobante()
		{
			decimal total = 0;
			treeViewEnginecomprserv.Clear();	// Limpia el treeview cuando realiza una nueva busqueda
			NpgsqlConnection conexion; 
			conexion = new NpgsqlConnection (connectionString+nombrebd);
	        // Verifica que la base de datos este conectada
			try{
				conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand ();
					comando.CommandText = "SELECT to_char(numero_comprobante_servicio,'99999999999') AS reciboservicio,id_quien_creo," +
										"to_char(osiris_erp_comprobante_servicio.fecha_comprobante,'yyyy-MM-dd') AS fechacomprobante,concepto_del_comprobante "+
									"FROM osiris_erp_comprobante_servicio "+
									"WHERE osiris_erp_comprobante_servicio.eliminado = 'false' "+
									"AND osiris_erp_comprobante_servicio.folio_de_servicio = '"+folioservicio.ToString()+"' ";
				//Console.WriteLine(comando.CommandText);
				NpgsqlDataReader lector = comando.ExecuteReader ();
				while (lector.Read()){	
					treeViewEnginecomprserv.AppendValues ((string) lector["reciboservicio"],
					                                   (string) lector["fechacomprobante"],
					                                   (string) lector["concepto_del_comprobante"],
					                                   (string) lector["id_quien_creo"],
					                                   " ",
					                                   " ");
					//total += decimal.Parse((string) lector["abono"]);
					//entry_total_abonos.Text = total.ToString();
				}
			}catch (NpgsqlException ex){
   				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
								MessageType.Error,ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
				msgBoxError.Run ();			msgBoxError.Destroy();
			}
			conexion.Close ();
		}
		
		void llenando_lista_pagare()
		{
			decimal total = 0;
			treeViewEnginepagare.Clear();	// Limpia el treeview cuando realiza una nueva busqueda
			NpgsqlConnection conexion; 
			conexion = new NpgsqlConnection (connectionString+nombrebd);
	        // Verifica que la base de datos este conectada
			try{
				conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand ();
					comando.CommandText = "SELECT to_char(numero_comprobante_pagare,'99999999999') AS reciboservicio,id_quien_creo," +
										"to_char(osiris_erp_comprobante_pagare.fecha_comprobante,'yyyy-MM-dd') AS fechacomprobante,"+
										"to_char(osiris_erp_comprobante_pagare.monto_pagare,'9999999999.99') AS montopagare, "+
										"to_char(osiris_erp_comprobante_pagare.fecha_vencimiento_pagare,'yyyy-MM-dd') AS fechavencimientopagare,"+
										"concepto_del_comprobante,observaciones,observaciones2,observaciones3," +
										"to_char(osiris_erp_comprobante_pagare.fechahora_pago,'yyyy-MM-dd') AS fechapago,pagado "+
									"FROM osiris_erp_comprobante_pagare "+
									"WHERE osiris_erp_comprobante_pagare.eliminado = 'false' "+
									"AND osiris_erp_comprobante_pagare.folio_de_servicio = '"+this.folioservicio.ToString()+"' ";
				//Console.WriteLine(comando.CommandText);
				NpgsqlDataReader lector = comando.ExecuteReader ();
				string fechadelpago;
				while (lector.Read()){
					if(lector["fechapago"].ToString().Trim() == "2000-01-01"){
						fechadelpago = "";	
					}else{
						fechadelpago = (string) lector["fechapago"];
					}
					treeViewEnginepagare.AppendValues ((string) lector["montopagare"],
					                                   (string) lector["fechacomprobante"],
					                                   (string) lector["fechavencimientopagare"],
					                                   (string) lector["id_quien_creo"],
					                                   (string) lector["reciboservicio"],
					                                   (string) lector["observaciones"],
					                                   (string) lector["observaciones2"],
					                                   (string) lector["observaciones3"],
					                                   fechadelpago,
					                                   (bool) lector["pagado"]);
					//total += decimal.Parse((string) lector["abono"]);
					//entry_total_abonos.Text = total.ToString();
				}
			}catch (NpgsqlException ex){
   				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
								MessageType.Error,ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
				msgBoxError.Run ();			msgBoxError.Destroy();
			}
			conexion.Close ();
		}
		
		void on_checkbutton_nuevo_abono_clicked(object sender, EventArgs args)
		{
			if(checkbutton_nuevo_abono.Active == true) { 
				MessageDialog msgBox = new MessageDialog (MyWin,DialogFlags.Modal,
				MessageType.Question,ButtonsType.YesNo,"¿ Esta seguro de querer realizar un nuevo abono?");
				ResponseType miResultado = (ResponseType)
				msgBox.Run ();				msgBox.Destroy();
	 			if (miResultado == ResponseType.Yes){
	 				llenado_formapago("nuevo",0,"");
	 				entry_monto_abono.Sensitive = true;
	 				entry_recibo_caja.Sensitive = true;
					entry_presupuesto.Sensitive = true;
					entry_paquete.Sensitive = true;
					entry_dia.Sensitive = true;
					entry_dia.Text = DateTime.Now.ToString("dd");
					entry_mes.Sensitive = true;
					entry_mes.Text = DateTime.Now.ToString("MM");
					entry_ano.Sensitive = true;
					entry_ano.Text = DateTime.Now.ToString("yyyy");
					entry_concepto_abono.Sensitive = true;
					button_guardar.Sensitive = true;
					button_imprimir.Sensitive = true;
					this.button_resumen.Sensitive = true;
					this.combobox_formapago.Sensitive = true;
					combobox_tipocomprobante.Sensitive = true;
					entry_recibo_caja.Text = (string) classpublic.lee_ultimonumero_registrado("osiris_erp_abonos","numero_recibo_caja"," WHERE id_tipo_comprobante = '"+idtipocomprobante.ToString().Trim()+"' ");
				}else{
					checkbutton_nuevo_abono.Active = false;
				}
			}
		}
				
		void on_button_guardar_clicked(object sender, EventArgs args)
		{
			Gtk.MessageDialog msgBox;
			bool verifica_tipocomprobante = true;
			if(checkbutton_nuevo_abono.Active == true){
				msgBox = new MessageDialog (MyWin,DialogFlags.Modal,MessageType.Question,
										ButtonsType.YesNo,"¿ Desea grabar esta infomacion ?");
				ResponseType miResultado = (ResponseType)msgBox.Run ();
				msgBox.Destroy();
				if (miResultado == ResponseType.Yes){
					if(entry_monto_convenio.Text.Trim() != ""){
						if(float.Parse(entry_monto_convenio.Text.Trim()) != 0){
							
							if(int.Parse(idtipocomprobante.ToString().Trim()) == 6){
								if(entry_valor_x_facturar.Text.ToString().Trim() == ""){
									verifica_tipocomprobante = false;
								}else{
									if(float.Parse(entry_valor_x_facturar.Text.ToString().Trim()) == 0){
										verifica_tipocomprobante = false;
									}
								}
								if(verifica_tipocomprobante == false){
									msgBox = new MessageDialog (MyWin,DialogFlags.Modal,
									MessageType.Error,ButtonsType.Ok,"Debe capturar EL MONTO del valor que se va a FACTURAR, verifique...");
									msgBox.Run ();msgBox.Destroy();
								}
							}else{
								if(int.Parse(idtipocomprobante.ToString().Trim()) == 1){
									verifica_tipocomprobante = false;
									msgBox = new MessageDialog (MyWin,DialogFlags.Modal,
									MessageType.Error,ButtonsType.Ok,"NO Selecciono un TIPO DE COMPROBANTE, verifique...");
									msgBox.Run ();msgBox.Destroy();
								}
							}
							if (idformadepago <= 1 ){
								verifica_tipocomprobante = false;
								msgBox = new MessageDialog (MyWin,DialogFlags.Modal,
									MessageType.Question,ButtonsType.Ok,"NO Selecciono la FORMA DE PAGO, verifique....");
								msgBox.Run ();		msgBox.Destroy();
							}							
							
							if(verifica_tipocomprobante){
								NpgsqlConnection conexion4;
								conexion4 = new NpgsqlConnection (connectionString+nombrebd);
			    	        	// Verifica que la base de datos este conectada
			    	        	try{
				    	        	conexion4.Open ();
									NpgsqlCommand comando4; 
									comando4 = conexion4.CreateCommand ();
					 				comando4.CommandText = "SELECT numero_recibo_caja,folio_de_servicio "+
													"FROM osiris_erp_abonos "+
													"WHERE numero_recibo_caja = '"+this.entry_recibo_caja.Text+"' "+
													"AND id_tipo_comprobante = '"+idtipocomprobante+"' "+
													"LIMIT 1 ;";
					 					
				 					NpgsqlDataReader lector4 = comando4.ExecuteReader ();
											
			               			if(lector4.Read() || (string) idtipocomprobante == "1"){
			               				msgBox = new MessageDialog (MyWin,DialogFlags.Modal,
										MessageType.Info,ButtonsType.Ok,"Este recibo de caja ya existe o no tiene tipo de Comprobante, verifique...");
										msgBox.Run ();msgBox.Destroy();
			               			}else{
					               		NpgsqlConnection conexion;
										conexion = new NpgsqlConnection (connectionString+nombrebd);
					    	        	// Verifica que la base de datos este conectada
					    	        	try{
											conexion.Open ();
											NpgsqlCommand comando; 
											comando = conexion.CreateCommand ();
								 			comando.CommandText = "INSERT INTO osiris_erp_abonos("+
															  	"monto_de_abono_procedimiento, "+
																"numero_recibo_caja,"+
																"id_quien_creo,"+
																"concepto_del_abono,"+
																"observaciones,"+
																"observaciones2," +
																"observaciones3,"+
																"fechahora_registro,"+
																"fecha_abono,"+
																"id_presupuesto,"+
																"id_paquete ,"+
																"id_forma_de_pago,"+
																"id_tipo_comprobante,"+
																"tipo_comprobante,"+
																"abono,"+
																"monto_convenio,"+
																"folio_de_servicio," +
																"id_tipo_paciente," +
																"pid_paciente," +
																"monto_x_facturar" +
																")"+
																"VALUES ('"+
						 										entry_monto_abono.Text.Trim()+"','"+
						 										(string) this.entry_recibo_caja.Text.Trim().ToUpper()+"','"+										  
						 										LoginEmpleado+"','"+
						 										"ABONO A PROCEDIMIENTO','"+
																entry_concepto_abono.Text.ToString().Trim().ToUpper()+"','"+
																entry_observaciones2.Text.ToString().ToUpper().Trim()+"','"+
																entry_observaciones3.Text.ToString().ToUpper().Trim()+"','"+
						 										DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"','"+
						 										(string) this.entry_ano.Text+" "+this.entry_mes.Text+" "+this.entry_dia.Text+"','"+
						 										(string) this.entry_presupuesto.Text.Trim().ToUpper()+"','"+
						 										(string) this.entry_paquete.Text.Trim().ToUpper()+"','"+
																idformadepago.ToString()+"','"+
						 										idtipocomprobante+"','"+
																"ABONO"+"','"+
																"true"+"','"+
																entry_monto_convenio.Text.ToString().Trim()+"','"+
																folioservicio+"','"+
																id_tipopaciente.ToString().Trim()+"','"+
						 										PidPaciente+"','"+
																entry_valor_x_facturar.Text.ToString().Trim()+"');";
					 						comando.ExecuteNonQuery();    	    	       	comando.Dispose();
					 						
					 						NpgsqlConnection conexion2; 
											conexion2 = new NpgsqlConnection (connectionString+nombrebd);
																						
											string reservafolio = "true";
											if(idtipocomprobante == "6"){
												reservafolio = "false";
												
											}else{
												reservafolio = "true";
											}
												
					    	        		//Verifica que la base de datos este conectada
					    	        		try{
								    	       	conexion2.Open ();
												NpgsqlCommand comando2; 
												comando2 = conexion2.CreateCommand ();
									 			comando2.CommandText = "UPDATE osiris_erp_cobros_enca SET tiene_abono = 'true',"+
									 										"total_abonos = total_abonos + '"+entry_monto_abono.Text+"', "+
																			"monto_convenio = '"+entry_monto_convenio.Text+"', "+
																			"reservacion = '"+reservafolio+"' "+
																			"WHERE folio_de_servicio = '"+this.folioservicio.ToString()+"' ;";
									 			//Console.WriteLine(comando2.CommandText);		
								 				comando2.ExecuteNonQuery();    	    	       	comando2.Dispose();
											}catch(NpgsqlException ex){
								   				MessageDialog msgBoxError5 = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
																	MessageType.Error,ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
												msgBoxError5.Run ();					msgBoxError5.Destroy();
											}
								       		conexion2.Close ();
							 				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
																MessageType.Info,ButtonsType.Close,"El Abono se guardo con exito");
											msgBoxError.Run ();					msgBoxError.Destroy();
											idtipocomprobante = "1";
					    	        		llenado_tipo_comprobante();
											llenando_lista_de_abonos();
											crearlog_cred_cobranza();
										}catch(NpgsqlException ex){
								   					MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
																	MessageType.Error,ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
													msgBoxError.Run ();					msgBoxError.Destroy();
					   					}
								       	conexion.Close ();
					       				entry_monto_abono.Sensitive = false;
										entry_recibo_caja.Sensitive = false;
										entry_presupuesto.Sensitive = false;
										entry_paquete.Sensitive = false;
										entry_dia.Sensitive = false;
										entry_mes.Sensitive = false;
										entry_ano.Sensitive = false;
										entry_concepto_abono.Sensitive = false;
										combobox_formapago.Sensitive = false;
										entry_monto_abono.Text = "";
										entry_recibo_caja.Text = "";
										entry_presupuesto.Text = "";
										entry_paquete.Text = "";
										entry_dia.Text = DateTime.Now.ToString("dd");
										entry_mes.Text = DateTime.Now.ToString("MM");
										entry_ano.Text = DateTime.Now.ToString("yyyy");
										entry_dia.Text = "";
										entry_mes.Text = "";
										entry_ano.Text = "";
										entry_concepto_abono.Text = "";
										this.checkbutton_nuevo_abono.Active = false;
										this.button_guardar.Sensitive = false;
									}
								}catch(NpgsqlException ex){
				   					MessageDialog msgBoxError5 = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
													MessageType.Error,ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
									msgBoxError5.Run ();					msgBoxError5.Destroy();
				       			}
				       			conexion4.Close();
							}
						}else{
							MessageDialog msgBoxError5 = new MessageDialog (MyWinError,DialogFlags.Modal,
									MessageType.Error,ButtonsType.Ok,"Verifique el monto del Convenio...");						
							msgBoxError5.Run ();msgBoxError5.Destroy();
						}
					}else{						
						MessageDialog msgBoxError5 = new MessageDialog (MyWinError,DialogFlags.Modal,
									MessageType.Error,ButtonsType.Ok,"Verifique el monto del Convenio...");						
						msgBoxError5.Run ();msgBoxError5.Destroy();
					}
				}
	    	}
	    } 
		
		void crearlog_cred_cobranza()
		{
			if((bool) tienepagare){
				NpgsqlConnection conexion; 
				conexion = new NpgsqlConnection (connectionString+nombrebd);
				//this.PidPaciente
				try{
					conexion.Open ();
					NpgsqlCommand comando; 
					comando = conexion.CreateCommand ();
					comando.CommandText = "INSERT INTO osiris_erp_gestcobrzmov(" +
								"folio_de_servicio," +
								"pid_paciente," +
								"fechahora_creacion," +
								"nota," +
								"telefono," +
								"id_tipo_paciente" +
								") VALUES ('" +
								folioservicio.ToString().Trim()+"','"+
								PidPaciente.ToString().Trim()+"','"+
								DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"','"+
								"SE GENERO UN ABONO EN EL ESTADO DE CUENTA $"+entry_monto_abono.Text.Trim()+"','"+
								telefono_paciente.ToString().Trim()+"','"+
								id_tipopaciente.ToString().Trim()+"')";
							
					//Console.WriteLine(comando.CommandText);
					comando.ExecuteNonQuery();
					comando.Dispose();					
				}catch (NpgsqlException ex){
					MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
																		MessageType.Error, 
																		ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
					msgBoxError.Run ();				conexion.Close();		
				}
				conexion.Close();
			}			
		}
		
		void llenado_formapago(string tipo_,int idformapago_, string descrippago_ )
		{
			combobox_formapago.Clear();
			CellRendererText cell3 = new CellRendererText();
			combobox_formapago.PackStart(cell3, true);
			combobox_formapago.AddAttribute(cell3,"text",0);	        
			ListStore store5 = new ListStore( typeof (string), typeof (int));
			combobox_formapago.Model = store5;
			if(tipo_ == "selecciona"){
				store5.AppendValues ( (string) descrippago_,(int) idformapago_ );
			}	      
	        NpgsqlConnection conexion; 
			conexion = new NpgsqlConnection (connectionString+nombrebd);
            // Verifica que la base de datos este conectada
			try{
				conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand ();
               	comando.CommandText = "SELECT * FROM osiris_erp_forma_de_pago "+
									"WHERE proveedor = false " +
									"AND activo_formapago = 'true' "+	
									"ORDER BY clave_sat;";
				NpgsqlDataReader lector = comando.ExecuteReader ();
               	while (lector.Read()){
					store5.AppendValues ((string) lector["descripcion_forma_de_pago"],
									 	(int) lector["id_forma_de_pago"] );
				}
			}catch (NpgsqlException ex){
				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
										MessageType.Error,ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
				msgBoxError.Run ();				msgBoxError.Destroy();
			}
			conexion.Close ();
	        
			TreeIter iter5;
			if (store5.GetIterFirst(out iter5)){
				combobox_formapago.SetActiveIter (iter5);
			}
			combobox_formapago.Changed += new EventHandler (onComboBoxChanged_formapago);
		}
		
		void onComboBoxChanged_formapago (object sender, EventArgs args)
		{
			ComboBox combobox_formapago = sender as ComboBox;
			if (sender == null) {return;}
			TreeIter iter;
			if (combobox_formapago.GetActiveIter (out iter)){ 
				idformadepago = (int) combobox_formapago.Model.GetValue(iter,1);
			}
		}
		
		void on_button_cancela_abonopago_clicked(object sender, EventArgs args)
		{
			if((string) classpublic.lee_registro_de_tabla("osiris_empleado","acceso_cancela_abopago","WHERE acceso_cancela_abopago = 'true' AND login_empleado = '"+LoginEmpleado+"' ","acceso_cancela_abopago","bool") == "True"){
				TreeModel model;
				TreeIter iterSelected;
				if (lista_abonos.Selection.GetSelected(out model, out iterSelected)){
					Glade.XML gxml = new Glade.XML (null, "registro_admision.glade", "cancelador_folios", null);
					gxml.Autoconnect (this);
					cancelador_folios.Show();
					entry_folio.IsEditable = false;
					entry_folio.Text = (string) model.GetValue(iterSelected, 4).ToString().Trim();
					button_cancelar.Clicked += new EventHandler(on_button_cancelar_clicked);
					button_salir.Clicked += new EventHandler(on_cierraventanas_clicked);
				}				
			}else{
				MessageDialog msgBox = new MessageDialog (MyWinError,DialogFlags.Modal,
									MessageType.Info,ButtonsType.Ok,"No tiene Permiso para esta Opcion");
				msgBox.Run ();msgBox.Destroy();
			}
		}
		
		void on_button_cancelar_clicked(object sender, EventArgs args)
		{
			TreeModel model;
			TreeIter iterSelected;
			if (lista_abonos.Selection.GetSelected(out model, out iterSelected)){
				MessageDialog msgBox = new MessageDialog (MyWin,DialogFlags.Modal,MessageType.Question,
									ButtonsType.YesNo,"¿ Desea Eliminar esta Pago/Abono ?");
				ResponseType miResultado = (ResponseType)msgBox.Run ();
				msgBox.Destroy();
			 	if(miResultado == ResponseType.Yes){
					if(entry_motivo.Text.Trim() != ""){
						string[,] parametros;
						object[] paraobj;
						parametros = new [,] {
							{"eliminado = '","true',"},
							{"fechahora_eliminado = '",DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"',"},
							{"id_quien_elimino = '",LoginEmpleado+"',"},
							{"motivo_eliminacion = '",entry_motivo.Text.Trim().ToUpper()+"/MONTO DEL COMPROBANTE $"+model.GetValue(iterSelected,0).ToString().Trim()+"',"},
							{"monto_de_abono_procedimiento = '","0.0' "},
							{"WHERE osiris_erp_abonos.numero_recibo_caja = '",entry_folio.Text.Trim()+"' "},
							{"AND id_tipo_comprobante = '",model.GetValue(iterSelected, 18).ToString().Trim()+"' "},
							{"AND osiris_erp_abonos.folio_de_servicio = '",folioservicio.ToString()+"';"},
						};
						paraobj = new [] {entry_folio_servicio};
						new osiris.update_registro("osiris_erp_abonos",parametros,paraobj);

						if (model.GetValue (iterSelected, 13).ToString ().Trim () == "CAJA") {
							parametros = new [,] {
								{"total_pago = total_pago - '",model.GetValue(iterSelected,0).ToString().Trim()+"' "},
								{"WHERE osiris_erp_cobros_enca.folio_de_servicio = '",folioservicio.ToString()+"';"}
							};
							paraobj = new [] {entry_folio_servicio};
							new osiris.update_registro("osiris_erp_cobros_enca",parametros,paraobj);
						}

						if (model.GetValue (iterSelected, 13).ToString ().Trim () == "ABONO") {
							parametros = new [,] {
								{"total_abonos = total_abonos - '",model.GetValue(iterSelected,0).ToString().Trim()+"' "},
								{"WHERE osiris_erp_cobros_enca.folio_de_servicio = '",folioservicio.ToString()+"';"}
							};
							paraobj = new [] {entry_folio_servicio};
							new osiris.update_registro("osiris_erp_cobros_enca",parametros,paraobj);
						}
						cancelador_folios.Destroy ();
					}else{
						msgBox = new MessageDialog (MyWinError,DialogFlags.Modal,
										MessageType.Info,ButtonsType.Ok,"Especifique el motivo de la eliminacion");
						msgBox.Run ();msgBox.Destroy();	
					}
				}
			}
		}
		
		void on_button_cancelar_compserv_clicked(object sender, EventArgs args)
		{
			if((string) classpublic.lee_registro_de_tabla("osiris_empleado","acceso_cancela_compserv","WHERE acceso_cancela_compserv = 'true' AND login_empleado = '"+LoginEmpleado+"' ","acceso_cancela_compserv","bool") == "True"){
				TreeModel model;
				TreeIter iterSelected;
				if (treeview_lista_comprserv.Selection.GetSelected(out model, out iterSelected)){
					Glade.XML gxml = new Glade.XML (null, "registro_admision.glade", "cancelador_folios", null);
					gxml.Autoconnect (this);
					cancelador_folios.Show();					
					entry_folio.IsEditable = false;
					entry_folio.Text = (string) model.GetValue(iterSelected, 0).ToString().Trim();
					button_cancelar.Clicked += new EventHandler(on_button_cancela_compserv_clicked);
					button_salir.Clicked += new EventHandler(on_cierraventanas_clicked);
				}				
			}else{
				MessageDialog msgBox = new MessageDialog (MyWinError,DialogFlags.Modal,
									MessageType.Info,ButtonsType.Ok,"No tiene Permiso para esta Opcion");
				msgBox.Run ();msgBox.Destroy();
			}
		}
		
		void on_button_cancela_compserv_clicked(object sender, EventArgs args)
		{
			MessageDialog msgBox = new MessageDialog (MyWin,DialogFlags.Modal,MessageType.Question,
								ButtonsType.YesNo,"¿ Desea Cancelar el Comprobante de Servicio Seleccionado... ?");
			ResponseType miResultado = (ResponseType)msgBox.Run ();
			msgBox.Destroy();
		 	if(miResultado == ResponseType.Yes){
				if(entry_motivo.Text.Trim() != ""){
					Npgsql.NpgsqlConnection conexion;
					conexion = new NpgsqlConnection(connectionString+nombrebd);
					try{
						conexion.Open();
						NpgsqlCommand comando;
						comando = conexion.CreateCommand();
						comando.CommandText = "UPDATE osiris_erp_comprobante_servicio SET "+
													"eliminado = 'true' , "+
													"fechahora_eliminado = '"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"',"+
													"motivo_eliminacion = '"+entry_motivo.Text.Trim().ToUpper()+"',"+
													"id_quien_elimino = '"+LoginEmpleado+"' "+
													"WHERE osiris_erp_comprobante_servicio.numero_comprobante_servicio = '"+entry_folio.Text.Trim()+"' " +
													"AND osiris_erp_comprobante_servicio.folio_de_servicio = '"+folioservicio.ToString()+"' ";
						comando.ExecuteNonQuery();                  comando.Dispose();
						//Console.WriteLine(comando.CommandText);
						msgBox = new MessageDialog (MyWin,DialogFlags.Modal,MessageType.Info,
																ButtonsType.Ok,"El Comprobante de Servicio se CANCELO con exito!!");
						msgBox.Run();				msgBox.Destroy();
						cancelador_folios.Destroy();
						llenando_lista_comprobante();
					}catch(Npgsql.NpgsqlException ex){
						Console.WriteLine ("PostgresSQL error: {0}",ex.Message);
						MessageDialog msgBox1 = new MessageDialog (MyWin,DialogFlags.Modal,MessageType.Error,
												ButtonsType.Ok,"PostgresSQL error: {0}",ex.Message);
						msgBox1.Run();				msgBox1.Destroy();
					}
				}else{
					msgBox = new MessageDialog (MyWinError,DialogFlags.Modal,
									MessageType.Info,ButtonsType.Ok,"Especifique el motivo de la eliminacion");
					msgBox.Run ();msgBox.Destroy();	
				}
			}
		}
		
		void on_button_cancelar_pagare_clicked(object sender, EventArgs args)
		{
			if((string) classpublic.lee_registro_de_tabla("osiris_empleado","acceso_cancela_pagare","WHERE acceso_cancela_pagare = 'true' AND login_empleado = '"+LoginEmpleado+"' ","acceso_cancela_pagare","bool") == "True"){
				TreeModel model;
				TreeIter iterSelected;
				if (treeview_lista_pagare.Selection.GetSelected(out model, out iterSelected)){
					Glade.XML gxml = new Glade.XML (null, "registro_admision.glade", "cancelador_folios", null);
					gxml.Autoconnect (this);
					cancelador_folios.Show();
					entry_folio.IsEditable = false;
					entry_folio.Text = (string) model.GetValue(iterSelected, 4).ToString().Trim();
					button_cancelar.Clicked += new EventHandler(on_button_elimina_pagare_clicked);
					button_salir.Clicked += new EventHandler(on_cierraventanas_clicked);
				}				
			}else{
				MessageDialog msgBox = new MessageDialog (MyWinError,DialogFlags.Modal,
									MessageType.Info,ButtonsType.Ok,"No tiene Permiso para esta Opcion");
				msgBox.Run ();msgBox.Destroy();
			}
		}
		
		void on_button_pagar_pagare_clicked(object sender, EventArgs args)
		{
			MessageDialog msgBox;
			if((string) classpublic.lee_registro_de_tabla("osiris_empleado","acceso_pagar_pagare","WHERE acceso_pagar_pagare = 'true' AND login_empleado = '"+LoginEmpleado+"' ","acceso_pagar_pagare","bool") == "True"){
				TreeModel model;
				TreeIter iterSelected;
				if (treeview_lista_pagare.Selection.GetSelected(out model, out iterSelected)){
					if(!(bool) model.GetValue(iterSelected, 9)){
						msgBox = new MessageDialog (MyWin,DialogFlags.Modal,MessageType.Question,
										ButtonsType.YesNo,"¿ Desea PAGAR el Pagare Seleccionado ?");
						ResponseType miResultado = (ResponseType)msgBox.Run ();
						msgBox.Destroy();
				 		if(miResultado == ResponseType.Yes){						
							//(string) model.GetValue(iterSelected, 4).ToString().Trim();
							Npgsql.NpgsqlConnection conexion;
							conexion = new NpgsqlConnection(connectionString+nombrebd);
							try{
								conexion.Open();
								NpgsqlCommand comando;
								comando = conexion.CreateCommand();
								comando.CommandText = "UPDATE osiris_erp_comprobante_pagare SET "+
														"pagado = 'true',"+
														"fechahora_pago = '"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"',"+
														"id_quien_pago = '"+LoginEmpleado+"' "+
														"WHERE osiris_erp_comprobante_pagare.numero_comprobante_pagare = '"+(string) model.GetValue(iterSelected, 4).ToString().Trim()+"' " +
														"AND osiris_erp_comprobante_pagare.folio_de_servicio = '"+folioservicio.ToString()+"' ";
								comando.ExecuteNonQuery();                  comando.Dispose();
								NpgsqlConnection conexion1; 
								conexion1 = new NpgsqlConnection (connectionString+nombrebd);
								// Verifica que la base de datos este conectada
								try{
									conexion1.Open ();
									NpgsqlCommand comando1; 
									comando1 = conexion1.CreateCommand ();
					 				comando1.CommandText = "UPDATE osiris_erp_cobros_enca "+
														"SET reservacion = 'false'," +
														"pagare = 'false' "+
														"WHERE folio_de_servicio = '"+folioservicio.ToString()+"';";
									//Console.WriteLine(comando1.CommandText);
					 				comando1.ExecuteNonQuery();    	    	       	comando1.Dispose();
								}catch(NpgsqlException ex){
				   					MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
													MessageType.Error,ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
									msgBoxError.Run ();					msgBoxError.Destroy();
				       			}
				       			conexion1.Close();
								
								//Console.WriteLine(comando.CommandText);
								msgBox = new MessageDialog (MyWin,DialogFlags.Modal,MessageType.Info,
																		ButtonsType.Ok,"El Nº de Pagare se PAGO con exito!!");
								msgBox.Run();				msgBox.Destroy();
								llenando_lista_pagare();
							}catch(Npgsql.NpgsqlException ex){
								Console.WriteLine ("PostgresSQL error: {0}",ex.Message);
								MessageDialog msgBox1 = new MessageDialog (MyWin,DialogFlags.Modal,MessageType.Error,
														ButtonsType.Ok,"PostgresSQL error: {0}",ex.Message);
								msgBox1.Run();				msgBox1.Destroy();
							}
						}
					}else{
						msgBox = new MessageDialog (MyWin,DialogFlags.Modal,MessageType.Error,
												ButtonsType.Ok,"El Pagare ya se Encuentra PAGADO, verifique...!!");
						msgBox.Run();				msgBox.Destroy();
					}
				}
			}else{
				msgBox = new MessageDialog (MyWinError,DialogFlags.Modal,
									MessageType.Info,ButtonsType.Ok,"No tiene Permiso para esta Opcion");
				msgBox.Run ();msgBox.Destroy();
			}
		}
		
		void on_button_elimina_pagare_clicked(object sender, EventArgs args)
		{
			MessageDialog msgBox = new MessageDialog (MyWin,DialogFlags.Modal,MessageType.Question,
								ButtonsType.YesNo,"¿ Desea CANCELAR el Pagare Seleccionado ?");
			ResponseType miResultado = (ResponseType)msgBox.Run ();
			msgBox.Destroy();
		 	if(miResultado == ResponseType.Yes){
				if(entry_motivo.Text.Trim() != ""){
					Npgsql.NpgsqlConnection conexion;
					conexion = new NpgsqlConnection(connectionString+nombrebd);
					try{
						conexion.Open();
						NpgsqlCommand comando;
						comando = conexion.CreateCommand();
						comando.CommandText = "UPDATE osiris_erp_comprobante_pagare SET "+
													"eliminado = 'true' , "+
													"fechahora_eliminado = '"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"',"+
													"motivo_eliminacion = '"+entry_motivo.Text.Trim().ToUpper()+"',"+
													"id_quien_elimino = '"+LoginEmpleado+"' "+
													"WHERE osiris_erp_comprobante_pagare.numero_comprobante_pagare = '"+entry_folio.Text.Trim()+"' " +
													"AND osiris_erp_comprobante_pagare.folio_de_servicio = '"+folioservicio.ToString()+"' ";
						comando.ExecuteNonQuery();                  comando.Dispose();
						//Console.WriteLine(comando.CommandText);
						msgBox = new MessageDialog (MyWin,DialogFlags.Modal,MessageType.Info,
																ButtonsType.Ok,"El Nº de Pagare se CANCELO con exito!!");
						msgBox.Run();				msgBox.Destroy();
						cancelador_folios.Destroy();
						llenando_lista_pagare();
					}catch(Npgsql.NpgsqlException ex){
						Console.WriteLine ("PostgresSQL error: {0}",ex.Message);
						MessageDialog msgBox1 = new MessageDialog (MyWin,DialogFlags.Modal,MessageType.Error,
												ButtonsType.Ok,"PostgresSQL error: {0}",ex.Message);
						msgBox1.Run();				msgBox1.Destroy();
					}
				}else{
					msgBox = new MessageDialog (MyWinError,DialogFlags.Modal,
									MessageType.Info,ButtonsType.Ok,"Especifique el motivo de la eliminacion");
					msgBox.Run ();msgBox.Destroy();	
				}
			}
		}
		
		void on_button_fact_comprobante_clicked(object sender, EventArgs args)
		{
			TreeModel model;
			TreeIter iterSelected;
			Gtk.MessageDialog msgBox;
			if (lista_abonos.Selection.GetSelected(out model, out iterSelected)){
				if((string) model.GetValue(iterSelected, 16).ToString().Trim() == ""){
					Glade.XML gxml = new Glade.XML (null, "caja.glade", "factura_comprobante_caja", null);
					gxml.Autoconnect (this);
			        factura_comprobante_caja.Show();				
					
					entry_nro_recibocaja.Text = (string) model.GetValue(iterSelected, 4).ToString().Trim();
					entry_fech_recibocaja.Text = (string) model.GetValue(iterSelected, 1).ToString().Trim();
					entry_monto_recibocaja.Text = (string) model.GetValue(iterSelected, 0).ToString().Trim();
					idtipocomprobante = (string) model.GetValue(iterSelected, 18).ToString().Trim();
					pagarfactura = (bool) model.GetValue(iterSelected, 19);
					if((int) model.GetValue(iterSelected, 18) == 6){
						entry_monto_recibocaja.Text = (string) model.GetValue(iterSelected, 14).ToString().Trim();
					}
					button_cargar_xml.Clicked += new EventHandler(on_button_cargar_xml_clicked);
					button_salir.Clicked += new EventHandler(on_cierraventanas_clicked);
					button_facturar.Clicked += new EventHandler(on_button_facturar_clicked);
					Gtk.FileFilter filter = new Gtk.FileFilter();
					filter.AddPattern("*.XML");
					filter.AddPattern("*.xml");
					filechooserbutton1.AddFilter(filter);
				}else{
					msgBox = new MessageDialog (MyWinError,DialogFlags.Modal,
									MessageType.Error,ButtonsType.Ok,"Este Comprobante de Caja se encuentra FACTURADO, verifique....");
				msgBox.Run ();msgBox.Destroy();
				}
			}else{
				msgBox = new MessageDialog (MyWinError,DialogFlags.Modal,
									MessageType.Info,ButtonsType.Ok,"Seleccione un Pago o Abono...");
				msgBox.Run ();msgBox.Destroy();
			}
		}		
		
		void on_button_imprimir_clicked(object sender, EventArgs args)
		{
			imprime_comprobante_resumen("caja");
		}
		
		void on_button_imprimir_comp_serv_clicked(object sender, EventArgs args)
		{
			imprime_comprobante_resumen("comprobante");
		}
		
		void on_button_imprimir_pagare_clicked(object sender, EventArgs args)
		{
			imprime_comprobante_resumen("pagare");
		}
		
		void on_button_resumen_clicked(object sender, EventArgs args)
		{
			imprime_comprobante_resumen("abonos_x_atencion");
		}

		void on_button_abonos_expediente_clicked(object sender, EventArgs args)
		{
			imprime_comprobante_resumen("abonos_x_expediente");
		}
		
		void imprime_comprobante_resumen(string tipo_reporte)
		{
			TreeModel model;
			TreeIter iterSelected;			
			if (tipo_reporte == "caja"){
				if (lista_abonos.Selection.GetSelected(out model, out iterSelected)){
	 				monto = (string) model.GetValue(iterSelected, 0); 				
	 				fecha = (string) model.GetValue(iterSelected, 1);
					concepto = (string) model.GetValue(iterSelected, 2);
					idcreo = (string) model.GetValue(iterSelected, 3);
					recibo = (string) model.GetValue(iterSelected, 4);
					presupuesto = (string) model.GetValue(iterSelected, 5);
					paquete = (string) model.GetValue(iterSelected, 6);
					descripcion = (string) model.GetValue(iterSelected, 7);
					
					if((string) model.GetValue(iterSelected, 13) == "CAJA"){
						new caja_comprobante(int.Parse(recibo),(string) model.GetValue(iterSelected,13), folioservicio,"SELECT osiris_erp_cobros_deta.folio_de_servicio AS foliodeservicio,osiris_erp_cobros_deta.pid_paciente AS pidpaciente, "+ 
								"osiris_his_tipo_admisiones.descripcion_admisiones,aplicar_iva, "+
								"osiris_erp_cobros_deta.id_tipo_admisiones AS idadmisiones,"+
								"osiris_grupo_producto.descripcion_grupo_producto, "+
								"osiris_productos.id_grupo_producto,  "+
								"to_char(osiris_erp_cobros_deta.porcentage_descuento,'999.99') AS porcdesc, "+
								"to_char(osiris_erp_cobros_enca.fechahora_creacion,'dd-mm-yyyy') AS fechcreacion,"+
								"to_char(osiris_erp_cobros_enca.fechahora_creacion,'HH:mi') AS horacreacion," +
								"to_char(osiris_erp_abonos.fechahora_registro,'dd-mm-yyyy') AS fechcreacomp," +
								"to_char(osiris_erp_abonos.fechahora_registro,'HH:mi') AS horacreacomp,"+
								"to_char(osiris_erp_cobros_deta.id_producto,'999999999999') AS idproducto,descripcion_producto, "+
								"to_char(osiris_erp_cobros_deta.cantidad_aplicada,'99999999.99') AS cantidadaplicada, "+
								"to_char(osiris_erp_cobros_deta.precio_producto,'9999999.99') AS preciounitario, "+
								"ltrim(to_char(osiris_erp_cobros_deta.precio_producto,'9999999.99')) AS preciounitarioprod, "+
								"to_char(osiris_erp_cobros_deta.iva_producto,'999999.99') AS ivaproducto, "+
								//"to_char(osiris_erp_cobros_deta.precio_por_cantidad,'999999.99') AS ppcantidad, "+
								"to_char(osiris_erp_cobros_deta.cantidad_aplicada * osiris_erp_cobros_deta.precio_producto,'99999999.99') AS ppcantidad,"+
								"to_char(osiris_productos.precio_producto_publico,'999999999.99999') AS preciopublico,osiris_erp_abonos.numero_recibo_caja AS numerorecibo,"+
								"osiris_his_paciente.nombre1_paciente || ' ' || osiris_his_paciente.nombre2_paciente || ' ' || osiris_his_paciente.apellido_paterno_paciente || ' ' || osiris_his_paciente.apellido_materno_paciente AS nombre_completo, "+
								"to_char(osiris_his_paciente.fecha_nacimiento_paciente, 'dd-MM-yyyy') AS fechanacpaciente, to_char(to_number(to_char(age('"+DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss")+"',osiris_his_paciente.fecha_nacimiento_paciente),'yyyy') ,'9999'),'9999') AS edadpaciente, "+
							    "telefono_particular1_paciente,osiris_erp_abonos.observaciones AS observacionesvarias,osiris_erp_abonos.observaciones2,osiris_erp_abonos.observaciones3," +
							    "osiris_erp_abonos.concepto_del_abono AS concepto_comprobante,"+
								"osiris_erp_cobros_enca.id_empresa,descripcion_empresa," +
						        "osiris_erp_cobros_enca.id_aseguradora,descripcion_aseguradora," +
								"osiris_erp_cobros_enca.nombre_medico_encabezado,nombre_medico_tratante,"+
							    "to_char(monto_de_abono_procedimiento,'999999999.99') AS montodelabono,osiris_erp_tipo_comprobante.id_tipo_comprobante,descripcion_tipo_comprobante,"+
						        "osiris_empleado.nombre1_empleado || ' ' || osiris_empleado.nombre2_empleado || ' ' || osiris_empleado.apellido_paterno_empleado || ' ' || osiris_empleado.apellido_materno_empleado AS quien_creo_comprobante "+
						        "FROM osiris_erp_cobros_deta,osiris_his_tipo_admisiones,osiris_productos,osiris_grupo_producto,osiris_erp_abonos,osiris_his_paciente,osiris_erp_cobros_enca,osiris_empresas,osiris_erp_tipo_comprobante,osiris_aseguradoras,osiris_empleado "+
								"WHERE osiris_erp_cobros_deta.id_tipo_admisiones = osiris_his_tipo_admisiones.id_tipo_admisiones "+
								"AND osiris_erp_cobros_deta.id_producto = osiris_productos.id_producto  "+ 
								"AND osiris_productos.id_grupo_producto = osiris_grupo_producto.id_grupo_producto "+
								"AND osiris_erp_cobros_deta.pid_paciente = osiris_his_paciente.pid_paciente "+
						        "AND osiris_erp_cobros_enca.id_empresa = osiris_empresas.id_empresa "+
						        "AND osiris_erp_cobros_enca.id_aseguradora = osiris_aseguradoras.id_aseguradora "+ 
							    "AND osiris_erp_cobros_enca.folio_de_servicio = osiris_erp_cobros_deta.folio_de_servicio "+
								"AND osiris_erp_cobros_deta.eliminado = 'false' "+
							    "AND osiris_erp_abonos.folio_de_servicio = osiris_erp_cobros_deta.folio_de_servicio "+
						        "AND osiris_erp_tipo_comprobante.id_tipo_comprobante = osiris_erp_abonos.id_tipo_comprobante " +
						        "AND osiris_erp_abonos.id_quien_creo = osiris_empleado.login_empleado ",nombrecajero);		
					}
					
					if((string) model.GetValue(iterSelected, 13) == "ABONO"){
						new caja_comprobante(int.Parse(recibo),(string) model.GetValue(iterSelected,13), folioservicio,"SELECT osiris_erp_cobros_enca.folio_de_servicio AS foliodeservicio,osiris_erp_cobros_enca.pid_paciente AS pidpaciente," +
							"osiris_erp_abonos.numero_recibo_caja AS numerorecibo," +
							"osiris_his_paciente.nombre1_paciente || ' ' || osiris_his_paciente.nombre2_paciente || ' ' || osiris_his_paciente.apellido_paterno_paciente || ' ' || osiris_his_paciente.apellido_materno_paciente AS nombre_completo," +
							"to_char(osiris_his_paciente.fecha_nacimiento_paciente, 'dd-MM-yyyy') AS fechanacpaciente," +
						    "to_char(osiris_erp_cobros_enca.fechahora_creacion,'dd-mm-yyyy') AS fechcreacion,"+
							"to_char(osiris_erp_cobros_enca.fechahora_creacion,'HH:mi') AS horacreacion,"+
						    "to_char(osiris_erp_abonos.fechahora_registro,'dd-mm-yyyy') AS fechcreacomp," +
							"to_char(osiris_erp_abonos.fechahora_registro,'HH:mi') AS horacreacomp,"+
							"to_char(to_number(to_char(age('"+DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss")+"',osiris_his_paciente.fecha_nacimiento_paciente),'yyyy') ,'9999'),'9999') AS edadpaciente," +
							"telefono_particular1_paciente,osiris_erp_abonos.observaciones AS observacionesvarias,osiris_erp_abonos.observaciones2,osiris_erp_abonos.observaciones3," +
							"osiris_erp_abonos.concepto_del_abono AS concepto_comprobante," +
							"osiris_erp_cobros_enca.id_empresa,descripcion_empresa," +
						    "osiris_erp_cobros_enca.id_aseguradora,descripcion_aseguradora," +                 
							"osiris_erp_cobros_enca.nombre_medico_encabezado,nombre_medico_tratante,to_char(monto_de_abono_procedimiento,'999999999.99') AS montodelabono," +
							"osiris_erp_tipo_comprobante.id_tipo_comprobante,descripcion_tipo_comprobante," +
							"osiris_empleado.nombre1_empleado || ' ' || osiris_empleado.nombre2_empleado || ' ' || osiris_empleado.apellido_paterno_empleado || ' ' || osiris_empleado.apellido_materno_empleado AS quien_creo_comprobante "+
						    "FROM osiris_erp_abonos,osiris_his_paciente,osiris_erp_cobros_enca,osiris_empresas,osiris_erp_tipo_comprobante,osiris_aseguradoras,osiris_empleado "+
							"WHERE osiris_erp_cobros_enca.id_empresa = osiris_empresas.id_empresa "+
						    "AND osiris_erp_cobros_enca.id_aseguradora = osiris_aseguradoras.id_aseguradora "+
							"AND osiris_erp_abonos.folio_de_servicio = osiris_erp_cobros_enca.folio_de_servicio " +
							"AND osiris_erp_abonos.id_tipo_comprobante = osiris_erp_tipo_comprobante.id_tipo_comprobante "+
							"AND osiris_his_paciente.pid_paciente = osiris_erp_cobros_enca.pid_paciente " +
							"AND osiris_erp_abonos.id_quien_creo = osiris_empleado.login_empleado ",nombrecajero);
					}

					if((string) model.GetValue(iterSelected, 13) == "HONORARIO"){
						// Pago en Caja total del procedimiento Console.WriteLine("Es un Abono");
						new caja_comprobante(int.Parse(recibo),(string) model.GetValue(iterSelected,13), folioservicio,"SELECT osiris_erp_cobros_enca.folio_de_servicio AS foliodeservicio,osiris_erp_cobros_enca.pid_paciente AS pidpaciente," +
							"osiris_erp_abonos.numero_recibo_caja AS numerorecibo," +
							"osiris_his_paciente.nombre1_paciente || ' ' || osiris_his_paciente.nombre2_paciente || ' ' || osiris_his_paciente.apellido_paterno_paciente || ' ' || osiris_his_paciente.apellido_materno_paciente AS nombre_completo," +
							"to_char(osiris_his_paciente.fecha_nacimiento_paciente, 'dd-MM-yyyy') AS fechanacpaciente," +
							"to_char(osiris_erp_cobros_enca.fechahora_creacion,'dd-mm-yyyy') AS fechcreacion,"+
							"to_char(osiris_erp_cobros_enca.fechahora_creacion,'HH:mi') AS horacreacion,"+
							"to_char(osiris_erp_abonos.fechahora_registro,'dd-mm-yyyy') AS fechcreacomp," +
							"to_char(osiris_erp_abonos.fechahora_registro,'HH:mi') AS horacreacomp,"+
							"to_char(to_number(to_char(age('"+DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss")+"',osiris_his_paciente.fecha_nacimiento_paciente),'yyyy') ,'9999'),'9999') AS edadpaciente," +
							"telefono_particular1_paciente,osiris_erp_abonos.observaciones AS observacionesvarias,osiris_erp_abonos.concepto_del_abono AS concepto_comprobante," +
							"osiris_erp_cobros_enca.id_empresa,descripcion_empresa," +
						    "osiris_erp_cobros_enca.id_aseguradora,descripcion_aseguradora," +
							"osiris_erp_cobros_enca.nombre_medico_encabezado,to_char(monto_de_abono_procedimiento,'999999999.99') AS montodelabono," +
							"descripcion_tipo_comprobante " +
							"FROM osiris_erp_abonos,osiris_his_paciente,osiris_erp_cobros_enca,osiris_empresas,osiris_erp_tipo_comprobante,osiris_aseguradoras "+
							"WHERE osiris_erp_cobros_enca.id_empresa = osiris_empresas.id_empresa "+
						    "AND osiris_erp_cobros_enca.id_aseguradora = osiris_aseguradoras.id_aseguradora "+
							"AND osiris_erp_abonos.folio_de_servicio = osiris_erp_cobros_enca.folio_de_servicio " +
							"AND osiris_erp_abonos.id_tipo_comprobante = osiris_erp_tipo_comprobante.id_tipo_comprobante "+
							"AND osiris_his_paciente.pid_paciente = osiris_erp_cobros_enca.pid_paciente ",nombrecajero);
					}
				}
			}
						
			if(tipo_reporte == "comprobante"){
				if (treeview_lista_comprserv.Selection.GetSelected(out model, out iterSelected)){					
					recibo = (string) model.GetValue(iterSelected, 0);					
					new caja_comprobante(int.Parse(recibo),"SERVICIO", folioservicio,"SELECT osiris_erp_cobros_deta.folio_de_servicio AS foliodeservicio,osiris_erp_cobros_deta.pid_paciente AS pidpaciente, "+ 
						"osiris_his_tipo_admisiones.descripcion_admisiones,aplicar_iva, "+
						"osiris_his_tipo_admisiones.id_tipo_admisiones AS idadmisiones,"+
						"osiris_grupo_producto.descripcion_grupo_producto, "+
						"osiris_productos.id_grupo_producto,  "+
						"to_char(osiris_erp_cobros_deta.porcentage_descuento,'999.99') AS porcdesc, "+
						"to_char(osiris_erp_cobros_deta.fechahora_creacion,'dd-mm-yyyy') AS fechcreacion,  "+
						"to_char(osiris_erp_cobros_deta.fechahora_creacion,'HH:mm') AS horacreacion,  "+
						"to_char(osiris_erp_cobros_deta.id_producto,'999999999999') AS idproducto,descripcion_producto, "+
						"to_char(osiris_erp_cobros_deta.cantidad_aplicada,'99999999.99') AS cantidadaplicada, "+
						"to_char(osiris_erp_cobros_deta.precio_producto,'9999999.99') AS preciounitario, "+
						"ltrim(to_char(osiris_erp_cobros_deta.precio_producto,'9999999.99')) AS preciounitarioprod, "+
						"to_char(osiris_erp_cobros_deta.iva_producto,'999999.99') AS ivaproducto, "+
						//"to_char(osiris_erp_cobros_deta.precio_por_cantidad,'999999.99') AS ppcantidad, "+
						"to_char(osiris_erp_cobros_deta.cantidad_aplicada * osiris_erp_cobros_deta.precio_producto,'99999999.99') AS ppcantidad,"+
						"to_char(osiris_productos.precio_producto_publico,'999999999.99999') AS preciopublico,osiris_erp_comprobante_servicio.numero_comprobante_servicio AS numerorecibo,"+
						"osiris_his_paciente.nombre1_paciente || ' ' || osiris_his_paciente.nombre2_paciente || ' ' || osiris_his_paciente.apellido_paterno_paciente || ' ' || osiris_his_paciente.apellido_materno_paciente AS nombre_completo, "+
						"to_char(osiris_his_paciente.fecha_nacimiento_paciente, 'dd-MM-yyyy') AS fechanacpaciente, to_char(to_number(to_char(age('"+DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss")+"',osiris_his_paciente.fecha_nacimiento_paciente),'yyyy') ,'9999'),'9999') AS edadpaciente, "+
					    "telefono_particular1_paciente,"+
					    "osiris_erp_comprobante_servicio.observaciones || ' ' || osiris_erp_comprobante_servicio.observaciones2 || ' ' || osiris_erp_comprobante_servicio.observaciones3 AS observacionesvarias,"+
					     "osiris_erp_comprobante_servicio.concepto_del_comprobante AS concepto_comprobante,"+
						"osiris_erp_cobros_enca.id_empresa,descripcion_empresa," +
					    "osiris_erp_cobros_enca.id_aseguradora,descripcion_aseguradora," +
						"osiris_erp_cobros_enca.nombre_medico_encabezado "+
					    //"to_char(monto_de_abono_procedimiento,'999999999.99') AS montodelabono "+
				        "FROM osiris_erp_cobros_deta,osiris_his_tipo_admisiones,osiris_productos,osiris_grupo_producto,osiris_erp_comprobante_servicio,osiris_his_paciente,osiris_erp_cobros_enca,osiris_empresas,osiris_aseguradoras "+
						"WHERE osiris_erp_cobros_deta.id_tipo_admisiones = osiris_his_tipo_admisiones.id_tipo_admisiones "+
						"AND osiris_erp_cobros_deta.id_producto = osiris_productos.id_producto  "+ 
						"AND osiris_productos.id_grupo_producto = osiris_grupo_producto.id_grupo_producto "+
						"AND osiris_erp_cobros_deta.pid_paciente = osiris_his_paciente.pid_paciente "+
				        "AND osiris_erp_cobros_enca.id_empresa = osiris_empresas.id_empresa "+
					    "AND osiris_erp_cobros_enca.id_aseguradora = osiris_aseguradoras.id_aseguradora "+
					    "AND osiris_erp_cobros_enca.folio_de_servicio = osiris_erp_cobros_deta.folio_de_servicio "+
						"AND osiris_erp_cobros_deta.eliminado = 'false' ", nombrecajero );
				}
			}
			
			if(tipo_reporte == "pagare"){
				if (treeview_lista_pagare.Selection.GetSelected(out model, out iterSelected)){
					recibo = (string) model.GetValue(iterSelected, 4);
					new caja_comprobante (int.Parse(recibo), "PAGARE", folioservicio,"SELECT osiris_erp_cobros_enca.folio_de_servicio AS foliodeservicio,osiris_erp_cobros_enca.pid_paciente AS pidpaciente," +
							"osiris_erp_comprobante_pagare.numero_comprobante_pagare AS numerorecibo," +
							"osiris_his_paciente.nombre1_paciente || ' ' || osiris_his_paciente.nombre2_paciente || ' ' || osiris_his_paciente.apellido_paterno_paciente || ' ' || osiris_his_paciente.apellido_materno_paciente AS nombre_completo," +
							"to_char(osiris_his_paciente.fecha_nacimiento_paciente, 'dd-MM-yyyy') AS fechanacpaciente," +
							"direccion_paciente,numero_casa_paciente,numero_departamento_paciente,codigo_postal_paciente,colonia_paciente,municipio_paciente,estado_paciente," +
						    "to_char(osiris_erp_cobros_enca.fechahora_creacion,'dd-mm-yyyy') AS fechcreacion,  "+
							"to_char(osiris_erp_cobros_enca.fechahora_creacion,'HH:mi') AS horacreacion,  "+
							"to_char(to_number(to_char(age('"+DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss")+"',osiris_his_paciente.fecha_nacimiento_paciente),'yyyy') ,'9999'),'9999') AS edadpaciente," +
							"telefono_particular1_paciente,osiris_erp_comprobante_pagare.observaciones AS observacionesvarias,osiris_erp_comprobante_pagare.concepto_del_comprobante AS concepto_comprobante," +
					        "osiris_erp_cobros_enca.id_aseguradora,descripcion_aseguradora," +
							"osiris_erp_cobros_enca.id_empresa,descripcion_empresa," +
							"osiris_erp_cobros_enca.nombre_medico_encabezado,to_char(osiris_erp_comprobante_pagare.monto_pagare,'999999999.99') AS montodelabono," +
							"descripcion_tipo_comprobante,to_char(fecha_vencimiento_pagare,'dd-mm-yyyy') AS vencimiento_pagare " +
							"FROM osiris_erp_comprobante_pagare,osiris_his_paciente,osiris_erp_cobros_enca,osiris_empresas,osiris_erp_tipo_comprobante,osiris_aseguradoras "+
							"WHERE osiris_erp_cobros_enca.id_empresa = osiris_empresas.id_empresa "+
							 "AND osiris_erp_cobros_enca.id_aseguradora = osiris_aseguradoras.id_aseguradora "+
					        "AND osiris_erp_comprobante_pagare.folio_de_servicio = osiris_erp_cobros_enca.folio_de_servicio " +
							"AND osiris_erp_comprobante_pagare.id_tipo_comprobante = osiris_erp_tipo_comprobante.id_tipo_comprobante "+
							"AND osiris_his_paciente.pid_paciente = osiris_erp_cobros_enca.pid_paciente " +
							"AND osiris_erp_comprobante_pagare.eliminado = 'false' ",nombrecajero);			
				}
			}
			if (tipo_reporte == "abonos_x_atencion"){
				new osiris.reporte_de_abonos(nombrebd,"abonospagos_x_atencion",LoginEmpleado,folioservicio,PidPaciente);			
			}
			if (tipo_reporte == "abonos_x_expediente"){
				new osiris.reporte_de_abonos(nombrebd,"abonospagos_x_expediente",LoginEmpleado,folioservicio,PidPaciente);			
			}
		}
		
		void on_button_cargar_xml_clicked(object sender, EventArgs args)
		{
			if(filechooserbutton1.Filename != null){
				LeerXML(filechooserbutton1.Filename,false);
			}else{
				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
									MessageType.Error,ButtonsType.Close,"XML Read error: {0}","No ha Seleccionado un Archivo");
				msgBoxError.Run ();			msgBoxError.Destroy();
			}
		}
		
		void LeerXML(string filename_xml,bool almacena_factura)
		{
			string fechahorapagofactura = "2000-01-02 00:00:00";
			string idquienpago = "";
			if(almacena_factura == true){
				if(entry_id_emisor.Text != ""){
					if(pagarfactura == true){
						fechahorapagofactura = (string) DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
						idquienpago = LoginEmpleado;					
					}				
					NpgsqlConnection conexion1; 
					conexion1 = new NpgsqlConnection (connectionString+nombrebd);
		        	try{
						conexion1.Open ();
						NpgsqlCommand comando1; 
						comando1 = conexion1.CreateCommand ();
						comando1.CommandText = "INSERT INTO osiris_erp_factura_enca(" +
							"id_cliente," +
							"numero_factura," +
							"descripcion_cliente," +
							"direccion_cliente," +
							"colonia_cliente," +
							"municipio_cliente," +
							"estado_cliente," +
							"rfc_cliente," +
							"localidad_cliente," +
							"noexterior_cliente," +
							"nointerior_cliente," +
							"referencia_cliente," +
							"pais_cliente," +
							"id_emisor," +
							"serie," +
							"no_certificado," +
							"certificado," +
							"sello," +
							"forma_de_pago," +
							"metodo_de_pago," +
							"tipo_de_comprobante," +
							"lugar_expedicion," +
							"tipo_cambio," +
							"moneda," +
							"sello_sat," +
							"sello_cfd," +
							"no_certificado_sat," +
							"fecha_timbrado," +
							"uuid," +
							"version," +
							"total_impuesto_factura," +
							"total_factura," +
							"subtotal_factura," +
							"fechahora_factura," +
							"fechahora_creacion_factura," +
							"id_quien_creo," +
							"fechahora_pago_factura," +
							"id_quien_pago," +
							"pagada) " +
							"VALUES ('"+
								entry_id_receptor.Text.Trim()+"','"+
								int.Parse(entry_folio_factura.Text.Trim()).ToString().Trim()+"','"+
								entry_nombre_receptor.Text.Trim()+"','"+
								calle_receptor+"','"+
								colonia_receptor+"','"+
								municipio_receptor+"','"+								
								estado_receptor+"','"+								
								entry_rfc_receptor.Text.Trim()+"','"+
								localidad_receptor+"','"+
								noexterior_receptor+"','"+
								nointerior_receptor+"','"+
								referencia_receptor+"','"+
								pais_receptor+"','"+
								entry_id_emisor.Text.Trim()+"','"+
								entry_serie_factura.Text.Trim()+"','"+
								nro_de_certificado_emisor+"','"+
								certificado_emisor+"','"+
								sello_emisor+"','"+
								forma_de_pago_emisor+"','"+
								metodo_de_pago_emisor+"','"+
								tipo_de_comprobante_emisor+"','"+
								lugar_expedicion_emisor+"','"+
								tipo_cambio+"','"+
								moneda+"','"+
								sello_sat+"','"+
								sello_cfd+"','"+
								no_certificado_sat+"','"+
								fecha_timbrado+"','"+
								entry_folio_fiscal.Text.Trim()+"','"+
								version_sat+"','"+
								entry_iva_fact.Text+"','"+
								entry_total_fact.Text+"','"+
								entry_subtotal_fact.Text+"','"+
								entry_ano_fechafactura.Text+"-"+entry_mes_fechafactura.Text+"-"+entry_dia_fechafactura.Text+" "+entry_horafactura.Text+"','"+
								(string) DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"','"+
								LoginEmpleado+"','"+
								fechahorapagofactura+"','"+
								idquienpago+"','"+
								pagarfactura.ToString().Trim()+
								"')";
								
						Console.WriteLine(comando1.CommandText);
						comando1.ExecuteNonQuery();    	    	       	comando1.Dispose();
		    	       	
					}catch(NpgsqlException ex){
						MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
										MessageType.Error,ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
						msgBoxError.Run ();					msgBoxError.Destroy();
		   			}
					conexion1.Close ();
				}
			}
			
			
			XmlTextReader reader_xml = new XmlTextReader(filename_xml);
			while (reader_xml.Read()){
				switch (reader_xml.NodeType){
					case XmlNodeType.Element:
						// version 3.2
						if (reader_xml.MoveToContent() == XmlNodeType.Element && reader_xml.Name == "cfdi:Comprobante"){
							if (reader_xml.HasAttributes){
								if(null != reader_xml.GetAttribute("serie")){
									entry_serie_factura.Text = reader_xml.GetAttribute("serie");
								}else{
									entry_serie_factura.Text = "";
								}
								entry_folio_factura.Text = reader_xml.GetAttribute("folio");								
								//fecha="2012-06-28T14:06:29"
								entry_ano_fechafactura.Text = reader_xml.GetAttribute("fecha").Substring(0,4);
								entry_mes_fechafactura.Text = reader_xml.GetAttribute("fecha").Substring(5,2);
								entry_dia_fechafactura.Text = reader_xml.GetAttribute("fecha").Substring(8,2);
								entry_horafactura.Text = reader_xml.GetAttribute("fecha").Substring(11,8);
								entry_subtotal_fact.Text = reader_xml.GetAttribute("subTotal");
								entry_total_fact.Text = reader_xml.GetAttribute("total");
								nro_de_certificado_emisor = reader_xml.GetAttribute("noCertificado");
								certificado_emisor = reader_xml.GetAttribute("certificado");
								sello_emisor = reader_xml.GetAttribute("sello");
								forma_de_pago_emisor = reader_xml.GetAttribute("formaDePago");
								metodo_de_pago_emisor = reader_xml.GetAttribute("metodoDePago");
								tipo_de_comprobante_emisor = reader_xml.GetAttribute("tipoDeComprobante");
								lugar_expedicion_emisor = reader_xml.GetAttribute("LugarExpedicion");
								tipo_cambio = reader_xml.GetAttribute("TipoCambio");
								
							}
						}
						if (reader_xml.MoveToContent() == XmlNodeType.Element && reader_xml.Name == "cfdi:Impuestos"){
							if (reader_xml.HasAttributes){
								entry_iva_fact.Text = reader_xml.GetAttribute("totalImpuestosTrasladados").ToString().Trim();	
							}
						}
						if (reader_xml.MoveToContent() == XmlNodeType.Element && reader_xml.Name == "cfdi:Receptor"){
							if (reader_xml.HasAttributes){
								entry_nombre_receptor.Text = reader_xml.GetAttribute("nombre").ToUpper();
								entry_rfc_receptor.Text = reader_xml.GetAttribute("rfc").ToUpper();
								entry_id_receptor.Text = (string) classpublic.lee_registro_de_tabla("osiris_erp_clientes","id_cliente","WHERE rfc_cliente = '"+reader_xml.GetAttribute("rfc").ToUpper()+"' AND cliente_activo = 'true' ","id_cliente","string");
							}
						}
						if (reader_xml.MoveToContent() == XmlNodeType.Element && reader_xml.Name == "cfdi:Domicilio"){
							if (reader_xml.HasAttributes){
								if(null != reader_xml.GetAttribute("calle")){
									calle_receptor = reader_xml.GetAttribute("calle").ToUpper();
								}
								if(null != reader_xml.GetAttribute("noExterior")){
									noexterior_receptor = reader_xml.GetAttribute("noExterior").ToUpper();
								}
								if (null != reader_xml.GetAttribute("noInterior")){
									nointerior_receptor = reader_xml.GetAttribute("noInterior").ToUpper();;
								}
								if(null != reader_xml.GetAttribute("colonia")){
									colonia_receptor = reader_xml.GetAttribute("colonia").ToUpper();
								}
								if(null != reader_xml.GetAttribute("localidad")){
									localidad_receptor = reader_xml.GetAttribute("localidad").ToUpper();
								}
								if(null != reader_xml.GetAttribute("referencia")){
									referencia_receptor = reader_xml.GetAttribute("referencia").ToUpper();
								}
								if(null != reader_xml.GetAttribute("municipio")){
									municipio_receptor = reader_xml.GetAttribute("municipio").ToUpper();
								}
								if(null != reader_xml.GetAttribute("estado")){
									estado_receptor = reader_xml.GetAttribute("estado").ToUpper();
								}
								if(null != reader_xml.GetAttribute("pais")){
									pais_receptor = reader_xml.GetAttribute("pais").ToUpper();
								}
								if(null != reader_xml.GetAttribute("codigoPostal")){
									codpostal_receptor = reader_xml.GetAttribute("codigoPostal").ToUpper();
								}
							}
						}					
						if (reader_xml.MoveToContent() == XmlNodeType.Element && reader_xml.Name == "cfdi:Emisor"){
							if (reader_xml.HasAttributes){
								entry_nombre_emisor.Text = reader_xml.GetAttribute("nombre").ToUpper();								
								entry_rfc_emisor.Text = reader_xml.GetAttribute("rfc").ToUpper();
								entry_id_emisor.Text = (string) classpublic.lee_registro_de_tabla("osiris_erp_emisor","id_emisor","WHERE rfc = '"+reader_xml.GetAttribute("rfc").ToUpper()+"' ","id_emisor","string");
							}
						}
						if (reader_xml.MoveToContent() == XmlNodeType.Element && reader_xml.Name == "tfd:TimbreFiscalDigital"){
							if (reader_xml.HasAttributes){
								entry_folio_fiscal.Text = reader_xml.GetAttribute("UUID");
								sello_sat = reader_xml.GetAttribute("selloSAT");
								sello_cfd = reader_xml.GetAttribute("selloCFD");
								no_certificado_sat = reader_xml.GetAttribute("noCertificadoSAT");
								fecha_timbrado = reader_xml.GetAttribute("FechaTimbrado").Substring(0,10)+" "+reader_xml.GetAttribute("FechaTimbrado").Substring(11,8);
								version_sat = reader_xml.GetAttribute("version");
							}
						}
						if (reader_xml.MoveToContent() == XmlNodeType.Element && reader_xml.Name == "cfdi:Concepto"){
							if (reader_xml.HasAttributes){
								string unidadprod;
								string noIdentificacion;
								if(null != reader_xml.GetAttribute("unidad")){
									unidadprod = reader_xml.GetAttribute("unidad");
								}else{
									unidadprod = "";
								}
								if(null != reader_xml.GetAttribute("noIdentificacion")){
									noIdentificacion = reader_xml.GetAttribute("noIdentificacion");
								}else{
									noIdentificacion = "";
								}							
								if(almacena_factura == true){
									if(entry_id_emisor.Text != ""){
									//if((string) classpublic.lee_registro_de_tabla("osiris_erp_factura_deta","numero_factura","WHERE serie = '"+entry_serie_factura.Text+"' AND numero_factura = '"+int.Parse(entry_folio_factura.Text).ToString().Trim()+"' AND id_emisor = '"+entry_id_emisor.Text.Trim()+"' ","numero_factura","string") == ""){
										NpgsqlConnection conexion; 
										conexion = new NpgsqlConnection (connectionString+nombrebd);
							        	try{
											conexion.Open ();
											NpgsqlCommand comando; 
											comando = conexion.CreateCommand ();
											comando.CommandText = "INSERT INTO osiris_erp_factura_deta(" +
												"numero_factura," +
												"cantidad_detalle," +
												"descripcion_detalle," +
												"precio_unitario," +
												"importe_detalle," +
												"serie," +
												"id_emisor," +
												"unidad," +
												"no_identificacion " +
												") VALUES ('" +
												int.Parse(entry_folio_factura.Text.Trim()).ToString().Trim()+"','"+
												reader_xml.GetAttribute("cantidad")+"','"+
												reader_xml.GetAttribute("descripcion")+"','"+
												reader_xml.GetAttribute("valorUnitario")+"','"+
												reader_xml.GetAttribute("importe")+"','"+
												entry_serie_factura.Text.Trim()+"','"+
												entry_id_emisor.Text.Trim()+"','"+
												unidadprod+"','"+
												noIdentificacion+
												"');";
											Console.WriteLine(comando.CommandText);
											comando.ExecuteNonQuery();    	    	       	comando.Dispose();
							    	       	
										}catch(NpgsqlException ex){
											MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
															MessageType.Error,ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
											msgBoxError.Run ();					msgBoxError.Destroy();
							   			}
										conexion.Close ();
									}
								}
							}
						}
						break;
					default:
						break;			
				}
			}						
		}		
		
		void on_button_facturar_clicked (object sender, EventArgs args)
		{
			if(entry_id_emisor.Text.Trim() != ""){
				MessageDialog msgBox = new MessageDialog (MyWin,DialogFlags.Modal,MessageType.Question,
									ButtonsType.YesNo,"¿ Desea enlasar comprobante de Caja con el CFDI ?");
				ResponseType miResultado = (ResponseType)msgBox.Run ();
				msgBox.Destroy();
			 	if(miResultado == ResponseType.Yes){
					if(filechooserbutton1.Filename != null){
						if(entry_id_receptor.Text.Trim() == ""){
							agrega_proveedor();
						}
						agrega_factura();
					}else{
						MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
										MessageType.Error,ButtonsType.Close,"XML Read error: {0}","No ha Seleccionado un Archivo");
						msgBoxError.Run ();			msgBoxError.Destroy();
					}
				}
			}else{
				MessageDialog msgBox = new MessageDialog (MyWin,DialogFlags.Modal,MessageType.Info,
											ButtonsType.Ok,"Verifique el Emisor NO esta registrado en la Tabla osiris_erp_emisor...");
				msgBox.Run();				msgBox.Destroy();
			}
		}
		
		void agrega_proveedor()
		{
			NpgsqlConnection conexion; 
			conexion = new NpgsqlConnection (connectionString+nombrebd);
        	// Verifica que la base de datos este conectada
        	
			try{
				conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand ();
				comando.CommandText = "INSERT INTO osiris_erp_clientes("+
										"descripcion_cliente,"+
										"rfc_cliente,"+
										"direccion_cliente,"+
										"colonia_cliente,"+
										"cp_cliente,"+
										"estado_cliente,"+
										"municipio_cliente,"+
										"localidad_cliente,"+
										"noexterior_cliente,"+
										"nointerior_cliente,"+
										"referencia_cliente,"+
										"pais_cliente,"+
										"cliente_activo,"+
										"id_quien_creo,"+
										"fechahora_creacion_cliente) "+
									  "VALUES ('"+
										entry_nombre_receptor.Text.Trim().ToUpper()+"','"+
										entry_rfc_receptor.Text.Trim().ToUpper()+"','"+
										calle_receptor+"','"+
										colonia_receptor+"','"+
										codpostal_receptor+"','"+
										estado_receptor+"','"+
										municipio_receptor+"','"+
										localidad_receptor+"','"+
										noexterior_receptor+"','"+
										nointerior_receptor+"','"+
										referencia_receptor+"','"+
										pais_receptor+"','"+
										"true"+"','"+
 										(string) LoginEmpleado+"','"+
 										(string) DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+
										"');";
				//Console.WriteLine(comando.CommandText);
				comando.ExecuteNonQuery();    	    	       	comando.Dispose();
    	       	entry_id_receptor.Text = (string) classpublic.lee_registro_de_tabla("osiris_erp_clientes","id_cliente","WHERE rfc_cliente = '"+entry_rfc_receptor.Text.Trim().ToUpper()+"' AND cliente_activo = 'true' ","id_cliente","string");
			}catch(NpgsqlException ex){
				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
								MessageType.Error,ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
				msgBoxError.Run ();					msgBoxError.Destroy();
   			}
			conexion.Close ();
		}
		
		void agrega_factura()
		{
			if((string) classpublic.lee_registro_de_tabla("osiris_erp_factura_enca","numero_factura","WHERE serie = '"+entry_serie_factura.Text+"' AND numero_factura ='"+int.Parse(entry_folio_factura.Text).ToString().Trim()+"' AND id_emisor = '"+entry_id_emisor.Text.Trim()+"' ","numero_factura","string") == ""){
				LeerXML(filechooserbutton1.Filename,true);
				actualizar_tablas();
			}else{
				actualizar_tablas();
			}			
		}
		
		void actualizar_tablas()
		{
			//Console.WriteLine("aqui actualiza");
			NpgsqlConnection conexion; 
			conexion = new NpgsqlConnection (connectionString+nombrebd);
        	// Verifica que la base de datos este conectada        	
			try{
				conexion.Open();
				NpgsqlCommand comando;
				comando = conexion.CreateCommand();
				comando.CommandText = "UPDATE osiris_erp_abonos SET "+
											"numero_factura = '"+entry_serie_factura.Text.Trim()+int.Parse(entry_folio_factura.Text.Trim()).ToString().Trim()+"',"+
											"fechahora_factura = '"+entry_ano_fechafactura.Text+"-"+entry_mes_fechafactura.Text+"-"+entry_dia_fechafactura.Text+" "+entry_horafactura.Text+"',"+
											"subtotal_facturado = '"+entry_subtotal_fact.Text+"',"+
											"iva_facturado = '"+entry_iva_fact.Text+"',"+
											"id_emisor = '"+entry_id_emisor.Text.Trim()+"'," +
											"folio_fiscal = '"+entry_folio_fiscal.Text.Trim()+"' "+
											"WHERE osiris_erp_abonos.numero_recibo_caja = '"+entry_nro_recibocaja.Text+"' " +
											"AND id_tipo_comprobante = '" +idtipocomprobante+"' "+
											"AND osiris_erp_abonos.folio_de_servicio = '"+folioservicio.ToString()+"'; ";
				//Console.WriteLine(comando.CommandText);
				comando.ExecuteNonQuery();                  comando.Dispose();
				
				if((string) classpublic.lee_registro_de_tabla("osiris_erp_cobros_enca","historial_facturados","WHERE historial_facturados LIKE '%"+entry_id_emisor.Text.Trim()+";"+entry_serie_factura.Text+int.Parse(entry_folio_factura.Text.Trim()).ToString().Trim()+"%' AND osiris_erp_cobros_enca.folio_de_servicio = '"+folioservicio.ToString()+"'","historial_facturados","string") == ""){
					comando.CommandText = "UPDATE osiris_erp_cobros_enca SET "+
											"numero_factura = numero_factura || '"+entry_serie_factura.Text.Trim()+int.Parse(entry_folio_factura.Text.Trim()).ToString().Trim()+";'," +
											"id_empleado_factura = '"+(string) LoginEmpleado+"',"+
											"historial_facturados = historial_facturados || '"+entry_id_emisor.Text.Trim()+";"+entry_serie_factura.Text.Trim()+int.Parse(entry_folio_factura.Text.Trim()).ToString().Trim()+";"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+";"+LoginEmpleado+"\n',"+
											"fechahora_factura = '"+entry_ano_fechafactura.Text+"-"+entry_mes_fechafactura.Text+"-"+entry_dia_fechafactura.Text+" "+entry_horafactura.Text+"' "+
											"WHERE osiris_erp_cobros_enca.folio_de_servicio = '"+folioservicio.ToString()+"'; ";
					
					//Console.WriteLine(comando.CommandText);
					comando.ExecuteNonQuery();                  comando.Dispose();
					MessageDialog msgBox = new MessageDialog (MyWin,DialogFlags.Modal,MessageType.Info,
																	ButtonsType.Ok,"El Nº de Comprobante se FACTURO con exito!!");
					msgBox.Run();				msgBox.Destroy();
				}				
				llenando_lista_de_abonos();
			}catch(Npgsql.NpgsqlException ex){
				Console.WriteLine ("PostgresSQL error: {0}",ex.Message);
				MessageDialog msgBox1 = new MessageDialog (MyWin,DialogFlags.Modal,MessageType.Error,
											ButtonsType.Ok,"PostgresSQL error: {0}",ex.Message);
				msgBox1.Run();				msgBox1.Destroy();
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