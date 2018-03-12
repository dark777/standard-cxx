//////////////////////////////////////////////////////////
// project created on 04/01/2010 at 10:20 a   
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
using Gtk;
using Gdk;
using System;
using Glade;
using Npgsql;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Net.Mail;

namespace osiris
{
	public class class_public
	{		
		public string LoginUsuario = "";
		public string NombrUsuario = "";
		public string idUsuario = "";
		
		// Informacion de la Empresa
		public string nombre_empresa = "MEDICA NORESTE ION"; 
									/*"MEDICA NORESTE ION" 
									"P R A C T I M E D" 
									"CLINICA ANAHUAC" 
									"CENTRO MEDICO AVENIDA" 
									"UNIDAD MEDICA ESCOBEDO"
									"HUMAN HOSPITAL"*/
		public string nombre_empresa2 = "CONTROL DE CLINICA S.C.";  
									/*"CONTROL DE CLINICA S.C." 
									"PRACTIMED" 
									"CLINICA LOS ANGELES S.A. DE C.V." 
									"CENTRO MEDICO AVENIDA" 
									"UNIDAD MEDICA ESCOBEDO"
									"CENTRO MEDICO MONTERREY SALTILLO S.A. DE C.V."*/
		public string direccion_empresa = "Jose Angel Conchello 2880, Col. Victoria"; 
									/*"Jose Angel Conchello 2880, Col. Victoria" 
									"Loma Grande 2703, Col. Loma de San Francisco"; 
									"Av. Palacio de Justicia 318, Col. Anahuac" 
									"Raul Salinas #236, Col. Felipe Carrillo P."
									"Nicolas Bravo 100, Col. Centro de Escobedo"
									"Humberto Hinojosa #462 Col. La Salle, Saltillo"*/
		public string telefonofax_empresa = "Telefono:(01)(81) 8351-3610"; 
									/*"Telefono:(01)(81) 8351-3610" 
									"Telefono:(01)(81) 8040-6060"  
									"Telefono:(01)(81) 8376-2020"
									"Telefono:(01)(81) 8384-3200"
									"Telefono:(01)(81) 8058-4127"
									"Telefono:(01)(84) 4415-2631"*/
		public string version_sistema = "Sistema Hospitalario OSIRIS ver. 2.0";
		public string ipserver = "192.168.1.10";
		public string mailserver = "192.168.1.146";
		public bool enviar_email = true;	// cambiar dependiedo el cliente
		public bool valid_dato_responsable = true;
		public bool muestradeta_comprcaja = false;
		public string lector_de_pdf_linux = "evince";				// Linux
		public string lectos_de_pdf_win = "c:\\Program Files\\";	// Windows
				
		public string ivaparaaplicar = "16.00";
		
		public int escala_linux_windows = 1;	// Linux = 1  Windows = 8
		public int horario_cita_inicio = 1;		// 7 am
		public int horario_cita_termino = 20;	// 8 pm
		public int horario_24_horas = 24; 		// media moche
		public int intervalo_minutos = 10;		// intervalo de minutos para las consultas
		
		// variable para la conexion---> los valores estan en facturador.cs
		string connectionString = "";
		string nombrebd = "";
		
		class_conexion conexion_a_DB = new class_conexion();
		
		//Declaracion de ventana de error y mensaje
		protected Gtk.Window MyWinError;
		
		const int gray50_width = 2;
		const int gray50_height = 2;
		const string gray50_bits = "\x02\x01";
		
		/// <summary>
		/// Funcion de Encriptacion en MD5 para las contraseñas de usuarios 
		/// </summary>
		/// <param name="password">
		/// A <see cref="System.String"/>
		/// </param>
		/// <returns>
		/// A <see cref="System.String"/>
		/// </returns>
		public string CreatePasswordMD5(string password)
		{
			MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
			byte[] bs = System.Text.Encoding.UTF8.GetBytes(password);
			bs = md5.ComputeHash(bs);
			System.Text.StringBuilder s = new System.Text.StringBuilder();
			foreach (byte b in bs){
				s.Append(b.ToString("x2").ToLower());
			}
			return s.ToString();			
		}
		
		/// <summary>
		/// Funcion de Encriptacion en SHA1
		/// </summary>
		/// <returns>
		/// The password SHA1.
		/// </returns>
		/// <param name='password'>
		/// Password.
		/// </param>
		public string CreatePasswordSHA1(string password)
		{
			SHA1CryptoServiceProvider sha1 = new SHA1CryptoServiceProvider();
			byte[] bs = System.Text.Encoding.UTF8.GetBytes(password);
			bs = sha1.ComputeHash(bs);
			System.Text.StringBuilder s = new System.Text.StringBuilder();
			foreach (byte b in bs){
				s.Append(b.ToString("x2").ToLower());
			}
			return s.ToString();			
		}
		
