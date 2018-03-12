// created on 17/05/2010 at 09:06 am
///////////////////////////////////////////////////////////
// project created on 24/10/2006 at 10:20 a
// Sistema Hospitalario OSIRIS
// Monterrey - Mexico
//
// Autor    	: Ing. Daniel Olivares C. cambio a GTKPrint con Pango y Cairo arcangeldoc@openmailbox.org
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
using Cairo;
using Pango;

namespace osiris
{
	public class rpt_ocupacion_hospitalaria
	{
		decimal sumacuenta = 0;
		decimal totabono = 0;
		
		private TreeStore treeViewEngineocupacion;
		
		class_public classpublic = new class_public();
		
		public rpt_ocupacion_hospitalaria (object treeViewEngineocupacion_,decimal sumacuenta_,decimal totabono_)
		{
			treeViewEngineocupacion = (object) treeViewEngineocupacion_ as Gtk.TreeStore;
			sumacuenta = sumacuenta_;
			totabono = totabono_;

			TreeIter iter;
			string tomovalor1 = "";
			int contadorprocedimientos = 0;
			if (this.treeViewEngineocupacion.GetIterFirst (out iter)){
				

				while (this.treeViewEngineocupacion.IterNext(ref iter)){
					

				}
			}
		}
	}
}
