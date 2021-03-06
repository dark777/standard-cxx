/*
 * 	   This program is a schedule contact.
 *     Copyright (C) 2011  RCAlves
 * 
 *     This program is free software: you can redistribute it and/or modify
 *     it under the terms of the GNU General Public License as published by
 *     the Free Software Foundation, either version 3 of the License, or
 *     (at your option) any later version.
 * 
 *     This program is distributed in the hope that it will be useful,
 *     but WITHOUT ANY WARRANTY; without even the implied warranty of
 *     MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *     GNU General Public License for more details.
 * 
 *     You should have received a copy of the GNU General Public License
 *     along with this program.  If not, see <http://www.gnu.org/licenses/>.
 */

#ifndef CMENU_H_
#define CMENU_H_

#include <vector>
#include "CContato.h"

using std::vector;

/*
 *
 */
class CMenu {
private:
	int idContato;
	vector<CContato> lista;
	void addContato( );
	void readContato( );
	void deleteContato( );
	void updateContato( );

	void readCPFContato( );
	void readNomeContato( );
	void readTelefoneContato( );
	void readTodos( );

	void updateCPFContato( );
	void updateNomeContato( );
	void updateTelefoneContato( );
	void updateTodos( );

	void deleteCPFContato( );
	void deleteNomeContato( );
	void deleteTelefoneContato( );
	void deleteTodos( );
public:
	void showMenu( );

	CMenu();
	virtual ~CMenu();
};

#endif /* CMENU_H_ */
