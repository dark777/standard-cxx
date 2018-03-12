////////////////////////////////////////////////////////////
// project created on 11/11/2010 at 10:20 a
// Sistema Hospitalario OSIRIS
// Monterrey - Mexico
//
// Autor    	: Ing. Daniel Olivares (Programacion)
// 				  
// Licencia		: GLP
// S.O. 		: GNU/Linux
//////////////////////////////////////////////////////////
//
// proyect osiris is free software; you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation; either version 2 of the License, or
// (at your option) any later version.
// 
// proyect osirir is distributed in the hope that it will be useful,
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
using Gtk;
using Glade;
using System.Data;

namespace osiris
{
	public class optica
	{
		// Boton general para salir de las ventanas
		// Todas la ventanas en glade este boton debe estra declarado identico
		[Widget] Gtk.Button button_salir;
		
		// Declarando ventana principal de Hospitalizacion
		[Widget] Gtk.Window menu_hospitalizacion = null;
		[Widget] Gtk.Button button_cargos_pacientes = null;
		[Widget] Gtk.Button button_soli_material = null;
		[Widget] Gtk.Button button_autorizacion_medicamento = null;
		[Widget] Gtk.Button button_inv_subalmacen = null;
		[Widget] Gtk.Button button_asignacion_habitacion = null;
		[Widget] Gtk.Button button_traspaso_subalmacenes = null;
		[Widget] Gtk.Button button_devolucion_mat = null;
		[Widget] Gtk.Button button_reportes = null;
		[Widget] Gtk.Label label35 = null;
		
		// Ventana de Rango de Fecha
		[Widget] Gtk.Window rango_de_fecha;
		[Widget] Gtk.Entry entry_dia1;
		[Widget] Gtk.Entry entry_dia2;
		[Widget] Gtk.Entry entry_mes1;
		[Widget] Gtk.Entry entry_mes2;
		[Widget] Gtk.Entry entry_ano1;
		[Widget] Gtk.Entry entry_ano2;
		[Widget] Gtk.Entry entry_referencia_inicial;
		[Widget] Gtk.Entry entry_cliente;
		[Widget] Gtk.Label label_orden;
		[Widget] Gtk.Label label_nom_cliente;
		[Widget] Gtk.Label label142;
		[Widget] Gtk.RadioButton radiobutton_cliente;
		[Widget] Gtk.RadioButton radiobutton_fecha;
		[Widget] Gtk.Button button_busca_cliente;
		[Widget] Gtk.Button button_imprime_rangofecha;
		[Widget] Gtk.CheckButton checkbutton_impr_todo_proce;
		[Widget] Gtk.CheckButton checkbutton_todos_los_clientes;
		[Widget] Gtk.CheckButton checkbutton_export_to = null;

		
		string LoginEmpleado;
		string NomEmpleado;
		string AppEmpleado;
		string ApmEmpleado;
		string nombrebd;
		string query_rango_fechas;

		string connectionString;

		class_conexion conexion_a_DB = new class_conexion();

		protected Gtk.Window MyWinError;
		
		public optica (string LoginEmp_, string NomEmpleado_, string AppEmpleado_, string ApmEmpleado_, string nombrebd_)
		{
			LoginEmpleado = LoginEmp_;
			NomEmpleado = NomEmpleado_;
			AppEmpleado = AppEmpleado_;
			ApmEmpleado = ApmEmpleado_;
			connectionString = conexion_a_DB._url_servidor+conexion_a_DB._port_DB+conexion_a_DB._usuario_DB+conexion_a_DB._passwrd_user_DB;
			nombrebd = conexion_a_DB._nombrebd;			
			Glade.XML gxml = new Glade.XML (null, "hospitalizacion.glade", "menu_hospitalizacion", null);
			gxml.Autoconnect (this);
			////// Muestra ventana de Glade
			menu_hospitalizacion.Show();
			menu_hospitalizacion.Title = "Menu de Optica";
			label35.Text = "Menu de Optica";
			////// Sale de la ventana
			button_salir.Clicked += new EventHandler(on_cierraventanas_clicked);
			button_cargos_pacientes.Clicked += new EventHandler(on_button_cargos_pacientes_clicked);
			button_soli_material.Clicked += new EventHandler(on_button_soli_material_clicked);
			button_autorizacion_medicamento.Clicked += new EventHandler(on_button_autorizacion_medicamento_clicked);
			button_inv_subalmacen.Clicked += new EventHandler(on_button_inv_subalmacen_clicked);
			button_asignacion_habitacion.Clicked += new EventHandler(on_button_asignacion_habitacion_clicked);
			button_traspaso_subalmacenes.Clicked += new EventHandler(on_button_traspaso_subalmacenes_clicked);
			button_devolucion_mat.Clicked += new EventHandler(on_button_devolucion_mat_clicked);
			button_reportes.Clicked += new EventHandler(on_button_reportes_clicked);
		}