		/// <summary>
		/// Creates the password SH a256.
		/// </summary>
		/// <returns>
		/// The password SH a256.
		/// </returns>
		/// <param name='password'>
		/// Password.
		/// </param>
		public string CreatePasswordSHA256(string password)
		{
			SHA256Managed sha256 = new SHA256Managed();
			byte[] bs = System.Text.Encoding.UTF8.GetBytes(password);
			bs = sha256.ComputeHash(bs);
			System.Text.StringBuilder s = new System.Text.StringBuilder();
			foreach (byte b in bs){
				s.Append(b.ToString("x2").ToLower());
			}
			return s.ToString();			
		}
		
		public string RemoveAccentsWithRegEx(string inputString)
		{
			Regex replace_a_Accents = new Regex("[á|à|ä|â|Á]", RegexOptions.Compiled);
			Regex replace_e_Accents = new Regex("[é|è|ë|ê|É]", RegexOptions.Compiled);
			Regex replace_i_Accents = new Regex("[í|ì|ï|î|Í]", RegexOptions.Compiled);
			Regex replace_o_Accents = new Regex("[ó|ò|ö|ô|Ó]", RegexOptions.Compiled);
			Regex replace_u_Accents = new Regex("[ú|ù|ü|û|Ú]", RegexOptions.Compiled);
			Regex replace_n_Accents = new Regex("[ñ|Ñ]", RegexOptions.Compiled);
			inputString = replace_a_Accents.Replace(inputString, "A");
			inputString = replace_e_Accents.Replace(inputString, "E");
			inputString = replace_i_Accents.Replace(inputString, "I");
			inputString = replace_o_Accents.Replace(inputString, "O");
			inputString = replace_u_Accents.Replace(inputString, "U");
			inputString = replace_n_Accents.Replace(inputString, "N");
			return inputString;
		}
		
		public string plataform_OS()
		{
			OperatingSystem os = Environment.OSVersion;
    		PlatformID	pid = os.Platform;
    		
			switch (pid){
				case PlatformID.Win32NT:
					break;
        		case PlatformID.Win32S:
					break;
        		case PlatformID.Win32Windows:
					break;
        		case PlatformID.WinCE:					
					break;
				case PlatformID.MacOSX:					
					break;
				case PlatformID.Unix:					
					break;
			}
			return(os.Platform.ToString());
		}
		
		/// Extrae espacios en blanco de un texto y solo deja uno
		/// </summary>
		/// <param name="character_extract">el string que se le estraeran los espacion</param>
		/// <returns>Regresa un string con un solo caracter en blanco</returns>
		public string extract_spaces(string character_extract)
		{
			return Regex.Replace(character_extract, @"\s+", " ");
		}
		
		/// <summary>
		/// Lee el ultimo numero de la tabla que se a creado
		/// </summary>
		/// <param name="name_table">Nombre de la tabla a buscar</param>
		/// <param name="name_field">Nombre del campo que va ser el ultimo numero</param>
		/// <param name="condition_table">Aplica una condicion a la busqueda del ultimo numero</param>
		/// <returns>Regresa el ultimo numero como cadena de caracteres</returns>
		public string lee_ultimonumero_registrado(string name_table,string name_field,string condition_table)
		{
			connectionString = conexion_a_DB._url_servidor+conexion_a_DB._port_DB+conexion_a_DB._usuario_DB+conexion_a_DB._passwrd_user_DB;
			nombrebd = conexion_a_DB._nombrebd;
			NpgsqlConnection conexion; 
			conexion = new NpgsqlConnection (connectionString+nombrebd);
            // Verifica que la base de datos este conectada
			string tomavalor = "1";
			try{
				conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand ();
				comando.CommandText = "SELECT to_char("+name_field+",'9999999999') AS field_last_number FROM "+name_table+" "+condition_table+" ORDER BY "+name_field+" DESC LIMIT 1;";
				//Console.WriteLine(comando.CommandText);
				NpgsqlDataReader lector = comando.ExecuteReader ();				
				if (lector.Read()){	
					tomavalor = (int.Parse((string) lector["field_last_number"])+1).ToString();
					conexion.Close();
					return tomavalor;					
				}else{
					conexion.Close();
					return tomavalor;					
				}
			}catch (NpgsqlException ex){
		   					MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
							MessageType.Error,ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
							msgBoxError.Run ();			msgBoxError.Destroy();
				conexion.Close();
				return tomavalor;
			}
		}
		
