
#include <fstream>
#include <sstream>
#include <iostream>
//http://www.dreamincode.net/forums/topic/39880-various-problems-validation-file-editing/
//#include"date_type.h"

struct Contact{
	std::string name;
	std::string address;
	std::string HomeNumber;
	std::string WorkNumber;
	std::string MobileNumber;
	std::string email;
	std::string dobYear;
	std::string dobMonth;
	std::string dobDay;
	std::string age;
};

const int MAX = 101;
Contact listContact[MAX];
int ContactCounter;

int checkEmail (Contact c)
{
	c.email.find("@");
	if (c.email.find("@")>c.email.length())
		return 0;
	else return 1;
}

bool checkNumber(std::string &s) {
   istringstream ssIn(s);
   int n;
   if (ssIn >> n) {
	  std::cout << n << endl;
	  // we got a number, feed it back
	  stringstream ssOut;
	  ssOut << n;
	  s = ssOut.str();	  
	  return true;
   } else { 
	  s.clear();
   }
   return false;
}

void loadNumber(std::string &s, const std::string &label, const int requiredSize) {
   while(true) {
	  std::cout << endl << "Enter " << label << "(" << requiredSize << " digits): ";
	  getline (cin, s);
	  if (checkNumber(s)) { 
		 if(s.length()==requiredSize) { break; }
	  }
	  std::cout << "Invalid " << label << "." << endl;
   }
}

 void loadString(std::string &s, const std::string &label) {
   while(true) {
	  std::cout << endl << "Enter " << label << ": ";
	  getline (cin, s);
	  if (s != "") { break; }
	  std::cout << "Invalid " << label << "." << endl;
   }
}

/*
Contact newContact()
{
	Contact c;
	loadString(c.name, "Name");
	loadString(c.address, "Address");
	loadString(c.HomeNumber, "Home Number");
	loadString(c.WorkNumber, "Work Number");
	loadString(c.MobileNumber, "Mobile Number");
	loadEmail(c.email, "Email Address");
	loadNumber(c.dobYear, "year of birth", 4);
	loadNumber(c.dobMonth, "month of birth", 2);
	loadNumber(c.dobDay, "day of birth", 2);
	return c;
}

 */
Contact newContact()
{
	Contact c;
	do {
		std::cout << endl << "Enter name : ";
	getline (cin, c.name);
	if (c.name == "")
		std::cout << "Invalid Name.";
	else;
	} while(c.name == "");
	
	do {
		std::cout << endl << "Enter address : ";
	getline (cin, c.address );
	if (c.address == "")
		std::cout << "Invalid Address.";
	else;
	} while (c.address == "");
	
	do {
		std::cout << endl << "Enter home number : ";
	getline (cin, c.HomeNumber );
	if (c.HomeNumber == "")
		std::cout << "Invalid Number.";
	else;
	} while (c.HomeNumber == "");
	
	do {
		std::cout << endl << "Enter work number : ";
	getline (cin, c.WorkNumber );
	if (c.WorkNumber == "")
		std::cout << "Invalid Number.";
	else;
	} while (c.WorkNumber == "");
	
	do {
		std::cout << endl << "Enter mobile number : ";
	getline (cin, c.MobileNumber );
	if (c.MobileNumber == "")
		std::cout << "Invalid Number.";
	else;
	} while(c.MobileNumber == "");
	
	do {
		std::cout << endl << "Enter email : ";
	getline (cin, c.email );
	if (checkEmail (c)==0)
		std::cout << "Invalid Email Address.";
	else;
	} while (checkEmail (c) ==0);
	
	do {
		std::cout << endl << "Enter year of birth(4 digits) : ";
	getline (cin, c.dobYear );
	} while (c.dobYear == "");
	
	do {
		std::cout << endl << "Enter month of birth(2 digits) : ";
	getline (cin, c.dobMonth);
	} while(c.dobMonth == "");
	
	do {
		std::cout << endl << "Enter day of birth(2 digits) : ";
	getline (cin, c.dobDay);
	} while (c.dobDay == "");

 	return c;
}