		void on_button_reportes_clicked(object sender, EventArgs args)
		{

		}
		
		void on_button_cargos_pacientes_clicked(object sender, EventArgs args)
		{
			//new osiris.cargos_hospitalizacion(LoginEmpleado,NomEmpleado,AppEmpleado,ApmEmpleado,nombrebd);
			new osiris.cargos_modulos_medicos(LoginEmpleado,NomEmpleado,AppEmpleado,ApmEmpleado,970,"OPTICA",20,"");
		}
		
		void on_button_soli_material_clicked(object sender, EventArgs args)
		{
			new osiris.solicitud_material(LoginEmpleado,NomEmpleado,AppEmpleado,ApmEmpleado,nombrebd,20,0,0,"");
		}
		
		void on_button_autorizacion_medicamento_clicked(object sender, EventArgs args)
		{
			 new osiris.orden_compra_urgencias(LoginEmpleado,NomEmpleado,AppEmpleado,ApmEmpleado,nombrebd,20,"OPTICA",0,"");
		}
		
		void on_button_inv_subalmacen_clicked(object sender, EventArgs args)
		{
			new osiris.inventario_sub_almacen(LoginEmpleado,NomEmpleado,AppEmpleado,ApmEmpleado,nombrebd,20,"OPTICA",1,false);
		}
		
		void on_button_traspaso_subalmacenes_clicked(object sender, EventArgs args)
		{
			//new osiris.inventario_sub_almacen(LoginEmpleado,NomEmpleado,AppEmpleado,ApmEmpleado,nombrebd,3,"HOSPITALIZACION",2);
			new osiris.trasp_devolu_materiales(LoginEmpleado,NomEmpleado,AppEmpleado,ApmEmpleado,nombrebd,20,"OPTICA",2,"TRASPASO",false,20);
		}

		void on_button_devolucion_mat_clicked(object sender, EventArgs args)
		{
			new osiris.trasp_devolu_materiales(LoginEmpleado,NomEmpleado,AppEmpleado,ApmEmpleado,nombrebd,20,"OPTICA",1,"DEVOLUCION",false,1);
		}
		
		void on_button_asignacion_habitacion_clicked(object sender, EventArgs args)
		{
		   new osiris.asignacion_de_habitacion(LoginEmpleado,NomEmpleado,AppEmpleado,ApmEmpleado,nombrebd, 0);
		}
		
		void onKeyPressEvent(object o, Gtk.KeyPressEventArgs args)
		{
			//Console.WriteLine(args.Event.Key);
			//Console.WriteLine(Convert.ToChar(args.Event.Key));
			string misDigitos = ".0123456789ﾰﾱﾲﾳﾴﾵﾶﾷﾸﾹﾮｔｒｓｑ（）";
			if (Array.IndexOf(misDigitos.ToCharArray(), Convert.ToChar(args.Event.Key)) == -1 && args.Event.Key.ToString()  != "BackSpace"){
				args.RetVal = true;
			}
		}
		
		void on_cierraventanas_clicked (object sender, EventArgs args)
		{
			Widget win = (Widget) sender;
			win.Toplevel.Destroy();
		}
	}
}