		/// <summary>
		/// Lee registro de tabla the specified name table, name field and condition table.
		/// </summary>
		/// <param name='name_table'>
		/// Nombre de la tabla de la busqueda.
		/// </param>
		/// <param name='name_field'>
		/// Nombre del registro
		/// </param>
		/// <param name='condition_table'>
		/// Condicion de la tabla
		/// </param>
		public string lee_registro_de_tabla(string name_table,string name_field,string condition_table,string name_field_out,string type_field)
		{
			connectionString = conexion_a_DB._url_servidor+conexion_a_DB._port_DB+conexion_a_DB._usuario_DB+conexion_a_DB._passwrd_user_DB;
			nombrebd = conexion_a_DB._nombrebd;
			NpgsqlConnection conexion; 
			conexion = new NpgsqlConnection (connectionString+nombrebd);
            // Verifica que la base de datos este conectada
			string tomavalor = "";
			try{
				conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand ();
				if(type_field == "string"){
					comando.CommandText = "SELECT "+name_field+" AS field_id_name,"+name_field_out+" AS name_fiel_output"+" FROM "+name_table+" "+condition_table+" ORDER BY "+name_field+" DESC LIMIT 1;";
				}
				if(type_field == "int"){
					comando.CommandText = "SELECT to_char("+name_field+",'9999999999') AS field_id_name,"+name_field_out+" AS name_fiel_output"+" FROM "+name_table+" "+condition_table+" ORDER BY "+name_field+" DESC LIMIT 1;";
				}
				if(type_field == "bool"){
					comando.CommandText = "SELECT "+name_field+" AS field_id_name,"+name_field_out+" AS name_fiel_output"+" FROM "+name_table+" "+condition_table+" ORDER BY "+name_field+" DESC LIMIT 1;";
				}
				//Console.WriteLine(comando.CommandText);
				NpgsqlDataReader lector = comando.ExecuteReader ();				
				if (lector.Read()){
					tomavalor = (string) lector["name_fiel_output"].ToString().Trim();					
				}
				conexion.Close();
				return tomavalor;
			}catch (NpgsqlException ex){
		   					MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
							MessageType.Error,ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
							msgBoxError.Run ();			msgBoxError.Destroy();
				conexion.Close();
				return tomavalor;
			}
		}
		
		/// <summary>
		/// Devuelve el nombre del Mes en castellano
		/// </summary>
		/// <returns>
		/// A <see cref="System.String"/>
		/// </returns>
		public string nom_mes(string number_month)
		{
			string[] args_nombremeses = {"Enero","Febrero","Marzo","Abril","Mayo","Junio","Julio","Agosto","Septiembre","Octubre","Noviembre","Diciembre"};
			return(args_nombremeses[int.Parse(number_month)-1]);
		}
		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="buffer">
		/// A <see cref="TextBuffer"/>
		/// </param>
		public void CreateTags (TextBuffer buffer)
		{
			// Create a bunch of tags. Note that it's also possible to
			// create tags with gtk_text_tag_new() then add them to the
			// tag table for the buffer, gtk_text_buffer_create_tag() is
			// just a convenience function. Also note that you don't have
			// to give tags a name; pass NULL for the name to create an
			// anonymous tag.
			//
			// In any real app, another useful optimization would be to create
			// a GtkTextTagTable in advance, and reuse the same tag table for
			// all the buffers with the same tag set, instead of creating
			// new copies of the same tags for every buffer.
			//
			// Tags are assigned default priorities in order of addition to the
			// tag table.	 That is, tags created later that affect the same text
			// property affected by an earlier tag will override the earlier
			// tag.  You can modify tag priorities with
			// gtk_text_tag_set_priority().

			TextTag tag  = new TextTag ("heading");
			tag.Weight = Pango.Weight.Bold;
			tag.Size = (int) Pango.Scale.PangoScale * 15;
			buffer.TagTable.Add (tag);

			tag  = new TextTag ("italic");
			tag.Style = Pango.Style.Italic;
			buffer.TagTable.Add (tag);

			tag  = new TextTag ("bold");
			tag.Weight = Pango.Weight.Bold;
			buffer.TagTable.Add (tag);

			tag  = new TextTag ("big");
			tag.Size = (int) Pango.Scale.PangoScale * 20;
			buffer.TagTable.Add (tag);

			tag  = new TextTag ("xx-small");
			tag.Scale = Pango.Scale.XXSmall;
			buffer.TagTable.Add (tag);

			tag  = new TextTag ("x-large");
			tag.Scale = Pango.Scale.XLarge;
			buffer.TagTable.Add (tag);

			tag  = new TextTag ("monospace");
			tag.Family = "monospace";
			buffer.TagTable.Add (tag);
			
			tag  = new TextTag ("courier new");
			tag.Family = "Courier New";
			buffer.TagTable.Add (tag);
			
			tag  = new TextTag ("courier new bold");
			tag.Weight = Pango.Weight.Bold;
			tag.Family = "Courier New";
			buffer.TagTable.Add (tag);

			tag  = new TextTag ("blue_foreground");
			tag.Foreground = "blue";
			buffer.TagTable.Add (tag);

			tag  = new TextTag ("red_background");
			tag.Background = "red";
			buffer.TagTable.Add (tag);

			// The C gtk-demo passes NULL for the drawable param, which isn't
			// multi-head safe, so it seems bad to allow it in the C# API.
			// But the Window isn't realized at this point, so we can't get
			// an actual Drawable from it. So we kludge for now.
			Pixmap stipple = Pixmap.CreateBitmapFromData (Gdk.Screen.Default.RootWindow, gray50_bits, gray50_width, gray50_height);

			tag  = new TextTag ("background_stipple");
			tag.BackgroundStipple = stipple;
			buffer.TagTable.Add (tag);

			tag  = new TextTag ("foreground_stipple");
			tag.ForegroundStipple = stipple;
			buffer.TagTable.Add (tag);

			tag  = new TextTag ("big_gap_before_line");
			tag.PixelsAboveLines = 30;
			buffer.TagTable.Add (tag);

			tag  = new TextTag ("big_gap_after_line");
			tag.PixelsBelowLines = 30;
			buffer.TagTable.Add (tag);

			tag  = new TextTag ("double_spaced_line");
			tag.PixelsInsideWrap = 10;
			buffer.TagTable.Add (tag);

			tag  = new TextTag ("not_editable");
			tag.Editable = false;
			buffer.TagTable.Add (tag);

			tag  = new TextTag ("word_wrap");
			tag.WrapMode = WrapMode.Word;
			buffer.TagTable.Add (tag);

			tag  = new TextTag ("char_wrap");
			tag.WrapMode = WrapMode.Char;
			buffer.TagTable.Add (tag);

			tag  = new TextTag ("no_wrap");
			tag.WrapMode = WrapMode.None;
			buffer.TagTable.Add (tag);

			tag  = new TextTag ("center");
			tag.Justification = Justification.Center;
			buffer.TagTable.Add (tag);

			tag  = new TextTag ("right_justify");
			tag.Justification = Justification.Right;
			buffer.TagTable.Add (tag);

			tag  = new TextTag ("wide_margins");
			tag.LeftMargin = 50;
			tag.RightMargin = 50;
			buffer.TagTable.Add (tag);

			tag  = new TextTag ("strikethrough");
			tag.Strikethrough = true;
			buffer.TagTable.Add (tag);

			tag  = new TextTag ("underline");
			tag.Underline = Pango.Underline.Single;
			buffer.TagTable.Add (tag);

			tag  = new TextTag ("double_underline");
			tag.Underline = Pango.Underline.Double;
			buffer.TagTable.Add (tag);

			tag  = new TextTag ("superscript");
			tag.Rise = (int) Pango.Scale.PangoScale * 10;
			tag.Size = (int) Pango.Scale.PangoScale * 8;
			buffer.TagTable.Add (tag);

			tag  = new TextTag ("subscript");
			tag.Rise = (int) Pango.Scale.PangoScale * -10;
			tag.Size = (int) Pango.Scale.PangoScale * 8;
			buffer.TagTable.Add (tag);

			tag  = new TextTag ("rtl_quote");
			tag.WrapMode = WrapMode.Word;
			tag.Direction = TextDirection.Rtl;
			tag.Indent = 30;
			tag.LeftMargin = 20;
			tag.RightMargin = 20;
			buffer.TagTable.Add (tag);
		}
		
