//
//  catalogo_marcas_productos.cs
//
//  Author:
//       dolivares <arcangeldoc@gmail.com>
//
//  Copyright (c) 2016 dolivares
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
using Npgsql;
using System.Data;
using Gtk;
using Glade;

namespace osiris
{
	public class catalogo_marcas_productos
	{
		[Widget] Gtk.Window catalogo1 = null;
		[Widget] Gtk.Entry entry_id = null;
		[Widget] Gtk.Entry entry_descripcion = null;
		[Widget] Gtk.ToggleButton togglebutton_editar = null;
		[Widget] Gtk.Button button_guardar = null;
		[Widget] Gtk.Button button_buscar = null;
		[Widget] Gtk.CheckButton checkbutton_activar = null;
		[Widget] Gtk.CheckButton checkbutton_nuevo = null;
		[Widget] Gtk.Button button_salir = null;
		[Widget] Gtk.Statusbar statusbar_catalogo1 = null; 

		//Declaracion de ventana de error y mensaje
		protected Gtk.Window MyWinError;
		protected Gtk.Window MyWin;

		class_conexion conexion_a_DB = new class_conexion();
		class_public classpublic = new class_public();
		class_buscador classfind_data = new class_buscador();

		// variable para la conexion---> los valores estan en class_conexion.cs
		string connectionString = "";
		string nombrebd = "";		
		//Informacion del Usuario  ------> los valores se recogen al inicio del sistema main.cs
		string LoginUsuario = "";
		string NombrUsuario = "";
		string idUsuario = "0";

		public catalogo_marcas_productos (string[] parametros)
		{
			Glade.XML gxml = new Glade.XML (null, "reportes.glade", "catalogo1", null);
			gxml.Autoconnect (this);
			catalogo1.Title = "Catalogo de Marcas";
			NombrUsuario = parametros[0];
			LoginUsuario = parametros[1];			

			connectionString = conexion_a_DB._url_servidor+conexion_a_DB._port_DB+conexion_a_DB._usuario_DB+conexion_a_DB._passwrd_user_DB;
			nombrebd = conexion_a_DB._nombrebd;

			button_salir.Clicked += new EventHandler(on_cierraventanas_clicked);
			button_buscar.Clicked += new EventHandler(on_button_buscar);
			checkbutton_nuevo.Clicked += new EventHandler(on_checkbutton_nuevo_clicked);
			togglebutton_editar.Clicked += new EventHandler(on_togglebutton_editar_clicked);
			button_guardar.Clicked += new EventHandler(on_button_guardar_clicked);

			entry_id.Sensitive = false;
			entry_descripcion.Sensitive = false;
			togglebutton_editar.Sensitive = false;
			button_guardar.Sensitive = false;
			checkbutton_activar.Sensitive = false;
			statusbar_catalogo1.Pop(0);
			statusbar_catalogo1.Push(1, "Login: "+LoginUsuario+" |Usuario: "+NombrUsuario);
			statusbar_catalogo1.HasResizeGrip = false;
		}

		void on_button_buscar(object sender, EventArgs args)
		{
			// Los parametros de del SQL siempre es primero cuando busca todo y la otra por expresion
			// la clase recibe tambien el orden del query
			// es importante definir que tipo de busqueda es para que los objetos caigan ahi mismo
			object[] parametros_objetos = {entry_id,entry_descripcion,togglebutton_editar,button_guardar,checkbutton_activar};
			string[] parametros_sql = {"SELECT * FROM osiris_erp_marca_productos "};
			string[] parametros_string = {};
			string[,] parametros_busqueda1 = {{"MARCA","WHERE descripcion_marca LIKE '%","%'"},
											{"ID MARCA","WHERE id_marca_producto = '","'"}};
			string[,] parametros_busqueda2 = {{"ID MARCA","WHERE id_marca_producto = '","'"},
											{"MARCA","WHERE descripcion_marca LIKE '%","%'"}};
			string[,] parametros_ordenpor = {{"",""}};
			classfind_data.buscandor(parametros_objetos,parametros_sql,parametros_string,"find_catalogo_marca",0,parametros_busqueda1,parametros_busqueda2,parametros_ordenpor);
		}

		void on_togglebutton_editar_clicked(object sender, EventArgs args)
		{
			if(togglebutton_editar.Active == true){
				entry_descripcion.Sensitive = true;
				button_guardar.Sensitive = true;
				checkbutton_activar.Sensitive = true;				
			}else{
				entry_descripcion.Sensitive = false;
				button_guardar.Sensitive = false;
				checkbutton_activar.Sensitive = false;
			}
		}

		void on_button_guardar_clicked(object sender, EventArgs args)
		{
			if(entry_descripcion.Text.Trim()!=""){	
				MessageDialog msgBox = new MessageDialog (MyWin,DialogFlags.Modal,
					MessageType.Question,ButtonsType.YesNo,"¿ Esta seguro de Guardar la Informacion...?");
				ResponseType miResultado = (ResponseType)
					msgBox.Run ();				msgBox.Destroy();
				if (miResultado == ResponseType.Yes){					
					if(togglebutton_editar.Active == false){
						string[,] parametros = {{"descripcion_marca,","'"+entry_descripcion.Text.Trim().ToUpper()+"',"},
											{"activa","'"+checkbutton_activar.Active.ToString()+"' "}};
						object [] paraobj = {entry_id};
						new osiris.insert_registro("osiris_erp_marca_productos",parametros,paraobj);
					}else{
						string[,] parametros = {{"descripcion_marca = '",entry_descripcion.Text.Trim().ToUpper()+"',"},
												{"activa = '",checkbutton_activar.Active.ToString()+"' "},
												{"WHERE id_marca_producto = '",entry_id.Text.Trim()+"';"}};
						object [] paraobj = {entry_id,checkbutton_activar};
						new osiris.update_registro("osiris_erp_marca_productos",parametros,paraobj);
					}						
					checkbutton_nuevo.Active = false;					
					button_buscar.Sensitive = true;
					togglebutton_editar.Active = false;
				}					
			}else{
				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
					MessageType.Error,ButtonsType.Close,"No escribio nada, verifique...","Error");
				msgBoxError.Run ();			msgBoxError.Destroy();
			}
		}

		void on_checkbutton_nuevo_clicked(object sender, EventArgs args)
		{
			if(checkbutton_nuevo.Active == true){ 
				MessageDialog msgBox = new MessageDialog (MyWin,DialogFlags.Modal,
					MessageType.Question,ButtonsType.YesNo,"¿ Esta seguro de querer crear un registro nuevo?");
				ResponseType miResultado = (ResponseType)
					msgBox.Run ();				msgBox.Destroy();
				if (miResultado == ResponseType.Yes){
					entry_id.Text = classpublic.lee_ultimonumero_registrado("osiris_erp_marca_productos","id_marca_producto","");
					entry_descripcion.Sensitive = true;
					entry_descripcion.Text = "";
					button_buscar.Sensitive = false;
					button_guardar.Sensitive = true;
					togglebutton_editar.Active = false;
					togglebutton_editar.Sensitive = false;
					checkbutton_activar.Sensitive = true;
				}else{
					checkbutton_nuevo.Active = false;
				}			
			}
			if (checkbutton_nuevo.Active == false){				
				entry_descripcion.Sensitive = false;
				button_guardar.Sensitive = false;
				togglebutton_editar.Sensitive = false;
				togglebutton_editar.Active = false;
				checkbutton_activar.Sensitive = false;
				button_buscar.Sensitive = true;
				entry_id.Text = "";
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