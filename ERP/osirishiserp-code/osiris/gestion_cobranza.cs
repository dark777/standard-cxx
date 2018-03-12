//  
//  gestion_cobranza.cs
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
using Gtk;
using Npgsql;
using Gdk;
using GLib;

using System.Collections;
using OfficeOpenXml;
using System.IO;
using System.Reflection;


namespace osiris
{
	public partial class gestion_cobranza : Gtk.Window
	{
		Gtk.TreeStore treeViewEngineGestion;
		Gtk.TreeStore treeViewEngineServicios;
		ArrayList columns = new ArrayList ();
		
		float montoconvenio = 0;
		int folioservicio = 0;	        		// Toma el valor de numero de atencion de paciente
		int PidPaciente = 0;		   			// Toma la actualizacion del pid del paciente
		int id_tipopaciente = 0;           		// Toma el valor del tipo de paciente
		
		string LoginEmpleado;
		string NomEmpleado;
		string AppEmpleado;
		string ApmEmpleado;
		string nombrebd;
		string connectionString;
		
		TextBuffer buffer = new TextBuffer(null);
		TextIter insertIter;
		
		TextBuffer buffernota = new TextBuffer(null);
		TextIter insertIternota;
		
		//Declaracion de ventana de error
		protected Gtk.Window MyWinError;
		
		class_conexion conexion_a_DB = new class_conexion();
		class_buscador classfind_data = new class_buscador();
		class_public classpublic = new class_public();
		
		public gestion_cobranza (string LoginEmp_,string NomEmpleado_,string AppEmpleado_,string ApmEmpleado_,string nombrebd_) : 
				base(Gtk.WindowType.Toplevel)
		{
			this.Build ();
			connectionString = conexion_a_DB._url_servidor + conexion_a_DB._port_DB + conexion_a_DB._usuario_DB + conexion_a_DB._passwrd_user_DB;
			nombrebd = conexion_a_DB._nombrebd;
			LoginEmpleado = LoginEmp_;
			NomEmpleado = NomEmpleado_;
			AppEmpleado = AppEmpleado_;
			ApmEmpleado = ApmEmpleado_;
			
			entry_pid_paciente.Text = "0";
			entry_folio_servicio.KeyPressEvent += onKeyPressEvent_enter_folio;
			entry_nro_pagare.KeyPressEvent += onKeyPressEvent_enter_nropagare;
			entry_folio_servicio.GrabFocus();
			entry_folio_servicio.ModifyBase(StateType.Normal, new Gdk.Color(255,243,169));	// Color Amarillo
			entry_pid_paciente.ModifyBase(StateType.Normal, new Gdk.Color(166,220,255));	// Color Celeste
			entry_nombre_paciente.ModifyBase(StateType.Normal, new Gdk.Color(166,220,255));	// Color Celeste
			entry_nro_pagare.ModifyBase(StateType.Normal, new Gdk.Color(166,220,255));		// Color Celeste
			entry_estatus.ModifyBase(StateType.Normal, new Gdk.Color(44,211,52));			// Color Verde
			crea_treeview_servicios();
			crea_treeview_gestcobrz();
		}

		protected void on_cierraventanas_clicked (object sender, System.EventArgs e)
		{
			Destroy ();
		}

		// Valida entradas que solo sean numericas, se utiliza en ventana de
		// carga de producto
		[GLib.ConnectBefore ()]   	  // Esto es indispensable para que funcione    
		public void onKeyPressEvent_enter_folio(object o, Gtk.KeyPressEventArgs args)
		{
			//Console.WriteLine(Convert.ToChar(args.Event.KeyValue));
			//Console.WriteLine(args.Event.Key);
			if (args.Event.Key.ToString() == "Return" || args.Event.Key.ToString() == "KP_Enter"){
				args.RetVal = true;
				llenado_folio_seleccionado("AND osiris_erp_cobros_enca.folio_de_servicio = '"+entry_folio_servicio.Text.Trim()+"';");
			}
			string misDigitos = ".0123456789ﾰﾱﾲﾳﾴﾵﾶﾷﾸﾹﾮｔｒｓｑ（）";
			if (Array.IndexOf(misDigitos.ToCharArray(), Convert.ToChar(args.Event.Key)) == -1 && args.Event.Key.ToString()  != "BackSpace")
			{
				args.RetVal = true;
			}
		}
		
		[GLib.ConnectBefore ()]   	  // Esto es indispensable para que funcione    
		public void onKeyPressEvent_enter_nropagare(object o, Gtk.KeyPressEventArgs args)
		{
			//Console.WriteLine(Convert.ToChar(args.Event.KeyValue));
			//Console.WriteLine(args.Event.Key);
			if (args.Event.Key.ToString() == "Return" || args.Event.Key.ToString() == "KP_Enter"){
				args.RetVal = true;
				llenado_folio_seleccionado("AND osiris_erp_comprobante_pagare.numero_comprobante_pagare = '"+entry_nro_pagare.Text.Trim()+"';");
			}
			string misDigitos = ".0123456789ﾰﾱﾲﾳﾴﾵﾶﾷﾸﾹﾮｔｒｓｑ（）";
			if (Array.IndexOf(misDigitos.ToCharArray(), Convert.ToChar(args.Event.Key)) == -1 && args.Event.Key.ToString()  != "BackSpace")
			{
				args.RetVal = true;
			}
		}

		protected void on_button_selecatencion_clicked (object sender, System.EventArgs e)
		{
			llenado_folio_seleccionado("AND osiris_erp_cobros_enca.folio_de_servicio = '"+entry_folio_servicio.Text.Trim()+"';");
		}
		
