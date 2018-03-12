//////////////////////////////////////////////////////////
// created on 21/06/2007 at 01:40 p
// Sistema Hospitalario OSIRIS
// Monterrey - Mexico
//
// Autor    	: Ing. Daniel Olivares (Programacion)
//				  Ing. Jesus Buentello Garza(Adecuaciones)
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
// Programa		: facturador.cs
// Proposito	: Facturador general
// Objeto		: tesoreria.cs
//////////////////////////////////////////////////////////	
using System;
using Npgsql;
using System.Data;
using Gtk;
using Glade;
using System.Xml;

namespace osiris
{
	public class facturador_tesoreria
	{
		// Boton general para salir de las ventanas
		// Todas la ventanas en glade este boton debe estra declarado identico
		[Widget] Gtk.Button button_salir;

		// Para todas las busquedas este es el nombre asignado
		// se declara una vez
		[Widget] Gtk.Entry entry_expresion;
		[Widget] Gtk.Button button_selecciona;
		[Widget] Gtk.Button button_buscar_busqueda;
				
		// Declarando ventana principal del facturador
		[Widget] Gtk.Window facturador;
		[Widget] Gtk.CheckButton checkbutton_nueva_factura;
		[Widget] Gtk.Entry entry_serie_factura_fact = null;
		[Widget] Gtk.Entry entry_numero_factura;
		[Widget] Gtk.Entry entry_fecha_factura;
		[Widget] Gtk.Entry entry_status_factura;
		[Widget] Gtk.ComboBox combobox_emisor = null;
		[Widget] Gtk.Entry entry_id_cliente; 
 		[Widget] Gtk.Entry entry_nombre_cliente;
 		[Widget] Gtk.Entry entry_rfc_cliente;
 		[Widget] Gtk.Entry entry_curp_cliente;
 		[Widget] Gtk.Entry entry_cp_cliente;
 		[Widget] Gtk.Entry entry_direccion_cliente;
 		[Widget] Gtk.Entry entry_colonia_cliente;
 		[Widget] Gtk.Entry entry_municipio_cliente;
		[Widget] Gtk.Entry entry_estado_cliente;
 		[Widget] Gtk.Entry entry_telefono_cliente;
 		[Widget] Gtk.Entry entry_fax_cliente;
 		[Widget] Gtk.Entry entry_correo_electronico;
 		// Detalle vacio
 		[Widget] Gtk.CheckButton checkbutton_detalle_vacio;
		[Widget] Gtk.Button button_detalle_vacio;
 		
 		[Widget] Gtk.Entry entry_subtotal_15;
 		[Widget] Gtk.Entry entry_subtotal_0;
 		[Widget] Gtk.Entry entry_total_iva;
 		[Widget] Gtk.Entry entry_subtotal;
 		[Widget] Gtk.Entry entry_deducible_factura;
 		[Widget] Gtk.Entry entry_coaseguro_porcentage;
 		[Widget] Gtk.Entry entry_coaseguro_factura;
 		[Widget] Gtk.Entry entry_total_factura;
 		[Widget] Gtk.Entry entry_creada_por;
 		 		
		[Widget] Gtk.Button button_selecciona_factura;
		[Widget] Gtk.Button button_busca_cliente;
		[Widget] Gtk.Button button_selecc_folios;
		[Widget] Gtk.Button button_agrega_cliente;
		[Widget] Gtk.Button button_cancela_factura;
		[Widget] Gtk.Button button_guardar_factura;
		[Widget] Gtk.Button button_deducible;
		[Widget] Gtk.Button button_coaseguro;
		[Widget] Gtk.Button button_imprime_factura;
		[Widget] Gtk.Button button_pagar_factura;
		[Widget] Gtk.Button button_quitar;

		
		[Widget] Gtk.TreeView treeview_detalle_de_factura;
		[Widget] Gtk.Statusbar statusbar_facturador; //Declarando la barra de estado
				
		/////// Ventana Busqueda de Clientes\\\\\\\\
		[Widget] Gtk.Window busca_cliente;
		[Widget] Gtk.TreeView lista_de_cliente;
		[Widget] Gtk.RadioButton radiobutton_nombre_client;
		[Widget] Gtk.RadioButton radiobutton_rfc_client;
		[Widget] Gtk.RadioButton radiobutton_num_client;
		
		// Ventana de Seleccion de Folios para la factura
		[Widget] Gtk.Window selecciona_folios_factura;
		[Widget] Gtk.TreeView treeview_selec_procediminetos;
		[Widget] Gtk.Button button_acepta_folios;
		//[Widget] Gtk.Entry entry_contador_proced;
		
		[Widget] Gtk.CheckButton checkbutton_info_paciente;
		[Widget] Gtk.CheckButton checkbutton_info_ingr_egre;
		[Widget] Gtk.CheckButton checkbutton_info_cirugia;
		[Widget] Gtk.CheckButton checkbutton_info_compr_caja;
		[Widget] Gtk.CheckButton checkbutton_poliza;
		[Widget] Gtk.CheckButton checkbutton_certificado;
		[Widget] Gtk.CheckButton checkbutton_honorario_medico;
		[Widget] Gtk.CheckButton checkbutton_infoanexo_1;
		[Widget] Gtk.CheckButton checkbutton_infoanexo_2;
		[Widget] Gtk.CheckButton checkbutton_infoanexo_3;
		[Widget] Gtk.CheckButton checkbutton_infoanexo_4;
		
		
		[Widget] Gtk.Entry entry_diagnostico_factura;
		[Widget] Gtk.Entry entry_num_recibos_factura;
		
		[Widget] Gtk.Entry entry_infoanexo_1;
		[Widget] Gtk.Entry entry_infoanexo_2;
		[Widget] Gtk.Entry entry_infoanexo_3;
		[Widget] Gtk.Entry entry_infoanexo_4;
		
		//Ventana de Deducible
		[Widget] Gtk.Window deducible_coaseguro;
		[Widget] Gtk.Label label_deducible_coaseguro;
		[Widget] Gtk.Entry entry_deducible_coaseguro;
		[Widget] Gtk.Button button_acepta_deducible;
		
		// Ventana de Pago de Factura
		[Widget] Gtk.Window fecha_pago_factura;
		[Widget] Gtk.Entry entry_dia;
		[Widget] Gtk.Entry entry_mes;
		[Widget] Gtk.Entry entry_ano;
		[Widget] Gtk.Button button_guardar_pago;
		
		//Ventana Detalle Vacio
		[Widget] Gtk.Window factura_detalle;
		[Widget] Gtk.CheckButton checkbutton_detalle;
		[Widget] Gtk.CheckButton checkbutton_iva;
		[Widget] Gtk.Entry entry_cantidad;
		[Widget] Gtk.Entry entry_precio;
		[Widget] Gtk.Entry entry_descripcion;	
		[Widget] Gtk.Button button_aceptar_detalle;
		[Widget] Gtk.Button button_cancelar_detalle;
		[Widget] Gtk.CheckButton checkbutton_honorarios_medico;
		[Widget] Gtk.Entry entry_honorarios_medico;
		
		//Nota de Credito
		[Widget] Gtk.Button button_nota_credito;
		
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
			
		// Declaracion de variables de la clase
		int idcliente = 1;					// Toma el id del cliente que se va facturar
		int idadmision_ = 0;					//tipo de admision en donde se realizaron los cargos...
		int idgrupoproducto = 0;
		int id_tipopaciente = 0;
		bool aplicar_descuento = true;
		bool aplicar_siempre = false;
		bool error_no_existe = false;
		bool enviofactura_cliente = false;
		bool enviofactura_factutu = false;	
		
		decimal precio_por_cantidad = 0;		//esta variable se utiliza para ir guwerdandop el precio de un producto dependiendo de cuanto se aplico de este
		decimal iva_del_grupo = 0;					//es un valor en donde se van a ir sumando cada iva que se le aplica al producto
		decimal porcentagedesc = 0;			//es el el descuento en porciento si es que existe un descuento
		decimal descuento_neto = 0;			// valor desc sin iva
		decimal descurento_del_grupo = 0;		//el descuento que se aplica en cada grupo de productos
		decimal iva_de_descuento = 0;			// valor iva del descuento 
		decimal descuento_del_grupo = 0;		// suma del iva del desc y del desc neto
		decimal subtotal_del_grupo = 0;		//subtotal del grupo de productos
		decimal subtotal_al_impuesto_grupo = 0;		//es el subtotal de los productos que contienen iva en un grupo de productos
		decimal subtotal_al_0_grupo = 0;		//es el subtotal de los productos que no contienen iva en un grupo de productos
		decimal subtotal_al_impuesto = 0;			//es el subtotal de los productos que contienen iva en todo el movimiento
		decimal subtotal_al_0 = 0;			//es el subtotal de los productos que no contienen iva en todo el movimiento
		decimal total_del_grupo = 0;			//precio total del grupo de productos
		decimal total_de_iva = 0;				//suma de todos los ivas de todos los lugares y grupos de productos
		decimal total_de_descuento_neto =0;	//es el descuento neto de facturacion
		decimal total_de_iva_descuento =0;	//es el iva del descuento neto de facturacion
		decimal total_descuento=0;			//es la la suma del descuento neto y el iva del descuento neto de facturacion
		decimal deducible_factura = 0;
		decimal coaseguro_factura = 0;
		decimal valor_coaseguro = 0;
		decimal total_honorario_medico = 0;
		float totaldefactura = 0;
		float totalhonorariomedico = 0; 
						
		string folioservicio_factura = "";
		string diagnostico_factura = "";
		string numeros_folios_seleccionado = "";
		string numeros_seleccionado = "";
		string cantidad_en_letras = "";
		int num_nota = 0;					// igualando la nota de credito
		
		int idaseguradora = 0;
		
		int marca_un_folio = 0;
		int ultimafactura = 0;
		string numerodefactura = "";
		string seriedefactura = "";
		string municipios = "";
		string estado = "";		
		