void writeContacts(Contact &c, std::ofstream &os)
{
	os << c.name << endl 
		<< c.address << endl 
		<< c.HomeNumber << endl 
		<< c.WorkNumber << endl 
		<< c.MobileNumber << endl 
		<< c.email << endl 
		<< c.dobYear << endl 
		<< c.dobMonth << endl 
		<< c.dobDay << endl;
}

void readContacts(Contact &c, std::ifstream &is)
{
	getline(is, c.name);
	getline(is, c.address);
	getline(is, c.HomeNumber);
	getline(is, c.WorkNumber);
	getline(is, c.MobileNumber);
	getline(is, c.email);
	getline(is, c.dobYear);
	getline(is, c.dobMonth);
	getline(is, c.dobDay);
}

void addContact()
{
	std::cout << endl << "Input new contact data: " << endl;
	listContact[ContactCounter] = newContact();
	ContactCounter++;
}

void display(Contact &c)
{
	std::cout << endl << "Contact Data:" << ContactCounter << endl
		<< "Name		  - " << c.name << endl
		<< "Address	   - " << c.address << endl
		<< "Home Number   - " << c.HomeNumber << endl
		<< "Work Number   - " << c.WorkNumber << endl
		<< "Mobile Number - " << c.MobileNumber << endl
		<< "Email		 - " << c.email << endl
		<< "Date of Birth - " << c.dobDay <<"/"<< c.dobMonth <<"/"<< c.dobYear << endl;
}

void displayContact()
{
	int c;
   do{
	  std::cout << "Enter number of contact: ";
	  cin >> c;
   } while(c<0 || c>=ContactCounter);
   display(listContact[c]);
}

void displayAll()
{
   for(int i=0; i<ContactCounter; i++)
	  display(listContact[i]);
}

/*void editContact()
{
	{
	int c;
   do{
	  std::cout << "Enter number of contact: ";
	  cin >> c;
   } while(c<0 || c>=ContactCounter);
newContact();
}*/

void readFile()
{
   try{
	  std::ifstream inFile("C:\Temp\Contacts.dat", std::ios::in);
	  inFile >> ContactCounter;
	  inFile.ignore();
	  for(int i=0; i<ContactCounter; i++)
	   display(listContact[i]);
	  inFile.close();
	  std::cout << endl << "Contact List Loaded." << endl;
   }
   catch(exception e) {
	  std::cout << endl << "<-|Error|->" << endl << "Could not read file." << endl;
   }
}

void writeFile()
{
	try{
		std::ofstream outFile("C:\Temp\Contacts.dat", std::ios::ate);
		outFile << ContactCounter << endl;
		for(int i=0; i<ContactCounter; i++)
			writeContacts(listContact[i], outFile);
		outFile.close();
		std::cout << endl << "Contact List Saved." << endl;
	}
	catch(exception e) {
	  std::cout << endl << "<-|Error|->" << endl << "Could not read file." << endl;
	}
}

void doMenu()
{
	  int choice;
	  int ContactNumber;
	date today;
	std::cout << today;
	std::cout << endl << "The year is " << today.year();
	std::cout << endl << "The month is " << today.month();
	std::cout << endl << "The day is " << today.day();
	std::cout << endl << endl << endl;
	  do{
		 std::cout << endl;
		 std::cout << "Enter your selection." << endl;
		 std::cout << "1. Load Contacts List" << endl;
		 std::cout << "2. Save Contacts List" << endl;
		 std::cout << "3. Add a Contact" << endl;
		 std::cout << "4. Display Contact(s) Details" << endl;
		 std::cout << "5. Display All Contact(s) Details" << endl;
		 //std::cout << "6. Edit Contact(s) Details" << endl;
		 std::cout << "6. Quit" << endl;
		 std::cout << endl;
		 cin >> choice;
		 cin.ignore();
		 switch(choice)
		 {
		 case 1: readFile();
			 break;
		 case 2: writeFile();
			 break;
		 case 3: addContact();
			 break;
		 case 4: displayContact();	
			 break;
		 case 5: displayAll();
			 break;
		 /*case 6: editContact();
			 break;*/
		 }
	  } while(choice != 6);
}

int main ()
{
	readFile();
	doMenu();
	writeFile();
}