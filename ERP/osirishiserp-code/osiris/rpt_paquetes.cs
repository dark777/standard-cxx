///////////////////////////////////////////////////////////
// created on 25/05/2007 at 08:36 a
// Sistema Hospitalario OSIRIS
// Monterrey - Mexico
//
// Autor    	: Ing. Juan Antonio Peña Gonzalez (Programacion)
// 				  Ing. Daniel Olivares C. (trapaso a GTKPrint)
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
	public class paquetes_reporte
	{
		private static int pangoScale = 1024;
		private PrintOperation print;
		private double fontSize = 8.0;
		int escala_en_linux_windows;		// Linux = 1  Windows = 8
		int comienzo_linea = 80;
		int comienzo_linea2 = 0;
		int separacion_linea = 11;
		int numpage = 1;
		
		int contador = 1;
		
		PrintContext context;
		
		string connectionString;
        string nombrebd;
		string cirugia = "";
		string medico = "";
		int idcirugia;
		string tiporeporte = "";
		string deposito_minimo = "";
		string dias_internamiento = "";
		string tel_medico = "";
		string tel_opcional = "";
		string fax = "";
		string notas = "";
		string numpresupuesto = "";
		string titulo_rpt = "";
		string schars = "";
		bool rptconprecio = true;
		
		//variables para rangos de fecha
				
		int idadmision_ = 0;
		int idproducto = 0;
		string datos = "";
		string fcreacion = "";
		decimal cantaplicada= 0;
		decimal ivaprod = 0;
		decimal subtotal = 0;
		decimal subt15 = 0;
		decimal subt0 = 0;
		decimal sumaiva = 0;
		decimal total = 0;
		decimal totaladm = 0;
		decimal subtotaldelmov = 0;
		decimal deducible = 0;
		decimal coaseguro = 0;
		decimal valoriva;
		
		Pango.FontDescription desc;
		
		class_conexion conexion_a_DB = new class_conexion();
		class_public classpublic = new class_public();
						
		//Declaracion de ventana de error
		protected Gtk.Window MyWinError;
		
		public paquetes_reporte ( int _id_ ,string nombcirugia,string _medico_,string _nombrebd_,string tiporeporte_,
								string deposito_minimo_,string dias_internamiento_,string tel_medico_,
								string tel_opcional_,string fax_,string numpresupuesto_,string notas_,bool rptconprecio_,string presupuesto_seleccionados_)
		{
			connectionString = conexion_a_DB._url_servidor+conexion_a_DB._port_DB+conexion_a_DB._usuario_DB+conexion_a_DB._passwrd_user_DB;
			nombrebd = conexion_a_DB._nombrebd;
			escala_en_linux_windows = classpublic.escala_linux_windows;
			cirugia = nombcirugia; 
			idcirugia = _id_;
			medico = _medico_;
			tiporeporte = tiporeporte_;
			deposito_minimo = deposito_minimo_;
			dias_internamiento = dias_internamiento_;
			tel_medico = tel_medico_;
			tel_opcional = tel_opcional_;
			fax = fax_;
			notas = notas_;
			numpresupuesto = numpresupuesto_;
			rptconprecio = rptconprecio_;
			valoriva = decimal.Parse(classpublic.ivaparaaplicar);
			
			if(tiporeporte == "presupuestos") { 
				titulo_rpt = "PRESUPUESTO_DE_CIRUGIA";
			}
			if(tiporeporte == "paquetes"){
				titulo_rpt = "PAQUETE_QUIRURGICO";
			}			
			print = new PrintOperation ();
			print.JobName = titulo_rpt;
			print.BeginPrint += new BeginPrintHandler (OnBeginPrint);
			print.DrawPage += new DrawPageHandler (OnDrawPage);
			print.EndPrint += new EndPrintHandler (OnEndPrint);
			print.Run (PrintOperationAction.PrintDialog, null);			
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
			decimal precioventaconvenido = 0;
			string query_consulta = "";
			Cairo.Context cr = context.CairoContext;
			Pango.Layout layout = context.CreatePangoLayout ();
			desc = Pango.FontDescription.FromString ("Sans");									
			// cr.Rotate(90)  Imprimir Orizontalmente rota la hoja cambian las posiciones de las lineas y columna					
			fontSize = 8.0;			layout = null;			layout = context.CreatePangoLayout ();
			desc.Size = (int)(fontSize * pangoScale);		layout.FontDescription = desc;
			if(tiporeporte == "paquetes"){
	           	query_consulta = "SELECT descripcion_producto,osiris_his_tipo_admisiones.descripcion_admisiones, "+
								"id_empleado,osiris_his_cirugias_deta.eliminado,osiris_productos.aplicar_iva,osiris_his_cirugias_deta.id_tipo_admisiones,  "+
								"osiris_productos.descripcion_producto,descripcion_grupo_producto,osiris_productos.id_grupo_producto, "+
								"to_char(osiris_his_tipo_cirugias.precio_de_venta,'999999999999') AS precioventa, "+
								"to_char(osiris_his_cirugias_deta.id_producto,'999999999999') AS idproducto, "+
								"to_char(osiris_his_cirugias_deta.cantidad_aplicada,'99999.99') AS cantidadaplicada, "+
								"to_char(osiris_productos.precio_producto_publico,'99999999.99') AS preciopublico,"+
								"to_char(osiris_productos.costo_por_unidad,'999999999.99') AS costoproductounitario, "+
								"to_char(osiris_productos.porcentage_ganancia,'99999.99') AS porcentageutilidad, "+
								"to_char(osiris_productos.costo_producto,'999999999.99') AS costoproducto, "+
								"to_char(osiris_his_cirugias_deta.fechahora_creacion,'dd-MM-yyyy HH:mi:ss') AS fechcreacion ,"+
								"to_char(osiris_his_cirugias_deta.id_secuencia,'9999999999') AS secuencia "+
								"FROM "+
								"osiris_his_cirugias_deta,osiris_productos,osiris_his_tipo_cirugias,osiris_his_tipo_admisiones,osiris_grupo_producto "+
								"WHERE "+
								"osiris_his_cirugias_deta.id_producto = osiris_productos.id_producto "+
								"AND osiris_productos.id_grupo_producto = osiris_grupo_producto.id_grupo_producto "+
								"AND osiris_his_cirugias_deta.id_tipo_cirugia = osiris_his_tipo_cirugias.id_tipo_cirugia "+
								"AND osiris_his_cirugias_deta.eliminado = false "+ 
								"AND osiris_his_cirugias_deta.id_tipo_admisiones = osiris_his_tipo_admisiones.id_tipo_admisiones "+
								"AND osiris_his_cirugias_deta.id_tipo_cirugia = '"+idcirugia.ToString() +"' "+
								"ORDER BY osiris_his_cirugias_deta.id_tipo_admisiones,osiris_productos.descripcion_producto,osiris_productos.id_grupo_producto;";
	        }
	        if(tiporeporte == "presupuestos"){
	        	query_consulta = "SELECT descripcion_producto,descripcion_admisiones, "+
							"id_empleado,osiris_his_presupuestos_deta.eliminado,osiris_productos.aplicar_iva,osiris_his_presupuestos_deta.id_tipo_admisiones,  "+
							"osiris_productos.descripcion_producto,descripcion_grupo_producto,osiris_productos.id_grupo_producto, "+
							"to_char(osiris_his_presupuestos_enca.precio_convenido,'999999999999') AS precioventa, "+
							"to_char(osiris_his_presupuestos_deta.id_producto,'999999999999') AS idproducto, "+
							"to_char(osiris_his_presupuestos_deta.cantidad_aplicada,'99999.99') AS cantidadaplicada, "+
							"to_char(osiris_productos.precio_producto_publico,'99999999.99') AS preciopublico,"+
							"to_char(osiris_productos.costo_por_unidad,'999999999.99') AS costoproductounitario, "+
							"to_char(osiris_productos.porcentage_ganancia,'99999.99') AS porcentageutilidad, "+
							"to_char(osiris_productos.costo_producto,'999999999.99') AS costoproducto, "+
							"to_char(osiris_his_presupuestos_deta.fechahora_creacion,'dd-MM-yyyy HH:mi:ss') AS fechcreacion ,"+
							"to_char(osiris_his_presupuestos_deta.id_secuencia,'9999999999') AS secuencia "+
							"FROM "+
							"osiris_his_presupuestos_enca,osiris_his_presupuestos_deta,osiris_productos,osiris_his_tipo_admisiones,osiris_grupo_producto "+
							"WHERE "+
							"osiris_his_presupuestos_deta.id_producto = osiris_productos.id_producto "+
							"AND osiris_productos.id_grupo_producto = osiris_grupo_producto.id_grupo_producto "+
							"AND osiris_his_presupuestos_enca.id_presupuesto = osiris_his_presupuestos_deta.id_presupuesto "+
							"AND osiris_his_presupuestos_deta.eliminado = 'false' "+ 
							"AND osiris_his_presupuestos_deta.id_tipo_admisiones = osiris_his_tipo_admisiones.id_tipo_admisiones "+
							"AND osiris_his_presupuestos_deta.id_presupuesto IN ('"+idcirugia.ToString()+"') "+							
							"ORDER BY osiris_his_presupuestos_deta.id_tipo_admisiones,osiris_productos.id_grupo_producto,osiris_productos.descripcion_producto;";
        	}
			NpgsqlConnection conexion; 
	        conexion = new NpgsqlConnection (connectionString+nombrebd);
			try{
 				conexion.Open ();
        		NpgsqlCommand comando; 
        		comando = conexion.CreateCommand (); 
				comando.CommandText = query_consulta;
        		Console.WriteLine(comando.CommandText);
				NpgsqlDataReader lector = comando.ExecuteReader ();
        		if (lector.Read()){
					imprime_encabezado(cr,layout,(string) lector["descripcion_admisiones"],"");
					precioventaconvenido = decimal.Parse((string) lector["precioventa"]);
        		
        			datos = (string) lector["descripcion_producto"];
	        		cantaplicada = decimal.Parse((string) lector["cantidadaplicada"]);
					subtotal = decimal.Parse((string) lector["preciopublico"])*cantaplicada;
					
					if((bool) lector["aplicar_iva"]== true){
						ivaprod = (subtotal*valoriva)/100;
						subt15 += subtotal;
					}else{
						subt0 += subtotal;
						ivaprod = 0;
					}
					sumaiva += ivaprod;
					total = subtotal + ivaprod;				
	        		totaladm += total;
					subtotaldelmov += total;
					
					imprime_linea_producto(cr,layout,(string) lector["idproducto"],(string) lector["cantidadaplicada"],datos,(string) lector["preciopublico"],subtotal,ivaprod,total);
					while (lector.Read()){
						precioventaconvenido = decimal.Parse((string) lector["precioventa"]);
        		
	        			datos = (string) lector["descripcion_producto"];
		        		cantaplicada = decimal.Parse((string) lector["cantidadaplicada"]);
						subtotal = decimal.Parse((string) lector["preciopublico"])*cantaplicada;
						
						if((bool) lector["aplicar_iva"]== true){
							ivaprod = (subtotal*valoriva)/100;
							subt15 += subtotal;
						}else{
							subt0 += subtotal;
							ivaprod = 0;
						}
						sumaiva += ivaprod;
						total = subtotal + ivaprod;				
		        		totaladm += total;
						subtotaldelmov += total;
						imprime_linea_producto(cr,layout,(string) lector["idproducto"],(string) lector["cantidadaplicada"],datos,(string) lector["preciopublico"],subtotal,ivaprod,total);
						contador +=1;
						salto_de_pagina(cr,layout,(string) lector["descripcion_admisiones"],"");						
					}
					cr.MoveTo(05*escala_en_linux_windows, (comienzo_linea2)*escala_en_linux_windows);
					cr.LineTo(05,(((comienzo_linea-2)+separacion_linea))*escala_en_linux_windows);		// vertical 1
					cr.MoveTo(67*escala_en_linux_windows, (comienzo_linea2)*escala_en_linux_windows);
					cr.LineTo(67,(((comienzo_linea-2)+separacion_linea))*escala_en_linux_windows);		// vertical 2
					cr.MoveTo(107*escala_en_linux_windows, (comienzo_linea2)*escala_en_linux_windows);
					cr.LineTo(107,(((comienzo_linea-2)+separacion_linea))*escala_en_linux_windows);		// vertical 3
					cr.MoveTo(390*escala_en_linux_windows, (comienzo_linea2)*escala_en_linux_windows);
					cr.LineTo(390,(((comienzo_linea-2)+separacion_linea))*escala_en_linux_windows);		// vertical 4
					cr.MoveTo(475*escala_en_linux_windows, (comienzo_linea2)*escala_en_linux_windows);
					cr.LineTo(475,(((comienzo_linea-2)+separacion_linea))*escala_en_linux_windows);		// vertical 5
					cr.MoveTo(565*escala_en_linux_windows, (comienzo_linea2)*escala_en_linux_windows);
					cr.LineTo(565,(((comienzo_linea-2)+separacion_linea))*escala_en_linux_windows);		// vertical 6
					cr.FillExtents();  //. FillPreserve(); 
					cr.SetSourceRGB (0, 0, 0);
					cr.LineWidth = 0.1;
					cr.Stroke();
				}
			}catch (NpgsqlException ex){
				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
				MessageType.Warning, ButtonsType.Ok, "PostgresSQL error: {0}",ex.Message);
				msgBoxError.Run ();		msgBoxError.Destroy();
				Console.WriteLine ("PostgresSQL error: {0}",ex.Message); 
			}			
		}
		
		void imprime_encabezado(Cairo.Context cr,Pango.Layout layout,string descrp_admin,string fech)
		{
			fontSize = 8.0;			layout = null;			layout = context.CreatePangoLayout ();
			desc.Size = (int)(fontSize * pangoScale);		layout.FontDescription = desc;
			layout.FontDescription = desc;
			//cr.Rotate(90);  //Imprimir Orizontalmente rota la hoja cambian las posiciones de las lineas y columna					
			desc.Size = (int)(fontSize * pangoScale);					layout.FontDescription = desc;
			layout.FontDescription.Weight = Weight.Bold;		// Letra negrita
			cr.MoveTo(05*escala_en_linux_windows,05*escala_en_linux_windows);			layout.SetText(classpublic.nombre_empresa);			Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(05*escala_en_linux_windows,15*escala_en_linux_windows);			layout.SetText(classpublic.direccion_empresa);		Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(05*escala_en_linux_windows,25*escala_en_linux_windows);			layout.SetText(classpublic.telefonofax_empresa);	Pango.CairoHelper.ShowLayout (cr, layout);
			fontSize = 6.0;
			desc.Size = (int)(fontSize * pangoScale);					layout.FontDescription = desc;
			cr.MoveTo(650*escala_en_linux_windows,05*escala_en_linux_windows);			layout.SetText("Fech.Rpt:"+(string) DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));		Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(650*escala_en_linux_windows,15*escala_en_linux_windows);			layout.SetText("N° Page :"+numpage.ToString().Trim());		Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(05*escala_en_linux_windows,35*escala_en_linux_windows);			layout.SetText("Sistema Hospitalario OSIRIS");		Pango.CairoHelper.ShowLayout (cr, layout);
			// Cambiando el tamaño de la fuente			
			fontSize = 10.0;
			desc.Size = (int)(fontSize * pangoScale);					layout.FontDescription = desc;
			// Cambiando el tamaño de la fuente			
			fontSize = 11.0;
			desc.Size = (int)(fontSize * pangoScale);					layout.FontDescription = desc;
			layout.FontDescription.Weight = Weight.Bold;		// Letra negrita
			layout.Alignment = Pango.Alignment.Center;
			
			double width = context.Width;
			layout.Width = (int) width;
			layout.Alignment = Pango.Alignment.Center;
			//layout.Wrap = Pango.WrapMode.Word;
			//layout.SingleParagraphMode = true;
			layout.Justify =  false;
			cr.MoveTo(width/2,45*escala_en_linux_windows);	layout.SetText(titulo_rpt);	Pango.CairoHelper.ShowLayout (cr, layout);
			//cr.MoveTo(225*escala_en_linux_windows, 35*escala_en_linux_windows);			layout.SetText(titulo_rpt);				Pango.CairoHelper.ShowLayout (cr, layout);
			fontSize = 8.0;			layout = null;			layout = context.CreatePangoLayout ();
			desc.Size = (int)(fontSize * pangoScale);		layout.FontDescription = desc;
			layout.FontDescription.Weight = Weight.Bold;		// Letra negrita
			cr.MoveTo(09*escala_en_linux_windows,70*escala_en_linux_windows);			layout.SetText("Nombre Paquete Qx: "+idcirugia.ToString()+"  "+cirugia);			Pango.CairoHelper.ShowLayout (cr, layout);
			imprime_titulo(cr, layout,descrp_admin,"");
			cr.MoveTo(230*escala_en_linux_windows, 740*escala_en_linux_windows);		layout.SetText("Pagina "+numpage.ToString()+"/  Fecha Rpt:"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));	Pango.CairoHelper.ShowLayout (cr, layout);
		}
		
		void imprime_titulo(Cairo.Context cr,Pango.Layout layout,string descrp_admin,string fech)
		{
			fontSize = 7.0;			layout = null;			layout = context.CreatePangoLayout ();
			desc.Size = (int)(fontSize * pangoScale);		layout.FontDescription = desc;
			comienzo_linea += separacion_linea;
			layout.FontDescription.Weight = Weight.Bold;   // Letra Negrita
			cr.MoveTo(200*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);			layout.SetText(descrp_admin.ToString()+"  "+fech.ToString());	Pango.CairoHelper.ShowLayout (cr, layout);
			comienzo_linea += separacion_linea;
			comienzo_linea += separacion_linea;
			comienzo_linea2 = comienzo_linea-2;
			cr.MoveTo(565*escala_en_linux_windows,(comienzo_linea-2)*escala_en_linux_windows);
			cr.LineTo(05,(comienzo_linea-2)*escala_en_linux_windows);
			cr.FillExtents();  //. FillPreserve(); 
			cr.SetSourceRGB (0, 0, 0);
			cr.LineWidth = 0.1;
			cr.Stroke();
			
			cr.MoveTo(025*escala_en_linux_windows, comienzo_linea*escala_en_linux_windows);				layout.SetText("CODIGO");			Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(075*escala_en_linux_windows, comienzo_linea*escala_en_linux_windows);				layout.SetText("CANT.");			Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(115*escala_en_linux_windows, comienzo_linea*escala_en_linux_windows);				layout.SetText("DESCRIPCION PRODUCTO");	Pango.CairoHelper.ShowLayout (cr, layout);
			if((bool) rptconprecio == true){
				cr.MoveTo(385*escala_en_linux_windows, comienzo_linea*escala_en_linux_windows);				layout.SetText("PRECIO");			Pango.CairoHelper.ShowLayout (cr, layout);
			}else{
				cr.MoveTo(420*escala_en_linux_windows, comienzo_linea*escala_en_linux_windows);				layout.SetText("USADO");			Pango.CairoHelper.ShowLayout (cr, layout);
				cr.MoveTo(490*escala_en_linux_windows, comienzo_linea*escala_en_linux_windows);				layout.SetText("DEVUELTO");			Pango.CairoHelper.ShowLayout (cr, layout);
			}
			layout.FontDescription.Weight = Weight.Normal;
			fontSize = 7.0;			layout = null;			layout = context.CreatePangoLayout ();
			desc.Size = (int)(fontSize * pangoScale);		layout.FontDescription = desc;
			cr.MoveTo(565*escala_en_linux_windows,(((comienzo_linea-2)+separacion_linea))*escala_en_linux_windows);
			cr.LineTo(05,(((comienzo_linea-2)+separacion_linea))*escala_en_linux_windows);
			cr.FillExtents();  //. FillPreserve(); 
			cr.SetSourceRGB (0, 0, 0);
			cr.LineWidth = 0.1;
			cr.Stroke();
		}
		
		void imprime_linea_producto(Cairo.Context cr,Pango.Layout layout,string idproducto_,string cantidadaplicada_,string datos_,string preciounitario_,decimal subtotal_,decimal ivaprod_,decimal total_)
		{
			fontSize = 7.0;			layout = null;			layout = context.CreatePangoLayout ();
			desc.Size = (int)(fontSize * pangoScale);		layout.FontDescription = desc;
			comienzo_linea += separacion_linea;
			cr.MoveTo(006*escala_en_linux_windows, comienzo_linea*escala_en_linux_windows);			layout.SetText(idproducto_);				Pango.CairoHelper.ShowLayout (cr, layout);
			cr.MoveTo(075*escala_en_linux_windows, comienzo_linea*escala_en_linux_windows);			layout.SetText(cantidadaplicada_);		Pango.CairoHelper.ShowLayout (cr, layout);
			if(datos_.Length > 61)	{				
				cr.MoveTo(110*escala_en_linux_windows, comienzo_linea*escala_en_linux_windows);		layout.SetText((string) datos_.Substring(0,60));					Pango.CairoHelper.ShowLayout (cr, layout);
			}else{
				cr.MoveTo(110*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);		layout.SetText((string) datos_);							Pango.CairoHelper.ShowLayout (cr, layout);
			}
			if((bool) rptconprecio == true){
				cr.MoveTo(380*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);			layout.SetText(preciounitario_);Pango.CairoHelper.ShowLayout (cr, layout);
				cr.MoveTo(430*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);			layout.SetText(subtotal_.ToString("N").PadLeft(10));		Pango.CairoHelper.ShowLayout (cr, layout);
				cr.MoveTo(480*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);			layout.SetText(ivaprod_.ToString("N").PadLeft(10));		Pango.CairoHelper.ShowLayout (cr, layout);
				cr.MoveTo(530*escala_en_linux_windows,comienzo_linea*escala_en_linux_windows);			layout.SetText(total_.ToString("N").PadLeft(10));			Pango.CairoHelper.ShowLayout (cr, layout);
			}
			cr.MoveTo(565*escala_en_linux_windows,(((comienzo_linea-2)+separacion_linea))*escala_en_linux_windows);
			cr.LineTo(05,(((comienzo_linea-2)+separacion_linea))*escala_en_linux_windows);
			cr.FillExtents();  //. FillPreserve(); 
			cr.SetSourceRGB (0, 0, 0);
			cr.LineWidth = 0.1;
			cr.Stroke();
		
		}
		
		void salto_de_pagina(Cairo.Context cr,Pango.Layout layout,string descrp_admin,string fech)			
		{
			if (contador > 51 ){					
				cr.MoveTo(05*escala_en_linux_windows, (comienzo_linea2)*escala_en_linux_windows);
				cr.LineTo(05,(((comienzo_linea-2)+separacion_linea))*escala_en_linux_windows);		// vertical 1
				cr.MoveTo(67*escala_en_linux_windows, (comienzo_linea2)*escala_en_linux_windows);
				cr.LineTo(67,(((comienzo_linea-2)+separacion_linea))*escala_en_linux_windows);		// vertical 2
				cr.MoveTo(107*escala_en_linux_windows, (comienzo_linea2)*escala_en_linux_windows);
				cr.LineTo(107,(((comienzo_linea-2)+separacion_linea))*escala_en_linux_windows);		// vertical 3
				cr.MoveTo(390*escala_en_linux_windows, (comienzo_linea2)*escala_en_linux_windows);
				cr.LineTo(390,(((comienzo_linea-2)+separacion_linea))*escala_en_linux_windows);		// vertical 4
				cr.MoveTo(475*escala_en_linux_windows, (comienzo_linea2)*escala_en_linux_windows);
				cr.LineTo(475,(((comienzo_linea-2)+separacion_linea))*escala_en_linux_windows);		// vertical 5
				cr.MoveTo(565*escala_en_linux_windows, (comienzo_linea2)*escala_en_linux_windows);
				cr.LineTo(565,(((comienzo_linea-2)+separacion_linea))*escala_en_linux_windows);		// vertical 6
				cr.FillExtents();  //. FillPreserve(); 
				cr.SetSourceRGB (0, 0, 0);
				cr.LineWidth = 0.1;
				cr.Stroke();
								
				cr.ShowPage();
				Pango.FontDescription desc = Pango.FontDescription.FromString ("Sans");								
				fontSize = 8.0;		desc.Size = (int)(fontSize * pangoScale);					layout.FontDescription = desc;
				comienzo_linea = 80;
				numpage += 1;
				contador = 1;
				imprime_encabezado(cr,layout,descrp_admin,fech);
			}
		}
		
		private void OnEndPrint (object obj, Gtk.EndPrintArgs args)
		{
		}
	}    
}