		//Variables Detalle Vacio
		decimal suma_sin_iva = 0;
		double iva = 0;
		//public decimal sumasiniva = 0;
		//public decimal sumaconiva = 0;
		decimal suma_ = 0;
		//public decimal suma_del_iva = 0;
		decimal subtotales = 0;
		double valoriva = 0;
		decimal honorarios = 0;
		bool pagarfactura = false;
		
		// valores del XML
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
		int idemisor = 1;
		
		string[] args_args = {""};
		int[] args_id_array = {0,1,2,3,4,5,6,7,8,9,10,11,12,13,14};
			
		string LoginEmpleado;
		string NomEmpleado;
		string AppEmpleado;
		string ApmEmpleado;
		
		string connectionString;
		string nombrebd;
				
		//Declaracion de ventana de error
		protected Gtk.Window MyWinError;
		protected Gtk.Window MyWin;
		
		private TreeStore treeViewEngineBusca3;   	// Clientes
		private TreeStore treeViewEngineSelProce;  	// Seleccion de procedimientos para facturar
		private TreeStore treeViewEngineDetaFact; 	// Detalle de la Factura 
		
		class_conexion conexion_a_DB = new class_conexion();
		class_public classpublic = new class_public();
		
		public facturador_tesoreria(string LoginEmp, string NomEmpleado_, string AppEmpleado_, string ApmEmpleado_, string nombrebd_ ) 
		{
			LoginEmpleado = LoginEmp;
			NomEmpleado = NomEmpleado_;
			AppEmpleado = AppEmpleado_;
			ApmEmpleado = ApmEmpleado_;
			connectionString = conexion_a_DB._url_servidor+conexion_a_DB._port_DB+conexion_a_DB._usuario_DB+conexion_a_DB._passwrd_user_DB;
			nombrebd = conexion_a_DB._nombrebd; 
			valoriva = double.Parse(classpublic.ivaparaaplicar)/100;
			
			Glade.XML gxml = new Glade.XML (null, "caja.glade", "facturador", null);
			gxml.Autoconnect (this);
	        
			// Muestra ventana de Glade
			facturador.Show();
			
			entry_numero_factura.KeyPressEvent += onKeyPressEvent_enter_factura;
			button_selecciona_factura.Clicked += new EventHandler(on_button_selecciona_factura_clicked);
			//button_selecc_folios.Clicked += new EventHandler(on_button_selecc_folios_clicked);
			button_cancela_factura.Clicked += new EventHandler(on_button_cancela_factura_clicked);
			
			checkbutton_nueva_factura.Clicked +=  new EventHandler(on_checkbutton_nueva_factura_clicked);
			button_pagar_factura.Clicked +=  new EventHandler(on_button_pagar_factura_clicked);
			button_nota_credito.Clicked += new EventHandler(on_button_nota_credito_clicked);
			
			// Desactivando botones en la entrada
			button_guardar_factura.Sensitive = false;
			button_selecc_folios.Sensitive = true;
			button_busca_cliente.Sensitive = false;
			button_deducible.Sensitive = false;
			button_coaseguro.Sensitive = false;
			button_imprime_factura.Sensitive = false;
			button_detalle_vacio.Sensitive = false;
			checkbutton_detalle_vacio.Sensitive = false;
			checkbutton_detalle_vacio.Sensitive = false;
						
			// Sale de la ventana
			button_salir.Clicked += new EventHandler(on_cierraventanas_clicked);
			
			// crea treeview del detalle de la factura
			crea_treeview_facturador();
			llenado_combobox(0,"",combobox_emisor,"sql","SELECT * FROM osiris_erp_emisor ORDER BY emisor;","emisor","id_emisor",args_args,args_id_array,"");
			
			entry_numero_factura.ModifyBase(StateType.Normal, new Gdk.Color(255,243,169));
			entry_status_factura.ModifyBase(StateType.Normal, new Gdk.Color(166,220,255));
			entry_creada_por.ModifyBase(StateType.Normal, new Gdk.Color(54,180,221));
												
			statusbar_facturador.Pop(0);
			statusbar_facturador.Push(1, "login: "+LoginEmpleado+"  |Usuario: "+NomEmpleado+" "+AppEmpleado+" "+ApmEmpleado);
			statusbar_facturador.HasResizeGrip = false; 
		}
		
		void on_button_nota_credito_clicked(object sender, EventArgs args)
		{
			new osiris.nota_de_credito(LoginEmpleado,NomEmpleado,AppEmpleado,ApmEmpleado,nombrebd,numerodefactura,this.idcliente,
			                          subtotal_al_0,subtotal_al_impuesto,total_de_iva,subtotales,num_nota);
		}
		