		/// <summary>
		/// Traduce un numero a Letras
		/// </summary>
		/// <param name="sNumero">
		/// A <see cref="System.String"/>
		/// </param>
		/// <param name="descriptipomoneda_">
		/// A <see cref="System.String"/>
		/// </param>
		/// <returns>
		/// A <see cref="System.String"/>
		/// </returns>
		public static string ConvertirCadena(string num,string moneda)
		{
			string res, dec = "";
           	Int64 entero;
           	int decimales;
           	double nro;
           	try{
               nro = Convert.ToDouble(num);
           	}catch{
				return "";
           }
           	entero = Convert.ToInt64(Math.Truncate(nro));
           	decimales = Convert.ToInt32(Math.Round((nro - entero) * 100, 2));
			//if (decimales > 0){
               dec = " "+moneda+" CON " + decimales.ToString() + "/100 M.N.";
			//}
			res = class_public.NumeroALetras(Convert.ToDouble(entero)) + dec;

           return res;
		}
		
		/// <summary>
		/// Transforma un Numero a letras.
		/// </summary>
		/// <returns>
		/// Traduce el numero a Letras
		/// </returns>
		/// <param name='value'>
		/// Value.
		/// </param>
		private static string NumeroALetras(double value)
		{
			string Num2Text = "";
           value = Math.Truncate(value); 
           if (value == 0) Num2Text = "CERO";
           else if (value == 1) Num2Text = "UNO";
			else if (value == 2) Num2Text = "DOS";
           else if (value == 3) Num2Text = "TRES";
           else if (value == 4) Num2Text = "CUATRO";
           else if (value == 5) Num2Text = "CINCO";
           else if (value == 6) Num2Text = "SEIS";
           else if (value == 7) Num2Text = "SIETE";
           else if (value == 8) Num2Text = "OCHO";
           else if (value == 9) Num2Text = "NUEVE";
           else if (value == 10) Num2Text = "DIEZ";
           else if (value == 11) Num2Text = "ONCE";
           else if (value == 12) Num2Text = "DOCE";
			else if (value == 13) Num2Text = "TRECE";
           else if (value == 14) Num2Text = "CATORCE";
           else if (value == 15) Num2Text = "QUINCE";
           else if (value < 20) Num2Text = "DIECI" + NumeroALetras(value - 10);
           else if (value == 20) Num2Text = "VEINTE";
           else if (value < 30) Num2Text = "VEINTI" + NumeroALetras(value - 20);
           else if (value == 30) Num2Text = "TREINTA";
           else if (value == 40) Num2Text = "CUARENTA";
           else if (value == 50) Num2Text = "CINCUENTA";
           else if (value == 60) Num2Text = "SESENTA";
           else if (value == 70) Num2Text = "SETENTA";
           else if (value == 80) Num2Text = "OCHENTA";
           else if (value == 90) Num2Text = "NOVENTA";
           else if (value < 100) Num2Text = NumeroALetras(Math.Truncate(value / 10) * 10) + " Y " + NumeroALetras(value % 10);
           else if (value == 100) Num2Text = "CIEN";
           else if (value < 200) Num2Text = "CIENTO " + NumeroALetras(value - 100);
           else if ((value == 200) || (value == 300) || (value == 400) || (value == 600) || (value == 800)) Num2Text = NumeroALetras(Math.Truncate(value / 100)) + "CIENTOS";
           else if (value == 500) Num2Text = "QUINIENTOS";
           else if (value == 700) Num2Text = "SETECIENTOS";
           else if (value == 900) Num2Text = "NOVECIENTOS";
           else if (value < 1000) Num2Text = NumeroALetras(Math.Truncate(value / 100) * 100) + " " + NumeroALetras(value % 100);
           else if (value == 1000) Num2Text = "MIL";
           else if (value < 2000) Num2Text = "MIL " + NumeroALetras(value % 1000);
           else if (value < 1000000)
           {
               Num2Text = NumeroALetras(Math.Truncate(value / 1000)) + " MIL";
               if ((value % 1000) > 0) Num2Text = Num2Text + " " + NumeroALetras(value % 1000);
			}
 
           else if (value == 1000000) Num2Text = "UN MILLON";
           else if (value < 2000000) Num2Text = "UN MILLON " + NumeroALetras(value % 1000000);
           else if (value < 1000000000000)
           {
               Num2Text = NumeroALetras(Math.Truncate(value / 1000000)) + " MILLONES ";
               if ((value - Math.Truncate(value / 1000000) * 1000000) > 0) Num2Text = Num2Text + " " + NumeroALetras(value - Math.Truncate(value / 1000000) * 1000000);
           }
           else if (value == 1000000000000) Num2Text = "UN BILLON";
           else if (value < 2000000000000) Num2Text = "UN BILLON " + NumeroALetras(value - Math.Truncate(value / 1000000000000) * 1000000000000);
           else
           {
               Num2Text = NumeroALetras(Math.Truncate(value / 1000000000000)) + " BILLONES";
               if ((value - Math.Truncate(value / 1000000000000) * 1000000000000) > 0) Num2Text = Num2Text + " " + NumeroALetras(value - Math.Truncate(value / 1000000000000) * 1000000000000);
           }
			return Num2Text;
		}
		
