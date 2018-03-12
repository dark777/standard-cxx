// created on 23/11/2012
////////////////////////////////////////////////////////////
// Sistema Hospitalario Osiris
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

namespace osiris
{
	public class surtir_dietas
	{
		// Boton general para salir de las ventanas
		// Todas la ventanas en glade este boton debe estra declarado identico
		[Widget] Gtk.Button button_salir;
		
		// Declarando ventana principal
		[Widget] Gtk.Window surtir_dietas_pacientes = null;
		
		string LoginEmpleado;
		string NomEmpleado;
		string AppEmpleado;
		string ApmEmpleado;
		string nombrebd;
		
		class_conexion conexion_a_DB = new class_conexion();
		
		public surtir_dietas ()
		{
			Glade.XML gxml = new Glade.XML (null, "nutricion.glade", "surtir_dietas_pacientes", null);
			gxml.Autoconnect (this);
			////// Muestra ventana de Glade
			surtir_dietas_pacientes.Show();
			
			////// Sale de la ventana
			button_salir.Clicked += new EventHandler(on_cierraventanas_clicked);	
		}
		
		void on_cierraventanas_clicked (object sender, EventArgs args)
		{
			Widget win = (Widget) sender;
			win.Toplevel.Destroy();
		}
	}
}