		void llenado_folio_seleccionado(string sql_tipo_busqueda)
		{
			buffer.Clear();
			buffer = textview_motivo_ingreso.Buffer;
			insertIter = buffer.StartIter;
			
			NpgsqlConnection conexion; 
			conexion = new NpgsqlConnection (connectionString+nombrebd);            
			// Verifica que la base de datos este conectada
			try{
				conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand ();
               	comando.CommandText = "SELECT osiris_erp_cobros_enca.folio_de_servicio AS foliodeatencion,"+
								"osiris_erp_cobros_enca.pagado,"+
								"osiris_erp_cobros_enca.cancelado,"+
								"osiris_erp_cobros_enca.cerrado,"+
								"osiris_erp_cobros_enca.alta_paciente,"+
								"osiris_erp_cobros_enca.bloqueo_de_folio,"+
								"to_char(osiris_erp_cobros_enca.pid_paciente,'9999999999') AS pidpaciente,"+
				            	"nombre1_paciente,nombre2_paciente,apellido_paterno_paciente,apellido_materno_paciente,"+
				            	"telefono_particular1_paciente,telefono_particular2_paciente,telefono_trabajo1_paciente,telefono_trabajo2_paciente,celular1_paciente,celular2_paciente," +
				            	"numero_poliza,folio_de_servicio_dep,"+
				            	"to_char(osiris_erp_cobros_enca.fechahora_creacion,'dd-MM-yyyy HH24:mi:ss') AS fecha_ingreso,"+
				            	"to_char(osiris_erp_cobros_enca.fecha_alta_paciente,'dd-MM-yyyy HH24:mi:ss') AS fecha_egreso,"+
				            	"osiris_erp_cobros_enca.numero_factura AS numerofactura,"+
				            	"to_char(osiris_erp_cobros_enca.deducible,'9999999.99') AS deduciblecaja, "+
				            	"to_char(osiris_erp_cobros_enca.total_abonos,'99999999.99') AS totalabonos, "+
				            	"to_char(osiris_erp_cobros_enca.total_pago,'99999999.99') AS totalpago, "+
				            	"to_char(osiris_erp_cobros_enca.coaseguro,'999.99') AS coasegurocaja,"+
				            	"osiris_erp_cobros_enca.numero_factura AS numfactura,osiris_erp_movcargos.id_tipo_paciente AS idtipopaciente, "+
				            	"osiris_erp_movcargos.id_tipo_admisiones AS idtipoadmision, "+
				            	"osiris_his_paciente.direccion_paciente,osiris_his_paciente.numero_casa_paciente,osiris_his_paciente.numero_departamento_paciente, "+
								"osiris_his_paciente.colonia_paciente,osiris_his_paciente.municipio_paciente,osiris_his_paciente.codigo_postal_paciente,osiris_his_paciente.estado_paciente,  "+
            					"descripcion_tipo_paciente,osiris_his_tipo_cirugias.descripcion_cirugia,"+
            					"osiris_erp_cobros_enca.id_empresa AS idempresa,osiris_empresas.descripcion_empresa,osiris_empresas.lista_de_precio AS listadeprecio_empresa,"+   ///
            					"descripcion_admisiones,osiris_his_tipo_especialidad.descripcion_especialidad,"+
				            	"osiris_erp_cobros_enca.id_aseguradora,descripcion_aseguradora,osiris_aseguradoras.lista_de_precio AS listadeprecio_aseguradora,"+   ///
				            	"osiris_erp_cobros_enca.id_medico,nombre_medico,nombre_medico_encabezado,osiris_erp_cobros_enca.id_medico_tratante,nombre_medico_tratante,"+
				            	"osiris_erp_movcargos.descripcion_diagnostico_movcargos,osiris_his_tipo_cirugias.id_tipo_cirugia,"+
				            	"osiris_erp_cobros_enca.facturacion,osiris_erp_cobros_enca.pagado,osiris_erp_cobros_enca.monto_convenio,id_referido,"+
				            	"osiris_erp_comprobante_pagare.numero_comprobante_pagare,osiris_his_paciente.paciente_bloqueado_cc," +
				            	"to_char(osiris_erp_comprobante_pagare.fechahora_creacion,'dd-MM-yyyy') AS fechacreapagare," +
				            	"to_char(osiris_erp_comprobante_pagare.fecha_vencimiento_pagare,'dd-MM-yyyy') AS fechahora_vencpagare " +
				            	"FROM "+ 
				            	"osiris_erp_cobros_enca,osiris_his_paciente,osiris_erp_movcargos,osiris_his_tipo_admisiones,osiris_his_tipo_pacientes, "+
				            	"osiris_erp_comprobante_pagare,osiris_aseguradoras,osiris_his_medicos,osiris_his_tipo_cirugias,osiris_his_tipo_especialidad,osiris_empresas "+
				            	"WHERE "+
				            	"osiris_erp_cobros_enca.pid_paciente = osiris_his_paciente.pid_paciente "+
				            	"AND osiris_erp_movcargos.folio_de_servicio = osiris_erp_cobros_enca.folio_de_servicio "+
				            	"AND osiris_erp_cobros_enca.id_medico = osiris_his_medicos.id_medico "+ 
								"AND osiris_erp_movcargos.id_tipo_admisiones = osiris_his_tipo_admisiones.id_tipo_admisiones "+
								"AND osiris_erp_movcargos.id_tipo_paciente = osiris_his_tipo_pacientes.id_tipo_paciente "+
								"AND osiris_erp_movcargos.id_tipo_cirugia = osiris_his_tipo_cirugias.id_tipo_cirugia "+
								"AND osiris_his_medicos.id_especialidad = osiris_his_tipo_especialidad.id_especialidad  "+
								"AND osiris_erp_cobros_enca.id_empresa = osiris_empresas.id_empresa "+
								"AND osiris_erp_cobros_enca.id_aseguradora = osiris_aseguradoras.id_aseguradora "+
								"AND osiris_erp_cobros_enca.folio_de_servicio = osiris_erp_comprobante_pagare.folio_de_servicio "+
								sql_tipo_busqueda;
								
				//Console.WriteLine(comando.CommandText);					
				NpgsqlDataReader lector = comando.ExecuteReader ();
				if(lector.Read()){
					entry_folio_servicio.Text = lector["foliodeatencion"].ToString();
					entry_ingreso.Text = lector["fecha_ingreso"].ToString();
					entry_egreso.Text = lector["fecha_egreso"].ToString();
					entry_pid_paciente.Text = lector["pidpaciente"].ToString().Trim();
					entry_nombre_paciente.Text = lector["nombre1_paciente"].ToString().Trim()+" "+
												lector["nombre2_paciente"].ToString().Trim()+" "+
												lector["apellido_paterno_paciente"].ToString().Trim()+" "+
												lector["apellido_materno_paciente"].ToString().Trim();
					buffer.Insert (ref insertIter,lector["descripcion_diagnostico_movcargos"].ToString().Trim().ToUpper());
					id_tipopaciente = (int) lector["idtipopaciente"];
					entry_tipo_paciente.Text = lector["descripcion_tipo_paciente"].ToString();
					montoconvenio = float.Parse(lector["monto_convenio"].ToString());
					entry_monto_convenio.Text = montoconvenio.ToString("F");
					entry_nro_pagare.Text = lector["numero_comprobante_pagare"].ToString().Trim();
					entry_fecha_pagare.Text = lector["fechacreapagare"].ToString();
					entry_fecha_vencpagare.Text = lector["fechahora_vencpagare"].ToString();
					entry_direccion_px.Text = lector["direccion_paciente"].ToString().Trim()+" "+lector["numero_casa_paciente"].ToString().Trim()+" "+
            									lector["numero_departamento_paciente"].ToString().Trim()+", COL. "+lector["colonia_paciente"].ToString().Trim()+
            									", CP. "+lector["codigo_postal_paciente"].ToString().Trim()+", "+lector["municipio_paciente"].ToString().Trim()+", "+lector["estado_paciente"].ToString().Trim();
					
					llena_servicios_realizados(lector["pidpaciente"].ToString().Trim());
					llena_movcobranza(lector["foliodeatencion"].ToString());
					folioservicio = (int) lector["foliodeatencion"];
					PidPaciente = int.Parse(lector["pidpaciente"].ToString());
					if((bool) lector["paciente_bloqueado_cc"]){
						entry_estatus.Text = "EXPEDIENTE BLOQUEADO";
						label30.Text = "DesBloqueo Exp";
					}else{
						label30.Text = "Bloquear Exp";
					}					
					
					comboboxentry_telefono.Clear();
					//Gtk.ComboBox combobox_llenado = (Gtk.ComboBox) obj;
					//Gtk.ComboBox combobox_pos_neg = obj as Gtk.ComboBox;
					CellRendererText cell = new CellRendererText();
					comboboxentry_telefono.PackStart(cell, true);
					comboboxentry_telefono.AddAttribute(cell,"text",0);
					
					CellRendererText ubicacion = new CellRendererText();
					comboboxentry_telefono.PackStart(ubicacion, true);
					comboboxentry_telefono.AddAttribute(ubicacion,"text",1);
					
					ListStore store = new ListStore( typeof (string),typeof (string));
					comboboxentry_telefono.Model = store;			
					
					store.AppendValues (lector["telefono_particular1_paciente"].ToString().Trim(),"PARTICULAR 1");
					store.AppendValues (lector["telefono_particular2_paciente"].ToString().Trim(),"PARTICULAR 2");
					store.AppendValues (lector["telefono_trabajo1_paciente"].ToString().Trim(),"TRAJABO 1");
					store.AppendValues (lector["telefono_trabajo2_paciente"].ToString().Trim(),"TRABAJO 2");
					store.AppendValues (lector["celular1_paciente"].ToString().Trim(),"CELULAR 1");
					store.AppendValues (lector["celular2_paciente"].ToString().Trim(),"CELULAR 2");
					
				}else{
					MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
							MessageType.Error,ButtonsType.Close, "Numero de Atencion no EXISTE, verifique...");
							msgBoxError.Run ();				msgBoxError.Destroy();
				}
			}catch (NpgsqlException ex){
				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.Modal,
						MessageType.Error, 
						ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
				msgBoxError.Run ();				msgBoxError.Destroy();
	   			
	       	}
       		conexion.Close ();
		}