		/// <summary>
		/// Digito verificador the EAN.
		/// 12 digitos unicamente
		/// </summary>
		/// <returns>
		/// The EA.
		/// </returns>
		/// <param name='EAN_string'>
		/// EA n_string.
		/// </param>
		public string digito_verificador_EAN(string EAN_)
		{
			//string EAN = "123456789041"; // 12 digitos unicamente
			// Cálculo del dígito de control EAN
			int iSum = 0;
			int iSumInpar = 0;
			int iDigit = 0;
						
			string EAN=EAN_.PadLeft(13,'0');
			
			for (int i = EAN.Length; i >= 1; i--){
				iDigit = Convert.ToInt32(EAN.Substring(i - 1, 1));
				if (i % 2 != 0){
					iSumInpar += iDigit;
				}else{
					iSum += iDigit;
			 	}
			}
			iDigit = (iSumInpar*3) + iSum ;
			int iCheckSum = (10 - (iDigit % 10)) % 10;
			return("Digito de control: " + iCheckSum.ToString());
		}

		/// <summary>
		/// Enviars the correo.
		/// </summary>
		/// <param name='mensaje_email'>
		/// Mensaje_email.
		/// </param>
		/// <param name='asuntoemail'>
		/// Asuntoemail.
		/// </param>
		/// <param name='mailsender'>
		/// Mailsender.
		/// </param>
		/// <param name='passwdsender'>
		/// Passwdsender.
		/// </param>
		/// <param name='mailreceive'>
		/// Mailreceive.
		/// </param>
		public void EnviarCorreo(string mensaje_email,string asuntoemail,string mailsender,string passwdsender,string mailreceive)
		{
			/*-------------------------MENSAJE DE CORREO----------------------*/
			//Creamos un nuevo Objeto de mensaje
			System.Net.Mail.MailMessage mmsg = new System.Net.Mail.MailMessage();
			
			//Direccion de correo electronico a la que queremos enviar el mensaje
			mmsg.To.Add(mailreceive);			
			//Nota: La propiedad To es una colección que permite enviar el mensaje a más de un destinatario
			
			//Asunto
			mmsg.Subject = asuntoemail;
			mmsg.SubjectEncoding = System.Text.Encoding.UTF8;
			
			//Direccion de correo electronico que queremos que reciba una copia del mensaje
			//mmsg.Bcc.Add("daniel.olivares@medicanoresteion.com.mx"); //Opcional
			
			//Cuerpo del Mensaje
			string msg_html = "\n this is a sample body with html in it. <br>" +
				"<b>This is bold</b> <br>" +
				"<font color=#E8A317>This is blue</font>";
			mmsg.Body = mensaje_email;
			mmsg.BodyEncoding = System.Text.Encoding.UTF8;
			mmsg.IsBodyHtml = true; //Si no queremos que se envíe como HTML
			
			//Correo electronico desde la que enviamos el mensaje
			mmsg.From = new System.Net.Mail.MailAddress(mailsender);
			
			
			/*-------------------------CLIENTE DE CORREO----------------------*/
			//Creamos un objeto de cliente de correo
			System.Net.Mail.SmtpClient cliente = new System.Net.Mail.SmtpClient();
			
			//Hay que crear las credenciales del correo emisor
			cliente.Credentials = new System.Net.NetworkCredential(mailsender, passwdsender);
			
			//Lo siguiente es obligatorio si enviamos el mensaje desde Gmail
			/*
			cliente.Port = 587;
			cliente.EnableSsl = true;
			*/
			
			cliente.Host = mailserver; //Para Gmail "smtp.gmail.com";			
			
			/*-------------------------ENVIO DE CORREO----------------------*/
			try{
	            //Enviamos el mensaje      
	            cliente.Send(mmsg);
	        }catch (System.Net.Mail.SmtpException ex){
	            //Aquí gestionamos los errores al intentar enviar el correo
	        }
		}
	}
	