		void on_checkbutton_nueva_factura_clicked(object sender, EventArgs args)
		{
			
			if (checkbutton_nueva_factura.Active == true){
				MessageDialog msgBox = new MessageDialog (MyWin,DialogFlags.Modal,
									MessageType.Question,ButtonsType.YesNo,"¿ Esta seguro de CARGAR (XML) una Nueva FACTURA ?");
				ResponseType miResultado = (ResponseType)msgBox.Run ();
				msgBox.Destroy();
		 		if (miResultado == ResponseType.Yes){
					Glade.XML gxml = new Glade.XML (null, "caja.glade", "factura_comprobante_caja", null);
					gxml.Autoconnect (this);
			        factura_comprobante_caja.Show();
					Gtk.FileFilter filter = new Gtk.FileFilter();
					filter.AddPattern("*.XML");
					filter.AddPattern("*.xml");
					filechooserbutton1.AddFilter(filter);
					
					button_cargar_xml.Clicked += new EventHandler(on_button_cargar_xml_clicked);
					button_facturar.Clicked += new EventHandler(on_button_facturar_clicked);
					button_salir.Clicked += new EventHandler(on_cierraventanas_clicked);
		 			
				}else{
		 			checkbutton_nueva_factura.Active = false;
		 			button_guardar_factura.Sensitive = false;
		 			button_selecc_folios.Sensitive = false;
		 			button_busca_cliente.Sensitive = false;
		 			button_deducible.Sensitive = false;
		 			button_coaseguro.Sensitive = false;
		 			this.checkbutton_detalle_vacio.Sensitive = false;
		 		}
		 	}else{
		 		button_guardar_factura.Sensitive = false;
		 		button_selecc_folios.Sensitive = false;
		 		button_busca_cliente.Sensitive = false;
		 		button_deducible.Sensitive = false;
		 	 	this.checkbutton_detalle_vacio.Sensitive = false;
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
			string nroidentificador = "";
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
								if(null != reader_xml.GetAttribute("nombre")){
									entry_nombre_emisor.Text = reader_xml.GetAttribute("nombre").ToUpper();
								}
								if(null != reader_xml.GetAttribute("rfc")){
									entry_rfc_emisor.Text = reader_xml.GetAttribute("rfc").ToUpper();
								}
								if(null != reader_xml.GetAttribute("rfc")){
									entry_id_emisor.Text = (string) classpublic.lee_registro_de_tabla("osiris_erp_emisor","id_emisor","WHERE rfc = '"+reader_xml.GetAttribute("rfc").ToUpper()+"' ","id_emisor","string");
								}
							}
						}
						if (reader_xml.MoveToContent() == XmlNodeType.Element && reader_xml.Name == "tfd:TimbreFiscalDigital"){
							if (reader_xml.HasAttributes){
								if(null != reader_xml.GetAttribute("UUID")){
									entry_folio_fiscal.Text = reader_xml.GetAttribute("UUID");
								}
								if(null != reader_xml.GetAttribute("selloSAT")){
									sello_sat = reader_xml.GetAttribute("selloSAT");
								}
								if(null != reader_xml.GetAttribute("selloCFD")){
									sello_cfd = reader_xml.GetAttribute("selloCFD");
								}
								if(null != reader_xml.GetAttribute("noCertificadoSAT")){
									no_certificado_sat = reader_xml.GetAttribute("noCertificadoSAT");
								}
								if(null != reader_xml.GetAttribute("FechaTimbrado")){
									fecha_timbrado = reader_xml.GetAttribute("FechaTimbrado").Substring(0,10)+" "+reader_xml.GetAttribute("FechaTimbrado").Substring(11,8);
								}else{
									fecha_timbrado = "2000-01-02 00:00:00";
								}
								if(null != reader_xml.GetAttribute("version")){
									version_sat = reader_xml.GetAttribute("version");
								}
							}
						}
						if (reader_xml.MoveToContent() == XmlNodeType.Element && reader_xml.Name == "cfdi:Concepto"){
							if (reader_xml.HasAttributes){
								string unidadprod;
								string noIdentificacion;
								if(almacena_factura == true){
									//if((string) classpublic.lee_registro_de_tabla("osiris_erp_factura_deta","numero_factura","WHERE serie = '"+entry_serie_factura.Text+"' AND numero_factura = '"+int.Parse(entry_folio_factura.Text).ToString().Trim()+"' AND id_emisor = '"+entry_id_emisor.Text.Trim()+"' ","numero_factura","string") == ""){
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
									//}
								}
							}
						}
						break;
					default:
						break;			
				}
			}
			if(almacena_factura == true){
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
			if(entry_id_emisor.Text != ""){
				if((string) classpublic.lee_registro_de_tabla("osiris_erp_factura_enca","numero_factura","WHERE serie = '"+entry_serie_factura.Text+"' AND numero_factura ='"+int.Parse(entry_folio_factura.Text).ToString().Trim()+"' AND id_emisor = '"+entry_id_emisor.Text.Trim()+"' ","numero_factura","string") == ""){
					LeerXML(filechooserbutton1.Filename,true);
				}else{
					MessageDialog msgBox = new MessageDialog (MyWin,DialogFlags.Modal,
												MessageType.Info,ButtonsType.Ok,"La factura NO se almaceno, verifique...");
					msgBox.Run();	msgBox.Destroy();
				}
			}else{
				MessageDialog msgBox = new MessageDialog (MyWin,DialogFlags.Modal,
												MessageType.Info,ButtonsType.Ok,"NO esta Almacenado el Emisor, verificar la tabla osris_erp_emisor...");
				msgBox.Run();	msgBox.Destroy();
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
				case "combobox_emisor":
					idemisor = (int) onComboBoxChanged.Model.GetValue(iter,1);
					break;
				}
			}
		}		
				
		void limpia_datos_de_entry()
		{
			entry_id_cliente.Text = ""; 
 			entry_nombre_cliente.Text = "";
 			entry_rfc_cliente.Text = "";
 			entry_curp_cliente.Text = "";
 			entry_cp_cliente.Text = "";
 			entry_direccion_cliente.Text = "";
 			entry_colonia_cliente.Text = "";
 			entry_municipio_cliente.Text = "";
			entry_estado_cliente.Text = "";
 			entry_telefono_cliente.Text = "";
 			entry_fax_cliente.Text = "";
 			entry_correo_electronico.Text = "";
 			entry_status_factura.Text = "";
						
			entry_subtotal_15.Text = "0";
 			entry_subtotal_0.Text = "0";
			entry_total_iva.Text = "0";
 			entry_subtotal.Text = "0";
 			entry_deducible_factura.Text = "0";
 			entry_coaseguro_porcentage.Text = "0";
 			entry_coaseguro_factura.Text = "0";
 			entry_total_factura.Text = "0";
 			entry_creada_por.Text = "";
		}
		
		void on_button_selecciona_factura_clicked(object sender, EventArgs args)
		{
			llenado_de_factura();
		}
		
		void llenado_de_factura()
		{
			//checkbutton_nueva_factura.Sensitive = false;
		 	button_guardar_factura.Sensitive = false;
		 	button_imprime_factura.Sensitive = true;
		 	checkbutton_nueva_factura.Active = false;
		 	button_cancela_factura.Sensitive = true;
		 	button_pagar_factura.Sensitive = true;
			// Procesando el encabezado de la Factura
			NpgsqlConnection conexion;
			conexion = new NpgsqlConnection (connectionString+nombrebd );
			// Verifica que la base de datos este conectada
			try{
				conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand ();
				comando.CommandText = "SELECT osiris_erp_factura_enca.id_cliente,osiris_erp_factura_enca.descripcion_cliente,osiris_erp_factura_enca.direccion_cliente,osiris_erp_factura_enca.colonia_cliente,osiris_erp_factura_enca.municipio_cliente,"+
									"osiris_erp_factura_enca.estado_cliente,osiris_erp_factura_enca.rfc_cliente,osiris_erp_factura_enca.curp_cliente,osiris_erp_factura_enca.telefono1_cliente,osiris_erp_factura_enca.telefono2_cliente,"+
									"osiris_erp_factura_enca.fax_cliente,osiris_erp_factura_enca.mail_cliente,osiris_erp_factura_enca.cp_cliente,"+
									"osiris_erp_factura_enca.contacto_cliente,osiris_erp_factura_enca.telefono_contacto_cliente,deducible AS deducible_,coaseguro AS coaseguro_,"+
									"honorario_medico AS honorariomedico,subtotal_factura,sub_total_sin_impuesto,"+
									"total_impuesto_factura,valor_coaseguro AS valorcoaseguro,numero_factura,total_factura,"+
									"cancelado,to_char(fechahora_cancelacion,'dd-MM-yyyy') AS fechahoracancelacion,to_char(fechahora_factura,'dd-MM-yyyy') AS fechafactura,"+
									"motivo_cancelacion,pagada,to_char(fechahora_pago_factura,'dd-MM-yyyy') AS fechahorapagofactura,"+
									"osiris_erp_factura_enca.id_quien_creo,osiris_erp_clientes.envio_factura,numero_ntacred,"+
									"osiris_erp_factura_enca.enviado "+
									"FROM osiris_erp_factura_enca,osiris_erp_clientes "+
									"WHERE osiris_erp_factura_enca.id_cliente = osiris_erp_clientes.id_cliente "+
									"AND id_emisor = '"+idemisor.ToString().Trim()+"' "+
									"AND serie = '"+entry_serie_factura_fact.Text.Trim().ToUpper()+"' "+
									"AND numero_factura = '"+entry_numero_factura.Text.Trim()+"';";
				//Console.WriteLine(comando.CommandText);
				NpgsqlDataReader lector = comando.ExecuteReader ();
						
				if ((bool) lector.Read()){
					
					enviofactura_cliente = (bool) lector["envio_factura"];
					enviofactura_factutu = (bool) lector["enviado"];				
				
					//nota Credito
					idcliente = (int) lector["id_cliente"];
					num_nota = (int) lector["numero_ntacred"];
					
					numerodefactura = entry_numero_factura.Text.Trim().ToUpper();
					seriedefactura = entry_serie_factura_fact.Text.Trim().ToUpper();
					entry_serie_factura_fact.Text = entry_serie_factura_fact.Text.Trim().ToUpper();					
					entry_id_cliente.Text = ""; 
					entry_fecha_factura.Text = (string) lector["fechafactura"];
		 			entry_nombre_cliente.Text = (string) lector["descripcion_cliente"];
		 			entry_rfc_cliente.Text = (string) lector["rfc_cliente"];
		 			entry_curp_cliente.Text = (string) lector["curp_cliente"];
		 			entry_cp_cliente.Text = (string) lector["cp_cliente"];
		 			entry_direccion_cliente.Text = (string) lector["direccion_cliente"];
		 			entry_colonia_cliente.Text = (string) lector["colonia_cliente"];
		 			entry_municipio_cliente.Text = (string) lector["municipio_cliente"];
					entry_estado_cliente.Text = (string) lector["estado_cliente"];
		 			entry_telefono_cliente.Text = (string) lector["telefono1_cliente"];
		 			entry_fax_cliente.Text = (string) lector["fax_cliente"];
		 			entry_correo_electronico.Text = (string) lector["mail_cliente"];
		 			entry_creada_por.Text = (string) lector["id_quien_creo"];
								
					entry_subtotal_15.Text = float.Parse(lector["subtotal_factura"].ToString().Trim(),System.Globalization.NumberStyles.Float,new System.Globalization.CultureInfo("es-MX")).ToString().PadLeft(10);
		 			entry_subtotal_0.Text = float.Parse(lector["sub_total_sin_impuesto"].ToString().Trim(),System.Globalization.NumberStyles.Float,new System.Globalization.CultureInfo("es-MX")).ToString("C").PadLeft(10);
					entry_total_iva.Text = float.Parse(lector["total_impuesto_factura"].ToString().Trim(),System.Globalization.NumberStyles.Float,new System.Globalization.CultureInfo("es-MX")).ToString("C").PadLeft(10);
					
		 			entry_subtotal.Text = (float.Parse(lector["honorariomedico"].ToString().Trim(),System.Globalization.NumberStyles.Float,new System.Globalization.CultureInfo("es-MX"))+ 
		 						  	  	   float.Parse(lector["subtotal_factura"].ToString().Trim(),System.Globalization.NumberStyles.Float,new System.Globalization.CultureInfo("es-MX"))+
		 							  	   float.Parse(lector["sub_total_sin_impuesto"].ToString().Trim(),System.Globalization.NumberStyles.Float,new System.Globalization.CultureInfo("es-MX"))+
		 							  	   float.Parse(lector["total_impuesto_factura"].ToString().Trim(),System.Globalization.NumberStyles.Float,new System.Globalization.CultureInfo("es-MX"))).ToString("C").PadLeft(10);
		 			
		 			entry_deducible_factura.Text = float.Parse(lector["deducible_"].ToString().Trim(),System.Globalization.NumberStyles.Float,new System.Globalization.CultureInfo("es-MX")).ToString("C").PadLeft(10);
		 			entry_coaseguro_porcentage.Text = float.Parse(lector["valorcoaseguro"].ToString().Trim(),System.Globalization.NumberStyles.Float,new System.Globalization.CultureInfo("es-MX")).ToString("C").PadLeft(10);
		 			entry_coaseguro_factura.Text = float.Parse(lector["coaseguro_"].ToString().Trim(),System.Globalization.NumberStyles.Float,new System.Globalization.CultureInfo("es-MX")).ToString("C").PadLeft(10);
		 					 			
		 			entry_total_factura.Text = ((float.Parse(lector["honorariomedico"].ToString().Trim(),System.Globalization.NumberStyles.Float,new System.Globalization.CultureInfo("es-MX"))+
		 									     float.Parse(lector["subtotal_factura"].ToString().Trim(),System.Globalization.NumberStyles.Float,new System.Globalization.CultureInfo("es-MX"))+
		 									     float.Parse(lector["sub_total_sin_impuesto"].ToString().Trim(),System.Globalization.NumberStyles.Float,new System.Globalization.CultureInfo("es-MX"))+
		 									     float.Parse(lector["total_impuesto_factura"].ToString().Trim(),System.Globalization.NumberStyles.Float,new System.Globalization.CultureInfo("es-MX")))-
		 									    (float.Parse(lector["deducible_"].ToString().Trim(),System.Globalization.NumberStyles.Float,new System.Globalization.CultureInfo("es-MX"))+
		 									     float.Parse(lector["valorcoaseguro"].ToString().Trim(),System.Globalization.NumberStyles.Float,new System.Globalization.CultureInfo("es-MX")))).ToString("C").Trim();
					
					totaldefactura = float.Parse(lector["total_factura"].ToString().Trim());
		 									     
		 			totalhonorariomedico = float.Parse(lector["honorariomedico"].ToString().Trim(),System.Globalization.NumberStyles.Float,new System.Globalization.CultureInfo("es-MX"));
					
					subtotal_al_0 = 0; //decimal.Parse(lector["subtotal_sin_impuesto"].ToString().Trim(),System.Globalization.NumberStyles.Float,new System.Globalization.CultureInfo("es-MX"));
                    
					subtotal_al_impuesto = 0;decimal.Parse(lector["total_impuesto_factura"].ToString().Trim(),System.Globalization.NumberStyles.Float,new System.Globalization.CultureInfo("es-MX"));
					
                    total_de_iva = 0; //decimal.Parse(lector["total_impuesto_factura"].ToString().Trim(),System.Globalization.NumberStyles.Float,new System.Globalization.CultureInfo("es-MX"));
                    subtotales = Convert.ToDecimal((float.Parse(lector["honorariomedico"].ToString().Trim(),System.Globalization.NumberStyles.Float,new System.Globalization.CultureInfo("es-MX"))+
                                                  float.Parse(lector["subtotal_factura"].ToString().Trim(),System.Globalization.NumberStyles.Float,new System.Globalization.CultureInfo("es-MX"))+
                                                  float.Parse(lector["sub_total_sin_impuesto"].ToString().Trim(),System.Globalization.NumberStyles.Float,new System.Globalization.CultureInfo("es-MX"))+
                                                  float.Parse(lector["total_impuesto_factura"].ToString().Trim(),System.Globalization.NumberStyles.Float,new System.Globalization.CultureInfo("es-MX")))-
                                                 (float.Parse(lector["deducible_"].ToString().Trim(),System.Globalization.NumberStyles.Float,new System.Globalization.CultureInfo("es-MX"))+
                                                  float.Parse(lector["valorcoaseguro"].ToString().Trim(),System.Globalization.NumberStyles.Float,new System.Globalization.CultureInfo("es-MX"))));
                    
					
					
		 			//Console.WriteLine(((float.Parse((string)lector["honorariomedico"])+float.Parse((string) lector["subtotal_15"])+float.Parse((string) lector["subtotal_0"])+float.Parse((string) lector["ivaal_15"]))-(float.Parse((string) lector["deducible_"])+float.Parse((string) lector["valorcoaseguro"]))).ToString("C").Trim());
		 			cantidad_en_letras = "";	     
		 			//cantidad_en_letras = class_public.ConvertirCadena(((float.Parse((string)lector["honorariomedico"])+float.Parse((string) lector["subtotal_factura"])+float.Parse((string) lector["subtotal_sin_impuesto"])+float.Parse((string) lector["total_impuesto_factura"]))-(float.Parse((string) lector["deducible_"])+float.Parse((string) lector["valorcoaseguro"]))).ToString().Trim(),"Peso");
		 			
		 			entry_status_factura.Text = "";
		 			
		 			if ((bool) lector["pagada"] == true){
		 				button_pagar_factura.Sensitive = false;
		 				this.button_cancela_factura.Sensitive = false;
		 				entry_status_factura.Text = "FAC. P A G A D A / "+(string) lector["fechahorapagofactura"];
		 				MessageDialog msgBox = new MessageDialog (MyWin,DialogFlags.Modal,
												MessageType.Info,ButtonsType.Ok,"Factura PAGADA con Fecha "+(string) lector["fechahorapagofactura"]);
						msgBox.Run();	msgBox.Destroy();
		 			}
		 			if ((bool) lector["cancelado"] == true){
						button_pagar_factura.Sensitive = false;
		 				this.button_cancela_factura.Sensitive = false;
		 				entry_status_factura.Text = "FAC. C A N C E L A D A / "+(string) lector["fechahoracancelacion"];
		 				MessageDialog msgBox = new MessageDialog (MyWin,DialogFlags.Modal,
												MessageType.Info,ButtonsType.Ok,"Factura CANCELADA con Fecha "+(string) lector["fechahoracancelacion"]);
						msgBox.Run();	msgBox.Destroy();
		 			}		

					conexion.Close();
					
					treeViewEngineDetaFact.Clear();  // limpia de talle de factura
					// Llenando el DETALLE DE LA FACTURA
					
					NpgsqlConnection conexion1;
					conexion1 = new NpgsqlConnection (connectionString+nombrebd );
					// Verifica que la base de datos este conectada
					try{
						conexion1.Open ();
						NpgsqlCommand comando1; 
						comando1 = conexion1.CreateCommand ();
						comando1.CommandText = "SELECT numero_factura,to_char(cantidad_detalle,'99999999.99') AS cantidaddetalle,descripcion_detalle,"+
												"to_char(precio_unitario,'99999999.99') AS preciounitario,to_char(importe_detalle,'99999999.99') AS importedetalle "+
												"FROM osiris_erp_factura_deta " +
												"WHERE numero_factura = '"+numerodefactura+"' " +
												"AND id_emisor = '"+idemisor.ToString().Trim()+"' "+
												"AND serie = '"+seriedefactura+"' " +
												"AND eliminado = 'false' "+
												"ORDER BY id_secuencia;";
						NpgsqlDataReader lector1 = comando1.ExecuteReader ();
						
						string variable_paso_01 = "";
						string variable_paso_02 = "";
						string variable_paso_03 = "";
						while (lector1.Read()){
							// validando cantidad detalle
							if (float.Parse((string)lector1["cantidaddetalle"]) == 0){
								 variable_paso_01 = "";
							}else{
								variable_paso_01 = (string) lector1["cantidaddetalle"];
							}
							// validando precio
							if (float.Parse((string) lector1["preciounitario"]) == 0){
								 variable_paso_02 = "";
							}else{
								variable_paso_02 = (string) lector1["preciounitario"];
							}
							// validando importe
							if (float.Parse((string)lector1["importedetalle"]) == 0){
								 variable_paso_03 = "";
							}else{
								variable_paso_03 = (string) lector1["importedetalle"];
							}
							treeViewEngineDetaFact.AppendValues(variable_paso_01,(string) lector1["descripcion_detalle"],variable_paso_02,variable_paso_03,true);
							
						}
						conexion1.Close();
					}catch (NpgsqlException ex){
						MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
												MessageType.Error, 
												ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
						msgBoxError.Run ();
						msgBoxError.Destroy();
					}
				}else{
					limpia_datos_de_entry();
					treeViewEngineDetaFact.Clear();  // limpia de talle de factura
					MessageDialog msgBox = new MessageDialog (MyWin,DialogFlags.Modal,
												MessageType.Info,ButtonsType.Ok,"La factura seleccionada NO EXISTE verifique...");
					msgBox.Run();	msgBox.Destroy();
					error_no_existe = true;
				}
				
			}catch (NpgsqlException ex){
					MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
											MessageType.Error, 
											ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
					msgBoxError.Run ();		msgBoxError.Destroy();
			}
		}
		
		void on_button_cancela_factura_clicked(object sender, EventArgs args)
		{
			MessageDialog msgBox = new MessageDialog (MyWin,DialogFlags.Modal,
			                           MessageType.Question,ButtonsType.YesNo,"¿ Esta seguro de CANCELA FACTURA Nº "+entry_numero_factura.Text.Trim()+" ?");
			ResponseType miResultado = (ResponseType)msgBox.Run ();
			msgBox.Destroy();
		 			
		 	if (miResultado == ResponseType.Yes){
				cancelar_factura();		 	
		 	}
		}

		void cancelar_factura()
		{
			if((string) classpublic.lee_registro_de_tabla("osiris_erp_factura_enca","cancelado","WHERE numero_factura = '"+numerodefactura+"' AND serie = '"+seriedefactura +"' AND id_emisor = '"+idemisor.ToString().Trim()+"' ","cancelado","bool") == "False"){
				string[,] parametros;
				object[] paraobj;

				parametros = new string[,] {
					{ "cancelado = '","true'," },
					{ "total_factura = total_factura * "," - 1," },
					{ "subtotal_factura = subtotal_factura * "," - 1," },
					{ "total_impuesto_factura = total_impuesto_factura * "," - 1," },
					{ "id_quien_cancelo = '",LoginEmpleado+"'," },
					{ "fechahora_cancelacion = '",DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"' " },
					{ "WHERE osiris_erp_factura_enca.numero_factura = '",numerodefactura+"' " },
					{ "AND osiris_erp_factura_enca.id_emisor = '",idemisor.ToString().Trim()+"';"}
				};
				paraobj = new object[] { entry_nro_factura};
				new osiris.update_registro ("osiris_erp_factura_enca", parametros, paraobj);

				// limpiar registros en tabla osiris_erp_abonos
				parametros = new string[,] {
					{ "numero_factura = '","' " },
					{ "WHERE numero_factura = '",seriedefactura.Trim()+numerodefactura.Trim()+"' " },
					{ "AND id_emisor = '",idemisor.ToString().Trim()+"';"}
				};
				paraobj = new object[] { entry_nro_factura};
				new osiris.update_registro ("osiris_erp_abonos", parametros, paraobj);

				// limpiar registros en tabla osiris_erp_cobros_enca
				// excluye el numero de factura del string donde almacena las facturas nombre del campo -->numero_factura<--
				string serie_nro_factura = seriedefactura.Trim () + numerodefactura.Trim ()+";";
				string numeros_de_factura;   // campo del cobro_enca donde esta almacenado los numero de factura
				char[] MyChar;
				string NewString;
				NpgsqlConnection conexion;
				conexion = new NpgsqlConnection (connectionString+nombrebd );
				// Verifica que la base de datos este conectada
				try{
					conexion.Open ();
					NpgsqlCommand comando; 
					comando = conexion.CreateCommand ();
					comando.CommandText = "SELECT * FROM osiris_erp_cobros_enca WHERE numero_factura = '"+serie_nro_factura+"';";
					//Console.WriteLine(comando.CommandText);
					NpgsqlDataReader lector = comando.ExecuteReader ();
					while ((bool) lector.Read()){
						numeros_de_factura = lector["numero_factura"].ToString().Trim();
						MyChar = serie_nro_factura.ToCharArray ();
						NewString = numeros_de_factura.TrimStart(MyChar);

						parametros = new string[,] {
							{ "numero_factura = '",NewString.Trim()+"' " },
							{ "WHERE numero_factura = '",serie_nro_factura+"'; " },
						};
						paraobj = new object[] { entry_nro_factura};
						new osiris.update_registro ("osiris_erp_cobros_enca", parametros, paraobj);
						//Console.WriteLine(NewString);
					}
				}catch(NpgsqlException ex){
					MessageDialog msgBoxError5 = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
									MessageType.Error,ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
					msgBoxError5.Run ();					msgBoxError5.Destroy();
				}
				conexion.Close();

				button_pagar_factura.Sensitive = false;
				this.button_cancela_factura.Sensitive = false;
				entry_status_factura.Text = "FAC. C A N C E L A D A / "+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
				MessageDialog msgBox = new MessageDialog (MyWin,DialogFlags.Modal,
					MessageType.Info,ButtonsType.Ok,"Factura CANCELADA con Fecha "+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
				msgBox.Run();	msgBox.Destroy();
			}	
		}
		
		void on_button_pagar_factura_clicked(object sender, EventArgs args)
		{
			if ((enviofactura_cliente == true && enviofactura_factutu == true) || (enviofactura_cliente == false && enviofactura_factutu == false)){
				if (numerodefactura != ""){
					Glade.XML gxml = new Glade.XML (null, "caja.glade", "fecha_pago_factura", null);
					gxml.Autoconnect (this);
			        // Muestra ventana de Glade
					fecha_pago_factura.Show();
					this.entry_dia.Text = DateTime.Now.ToString("dd");
					this.entry_mes.Text = DateTime.Now.ToString("MM");
					this.entry_ano.Text = DateTime.Now.ToString("yyyy");
					button_guardar_pago.Clicked +=  new EventHandler(on_button_graba_pago_clicked);
					button_salir.Clicked += new EventHandler(on_cierraventanas_clicked);
				}else{
					MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
							MessageType.Error, 	ButtonsType.Close, "No puede pagar factura ya que ha elegido ninguna, verifique...");
					msgBoxError.Run ();				msgBoxError.Destroy();
				}				
			}else{
				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
							MessageType.Error, 	ButtonsType.Close, "NO PUEDE PAGAR ESTA FACTURA YA QUE NO HA SIDO ENVIADA A COBRO...");
				msgBoxError.Run ();				msgBoxError.Destroy();
			}
		}	
		
		void on_button_graba_pago_clicked(object sender, EventArgs args){
			NpgsqlConnection conexion;
			conexion = new NpgsqlConnection (connectionString+nombrebd );
			// Verifica que la base de datos este conectada
			try{
				conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand ();
				comando.CommandText = "UPDATE osiris_erp_factura_enca SET pagada = 'true'," +
										"fechahora_pago_factura = '"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"'," +
										"fecha_pagofactura = '"+DateTime.Now.ToString("yyyy-MM-dd")+"'," +
										"id_quien_pago = '"+LoginEmpleado+"' "+
										"WHERE numero_factura = '"+numerodefactura+"' "+
										"AND id_emisor = '"+idemisor.ToString().Trim()+"';";
				comando.ExecuteNonQuery();
				comando.Dispose();				
				
			}catch (NpgsqlException ex){
				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
										MessageType.Error, 
											ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
				msgBoxError.Run ();
				msgBoxError.Destroy();
			}
			conexion.Close();
		}
						
		void crea_treeview_facturador()
		{
			// Creacion de Liststore
			treeViewEngineDetaFact = new TreeStore(	typeof (string),
													typeof (string),
													typeof (string),
													typeof (string),
													typeof (bool));
		        							   
			treeview_detalle_de_factura.Model = treeViewEngineDetaFact;
			
			CellRendererText cellrt1 = new CellRendererText();  // aplica a todas la columnas
				
			TreeViewColumn col_cantidad = new TreeViewColumn();
			col_cantidad.Title = "Cantidad";
			col_cantidad.PackStart(cellrt1, true);
			col_cantidad.AddAttribute (cellrt1, "text", 0);    // columna 0
			
			TreeViewColumn col_descripcion = new TreeViewColumn();
			col_descripcion.Title = "Descripcion";
			col_descripcion.PackStart(cellrt1, true);
			col_descripcion.AddAttribute (cellrt1, "text", 1);    // columna 1
			
			TreeViewColumn col_precio_unitario = new TreeViewColumn();
			col_precio_unitario.Title = "P. Unitario";
			col_precio_unitario.PackStart(cellrt1, true);
			col_precio_unitario.AddAttribute (cellrt1, "text", 2);    // columna 1
			
			TreeViewColumn col_importe = new TreeViewColumn();
			col_importe.Title = "Importe";
			col_importe.PackStart(cellrt1, true);
			col_importe.AddAttribute (cellrt1, "text", 3);    // columna 1

			treeview_detalle_de_factura.AppendColumn(col_cantidad);
			treeview_detalle_de_factura.AppendColumn(col_descripcion);
			treeview_detalle_de_factura.AppendColumn(col_precio_unitario);
			treeview_detalle_de_factura.AppendColumn(col_importe);
			
		}
		
		// Valida entradas que solo sean numericas, se utiliza en ventana
		// Principal cuando selecciona el folio de productos
		// Ademas controla la tecla ENTRER para ver el procedimiento
		[GLib.ConnectBefore ()]   	  // Esto es indispensable para que funcione    
		public void onKeyPressEvent_enter_factura(object o, Gtk.KeyPressEventArgs args)
		{
			//Console.WriteLine(args.Event.Key);
			if (args.Event.Key.ToString() == "Return" || args.Event.Key.ToString() == "KP_Enter"){
				args.RetVal = true;
				llenado_de_factura();				
			}
			string misDigitos = ".0123456789ﾰﾱﾲﾳﾴﾵﾶﾷﾸﾹﾮｔｒｓｑ（）";
			if (Array.IndexOf(misDigitos.ToCharArray(), Convert.ToChar(args.Event.Key)) == -1 && args.Event.Key.ToString()  != "BackSpace")
			{
				args.RetVal = true;
			}
		}
		
		// cierra ventanas emergentes
		void on_cierraventanas_clicked (object sender, EventArgs args)
		{
			Widget win = (Widget) sender;
			win.Toplevel.Destroy();
		}	        
	}
	

	/// ///////////////////////////////////////////////////////////////////////////// //// 
	/// /////////////////////////// NOTA DE CREDITO ///////////////////////////////// ////
	/// ///////////////////////////////////////////////////////////////////////////// ////
	
	
	public class nota_de_credito
	{
		//nota_de_credito(LoginEmpleado,NomEmpleado,AppEmpleado,ApmEmpleado,nombrebd);
	
		[Widget] Gtk.Window nota_credito;
		[Widget] Gtk.Entry entry_cliente;
		[Widget] Gtk.Entry entry_nota_credito;
		[Widget] Gtk.Entry entry_fecha;
		[Widget] Gtk.Entry entry_factura;	
		[Widget] Gtk.Entry entry_pesos;
		[Widget] Gtk.Entry entry_porciento;
		[Widget] Gtk.Entry entry_sub_15;
		[Widget] Gtk.Entry entry_sub_0;
		[Widget] Gtk.Entry entry_iva_15;
		[Widget] Gtk.Entry entry_sub_total;
		[Widget] Gtk.Entry entry_deducible;
		[Widget] Gtk.Entry entry_coaseguro;
		[Widget] Gtk.Entry entry_total;
		
		[Widget] Gtk.Entry entry_descripcion1;
		[Widget] Gtk.Entry entry_descripcion2;
		[Widget] Gtk.CheckButton checkbutton_descripcion;
		
		[Widget] Gtk.Statusbar statusbar_nota_credito;
		[Widget] Gtk.CheckButton checkbutton_descuento;
		[Widget] Gtk.RadioButton radiobutton_directo;
		[Widget] Gtk.RadioButton radiobutton_porcentage;
		
		[Widget] Gtk.Button button_cancelar;
		[Widget] Gtk.Button button_pagar;
		[Widget] Gtk.Button button_limpiar;
		[Widget] Gtk.Button button_calcular;
		[Widget] Gtk.Button	button_guardar;
		[Widget] Gtk.Button button_imprimir;
		[Widget] Gtk.Button button_salir;
		
		//[Widget] Gtk.TextView TextView_1;
		
		string toma_descrip_municipio = "";
		int num_nota = 0;
		int id_cliente = 0;
		int ultimafactura = 0;
		string numerodefactura;
		decimal calculo = 0;
		string descuento_cliente = "";
		
		//Variables Para utilizar la suma de total en nota de credito
		decimal sub_15 = 0;
		decimal sub_0 = 0;
		decimal tot_iva = 0;
		double valoriva;
					
		decimal subtotal_15;
		decimal subtotal;
		decimal subtotal_0;
		decimal total_de_iva;
		
		string LoginEmpleado;
		string NomEmpleado;
		string AppEmpleado;
		string ApmEmpleado;
			
		// Declarando variable de fuente para la impresion
		// Declaracion de fuentes tipo Bitstream Vera sans
		//public Gnome.Font fuente5 = Gnome.Font.FindClosest("Luxi Sans", 5);
		//public Gnome.Font fuente6 = Gnome.Font.FindClosest("Luxi Sans", 6);
		//public Gnome.Font fuente7 = Gnome.Font.FindClosest("Luxi Sans", 7);
		//public Gnome.Font fuente8 = Gnome.Font.FindClosest("Luxi Sans", 8);//Bitstream Vera Sans
		//public Gnome.Font fuente9 = Gnome.Font.FindClosest("Luxi Sans", 9);
		//public Gnome.Font fuente10 = Gnome.Font.FindClosest("Luxi Sans", 10);
		//public Gnome.Font fuente11 = Gnome.Font.FindClosest("Luxi Sans", 11);
		//public Gnome.Font fuente12 = Gnome.Font.FindClosest("Luxi Sans", 12);
		
		string connectionString;		
		string nombrebd;
		
		//Declaracion de ventana de error y pregunta
		protected Gtk.Window MyWinError;
		protected Gtk.Window MyWin;

		class_conexion conexion_a_DB = new class_conexion();
		class_public classpublic = new class_public();
		
		public nota_de_credito(string LoginEmp_, string NomEmpleado_, string AppEmpleado_, string ApmEmpleado_, string nombrebd_, 
		                       string numerodefactura_, int id_cliente_, decimal subtotal_al_0_, decimal subtotal_al_impuesto_, decimal total_de_iva_,
		                       decimal subtotales_, int num_nota_)
		{
			LoginEmpleado = LoginEmp_;
			NomEmpleado = NomEmpleado_;
			AppEmpleado = AppEmpleado_;
			ApmEmpleado = ApmEmpleado_;
			numerodefactura = numerodefactura_;
			id_cliente = id_cliente_;
			connectionString = conexion_a_DB._url_servidor+conexion_a_DB._port_DB+conexion_a_DB._usuario_DB+conexion_a_DB._passwrd_user_DB;
			nombrebd = conexion_a_DB._nombrebd;
			valoriva = double.Parse(classpublic.ivaparaaplicar)/10;
			
			subtotal_15 = subtotal_al_impuesto_;
			subtotal = subtotales_;
			subtotal_0 = subtotal_al_0_;
			total_de_iva = total_de_iva_;
		    num_nota = num_nota_;
			
			//Console.WriteLine(id_cliente);
			Glade.XML gxml = new Glade.XML (null, "caja.glade", "nota_credito", null);
			gxml.Autoconnect (this);        
			nota_credito.Show();
			
			button_pagar.Hide();
			entry_cliente.Sensitive = false;
			entry_nota_credito.Sensitive = true;
			entry_fecha.Sensitive = false;
			entry_factura.Sensitive = false;
			this.button_guardar.Sensitive = false;
			this.entry_descripcion1.Sensitive = false;
			this.entry_descripcion2.Sensitive = false;
			
			this.entry_porciento.Sensitive = false;
			this.radiobutton_porcentage.Clicked += new EventHandler(on_radiobutton_porcentage_producto);
			this.radiobutton_directo.Clicked += new EventHandler(on_radiobutton_directo);
			checkbutton_descripcion.Clicked += new EventHandler(on_checkbutton_descripcion);
			this.checkbutton_descuento.Clicked += new EventHandler(on_checkbutton_decuento_cliente);
			
			//valida numeros
			this.entry_pesos.KeyPressEvent += onKeyPressEvent_enter_valida_numeros;
			this.entry_porciento.KeyPressEvent += onKeyPressEvent_enter_valida_numeros;
			
			this.button_guardar.Clicked += new EventHandler(on_guarda_clicked);
			this.button_calcular.Clicked += new EventHandler(on_calcula_nota_credito_clicked);
			button_limpiar.Clicked += new EventHandler(on_limpiar_clicked);
			this.button_cancelar.Clicked += new EventHandler(on_cancelar_nota_clicked);
			button_imprimir.Clicked += new EventHandler(on_button_imprimir_clicked);
		
			button_salir.Clicked += new EventHandler(on_cierraventanas_clicked);
		
			statusbar_nota_credito.Pop(0);
			statusbar_nota_credito.Push(1, "login: "+LoginEmpleado+"  |Usuario: "+NomEmpleado+" "+AppEmpleado+" "+ApmEmpleado);
			statusbar_nota_credito.HasResizeGrip = false;		
			
			if(this.num_nota > 0)
			{
				entry_pesos.Sensitive = false;
				entry_porciento.Sensitive = false;
				button_calcular.Sensitive = false;
				button_limpiar.Sensitive = false;	
				button_guardar.Sensitive = false;	
				this.checkbutton_descripcion.Sensitive = false;	
				this.radiobutton_directo.Sensitive = false;	
				this.radiobutton_porcentage.Sensitive = false;	
				entry_nota_credito.Sensitive = false;	
					
				
				NpgsqlConnection conexion;
				conexion = new NpgsqlConnection (connectionString+nombrebd );
				// Verifica que la base de datos este conectada
				try{
					conexion.Open ();
					NpgsqlCommand comando; 
					comando = conexion.CreateCommand ();
			
					// asigna el numero de paciente (PID)
					comando.CommandText = "SELECT to_char(osiris_erp_notacredito_enca.numero_ntacred,'999999999') AS numeronota,"+
						"to_char(osiris_erp_notacredito_enca.numero_factura,'999999999') AS numfact,"+
				        "to_char(osiris_erp_notacredito_enca.sub_total_15,'999999999.99') AS sub15,"+
                        "to_char(osiris_erp_notacredito_enca.sub_total_0,'999999999.99') AS sub0,"+
                        "to_char(osiris_erp_notacredito_enca.iva_al_impuesto,'999999999.99') AS iva15,"+
						"to_char(osiris_erp_notacredito_enca.total,'999999999.99') AS total_,"+
					    "descripcion1, descripcion2,"+
					    "to_char(osiris_erp_notacredito_enca.sub_total_15 + osiris_erp_notacredito_enca.sub_total_0 + osiris_erp_notacredito_enca.iva_al_impuesto,'99999999.99') AS subtotal "+
						"FROM osiris_erp_notacredito_enca "+
	                    "WHERE osiris_erp_notacredito_enca.numero_factura = '"+numerodefactura+"' "+
                        "AND osiris_erp_notacredito_enca.cancelado = false ";

					Console.WriteLine(comando.CommandText.ToString());
	                NpgsqlDataReader lector = comando.ExecuteReader ();

					if(lector.Read())
					{
						this.entry_factura.Text = (string) lector["numfact"];
						this.entry_factura.Text = this.entry_factura.Text.Trim(); 
						
						this.entry_nota_credito.Text = (string) lector["numeronota"];
						this.entry_nota_credito.Text = this.entry_nota_credito.Text.Trim(); 
						
						this.entry_sub_15.Text = (string) lector["sub15"];
						this.entry_sub_15.Text = this.entry_sub_15.Text.Trim(); 
						
						this.entry_iva_15.Text = (string) lector["iva15"];
						this.entry_iva_15.Text = this.entry_iva_15.Text.Trim(); 
						
						this.entry_sub_0.Text = (string) lector["sub0"];
						this.entry_sub_0.Text = this.entry_sub_0.Text.Trim(); 
						
						this.entry_sub_total.Text = (string) lector["subtotal"];
						this.entry_sub_total.Text = this.entry_sub_total.Text.Trim(); 
						
						this.entry_total.Text = (string) lector["total_"];
						this.entry_total.Text = this.entry_total.Text.Trim(); 
						if((string) lector["descripcion1"] == ""){
						}else{						
							this.entry_descripcion1.Text = (string) lector["descripcion1"];						
						}
						if((string) lector["descripcion2"] == ""){
						}else{	
							this.entry_descripcion2.Text = (string) lector["descripcion2"];

						}
					}				
					lector.Close ();
				}catch (NpgsqlException ex){
					MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
					                                               MessageType.Error, 
					                                               ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
					msgBoxError.Run ();						msgBoxError.Destroy();
				}			
				conexion.Close ();
				
				
			}else{
				// Genera el numero nota credito
				NpgsqlConnection conexion;
				conexion = new NpgsqlConnection (connectionString+nombrebd );
				// Verifica que la base de datos este conectada
				try{
					conexion.Open ();
					NpgsqlCommand comando; 
					comando = conexion.CreateCommand ();
			
					// asigna el numero de paciente (PID)
					comando.CommandText = "SELECT numero_ntacred "+				
						"FROM osiris_erp_notacredito_enca "+
	                    "ORDER BY numero_ntacred DESC LIMIT 1;";

	                NpgsqlDataReader lector = comando.ExecuteReader ();

					if ((bool) lector.Read()){
						ultimafactura = (int) lector["numero_ntacred"] + 1;
					}else{		
						ultimafactura = 1;
					}
					
					entry_nota_credito.Text = ultimafactura.ToString();
					this.entry_factura.Text = this.numerodefactura;
				
					
					lector.Close ();
				}catch (NpgsqlException ex){
					MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
					                                               MessageType.Error, 
					                                               ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
					msgBoxError.Run ();						msgBoxError.Destroy();
				}			
				conexion.Close ();
			}
				
			NpgsqlConnection conexion1;
			conexion1 = new NpgsqlConnection (connectionString+nombrebd );
			// Verifica que la base de datos este conectada
			try{
				conexion1.Open ();
				NpgsqlCommand comando1; 
				comando1 = conexion1.CreateCommand ();
				
				// asigna el numero de paciente (PID)
				comando1.CommandText = "SELECT "+
					"osiris_erp_clientes.id_cliente,"+
					"osiris_erp_clientes.descripcion_cliente "+
                    "FROM osiris_erp_clientes "+
                    "WHERE osiris_erp_clientes.id_cliente = '"+id_cliente+"' ";
                    
				//Console.WriteLine("query"+comando1.CommandText);
				NpgsqlDataReader lector = comando1.ExecuteReader ();

				if(lector.Read())
				{
					this.entry_cliente.Text = (string) lector["descripcion_cliente"];
					this.entry_fecha.Text = DateTime.Now.ToString("yyyy-MM-dd");

				}
			
				
				lector.Close ();
			}catch (NpgsqlException ex){
				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
				                                               MessageType.Error, 
				                                               ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
				msgBoxError.Run ();						msgBoxError.Destroy();
			}			
			conexion1.Close ();
	
		}
		
		void on_button_imprimir_clicked(object sender, EventArgs args)
		{
			
		}
		
		/*

			Gnome.PrintJob    trabajo   = new Gnome.PrintJob(Gnome.PrintConfig.Default());
        	Gnome.PrintDialog dialogo   = new Gnome.PrintDialog(trabajo, "Nota de Credito", 0);
        	
        	int         respuesta = dialogo.Run ();
        	
        	if (respuesta == (int) PrintButtons.Cancel) 
			{
				dialogo.Hide (); 
				dialogo.Dispose (); 
				return;
			}
			Gnome.PrintContext ctx = trabajo.Context;
        	ComponerPagina(ctx, trabajo); 
			trabajo.Close();
            switch (respuesta)
        	{
				case (int) PrintButtons.Print:   
                trabajo.Print (); 
                break;
                case (int) PrintButtons.Preview:
                new PrintJobPreview(trabajo, "Nota de Credito").Show();
                break;
        	}
			dialogo.Hide (); dialogo.Dispose (); 
			
		}
			
		void ComponerPagina (Gnome.PrintContext ContextoImp, Gnome.PrintJob trabajoImpresion)
		{   
		
			NpgsqlConnection conexion; 
        	conexion = new NpgsqlConnection (connectionString+nombrebd);
        	// Verifica que la base de dato s este conectada
        	try{
				conexion.Open ();
				NpgsqlCommand comando; 
	        	comando = conexion.CreateCommand ();
	        	        	  
	           	comando.CommandText ="SELECT to_char(osiris_erp_notacredito_enca.numero_factura,'99999999') AS numfact,"+
							"to_char(osiris_erp_notacredito_enca.numero_ntacred,'99999999') AS numnota,"+
							"to_char(osiris_erp_notacredito_enca.sub_total_15,'99999999.99') AS subtotal15,"+
						    "to_char(osiris_erp_notacredito_enca.sub_total_0,'99999999.99') AS subtotal0,"+
							"to_char(osiris_erp_notacredito_enca.iva_al_impuesto,'99999999.99') AS iva15,"+
						    "to_char(osiris_erp_notacredito_enca.total,'99999999.99') AS total_,"+
						    "to_char(osiris_erp_notacredito_enca.fecha_creacion_nota_credito,'dd-MM-yyyy') AS fechcreacion,"+ 	
					        "to_char(osiris_erp_notacredito_enca.sub_total_15 + osiris_erp_notacredito_enca.sub_total_0 + osiris_erp_notacredito_enca.iva_al_impuesto,'99999999.99') AS subtotal,"+
						    "osiris_erp_factura_enca.descripcion_cliente,"+
						    "to_char(osiris_his_paciente.pid_paciente,'99999') AS pid,"+
						    "osiris_erp_notacredito_enca.id_quien_creo,"+
						    "descripcion2,descripcion1,"+
						    "nombre1_paciente || ' ' || nombre2_paciente || ' ' || apellido_paterno_paciente || ' ' || apellido_materno_paciente AS nombre_completo, "+
						    "osiris_erp_factura_enca.municipio_cliente "+
							"FROM osiris_erp_notacredito_enca,osiris_erp_factura_enca,osiris_his_paciente,osiris_erp_cobros_enca "+
							"WHERE osiris_erp_factura_enca.numero_factura = '"+(string) this.entry_factura.Text+"' "+
                            "AND osiris_erp_notacredito_enca.numero_ntacred = '"+(string) this.entry_nota_credito.Text+"' "+
							"AND osiris_erp_cobros_enca.pid_paciente = osiris_his_paciente.pid_paciente "+
						    "AND osiris_erp_cobros_enca.numero_factura = osiris_erp_factura_enca.numero_factura "+
                            "AND osiris_erp_notacredito_enca.numero_factura = '"+(string) this.entry_factura.Text+"' ;";
						Console.WriteLine("esteeeeee......"+comando.CommandText);
	        	NpgsqlDataReader lector = comando.ExecuteReader ();
				ContextoImp.BeginPage("Pagina 1");
				if (lector.Read()){
		        		    
					string traduce = "";
					traduce = traduce_numeros((string) lector["total_"]);
										Gnome.Print.Setfont (ContextoImp, fuente9);
					//ContextoImp.MoveTo(505, 755);		 ContextoImp.Show((string) lector["numnota"]);
					ContextoImp.MoveTo(500, 705);		 ContextoImp.Show((string) lector["fechcreacion"]);
					ContextoImp.MoveTo(500, 670);		 ContextoImp.Show((string) lector["numfact"]);


					
					toma_descrip_municipio = (string) lector["municipio_cliente"];
						if(toma_descrip_municipio.Length > 14){
							toma_descrip_municipio = toma_descrip_municipio.Substring(0,14);
						}  	
						ContextoImp.MoveTo(485, 635);		ContextoImp.Show(toma_descrip_municipio);
					
					
			
					ContextoImp.MoveTo(490, 460);		 ContextoImp.Show((string) lector["subtotal15"]);
					ContextoImp.MoveTo(490, 445);		 ContextoImp.Show((string) lector["subtotal0"]);
					ContextoImp.MoveTo(490, 430);		 ContextoImp.Show((string) lector["iva15"]);
					ContextoImp.MoveTo(490, 392);		 ContextoImp.Show((string) lector["subtotal"]);
					ContextoImp.MoveTo(490, 382);		 ContextoImp.Show((string) lector["total_"]);
					
					ContextoImp.MoveTo(90, 555);		 ContextoImp.Show((string) lector["descripcion2"]);
					ContextoImp.MoveTo(90, 545);		 ContextoImp.Show((string) lector["descripcion1"]);
					
					Gnome.Print.Setfont (ContextoImp, fuente10);
					ContextoImp.MoveTo(90, 610);		 ContextoImp.Show("PID.-  ");
					ContextoImp.MoveTo(120, 610);		 ContextoImp.Show((string) lector["pid"]);
					ContextoImp.MoveTo(90, 595);		 ContextoImp.Show("PACIENTE.-");						
					ContextoImp.MoveTo(150, 595);		 ContextoImp.Show((string) lector["nombre_completo"]);
					
					ContextoImp.MoveTo(70, 665);		 ContextoImp.Show((string) lector["descripcion_cliente"]);
								
					Gnome.Print.Setfont (ContextoImp, fuente12);
					ContextoImp.MoveTo(95, 465 );		 ContextoImp.Show("("+traduce.ToUpper()+")");	
					
					Gnome.Print.Setfont (ContextoImp, fuente6);	
					ContextoImp.MoveTo(400, 340);		 ContextoImp.Show("Creada");					
					ContextoImp.MoveTo(440, 340);		 ContextoImp.Show((string) lector["id_quien_creo"]);
					ContextoImp.MoveTo(400, 330);		 ContextoImp.Show("Nota");
					ContextoImp.MoveTo(450, 330);		 ContextoImp.Show((string) lector["numnota"]);
				}

				
				ContextoImp.ShowPage();
				}catch (NpgsqlException ex){
				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
				MessageType.Warning, ButtonsType.Ok, "PostgresSQL error: {0}",ex.Message);
				msgBoxError.Run ();
				                                               msgBoxError.Destroy();
			}	
				
		}
		*/
		
		void on_checkbutton_descripcion(object sender, EventArgs args)
		{
			if(checkbutton_descripcion.Active == true){				
				this.entry_descripcion1.Sensitive = true;
				this.entry_descripcion2.Sensitive = true;
			}else{
				this.entry_descripcion1.Sensitive = false;
				this.entry_descripcion2.Sensitive = false;
			}
		}
			
		void on_radiobutton_porcentage_producto(object sender, EventArgs args)
		{
			if(this.radiobutton_porcentage.Active == true){				
				this.entry_porciento.Sensitive = true;
			}else{
				this.entry_porciento.Sensitive = false;
			}
		}
		
		void on_radiobutton_directo(object sender, EventArgs args)
		{
			if(this.radiobutton_directo.Active == true){
				this.entry_pesos.Sensitive = true;
			}else{
				this.entry_pesos.Sensitive = false;
			}
		}
		
		void on_checkbutton_decuento_cliente(object sender, EventArgs args)
		{
			if(this.checkbutton_descuento.Active == true){
				NpgsqlConnection conexion; 
				conexion = new NpgsqlConnection (connectionString+nombrebd);
				// Verifica que la base de dato s este conectada
				try{
					conexion.Open ();
					NpgsqlCommand comando; 
					comando = conexion.CreateCommand ();
	        	        	  
					comando.CommandText ="SELECT id_cliente,"+
							"to_char(porcentage_descuento, '999.99') AS porcentagedescuento "+
							"FROM osiris_erp_clientes "+
							"WHERE id_cliente = '"+this.id_cliente+"' ;";
					Console.WriteLine("es   "+comando.CommandText);
					NpgsqlDataReader lector = comando.ExecuteReader ();
					if (lector.Read()){

						if(Convert.ToDecimal((string) lector["porcentagedescuento"]) > 0){
							this.entry_porciento.Text =  (string) lector["porcentagedescuento"];
							this.entry_pesos.Sensitive = false;
							this.radiobutton_directo.Sensitive = false;
						}else{							
							MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
							                                               MessageType.Error, 
							                                               ButtonsType.Close, "No existe ningun descuento  \n"+
							                                               "para este cliente");
							msgBoxError.Run ();
							msgBoxError.Destroy();
							
							this.radiobutton_porcentage.Active = false;
						}
								
					}
				}catch (NpgsqlException ex){
					MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
					MessageType.Warning, ButtonsType.Ok, "PostgresSQL error: {0}",ex.Message);
					msgBoxError.Run ();
					msgBoxError.Destroy();
				}	
			}
		}
		
		
		void on_calcula_nota_credito_clicked(object sender, EventArgs args)
		{
			sub_15 = 0;
			sub_0 = 0;
			tot_iva = 0;
			this.entry_iva_15.Text = "0.00";
			this.entry_sub_0.Text = "0.00";
			this.entry_sub_15.Text = "0.00";
			this.entry_sub_total.Text = "0.00";
			this.entry_total.Text = "0.00";
			this.button_guardar.Sensitive = true;
			if(this.radiobutton_directo.Active == true){
			
		
					if(this.id_cliente == 1)
				    {	
						calculo = Convert.ToDecimal(this.entry_pesos.Text);
						sub_0 = this.calculo;

						this.entry_sub_15.Text = sub_0.ToString("F");
						this.entry_sub_total.Text = (sub_15 + sub_0 + tot_iva).ToString("F");	 	
						this.entry_total.Text = (sub_15 + sub_0 + tot_iva).ToString("F");
					}else{	
						calculo = Convert.ToDecimal(this.entry_pesos.Text);
						sub_15 = Convert.ToDecimal(Convert.ToDouble(this.calculo)/valoriva);
						tot_iva = (calculo - sub_15);
						
						this.entry_iva_15.Text = tot_iva.ToString("F");
						this.entry_sub_15.Text = sub_15.ToString("F");
						this.entry_sub_total.Text = (sub_15 + sub_0 + tot_iva).ToString("F");	 	
						this.entry_total.Text = (sub_15 + sub_0 + tot_iva).ToString("F");
					}

			}else{
				if(this.id_cliente == 1)
				{	

					calculo = Convert.ToDecimal(entry_porciento.Text);		
										
					sub_15 = (this.subtotal_15 + this.total_de_iva)/calculo;
										
					this.entry_sub_15.Text = sub_15.ToString("F");
					this.entry_sub_total.Text = sub_15.ToString("F");	 	
					this.entry_total.Text = sub_15.ToString("F");
				}else{
					this.calculo = Convert.ToDecimal(entry_porciento.Text);		
					this.sub_0 = subtotal_0 / calculo;
					this.entry_sub_0.Text = sub_0.ToString("F");	
					Console.WriteLine(sub_0);
					this.sub_15 = subtotal_15 / calculo;
					this.entry_sub_15.Text = sub_15.ToString("F");	
									Console.WriteLine(sub_15);
					this.tot_iva = total_de_iva / calculo;
					this.entry_iva_15.Text = tot_iva.ToString("F");	
					
					this.entry_sub_total.Text = (sub_15 + sub_0 + tot_iva).ToString("F");	 	
					this.entry_total.Text = (sub_15 + sub_0 + tot_iva).ToString("F");	 
				}
			}
			this.entry_sub_total.Text = (sub_15 + sub_0 + tot_iva).ToString("F");	 	
			
		}
		
		void on_guarda_clicked (object sender, EventArgs args)
		{
			NpgsqlConnection conexion;
			conexion = new NpgsqlConnection (connectionString+nombrebd );
			// Verifica que la base de datos este conectada
			try{
				conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand ();
					
				comando.CommandText = "INSERT INTO osiris_erp_notacredito_enca ("+
										"numero_factura,"+//1
	             						"numero_ntacred,"+
	                					"fecha_creacion_nota_credito,"+
	                					"sub_total_15,"+
	                					"sub_total_0,"+
	                					"iva_al_impuesto,"+
						                "descripcion1,"+
						                "descripcion2,"+
	                					"id_quien_creo,"+
	                					"total) "+
	                					"VALUES ('"+
	                					this.entry_factura.Text.Trim()+"','"+
	                					this.entry_nota_credito.Text.Trim()+"','"+
	                				    DateTime.Now.ToString("yyyy-MM-dd")+"','"+
	                					this.entry_sub_15.Text.Trim()+"','"+
	                					this.entry_sub_0.Text.Trim()+"','"+
	                					this.entry_iva_15.Text.Trim()+"','"+

						                this.entry_descripcion1.Text.Trim().ToUpper()+"','"+
						                this.entry_descripcion2.Text.Trim().ToUpper()+"','"+
						                ///////
	                					LoginEmpleado+"','"+
				                 		this.entry_total.Text.Trim()+"');";
				comando.ExecuteNonQuery();
				comando.Dispose();
				
				NpgsqlConnection conexion2;
				conexion2 = new NpgsqlConnection (connectionString+nombrebd );
				// Verifica que la base de datos este conectada
				try{
					conexion2.Open ();
					NpgsqlCommand comando2; 
					comando2 = conexion2.CreateCommand ();
					comando2.CommandText =  "UPDATE osiris_erp_factura_enca SET id_quien_creo = ' "+LoginEmpleado+"',"+
											                    "numero_ntacred = '"+entry_nota_credito.Text.Trim()+"',"+
							                                    "id_quien_creo_ntacred = '"+LoginEmpleado+"',"+
											                    "total_ntacred = '"+entry_total.Text.Trim()+"', "+
												                "fechahora_creacion_ntacred = ' "+this.entry_fecha.Text.Trim()+" ' "+
											                    "WHERE numero_factura = '"+this.entry_factura.Text.Trim()+"' ;";
					comando2.ExecuteNonQuery();							
					comando2.Dispose();				
					NpgsqlConnection conexion3;
					conexion3 = new NpgsqlConnection (connectionString+nombrebd );
					// Verifica que la base de datos este conectada
					try{
						conexion3.Open ();
						NpgsqlCommand comando3; 
						comando3 = conexion3.CreateCommand ();
						
						comando3.CommandText =  "UPDATE osiris_erp_cobros_enca SET numero_ntacred = ' "+entry_nota_credito.Text.Trim()+"',"+
											                    "valor_total_notacredito = '"+entry_total.Text.Trim()+" ' "+
											                    "WHERE numero_factura = '"+this.entry_factura.Text.Trim()+"' ;";
		 						Console.WriteLine(comando3.CommandText);	
						comando3.ExecuteNonQuery();							
						comando3.Dispose();
					
					
		

						NpgsqlConnection conexion1;
						conexion1 = new NpgsqlConnection (connectionString+nombrebd );
						// Verifica que la base de datos este conectada
						try{
							conexion1.Open ();
							NpgsqlCommand comando1; 
							comando1 = conexion1.CreateCommand ();
					
							comando1.CommandText = "INSERT INTO osiris_erp_notacredito_deta ("+
								                   "numero_ntacred,"+//1
	                					           "total) "+
	                					           "VALUES ('"+
	                					           this.entry_nota_credito.Text.Trim()+"','"+
	                					           this.entry_total.Text.Trim()+"');";
 							
							comando1.ExecuteNonQuery();							comando1.Dispose();
							
						}catch (NpgsqlException ex){
							MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
							                                               MessageType.Error, 
							                                               ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
							msgBoxError.Run ();
							msgBoxError.Destroy();
						}
						conexion1.Close();

					

						MessageDialog msgBoxError1 = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
						                                                MessageType.Info,ButtonsType.Close, " La Nota de Credito se Guardo Correctamente ");
						msgBoxError1.Run ();			msgBoxError1.Destroy();
						
					}catch (NpgsqlException ex){
						MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
						                                               MessageType.Error, 
						                                               ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
						msgBoxError.Run ();
						msgBoxError.Destroy();
					}
					conexion3.Close();
					
				}catch (NpgsqlException ex){
					MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
				                                               MessageType.Error, 
				                                               ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
					msgBoxError.Run ();
					msgBoxError.Destroy();
				}
				conexion2.Close();
				
			}catch (NpgsqlException ex){
				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
				                                               MessageType.Error, 
				                                               ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
				msgBoxError.Run ();
				msgBoxError.Destroy();
			}
			conexion.Close();
		}		
			
		void on_limpiar_clicked (object sender, EventArgs args)
		{
			sub_15 = 0;
			sub_0 = 0;
			tot_iva = 0;
			this.entry_iva_15.Text = "0.00";
			this.entry_sub_0.Text = "0.00";
			this.entry_sub_15.Text = "0.00";
			this.entry_sub_total.Text = "0.00";
			this.entry_total.Text = "0.00";
		}
		
		void on_cancelar_nota_clicked (object sender, EventArgs args)		
		{
			NpgsqlConnection conexion;
			conexion = new NpgsqlConnection (connectionString+nombrebd );
			// Verifica que la base de datos este conectada
			try{
				conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand ();
		
			
				comando.CommandText = "UPDATE osiris_erp_notacredito_enca SET cancelado = 'true',"+
											    "id_quien_cancelo = ' "+LoginEmpleado+" ' "+
											    "fechahora_de_cancelacion '"+DateTime.Now.ToString("yyyy-MM-dd")+" ' "+
							                    "WHERE numero_ntacred = '"+this.entry_nota_credito.Text+"' ;"; 
							                //    Console.WriteLine("este"+comando3.CommandText.ToString());

				NpgsqlDataReader lector = comando.ExecuteReader ();

				MessageDialog msgBoxError1 = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
				                                               MessageType.Info,ButtonsType.Close, " La Nota de Credito fue Cancelada Satisfactoriamente ");
				msgBoxError1.Run ();			msgBoxError1.Destroy();
				
			}catch (NpgsqlException ex){
				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
				                                               MessageType.Error, 
				                                               ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
				msgBoxError.Run ();
				msgBoxError.Destroy();
			}
			conexion.Close();		
		}

		void on_cierraventanas_clicked (object sender, EventArgs args)
		{
			Widget win = (Widget) sender;
			win.Toplevel.Destroy();
		}
		
		// Valida entradas que solo sean numericas, se utiliza eb ventana de
		// carga de producto
		[GLib.ConnectBefore ()]   	  // Esto es indispensable para que funcione    
		public void onKeyPressEvent_enter_valida_numeros(object o, Gtk.KeyPressEventArgs args)
		{
			//Console.WriteLine(args.Event.Key);
			//Console.WriteLine(Convert.ToChar(args.Event.Key));
			string misDigitos = ".0123456789ﾰﾱﾲﾳﾴﾵﾶﾷﾸﾹﾮｔｒｓｑ（）";
			if (Array.IndexOf(misDigitos.ToCharArray(), Convert.ToChar(args.Event.Key)) == -1 && args.Event.Key.ToString()  != "BackSpace")
			{
				args.RetVal = true;
			}
		}
	}
}