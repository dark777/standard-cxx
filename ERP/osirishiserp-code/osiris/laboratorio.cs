// created on 08/06/2007 at 09:32 a
////////////////////////////////////////////////////////////
// project created on 24/10/2006 at 10:20 a
// Sistema Hospitalario OSIRIS
// Monterrey - Mexico
//
// Autor    	: Daniel Olivares - arcangeldoc@openmailbox.org (Programacion Mono)
//				  Daniel Olivares - arcangeldoc@openmailbox.org (Diseño de Pantallas Glade)
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
using System.Data;
using Gtk;
using Glade;
using System.Collections;


namespace osiris
{
	public class laboratorio
	{
		// Boton general para salir de las ventanas
		// Todas la ventanas en glade este boton debe estra declarado identico
		[Widget] Gtk.Button button_salir;
		
		// Declarando ventana principal de Hospitalizacion
		[Widget] Gtk.Window menu_laboratorio = null;
		[Widget] Gtk.Button button_cargos_pacientes = null;
		[Widget] Gtk.Button button_soli_material = null;
		[Widget] Gtk.Button button_solicitud_examenes = null;
		[Widget] Gtk.Button button_requisicion_materiales = null;
		[Widget] Gtk.Button button_inv_subalmacen = null;
		[Widget] Gtk.Button button_valores_referencia = null;
		
		string LoginEmpleado;
		string NomEmpleado;
		string AppEmpleado;
		string ApmEmpleado;
		string nombrebd;
		
		class_conexion conexion_a_DB = new class_conexion();
		class_public classpublic = new class_public();
		
		public laboratorio (string LoginEmp_, string NomEmpleado_, string AppEmpleado_, string ApmEmpleado_, string nombrebd_) 
		{
			LoginEmpleado = LoginEmp_;
			NomEmpleado = NomEmpleado_;
			AppEmpleado = AppEmpleado_;
			ApmEmpleado = ApmEmpleado_;
			nombrebd = conexion_a_DB._nombrebd;
			
			Glade.XML gxml = new Glade.XML (null, "laboratorio.glade", "menu_laboratorio", null);
			gxml.Autoconnect (this);
			////// Muestra ventana de Glade
			menu_laboratorio.Show();
			
			////// Sale de la ventana
			button_salir.Clicked += new EventHandler(on_cierraventanas_clicked);
			button_cargos_pacientes.Clicked += new EventHandler(on_button_cargos_pacientes_clicked);
			button_soli_material.Clicked += new EventHandler(on_button_soli_material_clicked);
			button_solicitud_examenes.Clicked += new EventHandler(on_button_solicitud_examenes_clicked);
			button_requisicion_materiales.Clicked += new EventHandler(on_button_requisicion_materiales_clicked);
			button_inv_subalmacen.Clicked += new EventHandler(on_button_inv_subalmacen_clicked);
			button_valores_referencia.Clicked += new EventHandler(on_button_valores_referencia_clicked);
		}
		
		void on_button_cargos_pacientes_clicked(object sender, EventArgs args)
		{
			new osiris.cargos_laboratorio(LoginEmpleado,NomEmpleado,AppEmpleado,ApmEmpleado,nombrebd);
		}
		
		void on_button_soli_material_clicked(object sender, EventArgs args)
		{
			new osiris.solicitud_material(LoginEmpleado,NomEmpleado,AppEmpleado,ApmEmpleado,nombrebd,26,0,0,"");
		}
		
		void on_button_inv_subalmacen_clicked(object sender, EventArgs args)
		{
			new osiris.inventario_sub_almacen(LoginEmpleado,NomEmpleado,AppEmpleado,ApmEmpleado,nombrebd,26,"LABORATORIO",1,false);
		}
		
		void on_button_requisicion_materiales_clicked(object sender, EventArgs args)
		{
			// centro de costo se debe enviar en el array y la clase 400   --   400
			int [] array_idtipoadmisiones = { 0, 400};
			string acceso_a_grupos = "AND osiris_grupo_producto.id_grupo_producto IN ("+(string) classpublic.lee_registro_de_tabla("osiris_his_tipo_admisiones","id_grupoprod_requisicion"," WHERE osiris_his_tipo_admisiones.id_tipo_admisiones = '400' ","id_grupoprod_requisicion","string")+")";
			new osiris.requisicion_materiales_compras(LoginEmpleado,NomEmpleado,AppEmpleado,ApmEmpleado,nombrebd,"LABORATORIO",400,acceso_a_grupos,array_idtipoadmisiones,0);
		}

		void on_button_solicitud_examenes_clicked(object sender, EventArgs args)
		{
			new osiris.solicitudes_rx_lab(LoginEmpleado,NomEmpleado,AppEmpleado,ApmEmpleado,nombrebd,"LABORATORIO",400);
		}

		void on_button_valores_referencia_clicked(object sender, EventArgs args)
		{
			new osiris.laboratorio_parametros (LoginEmpleado,NomEmpleado,AppEmpleado,ApmEmpleado,nombrebd,"");	
		}
				
		void on_cierraventanas_clicked (object sender, EventArgs args)
		{
			Widget win = (Widget) sender;
			win.Toplevel.Destroy();
		}
	}
}