	public class ProgressBarSample 
	{
		public struct ProgressData {
			public Gtk.Window window;
			public Gtk.ProgressBar pbar;
			public uint timer;
			public bool activity_mode;
		}
	 
		static ProgressData pdata;
	 	
		/* Update the value of the progress bar so that we get
		 * some movement */
		bool progress_timeout()
		{
			double new_val;
			
			Console.Write("ejecutando la consulta QuerySql "+pdata.pbar.Fraction.ToString()+"\n");
						
			if (pdata.activity_mode){
				pdata.pbar.Pulse();
				
			}else {
				/* Calculate the value of the progress bar using the
				 * value range set in the adjustment object */
				new_val = pdata.pbar.Fraction + 0.01;
				if (new_val > 1.0)
					new_val = 0.0;
	 
				/* Set the new value */
				pdata.pbar.Fraction = new_val;
			}
	 
			/* As this is a timeout function, return TRUE so that it
			 * continues to get called */
	 
			return true;
		}
		
		/* Callback that toggles the text display within the progress bar trough */
		void toggle_show_text (object obj, EventArgs args)
		{
			if (pdata.pbar.Text == "")
				pdata.pbar.Text = "Query cada 1 minuto";
			else
				pdata.pbar.Text = "";
		}
	 
		/* Callback that toggles the activity mode of the progress bar */
		void toggle_activity_mode (object obj, EventArgs args)
		{
			pdata.activity_mode = !pdata.activity_mode;
			if (pdata.activity_mode)
				pdata.pbar.Pulse();
			else
				pdata.pbar.Fraction = 0.0;
		}
	 
		/* Callback that toggles the orientation of the progress bar */
		void toggle_orientation (object obj, EventArgs args)
		{
			switch (pdata.pbar.Orientation) {
				case Gtk.ProgressBarOrientation.LeftToRight:
					pdata.pbar.Orientation = Gtk.ProgressBarOrientation.RightToLeft;
					break;
				case Gtk.ProgressBarOrientation.RightToLeft:
					pdata.pbar.Orientation = Gtk.ProgressBarOrientation.LeftToRight;
					break;
				}
		}	 
	 
		void destroy_progress (object sender, DeleteEventArgs args)
		{
			Widget win = (Widget) sender;
			win.Toplevel.Destroy();
		}
	 
		void button_click (object sender, EventArgs args)
		{
			Widget win = (Widget) sender;
			win.Toplevel.Destroy();
		}
	 
		void app_quit() {
			GLib.Source.Remove (pdata.timer);
			pdata.timer = 0;
			//Application.Quit ();
		}
	 
