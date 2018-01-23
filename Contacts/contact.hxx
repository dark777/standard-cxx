#ifndef _CONTACT_HXX
#define _CONTACT_HXX

#include <iostream>
#include "date.hxx"
#include "time.hxx"

struct Contact
{
    std::string name;
    std::string address;
    std::string neighborhood; //bairro
    std::string HouseNumber;
    std::string PhoneHomeNumber;
    std::string PhoneWorkNumber;
    std::string MobileNumber;
    std::string email;
    std::string dobYear;
    std::string dobMonth;
    std::string dobDay;
    std::string age;
    std::string datetime;
    Date date;
    Time time;
};

void loadNumber(std::string&, const std::string&, uint16_t);
void loadString(std::string&, const std::string&);
void loadStringMail(std::string&, const std::string&);
Contact getCheckDays(Contact&);
Contact newContact();
void writeContacts(Contact&, std::ofstream&);
void readContacts(Contact&, std::ifstream&);
void addContact();
void display(Contact&);
void display(Contact&, std::ifstream&);
void displayContact();
void displayAll();
void editContact();
void readFile();
void writeFile();
void doMenu();


#endif