		protected void on_button_abonar_clicked (object sender, System.EventArgs e)
		{
			if (entry_folio_servicio.Text == "" && PidPaciente != 0){	
				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
				MessageType.Error, 
				ButtonsType.Close, "Debe de llenar el campo de Folio de Atencion, verifique...");
				msgBoxError.Run ();
				msgBoxError.Destroy();
			}else{
				new osiris.abonos(int.Parse(entry_pid_paciente.Text),int.Parse(entry_folio_servicio.Text),nombrebd,
						entry_ingreso.Text,entry_egreso.Text,entry_numero_factura.Text,
						entry_nombre_paciente.Text,"Telefono px","doctor primer contacto",
						entry_tipo_paciente.Text,"entry_aseguradora.Text","edad","fecha_nacimiento",
						entry_direccion_px.Text,"cirugia","empresapac",id_tipopaciente,NomEmpleado+" "+AppEmpleado+" "+ApmEmpleado,LoginEmpleado,
				                  false,montoconvenio.ToString(),true,true);
				
				/*new osiris.abonos(PidPaciente,this.folioservicio,nombrebd,
				                 
						entry_ingreso.Text,entry_egreso.Text,entry_numero_factura.Text,
				                 
						entry_nombre_paciente.Text,entry_telefono_paciente.Text,entry_doctor.Text,
				                 
						entry_tipo_paciente.Text,entry_aseguradora.Text,edadpac+" Años y "+mesespac+" Meses",fecha_nacimiento,
				                 
						dir_pac,cirugia,empresapac,id_tipopaciente,NomEmpleado+" "+AppEmpleado+" "+ApmEmpleado,LoginEmpleado,
				                  agregarmasabonos,montoconvenio.ToString(),cuenta_cerrada);*/
				
				
			}
		}
		
		protected void on_button_bloqueo_exp_clicked (object sender, System.EventArgs e)
		{
			if (entry_folio_servicio.Text != "" && PidPaciente != 0){
				if((string) classpublic.lee_registro_de_tabla("osiris_his_paciente","paciente_bloqueado_cc","WHERE pid_paciente = '"+PidPaciente.ToString()+"' ","paciente_bloqueado_cc","bool") == "False"){
					MessageDialog msgBox = new MessageDialog (MyWinError,DialogFlags.Modal,MessageType.Question,ButtonsType.YesNo,"Desea BLOQUEAR este expediente, NO podra crear citas ni numeros de atencion ?");
					ResponseType miResultado = (ResponseType)msgBox.Run ();
					msgBox.Destroy(); 
					if (miResultado == ResponseType.Yes){				
						bloquea_expediente(PidPaciente,true);
						entry_estatus.Text = "EXPEDIENTE BLOQUEADO";
						label30.Text = "DesBloqueo Exp";
					}
				}else{
					MessageDialog msgBox = new MessageDialog (MyWinError,DialogFlags.Modal,MessageType.Question,ButtonsType.YesNo,"Desea DES-BLOQUEAR este expediente, podra crear citas y numeros de atencion ?");
					ResponseType miResultado = (ResponseType)msgBox.Run ();
					msgBox.Destroy(); 
					if (miResultado == ResponseType.Yes){				
						bloquea_expediente(PidPaciente,false);
						entry_estatus.Text = "";
						label30.Text = "Bloquear Exp";
					}
				}
			}else{
				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
											MessageType.Error, 	ButtonsType.Close, "Debe de llenar el campo de Folio de Atencion, verifique...");
				msgBoxError.Run ();				msgBoxError.Destroy();
			}
		}
		
		void bloquea_expediente(int PidPaciente_,bool estatus_expediente_)
		{
			NpgsqlConnection conexion; 
			conexion = new NpgsqlConnection (connectionString+nombrebd);            
			// Verifica que la base de datos este conectada
			try{
				conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand ();
               	comando.CommandText = "UPDATE osiris_his_paciente SET paciente_bloqueado_cc = '"+estatus_expediente_.ToString()+"'," +
					"historial_bloqueos_cc = historial_bloqueos_cc || '"+estatus_expediente_.ToString()+";"+LoginEmpleado+";"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"\n' "+
               		"WHERE pid_paciente = '"+PidPaciente_.ToString().Trim()+"';";
				//Console.WriteLine(comando.CommandText);
				comando.ExecuteNonQuery();
				comando.Dispose();
			}catch (NpgsqlException ex){
				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.Modal,
						MessageType.Error, 
						ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
				msgBoxError.Run ();				msgBoxError.Destroy();
	   			
	       	}
       		conexion.Close ();
		}

		protected void on_button_selecpagare_clicked (object sender, System.EventArgs e)
		{
			llenado_folio_seleccionado("AND osiris_erp_comprobante_pagare.numero_comprobante_pagare = '"+entry_nro_pagare.Text.Trim()+"';");
		}
		
		void crea_treeview_servicios()
		{
			// Creacion de Liststore
			treeViewEngineServicios = new TreeStore(typeof (string),typeof (string),
							typeof (string), typeof (string), 
							typeof (string),typeof (string),typeof (string),
							typeof (string), typeof (string),typeof (string),
							typeof (string),typeof (bool),typeof (string));
	        							   
			treeview_servicios.Model = treeViewEngineServicios;
			//treeViewEngine.SetSortColumnId (0, Gtk.SortType.Ascending);
			                            
			TreeViewColumn col_fecha = new TreeViewColumn();
			CellRendererText cellrt1 = new CellRendererText();  // aplica a todas la columnas
			col_fecha.Title = "Fecha-Hora"; // titulo de la cabecera de la columna, si está visible
			col_fecha.PackStart(cellrt1, true);
			col_fecha.AddAttribute (cellrt1, "text", 0);
			col_fecha.SortColumnId = (int) Column_serv.col_fecha;
             
			TreeViewColumn col_servicio = new TreeViewColumn();
			col_servicio.Title = "Servicio";
			col_servicio.PackStart(cellrt1, true);
			col_servicio.AddAttribute (cellrt1, "text", 1);
			col_servicio.SortColumnId = (int) Column_serv.col_servicio ;
      
			TreeViewColumn col_desc_servicio = new TreeViewColumn();
			col_desc_servicio.Title = "Motivo de Ingreso";
			col_desc_servicio.PackStart(cellrt1, true);
			col_desc_servicio.AddAttribute (cellrt1, "text", 2);
			col_desc_servicio.SortColumnId = (int) Column_serv.col_desc_servicio;
            
			TreeViewColumn col_valor = new TreeViewColumn();
			col_valor.Title = "Tipo de Cirugia";
			col_valor.PackStart(cellrt1, true);
			col_valor.AddAttribute (cellrt1, "text", 3);
			col_valor.SortColumnId = (int) Column_serv.col_valor;
            
			TreeViewColumn col_folio_ingreso = new TreeViewColumn();
			col_folio_ingreso.Title = "Folio Ingreso";
			col_folio_ingreso.PackStart(cellrt1, true);
			col_folio_ingreso.AddAttribute (cellrt1, "text", 4);
			col_folio_ingreso.SortColumnId = (int) Column_serv.col_folio_ingreso;
			
			TreeViewColumn col_num_factura = new TreeViewColumn();
			col_num_factura.Title = "N. Factura";
			col_num_factura.PackStart(cellrt1, true);
			col_num_factura.AddAttribute (cellrt1, "text", 5);
			col_num_factura.SortColumnId = (int) Column_serv.col_num_factura;			
            
			TreeViewColumn col_folio_ingreso_dep = new TreeViewColumn();
			col_folio_ingreso_dep.Title = "Folio Departamento";
			col_folio_ingreso_dep.PackStart(cellrt1, true);
			col_folio_ingreso_dep.AddAttribute (cellrt1, "text", 6);
			col_folio_ingreso_dep.SortColumnId = (int) Column_serv.col_folio_ingreso_dep;
			
			TreeViewColumn col_tipo_paciente = new TreeViewColumn();
			col_tipo_paciente.Title = "Tipo de Paciente";
			col_tipo_paciente.PackStart(cellrt1, true);
			col_tipo_paciente.AddAttribute (cellrt1, "text", 7);
			col_tipo_paciente.SortColumnId = (int) Column_serv.col_tipo_paciente;
			
			TreeViewColumn col_empresaasegu = new TreeViewColumn();
			col_empresaasegu.Title = "Empresa/Aseguradora";
			col_empresaasegu.PackStart(cellrt1, true);
			col_empresaasegu.AddAttribute (cellrt1, "text", 8);
			col_empresaasegu.SortColumnId = (int) Column_serv.col_empresaasegu;
						 
			TreeViewColumn col_admitio = new TreeViewColumn();
			col_admitio.Title = "Admitio";
			col_admitio.PackStart(cellrt1, true);
			col_admitio.AddAttribute (cellrt1, "text", 9);
			col_admitio.SortColumnId = (int) Column_serv.col_admitio;
			
			TreeViewColumn col_observaciones = new TreeViewColumn();
			col_observaciones.Title = "Observaciones";
			col_observaciones.PackStart(cellrt1, true);
			col_observaciones.AddAttribute (cellrt1, "text", 10);
			col_observaciones.SortColumnId = (int) Column_serv.col_observaciones;
			
			TreeViewColumn col_separacion = new TreeViewColumn();
			CellRendererToggle cellrtogg = new  CellRendererToggle();
			col_separacion.Title = "Separacion PQ.";
			col_separacion.PackStart(cellrtogg, true);
			col_separacion.AddAttribute (cellrtogg, "active", 11);
			col_separacion.SortColumnId = (int) Column_serv.col_separacion;
			
			TreeViewColumn col_servmedico = new TreeViewColumn();
			col_servmedico.Title = "Serv. Med.";
			col_servmedico.PackStart(cellrt1, true);
			col_servmedico.AddAttribute (cellrt1, "text", 12);
			col_servmedico.SortColumnId = (int) Column_serv.col_servmedico;
                        
			treeview_servicios.AppendColumn(col_fecha);
			treeview_servicios.AppendColumn(col_servicio);
			treeview_servicios.AppendColumn(col_desc_servicio);
			treeview_servicios.AppendColumn(col_observaciones);
			treeview_servicios.AppendColumn(col_servmedico);
			treeview_servicios.AppendColumn(col_valor);
			treeview_servicios.AppendColumn(col_folio_ingreso);
			treeview_servicios.AppendColumn(col_num_factura);
			treeview_servicios.AppendColumn(col_folio_ingreso_dep);
			treeview_servicios.AppendColumn(col_tipo_paciente);
			treeview_servicios.AppendColumn(col_empresaasegu);
			treeview_servicios.AppendColumn(col_admitio);
			treeview_servicios.AppendColumn(col_separacion);
		}
		
		enum Column_serv
		{
			col_fecha,
			col_servicio,
			col_desc_servicio,
			col_observaciones,
			col_servmedico,
			col_valor,
			col_folio_ingreso,
			col_num_factura,
			col_folio_ingreso_dep,
			col_tipo_paciente,
			col_empresaasegu,
			col_admitio,
			col_separacion
		}
		
		void llena_servicios_realizados(string pidpaciente_)
		{
			treeViewEngineServicios.Clear();
			NpgsqlConnection conexion; 
			conexion = new NpgsqlConnection (connectionString+nombrebd);
			//this.PidPaciente
			try{
				conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand ();
				comando.CommandText = "SELECT to_char(osiris_erp_cobros_enca.fechahora_creacion,'yyyy-MM-dd HH24:MI:SS') AS fechahoraadm, "+
									"osiris_his_tipo_admisiones.descripcion_admisiones,osiris_erp_cobros_enca.cancelado, "+
									"osiris_erp_movcargos.descripcion_diagnostico_movcargos, osiris_erp_movcargos.id_tipo_cirugia, "+
									//"osiris_his_tipo_cirugias.descripcion_cirugia, "+
									"to_char(osiris_erp_cobros_enca.folio_de_servicio,'9999999') AS folioserv, "+
									"to_char(osiris_erp_movcargos.folio_de_servicio_dep,'9999999') AS folioservdep, "+
									"osiris_his_tipo_pacientes.descripcion_tipo_paciente,osiris_erp_cobros_enca.id_empleado_admision,"+
									"osiris_erp_cobros_enca.id_aseguradora,descripcion_aseguradora,"+
									"osiris_erp_cobros_enca.id_empresa,descripcion_empresa,"+
									"osiris_erp_cobros_enca.reservacion,observacion_ingreso,otro_servicio_medico,"+
									"osiris_erp_cobros_enca.fecha_reservacion,"+
									"osiris_his_tipo_cirugias.descripcion_cirugia,"+
									"osiris_erp_cobros_enca.numero_factura AS numerofactura "+
									"FROM "+
									"osiris_erp_cobros_enca,osiris_erp_movcargos,osiris_his_tipo_pacientes,osiris_his_tipo_cirugias,osiris_his_tipo_admisiones,osiris_aseguradoras,osiris_empresas "+
									"WHERE "+
									" osiris_erp_cobros_enca.folio_de_servicio = osiris_erp_movcargos.folio_de_servicio "+
									"AND osiris_erp_movcargos.id_tipo_cirugia = osiris_his_tipo_cirugias.id_tipo_cirugia "+
									"AND osiris_erp_movcargos.id_tipo_paciente = osiris_his_tipo_pacientes.id_tipo_paciente "+
									"AND osiris_erp_movcargos.id_tipo_admisiones = osiris_his_tipo_admisiones.id_tipo_admisiones "+
									"AND osiris_erp_cobros_enca.pid_paciente = '"+pidpaciente_+"' "+
									"AND osiris_erp_cobros_enca.id_aseguradora = osiris_aseguradoras.id_aseguradora "+
									"AND osiris_erp_cobros_enca.id_empresa = osiris_empresas.id_empresa "+  
									"ORDER BY osiris_erp_cobros_enca.folio_de_servicio DESC;";
				//Console.WriteLine("Query: "+comando.CommandText);
				NpgsqlDataReader lector = comando.ExecuteReader ();
				string diagnostico_cirugia = "";
				string aseguradora_empresa = "";				
				while (lector.Read()){
					diagnostico_cirugia = (string) lector["descripcion_diagnostico_movcargos"];									
					if((int) lector ["id_aseguradora"] > 1){
						aseguradora_empresa = (string) lector["descripcion_aseguradora"];
					}else{
						aseguradora_empresa = (string) lector["descripcion_empresa"];
					}					
					if (!(bool) lector["cancelado"]){ 
						treeViewEngineServicios.AppendValues ((string) lector["fechahoraadm"],
															(string) lector["descripcion_admisiones"],
															diagnostico_cirugia,
															(string) lector["descripcion_cirugia"],
															(string) lector["folioserv"],
															(string) lector["numerofactura"],
															(string) lector["folioservdep"],
															(string) lector["descripcion_tipo_paciente"],
															aseguradora_empresa,
															(string) lector["id_empleado_admision"],
															(string) lector["observacion_ingreso"],
						                             		(bool) lector["reservacion"],
						                             		(string) lector["otro_servicio_medico"]);
					}
				}				
			}catch (NpgsqlException ex){
	   				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
										MessageType.Error, ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
					msgBoxError.Run ();			msgBoxError.Destroy();
			}
			conexion.Close ();
		}
		
		void crea_treeview_gestcobrz()
		{
			Gtk.CellRendererText text;
			Gtk.CellRendererToggle toggle;
			foreach (TreeViewColumn tvc in treeview_gestion_cobranza.Columns)
							treeview_gestion_cobranza.RemoveColumn(tvc);
			treeViewEngineGestion = new TreeStore(typeof(bool),
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
			treeview_gestion_cobranza.Model = treeViewEngineGestion;
			treeview_gestion_cobranza.RulesHint = true;
			//treeview_gestion_cobranza.Selection.Mode = SelectionMode.Multiple;
			
			// column for holiday names
			toggle = new CellRendererToggle ();
			toggle.Xalign = 0.0f;
			columns.Add (toggle);
			toggle.Toggled += new ToggledHandler (ItemToggled);
			TreeViewColumn column0 = new TreeViewColumn ("Seleccion", toggle,
						     "active", (int) Column.seleccion);
			column0.Sizing = TreeViewColumnSizing.Fixed;
			column0.FixedWidth = 65;
			column0.Clickable = true;
			column0.Resizable = true;
			column0.SortColumnId = (int) Column.seleccion;
			treeview_gestion_cobranza.InsertColumn (column0, (int) Column.seleccion);
			
			text = new CellRendererText ();
			text.Xalign = 0.0f;
			columns.Add (text);
			TreeViewColumn column1 = new TreeViewColumn("Fecha",text,"text",(int) Column.fecha);
			column1.Resizable = true;
			column1.SortColumnId = (int) Column.fecha;
			treeview_gestion_cobranza.InsertColumn (column1, (int) Column.fecha);
			
			text = new CellRendererText ();
			text.Xalign = 0.0f;
			columns.Add (text);
			TreeViewColumn column2 = new TreeViewColumn("Hora",text,"text",(int) Column.hora);
			column2.Resizable = true;
			column2.SortColumnId = (int) Column.hora;
			treeview_gestion_cobranza.InsertColumn (column2, (int) Column.hora);
			
			text = new CellRendererText ();
			text.Xalign = 0.0f;
			columns.Add (text);
			TreeViewColumn column3 = new TreeViewColumn("Telefono",text,"text",(int) Column.telefono);
			column3.Resizable = true;
			column3.SortColumnId = (int) Column.telefono;
			treeview_gestion_cobranza.InsertColumn (column3, (int) Column.telefono);
			
			text = new CellRendererText ();
			text.Xalign = 0.0f;
			columns.Add (text);
			TreeViewColumn column4 = new TreeViewColumn("Nota",text,"text",(int) Column.nota);
			column4.Resizable = true;
			column4.SortColumnId = (int) Column.nota;
			treeview_gestion_cobranza.InsertColumn (column4, (int) Column.nota);
		}
		
		enum Column
		{
			seleccion,
			fecha,
			hora,
			telefono,
			nota		
		}
		
		private void ItemToggled (object sender, ToggledArgs args)
		{
			Gtk.TreeIter iter; 			
			TreePath path = new TreePath (args.Path);
			if (treeview_gestion_cobranza.Model.GetIter (out iter, path)){					
				bool old = (bool) treeview_gestion_cobranza.Model.GetValue(iter,0);
				treeview_gestion_cobranza.Model.SetValue(iter,0,!old);
			}						
		}
		
		void llena_movcobranza(string foliodeatencion_)
		{
			treeViewEngineGestion.Clear();
			NpgsqlConnection conexion; 
			conexion = new NpgsqlConnection (connectionString+nombrebd);
			//this.PidPaciente
			try{
				conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand ();
				comando.CommandText = "SELECT to_char(osiris_erp_gestcobrzmov.fechahora_creacion,'yyyy-MM-dd') AS fechacrea,folio_de_servicio," +
				 	"to_char(osiris_erp_gestcobrzmov.fechahora_creacion,'HH24:MI') AS horacrea,nota,telefono,osiris_erp_gestcobrzmov.id_secuencia AS idsecuencia " +
					"FROM osiris_erp_gestcobrzmov " +
					"WHERE folio_de_servicio = '"+foliodeatencion_+"' " +
					"AND eliminado = 'false' " +
					"ORDER BY osiris_erp_gestcobrzmov.fechahora_creacion DESC;";
				//Console.WriteLine("Query: "+comando.CommandText);
				NpgsqlDataReader lector = comando.ExecuteReader ();
				while (lector.Read()){
					treeViewEngineGestion.AppendValues(false,
					                                   lector["fechacrea"].ToString(),
					                                   lector["horacrea"].ToString(),
					                                    lector["telefono"].ToString(),
					                                   lector["nota"].ToString(),
					                                   lector["idsecuencia"].ToString());
				}			
			}catch (NpgsqlException ex){
	   				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
										MessageType.Error, ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
					msgBoxError.Run ();			msgBoxError.Destroy();
			}
			conexion.Close ();
		}
		
		protected void on_button_busca_paciente_clicked (object sender, System.EventArgs e)
		{
			object[] parametros_objetos = {entry_pid_paciente,entry_nombre_paciente };
			string[] parametros_sql = {"SELECT osiris_his_paciente.pid_paciente,nombre1_paciente,nombre2_paciente, apellido_paterno_paciente,id_quienlocreo_paciente,"+
									"apellido_materno_paciente, to_char(fecha_nacimiento_paciente,'yyyy-MM-dd') AS fech_nacimiento,sexo_paciente,"+
									"to_char(to_number(to_char(age('"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"',osiris_his_paciente.fecha_nacimiento_paciente),'yyyy') ,'9999'),'9999') AS edad,"+
									"to_char(to_number(to_char(age('"+DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss")+"',osiris_his_paciente.fecha_nacimiento_paciente),'MM'),'99'),'99') AS mesesedad,"+
									"to_char(fechahora_registro_paciente,'dd-MM-yyyy HH:mi:ss') AS fech_creacion,activo " +
									"FROM osiris_his_paciente,osiris_erp_cobros_enca " +
									"WHERE activo = 'true' " +
									"AND osiris_his_paciente.pid_paciente = osiris_erp_cobros_enca.pid_paciente " +
									"AND osiris_erp_cobros_enca.pagare = 'true' " +
									"AND osiris_erp_cobros_enca.cancelado = 'false' "};
			string[] parametros_string = {};
			string[,] args_buscador1 = {{"APELLIDO PATERNO","AND apellido_paterno_paciente LIKE '%","%' "},
										{"APELLIDO MATERNO","AND apellido_materno_paciente LIKE '%","%' "},
										{"PRIMER NOMBRE","AND nombre1_paciente LIKE '%","%' "},
										{"NRO. EXPEDIENTE","AND osiris_his_paciente.pid_paciente = '","' "}};
			string[,] args_buscador2 = {{"APELLIDO MATERNO","AND apellido_materno_paciente LIKE '%","%' "},
										{"APELLIDO PATERNO","AND apellido_paterno_paciente LIKE '%","%' "},
										{"PRIMER NOMBRE","AND nombre1_paciente LIKE '%","%' "},
										{"NRO. EXPEDIENTE","AND osiris_his_paciente.pid_paciente = '","' "}};
			string[,] args_orderby = {{"",""}};
			classfind_data.buscandor(parametros_objetos,parametros_sql,parametros_string,"find_paciente_gestcobr",1,args_buscador1,args_buscador2,args_orderby);	
		}
		
		protected void on_button_impr_mov_clicked (object sender, System.EventArgs e)
		{
			//new osiris.class_traslate_xlsx();
			new osiris.rpt_mov_gestioncobrz (folioservicio,entry_nombre_paciente.Text.Trim());
			
			
		}

		protected void on_button_guardanota_clicked (object sender, System.EventArgs e)
		{
			almacena_nota(folioservicio,PidPaciente);
		}
		
		void almacena_nota(int folioservicio_,int PidPaciente_)
		{
			MessageDialog msgBox = new MessageDialog (MyWinError,DialogFlags.Modal,MessageType.Question,ButtonsType.YesNo,"¿ Desea grabar esta infomacion ?");
			ResponseType miResultado = (ResponseType)msgBox.Run ();
			msgBox.Destroy(); 
			if (miResultado == ResponseType.Yes){
				if(textview_notas.Buffer.Text.ToString().Trim() != ""){
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
							folioservicio_.ToString().Trim()+"','"+
							PidPaciente_.ToString().Trim()+"','"+
							DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"','"+
							textview_notas.Buffer.Text.ToString().ToUpper()+"','"+
							comboboxentry_telefono.ActiveText+"','"+
							id_tipopaciente+"')";
						
						//Console.WriteLine(comando.CommandText);
						comando.ExecuteNonQuery();
						comando.Dispose();					
						llena_movcobranza(folioservicio_.ToString().Trim());
						textview_notas.Buffer.Clear();					
					}catch (NpgsqlException ex){
						MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
																	MessageType.Error, 
																	ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
						msgBoxError.Run ();				conexion.Close();		
					}
					conexion.Close();
				}else{
					MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.Modal,
											MessageType.Error,ButtonsType.Close,"La NOTA se encuentra sin datos, verifique...");
					msgBoxError.Run ();						msgBoxError.Destroy();
				}
			}
		}
			
		protected void on_button_export_movgest_clicked (object sender, System.EventArgs e)
		{
			
			//if((string) classpublic.lee_registro_de_tabla("osiris_empleado","exportar_compserv","WHERE exportar_compserv = 'true' AND login_empleado = '"+LoginEmpleado+"' ","exportar_compserv","bool") == "True"){
				//new osiris.rptAdmision(nombrebd,"archivo","MOVIMIENTOS_GESTION_COBRANZA");  // rpt_rep1_admision.cs
				new osiris.reporte_de_abonos(nombrebd,"movimientos_gestion_cobranza",LoginEmpleado,0,0);
			//}else{
			//	MessageDialog msgBox = new MessageDialog (MyWinError,DialogFlags.Modal,
			//						MessageType.Info,ButtonsType.Ok,"No tiene Permiso para esta Opcion");
			//	msgBox.Run ();msgBox.Destroy();
			//}
		}

		protected void on_button_elimina_gestcobrz_clicked (object sender, System.EventArgs e)
		{
			TreeIter iter;
		 	if((string) classpublic.lee_registro_de_tabla("osiris_empleado","acceso_elimina_notagestcrz","WHERE acceso_elimina_notagestcrz = 'true' AND login_empleado = '"+LoginEmpleado+"' ","acceso_elimina_notagestcrz","bool") == "True"){
				MessageDialog msgBox = new MessageDialog (MyWinError,DialogFlags.Modal,MessageType.Question,ButtonsType.YesNo,"¿ Desea eliminar la(s) Nota(s) Seleccionadas. ?");
				ResponseType miResultado = (ResponseType)msgBox.Run ();
				msgBox.Destroy(); 
				if (miResultado == ResponseType.Yes){
						NpgsqlConnection conexion; 
					conexion = new NpgsqlConnection (connectionString+nombrebd);
					//this.PidPaciente
					try{
						conexion.Open ();
						NpgsqlCommand comando;
						comando = conexion.CreateCommand ();
						if (treeViewEngineGestion.GetIterFirst (out iter)){
							if((bool) treeview_gestion_cobranza.Model.GetValue(iter,0) == true){
								comando.CommandText =  "UPDATE osiris_erp_gestcobrzmov "+
													"SET eliminado = 'true'," +
													"fechahora_eliminado = '"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"',"+
													"id_quien_elimino = '"+LoginEmpleado+"' "+
													"WHERE id_secuencia = '"+treeview_gestion_cobranza.Model.GetValue(iter,5).ToString().Trim()+"';";
							
								//Console.WriteLine(comando.CommandText);
								comando.ExecuteNonQuery();
								comando.Dispose();
							}
						}
						
						while (treeViewEngineGestion.IterNext(ref iter)){
							if((bool) treeview_gestion_cobranza.Model.GetValue(iter,0) == true){
								comando.CommandText =  "UPDATE osiris_erp_gestcobrzmov "+
													"SET eliminado = 'true'," +
													"fechahora_eliminado = '"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"',"+
													"id_quien_elimino = '"+LoginEmpleado+"' "+
													"WHERE id_secuencia = '"+treeview_gestion_cobranza.Model.GetValue(iter,5).ToString().Trim()+"';";
							
								//Console.WriteLine(comando.CommandText);
								comando.ExecuteNonQuery();
								comando.Dispose();
							}
						}
						llena_movcobranza(folioservicio.ToString().Trim());
						textview_notas.Buffer.Clear();					
					}catch (NpgsqlException ex){
						MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
																	MessageType.Error, 
																	ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
						msgBoxError.Run ();				conexion.Close();		
					}
					conexion.Close();
				}
			}else{
				MessageDialog msgBox = new MessageDialog (MyWinError,DialogFlags.Modal,
									MessageType.Info,ButtonsType.Ok,"No tiene Permiso para esta Opcion");
				msgBox.Run ();msgBox.Destroy();
			}
		}
	}
}