		public ProgressBarSample()
		{
			Gtk.HSeparator separator;
			Gtk.Table table;
			Gtk.Button button;
			Gtk.CheckButton check;
			Gtk.VBox vbox;
	 
			//Application.Init ();
	 
			/* Allocate memory for the data that is passed to the callbacks*/
			pdata = new ProgressData();
			pdata.activity_mode = false;
			pdata.window = new Gtk.Window(Gtk.WindowType.Toplevel);
			pdata.window.Resizable = true;
	 
			pdata.window.DeleteEvent += destroy_progress;
			pdata.window.Title = "GtkProgressBar";
			pdata.window.BorderWidth = 0;
	 
			vbox = new Gtk.VBox(false, 5);
			vbox.BorderWidth = 10;
			pdata.window.Add(vbox);
			vbox.Show();
	 
			/* Create a centering alignment object */
			Gtk.Alignment align = new Gtk.Alignment( 1, 1, 0, 0);
			vbox.PackStart(align, false, false, 5);
			align.Show();
	 
			/* Create the GtkProgressBar */
			pdata.pbar = new Gtk.ProgressBar();
			pdata.pbar.Text = "";
			align.Add(pdata.pbar);
			pdata.pbar.Show();
	 
			/* Add a timer callback to update the value of the progress bar*/
			pdata.timer = GLib.Timeout.Add(10000, new GLib.TimeoutHandler (progress_timeout) );
	 	 
			separator = new Gtk.HSeparator();
			vbox.PackStart(separator, false, false, 0);
			separator.Show();
	 
			/* rows, columns, homogeneous */
			table = new Gtk.Table(2, 3, false);
			vbox.PackStart(table, false, true, 0);
			table.Show();
	 
			/* Add a check button to select displaying of the trough text*/
			check = new Gtk.CheckButton("Query cada 1 minuto");
			table.Attach(check, 0, 1, 0, 1, 
					Gtk.AttachOptions.Expand | Gtk.AttachOptions.Fill, 
					Gtk.AttachOptions.Expand | Gtk.AttachOptions.Fill, 
					5, 5);
			check.Clicked += toggle_show_text;
			check.Show();
	 
			/* Add a check button to toggle activity mode */
			check = new Gtk.CheckButton("Activity mode");
			table.Attach(check, 0, 1, 1, 2, 
					Gtk.AttachOptions.Expand | Gtk.AttachOptions.Fill, 
					Gtk.AttachOptions.Expand | Gtk.AttachOptions.Fill, 
					5, 5);
			check.Clicked += toggle_activity_mode;
			check.Active = true;
			check.Show();
	 
			/* Add a check button to toggle orientation */
			check = new Gtk.CheckButton("Right to Left");
			table.Attach(check, 0, 1, 2, 3, 
					Gtk.AttachOptions.Expand | Gtk.AttachOptions.Fill, 
					Gtk.AttachOptions.Expand | Gtk.AttachOptions.Fill, 
					5, 5);
			check.Clicked += toggle_orientation;
			check.Show();
	 
			/* Add a button to exit the program */
			button = new Gtk.Button("close");
			button.Clicked += button_click;
			vbox.PackStart(button, false, false, 0);
	 
			/* This makes it so the button is the default. */
			button.CanDefault = true;
	 
			/* This grabs this button to be the default button. Simply hitting
			* the "Enter" key will cause this button to activate. */
			button.GrabDefault();
			button.Show();
	 
			pdata.window.ShowAll();
	 
			//Application.Run ();
		}	
	}

	public class insert_registro
	{
		// variable para la conexion
		string connectionString = "";
		string nombrebd = "";

		class_conexion conexion_a_DB = new class_conexion();

		//Declaracion de ventana de error
		protected Gtk.Window MyWinError;
		protected Gtk.Window MyWin;

		/// <summary>
		/// Initializes a new instance of the <see cref="osiris.insert_codbarra"/> class.
		/// </summary>
		/// <param name="insert_or_update">If set to <c>true</c> insert or update.</param>
		/// <param name="parametros">Parametros.</param>
		/// <param name="paraobj">Paraobj.</param>
		public insert_registro(string name_table,string[,] parametros,object[] paraobj)
		{
			connectionString = conexion_a_DB._url_servidor+conexion_a_DB._port_DB+conexion_a_DB._usuario_DB+conexion_a_DB._passwrd_user_DB;
			nombrebd = conexion_a_DB._nombrebd;
			Gtk.Entry entry_id = (object) paraobj[0] as Gtk.Entry;
			NpgsqlConnection conexion; 
			conexion = new NpgsqlConnection (connectionString+nombrebd);
			try{
				conexion.Open ();
				NpgsqlCommand comando = conexion.CreateCommand ();						
				string sql_graba = "INSERT INTO "+name_table;
				if (parametros.Length > 0){
					if (parametros.GetUpperBound (0) > 0) {
						sql_graba = sql_graba + "(";
						for (int colum_field = 0; colum_field <= parametros.GetUpperBound (0); colum_field++) {
							//store.AppendValues (args_array [colum_field, 0], args_id_array [colum_field], 1, args_array [colum_field, 1], args_array [colum_field, 2]);
							sql_graba = sql_graba + parametros [colum_field, 0];
						}
						sql_graba = sql_graba + ") VALUES (";
						for (int colum_field = 0; colum_field <= parametros.GetUpperBound (0); colum_field++) {
							//store.AppendValues (args_array [colum_field, 0], args_id_array [colum_field], 1, args_array [colum_field, 1], args_array [colum_field, 2]);
							sql_graba = sql_graba + parametros [colum_field, 1];
						}
						sql_graba = sql_graba + ");";
					}
				}
				Console.WriteLine(sql_graba);
				comando.CommandText = sql_graba;
				comando.ExecuteNonQuery();    	    	       	comando.Dispose();
			}catch(NpgsqlException ex){
				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
					MessageType.Error,ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
				msgBoxError.Run ();		msgBoxError.Destroy();
			}
			conexion.Close ();
		}
	}

	public class update_registro
	{
		Gtk.Entry entry_id = null;

		// variable para la conexion
		string connectionString = "";
		string nombrebd = "";

		class_conexion conexion_a_DB = new class_conexion();

		//Declaracion de ventana de error
		protected Gtk.Window MyWinError;
		protected Gtk.Window MyWin;

		/// <summary>
		/// Initializes a new instance of the <see cref="osiris.update_codbarra"/> class.
		/// </summary>
		/// <param name="parametros">Parametros.</param>
		/// <param name="paraobj">Paraobj.</param>
		public update_registro(string name_table,string[,] parametros,object[] paraobj)
		{
			connectionString = conexion_a_DB._url_servidor+conexion_a_DB._port_DB+conexion_a_DB._usuario_DB+conexion_a_DB._passwrd_user_DB;
			nombrebd = conexion_a_DB._nombrebd;
			entry_id = (object) paraobj[0] as Gtk.Entry;
			NpgsqlConnection conexion; 
			conexion = new NpgsqlConnection (connectionString+nombrebd);
			try{
				conexion.Open ();
				NpgsqlCommand comando = conexion.CreateCommand ();						
				string sql_graba = "UPDATE "+name_table+" SET ";
				if (parametros.Length > 0){
					if (parametros.GetUpperBound (0) > 0) {
						for (int colum_field = 0; colum_field <= parametros.GetUpperBound (0); colum_field++) {
							//store.AppendValues (args_array [colum_field, 0], args_id_array [colum_field], 1, args_array [colum_field, 1], args_array [colum_field, 2]);
							sql_graba = sql_graba + parametros [colum_field, 0] + parametros [colum_field, 1];
						}
					}
				}
				//Console.WriteLine(sql_graba);									
				comando.CommandText = sql_graba;
				comando.ExecuteNonQuery();    	    	       	comando.Dispose();
			}catch(NpgsqlException ex){
				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
					MessageType.Error,ButtonsType.Close,"PostgresSQL error: {0}",ex.Message);
				msgBoxError.Run ();		msgBoxError.Destroy();
			}
			conexion.Close ();
		}
	}

	public class crea_colums_treeview
	{
		public crea_colums_treeview(object[] args,string [,] args_colums)
		{
			Gtk.CellRendererText text;
			Gtk.CellRendererToggle toggle;
			// crea los objetos para el uso del treeview
			Gtk.TreeView treeviewobject = null;
			Gtk.TreeStore treeViewEngine = null;

			ArrayList columns = new ArrayList ();
			treeviewobject = (object) args[0] as Gtk.TreeView;
			treeViewEngine = (object) args[1] as Gtk.TreeStore;
			treeViewEngine = new TreeStore(typeof(string),typeof(string),typeof(string),typeof(string),typeof(string),typeof(string),typeof(string),typeof(string),typeof(string),typeof(bool),typeof(bool));
			treeviewobject.Model = treeViewEngine;
			treeviewobject.RulesHint = true;
			treeviewobject.Selection.Mode = SelectionMode.Multiple;
			if(args_colums.GetUpperBound(0) >= 0){
				for (int colum_field = 0; colum_field <= args_colums.GetUpperBound (0); colum_field++) {
					if((string) args_colums [colum_field, 1] == "text"){
						// column for holiday names
						text = new CellRendererText ();
						text.Xalign = 0.0f;
						columns.Add (text);
						TreeViewColumn column0 = new TreeViewColumn((string) args_colums [colum_field, 0], text,"text", colum_field);
						column0.Resizable = true;
						//column0.SortColumnId = colum_field;
						treeviewobject.InsertColumn (column0, colum_field);					
					}
					if((string) args_colums [colum_field, 1] == "toogle"){
						toggle = new CellRendererToggle ();
						toggle.Xalign = 0.0f;
						columns.Add (toggle);
						TreeViewColumn column0 = new TreeViewColumn ((string) args_colums [colum_field, 0], toggle,"active",colum_field);
						column0.Sizing = TreeViewColumnSizing.Fixed;
						column0.Clickable = true;
						treeviewobject.InsertColumn (column0, colum_field);
					}
				}
			}
			args[0] = treeviewobject;
			args[1] = treeViewEngine;
		}
	}	
}